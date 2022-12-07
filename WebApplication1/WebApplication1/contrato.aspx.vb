Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Office.Interop.Excel
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.IO
Imports System
Imports System.Security.AccessControl
Imports System.Security


Public Class WebForm8
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim myCommand2 As SqlCommand
    Dim dr As SqlDataReader
    Dim dr2 As SqlDataReader
    Dim cambios
    Dim fecha


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("curCorreo")) = True Then
            Response.Write("<script language='javascript'>alert('Expiró su sesión');</script>")
            Session.Abandon()
            Response.Write("<script language='javascript'>location.href='Login.aspx';</script>")
            Exit Sub
        End If

        myCommand = New SqlCommand("set dateformat ymd")
        ExecuteNonQueryFunction(myCommand)

        If Not (Left(Request.ServerVariables("REMOTE_ADDR"), 10) = "192.168.0." Or Left(Request.ServerVariables("REMOTE_ADDR"), 10) = "127.0.0.1") Or Session("runAsAdmin") = "1" Then
            If IsNothing(Session("curCorreo")) = True Then
                Response.Write("<script language='javascript'>alert('Expiró su sesión');</script>")
                Session.Abandon()
                Response.Write("<script language='javascript'>location.href='Login.aspx';</script>")
                Exit Sub
            End If
        End If

        If Not IsPostBack Then  '1a vez
            If Left(Request.ServerVariables("REMOTE_ADDR"), 10) = "192.168.0." Or Left(Request.ServerVariables("REMOTE_ADDR"), 10) = "127.0.0.1" Then
                Session("runAsAdmin") = "1"
            End If

            If HttpContext.Current.Request.IsLocal Or Session("runAsAdmin") = "1" Then
                comisionPagada.Visible = True
                factTx.Visible = True
                enviada.Visible = True
                chkPostpago.Visible = True
                pagoRealizado.Visible = True
                lbluuid.Visible = True
                lblformapago.Visible = True
                uuid.Visible = True
                codCliente.Visible = True
                lblcortes.Visible = True
                lblfechaPagado.Visible = True
                sinCorr.Visible = True
                fechaPago.Visible = True
                cotizar.Visible = True
                calc.Visible = True
                redondear.Visible = True
                yFac.Visible = True
                chkSubtotal.Visible = True
                soloFac.Visible = True
                deCortesia.Visible = True
                vencido.Visible = True
                panelAdm.Visible = True
            Else
                comisionPagada.Visible = False
                factTx.Visible = False
                enviada.Visible = False
                chkPostpago.Visible = False
                pagoRealizado.Visible = False
                lbluuid.Visible = False
                lblformapago.Visible = False
                uuid.Visible = False
                codCliente.Visible = False
                lblcortes.Visible = False
                lblfechaPagado.Visible = False
                sinCorr.Visible = False
                fechaPago.Visible = False
                cotizar.Visible = False
                calc.Visible = False
                redondear.Visible = False
                yFac.Visible = False
                chkSubtotal.Visible = False
                soloFac.Visible = False
                deCortesia.Visible = False
                vencido.Visible = False
                panelAdm.Visible = False
            End If

            cambios = 0
            Session("esEl1oVal") = "0"
            If Session("GidContrato") = "" Or Session("GidContrato") = Nothing Then
                id.Text = ""
            Else
                id.Text = Session("GidContrato").ToString
            End If
            If Session("GidContrato") <> Nothing Then 'edit
                cargaDatos()
                del.Visible = True
                actPago.Visible = True
                nDeclHechasCaptura.Visible = True
                actNdeclsHechas.Visible = True
            Else 'alta
                nDeclHechas.Text = 0
                del.Visible = False
                actPago.Visible = False
                nDeclHechasCaptura.Visible = False
                actNdeclsHechas.Visible = False

                duracionMeses.Visible = False 'para basicos/ceros
                mesesRegularizacion.Value = 0
                mesesAnticipados.Value = 0
                fechaFinal.Visible = False
                nDeclContratadas.Visible = True
                nDeclHechas.Visible = True

                pagoRealizado.SelectedValue = "03"
            End If
            idCliente.Text = Session("GidCliente")
            Dim q2
            q2 = "SELECT razonSoc FROM clientes WHERE id=" + Session("GidCliente").ToString
            myCommand = New SqlCommand(q2)
            Dim v = ExecuteScalarFunction(myCommand)
            cliente.Text = v
            If Session("runAsAdmin") = "0" Then 'via cliente
                acepto.Enabled = True
                selCliente.Enabled = False
                idCliente.Enabled = False
                cliente.Enabled = False
                fechaPago.Enabled = False
                atras.Visible = False
                del.Visible = False
                actPago.Visible = False
                nDeclHechasCaptura.Visible = False
                actNdeclsHechas.Visible = False
                comisionPagada.Visible = False
                factTx.Visible = False
                enviada.Visible = False
                pagosInsuficiente.Visible = False
                piDiferencia.Visible = False
                piDiferenciaLbl.Visible = False
                parcialidades.Visible = False
                lblNadeudos.Visible = False
                nAdeudos.Visible = False
                lblMontoAdeudos.Visible = False
                montoAdeudos.Visible = False
                vencido.Visible = False
                lblNvoPrec.Visible = False
                nvoPrecNeto.Visible = False
                deCortesia.Visible = False

                If fechaPago.Text <> "" Then
                    'If CDate(fechaPago.Text.Trim) < DateTime.Now Then 'una vez pagado no puede modificar nada
                    periodoInicial.Enabled = False
                    elPlan.Enabled = False
                    acepto.Enabled = False
                    duracionMeses.Enabled = False
                    nDeclContratadas.Enabled = False
                    addEdit.Visible = False
                    cotizar.Visible = False
                    esRegularizacion.Enabled = False
                    codCliente.Enabled = False
                    'End If
                End If
            Else 'server o admin
                If IsNothing(Session("curCorreo")) = True Then 'sin sesion abierta
                    acepto.Enabled = False
                    'addEdit.Visible = False 'p comisiones 
                    misContra.Visible = False
                    pagos.Visible = False
                    'del.Visible = False

                    'Else
                    '    If Session("curCorreo").ToString.ToUpper = "PRUEBASDEIDE@GMAIL.COM" Then
                    '        acepto.Enabled = False
                    '    End If
                End If
            End If
        Else    'refresh after press butons
        End If

    End Sub

    Private Sub cargaDatos()
        Dim q
        q = "SELECT co.*,cli.razonSoc, pla.elplan FROM contratos co, clientes cli, planes pla  where co.id='" + id.Text + "' and co.idPlan=pla.id and co.idCliente=cli.id"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            dr.Read()
            idCliente.Text = dr("idcliente")
            cliente.Text = dr("razonSoc")
            If Not DBNull.Value.Equals(dr("fechapago")) Then
                fechaPago.Text = dr("fechapago")
            Else
                fechaPago.Text = ""
            End If
            If dr("esRegularizacion").Equals(False) Then
                esRegularizacion.Checked = False
            Else
                esRegularizacion.Checked = True
            End If
            fecha = Left(dr("fecha").ToString, 10)
            'Me.actPago.OnClientClick = "document.getElementById('form1').target = '_self'; return confirm('¿ Pasaron 3o+ dias hábiles desde la fecha del contrato " + fecha + " y Ya cotizó hoy sin diferencias con el monto pre-contratado ?');"
            acepto.Checked = True
            periodoInicial.Text = dr("periodoinicial")
            idPlan.Text = dr("idplan")

            elPlan.DataBind() 'combo enlazado a campo
            elPlan.Items.FindByValue(dr("elplan")).Selected = True

            If elPlan.Text = "PREMIUM" Then
                duracionMeses.Text = dr("duracionmeses")
                mesesRegularizacion.Value = dr("mesesRegularizacion")
                mesesAnticipados.Value = dr("mesesAnticipados")
                fechaFinal.Text = dr("fechafinal")

                duracionMeses.Visible = True 'para premium
                fechaFinal.Visible = True
                nDeclContratadas.Visible = False
                nDeclHechas.Visible = False
            Else
                nDeclContratadas.Text = dr("ndeclcontratadas")
                nDeclHechas.Text = dr("ndeclhechas")

                duracionMeses.Visible = False 'para basicos/ceros
                fechaFinal.Visible = False
                nDeclContratadas.Visible = True
                nDeclHechas.Visible = True

            End If

            Dim q3
            q3 = "SELECT * FROM desctos WHERE id IN (SELECT idDescto FROM desctosContra WHERE idContra=" + id.Text + ")"
            myCommand = New SqlCommand(q3)
            Using dr3 = ExecuteReaderFunction(myCommand)
                desglose.Text = ""
                Dim sumaDesctos = 0
                While dr3.Read()
                    sumaDesctos = sumaDesctos + dr3("porcen")
                    desglose.Text = desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + vbCrLf
                End While
            End Using

            precioNetoContrato.Text = FormatCurrency(dr("precionetocontrato"))
            If Not DBNull.Value.Equals(dr("nvoPrecNeto")) Then
                nvoPrecNeto.Text = FormatCurrency(dr("nvoPrecNeto"))
            End If

            If dr("esEl1o") = True Then
                Session("esEl1oVal") = "1"
            Else
                Session("esEl1oVal") = "0"
            End If

            If dr("comisionPagada").Equals(False) Then
                comisionPagada.Checked = False
            Else
                comisionPagada.Checked = True
            End If

            If dr("factTx").Equals(False) Then
                enviada.Checked = False
            Else
                enviada.Checked = True
            End If

            If dr("deCortesia").Equals(False) Then
                deCortesia.Checked = False
            Else
                deCortesia.Checked = True
            End If

            If (dr("postpago").Equals(True)) Then '
                chkPostpago.Checked = True
            Else
                chkPostpago.Checked = False
            End If

            If dr("parcialidades").Equals(False) Then
                parcialidades.Checked = False
            Else
                parcialidades.Checked = True
            End If

            If dr("vencido").Equals(False) Then
                vencido.Checked = False
            Else
                vencido.Checked = True
            End If

            nAdeudos.Text = dr("nAdeudos")
            montoAdeudos.Text = FormatCurrency(dr("montoAdeudos"))

            If Not DBNull.Value.Equals(dr("pagoRealizado")) Then
                pagoRealizado.SelectedValue = dr("pagoRealizado")
            Else
                pagoRealizado.SelectedValue = "03"
            End If

            If Not DBNull.Value.Equals(dr("uuid")) Then
                uuid.Text = dr("uuid")
            End If
        End Using

        q = "SELECT cod from desctos where tipo='REF' and id in (select idDescto from desctosContra where idContra=" + id.Text + ")"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            codCliente.Text = v
        End If
    End Sub

    Private Sub WebForm8_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'myConnection.Close()
    End Sub

    Protected Sub selCliente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles selCliente.Click
        Response.Write("<script language='javascript'>window.open('clienteList.aspx?op=c','clienteList','location=NO');</script>")
    End Sub

    Protected Sub elPlan_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles elPlan.SelectedIndexChanged

        preciosFechas()
    End Sub

    Private Function ultdiames(ByVal mesVariable, ByVal anio) As Integer
        Select Case mesVariable
            Case 1, 3, 5, 7, 8, 10, 12
                Return 31
            Case 4, 6, 9, 11
                Return 30
            Case 2 'Feb
                If anio Mod 4 <> 0 Then
                    Return 28 'normal 2009,2010,2011,2013,2014,2015
                Else
                    Return 29 'bisiesto; (Año)/4=entero; 2008,2012,2016
                End If
        End Select
    End Function

    Private Function requisitado(ByVal idPrerequisito) As Integer
        Dim q4
        q4 = "SELECT * FROM desctos WHERE id=" + idPrerequisito.ToString + " AND (tipo='PROMO' or tipo='REF') AND convert(datetime,convert(int,GETDATE())) <= fechaCaducidad" 'el parent promo y vigente
        myCommand = New SqlCommand(q4)
        Using dr4 = ExecuteReaderFunction(myCommand)
            If dr4.Read() Then
                If dr4("regularizacion").Equals(False) And dr4("anticipadas").Equals(False) And dr4("duracionMeses") = 0 Then 'PREMIUM
                    Dim dr5 As SqlDataReader
                    Dim q5
                    q5 = "SELECT id FROM contratos WHERE idPlan IN (SELECT id FROM planes WHERE elplan='PREMIUM') AND idCliente=" + idCliente.Text + " AND fechaPago IS NOT NULL"
                    myCommand = New SqlCommand(q5)
                    Dim v = ExecuteScalarFunction(myCommand)
                    If IsNothing(v) Then
                        Return 0
                    End If
                ElseIf dr4("regularizacion").Equals(False) And dr4("anticipadas").Equals(False) And dr4("duracionMeses") <> 0 Then 'PREMIUM y duracionMeses
                    Dim dr5 As SqlDataReader
                    Dim q5
                    q5 = "SELECT id FROM contratos WHERE idPlan IN (SELECT id FROM planes WHERE elplan='PREMIUM') AND idCliente=" + idCliente.Text + " AND fechaPago IS NOT NULL AND duracionMeses=" + dr4("duracionMeses").ToString
                    myCommand = New SqlCommand(q5)
                    Dim v = ExecuteScalarFunction(myCommand)
                    If IsNothing(v) Then
                        Return 0
                    End If
                ElseIf dr4("regularizacion").Equals(False) And dr4("anticipadas").Equals(True) And dr4("duracionMeses") <> 0 Then 'PREMIUM, duracionMeses, Anticipadas
                    Dim dr5 As SqlDataReader
                    Dim q5
                    q5 = "SELECT id FROM contratos WHERE idPlan IN (SELECT id FROM planes WHERE elplan='PREMIUM') AND idCliente=" + idCliente.Text + " AND fechaPago IS NOT NULL AND mesesAnticipados=" + dr4("duracionMeses").ToString
                    myCommand = New SqlCommand(q5)
                    Dim v = ExecuteScalarFunction(myCommand)
                    If IsNothing(v) Then
                        Return 0
                    End If
                ElseIf dr4("regularizacion").Equals(True) And dr4("anticipadas").Equals(False) And dr4("duracionMeses") <> 0 Then 'PREMIUM, duracionMeses, Regularizadas
                    Dim dr5 As SqlDataReader
                    Dim q5
                    q5 = "SELECT id FROM contratos WHERE idPlan IN (SELECT id FROM planes WHERE elplan='PREMIUM') AND idCliente=" + idCliente.Text + " AND fechaPago IS NOT NULL AND mesesRegularizacion=" + dr4("duracionMeses").ToString
                    myCommand = New SqlCommand(q5)
                    Dim v = ExecuteScalarFunction(myCommand)
                    If IsNothing(v) Then
                        Return 0
                    End If
                End If
            Else
                Return 0
            End If
        End Using


        Return 1
    End Function

    Private Sub preciosFechas()
        Dim q, planPrecioBaseMes, planIva, planInscrip

        If nDeclContratadas.Text = "" Then
            nDeclContratadas.Text = "0"
        End If

        If duracionMeses.Text = "" Then
            duracionMeses.Text = "0"
            mesesAnticipados.Value = 0
            mesesRegularizacion.Value = 0
        End If

        If elPlan.Text = "PREMIUM" And periodoInicial.Text <> "" Then
            fechaFinal.Text = DateAdd(DateInterval.Month, CDbl(duracionMeses.Text) - 1, CDate(periodoInicial.Text))
            fechaFinal.Text = ultdiames(DatePart(DateInterval.Month, CDate(fechaFinal.Text)), DatePart(DateInterval.Year, CDate(fechaFinal.Text))).ToString + Right(fechaFinal.Text, 8)
            Call distribuyeMeses()
        End If

        q = "SELECT * FROM planes where elplan='" + elPlan.Text + "'"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            dr.Read()
            idPlan.Text = dr("id")
            planPrecioBaseMes = dr("PrecioBaseMes")
            planIva = dr("iva")
            planInscrip = dr("Inscrip")
            Session("precioNetoInscripcion") = planInscrip * (1 + planIva / 100) 'p descontarla en comisiones
        End Using


        Dim sumaDesctos As Double
        sumaDesctos = 0.0
        desglose.Text = ""
        'descto.Text = ""
        Session("entroPremium") = 0
        Session("idAplicaPromo") = 0
        Session("inscripGratis") = 0
        Session("inscripMonto") = DBNull.Value


        If esRegularizacion.Checked = True And elPlan.Text <> "PREMIUM" Then
            q = "SELECT TOP 1 * FROM desctos WHERE tipo='REG' AND convert(datetime,convert(int,GETDATE())) <= fechaCaducidad ORDER BY id DESC"
            myCommand = New SqlCommand(q)
            Using dr2 = ExecuteReaderFunction(myCommand)
                If dr2.Read() Then
                    sumaDesctos = sumaDesctos + dr2("porcen")
                    desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr2("porcen").ToString + "% " + dr2("cod") + "</br>"
                End If
            End Using

        End If

        If codCliente.Text.Trim <> "" Then
            q = "SELECT * FROM desctos WHERE tipo='REF' and cod='" + codCliente.Text.ToUpper.Trim + "'"
            myCommand = New SqlCommand(q)
            Using dr2 = ExecuteReaderFunction(myCommand)
                If Not dr2.Read() Then
                    Response.Write("<script language='javascript'>alert('Código no existe, favor de verificarlo o quitarlo');</script>")
                    codCliente.Focus()
                    Exit Sub
                Else
                    If dr2("caduca").Equals(True) Then
                        If CDate(Format(Now(), "yyyy-MM-dd")) > CDate(Format(dr2("fechaCaducidad"), "yyyy-MM-dd")) Then
                            Response.Write("<script language='javascript'>alert('Código caducado, favor de verificarlo o quitarlo');</script>")
                            codCliente.Focus()
                            Exit Sub
                        End If
                    End If
                End If

                If dr2("elplan") <> "VACIO" Then 'p solo inscrip el plan en vacio
                    If dr2("elplan") <> elPlan.Text Then 'el plan de la REF <> del contratando
                        Response.Write("<script language='javascript'>alert('El Código pertenece a un plan distinto al que está especificando en este contrato, verificar o quitar el código, o elegir el plan adecuado');</script>")
                        codCliente.Focus()
                        Exit Sub
                    End If
                End If
                sumaDesctos = sumaDesctos + dr2("porcen")
                desglose.Text = desglose.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + dr2("porcen").ToString + "% " + dr2("cod") + "</br>"
                If dr2("inscripGratis").Equals(True) Then
                    Session("inscripGratis") = 1
                End If
                If Not DBNull.Value.Equals(dr2("inscripMonto")) Then
                    'desglose.Text = desglose.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Inscripción autorizada " + dr2("inscripMonto").ToString + "</br>"
                    Session("inscripMonto") = dr2("inscripMonto")
                    Session("precioNetoInscripcion") = dr2("inscripMonto") * (1 + planIva / 100) 'p descontarla en comisiones
                End If
                If dr2("regularizacion").Equals(True) Then
                    GoTo CodigoRegBrindaPromos
                End If
            End Using
        End If

        Dim q3
        q3 = "SELECT * FROM desctos WHERE tipo='PROMO' AND convert(datetime,convert(int,GETDATE())) <= fechaCaducidad"
        myCommand = New SqlCommand(q3)
        Using dr3 = ExecuteReaderFunction(myCommand)
            While dr3.Read()
                If dr3("elplan") <> "PREMIUM" And Not DBNull.Value.Equals(dr3("inscripMonto")) Then
                    'desglose.Text = desglose.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Inscripción autorizada " + dr2("inscripMonto").ToString + "</br>"
                    Session("inscripMonto") = dr3("inscripMonto")
                    Session("precioNetoInscripcion") = dr3("inscripMonto") * (1 + planIva / 100) 'p descontarla en comisiones
                End If
                If dr3("elplan") = "PREMIUM" And dr3("regularizacion").Equals(False) And dr3("anticipadas").Equals(False) And dr3("duracionMeses") = 0 Then 'PREMIUM
                    If elPlan.Text = "PREMIUM" Then
                        sumaDesctos = sumaDesctos + dr3("porcen")
                        desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                        Session("entroPremium") = dr3("id")
                        Session("idAplicaPromo") = dr3("id")
                        If dr3("inscripGratis").Equals(True) Then
                            Session("inscripGratis") = 1
                        End If
                        'If Not DBNull.Value.Equals(dr3("inscripMonto")) Then
                        '    Session("inscripMonto") = dr3("inscripMonto")
                        'End If
                    End If
                ElseIf dr3("elplan") = "PREMIUM" And dr3("regularizacion").Equals(False) And dr3("anticipadas").Equals(False) And dr3("duracionMeses") <> 0 Then 'PREMIUM y duracionMeses
                    If elPlan.Text = "PREMIUM" And CDbl(duracionMeses.Text.Trim) = dr3("duracionMeses") Then
                        sumaDesctos = sumaDesctos + dr3("porcen")
                        desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                        Session("entroPremium") = dr3("id")
                        Session("idAplicaPromo") = dr3("id")
                        If dr3("inscripGratis").Equals(True) Then
                            Session("inscripGratis") = 1
                        End If
                        'If Not DBNull.Value.Equals(dr3("inscripMonto")) Then
                        '    Session("inscripMonto") = dr3("inscripMonto")
                        'End If
                    End If
                ElseIf dr3("elplan") = "PREMIUM" And dr3("regularizacion").Equals(False) And dr3("anticipadas").Equals(True) And dr3("duracionMeses") <> 0 Then 'PREMIUM, duracionMeses, Anticipadas                
                    If elPlan.Text = "PREMIUM" And CDbl(mesesAnticipados.Value) = dr3("duracionMeses") Then
                        sumaDesctos = sumaDesctos + dr3("porcen")
                        desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                        Session("entroPremium") = dr3("id")
                        Session("idAplicaPromo") = dr3("id")
                        If dr3("inscripGratis").Equals(True) Then
                            Session("inscripGratis") = 1
                        End If
                        'If Not DBNull.Value.Equals(dr3("inscripMonto")) Then
                        '    Session("inscripMonto") = dr3("inscripMonto")
                        'End If
                    End If
                ElseIf dr3("elplan") = "PREMIUM" And dr3("regularizacion").Equals(True) And dr3("anticipadas").Equals(False) And dr3("duracionMeses") <> 0 Then 'PREMIUM, duracionMeses, Regularizadas
                    If elPlan.Text = "PREMIUM" And CDbl(mesesRegularizacion.Value) = dr3("duracionMeses") Then
                        sumaDesctos = sumaDesctos + dr3("porcen")
                        desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                        Session("entroPremium") = dr3("id")
                        Session("idAplicaPromo") = dr3("id")
                        If dr3("inscripGratis").Equals(True) Then
                            Session("inscripGratis") = 1
                        End If
                        'If Not DBNull.Value.Equals(dr3("inscripMonto")) Then
                        '    Session("inscripMonto") = dr3("inscripMonto")
                        'End If
                    End If
                ElseIf dr3("elplan") <> "PREMIUM" Then 'basico o ceros
                    If Not DBNull.Value.Equals(dr3("idPreRequisito")) Then
                        If requisitado(dr3("idPreRequisito")) = 1 Then
                            GoTo basicoCeros
                        Else
                            GoTo sigueCiclo
                        End If
                    End If
basicoCeros:
                    If dr3("elplan") = "BASICO" And dr3("regularizacion").Equals(False) And dr3("anticipadas").Equals(False) And dr3("nDeclContratadas") = 0 Then 'basico
                        If elPlan.Text = "BASICO" Then
                            sumaDesctos = sumaDesctos + dr3("porcen")
                            desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                            Session("idAplicaPromo") = dr3("id")
                        End If
                    ElseIf dr3("elplan") = "BASICO" And dr3("regularizacion").Equals(False) And dr3("anticipadas").Equals(False) And dr3("nDeclContratadas") <> 0 Then 'basico, nDeclContratadas
                        If elPlan.Text = "BASICO" And CDbl(nDeclContratadas.Text.Trim) >= dr3("nDeclContratadas") Then
                            sumaDesctos = sumaDesctos + dr3("porcen")
                            desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                            Session("idAplicaPromo") = dr3("id")
                        End If
                    ElseIf dr3("elplan") = "BASICO" And dr3("regularizacion").Equals(True) And dr3("anticipadas").Equals(False) And dr3("nDeclContratadas") <> 0 Then 'basico, nDeclContratadas, regularizacion
                        If elPlan.Text = "BASICO" And CDbl(nDeclContratadas.Text.Trim) >= dr3("nDeclContratadas") And esRegularizacion.Checked = True Then
                            sumaDesctos = sumaDesctos + dr3("porcen")
                            desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                            Session("idAplicaPromo") = dr3("id")
                        End If
                    ElseIf dr3("elplan") = "BASICO" And dr3("regularizacion").Equals(False) And dr3("anticipadas").Equals(True) And dr3("nDeclContratadas") <> 0 Then 'basico, nDeclContratadas, anticipadas
                        If elPlan.Text = "BASICO" And CDbl(nDeclContratadas.Text.Trim) >= dr3("nDeclContratadas") And esRegularizacion.Checked = False Then
                            sumaDesctos = sumaDesctos + dr3("porcen")
                            desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                            Session("idAplicaPromo") = dr3("id")
                        End If
                    ElseIf dr3("elplan") = "BASICO" And dr3("regularizacion").Equals(True) And dr3("anticipadas").Equals(False) And dr3("nDeclContratadas") = 0 Then 'basico, regularizacion
                        If elPlan.Text = "BASICO" And esRegularizacion.Checked = True Then
                            sumaDesctos = sumaDesctos + dr3("porcen")
                            desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                            Session("idAplicaPromo") = dr3("id")
                        End If
                    ElseIf dr3("elplan") = "BASICO" And dr3("regularizacion").Equals(False) And dr3("anticipadas").Equals(True) And dr3("nDeclContratadas") = 0 Then 'basico, anticipadas
                        If elPlan.Text = "BASICO" And esRegularizacion.Checked = False Then
                            sumaDesctos = sumaDesctos + dr3("porcen")
                            desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                            Session("idAplicaPromo") = dr3("id")
                        End If
                    End If

                    If dr3("elplan") = "CEROS" And dr3("regularizacion").Equals(False) And dr3("anticipadas").Equals(False) And dr3("nDeclContratadas") = 0 Then 'CEROS
                        If elPlan.Text = "CEROS" Then
                            sumaDesctos = sumaDesctos + dr3("porcen")
                            desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                            Session("idAplicaPromo") = dr3("id")
                        End If
                    ElseIf dr3("elplan") = "CEROS" And dr3("regularizacion").Equals(False) And dr3("anticipadas").Equals(False) And dr3("nDeclContratadas") <> 0 Then 'CEROS, nDeclContratadas
                        If elPlan.Text = "CEROS" And CDbl(nDeclContratadas.Text.Trim) >= dr3("nDeclContratadas") Then
                            sumaDesctos = sumaDesctos + dr3("porcen")
                            desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                            Session("idAplicaPromo") = dr3("id")
                        End If
                    ElseIf dr3("elplan") = "CEROS" And dr3("regularizacion").Equals(True) And dr3("anticipadas").Equals(False) And dr3("nDeclContratadas") <> 0 Then 'CEROS, nDeclContratadas, regularizacion
                        If elPlan.Text = "CEROS" And CDbl(nDeclContratadas.Text.Trim) >= dr3("nDeclContratadas") And esRegularizacion.Checked = True Then
                            sumaDesctos = sumaDesctos + dr3("porcen")
                            desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                            Session("idAplicaPromo") = dr3("id")
                        End If
                    ElseIf dr3("elplan") = "CEROS" And dr3("regularizacion").Equals(False) And dr3("anticipadas").Equals(True) And dr3("nDeclContratadas") <> 0 Then 'CEROS, nDeclContratadas, anticipadas
                        If elPlan.Text = "CEROS" And CDbl(nDeclContratadas.Text.Trim) >= dr3("nDeclContratadas") And esRegularizacion.Checked = False Then
                            sumaDesctos = sumaDesctos + dr3("porcen")
                            desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                            Session("idAplicaPromo") = dr3("id")
                        End If
                    ElseIf dr3("elplan") = "CEROS" And dr3("regularizacion").Equals(True) And dr3("anticipadas").Equals(False) And dr3("nDeclContratadas") = 0 Then 'CEROS, regularizacion
                        If elPlan.Text = "CEROS" And esRegularizacion.Checked = True Then
                            sumaDesctos = sumaDesctos + dr3("porcen")
                            desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                            Session("idAplicaPromo") = dr3("id")
                        End If
                    ElseIf dr3("elplan") = "CEROS" And dr3("regularizacion").Equals(False) And dr3("anticipadas").Equals(True) And dr3("nDeclContratadas") = 0 Then 'CEROS, anticipadas
                        If elPlan.Text = "CEROS" And esRegularizacion.Checked = False Then
                            sumaDesctos = sumaDesctos + dr3("porcen")
                            desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + dr3("porcen").ToString + "% " + dr3("cod") + "</br>"
                            Session("idAplicaPromo") = dr3("id")
                        End If
                    End If

                End If
sigueCiclo:
            End While
        End Using

        'descto.Text = sumaDesctos.ToString

CodigoRegBrindaPromos:
        If idCliente.Text <> "" Then
            'If id.Text <> "" Then 'update
            If Session("inscripGratis") = 1 Then
                q = "(SELECT id FROM contratos where idCliente=" + idCliente.Text + ")" 'nada que validar
            Else
                If id.Text <> "" Then 'edicion
                    q = "(SELECT id FROM contratos where idCliente=" + idCliente.Text + " and fechaPago is not null) union (SELECT id FROM clientes WHERE id=" + idCliente.Text + " ) union (SELECT id FROM contratos WHERE idCliente=" + idCliente.Text + " and esEl1o=1 and id<>" + id.Text + ")" 'id<>" + id.Text
                Else 'nuevo
                    q = "(SELECT id FROM contratos where idCliente=" + idCliente.Text + " and fechaPago is not null) union (SELECT id FROM clientes WHERE id=" + idCliente.Text + " ) union (SELECT id FROM contratos WHERE idCliente=" + idCliente.Text + " and esEl1o=1)" 'id<>" + id.Text
                End If
            End If

            'Else 'alta
            '    q = "SELECT id FROM contratos where idCliente=" + idCliente.Text
            'End If

            Dim pagaInscrip, cantInscrip, desctoInscrip
            myCommand = New SqlCommand(q)
            Dim v = ExecuteScalarFunction(myCommand)
            If Not IsNothing(v) Then   'solo paga mensualidad                
                Session("esEl1oVal") = "0"
                pagaInscrip = 0
                desctoInscrip = 0
                Session("precioNetoInscripcion") = 0 'no restar inscrip de la comision
            Else    'paga inscripcion
                Session("esEl1oVal") = "1"
                pagaInscrip = 1
                desctoInscrip = 0
                'Session("precioNetoInscripcion")  ya calculada
            End If
            If Session("inscripGratis") = 1 Then
                Session("esEl1oVal") = "0"
                pagaInscrip = 0
                desctoInscrip = planInscrip
                Session("precioNetoInscripcion") = 0 'no restar inscrip de la comision
            End If
            If pagaInscrip = 0 Then
                cantInscrip = 0
            Else
                If codCliente.Text <> "" Then
                    If Not DBNull.Value.Equals(Session("inscripMonto")) Then
                        cantInscrip = Session("inscripMonto")
                    Else
                        cantInscrip = planInscrip '0
                    End If
                Else
                    If Not DBNull.Value.Equals(Session("inscripMonto")) Then
                        cantInscrip = Session("inscripMonto")
                    Else
                        cantInscrip = planInscrip
                    End If
                End If
                desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + " CARGO DE INSCRIPCION AUTORIZADA (" + FormatCurrency(cantInscrip * (1 + planIva / 100), 2).ToString + ")</br>"
            End If

            If desctoInscrip <> 0 Then
                desglose.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + desglose.Text + " INSCRIPCION GRATIS (" + FormatCurrency(planInscrip * (1 + planIva / 100), 2).ToString + ")</br>"
            End If

            If elPlan.Text = "PREMIUM" Then
                'If session("esEl1oVal") = "1" Then '1er contrato
                '    If CDbl(duracionMeses.Text) >= 12 Then 'brinca inscripcion
                '        precioNetoContrato.Text = FormatCurrency(((planPrecioBaseMes * duracionMeses.Text)) + ((planPrecioBaseMes * duracionMeses.Text)) * planIva / 100, 2)
                '    Else 'paga inscrip
                '        precioNetoContrato.Text = FormatCurrency((planInscrip + (planPrecioBaseMes * duracionMeses.Text)) + (planInscrip + (planPrecioBaseMes * duracionMeses.Text)) * planIva / 100, 2)
                '    End If
                'Else
                cantInscrip = 0 'PREMIUM NO PAGA INSCRIPCION
                desctoInscrip = 0

                If nvoPrecNeto.Text = "" Then
                    precioNetoContrato.Text = FormatCurrency(((planPrecioBaseMes - (planPrecioBaseMes * sumaDesctos / 100)) * duracionMeses.Text * (1 + planIva / 100)) + cantInscrip * (1 + planIva / 100), 2)
                End If
                'End If
                desctoPesos.Text = FormatCurrency(((planPrecioBaseMes * sumaDesctos / 100) * duracionMeses.Text * (1 + planIva / 100)) + desctoInscrip * (1 + planIva / 100), 2)
                duracionMeses.Visible = True 'para basicos/ceros
                lblDurMes.Visible = True
                lblFF.Visible = True
                fechaFinal.Visible = True
                lblDeclContra.Visible = False
                nDeclContratadas.Visible = False
                nDeclHechas.Visible = False
                lblDeclHech.Visible = False

            Else
                'If session("esEl1oVal") = "1" Then
                '    precioNetoContrato.Text = FormatCurrency((planInscrip + (planPrecioBaseMes * nDeclContratadas.Text)) + (planInscrip + (planPrecioBaseMes * nDeclContratadas.Text)) * planIva / 100, 2)
                'Else
                'Response.Write("<script language='javascript'>alert('PB=" + planPrecioBaseMes.ToString + ", sumaDesctos=" + sumaDesctos.ToString + ",planIva=" + planIva.ToString + ", cantInscrip=" + cantInscrip.ToString + "');</script>")
                If nvoPrecNeto.Text = "" Then
                    precioNetoContrato.Text = FormatCurrency(((planPrecioBaseMes - (planPrecioBaseMes * sumaDesctos / 100)) * nDeclContratadas.Text * (1 + planIva / 100)) + cantInscrip * (1 + planIva / 100), 2)
                End If
                'End If
                desctoPesos.Text = FormatCurrency(((planPrecioBaseMes * sumaDesctos / 100) * nDeclContratadas.Text * (1 + planIva / 100)) + desctoInscrip * (1 + planIva / 100), 2)
                lblDurMes.Visible = False
                duracionMeses.Visible = False 'para basicos/ceros
                lblFF.Visible = False
                fechaFinal.Visible = False
                lblDeclContra.Visible = True
                nDeclContratadas.Visible = True
                lblDeclHech.Visible = True
                nDeclHechas.Visible = True
            End If
            If nvoPrecNeto.Text <> "" Then
                precioNetoContrato.Text = FormatCurrency(nvoPrecNeto.Text)
            End If
        Else
            Response.Write("<script language='javascript'>alert('Especifique 1o el cliente');</script>")
            Exit Sub
        End If

    End Sub

    Protected Sub periodoInicial_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles periodoInicial.TextChanged
        preciosFechas()
    End Sub

    Protected Sub duracionMeses_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles duracionMeses.TextChanged
        preciosFechas()
    End Sub

    Protected Sub nDeclContratadas_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nDeclContratadas.TextChanged
        preciosFechas()
    End Sub

    Private Function validaVacios() As Integer
        If elPlan.Text = "PREMIUM" And periodoInicial.Text <> "" Then
            fechaFinal.Text = DateAdd(DateInterval.Month, CDbl(duracionMeses.Text) - 1, CDate(periodoInicial.Text))
            fechaFinal.Text = ultdiames(DatePart(DateInterval.Month, CDate(fechaFinal.Text)), DatePart(DateInterval.Year, CDate(fechaFinal.Text))).ToString + Right(fechaFinal.Text, 8)
            Call distribuyeMeses()
        End If

        If Trim(cliente.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el cliente');</script>")
            Return 0
        End If
        If Trim(fechaPago.Text) = "" Then
            'MsgBox("Especifique la fecha de pago", , "")
            'fechaPago.Focus()
            'Return 0
        Else
            Dim dtnow As DateTime
            Dim regDate As New System.Text.RegularExpressions.Regex("^\d{1,2}\/\d{1,2}\/\d{4}$")
            If regDate.IsMatch(fechaPago.Text.Trim) Then
                If Not DateTime.TryParse(fechaPago.Text.Trim, dtnow) Then
                    fechaPago.Focus()
                    Response.Write("<script language='javascript'>alert('fecha de pago invalida');</script>")
                    Return 0
                End If
            Else
                fechaPago.Focus()
                Response.Write("<script language='javascript'>alert('fecha de pago formato no valido (dd/mm/aaaa)');</script>")
                Return 0
            End If
        End If
        If Trim(periodoInicial.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el periodo inicial');</script>")
            periodoInicial.Focus()
            Return 0
        Else
            Dim dtnow As DateTime
            Dim regDate As New System.Text.RegularExpressions.Regex("^\d{1,2}\/\d{1,2}\/\d{4}$")
            If regDate.IsMatch(periodoInicial.Text.Trim) Then
                If Not DateTime.TryParse(periodoInicial.Text.Trim, dtnow) Then
                    periodoInicial.Focus()
                    Response.Write("<script language='javascript'>alert('periodo inicial invalido');</script>")
                    Return 0
                End If
            Else
                periodoInicial.Focus()
                Response.Write("<script language='javascript'>alert('periodo inicial formato no valido (dd/mm/aaaa)');</script>")
                Return 0
            End If

        End If
        If elPlan.Text = "PREMIUM" Then
            If Trim(duracionMeses.Text) = "0" Or Trim(duracionMeses.Text) = "" Then
                Response.Write("<script language='javascript'>alert('Especifique la duracion en meses');</script>")
                duracionMeses.Focus()
                Return 0
            End If
        Else
            If Trim(nDeclContratadas.Text) = "0" Or Trim(nDeclContratadas.Text) = "" Then
                Response.Write("<script language='javascript'>alert('Especifique el # de declaraciones contratadas');</script>")
                nDeclContratadas.Focus()
                Return 0
            End If
        End If

        Dim fechaUltima = DateAdd(DateInterval.Day, -DatePart(DateInterval.Day, Now()) + 1, Now()) 'dia 1o del mes actual
        fechaUltima = DateAdd(DateInterval.Month, -1, fechaUltima) 'dia 1o del mes anterior

        If elPlan.Text = "PREMIUM" Then
            If esRegularizacion.Checked = True Then
                If CDate(fechaFinal.Text) >= CDate(Format(fechaUltima, "yyyy-MM-dd")) Then
                    Response.Write("<script language='javascript'>alert('La fecha final exede al dia " + Left(fechaUltima.ToString, 10) + " para una declaración de regularización de periodos anteriores, quite la marca de regularización o intente con un periodo inical mas antiguo o disminuya la cantidad de meses');</script>")
                    Return 0
                End If
                'Else
                '    If CDate(periodoInicial.Text) < CDate(Format(fechaUltima, "yyyy-MM-dd")) Then
                '        Response.Write("<script language='javascript'>alert('El periodo inicial es de una fecha anterior al dia " + Left(fechaUltima.ToString, 10) + " requiere marcar como regularización de periodos anteriores si desea manejar este periodo inicial');</script>")
                '        Return 0
                '    End If
            End If
        Else 'no premium
            If elPlan.Text = "BASICO" Or elPlan.Text = "CEROS" Then 'no incluir las anuales del 2014 en adelante
                If esRegularizacion.Checked = True Then
                    If CDate(periodoInicial.Text) >= CDate(Format(fechaUltima, "yyyy-MM-dd")) Then
                        Response.Write("<script language='javascript'>alert('El periodo inicial exede al dia " + Left(fechaUltima.ToString, 10) + " para una declaración definida como regularización de periodos anteriores, intente con un periodo inical mas antiguo');</script>")
                        Return 0
                    End If
                Else
                    If CDate(periodoInicial.Text) < CDate(Format(fechaUltima, "yyyy-MM-dd")) Then
                        Response.Write("<script language='javascript'>alert('El periodo inicial es de una fecha anterrior al dia " + Left(fechaUltima.ToString, 10) + " requiere marcar como regularización de periodos anteriores si desea manejar este periodo inicial');</script>")
                        Return 0
                    End If
                End If
            End If
        End If

        If comisionPagada.Checked = True And fechaPago.Text = "" Then
            Response.Write("<script language='javascript'>alert('Para registrar comision pagada es requisito registrar pago del cliente ');</script>")
            Return 0
        End If

        If nAdeudos.Text = "" Or IsNumeric(nAdeudos.Text) = False Then
            Response.Write("<script language='javascript'>alert('No. adeudos debe ser valor numerico');</script>")
            Return 0
        End If

        If montoAdeudos.Text = "" Or IsNumeric(montoAdeudos.Text) = False Then
            Response.Write("<script language='javascript'>alert('Monto adeudos debe ser valor numerico');</script>")
            Return 0
        End If

        If nvoPrecNeto.Text <> "" And IsNumeric(nvoPrecNeto.Text) = False Then
            Response.Write("<script language='javascript'>alert('Nuevo precio neto pactado debe ser valor numerico');</script>")
            Return 0
        End If

        If uuid.Text <> "" Then
            Dim expresion = "[a-f0-9A-F]{8}-[a-f0-9A-F]{4}-[a-f0-9A-F]{4}-[a-f0-9A-F]{4}-[a-f0-9A-F]{12}"
            If Not Regex.IsMatch(uuid.Text, expresion) Then
                Response.Write("<script language='javascript'>alert('formato uuid incorrecto');</script>")
                uuid.Focus()
                Return 0
            End If
        End If

        'If fechaPago.Text <> "" Then
        '    If esRegularizacion.Checked = False And CDate(periodoInicial.Text) < CDate(fechaPago.Text) Then
        '        Dim correo, q
        '        q = "SELECT correo FROM clientes WHERE id=" + idCliente.Text
        '        myCommand2 = New SqlCommand(q, myConnection)
        '        dr2 = myCommand2.ExecuteReader()
        '        dr2.Read()
        '        correo = dr2("correo")
        '        dr2.Close()
        '        Dim elcorreo As New System.Net.Mail.MailMessage
        '        Using elcorreo
        '            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
        '            elcorreo.To.Add(correo)
        '            elcorreo.Subject = "Modifique el periodo inicial de su contrato IDE"
        '            elcorreo.Body = "<html><body>Buen dia " + cliente.Text + ",<br><br>En la sección Mis contratos de su Cuenta, en el contrato Número " + session("GidContrato").ToString + " requiere especificar un periodo inicial >= a la fecha de pago " + fechaPago.Text + " para comenzar a declarar a partir de ese periodo<br><br>Atentamente, <a href='declaracioneside.com'>declaracioneside.com</a> </body></html>"
        '            elcorreo.IsBodyHtml = True
        '            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        '            Dim smpt As New System.Net.Mail.SmtpClient
        '            smpt.Host = "smtp.gmail.com"
        '            smpt.Port = "587"
        '            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
        '            smpt.EnableSsl = True 'req p server gmail
        '            Try
        '                smpt.Send(elcorreo)
        '                elcorreo.Dispose()
        '            Catch ex As Exception
        '                MsgBox("Error: " & ex.Message, , "")
        '            Finally
        '                MsgBox("Notificación de requerimiento de actualización de periodo inicial enviada" & vbCrLf & "El periodo inicial para comenzar a declarar en este contrato debe ser >= a la fecha de pago" & vbCrLf & "Corríjala para recibir x sistema la fecha de pago", , "")
        '            End Try
        '        End Using
        '        fechaPago.Focus()
        '        Return 0
        '    End If
        'End If


        Return 1
    End Function

    Private Function validaDuplMod() As Integer
        Dim q
        q = "SELECT id FROM contratos WHERE id<>" + id.Text + " and (idcliente='" + Trim(idCliente.Text) + "' and periodoInicial='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "' and idPlan=" + idPlan.Text + ")"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('Ya tiene un contrato con ese periodo inicial y plan, para las declaraciones complementarias el periodo inicial comienza con un dia posterior cada una, por ejemplo dia 02 para la 1er complementaria, dia 03 para la 2a, etc.');</script>")
            Return 0
        End If

        If elPlan.Text = "PREMIUM" Then
            q = "SELECT id FROM contratos WHERE id<>" + id.Text + " and idcliente='" + Trim(idCliente.Text) + "' and ((periodoInicial<='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "' and fechaFinal>='" + Format(Convert.ToDateTime(Trim(fechaFinal.Text)), "yyyy-MM-dd") + "') or (periodoInicial>='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "' and fechaFinal<='" + Format(Convert.ToDateTime(Trim(fechaFinal.Text)), "yyyy-MM-dd") + "') or (periodoInicial>='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "' and fechaFinal>='" + Format(Convert.ToDateTime(Trim(fechaFinal.Text)), "yyyy-MM-dd") + "' and periodoInicial<='" + Format(Convert.ToDateTime(Trim(fechaFinal.Text)), "yyyy-MM-dd") + "') or (periodoInicial<='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "' and fechaFinal<='" + Format(Convert.ToDateTime(Trim(fechaFinal.Text)), "yyyy-MM-dd") + "' and fechaFinal>='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "') )"
            myCommand = New SqlCommand(q)
            v = ExecuteScalarFunction(myCommand)
            If Not IsNothing(v) Then
                Response.Write("<script language='javascript'>alert('Existe traslape con el periodo del contrato " + dr("id").ToString + "');</script>")
                Return 0
            End If
        End If

        Return 1
    End Function

    Private Function validaDupl() As Integer
        Dim q
        q = "SELECT idcliente FROM contratos WHERE idcliente='" + Trim(idCliente.Text) + "' and periodoInicial='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "' and idPlan=" + idPlan.Text
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('Ya existe un contrato registrado con ese periodo inicial y plan, para las declaraciones complementarias el periodo inicial comienza con un dia posterior cada una, por ejemplo dia 02 para la 1er complementaria, dia 03 para la 2a, etc.');</script>")
            Return 0
        End If

        If elPlan.Text = "PREMIUM" Then
            q = "SELECT id FROM contratos WHERE idcliente='" + Trim(idCliente.Text) + "' and ((periodoInicial<='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "' and fechaFinal>='" + Format(Convert.ToDateTime(Trim(fechaFinal.Text)), "yyyy-MM-dd") + "') or (periodoInicial>='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "' and fechaFinal<='" + Format(Convert.ToDateTime(Trim(fechaFinal.Text)), "yyyy-MM-dd") + "') or (periodoInicial>='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "' and fechaFinal>='" + Format(Convert.ToDateTime(Trim(fechaFinal.Text)), "yyyy-MM-dd") + "' and periodoInicial<='" + Format(Convert.ToDateTime(Trim(fechaFinal.Text)), "yyyy-MM-dd") + "') or (periodoInicial<='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "' and fechaFinal<='" + Format(Convert.ToDateTime(Trim(fechaFinal.Text)), "yyyy-MM-dd") + "' and fechaFinal>='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "') )"
            myCommand = New SqlCommand(q)
            v = ExecuteScalarFunction(myCommand)
            If Not IsNothing(v) Then
                Response.Write("<script language='javascript'>alert('Existe traslape con el periodo del contrato " + v.ToString + "');</script>")
                Return 0
            End If
        End If

        Return 1
    End Function


    Private Sub instruccionesDePago(addEdit)
        If sinCorr.Checked.Equals(True) Then
            Exit Sub
        End If
        Dim pcioSinIva = FormatCurrency(precioNetoContrato.Text / 1.16, 2).ToString
        Dim elcorreo As New System.Net.Mail.MailMessage
        Using elcorreo
            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
            elcorreo.To.Add(Session("curCorreo"))
            elcorreo.CC.Add("declaracioneside@gmail.com")
            elcorreo.Subject = addEdit & " contrato Núm. " + id.Text + ", " + Session("curCorreo") + " : Instrucciones de Pago"
            elcorreo.Body = "<html><body>Hola " + cliente.Text.Trim.ToUpper + " ( id " + idCliente.Text + " )<br><br> Puede realizar pagos en línea en nuestra página si dispone de tarjeta de crédito, de debito o de una cuenta paypal, ingresando a su cuenta->mis contratos->seleccionando el contrato a pagar. <br><br> Para transferencias interbancarias: Banamex, CLABE 002470701248748996, referencia o concepto C" + Session("GidContrato").ToString + ", RFC COPJ7809196S2 <br><br>Para depósitos bancarios: Banamex, #Cuenta 7012000004874899, referencia o concepto C" + Session("GidContrato").ToString + " <br><br>Para depósitos en oxxo o 7eleven: Banamex, #tarjeta 5204167339542094, referencia o concepto C" + Session("GidContrato").ToString + " estas instituciones cobran un cargo adicional que aquí no está contemplado <br><br> Todos los pagos son a nombre de JOB JOSUE CONSTANTINO PRADO, Recuerde especificar la referencia o concepto para identificar su pago. <br><br> Para mas detalles, consulte su contrato #" + Session("GidContrato").ToString + ". Producto: " + elPlan.SelectedItem.Text + ", Cantidad: " + nDeclContratadas.Text + ", MesesPremium: " + duracionMeses.Text + "<br><br>El monto neto a pagar por este contrato es " + precioNetoContrato.Text.ToString + " (Pcio sin IVA=" + pcioSinIva + ")<br><br>Si opta por transferencia o depósito, envíenos escaneado o foto del comprobante de pago en formato pdf a declaracioneside@gmail.com<br><br>Dispone de 15 dias para realizar este pago a partir de hoy para que se le respete este monto, ya que pasando ese periodo es posible que el precio al dia sea mayor al presente.<br><br>Favor de responder este correo indicando Metodo de pago, Uso del CFDI, y si es Pago en una sola exhibición PUE o en parcialidades para la generación de su factura. <br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet<br>Tel 4436903616, 4432180237<br>Correo declaracioneside@gmail.com<br><a href='https://twitter.com/declaracionesid' target='_blank'><img src='declaracioneside.com/twitter.jpg' alt='Clic aquí, siguenos en twitter' Height='30px' Width='30px' BorderWidth='0px'></a>&nbsp;<a href='http://www.youtube.com/user/declaracioneside' target='_blank'><img src='declaracioneside.com/iconoyoutube.png' alt='Suscribete a nuestro canal declaraciones de depósitos en efectivo e IDE en youtube' Height='30px' Width='30px' BorderWidth='0px'></a> &nbsp;<a href='http://www.facebook.com/depositosenefectivo' target='_blank'><img src='declaracioneside.com/facebook.jpg' alt='Clic aquí para seguirnos en facebook' Height='30px' Width='30px' BorderWidth='0px'></a>&nbsp;&nbsp;<a href='https://mx.linkedin.com/in/declaraciones-depósitos-en-efectivo-1110125b' target='_blank'><img src='declaracioneside.com/linkedin.png' alt='Siguenos en linkedin' Height='30px' Width='30px' BorderWidth='0px'></a>&nbsp;<a href='http://plus.google.com/107594546767340388428?prsrc=3'><img src='http://ssl.gstatic.com/images/icons/gplus-32.png' alt='Google+' Height='30px' Width='30px' BorderWidth='0px'></a><br/></body></html>"
            elcorreo.IsBodyHtml = True
            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
            Dim smpt As New System.Net.Mail.SmtpClient
            smpt.Host = "smtp.gmail.com"
            smpt.Port = "587"
            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
            smpt.EnableSsl = True 'req p server gmail
            Try
                smpt.Send(elcorreo)
                elcorreo.Dispose()
            Catch ex As Exception

                Response.Write("<script language='javascript'>alert('Error enviando instrucciones de pago: " & ex.Message + "');</script>")
                Exit Sub
            Finally
                Response.Write("<script language='javascript'>alert('Le han sido enviadas a su correo las instrucciones de pago');</script>")
            End Try
        End Using

    End Sub

    Private Sub distribuyeMeses()
        Dim temp = DateAdd(DateInterval.Day, -DatePart(DateInterval.Day, CDate(periodoInicial.Text)) + 1, CDate(periodoInicial.Text)) 'dia 1o del periodo inicial
        Dim actual = DateAdd(DateInterval.Day, -DatePart(DateInterval.Day, Now()) + 1, Now()) 'dia 1o del mes actual
        actual = DateAdd(DateInterval.Month, -1, actual) 'dia 1o del mes anterior
        Dim final = DateAdd(DateInterval.Day, -DatePart(DateInterval.Day, CDate(fechaFinal.Text)) + 1, CDate(fechaFinal.Text)) 'dia 1o de fecha final
        mesesRegularizacion.Value = 0
        mesesAnticipados.Value = 0
        While CDate(Format(temp, "yyyy-MM-dd")) <= CDate(Format(final, "yyyy-MM-dd"))
            If CDate(Format(temp, "yyyy-MM-dd")) < CDate(Format(actual, "yyyy-MM-dd")) Then
                mesesRegularizacion.Value = mesesRegularizacion.Value + 1
            Else
                mesesAnticipados.Value = mesesAnticipados.Value + 1
            End If
            temp = DateAdd(DateInterval.Month, 1, temp)
        End While
    End Sub

    Protected Sub addEdit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles addEdit.Click
        If acepto.Checked = False Then
            acepto.Focus()
            Response.Write("<script language='javascript'>alert('Si no acepta los términos del servicio, las políticas de uso, aviso de privacidad y características del contrato, no puede establecer contratos');</script>")
            Exit Sub
        End If

        If validaVacios() < 1 Then
            Exit Sub
        End If
        preciosFechas()

        If esRegularizacion.Checked = True Then
            Session("esRegularizacionVal") = "1"
        Else
            Session("esRegularizacionVal") = "0"
        End If

        Dim comisionPagadaVal, parcialidadesVal, vencidoVal
        If comisionPagada.Checked = True Then
            comisionPagadaVal = "1"
        Else
            comisionPagadaVal = "0"
        End If
        If parcialidades.Checked = True Then
            parcialidadesVal = "1"
        Else
            parcialidadesVal = "0"
        End If
        If vencido.Checked = True Then
            vencidoVal = "1"
        Else
            vencidoVal = "0"
        End If

        Dim factTxVal
        If enviada.Checked = True Then
            factTxVal = "1"
        Else
            factTxVal = "0"
        End If

        Dim postpagoVal
        If chkPostpago.Checked = True Then
            postpagoVal = "1"
        Else
            postpagoVal = "0"
        End If

        Dim deCortesiaVal
        If deCortesia.Checked = True Then
            deCortesiaVal = "1"
        Else
            deCortesiaVal = "0"
        End If

        Dim elNuevoPrecio = ""
        If nvoPrecNeto.Text <> "" Then
            elNuevoPrecio = ",nvoPrecNeto='" + Trim(Replace(nvoPrecNeto.Text, ",", "")) + "'"
        End If


        If id.Text <> "" And id.Text <> "-1" Then 'editar
            If validaDuplMod() < 1 Then
                Exit Sub
            End If

            Dim q, bkComisionPagada, bkPrecioNetoContrato
            q = "SELECT comisionPagada,precioNetoContrato FROM contratos WHERE id=" + id.Text
            myCommand = New SqlCommand(q)
            Using dr = ExecuteReaderFunction(myCommand)
                dr.Read()
                bkComisionPagada = dr("comisionPagada")
                bkPrecioNetoContrato = dr("precioNetoContrato")
            End Using

            q = "SELECT id FROM contratos WHERE id<>" + id.Text + " AND idcliente='" + Trim(idCliente.Text) + "' AND periodoinicial='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "' AND idplan='" + Trim(idPlan.Text) + "'"
            myCommand = New SqlCommand(q)
            Dim v = ExecuteScalarFunction(myCommand)
            If Not IsNothing(v) Then
                Response.Write("<script language='javascript'>alert('Ya existe un contrato con ese periodo inicial y plan, verifique o elimine alguno');</script>")
                Exit Sub
            End If

            If elPlan.Text = "PREMIUM" Then
                Dim anualEnPremiumV
                If CInt(duracionMeses.Text) >= 12 Then
                    anualEnPremiumV = ", anualEnPremium=1"
                Else
                    anualEnPremiumV = ""
                End If
                Call distribuyeMeses()
                'la fechaPago se actualiza en boton aparte
                q = "UPDATE contratos SET idcliente='" + Trim(idCliente.Text) + "',periodoinicial='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "', duracionMeses='" + Trim(Replace(duracionMeses.Text, ",", "")) + "', idplan='" + Trim(idPlan.Text) + "', precionetocontrato='" + Trim(Replace(precioNetoContrato.Text, ",", "")) + "', fechaFinal='" + Format(Convert.ToDateTime(Trim(fechaFinal.Text)), "yyyy-MM-dd") + "', esEl1o=" + Session("esEl1oVal") + ", esRegularizacion=" + Session("esRegularizacionVal") + anualEnPremiumV + ", comisionPagada=" + comisionPagadaVal + ", factTx=" + factTxVal + ", postpago=" + postpagoVal + ", mesesRegularizacion=" + mesesRegularizacion.Value.ToString + ", mesesAnticipados=" + mesesAnticipados.Value.ToString + ", parcialidades=" + parcialidadesVal + ", vencido=" + vencidoVal + ", nAdeudos=" + nAdeudos.Text + ", montoAdeudos='" + Trim(Replace(montoAdeudos.Text, ",", "")) + "', deCortesia=" + deCortesiaVal + elNuevoPrecio + " WHERE id=" + id.Text
            Else
                q = "UPDATE contratos SET idcliente='" + Trim(idCliente.Text) + "',periodoinicial='" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "', idplan='" + Trim(idPlan.Text) + "', precionetocontrato='" + Trim(Replace(precioNetoContrato.Text, ",", "")) + "', nDeclContratadas='" + Trim(Replace(nDeclContratadas.Text, ",", "")) + "', ndeclHechas='" + Trim(Replace(nDeclHechas.Text, ",", "")) + "', esEl1o=" + Session("esEl1oVal") + ", esRegularizacion=" + Session("esRegularizacionVal") + ", comisionPagada=" + comisionPagadaVal + ", factTx=" + factTxVal + ", postpago=" + postpagoVal + ", parcialidades=" + parcialidadesVal + ", vencido=" + vencidoVal + ", nAdeudos=" + nAdeudos.Text + ", montoAdeudos='" + Trim(Replace(montoAdeudos.Text, ",", "")) + "', deCortesia=" + deCortesiaVal + elNuevoPrecio + " WHERE id=" + id.Text
            End If
            myCommand = New SqlCommand(q)
            ExecuteNonQueryFunction(myCommand)

            myCommand = New SqlCommand("DELETE FROM desctosContra WHERE idContra=" + id.Text)
            ExecuteNonQueryFunction(myCommand)

            'desctos vigentes
            If Session("idAplicaPromo") <> 0 Then
                myCommand = New SqlCommand("INSERT INTO desctosContra(idDescto,idContra) VALUES(" + Session("idAplicaPromo").ToString + "," + id.Text + ")")
                ExecuteNonQueryFunction(myCommand)
            End If

            If esRegularizacion.Checked = True And elPlan.Text <> "PREMIUM" Then
                Dim idDesctoV
                q = "SELECT TOP 1 id FROM desctos WHERE tipo='REG' AND convert(datetime,convert(int,GETDATE())) <= fechaCaducidad ORDER BY id DESC"
                myCommand = New SqlCommand(q)
                v = ExecuteScalarFunction(myCommand)
                If Not IsNothing(v) Then
                    idDesctoV = v
                    myCommand = New SqlCommand("INSERT INTO desctosContra(idDescto,idContra) VALUES(" + idDesctoV.ToString + "," + id.Text + ")")
                    ExecuteNonQueryFunction(myCommand)
                End If
            End If

            If codCliente.Text.Trim <> "" Then
                Dim idDesctoV
                q = "SELECT top 1 id FROM desctos WHERE tipo='REF' and cod='" + codCliente.Text.Trim + "' AND convert(datetime,convert(int,GETDATE())) <= fechaCaducidad order by id desc"
                myCommand = New SqlCommand(q)
                v = ExecuteScalarFunction(myCommand)
                If Not IsNothing(v) Then
                    idDesctoV = v
                    myCommand = New SqlCommand("INSERT INTO desctosContra(idDescto,idContra) VALUES(" + idDesctoV.ToString + "," + id.Text + ")")
                    ExecuteNonQueryFunction(myCommand)
                End If

            End If

            If FormatCurrency(bkPrecioNetoContrato.ToString) <> FormatCurrency(precioNetoContrato.Text.Trim) Then
                q = "UPDATE contratos SET fecha='" + Format(Now(), "yyyy-MM-dd") + "' WHERE id=" + id.Text
                myCommand = New SqlCommand(q)
                ExecuteNonQueryFunction(myCommand)

                instruccionesDePago("Edición de")
            End If

            If comisionPagada.Checked = True And bkComisionPagada.Equals(False) Then
                notificaPagoDistribuidor()
            End If
            Response.Write("<script language='javascript'>alert('Actualizado correctamente');</script>")
        Else    'add
            If validaDupl() < 1 Then
                Exit Sub
            End If

            Try
                periodoInicial.Text = "01/" & Mid(periodoInicial.Text, 4)
                Dim q

                If elPlan.Text = "PREMIUM" Then
                    Dim anualEnPremiumV
                    If CInt(duracionMeses.Text) >= 12 Then
                        anualEnPremiumV = "1"
                    Else
                        anualEnPremiumV = "0"
                    End If
                    Call distribuyeMeses()
                    If nvoPrecNeto.Text <> "" Then
                        q = "INSERT INTO contratos(idcliente,periodoinicial,duracionmeses,idplan,precionetocontrato,fechafinal,esEl1o,anualEnPremium,esRegularizacion,comisionPagada,fecha,factTx,mesesRegularizacion,mesesAnticipados,postpago,deCortesia,nvoPrecNeto) VALUES('" + Trim(idCliente.Text) + "','" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "','" + Replace(Trim(duracionMeses.Text), ",", "") + "','" + Trim(idPlan.Text) + "','" + Replace(Trim(precioNetoContrato.Text), ",", "") + "','" + Format(Convert.ToDateTime(Trim(fechaFinal.Text)), "yyyy-MM-dd") + "'," + Session("esEl1oVal") + "," + anualEnPremiumV + "," + Session("esRegularizacionVal") + "," + comisionPagadaVal + ",'" + Format(Now(), "yyyy-MM-dd") + "', " + factTxVal + "," + mesesRegularizacion.Value.ToString + "," + mesesAnticipados.Value.ToString + "," + postpagoVal + "," + deCortesiaVal + ",'" + Trim(Replace(nvoPrecNeto.Text, ",", "")) + "')"
                    Else
                        q = "INSERT INTO contratos(idcliente,periodoinicial,duracionmeses,idplan,precionetocontrato,fechafinal,esEl1o,anualEnPremium,esRegularizacion,comisionPagada,fecha,factTx,mesesRegularizacion,mesesAnticipados,postpago,deCortesia) VALUES('" + Trim(idCliente.Text) + "','" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "','" + Replace(Trim(duracionMeses.Text), ",", "") + "','" + Trim(idPlan.Text) + "','" + Replace(Trim(precioNetoContrato.Text), ",", "") + "','" + Format(Convert.ToDateTime(Trim(fechaFinal.Text)), "yyyy-MM-dd") + "'," + Session("esEl1oVal") + "," + anualEnPremiumV + "," + Session("esRegularizacionVal") + "," + comisionPagadaVal + ",'" + Format(Now(), "yyyy-MM-dd") + "', " + factTxVal + "," + mesesRegularizacion.Value.ToString + "," + mesesAnticipados.Value.ToString + "," + postpagoVal + "," + deCortesiaVal + ")"
                    End If

                Else
                    If nvoPrecNeto.Text <> "" Then
                        q = "INSERT INTO contratos(idcliente,periodoinicial,idplan,precionetocontrato,ndeclcontratadas,ndeclhechas,esEl1o,esRegularizacion,comisionPagada,fecha,factTx,postpago,deCortesia,nvoPrecNeto) VALUES('" + Trim(idCliente.Text) + "','" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "','" + Trim(idPlan.Text) + "','" + Replace(Trim(precioNetoContrato.Text), ",", "") + "','" + Replace(Trim(nDeclContratadas.Text), ",", "") + "','" + Trim(Replace(nDeclHechas.Text, ",", "")) + "'," + Session("esEl1oVal") + "," + Session("esRegularizacionVal") + "," + comisionPagadaVal + ",'" + Format(Now(), "yyyy-MM-dd") + "', " + factTxVal + "," + postpagoVal + "," + deCortesiaVal + ",'" + Trim(Replace(nvoPrecNeto.Text, ",", "")) + "')"
                    Else
                        q = "INSERT INTO contratos(idcliente,periodoinicial,idplan,precionetocontrato,ndeclcontratadas,ndeclhechas,esEl1o,esRegularizacion,comisionPagada,fecha,factTx,postpago,deCortesia) VALUES('" + Trim(idCliente.Text) + "','" + Format(Convert.ToDateTime(Trim(periodoInicial.Text)), "yyyy-MM-dd") + "','" + Trim(idPlan.Text) + "','" + Replace(Trim(precioNetoContrato.Text), ",", "") + "','" + Replace(Trim(nDeclContratadas.Text), ",", "") + "','" + Trim(Replace(nDeclHechas.Text, ",", "")) + "'," + Session("esEl1oVal") + "," + Session("esRegularizacionVal") + "," + comisionPagadaVal + ",'" + Format(Now(), "yyyy-MM-dd") + "', " + factTxVal + "," + postpagoVal + "," + deCortesiaVal + ")"
                    End If

                End If
                myCommand = New SqlCommand(q)
                ExecuteNonQueryFunction(myCommand)

                q = "SELECT id FROM contratos WHERE idCliente=" + Session("GidCliente").ToString + " and periodoInicial='" + Format(Convert.ToDateTime(periodoInicial.Text.Trim), "yyyy-MM-dd") + "' and idplan=" + idPlan.Text
                myCommand = New SqlCommand(q)
                Dim v = ExecuteScalarFunction(myCommand)
                id.Text = v
                Session("GidContrato") = id.Text

                If Session("GidCliente") = "" Or Session("GidCliente") = Nothing Then
                    del.Visible = True
                    actPago.Visible = True
                End If

                'desctos vigentes
                If Session("idAplicaPromo") <> 0 Then
                    myCommand = New SqlCommand("INSERT INTO desctosContra(idDescto,idContra) VALUES(" + Session("idAplicaPromo").ToString + "," + id.Text + ")")
                    ExecuteNonQueryFunction(myCommand)
                End If

                If esRegularizacion.Checked = True And elPlan.Text <> "PREMIUM" Then
                    q = "SELECT TOP 1 id FROM desctos WHERE tipo='REG' AND convert(datetime,convert(int,GETDATE())) <= fechaCaducidad ORDER BY id DESC"
                    myCommand = New SqlCommand(q)
                    v = ExecuteScalarFunction(myCommand)
                    If Not IsNothing(v) Then
                        Dim idDesctoV = v
                        myCommand = New SqlCommand("INSERT INTO desctosContra(idDescto,idContra) VALUES(" + idDesctoV.ToString + "," + id.Text + ")")
                        ExecuteNonQueryFunction(myCommand)
                    End If
                End If

                If codCliente.Text.Trim <> "" Then
                    q = "SELECT TOP 1 id FROM desctos WHERE tipo='REF' and cod='" + codCliente.Text.Trim + "' order by id desc" ' la caducidad se validó en preciosFechas
                    'q = "SELECT TOP 1 id FROM desctos WHERE tipo='REF' and cod='" + codCliente.Text.Trim + "' AND convert(datetime,convert(int,GETDATE())) <= fechaCaducidad order by id desc"
                    myCommand = New SqlCommand(q)
                    v = ExecuteScalarFunction(myCommand)
                    If Not IsNothing(v) Then
                        Dim idDesctoV = v
                        myCommand = New SqlCommand("INSERT INTO desctosContra(idDescto,idContra) VALUES(" + idDesctoV.ToString + "," + id.Text + ")")
                        ExecuteNonQueryFunction(myCommand)
                    End If
                    dr.Close()
                End If

                instruccionesDePago("Nuevo")

                If comisionPagada.Checked = True Then
                    notificaPagoDistribuidor()
                End If
                Response.Write("<script language='javascript'>alert('Contrato Num. " + id.Text + " creado');</script>")
                If Session("GidCliente") = Nothing Or Session("GidCliente") = "" Then
                    Response.Write("<script>location.href = 'contrato.aspx?id=-1';</script>")
                End If

            Catch ex As Exception
                Response.Write("<script>" + ex.Message + "</script>")
            End Try

        End If
    End Sub

    Private Sub notificaPagoDistribuidor()
        Dim q, numDistribuidor
        q = "SELECT idDistribuidor FROM clientes WHERE id=" + idCliente.Text
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        numDistribuidor = v

        q = "select * from distribuidores where id=" + numDistribuidor.ToString
        myCommand = New SqlCommand(q)
        Dim comisPorcen
        Dim nombreFiscal
        Dim correo
        Using dr = ExecuteReaderFunction(myCommand)
            dr.Read()
            nombreFiscal = dr("nombreFiscal")
            correo = dr("correo")

            If dr("clisForzosos").Equals(True) Then
                comisPorcen = 15
            Else
                comisPorcen = dr("comisPorcen")
            End If
        End Using


        q = "select * from iva where porcen in (select ivaPorcen from actuales)"
        myCommand = New SqlCommand(q)
        Dim precioBaseContrato
        Dim comision
        Using dr = ExecuteReaderFunction(myCommand)
            dr.Read()
            precioBaseContrato = precioNetoContrato.Text / (1 + dr("porcen") / 100)
            comision = FormatCurrency(precioBaseContrato * comisPorcen / 100, 0) 'sobre precioBase Nivel 1 de distribuidor
        End Using


        Dim elcorreo As New System.Net.Mail.MailMessage
        Using elcorreo
            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
            elcorreo.To.Add(correo)
            elcorreo.Subject = "Ha recibido un pago de comisión en su cuenta bancaria"
            elcorreo.Body = "<html><body>Hola " + nombreFiscal + "<br><br>Comisión pagada correspondiente al Contrato #" + Session("GidContrato").ToString + " de " + cliente.Text + "<br>Comisión $ = " + comision.ToString + "<br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
            elcorreo.IsBodyHtml = True
            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
            Dim smpt As New System.Net.Mail.SmtpClient
            smpt.Host = "smtp.gmail.com"
            smpt.Port = "587"
            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
            smpt.EnableSsl = True 'req p server gmail
            Try
                smpt.Send(elcorreo)
                elcorreo.Dispose()
            Catch ex As Exception
                Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
                Exit Sub
            Finally
                Response.Write("<script language='javascript'>alert('Notificación de comisión pagada enviada');</script>")
            End Try
        End Using
    End Sub

    Protected Sub atras_Click(ByVal sender As Object, ByVal e As EventArgs) Handles atras.Click
        Response.Redirect("~/contratos.aspx")
    End Sub

    Protected Sub del_Click(ByVal sender As Object, ByVal e As EventArgs) Handles del.Click
        Dim q

        'borro cascadas
        myCommand = New SqlCommand("DELETE FROM desctosContra WHERE idContra=" + id.Text)
        ExecuteNonQueryFunction(myCommand)

        q = "DELETE FROM contratos WHERE id=" + Trim(id.Text)
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)

        Response.Write("<script language='javascript'>alert('Se ha eliminado');</script>")
        Response.Write("<script>location.href = 'miscontra.aspx';</script>")

    End Sub

    Protected Sub misContra_Click(ByVal sender As Object, ByVal e As EventArgs) Handles misContra.Click
        Response.Redirect("~/misContra.aspx")
    End Sub

    Protected Sub esRegularizacion_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles esRegularizacion.CheckedChanged

    End Sub

    Private Sub refactPedido_ValueChanged(sender As Object, e As EventArgs) Handles refactPedido.ValueChanged
        Timbrar()
        refactPedido.Value = ""
    End Sub

    Private Sub timbrarFactura()
        If uuid.Text <> "" Then
            Dim MSG = "<script language='javascript'>alert('ya se facturo con el uud " + uuid.Text + ", limpia el uuid, reFactura y cancela esta anotando el uuid en txt');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            'Dim MSG = "<script language='javascript'>pregunta('" + uuid.Text + "','" + id.Text + "');</script>"
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            'Dim MSG = "<script language='javascript'>callConfirm('1');</script>"
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Exit Sub
        Else
            Timbrar()
        End If
    End Sub

    Private Sub Timbrar()
        If nvoPrecNeto.Text <> "" Then
            precioNetoContrato.Text = FormatCurrency(nvoPrecNeto.Text)
        End If
        Dim facTerceroVal, facRfcVal, facRazonVal, facUsoVal, facFPVal, facCorreosVal, facRetensVal
        facCorreosVal = ""
        Dim q = "select facTercero, facRfc, facRazon, facUso, facFP, razonSoc, rfcDeclarante, facCorreos, facRetens from clientes where correo='" + Session("curCorreo") + "'"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            dr.Read()
            facTerceroVal = 0
            facRfcVal = SecurityElement.Escape(dr("rfcDeclarante"))
            facRazonVal = SecurityElement.Escape(dr("razonSoc"))
            facUsoVal = "G03"
            facFPVal = pagoRealizado.SelectedValue
            If Not DBNull.Value.Equals(dr("facCorreos")) Then
                facCorreosVal = "," + dr("facCorreos")
            Else
                facCorreosVal = ""
            End If
            facRetensVal = dr("facRetens")
            If Not DBNull.Value.Equals(dr("facTercero")) Then
                If dr("facTercero").Equals(True) Then
                    facTerceroVal = 1
                    facRfcVal = SecurityElement.Escape(dr("facRfc"))
                    facRazonVal = SecurityElement.Escape(dr("facRazon"))
                    facUsoVal = dr("facUso")
                End If
            End If
        End Using

        Dim fecha = Date.Now.ToString("yyyy/MM/dd HH:mm:ss")
        Dim total = CDbl(precioNetoContrato.Text)
        Dim subtotal
        Dim ivaF
        Dim ivaR
        Dim isrR
        Dim totR
        Dim totIVA

        If chkSubtotal.Checked.Equals(False) Then
            If redondear.Checked.Equals(False) Then
                subtotal = TruncateDecimal(total / 1.16, 2)
                ivaF = TruncateDecimal(subtotal * 0.16, 2)
                ivaR = TruncateDecimal(subtotal * 0.11, 2)
                isrR = TruncateDecimal(subtotal * 0.1, 2)
                totR = CDbl(TruncateDecimal(CDbl(isrR) + CDbl(ivaR), 2)).ToString("###############0.00")
                If facRetensVal.Equals(True) Then
                    total = TruncateDecimal(subtotal + ivaF - ivaR - isrR, 2)
                Else
                    total = TruncateDecimal(subtotal + ivaF, 2)
                End If
                totIVA = TruncateDecimal(ivaF + ivaR, 2).ToString("###############0.00")
            Else ' redondear            
                subtotal = FormatNumber(CDbl(total / 1.16), 2)
                ivaF = FormatNumber(CDbl(subtotal * 0.16), 2)
                ivaR = FormatNumber(CDbl(subtotal * 0.11), 2)
                isrR = FormatNumber(CDbl(subtotal * 0.1), 2)
                totR = FormatNumber(CDbl(CDbl(isrR) + CDbl(ivaR)), 2)
                If facRetensVal.Equals(True) Then
                    total = FormatNumber(CDbl(subtotal) + CDbl(ivaF) - CDbl(ivaR) - CDbl(isrR), 2)
                Else
                    total = FormatNumber(CDbl(subtotal) + CDbl(ivaF), 2)
                End If
                totIVA = FormatNumber(CDbl(ivaF) + CDbl(ivaR), 2)
            End If
        Else
            'subtotal editado
            subtotalTxt.Text = subtotalTxt.Text.Replace("$", "").Replace(",", "")
            If redondear.Checked.Equals(False) Then
                subtotal = TruncateDecimal(subtotalTxt.Text, 2)
                ivaF = TruncateDecimal(subtotal * 0.16, 2)
                ivaR = TruncateDecimal(subtotal * 0.11, 2)
                isrR = TruncateDecimal(subtotal * 0.1, 2)
                totR = CDbl(TruncateDecimal(CDbl(isrR) + CDbl(ivaR), 2)).ToString("###############0.00")
                If facRetensVal.Equals(True) Then
                    total = TruncateDecimal(subtotal + ivaF - ivaR - isrR, 2)
                Else
                    total = TruncateDecimal(subtotal + ivaF, 2)
                End If
                totIVA = TruncateDecimal(ivaF + ivaR, 2).ToString("###############0.00")
            Else ' redondear            
                subtotal = FormatNumber(CDbl(subtotalTxt.Text), 2)
                ivaF = FormatNumber(CDbl(subtotal * 0.16), 2)
                ivaR = FormatNumber(CDbl(subtotal * 0.11), 2)
                isrR = FormatNumber(CDbl(subtotal * 0.1), 2)
                totR = FormatNumber(CDbl(CDbl(isrR) + CDbl(ivaR)), 2)
                If facRetensVal.Equals(True) Then
                    total = FormatNumber(CDbl(subtotal) + CDbl(ivaF) - CDbl(ivaR) - CDbl(isrR), 2)
                Else
                    total = FormatNumber(CDbl(subtotal) + CDbl(ivaF), 2)
                End If
                totIVA = FormatNumber(CDbl(ivaF) + CDbl(ivaR), 2)
            End If
        End If

        total = CDbl(Val(total)).ToString("###############0.00")
        subtotal = CDbl(subtotal).ToString("###############0.00") 'casteo a cadena
        ivaR = CDbl(ivaR).ToString("###############0.00") 'casteo a cadena
        isrR = CDbl(isrR).ToString("###############0.00") 'casteo a cadena
        ivaF = CDbl(ivaF).ToString("###############0.00") 'casteo a cadena

        If facRetensVal.Equals(True) Then
            calc.Text = "subtotal=" + subtotal.ToString + ", ivaTras=" + ivaF.ToString + ", ivaRet=" + ivaR.ToString + ", isrRet=" + isrR.ToString + ", Resultado=" + total.ToString
        Else
            calc.Text = "subtotal=" + subtotal.ToString + ", ivaTras=" + ivaF.ToString + ", Resultado=" + total.ToString
        End If

        Dim concepto
        If elPlan.SelectedItem.Text = "PREMIUM" Then
            concepto = "Paquete de " + duracionMeses.Text + " meses PREMIUM de Delaraciones de depósitos en efectivo, apartir del " + periodoInicial.Text + ": Plataforma web para presentación y envío por el usuario."
        ElseIf elPlan.SelectedItem.Text = "CEROS" Or elPlan.SelectedItem.Text = "BASICO" Then
            concepto = "Paquete de " + nDeclContratadas.Text + " Delaraciones de depósitos en efectivo en plan " + elPlan.SelectedItem.Text + ": Plataforma web para presentación y envío por el usuario."
        ElseIf elPlan.SelectedItem.Text = "ANUAL DESDE 2014 CEROS NORMAL" Then
            concepto = "Paquete de " + nDeclContratadas.Text + " Delaraciones Anuales de depósitos en efectivo en Ceros: Plataforma web para presentación y envío por el usuario."
        ElseIf elPlan.SelectedItem.Text = "ANUAL DESDE 2014 CEROS COMPLEMENTARIA" Then
            concepto = "Paquete de " + nDeclContratadas.Text + " Delaraciones Anuales de depósitos en efectivo en Ceros Complementaria: Plataforma web para presentación y envío por el usuario."
        ElseIf elPlan.SelectedItem.Text = "ANUAL DESDE 2014 DATOS NORMAL" Then
            concepto = "Paquete de " + nDeclContratadas.Text + " Delaraciones Anuales de depósitos en efectivo con Datos: Plataforma web para presentación y envío por el usuario."
        ElseIf elPlan.SelectedItem.Text = "ANUAL DESDE 2014 DATOS COMPLEMENTARIA" Then
            concepto = "Paquete de " + nDeclContratadas.Text + " Delaraciones Anuales de depósitos en efectivo con Datos Complementaria: Plataforma web para presentación y envío por el usuario."
        Else
            concepto = "Paquete de " + nDeclContratadas.Text + " Delaraciones de depósitos en efectivo en plan " + elPlan.SelectedItem.Text + ": Plataforma web para presentación y envío por el usuario."
        End If

        Dim idSerie = "636"
        Dim folio = id.Text
        Dim serie = "C"
        Dim cadenaFACT
        cadenaFACT = "DOCUMENTO|Factura|ENVIO|" + facRfcVal + "|" + Session("curCorreo") + ",declaracioneside@gmail.com" + facCorreosVal + "|COMPROBANTE|3.3|" + serie + "|" + folio + "|" + fecha + "|" + facFPVal + "|" + subtotal.ToString + "||MXN||" + total.ToString + "|I|PUE|58230|||EMISOR|COPJ7809196S2|JOB JOSUE CONSTANTINO PRADO|612|RECEPTOR|" + facRfcVal + "|" + facRazonVal + "|||" + facUsoVal + "|CONCEPTO|81112106||1|E48|SERVICIO|" + concepto + "|" + subtotal.ToString + "|" + subtotal.ToString + "||"
        If facRetensVal = 0 Then 'iva 16%
            cadenaFACT = cadenaFACT + "C_IMP_TRASLADADO|" + subtotal.ToString + "|002|Tasa|0.160000|" + ivaF.ToString + "|IMPUESTOSTOTALES|" + ivaF.ToString + "||IMPUESTOSCOMPROBANTE|IMP_TRASLADADO|002|" + ivaF.ToString + "|0.160000|Tasa|ADDENDA|ARFINSA|OBSERVACIONES|OBSERVACION|C" + id.Text + "|"
        Else 'ret iva(002), isr(001)
            cadenaFACT = cadenaFACT + "C_IMP_TRASLADADO|" + subtotal.ToString + "|002|Tasa|0.160000|" + ivaF.ToString + "|C_IMP_RETENIDO|" + subtotal.ToString + "|002|Tasa|0.110000|" + ivaR.ToString + "|C_IMP_RETENIDO|" + subtotal.ToString + "|001|Tasa|0.10000|" + isrR.ToString + "|IMPUESTOSTOTALES|" + ivaF.ToString + "|" + totR.ToString + "|IMPUESTOSCOMPROBANTE|IMP_TRASLADADO|002|" + totIVA.ToString + "|0.160000|Tasa|IMP_RETENIDO|002|" + ivaR.ToString + "|0.110000|Tasa|IMP_RETENIDO|001|" + isrR.ToString + "|0.100000|Tasa|ADDENDA|ARFINSA|OBSERVACIONES|OBSERVACION|C" + id.Text + "|"
        End If

        'Se procede a consumir el WS
        Dim Proceso = "811110"
        Dim pass = "jobjosue"
        '-----------Pruebas Factura----------
        'Dim wpruebas As New pruebasWsTimbradoTexto33.wsTimbradoTexto33
        'Dim vpruebas As New pruebasWsTimbradoTexto33.resultado33
        'Dim wTpruebas As New pruebasWsTimbradoTexto33.wsTimbradoTexto33

        '-----------Produccion Factura----------
        Dim v As New prodWsTimbradoTexto33.resultado33
        Dim wT As New prodWsTimbradoTexto33.wsTimbradoTexto33
        Try
            v = wT.TimbrarPorTexto33("JOB001@HOTMAIL.COM", pass, Proceso, cadenaFACT)
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('Excepcion al facturar:  " + ex.Message + "');</script>")
        End Try
        If v.errores <> "" Then
            Response.Write("<script language='javascript'>alert('Error al facturar: " & v.errores.ToString + "');</script>")
        Else
            myCommand = New SqlCommand("UPDATE contratos SET uuid='" + v.folioUUID + "' WHERE id=" + id.Text)
            ExecuteNonQueryFunction(myCommand)
            uuid.Text = v.folioUUID
            Response.Write("<script language='javascript'>alert('Factura timbrada y enviada, uuid " + v.folioUUID + "');</script>")
        End If
    End Sub
    Protected Sub actPago_Click(ByVal sender As Object, ByVal e As EventArgs) Handles actPago.Click
        If id.Text = "" Then
            Response.Write("<script language='javascript'>alert('Guarde 1o el contrato');</script>")
            Exit Sub
        End If

        Call preciosFechas() 'p vars d comisiones

        If fechaPago.Text.Trim <> "" Then
            myCommand = New SqlCommand("UPDATE contratos SET fechaPago='" + Format(Convert.ToDateTime(fechaPago.Text.Trim), "yyyy-MM-dd") + "' WHERE id=" + id.Text)
            ExecuteNonQueryFunction(myCommand)

            Dim fechaUltima = DateAdd(DateInterval.Day, -DatePart(DateInterval.Day, Now()) + 1, Now()) 'dia 1o del mes actual
            fechaUltima = DateAdd(DateInterval.Month, -1, fechaUltima) 'dia 1o del mes anterior
            If CDate(periodoInicial.Text) < CDate(Format(fechaUltima, "yyyy-MM-dd")) Then 'convirtiendolo autom en esRegularizacion si ya se pasó
                myCommand = New SqlCommand("UPDATE contratos SET esRegularizacion=1 WHERE id=" + id.Text)
                ExecuteNonQueryFunction(myCommand)
            End If

            'Al pagar un contrato ya cubre inscripcion
            'myCommand = New SqlCommand("UPDATE clientes SET inscripcionPagada=1  WHERE id=" + idCliente.Text, myConnection)
            'myCommand.ExecuteNonQuery()
            'INSCRIPCION GRATIS, factura de inscrip auto-enviada (p no preoc de eso)
            'Dim q
            'q = "SELECT id FROM desctosContra WHERE idContra=" + id.Text + " AND idDescto IN (SELECT id FROM desctos WHERE inscripGratis=1)"
            'myCommand = New SqlCommand(q, myConnection)
            'dr = myCommand.ExecuteReader()
            'If dr.Read() Then
            '    myCommand = New SqlCommand("UPDATE clientes SET inscripcionPagada=1  WHERE id=" + idCliente.Text, myConnection)
            '    myCommand.ExecuteNonQuery()
            'End If
            'dr.Close()


            Dim correo, numDistribuidor, q
            q = "SELECT correo,idDistribuidor FROM clientes WHERE id=" + idCliente.Text
            myCommand = New SqlCommand(q)
            Using dr = ExecuteReaderFunction(myCommand)
                dr.Read()
                correo = dr("correo")
                numDistribuidor = dr("idDistribuidor")
            End Using

            'notificacion de pago al cliente
            Dim elcorreo As New System.Net.Mail.MailMessage
            Using elcorreo
                elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                elcorreo.To.Add(correo)
                elcorreo.Subject = "Pago confirmado"
                elcorreo.Body = "<html><body>Buen dia " + cliente.Text + ",<br><br>Si ya fué previamente notificado sobre la liberación de su socket/canal, ahora puede usar su plan<br><br>En la sección Mis contratos de su Cuenta, podrá ver los detalles correspondientes al contrato Número " + Session("GidContrato").ToString + ", ingrese a la sección 'Declarar' para enviar sus declaraciones de IDE y consulte la ayuda ahi disponible que le guiará paso a paso, en la sección 'Mis declaraciones' se muestra un resumen de sus declaraciones <br><br>Si aún no le han notificado la asignación de su canal/socket, espere a ello para de inmediato poder declarar <br><br>Atentamente, <a href='declaracioneside.com'>declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet </body></html>"
                elcorreo.IsBodyHtml = True
                elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                Dim smpt As New System.Net.Mail.SmtpClient
                smpt.Host = "smtp.gmail.com"
                smpt.Port = "587"
                smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
                smpt.EnableSsl = True 'req p server gmail
                Try
                    smpt.Send(elcorreo)
                    elcorreo.Dispose()
                Catch ex As Exception
                    Response.Write("<script language='javascript'>alert('Error en Notificación de pago enviada al cliente: " & ex.Message + "');</script>")
                    Exit Sub
                Finally
                    Response.Write("<script language='javascript'>alert('Notificación de pago enviada al cliente');</script>")
                End Try
            End Using

            'TIMBRAR FACTURA AQUI
            If yFac.Checked.Equals(True) Then
                Call timbrarFactura()
            End If

            Dim precioBaseContrato
            q = "select * from iva where porcen in (select ivaPorcen from actuales)"
            myCommand = New SqlCommand(q)
            Using dr = ExecuteReaderFunction(myCommand)
                dr.Read()
                precioBaseContrato = precioNetoContrato.Text / (1 + dr("porcen") / 100)
            End Using


            q = "select * from distribuidores where id=" + numDistribuidor.ToString
            myCommand = New SqlCommand(q)

            Using dr = ExecuteReaderFunction(myCommand)
                dr.Read()
                Dim nombreFiscal = dr("nombreFiscal")
                Dim banco = dr("banco")
                Dim clabe = dr("clabe")
                Dim correoDistribuidor = dr("correo")
                Dim comisPorcen
                If dr("clisForzosos").Equals(True) Then
                    comisPorcen = 15
                Else
                    comisPorcen = dr("comisPorcen")
                End If
                Dim comisCaduca = dr("comisCaduca")
                Dim comisMesesCaducidad = dr("comisMesesCaducidad")
                Dim comision = FormatCurrency((precioNetoContrato.Text - Session("precioNetoInscripcion")) * comisPorcen / 100, 0) 'sobre precioNeto desp de impuestos, sobre contratos que no incluye inscripcion

                If dr("nombreFiscal") <> "DEFAULT" Then 'distribuidores
                    If dr("doctos").Equals(True) Then 'solo pagar a los autorizados
                        Dim activo
                        If dr("clisForzosos").Equals(True) Then


                            q = "select esEl1o from contratos where id=" + id.Text
                            myCommand = New SqlCommand(q)
                            Dim v = ExecuteScalarFunction(myCommand)
                            Dim esEl1o = v

                            If esEl1o.Equals(False) Then
                                q = "select id from clientes where idDistribuidor=" + numDistribuidor.ToString + " and YEAR(fechaRegistro)='" + DatePart(DateInterval.Year, Convert.ToDateTime(fechaPago.Text.Trim)).ToString + "' and MONTH(fechaRegistro)='" + DatePart(DateInterval.Month, Convert.ToDateTime(fechaPago.Text.Trim)).ToString + "'"
                                myCommand = New SqlCommand(q)
                                v = ExecuteScalarFunction(myCommand)
                                If Not IsNothing(v) Then
                                    activo = 1
                                Else
                                    activo = 0
                                End If
                            Else
                                activo = 1
                            End If
                        Else 'distribuidores comision negociada en base a % y caducidad sin oblig de ingreso cte de clis
                            If comisCaduca.Equals(False) Then 'indefinida
                                activo = 1
                            Else 'caduca

                                q = "select esEl1o from contratos where id=" + id.Text
                                myCommand = New SqlCommand(q)
                                Dim v = ExecuteScalarFunction(myCommand)
                                Dim esEl1o = v

                                If esEl1o.Equals(False) Then
                                    q = "select fechaRegistro from clientes where id=" + idCliente.Text.Trim
                                    myCommand = New SqlCommand(q)
                                    v = ExecuteScalarFunction(myCommand)
                                    Dim fechaRegistro = v
                                    Dim fechaCaducidad = DateAdd(DateInterval.Month, comisMesesCaducidad, fechaRegistro) 'n meses tras registro
                                    If Format(Convert.ToDateTime(Now()), "dd-MM-yyyy") >= fechaCaducidad Then
                                        activo = 0
                                    Else
                                        activo = 1
                                    End If
                                Else
                                    activo = 1
                                End If
                            End If
                        End If

                        If activo = 1 Then
                            'recordatorio para pagarles comisiones
                            Dim elcorreo2 As New System.Net.Mail.MailMessage
                            Using elcorreo2
                                elcorreo2.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                                elcorreo2.To.Add("declaracioneside@gmail.com")
                                elcorreo2.Subject = "Comisiones al distribuidor #" + numDistribuidor.ToString
                                elcorreo2.Body = "<html><body>NombreFiscal = " + nombreFiscal + "<br>Banco = " + banco + "<br>Clabe interbancaria = " + clabe + "<br>Contrato #" + Session("GidContrato").ToString + "<br>Comisión $ = " + comision.ToString + " <br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
                                elcorreo2.IsBodyHtml = True
                                elcorreo2.Priority = System.Net.Mail.MailPriority.Normal
                                Dim smpt As New System.Net.Mail.SmtpClient
                                smpt.Host = "smtp.gmail.com"
                                smpt.Port = "587"
                                smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
                                smpt.EnableSsl = True 'req p server gmail
                                Try
                                    smpt.Send(elcorreo2)
                                    elcorreo2.Dispose()
                                Catch ex As Exception
                                    Response.Write("<script language='javascript'>alert('Error en Notificación de recordatorio de pago de comisiones: " & ex.Message + "');</script>")
                                    Exit Sub
                                Finally
                                    Response.Write("<script language='javascript'>alert('Notificación de recordatorio de pago de comisiones');</script>")
                                End Try
                            End Using

                            Dim elcorreo3 As New System.Net.Mail.MailMessage
                            Using elcorreo3
                                elcorreo3.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                                elcorreo3.To.Add(correoDistribuidor)
                                elcorreo3.Subject = "Favor de enviarnos su factura/recibo de honorarios para poder pagarle su comisión"
                                elcorreo3.Body = "<html><body>Hola " + nombreFiscal + "<br>Favor de enviarnos su factura/recibo de honorarios (electrónica, impresa o escaneada) con las siguientes especificaciones: <br><br><br><br>A nombre de: Job Josué Constantino Prado<br>RFC: COPJ7809196S2 <br>Domicilio: Lacas de Uruapan 737 Col. Vasco de Quiroga C.P. 58230 Morelia, Mich. <br><br>Monto total (impuestos incluidos) $ = " + comision.ToString + "<br><br> Concepto: Comisión por renta de servicio informático logrado con el contrato # " + Session("GidContrato").ToString + "<br><br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
                                elcorreo3.IsBodyHtml = True
                                elcorreo3.Priority = System.Net.Mail.MailPriority.Normal
                                Dim smpt As New System.Net.Mail.SmtpClient
                                smpt.Host = "smtp.gmail.com"
                                smpt.Port = "587"
                                smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
                                smpt.EnableSsl = True 'req p server gmail
                                Try
                                    smpt.Send(elcorreo3)
                                    elcorreo3.Dispose()
                                Catch ex As Exception
                                    Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
                                    Exit Sub
                                Finally
                                    Response.Write("<script language='javascript'>alert('Solicitud de facturacion enviada al distribuidor');</script>")
                                End Try
                            End Using

                        End If
                    End If
                End If
            End Using

        Else ' borrar fechaPago
            myCommand = New SqlCommand("UPDATE contratos SET fechaPago=NULL WHERE id=" + id.Text)
            ExecuteNonQueryFunction(myCommand)
        End If
        myCommand = New SqlCommand("UPDATE contratos SET pagoRealizado='" + pagoRealizado.SelectedItem.Value + "' WHERE id=" + id.Text)
        ExecuteNonQueryFunction(myCommand)
        Response.Write("<script language='javascript'>alert('Actualizado');</script>")
    End Sub

    Protected Sub pagos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles pagos.Click
        Dim hp
        If id.Text = "" Then
            hp = "0"
        Else
            cambios = 0
            hp = "1"
            Dim q = "select * from contratos where id=" + id.Text
            myCommand = New SqlCommand(q)
            Using dr = ExecuteReaderFunction(myCommand)
                dr.Read()
                If (dr("esRegularizacion").Equals(True) And esRegularizacion.Checked = False) Then
                    cambios = 1
                End If
                If (dr("esRegularizacion").Equals(False) And esRegularizacion.Checked = True) Then
                    cambios = 1
                End If
                If dr("periodoInicial") <> periodoInicial.Text.Trim Or dr("duracionMeses") <> duracionMeses.Text.Trim Or dr("idPlan") <> idPlan.Text.Trim Or dr("nDeclContratadas") <> nDeclContratadas.Text.Trim Then
                    cambios = 1
                End If
            End Using


            q = "select cod from desctos where tipo='REF' and id in (select idDescto from desctosContra where idContra=" + id.Text + ")"
            myCommand = New SqlCommand(q)
            Dim v = ExecuteScalarFunction(myCommand)
            If Not IsNothing(v) Then
                If (codCliente.Text.Trim.ToUpper <> v) Then
                    cambios = 1
                End If
            Else
                If (codCliente.Text.Trim.ToUpper <> "") Then
                    cambios = 1
                End If
            End If
            If cambios = 1 Then
                Response.Write("<script>alert('Ha realizado cambios, guardelos primero, o salga y regrese al contrato sin hacer cambios para acceder a pagos');</script>")
                Exit Sub
            End If
        End If
        If fechaPago.Text <> "" Then
            hp = "0"
        End If
        If hp = "1" Then
            Session("GidContrato") = Session("GidContrato").ToString
            Session("GidCliente") = Session("GidCliente").ToString
            'Session("referencia") = referencia(Session("GidCliente").ToString)
            Session("GmontoContra") = precioNetoContrato.Text
        Else
            Session("GidContrato") = ""
            Session("referencia") = ""
            Session("GidCliente") = ""
            Session("GmontoContra") = ""
        End If
        Response.Write("<script>location.href='pagos.aspx?hp=" + hp + "';</script>")
    End Sub

    Sub AddFileSecurity(ByVal fileName As String, ByVal account As String,
    ByVal rights As FileSystemRights, ByVal controlType As AccessControlType)

        Dim fSecurity As FileSecurity = File.GetAccessControl(fileName)
        Dim accessRule As FileSystemAccessRule =
        New FileSystemAccessRule(account, rights, controlType)
        fSecurity.AddAccessRule(accessRule)
        File.SetAccessControl(fileName, fSecurity)

    End Sub

    Private Function IsFileOpen(filePath As String) As Boolean
        Dim rtnvalue As Boolean = False
        Try
            Dim fs As System.IO.FileStream = System.IO.File.OpenWrite(filePath)
            fs.Close()
        Catch ex As System.IO.IOException
            rtnvalue = True
        End Try
        Return rtnvalue
    End Function

    Private Function referencia(ByVal nCliente) As String
        Try
            If Not IsFileOpen("C:\SAT\referencias.xlsx") Then
                'AddFileSecurity("C:\SAT\referencias.xlsx", Session("identidad"), FileSystemRights.FullControl, AccessControlType.Allow)
                Dim excel As Application = New Application
                Dim w As Workbook = excel.Workbooks.Open("C:\SAT\referencias.xlsx",, True)
                Dim sheet As Worksheet = w.Sheets(1)
                Dim r As Range = sheet.UsedRange
                Dim array(,) As Object = r.Value(XlRangeValueDataType.xlRangeValueDefault)
                w.Close(False)   'cierro excel y trabajo con la var
                If array IsNot Nothing Then
                    Return array(nCliente, 3).ToString.Trim()
                End If
            End If

        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('" + ex.Message + "');</script>")
        End Try
        Return ""
    End Function

    Protected Sub cotizar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cotizar.Click
        If validaVacios() < 1 Then
            Exit Sub
        End If
        preciosFechas()
    End Sub

    Protected Sub fechaPago_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles fechaPago.TextChanged

    End Sub

    Protected Sub comisionPagada_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles comisionPagada.CheckedChanged

    End Sub

    Protected Sub pagosInsuficiente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles pagosInsuficiente.Click

        If piDiferencia.Text.Trim = "" Then
            Response.Write("<script language='javascript'>alert('Introduzca la diferencia resultante a notificar por pago insuficiente');</script>")
            piDiferencia.Focus()
            Exit Sub
        End If

        Dim elcorreo As New System.Net.Mail.MailMessage
        Using elcorreo
            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
            elcorreo.To.Add(Session("curCorreo"))
            elcorreo.Bcc.Add("declaracioneside@gmail.com")
            elcorreo.Subject = "Pago require actualización"
            elcorreo.Body = "<html><body>Hola " + cliente.Text.Trim.ToUpper + "<br><br>Se validó su pago del contrato #" + Session("GidContrato").ToString + ", pero al momento de recibirlo el cotizador arroja un precio neto distinto con una diferencia de " + FormatCurrency(piDiferencia.Text.Trim, 2) + " que requiere pagar, se da por caducado este contrato y le solicitamos realizar uno nuevo que lo reemplace y al guardarlo recibirá nuevas instrucciones de pago y deberá pagar únicamente la diferencia mencionada para cubrir por completo el nuevo contrato y notificarnos su nuevo pago e indicarnos que le acreditemos el pago del contrato anterior #" + Session("GidContrato").ToString + " <br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet <br><br></body></html>"
            elcorreo.IsBodyHtml = True
            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
            Dim smpt As New System.Net.Mail.SmtpClient
            smpt.Host = "smtp.gmail.com"
            smpt.Port = "587"
            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
            smpt.EnableSsl = True 'req p server gmail
            Try
                smpt.Send(elcorreo)
                elcorreo.Dispose()
            Catch ex As Exception
                Response.Write("<script language='javascript'>alert('Error enviando instrucciones de pago: " & ex.Message + "');</script>")
                Exit Sub
            Finally
                Response.Write("<script language='javascript'>alert('Le han sido enviadas a su correo las instrucciones de pago');</script>")
            End Try
        End Using

    End Sub

    Protected Sub Sugerencias_Click(sender As Object, e As EventArgs) Handles Sugerencias.Click
        Response.Write("<script language='javascript'>alert('Para promociones: primero haga los contratos premium y después de pagarlos haga contratos básicos o ceros para recibir los beneficios derivados de la previa contratación de planes premium \n Para promociones en planes premium, aplica el # de meses exacto ahí especificado \n Las promociones en plan basico o ceros aplican sobre un mínimo de # de declaraciones \n ');</script>")
    End Sub

    Protected Sub actNdeclsHechas_Click(sender As Object, e As EventArgs) Handles actNdeclsHechas.Click
        If ndeclhechascaptura.Text.Trim = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el valor del #declHechas');</script>")
        Else
            myCommand = New SqlCommand("UPDATE contratos SET nDeclHechas=" + nDeclHechasCaptura.Text.Trim + " where id=" + id.Text)
            ExecuteNonQueryFunction(myCommand)

            nDeclHechas.Text = nDeclHechasCaptura.Text.Trim
            Dim MSG As String = "<script language='javascript'>alert('Actualizado');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)

        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles instruccPago.Click
        If id.Text <> "" Then
            preciosFechas()
            If validaCambios() = 0 Then
                Response.Write("<script language='javascript'>alert('Se detectaron cambios, 1o guarde el contrato');</script>")
            Else
                instruccionesDePago("Notificación de ")
            End If
        Else
            Response.Write("<script language='javascript'>alert('1o guarde un contrato');</script>")
        End If
    End Sub

    Private Function validaCambios() As Integer
        Dim q = "select precioNetoContrato from contratos where id=" + id.Text
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If FormatCurrency(v, 2) <> FormatCurrency(precioNetoContrato.Text, 2) Then
            Return 0
        End If

        Return 1
    End Function

    Protected Sub factTx_Click(sender As Object, e As EventArgs) Handles factTx.Click
        'If fechaPago.Text = "" Then
        '    Response.Write("<script language='javascript'>alert('Para facturar es requisito registrar pago del cliente ');</script>")
        '    Exit Sub
        'End If

        myCommand = New SqlCommand("UPDATE contratos SET factTx=1 WHERE id=" + id.Text)
        ExecuteNonQueryFunction(myCommand)
        myCommand = New SqlCommand("UPDATE clientes SET factTx=1 WHERE id=" + idCliente.Text)
        ExecuteNonQueryFunction(myCommand)
        enviada.Checked = True
        Response.Write("<script language='javascript'>alert('actualizado');</script>")
    End Sub

    Protected Sub cliente_TextChanged(sender As Object, e As EventArgs) Handles cliente.TextChanged
        cliente.Text = Server.HtmlEncode(cliente.Text)
    End Sub

    Protected Sub nvoPrecNeto_TextChanged(sender As Object, e As EventArgs) Handles nvoPrecNeto.TextChanged

    End Sub

    Private Sub soloFac_Click(sender As Object, e As EventArgs) Handles soloFac.Click
        Call timbrarFactura()
    End Sub


    Protected Sub redondear_CheckedChanged(sender As Object, e As EventArgs) Handles redondear.CheckedChanged
        aRedondear()
    End Sub

    Private Sub aRedondear()
        Dim total = CDbl(precioNetoContrato.Text)
        Dim subtotal
        Dim ivaF
        Dim ivaR
        Dim isrR
        Dim totR
        Dim totIVA

        Dim q = "select facRetens from clientes where correo='" + Session("curCorreo") + "'"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        Dim facRetensVal = v

        If chkSubtotal.Checked.Equals(False) Then
            If redondear.Checked.Equals(False) Then
                subtotal = TruncateDecimal(total / 1.16, 2)
                ivaF = TruncateDecimal(subtotal * 0.16, 2)
                ivaR = TruncateDecimal(subtotal * 0.11, 2)
                isrR = TruncateDecimal(subtotal * 0.1, 2)
                totR = CDbl(TruncateDecimal(CDbl(isrR) + CDbl(ivaR), 2)).ToString("###############0.00")
                If facRetensVal.Equals(True) Then
                    total = TruncateDecimal(subtotal + ivaF - ivaR - isrR, 2)
                Else
                    total = TruncateDecimal(subtotal + ivaF, 2)
                End If
                totIVA = TruncateDecimal(ivaF + ivaR, 2).ToString("###############0.00")
            Else ' redondear            
                subtotal = FormatNumber(CDbl(total / 1.16), 2)
                ivaF = FormatNumber(CDbl(subtotal * 0.16), 2)
                ivaR = FormatNumber(CDbl(subtotal * 0.11), 2)
                isrR = FormatNumber(CDbl(subtotal * 0.1), 2)
                totR = FormatNumber(CDbl(CDbl(isrR) + CDbl(ivaR)), 2)
                If facRetensVal.Equals(True) Then
                    total = FormatNumber(CDbl(subtotal) + CDbl(ivaF) - CDbl(ivaR) - CDbl(isrR), 2)
                Else
                    total = FormatNumber(CDbl(subtotal) + CDbl(ivaF), 2)
                End If
                totIVA = FormatNumber(CDbl(ivaF) + CDbl(ivaR), 2)
            End If
        Else
            'subtotal editado
            subtotalTxt.Text = subtotalTxt.Text.Replace("$", "").Replace(",", "")
            If redondear.Checked.Equals(False) Then
                subtotal = TruncateDecimal(subtotalTxt.Text, 2)
                ivaF = TruncateDecimal(subtotal * 0.16, 2)
                ivaR = TruncateDecimal(subtotal * 0.11, 2)
                isrR = TruncateDecimal(subtotal * 0.1, 2)
                totR = CDbl(TruncateDecimal(CDbl(isrR) + CDbl(ivaR), 2)).ToString("###############0.00")
                If facRetensVal.Equals(True) Then
                    total = TruncateDecimal(subtotal + ivaF - ivaR - isrR, 2)
                Else
                    total = TruncateDecimal(subtotal + ivaF, 2)
                End If
                totIVA = TruncateDecimal(ivaF + ivaR, 2).ToString("###############0.00")
            Else ' redondear            
                subtotal = FormatNumber(CDbl(subtotalTxt.Text), 2)
                ivaF = FormatNumber(CDbl(subtotal * 0.16), 2)
                ivaR = FormatNumber(CDbl(subtotal * 0.11), 2)
                isrR = FormatNumber(CDbl(subtotal * 0.1), 2)
                totR = FormatNumber(CDbl(CDbl(isrR) + CDbl(ivaR)), 2)
                If facRetensVal.Equals(True) Then
                    total = FormatNumber(CDbl(subtotal) + CDbl(ivaF) - CDbl(ivaR) - CDbl(isrR), 2)
                Else
                    total = FormatNumber(CDbl(subtotal) + CDbl(ivaF), 2)
                End If
                totIVA = FormatNumber(CDbl(ivaF) + CDbl(ivaR), 2)
            End If
        End If

        total = CDbl(Val(total)).ToString("###############0.00")
        subtotal = CDbl(subtotal).ToString("###############0.00") 'casteo a cadena
        ivaR = CDbl(ivaR).ToString("###############0.00") 'casteo a cadena
        isrR = CDbl(isrR).ToString("###############0.00") 'casteo a cadena
        ivaF = CDbl(ivaF).ToString("###############0.00") 'casteo a cadena

        If facRetensVal.Equals(True) Then
            calc.Text = "subtotal=" + subtotal.ToString + ", ivaTras=" + ivaF.ToString + ", ivaRet=" + ivaR.ToString + ", isrRet=" + isrR.ToString + ", Resultado=" + total.ToString
        Else
            calc.Text = "subtotal=" + subtotal.ToString + ", ivaTras=" + ivaF.ToString + ", Resultado=" + total.ToString
        End If

    End Sub

    Protected Sub chkSubtotal_CheckedChanged(sender As Object, e As EventArgs) Handles chkSubtotal.CheckedChanged
        subTotalClic()
    End Sub
    Private Sub subTotalClic()
        If chkSubtotal.Checked.Equals(True) Then
            subtotalTxt.Visible = True
            If subtotalTxt.Text = "" Or subtotalTxt.Text = "0" Then
                If redondear.Checked.Equals(False) Then
                    subtotalTxt.Text = TruncateDecimal(CDbl(precioNetoContrato.Text) / 1.16, 2)
                Else
                    subtotalTxt.Text = FormatNumber(CDbl(precioNetoContrato.Text) / 1.16, 2)
                End If

            End If
        Else
            subtotalTxt.Visible = False
        End If
        aRedondear()
    End Sub

    Protected Sub subtotalTxt_TextChanged(sender As Object, e As EventArgs) Handles subtotalTxt.TextChanged
        subTotalClic()
    End Sub
End Class