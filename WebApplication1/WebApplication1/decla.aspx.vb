Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Win32

Public Class WebForm11
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim myCommand2 As SqlCommand
    Dim dr As SqlDataReader
    Dim dr2 As SqlDataReader

    Private Sub controlaAcceso()
        Dim q
        q = "SELECT rl.id FROM reprLegal rl, clientes cli WHERE cli.correo='" + Session("curCorreo") + "' AND cli.id=rl.idCliente AND rl.esActual=1"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        If Not dr.HasRows Then
            dr.Close()
            redir.Text = "1"
            Response.Write("<script language='javascript'>alert('Requiere especificar un representante legal actual');</script>")
            Response.Write("<script>location.href='cliente.aspx';</script>")
            Exit Sub
        End If
        dr.Close()

        If InStr(Request.ServerVariables("HTTP_USER_AGENT"), "MSIE") Then
            redir.Text = "1"
            Response.Write("<script language='javascript'>alert('Requiere iniciar sesión con un navegador distinto a Internet Explorer, puede descargar e instalar Chrome desde la sección inferior de descargas en el menú <Cuenta>');</script>")
            Response.Write("<script>location.href='cliente.aspx';</script>")
            Exit Sub
        End If

    End Sub

    Private Sub cargaNumDecls()
        Dim q
        If tipoMensAn.SelectedItem.Text = "Mensual" Then
            q = "SELECT id FROM ideMens WHERE mes='" + mes.SelectedValue.ToString + "' and idAnual=" + Session("GidAnual").ToString + " order by id ASC"
        Else
            q = "SELECT id FROM ideAnual WHERE ejercicio='" + ejercicio.SelectedItem.Text + "' and idCliente=" + Session("GidCliente").ToString + " order by id ASC"
        End If
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        Dim n = 0
        numDecl.Items.Clear()
        If dr.HasRows Then
            While dr.Read()
                numDecl.Items.Add(New ListItem(dr("id"), dr("id")))
                n = 1
            End While
        End If
        dr.Close()
        If n = 0 Then
            numDecl.Visible = False
            lblNumDecl.Visible = False
        Else
            numDecl.Visible = True
            lblNumDecl.Visible = True
        End If

        If tipoMensAn.SelectedItem.Text = "Mensual" Then
            q = "SELECT TOP 1 M.id FROM ideMens M, ideAnual A WHERE A.ejercicio='" + ejercicio.SelectedItem.Text + "' and A.idCliente=" + Session("GidCliente").ToString + " and M.mes=" + mes.SelectedValue.ToString + " and M.idAnual=A.id and (M.estado='ACEPTADA' or M.estado='CONTINGENCIA')"
            myCommand = New SqlCommand(q, myConnection)
            myCommand.CommandText = q
            dr = myCommand.ExecuteReader()
            dr.Read()
            If dr.HasRows Then
                dr.Close()
                'oper.Items.FindByValue("1").Enabled = False 'deshabilita envio en ceros, si ya hay declaraciones en ese mes, tendria que hacerse via complementaria
            Else
                dr.Close()
                Dim fechaUltima = DateAdd(DateInterval.Day, -DatePart(DateInterval.Day, Now()) + 1, Now()) 'dia 1o del mes actual
                fechaUltima = DateAdd(DateInterval.Month, -1, fechaUltima) 'dia 1o del mes anterior
                q = "SELECT co.id FROM contratos co, planes pla WHERE co.id=" + idContrato.Text + " AND co.idPlan=pla.id AND ( (nDeclHechas < nDeclContratadas and (pla.elplan='BASICO' OR pla.elplan='CEROS')) or ('" + Format(Convert.ToDateTime(fechaUltima), "yyyy-MM-dd") + "' <= fechaFinal and pla.elplan='PREMIUM' and co.esRegularizacion=0) or (pla.elplan='PREMIUM' and co.esRegularizacion=1) ) "
                myCommand = New SqlCommand(q, myConnection)
                myCommand.CommandText = q
                dr = myCommand.ExecuteReader()
                If Not dr.Read() Then
                    oper.Items.FindByValue("0").Enabled = False
                    oper.Items.FindByValue("1").Enabled = False
                Else
                    oper.Items.FindByValue("0").Enabled = True
                    oper.Items.FindByValue("1").Enabled = True
                End If
                dr.Close()
            End If
        Else 'anual
            q = "SELECT TOP 1 id FROM ideAnual WHERE ejercicio='" + ejercicio.SelectedItem.Text + "' and idCliente=" + Session("GidCliente").ToString + " and (estado='ACEPTADA' or estado='CONTINGENCIA')"
            myCommand = New SqlCommand(q, myConnection)
            myCommand.CommandText = q
            dr = myCommand.ExecuteReader()
            dr.Read()
            If dr.HasRows Then
                dr.Close()
                'oper.Items.FindByValue("1").Enabled = False 'deshabilita envio en ceros, si ya hay declaraciones en ese año, tendria que hacerse via complementaria
                'oper.Items.FindByValue("3").Enabled = False 'deshab creacion via 12 mens
            Else
                dr.Close()
                Dim fechaVariable = CDate(CStr(DatePart(DateInterval.Year, Now())) + "/01/01") 'dia 1o de este año
                q = "SELECT co.id,co.anualEnPremium FROM contratos co, planes pla WHERE co.id=" + idContrato.Text + " AND co.idPlan=pla.id AND ( (nDeclHechas < nDeclContratadas and pla.elplan<>'PREMIUM') or ('" + Format(Convert.ToDateTime(fechaVariable), "yyyy-MM-dd") + "' > periodoInicial and '" + Format(Convert.ToDateTime(fechaVariable), "yyyy-MM-dd") + "' <= fechaFinal and pla.elplan='PREMIUM' and co.anualEnPremium=1 and co.esRegularizacion=0) or (pla.elplan='PREMIUM' and co.anualEnPremium=1 and co.esRegularizacion=1)  ) "
                myCommand = New SqlCommand(q, myConnection)
                myCommand.CommandText = q
                dr = myCommand.ExecuteReader()
                If Not dr.HasRows Then
                    oper.Items.FindByValue("0").Enabled = False
                    oper.Items.FindByValue("1").Enabled = False
                    'oper.Items.FindByValue("3").Enabled = False
                Else
                    dr.Read()
                    oper.Items.FindByValue("0").Enabled = True
                    oper.Items.FindByValue("1").Enabled = True
                    If dr("anualEnPremium").Equals(True) Then
                        If numDecl.Visible = True Then
                            q = "SELECT estado FROM ideAnual WHERE id=" + numDecl.SelectedItem.Text
                            myCommand = New SqlCommand(q, myConnection)
                            myCommand.CommandText = q
                            dr2 = myCommand.ExecuteReader()
                            dr2.Read()
                            'If dr2("estado") = "VACIA" Or dr2("estado") = "CREADA" Or dr2("estado") = "IMPORTADA" Or dr2("estado") = "ERROR_ENVIO" Then
                            '    oper.Items.FindByValue("3").Enabled = True
                            'Else
                            '    oper.Items.FindByValue("3").Enabled = False
                            'End If
                            dr2.Close()
                        Else
                            'oper.Items.FindByValue("3").Enabled = True
                        End If
                    End If
                End If
                dr.Close()
            End If
        End If

        If elplan.Text = "CEROS" Or elplan.Text = "ANUAL DESDE 2014 CEROS COMPLEMENTARIA" Or elplan.Text = "ANUAL DESDE 2014 CEROS NORMAL" Then
            oper.Items.FindByValue("0").Enabled = False
        End If

        If elplan.Text <> "CEROS" And elplan.Text <> "BASICO" And elplan.Text <> "PREMIUM" Then 'ANUALES
            tipoMensAn.Items.FindByValue("Mensual").Enabled = False
            mes.Visible = False
            lblMes.Visible = False
            tipoMensAn.SelectedValue = "Anual"
        Else
            tipoMensAn.Items.FindByValue("Mensual").Enabled = True
            If tipoMensAn.SelectedItem.Value = "Mensual" Then
                mes.Visible = True
                lblMes.Visible = True
            Else
                mes.Visible = False
                lblMes.Visible = False
            End If            
        End If


        Call cambiaVia()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("curCorreo")) = True Then
            Response.Write("<script language='javascript'>alert('Expiró su sesión');</script>")
            Session.Abandon()
            Response.Write("<script language='javascript'>location.href='Login.aspx';</script>")
            Exit Sub
        End If


        Dim q

        myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';MultipleActiveResultSets=True")
        myConnection.Open()
        myCommand = New SqlCommand("set dateformat ymd", myConnection)
        myCommand.ExecuteNonQuery()


        controlaAcceso()
        If redir.Text = "1" Then
            Exit Sub
        End If

        ScriptManager1.RegisterPostBackControl(aplicar)
        ScriptManager1.RegisterPostBackControl(restablecer)

        If Not IsPostBack Then
            If HttpContext.Current.Request.IsLocal Or Session("runAsAdmin") = "1" Then
                restablecer.Visible = True
            Else
                restablecer.Visible = False
            End If


            Dim year As Integer = System.DateTime.Now.Year
            For intCount As Integer = year To 2008 Step -1
                ejercicio.Items.Add(New ListItem(intCount.ToString(), intCount.ToString()))
            Next

            q = "SELECT co.id, pla.elplan FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND (co.fechaPago IS NOT NULL OR co.postpago=1) order by case when co.esRegularizacion=1 then 1 else 2 end, case when pla.elplan='PREMIUM' then 1 else 2 end, co.periodoInicial"
            var.Text = q

            myCommand = New SqlCommand(q, myConnection)
            dr = myCommand.ExecuteReader()
            If (Not dr.HasRows) Then 'sin contratos pagados 
                dr.Close()
                Response.Write("<script language='javascript'>alert('No hay contratos pagados');</script>")
                Response.Write("<script>location.href='misContra.aspx';</script>")
                Exit Sub
            End If

            contratos.Items.Clear()
            While dr.Read()
                contratos.Items.Add(New ListItem(dr("id"), dr("id"))) '->> INVOCA A CONTRATOS (SELECTEDINDEX CHANGE) -> NUMDECLS SELECTEDINDEX CHANGE
            End While
            dr.Close()
            contratos.Visible = True
            lblContratos.Visible = True
            'cargaContrato()            
            aplicar.Visible = True
            complementaria.Visible = True
            via.Visible = True

            If String.IsNullOrEmpty(Request.QueryString("m")) Then
                q = "SELECT TOP 1 id FROM ideAnual WHERE ejercicio='" + ejercicio.SelectedItem.Text + "' and idCliente=" + Session("GidCliente").ToString + " order by id ASC"
                myCommand = New SqlCommand(q, myConnection)
                myCommand.CommandText = q
                dr = myCommand.ExecuteReader()
                If dr.HasRows Then
                    dr.Read()
                    Session("GidAnual") = dr("id")
                    var.Text = "1"
                Else
                    Session("GidAnual") = 0
                    var.Text = "2"
                End If
                dr.Close()
                cargaContrato()
            Else
                Session("GidAnual") = Session("misIdAnual")
                var.Text = "3"


                If Session("misTipo") = "Anual" Then
                    tipoMensAn.SelectedValue = "Anual"
                ElseIf Session("misTipo") = "Mensual" Then
                    tipoMensAn.SelectedValue = "Mensual"
                End If
                If Session("misContrato") <> "" Then
                    contratos.SelectedValue = Session("misContrato")
                End If
                If Session("misEjercicio") <> "" Then
                    ejercicio.SelectedValue = Session("misEjercicio")
                End If
                If Session("misTipo") = "Mensual" Then
                    mes.SelectedValue = Session("misMes")
                End If

                cargaContrato()
                If Session("misTipo") = "Anual" Then
                    tipoMensAn_SelectedIndexChanged(Me.tipoMensAn, EventArgs.Empty)
                ElseIf Session("misTipo") = "Mensual" Then

                    tipoMensAn_SelectedIndexChanged(Me.tipoMensAn, EventArgs.Empty)
                End If
                If Session("misContrato") <> "" Then
                    contratos_SelectedIndexChanged(Me.contratos, EventArgs.Empty)
                End If
                If Session("misEjercicio") <> "" Then
                    ejercicio_SelectedIndexChanged(Me.ejercicio, EventArgs.Empty)
                End If
                If Session("misTipo") = "Mensual" Then
                    mes_SelectedIndexChanged(Me.mes, EventArgs.Empty)
                End If
                If Session("misNdecla") <> "" Then
                    numDecl.SelectedValue = Session("misNdecla")
                End If
                If Session("misNdecla") <> "" Then
                    numDecl_SelectedIndexChanged(Me.numDecl, EventArgs.Empty)
                End If
            End If

        End If

        'If Session("misMes") = "" Then


        'Else

        'End If
        idAnual.Text = Session("GidAnual").ToString

        If Not IsPostBack Then
            If Not (String.IsNullOrEmpty(Request.QueryString("m")) Or Request.QueryString("m") Is Nothing) Then 'viene de misdecla
                If Session("misTipo") = "Mensual" Then
                    q = "SELECT normalComplementaria FROM ideMens WHERE id=" + numDecl.Text
                Else
                    q = "SELECT normalComplementaria FROM ideAnual WHERE id=" + numDecl.Text
                End If
                myCommand = New SqlCommand(q, myConnection)
                myCommand.CommandText = q
                dr = myCommand.ExecuteReader()
                If dr.HasRows Then
                    dr.Read()
                    If dr("normalComplementaria") = "NORMAL" Then
                        cargaNumDecls()
                    End If
                End If
                dr.Close()
            End If
        End If

        'If Session("misNdecla") = "" Then

    End Sub

    Protected Sub tipoMensAn_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tipoMensAn.SelectedIndexChanged
        If tipoMensAn.SelectedItem.Text = "Mensual" Then
            mes.Visible = True
            lblMes.Visible = True
            'oper.Items.FindByValue("3").Enabled = False
        Else 'anual
            mes.Visible = False
            lblMes.Visible = False
            'oper.Items.FindByValue("3").Enabled = True 'anual via 12 meses
        End If
        cargaNumDecls()
    End Sub

    Protected Sub aplicar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles aplicar.Click
        'Por defa es AUTOPOSTBACK y carga 1o el LOAD **
        Dim deseado, actual
        If tipoMensAn.SelectedItem.Text = "Mensual" Then
            deseado = ejercicio.SelectedItem.Text + "/" + mes.SelectedValue.ToString + "/01"
            actual = CStr(DatePart(DateInterval.Year, Now())) + "/" + CStr(DatePart(DateInterval.Month, Now())) + "/01"
            If CDate(deseado) > CDate(actual) Then
                Response.Write("<script language='javascript'>alert('El mes del año especificado es mayor al actual');</script>")
                Exit Sub
            End If
        Else
            If CStr(DatePart(DateInterval.Year, Now())) = ejercicio.SelectedItem.Text Then
                Response.Write("<script language='javascript'>alert('El año actual solo puede declararse hasta el próximo año');</script>")
                Exit Sub
            End If
            If CLng(ejercicio.SelectedItem.Text) >= 2014 Then 'anuales desde 2014
                If elplan.Text = "CEROS" Or elplan.Text = "BASICO" Or elplan.Text = "PREMIUM" Then
                    Response.Write("<script language='javascript'>alert('Para anuales >= 2014, seleccione un contrato de anuales desde 2014');</script>")
                    Exit Sub
                End If
                If (elplan.Text = "ANUAL DESDE 2014 CEROS COMPLEMENTARIA" Or elplan.Text = "ANUAL DESDE 2014 DATOS COMPLEMENTARIA") And (complementaria.Checked = False And numDecl.SelectedIndex = 0) Then
                    Response.Write("<script language='javascript'>alert('Para anuales >= 2014, no puede seleccionar contratos de complementarias para declaraciones normales');</script>")
                    Exit Sub
                End If
                If (elplan.Text = "ANUAL DESDE 2014 CEROS NORMAL" Or elplan.Text = "ANUAL DESDE 2014 DATOS NORMAL") And complementaria.Checked = True Then
                    Response.Write("<script language='javascript'>alert('Para anuales >= 2014, no puede seleccionar contratos de normales para declaraciones complementarias');</script>")
                    Exit Sub
                End If
                If (elplan.Text = "ANUAL DESDE 2014 CEROS NORMAL" Or elplan.Text = "ANUAL DESDE 2014 CEROS COMPLEMENTARIA") And oper.SelectedItem.Value = "0" Then
                    Response.Write("<script language='javascript'>alert('Para anuales >= 2014, no puede seleccionar contratos de ceros para declaraciones con datos');</script>")
                    Exit Sub
                End If
                If (elplan.Text = "ANUAL DESDE 2014 DATOS NORMAL" Or elplan.Text = "ANUAL DESDE 2014 DATOS COMPLEMENTARIA") And oper.SelectedItem.Value = "1" Then
                    Response.Write("<script language='javascript'>alert('Para anuales >= 2014, no puede seleccionar contratos de datos para declaraciones en ceros');</script>")
                    Exit Sub
                End If
            End If

        End If

        Dim q


        Dim dr3 As SqlDataReader
        q = "SELECT id FROM reprLegal WHERE idCliente=" + Session("GidCliente").ToString + " and esActual=1"
        myCommand2 = New SqlCommand(q, myConnection)
        dr3 = myCommand2.ExecuteReader()
        If Not dr3.Read() Then
            dr3.Close()
            Response.Write("<script language='javascript'>alert('Requiere especificar el representante legal actual en su cuenta');</script>")
            Exit Sub
        End If
        dr3.Close()


        If validaContrato() < 1 Then
            Exit Sub
        End If

        If tipoMensAn.SelectedItem.Text = "Mensual" Then
            q = "SELECT TOP 1 id FROM ideAnual WHERE ejercicio='" + ejercicio.SelectedItem.Text + "' and idCliente=" + Session("GidCliente").ToString + " order by id ASC"
            myCommand.CommandText = q
            dr = myCommand.ExecuteReader()
            If Not dr.Read() Then
                Session("GidAnual") = 0
            Else
                Session("GidAnual") = dr("id")
            End If
            dr.Close()
        Else 'anual
            If numDecl.Visible = True Then
                Session("GidAnual") = numDecl.SelectedItem.Text
            Else
                Session("GidAnual") = 0
            End If
        End If

        Dim comple
        If complementaria.Checked = True Then
            comple = "1"
            If tipoMensAn.SelectedItem.Text = "Mensual" Then
                q = "SELECT id FROM ideMens WHERE mes='" + mes.SelectedValue.ToString + "' and idAnual=" + Session("GidAnual").ToString + " and normalComplementaria='NORMAL'"
            Else
                q = "SELECT id FROM ideAnual WHERE ejercicio='" + ejercicio.SelectedItem.Text + "' and idCliente=" + Session("GidCliente").ToString + " and normalComplementaria='NORMAL'"
            End If
            myCommand.CommandText = q
            dr = myCommand.ExecuteReader()
            If Not dr.Read() Then
                'If DBNull.Value.Equals(dr("id")) Then
                Response.Write("<script language='javascript'>alert('No tiene declaraciones normales en este periodo');</script>")
                Exit Sub
            End If
            dr.Close()

            If tipoMensAn.SelectedItem.Text = "Mensual" Then
                q = "SELECT TOP 1 numOper,fechaPresentacion,estado FROM ideMens WHERE mes='" + mes.SelectedValue.ToString + "' and idAnual=" + Session("GidAnual").ToString + " order by id desc"
            Else
                q = "SELECT TOP 1 numOper,fechaPresentacion,estado FROM ideAnual WHERE ejercicio='" + ejercicio.SelectedItem.Text + "' and idCliente=" + Session("GidCliente").ToString + " order by id desc"
            End If
            myCommand.CommandText = q
            dr = myCommand.ExecuteReader()
            If Not dr.Read() Then
                'If DBNull.Value.Equals(dr("id")) Then
                dr.Close()
                Response.Write("<script language='javascript'>alert('No se encontró declaración en este periodo');</script>")
                Exit Sub
            Else
                If dr("numOper") = "0" Then 'And dr("estado") = "ACEPTADA"
                    dr.Close()
                    Response.Write("<script language='javascript'>alert('Requiere bajar el acuse de la ultima declaracion de este periodo');</script>")
                    Exit Sub
                End If
            End If
            dr.Close()
        Else
            comple = "0"
        End If
        Dim tipo
        If numDecl.Visible = False Then
            tipo = "N"
        Else
            If numDecl.SelectedIndex = 0 Or numDecl.SelectedIndex = -1 Then
                tipo = "N"
            Else
                tipo = "C"
            End If
        End If
        If tipoMensAn.SelectedItem.Text = "Mensual" Then
            If numDecl.Items.Count > 0 Then
                Session("GidMens") = numDecl.SelectedItem.Text
            Else
                Session("GidMens") = 0
            End If
            If comple = "1" Then
                Session("GidMens") = 0
            End If
            If Session("GidMens") = 0 And oper.SelectedValue.ToString = "2" Then
                Response.Write("<script language='javascript'>alert('No existe declaración en el periodo');</script>")
                Exit Sub
            End If
            If ejercicio.SelectedItem.Text < 2014 Then
                frame1.Attributes("src") = "mensual.aspx?ejercicio=" + ejercicio.SelectedItem.Text + "&mes=" + mes.SelectedValue.ToString + "&op=" + oper.SelectedValue.ToString + "&subop=" + via.SelectedValue.ToString + "&comple=" + comple + "&nc=" + tipo + "&pl=" + elplan.Text + "&contra=" + contratos.SelectedItem.Text
            Else
                frame1.Attributes("src") = "mensual2.aspx?ejercicio=" + ejercicio.SelectedItem.Text + "&mes=" + mes.SelectedValue.ToString + "&op=" + oper.SelectedValue.ToString + "&subop=" + via.SelectedValue.ToString + "&comple=" + comple + "&nc=" + tipo + "&pl=" + elplan.Text + "&contra=" + contratos.SelectedItem.Text
            End If
        Else 'anual
            If comple = "1" Then
                Session("GidAnual") = 0
            End If
            If Session("GidAnual") = 0 And oper.SelectedValue.ToString = "2" Then
                Response.Write("<script language='javascript'>alert('No existe declaración en el periodo');</script>")
                Exit Sub
            End If
            'If session("GidAnual") = 0 And oper.SelectedValue.ToString = "3" Then
            '    Response.Write("<script language='javascript'>alert('No hay declaraciones mensuales en el ejercicio, elija Crear declaración en ceros');</script>")
            '    Exit Sub
            'End If
            'If session("GidAnual") <> 0 Then
            '    q = "SELECT nOpers FROM ideAnual WHERE id=" + numDecl.Text
            '    myCommand = New SqlCommand(q, myConnection)
            '    dr = myCommand.ExecuteReader()
            '    dr.Read()
            '    If dr("nOpers") <> 0 And oper.SelectedValue.ToString = "1" Then 'ceros
            '        dr.Close()
            '        Response.Write("<script language='javascript'>alert('Ya hay datos en el periodo, no se puede crear en ceros');</script>")
            '        Exit Sub
            '    End If
            '    dr.Close()
            'End If
            If ejercicio.SelectedItem.Text < 2014 Then
                frame1.Attributes("src") = "anual.aspx?ejercicio=" + ejercicio.SelectedItem.Text + "&op=" + oper.SelectedValue.ToString + "&subop=" + via.SelectedValue.ToString + "&comple=" + comple + "&nc=" + tipo + "&pl=" + elplan.Text + "&contra=" + contratos.SelectedItem.Text
            Else
                frame1.Attributes("src") = "anual2.aspx?ejercicio=" + ejercicio.SelectedItem.Text + "&op=" + oper.SelectedValue.ToString + "&subop=" + via.SelectedValue.ToString + "&comple=" + comple + "&nc=" + tipo + "&pl=" + elplan.Text + "&contra=" + contratos.SelectedItem.Text
            End If

        End If
    End Sub

    Private Function validaContrato() As Integer
        Dim q
        q = "SELECT periodoInicial, fechaFinal, esRegularizacion, anualEnPremium, nAdeudos FROM contratos where id=" + contratos.SelectedItem.Text
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()

        If contratos.Visible = False And oper.SelectedValue.ToString <> "2" Then 'sin contrato vigente y no es consulta            
            Response.Write("<script language='javascript'>alert('No cuenta con contrato vigente para aceder a estas operaciones o bien A alcanzado el máximo de declaraciones contratadas');</script>")
            dr.Close()
            Return 0
        End If

        If oper.SelectedValue.ToString <> "2" Then
            If tipoMensAn.SelectedItem.Text = "Mensual" Then
                If elplan.Text = "PREMIUM" Then
                    If CDate(ejercicio.SelectedItem.Text + "/" + mes.SelectedValue.ToString + "/01") < CDate(dr("periodoInicial")) Or CDate(ejercicio.SelectedItem.Text + "/" + mes.SelectedValue.ToString + "/01") > CDate(dr("fechaFinal")) Then
                        Response.Write("<script language='javascript'>alert('En este contrato el periodo a declarar solo puede estar entre el rango de fechas " + Left(dr("periodoInicial").ToString, 10) + " - " + Left(dr("fechaFinal").ToString, 10) + "');</script>")
                        dr.Close()
                        Return 0
                    End If
                    'Else 'no premiums
                    '    'se considera el mes anterior todavia como actual por las fechas de corte
                    '    Dim fechaUltima = DateAdd(DateInterval.Day, -DatePart(DateInterval.Day, Now()) + 1, Now()) 'dia 1o del mes actual
                    '    fechaUltima = DateAdd(DateInterval.Month, -1, fechaUltima) 'dia 1o del mes anterior
                    '    If dr("esRegularizacion").Equals(True) Then
                    '        If CDate(ejercicio.Text + "/" + mes.Text + "/01") >= CDate(Format(fechaUltima, "yyyy-MM-dd")) Then
                    '            Response.Write("<script language='javascript'>alert('En este contrato solo puede declarar periodos previos al " + Format(fechaUltima, "dd/MM/yyyy") + ", pues contrató regularizaciones de periodos anteriores');</script>")
                    '            dr.Close()
                    '            Return 0
                    '        End If
                    '    Else 'si ya me pagó mas sin regularizacion(sin descto), puede usarla en fechasPrevias
                    '        If CDate(ejercicio.Text + "/" + mes.Text + "/01") < CDate(fechaUltima) Then
                    '            dr.Close()
                    '            Response.Write("<script language='javascript'>if (confirm('Este contrato NO es de regularización, le sugerimos usar uno que si lo sea para el periodo elegido aunque podría utilizar este \n ¿Desea elegir otro contrato?')==true){document.getElementById('<%=HiddenField1.ClientID %>').value = 'si';return 1;}else{document.getElementById('<%=HiddenField1.ClientID %>').value = 'no';return 0;}</script>")
                    '            '    MsgBox("En este contrato solo puede declarar periodos >= al " + fechaUltima.ToString, , "")
                    '            If HiddenField1.Value = "si" Then 'no tiene value
                    '                Return 0
                    '            End If
                    '        End If
                    '    End If
                End If
            Else 'anual
                If elplan.Text = "PREMIUM" And dr("anualEnPremium").Equals(True) Then
                    If DatePart(DateInterval.Year, dr("periodoInicial")) <> DatePart(DateInterval.Year, dr("fechaFinal")) Then 'cambio de años
                        If ejercicio.SelectedItem.Text < CStr(DatePart(DateInterval.Year, dr("periodoInicial"))) Or (ejercicio.SelectedItem.Text >= CStr(DatePart(DateInterval.Year, dr("fechaFinal"))) And CStr(DatePart(DateInterval.Month, dr("fechaFinal"))) <> "12") Then
                            Response.Write("<script language='javascript'>alert('En este contrato el periodo a declarar solo puede estar entre " + CStr(DatePart(DateInterval.Year, dr("periodoInicial"))) + " - " + CStr(DatePart(DateInterval.Year, DateAdd(DateInterval.Year, -1, dr("fechaFinal")))) + "');</script>")
                            dr.Close()
                            Return 0
                        End If
                        If ejercicio.SelectedItem.Text < CStr(DatePart(DateInterval.Year, dr("periodoInicial"))) Or (ejercicio.SelectedItem.Text > CStr(DatePart(DateInterval.Year, dr("fechaFinal"))) And CStr(DatePart(DateInterval.Month, dr("fechaFinal"))) = "12") Then
                            Response.Write("<script language='javascript'>alert('En este contrato el periodo a declarar solo puede estar entre " + CStr(DatePart(DateInterval.Year, dr("periodoInicial"))) + " - " + CStr(DatePart(DateInterval.Year, dr("fechaFinal"))) + "');</script>")
                            dr.Close()
                            Return 0
                        End If
                    Else 'mismo año
                        If ejercicio.SelectedItem.Text <> CStr(DatePart(DateInterval.Year, dr("periodoInicial"))) Then
                            Response.Write("<script language='javascript'>alert('En este contrato el periodo a declarar solo puede ser del " + CStr(DatePart(DateInterval.Year, dr("periodoInicial"))) + "');</script>")
                            dr.Close()
                            Return 0
                        End If
                    End If


                    'Else 'no premiums
                    '    'se considera el año anterior todavia como actual por las fechas de corte
                    '    Dim fechaUltima = DateAdd(DateInterval.Year, -1, Now()) 'dia 1o del año anterior
                    '    If dr("esRegularizacion").Equals(True) Then
                    '        If CDate(ejercicio.Text + "/01/01") >= CDate(Format(fechaUltima, "yyyy-MM-dd")) Then
                    '            Response.Write("<script language='javascript'>alert('En este contrato solo puede declarar anuales previas al " + Format(fechaUltima, "yyyy") + ", pues contrató regularizaciones de periodos anteriores');</script>")
                    '            dr.Close()
                    '            Return 0
                    '        End If
                    '    Else 'si ya me pagó mas sin regularizacion(sin descto), puede usarla en fechasPrevias
                    '        If CDate(ejercicio.Text + "/" + mes.Text + "/01") < CDate(fechaUltima) Then
                    '            dr.Close()
                    '            Response.Write("<script language='javascript'>if (confirm('Este contrato NO es de regularización, le sugerimos usar uno que si lo sea para el ejercicio elegido aunque podría utilizar este \n ¿Desea elegir otro contrato?')==true){document.getElementById('<%=HiddenField1.ClientID %>').value = 'si';return 1;}else{document.getElementById('<%=HiddenField1.ClientID %>').value = 'no';return 0;}</script>")
                    '            '    MsgBox("En este contrato solo puede declarar periodos >= al " + fechaUltima.ToString, , "")
                    '            If HiddenField1.Value = "si" Then
                    '                Return 0
                    '            End If
                    '        End If
                    '    End If
                End If
            End If
            If dr("nAdeudos") > 1 Then 'mas de 1 adeudos 
                Response.Write("<script language='javascript'>alert('Tiene 2 ó + Adeudos pendientes en este contrato');</script>")
                dr.Close()
                Return 0
            End If
        End If
        dr.Close()

        Return 1
    End Function


    Protected Sub oper_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles oper.SelectedIndexChanged
        If oper.SelectedValue.ToString <> "1" Then 'crear o consultar decl c vals 
            lblVia.Visible = True
            via.Visible = True
        Else
            lblVia.Visible = False
            via.Visible = False
            'complementaria.Checked = False
        End If
    End Sub

    Protected Sub ejercicio_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ejercicio.SelectedIndexChanged
        Dim q = "SELECT TOP 1 id FROM ideAnual WHERE ejercicio='" + ejercicio.SelectedItem.Text + "' and idCliente=" + Session("GidCliente").ToString + " order by id ASC"
        myCommand = New SqlCommand(q, myConnection)
        myCommand.CommandText = q
        dr = myCommand.ExecuteReader()
        If Not dr.Read() Then
            Session("GidAnual") = 0
        Else
            Session("GidAnual") = dr("id")
        End If
        dr.Close()

        cargaNumDecls()

    End Sub

    Protected Sub mes_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles mes.SelectedIndexChanged
        cargaNumDecls()
    End Sub

    Protected Sub contratos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles contratos.SelectedIndexChanged
        cargaContrato()
        cargaNumDecls()
        'If IsPostBack Then
        '    cargaNumDecls()
        'End If
    End Sub

    Private Sub cargaContrato()
        Dim q = "SELECT pla.elplan, co.esRegularizacion FROM planes pla, contratos co WHERE co.id=" + contratos.SelectedItem.Text + " and co.idPlan=pla.id"
        myCommand2 = New SqlCommand(q, myConnection)
        myCommand2.CommandText = q
        dr = myCommand2.ExecuteReader()
        dr.Read()
        elplan.Text = dr("elplan")
        idContrato.Text = contratos.SelectedItem.Text
        Session("GidContrato") = contratos.SelectedItem.Text
        If dr("esRegularizacion").Equals(True) Then
            esRegularizacion.Text = "Regulariza periodos anteriores"
        Else
            esRegularizacion.Text = "No regulariza periodos anteriores"
        End If
        dr.Close()

        If elplan.Text = "ANUAL DESDE 2014 CEROS COMPLEMENTARIA" Or elplan.Text = "ANUAL DESDE 2014 CEROS NORMAL" Or elplan.Text = "ANUAL DESDE 2014 DATOS COMPLEMENTARIA" Or elplan.Text = "ANUAL DESDE 2014 DATOS NORMAL" Then
            tipoMensAn.Items.FindByValue("Mensual").Enabled = False
            mes.Visible = False
            lblMes.Visible = False
            tipoMensAn.SelectedValue = "Anual"
        Else
            tipoMensAn.Items.FindByValue("Mensual").Enabled = True
            mes.Visible = True
            lblMes.Visible = True
        End If

        'ocultando crear declaraciones para los contratos no vigentes
        If tipoMensAn.SelectedItem.Text = "Mensual" Then
            Dim fechaUltima = DateAdd(DateInterval.Day, -DatePart(DateInterval.Day, Now()) + 1, Now()) 'dia 1o del mes actual
            fechaUltima = DateAdd(DateInterval.Month, -1, fechaUltima) 'dia 1o del mes anterior
            q = "SELECT co.id FROM contratos co, planes pla WHERE co.id=" + idContrato.Text + " AND co.idPlan=pla.id AND ( (nDeclHechas < nDeclContratadas and (pla.elplan='BASICO' OR pla.elplan='CEROS')) or ('" + Format(Convert.ToDateTime(fechaUltima), "yyyy-MM-dd") + "' <= fechaFinal and pla.elplan='PREMIUM' and co.esRegularizacion=0) or (pla.elplan='PREMIUM' and co.esRegularizacion=1) ) "
            myCommand = New SqlCommand(q, myConnection)
            myCommand.CommandText = q
            dr = myCommand.ExecuteReader()
            dr.Read()
            If Not dr.HasRows Then
                dr.Close()
                oper.Items.FindByValue("0").Enabled = False
                oper.Items.FindByValue("1").Enabled = False
                complementaria.Visible = False
                via.Visible = False
            Else
                oper.Items.FindByValue("0").Enabled = True
                oper.Items.FindByValue("1").Enabled = True                
                complementaria.Visible = True
                via.Visible = True
                If elplan.Text = "CEROS" Then
                    oper.Items.FindByValue("0").Enabled = False 'deshabilita envio <> de ceros
                Else
                    oper.Items.FindByValue("0").Enabled = True
                End If                
                dr.Close()
                q = "SELECT TOP 1 M.id FROM ideMens M, ideAnual A WHERE A.ejercicio='" + ejercicio.SelectedItem.Text + "' and A.idCliente=" + Session("GidCliente").ToString + " and M.mes=" + mes.SelectedValue.ToString + " and M.idAnual=A.id and (M.estado='ACEPTADA' or M.estado='CONTINGENCIA')"
                myCommand = New SqlCommand(q, myConnection)
                myCommand.CommandText = q
                dr = myCommand.ExecuteReader()
                If dr.Read() Then
                    'oper.Items.FindByValue("1").Enabled = False 'deshabilita envio en ceros, si ya hay declaraciones en ese mes, tendria que hacerse via complementaria
                Else
                    oper.Items.FindByValue("1").Enabled = True
                End If
                dr.Close()                
            End If
        Else 'anual
            Dim fechaVariable = CDate(CStr(DatePart(DateInterval.Year, Now())) + "/01/01") 'dia 1o de este año
            Dim anualEnPremiumVal
            q = "SELECT co.id,co.anualEnPremium FROM contratos co, planes pla WHERE co.id=" + idContrato.Text + " AND co.idPlan=pla.id AND ( (nDeclHechas < nDeclContratadas and pla.elplan<>'PREMIUM') or ('" + Format(Convert.ToDateTime(fechaVariable), "yyyy-MM-dd") + "' > periodoInicial and '" + Format(Convert.ToDateTime(fechaVariable), "yyyy-MM-dd") + "' <= fechaFinal and pla.elplan='PREMIUM' and co.anualEnPremium=1 and co.esRegularizacion=0) or (pla.elplan='PREMIUM' and co.anualEnPremium=1 and co.esRegularizacion=1)  ) "
            myCommand = New SqlCommand(q, myConnection)
            myCommand.CommandText = q
            dr = myCommand.ExecuteReader()
            If Not dr.HasRows Then
                dr.Close()
                oper.Items.FindByValue("0").Enabled = False
                oper.Items.FindByValue("1").Enabled = False
                'oper.Items.FindByValue("3").Enabled = False
                complementaria.Visible = False
                via.Visible = False                
            Else
                dr.Read()
                If dr("anualEnPremium").Equals(True) Then
                    anualEnPremiumVal = 1
                Else
                    anualEnPremiumVal = 0
                End If
                oper.Items.FindByValue("0").Enabled = True
                oper.Items.FindByValue("1").Enabled = True
                complementaria.Visible = True                
                via.Visible = True
                If elplan.Text = "CEROS" Or elplan.Text = "ANUAL DESDE 2014 CEROS COMPLEMENTARIA" Or elplan.Text = "ANUAL DESDE 2014 CEROS NORMAL" Then
                    oper.Items.FindByValue("0").Enabled = False 'deshabilita envio <> de ceros
                Else
                    oper.Items.FindByValue("0").Enabled = True
                End If
                dr.Close()
                'q = "SELECT TOP 1 id FROM ideAnual WHERE ejercicio='" + ejercicio.SelectedItem.Text + "' and idCliente=" + Session("GidCliente").ToString + " and (estado='ACEPTADA' or estado='CONTINGENCIA')"
                'myCommand = New SqlCommand(q, myConnection)
                'myCommand.CommandText = q
                'dr = myCommand.ExecuteReader()
                'If dr.Read() Then
                '    oper.Items.FindByValue("1").Enabled = False 'deshabilita envio en ceros, si ya hay declaraciones en ese mes, tendria que hacerse via complementaria
                'Else
                '    oper.Items.FindByValue("1").Enabled = True
                'End If
                'dr.Close()

                'validando la anual via 12 meses
                If elplan.Text = "PREMIUM" And anualEnPremiumVal = 1 Then
                    q = "SELECT TOP 1 id FROM ideAnual WHERE ejercicio='" + ejercicio.SelectedItem.Text + "' and idCliente=" + Session("GidCliente").ToString + " and estado='VACIA'"
                    myCommand = New SqlCommand(q, myConnection)
                    myCommand.CommandText = q
                    dr = myCommand.ExecuteReader()
                    If dr.Read() Then
                        Dim idAn = dr("id")
                        dr.Close()

                        Dim i
                        Dim entro = 0
                        For i = 1 To 12
                            q = "SELECT id FROM ideMens WHERE idAnual=" + idAn.ToString + " and mes='" + i.ToString + "'"
                            myCommand = New SqlCommand(q, myConnection)
                            myCommand.CommandText = q
                            dr = myCommand.ExecuteReader()
                            If Not dr.Read() Then
                                entro = 1
                                Exit For
                            End If
                            dr.Close()
                        Next i
                        'If entro = 1 Then
                        '    oper.Items.FindByValue("3").Enabled = False
                        'Else
                        '    oper.Items.FindByValue("3").Enabled = True
                        'End If
                    Else
                        dr.Close()
                        'oper.Items.FindByValue("3").Enabled = False
                    End If
                Else
                    'oper.Items.FindByValue("3").Enabled = False
                End If
            End If
        End If

        If elplan.Text = "CEROS" Or elplan.Text = "ANUAL DESDE 2014 CEROS COMPLEMENTARIA" Or elplan.Text = "ANUAL DESDE 2014 CEROS NORMAL" Then
            oper.Items.FindByValue("0").Enabled = False
        End If
    End Sub

    Protected Sub numDecl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles numDecl.SelectedIndexChanged        
        Call cambiaVia()

        'If IsPostBack Then
        '    Session("misNdecla") = numDecl.Text
        'End If
    End Sub


    Protected Sub NavigationMenu_MenuItemClick(sender As Object, e As System.Web.UI.WebControls.MenuEventArgs) Handles NavigationMenu.MenuItemClick
        If NavigationMenu.SelectedItem.Text = "Declarar" Then
            Call limpiaSesionesDeMisDecla()
        End If
        var.Text = "4"
    End Sub

    Private Sub cambiaVia()
        Dim valini = via.SelectedValue
        via.Items.FindByValue(via.SelectedValue).Selected = False
        Dim q
        If numDecl.Visible = True Then
            If tipoMensAn.SelectedItem.Text = "Mensual" Then
                q = "SELECT viaImportacion, estado FROM ideMens WHERE id=" + numDecl.SelectedItem.Text
            Else
                q = "SELECT viaImportacion, estado FROM ideAnual WHERE id=" + numDecl.SelectedItem.Text
            End If
            myCommand = New SqlCommand(q, myConnection)
            dr = myCommand.ExecuteReader()
            If DBNull.Value.Equals(dr.Read()) Then 'null defa=xls
                via.Items.FindByValue(valini).Selected = True
            ElseIf dr("estado") = "ACEPTADA" And complementaria.Checked = False Then
                If dr("viaImportacion") = 0 Then
                    via.Items.FindByValue("0").Selected = True
                ElseIf dr("viaImportacion") = 1 Then
                    via.Items.FindByValue("0").Selected = True
                ElseIf dr("viaImportacion") = 2 Then
                    via.Items.FindByValue("1").Selected = True
                End If
            Else
                via.Items.FindByValue(valini).Selected = True 'restablecer
            End If
        Else 'nueva decl
            via.Items.FindByValue(valini).Selected = True 'restablecer
        End If
    End Sub

    Protected Sub via_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles via.SelectedIndexChanged
        Call cambiaVia()
    End Sub

    Protected Sub restablecer_Click(sender As Object, e As EventArgs) Handles restablecer.Click
        myCommand = New SqlCommand("UPDATE contratos SET nDeclHechas=nDeclHechas-1 WHERE id=" + contratos.SelectedItem.Text + " and nDeclHechas>0", myConnection)
        myCommand.ExecuteNonQuery()

        If tipoMensAn.SelectedItem.Text = "Anual" Then
            'myCommand2 = New SqlCommand("UPDATE ideAnual SET estado='VACIA' WHERE id=" + numDecl.SelectedItem.Text, myConnection)
            'myCommand2.ExecuteNonQuery()

            myCommand2 = New SqlCommand("DELETE FROM tCotitularAnual WHERE idCotitularesCuentaAnual In (Select id FROM cotitularesCuentaAnual WHERE idideDetAnual In (Select id FROM ideDetAnual WHERE idAnual=" + numDecl.SelectedItem.Text + "))", myConnection)
            myCommand2.ExecuteNonQuery()
            myCommand2 = New SqlCommand("DELETE FROM cotitularesCuentaAnual WHERE idideDetAnual IN (SELECT id FROM ideDetAnual WHERE idAnual=" + numDecl.SelectedItem.Text + ")", myConnection)
            myCommand2.ExecuteNonQuery()
            myCommand2 = New SqlCommand("DELETE FROM cotitularesCuentaAnual WHERE idideDetAnual IN (SELECT id FROM ideDetAnual WHERE idAnual=" + numDecl.SelectedItem.Text + ")", myConnection)
            myCommand2.ExecuteNonQuery()
            myCommand2 = New SqlCommand("DELETE FROM mov WHERE idCuentasIdeDetAnual IN (SELECT id FROM cuentasIdeDetAnual where idIdeDetAnual IN (select id FROM ideDetAnual WHERE idAnual=" + numDecl.SelectedItem.Text + "))", myConnection)
            myCommand2.ExecuteNonQuery()
            myCommand2 = New SqlCommand("DELETE FROM cuentasIdeDetAnual WHERE idideDetAnual IN (SELECT id FROM ideDetAnual WHERE idAnual=" + numDecl.SelectedItem.Text + ")", myConnection)
            myCommand2.ExecuteNonQuery()
            myCommand2 = New SqlCommand("DELETE FROM ideDetAnual WHERE idAnual=" + numDecl.SelectedItem.Text, myConnection)
            myCommand2.ExecuteNonQuery()


            Dim dr3 As SqlDataReader
            Dim q = "SELECT TOP 1 id FROM ideMens WHERE idAnual=" + numDecl.SelectedItem.Text + " AND estado<>'VACIA'"
            myCommand2 = New SqlCommand(q, myConnection)
            dr3 = myCommand2.ExecuteReader()
            If (Not dr3.HasRows) Then 'sin mensuales aceptadas, importadas o creadas, errEnvio
                myCommand2 = New SqlCommand("DELETE FROM ideMens WHERE idAnual=" + numDecl.SelectedItem.Text, myConnection)
                myCommand2.ExecuteNonQuery()
                myCommand2 = New SqlCommand("DELETE FROM ideAnual WHERE id=" + numDecl.SelectedItem.Text, myConnection)
                myCommand2.ExecuteNonQuery()
            Else
                'limpiando anual
                myCommand2 = New SqlCommand("UPDATE ideAnual SET nOpers=0,impteExcedente=0,impteDeterminado=0,impteRecaudado=0,imptePendienteRecaudar=0,numOper='0',normalComplementaria='NORMAL', estado='VACIA' WHERE id=" + numDecl.SelectedItem.Text, myConnection)
                myCommand2.ExecuteNonQuery()
            End If
            dr3.Close()
        Else
            'myCommand2 = New SqlCommand("UPDATE ideMens SET estado='VACIA' WHERE id=" + numDecl.SelectedItem.Text, myConnection)
            'myCommand2.ExecuteNonQuery()
            Dim q = "DELETE FROM tCotitular WHERE idCotitularesCuenta IN (SELECT id FROM cotitularesCuenta WHERE idideDet IN (SELECT id FROM ideDet WHERE idMens=" + numDecl.SelectedItem.Text + "))"
            myCommand = New SqlCommand(q, myConnection)
            myCommand.ExecuteNonQuery()
            q = "DELETE FROM cotitularesCuenta WHERE idideDet IN (SELECT id FROM ideDet WHERE idMens=" + numDecl.SelectedItem.Text + ")"
            myCommand = New SqlCommand(q, myConnection)
            myCommand.ExecuteNonQuery()
            q = "DELETE FROM ideDet WHERE idMens=" + numDecl.SelectedItem.Text
            myCommand = New SqlCommand(q, myConnection)
            myCommand.ExecuteNonQuery()
            myCommand2 = New SqlCommand("DELETE FROM ideMens WHERE id=" + numDecl.SelectedItem.Text, myConnection)
            myCommand2.ExecuteNonQuery()
        End If
        Response.Write("<script language='javascript'>alert('Restablecido');</script>")
        Response.Write("<script>location.href='misdecla.aspx';</script>")
    End Sub

    Private Sub limpiaSesionesDeMisDecla()
        Session("misEjercicio") = ""
        Session("misNdecla") = ""
        Session("misContrato") = ""
        Session("misMes") = ""
        Session("misTipo") = ""
        Session("misIdAnual") = ""
    End Sub


    Protected Sub complementaria_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles complementaria.CheckedChanged

    End Sub
End Class