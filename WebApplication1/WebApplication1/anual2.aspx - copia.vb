Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Office.Interop.Excel
Imports Microsoft.Office.Interop.Word
Imports System
Imports System.IO
Imports System.Text
Imports Ionic.Zip
Imports System.Runtime.InteropServices
Imports System.Security
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Xml
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Security.AccessControl
Imports System.Xml.Schema
Imports FastReport
Imports FastReport.Web
Imports System.Threading

Public Class anual2
    Inherits System.Web.UI.Page

    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim myCommand2 As SqlCommand
    Dim myCommand3 As SqlCommand
    Dim myCommand4 As SqlCommand
    Dim myCommand5 As SqlCommand
    Dim dr As SqlDataReader
    Dim ejercicio
    Dim comple
    Dim savePath
    Dim nomArchMens
    Dim nomArchMensSinPath
    Dim ContNomArchMens
    Dim idContrato
    Dim pl


    Private Sub controlaAcceso()
        Dim idcli
        Dim q
        q = "SELECT id, solSocketEstatus, loginSAT FROM clientes WHERE correo='" + Session("curCorreo") + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()

        If chkPostpago.Checked.Equals(False) Then
            'If dr("inscripcionPagada").Equals(False) Then
            '    dr.Close()
            '    redir.Text = "1"
            '    Response.Write("<script language='javascript'>alert('Es necesario que cubra el pago de su inscripción');</script>")
            '    Response.Write("<script>location.href='cliente.aspx';</script>")
            '    Exit Sub
            'End If
        End If

        idcli = dr("id")

        If dr("solSocketEstatus").Equals("VACIA") Then
            dr.Close()
            redir.Text = "1"
            Response.Write("<script language='javascript'>alert('Es necesario que vaya a su cuenta y suba el archivo de autorización de socket');</script>")
            Response.Write("<script>location.href='cliente.aspx';</script>")
            Exit Sub
        ElseIf dr("solSocketEstatus") <> "APROBADA" Then
            dr.Close()
            redir.Text = "1"
            Response.Write("<script language='javascript'>alert('Estamos en espera de que el SAT nos asigne su matriz de conexión segura y su socket, para poder acceder a esta sección');</script>")
            Response.Write("<script>location.href='cliente.aspx';</script>")
            Exit Sub
        End If

        If chkPostpago.Checked.Equals(False) Then
            q = "SELECT count(*) as cuenta FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND co.fechaPago IS NOT NULL and ((pla.elplan<>'PREMIUM' and nDeclHechas<nDeclContratadas) or (pla.elplan='PREMIUM' and '" + Now.ToString("yyyy-MM-dd") + "' >= periodoInicial and '" + Now.ToString("yyyy-MM-dd") + "' <= fechaFinal))"
        Else
            q = "SELECT count(*) as cuenta FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND postpago IS NOT NULL and postpago=1 and ((pla.elplan<>'PREMIUM' and nDeclHechas<nDeclContratadas) or (pla.elplan='PREMIUM' and '" + Now.ToString("yyyy-MM-dd") + "' >= periodoInicial and '" + Now.ToString("yyyy-MM-dd") + "' <= fechaFinal))"
        End If
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        If (dr("cuenta").Equals(0)) Then 'sin contratos pagados 
            dr.Close()
            redir.Text = "1"
            Response.Write("<script language='javascript'>alert('No hay contratos pagados con declaraciones disponibles');</script>")
            Response.Write("<script>location.href='misContra.aspx';</script>")
            Exit Sub
        End If
        dr.Close()
    End Sub

    Sub AddFileSecurity(ByVal fileName As String, ByVal account As String,
            ByVal rights As FileSystemRights, ByVal controlType As AccessControlType)

        Dim fSecurity As FileSecurity = File.GetAccessControl(fileName)
        Dim accessRule As FileSystemAccessRule = New FileSystemAccessRule(account, rights, controlType)
        fSecurity.AddAccessRule(accessRule)
        File.SetAccessControl(fileName, fSecurity)
    End Sub

    Private Sub habilitacionTotales(ByVal valor)
        impteExcedente.Enabled = valor
        impteCheque.Enabled = valor
    End Sub

    Private Sub muestraComple()
        fechaPresentacionAnt.Visible = True
        lblFechaPresentacionAnt.Visible = True
        numOperAnt.Visible = True
        lblNumOperAnt.Visible = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("curCorreo")) = True Then
            Response.Write("<script language='javascript'>alert('Expiró su sesión');</script>")
            Session.Abandon()
            Response.Write("<script language='javascript'>location.href='Login.aspx';</script>")
            Exit Sub
        End If

        If Not IsPostBack Then  '1a vez    
            myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';MultipleActiveResultSets=True") 'sql timeout segundos, 36hrs ;connection timeout=129600
            myConnection.Open()
            myCommand = New SqlCommand("set dateformat ymd", myConnection)
            myCommand.ExecuteNonQuery()


            ejercicio = Request.QueryString("ejercicio")
            idContrato = Request.QueryString("contra")
            pl = Request.QueryString("pl")
            Dim q

            q = "SELECT postpago FROM contratos WHERE id=" + idContrato.ToString
            myCommand = New SqlCommand(q, myConnection)
            dr = myCommand.ExecuteReader()
            dr.Read()
            If (dr("postpago").Equals(True)) Then '
                chkPostpago.Checked = True
            Else
                chkPostpago.Checked = False
            End If
            dr.Close()

            If Request.QueryString("subop") = "1" Then  'xml
                nOpers.Visible = False
                impteExcedente.Visible = False
                impteCheque.Visible = False
            Else
                nOpers.Visible = True
                impteExcedente.Visible = True
                impteCheque.Visible = True
            End If

            enviarDeclaracionExcel.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(enviarDeclaracionExcel, "") + ";")
            btnMod.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(btnMod, "") + ";")
            btnContingencia.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(btnContingencia, "") + ";")
            'importMensXls.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(importMensXls, "") + ";")
            importarXml.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(importarXml, "") + ";")
            bajarAcuseExcel.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(bajarAcuseExcel, "") + ";")
            bajaAcuseXml.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(bajaAcuseXml, "") + ";")


            If Left(Request.ServerVariables("REMOTE_ADDR"), 10) = "192.168.0." Or Left(Request.ServerVariables("REMOTE_ADDR"), 10) = "127.0.0.1" Or Session("runAsAdmin") = "1" Then 'red local
                chkPostpago.Visible = True
                chkSinCorreo.Visible = True
                acuSinCorr.Visible = True
                'fenvio.Visible = True
                'resul.Visible = True
                'lost.Visible = True
            Else
                chkPostpago.Visible = False
                chkSinCorreo.Visible = False
                acuSinCorr.Visible = False
                fenvio.Visible = False
                resul.Visible = False
                lost.Visible = False
            End If
            comple = Request.QueryString("comple")
            Dim ncV = Request.QueryString("nc")
            Dim Vcomple
            If comple = "1" Then
                Vcomple = ", Complementaria nueva"    'creando complem
                normalComplementaria.Text = "COMPLEMENTARIA"
            Else
                Vcomple = ""
                If ncV = "C" Then
                    Vcomple = ", Complementaria"
                    normalComplementaria.Text = "COMPLEMENTARIA"
                Else
                    Vcomple = ", Normal"
                    normalComplementaria.Text = "NORMAL"
                End If
            End If
            encab.Text = "Declaración Anual: Ejercicio " + ejercicio + Vcomple

            Select Case Request.QueryString("op")
                Case "0" 'crear/editar
                    If Request.QueryString("subop") = "0" Then  'xls
                        MultiView1.ActiveViewIndex = Int32.Parse(0)
                        If Request.QueryString("comple") = "1" Then 'crea comple
                            Call muestraComple()
                        ElseIf ncV = "C" Then   'edit comple
                            Call muestraComple()
                        End If
                    ElseIf Request.QueryString("subop") = "1" Then  'xml
                        '[mod].Visible = False
                        MultiView1.ActiveViewIndex = Int32.Parse(1)
                    Else 'edit
                        MultiView1.ActiveViewIndex = Int32.Parse(2)
                    End If
                    If pl = "CEROS" Then 'edit
                        habilitacionTotales(False)
                        btnMod.Visible = False
                    End If
                    enviarDeclaracionExcel.Visible = True
                    btnContingencia.Visible = True
                Case "1" 'ceros 'creación
                    MultiView1.ActiveViewIndex = Int32.Parse(3)
                    habilitacionTotales(False)
                    Call limpiaAño()
                    btnMod.Visible = False
                    enviarDeclaracionExcel.Visible = True
                    btnContingencia.Visible = True
                Case "2" 'consultar
                    If Request.QueryString("subop") = "0" Then  'xls
                        MultiView1.ActiveViewIndex = Int32.Parse(4)
                    Else 'xml
                        MultiView1.ActiveViewIndex = Int32.Parse(5)
                    End If

                    back.Visible = False
                    btnMod.Visible = False
                    If pl = "CEROS" Then 'edit
                        habilitacionTotales(False)
                    End If

                    Session("numOperAcuse") = ""
                    Session("fechaPresentacionAcuse") = ""
                    Session("rfcAcuse") = ""
                    Session("denominacionAcuse") = ""
                    Session("recaudadoAcuse") = ""
                    Session("ejercicioAcuse") = ""
                    Session("tipoAcuse") = ""
                    Session("folioAcuse") = ""
                    Session("archivoAcuse") = ""
                    Session("selloAcuse") = ""
                    enviarDeclaracionExcel.Visible = False
                    btnContingencia.Visible = False

                Case "3" 'anual via 12 meses
                    MultiView1.ActiveViewIndex = Int32.Parse(6)
                    cargaGrid()
                    Call refrescaTotales()

            End Select

            If Session("GidAnual") <> 0 And Request.QueryString("op") <> "1" Then

                Call cargaAño()
            Else
                Call limpiaAño()
            End If
            id.Text = Session("GidAnual").ToString

            progressbar1.Style("width") = "0px"
            statusImport.Text = ""

            Dim tipo
            If normalComplementaria.Text = "NORMAL" Then
                tipo = "N"
                fechaPresentacionAnt.Visible = False
                lblFechaPresentacionAnt.Visible = False
                numOperAnt.Visible = False
                lblNumOperAnt.Visible = False
            Else
                tipo = "C"
                fechaPresentacionAnt.Visible = True
                lblFechaPresentacionAnt.Visible = True
                numOperAnt.Visible = True
                lblNumOperAnt.Visible = True
            End If

            'M=mensual
            Dim casfim, q2
            q2 = "SELECT casfim FROM clientes WHERE id=" + Session("GidCliente").ToString
            myCommand = New SqlCommand(q2, myConnection)
            dr = myCommand.ExecuteReader()
            dr.Read()
            casfim = dr("casfim")
            dr.Close()
        End If



    End Sub

    Private Sub cargaGrid()
        TreeView1.Nodes.Clear()
        Dim q
        q = "Select cast(periodo As int) As periodo, COUNT(*) As nopers, SUM(exedente) As Exedente FROM ideDetAnual WHERE idAnual=" + id.Text + " GROUP BY periodo order by cast(periodo As int)"
        'q = "SELECT DISTINCT cast(periodo as int) as periodo FROM ideDetAnual WHERE idAnual=" + id.Text + " order by cast(periodo as int)"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If Not dr.HasRows Then
            Dim MSG As String = "<script language='javascript'>alert('Cuando su declaracion esta aceptada, Posiblemente no vea los datos cargados por actualizacion de la base de datos, pero todos ellos fueron enviados correctamente en su declaracion, pulse el boton Evidencia de Envio o revise su correo incluso en spam/no deseados');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
        End If
        While dr.Read()
            Dim periodo As New TreeNode()
            periodo.Text = dr("periodo").ToString + ": Operaciones " + String.Format(dr("nopers"), "N2") + ", Excedentes " + Strings.FormatCurrency(dr("exedente"), 2)
            TreeView1.Nodes.Add(periodo)

            Dim q2
            Dim dr2 As SqlDataReader
            q2 = "SELECT DISTINCT d.id, c.nombres,c.ap1,c.ap2,c.nombreFull,c.razonSocial,d.exedente,d.chqCajaMonto FROM ideDetAnual d, contribuyente c WHERE d.idContribuyente=c.id AND idAnual=" + id.Text + " and periodo='" + dr("periodo").ToString + "' order by d.id" '+ " order by case when razonSocial = '' then nombres+ap1+ap2 else razonSocial end"
            myCommand2 = New SqlCommand(q2, myConnection)
            dr2 = myCommand2.ExecuteReader()
            While dr2.Read()
                Dim ideDetAnual = dr2("id")
                Dim contrib As New TreeNode()
                If dr2("razonSocial") <> "" Then
                    contrib.Text = dr2("razonSocial") + " , "
                Else
                    contrib.Text = IIf(Not DBNull.Value.Equals(dr2("nombreFull")), dr2("nombreFull"), dr2("nombres") & " " & dr2("ap1") & " " & dr2("ap2")) + " , "
                End If
                contrib.Text = contrib.Text + "Excedente: " + FormatCurrency(dr2("exedente")) + " , "
                If Not DBNull.Value.Equals(dr2("chqCajaMonto")) Then
                    contrib.Text = contrib.Text + "Cheques caja efectivo: " + FormatCurrency(dr2("chqCajaMonto")) + " , "
                End If
                periodo.ChildNodes.Add(contrib)

                Dim dr3 As SqlDataReader
                q2 = "SELECT * FROM cotitularesCuentaAnual WHERE idideDetAnual=" + ideDetAnual.ToString + " order by id"
                myCommand2 = New SqlCommand(q2, myConnection)
                dr3 = myCommand2.ExecuteReader()
                While dr3.Read()
                    Dim idCotitularesCuentaAnual = dr3("id")
                    Dim cuenta As New TreeNode()
                    cuenta.Text = "Cuenta: " + dr3("numeroCuenta").ToString + ", Cotitulares: " + dr3("numeroCotitulares").ToString
                    contrib.ChildNodes.Add(cuenta)

                    Dim dr4 As SqlDataReader
                    q2 = "SELECT * FROM tcotitularAnual WHERE idCotitularesCuentaAnual=" + idCotitularesCuentaAnual.ToString + " order by id"
                    myCommand2 = New SqlCommand(q2, myConnection)
                    dr4 = myCommand2.ExecuteReader()
                    While dr4.Read()
                        Dim cotit As New TreeNode()
                        cotit.Text = dr4("nombreCompleto").ToString + ", " + dr4("rfc").ToString + ", " + dr4("curp").ToString + ", " + dr4("proporcion").ToString + "%"
                        cuenta.ChildNodes.Add(cotit)
                    End While
                    dr4.Close()

                End While
                dr3.Close()

            End While
            dr2.Close()
        End While
        dr.Close()
    End Sub

    Private Sub limpiaAño()
        Dim q
        impteExcedente.Text = 0
        impteCheque.Text = 0
        numOper.Text = 0
        fechaPresentacion.Text = Left(Now(), 10).ToString

        Dim esComple
        If comple = "1" Then 'crea comple
            esComple = 1
        Else
            If Request.QueryString("nc") = "C" Then 'edita comple
                esComple = 1
            Else 'normal: crea/edita
                esComple = 0
            End If
        End If

        If esComple = 0 Then
            numOperAnt.Text = 0
            fechaPresentacionAnt.Text = Left(Now(), 10).ToString
            normalComplementaria.Text = "NORMAL"
        Else
            normalComplementaria.Text = "COMPLEMENTARIA"
            q = "SELECT TOP 1 numOper,fechaPresentacion FROM ideAnual WHERE ejercicio=" + ejercicio.ToString + " and numOper<>'0' and idcliente=" + Session("GidCliente").ToString + " order by id desc"
            myCommand = New SqlCommand(q, myConnection)
            myCommand.CommandText = q
            dr = myCommand.ExecuteReader()
            dr.Read()
            fechaPresentacionAnt.Text = dr("fechaPresentacion")
            numOperAnt.Text = dr("numOper")
            dr.Close()
        End If

        Dim dr3 As SqlDataReader
        q = "SELECT * FROM reprLegal WHERE idCliente=" + Session("GidCliente").ToString + " and esActual=1"
        myCommand2 = New SqlCommand(q, myConnection)
        dr3 = myCommand2.ExecuteReader()
        dr3.Read()
        idRepresentanteLegal.Text = dr3("id")
        RepresentanteLegal.Text = dr3("nombreCompleto")
        dr3.Close()

        'idIdeConf.Text = dr2("idIdeConf")
        Dim dr4 As SqlDataReader
        If ejercicio.ToString = "2008" Or ejercicio.ToString = "2009" Then
            myCommand3 = New SqlCommand("SELECT * FROM ideConf WHERE limite=25000.00 and porcen=2.00", myConnection)
        Else
            myCommand3 = New SqlCommand("SELECT * FROM actuales", myConnection)
        End If
        dr4 = myCommand3.ExecuteReader()
        dr4.Read()
        If ejercicio.ToString = "2008" Or ejercicio.ToString = "2009" Then
            ideConfLimite.Text = FormatNumber(dr4("limite"), 2)
        Else
            ideConfLimite.Text = FormatNumber(dr4("ideLim"), 2)
        End If
        dr4.Close()

        id.Text = 0
        estado.Text = "VACIA"
    End Sub

    Private Sub cargaAño()
        Dim dr2 As SqlDataReader
        Dim q = "SELECT * FROM ideAnual WHERE id=" + Session("GidAnual").ToString
        myCommand2 = New SqlCommand(q, myConnection)
        dr2 = myCommand2.ExecuteReader()
        dr2.Read()
        nOpers.Text = FormatNumber(dr2("nOpers"), 0)
        impteExcedente.Text = CDbl(dr2("impteExcedente")).ToString("###,###,###,##0")
        If DBNull.Value.Equals(dr2("numOperAnt")) Then
            impteCheque.Text = ""
        Else
            impteCheque.Text = CDbl(dr2("impteCheque")).ToString("###,###,###,##0")
        End If

        numOper.Text = dr2("numOper")
        If DBNull.Value.Equals(dr2("numOperAnt")) Then
            numOperAnt.Text = ""
        Else
            numOperAnt.Text = dr2("numOperAnt")
        End If
        fechaPresentacion.Text = dr2("fechaPresentacion")
        If DBNull.Value.Equals(dr2("fechaPresentacionAnt")) Then
            fechaPresentacionAnt.Text = ""
        Else
            fechaPresentacionAnt.Text = dr2("fechaPresentacionAnt")
        End If
        If DBNull.Value.Equals(dr2("fechaEnvio")) Then
            fechaEnvio.Text = ""
        Else
            fechaEnvio.Text = dr2("fechaEnvio")
        End If
        normalComplementaria.Text = dr2("normalComplementaria")
        estado.Text = dr2("estado")
        Dim dr3 As SqlDataReader
        If estado.Text = "ACEPTADA" Or estado.Text = "CONTINGENCIA" Then 'no se puede editar
            q = "SELECT * FROM reprLegal WHERE id=" + CStr(dr2("idRepresentanteLegal"))
            myCommand4 = New SqlCommand(q, myConnection)
            dr3 = myCommand4.ExecuteReader()
            dr3.Read()
            idRepresentanteLegal.Text = dr2("idRepresentanteLegal")
            RepresentanteLegal.Text = dr3("nombreCompleto")
            dr3.Close()
        Else 'editable
            q = "SELECT * FROM reprLegal WHERE esActual=1 and idCliente=" + CStr(Session("GidCliente"))
            myCommand4 = New SqlCommand(q, myConnection)
            dr3 = myCommand4.ExecuteReader()
            dr3.Read()
            idRepresentanteLegal.Text = dr3("id")
            RepresentanteLegal.Text = dr3("nombreCompleto")
            dr3.Close()
        End If

        Dim dr4 As SqlDataReader
        If ejercicio.ToString = "2008" Or ejercicio.ToString = "2009" Then
            myCommand3 = New SqlCommand("SELECT * FROM ideConf WHERE limite=25000.00 and porcen=2.00", myConnection)
        Else
            myCommand3 = New SqlCommand("SELECT * FROM actuales", myConnection)
        End If
        dr4 = myCommand3.ExecuteReader()
        dr4.Read()
        If ejercicio.ToString = "2008" Or ejercicio.ToString = "2009" Then
            ideConfLimite.Text = FormatNumber(dr4("limite"), 2)
        Else
            ideConfLimite.Text = FormatNumber(dr4("ideLim"), 2)
        End If
        dr4.Close()

        id.Text = dr2("id")

        dr2.Close()
        'cargaGrid()
    End Sub

    Private Sub insertaAnualVacia()
        Dim q, idIdeConf

        q = "SELECT id FROM reprLegal WHERE idCliente=" + Session("GidCliente").ToString + " and esActual=1"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        idRepresentanteLegal.Text = dr("id")
        dr.Close()

        Dim dr2 As SqlDataReader
        q = "SELECT id FROM ideConf WHERE limite='" + ideConfLimite.Text + "'"
        myCommand2 = New SqlCommand(q, myConnection)
        dr2 = myCommand2.ExecuteReader()
        dr2.Read()
        idIdeConf = dr2("id")
        dr2.Close()

        q = "INSERT INTO ideAnual(ejercicio,nOpers,impteExcedente,impteDeterminado,impteRecaudado,imptePendienteRecaudar,numOper,fechaPresentacion,normalComplementaria,idRepresentanteLegal,idIdeConf,idCliente,viaImportacion) VALUES('" + ejercicio.ToString + "',0,0,0,0,0,'0','" + Now().ToString("yyyy-MM-dd") + "','" + normalComplementaria.Text + "'," + idRepresentanteLegal.Text.ToString + "," + idIdeConf.ToString + "," + Session("GidCliente").ToString + ",0)"
        myCommand3 = New SqlCommand(q, myConnection)
        myCommand3.ExecuteNonQuery()

        Dim dr3 As SqlDataReader
        q = "SELECT TOP 1 id FROM ideAnual WHERE ejercicio='" + ejercicio + "' and idCliente=" + Session("GidCliente").ToString + " order by id desc"
        myCommand4 = New SqlCommand(q, myConnection)
        dr3 = myCommand4.ExecuteReader()
        dr3.Read()
        id.Text = dr3("id")
        Session("GidAnual") = id.Text
        dr3.Close()

    End Sub

    Private Sub insertaMensualVacia()
        Dim q, idIdeConf

        q = "SELECT id FROM reprLegal WHERE idCliente=" + Session("GidCliente").ToString + " and esActual=1"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        idRepresentanteLegal.Text = dr("id")
        dr.Close()

        Dim dr2 As SqlDataReader
        q = "SELECT id FROM ideConf WHERE limite='" + ideConfLimite.Text + "'"
        myCommand2 = New SqlCommand(q, myConnection)
        dr2 = myCommand2.ExecuteReader()
        dr2.Read()
        idIdeConf = dr2("id")
        dr2.Close()

        Dim i
        For i = 1 To 12
            q = "INSERT INTO ideMens(idAnual,mes,impteExcedente,impteDeterminado,impteRecaudado,imptePendienteRecaudar,impteRemanente,impteCheques,fechaPresentacion,fechaCorte,normalComplementaria,idRepresentanteLegal,idIdeConf,fedFechaEntero,fedImpto,fedNumOper,enteroPropInstit,enteroPropInstitRfc,viaImportacion,impteSaldoPendienteRecaudar,fedFechaRecaudacion) VALUES(" + Session("GidAnual").ToString + ",'" + i.ToString + "',0,0,0,0,0,0,'" + Now().ToString("yyyy-MM-dd") + "','" + Now().ToString("yyyy-MM-dd") + "','NORMAL'," + idRepresentanteLegal.Text.ToString + "," + idIdeConf.ToString + ",'" + Now().ToString("yyyy-MM-dd") + "',0,'0','','',0,0,'" + Now().ToString("yyyy-MM-dd") + "')"
            myCommand3 = New SqlCommand(q, myConnection)
            myCommand3.ExecuteNonQuery()
        Next i

    End Sub

    Private Sub insertaMensualParciales()
        Dim q, idIdeConf

        q = "SELECT id FROM reprLegal WHERE idCliente=" + Session("GidCliente").ToString + " and esActual=1"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        idRepresentanteLegal.Text = dr("id")
        dr.Close()

        Dim dr2 As SqlDataReader
        q = "SELECT id FROM ideConf WHERE limite='" + ideConfLimite.Text + "'"
        myCommand2 = New SqlCommand(q, myConnection)
        dr2 = myCommand2.ExecuteReader()
        dr2.Read()
        idIdeConf = dr2("id")
        dr2.Close()

        Dim i
        For i = 1 To 12
            q = "SELECT id FROM ideMens WHERE idAnual=" + Session("GidAnual").ToString + " and mes=" + i.ToString
            myCommand = New SqlCommand(q, myConnection)
            dr = myCommand.ExecuteReader()
            If Not dr.Read() Then 'donde no haya mensuales las crea vacias
                q = "INSERT INTO ideMens(idAnual,mes,impteExcedente,impteDeterminado,impteRecaudado,imptePendienteRecaudar,impteRemanente,impteCheques,fechaPresentacion,fechaCorte,normalComplementaria,idRepresentanteLegal,idIdeConf,fedFechaEntero,fedImpto,fedNumOper,enteroPropInstit,enteroPropInstitRfc,viaImportacion,impteSaldoPendienteRecaudar,fedFechaRecaudacion) VALUES(" + Session("GidAnual").ToString + ",'" + i.ToString + "',0,0,0,0,0,0,'" + Now().ToString("yyyy-MM-dd") + "','" + Now().ToString("yyyy-MM-dd") + "','NORMAL'," + idRepresentanteLegal.Text.ToString + "," + idIdeConf.ToString + ",'" + Now().ToString("yyyy-MM-dd") + "',0,'0','','',0,0,'" + Now().ToString("yyyy-MM-dd") + "')"
                myCommand3 = New SqlCommand(q, myConnection)
                myCommand3.ExecuteNonQuery()
            End If
            dr.Close()
        Next i

    End Sub


    Protected Sub importMensXls_Click(ByVal sender As Object, ByVal e As EventArgs) Handles importMensXls.Click
        If Not FileUpload1.HasFile Then
            Response.Write("<script language='javascript'>alert('No especificó el archivo a subir');</script>")
            Exit Sub
        End If

        Dim fileName As String = Server.HtmlEncode(FileUpload1.FileName)
        Dim extension As String = System.IO.Path.GetExtension(fileName)
        If InStr(fileName, "á") > 0 Or InStr(fileName, "é") > 0 Or InStr(fileName, "í") > 0 Or InStr(fileName, "ó") > 0 Or InStr(fileName, "ú") > 0 Or InStr(fileName, "Á") > 0 Or InStr(fileName, "É") > 0 Or InStr(fileName, "Í") > 0 Or InStr(fileName, "Ó") > 0 Or InStr(fileName, "Ú") > 0 Then
            Response.Write("<script language='javascript'>alert('Cambie el nombre del archivo para que no tenga acentos e intente de nuevo');</script>")
            Exit Sub
        End If
        If Not (extension = ".xls" Or extension = ".xlsx") Then
            Response.Write("<script language='javascript'>alert('El archivo debe ser formato Excel');</script>")
            Exit Sub
        End If

        importMensXls.Enabled = False
        progressbar1.Style("width") = "0px"
        statusImport.Text = ""
        descrip.Text = ""

        If Session("GidAnual") = 0 Then 'no hay anual del ejercicio -> insertar anual vacia
            Call insertaAnualVacia()
            Call insertaMensualVacia() 'todas las mensuales del año
        Else
            Call insertaMensualParciales() 'donde no haya mensuales las crea vacias
        End If

        Dim q, casfim
        q = "SELECT casfim FROM clientes WHERE id=" + Session("GidCliente").ToString
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        casfim = dr("casfim")
        dr.Close()

        Dim tipo
        If normalComplementaria.Text = "NORMAL" Then
            tipo = "N"
        Else
            tipo = "C"
        End If

        savePath = "C:\SAT\" + casfim + "\" 'pend: en su casfim
        savePath += casfim + "-A-" + ejercicio + "-" + tipo + "-" + Server.HtmlEncode(FileUpload1.FileName)
        h1.Value = savePath
        Try
            FileUpload1.SaveAs(savePath)
        Catch ex As Exception
            importMensXls.Enabled = True
            Dim MSG = "<script language='javascript'>alert('" + ex.Message + "');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Exit Sub
        Finally
            FileUpload1.PostedFile.InputStream.Flush()
            FileUpload1.PostedFile.InputStream.Close()
            FileUpload1.FileContent.Dispose()
            FileUpload1.Dispose()
        End Try

        If validaDecla() = 0 Then
            importMensXls.Enabled = True
            Exit Sub
        End If
        'AddFileSecurity(savePath, Session("identidad"), FileSystemRights.ReadData, AccessControlType.Allow)


        Session("error") = ""
        Session("barraN") = 1
        Session("barraIteracion") = 0

        progressbar1.Style("width") = "0px"
        lblAvance.Text = ""
        statusImport.Text = ""

        Dim objThread As New Thread(New System.Threading.ThreadStart(AddressOf DoTheWork))
        objThread.IsBackground = True
        objThread.Start()
        Session("Thread") = objThread

        Timer1.Enabled = True
        'Response.Write("<script language='javascript'>window.open('progressBarNum.aspx','progress','directories=no,titlebar=no,addressbar=0,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no, width=630,height=360');</script>")
    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblAvance.Text = "Procesando " + Session("barraIteracion").ToString + " de " + Session("barraN").ToString
        Dim ren = Session("barraIteracion")
        Dim rens = Session("barraN")
        Dim percent = Double.Parse(ren * 100 / rens).ToString("0")
        progressbar1.Style("width") = percent + "px"

        If rens = ren Or Session("error") <> "" Then
            Timer1.Dispose()
            Timer1.Enabled = False
            If Session("error") <> "" Then
                statusImport.Text = Session("error") + ". Se procesaron exitosamente los primeros " + Session("barraIteracion").ToString + " registros"
                'File.Delete(h1.Value) 'el de excel
            Else
                statusImport.Text = " Importación IDE realizada "
                Call refrescaTotales()
                estado.Text = "IMPORTADA"
            End If

            importMensXls.Enabled = True
        End If
    End Sub

    Protected Sub DoTheWork()
        importarIDEanual()
        importMensXls.Enabled = True
        If normalComplementaria.Text = "COMPLEMENTARIA" Then
            myCommand = New SqlCommand("set dateformat ymd", myConnection)
            myCommand.ExecuteNonQuery()
            Dim q = ""
            Try
                q = "UPDATE ideAnual SET fechaPresentacionAnt='" + Convert.ToDateTime(fechaPresentacionAnt.Text).ToString("yyyy-MM-dd") + "', numOperAnt='" + numOperAnt.Text + "', normalComplementaria='COMPLEMENTARIA' WHERE id=" + id.Text
                myCommand3 = New SqlCommand(q, myConnection)
                myCommand3.ExecuteNonQuery()
            Catch ex As Exception
                errImport.Text = "Error de excepcion " + ex.StackTrace + "; al procesar " + q
            End Try
        End If

    End Sub

    Private Sub creaTagsAnual()
        Dim reprLegalAp1, reprLegalAp2, reprLegalRfc, reprLegalNombres, tipo, idArch, totExedente, totCheque, totOpers
        Dim q
        q = "SELECT * FROM reprLegal WHERE idCliente=" + Session("GidCliente").ToString + " and esActual=1"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        reprLegalNombres = SecurityElement.Escape(dr("nombres"))
        reprLegalAp1 = SecurityElement.Escape(dr("ap1"))
        reprLegalAp2 = SecurityElement.Escape(dr("ap2"))
        reprLegalRfc = dr("rfc")
        dr.Close()

        q = "SELECT nOpers,impteExcedente,impteCheque FROM ideAnual WHERE id=" + id.Text
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        totExedente = dr("impteExcedente")
        totCheque = dr("impteCheque")
        totOpers = dr("nOpers")
        dr.Close()

        If normalComplementaria.Text = "NORMAL" Then
            tipo = "N"
            idArch = ""
        Else
            tipo = "C"
            idArch = id.Text
        End If

        'A=anual
        Dim casfim, vRfc, vempresa, esInstitCredito, nombreFullVal
        q = "SELECT casfim,rfcDeclarante,razonSoc,esInstitCredito,nombreFull FROM clientes WHERE id=" + Session("GidCliente").ToString
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        casfim = dr("casfim")
        vRfc = dr("rfcDeclarante")
        vempresa = SecurityElement.Escape(dr("razonSoc"))
        If dr("esInstitCredito").Equals(True) Then
            esInstitCredito = 1
        Else
            esInstitCredito = 0
        End If
        If dr("nombreFull").Equals(True) Then
            nombreFullVal = 1
        Else
            nombreFullVal = 0
        End If
        dr.Close()

        Dim periodos
        q = "SELECT count(*) as cuenta FROM ideDetAnual WHERE idAnual=" + id.Text
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        periodos = dr("cuenta")
        dr.Close()

        nomArchMens = "C:\SAT\" + casfim + "\" + casfim + "-A-" + ejercicio.ToString + tipo + idArch + ".XML"
        nomArchMensSinPath = casfim + "-A-" + ejercicio.ToString + tipo + idArch + ".XML"

        If File.Exists(nomArchMens) Then
            File.Delete(nomArchMens)
        End If

        Dim utf8WithoutBom As New System.Text.UTF8Encoding(False)
        Dim archivo As StreamWriter = New System.IO.StreamWriter(nomArchMens, False, utf8WithoutBom)
        archivo.WriteLine("<?xml version='1.0' encoding='UTF-8'?>")
        archivo.WriteLine("    <DeclaracionInformativaAnualISR xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:noNamespaceSchemaLocation='C:/SAT/ide_20150112_V3_0.xsd' version='3.0' rfcDeclarante='" + Left(vRfc, 12) + "' denominacion='" + vempresa + "'>")
        archivo.WriteLine("                <RepresentanteLegal rfc='" + reprLegalRfc + "'>")
        archivo.WriteLine("                    <Nombre>")
        archivo.WriteLine("                        <Nombres>" + reprLegalNombres + "</Nombres>")
        archivo.WriteLine("                        <PrimerApellido>" + reprLegalAp1 + "</PrimerApellido>")
        archivo.WriteLine("                        <SegundoApellido>" + reprLegalAp2 + "</SegundoApellido>")
        archivo.WriteLine("                    </Nombre>")
        archivo.WriteLine("                </RepresentanteLegal>")
        If tipo = "N" Then
            archivo.WriteLine("                <Normal ejercicio='" + ejercicio.ToString + "'></Normal>")
        Else
            archivo.WriteLine("                <Complementaria ejercicio='" + ejercicio.ToString + "' opAnterior='" + numOperAnt.Text.Trim + "' fechaPresentacion='" + CDate(fechaPresentacionAnt.Text.Trim).ToString("yyyy-MM-dd") + "'></Complementaria>")
        End If
        If esInstitCredito = 1 Then
            archivo.WriteLine("                <InstitucionDeCredito>")
        Else
            archivo.WriteLine("                <InstitucionDistintaDeCredito>")
        End If

        q = "SELECT DISTINCT periodo FROM ideDetAnual WHERE idAnual=" + id.Text
        myCommand = New SqlCommand(q, myConnection)
        Dim dr3 As SqlDataReader
        dr3 = myCommand.ExecuteReader()
        If dr3.HasRows Then
            While dr3.Read()
                Dim periodo = dr3("periodo")
                archivo.WriteLine("                             <ReporteDeDepositosEnEfectivo Periodo='" + periodo + "'>")
                Dim dr4 As SqlDataReader
                q = "SELECT i.*, c.*, i.id as iId, c.id as cId FROM ideDetAnual i, contribuyente c WHERE idAnual=" + id.Text + " and periodo='" + periodo.ToString + "' and i.idContribuyente=c.id"
                myCommand = New SqlCommand(q, myConnection)
                dr4 = myCommand.ExecuteReader()
                If dr4.HasRows Then
                    Dim dr2 As SqlDataReader
                    While dr4.Read()
                        Dim ideDetAnual, idCuenta, nombres, ap1, ap2, nombreFull, razonSocial, rfc, Dom, numSocioCliente, sumaDeposEfe, montoExcedente, impuestoDeterminado, impuestoRecaudado, recaudacionPendiente, cheque
                        Dim telefono1 As String, telefono2 As String, curp, correo
                        correo = ""

                        razonSocial = SecurityElement.Escape(dr4("razonSocial"))
                        If nombreFullVal = 0 Then
                            If Not DBNull.Value.Equals(dr4("nombres")) Then
                                nombres = SecurityElement.Escape(dr4("nombres"))
                            End If
                            If Not DBNull.Value.Equals(dr4("ap1")) Then
                                ap1 = SecurityElement.Escape(dr4("ap1"))
                            End If
                            If Not DBNull.Value.Equals(dr4("ap2")) Then
                                ap2 = SecurityElement.Escape(dr4("ap2"))
                            End If
                        Else
                            If Not DBNull.Value.Equals(dr4("nombreFull")) Then
                                nombreFull = SecurityElement.Escape(dr4("nombreFull"))
                            End If
                        End If
                        Dim numSocioClienteVal = ""
                        If Not DBNull.Value.Equals(dr4("numSocioCliente")) Then
                            If dr4("numSocioCliente") <> "" Then
                                numSocioCliente = dr4("numSocioCliente")
                                numSocioClienteVal = " NumeroCliente='" + numSocioCliente + "'"
                            End If
                        End If
                        rfc = dr4("rfc")
                        Dom = SecurityElement.Escape(dr4("Dom"))
                        If Not DBNull.Value.Equals(dr4("telefono1")) Then
                            telefono1 = dr4("telefono1")
                        End If
                        If Not DBNull.Value.Equals(dr4("telefono2")) Then
                            telefono2 = dr4("telefono2")
                        End If
                        montoExcedente = CLng(dr4("exedente"))
                        If Not DBNull.Value.Equals(dr4("chqCajaMonto")) Then
                            cheque = CLng(dr4("chqCajaMonto"))
                        End If
                        ideDetAnual = dr4("iId")
                        If Not DBNull.Value.Equals(dr4("curp")) Then
                            curp = dr4("curp")
                        End If
                        If Not DBNull.Value.Equals(dr4("correo")) Then
                            correo = dr4("correo")
                        End If
                        If correo = "" Then
                            correo = "sin correo"
                        End If
                        archivo.WriteLine("                                 <RegistroDeDetalle>")
                        If telefono1 = "" Then
                            telefono1 = "000000000000000"
                        End If
                        If Len(telefono1) < 15 Then
                            telefono1 = telefono1.PadLeft(15, "0"c)
                        End If
                        If telefono2 = "" Then
                            telefono2 = "000000000000000"
                        End If
                        If Len(telefono2) < 15 Then
                            telefono2 = telefono2.PadLeft(15, "0"c)
                        End If
                        If razonSocial = "" Then
                            archivo.WriteLine("                                     <PersonaFisica rfc='" + Left(rfc, 13) + "' telefono1='" & Right(telefono1, 15) & "' telefono2='" & Right(telefono2, 15) & "' curp='" & Left(curp, 18) & "'" & numSocioClienteVal & " correoElectronico='" & correo & "'>") '15 letrasNumeros
                            archivo.WriteLine("                                         <Nombre>")
                            If nombreFullVal = 0 Then
                                archivo.WriteLine("                                             <Nombres>" & Left(nombres, 40) & "</Nombres>")
                                archivo.WriteLine("                                             <PrimerApellido>" & Left(ap1, 40) & "</PrimerApellido>")
                                If ap2 <> "" Then
                                    archivo.WriteLine("                                             <SegundoApellido>" & Left(ap2, 40) & "</SegundoApellido>")
                                End If
                            Else
                                archivo.WriteLine("                                             <NombreCompleto>" & Left(nombreFull, 120) & "</NombreCompleto>")
                            End If

                            archivo.WriteLine("                                         </Nombre>")
                                archivo.WriteLine("                                          <Domicilio>")
                                archivo.WriteLine("                                                 <DomicilioCompleto>" & Left(Dom, 150) & "</DomicilioCompleto>")
                                archivo.WriteLine("                                          </Domicilio>")
                            Else
                            archivo.WriteLine("                                     <PersonaMoral rfc='" + Left(rfc, 12) + "' telefono1='" & Right(telefono1, 15) & "' telefono2='" & Right(telefono2, 15) & "'" & numSocioClienteVal & " correoElectronico='" & correo & "'>") '15 letrasNumeros
                            archivo.WriteLine("                                         <Denominacion>" & Left(razonSocial, 250) & "</Denominacion>")
                            archivo.WriteLine("                                          <Domicilio>")
                            archivo.WriteLine("                                                 <DomicilioCompleto>" & Left(Dom, 150) & "</DomicilioCompleto>")
                            archivo.WriteLine("                                          </Domicilio>")
                        End If
                        myCommand = New SqlCommand("SELECT numeroCuenta FROM cotitularesCuentaAnual WHERE idideDetAnual =" + ideDetAnual.ToString, myConnection) 'todos los #s cta
                        dr = myCommand.ExecuteReader()
                        While dr.Read()
                            archivo.WriteLine("                                         <numeroCuenta>" & CStr(dr("numeroCuenta")) & "</numeroCuenta>")
                        End While
                        dr.Close()
                        If razonSocial = "" Then
                            archivo.WriteLine("                                     </PersonaFisica>")
                        Else
                            archivo.WriteLine("                                     </PersonaMoral>")
                        End If
                        If Not DBNull.Value.Equals(montoExcedente) Or Not DBNull.Value.Equals(cheque) Then
                            If Not DBNull.Value.Equals(montoExcedente) Then
                                archivo.WriteLine("                                     <DepositoEnEfectivo montoExcedenteDeposito='" & CStr(montoExcedente) & "' />")
                            End If
                            If esInstitCredito = 1 And Not DBNull.Value.Equals(cheque) Then
                                archivo.WriteLine("                                     <ChequeDeCaja montoCheque='" & CStr(cheque) & "' />")
                            End If
                            myCommand = New SqlCommand("SELECT * FROM cotitularesCuentaAnual WHERE idideDetAnual =" + ideDetAnual.ToString + " and numeroCotitulares>0", myConnection) 'solo #s cta con cotit > 0
                            dr = myCommand.ExecuteReader()
                            While dr.Read()
                                archivo.WriteLine("                                 <Cotitulares numeroCuenta='" & CStr(dr("numeroCuenta")) & "' numeroCotitulares='" & CStr(dr("numeroCotitulares")) & "'>")
                                Dim idCotitularesCuentaAnual = dr("id")
                                myCommand2 = New SqlCommand("SELECT * FROM tcotitularAnual WHERE idCotitularesCuentaAnual =" + idCotitularesCuentaAnual.ToString, myConnection)
                                dr2 = myCommand2.ExecuteReader()
                                While dr2.Read()
                                    archivo.WriteLine("                                    <tCotitular RFC='" & CStr(dr2("rfc")) & "' curp='" & CStr(dr2("curp")) & "' Proporcion='" & CDbl(dr2("proporcion")).ToString("###.0000") & "'>")
                                    archivo.WriteLine("                                        <Nombre>")
                                    archivo.WriteLine("                                            <NombreCompleto> " & CStr(SecurityElement.Escape(dr2("nombreCompleto"))) & " </NombreCompleto>")
                                    archivo.WriteLine("                                        </Nombre>")
                                    archivo.WriteLine("                                    </tCotitular>")
                                End While
                                dr2.Close()
                                archivo.WriteLine("                                 </Cotitulares>")
                            End While
                            dr.Close()
                        End If
                        archivo.WriteLine("                                 </RegistroDeDetalle>")
                    End While
                End If
                dr4.Close()
                archivo.WriteLine("                             </ReporteDeDepositosEnEfectivo>")
            End While
        End If
        dr3.Close()

        For p = 1 To 12
            q = "SELECT DISTINCT id FROM ideDetAnual WHERE idAnual=" + id.Text + " and periodo='" + CStr(p) + "'"
            myCommand = New SqlCommand(q, myConnection)
            dr3 = myCommand.ExecuteReader()
            If Not dr3.HasRows Then
                archivo.WriteLine("                             <ReporteDeDepositosEnEfectivo Periodo='" + CStr(p) + "' />")
            End If
            dr3.Close()
        Next p

        If esInstitCredito = 1 Then
            archivo.WriteLine("                                 <Totales importeCheques='" & CLng(totCheque).ToString("###########0") & "' operacionesRelacionadas='" & CLng(totOpers).ToString("###########0") & "' importeExcedenteDepositos='" & CDbl(totExedente).ToString("#############0") & "' />")
        Else
            archivo.WriteLine("                                 <Totales operacionesRelacionadas='" & CLng(totOpers).ToString("###########0") & "' importeExcedenteDepositos='" & CDbl(totExedente).ToString("#############0") & "' />")
        End If
        If esInstitCredito = 1 Then
            archivo.WriteLine("                 </InstitucionDeCredito>")
        Else
            archivo.WriteLine("                 </InstitucionDistintaDeCredito>")
        End If
        archivo.WriteLine("     </DeclaracionInformativaAnualISR>")

        archivo.Close()
    End Sub

    Private Sub subeXMLanualBD()
        'subir archivo a la BD
        Dim fstream As FileStream
        Dim imgdata As Byte()
        Dim data As Byte()
        Dim finfo As FileInfo
        finfo = New FileInfo(nomArchMens & ".ZIP")
        Dim numbyte As Long
        Dim br As BinaryReader
        numbyte = finfo.Length
        fstream = New FileStream(nomArchMens & ".ZIP", FileMode.Open, FileAccess.Read)
        br = New BinaryReader(fstream)
        data = br.ReadBytes(numbyte)
        imgdata = data

        Dim myCommand2 As New SqlCommand
        With myCommand2
            .Connection = myConnection
            .CommandType = CommandType.StoredProcedure
            .CommandText = "ideAnualSubexml"
            .Parameters.AddWithValue("@ID", id.Text)    '1 xml x decl ya sea norm o complems del año
            .Parameters.AddWithValue("@Logo", imgdata)
            dr = .ExecuteReader()
        End With
        br.Close()
        fstream.Close()
        dr.Close()

    End Sub

    Private Sub comprimeAnual()

        If File.Exists(nomArchMens & ".ZIP") Then
            File.Delete(nomArchMens & ".ZIP")
        End If

        Try
            Using zip As ZipFile = New ZipFile
                zip.AddFile(nomArchMens, "")
                zip.Save(nomArchMens & ".ZIP")
            End Using
        Catch ex1 As Exception
            statusImport.Text = "Error al comprimir: " + ex1.ToString
        Finally
            If File.Exists(nomArchMens) Then 'borro el xml
                File.Delete(nomArchMens)
            End If
        End Try

    End Sub

#Region "Validar XML"
    Dim TotErrores As New System.Text.StringBuilder
    ''' <summary>
    ''' Valida la estructura del XML generado usando un archivo XSD
    ''' </summary>
    ''' <param name="xml"></param>
    ''' <param name="xsdNameSpace" description="Name Space del Archivo XSD"></param>
    ''' <returns>Regresa False si el XML tiene errores VS el Archivo XSD, True si no Contiene Errores</returns>
    Public Function ValidaXML(ByVal xml As XDocument, ByRef Errores As String,
                              Optional ByVal xsdNameSpace As String = "") As Boolean

        Dim Valido As Boolean = True
        Dim settings As New XmlReaderSettings()
        'settings.Schemas.Add(Nothing, "www.declaracioneside.com/ide_20150112_V3_0.xsd")
        Dim Path As String = System.AppDomain.CurrentDomain.BaseDirectory
        settings.Schemas.Add(Nothing, IO.Path.Combine(Path, "ide_20150112_V3_0.xsd"))

        settings.ValidationType = ValidationType.Schema
        settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings

        AddHandler settings.ValidationEventHandler,
        AddressOf settings_ValidationEventHandler

        'settings.IgnoreWhitespace = True
        'settings.IgnoreComments = True

        Dim Temp = System.IO.Path.GetTempFileName()
        xml.Save(Temp)

        Using reader As XmlReader = XmlReader.Create(Temp, settings)
            While (reader.Read())
                'Do Nothing
            End While
        End Using

        If TotErrores.ToString <> "" Then
            Valido = False
            Errores = TotErrores.ToString
        End If

        Return Valido
    End Function
    Private Sub settings_ValidationEventHandler(ByVal sender As Object,
            ByVal e As System.Xml.Schema.ValidationEventArgs)
        TotErrores.Append(e.Message & vbNewLine)
    End Sub
#End Region

    Private Function validacion() As Boolean
        'validar
        Dim xDoc As XDocument = XDocument.Load(nomArchMens)
        Dim errores As String = ""
        If ValidaXML(xDoc, errores) = False Then
            descrip.Text = "Se encontraron errores: " + errores + ", Contáctenos"
            Dim elcorreo As New System.Net.Mail.MailMessage
            Using elcorreo
                elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                elcorreo.To.Add("declaracioneside@gmail.com")
                elcorreo.Subject = "Decl. an ERROR xml"
                elcorreo.Body = "<html><body>cliente=" + Session("curCorreo") + ", ejercicio=" + ejercicio + ", error=" + errores + "</body></html>"
                elcorreo.IsBodyHtml = True
                elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                Dim smpt As New System.Net.Mail.SmtpClient
                smpt.Host = "smtp.gmail.com"
                smpt.Port = "587"
                smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a")
                smpt.EnableSsl = True 'req p server gmail
                Try
                    smpt.Send(elcorreo)
                    elcorreo.Dispose()
                Catch ex As Exception
                    Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
                    Return False
                Finally
                End Try
            End Using
            Dim MSG As String = "<script language='javascript'>alert('Se encontraron errores: " + errores + ", Contáctenos');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Return False
        End If
        Return True
    End Function
    Private Function creaXMLaño() As String
        Call creaTagsAnual()

        If validacion() = False Then
            Return "Errores al validar archivo"
        End If

        Call comprimeAnual() 'borra xml crea zip
        Call subeXMLanualBD()
        Return ""
        'Call enviaArchivo(nomArchMensSinPath)        
    End Function

    Private Sub creaXMLañoCeros()
        Call creaTagsAnCeros()
        Call comprimeAnual()
        Call subeXMLanualBD()
        'Call enviaArchivo(nomArchMensSinPath)
        statusImport.Text = "Declaración creada"
        Response.Write("<script language='javascript'>alert('Declaración creada');</script>")
    End Sub

    Private Function validaSecuencia(ByVal descrip, ByVal descripAnt, ByVal ren, ByRef msgErr) As Integer
        If descripAnt = "" And descrip <> "PER" Then
            msgErr = msgErr + ". " + "En el renglón 5 debe indicar PER en la columna descripción"
            Return 0
        End If
        If descrip = "PER" Then
            If descripAnt <> "" And descripAnt <> "CTA" And descripAnt <> "COT" Then
                msgErr = msgErr + ". " + "Una descripción PER solo puede ser precedida por una CTA o COT, verifique en el renglón " + ren.ToString
                Return 0
            End If
        ElseIf descrip = "CTA" Then
            If descripAnt <> "CON" And descripAnt <> "CTA" And descripAnt <> "COT" Then
                msgErr = msgErr + ". " + "Una descripción CTA solo puede ser precedida por una CON, CTA o COT, verifique en el renglón " + ren.ToString
                Return 0
            End If
        ElseIf descrip = "COT" Then
            If descripAnt <> "CTA" And descripAnt <> "COT" Then
                msgErr = msgErr + ". " + "Una descripción COT solo puede ser precedida por una CTA o COT, verifique en el renglón " + ren.ToString
                Return 0
            End If
        Else 'CON
            If descripAnt <> "CTA" And descripAnt <> "PER" And descripAnt <> "COT" Then
                msgErr = msgErr + ". " + "Una descripción CON solo puede ser precedida por una PER, CTA o COT, verifique en el renglón " + ren.ToString
                Response.Write("<script language='javascript'>alert('Una descripción CON solo puede ser precedida por una PER, CTA o COT, verifique en el renglón " + ren.ToString + "');</script>")
                Return 0
            End If
        End If

        Return 1
    End Function


    Private Function validaDecla() As Integer
        Dim ctrlErr = 0
        Dim msgErr = ""
        Try

            Dim excel As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application
            'Dim w As Workbook = excel.Workbooks.Open(savePath,,,,,,,,,,,,,, True)
            Dim w As Workbook = excel.Workbooks.Open(savePath)

            'For i As Integer = 1 To w.Sheets.Count
            Dim sheet As Worksheet = w.Sheets(1) 'i     'abrirá la 1er hoja del libro
            'xlHoja = xlApp.Worksheets(CStr(DatePart("m", mes.Value))) ' hojas: 1:12

            If sheet.UsedRange.Rows.Count < 5 Then 'rens del encab
                w.Close(False)
                excel.Quit()
                w = Nothing
                excel = Nothing
                msgErr = msgErr + ". " + vbCr + "Es necesario dejar el encabezado de los primeros 5 renglones tal cual se le indica en la plantilla default"
                ctrlErr = 1
                GoTo etqErr
            End If

            If sheet.UsedRange.Columns.Count < 14 Then 'cols del encab
                w.Close(False)
                excel.Quit()
                w = Nothing
                excel = Nothing
                msgErr = msgErr + ". " + vbCr + "Es necesario dejar el encabezado de los primeros 5 renglones tal cual se le indica en la plantilla default con 14 columnas"
                ctrlErr = 1
                GoTo etqErr
            End If

            Dim r As Microsoft.Office.Interop.Excel.Range = sheet.UsedRange
            Dim array(,) As Object = r.Value(XlRangeValueDataType.xlRangeValueDefault)
            Dim nRensPre = sheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row 'sin rens en bco

            w.Close(False)   'cierro excel y trabajo con la var
            excel.Quit()
            w = Nothing
            excel = Nothing

            If array IsNot Nothing Then
                Dim rens As Integer = nRensPre 'array.GetUpperBound(0)
                'Dim cols As Integer = array.GetUpperBound(1)

                Dim descrip, descripAnt, movFecha, movOper, movMonto, ctaNum, ctaCotit, ctaPropor, ctaIdeRec, nombres, ap1, ap2, razon, rfc, Dom, telefono1, telefono2, exedente, determinado, recaudado, pendienteRecaudar, numSocioCliente, sumaDeposEfe, ctaTipo, ctaTipoMoneda, curp, correo
                Dim q, idIdeDet, idContrib, ctaActual, ideDetAnualActual, cuentasIdeDetAnualActual, movMontoMN, cheque, mes, cotProporcion, cotRfc, cotNom, cotitularesCuentaActual, cotCurp

                Dim esInstitCredito, nombreFullVal
                q = "SELECT esInstitCredito, nombreFull FROM clientes WHERE id=" + Session("GidCliente").ToString
                myCommand = New SqlCommand(q, myConnection)
                dr = myCommand.ExecuteReader()
                dr.Read()
                If dr("esInstitCredito").Equals(True) Then
                    esInstitCredito = 1
                Else
                    esInstitCredito = 0
                End If
                If dr("nombreFull").Equals(True) Then
                    nombreFullVal = 1
                Else
                    nombreFullVal = 0
                End If
                dr.Close()

                descripAnt = ""
                For ren As Integer = 6 To rens '1-5rens=encab 6o=datos
                    'For col As Integer = 1 To cols
                    If array(ren, 1) Is Nothing And array(ren, 2) Is Nothing And array(ren, 3) Is Nothing And array(ren, 4) Is Nothing And array(ren, 5) Is Nothing And array(ren, 6) Is Nothing And array(ren, 7) Is Nothing And array(ren, 8) Is Nothing And array(ren, 9) Is Nothing And array(ren, 10) Is Nothing And array(ren, 11) Is Nothing And array(ren, 12) Is Nothing And array(ren, 13) Is Nothing And array(ren, 14) Is Nothing Then ' ren bco
                        GoTo siguiente
                    End If

                    If Not array(ren, 1) Is Nothing Then
                        descrip = Trim(UCase(array(ren, 1)))
                        If descrip = "CON" Or descrip = "CTA" Or descrip = "PER" Or descrip = "COT" Then
                            If validaSecuencia(descrip, descripAnt, ren, msgErr) < 1 Then
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        Else
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(ren) + " la descripción valida solo puede ser CON, PER, CTA, COT verifique"
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                    Else
                        descrip = ""
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(ren) + " la descripción no puede estar vacia"
                        ctrlErr = 1
                        GoTo siguiente
                    End If
                    descripAnt = descrip
                    If descrip = "PER" Then
                        If Not array(ren, 2) Is Nothing Then
                            mes = array(ren, 2).ToString.ToUpper.Trim
                        Else
                            mes = ""
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(ren) + " requiere especificar el número de mes"
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                        If IsNumeric(mes) = False Then
                            msgErr = msgErr + ". " + vbCr + "El renglon " + CStr(ren) + " no contiene un valor numérico para el numero de mes, valor entre 1 y 12, sin ceros a la izquierda"
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                        If CDbl(mes) < 0 Or CDbl(mes) > 12 Then
                            msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(ren) + " debe tener un mes entre 1 y 12"
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                        mes = CDbl(mes).ToString
                    ElseIf descrip = "CON" Then 'contribuyente
                        'movOper = Trim(UCase(array(ren, 2))) 'ACI,ahorroCorrienteInversion
                        If Not array(ren, 2) Is Nothing Then
                            If array(ren, 2).ToString <> "" Then
                                If nombreFullVal = 0 Then
                                    If Len(array(ren, 2).ToString.ToUpper.Trim) > 40 Then
                                        msgErr = msgErr + ". " + vbCr + "Truncando nombre a 40 caracteres en el renglon " + CStr(ren)
                                    End If
                                    nombres = Left(array(ren, 2).ToString.ToUpper.Trim, 40).Replace("'", "''")
                                Else
                                    If Len(array(ren, 2).ToString.ToUpper.Trim) > 120 Then
                                        msgErr = msgErr + ". " + vbCr + "Truncando nombre completo a 120 caracteres en el renglon " + CStr(ren)
                                    End If
                                    nombres = Left(array(ren, 2).ToString.ToUpper.Trim, 120).Replace("'", "''")
                                End If
                            Else
                                nombres = ""
                            End If
                        Else
                                nombres = ""
                        End If
                        If Not array(ren, 3) Is Nothing Then
                            If array(ren, 3).ToString <> "" Then
                                If nombreFullVal = 0 Then
                                    If Len(array(ren, 3).ToString.ToUpper.Trim) > 40 Then
                                        msgErr = msgErr + ". " + vbCr + "Truncando apellido paterno a 40 caracteres en el renglon " + CStr(ren)
                                    End If
                                    ap1 = Left(array(ren, 3).ToString.ToUpper.Trim, 40).Replace("'", "''")
                                End If
                            Else
                                ap1 = ""
                            End If
                        Else
                                ap1 = ""
                        End If
                        If Not array(ren, 4) Is Nothing Then
                            If array(ren, 4).ToString <> "" Then
                                If nombreFullVal = 0 Then
                                    If Len(array(ren, 4).ToString.ToUpper.Trim) > 40 Then
                                        msgErr = msgErr + ". " + vbCr + "Truncando apellido materno a 40 caracteres en el renglon " + CStr(ren)
                                    End If
                                    ap2 = Left(array(ren, 4).ToString.ToUpper.Trim, 40).Replace("'", "''")
                                End If
                            Else
                                ap2 = ""
                            End If
                        Else
                                ap2 = ""
                        End If
                        If Not array(ren, 5) Is Nothing Then
                            If array(ren, 5).ToString <> "" Then
                                If Len(array(ren, 5).ToString.ToUpper.Trim) > 250 Then
                                    msgErr = msgErr + ". " + vbCr + "Truncando razon social a 250 caracteres en el renglon " + CStr(ren)
                                End If
                                razon = Left(array(ren, 5).ToString.ToUpper.Trim, 250).Replace("'", "''")
                            Else
                                razon = ""
                            End If
                        Else
                                razon = ""
                        End If
                        If nombreFullVal = 0 Then
                            If (nombres = "" And ap1 = "") And razon = "" Then
                                msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(ren) + " requiere especificar el nombre con apellidos o bien la razon social"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        Else
                            If nombres = "" And razon = "" Then
                                msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(ren) + " requiere especificar el nombre completo o bien la razon social"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        End If

                        If nombres <> "" And razon <> "" Then
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(ren) + " si no está reportando una razon social dejela en blanco, en caso contrario deje en blanco el nombre"
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                        If nombreFullVal = 0 Then
                            If ap1 = "" And ap2 <> "" Then
                                msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(ren) + " falta el primer apellido ya que si indico un segundo apellido"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        End If
                        If Not array(ren, 6) Is Nothing Then
                            If array(ren, 6).ToString <> "" Then
                                'If InStr(array(ren, 6).ToString, " ") > 0 Or InStr(array(ren, 6).ToString, "-") > 0 Then
                                '    msgErr = msgErr + ". " + "En el renglon " + CStr(ren) + " el rfc no debe tener espacios ni guiones"
                                '    ctrlErr = 1
                                '    GoTo siguiente
                                'End If
                                rfc = array(ren, 6).ToString.ToUpper.Trim.Replace(" ", "").Replace("-", "")
                                Dim expresion
                                If razon = "" Then 'pf
                                    expresion = "[A-Z,Ñ,&amp;]{4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?"
                                    If Len(rfc) < 9 Or Len(rfc) > 13 Then
                                        msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(ren) + " el tamaño de rfc debe ser 10-13 caracteres"
                                        ctrlErr = 1
                                        GoTo siguiente
                                    End If
                                Else 'pm
                                    expresion = "[A-Z,Ñ,&amp;]{3}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?"
                                    If Len(rfc) < 9 Or Len(rfc) > 12 Then
                                        msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(ren) + " el tamaño de rfc debe ser 9-12 caracteres"
                                        ctrlErr = 1
                                        GoTo siguiente
                                    End If
                                End If
                                If Not Regex.IsMatch(rfc, expresion) Then
                                    msgErr = msgErr + ". " + vbCr + "Formato de rfc invalido en el renglon " + CStr(ren)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                            Else
                                rfc = ""
                            End If
                        Else
                                rfc = ""
                            End If
                        If Not array(ren, 7) Is Nothing Then
                            If array(ren, 7).ToString <> "" Then
                                If Len(Regex.Replace(array(ren, 7).ToString.ToUpper.Trim, "\s+", " ")) > 150 Then
                                    'msgErr = msgErr + ". " + vbCr + "Truncando domicilio a 150 caracteres en el renglon " + CStr(ren)
                                End If
                                Dom = Left(array(ren, 7).ToString.ToUpper.Trim, 150).Replace("'", "''")
                                Dom = Regex.Replace(Dom, "\s+", " ")
                            Else
                                Dom = ""
                            End If
                        Else
                                Dom = ""
                            End If
                        If Not array(ren, 8) Is Nothing Then
                            If array(ren, 8).ToString <> "" Then
                                If Len(array(ren, 8).ToString.ToUpper.Trim) > 15 Then
                                    msgErr = msgErr + ". " + vbCr + "Truncando telefono1 a 15 caracteres en el renglon " + CStr(ren)
                                End If
                                If Not IsNumeric(Regex.Replace(array(ren, 8).ToString.ToUpper.Trim.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("'", ""), "[a-zA-Z\s]+", "")) Then
                                    msgErr = msgErr + ". " + vbCr + "Telefono1 " + array(ren, 8).ToString.ToUpper.Trim.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("'", "") + " solo debe tener numeros en el renglon " + CStr(ren)
                                End If
                                telefono1 = Left(Regex.Replace(array(ren, 8).ToString.ToUpper.Trim.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("'", ""), "[a-zA-Z\s]+", ""), 15).Trim
                            Else
                                telefono1 = ""
                            End If
                        Else
                                telefono1 = ""
                            End If
                        If Not array(ren, 9) Is Nothing Then
                            If array(ren, 9).ToString <> "" Then
                                If Len(array(ren, 9).ToString.ToUpper.Trim) > 15 Then
                                    msgErr = msgErr + ". " + vbCr + "Truncando telefono2 a 15 caracteres en el renglon " + CStr(ren)
                                End If
                                If Not IsNumeric(Regex.Replace(array(ren, 9).ToString.ToUpper.Trim.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("'", ""), "[a-zA-Z\s]+", "")) Then
                                    msgErr = msgErr + ". " + vbCr + "Telefono2 " + array(ren, 9).ToString.ToUpper.Trim.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("'", "") + " solo debe tener numeros en el renglon " + CStr(ren)
                                End If
                                telefono2 = Left(regex.Replace(array(ren, 9).ToString.ToUpper.Trim.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("'", ""), "[a-zA-Z\s]+", ""), 15).Trim
                            Else
                                telefono2 = ""
                            End If
                        Else
                                telefono2 = ""
                            End If
                        If Not array(ren, 10) Is Nothing Then
                            If array(ren, 10).ToString <> "" Then
                                If Len(array(ren, 10).ToString.ToUpper.Trim) > 20 Then
                                    msgErr = msgErr + ". " + vbCr + "Truncando numero de socio a 20 caracteres en el renglon " + CStr(ren)
                                End If
                                numSocioCliente = Left(array(ren, 10).ToString.ToUpper.Trim, 20)
                            Else
                                numSocioCliente = ""
                            End If
                        Else
                                numSocioCliente = ""
                            End If
                        If Not array(ren, 11) Is Nothing Then
                            If array(ren, 11).ToString <> "" Then
                                If Not IsNumeric(array(ren, 11)) Then
                                    msgErr = msgErr + ". " + vbCr + "El excedente debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(ren)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                                If CDbl(array(ren, 11)) < 0 Then
                                    msgErr = msgErr + ". " + vbCr + "el excedente debe ser >= 0 en el renglon " + CStr(ren)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If

                                exedente = redondea(array(ren, 11)).ToString.Trim
                            Else
                                exedente = ""
                            End If
                        Else
                                exedente = ""
                            End If
                        If Not array(ren, 12) Is Nothing Then
                            If array(ren, 12).ToString <> "" Then
                                If Not IsNumeric(array(ren, 12)) Then
                                    msgErr = msgErr + ". " + vbCr + "El monto de cheques debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(ren)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                                If CDbl(array(ren, 12)) < 0 Then
                                    msgErr = msgErr + ". " + vbCr + "el monto de cheques debe ser >= 0 en el renglon " + CStr(ren)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If

                                cheque = redondea(array(ren, 12)).ToString.Trim
                            Else
                                cheque = ""
                            End If
                        Else
                                cheque = ""
                            End If
                        If Not array(ren, 13) Is Nothing Then
                            If array(ren, 13).ToString <> "" Then
                                If Len(array(ren, 13).ToString.ToUpper.Trim) > 18 Then
                                    msgErr = msgErr + ". " + vbCr + "Truncando curp a 18 caracteres en el renglon " + CStr(ren)
                                End If
                                curp = Left(array(ren, 13).ToString.Trim, 18)
                            Else
                                curp = ""
                            End If
                        Else
                                curp = ""
                            End If
                        If Not array(ren, 14) Is Nothing Then
                            If array(ren, 14).ToString <> "" Then
                                If Len(array(ren, 14).ToString.ToUpper.Trim) > 50 Then
                                    msgErr = msgErr + ". " + vbCr + "Truncando correo a 50 caracteres en el renglon " + CStr(ren)
                                End If
                                correo = Regex.Replace(Left(array(ren, 14).ToString.Trim, 50).Replace("'", "''").Replace("<", "").Replace(">", ""), "[^a-zA-Z0-9-_\.@]+", "")
                                'Dim expresion
                                'expresion = "[A-Z,Ñ,&amp;]{4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?"
                                'If Not Regex.IsMatch(correo, expresion) Then
                                '    msgErr = msgErr + ". " + vbCr + "Formato de correo invalido en el renglon " + CStr(ren)
                                '    ctrlErr = 1
                                '    GoTo siguiente
                                'End If
                            Else
                                correo = ""
                            End If
                        Else
                                correo = ""
                            End If

                            If Dom = "" Then
                                msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(ren) + " requiere especificar domicilio"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If rfc = "" Then
                                msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(ren) + " requiere especificar rfc"
                                ctrlErr = 1
                                GoTo siguiente
                            Else
                                'debe existir el rfc de la anual en al menos un registro de detalle de alguna mensual (ideDet) del año
                                'myCommand = New SqlCommand("SELECT idd.id FROM idedetAnual idd, ideAnual a, contribuyente c WHERE a.ejercicio='" + ejercicio.ToString + "' AND idd.idAnual=a.id AND idd.idContribuyente=c.id AND c.rfc='" + rfc + "'", myConnection)
                                'dr = myCommand.ExecuteReader()
                                'If Not dr.Read() Then
                                '    Response.Write("<script language='javascript'>alert('El renglon " + CStr(ren) + " requiere que tenga presentada previamente alguna declaración mensual con este RFC');</script>")
                                '    ctrlErr = 1
                                '    Goto siguiente
                                'End If
                                'dr.Close()
                            End If

                            'If numSocioCliente = "" Then
                            '    MsgBox("el renglon " + CStr(ren) + " no debe estar vacio en columna F", , "Descartando, importación finalizada")
                            '    ctrlErr = 1
                            '    Goto siguiente
                            'End If

                            If esInstitCredito = 1 Then
                                If (exedente = "" Or CDbl(Replace(exedente, ",", "")) = 0) And (cheque = "" Or CDbl(Replace(cheque, ",", "")) = 0) Then
                                    msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(ren) + " indique el excedente distinto de 0 o bien el monto de cheques distinto de 0"
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                            Else
                                If (exedente = "" Or CDbl(Replace(exedente, ",", "")) = 0) Then
                                    msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(ren) + " indique el excedente distinto de 0"
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                            End If

                            If exedente <> "" Then
                                If IsNumeric(exedente) = False Then
                                    msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(ren) + " no contiene excedente valido"
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                            End If
                            If cheque <> "" Then
                                If IsNumeric(cheque) = False Then
                                    msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(ren) + " no contiene monto de cheque valido"
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                            End If

                            If razon = "" And curp = "" Then ' PF
                                msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(ren) + " requiere una curp para la persona fisica"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If curp <> "" Then
                                Dim expresion = "[A-Z][A,E,I,O,U,X][A-Z]{2}[0-9]{2}[0-1][0-9][0-3][0-9][M,H][A-Z]{2}[B,C,D,F,G,H,J,K,L,M,N,Ñ,P,Q,R,S,T,V,W,X,Y,Z]{3}[0-9,A-Z][0-9]"
                                If Not Regex.IsMatch(curp, expresion) Then
                                    msgErr = msgErr + ". " + vbCr + "Formato de curp invalido en el renglon " + CStr(ren)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                            End If

                        ElseIf descrip = "CTA" Then
                            If Not array(ren, 2) Is Nothing Then
                                If Len(array(ren, 2).ToString.ToUpper.Trim) > 20 Then
                                    msgErr = msgErr + ". " + vbCr + "Truncando numero de cuenta a 20 caracteres en el renglon " + CStr(ren)
                                End If
                                ctaNum = Left(array(ren, 2).ToString.ToUpper.Trim, 20)
                            Else
                                If exedente = "" And cheque <> "" Then
                                    ctaNum = "9"
                                Else
                                    ctaNum = ""
                                    msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(ren) + " requiere especificar el numero de cuenta o de contrato"
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If

                            End If
                            If Not array(ren, 3) Is Nothing Then
                                ctaCotit = array(ren, 3).ToString.Trim
                                If IsNumeric(ctaCotit) = False Then
                                    msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(ren) + " no contiene numero de cotitulares en formato numerico"
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                                ctaCotit = CInt(ctaCotit).ToString
                            Else
                                If exedente = "" And cheque <> "" Then
                                    ctaCotit = "0"
                                Else
                                    ctaCotit = ""
                                    msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(ren) + " requiere especificar el # de cotitulares"
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If

                            End If
                        ElseIf descrip = "COT" Then
                            If Not array(ren, 2) Is Nothing Then
                            cotNom = array(ren, 2).Trim.ToUpper.Replace("'", "''")
                        Else
                            cotNom = ""
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(ren) + " requiere especificar nombre completo del cotitular"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                        If Not array(ren, 3) Is Nothing Then
                            cotRfc = array(ren, 3).ToUpper.Trim.Replace(" ", "").Replace("-", "")
                            If Len(cotRfc) < 9 Or Len(cotRfc) > 13 Then
                                msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(ren) + " el tamaño de rfc debe ser de 9 a 13 caracteres"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            Dim expresion1, expresion2
                            expresion1 = "[A-Z,Ñ,&amp;]{4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?"
                            expresion2 = "[A-Z,Ñ,&amp;]{3}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?"
                            If Not Regex.IsMatch(cotRfc, expresion1) Then
                                If Not Regex.IsMatch(cotRfc, expresion2) Then
                                    msgErr = msgErr + ". " + vbCr + "Formato de rfc invalido en el renglon " + CStr(ren)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                            End If
                        Else
                            cotRfc = ""
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(ren) + " requiere especificar RFC del cotitular con o sin homoclave"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                        If Not array(ren, 4) Is Nothing Then
                            cotProporcion = array(ren, 4)
                            If IsNumeric(cotProporcion) = False Then
                                msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(ren) + " no contiene una proporción en formato numérico"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If CDbl(Replace(cotProporcion, ",", "")) < 0 Or CDbl(Replace(cotProporcion, ",", "")) > 100 Then
                                msgErr = msgErr + ". " + vbCr + "en el renglon " + CStr(ren) + "el porcentaje de proporción debe estar entre 0 y 100"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        Else
                            cotProporcion = ""
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(ren) + " requiere especificar el % de proporción proporción"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                        If Not array(ren, 5) Is Nothing Then
                            cotCurp = Left(array(ren, 5), 18)
                        Else
                            msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(ren) + " requiere la curp del cotitular"
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                        Dim expresion = "[A-Z][A,E,I,O,U,X][A-Z]{2}[0-9]{2}[0-1][0-9][0-3][0-9][M,H][A-Z]{2}[B,C,D,F,G,H,J,K,L,M,N,Ñ,P,Q,R,S,T,V,W,X,Y,Z]{3}[0-9,A-Z][0-9]"
                        If Not Regex.IsMatch(cotCurp, expresion) Then
                            msgErr = msgErr + ". " + vbCr + "Formato de curp invalido en el renglon " + CStr(ren)
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                    End If
siguiente:
                Next
etqErr:
                If ctrlErr = 1 Then
                    estado.Text = "VACIA"
                    myCommand2 = New SqlCommand("UPDATE ideAnual SET estado='VACIA' WHERE id=" + id.Text, myConnection)
                    myCommand2.ExecuteNonQuery()
                    lblErrImport.Visible = True
                    errImport.Visible = True
                    errImport.Text = msgErr
                    Response.Write("<script language='javascript'>alert('Detectamos errores, acepte para verlos');</script>")
                    Return 0
                Else
                    If msgErr <> "" Then
                        lblErrImport.Visible = True
                        errImport.Visible = True
                        errImport.Text = msgErr
                        Response.Write("<script language='javascript'>alert('Detectamos errores, acepte para verlos');</script>")
                        Return 0
                    Else
                        lblErrImport.Visible = False
                        errImport.Visible = False
                    End If

                End If
            End If

        Catch ex As Exception
            Dim MSG = "<script language='javascript'>alert('" + ex.StackTrace + "');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            errImport.Text = ex.Message + ". " + ex.StackTrace
            Return 0
        End Try


        Return 1
    End Function

    Private Function importarIDEanual() As Integer
        Dim objThread As Thread = CType(Session("Thread"), Thread)

        'Dim oldCI As System.Globalization.CultureInfo
        'oldCI = System.Threading.Thread.CurrentThread.CurrentCulture
        'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

        Try

            Dim excel As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application
            'Dim w As Workbook = excel.Workbooks.Open(savePath,,,,,,,,,,,,,, True)
            Dim w As Workbook = excel.Workbooks.Open(savePath)

            'For i As Integer = 1 To w.Sheets.Count
            Dim sheet As Worksheet = w.Sheets(1) 'i     'abrirá la 1er hoja del libro
            'xlHoja = xlApp.Worksheets(CStr(DatePart("m", mes.Value))) ' hojas: 1:12

            Dim r As Microsoft.Office.Interop.Excel.Range = sheet.UsedRange
            Dim array(,) As Object = r.Value(XlRangeValueDataType.xlRangeValueDefault)
            Dim nRensPre = sheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row 'sin rens en bco

            Session("barraN") = nRensPre

            w.Close(False)   'cierro excel y trabajo con la var
            excel.Quit()
            w = Nothing
            excel = Nothing

            If array IsNot Nothing Then
                Dim rens As Integer = nRensPre 'array.GetUpperBound(0)
                'Dim cols As Integer = array.GetUpperBound(1)

                Dim descrip, descripAnt, movFecha, movOper, movMonto, ctaNum, ctaCotit, ctaPropor, ctaIdeRec, nombres, ap1, ap2, razon, rfc, Dom, telefono1, telefono2, exedente, determinado, recaudado, pendienteRecaudar, numSocioCliente, sumaDeposEfe, ctaTipo, ctaTipoMoneda, curp, correo
                Dim q, idIdeDet, idContrib, ctaActual, ideDetAnualActual, cuentasIdeDetAnualActual, movMontoMN, cheque, mes, cotProporcion, cotRfc, cotNom, cotitularesCuentaActual, cotCurp

                Dim esInstitCredito, nombreFullVal
                q = "SELECT esInstitCredito,nombreFull FROM clientes WHERE id=" + Session("GidCliente").ToString
                myCommand = New SqlCommand(q, myConnection)
                dr = myCommand.ExecuteReader()
                dr.Read()
                If dr("esInstitCredito").Equals(True) Then
                    esInstitCredito = 1
                Else
                    esInstitCredito = 0
                End If
                If dr("nombreFull").Equals(True) Then
                    nombreFullVal = 1
                Else
                    nombreFullVal = 0
                End If
                dr.Close()

                'borra los registros del detalle via importacion anual p q no borre los de las mensuales debido a reimportaciones de la anual, los nuevos se agregan y los distintos se actualizan
                q = "DELETE FROM tCotitularAnual WHERE idCotitularesCuentaAnual IN (SELECT id FROM cotitularesCuentaAnual WHERE idideDetAnual IN (SELECT id FROM ideDetAnual WHERE idAnual=" + id.Text + "))"
                myCommand = New SqlCommand(q, myConnection)
                myCommand.ExecuteNonQuery()
                q = "DELETE FROM cotitularesCuentaAnual WHERE idideDetAnual IN (SELECT id FROM ideDetAnual WHERE idAnual=" + id.Text + ")"
                myCommand = New SqlCommand(q, myConnection)
                myCommand.ExecuteNonQuery()
                q = "DELETE FROM ideDetAnual WHERE idAnual=" + id.Text
                myCommand = New SqlCommand(q, myConnection)
                myCommand.ExecuteNonQuery()

                For ren As Integer = 6 To rens '1-5rens=encab 6o=datos
                    'For col As Integer = 1 To cols
                    If array(ren, 1) Is Nothing And array(ren, 2) Is Nothing And array(ren, 3) Is Nothing And array(ren, 4) Is Nothing And array(ren, 5) Is Nothing And array(ren, 6) Is Nothing And array(ren, 7) Is Nothing And array(ren, 8) Is Nothing And array(ren, 9) Is Nothing And array(ren, 10) Is Nothing And array(ren, 11) Is Nothing And array(ren, 12) Is Nothing And array(ren, 13) Is Nothing And array(ren, 14) Is Nothing Then ' ren bco
                        GoTo siguiente2
                    End If

                    If Not array(ren, 1) Is Nothing Then
                        descrip = Trim(UCase(array(ren, 1)))
                    End If

                    If descrip = "PER" Then
                        If Not array(ren, 2) Is Nothing Then
                            mes = array(ren, 2).ToString.ToUpper.Trim
                        End If
                        mes = CDbl(mes).ToString
                    ElseIf descrip = "CON" Then 'contribuyente
                        'movOper = Trim(UCase(array(ren, 2))) 'ACI,ahorroCorrienteInversion
                        If Not array(ren, 2) Is Nothing Then
                            If nombreFullVal = 0 Then
                                nombres = Left(array(ren, 2).ToString.ToUpper.Trim, 40).Replace("'", "''")
                            Else
                                nombres = Left(array(ren, 2).ToString.ToUpper.Trim, 120).Replace("'", "''")
                            End If
                        Else
                                nombres = ""
                        End If
                        If Not array(ren, 3) Is Nothing Then
                            If nombreFullVal = 0 Then
                                ap1 = Left(array(ren, 3).ToString.ToUpper.Trim, 40).Replace("'", "''")
                            End If
                        Else
                                ap1 = ""
                        End If
                        If Not array(ren, 4) Is Nothing Then
                            If nombreFullVal = 0 Then
                                ap2 = Left(array(ren, 4).ToString.ToUpper.Trim, 40).Replace("'", "''")
                            End If
                        Else
                                ap2 = ""
                        End If
                        If Not array(ren, 5) Is Nothing Then
                            razon = Left(array(ren, 5).ToString.ToUpper.Trim, 250).Replace("'", "''").Replace("&", "&amp;")
                        Else
                            razon = ""
                        End If

                        If Not array(ren, 6) Is Nothing Then

                            rfc = array(ren, 6).ToString.ToUpper.Trim.Replace(" ", "").Replace("-", "") 'array(ren, 6).ToString.ToUpper.Trim
                        Else
                            rfc = ""
                        End If
                        If Not array(ren, 7) Is Nothing Then
                            Dom = Left(array(ren, 7).ToString.ToUpper.Trim, 150).Replace("'", "''")
                            Dom = Regex.Replace(Dom, "\s+", " ")
                        Else
                            Dom = ""
                        End If
                        If Not array(ren, 8) Is Nothing Then
                            telefono1 = Regex.Replace(Left(array(ren, 8).ToString.ToUpper.Trim.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("'", ""), 15), "[a-zA-Z\s]+", "")
                        Else
                            telefono1 = ""
                        End If
                        If Not array(ren, 9) Is Nothing Then
                            telefono2 = Regex.Replace(Left(array(ren, 9).ToString.ToUpper.Trim.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("'", ""), 15), "[a-zA-Z\s]+", "")
                        Else
                            telefono2 = ""
                        End If
                        If Not array(ren, 10) Is Nothing Then
                            numSocioCliente = Left(array(ren, 10).ToString.ToUpper.Trim, 20)
                        Else
                            numSocioCliente = ""
                        End If
                        If Not array(ren, 11) Is Nothing Then
                            exedente = redondea(array(ren, 11)).ToString.Trim
                        Else
                            exedente = ""
                        End If
                        If Not array(ren, 12) Is Nothing Then
                            cheque = redondea(array(ren, 12)).ToString.Trim
                        Else
                            cheque = ""
                        End If
                        If Not array(ren, 13) Is Nothing Then
                            curp = Left(array(ren, 13).ToString.Trim, 18)
                        Else
                            curp = ""
                        End If
                        If Not array(ren, 14) Is Nothing Then
                            correo = Regex.Replace(Left(array(ren, 14).ToString.Trim, 50).Replace("'", "''").Replace("<", "").Replace(">", ""), "[^a-zA-Z0-9-_\.@]+", "")
                        Else
                            correo = ""
                        End If

                    ElseIf descrip = "CTA" Then
                        If Not array(ren, 2) Is Nothing Then
                            ctaNum = Left(array(ren, 2).ToString.ToUpper.Trim, 20)
                        Else
                            If exedente = "" And cheque <> "" Then
                                ctaNum = "9"
                            Else
                                ctaNum = ""
                            End If
                        End If
                        If Not array(ren, 3) Is Nothing Then
                            ctaCotit = array(ren, 3).ToString.Trim
                            ctaCotit = CInt(ctaCotit).ToString
                        Else
                            If exedente = "" And cheque <> "" Then
                                ctaCotit = "0"
                            Else
                                ctaCotit = ""
                            End If
                        End If
                    ElseIf descrip = "COT" Then
                        If Not array(ren, 2) Is Nothing Then
                            cotNom = array(ren, 2).Trim.ToUpper.Replace("'", "''")
                        Else
                            cotNom = ""
                        End If

                        If Not array(ren, 3) Is Nothing Then
                            cotRfc = array(ren, 3).ToUpper.Trim
                        End If

                        If Not array(ren, 4) Is Nothing Then
                            cotProporcion = array(ren, 4)
                        Else
                            cotProporcion = ""
                        End If

                        If Not array(ren, 5) Is Nothing Then
                            cotCurp = Left(array(ren, 5), 18)
                        End If
                    End If


                    If descrip = "CON" Then
                        q = "SELECT DISTINCT c.id, c.razonSocial FROM contribuyente c, ideAnual a, ideDetAnual idd WHERE c.rfc='" + rfc + "' and a.ejercicio='" + ejercicio.ToString + "' and idd.idAnual=a.id and idd.idContribuyente=c.id and a.id=" + id.Text
                        myCommand = New SqlCommand(q, myConnection)
                        dr = myCommand.ExecuteReader()
                        If dr.HasRows Then 'registro duplicado en la anual del cliente x imports en el archivo xls->reemplazarlo por el mas reciente
                            dr.Read()
                            idContrib = dr("id")
                            If dr("razonSocial") = "" Then
                                If nombreFullVal = 0 Then
                                    q = "UPDATE contribuyente SET nombres='" + nombres + "', ap1='" + ap1 + "', ap2='" + ap2 + "', numSocioCliente='" + numSocioCliente + "',Dom='" + Dom + "',telefono1='" + telefono1 + "',telefono2='" + telefono2 + "',correo='" + correo + "', curp='" + curp + "' WHERE id=" + idContrib.ToString
                                Else
                                    q = "UPDATE contribuyente SET nombreFull='" + nombres + "', numSocioCliente='" + numSocioCliente + "',Dom='" + Dom + "',telefono1='" + telefono1 + "',telefono2='" + telefono2 + "',correo='" + correo + "', curp='" + curp + "' WHERE id=" + idContrib.ToString
                                End If
                            Else
                                    q = "UPDATE contribuyente SET razonSocial='" + razon + "', numSocioCliente='" + numSocioCliente + "',Dom='" + Dom + "',telefono1='" + telefono1 + "',telefono2='" + telefono2 + "',correo='" + correo + "' WHERE id=" + idContrib.ToString
                            End If
                            dr.Close()

                            myCommand2 = New SqlCommand(q, myConnection)
                            myCommand2.ExecuteNonQuery()
                            'Response.Write("<script language='javascript'>alert('En el renglon " + CStr(ren) + " curp duplicada: " + curp + "');</script>")
                        Else    'nuevo registro
                            dr.Close()

                            'veo si existe en cat
                            q = "SELECT DISTINCT c.id, c.razonSocial FROM contribuyente c WHERE c.rfc='" + rfc + "'"
                            myCommand = New SqlCommand(q, myConnection)
                            dr = myCommand.ExecuteReader()
                            If Not dr.HasRows Then
                                dr.Close()
                                'errImport.Text = "INSERT INTO contribuyente(nombres,ap1,ap2,numSocioCliente,razonSocial,rfc,Dom,telefono1,telefono2,correo,curp) VALUES('" + nombres + "','" + ap1 + "','" + ap2 + "','" + numSocioCliente + "','" + razon + "','" + rfc + "','" + Dom + "','" + telefono1 + "','" + telefono2 + "','" + correo + "','" + curp + "')"
                                If nombreFullVal = 0 Then
                                    myCommand2 = New SqlCommand("INSERT INTO contribuyente(nombres,ap1,ap2,numSocioCliente,razonSocial,rfc,Dom,telefono1,telefono2,correo,curp) VALUES('" + nombres + "','" + ap1 + "','" + ap2 + "','" + numSocioCliente + "','" + razon + "','" + rfc + "','" + Dom + "','" + telefono1 + "','" + telefono2 + "','" + correo + "','" + curp + "')", myConnection)
                                Else
                                    myCommand2 = New SqlCommand("INSERT INTO contribuyente(nombreFull,numSocioCliente,razonSocial,rfc,Dom,telefono1,telefono2,correo,curp) VALUES('" + nombres + "','" + numSocioCliente + "','" + razon + "','" + rfc + "','" + Dom + "','" + telefono1 + "','" + telefono2 + "','" + correo + "','" + curp + "')", myConnection)
                                End If

                                myCommand2.ExecuteNonQuery()
                                    q = "SELECT TOP 1 id FROM contribuyente ORDER BY id DESC"
                                    myCommand = New SqlCommand(q, myConnection)
                                    dr = myCommand.ExecuteReader()
                                    dr.Read()
                                    idContrib = dr("id")
                                    dr.Close()

                                Else 'registro duplicado (llaves) x imports en el archivo xls->reemplazarlo por el mas reciente
                                    dr.Read()
                                idContrib = dr("id")
                                If dr("razonSocial") = "" Then
                                    If nombreFullVal = 0 Then
                                        q = "UPDATE contribuyente SET nombres='" + nombres + "', ap1='" + ap1 + "', ap2='" + ap2 + "', numSocioCliente='" + numSocioCliente + "',Dom='" + Dom + "',telefono1='" + telefono1 + "',telefono2='" + telefono2 + "',correo='" + correo + "', curp='" + curp + "' WHERE id=" + idContrib.ToString
                                    Else
                                        q = "UPDATE contribuyente SET nombreFull='" + nombres + "', numSocioCliente='" + numSocioCliente + "',Dom='" + Dom + "',telefono1='" + telefono1 + "',telefono2='" + telefono2 + "',correo='" + correo + "', curp='" + curp + "' WHERE id=" + idContrib.ToString
                                    End If
                                Else
                                        q = "UPDATE contribuyente SET razonSocial='" + razon + "', numSocioCliente='" + numSocioCliente + "',Dom='" + Dom + "',telefono1='" + telefono1 + "',telefono2='" + telefono2 + "',correo='" + correo + "' WHERE id=" + idContrib.ToString
                                End If
                                dr.Close()
                                myCommand2 = New SqlCommand(q, myConnection)
                                myCommand2.ExecuteNonQuery()
                                'Response.Write("<script language='javascript'>alert('En el renglon " + CStr(ren) + " curp duplicada: " + curp + "');</script>")
                            End If
                        End If

                        q = "SELECT d.id FROM ideDetAnual d, contribuyente c WHERE d.idContribuyente=c.id AND idAnual=" + id.Text + " AND c.id=" + idContrib.ToString + " AND d.periodo='" + mes.ToString + "'"
                        myCommand = New SqlCommand(q, myConnection)
                        dr = myCommand.ExecuteReader()
                        If dr.Read() Then 'registro duplicado (llaves) en el archivo->reemplazarlo por el mas reciente
                            idIdeDet = dr("id")
                            ideDetAnualActual = idIdeDet
                            dr.Close()

                            If esInstitCredito = 1 Then
                                If exedente <> "" Then
                                    q = "UPDATE ideDetAnual SET exedente='" + exedente + "' WHERE id=" + idIdeDet.ToString
                                    myCommand2 = New SqlCommand(q, myConnection)
                                    myCommand2.ExecuteNonQuery()
                                End If
                                If cheque <> "" Then
                                    q = "UPDATE ideDetAnual SET chqCajaMonto='" + cheque + "' WHERE id=" + idIdeDet.ToString
                                    myCommand2 = New SqlCommand(q, myConnection)
                                    myCommand2.ExecuteNonQuery()
                                End If
                            Else
                                q = "UPDATE ideDetAnual SET exedente='" + exedente + "' WHERE id=" + idIdeDet.ToString
                                myCommand2 = New SqlCommand(q, myConnection)
                                myCommand2.ExecuteNonQuery()

                                If cheque <> "" Then
                                    q = "UPDATE ideDetAnual SET chqCajaMonto='" + cheque + "' WHERE id=" + idIdeDet.ToString
                                    myCommand2 = New SqlCommand(q, myConnection)
                                    myCommand2.ExecuteNonQuery()
                                End If
                            End If
                        Else    'nuevo registro
                            dr.Close()

                            'If esInstitCredito = 1 Then
                            q = "INSERT INTO ideDetAnual(idAnual,idContribuyente, periodo"
                            If exedente <> "" Then
                                q = q + ",exedente"
                            End If
                            If cheque <> "" Then
                                q = q + ",chqCajaMonto"
                            End If
                            q = q + ") VALUES(" + id.Text + "," + idContrib.ToString + ",'" + mes + "'"
                            If exedente <> "" Then
                                q = q + ",'" + exedente + "'"
                            End If
                            If cheque <> "" Then
                                q = q + ",'" + cheque + "'"
                            End If
                            q = q + ")"
                            myCommand2 = New SqlCommand(q, myConnection)
                            myCommand2.ExecuteNonQuery()

                            q = "SELECT TOP 1 id FROM ideDetAnual ORDER BY id DESC"
                            myCommand = New SqlCommand(q, myConnection)
                            dr = myCommand.ExecuteReader()
                            dr.Read()
                            ideDetAnualActual = dr("id")
                            dr.Close()

                        End If
                    ElseIf descrip = "CTA" Then
                        'repetidos: update
                        Dim idCotitularesCuenta
                        q = "SELECT id FROM cotitularesCuentaAnual WHERE numeroCuenta='" + ctaNum + "' AND idideDetAnual =" + ideDetAnualActual.ToString
                        myCommand = New SqlCommand(q, myConnection)
                        dr = myCommand.ExecuteReader()
                        If dr.Read() Then
                            idCotitularesCuenta = dr("id")
                            cotitularesCuentaActual = idCotitularesCuenta
                            dr.Close()
                            q = "UPDATE cotitularesCuentaAnual SET numeroCotitulares='" + ctaCotit + "' WHERE numeroCuenta='" + ctaNum + "' AND idideDetAnual =" + ideDetAnualActual.ToString
                            myCommand2 = New SqlCommand(q, myConnection)
                            myCommand2.ExecuteNonQuery()
                        Else
                            dr.Close()
                            myCommand2 = New SqlCommand("INSERT INTO cotitularesCuentaAnual(numeroCuenta,numeroCotitulares,idideDetAnual) VALUES('" + ctaNum.ToString + "'," + ctaCotit.ToString + "," + ideDetAnualActual.ToString + ")", myConnection)
                            myCommand2.ExecuteNonQuery()

                            q = "SELECT TOP 1 id FROM cotitularesCuentaAnual ORDER BY id DESC"
                            myCommand = New SqlCommand(q, myConnection)
                            dr = myCommand.ExecuteReader()
                            dr.Read()
                            cotitularesCuentaActual = dr("id")
                            dr.Close()
                        End If

                    ElseIf descrip = "COT" Then
                        myCommand2 = New SqlCommand("INSERT INTO tcotitularAnual(idCotitularesCuentaAnual,nombreCompleto,rfc,proporcion,curp) VALUES(" + cotitularesCuentaActual.ToString + ",'" + cotNom + "','" + cotRfc + "','" + cotProporcion.ToString + "','" + cotCurp + "')", myConnection)
                        myCommand2.ExecuteNonQuery()
                    End If
siguiente2:
                    Session("barraIteracion") = Session("barraIteracion") + 1
                    'percent = Double.Parse(ren * 100 / rens).ToString("0")
                    'progressbar.Style("width") = percent + "px"
                Next

                Session("barraIteracion") = Session("barraN")
                myCommand2 = New SqlCommand("UPDATE ideAnual SET estado='IMPORTADA', idContrato=" + idContrato + ", viaImportacion=1 WHERE id=" + id.Text, myConnection)
                myCommand2.ExecuteNonQuery()

                Return 1
            Else
                objThread.Abort()
                Return 0
            End If
        Catch ex As Exception
            Session("error") = ex.Message
            Response.Write("<script language='javascript'>alert('" + ex.Message + "');</script>")
            statusImport.Text = Session("error") + ". Se procesaron exitosamente los primeros " + Session("barraIteracion").ToString + " registros"
            objThread.Abort()
            Return 0
        End Try
    End Function


    Protected Sub ver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ver.Click
        MultiView1.ActiveViewIndex = Int32.Parse(4)
        cargaGrid()
        progressbar1.Style("width") = "0px"
        statusImport.Text = ""
        descrip.Text = ""
        lblAvance.Text = ""
    End Sub

    Private Sub refrescaTotales()
        'actualiza la anual
        Dim q
        q = "SELECT SUM(exedente) AS sumaExedente, SUM(chqCajaMonto) AS sumaCheque, COUNT(*) as cuantos FROM ideDetAnual WHERE idAnual=" + Session("GidAnual").ToString
        myCommand2 = New SqlCommand(q, myConnection)
        dr = myCommand2.ExecuteReader()
        If dr.Read() Then
            q = "UPDATE ideAnual SET impteExcedente='" + dr("sumaExedente").ToString + "',impteCheque='" + dr("sumaCheque").ToString + "',nOpers=" + dr("cuantos").ToString + " WHERE id=" + Session("GidAnual").ToString
            myCommand = New SqlCommand(q, myConnection)
            myCommand.ExecuteNonQuery()
            If DBNull.Value.Equals(dr("sumaExedente")) Then
                impteExcedente.Text = FormatNumber(0, 0)
            Else
                impteExcedente.Text = FormatNumber(dr("sumaExedente"), 0)
            End If
            If DBNull.Value.Equals(dr("sumaCheque")) Then
                impteCheque.Text = FormatNumber(0, 0)
            Else
                impteCheque.Text = FormatNumber(dr("sumaCheque"), 0)
            End If
            nOpers.Text = FormatNumber(dr("cuantos"), 0)
        End If
        dr.Close()

    End Sub

    Private Function validar() As Integer
        If Request.QueryString("nc") = "C" Then 'complementaria
            If fechaPresentacion.Text.Trim <> "" Then
                Dim dtnow As DateTime
                Dim regDate As New System.Text.RegularExpressions.Regex("^\d{1,2}\/\d{1,2}\/\d{4}$")
                If regDate.IsMatch(fechaPresentacion.Text.Trim) Then
                    If Not DateTime.TryParse(fechaPresentacion.Text.Trim, dtnow) Then
                        fechaPresentacion.Focus()
                        descrip.Text = "fecha complementaria invalida"
                        Response.Write("<script language='javascript'>alert('fecha complementaria invalida');</script>")
                        Return 0
                    End If
                Else
                    fechaPresentacion.Focus()
                    descrip.Text = "fecha Complementaria formato de fecha no valido (dd/mm/aaaa)"
                    Response.Write("<script language='javascript'>alert('fecha Complementaria formato de fecha no valido (dd/mm/aaaa)');</script>")
                    Return 0
                End If
            Else
                fechaPresentacion.Text = Left(Now(), 10).ToString
            End If
        End If


        Return 1

    End Function

    Protected Sub mod_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMod.Click
        If id.Text = "0" Then
            descrip.Text = "Primero importe los datos o Crear en ceros"
            Response.Write("<script language='javascript'>alert('Primero importe los datos o Crear en ceros');</script>")
            Exit Sub
        End If

        If Request.QueryString("op") = "0" And Request.QueryString("subop") = "0" Then 'crear editar excel
            cargaGrid()
            progressbar1.Style("width") = "0px"
            statusImport.Text = ""
            descrip.Text = ""
            lblAvance.Text = ""
        End If

        If Request.QueryString("op") = "0" And CLng(nOpers.Text) > 0 Then 'no se valida para 0s o consulta, ni al crear/editar cuando se importaron 0 regs exitosam ya sea normal o complem.
            If validar() < 1 Then
                Exit Sub
            End If
        End If

        myCommand = New SqlCommand("UPDATE ideAnual SET guardadaUsuario=1 WHERE id=" + Session("GidAnual").ToString, myConnection)
        myCommand.ExecuteNonQuery()

        If Request.QueryString("op") = "0" And Request.QueryString("subop") = "0" Then 'crear editar excel
            Dim v = creaXMLaño()
            If v <> "" Then 'crea el zip del xml y lo copia a BD
                'descrip.Text = v
                Response.Write("<script language='javascript'>alert('" + v + "');</script>")
                Exit Sub
            End If
        End If

        Response.Write("<script language='javascript'>alert('Ahora puede enviar su declaración');</script>")
        descrip.Text = ""
    End Sub

    Private Sub bajaBDxmlMens()
        'bajar de la BD
        Dim myCommand2 As New SqlCommand
        With myCommand2
            .Connection = myConnection
            .CommandType = CommandType.StoredProcedure
            .CommandText = "ideAnualBajaxml"
            .Parameters.AddWithValue("@ID", id.Text)
            dr = .ExecuteReader()
            If Not dr Is Nothing Then
                If dr.Read Then
                    If DBNull.Value.Equals(dr("archivoXML")) Then
                        Response.Write("<script language='javascript'>alert('Aun no ha cargado el archivo xml');</script>")
                        dr.Close()
                        Exit Sub
                    End If
                    Dim writeStream As FileStream
                    'writeStream = New FileStream(System.AppDomain.CurrentDomain.BaseDirectory() + nomArchMensSinPath, FileMode.Create) 'p abrirlos en navegador
                    writeStream = New FileStream(nomArchMens, FileMode.Create)
                    Dim writeBinay As New BinaryWriter(writeStream)
                    writeBinay.Write(dr("archivoXML"))
                    writeBinay.Close()
                    'Response.Write("<script language='javascript'>window.open('" + nomArchMensSinPath + "');</script>")
                    'File.Delete(System.AppDomain.CurrentDomain.BaseDirectory() + nomArchMensSinPath)

                    System.Diagnostics.Process.Start(nomArchMens)
                    'File.Delete(nomArchMens)
                End If
            End If
        End With
    End Sub


    'Protected Sub verXml_Click(sender As Object, e As EventArgs) Handles verXml.Click
    '    progressbar.Style("width") = "0px"
    '    statusImport.Text = ""
    '    Call bajaBDxmlMens()
    'End Sub

    Protected Sub back_Click(ByVal sender As Object, ByVal e As EventArgs) Handles back.Click
        descrip.Text = ""
        Select Case Request.QueryString("op")
            Case "0" 'crear
                If Request.QueryString("subop") = "0" Then  'xls
                    MultiView1.ActiveViewIndex = Int32.Parse(0)
                ElseIf Request.QueryString("subop") = "1" Then  'xml
                    MultiView1.ActiveViewIndex = Int32.Parse(1)
                Else 'edit
                    MultiView1.ActiveViewIndex = Int32.Parse(2)
                End If
            Case "1" 'ceros
                MultiView1.ActiveViewIndex = Int32.Parse(3)
            Case "2" 'consultar
                If Request.QueryString("subop") = "0" Then  'xls
                    MultiView1.ActiveViewIndex = Int32.Parse(4)
                Else 'xml
                    MultiView1.ActiveViewIndex = Int32.Parse(5)
                End If
                cargaGrid()
            Case "3" 'via 12 mens
                MultiView1.ActiveViewIndex = Int32.Parse(6)

        End Select
    End Sub

    Private Sub enviarDeclaracion()
        Dim loginSAT, archivoLocal, directorioServidor, casfim, tipo, idArch, ipSAT, directorioSAT, archivoLocalSinDir
        Dim q = "SELECT loginSAT,directorioServidor,casfim,ipSAT,directorioSAT FROM clientes WHERE id=" + Session("GidCliente").ToString

        descrip.Text = ""

        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        loginSAT = dr("loginSAT")
        casfim = dr("casfim")
        directorioServidor = "C:\SAT\" + dr("directorioServidor")
        ipSAT = dr("ipSAT")
        directorioSAT = dr("directorioSAT")
        dr.Close()

        Dim elplan = Request.QueryString("pl")

        If normalComplementaria.Text = "NORMAL" Then
            tipo = "N"
            idArch = ""
        Else
            tipo = "C"
            idArch = id.Text
        End If
        If File.Exists(archivoLocal) Then
            Response.Write("<script language='javascript'>alert('Esta declaración ya se envió anteriormente, o no ha realizado Importación/Crear para este tipo de declaración, si su declaración es con datos pruebe importar nuevamente, si va a declarar en ceros puede importar un archivo de excel sin registros de detalle sino unicamente el encabezado');</script>")
            Exit Sub
        End If
        Dim fechaHora = Now().ToString("yyyy-MM-dd HH:mm:ss")
        Dim fechaHoraFmt = fechaHora.Replace(" ", "_").Replace(":", "-")
        nomArchMens = "C:\SAT\" + casfim + "\" + casfim + "-A-" + ejercicio.ToString + tipo + idArch + ".XML"
        nomArchMensSinPath = casfim + "-A-" + ejercicio.ToString + tipo + idArch + ".XML"
        archivoLocal = nomArchMens + ".ZIP"
        archivoLocalSinDir = nomArchMensSinPath + ".ZIP"

        If Not File.Exists(archivoLocal) Then
            Response.Write("<script language='javascript'>alert('Esta declaración ya se envió anteriormente, o no ha realizado Importación/Crear para este tipo de declaración, si su declaración es con datos pruebe importar nuevamente, si va a declarar en ceros puede importar un archivo de excel sin registros de detalle sino unicamente el encabezado');</script>")
            Exit Sub
        End If
        Dim nomArchMens2 = "C:\SAT\" + casfim + "\" + casfim + "-A-" + ejercicio.ToString + tipo + idArch + fechaHoraFmt + ".XML.ZIP"
        Dim nomArchMensSinPath2 = casfim + "-A-" + ejercicio.ToString + tipo + idArch + fechaHoraFmt + ".XML.ZIP"
        File.Copy(archivoLocal, nomArchMens2)


        'Dim nWnd As IntPtr
        'Dim ceroIntPtr As New IntPtr(0)
        'Dim Wnd_name As String
        'Dim hWnd As New IntPtr(0)
        'Dim hWnd2 As New IntPtr(0)
        'Dim hWnd3 As New IntPtr(0)
        'Dim hWnd4 As New IntPtr(0)
        'Dim hWnd5 As New IntPtr(0)
        'Dim hWnd6 As New IntPtr(0)
        'Dim hWnd7 As New IntPtr(0)

        'Dim WM_CLOSE = &H10
        'Dim WM_SETTEXT = &HC
        'Dim WM_GETTEXT = &HD
        'Dim BM_CLICK = &HF5

        'Dim retval As IntPtr
        'Dim retval2 As IntPtr
        'Dim retval3 As IntPtr
        'Dim retval4 As IntPtr
        'Dim retval5 As IntPtr
        'Dim retval6 As IntPtr
        'Dim retval7 As IntPtr

        Dim proceso As Process
        Dim p As New ProcessStartInfo("C:\SAT\Soky_nt_bank.exe") '("C:\SAT\TestAcuseVB.exe")
        p.Arguments = ipSAT + " " + loginSAT + " " + nomArchMens2 + " " + directorioSAT + "/" + nomArchMensSinPath2
        p.UseShellExecute = False
        p.RedirectStandardOutput = True
        Dim std_out As StreamReader
        Dim resultado As String = ""
        Try
            proceso = Process.Start(p)
            std_out = proceso.StandardOutput()
            proceso.WaitForExit()
            resultado = std_out.ReadToEnd
            std_out.Close()
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('" + ex.Message + "');</script>")
        End Try

        If InStr(resultado, "Transmision Correcta") = 0 Then '
            estado.Text = "ERROR_ENVIO"
            myCommand2 = New SqlCommand("UPDATE ideAnual SET estado='ERROR_ENVIO' WHERE id=" + id.Text, myConnection)
            myCommand2.ExecuteNonQuery()

            Dim elcorreo As New System.Net.Mail.MailMessage
            Using elcorreo
                elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                elcorreo.To.Add("declaracioneside@gmail.com")
                elcorreo.Subject = "Decl. anual ERROR_ENVIO"
                elcorreo.Body = "<html><body>cliente=" + Session("curCorreo") + ", ejercicio=" + ejercicio + ", error=" + resultado + "</body></html>"
                elcorreo.IsBodyHtml = True
                elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                Dim smpt As New System.Net.Mail.SmtpClient
                smpt.Host = "smtp.gmail.com"
                smpt.Port = "587"
                smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a")
                smpt.EnableSsl = True 'req p server gmail
                Try
                    smpt.Send(elcorreo)
                    elcorreo.Dispose()
                Catch ex As Exception
                    Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
                    Exit Sub
                Finally
                End Try
            End Using
            descrip.Text = "Error de transmisión en servidor del SAT, notifiquelo al proveedor o espere a que sea restablecido el servidor del SAT"
            'descrip.Text = resultado
            'Response.Write("<script language='javascript'>alert('Error de transmisión en servidor del SAT, notifiquelo al proveedor o espere a que sea restablecido el servidor del SAT: " + resultado + "');</script>")
        Else
            If elplan <> "PREMIUM" Then
                q = "UPDATE contratos SET nDeclHechas=nDeclHechas+1 WHERE id=" + Session("GidContrato").ToString
                myCommand = New SqlCommand(q, myConnection)
                myCommand.ExecuteNonQuery()
            End If

            estado.Text = "ACEPTADA"
            fechaEnvio.Text = fechaHora
            myCommand2 = New SqlCommand("UPDATE ideAnual SET estado='ACEPTADA', fechaEnvio='" + fechaEnvio.Text + "', acuseSolicitado=0, evidEnvio='" + resultado + "' WHERE id=" + id.Text, myConnection)
            myCommand2.ExecuteNonQuery()

            'Response.Write("<script language='javascript'>alert('Envio exitoso');</script>")
            descrip.Text = resultado
            Dim MSG As String = "<script language='javascript'>alert('" + resultado + "');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)

            Dim elcorreo As New System.Net.Mail.MailMessage
            Using elcorreo
                elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                If chkSinCorreo.Checked.Equals(True) Then
                    elcorreo.To.Add("declaracioneside@gmail.com")
                Else
                    elcorreo.To.Add(Session("curCorreo"))
                End If
                elcorreo.Subject = "Declaración Anual Ejercicio " + ejercicio + ", constancia de envío"
                elcorreo.Body = "<html><body>Evidencia de envío: <br/><br/>" + resultado + "<br/><br/>Favor de conservar este correo para rastreo de acuses en caso necesario; En cuanto el SAT deposite el acuse en nuestros servidores, podrá descargarlo de nuestra página o bien si después de 3 dias hábiles no lo puede bajar, solicítelo a este correo y le será enviado en caso de ya haberlo recibido del SAT, Saludos. <br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet<br>Tel 01 443 690 3616<br>Correo declaracioneside@gmail.com<br><a href='https://twitter.com/declaracionesid' target='_blank'><img src='declaracioneside.com/twitter.jpg' alt='Clic aquí, siguenos en twitter' Height='30px' Width='30px' BorderWidth='0px'></a>&nbsp;<a href='http://www.youtube.com/user/declaracioneside' target='_blank'><img src='declaracioneside.com/iconoyoutube.png' alt='Suscribete a nuestro canal declaraciones de depósitos en efectivo e IDE en youtube' Height='30px' Width='30px' BorderWidth='0px'></a> &nbsp;<a href='http://www.facebook.com/depositosenefectivo' target='_blank'><img src='declaracioneside.com/facebook.jpg' alt='Clic aquí para seguirnos en facebook' Height='30px' Width='30px' BorderWidth='0px'></a>&nbsp;&nbsp;<a href='https://mx.linkedin.com/in/declaraciones-depósitos-en-efectivo-1110125b' target='_blank'><img src='declaracioneside.com/linkedin.png' alt='Siguenos en linkedin' Height='30px' Width='30px' BorderWidth='0px'></a>&nbsp;<a href='http://plus.google.com/107594546767340388428?prsrc=3'><img src='http://ssl.gstatic.com/images/icons/gplus-32.png' alt='Google+' Height='30px' Width='30px' BorderWidth='0px'></a><br/></body></html>"
                elcorreo.IsBodyHtml = True
                elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                Dim smpt As New System.Net.Mail.SmtpClient
                smpt.Host = "smtp.gmail.com"
                smpt.Port = "587"
                smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a")
                smpt.EnableSsl = True 'req p server gmail
                Try
                    smpt.Send(elcorreo)
                    elcorreo.Dispose()
                Catch ex As Exception
                    Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
                    Exit Sub
                Finally
                End Try
            End Using

        End If
        'File.Delete(nomArchMens2)

        'Dim p As New ProcessStartInfo
        'p.FileName = "C:\SAT\TestIDE.exe" '"C:\SAT\TestAcuseVB.exe"
        'p.WindowStyle = ProcessWindowStyle.Normal
        'Process.Start(p)
        'System.Threading.Thread.Sleep(1000)

        'Wnd_name = "Declaraciones y Acuses IDE ver 3.0"
        ''Wnd_name = "Form1" 'antes correr el hostProy.exe hasta q quede abierta la form 
        'nWnd = FindWindow(Nothing, Wnd_name)
        ''hWnd6 = FindWindowEx(nWnd, IntPtr.Zero, "ThunderRT6CommandButton", "cancel") 'cancel
        ''If Not hWnd6.Equals(ceroIntPtr) Then
        ''    SetActiveWindow(nWnd)
        ''    retval6 = SendNotifyMessage(hWnd6, BM_CLICK, IntPtr.Zero, 0)
        ''End If

        'If nWnd.Equals(ceroIntPtr) Then
        '    descrip.Text = "Aplicación de hacienda no se lanzó"
        '    'Response.Write("<script language='javascript'>alert('Aplicación de hacienda no se lanzó');</script>")
        'Else
        '    hWnd = FindWindowEx(nWnd, IntPtr.Zero, "ThunderRT6TextBox", "")    'acuse: ruta/repositorio
        '    If Not hWnd.Equals(ceroIntPtr) Then
        '        hWnd2 = FindWindowEx(nWnd, hWnd, "ThunderRT6TextBox", "")      'acuse: cuenta sat (login remoto )
        '        If Not hWnd2.Equals(ceroIntPtr) Then
        '            hWnd3 = FindWindowEx(nWnd, hWnd2, "ThunderRT6TextBox", "")     'resultados del comando
        '            If Not hWnd3.Equals(ceroIntPtr) Then
        '                hWnd4 = FindWindowEx(nWnd, hWnd3, "ThunderRT6TextBox", "") 'tx: login remoto 
        '                If Not hWnd4.Equals(ceroIntPtr) Then
        '                    retval4 = SendMessage(hWnd4, WM_SETTEXT, IntPtr.Zero, loginSAT) 'loginSAT
        '                    hWnd5 = FindWindowEx(nWnd, hWnd4, "ThunderRT6TextBox", "") 'tx: archivo local
        '                    If Not hWnd5.Equals(ceroIntPtr) Then
        '                        retval5 = SendMessage(hWnd5, WM_SETTEXT, IntPtr.Zero, archivoLocal)
        '                        SetActiveWindow(nWnd)
        '                        hWnd6 = FindWindowEx(nWnd, IntPtr.Zero, "ThunderRT6CommandButton", "&Procesar") 'Procesar (&Subrayado)
        '                        If Not hWnd6.Equals(ceroIntPtr) Then
        '                            retval6 = SendNotifyMessage(hWnd6, BM_CLICK, IntPtr.Zero, 0)   'HABILITAR CLIC TX
        '                            Dim Handle As IntPtr = Marshal.AllocHGlobal(500)
        '                            Dim resultado As String
        '                            Dim numText As IntPtr
        '                            Dim tam As IntPtr
        '                            tam = 500
        '                            Do
        '                                numText = SendMessage(hWnd3, WM_GETTEXT, tam, Handle)    'resultados del comando                                        
        '                                resultado = Marshal.PtrToStringUni(Handle)
        '                            Loop While resultado.Equals("")     'vs tiempo fijo
        '                            If InStr(resultado, "ERROR") Or InStr(resultado, "FALLA") Or InStr(resultado, "Falla") Or InStr(resultado, "Atencion") Or InStr(resultado, "errno") Then 'o distinto de OK
        '                                estado.Text = "ERROR_ENVIO"
        '                                myCommand2 = New SqlCommand("UPDATE ideAnual SET estado='ERROR_ENVIO' WHERE id=" + id.Text, myConnection)
        '                                myCommand2.ExecuteNonQuery()

        '                                Dim elcorreo As New System.Net.Mail.MailMessage
        '                                Using elcorreo
        '                                    elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
        '                                    elcorreo.To.Add("declaracioneside@gmail.com")
        '                                    elcorreo.Subject = "Decl. anual ERROR_ENVIO"
        '                                    elcorreo.Body = "<html><body>cliente=" + session("curCorreo") + ", ejercicio=" + ejercicio + ", error=" + resultado + "</body></html>"
        '                                    elcorreo.IsBodyHtml = True
        '                                    elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        '                                    Dim smpt As New System.Net.Mail.SmtpClient
        '                                    smpt.Host = "smtp.gmail.com"
        '                                    smpt.Port = "587"
        '                                    smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a")
        '                                    smpt.EnableSsl = True 'req p server gmail
        '                                    Try
        '                                        smpt.Send(elcorreo)
        '                                        elcorreo.Dispose()
        '                                    Catch ex As Exception
        '                                        Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
        '                                        Exit Sub
        '                                    Finally
        '                                    End Try
        '                                End Using
        '                                descrip.Text = "Error de transmisión en servidor del SAT, notifiquelo al proveedor o espere a que sea restablecido el servidor del SAT"
        '                                'Response.Write("<script language='javascript'>alert('Error de transmisión en servidor del SAT, notifiquelo al proveedor o espere a que sea restablecido el servidor del SAT: " + resultado + "');</script>")
        '                            Else
        '                                File.Delete(archivoLocal)   'borro el zip        
        '                                If elplan <> "PREMIUM" Then
        '                                    q = "UPDATE contratos SET nDeclHechas=nDeclHechas+1 WHERE id=" + session("GidContrato").ToString
        '                                    myCommand = New SqlCommand(q, myConnection)
        '                                    myCommand.ExecuteNonQuery()
        '                                End If

        '                                estado.Text = "ACEPTADA"
        '                                myCommand2 = New SqlCommand("UPDATE ideAnual SET estado='ACEPTADA' WHERE id=" + id.Text, myConnection)
        '                                myCommand2.ExecuteNonQuery()

        '                                'Response.Write("<script language='javascript'>alert('Envio exitoso');</script>")
        '                                descrip.Text = "Envio exitoso"
        '                            End If
        '                            retval = SendMessage(nWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero)    'cerrar
        '                        Else
        '                            'Response.Write("<script language='javascript'>alert('Componente Procesar no localizado');</script>")
        '                            descrip.Text = "Componente Enviar declaracion/Procesar no localizado"
        '                        End If
        '                    Else
        '                        'Response.Write("<script language='javascript'>alert('Componente Archivo no localizado');</script>")
        '                        descrip.Text = "Componente archivo declaracion / archivo local no localizado"

        '                    End If
        '                Else
        '                    'Response.Write("<script language='javascript'>alert('Componente login transmisor no localizado');</script>")
        '                    descrip.Text = "Componente cuenta sat / login transmisor no localizado"
        '                End If
        '            Else
        '                descrip.Text = "Componente resultados no localizado"
        '            End If
        '        Else
        '            'Response.Write("<script language='javascript'>alert('Componente login acuses no localizado');</script>")
        '            descrip.Text = "Componente cuenta sat / login acuses no localizado"
        '        End If
        '        Else
        '            'Response.Write("<script language='javascript'>alert('Componente directorio no localizado');</script>")
        '            descrip.Text = "Componente repositorio/directorio acuses no localizado"
        '        End If
        'End If

    End Sub

    Private Function validaModificada()
        Dim q = "SELECT guardadaUsuario FROM ideAnual WHERE id=" + Session("GidAnual").ToString
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            If dr("guardadaUsuario").Equals(False) Then
                dr.Close()
                Response.Write("<script language='javascript'>alert('1o de clic en Validar');</script>")
                Return 0
            End If
        Else
            dr.Close()
            Response.Write("<script language='javascript'>alert('1o importe los datos o creela en ceros');</script>")
            Return 0
        End If
        dr.Close()

        Return 1
    End Function

    Protected Sub enviarDeclaracionExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles enviarDeclaracionExcel.Click
        Dim q
        q = "SELECT id, loginSAT FROM clientes cli WHERE cli.correo='" + Session("curCorreo") + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        If DBNull.Value.Equals(dr("loginSAT")) Then
            dr.Close()
            Response.Write("<script language='javascript'>alert('El SAT aún no le asigna una cuenta, este proceso tarda de 1 a 2 semanas aprox. desde su 1er contrato con nosotros');</script>")
            Exit Sub
        End If
        dr.Close()

        'xml
        If (Request.QueryString("op") = "0" And Request.QueryString("subop") = "1") Or (Request.QueryString("op") = "2" And Request.QueryString("subop") = "1") Then
            If estado.Text = "VACIA" Then
                Response.Write("<script language='javascript'>alert('No ha importado su archivo xml ninguna vez');</script>")
                Exit Sub
            End If
            '0s
        ElseIf Request.QueryString("op") = "1" Or (Request.QueryString("op") = "2" And pl = "CEROS") Then  '0s
            If id.Text = "0" Or estado.Text = "VACIA" Then
                Response.Write("<script language='javascript'>alert('1o haga clic en crear');</script>")
                Exit Sub
            End If
        End If

        'excel o xml
        If (Request.QueryString("op") = "0" And Request.QueryString("subop") = "1") Or (Request.QueryString("op") = "2" And Request.QueryString("subop") = "1") Or (Request.QueryString("op") = "0" And Request.QueryString("subop") = "0") Or (Request.QueryString("op") = "2" And Request.QueryString("subop") = "0") Then
            If validaModificada() < 1 Then
                Exit Sub
            End If

            progressbar1.Style("width") = "0px"
            statusImport.Text = ""
        End If

        Dim contra
        Dim fechaDeclarar = Convert.ToDateTime(Trim("01/01/" + ejercicio.ToString)).ToString("yyyy-MM-dd")
        If pl.ToString = "PREMIUM" Then
            q = "SELECT periodoInicial, fechaFinal FROM contratos WHERE id=" + idContrato.ToString
            myCommand = New SqlCommand(q, myConnection)
            dr = myCommand.ExecuteReader()
            contra = dr.Read()
            If (Left(dr("periodoInicial"), 4) <> Left(dr("fechaFinal"), 4)) Then 'cambio de año
                fechaDeclarar = DateAdd(DateInterval.Year, 1, CDate(fechaDeclarar)).ToString("yyyy-MM-dd")
            End If
            dr.Close()
        End If

        q = "SELECT co.id, pla.elplan FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND co.id=" + Session("GidContrato").ToString + " AND ( (nDeclHechas < nDeclContratadas and (pla.elplan<>'PREMIUM')) or (('" + Convert.ToDateTime(fechaDeclarar).ToString("yyyy-MM-dd") + "' >= periodoInicial and pla.elplan='PREMIUM' and co.anualEnPremium=1) and ('" + Convert.ToDateTime(fechaDeclarar).ToString("yyyy-MM-dd") + "' <= fechaFinal and pla.elplan='PREMIUM' and co.anualEnPremium=1) ) ) order by co.id"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If (Not dr.HasRows) Then 'sin contrato vigente 
            Response.Write("<script language='javascript'>alert('A alcanzado el máximo de declaraciones contratadas o bien ha caducado su contrato, o el periodo no está cubierto en este contrato');</script>")
            Exit Sub
        End If
        dr.Close()


        controlaAcceso()
        If redir.Text = "1" Then
            Exit Sub
        End If

        Call enviarDeclaracion()
    End Sub

    Private Sub extraeNumoperDeAcuse(ByVal allRead As String)
        Dim pos, pos2, tam, numOperV, fechaPresentacionV, q, rfcV, denominacionV, recaudadoV, ejercicioV, tipoV, folioV, archivoV, selloV

        pos = allRead.IndexOf("fechaPresentacion")
        'pos2 = allRead.IndexOf("""", pos)
        'tam = pos2 - pos + 1
        fechaPresentacionV = allRead.Substring(pos + 19, 10)

        pos = allRead.IndexOf("numeroOperacion")
        pos2 = allRead.IndexOf("""", pos + 17)
        tam = pos2 - pos - 17
        numOperV = allRead.Substring(pos + 17, tam)

        q = "UPDATE ideAnual SET numOper='" + numOperV + "', fechaPresentacion='" + Convert.ToDateTime(fechaPresentacionV).ToString("yyyy-MM-dd") + "' WHERE id=" + id.Text
        myCommand = New SqlCommand(q, myConnection)
        myCommand.ExecuteNonQuery()

        numOper.Text = numOperV
        fechaPresentacion.Text = fechaPresentacionV

        'tomando el resto para acusePDF
        pos = allRead.IndexOf("rfc")
        pos2 = allRead.IndexOf("""", pos + 5)
        tam = pos2 - pos - 5
        rfcV = allRead.Substring(pos + 5, tam)

        pos = allRead.IndexOf("denominacion")
        pos2 = allRead.IndexOf("""", pos + 14)
        tam = pos2 - pos - 14
        denominacionV = SecurityElement.Escape(allRead.Substring(pos + 14, tam))

        pos = allRead.IndexOf("folioRecepcion")
        pos2 = allRead.IndexOf("""", pos + 16)
        tam = pos2 - pos - 16
        folioV = allRead.Substring(pos + 16, tam)

        pos = allRead.IndexOf("nombreArchivo")
        pos2 = allRead.IndexOf("""", pos + 15)
        tam = pos2 - pos - 15
        archivoV = allRead.Substring(pos + 15, tam)

        pos = allRead.IndexOf("ejercicio")
        pos2 = allRead.IndexOf("""", pos + 11)
        tam = pos2 - pos - 11
        ejercicioV = allRead.Substring(pos + 11, tam)

        pos = allRead.IndexOf("tipo")
        pos2 = allRead.IndexOf("""", pos + 6)
        tam = pos2 - pos - 6
        tipoV = allRead.Substring(pos + 6, tam)

        pos = allRead.IndexOf("totalRecaudado")
        pos2 = allRead.IndexOf("""", pos + 16)
        tam = pos2 - pos - 16
        recaudadoV = allRead.Substring(pos + 16, tam)

        pos = allRead.IndexOf("sello")
        pos2 = allRead.IndexOf("""", pos + 7)
        tam = pos2 - pos - 7
        selloV = allRead.Substring(pos + 7, tam)

        Session("numOperAcuse") = numOperV
        Session("fechaPresentacionAcuse") = fechaPresentacionV
        Session("rfcAcuse") = rfcV
        Session("denominacionAcuse") = denominacionV
        Session("recaudadoAcuse") = recaudadoV
        Session("ejercicioAcuse") = ejercicioV
        Session("tipoAcuse") = tipoV
        Session("folioAcuse") = folioV
        Session("archivoAcuse") = archivoV
        Session("selloAcuse") = selloV
    End Sub

    Private Sub bajarAcuse()
        progressbar1.Style("width") = "0px"
        statusImport.Text = ""
        descrip.Text = ""
        Dim qAcuseSolicitado, qFechaEnvio
        Dim q = "SELECT id,estado,acuseSolicitado,fechaEnvio FROM ideAnual WHERE id='" + id.Text + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If Not dr.Read() Then
            Response.Write("<script language='javascript'>alert('No se ha producido información para esta declaración ya sea vía importación o en ceros');</script>")
            dr.Close()
            Exit Sub
        ElseIf dr("estado") <> "ACEPTADA" Then
            Response.Write("<script language='javascript'>alert('1o necesita Enviar satisfactoriamente esta declaración o bien bajar y presentar la contingencia');</script>")
            dr.Close()
            Exit Sub
        End If
        qAcuseSolicitado = dr("acuseSolicitado")
        qFechaEnvio = dr("fechaEnvio")
        dr.Close()

        Dim loginSAT, directorioServidor, casfim, tipo, idArch, razonSoc
        q = "SELECT loginSAT,directorioServidor,casfim,razonSoc FROM clientes WHERE id=" + Session("GidCliente").ToString
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        loginSAT = dr("loginSAT")
        directorioServidor = "C:\SAT\" + dr("directorioServidor")
        casfim = dr("casfim")
        razonSoc = dr("razonSoc")
        dr.Close()
        If normalComplementaria.Text = "NORMAL" Then
            tipo = "N"
            idArch = ""
        Else
            tipo = "C"
            idArch = id.Text
        End If
        nomArchMens = "C:\SAT\" + casfim + "\" + casfim + "-" + "A-" + ejercicio.ToString + tipo + idArch + ".XML"
        nomArchMensSinPath = casfim + "-" + "A-" + ejercicio.ToString + tipo + idArch + ".XML"
        Dim fechaHoraFmt
        fechaHoraFmt = CDate(qFechaEnvio).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "_").Replace(":", "-")
        If CDate(qFechaEnvio).ToString("yyyy-MM-dd") >= "2017-03-15" And CDate(qFechaEnvio).ToString("yyyy-MM-dd") < "2021-01-01" Then  'cambio de nomenclatura de archivos
            nomArchMens = "C:\SAT\" + casfim + "\" + "A-" + ejercicio.ToString + tipo + idArch + fechaHoraFmt + ".XML"
            nomArchMensSinPath = "A-" + ejercicio.ToString + tipo + idArch + fechaHoraFmt + ".XML"
        ElseIf CDate(qFechaEnvio).ToString("yyyy-MM-dd") >= "2021-01-01" Then
            nomArchMens = "C:\SAT\" + casfim + "\" + casfim + "-" + "A-" + ejercicio.ToString + tipo + idArch + fechaHoraFmt + ".XML"
            nomArchMensSinPath = casfim + "-" + "A-" + ejercicio.ToString + tipo + idArch + fechaHoraFmt + ".XML"
        End If

        'Dim nWnd As IntPtr
        'Dim ceroIntPtr As New IntPtr(0)
        'Dim Wnd_name As String
        'Dim hWnd As New IntPtr(0)
        'Dim hWnd2 As New IntPtr(0)
        'Dim hWnd3 As New IntPtr(0)
        'Dim hWnd4 As New IntPtr(0)
        'Dim hWnd5 As New IntPtr(0)
        'Dim hWnd6 As New IntPtr(0)
        'Dim hWnd7 As New IntPtr(0)

        'Dim WM_CLOSE = &H10
        'Dim WM_SETTEXT = &HC
        'Dim WM_GETTEXT = &HD
        'Dim BM_CLICK = &HF5

        'Dim retval As IntPtr
        'Dim retval2 As IntPtr
        'Dim retval3 As IntPtr
        'Dim retval4 As IntPtr
        'Dim retval5 As IntPtr
        'Dim retval6 As IntPtr
        'Dim retval7 As IntPtr

        'Dim p As New ProcessStartInfo
        'p.FileName = "C:\SAT\TestAcuseVB.exe"
        'p.WindowStyle = ProcessWindowStyle.Normal
        'Process.Start(p)
        'System.Threading.Thread.Sleep(1000)

        'Wnd_name = "Declaraciones y Acuses IDE ver 3.0"
        ''Wnd_name = "Form1" 'antes correr el hostProy.exe hasta q quede abierta la form 
        'nWnd = FindWindow(Nothing, Wnd_name)
        ''hWnd6 = FindWindowEx(nWnd, IntPtr.Zero, "ThunderRT6CommandButton", "cancel") 'cancel
        ''If Not hWnd6.Equals(ceroIntPtr) Then
        ''    SetActiveWindow(nWnd)
        ''    retval6 = SendNotifyMessage(hWnd6, BM_CLICK, IntPtr.Zero, 0)
        ''End If

        'If nWnd.Equals(ceroIntPtr) Then
        '    'Response.Write("<script language='javascript'>alert('Aplicación de hacienda no se lanzó');</script>")
        '    descrip.Text = "Aplicación de hacienda no se lanzó"
        'Else
        '    hWnd = FindWindowEx(nWnd, IntPtr.Zero, "ThunderRT6TextBox", "")    'acuse: ruta/repositorio
        '    If Not hWnd.Equals(ceroIntPtr) Then
        '        retval = SendMessage(hWnd, WM_SETTEXT, IntPtr.Zero, directorioServidor)
        '        hWnd2 = FindWindowEx(nWnd, hWnd, "ThunderRT6TextBox", "")      'acuse: login remoto 
        '        If Not hWnd2.Equals(ceroIntPtr) Then
        '            retval2 = SendMessage(hWnd2, WM_SETTEXT, IntPtr.Zero, loginSAT)
        '            hWnd3 = FindWindowEx(nWnd, hWnd2, "ThunderRT6TextBox", "")     'resultados del comando
        '            If Not hWnd3.Equals(ceroIntPtr) Then
        '                SetActiveWindow(nWnd)
        '                hWnd7 = FindWindowEx(nWnd, IntPtr.Zero, "ThunderRT6CommandButton", "Acuses") 'Acuses
        '                If Not hWnd7.Equals(ceroIntPtr) Then
        '                    retval7 = SendNotifyMessage(hWnd7, BM_CLICK, IntPtr.Zero, 0)   'HABILITAR CLIC RX
        '                    Dim Handle As IntPtr = Marshal.AllocHGlobal(500)
        '                    Dim resultado As String
        '                    Dim numText As IntPtr
        '                    Dim tam As IntPtr
        '                    tam = 500
        '                    Do
        '                        numText = SendMessage(hWnd3, WM_GETTEXT, tam, Handle)    'resultados del comando                                        
        '                        resultado = Marshal.PtrToStringUni(Handle)
        '                    Loop While resultado.Equals("")     'vs tiempo fijo
        '                    If InStr(resultado, "ERROR") Or InStr(resultado, "FALLA") Or InStr(resultado, "Falla") Or InStr(resultado, "Atencion") Or InStr(resultado, "errno") Then 'o distinto de Exito

        '                        Dim elcorreo As New System.Net.Mail.MailMessage
        '                        Using elcorreo
        '                            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
        '                            elcorreo.To.Add("declaracioneside@gmail.com")
        '                            elcorreo.Subject = "Decl. anual ERROR_ACUSE"
        '                            elcorreo.Body = "<html><body>cliente=" + session("curCorreo") + ", ejercicio=" + ejercicio + ", error=" + resultado + "</body></html>"
        '                            elcorreo.IsBodyHtml = True
        '                            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        '                            Dim smpt As New System.Net.Mail.SmtpClient
        '                            smpt.Host = "smtp.gmail.com"
        '                            smpt.Port = "587"
        '                            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a")
        '                            smpt.EnableSsl = True 'req p server gmail
        '                            Try
        '                                smpt.Send(elcorreo)
        '                                elcorreo.Dispose()
        '                            Catch ex As Exception
        '                                Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
        '                                Exit Sub
        '                            Finally
        '                            End Try
        '                        End Using
        '                        descrip.Text = "Error bajando acuses, notifiquelo al proveedor o espere a que sea restablecido el servidor del SAT. "

        '                    Else

        '                    End If

        'creando comprimido con acuses y enviandolo por correo
        If File.Exists(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP") Then 'sin la ext .xml
            File.Delete(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP")
        End If

        Dim MSG As String
        Dim di As New IO.DirectoryInfo(directorioServidor + "\")
        Dim diar1 As IO.FileInfo() = di.GetFiles("*.xml")
        Dim dra As IO.FileInfo
        Dim fName As String
        Dim allRead As String
        Dim regMatch As String 'string to search for inside of text file. It is case sensitive.
        regMatch = nomArchMensSinPath  'buscando el nomArchMensSinPath como texto dentro del archivo
        Try
            Using zip As ZipFile = New ZipFile
                Dim c As Integer, archPdf As String, listaAcuses As New List(Of String)(), cont As Integer
                c = 0
                cont = 0
                For Each dra In diar1   'busca aceptaciones y rechachazos del archivo
                    fName = dra.FullName 'path to text file
                    Dim testTxt As StreamReader = New StreamReader(fName)
                    allRead = testTxt.ReadToEnd() 'Reads the whole text file to the end
                    testTxt.Close() 'Closes the text file after it is fully read.
                    If (Regex.IsMatch(allRead, regMatch)) Then 'If match found in allRead
                        zip.AddFile(fName, "")
                        If Left(dra.Name, 2) = "AA" Then 'acuse aceptado, solo esos traen numOper y fechaPresentacion
                            extraeNumoperDeAcuse(allRead)
                            'borro movs de decls con acuse de aceptacion, ya se aceptó y ya no están disp pal user y se libera espacio
                            myCommand = New SqlCommand("DELETE FROM mov WHERE idCuentasIdeDetAnual IN (SELECT id FROM cuentasIdeDetAnual WHERE idideDetAnual IN (SELECT id FROM ideDetAnual WHERE idAnual=" + id.Text + "))", myConnection)
                            myCommand.ExecuteNonQuery()

                            archPdf = acusePdf("A", dra.DirectoryName, dra.Name, casfim) 'aceptado, ruta, nombre
                        Else 'solo trae fechaPresentacion y archivo
                            archPdf = acusePdf("R", dra.DirectoryName, dra.Name, casfim) 'rechazado
                        End If
                        zip.AddFile(archPdf, "")

                        c = 1
                        listaAcuses.Add(archPdf)
                        cont = cont + 1
                    End If
                Next
                If c = 0 Then
                    'Response.Write("<script language='javascript'>alert('No se encontraron acuses para este periodo');</script>")
                    descrip.Text = descrip.Text + "No se encontraron aún acuses para este periodo"
                    Dim nulo
                    If DBNull.Value.Equals(qAcuseSolicitado) Then
                        nulo = True
                    Else
                        nulo = False
                    End If

                    If ((nulo = False And qAcuseSolicitado.Equals(False)) Or nulo = True) And DateDiff("h", CDate(qFechaEnvio), CDate(Now)) > 23 Then '24hrs
                        Dim elcorreo As New System.Net.Mail.MailMessage
                        Using elcorreo
                            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                            elcorreo.To.Add("armando.delatorre@sat.gob.mx")
                            elcorreo.CC.Add("miguel.chantes@sat.gob.mx")
                            elcorreo.CC.Add("declaracioneside@gmail.com")
                            elcorreo.Subject = "Solicitud de acuses"
                            elcorreo.Body = "<html><body>Buen dia<br><br>Nos podría proporcionar los acuses de la declaración anual " + ejercicio.ToString + " de " + razonSoc + ", casfim " + casfim + ", Enviado en la fecha (año-mes-dia): " + CDate(qFechaEnvio).ToString("yyyy-MM-dd") + ", en el archivo " + nomArchMensSinPath + ".ZIP" + " <br><br>Atentamente, <a href='declaracioneside.com'>declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet </body></html>"
                            elcorreo.IsBodyHtml = True
                            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                            Dim smpt As New System.Net.Mail.SmtpClient
                            smpt.Host = "smtp.gmail.com"
                            smpt.Port = "587"
                            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a")
                            smpt.EnableSsl = True 'req p server gmail
                            Try
                                smpt.Send(elcorreo)
                                elcorreo.Dispose()
                                descrip.Text = descrip.Text + ". Acuses solicitados al SAT espere a que resuelvan la solicitud reitentando posteriormente"
                                myCommand = New SqlCommand("UPDATE ideAnual SET acuseSolicitado=1 WHERE id=" + id.Text, myConnection)
                                myCommand.ExecuteNonQuery()
                            Catch ex As Exception
                                'Response.Write("<script language='javascript'>alert('Error enviando acuses a su correo: " & ex.Message + "');</script>")
                                descrip.Text = "Error solicitando acuses al SAT " & ex.Message
                                Exit Sub
                            Finally
                                File.Delete(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP")
                            End Try
                        End Using
                    End If
                    MSG = "<script language='javascript'>alert('" + descrip.Text + "');</script>"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
                Else
                    If acuSinCorr.Checked.Equals(False) Then
                        zip.Save(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP")
                        For j = 0 To cont - 1
                            'AddFileSecurity(listaAcuses(j), Session("identidad"), FileSystemRights.FullControl, AccessControlType.Allow)
                            File.Delete(listaAcuses(j)) 'borrando pdfs de acuses
                        Next

                        'File.Delete(nomArchMens + ".ZIP")   'borro el zip de la declaracion enviada

                        Dim elcorreo As New System.Net.Mail.MailMessage
                        Using elcorreo
                            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                            elcorreo.To.Add(Session("curCorreo"))
                            elcorreo.Subject = "Acuses IDE, ejercicio " + ejercicio.ToString + " de" + razonSoc
                            elcorreo.Body = "<html><body>Buen dia<br><br>Se adjunta el archivo con los acuses del año <br><br>Los acuses de aceptación y rechazo respetaran la siguiente conformación para el nombramiento de los archivos:<br><br>AXYIIIIIAAAAMMDDHHMM.XML<br><br>En donde:<br><br>A es el identificador de archivo de ACUSE<br>X es el identificador de tipo de acuse siendo las posibles opciones: (A de Aceptado, R de Rechazo)<br>Y es el identificador de Tipo de declaración, siendo las posibles opciones: (M de Mensual, A de Anual)<br>IIIII es la clave de la Institución financiera que envía<br>AAAA es el año en que se proceso el acuse<br>MM es el mes en que se proceso el acuse en formato 2 cifras<br>DD es el día en que se proceso el acuse<br>HH es la hora en que se proceso el acuse<br>MM son los minutos en que se proceso el acuse<br>SS son los segundos en que se proceso el acuse <br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet<br>Tel 443 690 3616<br>Correo declaracioneside@gmail.com<br><a href='https://twitter.com/declaracionesid' target='_blank'><img src='declaracioneside.com/twitter.jpg' alt='Clic aquí, siguenos en twitter' Height='30px' Width='30px' BorderWidth ='0px'></a>&nbsp;<a href='http://www.youtube.com/user/declaracioneside' target='_blank'> <img src='declaracioneside.com/iconoyoutube.png'  alt='Suscribete a nuestro canal declaraciones de depósitos en efectivo e IDE en youtube' Height='30px' Width='30px' BorderWidth ='0px'></a> &nbsp;<a href='http://www.facebook.com/depositosenefectivo' target='_blank'><img src='declaracioneside.com/facebook.jpg' alt='Clic aquí para seguirnos en facebook' Height='30px' Width='30px' BorderWidth ='0px'></a>&nbsp;&nbsp;<a href='https://mx.linkedin.com/in/declaraciones-depósitos-en-efectivo-1110125b' target='_blank'><img src='declaracioneside.com/linkedin.png' alt='Siguenos en linkedin' Height='30px' Width='30px' BorderWidth ='0px'></a>&nbsp;<a href='http://plus.google.com/107594546767340388428?prsrc=3'><img src='http://ssl.gstatic.com/images/icons/gplus-32.png' alt='Google+' Height='30px' Width='30px' BorderWidth ='0px'></a><br /> </body></html>"
                            elcorreo.IsBodyHtml = True
                            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                            elcorreo.Attachments.Add(New System.Net.Mail.Attachment(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP"))
                            Dim smpt As New System.Net.Mail.SmtpClient
                            smpt.Host = "smtp.gmail.com"
                            smpt.Port = "587"
                            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a")
                            smpt.EnableSsl = True 'req p server gmail
                            Try
                                smpt.Send(elcorreo)
                                elcorreo.Dispose()

                                myCommand = New SqlCommand("UPDATE ideAnual SET acuseDescargado=1 WHERE id=" + id.Text, myConnection)
                                myCommand.ExecuteNonQuery()

                                'Response.Write("<script language='javascript'>alert('Envío exitoso de acuses presentes en el sistema a su correo');</script>")
                                descrip.Text = "Envío exitoso de acuses presentes en el sistema a su correo"
                            Catch ex As Exception
                                'Response.Write("<script language='javascript'>alert('Error enviando acuses a su correo: " & ex.Message + "');</script>")
                                descrip.Text = "Error enviando acuses a su correo: " & ex.Message
                                MSG = "<script language='javascript'>alert('" + descrip.Text + "');</script>"
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
                                Exit Sub
                            Finally
                                'File.Delete(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP")
                            End Try
                        End Using
                        MSG = "<script language='javascript'>alert('" + descrip.Text + "');</script>"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
                    End If
                End If
            End Using
        Catch ex1 As Exception
            descrip.Text = "Error al convertir acuse" ' ex1.Message '
            Dim elcorreo As New System.Net.Mail.MailMessage
            Using elcorreo
                elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                elcorreo.To.Add("declaracioneside@gmail.com")
                elcorreo.Subject = "Acuses IDE " + Session("curCorreo") + ", ejercicio " + ejercicio.ToString
                elcorreo.Body = "<html><body>" + ex1.ToString + "</body></html>"
                elcorreo.IsBodyHtml = True
                elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                Dim smpt As New System.Net.Mail.SmtpClient
                smpt.Host = "smtp.gmail.com"
                smpt.Port = "587"
                smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a")
                smpt.EnableSsl = True 'req p server gmail
                Try
                    smpt.Send(elcorreo)
                    elcorreo.Dispose()
                Catch ex As Exception
                    Exit Sub
                Finally

                End Try
            End Using

        End Try
        'retval = SendMessage(nWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero)    'cerrar
        '                Else
        ''Response.Write("<script language='javascript'>alert('Componente Acuses no localizado');</script>")
        'descrip.Text = "Componente Boton Recibe Acuses no localizado"
        '                End If
        '            Else
        ''Response.Write("<script language='javascript'>alert('Componente login acuses no localizado');</script>")
        'descrip.Text = "Componente de mensajes de aplicacion del SAT no localizado"
        '            End If

        ' ''el campo de resultados en la vers ant del testacusevb era un text, aqui es un caption/label/static
        '        Else
        ''Response.Write("<script language='javascript'>alert('Componente login acuses no localizado');</script>")
        'descrip.Text = "Componente cuenta sat / login remoto acuses no localizado"
        '        End If
        '        Else
        ''Response.Write("<script language='javascript'>alert('Componente directorio no localizado');</script>")
        'descrip.Text = "Componente repositorio/ruta/directorio no localizado"
        '        End If
        'End If
    End Sub

    Private Function acusePdf(ByVal estatus, ByVal ruta, ByVal arch, ByVal casfim) As String
        'Generando doc del acuse

        'If (File.Exists(ruta + "\acuseAnual.doc")) Then
        '    'AddFileSecurity(ruta + "\acuseAnual.doc", Session("identidad"), FileSystemRights.FullControl, AccessControlType.Allow)
        '    File.Delete(ruta + "\acuseAnual.doc")
        'End If

        'Dim p As New Process
        'p.StartInfo.FileName = "C:\inetpub\wwwroot\docAcuse.exe"
        'p.StartInfo.Arguments = "A" + "'" + Session("rfcAcuse") + "'" + Session("denominacionAcuse") + "'" + Session("recaudadoAcuse") + "'VACIO'" + Session("ejercicioAcuse") + "'VACIO'" + Session("tipoAcuse") + "'" + Session("fechaPresentacionAcuse") + "'" + Session("folioAcuse") + "'" + Session("numOperAcuse") + "'" + Session("archivoAcuse") + "'" + Session("selloAcuse") + "'" + estatus + "'" + casfim
        'p.Start()
        'p.WaitForExit()

        ''WORD TO PDF
        'Dim newApp As Microsoft.Office.Interop.Word.Application = New Microsoft.Office.Interop.Word.Application
        ''Dim newApp As New Word.Application()
        'Dim Source As Object = "C:\SAT\" + casfim + "\acuseAnual.doc"
        'Dim Target As Object = ruta + "\" + arch + ".pdf"
        'Dim Unknown As Object = Type.Missing

        'If (File.Exists(Target)) Then
        '    'AddFileSecurity(Target, Session("identidad"), FileSystemRights.FullControl, AccessControlType.Allow)
        '    File.Delete(Target)
        'End If

        'newApp.Documents.Open(Source, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown)
        'Dim format As Object = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF
        'newApp.ActiveDocument.SaveAs(Target, format, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown)
        'newApp.ActiveDocument.Close() 'P Q NO QUEDE ABIERTO EL FUENTE
        'newApp.Quit(Unknown, Unknown, Unknown)

        'If (File.Exists(Source)) Then
        '    'AddFileSecurity(Source, Session("identidad"), FileSystemRights.FullControl, AccessControlType.Allow)
        '    File.Delete(Source)
        'End If
        'Return Target


        Return docAcuse("A" + "'" + Session("rfcAcuse") + "'" + Session("denominacionAcuse") + "'" + Session("recaudadoAcuse") + "'VACIO'" + Session("ejercicioAcuse") + "'VACIO'" + Session("tipoAcuse") + "'" + Session("fechaPresentacionAcuse") + "'" + Session("folioAcuse") + "'" + Session("numOperAcuse") + "'" + Session("archivoAcuse") + "'" + Session("selloAcuse") + "'" + estatus + "'" + casfim, ruta, arch)

    End Function

    Private Function docAcuse(ByVal Command As String, ByVal ruta As String, ByVal arch As String) As String
        Dim diseño = Server.MapPath("~/acuseAnual.frx")
        WebReport1.Report.Load(diseño)
        Dim logo = Server.MapPath("~/logo1.png")
        Dim picturelogo As PictureObject = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("logo"), PictureObject)
        picturelogo.ImageLocation = logo

        Dim Data() As String
        Data = Split(Command, "'") 'lee argumentos mensAnual, rfc, denominacion, recaudado, enterado, ejercicio, periodo, tipo, fecha, folio, numero, archivo, sello, estatus, casfim
        Dim TextEstatus As TextObject = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("estatus"), TextObject)
        Dim TextEmisorNom As TextObject = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("emisorNom"), TextObject)
        Dim TextEmisorRFC As TextObject = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("emisorRFC"), TextObject)
        Dim TexttotalRecaudado As TextObject = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("totalRecaudado"), TextObject)
        Dim Textenterado As TextObject
        Dim Textejercicio As TextObject = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("ejercicio"), TextObject)
        Dim Textperiodo As TextObject
        Dim Texttipo As TextObject = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("tipo"), TextObject)
        Dim Textfecha As TextObject = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("fecha"), TextObject)
        Dim Textfolio As TextObject = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("folio"), TextObject)
        Dim Textnumero As TextObject = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("numero"), TextObject)
        Dim Textarchivo As TextObject = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("archivo"), TextObject)
        Dim Textsello As TextObject = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("sello"), TextObject)
        Dim TextEncab As TextObject = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("TextEncab"), TextObject)
        TextEncab.Text = "Acuse Recepción Declaración Anual de Depósitos en Efectivo"

        If Data(13) = "R" Then
            TextEstatus.Text = "Error en acuse, contáctenos"
        Else
            TextEmisorNom.Text = Data(2).Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").Replace("&quot;", """").Replace("&apos;", "'")
            TextEmisorRFC.Text = Data(1)
            TexttotalRecaudado.Text = Data(3)
            If Data(0) = "M" Then
                Textenterado = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("totalEnterado"), TextObject)
                Textenterado.Text = Data(4)
            End If
            Textejercicio.Text = Data(5)
            If Data(0) = "M" Then
                Textperiodo = DirectCast(DirectCast(WebReport1, WebReport).Report.FindObject("periodo"), TextObject)
                Textperiodo.Text = Data(6)
            End If
            Texttipo.Text = Data(7)
            Textfecha.Text = CDate(Data(8)).ToString("dd/MM/yyyy")
            Textfolio.Text = Data(9)
            Textnumero.Text = Data(10)
            Textarchivo.Text = Data(11)
            Textsello.Text = Data(12)
        End If


        WebReport1.Report.Prepare()
        Dim export As FastReport.Export.Pdf.PDFExport = New FastReport.Export.Pdf.PDFExport()
        export.EmbeddingFonts = False   'sube tam a 220Kb pero se ve en preview del navegador
        export.Background = False
        export.PdfA = False ''sube tam a 230Kb pero se ve en preview del navegador
        export.Compressed = True
        export.PrintOptimized = False 'sube tam a 85Kb y no se ve en preview del navegador
        export.AllowPrint = True
        ' las fuentes deben ser tahoma p q se vean en preview del navegador
        Dim Target As String = ruta + "\" + arch + ".pdf"
        WebReport1.Report.Export(export, Target)

        Return Target
    End Function
    'Private Sub bajarAcuse() 'version pruebaIDE.exe pero no lo abre en el server por ondas del .net
    '    Dim loginSAT, directorioServidor, casfim, tipo, idArch
    '    Dim q = "Select loginSAT, directorioServidor, casfim FROM clientes WHERE id=" + session("GidCliente").ToString
    '    myCommand = New SqlCommand(q, myConnection)
    '    dr = myCommand.ExecuteReader()
    '    dr.Read()
    '    loginSAT = dr("loginSAT")
    '    directorioServidor = "C:  \SAT\" + dr("directorioServidor")
    '    casfim = dr("casfim")
    '    dr.Close()
    '    If normalComplementaria.Text = "NORMAL" Then
    '        tipo = "N"
    '        idArch = ""
    '    Else
    '        tipo = "C"
    '        idArch = id.Text
    '    End If
    '    nomArchMens = "C:\SAT\" + casfim + "\" + "A-" + ejercicio.ToString + tipo + idArch + ".XML"
    '    nomArchMensSinPath = "A-" + ejercicio.ToString + tipo + idArch + ".XML"

    '    Dim nWnd As IntPtr
    '    Dim ceroIntPtr As New IntPtr(0)
    '    Dim Wnd_name As String
    '    Dim hWnd As New IntPtr(0)
    '    Dim hWnd2 As New IntPtr(0)
    '    Dim hWnd3 As New IntPtr(0)
    '    Dim hWnd4 As New IntPtr(0)
    '    Dim hWnd5 As New IntPtr(0)
    '    Dim hWnd6 As New IntPtr(0)
    '    Dim hWnd7 As New IntPtr(0)

    '    Dim WM_CLOSE = &H10
    '    Dim WM_SETTEXT = &HC
    '    Dim WM_GETTEXT = &HD
    '    Dim BM_CLICK = &HF5

    '    Dim retval As IntPtr
    '    Dim retval2 As IntPtr
    '    Dim retval3 As IntPtr
    '    Dim retval4 As IntPtr
    '    Dim retval5 As IntPtr
    '    Dim retval6 As IntPtr
    '    Dim retval7 As IntPtr

    '    Dim sClassName As New StringBuilder("", 256)
    '    Dim clase

    '    Dim p As New ProcessStartInfo
    '    p.FileName = "C:\SAT\TestIDE.exe" '"C:\SAT\TestAcuseVB.exe"
    '    p.WindowStyle = ProcessWindowStyle.Normal
    '    Process.Start(p)
    '    System.Threading.Thread.Sleep(1000)

    '    Wnd_name = "IDE    Declaraciones y Acuses  ver 3.0" '"Declaraciones y Acuses IDE ver 3.0"
    '    'Wnd_name = "Form1" 'antes correr el hostProy.exe hasta q quede abierta la form 
    '    nWnd = FindWindow(Nothing, Wnd_name)
    '    'hWnd6 = FindWindowEx(nWnd, IntPtr.Zero, "ThunderRT6CommandButton", "cancel") 'cancel
    '    'If Not hWnd6.Equals(ceroIntPtr) Then
    '    '    SetActiveWindow(nWnd)
    '    '    retval6 = SendNotifyMessage(hWnd6, BM_CLICK, IntPtr.Zero, 0)
    '    'End If

    '    If nWnd.Equals(ceroIntPtr) Then
    '        'Response.Write("<script language='javascript'>alert('Aplicación de hacienda no se lanzó');</script>")
    '        descrip.Text = "Aplicación de hacienda no se lanzó"
    '    Else
    '        Call GetClassName(nWnd, sClassName, 256)
    '        clase = sClassName.ToString.Replace("Window.8", "EDIT")
    '        hWnd = FindWindowEx(nWnd, IntPtr.Zero, clase, "")    'acuse: ruta/repositorio
    '        'hWnd = FindWindowEx(nWnd, IntPtr.Zero, "ThunderRT6TextBox", "")    'acuse: ruta/repositorio
    '        If Not hWnd.Equals(ceroIntPtr) Then
    '            retval = SendMessage(hWnd, WM_SETTEXT, IntPtr.Zero, directorioServidor)
    '            hWnd2 = FindWindowEx(nWnd, hWnd, clase, "")      'acuse: login remoto 
    '            If Not hWnd2.Equals(ceroIntPtr) Then
    '                retval2 = SendMessage(hWnd2, WM_SETTEXT, IntPtr.Zero, loginSAT)
    '                clase = sClassName.ToString.Replace("Window.8", "STATIC")
    '                hWnd3 = FindWindowEx(nWnd, IntPtr.Zero, clase, "")     'resultados del comando
    '                SetActiveWindow(nWnd)
    '                clase = sClassName.ToString.Replace("Window.8", "BUTTON")
    '                hWnd7 = FindWindowEx(nWnd, IntPtr.Zero, clase, "Recibe Acuse") 'Acuses
    '                If Not hWnd7.Equals(ceroIntPtr) Then
    '                    retval7 = SendNotifyMessage(hWnd7, BM_CLICK, IntPtr.Zero, 0)   'HABILITAR CLIC RX
    '                    Dim Handle As IntPtr = Marshal.AllocHGlobal(500)
    '                    Dim resultado As String
    '                    Dim numText As IntPtr
    '                    Dim tam As IntPtr
    '                    tam = 500
    '                    Do
    '                        numText = SendMessage(hWnd3, WM_GETTEXT, tam, Handle)    'resultados del comando                                        
    '                        resultado = Marshal.PtrToStringUni(Handle)
    '                    Loop While resultado.Equals("")     'vs tiempo fijo
    '                    If InStr(resultado, "ERROR") Or InStr(resultado, "FALLA") Or InStr(resultado, "Falla") Or InStr(resultado, "Atencion") Or InStr(resultado, "errno") Then 'o distinto de Exito

    '                        Dim elcorreo As New System.Net.Mail.MailMessage
    '                        Using elcorreo
    '                            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
    '                            elcorreo.To.Add("declaracioneside@gmail.com")
    '                            elcorreo.Subject = "Decl. anual ERROR_ACUSE"
    '                            elcorreo.Body = "<html><body>cliente=" + session("curCorreo") + ", ejercicio=" + ejercicio + ", error=" + resultado + "</body></html>"
    '                            elcorreo.IsBodyHtml = True
    '                            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
    '                            Dim smpt As New System.Net.Mail.SmtpClient
    '                            smpt.Host = "smtp.gmail.com"
    '                            smpt.Port = "587"
    '                            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a")
    '                            smpt.EnableSsl = True 'req p server gmail
    '                            Try
    '                                smpt.Send(elcorreo)
    '                                elcorreo.Dispose()
    '                            Catch ex As Exception
    '                                Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
    '                                Exit Sub
    '                            Finally
    '                            End Try
    '                        End Using
    '                        descrip.Text = "Error bajando acuses, notifiquelo al proveedor o espere a que sea restablecido el servidor del SAT. "

    '                    Else

    '                    End If

    '                    'creando comprimido con acuses y enviandolo por correo
    '                    If File.Exists(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP") Then 'sin la ext .xml
    '                        File.Delete(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP")
    '                    End If

    '                    Dim di As New IO.DirectoryInfo(directorioServidor + "\")
    '                    Dim diar1 As IO.FileInfo() = di.GetFiles("*.xml")
    '                    Dim dra As IO.FileInfo
    '                    Dim fName As String
    '                    Dim allRead As String
    '                    Dim regMatch As String 'string to search for inside of text file. It is case sensitive.
    '                    regMatch = nomArchMensSinPath  'buscando el nomArchMensSinPath como texto dentro del archivo
    '                    Try
    '                        Using zip As ZipFile = New ZipFile
    '                            Dim c = 0
    '                            For Each dra In diar1   'busca aceptaciones y rechachazos del archivo
    '                                fName = dra.FullName 'path to text file
    '                                Dim testTxt As StreamReader = New StreamReader(fName)
    '                                allRead = testTxt.ReadToEnd() 'Reads the whole text file to the end
    '                                testTxt.Close() 'Closes the text file after it is fully read.
    '                                If (Regex.IsMatch(allRead, regMatch)) Then 'If match found in allRead
    '                                    zip.AddFile(fName, "")
    '                                    If Left(dra.Name, 2) = "AA" Then 'acuse aceptado, solo esos traen numOper y fechaPresentacion
    '                                        extraeNumoperDeAcuse(allRead)

    '                                        'borro movs de decls con acuse de aceptacion, ya se aceptó y ya no están disp pal user y se libera espacio
    '                                        myCommand = New SqlCommand("DELETE FROM mov WHERE idCuentasIdeDetAnual IN (SELECT id FROM cuentasIdeDetAnual WHERE idideDetAnual IN (SELECT id FROM ideDetAnual WHERE idAnual=" + id.Text + "))", myConnection)
    '                                        myCommand.ExecuteNonQuery()
    '                                    End If
    '                                    c = 1
    '                                End If
    '                            Next
    '                            If c = 0 Then
    '                                'Response.Write("<script language='javascript'>alert('No se encontraron acuses para este periodo');</script>")
    '                                descrip.Text = descrip.Text + "No se encontraron acuses para este periodo"
    '                            Else
    '                                zip.Save(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP")

    '                                Dim elcorreo As New System.Net.Mail.MailMessage
    '                                Using elcorreo
    '                                    elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
    '                                    elcorreo.To.Add(session("curCorreo"))
    '                                    elcorreo.Subject = "Acuses IDE, ejercicio " + ejercicio.ToString
    '                                    elcorreo.Body = "<html><body>Buen dia<br><br>Se adjunta el archivo con los acuses del año <br><br>Los acuses de aceptación y rechazo respetaran la siguiente conformación para el nombramiento de los archivos:<br><br>AXYIIIIIAAAAMMDDHHMM.XML<br><br>En donde:<br><br>A es el identificador de archivo de ACUSE<br>X es el identificador de tipo de acuse siendo las posibles opciones: (A de Aceptado, R de Rechazo)<br>Y es el identificador de Tipo de declaración, siendo las posibles opciones: (M de Mensual, A de Anual)<br>IIIII es la clave de la Institución financiera que envía<br>AAAA es el año en que se proceso el acuse<br>MM es el mes en que se proceso el acuse en formato 2 cifras<br>DD es el día en que se proceso el acuse<br>HH es la hora en que se proceso el acuse<br>MM son los minutos en que se proceso el acuse<br>SS son los segundos en que se proceso el acuse <br><br>Atentamente, <a href='declaracioneside.com'>declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet </body></html>"
    '                                    elcorreo.IsBodyHtml = True
    '                                    elcorreo.Priority = System.Net.Mail.MailPriority.Normal
    '                                    elcorreo.Attachments.Add(New Attachment(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP"))
    '                                    Dim smpt As New System.Net.Mail.SmtpClient
    '                                    smpt.Host = "smtp.gmail.com"
    '                                    smpt.Port = "587"
    '                                    smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a")
    '                                    smpt.EnableSsl = True 'req p server gmail
    '                                    Try
    '                                        smpt.Send(elcorreo)
    '                                        elcorreo.Dispose()
    '                                        'Response.Write("<script language='javascript'>alert('Envío exitoso de acuses presentes en el sistema a su correo');</script>")
    '                                        descrip.Text = "Envío exitoso de acuses presentes en el sistema a su correo"
    '                                    Catch ex As Exception
    '                                        'Response.Write("<script language='javascript'>alert('Error enviando acuses a su correo: " & ex.Message + "');</script>")
    '                                        descrip.Text = "Error enviando acuses a su correo: " & ex.Message
    '                                        Exit Sub
    '                                    Finally
    '                                        File.Delete(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP")
    '                                    End Try
    '                                End Using
    '                            End If
    '                        End Using
    '                    Catch ex1 As Exception
    '                        descrip.Text = "Error al convertir acuse. "
    '                        Dim elcorreo As New System.Net.Mail.MailMessage
    '                        Using elcorreo
    '                            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
    '                            elcorreo.To.Add("declaracioneside@gmail.com")
    '                            elcorreo.Subject = "Acuses IDE " + session("curCorreo") + ", ejercicio " + ejercicio.ToString
    '                            elcorreo.Body = "<html><body>" + ex1.ToString + "</body></html>"
    '                            elcorreo.IsBodyHtml = True
    '                            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
    '                            Dim smpt As New System.Net.Mail.SmtpClient
    '                            smpt.Host = "smtp.gmail.com"
    '                            smpt.Port = "587"
    '                            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a")
    '                            smpt.EnableSsl = True 'req p server gmail
    '                            Try
    '                                smpt.Send(elcorreo)
    '                                elcorreo.Dispose()
    '                            Catch ex As Exception
    '                                Exit Sub
    '                            Finally

    '                            End Try
    '                        End Using

    '                    End Try
    '                    retval = SendMessage(nWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero)    'cerrar
    '                Else
    '                    'Response.Write("<script language='javascript'>alert('Componente Acuses no localizado');</script>")
    '                    descrip.Text = "Componente Boton Recibe Acuses no localizado"
    '                End If
    '                ''el campo de resultados en la vers ant del testacusevb era un text, aqui es un caption/label/static
    '            Else
    '                'Response.Write("<script language='javascript'>alert('Componente login acuses no localizado');</script>")
    '                descrip.Text = "Componente cuenta sat / login remoto acuses no localizado"
    '            End If
    '        Else
    '            'Response.Write("<script language='javascript'>alert('Componente directorio no localizado');</script>")
    '            descrip.Text = "Componente repositorio/ruta/directorio no localizado"
    '        End If
    '    End If
    'End Sub
    Protected Sub bajarAcuseExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bajarAcuseExcel.Click
        Call bajarAcuse()
    End Sub

    Friend Shared Function ReadFile(ByVal fileName As String) As Byte()
        Dim f As New FileStream(fileName, FileMode.Open, FileAccess.Read)
        Dim size As Integer = Fix(f.Length)
        Dim data(size) As Byte
        size = f.Read(data, 0, size)
        f.Close()
        Return data

    End Function


    '    protected static X509Certificate2 BuscarCertificado (string RFC)
    '    {
    '      // Método para obtener el certificado que pertenece a un RFC

    '      // Cargamos la lista de certificados personales instalados en Windows
    '      X509Store Certificados = new X509Store (StoreName.My, StoreLocation.CurrentUser);
    '      Certificados.Open (OpenFlags.ReadOnly);

    '      // Buscamos el certificado del contribuyente
    '      foreach (X509Certificate2 Resultado in Certificados.Certificates)
    '        /* El sujeto (propiedad Subject) del certificado puede contener algo como:
    '           "OU=Unidad 1, SERIALNUMBER=" / AAAA010101HDFRXX01", " (continúa)
    '           "OID.2.5.4.45=AAA010101AAA / AAAA010101AAA, O=Matriz SA, " (continúa)
    '           "OID.2.5.4.41=Matriz SA, CN=Matriz SA" 

    '           "AAA010101AAA / AAAA010101AAA" son el RFC del contribuyente (persona moral o física) y,
    '           opcionalmente, el RFC de la persona física que representa a la persona moral 
    '           (posiblemente).  El primero es el que nos interesa y debe ser igual al parámetro RFC para
    '           dar por encontrado el certificado. 

    '           Revisaremos cada par "llave=valor" del sujeto.  NOTA: Puede que convenga robustecer el
    '           código de este ciclo anidado. */
    '        foreach (string Dato in Resultado.Subject.Split (','))
    '        {
    '          string[] LlaveValor = Dato.Trim().Split ('=');

    '          if ((LlaveValor.Length == 2) && LlaveValor [0].EndsWith ("2.5.4.45") && 
    '          (LlaveValor [1].Split ('/') [0].Trim () == RFC))
    '            return Resultado;  // Encontrado
    '        }

    '      throw new Exception ("No hay un certificado instalado para el RFC que se indicó.");

    'End Function

    Private Sub contingencia()

        If estado.Text = "VACIA" Or estado.Text = "IMPORTADA" Then
            Response.Write("<script language='javascript'>alert('La declaración esta vacía o recién importada, pruebe a enviarla primero');</script>")
            Exit Sub
        End If

        descrip.Text = ""

        progressbar1.Style("width") = "0px"
        statusImport.Text = ""

        Dim q, contra, elplan
        Dim fechaDeclarar = Convert.ToDateTime(Trim("01/01/" + ejercicio.ToString)).ToString("yyyy-MM-dd")
        q = "SELECT co.id, pla.elplan FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND co.id=" + Session("GidContrato").ToString + " AND ( (nDeclHechas < nDeclContratadas and (pla.elplan<>'PREMIUM')) or (('" + Convert.ToDateTime(fechaDeclarar).ToString("yyyy-MM-dd") + "' >= periodoInicial and pla.elplan='PREMIUM' and co.anualEnPremium=1) and ('" + Convert.ToDateTime(fechaDeclarar).ToString("yyyy-MM-dd") + "' <= fechaFinal and pla.elplan='PREMIUM' and co.anualEnPremium=1) ) ) order by co.id"

        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        contra = dr.Read()
        If (Not contra) Then 'sin contrato vigente 
            Response.Write("<script language='javascript'>alert('A alcanzado el máximo de declaraciones contratadas o bien ha caducado su contrato, o el periodo no está cubierto en este contrato');</script>")
            Exit Sub
        Else
            elplan = dr("elplan")
        End If
        dr.Close()

        Dim casfim, q2, razon
        q2 = "SELECT casfim, razonSoc FROM clientes WHERE id=" + Session("GidCliente").ToString
        myCommand = New SqlCommand(q2, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        casfim = dr("casfim")
        razon = dr("razonSoc")
        dr.Close()
        Dim tipo, idArch
        If normalComplementaria.Text = "NORMAL" Then
            tipo = "N"
            idArch = ""
        Else
            tipo = "C"
            idArch = id.Text
        End If
        ContNomArchMens = "C:\SAT\" + casfim + "\" + casfim + "-ContA-" + ejercicio.ToString + tipo + idArch + ".XML.ZIP"

        'bajar de la BD
        If File.Exists(ContNomArchMens) Then
            File.Delete(ContNomArchMens)
        End If
        Dim myCommand2 As New SqlCommand
        With myCommand2
            .Connection = myConnection
            .CommandType = CommandType.StoredProcedure
            .CommandText = "ideAnualBajaxml"
            .Parameters.AddWithValue("@ID", id.Text)
            dr = .ExecuteReader()
            If Not dr Is Nothing Then
                If dr.Read Then
                    If DBNull.Value.Equals(dr("archivoXML")) Then
                        Response.Write("<script language='javascript'>alert('1o importe la información');</script>")
                        dr.Close()
                        Exit Sub
                    End If
                    Dim writeStream As FileStream
                    writeStream = New FileStream(ContNomArchMens, FileMode.Create)
                    Dim writeBinay As New BinaryWriter(writeStream)
                    writeBinay.Write(dr("archivoXML"))
                    writeBinay.Close()
                End If
            End If
        End With

        'Call firmaXML()

        Dim elcorreo As New System.Net.Mail.MailMessage
        Using elcorreo
            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
            If quienContin.SelectedValue.Equals("Proveedor") Then
                elcorreo.To.Add("declaracioneside@gmail.com")
                elcorreo.Subject = "Contingencia IDE por el proveedor, ejercicio " + ejercicio.ToString + " de " + razon + " (" + Session("curCorreo") + ")"
            Else 'contribuyente
                elcorreo.To.Add(Session("curCorreo"))
                elcorreo.Subject = "Contingencia IDE por el contribuyente, ejercicio " + ejercicio.ToString + " de " + razon + " (" + Session("curCorreo") + ")"
            End If
            elcorreo.Body = "<html><body>Buen dia<br><br>Se adjunta el archivo de contingencia del ejercicio, <br><br> en el siguiente enlace se encuentra el <a href='ftp://ftp2.sat.gob.mx/asistencia_servicio_ftp/publicaciones/IDE08/IDE_contingencia_nov10.pdf'>Instructivo</a><br><br>Atentamente, <a href='declaracioneside.com'>declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet </body></html>"
            elcorreo.IsBodyHtml = True
            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
            elcorreo.Attachments.Add(New System.Net.Mail.Attachment(ContNomArchMens))
            Dim smpt As New System.Net.Mail.SmtpClient
            smpt.Host = "smtp.gmail.com"
            smpt.Port = "587"
            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a")
            smpt.EnableSsl = True 'req p server gmail
            Try
                smpt.Send(elcorreo)
                elcorreo.Dispose()
                Response.Write("<script language='javascript'>alert('Envío exitoso de contingencia a su correo');</script>")
            Catch ex As Exception
                Response.Write("<script language='javascript'>alert('Error enviando contingencia a su correo: " & ex.Message + "');</script>")
                Exit Sub
            Finally
                'If File.Exists(ContNomArchMens) Then 'borro el zip
                'File.Delete(ContNomArchMens)
                'End If
            End Try
        End Using

        If elplan <> "PREMIUM" Then
            q = "UPDATE contratos SET nDeclHechas=nDeclHechas+1 WHERE id=" + Session("GidContrato").ToString
            myCommand = New SqlCommand(q, myConnection)
            myCommand.ExecuteNonQuery()
        End If

        estado.Text = "CONTINGENCIA"
        myCommand3 = New SqlCommand("UPDATE ideAnual SET estado='CONTINGENCIA' WHERE id=" + id.Text, myConnection)
        myCommand3.ExecuteNonQuery()

    End Sub

    Protected Sub btnContingencia_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnContingencia.Click
        'excel o xml
        If (Request.QueryString("op") = "0" And Request.QueryString("subop") = "1") Or (Request.QueryString("op") = "2" And Request.QueryString("subop") = "1") Or (Request.QueryString("op") = "0" And Request.QueryString("subop") = "0") Or (Request.QueryString("op") = "2" And Request.QueryString("subop") = "0") Then
            If validaModificada() < 1 Then
                Exit Sub
            End If
        Else '0s
            If id.Text = "0" Or estado.Text = "VACIA" Then
                Response.Write("<script language='javascript'>alert('1o haga clic en crear');</script>")
                Exit Sub
            End If
        End If

        If chkPostpago.Checked.Equals(False) Then
            Dim idcli
            Dim q
            q = "SELECT id, solSocketEstatus, loginSAT FROM clientes WHERE correo='" + Session("curCorreo") + "'"
            myCommand = New SqlCommand(q, myConnection)
            dr = myCommand.ExecuteReader()
            dr.Read()
            'If dr("inscripcionPagada").Equals(False) Then
            '    dr.Close()
            '    Response.Write("<script language='javascript'>alert('Es necesario que cubra el pago de su inscripción');</script>")
            '    Response.Write("<script>location.href='misContra.aspx';</script>")
            '    Exit Sub
            'End If
            dr.Close()

            q = "SELECT count(*) as cuenta FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND co.fechaPago IS NOT NULL"
            myCommand = New SqlCommand(q, myConnection)
            dr = myCommand.ExecuteReader()
            dr.Read()
            If (dr("cuenta").Equals(0)) Then 'sin contratos pagados 
                dr.Close()
                Response.Write("<script language='javascript'>alert('No hay contratos pagados');</script>")
                Response.Write("<script>location.href='misContra.aspx';</script>")
                Exit Sub
            End If
            dr.Close()
        End If

        Call contingencia()
    End Sub

    Private Sub creaTagsAnCeros()
        Dim reprLegalAp1, reprLegalAp2, reprLegalRfc, reprLegalNombres, tipo
        Dim q
        q = "SELECT * FROM reprLegal WHERE idCliente=" + Session("GidCliente").ToString + " and esActual=1"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        reprLegalNombres = dr("nombres")
        reprLegalAp1 = dr("ap1")
        reprLegalAp2 = dr("ap2")
        reprLegalRfc = dr("rfc")
        dr.Close()

        Dim idArch
        If normalComplementaria.Text = "NORMAL" Then
            tipo = "N"
            idArch = ""
        Else
            tipo = "C"
            idArch = id.Text
        End If

        'A=anual
        Dim casfim, vRfc, vempresa, esInstitCredito
        q = "SELECT casfim,rfcDeclarante,razonSoc,esInstitCredito FROM clientes WHERE id=" + Session("GidCliente").ToString
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        casfim = dr("casfim")
        vRfc = dr("rfcDeclarante")
        vempresa = dr("razonSoc")
        If dr("esInstitCredito").Equals(True) Then
            esInstitCredito = 1
        Else
            esInstitCredito = 0
        End If
        dr.Close()

        nomArchMens = "C:\SAT\" + casfim + "\" + casfim + "-A-" + ejercicio.ToString + tipo + idArch + ".XML"
        nomArchMensSinPath = casfim + "-A-" + ejercicio.ToString + tipo + idArch + ".XML"

        If File.Exists(nomArchMens) Then
            File.Delete(nomArchMens)
        End If

        Dim utf8WithoutBom As New System.Text.UTF8Encoding(False)
        Dim archivo As StreamWriter = New System.IO.StreamWriter(nomArchMens, False, utf8WithoutBom)
        archivo.WriteLine("<?xml version='1.0' encoding='UTF-8'?>")
        archivo.WriteLine("    <DeclaracionInformativaAnualISR xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:noNamespaceSchemaLocation='C:/SAT/ide_20150112_V3_0.xsd' version='3.0' rfcDeclarante='" + Left(vRfc, 12) + "' denominacion='" + vempresa + "'>")
        archivo.WriteLine("                <RepresentanteLegal rfc='" + reprLegalRfc + "'>")
        archivo.WriteLine("                    <Nombre>")
        archivo.WriteLine("                        <Nombres>" + reprLegalNombres + "</Nombres>")
        archivo.WriteLine("                        <PrimerApellido>" + reprLegalAp1 + "</PrimerApellido>")
        archivo.WriteLine("                        <SegundoApellido>" + reprLegalAp2 + "</SegundoApellido>")
        archivo.WriteLine("                    </Nombre>")
        archivo.WriteLine("                </RepresentanteLegal>")
        If tipo = "N" Then
            archivo.WriteLine("                <Normal ejercicio='" + ejercicio.ToString + "'></Normal>")
        Else
            archivo.WriteLine("                <Complementaria ejercicio='" + ejercicio.ToString + "' opAnterior='" + numOperAnt.Text.Trim + "' fechaPresentacion='" + CDate(fechaPresentacionAnt.Text.Trim).ToString("yyyy-MM-dd") + "'></Complementaria>")
        End If
        If esInstitCredito = 1 Then
            archivo.WriteLine("                <InstitucionDeCredito>")
        Else
            archivo.WriteLine("                <InstitucionDistintaDeCredito>")
        End If
        For p = 1 To 12
            archivo.WriteLine("                             <ReporteDeDepositosEnEfectivo Periodo='" + CStr(p) + "' />")
        Next p
        If esInstitCredito = 1 Then
            archivo.WriteLine("                             <Totales importeCheques='0' operacionesRelacionadas='0' importeExcedenteDepositos='0' />")
        Else
            archivo.WriteLine("                             <Totales operacionesRelacionadas='0' importeExcedenteDepositos='0' />")
        End If

        If esInstitCredito = 1 Then
            archivo.WriteLine("                </InstitucionDeCredito>")
        Else
            archivo.WriteLine("                </InstitucionDistintaDeCredito>")
        End If
        archivo.WriteLine("     </DeclaracionInformativaAnualISR>")

        archivo.Close()
    End Sub




    Protected Sub Crear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Crear.Click
        'If validaModificada() < 1 Then
        '    Exit Sub
        'End If

        descrip.Text = ""

        Dim q, contra
        Dim fechaVariable = CDate(CStr(DatePart(DateInterval.Year, Now())) + "/01/01") 'dia 1o de este año
        q = "SELECT co.id, pla.elplan FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND ( (nDeclHechas < nDeclContratadas and (pla.elplan<>'PREMIUM')) or ('" + Convert.ToDateTime(fechaVariable).ToString("yyyy-MM-dd") + "' > periodoInicial and '" + Convert.ToDateTime(fechaVariable).ToString("yyyy-MM-dd") + "' <= fechaFinal and pla.elplan='PREMIUM' and co.anualEnPremium=1 and co.esRegularizacion=0) or (pla.elplan='PREMIUM' and co.anualEnPremium=1 and co.esRegularizacion=1)  ) "

        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        contra = dr.Read()
        If (Not contra) Then 'sin contrato vigente 
            Response.Write("<script language='javascript'>alert('A alcanzado el máximo de declaraciones contratadas o bien ha caducado su contrato');</script>")
            Exit Sub
        End If
        dr.Close()

        If Session("GidAnual") = 0 Then 'no hay anual del ejercicio -> insertar anual vacia
            Call insertaAnualVacia()
            Call insertaMensualVacia()
        Else
            id.Text = Session("GidAnual").ToString
            Call insertaMensualParciales()
        End If


        Call creaXMLañoCeros() 'crea el zip del xml y lo copia a BD        

        estado.Text = "CREADA"
        myCommand2 = New SqlCommand("UPDATE ideAnual SET estado='CREADA', idContrato=" + idContrato.ToString + ", nOpers=0, impteExcedente=0, impteCheque=0,normalComplementaria='" + normalComplementaria.Text + "' WHERE id=" + id.Text, myConnection)
        myCommand2.ExecuteNonQuery()

    End Sub

    Protected Sub verCeros_Click(ByVal sender As Object, ByVal e As EventArgs) Handles verCeros.Click
        MultiView1.ActiveViewIndex = Int32.Parse(4)
        cargaGrid()
        progressbar1.Style("width") = "0px"
        statusImport.Text = ""
        descrip.Text = ""
    End Sub

    Protected Sub importarXml_Click(ByVal sender As Object, ByVal e As EventArgs) Handles importarXml.Click
        If Not FileUpload2.HasFile Then
            Response.Write("<script language='javascript'>alert('No especificó el archivo a subir');</script>")
            Exit Sub
        End If

        Dim fileName As String = Server.HtmlEncode(FileUpload2.FileName)
        Dim extension As String = System.IO.Path.GetExtension(fileName)
        If Not (extension = ".xml" Or extension = ".XML") Then
            Response.Write("<script language='javascript'>alert('El archivo debe ser formato xml');</script>")
            Exit Sub
        End If

        If InStr(fileName, "á") > 0 Or InStr(fileName, "é") > 0 Or InStr(fileName, "í") > 0 Or InStr(fileName, "ó") > 0 Or InStr(fileName, "ú") > 0 Or InStr(fileName, "Á") > 0 Or InStr(fileName, "É") > 0 Or InStr(fileName, "Í") > 0 Or InStr(fileName, "Ó") > 0 Or InStr(fileName, "Ú") > 0 Then
            Response.Write("<script language='javascript'>alert('Cambie el nombre del archivo para que no tenga acentos e intente de nuevo');</script>")
            Exit Sub
        End If


        progressbarXml.Style("width") = "0px"
        statusImportXml.Text = ""
        descrip.Text = ""

        If Session("GidAnual") = 0 Then 'no hay anual del ejercicio -> insertar anual vacia
            Call insertaAnualVacia()
            Call insertaMensualVacia()
        Else
            Call insertaMensualParciales() 'donde no haya mensuales las crea vacias
        End If


        Dim q, casfim
        q = "SELECT casfim FROM clientes WHERE id=" + Session("GidCliente").ToString
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        casfim = dr("casfim")
        dr.Close()

        Dim tipo, idArch
        If normalComplementaria.Text = "NORMAL" Then
            tipo = "N"
            idArch = ""
        Else
            tipo = "C"
            idArch = id.Text
        End If
        nomArchMens = "C:\SAT\" + casfim + "\" + casfim + "-A-" + ejercicio.ToString + tipo + idArch + ".XML"
        nomArchMensSinPath = casfim + "-A-" + ejercicio.ToString + tipo + idArch + ".XML"

        If File.Exists(nomArchMens) Then
            File.Delete(nomArchMens)
        End If

        FileUpload2.SaveAs(nomArchMens)

        'AddFileSecurity(nomArchMens, Session("identidad"), FileSystemRights.FullControl, AccessControlType.Allow)
        'AddFileSecurity(savePath, "IIS_WPG", FileSystemRights.ReadData, AccessControlType.Allow)

        If validacion() = False Then
            Exit Sub
        End If

        Call comprimeAnual() 'borra xml crea zip
        Call subeXMLanualBD()
        statusImportXml.Text = " Importación IDE realizada "
        progressbarXml.Style("width") = "100px"
        estado.Text = "IMPORTADA"

        myCommand2 = New SqlCommand("UPDATE ideAnual SET estado='IMPORTADA', idContrato=" + idContrato.ToString + ", viaImportacion=2 WHERE id=" + id.Text, myConnection)
        myCommand2.ExecuteNonQuery()

        If normalComplementaria.Text = "COMPLEMENTARIA" Then
            q = "UPDATE ideAnual SET fechaPresentacionAnt='" + Convert.ToDateTime(fechaPresentacionAnt.Text).ToString("yyyy-MM-dd") + "', numOperAnt='" + numOperAnt.Text + "', normalComplementaria='COMPLEMENTARIA' WHERE id=" + id.Text
            myCommand3 = New SqlCommand(q, myConnection)
            myCommand3.ExecuteNonQuery()
        End If
        'ClientScript.RegisterStartupScript(Me.GetType(), "Load", "<script type='text/javascript'>window.parent.location.href = 'decla.aspx'; </script>")
    End Sub

    Protected Sub verXml_Click(ByVal sender As Object, ByVal e As EventArgs) Handles verXml.Click
        MultiView1.ActiveViewIndex = Int32.Parse(5)

        If File.Exists("C:\inetpub\wwwroot\xmlSubidos\" + Session("curCorreo") + "." + ejercicio.ToString + ".xml.ZIP") Then
            File.Delete("C:\inetpub\wwwroot\xmlSubidos\" + Session("curCorreo") + "." + ejercicio.ToString + ".xml.ZIP")
        End If
        'bajar de la BD
        Dim myCommand2 As New SqlCommand
        With myCommand2
            .Connection = myConnection
            .CommandType = CommandType.StoredProcedure
            .CommandText = "ideAnualBajaxml"
            .Parameters.AddWithValue("@ID", id.Text)
            dr = .ExecuteReader()
            If Not dr Is Nothing Then
                If dr.Read Then
                    If DBNull.Value.Equals(dr("archivoXML")) Then
                        Response.Write("<script language='javascript'>alert('Aun no ha subido su xml');</script>")
                        dr.Close()
                        Exit Sub
                    End If
                    Dim writeStream As FileStream
                    writeStream = New FileStream("C:\inetpub\wwwroot\xmlSubidos\" + Session("curCorreo") + "." + ejercicio.ToString + ".xml.ZIP", FileMode.Create)
                    Dim writeBinay As New BinaryWriter(writeStream)
                    writeBinay.Write(dr("archivoXML"))
                    writeBinay.Close()
                End If
            End If
        End With

        progressbarXml.Style("width") = "0px"
        statusImportXml.Text = ""
        descrip.Text = ""
        lblAvance.Text = ""
    End Sub


    Private Sub regresar()
        descrip.Text = ""
        Select Case Request.QueryString("op")
            Case "0" 'crear
                If Request.QueryString("subop") = "0" Then  'xls
                    MultiView1.ActiveViewIndex = Int32.Parse(0)
                ElseIf Request.QueryString("subop") = "1" Then  'xml
                    MultiView1.ActiveViewIndex = Int32.Parse(1)
                Else 'edit
                    MultiView1.ActiveViewIndex = Int32.Parse(2)
                End If
            Case "1" 'ceros
                MultiView1.ActiveViewIndex = Int32.Parse(3)
            Case "2" 'consultar
                If Request.QueryString("subop") = "0" Then  'xls
                    MultiView1.ActiveViewIndex = Int32.Parse(4)
                Else 'xml
                    MultiView1.ActiveViewIndex = Int32.Parse(5)
                End If
                cargaGrid()
            Case "3" 'via 12 mens
                MultiView1.ActiveViewIndex = Int32.Parse(6)
        End Select

    End Sub

    Protected Sub backXml_Click(ByVal sender As Object, ByVal e As EventArgs) Handles backXml.Click
        Call regresar()
    End Sub

    Protected Sub consultarXml_Click(ByVal sender As Object, ByVal e As EventArgs) Handles consultarXml.Click
        If estado.Text = "VACIA" Then
            Response.Write("<script language='javascript'>alert('La declaración esta vacía, pruebe a importarla primero');</script>")
            Exit Sub
        End If

        'descarga archivo, file download
        Dim filename As String = Session("curCorreo") + "." + ejercicio.ToString + ".xml.ZIP"
        Dim path As String = Server.MapPath("./xmlSubidos/" & filename)
        Dim file1 As System.IO.FileInfo = New System.IO.FileInfo(path)
        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" + file1.Name)
        Response.AddHeader("Content-Length", file1.Length.ToString())
        Response.ContentType = "application/octet-stream"
        Response.WriteFile(file1.FullName)
        Response.End()
    End Sub

    Protected Sub bajaAcuseXml_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bajaAcuseXml.Click
        Call bajarAcuse()
    End Sub

    Protected Sub b1_Click(sender As Object, e As EventArgs) Handles b1.Click
        Timer1.Enabled = False
    End Sub

    Protected Sub mod_Click1(sender As Object, e As EventArgs) Handles btnMod.Click

    End Sub

    Protected Sub mod_Click2(sender As Object, e As EventArgs) Handles btnMod.Click

    End Sub

    Protected Sub mod_Click3(sender As Object, e As EventArgs) Handles btnMod.Click

    End Sub

    Protected Sub mod_Click4(sender As Object, e As EventArgs) Handles btnMod.Click

    End Sub

    Protected Sub mod_Click5(sender As Object, e As EventArgs) Handles btnMod.Click

    End Sub

    Protected Sub export_Click(sender As Object, e As EventArgs) Handles export.Click
        Dim q
        q = "SELECT DISTINCT cast(periodo as int) as periodo FROM ideDetAnual WHERE idAnual=" + id.Text + " order by cast(periodo as int)"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If Not dr.HasRows Then
            dr.Close()
            Response.Write("<script language='javascript'>alert('Nada que exportar');</script>")
            Exit Sub
        Else
            dr.Close()
        End If

        If (Not System.IO.Directory.Exists(Server.MapPath("~") + "exports")) Then
            System.IO.Directory.CreateDirectory(Server.MapPath("~") + "exports")
        End If
        Dim arch = Server.MapPath("~") + "exports/" + Session("curCorreo").ToString + ejercicio + ".xlsx"
        If File.Exists(arch) Then
            File.Delete(arch)
        End If

        Dim oExcel As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application
        Dim oBook As Microsoft.Office.Interop.Excel.Workbook = oExcel.Workbooks.Add
        Dim oSheet As Microsoft.Office.Interop.Excel.Worksheet = oBook.Sheets(1)
        oSheet.Cells(1, 1).value = "DESCRIPCION"
        oSheet.Cells(2, 1).value = "PER"
        oSheet.Cells(2, 2).value = "MES"
        oSheet.Cells(3, 1).value = "CON"
        oSheet.Cells(3, 2).value = "NOMBRES"
        oSheet.Cells(3, 3).value = "AP PATERNO"
        oSheet.Cells(3, 4).value = "AP MATERNO"
        oSheet.Cells(3, 5).value = "RAZON SOCIAL"
        oSheet.Cells(3, 6).value = "RFC"
        oSheet.Cells(3, 7).value = "DOMICILIO"
        oSheet.Cells(3, 8).value = "TELEFONO1 (opc)"
        oSheet.Cells(3, 9).value = "TELEFONO2(opc)"
        oSheet.Cells(3, 10).value = "NUM. DE SOCIO/CLIENTE (opc.)"
        oSheet.Cells(3, 11).value = "MONTO EXCEDENTE "
        oSheet.Cells(3, 12).value = "MONTO DEL CHEQUE CAJA (bancos) "
        oSheet.Cells(3, 13).value = "CURP (Pers. Físicas)"
        oSheet.Cells(3, 14).value = "CORREO ELECTRONICO (opc.)"
        oSheet.Cells(4, 1).value = "CTA"
        oSheet.Cells(4, 2).value = "NUM. DE CUENTA O CONTRATO"
        oSheet.Cells(4, 3).value = "NUM. DE COTITULARES"
        oSheet.Cells(5, 1).value = "COT"
        oSheet.Cells(5, 2).value = "NOMBRE COMPLETO COTITULAR"
        oSheet.Cells(5, 3).value = "RFC COTITULAR"
        oSheet.Cells(5, 4).value = "% DE PROPORCION COTITULAR"
        oSheet.Cells(5, 5).value = "CURP COTITULAR"

        oSheet.Cells(5, 1).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(5, 1).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(5, 2).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(5, 2).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(5, 3).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(5, 3).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(5, 4).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(5, 4).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(5, 5).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(5, 5).Font.Bold = True ' Fuente en negrita

        Dim ren = 6

        q = "SELECT DISTINCT cast(periodo as int) as periodo FROM ideDetAnual WHERE idAnual=" + id.Text + " order by cast(periodo as int)"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        While dr.Read()
            oSheet.Cells(ren, 1).value = "PER"
            oSheet.Cells(ren, 2).value = dr("periodo")
            ren = ren + 1

            Dim q2
            Dim dr2 As SqlDataReader
            q2 = "SELECT DISTINCT d.id, c.nombres,c.ap1,c.ap2,c.razonSocial,c.rfc,c.dom,c.telefono1, c.telefono2,c.curp,c.correo,c.numsociocliente,d.exedente,d.chqCajaMonto FROM ideDetAnual d, contribuyente c WHERE d.idContribuyente=c.id AND idAnual=" + id.Text + " and periodo='" + dr("periodo").ToString + "' order by d.id"
            myCommand2 = New SqlCommand(q2, myConnection)
            dr2 = myCommand2.ExecuteReader()
            While dr2.Read()
                oSheet.Cells(ren, 1).value = "CON"
                Dim ideDetAnual = dr2("id")
                If dr2("razonSocial") <> "" Then
                    oSheet.Cells(ren, 5).value = dr2("razonSocial")
                Else
                    oSheet.Cells(ren, 2).value = dr2("nombres")
                    oSheet.Cells(ren, 3).value = dr2("ap1")
                    oSheet.Cells(ren, 4).value = dr2("ap2")
                End If
                oSheet.Cells(ren, 6).value = dr2("rfc")
                oSheet.Cells(ren, 7).value = dr2("dom")
                If Not dr2("telefono1").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 8).value = dr2("telefono1")
                End If
                If Not dr2("telefono2").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 9).value = dr2("telefono2")
                End If
                If Not dr2("numsociocliente").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 10).value = dr2("numsociocliente")
                End If
                oSheet.Cells(ren, 11).value = dr2("exedente")
                If Not dr2("chqCajaMonto").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 12).value = dr2("chqCajaMonto")
                End If
                If Not DBNull.Value.Equals(dr2("curp")) Then
                    oSheet.Cells(ren, 13).value = dr2("curp")
                End If
                If Not DBNull.Value.Equals(dr2("correo")) Then
                    oSheet.Cells(ren, 14).value = dr2("correo")
                End If
                ren = ren + 1

                Dim dr3 As SqlDataReader
                q2 = "Select * FROM cotitularesCuentaAnual WHERE idideDetAnual=" + ideDetAnual.ToString + " order by id"
                myCommand2 = New SqlCommand(q2, myConnection)
                dr3 = myCommand2.ExecuteReader()
                While dr3.Read()
                    Dim idCotitularesCuentaAnual = dr3("id")
                    oSheet.Cells(ren, 1).value = "CTA"
                    oSheet.Cells(ren, 2).value = dr3("numeroCuenta")
                    oSheet.Cells(ren, 3).value = dr3("numeroCotitulares")
                    ren = ren + 1

                    Dim dr4 As SqlDataReader
                    q2 = "Select * FROM tcotitularAnual WHERE idCotitularesCuentaAnual=" + idCotitularesCuentaAnual.ToString + " order by id"
                    myCommand2 = New SqlCommand(q2, myConnection)
                    dr4 = myCommand2.ExecuteReader()
                    While dr4.Read()
                        oSheet.Cells(ren, 1).value = "COT"
                        oSheet.Cells(ren, 2).value = dr4("nombreCompleto")
                        oSheet.Cells(ren, 3).value = dr4("rfc")
                        oSheet.Cells(ren, 4).value = dr4("proporcion")
                        oSheet.Cells(ren, 5).value = dr4("curp")
                        ren = ren + 1

                    End While
                    dr4.Close()

                End While
                dr3.Close()

            End While
            dr2.Close()
        End While
        dr.Close()

        oSheet.Columns("A:A").EntireColumn.AutoFit()
        oSheet.Columns("B:B").EntireColumn.AutoFit()
        oSheet.Columns("C:C").EntireColumn.AutoFit()
        oSheet.Columns("D:D").EntireColumn.AutoFit()
        oSheet.Columns("E:E").EntireColumn.AutoFit()
        oSheet.Columns("F:F").EntireColumn.AutoFit()
        oSheet.Columns("G:G").EntireColumn.AutoFit()
        oSheet.Columns("H:H").EntireColumn.AutoFit()
        oSheet.Columns("I:I").EntireColumn.AutoFit()
        oSheet.Columns("J:J").EntireColumn.AutoFit()
        oSheet.Columns("K:K").EntireColumn.AutoFit()
        oSheet.Columns("L:L").EntireColumn.AutoFit()
        oSheet.Columns("M:M").EntireColumn.AutoFit()
        oSheet.Columns("N:N").EntireColumn.AutoFit()

        oExcel.Visible = False
        oExcel.UserControl = True
        oExcel.DisplayAlerts = False
        oBook.SaveAs(arch)
        oBook.Close(True)
        oBook = Nothing
        oExcel.Quit()
        oExcel = Nothing

        Response.Clear()
        Response.ClearHeaders()
        Response.ClearContent()
        Response.AddHeader("content-disposition", "attachment; filename=" + Session("curCorreo").ToString + ejercicio + ".xlsx")
        Response.ContentType = "application/vnd.ms-excel"
        Response.WriteFile(arch)
        Response.End()

        File.Delete(arch)

        Dim MSG As String = "<script language='javascript'>alert('Descargo exitoso hacia su equipo, revise su carpeta de descargas');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)

    End Sub

    Protected Sub verDatos_Click(sender As Object, e As EventArgs) Handles verDatos.Click
        cargaGrid()
    End Sub

    Protected Sub mod_Click6(sender As Object, e As EventArgs) Handles btnMod.Click

    End Sub

    Protected Sub mod_Click7(sender As Object, e As EventArgs) Handles btnMod.Click

    End Sub

    Protected Sub mod_Click8(sender As Object, e As EventArgs) Handles btnMod.Click

    End Sub

    Protected Sub mod_Click9(sender As Object, e As EventArgs) Handles btnMod.Click

    End Sub

    Protected Sub mod_Click10(sender As Object, e As EventArgs) Handles btnMod.Click

    End Sub

    Protected Sub mod_Click11(sender As Object, e As EventArgs) Handles btnMod.Click

    End Sub

    Protected Sub mod_Click12(sender As Object, e As EventArgs) Handles btnMod.Click

    End Sub

    Protected Sub mod_Click13(sender As Object, e As EventArgs) Handles btnMod.Click

    End Sub

    Private Sub lost_Click(sender As Object, e As EventArgs) Handles lost.Click
        If Request.QueryString("pl") <> "ANUAL DESDE 2014 CEROS NORMAL" Then
            If Session("GidAnual") = 0 Then 'no hay anual del ejercicio -> insertar anual vacia
                Call insertaAnualVacia()
                Call insertaMensualVacia() 'todas las mensuales del año
            Else
                Call insertaMensualParciales() 'donde no haya mensuales las crea vacias
            End If
        End If

        myCommand2 = New SqlCommand("UPDATE ideAnual SET lost=1, estado='ACEPTADA', idContrato=" + idContrato + ", viaImportacion=1, guardadaUsuario=1, acuseSolicitado=0, fechaEnvio='" + fenvio.Text.Trim.Replace("_", " ").Remove(13, 1).Insert(13, ":").Remove(16, 1).Insert(16, ":") + "', evidEnvio='" + resul.Text + "' WHERE id=" + id.Text, myConnection)
        myCommand2.ExecuteNonQuery()

        If Request.QueryString("pl") <> "PREMIUM" Then
            myCommand = New SqlCommand("UPDATE contratos SET nDeclHechas=nDeclHechas-1 WHERE id=" + idContrato, myConnection)
            myCommand.ExecuteNonQuery()
        End If

        Dim MSG As String = "<script language='javascript'>alert('ok');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
    End Sub

    Protected Sub mod_Click14(sender As Object, e As EventArgs) Handles btnMod.Click

    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub

    Private Sub verEvidEnvio_Click(sender As Object, e As EventArgs) Handles verEvidEnvio.Click
        Dim q, evi
        q = "SELECT evidEnvio FROM ideAnual WHERE id=" + id.Text
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If Not dr.HasRows Then
            dr.Close()
            Response.Write("<script language='javascript'>alert('revise su correo con la fecha de presentacion, si es que acaso la envio');</script>")
            Exit Sub
        Else
            dr.Read()
            evi = dr("evidEnvio")
            If evi.ToString = "" Then
                evi = "revise su correo con la fecha de presentacion, si es que acaso la envio"
            End If
            dr.Close()
        End If
        Dim MSG As String = "<script language='javascript'>alert('" + Server.HtmlEncode(evi) + "');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
    End Sub
End Class