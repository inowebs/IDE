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

Public Class WebForm12
    Inherits System.Web.UI.Page

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function FindWindow( _
     ByVal lpClassName As String, _
     ByVal lpWindowName As String) As IntPtr
    End Function

    <DllImport("user32.dll", EntryPoint:="FindWindow", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function FindWindowByClass( _
     ByVal lpClassName As String, _
     ByVal zero As IntPtr) As IntPtr
    End Function


    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function FindWindowEx(ByVal parentHandle As IntPtr, ByVal childAfter As IntPtr, _
                ByVal lpszClass As String, ByVal lpszWindow As String) As IntPtr
    End Function


    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByVal lParam As String) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function    'buton clic pero esperaria ante un msgbox 

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function SendNotifyMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByVal lParam As Integer) As IntPtr
    End Function    'buton clic sin esperar ante un msgbox 


    <DllImport("user32.dll", EntryPoint:="SetActiveWindow")> _
    Private Shared Function SetActiveWindow(ByVal hWnd As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)> _
    Private Shared Sub GetClassName(ByVal hWnd As System.IntPtr, _
   ByVal lpClassName As System.Text.StringBuilder, ByVal nMaxCount As Integer)
        ' Leave function empty     
    End Sub

    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim myCommand2 As SqlCommand
    Dim myCommand3 As SqlCommand
    Dim myCommand4 As SqlCommand
    Dim myCommand5 As SqlCommand
    Dim dr As SqlDataReader
    Dim ejercicio
    Dim mes
    Dim comple
    Dim savePath
    Dim nomArchMens
    Dim nomArchMensSinPath
    Dim ContNomArchMens
    Dim idContrato
    Dim pl

    Sub AddFileSecurity(ByVal fileName As String, ByVal account As String, _
            ByVal rights As FileSystemRights, ByVal controlType As AccessControlType)

        Dim fSecurity As FileSecurity = File.GetAccessControl(fileName)
        Dim accessRule As FileSystemAccessRule = New FileSystemAccessRule(account, rights, controlType)
        fSecurity.AddAccessRule(accessRule)
        File.SetAccessControl(fileName, fSecurity)
    End Sub


    Private Sub verEnteroPropio(ByVal visibilidad)
        fedFechaEntero.Visible = visibilidad
        fedImpto.Visible = visibilidad
        fedNumOper.Visible = visibilidad
        enteroPropInstit.Visible = visibilidad
        enteroPropInstitRfc.Visible = visibilidad
        fedFechaRecaudacion.Visible = visibilidad

        lblfedFechaEntero.Visible = visibilidad
        lblfedImpto.Visible = visibilidad
        lblfedNumOper.Visible = visibilidad
        lblenteroPropInstit.Visible = visibilidad
        lblenteroPropInstitRfc.Visible = visibilidad
        lblfedFechaRecaudacion.Visible = visibilidad
    End Sub

    Private Sub habilitacionTotales(ByVal valor)
        impteExcedente.Enabled = valor
        impteDeterminado.Enabled = valor
        impteRecaudado.Enabled = valor
        imptePendienteRecaudar.Enabled = valor
        impteRemanente.Enabled = valor
        impteCheques.Enabled = valor
        impteSaldoPendienteRecaudar.Enabled = valor

        If mes.ToString = "1" Then
            impteRemanente.Text = 0
            impteRemanente.Enabled = False
        Else
            impteRemanente.Enabled = valor
        End If
    End Sub

    Private Sub muestraComple()
        fechaPresentacionAnt.Visible = True
        lblFechaPresentacionAnt.Visible = True
        numOperAnt.Visible = True
        lblNumOperAnt.Visible = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';MultipleActiveResultSets=True")
        myConnection.Open()
        myCommand = New SqlCommand("set dateformat ymd", myConnection)
        myCommand.ExecuteNonQuery()

        If IsNothing(Session("curCorreo")) = True Then
            Response.Write("<script language='javascript'>alert('Expiró su sesión');</script>")
            Session.Abandon()
            Response.Write("<script language='javascript'>location.href='Login.aspx';</script>")
            Exit Sub
        End If

        If Not IsPostBack Then  '1a vez    
            If Left(Request.ServerVariables("REMOTE_ADDR"), 10) = "192.168.0." Or Left(Request.ServerVariables("REMOTE_ADDR"), 10) = "127.0.0.1" Or Session("runAsAdmin") = "1" Then 'red local
                chkPostpago.Visible = True
            Else
                chkPostpago.Visible = False
            End If
        End If

        ejercicio = Request.QueryString("ejercicio")
        idContrato = Request.QueryString("contra")
        pl = Request.QueryString("pl")
        mes = Request.QueryString("mes")

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

        'Page.ClientScript.RegisterStartupScript(GetType(Microsoft.Office.Interop.Excel.Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + divScroll.ClientID + "');", True)
        btnEnviarDeclaracion.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(btnEnviarDeclaracion, "") + ";")
        btnContingencia.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(btnContingencia, "") + ";")
        importMensXls.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(importMensXls, "") + ";")
        importarXml.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(importarXml, "") + ";")
        bajarAcuseExcel.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(bajarAcuseExcel, "") + ";")
        bajaAcuseXml.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(bajaAcuseXml, "") + ";")


        If Request.QueryString("subop") = "1" Then  'xml
            impteRemanente.Visible = False
            impteDeterminado.Visible = False
            impteExcedente.Visible = False
            imptePendienteRecaudar.Visible = False
            impteRecaudado.Visible = False
            impteCheques.Visible = False
            impteSaldoPendienteRecaudar.Visible = False
        Else
            impteRemanente.Visible = True
            impteDeterminado.Visible = True
            impteExcedente.Visible = True
            imptePendienteRecaudar.Visible = True
            impteRecaudado.Visible = True
            'impteCheques.Visible = True
            impteSaldoPendienteRecaudar.Visible = True
        End If

        If Not IsPostBack Then  '1a vez
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
            encab.Text = "Declaración Mensual: Ejercicio " + ejercicio + ", Mes " + mes + Vcomple

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
                        modi.Visible = False
                        MultiView1.ActiveViewIndex = Int32.Parse(1)
                    Else 'edit
                        MultiView1.ActiveViewIndex = Int32.Parse(2)
                    End If
                    If pl = "CEROS" Then 'edit
                        verEnteroPropio(False)
                        habilitacionTotales(False)
                        lblFechaCorte.Visible = False
                        fechaCorte.Visible = False
                        modi.Visible = False
                    End If
                    btnEnviarDeclaracion.Visible = True
                    btnContingencia.Visible = True
                Case "1" 'ceros 'creación
                    MultiView1.ActiveViewIndex = Int32.Parse(3)
                    verEnteroPropio(False)
                    habilitacionTotales(False)
                    lblFechaCorte.Visible = False
                    fechaCorte.Visible = False
                    Call limpiaMes()
                    modi.Visible = False
                    btnEnviarDeclaracion.Visible = True
                    btnContingencia.Visible = True

                Case "2" 'consultar
                    If Request.QueryString("subop") = "0" Then  'xls
                        MultiView1.ActiveViewIndex = Int32.Parse(4)
                    Else 'xml
                        MultiView1.ActiveViewIndex = Int32.Parse(5)
                    End If
                    cargaGrid()
                    back.Visible = False
                    modi.Visible = False
                    If pl = "CEROS" Then 'edit
                        verEnteroPropio(False)
                        habilitacionTotales(False)
                        lblFechaCorte.Visible = False
                        fechaCorte.Visible = False
                    End If

                    Session("numOperAcuse") = ""
                    Session("fechaPresentacionAcuse") = ""
                    Session("rfcAcuse") = ""
                    Session("denominacionAcuse") = ""
                    Session("recaudadoAcuse") = ""
                    Session("enteradoAcuse") = ""
                    Session("ejercicioAcuse") = ""
                    Session("periodoAcuse") = ""
                    Session("tipoAcuse") = ""
                    Session("folioAcuse") = ""
                    Session("archivoAcuse") = ""
                    Session("selloAcuse") = ""

                    btnEnviarDeclaracion.Visible = False
                    btnContingencia.Visible = False

            End Select

            If Session("GidAnual") <> 0 Then
                Dim dr2 As SqlDataReader
                q = "SELECT * FROM ideMens WHERE idAnual=" + Session("GidAnual").ToString + " and id=" + Session("GidMens").ToString
                myCommand2 = New SqlCommand(q, myConnection)
                dr2 = myCommand2.ExecuteReader()
                If dr2.Read() Then
                    Call cargaMes(dr2)
                    If mes.ToString = "1" Then
                        If CDbl(impteRemanente.Text) > 0 Then
                            Response.Write("<script language='javascript'>alert('Cambiando a 0 el importe remanente de IDE para los meses de Enero, deacuerdo a la RMF 2013 de Julio');</script>")
                        End If
                        impteRemanente.Text = 0
                        impteRemanente.Enabled = False
                    End If
                Else
                    Call limpiaMes()
                End If
                dr2.Close()
            Else
                Call limpiaMes()
            End If
            idAnual.Text = Session("GidAnual").ToString
            id.Text = Session("GidMens").ToString

            progressbar.Style("width") = "0px"
            statusImport.Text = ""

            q = "SELECT esInstitCredito FROM clientes WHERE id=" + Session("GidCliente").ToString
            myCommand = New SqlCommand(q, myConnection)
            dr = myCommand.ExecuteReader()
            dr.Read()
            If dr("esInstitCredito").Equals(True) Then
                lblimpteCheques.Visible = True
                impteCheques.Visible = True
            Else
                lblimpteCheques.Visible = False
                impteCheques.Visible = False
            End If
            dr.Close()
        End If


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
        'nomArchMens = "C:\SAT\" + casfim + "\" + "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + ".XML"
        'ContNomArchMens = "C:\SAT\" + casfim + "\" + "ContM-" + ejercicio.ToString + "-" + mes.ToString + tipo + ".XML"
        'nomArchMensSinPath = "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + ".XML"
    End Sub

    Private Sub cargaGrid()
        'SqlDataSource3.ConnectionString = "$ ConnectionStrings:ideConnectionString "
        SqlDataSource3.SelectCommand = "SELECT d.id,nombres,ap1,ap2,razonSocial,rfc,Dom,telefono1,telefono2,numSocioCliente,sumaDeposEfe,exedente,determinado,recaudado,pendienteRecaudar,remanente,impteSaldoPendienteRecaudar,chqCajaMonto,chqCajaMontoRecaudado FROM ideDet d, contribuyente c WHERE c.id=d.idContribuyente AND idAnual=" + Session("GidAnual").ToString + " AND idMens=" + Session("GidMens").ToString + " order by case when razonSocial = '' then nombres+ap1+ap2 else razonSocial end"
        GridView3.DataBind()
        nRegs.Text = FormatNumber(GridView3.Rows.Count.ToString, 0) + " Registros ordenados por nombre/razón social (se omiten decimales)"
    End Sub

    Private Sub limpiaMes()
        Dim q
        impteExcedente.Text = 0
        impteDeterminado.Text = 0
        impteRecaudado.Text = 0
        imptePendienteRecaudar.Text = 0
        impteRemanente.Text = 0
        impteCheques.Text = 0
        impteSaldoPendienteRecaudar.Text = 0
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

        If esComple = 1 Then
            normalComplementaria.Text = "COMPLEMENTARIA"
            q = "SELECT TOP 1 numOper,fechaPresentacion FROM ideMens WHERE mes='" + mes + "' and idAnual=" + Session("GidAnual").ToString + " and numOper<>'0' order by id desc"
            myCommand = New SqlCommand(q, myConnection)
            myCommand.CommandText = q
            dr = myCommand.ExecuteReader()
            dr.Read()
            fechaPresentacionAnt.Text = dr("fechaPresentacion")
            numOperAnt.Text = dr("numOper")
            dr.Close()
        Else
            numOperAnt.Text = 0
            fechaPresentacionAnt.Text = Left(Now(), 10).ToString
            normalComplementaria.Text = "NORMAL"
        End If
        fechaCorte.Text = Left(DateSerial(ejercicio, mes + 1, 0), 10).ToString

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
            ideConfPorcen.Text = FormatNumber(dr4("porcen"), 2)
        Else
            ideConfLimite.Text = FormatNumber(dr4("ideLim"), 2)
            ideConfPorcen.Text = FormatNumber(dr4("idePorcen"), 2)
        End If
        dr4.Close()

        fedFechaEntero.Text = Left(Now(), 10).ToString
        If esComple = 1 And ejercicio.ToString + "-" + CInt(mes).ToString("0#") + "-01" < "2013-06-01" Then 'complem previa a v2.0
            fedFechaRecaudacion.Text = "01/01/2012"
        Else
            fedFechaRecaudacion.Text = Left(DateSerial(ejercicio, mes + 1, 0), 10).ToString
        End If

        fedImpto.Text = 0
        fedNumOper.Text = 0
        enteroPropInstit.Text = ""
        enteroPropInstitRfc.Text = ""
        id.Text = 0
        estado.Text = "VACIA"
    End Sub

    Private Sub cargaMes(ByVal dr2)
        impteExcedente.Text = CDbl(dr2("impteExcedente")).ToString("###,###,###,##0")
        impteDeterminado.Text = CDbl(dr2("impteDeterminado")).ToString("###,###,###,##0")
        impteRecaudado.Text = CDbl(dr2("impteRecaudado")).ToString("###,###,###,##0")
        imptePendienteRecaudar.Text = CDbl(dr2("imptePendienteRecaudar")).ToString("###,###,###,##0")
        impteRemanente.Text = CDbl(dr2("impteRemanente")).ToString("###,###,###,##0")
        impteCheques.Text = CDbl(dr2("impteCheques")).ToString("###,###,###,##0")
        impteSaldoPendienteRecaudar.Text = CDbl(dr2("impteSaldoPendienteRecaudar")).ToString("###,###,###,##0")
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
        fechaCorte.Text = dr2("fechaCorte")
        normalComplementaria.Text = dr2("normalComplementaria")
        estado.Text = dr2("estado")
        Dim dr3 As SqlDataReader
        Dim q
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
            ideConfPorcen.Text = FormatNumber(dr4("porcen"), 2)
        Else
            ideConfLimite.Text = FormatNumber(dr4("ideLim"), 2)
            ideConfPorcen.Text = FormatNumber(dr4("idePorcen"), 2)
        End If
        dr4.Close()

        fedFechaEntero.Text = dr2("fedFechaEntero")
        If DBNull.Value.Equals(dr2("fedFechaRecaudacion")) Then
            fedFechaRecaudacion.Text = "01/01/2012"
        Else
            fedFechaRecaudacion.Text = dr2("fedFechaRecaudacion")
        End If
        fedImpto.Text = CDbl(dr2("fedImpto")).ToString("###,###,###,##0")
        fedNumOper.Text = dr2("fedNumOper")
        enteroPropInstit.Text = dr2("enteroPropInstit")
        enteroPropInstitRfc.Text = dr2("enteroPropInstitRfc")
        id.Text = dr2("id")

        cargaGrid()
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
        q = "SELECT id FROM ideConf WHERE limite='" + ideConfLimite.Text + "' and porcen='" + ideConfPorcen.Text + "'"
        myCommand2 = New SqlCommand(q, myConnection)
        dr2 = myCommand2.ExecuteReader()
        dr2.Read()
        idIdeConf = dr2("id")
        dr2.Close()

        q = "INSERT INTO ideAnual(ejercicio,nOpers,impteExcedente,impteDeterminado,impteRecaudado,imptePendienteRecaudar,numOper,fechaPresentacion,normalComplementaria,idRepresentanteLegal,idIdeConf,idCliente,viaImportacion) VALUES('" + ejercicio.ToString + "',0,0,0,0,0,'0','" + Now().ToString("yyyy-MM-dd") + "','NORMAL'," + idRepresentanteLegal.Text.ToString + "," + idIdeConf.ToString + "," + Session("GidCliente").ToString + ",0)"
        myCommand3 = New SqlCommand(q, myConnection)
        myCommand3.ExecuteNonQuery()

        Dim dr3 As SqlDataReader
        q = "SELECT TOP 1 id FROM ideAnual WHERE ejercicio='" + ejercicio + "' and idCliente=" + Session("GidCliente").ToString + " order by id desc"
        myCommand4 = New SqlCommand(q, myConnection)
        dr3 = myCommand4.ExecuteReader()
        dr3.Read()
        idAnual.Text = dr3("id")
        Session("GidAnual") = idAnual.Text
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
        q = "SELECT id FROM ideConf WHERE limite='" + ideConfLimite.Text + "' and porcen='" + ideConfPorcen.Text + "'"
        myCommand2 = New SqlCommand(q, myConnection)
        dr2 = myCommand2.ExecuteReader()
        dr2.Read()
        idIdeConf = dr2("id")
        dr2.Close()

        q = "INSERT INTO ideMens(idAnual,mes,impteExcedente,impteDeterminado,impteRecaudado,imptePendienteRecaudar,impteRemanente,impteCheques,fechaPresentacion,fechaCorte,normalComplementaria,idRepresentanteLegal,idIdeConf,fedFechaEntero,fedImpto,fedNumOper,enteroPropInstit,enteroPropInstitRfc,viaImportacion,impteSaldoPendienteRecaudar,fedFechaRecaudacion) VALUES(" + Session("GidAnual").ToString + ",'" + mes.ToString + "',0,0,0,0,0,0,'" + Now().ToString("yyyy-MM-dd") + "','" + Now().ToString("yyyy-MM-dd") + "','" + normalComplementaria.Text + "'," + idRepresentanteLegal.Text.ToString + "," + idIdeConf.ToString + ",'" + Now().ToString("yyyy-MM-dd") + "',0,'0','','',0,0,'" + Now().ToString("yyyy-MM-dd") + "')"
        myCommand3 = New SqlCommand(q, myConnection)
        myCommand3.ExecuteNonQuery()

        Dim dr3 As SqlDataReader
        q = "SELECT TOP 1 id FROM ideMens WHERE mes='" + mes + "' and idAnual=" + Session("GidAnual").ToString + " order by id desc"
        myCommand2 = New SqlCommand(q, myConnection)
        dr3 = myCommand2.ExecuteReader()
        dr3.Read()
        id.Text = dr3("id")
        Session("GidMens") = id.Text
        dr3.Close()
    End Sub

    Protected Sub importMensXls_Click(ByVal sender As Object, ByVal e As EventArgs) Handles importMensXls.Click
        If Not FileUpload1.HasFile Then
            Response.Write("<script language='javascript'>alert('No especificó el archivo a subir');</script>")
            Exit Sub
        End If

        Dim fileName As String = Server.HtmlEncode(FileUpload1.FileName)
        Dim extension As String = System.IO.Path.GetExtension(fileName)
        If Not (extension = ".xls" Or extension = ".xlsx") Then
            Response.Write("<script language='javascript'>alert('El archivo debe ser formato Excel');</script>")
            Exit Sub
        End If

        If InStr(fileName, "á") > 0 Or InStr(fileName, "é") > 0 Or InStr(fileName, "í") > 0 Or InStr(fileName, "ó") > 0 Or InStr(fileName, "ú") > 0 Or InStr(fileName, "Á") > 0 Or InStr(fileName, "É") > 0 Or InStr(fileName, "Í") > 0 Or InStr(fileName, "Ó") > 0 Or InStr(fileName, "Ú") > 0 Then
            Response.Write("<script language='javascript'>alert('Cambie el nombre del archivo para que no tenga acentos e intente de nuevo');</script>")
            Exit Sub
        End If


        'If ejercicio.ToString & "-" & CInt(mes).ToString("0#") & "-01" > "2013-06-01" Then 'subsecuentes impteSaldoPendienteRecaudar
        '    'las decl de Jul 2013 en adel, requieren tener aceptadas las de Jun 2013 hasta la mensual inmediata anterior
        '    Dim elmes
        '    For año = 2013 To CDbl(DatePart(DateInterval.Year, Now()))
        '        If año = 2013 Then
        '            elmes = 6
        '        Else
        '            elmes = 1
        '        End If
        '        myCommand = New SqlCommand("SELECT mes FROM ideMens m, ideAnual a WHERE a.id=m.idAnual AND m.estado<>'ACEPTADA' AND a.ejercicio='" + año.ToString + "' AND CAST(m.mes AS INT)>=" + elmes.ToString + " AND CAST(m.mes AS INT)<=" & (CDbl(DatePart(DateInterval.Month, Now())) - 1).ToString("0#"), myConnection)
        '        dr = myCommand.ExecuteReader()
        '        If dr.Read() Then
        '            Response.Write("<script language='javascript'>alert('Requiere tener presentada y aceptada la declaración del ejercicio " + año.ToString + ", mes " + dr("mes").ToString + "');</script>")
        '            dr.Close()
        '        Else
        '            dr.Close()
        '        End If
        '    Next año
        '    Exit Sub
        'End If

        progressbar.Style("width") = "0px"
        statusImport.Text = ""
        descrip.Text = ""

        If Session("GidAnual") = 0 Then 'no hay anual del ejercicio -> insertar anual vacia
            Call insertaAnualVacia()
            Call insertaMensualVacia()
        Else
            If Session("GidMens") = 0 Then 'sin mensual ->crearla
                Call insertaMensualVacia()
            Else
            End If
        End If


        Dim q, casfim
        q = "SELECT casfim FROM clientes WHERE id=" + Session("GidCliente").ToString
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        casfim = dr("casfim")
        dr.Close()

        savePath = "C:\SAT\" + casfim + "\" 'pend: en su casfim
        savePath += Server.HtmlEncode(FileUpload1.FileName)
        Try
            FileUpload1.SaveAs(savePath)
        Catch ex As Exception
            Dim MSG = "<script language='javascript'>alert('" + ex.Message + "');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Exit Sub
        Finally
            FileUpload1.PostedFile.InputStream.Flush()
            FileUpload1.PostedFile.InputStream.Close()
            FileUpload1.FileContent.Dispose()
            FileUpload1.Dispose()
        End Try

        'AddFileSecurity(savePath, Session("identidad"), FileSystemRights.ReadData, AccessControlType.Allow)
        'AddFileSecurity(savePath, "IIS_WPG", FileSystemRights.ReadData, AccessControlType.Allow)

        If importarIDEmens() > 0 Then
            cargaGrid()
            Call refrescaTotalesMens()
            If GridView3.Rows.Count = 0 Then    'importó en ceros
                verEnteroPropio(False)
            Else
                verEnteroPropio(True)
            End If
            statusImport.Text = " Importación IDE realizada "
        End If
        File.Delete(savePath) 'el de excel
        If normalComplementaria.Text = "COMPLEMENTARIA" Then
            q = "UPDATE ideMens SET fechaPresentacionAnt='" + Convert.ToDateTime(fechaPresentacionAnt.Text).ToString("yyyy-MM-dd") + "', numOperAnt='" + numOperAnt.Text + "', normalComplementaria='COMPLEMENTARIA' WHERE id=" + id.Text
            myCommand3 = New SqlCommand(q, myConnection)
            myCommand3.ExecuteNonQuery()
        End If
        'ClientScript.RegisterStartupScript(Me.GetType(), "Load", "<script type='text/javascript'>window.parent.location.href = 'decla.aspx'; </script>")
    End Sub

    Private Sub creaTagsMens()
        Dim reprLegalAp1, reprLegalAp2, reprLegalRfc, reprLegalNombres, tipo, idArch
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

        If normalComplementaria.Text = "NORMAL" Then
            tipo = "N"
            idArch = ""
        Else
            tipo = "C"
            idArch = id.Text
        End If

        'M=mensual
        Dim casfim, vRfc, vempresa, esInstitCredito
        q = "SELECT casfim,rfcDeclarante,razonSoc,esInstitCredito FROM clientes WHERE id=" + Session("GidCliente").ToString
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
        dr.Close()

        nomArchMens = "C:\SAT\" + casfim + "\" + "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML"
        nomArchMensSinPath = "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML"

        If File.Exists(nomArchMens) Then
            File.Delete(nomArchMens)
        End If

        Dim archivo As StreamWriter = File.CreateText(nomArchMens)
        archivo.WriteLine("<?xml version='1.0' encoding='UTF-8'?>")
        archivo.WriteLine("    <DeclaracionInformativaMensualIDE xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:altova='http://www.altova.com/xml-schema-extensions' xsi:noNamespaceSchemaLocation='C:/SAT/ide_20130430.xsd' version='2.0' rfcDeclarante='" + Left(vRfc, 12) + "' denominacion='" + vempresa + "'>")
        archivo.WriteLine("                <RepresentanteLegal rfc='" + reprLegalRfc + "'>") 'RFC mayusculas
        archivo.WriteLine("                    <Nombre>")
        archivo.WriteLine("                        <Nombres>" + reprLegalNombres + "</Nombres>")
        archivo.WriteLine("                        <PrimerApellido>" + reprLegalAp1 + "</PrimerApellido>")
        If reprLegalAp2 <> "" Then
            archivo.WriteLine("                        <SegundoApellido>" + reprLegalAp2 + "</SegundoApellido>")
        End If
        archivo.WriteLine("                    </Nombre>")
        archivo.WriteLine("                </RepresentanteLegal>")
        If tipo = "N" Then
            archivo.WriteLine("                <Normal ejercicio='" + ejercicio.ToString + "' periodo='" + mes.ToString + "'></Normal>")
        Else
            archivo.WriteLine("                <Complementaria ejercicio='" + ejercicio.ToString + "' periodo='" + mes.ToString + "' opAnterior='" + numOperAnt.Text.Trim + "' fechaPresentacion='" + CDate(fechaPresentacionAnt.Text.Trim).ToString("yyyy-MM-dd") + "'></Complementaria>")
        End If
        If esInstitCredito = 1 Then
            archivo.WriteLine("                <InstitucionDeCredito>")
        Else
            archivo.WriteLine("                <InstitucionDistintaDeCredito>")
        End If
        If GridView3.Rows.Count > 0 Then
            archivo.WriteLine("                                <ReporteDeRecaudacionYEnteroDiaria fechaDeCorte='" + CDate(fechaCorte.Text.Trim).ToString("yyyy-MM-dd") + "'>")
            Dim ideDet, nombres, ap1, ap2, razonSocial, rfc, Dom, numSocioCliente, sumaDeposEfe, montoExcedente, impuestoDeterminado, impuestoRecaudado, recaudacionPendiente, remanentePeriodosAnteriores, chqCajaMonto, chqCajaMontoRecaudado, telefono1, telefono2, numeroCuenta, saldoPendienteRecaudar, rfcCotitular, nombreCompletoCotitular, idCotitularesCuenta
            Dim dr2 As SqlDataReader
            For i = 0 To CDbl(GridView3.Rows.Count) - 1
                ideDet = GridView3.Rows(i).Cells(1).Text
                myCommand = New SqlCommand("SELECT * FROM cotitularesCuenta WHERE idideDet=" + ideDet.ToString + " ORDER BY id", myConnection)
                dr = myCommand.ExecuteReader()

                archivo.WriteLine("                                 <RegistroDeDetalle>")
                nombres = SecurityElement.Escape(GridView3.Rows(i).Cells(2).Text)
                If nombres = "&nbsp;" Then
                    nombres = ""
                End If
                ap1 = SecurityElement.Escape(GridView3.Rows(i).Cells(3).Text)
                If ap1 = "&nbsp;" Then
                    ap1 = ""
                End If
                ap2 = SecurityElement.Escape(GridView3.Rows(i).Cells(4).Text)
                If ap2 = "&nbsp;" Then
                    ap2 = ""
                End If
                razonSocial = SecurityElement.Escape(GridView3.Rows(i).Cells(5).Text)
                If razonSocial = "&nbsp;" Then
                    razonSocial = ""
                End If
                rfc = GridView3.Rows(i).Cells(6).Text
                If rfc = "&nbsp;" Then
                    rfc = ""
                End If
                Dom = SecurityElement.Escape(GridView3.Rows(i).Cells(7).Text)
                If Dom = "&nbsp;" Then
                    Dom = ""
                End If
                telefono1 = GridView3.Rows(i).Cells(8).Text
                If telefono1 = "&nbsp;" Then
                    telefono1 = "000000000000000"
                End If
                If Len(telefono1) < 15 Then
                    telefono1 = telefono1.PadLeft(15, "0"c)
                End If
                telefono2 = GridView3.Rows(i).Cells(9).Text
                If telefono2 = "&nbsp;" Then
                    telefono2 = "000000000000000"
                End If
                If Len(telefono2) < 15 Then
                    telefono2 = telefono2.PadLeft(15, "0"c)
                End If
                numSocioCliente = GridView3.Rows(i).Cells(10).Text
                If numSocioCliente = "&nbsp;" Then
                    numSocioCliente = ""
                End If
                sumaDeposEfe = Replace(Fix(GridView3.Rows(i).Cells(11).Text).ToString, ",", "")
                montoExcedente = Replace(Fix(GridView3.Rows(i).Cells(12).Text).ToString, ",", "")
                impuestoDeterminado = Replace(Fix(GridView3.Rows(i).Cells(13).Text).ToString, ",", "")
                impuestoRecaudado = Replace(Fix(GridView3.Rows(i).Cells(14).Text).ToString, ",", "")
                recaudacionPendiente = Replace(Fix(GridView3.Rows(i).Cells(15).Text).ToString, ",", "")
                remanentePeriodosAnteriores = Replace(Fix(GridView3.Rows(i).Cells(16).Text).ToString, ",", "")
                saldoPendienteRecaudar = Replace(Fix(GridView3.Rows(i).Cells(17).Text).ToString, ",", "")
                chqCajaMonto = Replace(Fix(GridView3.Rows(i).Cells(18).Text).ToString, ",", "")
                chqCajaMontoRecaudado = Replace(Fix(GridView3.Rows(i).Cells(19).Text).ToString, ",", "")
                If razonSocial = "" Then
                    archivo.WriteLine("                                     <PersonaFisica telefono1='" & Right(telefono1, 15) & "' telefono2='" & Right(telefono2, 15) & "'>") '15 letrasNumeros
                    archivo.WriteLine("                                         <Nombre>")
                    archivo.WriteLine("                                             <Nombres>" & Left(nombres, 40) & "</Nombres>")
                    archivo.WriteLine("                                             <PrimerApellido>" & Left(ap1, 40) & "</PrimerApellido>")
                    If ap2 <> "" Then
                        archivo.WriteLine("                                             <SegundoApellido>" & Left(ap2, 40) & "</SegundoApellido>")
                    End If
                    archivo.WriteLine("                                         </Nombre>")
                    archivo.WriteLine("                                          <Domicilio>")
                    archivo.WriteLine("                                                 <DomicilioCompleto>" & Left(Dom, 150) & "</DomicilioCompleto>")
                    archivo.WriteLine("                                          </Domicilio>")
                    While dr.Read()
                        numeroCuenta = dr("numeroCuenta").ToString
                        archivo.WriteLine("                                      <numeroCuenta>" & numeroCuenta & "</numeroCuenta>") 'o #contrato string
                    End While
                    dr.Close()
                    archivo.WriteLine("                                     </PersonaFisica>")
                Else
                    archivo.WriteLine("                                     <PersonaMoral rfc='" + Left(rfc, 12) + "' telefono1='" & Right(telefono1, 15) & "' telefono2='" & Right(telefono2, 15) & "'>") 'tels 15 letrasNumeros
                    archivo.WriteLine("                                         <Denominacion>" & Left(razonSocial, 250) & "</Denominacion>")
                    archivo.WriteLine("                                          <Domicilio>")
                    archivo.WriteLine("                                                 <DomicilioCompleto>" & Left(Dom, 150) & "</DomicilioCompleto>")
                    archivo.WriteLine("                                          </Domicilio>")
                    Dim co = 0
                    While dr.Read()
                        co = 1
                        numeroCuenta = dr("numeroCuenta").ToString
                        archivo.WriteLine("                                      <numeroCuenta>" & numeroCuenta & "</numeroCuenta>") 'o #contrato string
                    End While
                    dr.Close()
                    If co = 0 Then
                        archivo.WriteLine("                                      <numeroCuenta>9</numeroCuenta>") 'para el contribuyente cuando todo es 0 excepto el saldopendientederecaudar
                    End If
                    archivo.WriteLine("                                     </PersonaMoral>")
                End If
                If impuestoDeterminado <> "0" Or saldoPendienteRecaudar <> "0" Or remanentePeriodosAnteriores <> "0" Then
                    archivo.WriteLine("                                     <DepositoEnEfectivo montoExcedente='" & CStr(montoExcedente) & "' impuestoDeterminado='" & CStr(impuestoDeterminado) & "' impuestoRecaudado='" & CStr(impuestoRecaudado) & "' recaudacionPendiente='" & CStr(recaudacionPendiente) & "' remanentePeriodosAnteriores='" & CStr(remanentePeriodosAnteriores) & "' saldoPendienteRecaudar='" & CStr(saldoPendienteRecaudar) & "'></DepositoEnEfectivo>") 'saldo pendiente acumulado a la fecha de presentación de la declaración informativa mensual:long 12dig
                End If
                If esInstitCredito = 1 Then
                    If chqCajaMonto <> "0" Then
                        archivo.WriteLine("                                 <ChequeDeCaja montoCheque='" & CStr(chqCajaMonto) & "' montoRecaudado='" & CStr(chqCajaMontoRecaudado) & "'></ChequeDeCaja>") 'opcional
                    End If
                End If

                '(COT) Cotitulares es opcional en el xml f numCotitulares, pero en el excel debe indicarse el ren de CTA
                myCommand = New SqlCommand("SELECT * FROM cotitularesCuenta WHERE idideDet=" + ideDet.ToString + " ORDER BY id", myConnection)
                dr = myCommand.ExecuteReader()
                While dr.Read()
                    If dr("numeroCotitulares").ToString = "0" Then
                        Continue While
                    End If
                    idCotitularesCuenta = dr("id")
                    archivo.WriteLine("                                         <Cotitulares numeroCuenta='" & CStr(dr("numeroCuenta")) & "' numeroCotitulares='" & CStr(dr("numeroCotitulares")) & "'>") '#cta o contrato
                    myCommand2 = New SqlCommand("SELECT * FROM tCotitular WHERE idCotitularesCuenta=" + idCotitularesCuenta.ToString + " ORDER BY id", myConnection)
                    dr2 = myCommand2.ExecuteReader()
                    While dr2.Read()
                        archivo.WriteLine("                                             <tCotitular RFC='" & dr2("rfc").ToString & "' Proporcion='" & CDbl(dr2("proporcion")).ToString("###.0000") & "'>") 'proporc c 4 decimales, rfc:9-13cars. RFC mayusculas
                        archivo.WriteLine("                                                 <Nombre>")
                        archivo.WriteLine("                                                     <NombreCompleto>" & SecurityElement.Escape(dr2("nombreCompleto").ToString) & "</NombreCompleto>")
                        archivo.WriteLine("                                                 </Nombre>")
                        archivo.WriteLine("                                             </tCotitular>")
                    End While
                    dr2.Close()
                    archivo.WriteLine("                                         </Cotitulares>")
                End While
                dr.Close()
                archivo.WriteLine("                                 </RegistroDeDetalle>")
            Next i
            archivo.WriteLine("                                     <EnteroPropio fechaRecaudacion='" & CDate(fedFechaRecaudacion.Text.Trim).ToString("yyyy-MM-dd") & "' fechaEntero='" & CDate(fedFechaEntero.Text.Trim).ToString("yyyy-MM-dd") & "' impuestoEnterado='" & CDbl(fedImpto.Text.Trim).ToString("###########0") & "' noOperacion='" & fedNumOper.Text.Trim & "' nombreInstitucion='" & SecurityElement.Escape(enteroPropInstit.Text.Trim) & "' rfcInstitucion='" & enteroPropInstitRfc.Text.Trim & "'></EnteroPropio>") 'opc: a la federacion de la institución de crédito, auxiliar de la TESOFE, mediante la cual se realiza el entero a la federación. Su uso se convierte en obligatorio para las instituciones de crédito distintas de las auxiliares de la TESOFE<-->(las cajas son distintas de las aux de la tesofe(solo hay 17 aux de la tesofe en el pais)); Número de operación asignado por la institución bancaria. Cuando el entero sea a través de Depósito Referenciado se debe anotar la Línea de Captura obtenida de la Declaración del Servicio de Declaraciones y Pagos
            archivo.WriteLine("                                 </ReporteDeRecaudacionYEnteroDiaria>")
        End If
        If esInstitCredito = 1 Then
            archivo.WriteLine("                                 <Totales operacionesRelacionadas='" & CLng(GridView3.Rows.Count).ToString("###########0") & "' importeExcedenteDepositos='" & CDbl(impteExcedente.Text.Trim).ToString("#############0") & "' importeDeterminadoDepositos='" & CDbl(impteDeterminado.Text.Trim).ToString("#############0") & "' importeRecaudadoDepositos='" & CDbl(impteRecaudado.Text.Trim).ToString("#############0") & "' importePendienteRecaudacion='" & CDbl(imptePendienteRecaudar.Text.Trim).ToString("#############0") & "' importeRemanenteDepositos='" & CDbl(impteRemanente.Text.Trim).ToString("#############0") & "' importeEnterado='" & CDbl(fedImpto.Text.Trim).ToString("#############0") & "' importeSaldoPendienteRecaudar='" & CDbl(impteSaldoPendienteRecaudar.Text.Trim).ToString("###############0") & "' importeCheques='" & CDbl(impteCheques.Text.Trim).ToString("#############0") & "'></Totales>") 'saldopendienterecaudar: sumatoria de las cantidades capturadas en el campo saldo pendiente acumulado a la fecha de presentación de la declaración informativa mensual
            archivo.WriteLine("                 </InstitucionDeCredito>")
        Else
            archivo.WriteLine("                                 <Totales operacionesRelacionadas='" & CLng(GridView3.Rows.Count).ToString("###########0") & "' importeExcedenteDepositos='" & CDbl(impteExcedente.Text.Trim).ToString("#############0") & "' importeDeterminadoDepositos='" & CDbl(impteDeterminado.Text.Trim).ToString("#############0") & "' importeRecaudadoDepositos='" & CDbl(impteRecaudado.Text.Trim).ToString("#############0") & "' importePendienteRecaudacion='" & CDbl(imptePendienteRecaudar.Text.Trim).ToString("#############0") & "' importeRemanenteDepositos='" & CDbl(impteRemanente.Text.Trim).ToString("#############0") & "' importeEnterado='" & CDbl(fedImpto.Text.Trim).ToString("#############0") & "' importeSaldoPendienteRecaudar='" & CDbl(impteSaldoPendienteRecaudar.Text.Trim).ToString("###############0") & "'></Totales>")
            archivo.WriteLine("                 </InstitucionDistintaDeCredito>")
        End If

        archivo.WriteLine("     </DeclaracionInformativaMensualIDE>")

        archivo.Close()
    End Sub

    Private Sub subeXMLmensBD()
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
            .CommandText = "ideMensSubexml"
            .Parameters.AddWithValue("@ID", id.Text)    '1 xml x decl ya sea norm o complems del mes
            .Parameters.AddWithValue("@Logo", imgdata)
            dr = .ExecuteReader()
        End With
        br.Close()
        fstream.Close()
        dr.Close()

    End Sub

    Private Sub comprimeMens()

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
        'settings.Schemas.Add(Nothing, "www.declaracioneside.com/ide_20130430.xsd")
        Dim Path As String = System.AppDomain.CurrentDomain.BaseDirectory
        settings.Schemas.Add(Nothing, IO.Path.Combine(Path, "ide_20130430.xsd"))

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
            Dim elcorreo As New System.Net.Mail.MailMessage
            Using elcorreo
                elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                elcorreo.To.Add("declaracioneside@gmail.com")
                elcorreo.Subject = "Decl. mens ERROR xml"
                elcorreo.Body = "<html><body>cliente=" + Session("curCorreo") + ", ejercicio=" + ejercicio + ", mes=" + mes.ToString + ", error=" + errores + "</body></html>"
                elcorreo.IsBodyHtml = True
                elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                Dim smpt As New System.Net.Mail.SmtpClient
                smpt.Host = "smtp.gmail.com"
                smpt.Port = "587"
                smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a.")
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
            descrip.Text = "Se encontraron errores: " + errores + ", Contáctenos"
            Return False
        End If
        Return True
    End Function

    Private Sub creaXMLmes()
        Call creaTagsMens()

        If validacion() = False Then
            Exit Sub
        End If

        Call comprimeMens() 'borra xml crea zip
        Call subeXMLmensBD()
        'Call enviaArchivo(nomArchMensSinPath)        
    End Sub

    Private Sub creaXMLmesCeros()
        Call creaTagsMensCeros()
        Call comprimeMens()
        Call subeXMLmensBD()
        'Call enviaArchivo(nomArchMensSinPath)
        statusImport.Text = "Declaración creada"
        Response.Write("<script language='javascript'>alert('Declaración creada');</script>")
    End Sub

    Private Function validaSecuencia(ByVal descrip, ByVal descripAnt, ByVal ren, ByVal valCol3RenAnt, ByRef msgErr) As Integer
        If descripAnt = "" And descrip <> "CON" Then
            msgErr = msgErr + ". " + "En el renglón 5 debe indicar CON en la columna descripción"
            Return 0
        End If
        'CON proviene de CON, CTA o COT
        If descrip = "CTA" Then
            If CStr(valCol3RenAnt) <> "0" Then
                If descripAnt <> "CON" And descripAnt <> "COT" Then
                    msgErr = msgErr + ". " + "Una descripción CTA solo puede ser precedida por una CON o una COT, verifique en el renglón " + ren.ToString
                    Return 0
                End If
            End If

        ElseIf descrip = "COT" Then
            If descripAnt <> "CTA" And descripAnt <> "COT" Then
                msgErr = msgErr + ". " + "Una descripción COT solo puede ser precedida por una COT o una CTA, verifique en el renglón " + ren.ToString
                Return 0
            End If
        End If

        Return 1
    End Function

    Private Function importarIDEmens() As Integer
        Dim ctrlErr = 0
        Dim msgErr = ""
        progressbar.Style("width") = "0px"
        statusImport.Text = ""
        Dim percent As String
        Try

            Dim excel As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application
            Dim w As Workbook = excel.Workbooks.Open(savePath)
            'For i As Integer = 1 To w.Sheets.Count
            Dim sheet As Worksheet = w.Sheets(1) 'i     'abrirá la 1er hoja del libro
            'xlHoja = xlApp.Worksheets(CStr(DatePart("m", mes.Value))) ' hojas: 1:12

            If sheet.UsedRange.Rows.Count < 4 Then 'rens del encab
                w.Close(False)
                excel.Quit()
                w = Nothing
                excel = Nothing
                Response.Write("<script language='javascript'>alert('Es necesario dejar el encabezado de los primeros 4 renglones tal cual se le indica en la plantilla default');</script>")
                ctrlErr = 1
                GoTo etqErr
            End If

            If sheet.UsedRange.Columns.Count < 19 Then 'cols del encab
                w.Close(False)
                excel.Quit()
                w = Nothing
                excel = Nothing
                Response.Write("<script language='javascript'>alert('Es necesario dejar el encabezado de los primeros 4 renglones tal cual se le indica en la plantilla default con 19 columnas');</script>")
                ctrlErr = 1
                GoTo etqErr
            End If

            Dim r As Microsoft.Office.Interop.Excel.Range = sheet.UsedRange
            Dim array(,) As Object = r.Value(XlRangeValueDataType.xlRangeValueDefault) 'excel.XlRangeValueDataType.xlRangeValueDefault. 'System.Reflection.Missing.Value
            Dim nRensPre = sheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row
            'Dim nRensPre = sheet.Range("A1").CurrentRegion.Rows.Count 'sin rens en bco
            w.Close(False)   'cierro excel y trabajo con la var
            excel.Quit()
            w = Nothing
            excel = Nothing

            If array IsNot Nothing Then
                Dim rens As Integer = nRensPre 'array.GetUpperBound(0)
                'Dim cols As Integer = array.GetUpperBound(1)

                Dim descrip, cotNom, cotRfc, cotProporcion, ctaNum, ctaCotit, nombres, ap1, ap2, razon, rfc, Dom, telefono1, telefono2, exedente, determinado, recaudado, pendienteRecaudar, remanente, numSocioCliente, sumaDeposEfe, chqCajaMonto, chqCajaMontoRecaudado, saldoPendienteRecaudar
                Dim q, idIdeDet, idContrib, descripAnt, cotitularesCuentaActual, ideDetActual, cuentasIdeDetActual

                Dim esInstitCredito
                q = "SELECT esInstitCredito FROM clientes WHERE id=" + Session("GidCliente").ToString
                myCommand = New SqlCommand(q, myConnection)
                dr = myCommand.ExecuteReader()
                dr.Read()
                If dr("esInstitCredito").Equals(True) Then
                    esInstitCredito = 1
                Else
                    esInstitCredito = 0
                End If
                dr.Close()

                descripAnt = ""
                For ren As Integer = 5 To rens '1-4rens=encab 5o=datos
                    'For col As Integer = 1 To cols
                    If array(ren, 1) Is Nothing And array(ren, 2) Is Nothing And array(ren, 3) Is Nothing And array(ren, 4) Is Nothing And array(ren, 5) Is Nothing And array(ren, 6) Is Nothing And array(ren, 7) Is Nothing And array(ren, 8) Is Nothing And array(ren, 9) Is Nothing And array(ren, 10) Is Nothing And array(ren, 11) Is Nothing And array(ren, 12) Is Nothing And array(ren, 13) Is Nothing And array(ren, 14) Is Nothing And array(ren, 15) Is Nothing And array(ren, 16) Is Nothing And array(ren, 17) Is Nothing And array(ren, 18) Is Nothing And array(ren, 19) Is Nothing Then ' ren bco
                        GoTo siguiente
                    End If

                    If Not array(ren, 1) Is Nothing Then
                        descrip = Trim(UCase(array(ren, 1)))
                        If descrip = "CON" Or descrip = "CTA" Or descrip = "COT" Then
                            If validaSecuencia(descrip, descripAnt, ren, array(ren - 1, 3), msgErr) < 1 Then
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        Else
                            msgErr = msgErr + ". " + "En el renglon " + CStr(ren) + " la descripción es inválida"
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                    Else
                        descrip = ""
                        msgErr = msgErr + ". " + "En el renglon " + CStr(ren) + " la descripción no puede estar vacia"
                        ctrlErr = 1
                        GoTo siguiente
                    End If
                    descripAnt = descrip

                    If descrip = "CON" Then 'contribuyente
                        If Not array(ren, 2) Is Nothing Then
                            If Len(array(ren, 2).ToString.ToUpper.Trim) > 40 Then
                                msgErr = msgErr + ". " + "Truncando nombre a 40 caracteres en el renglon " + CStr(ren)
                            End If
                            nombres = Left(array(ren, 2).ToString.ToUpper.Trim, 40).Replace("'", "''")
                        Else
                            nombres = ""
                        End If
                        If Not array(ren, 3) Is Nothing Then
                            If Len(array(ren, 3).ToString.ToUpper.Trim) > 40 Then
                                msgErr = msgErr + ". " + "Truncando apellido paterno a 40 caracteres en el renglon " + CStr(ren)
                            End If
                            ap1 = Left(array(ren, 3).ToString.ToUpper.Trim, 40).Replace("'", "''")
                        Else
                            ap1 = ""
                        End If
                        If Not array(ren, 4) Is Nothing Then
                            If Len(array(ren, 4).ToString.ToUpper.Trim) > 40 Then
                                msgErr = msgErr + ". " + "Truncando apellido materno a 40 caracteres en el renglon " + CStr(ren)
                            End If
                            ap2 = Left(array(ren, 4).ToString.ToUpper.Trim, 40).Replace("'", "''")
                        Else
                            ap2 = ""
                        End If
                        If Not array(ren, 5) Is Nothing Then
                            If Len(array(ren, 5).ToString.ToUpper.Trim) > 250 Then
                                msgErr = msgErr + ". " + "Truncando razon social a 250 caracteres en el renglon " + CStr(ren)
                            End If
                            razon = Left(array(ren, 5).ToString.ToUpper.Trim, 250).Replace("'", "''")
                        Else
                            razon = ""
                        End If

                        If (nombres = "" And ap1 = "") And razon = "" Then
                            msgErr = msgErr + ". " + "En el renglon " + CStr(ren) + " requiere especificar el nombre con apellidos o bien la razon social"
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                        If nombres <> "" And ap1 = "" Then
                            msgErr = msgErr + ". " + "En el renglon " + CStr(ren) + " requiere especificar el apellido o bien, quite el nombre y agregue la razon social"
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                        If nombres <> "" And razon <> "" Then
                            msgErr = msgErr + ". " + "En el renglon " + CStr(ren) + " si no está reportando una razon social dejela en blanco, en caso contrario deje en blanco el nombre y los apellidos"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                        If Not array(ren, 6) Is Nothing Then
                            'If InStr(array(ren, 6).ToString, " ") > 0 Or InStr(array(ren, 6).ToString, "-") > 0 Then
                            '    msgErr = msgErr + ". " + "En el renglon " + CStr(ren) + " el rfc no debe tener espacios ni guiones"
                            '    ctrlErr = 1
                            '    GoTo siguiente
                            'End If
                            rfc = Left(array(ren, 6).ToString.ToUpper.Trim.Replace("-", "").Replace(" ", ""), 13)
                            Dim expresion
                            If razon = "" Then 'pf
                                expresion = "^([A-Z\s]{4})\d{6}([A-Z\w]{0,3})$"
                                If Len(rfc) < 10 Or Len(rfc) > 13 Then
                                    msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " el tamaño de rfc debe ser 10-13 caracteres"
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                            Else 'pm
                                expresion = "^([A-Z\s]{3})\d{6}([A-Z\w]{0,3})$"
                                If Len(rfc) < 9 Or Len(rfc) > 12 Then
                                    msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " el tamaño de rfc debe ser 9-12 caracteres"
                                    ctrlErr = 1
                                    GoTo siguiente
                                ElseIf Len(rfc) = 9 Then
                                    q = "SELECT rfcComodinPm FROM clientes where id=" + Session("GidCliente").ToString
                                    myCommand = New SqlCommand(q, myConnection)
                                    dr = myCommand.ExecuteReader()
                                    dr.Read()
                                    If dr("rfcComodinPm").Equals(True) Then 'usar comodin rfc sat
                                        rfc = "III991231AAA"    'comodin sat personas morales sin rfc
                                    Else
                                        msgErr = msgErr + ". " + "En el renglon " + CStr(ren) + " indica un rfc de 9 caracteres para la razon social, completelo a 12 caracteres o bien en su cuenta indique usar el RFC comodin proporcionado por el SAT de 12 caracteres"
                                        ctrlErr = 1
                                        dr.Close()
                                        GoTo siguiente
                                    End If
                                    dr.Close()
                                End If
                            End If
                            If Not Regex.IsMatch(rfc, expresion) Then
                                msgErr = msgErr + ". " + "Formato de rfc invalido en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        Else
                            rfc = ""
                        End If
                        If Not array(ren, 7) Is Nothing Then
                            If Len(array(ren, 7).ToString.ToUpper.Trim) > 150 Then
                                msgErr = msgErr + ". " + "Truncando domicilio a 150 caracteres en el renglon " + CStr(ren)
                            End If
                            Dom = Left(array(ren, 7).ToString.ToUpper.Trim, 150).Replace("'", "''")
                        Else
                            Dom = ""
                        End If
                        If Not array(ren, 8) Is Nothing Then
                            If Len(array(ren, 8).ToString.ToUpper.Trim) > 15 Then
                                msgErr = msgErr + ". " + "Truncando telefono1 a 15 caracteres en el renglon " + CStr(ren)
                            End If
                            telefono1 = Left(array(ren, 8).ToString.ToUpper.Trim.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", ""), 15)
                        Else
                            telefono1 = ""
                        End If
                        If Not array(ren, 9) Is Nothing Then
                            If Len(array(ren, 9).ToString.ToUpper.Trim) > 40 Then
                                msgErr = msgErr + ". " + "Truncando telefono2 a 15 caracteres en el renglon " + CStr(ren)
                            End If
                            telefono2 = Left(array(ren, 9).ToString.ToUpper.Trim.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", ""), 15)
                        Else
                            telefono2 = ""
                        End If
                        If Not array(ren, 10) Is Nothing Then
                            If Len(array(ren, 10).ToString.ToUpper.Trim) > 20 Then
                                msgErr = msgErr + ". " + "Truncando numero de socio a 20 caracteres en el renglon " + CStr(ren)
                            End If
                            numSocioCliente = Left(array(ren, 10).ToString.ToUpper.Trim, 20)
                        Else
                            numSocioCliente = ""
                        End If
                        If Not array(ren, 11) Is Nothing Then
                            If Not IsNumeric(array(ren, 11)) Then
                                msgErr = msgErr + ". " + "la suma de depositos en efectivo debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If CDbl(array(ren, 11)) < 0 Then
                                msgErr = msgErr + ". " + "la suma de depositos en efectivo debe ser >= 0 en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If

                            sumaDeposEfe = array(ren, 11).ToString
                        Else
                            sumaDeposEfe = ""
                        End If
                        If Not array(ren, 12) Is Nothing Then
                            If Not IsNumeric(array(ren, 12)) Then
                                msgErr = msgErr + ". " + "El excedente debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If CDbl(array(ren, 12)) < 0 Then
                                msgErr = msgErr + ". " + "el excedente debe ser >= 0 en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If

                            exedente = array(ren, 12).ToString
                        Else
                            exedente = ""
                        End If
                        If Not array(ren, 13) Is Nothing Then
                            If Not IsNumeric(array(ren, 13)) Then
                                msgErr = msgErr + ". " + "El monto determinado debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If CDbl(array(ren, 13)) < 0 Then
                                msgErr = msgErr + ". " + "el monto determinado debe ser >= 0 en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If

                            determinado = redondea(array(ren, 13)).ToString
                        Else
                            determinado = ""
                        End If
                        If Not array(ren, 14) Is Nothing Then
                            If Not IsNumeric(array(ren, 14)) Then
                                msgErr = msgErr + ". " + "El monto recaudado debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If CDbl(array(ren, 14)) < 0 Then
                                msgErr = msgErr + ". " + "el monto recaudado debe ser >= 0 en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If

                            recaudado = redondea(array(ren, 14)).ToString
                        Else
                            recaudado = ""
                        End If
                        If Not array(ren, 15) Is Nothing Then
                            If Not IsNumeric(array(ren, 15)) Then
                                msgErr = msgErr + ". " + "El monto pendiente de recaudar debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If CDbl(array(ren, 15)) < 0 Then
                                msgErr = msgErr + ". " + "el monto pendiente de recaudar debe ser >= 0 en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If

                            pendienteRecaudar = redondea(array(ren, 15)).ToString
                        Else
                            pendienteRecaudar = ""
                        End If
                        If Not array(ren, 16) Is Nothing Then
                            If Not IsNumeric(array(ren, 16)) Then
                                msgErr = msgErr + ". " + "El remanente debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If CDbl(array(ren, 16)) < 0 Then
                                msgErr = msgErr + ". " + "el remanente debe ser >= 0 en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If

                            remanente = redondea(array(ren, 16)).ToString
                        Else
                            remanente = ""
                        End If
                        If Not array(ren, 17) Is Nothing Then
                            If Not IsNumeric(array(ren, 17)) Then
                                msgErr = msgErr + ". " + "El saldo pendiente de recaudar debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If CDbl(array(ren, 17)) < 0 Then
                                msgErr = msgErr + ". " + "el saldo pendiente de recaudar debe ser >= 0 en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If

                            saldoPendienteRecaudar = redondea(array(ren, 17)).ToString
                        Else
                            saldoPendienteRecaudar = ""
                        End If
                        If Not array(ren, 18) Is Nothing Then
                            If Not IsNumeric(array(ren, 18)) Then
                                msgErr = msgErr + ". " + "El monto de cheque de caja debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If CDbl(array(ren, 18)) < 0 Then
                                msgErr = msgErr + ". " + "el monto de cheque de caja debe ser >= 0 en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If

                            chqCajaMonto = array(ren, 18).ToString
                        Else
                            chqCajaMonto = ""
                        End If
                        If Not array(ren, 19) Is Nothing Then
                            If Not IsNumeric(array(ren, 19)) Then
                                msgErr = msgErr + ". " + "El monto recaudado de cheque de caja debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If CDbl(array(ren, 19)) < 0 Then
                                msgErr = msgErr + ". " + "el monto recaudado debe ser >= 0 en el renglon " + CStr(ren)
                                ctrlErr = 1
                                GoTo siguiente
                            End If

                            chqCajaMontoRecaudado = redondea(array(ren, 19)).ToString
                        Else
                            chqCajaMontoRecaudado = ""
                        End If

                        'ren vacio
                        If nombres = "" And ap1 = "" And ap2 = "" And razon = "" And rfc = "" And Dom = "" And telefono1 = "" And telefono2 = "" And numSocioCliente = "" And chqCajaMonto = "" And chqCajaMontoRecaudado = "" And exedente = "" And determinado = "" And recaudado = "" And pendienteRecaudar = "" And remanente = "" And saldoPendienteRecaudar = "" And sumaDeposEfe = "" Then
                            GoTo siguiente
                        End If

                        If Dom = "" Then
                            msgErr = msgErr + ". " + "En el renglon " + CStr(ren) + " requiere especificar domicilio"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                        If razon <> "" And rfc = "" Then 'oblig p pers morales, pero el sat lo toma como llave
                            msgErr = msgErr + ". " + "En el renglon " + CStr(ren) + " requiere especificar rfc"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                        'If numSocioCliente = "" Then
                        '    MsgBox("el renglon " + CStr(ren) + " no debe estar vacio en columna F", , "Descartando, importación finalizada")
                        '    ctrlErr = 1
                        '    GoTo siguiente
                        'End If
                        If chqCajaMonto = "" And chqCajaMontoRecaudado = "" And exedente = "" And determinado = "" And recaudado = "" And pendienteRecaudar = "" And remanente = "" And sumaDeposEfe = "" And saldoPendienteRecaudar = "" Then
                            msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no debe tener importes vacios "
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                        If (esInstitCredito = 1 And ((chqCajaMonto = "" And chqCajaMontoRecaudado = "") Or (exedente <> "" Or determinado <> "" Or recaudado <> "" Or pendienteRecaudar <> "" Or remanente <> "" Or sumaDeposEfe <> "" Or saldoPendienteRecaudar <> ""))) Or esInstitCredito = 0 Then
                            If sumaDeposEfe = "" Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no debe tener suma de depositos en efectivo vacio"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If CDbl(Replace(sumaDeposEfe, ",", "")) > 0 And CDbl(Replace(sumaDeposEfe, ",", "")) < 15034 Then 'es el importe minimo que genera un ide de 1 peso
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " presenta un deposito en efectivo menor a $15,034 que no genera un ide de mínimo un peso, elimine el registro o bien corrija los montos "
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If exedente = "" Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no debe tener importe excedente vacio"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If determinado = "" Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no debe tener importe determinado vacio"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If recaudado = "" Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no debe tener importe recaudado vacio"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If pendienteRecaudar = "" Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no debe tener importe pendiente de recaudar vacio"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If remanente = "" Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no debe tener importe remanente vacio"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If saldoPendienteRecaudar = "" Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no debe tener importe de saldo pendiente de recaudar vacio, al menos indique 0"
                                ctrlErr = 1
                                GoTo siguiente
                            End If

                            If IsNumeric(sumaDeposEfe) = False Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no contiene suma de depositos en efectivo valido"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If IsNumeric(exedente) = False Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no contiene importe excedente valido"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If IsNumeric(determinado) = False Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no contiene importe determinado valido"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If IsNumeric(recaudado) = False Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no contiene importe recaudado valido"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If IsNumeric(pendienteRecaudar) = False Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no contiene importe pendiente de recaudar valido"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If IsNumeric(remanente) = False Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no contiene importe remanente valido"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If IsNumeric(saldoPendienteRecaudar) = False Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no contiene importe de saldo pendiente de recaudar valido"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        End If
                        If esInstitCredito = 1 And ((chqCajaMonto <> "" Or chqCajaMontoRecaudado <> "") Or (exedente = "" And determinado = "" And recaudado = "" And pendienteRecaudar = "" And remanente = "" And sumaDeposEfe = "" And saldoPendienteRecaudar = "")) Then
                            If chqCajaMonto = "" Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no debe tener importe vacio en monto del cheque de caja"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If chqCajaMontoRecaudado = "" Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no debe tener importe vacio en monto recaudado del cheque de caja"
                                ctrlErr = 1
                                GoTo siguiente
                            End If

                            If IsNumeric(chqCajaMonto) = False Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no contiene importe valido en monto del cheque de caja"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If IsNumeric(chqCajaMontoRecaudado) = False Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no contiene importe valido en monto recaudado del cheque de caja"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        End If
                        If mes.ToString = "1" Then
                            If CDbl(remanente) > 0 Then
                                remanente = "0"
                            End If
                        End If
                    ElseIf descrip = "CTA" Then
                        If Not array(ren, 2) Is Nothing Then
                            ctaNum = Left(array(ren, 2).ToString.ToUpper.Trim, 20)
                        Else
                            If normalComplementaria.Text = "COMPLEMENTARIA" Then
                                ctaNum = "99999"    'ctaNum defa p complementarias previas a la vers 2.0
                            Else
                                ctaNum = "9"
                            End If
                        End If
                        If Not array(ren, 3) Is Nothing Then
                            ctaCotit = array(ren, 3).ToString.Trim
                            If IsNumeric(ctaCotit) = False Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no contiene numero de cotitulares en formato numerico"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        Else
                            ctaCotit = ""
                            msgErr = msgErr + ". " + "En el renglon " + CStr(ren) + " requiere especificar el # de cotitulares"
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                    ElseIf descrip = "COT" Then
                        If Not array(ren, 2) Is Nothing Then
                            cotNom = array(ren, 2).ToString.Trim.ToUpper.Replace("'", "''")
                        Else
                            cotNom = ""
                            msgErr = msgErr + ". " + "En el renglon " + CStr(ren) + " requiere especificar nombre completo del cotitular"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                        If Not array(ren, 3) Is Nothing Then
                            cotRfc = array(ren, 3).ToString.ToUpper.Trim
                            If Len(cotRfc) < 9 Or Len(cotRfc) > 13 Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " el tamaño de rfc debe ser de 9 a 13 caracteres"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If Len(cotRfc) = 9 Then
                                q = "SELECT rfcComodinPm FROM clientes where idCliente=" + Session("GidCliente").ToString
                                myCommand = New SqlCommand(q, myConnection)
                                dr = myCommand.ExecuteReader()
                                dr.Read()
                                If dr("rfcComodinPm").Equals(True) Then 'usar comodin rfc sat
                                    cotRfc = "III991231AAA"    'comodin sat personas morales sin rfc
                                Else
                                    msgErr = msgErr + ". " + "En el renglon " + CStr(ren) + " indica un rfc de 9 caracteres, completelo a 12 caracteres o bien en su cuenta indique usar el RFC comodin proporcionado por el SAT de 12 caracteres"
                                    ctrlErr = 1
                                    dr.Close()
                                    GoTo siguiente
                                End If
                                dr.Close()
                            End If
                            Dim expresion1, expresion2
                            expresion1 = "^([A-Z\s]{4})\d{6}([A-Z\w]{3})$"
                            expresion2 = "^([A-Z\s]{3})\d{6}([A-Z\w]{3})$"
                            If Not Regex.IsMatch(cotRfc, expresion1) Then
                                If Not Regex.IsMatch(cotRfc, expresion2) Then
                                    msgErr = msgErr + ". " + "Formato de rfc invalido en el renglon " + CStr(ren)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                            End If
                        End If

                        If Not array(ren, 4) Is Nothing Then
                            cotProporcion = array(ren, 4).ToString.ToUpper.Trim
                            If IsNumeric(cotProporcion) = False Then
                                msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " no contiene una proporción en formato numérico"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If CDbl(Replace(cotProporcion, ",", "")) < 0 Or CDbl(Replace(cotProporcion, ",", "")) > 100 Then
                                msgErr = msgErr + ". " + "en el renglon " + CStr(ren) + "el porcentaje de proporción debe estar entre 0 y 100"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        Else
                            cotProporcion = ""
                            msgErr = msgErr + ". " + "En el renglon " + CStr(ren) + " requiere especificar el % de proporción proporción"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                    Else
                        msgErr = msgErr + ". " + "el renglon " + CStr(ren) + " contiene una descripción inválida en columna A"
                        ctrlErr = 1
                        GoTo siguiente
                    End If

                    If descrip = "CON" Then
                        If CDbl(Replace(determinado, ",", "")) <> CDbl(Replace(recaudado, ",", "")) + CDbl(Replace(pendienteRecaudar, ",", "")) Then
                            msgErr = msgErr + ". " + "El determinado " + determinado.ToString + " debe ser igual al recaudado " + recaudado.ToString + " mas el pendiente de recaudar " + pendienteRecaudar.ToString + " en el renglon " + CStr(ren) + ", verifique incluso decimales"
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                        If Math.Abs(CDbl(Replace(determinado, ",", "")) - redondea(CDbl(Replace(exedente, ",", "")) * CDbl(ideConfPorcen.Text) / 100)) > 0.001 Then 'vs +decimales en excel
                            msgErr = msgErr + ". " + "El determinado debe ser igual al exedente por la tasa en el renglon " + CStr(ren) + ", verifique incluso decimales"
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                        If CDbl(Replace(recaudado, ",", "")) > CDbl(Replace(determinado, ",", "")) Then
                            msgErr = msgErr + ". " + "El recaudado no puede ser mayor al determinado en el renglon " + CStr(ren)
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                        If Math.Abs(CDbl(Replace(chqCajaMontoRecaudado, ",", "")) - redondea(CDbl(Replace(chqCajaMonto, ",", "")) * CDbl(ideConfPorcen.Text) / 100)) > 0.001 Then 'vs +decimales en excel
                            msgErr = msgErr + ". " + "El ide recaudado de cheques de caja debe ser igual al monto del cheque de caja por la tasa en el renglon " + CStr(ren)
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                        If mes.ToString = "1" Then 'enero 1er impteSaldoPendienteRecaudar 
                            If CDbl(Replace(determinado, ",", "")) <> 0 Or CDbl(Replace(recaudado, ",", "")) <> 0 Then 'si trae recaudacion
                                If CDbl(Replace(saldoPendienteRecaudar, ",", "")) <> CDbl(Replace(determinado, ",", "")) - CDbl(Replace(recaudado, ",", "")) Then
                                    msgErr = msgErr + ". " + "En este periodo, se requiere SaldoPendienteRecaudar = Determinado - Recaudado en el renglon " + CStr(ren) + ", verifique incluso decimales"
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                            End If
                        Else 'impteSaldoPendienteRecaudar  meses subsecuentes
                            If CDbl(Replace(determinado, ",", "")) <> 0 Or CDbl(Replace(recaudado, ",", "")) <> 0 Or CDbl(Replace(remanente, ",", "")) <> 0 Then 'si trae recaudacion en efectivo
                                Dim cmpContrib
                                If razon = "" Then
                                    cmpContrib = "c.nombres='" + nombres + "' AND c.ap1='" + ap1 + "' AND c.ap2='" + ap2 + "'"
                                Else
                                    cmpContrib = "c.razonSocial='" + razon + "'" 'de la PM
                                End If
                                Dim qq = "SELECT id.impteSaldoPendienteRecaudar FROM ideDet id, ideMens m, ideAnual a, contribuyente c WHERE m.idAnual=a.id AND a.ejercicio='" + ejercicio.ToString + "' AND CAST(m.mes AS INT)=" + (CDbl(mes) - 1).ToString + " AND id.idMens=m.id AND id.idAnual=a.id AND id.idContribuyente=c.id AND " + cmpContrib
                                myCommand = New SqlCommand("SELECT id.impteSaldoPendienteRecaudar FROM ideDet id, ideMens m, ideAnual a, contribuyente c WHERE m.idAnual=a.id AND a.ejercicio='" + ejercicio.ToString + "' AND CAST(m.mes AS INT)=" + (CDbl(mes) - 1).ToString + " AND id.idMens=m.id AND id.idAnual=a.id AND id.idContribuyente=c.id AND " + cmpContrib, myConnection) 'impteSaldoPendienteRecaudar del mes anterior
                                dr = myCommand.ExecuteReader()
                                If dr.HasRows Then
                                    dr.Read()
                                    Dim calculadoSaldoPendienteRecaudar = dr("impteSaldoPendienteRecaudar") + CDbl(Replace(determinado, ",", "")) - CDbl(Replace(recaudado, ",", "")) - CDbl(Replace(remanente, ",", ""))
                                    If calculadoSaldoPendienteRecaudar > 0 And CDbl(Replace(saldoPendienteRecaudar, ",", "")) <> calculadoSaldoPendienteRecaudar Then
                                        msgErr = msgErr + ". " + "El SaldoPendienteRecaudar del mes anterior es " + FormatCurrency(dr("impteSaldoPendienteRecaudar")).ToString + " para el contribuyente del renglon " + CStr(ren) + ", Se requiere SaldoPendienteRecaudarActual = SaldoPendienteRecaudarAnterior + Determinado - Recaudado - Remanente, verifique incluso decimales"
                                        dr.Close()
                                        ctrlErr = 1
                                        GoTo siguiente
                                    End If
                                Else
                                    dr.Close()
                                End If
                            End If
                        End If
                    End If
siguiente:
                Next
etqErr:
                If ctrlErr = 1 Then
                    estado.Text = "VACIA"
                    myCommand2 = New SqlCommand("UPDATE ideMens SET estado='VACIA' WHERE id=" + id.Text, myConnection)
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
                    Else
                        lblErrImport.Visible = False
                        errImport.Visible = False
                    End If

                    'borra los registros del detalle, importante el orden de eliminacion
                    q = "DELETE FROM tCotitular WHERE idCotitularesCuenta IN (SELECT id FROM cotitularesCuenta WHERE idideDet IN (SELECT id FROM ideDet WHERE idMens=" + id.Text + " AND idAnual=" + idAnual.Text + "))"
                    myCommand = New SqlCommand(q, myConnection)
                    myCommand.ExecuteNonQuery()

                    q = "DELETE FROM cotitularesCuenta WHERE idideDet IN (SELECT id FROM ideDet WHERE idMens=" + id.Text + " AND idAnual=" + idAnual.Text + ")"
                    myCommand = New SqlCommand(q, myConnection)
                    myCommand.ExecuteNonQuery()

                    q = "DELETE FROM ideDet WHERE idMens=" + id.Text + " AND idAnual=" + idAnual.Text
                    myCommand = New SqlCommand(q, myConnection)
                    myCommand.ExecuteNonQuery()

                    For ren As Integer = 5 To rens '1-4rens=encab 5o=datos
                        'For col As Integer = 1 To cols
                        If array(ren, 1) Is Nothing And array(ren, 2) Is Nothing And array(ren, 3) Is Nothing And array(ren, 4) Is Nothing And array(ren, 5) Is Nothing And array(ren, 6) Is Nothing And array(ren, 7) Is Nothing And array(ren, 8) Is Nothing And array(ren, 9) Is Nothing And array(ren, 10) Is Nothing And array(ren, 11) Is Nothing And array(ren, 12) Is Nothing And array(ren, 13) Is Nothing And array(ren, 14) Is Nothing And array(ren, 15) Is Nothing And array(ren, 16) Is Nothing And array(ren, 17) Is Nothing And array(ren, 18) Is Nothing And array(ren, 19) Is Nothing Then ' ren bco
                            GoTo siguiente2
                        End If

                        If Not array(ren, 1) Is Nothing Then
                            descrip = Trim(UCase(array(ren, 1)))
                        Else
                            descrip = ""
                        End If
                        descripAnt = descrip

                        If descrip = "CON" Then 'contribuyente
                            If Not array(ren, 2) Is Nothing Then
                                nombres = Left(array(ren, 2).ToString.ToUpper.Trim, 40).Replace("'", "''")
                            Else
                                nombres = ""
                            End If
                            If Not array(ren, 3) Is Nothing Then
                                ap1 = Left(array(ren, 3).ToString.ToUpper.Trim, 40).Replace("'", "''")
                            Else
                                ap1 = ""
                            End If
                            If Not array(ren, 4) Is Nothing Then
                                ap2 = Left(array(ren, 4).ToString.ToUpper.Trim, 40).Replace("'", "''")
                            Else
                                ap2 = ""
                            End If
                            If Not array(ren, 5) Is Nothing Then
                                razon = Left(array(ren, 5).ToString.ToUpper.Trim, 250).Replace("'", "''")
                            Else
                                razon = ""
                            End If

                            If Not array(ren, 6) Is Nothing Then
                                rfc = Left(array(ren, 6).ToString.ToUpper.Trim.Replace("-", "").Replace(" ", ""), 13)
                                If razon = "" Then 'pf
                                Else 'pm
                                    If Len(rfc) < 9 Or Len(rfc) > 12 Then
                                    ElseIf Len(rfc) = 9 Then
                                        q = "SELECT rfcComodinPm FROM clientes where id=" + Session("GidCliente").ToString
                                        myCommand = New SqlCommand(q, myConnection)
                                        dr = myCommand.ExecuteReader()
                                        dr.Read()
                                        If dr("rfcComodinPm").Equals(True) Then 'usar comodin rfc sat
                                            rfc = "III991231AAA"    'comodin sat personas morales sin rfc
                                        End If
                                        dr.Close()
                                    End If
                                End If
                            Else
                                rfc = ""
                            End If
                            If Not array(ren, 7) Is Nothing Then
                                Dom = Left(array(ren, 7).ToString.ToUpper.Trim, 150).Replace("'", "''")
                            Else
                                Dom = ""
                            End If
                            If Not array(ren, 8) Is Nothing Then
                                telefono1 = Left(array(ren, 8).ToString.ToUpper.Trim.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", ""), 15)
                            Else
                                telefono1 = ""
                            End If
                            If Not array(ren, 9) Is Nothing Then
                                telefono2 = Left(array(ren, 9).ToString.ToUpper.Trim.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", ""), 15)
                            Else
                                telefono2 = ""
                            End If
                            If Not array(ren, 10) Is Nothing Then
                                numSocioCliente = Left(array(ren, 10).ToString.ToUpper.Trim, 20)
                            Else
                                numSocioCliente = ""
                            End If
                            If Not array(ren, 11) Is Nothing Then
                                sumaDeposEfe = array(ren, 11).ToString
                            Else
                                sumaDeposEfe = ""
                            End If
                            If Not array(ren, 12) Is Nothing Then
                                exedente = array(ren, 12).ToString
                            Else
                                exedente = ""
                            End If
                            If Not array(ren, 13) Is Nothing Then
                                determinado = redondea(array(ren, 13)).ToString
                            Else
                                determinado = ""
                            End If
                            If Not array(ren, 14) Is Nothing Then
                                recaudado = redondea(array(ren, 14)).ToString
                            Else
                                recaudado = ""
                            End If
                            If Not array(ren, 15) Is Nothing Then
                                pendienteRecaudar = redondea(array(ren, 15)).ToString
                            Else
                                pendienteRecaudar = ""
                            End If
                            If Not array(ren, 16) Is Nothing Then
                                remanente = redondea(array(ren, 16)).ToString
                            Else
                                remanente = ""
                            End If
                            If Not array(ren, 17) Is Nothing Then
                                saldoPendienteRecaudar = redondea(array(ren, 17)).ToString
                            Else
                                saldoPendienteRecaudar = ""
                            End If
                            If Not array(ren, 18) Is Nothing Then
                                chqCajaMonto = array(ren, 18).ToString
                            Else
                                chqCajaMonto = ""
                            End If
                            If Not array(ren, 19) Is Nothing Then
                                chqCajaMontoRecaudado = redondea(array(ren, 19)).ToString
                            Else
                                chqCajaMontoRecaudado = ""
                            End If

                            'ren vacio
                            If nombres = "" And ap1 = "" And ap2 = "" And razon = "" And rfc = "" And Dom = "" And telefono1 = "" And telefono2 = "" And numSocioCliente = "" And chqCajaMonto = "" And chqCajaMontoRecaudado = "" And exedente = "" And determinado = "" And recaudado = "" And pendienteRecaudar = "" And remanente = "" And saldoPendienteRecaudar = "" And sumaDeposEfe = "" Then
                                GoTo siguiente2
                            End If

                            If mes.ToString = "1" Then
                                If CDbl(remanente) > 0 Then
                                    remanente = "0"
                                    Response.Write("<script language='javascript'>alert('Cambiando a 0 el importe remanente de IDE para los meses de Enero, deacuerdo a la RMF 2013 de Julio, en el renglon " + ren.ToString + "');</script>")
                                End If
                            End If
                        ElseIf descrip = "CTA" Then
                            If Not array(ren, 2) Is Nothing Then
                                ctaNum = Left(array(ren, 2).ToString.ToUpper.Trim, 20)
                            Else
                                If normalComplementaria.Text = "COMPLEMENTARIA" Then
                                    ctaNum = "99999"    'ctaNum defa p complementarias previas a la vers 2.0
                                Else
                                    ctaNum = "9"
                                End If
                            End If
                            If Not array(ren, 3) Is Nothing Then
                                ctaCotit = array(ren, 3).ToString.Trim
                            Else
                                ctaCotit = ""
                            End If
                        ElseIf descrip = "COT" Then
                            If Not array(ren, 2) Is Nothing Then
                                cotNom = array(ren, 2).ToString.Trim.ToUpper.Replace("'", "''")
                            Else
                                cotNom = ""
                            End If

                            If Not array(ren, 3) Is Nothing Then
                                cotRfc = array(ren, 3).ToString.ToUpper.Trim
                                If Len(cotRfc) = 9 Then
                                    q = "SELECT rfcComodinPm FROM clientes where idCliente=" + Session("GidCliente").ToString
                                    myCommand = New SqlCommand(q, myConnection)
                                    dr = myCommand.ExecuteReader()
                                    dr.Read()
                                    If dr("rfcComodinPm").Equals(True) Then 'usar comodin rfc sat
                                        cotRfc = "III991231AAA"    'comodin sat personas morales sin rfc
                                    End If
                                    dr.Close()
                                End If
                            End If

                            If Not array(ren, 4) Is Nothing Then
                                cotProporcion = array(ren, 4).ToString.ToUpper.Trim
                            Else
                                cotProporcion = ""
                            End If
                        End If

                        If descrip = "CON" Then
                            q = "SELECT id FROM contribuyente c WHERE ((c.nombres='" + nombres + "' AND c.ap1='" + ap1 + "' AND c.ap2='" + ap2 + "' and c.razonSocial='') or (c.razonSocial='" + razon + "' and c.razonSocial<>''))"
                            myCommand = New SqlCommand(q, myConnection)
                            dr = myCommand.ExecuteReader()
                            If dr.Read() Then 'registro duplicado (llaves) en el archivo->reemplazarlo por el mas reciente
                                idContrib = dr("id")
                                dr.Close()
                                q = "UPDATE contribuyente SET numSocioCliente='" + numSocioCliente + "',rfc='" + rfc + "',Dom='" + Dom + "',telefono1='" + telefono1 + "',telefono2='" + telefono2 + "' WHERE id=" + idContrib.ToString
                                myCommand2 = New SqlCommand(q, myConnection)
                                myCommand2.ExecuteNonQuery()
                            Else    'nuevo registro
                                dr.Close()
                                statusImport.Text = nombres + " " + ap1 + " " + ap2 + " " + numSocioCliente + " " + razon + " " + rfc + " " + Dom + " " + telefono1 + " " + telefono2
                                myCommand2 = New SqlCommand("INSERT INTO contribuyente(nombres,ap1,ap2,numSocioCliente,razonSocial,rfc,Dom,telefono1,telefono2) VALUES('" + nombres + "','" + ap1 + "','" + ap2 + "','" + numSocioCliente + "','" + razon + "','" + rfc + "','" + Dom + "','" + telefono1 + "','" + telefono2 + "')", myConnection)
                                myCommand2.ExecuteNonQuery()
                                q = "SELECT TOP 1 id FROM contribuyente ORDER BY id DESC"
                                myCommand = New SqlCommand(q, myConnection)
                                dr = myCommand.ExecuteReader()
                                dr.Read()
                                idContrib = dr("id")
                                dr.Close()
                            End If

                            q = "SELECT d.id FROM ideDet d, contribuyente c WHERE idMens=" + id.Text + " AND idAnual=" + idAnual.Text + " AND d.idContribuyente=c.id AND c.id=" + idContrib.ToString
                            myCommand = New SqlCommand(q, myConnection)
                            dr = myCommand.ExecuteReader()
                            If dr.Read() Then 'registro duplicado (llaves) en el archivo->reemplazarlo por el mas reciente
                                idIdeDet = dr("id")
                                dr.Close()

                                If esInstitCredito = 1 Then
                                    If sumaDeposEfe <> "" Then
                                        q = "UPDATE ideDet SET exedente='" + exedente + "',determinado='" + determinado + "',recaudado='" + recaudado + "',pendienteRecaudar='" + pendienteRecaudar + "',remanente='" + remanente + "',impteSaldoPendienteRecaudar='" + saldoPendienteRecaudar + "' WHERE id=" + idIdeDet.ToString
                                        myCommand2 = New SqlCommand(q, myConnection)
                                        myCommand2.ExecuteNonQuery()
                                    End If
                                    If chqCajaMonto <> "" Then
                                        q = "UPDATE ideDet SET chqCajaMonto='" + chqCajaMonto + "',chqCajaMontoRecaudado='" + chqCajaMontoRecaudado + "' WHERE id=" + idIdeDet.ToString
                                        myCommand2 = New SqlCommand(q, myConnection)
                                        myCommand2.ExecuteNonQuery()
                                    End If
                                Else
                                    q = "UPDATE ideDet SET exedente='" + exedente + "',determinado='" + determinado + "',recaudado='" + recaudado + "',pendienteRecaudar='" + pendienteRecaudar + "',remanente='" + remanente + "',impteSaldoPendienteRecaudar='" + saldoPendienteRecaudar + "' WHERE id=" + idIdeDet.ToString
                                    myCommand2 = New SqlCommand(q, myConnection)
                                    myCommand2.ExecuteNonQuery()
                                End If
                            Else    'nuevo registro
                                dr.Close()

                                If esInstitCredito = 1 Then
                                    q = "INSERT INTO ideDet(idMens,idAnual,idContribuyente"
                                    If sumaDeposEfe <> "" Then
                                        q = q + ",exedente,determinado,recaudado,pendienteRecaudar,remanente,sumaDeposEfe,impteSaldoPendienteRecaudar"
                                    End If
                                    If chqCajaMonto <> "" Then
                                        q = q + ",chqCajaMonto, chqCajaMontoRecaudado"
                                    End If
                                    q = q + ") VALUES(" + id.Text + "," + idAnual.Text + "," + idContrib.ToString
                                    If sumaDeposEfe <> "" Then
                                        q = q + ",'" + exedente + "','" + determinado + "','" + recaudado + "','" + pendienteRecaudar + "','" + remanente + "','" + sumaDeposEfe + "','" + saldoPendienteRecaudar + "'"
                                    End If
                                    If chqCajaMonto <> "" Then
                                        q = q + ",'" + chqCajaMonto + "','" + chqCajaMontoRecaudado + "'"
                                    End If
                                    q = q + ")"
                                Else
                                    q = "INSERT INTO ideDet(idMens,idAnual,idContribuyente,exedente,determinado,recaudado,pendienteRecaudar,remanente,sumaDeposEfe,impteSaldoPendienteRecaudar) VALUES(" + id.Text + "," + idAnual.Text + "," + idContrib.ToString + ",'" + exedente + "','" + determinado + "','" + recaudado + "','" + pendienteRecaudar + "','" + remanente + "','" + sumaDeposEfe + "','" + saldoPendienteRecaudar + "')"
                                End If
                                myCommand2 = New SqlCommand(q, myConnection)
                                myCommand2.ExecuteNonQuery()

                                q = "SELECT d.id FROM ideDet d, contribuyente c WHERE idMens=" + id.Text + " AND idAnual=" + idAnual.Text + " AND d.idContribuyente=c.id AND c.id=" + idContrib.ToString
                                myCommand = New SqlCommand(q, myConnection)
                                dr = myCommand.ExecuteReader()
                                dr.Read()
                                idIdeDet = dr("id")
                                dr.Close()
                            End If

                        ElseIf descrip = "CTA" Then
                            'repetidos: update
                            Dim idCotitularesCuenta
                            q = "SELECT id FROM cotitularesCuenta WHERE numeroCuenta='" + ctaNum + "' AND idideDet =" + idIdeDet.ToString
                            myCommand = New SqlCommand(q, myConnection)
                            dr = myCommand.ExecuteReader()
                            If dr.Read() Then
                                idCotitularesCuenta = dr("id")
                                cotitularesCuentaActual = idCotitularesCuenta
                                dr.Close()
                                q = "UPDATE cotitularesCuenta SET numeroCotitulares='" + ctaCotit + "' WHERE numeroCuenta='" + ctaNum + "' AND idideDet =" + idIdeDet.ToString
                                myCommand2 = New SqlCommand(q, myConnection)
                                myCommand2.ExecuteNonQuery()
                            Else
                                dr.Close()
                                myCommand2 = New SqlCommand("INSERT INTO cotitularesCuenta(numeroCuenta,numeroCotitulares,idideDet) VALUES('" + ctaNum.ToString + "'," + ctaCotit.ToString + "," + idIdeDet.ToString + ")", myConnection)
                                myCommand2.ExecuteNonQuery()

                                q = "SELECT TOP 1 id FROM cotitularesCuenta ORDER BY id DESC"
                                myCommand = New SqlCommand(q, myConnection)
                                dr = myCommand.ExecuteReader()
                                dr.Read()
                                cotitularesCuentaActual = dr("id")
                                dr.Close()
                            End If

                        ElseIf descrip = "COT" Then
                            myCommand2 = New SqlCommand("INSERT INTO tcotitular(idCotitularesCuenta,nombreCompleto,rfc,proporcion) VALUES(" + cotitularesCuentaActual.ToString + ",'" + cotNom.ToString + "','" + cotRfc + "','" + cotProporcion + "')", myConnection)
                            myCommand2.ExecuteNonQuery()
                        End If
siguiente2:
                        percent = Double.Parse(ren * 100 / rens).ToString("0")
                        progressbar.Style("width") = percent + "px"
                    Next

                    progressbar.Style("width") = "100px"
                    estado.Text = "IMPORTADA"
                    myCommand2 = New SqlCommand("UPDATE ideMens SET estado='IMPORTADA', idContrato=" + idContrato.ToString + ", viaImportacion=1 WHERE id=" + id.Text, myConnection)
                    myCommand2.ExecuteNonQuery()

                    statusImport.Text = "Archivo cargado, porfavor espere"
                    'MsgBox(" Importación IDE realizada ", , "Felicidades") 'no deja refrescar la barra hasta que des ok
                    Return 1

                End If
            End If
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('" + ex.Message + "');</script>")
        End Try
    End Function


    Protected Sub ver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ver.Click
        MultiView1.ActiveViewIndex = Int32.Parse(4)
        cargaGrid()
        progressbar.Style("width") = "0px"
        statusImport.Text = ""
        descrip.Text = ""
    End Sub

    Private Sub refrescaTotalesMens()
        'actualiza la mensual
        Dim q, esInstitCredito
        q = "SELECT esInstitCredito FROM clientes WHERE id=" + Session("GidCliente").ToString
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        If dr("esInstitCredito").Equals(True) Then
            esInstitCredito = 1
        Else
            esInstitCredito = 0
        End If
        dr.Close()

        q = "SELECT SUM(exedente) AS sumaExedente, SUM(determinado) AS sumaDeterminado, SUM(recaudado) AS sumaRecaudado, SUM(pendienteRecaudar) AS sumaPendienteRecaudar, SUM(remanente) AS sumaRemanente, SUM(impteSaldoPendienteRecaudar) AS sumaImpteSaldoPendienteRecaudar, SUM(chqCajaMontoRecaudado) AS sumaChqCajaMontoRecaudado FROM ideDet WHERE idMens=" + id.Text + " AND idAnual=" + idAnual.Text
        myCommand2 = New SqlCommand(q, myConnection)
        dr = myCommand2.ExecuteReader()
        If dr.Read() Then
            Dim aplicaRecaudadoCheques
            If esInstitCredito = 1 Then
                aplicaRecaudadoCheques = dr("sumaChqCajaMontoRecaudado")
            Else
                aplicaRecaudadoCheques = 0
            End If
            q = "UPDATE ideMens SET impteExcedente='" + dr("sumaExedente").ToString + "',impteDeterminado='" + dr("sumaDeterminado").ToString + "',impteRecaudado='" + dr("sumaRecaudado").ToString + "',imptePendienteRecaudar='" + dr("sumaPendienteRecaudar").ToString + "',impteRemanente='" + dr("sumaRemanente").ToString + "',impteSaldoPendienteRecaudar='" + dr("sumaImpteSaldoPendienteRecaudar").ToString + "',fedImpto='" + CStr(dr("sumaRecaudado") + dr("sumaRemanente") + aplicaRecaudadoCheques) + "',impteCheques='" + dr("sumaChqCajaMontoRecaudado").ToString + "' WHERE idAnual=" + idAnual.Text + " AND id='" + id.Text + "'"
            myCommand = New SqlCommand(q, myConnection)
            myCommand.ExecuteNonQuery()
            If DBNull.Value.Equals(dr("sumaExedente")) Then
                impteExcedente.Text = FormatNumber(0, 0)
            Else
                impteExcedente.Text = FormatNumber(dr("sumaExedente"), 0)
            End If
            If DBNull.Value.Equals(dr("sumaDeterminado")) Then
                impteDeterminado.Text = FormatNumber(0, 0)
            Else
                impteDeterminado.Text = FormatNumber(dr("sumaDeterminado"), 0)
            End If
            If DBNull.Value.Equals(dr("sumaRecaudado")) Then
                impteRecaudado.Text = FormatNumber(0, 0)
            Else
                impteRecaudado.Text = FormatNumber(dr("sumaRecaudado"), 0)
            End If
            If DBNull.Value.Equals(dr("sumaPendienteRecaudar")) Then
                imptePendienteRecaudar.Text = FormatNumber(0, 0)
            Else
                imptePendienteRecaudar.Text = FormatNumber(dr("sumaPendienteRecaudar"), 0)
            End If
            If DBNull.Value.Equals(dr("sumaRemanente")) Then
                impteRemanente.Text = FormatNumber(0, 0)
            Else
                impteRemanente.Text = FormatNumber(dr("sumaRemanente"), 0)
            End If
            If DBNull.Value.Equals(dr("sumaImpteSaldoPendienteRecaudar")) Then
                impteSaldoPendienteRecaudar.Text = FormatNumber(0, 0)
            Else
                impteSaldoPendienteRecaudar.Text = FormatNumber(dr("sumaImpteSaldoPendienteRecaudar"), 0)
            End If

            If esInstitCredito = 1 Then
                If DBNull.Value.Equals(dr("sumaRecaudado")) And DBNull.Value.Equals(dr("sumaRemanente")) And DBNull.Value.Equals(dr("sumaChqCajaMontoRecaudado")) Then
                    fedImpto.Text = FormatNumber(0, 0)
                Else
                    fedImpto.Text = FormatNumber(dr("sumaRecaudado") + dr("sumaRemanente") + dr("sumaChqCajaMontoRecaudado"), 0)
                End If
            Else
                If DBNull.Value.Equals(dr("sumaRecaudado")) And DBNull.Value.Equals(dr("sumaRemanente")) Then
                    fedImpto.Text = FormatNumber(0, 0)
                Else
                    fedImpto.Text = FormatNumber(dr("sumaRecaudado") + dr("sumaRemanente"), 0)
                End If
            End If

            If DBNull.Value.Equals(dr("sumaChqCajaMontoRecaudado")) Then
                impteCheques.Text = FormatNumber("0", 0)
            Else
                impteCheques.Text = FormatNumber(dr("sumaChqCajaMontoRecaudado"), 0)
            End If
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
                        Response.Write("<script language='javascript'>alert('fecha complementaria invalida');</script>")
                        Return 0
                    End If
                Else
                    fechaPresentacion.Focus()
                    Response.Write("<script language='javascript'>alert('fecha Complementaria formato de fecha no valido (dd/mm/aaaa)');</script>")
                    Return 0
                End If
            Else
                fechaPresentacion.Text = Left(Now(), 10).ToString
            End If
        End If
        If fechaCorte.Text.Trim <> "" Then
            Dim dtnow As DateTime
            Dim regDate As New System.Text.RegularExpressions.Regex("^\d{1,2}\/\d{1,2}\/\d{4}$")
            If regDate.IsMatch(fechaCorte.Text.Trim) Then
                If Not DateTime.TryParse(fechaCorte.Text.Trim, dtnow) Then
                    fechaCorte.Focus()
                    Response.Write("<script language='javascript'>alert('fecha Corte invalida');</script>")
                    Return 0
                End If
            Else
                fechaCorte.Focus()
                Response.Write("<script language='javascript'>alert('fecha Corte formato de fecha no valido (dd/mm/aaaa)');</script>")
                Return 0
            End If
        Else
            fechaCorte.Text = Left(Now(), 10).ToString
        End If
        If fedFechaEntero.Text.Trim <> "" Then
            Dim dtnow As DateTime
            Dim regDate As New System.Text.RegularExpressions.Regex("^\d{1,2}\/\d{1,2}\/\d{4}$")
            If regDate.IsMatch(fedFechaEntero.Text.Trim) Then
                If Not DateTime.TryParse(fedFechaEntero.Text.Trim, dtnow) Then
                    fedFechaEntero.Focus()
                    Response.Write("<script language='javascript'>alert('Fecha Entero invalida');</script>")
                    Return 0
                End If
            Else
                fedFechaEntero.Focus()
                Response.Write("<script language='javascript'>alert('Fecha Entero formato de fecha no valido (dd/mm/aaaa)');</script>")
                Return 0
            End If
        Else
            fedFechaEntero.Text = Left(Now(), 10).ToString
        End If

        If fedFechaRecaudacion.Text.Trim <> "" Then
            Dim dtnow As DateTime
            Dim regDate As New System.Text.RegularExpressions.Regex("^\d{1,2}\/\d{1,2}\/\d{4}$")
            If regDate.IsMatch(fedFechaRecaudacion.Text.Trim) Then
                If Not DateTime.TryParse(fedFechaRecaudacion.Text.Trim, dtnow) Then
                    fedFechaRecaudacion.Focus()
                    Response.Write("<script language='javascript'>alert('Fecha de Recaudación invalida');</script>")
                    Return 0
                End If
            Else
                fedFechaRecaudacion.Focus()
                Response.Write("<script language='javascript'>alert('Fecha de Recaudación formato de fecha no valido (dd/mm/aaaa)');</script>")
                Return 0
            End If
        Else
            If normalComplementaria.Text = "COMPLEMENTARIA" And ejercicio.ToString + "-" + CInt(mes).ToString("0#") + "-01" < "2013-06-01" Then 'complem previa a v2.0
                fedFechaRecaudacion.Text = "01/01/2012"
            Else
                fedFechaRecaudacion.Text = Left(Now(), 10).ToString
            End If
        End If

        If enteroPropInstit.Text.Trim = "" Then
            Response.Write("<script language='javascript'>alert('Indique la Institución mediante la que hizo el entero');</script>")
            enteroPropInstit.Focus()
            Return 0
        End If

        If enteroPropInstitRfc.Text.Trim = "" Then
            Response.Write("<script language='javascript'>alert('Indique Rfc de la Institución mediante la que hizo el entero');</script>")
            enteroPropInstitRfc.Focus()
            Return 0
        End If

        Dim expresion = "^([A-Z\s]{3})\d{6}([A-Z\w]{3})$" 'pm 12 cars
        If Not Regex.IsMatch(enteroPropInstitRfc.Text.ToUpper.Trim, expresion) Then
            Response.Write("<script language='javascript'>alert('Formato de rfc invalido');</script>")
            enteroPropInstitRfc.Focus()
            Return 0
        End If

        If fedImpto.Text.Trim = "" Or fedImpto.Text.Trim = "0" Then
            Response.Write("<script language='javascript'>alert('Indique el impuesto enterado a la federación');</script>")
            fedImpto.Focus()
            Return 0
        End If
        If fedNumOper.Text.Trim = "" Or fedNumOper.Text.Trim = "0" Then
            Response.Write("<script language='javascript'>alert('Indique el número de operación (folio, clave o guia CIE) de su comprobante bancario que le fué proporcionado al momento de enterar el IDE, o bien ingrese la linea de captura para depositos referenciados');</script>")
            fedNumOper.Focus()
            Return 0
        End If

        Dim q = "SELECT esInstitCredito FROM clientes WHERE id=" + Session("GidCliente").ToString
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        If dr("esInstitCredito").Equals(True) Then
            Dim suma = CDbl(impteRecaudado.Text) + CDbl(impteRemanente.Text) + CDbl(impteCheques.Text)
            If CDbl(Replace(fedImpto.Text, ",", "")) <> suma Then
                dr.Close()
                Response.Write("<script language='javascript'>alert('Debe ser: Impuesto pagado(enterado) = Recaudado + Remanente + RecaudadoDeChequesDeCaja, verifique');</script>")
                Return 0
            End If
        Else
            Dim suma = CDbl(impteRecaudado.Text) + CDbl(impteRemanente.Text)
            If CDbl(Replace(fedImpto.Text, ",", "")) <> suma Then
                dr.Close()
                Response.Write("<script language='javascript'>alert('Debe ser: Impuesto pagado(enterado) = Recaudado + Remanente, verifique: " + CDbl(Replace(fedImpto.Text, ",", "")).ToString + ", " + CStr(suma) + "');</script>")
                Return 0
            End If
        End If
        dr.Close()

        Return 1

    End Function

    Protected Sub mod_Click(ByVal sender As Object, ByVal e As EventArgs) Handles modi.Click
        If id.Text = "0" Then
            Response.Write("<script language='javascript'>alert('Primero importe los datos o Crear en ceros');</script>")
            Exit Sub
        End If

        If Request.QueryString("op") = "0" And Request.QueryString("subop") = "0" Then 'crear editar excel
            cargaGrid()
            progressbar.Style("width") = "0px"
            statusImport.Text = ""
            descrip.Text = ""
        End If

        If Request.QueryString("op") = "0" And GridView3.Rows.Count > 0 Then 'no se valida para 0s o consulta, ni al crear/editar cuando se importaron 0 regs exitosam ya sea normal o complem.
            If validar() < 1 Then
                Exit Sub
            End If
        End If

        Dim q
        q = "UPDATE ideMens SET impteExcedente='" + impteExcedente.Text.Trim + "',impteDeterminado='" + impteDeterminado.Text.Trim + "',impteRecaudado='" + impteRecaudado.Text.Trim + "',imptePendienteRecaudar='" + imptePendienteRecaudar.Text.Trim + "',impteRemanente='" + impteRemanente.Text.Trim + "',impteSaldoPendienteRecaudar='" + impteSaldoPendienteRecaudar.Text.Trim + "',numOper='" + numOper.Text.Trim + "', fechaPresentacion='" + Convert.ToDateTime(fechaPresentacion.Text.Trim).ToString("yyyy-MM-dd") + "', fechaCorte='" + Convert.ToDateTime(fechaCorte.Text.Trim).ToString("yyyy-MM-dd") + "',fedFechaRecaudacion='" + Convert.ToDateTime(fedFechaRecaudacion.Text.Trim).ToString("yyyy-MM-dd") + "',normalComplementaria='" + normalComplementaria.Text + "', fedFechaEntero='" + Convert.ToDateTime(fedFechaEntero.Text.Trim).ToString("yyyy-MM-dd") + "',fedImpto='" + fedImpto.Text.Trim + "',fedNumOper='" + fedNumOper.Text.Trim + "',enteroPropInstit='" + enteroPropInstit.Text.ToUpper.Trim + "',enteroPropInstitRfc='" + enteroPropInstitRfc.Text.ToUpper.Trim + "', guardadaUsuario=1 WHERE id=" + id.Text + " AND idAnual=" + idAnual.Text + " AND mes='" + mes.ToString + "'"
        myCommand = New SqlCommand(q, myConnection)
        myCommand.ExecuteNonQuery()

        If numOperAnt.Text <> "" Then
            q = "UPDATE ideMens SET numOperAnt='" + numOperAnt.Text.Trim + "', fechaPresentacionAnt='" + Convert.ToDateTime(fechaPresentacionAnt.Text.Trim).ToString("yyyy-MM-dd") + "' WHERE id=" + id.Text + " AND idAnual=" + idAnual.Text + " AND mes='" + mes.ToString + "'"
            myCommand5 = New SqlCommand(q, myConnection)
            myCommand5.ExecuteNonQuery()
        End If

        If Request.QueryString("op") = "0" Then 'no se valida para 0s o consulta
            Call creaXMLmes() 'actualizo el zip del xml y lo copia a BD con los datos guardados
        End If
        Response.Write("<script language='javascript'>alert('Cambios guardados');</script>")
        descrip.Text = ""
    End Sub

    Private Sub bajaBDxmlMens()
        'bajar de la BD
        Dim myCommand2 As New SqlCommand
        With myCommand2
            .Connection = myConnection
            .CommandType = CommandType.StoredProcedure
            .CommandText = "ideMensBajaxml"
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
        Call regresar()
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

        Dim fechaUltima = DateAdd(DateInterval.Day, -DatePart(DateInterval.Day, Now()) + 1, Now()) 'dia 1o del mes actual
        fechaUltima = DateAdd(DateInterval.Month, -1, fechaUltima) 'dia 1o del mes anterior
        q = "SELECT co.id, pla.elplan FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND ( (nDeclHechas < nDeclContratadas and (pla.elplan<>'PREMIUM')) or ('" + Convert.ToDateTime(fechaUltima).ToString("yyyy-MM-dd") + "' <= fechaFinal and pla.elplan='PREMIUM' and co.esRegularizacion=0) or (pla.elplan='PREMIUM' and co.esRegularizacion=1) ) order by case when pla.elplan='PREMIUM' then 1 else 2 end, pla.elplan, co.id"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        Dim elplan = dr("elplan")
        dr.Close()

        If normalComplementaria.Text = "NORMAL" Then
            tipo = "N"
            idArch = ""
        Else
            tipo = "C"
            idArch = id.Text
        End If
        Dim fechaHora = Now().ToString("yyyy-MM-dd HH:mm:ss")
        Dim fechaHoraFmt = fechaHora.Replace(" ", "_").Replace(":", "-")
        nomArchMens = "C:\SAT\" + casfim + "\" + "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML"
        nomArchMensSinPath = "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML"
        archivoLocal = nomArchMens + ".ZIP"
        archivoLocalSinDir = nomArchMensSinPath + ".ZIP"

        If Not File.Exists(archivoLocal) Then
            Response.Write("<script language='javascript'>alert('Esta declaración ya se envió anteriormente, o no ha realizado Importación/Crear para este tipo de declaración, si su declaración es con datos pruebe importar nuevamente, si va a declarar en ceros puede importar un archivo de excel sin registros de detalle sino unicamente el encabezado');</script>")
            Exit Sub
        End If
        Dim nomArchMens2 = "C:\SAT\" + casfim + "\" + "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + fechaHoraFmt + ".XML.ZIP"
        Dim nomArchMensSinPath2 = "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + fechaHoraFmt + ".XML.ZIP"
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
            myCommand2 = New SqlCommand("UPDATE ideMens SET estado='ERROR_ENVIO' WHERE id=" + id.Text, myConnection)
            myCommand2.ExecuteNonQuery()

            Dim elcorreo As New System.Net.Mail.MailMessage
            Using elcorreo
                elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                elcorreo.To.Add("declaracioneside@gmail.com")
                elcorreo.Subject = "Decl. mens ERROR_ENVIO"
                elcorreo.Body = "<html><body>cliente=" + Session("curCorreo") + ", ejercicio=" + ejercicio + ", mes=" + mes + ", error=" + resultado + "</body></html>"
                elcorreo.IsBodyHtml = True
                elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                Dim smpt As New System.Net.Mail.SmtpClient
                smpt.Host = "smtp.gmail.com"
                smpt.Port = "587"
                smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a.")
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
            Dim MSG As String = "<script language='javascript'>alert('" + descrip.Text + "');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            'Response.Write("<script language='javascript'>alert('Error de transmisión en servidor del SAT, notifiquelo al proveedor o espere a que sea restablecido el servidor del SAT: " + resultado + "');</script>")
        Else
            If elplan <> "PREMIUM" Then
                q = "UPDATE contratos SET nDeclHechas=nDeclHechas+1 WHERE id=" + Session("GidContrato").ToString
                myCommand = New SqlCommand(q, myConnection)
                myCommand.ExecuteNonQuery()
            End If

            estado.Text = "ACEPTADA"
            fechaEnvio.Text = fechaHora
            myCommand2 = New SqlCommand("UPDATE ideMens SET estado='ACEPTADA', fechaEnvio='" + fechaEnvio.Text + "', acuseSolicitado=0 WHERE id=" + id.Text, myConnection)
            myCommand2.ExecuteNonQuery()

            'Response.Write("<script language='javascript'>alert('Envio exitoso');</script>")
            descrip.Text = resultado
            Dim MSG As String = "<script language='javascript'>alert('" + resultado + "');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)

            Dim elcorreo As New System.Net.Mail.MailMessage
            Using elcorreo
                elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                elcorreo.To.Add(Session("curCorreo"))
                elcorreo.Subject = "Declaración Mensual ejercicio " + ejercicio + " mes " + mes + ", constancia de envío"
                elcorreo.Body = "<html><body>Evidencia de envío: <br /><br />" + resultado + "<br /><br />Favor de conservar este correo para rastreo de acuses en caso necesario; En cuanto el SAT deposite el acuse en nuestros servidores, podrá descargarlo de nuestra página o bien si después de 3 dias hábiles no lo puede bajar, solicítelo a este correo y le será enviado en caso de ya haberlo recibido del SAT, Saludos. <br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet<br>Tel 01 443 690 3616<br>Correo declaracioneside@gmail.com<br><a href='https://twitter.com/declaracionesid' target='_blank'><img src='declaracioneside.com/twitter.jpg' alt='Clic aquí, siguenos en twitter' Height='30px' Width='30px' BorderWidth ='0px'></a>&nbsp;<a href='http://www.youtube.com/user/declaracioneside' target='_blank'> <img src='declaracioneside.com/iconoyoutube.png'  alt='Suscribete a nuestro canal declaraciones de depósitos en efectivo e IDE en youtube' Height='30px' Width='30px' BorderWidth ='0px'></a> &nbsp;<a href='http://www.facebook.com/depositosenefectivo' target='_blank'><img src='declaracioneside.com/facebook.jpg' alt='Clic aquí para seguirnos en facebook' Height='30px' Width='30px' BorderWidth ='0px'></a>&nbsp;&nbsp;<a href='https://mx.linkedin.com/in/declaraciones-depósitos-en-efectivo-1110125b' target='_blank'><img src='declaracioneside.com/linkedin.png' alt='Siguenos en linkedin' Height='30px' Width='30px' BorderWidth ='0px'></a>&nbsp;<a href='http://plus.google.com/107594546767340388428?prsrc=3'><img src='http://ssl.gstatic.com/images/icons/gplus-32.png' alt='Google+' Height='30px' Width='30px' BorderWidth ='0px'></a><br /></body></html>"
                elcorreo.IsBodyHtml = True
                elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                Dim smpt As New System.Net.Mail.SmtpClient
                smpt.Host = "smtp.gmail.com"
                smpt.Port = "587"
                smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a.")
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
        File.Delete(nomArchMens2)

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
        '    descrip.Text = "Aplicación de hacienda no se lanzó"
        '    'Response.Write("<script language='javascript'>alert('Aplicación de hacienda no se lanzó');</script>")
        'Else
        '    'Call GetClassName(nWnd, sClassName, 256)
        '    'clase = sClassName.ToString.Replace("Window.8", "EDIT")
        '    hWnd = FindWindowEx(nWnd, IntPtr.Zero, "ThunderRT6TextBox", "")    'acuse: ruta/repositorio
        '    If Not hWnd.Equals(ceroIntPtr) Then
        '        hWnd2 = FindWindowEx(nWnd, hWnd, "ThunderRT6TextBox", "")      'acuse: cuenta sat (login remoto )
        '        If Not hWnd2.Equals(ceroIntPtr) Then
        '            hWnd3 = FindWindowEx(nWnd, hWnd2, "ThunderRT6TextBox", "")     'resultados del comando
        '            If Not hWnd3.Equals(ceroIntPtr) Then
        '                hWnd4 = FindWindowEx(nWnd, hWnd3, "ThunderRT6TextBox", "") 'tx: login remoto
        '                If Not hWnd4.Equals(ceroIntPtr) Then
        '                    retval4 = SendMessage(hWnd4, WM_SETTEXT, IntPtr.Zero, loginSAT)
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
        '                                myCommand2 = New SqlCommand("UPDATE ideMens SET estado='ERROR_ENVIO' WHERE id=" + id.Text, myConnection)
        '                                myCommand2.ExecuteNonQuery()

        '                                Dim elcorreo As New System.Net.Mail.MailMessage
        '                                Using elcorreo
        '                                    elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
        '                                    elcorreo.To.Add("declaracioneside@gmail.com")
        '                                    elcorreo.Subject = "Decl. mens ERROR_ENVIO"
        '                                    elcorreo.Body = "<html><body>cliente=" + session("curCorreo") + ", ejercicio=" + ejercicio + ", mes=" + mes + ", error=" + resultado + "</body></html>"
        '                                    elcorreo.IsBodyHtml = True
        '                                    elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        '                                    Dim smpt As New System.Net.Mail.SmtpClient
        '                                    smpt.Host = "smtp.gmail.com"
        '                                    smpt.Port = "587"
        '                                    smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a.")
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
        '                                myCommand2 = New SqlCommand("UPDATE ideMens SET estado='ACEPTADA' WHERE id=" + id.Text, myConnection)
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
        '                        descrip.Text = "Componente cuenta sat / login transmisor no localizado"
        '                    End If
        '                Else
        '                    'Response.Write("<script language='javascript'>alert('Componente login transmisor no localizado');</script>")
        '                    descrip.Text = "Componente archivo declaracion / archivo local no localizado"
        '                End If
        '            Else
        '                'Response.Write("<script language='javascript'>alert('Componente login transmisor no localizado');</script>")
        '                descrip.Text = "Componente de mensajes de aplicacion SAT no localizado"
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
    'Private Sub enviarDeclaracion() 'version pruebaIDE.exe pero no lo abre en el server por ondas del .net
    '    Dim loginSAT, archivoLocal, directorioServidor, casfim, tipo, idArch
    '    Dim q = "SELECT loginSAT,directorioServidor,casfim FROM clientes WHERE id=" + session("GidCliente").ToString

    '    descrip.Text = ""

    '    myCommand = New SqlCommand(q, myConnection)
    '    dr = myCommand.ExecuteReader()
    '    dr.Read()
    '    loginSAT = dr("loginSAT")
    '    casfim = dr("casfim")
    '    directorioServidor = "C:\SAT\" + dr("directorioServidor")
    '    dr.Close()

    '    Dim fechaUltima = DateAdd(DateInterval.Day, -DatePart(DateInterval.Day, Now()) + 1, Now()) 'dia 1o del mes actual
    '    fechaUltima = DateAdd(DateInterval.Month, -1, fechaUltima) 'dia 1o del mes anterior
    '    q = "SELECT co.id, pla.elplan FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + session("curCorreo") + "' AND ( (nDeclHechas < nDeclContratadas and (pla.elplan<>'PREMIUM')) or ('" + Format(Convert.ToDateTime(fechaUltima), "yyyy-MM-dd") + "' <= fechaFinal and pla.elplan='PREMIUM') ) order by case when pla.elplan='PREMIUM' then 1 else 2 end, pla.elplan, co.id"
    '    myCommand = New SqlCommand(q, myConnection)
    '    dr = myCommand.ExecuteReader()
    '    dr.Read()
    '    Dim elplan = dr("elplan")
    '    dr.Close()

    '    If normalComplementaria.Text = "NORMAL" Then
    '        tipo = "N"
    '        idArch = ""
    '    Else
    '        tipo = "C"
    '        idArch = id.Text
    '    End If
    '    nomArchMens = "C:\SAT\" + casfim + "\" + "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML"
    '    nomArchMensSinPath = "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML"
    '    archivoLocal = nomArchMens + ".ZIP"


    '    If Not File.Exists(archivoLocal) Then
    '        Response.Write("<script language='javascript'>alert('Esta declaración ya se envió anteriormente, o no ha realizado Importación/Crear para este tipo de declaración, si va a declarar en ceros puede importar un archivo de excel sin registros de detalle sino unicamente el encabezado');</script>")
    '        Exit Sub
    '    End If


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
    '        descrip.Text = "Aplicación de hacienda no se lanzó"
    '        'Response.Write("<script language='javascript'>alert('Aplicación de hacienda no se lanzó');</script>")
    '    Else
    '        Call GetClassName(nWnd, sClassName, 256)
    '        clase = sClassName.ToString.Replace("Window.8", "EDIT")
    '        hWnd = FindWindowEx(nWnd, IntPtr.Zero, clase, "")    'acuse: ruta/repositorio
    '        'hWnd = FindWindowEx(nWnd, IntPtr.Zero, "ThunderRT6TextBox", "")    'acuse: ruta/repositorio
    '        If Not hWnd.Equals(ceroIntPtr) Then
    '            hWnd2 = FindWindowEx(nWnd, hWnd, clase, "")      'acuse: cuenta sat (login remoto )
    '            If Not hWnd2.Equals(ceroIntPtr) Then
    '                hWnd4 = FindWindowEx(nWnd, hWnd2, clase, "") 'tx: archivo declaracion/local
    '                If Not hWnd4.Equals(ceroIntPtr) Then
    '                    retval4 = SendMessage(hWnd4, WM_SETTEXT, IntPtr.Zero, archivoLocal)
    '                    hWnd5 = FindWindowEx(nWnd, hWnd4, clase, "") 'tx: cuenta sat (login remoto)
    '                    If Not hWnd5.Equals(ceroIntPtr) Then
    '                        retval5 = SendMessage(hWnd5, WM_SETTEXT, IntPtr.Zero, loginSAT)
    '                        clase = sClassName.ToString.Replace("Window.8", "STATIC")
    '                        hWnd3 = FindWindowEx(nWnd, IntPtr.Zero, clase, "")    'resultados del comando
    '                        SetActiveWindow(nWnd)
    '                        clase = sClassName.ToString.Replace("Window.8", "BUTTON")
    '                        hWnd6 = FindWindowEx(nWnd, IntPtr.Zero, clase, "Envia Declaracion") 'Procesar (&Subrayado)
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
    '                            If InStr(resultado, "ERROR") Or InStr(resultado, "FALLA") Or InStr(resultado, "Atencion") Or InStr(resultado, "errno") Then 'o distinto de OK
    '                                estado.Text = "ERROR_ENVIO"
    '                                myCommand2 = New SqlCommand("UPDATE ideMens SET estado='ERROR_ENVIO' WHERE id=" + id.Text, myConnection)
    '                                myCommand2.ExecuteNonQuery()

    '                                Dim elcorreo As New System.Net.Mail.MailMessage
    '                                Using elcorreo
    '                                    elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
    '                                    elcorreo.To.Add("declaracioneside@gmail.com")
    '                                    elcorreo.Subject = "Decl. mens ERROR_ENVIO"
    '                                    elcorreo.Body = "<html><body>cliente=" + session("curCorreo") + ", ejercicio=" + ejercicio + ", mes=" + mes + ", error=" + resultado + "</body></html>"
    '                                    elcorreo.IsBodyHtml = True
    '                                    elcorreo.Priority = System.Net.Mail.MailPriority.Normal
    '                                    Dim smpt As New System.Net.Mail.SmtpClient
    '                                    smpt.Host = "smtp.gmail.com"
    '                                    smpt.Port = "587"
    '                                    smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a.")
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
    '                                myCommand2 = New SqlCommand("UPDATE ideMens SET estado='ACEPTADA' WHERE id=" + id.Text, myConnection)
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
    '                        descrip.Text = "Componente cuenta sat / login transmisor no localizado"
    '                    End If
    '                Else
    '                    'Response.Write("<script language='javascript'>alert('Componente login transmisor no localizado');</script>")
    '                    descrip.Text = "Componente archivo declaracion / archivo local no localizado"
    '                End If
    '                'para la version anterior del testacusevb los resultados del comando van en un text, para testIDE van en un static(label/caption)
    '            Else
    '                'Response.Write("<script language='javascript'>alert('Componente login acuses no localizado');</script>")
    '                descrip.Text = "Componente cuenta sat / login acuses no localizado"
    '            End If
    '        Else
    '            'Response.Write("<script language='javascript'>alert('Componente directorio no localizado');</script>")
    '            descrip.Text = "Componente repositorio/directorio acuses no localizado"
    '        End If
    '    End If

    'End Sub

    Private Function validaModificada()
        Dim q = "SELECT guardadaUsuario FROM ideMens WHERE id=" + Session("GidMens").ToString
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            If dr("guardadaUsuario").Equals(False) Then
                dr.Close()
                Response.Write("<script language='javascript'>alert('1o Guarde los datos de la declaración presionando el botón modificar');</script>")
                Return 0
            End If
            dr.Close()
        Else
            dr.Close()
            Response.Write("<script language='javascript'>alert('1o importe los datos o creela en ceros');</script>")
            Return 0
        End If
        dr.Close()

        Return 1
    End Function

    Protected Sub btnEnviarDeclaracion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEnviarDeclaracion.Click
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

            progressbar.Style("width") = "0px"
            statusImport.Text = ""
        End If

        Dim mes2dig, contra
        If mes.ToString.Length = 1 Then
            mes2dig = "0" & mes.ToString
        Else
            mes2dig = mes.ToString
        End If

        Dim fechaDeclarar = Convert.ToDateTime(Trim("01/" + mes2dig + "/" + ejercicio.ToString)).ToString("yyyy-MM-dd")

        q = "SELECT co.id, pla.elplan FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND co.id=" + Session("GidContrato").ToString + " AND ( (nDeclHechas < nDeclContratadas and (pla.elplan<>'PREMIUM')) or (('" + Convert.ToDateTime(fechaDeclarar).ToString("yyyy-MM-dd") + "' >= periodoInicial and pla.elplan='PREMIUM') and ('" + Convert.ToDateTime(fechaDeclarar).ToString("yyyy-MM-dd") + "' <= fechaFinal and pla.elplan='PREMIUM') ) ) order by co.id"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If (Not dr.HasRows) Then 'sin contrato vigente 
            Response.Write("<script language='javascript'>alert('A alcanzado el máximo de declaraciones contratadas o bien ha caducado su contrato, o los periodos a declarar no están cubiertos por este contrato');</script>")
            Exit Sub
        End If
        dr.Close()

        controlaAcceso()
        If redir.Text = "1" Then
            Exit Sub
        End If

        Call enviarDeclaracion()
    End Sub

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
                Response.Write("<script language='javascript'>alert('No hay contratos pagados');</script>")
                Response.Write("<script>location.href='misContra.aspx';</script>")
                Exit Sub
            End If
            dr.Close()


    End Sub

    Private Sub extraeNumoperDeAcuse(ByVal allRead As String)
        Dim pos, pos2, tam, q, numOperV, fechaPresentacionV, rfcV, denominacionV, recaudadoV, enteradoV, ejercicioV, periodoV, tipoV, folioV, archivoV, selloV

        pos = allRead.IndexOf("fechaPresentacion")
        'pos2 = allRead.IndexOf("""", pos)
        'tam = pos2 - pos + 1
        fechaPresentacionV = allRead.Substring(pos + 19, 10)

        pos = allRead.IndexOf("numeroOperacion")
        pos2 = allRead.IndexOf("""", pos + 17)
        tam = pos2 - pos - 17
        numOperV = allRead.Substring(pos + 17, tam)

        q = "UPDATE ideMens SET numOper='" + numOperV + "', fechaPresentacion='" + Convert.ToDateTime(fechaPresentacionV).ToString("yyyy-MM-dd") + "' WHERE id=" + id.Text
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
        denominacionV = allRead.Substring(pos + 14, tam)

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

        pos = allRead.IndexOf("periodo")
        pos2 = allRead.IndexOf("""", pos + 9)
        tam = pos2 - pos - 9
        periodoV = allRead.Substring(pos + 9, tam)

        pos = allRead.IndexOf("tipo")
        pos2 = allRead.IndexOf("""", pos + 6)
        tam = pos2 - pos - 6
        tipoV = allRead.Substring(pos + 6, tam)

        pos = allRead.IndexOf("totalRecaudado")
        pos2 = allRead.IndexOf("""", pos + 16)
        tam = pos2 - pos - 16
        recaudadoV = allRead.Substring(pos + 16, tam)

        pos = allRead.IndexOf("totalEnterado")
        pos2 = allRead.IndexOf("""", pos + 15)
        tam = pos2 - pos - 15
        enteradoV = allRead.Substring(pos + 15, tam)

        pos = allRead.IndexOf("sello")
        pos2 = allRead.IndexOf("""", pos + 7)
        tam = pos2 - pos - 7
        selloV = allRead.Substring(pos + 7, tam)

        Session("numOperAcuse") = numOperV
        Session("fechaPresentacionAcuse") = fechaPresentacionV
        Session("rfcAcuse") = rfcV
        Session("denominacionAcuse") = denominacionV
        Session("recaudadoAcuse") = recaudadoV
        Session("enteradoAcuse") = enteradoV
        Session("ejercicioAcuse") = ejercicioV
        Session("periodoAcuse") = periodoV
        Session("tipoAcuse") = tipoV
        Session("folioAcuse") = folioV
        Session("archivoAcuse") = archivoV
        Session("selloAcuse") = selloV
    End Sub

    Private Sub bajarAcuse()
        progressbar.Style("width") = "0px"
        statusImport.Text = ""
        descrip.Text = ""

        Dim qAcuseSolicitado, qFechaEnvio
        Dim q = "SELECT id,estado,acuseSolicitado,fechaEnvio FROM ideMens WHERE idAnual=" + idAnual.Text + " and id='" + id.Text + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If Not dr.Read() Then
            Response.Write("<script language='javascript'>alert('No se ha producido información para esta declaración ya sea vía importación o en ceros');</script>")
            dr.Close()
            Exit Sub
        ElseIf dr("estado") = "VACIA" Or dr("estado") = "CREADA" Or dr("estado") = "IMPORTADA" Or dr("estado") = "ERROR_ENVIO" Then
            Response.Write("<script language='javascript'>alert('1o necesita Enviar satisfactoriamente esta declaración o bien bajar y presentar la contingencia');</script>")
            dr.Close()
            Exit Sub
        End If
        qAcuseSolicitado = dr("acuseSolicitado")
        qFechaEnvio = dr("fechaEnvio")
        dr.Close()

        Try

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
            nomArchMens = "C:\SAT\" + casfim + "\" + "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML"
            nomArchMensSinPath = "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML"
            If CDate(qFechaEnvio).ToString("yyyy-MM-dd") >= "2017-03-15" Then  'cambio de nomenclatura de archivos
                Dim fechaHoraFmt = CDate(qFechaEnvio).ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "_").Replace(":", "-")
                nomArchMens = "C:\SAT\" + casfim + "\" + "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + fechaHoraFmt + ".XML"
                nomArchMensSinPath = "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + fechaHoraFmt + ".XML"
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
            '        hWnd2 = FindWindowEx(nWnd, hWnd, "ThunderRT6TextBox", "")      'acuse: cuenta sat (login remoto )
            '        If Not hWnd2.Equals(ceroIntPtr) Then
            '            retval2 = SendMessage(hWnd2, WM_SETTEXT, IntPtr.Zero, loginSAT) 'loginSAT
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
            '                    If InStr(resultado, "ERROR") Or InStr(resultado, "FALLA") Or InStr(resultado, "Falla") Or InStr(resultado, "Atencion") Or InStr(resultado, "errno") Then 'distinto de Exito

            '                        Dim elcorreo As New System.Net.Mail.MailMessage
            '                        Using elcorreo
            '                            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
            '                            elcorreo.To.Add("declaracioneside@gmail.com")
            '                            elcorreo.Subject = "Decl. mens ERROR_ACUSE"
            '                            elcorreo.Body = "<html><body>cliente=" + session("curCorreo") + ", ejercicio=" + ejercicio + ", mes=" + mes + ", error=" + resultado + "</body></html>"
            '                            elcorreo.IsBodyHtml = True
            '                            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
            '                            Dim smpt As New System.Net.Mail.SmtpClient
            '                            smpt.Host = "smtp.gmail.com"
            '                            smpt.Port = "587"
            '                            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a.")
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

            Dim di As New IO.DirectoryInfo(directorioServidor + "\")
            Dim diar1 As IO.FileInfo() = di.GetFiles("*.xml")
            Dim dra As IO.FileInfo
            Dim fName As String
            Dim allRead As String
            Dim regMatch As String 'string to search for inside of text file. It is case sensitive.
            regMatch = nomArchMensSinPath  'buscando el nomArchMensSinPath como texto dentro del archivo        
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
                            'elcorreo.CC.Add("miguel.chantes@sat.gob.mx")
                            elcorreo.CC.Add("declaracioneside@gmail.com")
                            elcorreo.Subject = "Solicitud de acuses"
                            elcorreo.Body = "<html><body>Buen dia<br><br>Nos podría proporcionar los acuses de la declaración mensual del año " + ejercicio.ToString + " mes " + mes.ToString + " de " + razonSoc + ", casfim " + casfim + ", Enviado en la fecha (año-mes-dia): " + CDate(qFechaEnvio).ToString("yyyy-MM-dd") + ", en el archivo " + nomArchMensSinPath + ".ZIP" + " <br><br>Atentamente, <a href='declaracioneside.com'>declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet </body></html>"
                            elcorreo.IsBodyHtml = True
                            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                            Dim smpt As New System.Net.Mail.SmtpClient
                            smpt.Host = "smtp.gmail.com"
                            smpt.Port = "587"
                            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a.")
                            smpt.EnableSsl = True 'req p server gmail
                            Try
                                smpt.Send(elcorreo)
                                elcorreo.Dispose()
                                descrip.Text = descrip.Text + ". Acuses solicitados al SAT espere a que resuelvan la solicitud reitentando posteriormente"
                                myCommand = New SqlCommand("UPDATE ideMens SET acuseSolicitado=1 WHERE id=" + id.Text, myConnection)
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
                    Dim MSG As String = "<script language='javascript'>alert('" + descrip.Text + "');</script>"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
                Else
                    zip.Save(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP")
                    For j = 0 To cont - 1
                        'AddFileSecurity(listaAcuses(j), Session("identidad"), FileSystemRights.FullControl, AccessControlType.Allow)
                        File.Delete(listaAcuses(j)) 'borrando pdfs de acuses
                    Next

                    File.Delete(nomArchMens + ".ZIP")   'borro el zip de la declaracion enviada

                    Dim elcorreo As New System.Net.Mail.MailMessage
                    Using elcorreo
                        elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                        elcorreo.To.Add(Session("curCorreo"))
                        'elcorreo.To.Add("declaracioneside@gmail.com")
                        elcorreo.Subject = "Acuses IDE, ejercicio " + ejercicio.ToString + " mes " + mes.ToString
                        elcorreo.Body = "<html><body>Buen dia<br><br>Se adjunta el archivo con los acuses del periodo <br><br>Los acuses de aceptación y rechazo respetaran la siguiente conformación para el nombramiento de los archivos:<br><br>AXYIIIIIAAAAMMDDHHMM.XML<br><br>En donde:<br><br>A es el identificador de archivo de ACUSE<br>X es el identificador de tipo de acuse siendo las posibles opciones: (A de Aceptado, R de Rechazo)<br>Y es el identificador de Tipo de declaración, siendo las posibles opciones: (M de Mensual, A de Anual)<br>IIIII es la clave de la Institución financiera que envía<br>AAAA es el año en que se proceso el acuse<br>MM es el mes en que se proceso el acuse en formato 2 cifras<br>DD es el día en que se proceso el acuse<br>HH es la hora en que se proceso el acuse<br>MM son los minutos en que se proceso el acuse<br>SS son los segundos en que se proceso el acuse <br><br>Atentamente, <a href='declaracioneside.com'>declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet </body></html>"
                        elcorreo.IsBodyHtml = True
                        elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                        elcorreo.Attachments.Add(New System.Net.Mail.Attachment(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP"))
                        Dim smpt As New System.Net.Mail.SmtpClient
                        smpt.Host = "smtp.gmail.com"
                        smpt.Port = "587"
                        smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a.")
                        smpt.EnableSsl = True 'req p server gmail
                        Try
                            smpt.Send(elcorreo)
                            elcorreo.Dispose()

                            myCommand = New SqlCommand("UPDATE ideMens SET acuseDescargado=1 WHERE id=" + id.Text, myConnection)
                            myCommand.ExecuteNonQuery()

                            'Response.Write("<script language='javascript'>alert('Envío exitoso de acuses presentes en el sistema a su correo');</script>")
                            descrip.Text = "Envío exitoso de acuses presentes en el sistema a su correo"
                            Dim MSG As String = "<script language='javascript'>alert('Envío exitoso de acuses presentes en el sistema a su correo');</script>"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
                        Catch ex As Exception
                            'Response.Write("<script language='javascript'>alert('Error enviando acuses a su correo: " & ex.Message + "');</script>")
                            descrip.Text = "Error enviando acuses a su correo: " & ex.Message
                            Dim MSG As String = "<script language='javascript'>alert('" + descrip.Text + "');</script>"
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
                            Exit Sub
                        Finally
                            File.Delete(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP")
                        End Try
                    End Using
                End If
            End Using
        Catch ex1 As Exception
            descrip.Text = "Error al convertir acuse" 'ex1.Message '
            Dim MSG As String = "<script language='javascript'>alert('Error al convertir acuse');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Dim elcorreo As New System.Net.Mail.MailMessage
            Using elcorreo
                elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                elcorreo.To.Add("declaracioneside@gmail.com")
                elcorreo.Subject = "Acuses IDE " + Session("curCorreo") + ", ejercicio " + ejercicio.ToString + " mes " + mes.ToString + "Error al convertir acuse"
                elcorreo.Body = "<html><body>" + ex1.ToString + "</body></html>"
                elcorreo.IsBodyHtml = True
                elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                Dim smpt As New System.Net.Mail.SmtpClient
                smpt.Host = "smtp.gmail.com"
                smpt.Port = "587"
                smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a.")
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
        ''Response.Write("<script language='javascript'>alert('Componente Resultados no localizado');</script>")
        'descrip.Text = "Componente de mensajes de aplicacion SAT no localizado"
        '            End If

        ''el campo de resultados en la vers ant del testacusevb era un text, aqui es un caption/label/static
        '        Else
        ''Response.Write("<script language='javascript'>alert('Componente login acuses no localizado');</script>")
        'descrip.Text = "Componente cuenta sat / login remoto acuses no localizado"
        '        End If
        '        Else
        ''Response.Write("<script language='javascript'>alert('Componente directorio no localizado');</script>")
        'descrip.Text = "Componente repositorio/ruta/directorio acuses no localizado"
        '        End If
        'End If
    End Sub

    Private Function acusePdf(ByVal estatus, ByVal ruta, ByVal arch, ByVal casfim) As String
        'Generando doc del acuse

        'If (File.Exists(ruta + "\acuseMensual.doc")) Then
        '    'AddFileSecurity(ruta + "\acuseMensual.doc", Session("identidad"), FileSystemRights.FullControl, AccessControlType.Allow)
        '    File.Delete(ruta + "\acuseMensual.doc")
        'End If

        'Dim p As New Process
        'p.StartInfo.FileName = "C:\inetpub\wwwroot\docAcuse.exe"
        'p.StartInfo.Arguments = "M" + "'" + Session("rfcAcuse") + "'" + Session("denominacionAcuse") + "'" + Session("recaudadoAcuse") + "'" + Session("enteradoAcuse") + "'" + Session("ejercicioAcuse") + "'" + Session("periodoAcuse") + "'" + Session("tipoAcuse") + "'" + Session("fechaPresentacionAcuse") + "'" + Session("folioAcuse") + "'" + Session("numOperAcuse") + "'" + Session("archivoAcuse") + "'" + Session("selloAcuse") + "'" + estatus + "'" + casfim
        'p.Start()
        'p.WaitForExit()

        ''WORD TO PDF
        'Dim newApp As Microsoft.Office.Interop.Word.Application = New Microsoft.Office.Interop.Word.Application
        ''Dim newApp As New Word.Application()
        'Dim Source As Object = "C:\SAT\" + casfim + "\acuseMensual.doc"
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
        Return docAcuse("M" + "'" + Session("rfcAcuse") + "'" + Session("denominacionAcuse") + "'" + Session("recaudadoAcuse") + "'" + Session("enteradoAcuse") + "'" + Session("ejercicioAcuse") + "'" + Session("periodoAcuse") + "'" + Session("tipoAcuse") + "'" + Session("fechaPresentacionAcuse") + "'" + Session("folioAcuse") + "'" + Session("numOperAcuse") + "'" + Session("archivoAcuse") + "'" + Session("selloAcuse") + "'" + estatus + "'" + casfim, ruta, arch)

    End Function

    Private Function docAcuse(ByVal Command As String, ByVal ruta As String, ByVal arch As String) As String
        Dim diseño = Server.MapPath("~/acuseMensual.frx")
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
        TextEncab.Text = "Acuse Recepción Mensual IDE"

        If Data(13) = "R" Then
            TextEstatus.Text = "Error en acuse, contáctenos"
        Else
            TextEmisorNom.Text = Data(2)
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
                ElseIf Request.QueryString("subop") = "1" Then  'xml
                    MultiView1.ActiveViewIndex = Int32.Parse(5)
                End If
                cargaGrid()
        End Select

    End Sub

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

        progressbar.Style("width") = "0px"
        statusImport.Text = ""

        Dim q, contra, elplan
        Dim mes2dig
        If mes.ToString.Length = 1 Then
            mes2dig = "0" & mes.ToString
        Else
            mes2dig = mes.ToString
        End If

        Dim fechaDeclarar = Convert.ToDateTime(Trim("01/" + mes2dig + "/" + ejercicio.ToString)).ToString("yyyy-MM-dd")
        q = "SELECT co.id, pla.elplan FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND co.id=" + Session("GidContrato").ToString + " AND ( (nDeclHechas < nDeclContratadas and (pla.elplan<>'PREMIUM')) or (('" + Convert.ToDateTime(fechaDeclarar).ToString("yyyy-MM-dd") + "' >= periodoInicial and pla.elplan='PREMIUM') and ('" + Convert.ToDateTime(fechaDeclarar).ToString("yyyy-MM-dd") + "' <= fechaFinal and pla.elplan='PREMIUM') ) ) order by case when pla.elplan='PREMIUM' then 1 else 2 end, pla.elplan, co.id"
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
        ContNomArchMens = "C:\SAT\" + casfim + "\" + "ContM-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML.ZIP"

        'bajar de la BD
        If File.Exists(ContNomArchMens) Then
            File.Delete(ContNomArchMens)
        End If
        Dim myCommand2 As New SqlCommand
        With myCommand2
            .Connection = myConnection
            .CommandType = CommandType.StoredProcedure
            .CommandText = "ideMensBajaxml"
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
                elcorreo.Subject = "Contingencia IDE por el proveedor, ejercicio " + ejercicio.ToString + " mes " + mes.ToString + " de " + razon + " (" + Session("curCorreo") + ")"
            Else 'contribuyente
                elcorreo.To.Add(Session("curCorreo"))
                elcorreo.Subject = "Contingencia IDE por el contribuyente, ejercicio " + ejercicio.ToString + " mes " + mes.ToString + " de " + razon + " (" + Session("curCorreo") + ")"
            End If
            elcorreo.Body = "<html><body>Buen dia<br><br>Se adjunta el archivo de contingencia del periodo, <br><br> en el siguiente enlace se encuentra el <a href='ftp://ftp2.sat.gob.mx/asistencia_servicio_ftp/publicaciones/IDE08/IDE_contingencia_nov10.pdf'>Instructivo</a><br><br>Atentamente, <a href='declaracioneside.com'>declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet </body></html>"
            elcorreo.IsBodyHtml = True
            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
            elcorreo.Attachments.Add(New System.Net.Mail.Attachment(ContNomArchMens))
            Dim smpt As New System.Net.Mail.SmtpClient
            smpt.Host = "smtp.gmail.com"
            smpt.Port = "587"
            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a.")
            smpt.EnableSsl = True 'req p server gmail
            Try
                smpt.Send(elcorreo)
                elcorreo.Dispose()
                Response.Write("<script language='javascript'>alert('Envío exitoso de contingencia a su correo');</script>")
            Catch ex As Exception
                Response.Write("<script language='javascript'>alert('Error enviando contingencia a su correo: " & ex.Message + "');</script>")
                Exit Sub
            Finally
                If File.Exists(ContNomArchMens) Then 'borro el zip
                    File.Delete(ContNomArchMens)
                End If
            End Try
        End Using

        If elplan <> "PREMIUM" Then
            q = "UPDATE contratos SET nDeclHechas=nDeclHechas+1 WHERE id=" + Session("GidContrato").ToString
            myCommand = New SqlCommand(q, myConnection)
            myCommand.ExecuteNonQuery()
        End If

        estado.Text = "CONTINGENCIA"
        myCommand3 = New SqlCommand("UPDATE ideMens SET estado='CONTINGENCIA' WHERE id=" + id.Text, myConnection)
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

    Private Sub creaTagsMensCeros()
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

        'M=mensual
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

        nomArchMens = "C:\SAT\" + casfim + "\" + "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML"
        nomArchMensSinPath = "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML"

        If File.Exists(nomArchMens) Then
            File.Delete(nomArchMens)
        End If

        Dim archivo As StreamWriter = File.CreateText(nomArchMens)
        archivo.WriteLine("<?xml version='1.0' encoding='UTF-8'?>")
        archivo.WriteLine("    <DeclaracionInformativaMensualIDE xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:altova='http://www.altova.com/xml-schema-extensions' xsi:noNamespaceSchemaLocation='C:/SAT/ide_20130430.xsd' version='2.0' rfcDeclarante='" + Left(vRfc, 12) + "' denominacion='" + vempresa + "'>")
        archivo.WriteLine("                <RepresentanteLegal rfc='" + reprLegalRfc + "'>") 'mayusc
        archivo.WriteLine("                    <Nombre>")
        archivo.WriteLine("                        <Nombres>" + reprLegalNombres + "</Nombres>")
        archivo.WriteLine("                        <PrimerApellido>" + reprLegalAp1 + "</PrimerApellido>")
        archivo.WriteLine("                        <SegundoApellido>" + reprLegalAp2 + "</SegundoApellido>")
        archivo.WriteLine("                    </Nombre>")
        archivo.WriteLine("                </RepresentanteLegal>")
        If tipo = "N" Then
            archivo.WriteLine("                <Normal ejercicio='" + ejercicio.ToString + "' periodo='" + mes.ToString + "'></Normal>")
        Else
            archivo.WriteLine("                <Complementaria ejercicio='" + ejercicio.ToString + "' periodo='" + mes.ToString + "' opAnterior='" + numOperAnt.Text.Trim + "' fechaPresentacion='" + CDate(fechaPresentacionAnt.Text.Trim).ToString("yyyy-MM-dd") + "'></Complementaria>")
        End If
        If esInstitCredito = 1 Then
            archivo.WriteLine("                <InstitucionDeCredito>")
            archivo.WriteLine("                                 <Totales operacionesRelacionadas='0' importeExcedenteDepositos='0' importeDeterminadoDepositos='0' importeRecaudadoDepositos='0' importePendienteRecaudacion='0' importeRemanenteDepositos='0' importeEnterado='0' importeSaldoPendienteRecaudar='0' importeCheques='0'></Totales>")
            archivo.WriteLine("                 </InstitucionDeCredito>")
        Else
            archivo.WriteLine("                <InstitucionDistintaDeCredito>")
            archivo.WriteLine("                                 <Totales operacionesRelacionadas='0' importeExcedenteDepositos='0' importeDeterminadoDepositos='0' importeRecaudadoDepositos='0' importePendienteRecaudacion='0' importeRemanenteDepositos='0' importeEnterado='0' importeSaldoPendienteRecaudar='0'></Totales>")
            archivo.WriteLine("                 </InstitucionDistintaDeCredito>")
        End If


        archivo.WriteLine("     </DeclaracionInformativaMensualIDE>")

        archivo.Close()
    End Sub




    Protected Sub Crear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Crear.Click
        'If validaModificada() < 1 Then
        '    Exit Sub
        'End If

        descrip.Text = ""

        Dim q, contra
        Dim fechaUltima = DateAdd(DateInterval.Day, -DatePart(DateInterval.Day, Now()) + 1, Now()) 'dia 1o del mes actual
        fechaUltima = DateAdd(DateInterval.Month, -1, fechaUltima) 'dia 1o del mes anterior
        q = "SELECT co.id, pla.elplan FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND ( (nDeclHechas < nDeclContratadas and (pla.elplan<>'PREMIUM')) or ('" + Convert.ToDateTime(fechaUltima).ToString("yyyy-MM-dd") + "' <= fechaFinal and pla.elplan='PREMIUM' and co.esRegularizacion=0) or (pla.elplan='PREMIUM' and co.esRegularizacion=1) ) order by co.id"

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
            idAnual.Text = Session("GidAnual").ToString
            If Session("GidMens") = 0 Then 'sin mensual ->crearla
                Call insertaMensualVacia()
            Else
            End If
        End If


        Call creaXMLmesCeros() 'crea el zip del xml y lo copia a BD        

        estado.Text = "CREADA"
        myCommand2 = New SqlCommand("UPDATE ideMens SET estado='CREADA', idContrato=" + idContrato.ToString + ",normalComplementaria='" + normalComplementaria.Text + "' WHERE id=" + id.Text, myConnection)
        myCommand2.ExecuteNonQuery()

    End Sub

    Protected Sub verCeros_Click(ByVal sender As Object, ByVal e As EventArgs) Handles verCeros.Click
        MultiView1.ActiveViewIndex = Int32.Parse(4)
        cargaGrid()
        progressbar.Style("width") = "0px"
        statusImport.Text = ""
        descrip.Text = ""
    End Sub



    Protected Sub GridView3_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView3.SelectedIndexChanged

    End Sub

    Private Sub WebForm12_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'myConnection.Close()
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
            If Session("GidMens") = 0 Then 'sin mensual ->crearla
                Call insertaMensualVacia()
            Else
            End If
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
        nomArchMens = "C:\SAT\" + casfim + "\" + "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML"
        nomArchMensSinPath = "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML"

        If File.Exists(nomArchMens) Then
            File.Delete(nomArchMens)
        End If

        FileUpload2.SaveAs(nomArchMens)

        'AddFileSecurity(nomArchMens, Session("identidad"), FileSystemRights.FullControl, AccessControlType.Allow)
        'AddFileSecurity(savePath, "IIS_WPG", FileSystemRights.ReadData, AccessControlType.Allow)

        Dim dra As IO.FileInfo = New FileInfo(nomArchMens)
        Dim fName As String = dra.FullName 'path to text file
        Dim allRead As String
        Dim regMatch As String = "RegistroDeDetalle" 'string to search for inside of text file. It is case sensitive.
        Dim testTxt As StreamReader = New StreamReader(fName)
        allRead = testTxt.ReadToEnd() 'Reads the whole text file to the end
        testTxt.Close() 'Closes the text file after it is fully read.
        If (Regex.IsMatch(allRead, regMatch)) Then 'If match found in allRead
            verEnteroPropio(True)
        Else
            verEnteroPropio(False) 'importo en ceros
        End If

        If validacion() = False Then
            Exit Sub
        End If

        Call comprimeMens() 'borra xml crea zip
        Call subeXMLmensBD()
        statusImportXml.Text = " Importación IDE realizada "
        progressbarXml.Style("width") = "100px"
        estado.Text = "IMPORTADA"

        myCommand2 = New SqlCommand("UPDATE ideMens SET estado='IMPORTADA', idContrato=" + idContrato.ToString + ", viaImportacion=2 WHERE id=" + id.Text, myConnection)
        myCommand2.ExecuteNonQuery()

        If normalComplementaria.Text = "COMPLEMENTARIA" Then
            q = "UPDATE ideMens SET fechaPresentacionAnt='" + Convert.ToDateTime(fechaPresentacionAnt.Text).ToString("yyyy-MM-dd") + "', numOperAnt='" + numOperAnt.Text + "', normalComplementaria='COMPLEMENTARIA' WHERE id=" + id.Text
            myCommand3 = New SqlCommand(q, myConnection)
            myCommand3.ExecuteNonQuery()
        End If
        'ClientScript.RegisterStartupScript(Me.GetType(), "Load", "<script type='text/javascript'>window.parent.location.href = 'decla.aspx'; </script>")
    End Sub



    Protected Sub verXml_Click(ByVal sender As Object, ByVal e As EventArgs) Handles verXml.Click
        MultiView1.ActiveViewIndex = Int32.Parse(5)

        If File.Exists("C:\inetpub\wwwroot\xmlSubidos\" + Session("curCorreo") + "." + ejercicio.ToString + "." + mes.ToString + ".xml.ZIP") Then
            File.Delete("C:\inetpub\wwwroot\xmlSubidos\" + Session("curCorreo") + "." + ejercicio.ToString + "." + mes.ToString + ".xml.ZIP")
        End If
        'bajar de la BD
        Dim myCommand2 As New SqlCommand
        With myCommand2
            .Connection = myConnection
            .CommandType = CommandType.StoredProcedure
            .CommandText = "ideMensBajaxml"
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
                    writeStream = New FileStream("C:\inetpub\wwwroot\xmlSubidos\" + Session("curCorreo") + "." + ejercicio.ToString + "." + mes.ToString + ".xml.ZIP", FileMode.Create)
                    Dim writeBinay As New BinaryWriter(writeStream)
                    writeBinay.Write(dr("archivoXML"))
                    writeBinay.Close()
                End If
            End If
        End With

        progressbarXml.Style("width") = "0px"
        statusImportXml.Text = ""
        descrip.Text = ""
    End Sub

    Protected Sub consultarXml_Click(ByVal sender As Object, ByVal e As EventArgs) Handles consultarXml.Click
        If estado.Text = "VACIA" Then
            Response.Write("<script language='javascript'>alert('La declaración esta vacía, pruebe a importarla primero');</script>")
            Exit Sub
        End If


        'descarga archivo, file download
        Dim filename As String = Session("curCorreo") + "." + ejercicio.ToString + "." + mes.ToString + ".xml.ZIP"
        Dim path As String = Server.MapPath("./xmlSubidos/" & filename)
        Dim file1 As System.IO.FileInfo = New System.IO.FileInfo(path)
        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" + file1.Name)
        Response.AddHeader("Content-Length", file1.Length.ToString())
        Response.ContentType = "application/octet-stream"
        Response.WriteFile(file1.FullName)
        Response.End()
    End Sub


    Protected Sub backXml_Click(ByVal sender As Object, ByVal e As EventArgs) Handles backXml.Click
        Call regresar()
    End Sub

    Protected Sub bajaAcuseXml_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bajaAcuseXml.Click
        Call bajarAcuse()
    End Sub

    Protected Sub fedNumOper_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles fedNumOper.TextChanged

    End Sub

    Protected Sub enteroPropInstit_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles enteroPropInstit.TextChanged

    End Sub

    Protected Sub enteroPropInstitRfc_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles enteroPropInstitRfc.TextChanged

    End Sub

    Protected Sub export_Click(sender As Object, e As EventArgs) Handles export.Click
        If GridView3.Rows.Count < 1 Then
            Response.Write("<script language='javascript'>alert('Nada que exportar');</script>")
            Exit Sub
        End If

        If (Not System.IO.Directory.Exists(Server.MapPath("~") + "exports")) Then
            System.IO.Directory.CreateDirectory(Server.MapPath("~") + "exports")
        End If
        Dim arch = Server.MapPath("~") + "exports/" + Session("curCorreo").ToString + ejercicio + mes + ".xlsx"

        If File.Exists(arch) Then
            File.Delete(arch)
        End If

        Dim oExcel As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application
        Dim oBook As Microsoft.Office.Interop.Excel.Workbook = oExcel.Workbooks.Add
        Dim oSheet As Microsoft.Office.Interop.Excel.Worksheet = oBook.Sheets(1)

        oSheet.Cells(2, 1).value = "Nombres"
        oSheet.Cells(2, 2).value = "Ap. Paterno"
        oSheet.Cells(2, 3).value = "Ap. Materno"
        oSheet.Cells(2, 4).value = "Razón social"
        oSheet.Cells(2, 5).value = "RFC"
        oSheet.Cells(2, 6).value = "Domicilio"
        oSheet.Cells(2, 7).value = "Tel1"
        oSheet.Cells(2, 8).value = "Tel2"
        oSheet.Cells(2, 9).value = "# Socio/cliente"
        oSheet.Cells(2, 10).value = "Depósitos"
        oSheet.Cells(2, 11).value = "Excedente"
        oSheet.Cells(2, 12).value = "Determinado"
        oSheet.Cells(2, 13).value = "Recaudado"
        oSheet.Cells(2, 14).value = "Pendiente recaudar"
        oSheet.Cells(2, 15).value = "Remanente"
        oSheet.Cells(2, 16).value = "Saldo pendiente recaudar"
        oSheet.Cells(2, 17).value = "Cheque caja monto"
        oSheet.Cells(2, 18).value = "Cheque caja recaudado"
        oSheet.Cells(2, 1).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 1).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 2).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 2).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 3).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 3).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 4).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 4).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 5).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 5).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 6).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 6).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 7).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 7).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 8).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 8).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 9).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 9).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 10).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 10).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 11).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 11).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 12).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 12).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 13).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 13).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 14).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 14).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 15).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 15).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 16).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 16).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 17).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 17).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 18).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 18).Font.Bold = True ' Fuente en negrita

        oSheet.Range("J:J").NumberFormat = "###,###,###,##0.00" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha
        oSheet.Range("K:K").NumberFormat = "###,###,###,##0.00" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha
        oSheet.Range("L:L").NumberFormat = "###,###,###,##0.00" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha
        oSheet.Range("M:M").NumberFormat = "###,###,###,##0.00" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha
        oSheet.Range("N:N").NumberFormat = "###,###,###,##0.00" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha
        oSheet.Range("O:O").NumberFormat = "###,###,###,##0.00" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha
        oSheet.Range("P:P").NumberFormat = "###,###,###,##0.00" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha
        oSheet.Range("Q:Q").NumberFormat = "###,###,###,##0.00" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha
        oSheet.Range("R:R").NumberFormat = "###,###,###,##0.00" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha

        Dim ren = 3
        For Each row As GridViewRow In GridView3.Rows
            oSheet.Cells(ren, 1).value = IIf(row.Cells(2).Text <> "&nbsp;", row.Cells(2).Text, "")
            oSheet.Cells(ren, 2).value = IIf(row.Cells(3).Text <> "&nbsp;", row.Cells(3).Text, "")
            oSheet.Cells(ren, 3).value = IIf(row.Cells(4).Text <> "&nbsp;", row.Cells(4).Text, "")
            oSheet.Cells(ren, 4).value = IIf(row.Cells(5).Text <> "&nbsp;", row.Cells(5).Text, "")
            oSheet.Cells(ren, 5).value = IIf(row.Cells(6).Text <> "&nbsp;", row.Cells(6).Text, "")
            oSheet.Cells(ren, 6).value = IIf(row.Cells(7).Text <> "&nbsp;", row.Cells(7).Text, "")
            oSheet.Cells(ren, 7).value = IIf(row.Cells(8).Text <> "&nbsp;", row.Cells(8).Text, "")
            oSheet.Cells(ren, 8).value = IIf(row.Cells(9).Text <> "&nbsp;", row.Cells(9).Text, "")
            oSheet.Cells(ren, 9).value = IIf(row.Cells(10).Text <> "&nbsp;", row.Cells(10).Text, "")
            oSheet.Cells(ren, 10).value = IIf(row.Cells(11).Text <> "&nbsp;", row.Cells(11).Text, "")
            oSheet.Cells(ren, 11).value = IIf(row.Cells(12).Text <> "&nbsp;", row.Cells(12).Text, "")
            oSheet.Cells(ren, 12).value = IIf(row.Cells(13).Text <> "&nbsp;", row.Cells(13).Text, "")
            oSheet.Cells(ren, 13).value = IIf(row.Cells(14).Text <> "&nbsp;", row.Cells(14).Text, "")
            oSheet.Cells(ren, 14).value = IIf(row.Cells(15).Text <> "&nbsp;", row.Cells(15).Text, "")
            oSheet.Cells(ren, 15).value = IIf(row.Cells(16).Text <> "&nbsp;", row.Cells(16).Text, "")
            oSheet.Cells(ren, 16).value = IIf(row.Cells(17).Text <> "&nbsp;", row.Cells(17).Text, "")
            oSheet.Cells(ren, 17).value = IIf(row.Cells(18).Text <> "&nbsp;", row.Cells(18).Text, "")
            oSheet.Cells(ren, 18).value = IIf(row.Cells(19).Text <> "&nbsp;", row.Cells(19).Text, "")
            ren = ren + 1
        Next

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
        oSheet.Columns("O:O").EntireColumn.AutoFit()
        oSheet.Columns("P:P").EntireColumn.AutoFit()
        oSheet.Columns("Q:Q").EntireColumn.AutoFit()
        oSheet.Columns("R:R").EntireColumn.AutoFit()


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
        Response.AddHeader("content-disposition", "attachment; filename=" + Session("curCorreo").ToString + ejercicio + mes + ".xlsx")
        Response.ContentType = "application/vnd.ms-excel"
        Response.WriteFile(arch)
        Response.End()

        File.Delete(arch)

        Dim MSG As String = "<script language='javascript'>alert('Descargo exitoso hacia su equipo, revise su carpeta de descargas');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
    End Sub

End Class