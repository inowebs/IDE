Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports Microsoft.Office.Interop.Excel
Imports Microsoft.Office.Interop.Word
Imports System.IO
Imports Ionic.Zip
Imports System.Xml
Imports System.Security.AccessControl
Imports Microsoft.Office.Interop.Access
Imports System.Xml.Schema
Imports FastReport
Imports FastReport.Web
Imports System.Threading
Imports System.Security

Public Class mensual22
    Inherits System.Web.UI.Page

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

    Sub AddFileSecurity(ByVal fileName As String, ByVal account As String,
            ByVal rights As FileSystemRights, ByVal controlType As AccessControlType)

        Dim fSecurity As FileSecurity = File.GetAccessControl(fileName)
        Dim accessRule As FileSystemAccessRule = New FileSystemAccessRule(account, rights, controlType)
        fSecurity.AddAccessRule(accessRule)
        File.SetAccessControl(fileName, fSecurity)
    End Sub

    Private Sub habilitacionTotales(ByVal valor)
        impteExcedente.Enabled = valor
        sumaDepEfe.Enabled = valor
        montoChqCaja.Enabled = valor
        ntit2.Enabled = valor
        nChq2.Enabled = valor
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim v
        If IsNothing(Session("curCorreo")) = True Then
            Response.Write("<script language='javascript'>alert('Expiró su sesión');</script>")
            Session.Abandon()
            Response.Write("<script language='javascript'>location.href='Login.aspx';</script>")
            Exit Sub
        End If

        'myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';MultipleActiveResultSets=True")
        'myConnection.Open()
        myCommand = New SqlCommand("set dateformat ymd")
        ExecuteNonQueryFunction(myCommand)



        If Not IsPostBack Then  '1a vez    
            fDescargada.Enabled = False
            validada.Enabled = False
            chkPostpago.Enabled = False
            crearDecla.Enabled = True
            If Left(Request.ServerVariables("REMOTE_ADDR"), 10) = "192.168.0." Or Left(Request.ServerVariables("REMOTE_ADDR"), 10) = "127.0.0.1" Or Session("runAsAdmin") = "1" Or HttpContext.Current.Request.IsLocal Then 'red local
                'chkSinCorreo.Visible = True
            Else
                myCommand = New SqlCommand("select formaPresentacion from clientes where id=" + Session("GidCliente").ToString)
                v = ExecuteScalarFunction(myCommand)
                If IsNothing(v) Then
                    crearDecla.Enabled = False
                Else
                    If v = "Por Cliente" Then
                        crearDecla.Enabled = True
                    Else
                        crearDecla.Enabled = False
                    End If
                End If

                If Session("curCorreo").ToString.ToUpper = "PRUEBASDEIDE@GMAIL.COM" Then
                    crearDecla.Enabled = False
                End If
                'chkSinCorreo.Visible = False
            End If
        End If

        ejercicio = Request.QueryString("ejercicio")
        idContrato = Request.QueryString("contra")
        pl = Request.QueryString("pl")
        mes = Request.QueryString("mes")

        Dim q
        q = "SELECT postpago FROM contratos WHERE id=" + idContrato.ToString
        myCommand = New SqlCommand(q)
        v = ExecuteScalarFunction(myCommand)
        If (v.Equals(True)) Then '
            chkPostpago.Checked = True
        Else
            chkPostpago.Checked = False
        End If

        If Not IsPostBack Then  '1a vez    
            q = "SELECT formaPresentacion FROM clientes WHERE correo='" + Session("curCorreo") + "'"
            myCommand = New SqlCommand(q)
            v = ExecuteScalarFunction(myCommand)
            If Not IsNothing(v) Then
                If (v = "Conexion remota") Then '
                    lblCita.Visible = True
                    fCita.Visible = True
                    hrs.Visible = True
                    mins.Visible = True
                    cita.Visible = True
                Else
                    lblCita.Visible = False
                    fCita.Visible = False
                    hrs.Visible = False
                    mins.Visible = False
                    cita.Visible = False
                End If
                tipoEnvio.SelectedValue = v
            Else
                Response.Write("<script language='javascript'>alert('Indique la forma de presentar la declaracion en el menu cuenta');</script>")
                Response.Write("<script language='javascript'>location.href='clientes.aspx';</script>")
                Exit Sub
            End If
        End If

        'Page.ClientScript.RegisterStartupScript(GetType(Microsoft.Office.Interop.Excel.Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + divScroll.ClientID + "');", True)

        'btnEnviarDeclaracion.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(btnEnviarDeclaracion, "") + ";")        
        importMensXls.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(importMensXls, "") + ";")
        crearDecla.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(crearDecla, "") + ";")
        export.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(export, "") + ";")
        'bajarAcuseExcel.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(bajarAcuseExcel, "") + ";")
        'setStatusDecla.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(setStatusDecla, "") + ";")
        'setFechas.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(setFechas, "") + ";")
        'btnMod.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(btnMod, "") + ";")
        'descargaLocal.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(descargaLocal, "") + ";")
        'descargarAcuse.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(descargarAcuse, "") + ";")
        'downCorr.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(downCorr, "") + ";")
        'acuseSet.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(acuseSet, "") + ";")
        'saveTipoEnvio.Attributes.Add("onclick", " this.disabled = true;" + ClientScript.GetPostBackEventReference(saveTipoEnvio, "") + ";")


        If Not IsPostBack Then  '1a vez
            encab.Text = "Declaración Mensual: Ejercicio " + ejercicio + ", Mes " + mes

            Select Case Request.QueryString("op")
                Case "0" 'crear/editar
                    If Request.QueryString("subop") = "0" Then  'xls
                        MultiView1.ActiveViewIndex = Int32.Parse(0)
                    ElseIf Request.QueryString("subop") = "1" Then  'xml
                        crearDecla.Visible = False 'se valida al importar
                        MultiView1.ActiveViewIndex = Int32.Parse(1)
                    Else 'edit
                        MultiView1.ActiveViewIndex = Int32.Parse(2)
                    End If
                    If pl = "CEROS" Then 'edit
                        habilitacionTotales(False)
                        crearDecla.Visible = False
                    End If
                Case "1" 'ceros 'creación
                    MultiView1.ActiveViewIndex = Int32.Parse(3)
                    habilitacionTotales(False)
                    Call limpiaMes()
                    crearDecla.Visible = False

                Case "2" 'consultar
                    If Request.QueryString("subop") = "0" Then  'xls
                        MultiView1.ActiveViewIndex = Int32.Parse(2)
                    Else 'xml
                        MultiView1.ActiveViewIndex = Int32.Parse(5)
                    End If
                    cargaLimpiaMes()
                    cargaGrid()
                    back.Visible = False
                    crearDecla.Visible = False
                    If pl = "CEROS" Then 'edit
                        habilitacionTotales(False)
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


                    crearDecla.Visible = False

            End Select

            cargaLimpiaMes()


        Else
            If Not Session("timer") Is Nothing Then
                If statusImport.Text = " Importación IDE realizada " Then
                    Timer1.Enabled = False
                End If
            End If

        End If

        Dim tipo
        'M=mensual
        Dim casfim, q2
        q2 = "SELECT casfim FROM clientes WHERE id=" + Session("GidCliente").ToString
        myCommand = New SqlCommand(q2)
        casfim = ExecuteScalarFunction(myCommand)
        Session("casfim") = casfim
    End Sub

    Private Sub cargaLimpiaMes()
        If Session("GidMens") <> 0 Then
            id.Text = Session("GidMens").ToString
        End If
        If Session("GidMens") <> 0 Then
            Dim q = "SELECT * FROM ideMens2 WHERE id=" + Session("GidMens").ToString
            myCommand2 = New SqlCommand(q)
            Using dr2 = ExecuteReaderFunction(myCommand2)
                If dr2.HasRows Then
                    dr2.Read()
                    Call cargaMes(dr2)
                Else
                    Call limpiaMes()
                End If
            End Using
        Else
            Call limpiaMes()
        End If
        'idAnual.Text = Session("GidAnual").ToString
        id.Text = Session("GidMens").ToString

        'progressbar1.Style("width") = "0px"
        'statusImport.Text = ""
    End Sub
    Private Sub cargaGrid()

        TreeView1.Nodes.Clear()
        Dim q
        q = "SELECT d.*, c.*, d.id as idC FROM titular2 d, contrib2 c WHERE d.idContrib2=c.id AND idIdeMens2=" + id.Text + " order by d.id"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            While dr.Read()
                Dim tit As New TreeNode()
                tit.Text = "TIT" + ", " + dr("rfc") + ", " + dr("apPat").ToString + ", " + dr("apMat").ToString + ", " + dr("nom").ToString + ", " + dr("razonSocial").ToString + ", " + dr("nContratoCta").ToString + ", " + dr("curp") + ", " + dr("numIdFis") + ", " + getNomEntFed(dr("idEntFed")) + ", " + dr("calle") + ", " + dr("nExt") + ", " + dr("nInt") + ", " + dr("cp") + ", " + dr("col") + ", " + dr("loc") + ", " + dr("correo") + ", " + dr("tel1").ToString + ", " + dr("tel2").ToString + ", " + FormatCurrency(dr("sumaDepoEfe"), 0) + ", " + FormatCurrency(dr("excedente"), 0) + ", " + dr("moneda").ToString + ", " + FormatNumber(dr("tipoCamb"), 2) + ", " + FormatNumber(dr("porcProp"), 4)
                TreeView1.Nodes.Add(tit)

                Dim q2
                q2 = "SELECT cot.*, con.* FROM cotit2 cot, contrib2 con WHERE cot.idContrib2=con.id and idTitular2=" + dr("idC").ToString + " order by cot.id"
                myCommand2 = New SqlCommand(q2)
                Using dr2 = ExecuteReaderFunction(myCommand2)
                    While dr2.Read()
                        Dim cot As New TreeNode()
                        cot.Text = "COT" + ", " + dr2("rfc") + ", " + dr2("apPat").ToString + ", " + dr2("apMat").ToString + ", " + dr2("nom").ToString + ", " + dr2("razonSocial").ToString + ", " + dr2("curp") + ", " + dr2("numIdFis") + ", " + getNomEntFed(dr2("idEntFed")) + ", " + dr2("calle") + ", " + dr2("nExt") + ", " + dr2("nInt") + ", " + dr2("cp") + ", " + dr2("col") + ", " + dr2("loc") + ", " + dr2("correo") + ", " + dr2("tel1").ToString + ", " + dr2("tel2").ToString + ", " + FormatNumber(dr2("porcProp"), 4)
                        tit.ChildNodes.Add(cot)
                    End While
                End Using
            End While
        End Using

        q = "SELECT d.*, c.* FROM chq2 d, contrib2 c WHERE d.idContrib2=c.id AND idIdeMens2=" + id.Text + " order by d.id"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            While dr.Read()
                Dim chq As New TreeNode()
                chq.Text = "CHQ" + ", " + dr("rfc") + ", " + dr("apPat").ToString + ", " + dr("apMat").ToString + ", " + dr("nom").ToString + ", " + dr("razonSocial").ToString + ", " + dr("curp") + ", " + dr("numIdFis") + ", " + getNomEntFed(dr("idEntFed")) + ", " + dr("calle") + ", " + dr("nExt") + ", " + dr("nInt") + ", " + dr("cp") + ", " + dr("col") + ", " + dr("loc") + ", " + dr("correo") + ", " + dr("tel1").ToString + ", " + dr("tel2").ToString + ", " + dr("moneda").ToString + ", " + FormatNumber(dr("tipoCamb"), 2) + ", " + FormatCurrency(dr("montoChqCajaMens"), 0)
                TreeView1.Nodes.Add(chq)
            End While
        End Using

    End Sub

    Private Sub limpiaMes()
        Dim q
        chkAcuse.Checked = 0
        impteExcedente.Text = 0
        sumaDepEfe.Text = 0
        montoChqCaja.Text = 0
        ntit2.Text = 0
        nChq2.Text = 0
        fPresentada.Text = ""
        fCita.Text = ""
        fDescargada.Text = ""
        hrs.SelectedIndex = 0
        mins.SelectedIndex = 0
        tipoEnvio.SelectedIndex = 0
        validada.Checked = 0

        'idIdeConf.Text = dr2("idIdeConf")
        If ejercicio.ToString = "2008" Or ejercicio.ToString = "2009" Then
            myCommand3 = New SqlCommand("SELECT * FROM ideConf WHERE limite=25000.00 and porcen=2.00")
        Else
            myCommand3 = New SqlCommand("SELECT * FROM actuales")
        End If
        Using dr4 = ExecuteReaderFunction(myCommand3)
            dr4.Read()
            If ejercicio.ToString = "2008" Or ejercicio.ToString = "2009" Then
                ideConfLimite.Text = FormatNumber(dr4("limite"), 2)
            Else
                ideConfLimite.Text = FormatNumber(dr4("ideLim"), 2)
            End If
        End Using
        id.Text = 0
        estado.SelectedIndex = 0
    End Sub

    Private Sub cargaMes(ByVal dr2)
        If DBNull.Value.Equals(dr2("excedente")) Then
            impteExcedente.Text = 0
        Else
            impteExcedente.Text = CDbl(dr2("excedente")).ToString("###,###,###,##0")
        End If
        If DBNull.Value.Equals(dr2("sumaDepoEfe")) Then
            sumaDepEfe.Text = 0
        Else
            sumaDepEfe.Text = CDbl(dr2("sumaDepoEfe")).ToString("###,###,###,##0")
        End If
        If DBNull.Value.Equals(dr2("montoChqCaja")) Then
            montoChqCaja.Text = 0
        Else
            montoChqCaja.Text = CDbl(dr2("montoChqCaja")).ToString("###,###,###,##0")
        End If
        If DBNull.Value.Equals(dr2("nTit2")) Then
            ntit2.Text = 0
        Else
            ntit2.Text = CDbl(dr2("nTit2")).ToString("###,###,###,##0")
        End If
        If DBNull.Value.Equals(dr2("nChq2")) Then
            nChq2.Text = 0
        Else
            nChq2.Text = CDbl(dr2("nChq2")).ToString("###,###,###,##0")
        End If
        If DBNull.Value.Equals(dr2("fPresentada")) Then
            fPresentada.Text = ""
        Else
            fPresentada.Text = dr2("fPresentada")
        End If
        If DBNull.Value.Equals(dr2("fDescargada")) Then
            fDescargada.Text = ""
        Else
            fDescargada.Text = dr2("fDescargada")
        End If
        If DBNull.Value.Equals(dr2("fCita")) Then
            fCita.Text = ""
        Else
            fCita.Text = dr2("fCita")
        End If
        If DBNull.Value.Equals(dr2("hrCita")) Then
            hrs.SelectedIndex = 0
        Else
            hrs.SelectedValue = dr2("hrCita")
        End If
        If DBNull.Value.Equals(dr2("minCita")) Then
            mins.SelectedIndex = 0
        Else
            mins.SelectedValue = dr2("minCita")
        End If
        If DBNull.Value.Equals(dr2("validada")) Then
            validada.Checked = 0
        Else
            validada.Checked = dr2("validada")
        End If
        Dim dr3 As SqlDataReader
        Dim q
        estado.SelectedValue = dr2("idEstatusDecla")
        chkAcuse.Checked = dr2("tieneAcuse")
        tipoEnvio.SelectedValue = dr2("metodoPresentac")

        If ejercicio.ToString = "2008" Or ejercicio.ToString = "2009" Then
            myCommand3 = New SqlCommand("SELECT * FROM ideConf WHERE limite=25000.00 and porcen=2.00")
        Else
            myCommand3 = New SqlCommand("SELECT * FROM actuales")
        End If
        Using dr4 = ExecuteReaderFunction(myCommand3)
            dr4.Read()
            If ejercicio.ToString = "2008" Or ejercicio.ToString = "2009" Then
                ideConfLimite.Text = FormatNumber(dr4("limite"), 2)
            Else
                ideConfLimite.Text = FormatNumber(dr4("ideLim"), 2)
            End If
        End Using
        id.Text = dr2("id")

        'cargaGrid()
    End Sub

    Private Sub insertaMensualVacia()
        Dim q, idIdeConf

        Dim dr2 As SqlDataReader
        q = "SELECT id FROM ideConf WHERE limite='" + ideConfLimite.Text + "'"
        myCommand2 = New SqlCommand(q)
        idIdeConf = ExecuteScalarFunction(myCommand2)
        Dim idEstatus = returnID("estatusDecla2", "pendiente recibir archivos")

        q = "INSERT INTO ideMens2(ejercicio,mes,sumaDepoEfe,excedente,montoChqCaja,idIdeConf,idEstatusDecla,validada,idContrato,ntit2,nchq2,idCliente,tieneAcuse,metodoPresentac) VALUES(" + ejercicio + ",'" + mes.ToString + "',0,0,0," + idIdeConf.ToString + "," + idEstatus.ToString + ",0," + idContrato.ToString + ",0,0," + Session("GidCliente").ToString + ",0,'Contribuyente')"
        myCommand3 = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand3)

        q = "SELECT TOP 1 id FROM ideMens2 WHERE mes='" + mes + "' and ejercicio=" + ejercicio + " and idCliente=" + Session("GidCliente").ToString + " order by id desc"
        myCommand2 = New SqlCommand(q)
        id.Text = ExecuteScalarFunction(myCommand2)
        Session("GidMens") = id.Text
    End Sub

    Protected Sub importMensXls_Click(ByVal sender As Object, ByVal e As EventArgs) Handles importMensXls.Click
        Dim v
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


        importMensXls.Enabled = False
        progressbar1.Style("width") = "0px"
        statusImport.Text = ""
        lbldescrip.Text = ""

        myCommand2 = New SqlCommand("SELECT TOP 1 id FROM ideMens2 WHERE mes='" + mes + "' and ejercicio='" + ejercicio + "' AND idCliente In (SELECT id FROM clientes WHERE correo='" + Session("curCorreo").ToString.ToUpper + "') order by id desc")
        v = ExecuteScalarFunction(myCommand2)
        If IsNothing(v) Then
            Call insertaMensualVacia()
        End If

        Dim casfim
        Dim q = "SELECT casfim FROM clientes WHERE id=" + Session("GidCliente").ToString
        myCommand = New SqlCommand(q)
        casfim = ExecuteScalarFunction(myCommand)
        Dim tipo

        savePath = "C:\SAT\" + casfim + "\" 'pend: en su casfim
        savePath += casfim + "-M-" + ejercicio + "-" + mes + extension
        h1.Value = savePath
        If File.Exists(savePath) Then
            File.Delete(savePath)
        End If
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

        'AddFileSecurity(savePath, Session("identidad"), FileSystemRights.FullControl, AccessControlType.Allow)

        If validaDecla() = 0 Then
            importMensXls.Enabled = True
            Exit Sub
        End If

        Session("error") = ""
        Session("barraN") = 1
        Session("barraIteracion") = 0

        'progressbar1.Style("width") = "0px"
        'lblAvance.Text = ""
        'statusImport.Text = ""        

        Dim objThread As New Thread(New System.Threading.ThreadStart(AddressOf DoTheWork))
        objThread.IsBackground = True
        objThread.Start()
        Session("Thread") = objThread

        Timer1.Enabled = True
    End Sub


    Protected Sub DoTheWork()
        Dim p = importarIDEmens2()

        importMensXls.Enabled = True
    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'lblAvance.Text = DateTime.Now.ToString
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
            Else
                statusImport.Text = " Importación IDE realizada "
                Call refrescaTotalesMens()
                estado.SelectedValue = returnID("estatusDecla2", "validada")
            End If
            'File.Delete(h1.Value) 'el de excel
            importMensXls.Enabled = True
        End If
    End Sub

    Private Sub creaTagsMens()
        creaArch()
    End Sub
    Private Sub creaArch()
        Dim idArch, totExedente, totCheque, totOpers
        Dim q

        If (Not System.IO.Directory.Exists("C:\SAT\" + Session("casfim"))) Then
            System.IO.Directory.CreateDirectory("C:\SAT\" + Session("casfim"))
        End If

        nomArchAnualDatos.Value = "C:\SAT\" + Session("casfim") + "\IDE_" + Session("curCorreo") + "_" + ejercicio.ToString + "_" + mes.ToString + ".txt"
        If File.Exists(nomArchAnualDatos.Value) Then
            File.Delete(nomArchAnualDatos.Value)
        End If

        Dim utf8WithoutBom As New System.Text.UTF8Encoding(False)
        Dim archivo As StreamWriter = New System.IO.StreamWriter(nomArchAnualDatos.Value, False, utf8WithoutBom)
        Dim entFedClave

        q = "SELECT d.*, c.*, d.id as idC FROM titular2 d, contrib2 c WHERE d.idContrib2=c.id AND idIdeMens2=" + id.Text + " order by d.id"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            While dr.Read()
                entFedClave = getClaveEntFed(dr("idEntFed")).ToString
                archivo.WriteLine("1|" + dr("rfc") + "|" + IIf(DBNull.Value.Equals(dr("apPat")), "", dr("apPat")) + "|" + IIf(DBNull.Value.Equals(dr("apMat")), "", dr("apMat")) + "|" + IIf(DBNull.Value.Equals(dr("nom")), "", dr("nom")) + "|" + IIf(DBNull.Value.Equals(dr("razonSocial")), "", dr("razonSocial")) + "|" + dr("nContratoCta") + "|" + IIf(DBNull.Value.Equals(dr("curp")), "", dr("curp")) + "|" + IIf(DBNull.Value.Equals(dr("numIdFis")), "", dr("numIdFis")) + "|" + entFedClave + "|" + dr("calle") + "|" + dr("nExt") + "|" + IIf(DBNull.Value.Equals(dr("nInt")), "", dr("nInt")) + "|" + dr("cp") + "|" + dr("col") + "|" + dr("loc") + "|" + dr("correo") + "|" + dr("tel1") + "|" + IIf(DBNull.Value.Equals(dr("tel2")), "", dr("tel2")) + "|" + (Math.Truncate(100 * dr("sumaDepoEfe").ToString.Trim.Replace(",", "")) / 100).ToString + "|" + (Math.Truncate(100 * dr("excedente").ToString.Trim.Replace(",", "")) / 100).ToString + "|" + dr("moneda") + "|" + IIf(DBNull.Value.Equals(dr("tipoCamb")), "", IIf(dr("tipoCamb") = 0, "", Math.Round(dr("tipoCamb"), 2).ToString)) + "|" + IIf(DBNull.Value.Equals(dr("porcProp")), "", IIf(dr("porcProp") = 1, "", dr("porcProp").ToString)))

                Dim q2
                q2 = "SELECT cot.*, con.* FROM cotit2 cot, contrib2 con WHERE idTitular2=" + dr("idC").ToString + " AND cot.idContrib2=con.id  order by cot.id"
                myCommand2 = New SqlCommand(q2)
                Using dr2 = ExecuteReaderFunction(myCommand2)
                    While dr2.Read()
                        entFedClave = getClaveEntFed(dr2("idEntFed")).ToString
                        archivo.WriteLine("2|" + dr2("rfc") + "|" + IIf(DBNull.Value.Equals(dr2("apPat")), "", dr2("apPat")) + "|" + IIf(DBNull.Value.Equals(dr2("apMat")), "", dr2("apMat")) + "|" + IIf(DBNull.Value.Equals(dr2("nom")), "", dr2("nom")) + "|" + IIf(DBNull.Value.Equals(dr2("razonSocial")), "", dr2("razonSocial")) + "|" + IIf(DBNull.Value.Equals(dr2("curp")), "", dr2("curp")) + "|" + IIf(DBNull.Value.Equals(dr2("numIdFis")), "", dr2("numIdFis")) + "|" + entFedClave + "|" + dr2("calle") + "|" + dr2("nExt") + "|" + IIf(DBNull.Value.Equals(dr2("nInt")), "", dr2("nInt")) + "|" + dr2("cp") + "|" + dr2("col") + "|" + dr2("loc") + "|" + dr2("correo") + "|" + dr2("tel1") + "|" + IIf(DBNull.Value.Equals(dr2("tel2")), "", dr2("tel2")) + "|" + IIf(DBNull.Value.Equals(dr2("porcProp")), "", IIf(dr2("porcProp") = 1, "", dr2("porcProp").ToString)))
                    End While
                End Using
            End While
        End Using

        q = "SELECT d.*, c.* FROM chq2 d, contrib2 c WHERE d.idContrib2=c.id AND idIdeMens2=" + id.Text + " order by d.id"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            While dr.Read()
                entFedClave = getClaveEntFed(dr("idEntFed")).ToString
                archivo.WriteLine("3|" + dr("rfc") + "|" + IIf(DBNull.Value.Equals(dr("apPat")), "", dr("apPat")) + "|" + IIf(DBNull.Value.Equals(dr("apMat")), "", dr("apMat")) + "|" + IIf(DBNull.Value.Equals(dr("nom")), "", dr("nom")) + "|" + IIf(DBNull.Value.Equals(dr("razonSocial")), "", dr("razonSocial")) + "|" + IIf(DBNull.Value.Equals(dr("curp")), "", dr("curp")) + "|" + IIf(DBNull.Value.Equals(dr("numIdFis")), "", dr("numIdFis")) + "|" + entFedClave + "|" + dr("calle") + "|" + dr("nExt") + "|" + IIf(DBNull.Value.Equals(dr("nInt")), "", dr("nInt")) + "|" + dr("cp") + "|" + dr("col") + "|" + dr("loc") + "|" + dr("correo") + "|" + dr("tel1") + "|" + IIf(DBNull.Value.Equals(dr("tel2")), "", dr("tel2")) + "|" + (Math.Truncate(100 * dr("montoChqCajaMens").ToString.Trim.Replace(",", "")) / 100).ToString + "|" + dr("moneda") + "|" + IIf(DBNull.Value.Equals(dr("tipoCamb")), "", IIf(dr("tipoCamb") = 0, "", Math.Round(dr("tipoCamb"), 2).ToString)))
            End While
        End Using

        archivo.Close()
    End Sub

    Private Function creaTxtMes() As String
        Call creaTagsMens()
        'If validacion() = False Then   'de momento no hay xml para validar ni programa externo
        '    Return "Errores al validar archivo"
        'End If

        'Call subeXMLmensBD()
        Return ""
    End Function

    Private Function validaSecuencia(ByVal descrip, ByVal descripAnt, ByVal ren, ByVal valCol3RenAnt, ByRef msgErr) As Integer
        If descripAnt = "" And descrip <> "TIT" Then
            msgErr = msgErr + ". " + "En el renglón 5 debe indicar TIT en la columna tipo"
            Return 0
        End If

        If descrip = "COT" Then
            If descripAnt <> "TIT" And descripAnt <> "COT" Then
                msgErr = msgErr + ". " + "Un tipo COT solo puede ser precedido por un COT o un TIT, verifique en el renglón " + ren.ToString
                Return 0
            End If
        End If

        Return 1
    End Function


    Private Function importarIDEmens2() As Integer
        Dim objThread As Thread = CType(Session("Thread"), Thread)
        Try
            Dim excel As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application
            excel.DisplayAlerts = False
            Dim w As Workbook = excel.Workbooks.Open(savePath)
            'For i As Integer = 1 To w.Sheets.Count
            Dim sheet As Worksheet = w.Sheets(1) 'i     'abrirá la 1er hoja del libro
            'xlHoja = xlApp.Worksheets(CStr(DatePart("m", mes.Value))) ' hojas: 1:12

            Dim r As Microsoft.Office.Interop.Excel.Range = sheet.UsedRange
            Dim array(,) As Object = r.Value(XlRangeValueDataType.xlRangeValueDefault)
            Dim nRensPre = sheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row 'sin rens en bco

            Session("barraN") = nRensPre

            w.Close(False)
            excel.Quit()
            w = Nothing
            excel = Nothing

            If array IsNot Nothing Then
                Dim idCliente

                Dim descrip, nom, apPat, apMat, razon, rfc, curp, numIdFis, entFed, idEntFed, calle, nExterior, nInt, cp, col, loc, correo, tel1, tel2, idContrib2, nContratoCta, sumaDepoEfe, exedente, moneda, tipoCamb, porcPropTit, porcPropCot, idTitular2, montoChqCajaMens, idIdeMens2
                Dim q, descripAnt, ColCAnt

                descripAnt = ""
                lblErrImport.Visible = False
                errImport.Visible = False

                'borra los registros del detalle, importante el orden de eliminacion
                q = "DELETE FROM cotit2 WHERE idTitular2 IN (SELECT id FROM titular2 WHERE idIdeMens2=" + id.Text + ")"
                myCommand = New SqlCommand(q)
                ExecuteNonQueryFunction(myCommand)
                q = "DELETE FROM chq2 WHERE idIdeMens2=" + id.Text
                myCommand = New SqlCommand(q)
                ExecuteNonQueryFunction(myCommand)
                q = "DELETE FROM titular2 WHERE idIdeMens2=" + id.Text
                myCommand = New SqlCommand(q)
                ExecuteNonQueryFunction(myCommand)
                descripAnt = ""

                q = "SELECT id FROM clientes WHERE correo='" + Session("curCorreo") + "'"
                myCommand = New SqlCommand(q)
                idCliente = ExecuteScalarFunction(myCommand)

                ColCAnt = array(4, 3)

                For i As Integer = 5 To nRensPre '1-4rens=encab 5o=datos
                    If IsNothing(array(i, 1)) And IsNothing(array(i, 2)) And IsNothing(array(i, 3)) And IsNothing(array(i, 4)) And IsNothing(array(i, 5)) And IsNothing(array(i, 6)) And IsNothing(array(i, 7)) And IsNothing(array(i, 8)) And IsNothing(array(i, 9)) And IsNothing(array(i, 10)) And IsNothing(array(i, 11)) And IsNothing(array(i, 12)) And IsNothing(array(i, 13)) And IsNothing(array(i, 14)) And IsNothing(array(i, 15)) And IsNothing(array(i, 16)) And IsNothing(array(i, 17)) And IsNothing(array(i, 18)) And IsNothing(array(i, 19)) And IsNothing(array(i, 20)) And IsNothing(array(i, 21)) And IsNothing(array(i, 22)) And IsNothing(array(i, 23)) And IsNothing(array(i, 24)) And IsNothing(array(i, 25)) Then ' ren bco
                        GoTo siguiente2
                    End If

                    If Not IsNothing(array(i, 1)) Then
                        descrip = Trim(UCase(array(i, 1)))
                    Else
                        descrip = ""
                    End If
                    descripAnt = descrip

                    If Not IsNothing(array(i, 2)) Then
                        rfc = array(i, 2).ToString.ToUpper.Trim.Replace("-", "").Replace(" ", "")
                    Else
                        rfc = ""
                    End If
                    If Not IsNothing(array(i, 3)) Then
                        apPat = Left(array(i, 3).ToString.ToUpper.Trim, 180).Replace("'", "''")
                    Else
                        apPat = ""
                    End If
                    If Not IsNothing(array(i, 4)) Then
                        apMat = Left(array(i, 4).ToString.ToUpper.Trim, 180).Replace("'", "''")
                    Else
                        apMat = ""
                    End If
                    If Not IsNothing(array(i, 5)) Then
                        nom = Left(array(i, 5).ToString.ToUpper.Trim, 180).Replace("'", "''")
                    Else
                        nom = ""
                    End If
                    If Not IsNothing(array(i, 6)) Then
                        razon = Left(array(i, 6).ToString.ToUpper.Trim, 500).Replace("'", "''").Replace("&", "&amp;")
                    Else
                        razon = ""
                    End If
                    If Not IsNothing(array(i, 8)) Then
                        curp = Left(array(i, 8).ToString.ToUpper.Trim, 18)
                    Else
                        curp = ""
                    End If
                    If Not IsNothing(array(i, 9)) Then
                        numIdFis = Left(array(i, 9).ToString.ToUpper.Trim, 30)
                    Else
                        numIdFis = ""
                    End If
                    If Not IsNothing(array(i, 10)) Then
                        entFed = Left(array(i, 10).ToString.Trim, 40)
                        q = "SELECT id FROM entfed2 where descr='" + entFed + "'"
                        myCommand = New SqlCommand(q)
                        Dim x = ExecuteScalarFunction(myCommand)
                        If Not IsNothing(x) Then
                            idEntFed = x.ToString
                        Else
                            idEntFed = ""
                        End If
                    Else
                        entFed = ""
                        idEntFed = ""
                    End If
                    If Not IsNothing(array(i, 11)) Then
                        calle = Left(array(i, 11).ToString.ToUpper.Trim, 200)
                    Else
                        calle = ""
                    End If
                    If Not IsNothing(array(i, 12)) Then
                        nExterior = Left(array(i, 12).ToString.ToUpper.Trim, 20)
                    Else
                        nExterior = ""
                    End If
                    If Not IsNothing(array(i, 13)) Then
                        nInt = Left(array(i, 13).ToString.ToUpper.Trim, 20)
                    Else
                        nInt = ""
                    End If
                    If Not IsNothing(array(i, 14)) Then
                        cp = Left(array(i, 14).ToString.ToUpper.Trim, 5)
                    Else
                        cp = ""
                    End If
                    If Not IsNothing(array(i, 15)) Then
                        col = Left(array(i, 15).ToString.ToUpper.Trim, 200)
                    Else
                        col = ""
                    End If
                    If Not IsNothing(array(i, 16)) Then
                        loc = Left(array(i, 16).ToString.ToUpper.Trim, 200)
                    Else
                        loc = ""
                    End If
                    If Not IsNothing(array(i, 17)) Then
                        correo = Left(array(i, 17).ToString.ToUpper.Trim, 200)
                    Else
                        correo = ""
                    End If
                    If Not IsNothing(array(i, 18)) Then
                        tel1 = Regex.Replace(Left(array(i, 18).ToString.ToUpper.Trim.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace("'", ""), 10), "[a-zA-Z\s]+", "")
                    Else
                        tel1 = ""
                    End If
                    If Not IsNothing(array(i, 19)) Then
                        tel2 = Regex.Replace(Left(array(i, 19).ToString.ToUpper.Trim.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace("'", ""), 10), "[a-zA-Z\s]+", "")
                    Else
                        tel2 = ""
                    End If

                    If descrip = "TIT" Then 'titular
                        If Not IsNothing(array(i, 7)) Then
                            nContratoCta = Left(array(i, 7).ToString.ToUpper.Trim, 30)
                        Else
                            nContratoCta = ""
                        End If

                        If Not IsNothing(array(i, 20)) Then
                            sumaDepoEfe = array(i, 20).ToString
                        Else
                            sumaDepoEfe = ""
                        End If
                        If Not IsNothing(array(i, 21)) Then
                            exedente = array(i, 21).ToString
                        Else
                            exedente = ""
                        End If
                        If Not IsNothing(array(i, 22)) Then
                            moneda = Left(array(i, 22).ToString.ToUpper.Trim, 3)
                        Else
                            moneda = ""
                        End If
                        If Not IsNothing(array(i, 23)) Then
                            tipoCamb = array(i, 23).ToString
                        Else
                            tipoCamb = "0"
                        End If
                        If Not IsNothing(array(i, 24)) Then
                            porcPropTit = array(i, 24).ToString
                        Else
                            porcPropTit = "1"
                        End If

                        'ren vacio
                        If nom = "" And apPat = "" And apMat = "" And razon = "" And rfc = "" And curp = "" And numIdFis = "" And entFed = "" And calle = "" And nExterior = "" And nInt = "" And cp = "" And col = "" And loc = "" And correo = "" And tel1 = "" And tel2 = "" And nContratoCta = "" And exedente = "" And sumaDepoEfe = "" And moneda = "" And tipoCamb = "0" And porcPropTit = "1" Then
                            GoTo siguiente2
                        End If

                    ElseIf descrip = "CHQ" Then
                        If Not IsNothing(array(i, 22)) Then
                            moneda = Left(array(i, 22).ToString.ToUpper.Trim, 3)
                        Else
                            moneda = ""
                        End If
                        If Not IsNothing(array(i, 23)) Then
                            tipoCamb = array(i, 23).ToString
                        Else
                            tipoCamb = "0"
                        End If
                        If Not IsNothing(array(i, 25)) Then
                            If array(i, 25).ToString <> "-" Then
                                montoChqCajaMens = array(i, 25).ToString.Trim
                            Else
                                montoChqCajaMens = ""
                            End If
                        Else
                            montoChqCajaMens = ""
                        End If
                        'ren vacio
                        If nom = "" And apPat = "" And apMat = "" And razon = "" And rfc = "" And curp = "" And numIdFis = "" And entFed = "" And calle = "" And nExterior = "" And nInt = "" And cp = "" And col = "" And loc = "" And correo = "" And tel1 = "" And tel2 = "" And moneda = "" And tipoCamb = "0" Then
                            GoTo siguiente2
                        End If

                    ElseIf descrip = "COT" Then
                        If Not IsNothing(array(i, 24)) Then
                            porcPropCot = array(i, 24).ToString
                        Else
                            porcPropCot = "1"
                        End If
                        'ren vacio
                        If nom = "" And apPat = "" And apMat = "" And razon = "" And rfc = "" And curp = "" And numIdFis = "" And entFed = "" And calle = "" And nExterior = "" And nInt = "" And cp = "" And col = "" And loc = "" And correo = "" And tel1 = "" And tel2 = "" And porcPropCot = "1" Then
                            GoTo siguiente2
                        End If

                    End If

                    q = "SELECT id FROM contrib2 c WHERE ((c.nom='" + nom + "' AND c.apPat='" + apPat + "' AND c.apMat='" + apMat + "' and c.razonSocial='' and c.idCliente=" + idCliente.ToString + ") or (c.razonSocial='" + razon + "' and c.razonSocial<>''  and c.idCliente=" + idCliente.ToString + "))"
                    myCommand = New SqlCommand(q)
                    Dim v = ExecuteScalarFunction(myCommand)
                    If Not IsNothing(v) Then 'registro duplicado (llaves) en el archivo->reemplazarlo por el mas reciente
                        idContrib2 = v
                        q = "UPDATE contrib2 SET curp='" + curp + "',rfc='" + rfc + "',numIdFis='" + numIdFis + "',idEntFed='" + idEntFed + "',calle='" + calle + "',nExt='" + nExterior + "',nInt='" + nInt + "',cp='" + cp + "',col='" + col + "',loc='" + loc + "',correo='" + correo + "',tel1='" + tel1 + "',tel2='" + tel2 + "' WHERE id=" + idContrib2.ToString
                        myCommand2 = New SqlCommand(q)
                        ExecuteNonQueryFunction(myCommand2)
                    Else    'nuevo registro
                        myCommand2 = New SqlCommand("INSERT INTO contrib2(nom,apPat,apMat,numIdFis,razonSocial,rfc,curp,idEntFed,calle,nExt,nInt,cp,col,loc,correo,tel1,tel2,idCliente) 
VALUES('" + nom + "','" + apPat + "','" + apMat + "','" + numIdFis + "','" + razon + "','" + rfc + "','" + curp + "'," + idEntFed + ",'" + calle + "','" + nExterior + "','" + nInt + "','" + cp + "','" + col + "','" + loc + "','" + correo + "','" + tel1 + "','" + tel2 + "', " + idCliente.ToString + ")")
                        ExecuteNonQueryFunction(myCommand2)
                        q = "SELECT TOP 1 id FROM contrib2 where idCliente=" + idCliente.ToString + " ORDER BY id DESC"
                        myCommand = New SqlCommand(q)
                        idContrib2 = ExecuteScalarFunction(myCommand)
                    End If

                    If descrip = "TIT" Then
                        q = "INSERT INTO titular2(idContrib2,nContratoCta,sumaDepoEfe,excedente,moneda,tipocamb,porcProp,idIdeMens2) VALUES(" + idContrib2.ToString + ",'" + nContratoCta + "'," + sumaDepoEfe + "," + exedente + ",'" + moneda + "'," + tipoCamb + "," + porcPropTit + "," + id.Text + ")"
                        myCommand2 = New SqlCommand(q)
                        ExecuteNonQueryFunction(myCommand2)

                        q = "SELECT t.id FROM titular2 t, contrib2 c WHERE idIdeMens2=" + id.Text + " AND t.idContrib2=c.id AND c.id=" + idContrib2.ToString
                        myCommand = New SqlCommand(q)
                        idTitular2 = ExecuteScalarFunction(myCommand)

                        'validamos numContratoCta unico para el mismo contrib, mes
                        q = "SELECT count(*) as n FROM titular2 WHERE nContratoCta='" + nContratoCta + "' AND idIdeMens2=" + id.Text
                        myCommand = New SqlCommand(q)
                        Using dr = ExecuteReaderFunction(myCommand)
                            If dr.HasRows Then
                                dr.Read()
                                If dr("n") > 1 Then
                                    Session("error") = "El numero de contrato o cuenta (" + nContratoCta + ") aparece mas de 1 vez, en el renglon " + i.ToString + ", verifique"
                                    Response.Write("<script language='javascript'>alert('El numero de contrato o cuenta  (" + nContratoCta + ") aparece mas de 1 vez, en el renglon " + i.ToString + ", verifique');</script>")
                                    statusImport.Text = Session("error") + ". Se procesaron exitosamente los primeros " + Session("barraIteracion").ToString + " registros"
                                    objThread.Abort()
                                    Return 0
                                End If
                            End If
                        End Using

                    ElseIf descrip = "COT" Then
                        q = "INSERT INTO cotit2(idContrib2,porcProp,idTitular2) VALUES(" + idContrib2.ToString + "," + porcPropCot.ToString + "," + idTitular2.ToString + ")"
                        myCommand2 = New SqlCommand(q)
                        ExecuteNonQueryFunction(myCommand2)
                    ElseIf descrip = "CHQ" Then
                        q = "INSERT INTO chq2(idContrib2,moneda,tipocamb,montoChqCajaMens,idIdeMens2) VALUES(" + idContrib2.ToString + ",'" + moneda + "'," + tipoCamb + "," + montoChqCajaMens + "," + id.Text + ")"
                        myCommand2 = New SqlCommand(q)
                        ExecuteNonQueryFunction(myCommand2)

                    End If
siguiente2:
                    ColCAnt = array(i, 3)
                    Session("barraIteracion") = Session("barraIteracion") + 1
                Next
                Dim q2, dr2
                'validamos las proporciones sumen1 
                Dim ban = 0
                q = "SELECT id,porcProp,nContratoCta FROM titular2 WHERE porcProp IS NOT NULL AND porcProp<>1 and idIdeMens2=" + id.Text
                myCommand = New SqlCommand(q)
                Using dr = ExecuteReaderFunction(myCommand)
                    While dr.Read()
                        idTitular2 = dr("id")
                        porcPropTit = dr("porcProp")
                        nContratoCta = dr("nContratoCta")
                        q2 = "SELECT SUM(porcProp) as porcProtCot FROM cotit2 WHERE idTitular2=" + idTitular2.ToString
                        myCommand2 = New SqlCommand(q2)
                        porcPropCot = ExecuteScalarFunction(myCommand2)

                        Dim porcPropTot = Val(porcPropTit) + Val(porcPropCot)
                        If porcPropTot <> 1 Then
                            ban = 1
                            Session("error") = Session("error") + "La suma de las proporciones es diferente a 1, es " + porcPropTot.ToString + ", verifique titular y cotitulares del contrato o cuenta " + nContratoCta + " . "
                        End If
                    End While
                End Using

                If ban = 1 Then
                    Response.Write("<script language='javascript'>alert('La suma de las proporciones es diferente a 1, revise la lista de errores en la pagina');</script>")
                    statusImport.Text = Session("error") + ". Se procesaron exitosamente los primeros " + Session("barraIteracion").ToString + " registros"
                    objThread.Abort()
                    Return 0
                End If

                Session("barraIteracion") = Session("barraN")

                Return 1
            Else
                objThread.Abort()
                Return 0
            End If

        Catch ex As Exception
            'Dim st As New StackTrace(True)
            'st = New StackTrace(ex, True)
            Session("error") = ex.Message + ":" + ex.StackTrace
            statusImport.Text = Session("error") + ". Se procesaron exitosamente los primeros " + Session("barraIteracion").ToString + " registros"
            Response.Write("<script language='javascript'>alert('" + ex.Message + ":" + ex.StackTrace + "');</script>")
            objThread.Abort()
            Return 0
        End Try
    End Function

    Private Function validaDecla() As Integer
        Dim ctrlErr = 0
        Dim msgErr = ""
        Try
            Dim excel As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application
            excel.DisplayAlerts = False
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

            If sheet.UsedRange.Columns.Count < 24 Then 'cols del encab
                w.Close(False)
                excel.Quit()
                w = Nothing
                excel = Nothing
                Response.Write("<script language='javascript'>alert('Es necesario dejar el encabezado de los primeros 4 renglones tal cual se le indica en la plantilla default con 24 columnas (25 en caso de cheques de caja)');</script>")
                ctrlErr = 1
                GoTo etqErr
            End If

            Dim r As Microsoft.Office.Interop.Excel.Range = sheet.UsedRange
            Dim array(,) As Object = r.Value(XlRangeValueDataType.xlRangeValueDefault)
            Dim nRensPre = sheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row 'sin rens en bco

            w.Close(False)
            excel.Quit()
            w = Nothing
            excel = Nothing

            If array IsNot Nothing Then

                Dim descrip, nom, apPat, apMat, razon, rfc, curp, numIdFis, entFed, idEntFed, calle, nExterior, nInt, cp, col, loc, correo, tel1, tel2, idContrib2, nContratoCta, sumaDepoEfe, exedente, moneda, tipoCamb, porcPropTit, porcPropCot, idTitular2, montoChqCajaMens, idIdeMens2
                Dim q, descripAnt, ideTitular2actual, ColCAnt

                descripAnt = ""
                ColCAnt = array(4, 3)

                Dim rens
                For i As Integer = 5 To nRensPre '1-5rens=encab 6o=datos
                    If IsNothing(array(i, 1)) And IsNothing(array(i, 2)) And IsNothing(array(i, 3)) And IsNothing(array(i, 4)) And IsNothing(array(i, 5)) And IsNothing(array(i, 6)) And IsNothing(array(i, 7)) And IsNothing(array(i, 8)) And IsNothing(array(i, 9)) And IsNothing(array(i, 10)) And IsNothing(array(i, 11)) And IsNothing(array(i, 12)) And IsNothing(array(i, 13)) And IsNothing(array(i, 14)) And IsNothing(array(i, 15)) And IsNothing(array(i, 16)) And IsNothing(array(i, 17)) And IsNothing(array(i, 18)) And IsNothing(array(i, 19)) And IsNothing(array(i, 20)) And IsNothing(array(i, 21)) And IsNothing(array(i, 22)) And IsNothing(array(i, 23)) And IsNothing(array(i, 24)) And IsNothing(array(i, 25)) Then ' ren bco
                        GoTo siguiente
                    End If

                    If Not IsNothing(array(i, 1)) Then
                        descrip = Trim(UCase(array(i, 1)))
                        If descrip = "TIT" Or descrip = "COT" Or descrip = "CHQ" Then
                            If validaSecuencia(descrip, descripAnt, i, ColCAnt, msgErr) < 1 Then
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        Else
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " el tipo es inválido"
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                    Else
                        descrip = ""
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " el tipo no puede estar vacio"
                        ctrlErr = 1
                        GoTo siguiente
                    End If
                    descripAnt = descrip

                    If Not IsNothing(array(i, 5)) Then
                        If Len(array(i, 5).ToString.ToUpper.Trim) > 180 Then
                            msgErr = msgErr + ". " + vbCr + "Truncando nombre a 180 caracteres en el renglon " + CStr(i)
                        End If
                        nom = Left(array(i, 5).ToString.ToUpper.Trim, 180).Replace("'", "''")
                    Else
                        nom = ""
                    End If
                    If Not IsNothing(array(i, 3)) Then
                        If Len(array(i, 3).ToString.ToUpper.Trim) > 180 Then
                            msgErr = msgErr + ". " + vbCr + "Truncando apellido paterno a 180 caracteres en el renglon " + CStr(i)
                        End If
                        apPat = Left(array(i, 3).ToString.ToUpper.Trim, 180).Replace("'", "''")
                    Else
                        apPat = ""
                    End If
                    If Not IsNothing(array(i, 4)) Then
                        If Len(array(i, 4).ToString.ToUpper.Trim) > 180 Then
                            msgErr = msgErr + ". " + vbCr + "Truncando apellido materno a 180 caracteres en el renglon " + CStr(i)
                        End If
                        apMat = Left(array(i, 4).ToString.ToUpper.Trim, 180).Replace("'", "''")
                    Else
                        apMat = ""
                    End If
                    If Not IsNothing(array(i, 6)) Then
                        If Len(array(i, 6).ToString.ToUpper.Trim) > 500 Then
                            msgErr = msgErr + ". " + vbCr + "Truncando razon social a 500 caracteres en el renglon " + CStr(i)
                        End If
                        razon = Left(array(i, 6).ToString.ToUpper.Trim, 500).Replace("'", "''")
                    Else
                        razon = ""
                    End If

                    If (nom = "" And apPat = "") And razon = "" Then
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar el nombre con apellidos o bien la razon social"
                        ctrlErr = 1
                        GoTo siguiente
                    End If
                    If nom <> "" And apPat = "" Then
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar el apellido o bien, quite el nombre y agregue la razon social"
                        ctrlErr = 1
                        GoTo siguiente
                    End If
                    If nom <> "" And razon <> "" Then
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " si no está reportando una razon social dejela en blanco, en caso contrario deje en blanco el nombre y los apellidos"
                        ctrlErr = 1
                        GoTo siguiente
                    End If

                    If Not IsNothing(array(i, 2)) Then
                        rfc = array(i, 2).ToString.ToUpper.Trim.Replace("-", "").Replace(" ", "")
                        Dim expresion
                        If razon = "" Then 'pf
                            expresion = "^([A-Z\s]{4})\d{6}([A-Z\w]{0,3})$"
                            If Len(rfc) <> 13 Then
                                msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(i) + " el tamaño de rfc debe ser 13 caracteres homoclave incluida en mayusculas"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        Else 'pm
                            expresion = "^([A-Z\s]{3})\d{6}([A-Z\w]{0,3})$"
                            If Len(rfc) <> 12 Then
                                msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(i) + " el tamaño de rfc debe ser 12 caracteres homoclave incluida en mayusculas"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        End If
                        If Not Regex.IsMatch(rfc, expresion) Then
                            msgErr = msgErr + ". " + vbCr + "Formato de rfc invalido en el renglon " + CStr(i)
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                    Else
                        rfc = ""
                    End If
                    If Not IsNothing(array(i, 8)) Then
                        curp = array(i, 8).ToString.ToUpper.Trim.Replace("-", "").Replace(" ", "")
                        Dim expresion
                        If razon = "" Then 'pf
                            expresion = "^([A-Z&]|[a-z&]{1})([AEIOU]|[aeiou]{1})([A-Z&]|[a-z&]{1})([A-Z&]|[a-z&]{1})([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])([HM]|[hm]{1})([AS|as|BC|bc|BS|bs|CC|cc|CS|cs|CH|ch|CL|cl|CM|cm|DF|df|DG|dg|GT|gt|GR|gr|HG|hg|JC|jc|MC|mc|MN|mn|MS|ms|NT|nt|NL|nl|OC|oc|PL|pl|QT|qt|QR|qr|SP|sp|SL|sl|SR|sr|TC|tc|TS|ts|TL|tl|VZ|vz|YN|yn|ZS|zs|NE|ne]{2})([^A|a|E|e|I|i|O|o|U|u]{1})([^A|a|E|e|I|i|O|o|U|u]{1})([^A|a|E|e|I|i|O|o|U|u]{1})([0-9]{2})$"
                            If Len(curp) <> 18 Then
                                msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(i) + " el tamaño de curp debe ser 18 caracteres en mayusculas"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        End If
                        If Not Regex.IsMatch(curp, expresion) Then
                            msgErr = msgErr + ". " + vbCr + "Formato de curp invalido en el renglon " + CStr(i)
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                    Else
                        curp = ""
                    End If
                    If Not IsNothing(array(i, 9)) Then
                        If Len(array(i, 9).ToString.ToUpper.Trim) > 30 Then
                            msgErr = msgErr + ". " + vbCr + "Truncando numero de identificacion fiscal numIdFis a 30 caracteres en el renglon " + CStr(i)
                        End If
                        numIdFis = Left(array(i, 9).ToString.ToUpper.Trim, 30).Replace("'", "''")
                    Else
                        numIdFis = ""
                    End If
                    If Not IsNothing(array(i, 10)) Then
                        entFed = Left(array(i, 10).ToString.ToUpper.Trim, 40)
                        q = "SELECT id FROM entFed2 where descr='" + entFed + "'"
                        myCommand = New SqlCommand(q)
                        Dim v = ExecuteScalarFunction(myCommand)
                        If Not IsNothing(v) Then
                            idEntFed = v
                        Else
                            idEntFed = ""
                            msgErr = msgErr + ". " + vbCr + "Entidad federativa invalida en el renglon " + CStr(i)
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                    Else
                        entFed = ""
                        idEntFed = ""
                        msgErr = msgErr + ". " + vbCr + "Entidad federativa invalida en el renglon " + CStr(i)
                        ctrlErr = 1
                        GoTo siguiente
                    End If
                    If Not IsNothing(array(i, 11)) Then
                        If Len(array(i, 11).ToString.ToUpper.Trim) > 200 Then
                            msgErr = msgErr + ". " + vbCr + "Truncando calle a 200 caracteres en el renglon " + CStr(i)
                        End If
                        calle = Left(array(i, 11).ToString.ToUpper.Trim, 200).Replace("'", "''")
                    Else
                        calle = ""
                        msgErr = msgErr + ". " + vbCr + "Calle vacia en el renglon " + CStr(i)
                        ctrlErr = 1
                        GoTo siguiente
                    End If
                    If Not IsNothing(array(i, 12)) Then
                        If Len(array(i, 12).ToString.ToUpper.Trim) > 20 Then
                            msgErr = msgErr + ". " + vbCr + "Truncando numero exterior a 20 caracteres en el renglon " + CStr(i)
                        End If
                        nExterior = Left(array(i, 12).ToString.ToUpper.Trim, 20).Replace("'", "''")
                    Else
                        nExterior = ""
                        msgErr = msgErr + ". " + vbCr + "Num. Exterior invalido en el renglon " + CStr(i)
                        ctrlErr = 1
                        GoTo siguiente
                    End If
                    If Not IsNothing(array(i, 13)) Then
                        If Len(array(i, 13).ToString.ToUpper.Trim) > 20 Then
                            msgErr = msgErr + ". " + vbCr + "Truncando numero interior a 20 caracteres en el renglon " + CStr(i)
                        End If
                        nInt = Left(array(i, 13).ToString.ToUpper.Trim, 20).Replace("'", "''")
                    Else
                        nInt = ""
                    End If
                    If Not IsNothing(array(i, 14)) Then
                        If Len(array(i, 14).ToString.ToUpper.Trim) > 5 Then
                            msgErr = msgErr + ". " + vbCr + "Truncando codigo postal a 5 caracteres en el renglon " + CStr(i)
                        End If
                        If Not IsNumeric(array(i, 14).ToString.ToUpper.Trim.Replace(" ", "")) Then
                            msgErr = msgErr + ". " + vbCr + "codigo postal solo debe tener numeros en el renglon " + CStr(i)
                        End If
                        cp = Left(array(i, 14).ToString.ToUpper.Trim, 5).Replace("'", "''")
                    Else
                        cp = ""
                        msgErr = msgErr + ". " + vbCr + "Código postal invalido en el renglon " + CStr(i)
                        ctrlErr = 1
                        GoTo siguiente
                    End If
                    If Not IsNothing(array(i, 15)) Then
                        If Len(array(i, 15).ToString.ToUpper.Trim) > 200 Then
                            msgErr = msgErr + ". " + vbCr + "Truncando colonia a 200 caracteres en el renglon " + CStr(i)
                        End If
                        col = Left(array(i, 15).ToString.ToUpper.Trim, 200).Replace("'", "''")
                    Else
                        col = ""
                        msgErr = msgErr + ". " + vbCr + "Colonia vacia en el renglon " + CStr(i)
                        ctrlErr = 1
                        GoTo siguiente
                    End If
                    If Not IsNothing(array(i, 16)) Then
                        If Len(array(i, 16).ToString.ToUpper.Trim) > 200 Then
                            msgErr = msgErr + ". " + vbCr + "Truncando localidad o municipio a 200 caracteres en el renglon " + CStr(i)
                        End If
                        loc = Left(array(i, 16).ToString.ToUpper.Trim, 200).Replace("'", "''")
                    Else
                        loc = ""
                        msgErr = msgErr + ". " + vbCr + "Localidad vacia en el renglon " + CStr(i)
                        ctrlErr = 1
                        GoTo siguiente
                    End If
                    If Not IsNothing(array(i, 17)) Then
                        If Len(array(i, 17).ToString.ToUpper.Trim) > 200 Then
                            msgErr = msgErr + ". " + vbCr + "Truncando correo a 200 caracteres en el renglon " + CStr(i)
                        End If
                        correo = Left(array(i, 17).ToString.ToUpper.Trim, 200).Replace("'", "''")
                    Else
                        correo = ""
                        msgErr = msgErr + ". " + vbCr + "Correo vacio en el renglon " + CStr(i)
                        ctrlErr = 1
                        GoTo siguiente
                    End If
                    If Not IsNothing(array(i, 18)) Then
                        If array(i, 18).ToString.ToUpper.Trim <> "" Then
                            If Len(array(i, 18).ToString.ToUpper.Trim) > 10 Then
                                msgErr = msgErr + ". " + vbCr + "Truncando telefono1 a 10 caracteres en el renglon " + CStr(i)
                            End If
                            If Not IsNumeric(Regex.Replace(array(i, 18).ToString.ToUpper.Trim.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("'", ""), "[a-zA-Z\s]+", "")) Then
                                msgErr = msgErr + ". " + vbCr + "Telefono1 solo debe tener numeros en el renglon " + CStr(i)
                            End If
                            tel1 = Regex.Replace(Left(array(i, 18).ToString.ToUpper.Trim.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace("'", ""), 10), "[a-zA-Z\s]+", "")
                        Else
                            msgErr = msgErr + ". " + vbCr + "Telefono vacio en el renglon " + CStr(i)
                            ctrlErr = 1
                            GoTo siguiente
                            tel1 = ""
                        End If
                    Else
                        tel1 = ""
                        msgErr = msgErr + ". " + vbCr + "Tel1 vacio en el renglon " + CStr(i)
                        ctrlErr = 1
                        GoTo siguiente
                    End If
                    If Not IsNothing(array(i, 19)) Then
                        If array(i, 19).ToString.ToUpper.Trim <> "" Then
                            If Len(array(i, 19).ToString.ToUpper.Trim) > 10 Then
                                msgErr = msgErr + ". " + vbCr + "Truncando telefono2 a 10 caracteres en el renglon " + CStr(i)
                            End If
                            If Not IsNumeric(Regex.Replace(array(i, 19).ToString.ToUpper.Trim.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("'", ""), "[a-zA-Z\s]+", "")) Then
                                msgErr = msgErr + ". " + vbCr + "Telefono2 solo debe tener numeros en el renglon " + CStr(i)
                            End If
                            tel2 = Regex.Replace(Left(array(i, 19).ToString.ToUpper.Trim.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace("'", ""), 10), "[a-zA-Z\s]+", "")
                        Else
                            tel2 = ""
                        End If
                    Else
                        tel2 = ""
                    End If

                    If curp = "" And rfc = "XAXX010101000" Then
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar curp de la persona fisica de rfc generico nacional"
                        ctrlErr = 1
                        GoTo siguiente
                    End If

                    If rfc = "" Then
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar rfc"
                        ctrlErr = 1
                        GoTo siguiente
                    End If

                    If numIdFis = "" And (rfc = "EXTF990101000" Or rfc = "EXT990101000") Then
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar el numero de indentificacion fiscal del extranjero debido al rfc extranjero"
                        ctrlErr = 1
                        GoTo siguiente
                    End If

                    If entFed = "" Then
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar la entidad federativa"
                        ctrlErr = 1
                        GoTo siguiente
                    End If

                    If calle = "" Then
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar la calle"
                        ctrlErr = 1
                        GoTo siguiente
                    End If

                    If nExterior = "" Then
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar el numero exterior"
                        ctrlErr = 1
                        GoTo siguiente
                    End If

                    If cp = "" Then
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar codigo postal"
                        ctrlErr = 1
                        GoTo siguiente
                    End If

                    If col = "" Then
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar la colonia"
                        ctrlErr = 1
                        GoTo siguiente
                    End If

                    If loc = "" Then
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar localidad o municipio"
                        ctrlErr = 1
                        GoTo siguiente
                    End If

                    If correo = "" Then
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar el correo"
                        ctrlErr = 1
                        GoTo siguiente
                    End If

                    If tel1 = "" Then
                        msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar el primer telefono"
                        ctrlErr = 1
                        GoTo siguiente
                    End If

                    If descrip = "TIT" Then 'titular
                        If Not IsNothing(array(i, 7)) Then
                            If Len(array(i, 7).ToString.ToUpper.Trim) > 30 Then
                                msgErr = msgErr + ". " + vbCr + "Truncando numero de contrato o cuenta o cliente a 30 caracteres en el renglon " + CStr(i)
                            End If
                            nContratoCta = Left(array(i, 7).ToString.ToUpper.Trim, 300).Replace("'", "''")
                        Else
                            nContratoCta = ""
                        End If

                        If Not IsNothing(array(i, 20)) Then
                            If array(i, 20).ToString.Trim <> "" Then
                                If Not IsNumeric(array(i, 20)) Then
                                    msgErr = msgErr + ". " + vbCr + "la suma de efectivo debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(i)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                                If CDbl(array(i, 20)) <= 0 Then
                                    msgErr = msgErr + ". " + vbCr + "la suma de efectivo " + array(i, 20).ToString + " debe ser > 0 en el renglon " + CStr(i)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                                sumaDepoEfe = array(i, 20).ToString
                            Else
                                sumaDepoEfe = ""
                            End If
                        Else
                            sumaDepoEfe = ""
                        End If
                        If Not IsNothing(array(i, 21)) Then
                            If array(i, 21).ToString.Trim <> "" Then
                                If Not IsNumeric(array(i, 21)) Then
                                    msgErr = msgErr + ". " + vbCr + "El excedente debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(i)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                                If CDbl(array(i, 21)) < 0 Then
                                    msgErr = msgErr + ". " + vbCr + "el excedente debe ser >= 0 en el renglon " + CStr(i)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                                exedente = array(i, 21).ToString
                            Else
                                exedente = ""
                            End If
                        Else
                            exedente = ""
                        End If
                        If Not IsNothing(array(i, 22)) Then
                            If Len(array(i, 22).ToString.ToUpper.Trim) > 3 Then
                                msgErr = msgErr + ". " + vbCr + "Truncando moneda a 3 caracteres en el renglon " + CStr(i)
                            End If
                            moneda = Left(array(i, 22).ToString.ToUpper.Trim, 3).Replace("'", "''")
                        Else
                            moneda = ""
                        End If
                        If Not IsNothing(array(i, 23)) Then
                            If array(i, 23).ToString.Trim <> "" Then
                                If Not IsNumeric(array(i, 23)) Then
                                    msgErr = msgErr + ". " + vbCr + "El tipo de cambio debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(i)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                                If CDbl(array(i, 23)) <= 0 Then
                                    msgErr = msgErr + ". " + vbCr + "el tipo de cambio debe ser > 0 en el renglon " + CStr(i)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                                tipoCamb = array(i, 23).ToString
                            Else
                                tipoCamb = ""
                            End If
                        Else
                            tipoCamb = ""
                        End If

                        If moneda = "" Then
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar la moneda"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                        If tipoCamb = "" And moneda <> "MXN" Then
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar el tipo de cambio numerico > 0 cuando la moneda es MXN"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                        If Not IsNothing(array(i, 24)) Then
                            If Not IsNumeric(array(i, 24)) Then
                                msgErr = msgErr + ". " + vbCr + "La proporcion debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(i)
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If CDbl(array(i, 24)) <= 0 Then
                                msgErr = msgErr + ". " + vbCr + "La proporcion debe ser > 0 en el renglon " + CStr(i)
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            porcPropTit = array(i, 24).ToString
                        Else
                            porcPropTit = ""
                        End If

                        'ren vacio
                        If nom = "" And apPat = "" And apMat = "" And razon = "" And rfc = "" And curp = "" And numIdFis = "" And entFed = "" And calle = "" And nExterior = "" And nInt = "" And cp = "" And col = "" And loc = "" And correo = "" And tel1 = "" And tel2 = "" And nContratoCta = "" And exedente = "" And sumaDepoEfe = "" And moneda = "" And tipoCamb = "" And porcPropTit = "" Then
                            GoTo siguiente
                        End If

                        If nContratoCta = "" Then
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar el numero de contrato, de cuenta o de socio o de cliente"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                        If sumaDepoEfe = "" Then
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar la suma de los depositos en efectivo"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                        If exedente = "" Then
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar el exedente"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                        If CDbl(Replace(sumaDepoEfe, ",", "")) > 0 And CDbl(Replace(sumaDepoEfe, ",", "")) < 15000 Then 'es el importe minimo que genera un ide de 1 peso
                            msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(i) + " presenta un deposito en efectivo menor a $15,000, elimine el registro o bien corrija los montos "
                            ctrlErr = 1
                            GoTo siguiente
                        End If
                    ElseIf descrip = "CHQ" Then
                        If Not IsNothing(array(i, 22)) Then
                            If Len(array(i, 22).ToString.ToUpper.Trim) > 3 Then
                                msgErr = msgErr + ". " + vbCr + "Truncando moneda a 3 caracteres en el renglon " + CStr(i)
                            End If
                            moneda = Left(array(i, 22).ToString.ToUpper.Trim, 3).Replace("'", "''")
                        Else
                            moneda = ""
                        End If
                        If Not IsNothing(array(i, 23)) Then
                            If array(i, 23).ToString.Trim <> "" Then
                                If Not IsNumeric(array(i, 23)) Then
                                    msgErr = msgErr + ". " + vbCr + "El tipo de cambio debe ser tipo numerico o dejelo en blanco eliminando el contenido en el renglon " + CStr(i)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                                If CDbl(array(i, 23)) <= 0 Then
                                    msgErr = msgErr + ". " + vbCr + "el tipo de cambio debe ser > 0 en el renglon " + CStr(i)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                                tipoCamb = array(i, 23).ToString
                            Else
                                tipoCamb = ""
                            End If
                        Else
                            tipoCamb = ""
                        End If

                        If moneda = "" Then
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar la moneda"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                        If tipoCamb = "" And moneda <> "MXN" Then
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar el tipo de cambio numerico > 0 cuando la moneda es MXN"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                        If Not IsNothing(array(i, 25)) Then
                            If array(i, 25).ToString <> "-" Then

                                If Not IsNumeric(array(i, 25)) Then
                                    msgErr = msgErr + ". " + vbCr + "El monto de cheque de caja debe ser tipo numerico en el renglon " + CStr(i)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                                If CDbl(array(i, 25)) <= 0 Then
                                    msgErr = msgErr + ". " + vbCr + "el monto del cheque de caja debe ser > 0 en el renglon " + CStr(i)
                                    ctrlErr = 1
                                    GoTo siguiente
                                End If
                                montoChqCajaMens = array(i, 25).ToString.Trim

                            Else
                                montoChqCajaMens = ""
                            End If
                        Else
                            montoChqCajaMens = ""
                        End If
                        If montoChqCajaMens = "" Then
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar el monto del cheque de caja > 0"
                            ctrlErr = 1
                            GoTo siguiente
                        End If

                    ElseIf descrip = "COT" Then
                        If Not IsNothing(array(i, 24)) Then
                            porcPropCot = array(i, 24).ToString.ToUpper.Trim
                            If IsNumeric(porcPropCot) = False Then
                                msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(i) + " no contiene una proporción en formato numérico"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                            If CDbl(Replace(porcPropCot, ",", "")) <= 0 Or CDbl(Replace(porcPropCot, ",", "")) >= 1 Then
                                msgErr = msgErr + ". " + vbCr + "en el renglon " + CStr(i) + "el porcentaje de proporción debe ser >0 y <1"
                                ctrlErr = 1
                                GoTo siguiente
                            End If
                        Else
                            porcPropCot = ""
                            msgErr = msgErr + ". " + vbCr + "En el renglon " + CStr(i) + " requiere especificar el % de proporción "
                            ctrlErr = 1
                            GoTo siguiente
                        End If


                    Else
                        msgErr = msgErr + ". " + vbCr + "el renglon " + CStr(i) + " contiene un tipo inválido en columna A"
                        ctrlErr = 1
                        GoTo siguiente
                    End If
siguiente:
                    ColCAnt = array(i, 3)
                Next
etqErr:
                If ctrlErr = 1 Then
                    Dim idEstatus = returnID("estatusDecla2", "pendiente recibir archivos")
                    estado.SelectedValue = idEstatus
                    myCommand2 = New SqlCommand("UPDATE ideMens2 SET idEstatusDecla=" + idEstatus.ToString + " WHERE id=" + id.Text)
                    ExecuteNonQueryFunction(myCommand2)
                    lblErrImport.Visible = True
                    errImport.Visible = True
                    errImport.Text = msgErr
                    importMensXls.Enabled = True
                    Response.Write("<script language='javascript'>alert('Detectamos errores, acepte para verlos');</script>")
                    Return 0
                Else
                    If msgErr <> "" Then
                        lblErrImport.Visible = True
                        errImport.Visible = True
                        errImport.Text = msgErr
                        importMensXls.Enabled = True
                        Response.Write("<script language='javascript'>alert('Detectamos errores, acepte para verlos');</script>")
                        Return 0
                    Else
                        lblErrImport.Visible = False
                        errImport.Visible = False
                        importMensXls.Enabled = True
                    End If
                End If
            End If
        Catch ex As Exception
            'Dim st As New StackTrace(True)
            'st = New StackTrace(ex, True)
            lblErrImport.Visible = True
            errImport.Visible = True
            errImport.Text = ex.StackTrace
            importMensXls.Enabled = True
            Dim MSG = "<script language='javascript'>alert('" + ex.StackTrace + "');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Return 0
        End Try
        Return 1

    End Function

    Protected Sub ver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ver.Click
        MultiView1.ActiveViewIndex = Int32.Parse(2)
        cargaGrid()
        progressbar1.Style("width") = "0px"
        statusImport.Text = ""
        lbldescrip.Text = ""
    End Sub

    Private Sub refrescaTotalesMens()
        'totales
        Dim q2 = "SELECT SUM(sumaDepoEfe) as sumaDepoEfe, SUM(excedente) as sumaExcedente, COUNT(*) as nTit2  FROM titular2 WHERE idideMens2=" + id.Text
        myCommand2 = New SqlCommand(q2)
        Using dr2 = ExecuteReaderFunction(myCommand2)
            dr2.Read()
            If DBNull.Value.Equals(dr2("sumaExcedente")) Then
                impteExcedente.Text = 0
            Else
                impteExcedente.Text = FormatCurrency(dr2("sumaExcedente"), 0)
            End If
            If DBNull.Value.Equals(dr2("sumaDepoEfe")) Then
                sumaDepEfe.Text = 0
            Else
                sumaDepEfe.Text = FormatCurrency(dr2("sumaDepoEfe"), 0)
            End If
            If DBNull.Value.Equals(dr2("nTit2")) Then
                ntit2.Text = 0
            Else
                ntit2.Text = FormatNumber(dr2("nTit2"), 0)
            End If
        End Using


        Dim q3 = "SELECT SUM(montoChqCajaMens) as sumaChq, COUNT(*) as nChq2 FROM chq2 WHERE idideMens2=" + id.Text
        myCommand2 = New SqlCommand(q3)
        Using dr3 = ExecuteReaderFunction(myCommand2)
            dr3.Read()
            If DBNull.Value.Equals(dr3("sumaChq")) Then
                montoChqCaja.Text = 0
            Else
                montoChqCaja.Text = FormatCurrency(dr3("sumaChq"), 0)
            End If

            If DBNull.Value.Equals(dr3("nChq2")) Then
                nChq2.Text = 0
            Else
                nChq2.Text = FormatNumber(dr3("nChq2"), 0)
            End If
        End Using

        Dim idEstatus = returnID("estatusDecla2", "validada")

        myCommand2 = New SqlCommand("UPDATE ideMens2 SET excedente=" + impteExcedente.Text.Trim.Replace(",", "") + ",sumaDepoEfe=" + sumaDepEfe.Text.Trim.Replace(",", "") + ",montoChqCaja=" + montoChqCaja.Text.Trim.Replace(",", "") + ",nTit2=" + ntit2.Text.Trim.Replace(",", "") + ",nchq2=" + nChq2.Text.Trim.Replace(",", "") + ", idEstatusDecla=" + idEstatus.ToString + ", idContrato=" + idContrato.ToString + ", validada=1 WHERE id=" + id.Text)
        ExecuteNonQueryFunction(myCommand2)

    End Sub


    Protected Sub mod_Click(ByVal sender As Object, ByVal e As EventArgs) Handles crearDecla.Click
        If id.Text = "0" Then
            lbldescrip.Text = "Primero importe los datos o Crear en ceros"
            Response.Write("<script language='javascript'>alert('Primero importe los datos o Crear en ceros');</script>")
            Exit Sub
        End If
        progressbar1.Style("width") = "0px"
        statusImport.Text = ""

        Dim mes2dig, contra
        If mes.ToString.Length = 1 Then
            mes2dig = "0" & mes.ToString
        Else
            mes2dig = mes.ToString
        End If

        Dim fechaDeclarar = Convert.ToDateTime(Trim("01/" + mes2dig + "/" + ejercicio.ToString)).ToString("yyyy-MM-dd")

        Dim q = "SELECT co.id, pla.elplan FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND co.id=" + Session("GidContrato").ToString + " AND ( (nDeclHechas < nDeclContratadas and (pla.elplan<>'PREMIUM')) or (('" + Convert.ToDateTime(fechaDeclarar).ToString("yyyy-MM-dd") + "' >= periodoInicial and pla.elplan='PREMIUM') and ('" + Convert.ToDateTime(fechaDeclarar).ToString("yyyy-MM-dd") + "' <= fechaFinal and pla.elplan='PREMIUM') ) ) order by co.id"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            If (Not dr.HasRows) Then 'sin contrato vigente 
                Response.Write("<script language='javascript'>alert('A alcanzado el máximo de declaraciones contratadas o bien ha caducado su contrato, o los periodos a declarar no están cubiertos por este contrato');</script>")
                Exit Sub
            End If
        End Using

        controlaAcceso2()
        If redir.Text = "1" Then
            Exit Sub
        End If

        If Request.QueryString("op") = "0" And Request.QueryString("subop") = "0" Then 'crear editar excel
            cargaGrid()
            progressbar1.Style("width") = "0px"
            statusImport.Text = ""
            lbldescrip.Text = ""
        End If

        q = "UPDATE ideMens2 SET excedente=" + impteExcedente.Text.Trim.Replace(",", "") + ",sumaDepoEfe=" + sumaDepEfe.Text.Trim.Replace(",", "") + ",montoChqCaja=" + montoChqCaja.Text.Trim.Replace(",", "") + ",nTit2=" + ntit2.Text.Trim.Replace(",", "") + ",nchq2=" + nChq2.Text.Trim.Replace(",", "") + " WHERE id=" + id.Text
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)


        If Request.QueryString("op") = "0" Then 'no se valida para 0s o consulta
            Dim v = creaTxtMes()
            If v <> "" Then 'crea el zip del xml y lo copia a BD
                'lbldescrip.Text = v
                crearDecla.Enabled = True
                Response.Write("<script language='javascript'>alert('" + v + "');</script>")
                Exit Sub
            End If
        End If
        'consume creditos
        Dim fechaUltima = DateAdd(DateInterval.Day, -DatePart(DateInterval.Day, Now()) + 1, Now()) 'dia 1o del mes actual
        fechaUltima = DateAdd(DateInterval.Month, -1, fechaUltima) 'dia 1o del mes anterior
        q = "SELECT co.id, pla.elplan FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND ( (nDeclHechas < nDeclContratadas and (pla.elplan<>'PREMIUM')) or ('" + Convert.ToDateTime(fechaUltima).ToString("yyyy-MM-dd") + "' <= fechaFinal and pla.elplan='PREMIUM' and co.esRegularizacion=0) or (pla.elplan='PREMIUM' and co.esRegularizacion=1) ) order by case when pla.elplan='PREMIUM' then 1 else 2 end, pla.elplan, co.id"
        myCommand = New SqlCommand(q)
        Dim elplan
        Using dr = ExecuteReaderFunction(myCommand)
            dr.Read()
            elplan = dr("elplan")
        End Using

        If elplan <> "PREMIUM" Then
            q = "UPDATE contratos SET nDeclHechas=nDeclHechas+1 WHERE id=" + Session("GidContrato").ToString
            myCommand = New SqlCommand(q)
            ExecuteNonQueryFunction(myCommand)
        End If
        estado.SelectedValue = returnID("estatusDecla2", "validada")
        myCommand = New SqlCommand("update ideMens2 set idEstatusDecla=" + estado.SelectedValue.ToString + " where id=" + id.Text)
        ExecuteNonQueryFunction(myCommand)
        crearDecla.Enabled = True
        Response.Write("<script language='javascript'>alert('Delaracion creada');</script>")
        lbldescrip.Text = ""
    End Sub

    Protected Sub back_Click(ByVal sender As Object, ByVal e As EventArgs) Handles back.Click
        Call regresar()
    End Sub

    Private Function validaModificada()
        Dim q = "SELECT guardadaUsuario FROM ideMens WHERE id=" + Session("GidMens").ToString
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            If v.Equals(False) Then
                Response.Write("<script language='javascript'>alert('1o presione el botón Crear');</script>")
                Return 0
            End If
        Else
            Response.Write("<script language='javascript'>alert('1o importe los datos');</script>")
            Return 0
        End If

        Return 1
    End Function

    Private Sub controlaAcceso2()
        Dim q
        If chkPostpago.Checked.Equals(False) Then
            q = "SELECT count(*) as cuenta FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND co.fechaPago IS NOT NULL and ((pla.elplan<>'PREMIUM' and nDeclHechas<nDeclContratadas) or (pla.elplan='PREMIUM' and '" + Now.ToString("yyyy-MM-dd") + "' >= periodoInicial and '" + Now.ToString("yyyy-MM-dd") + "' <= fechaFinal))"
        Else
            q = "SELECT count(*) as cuenta FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND postpago IS NOT NULL and postpago=1 and ((pla.elplan<>'PREMIUM' and nDeclHechas<nDeclContratadas) or (pla.elplan='PREMIUM' and '" + Now.ToString("yyyy-MM-dd") + "' >= periodoInicial and '" + Now.ToString("yyyy-MM-dd") + "' <= fechaFinal))"
        End If
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If (v.Equals(0)) Then 'sin contratos pagados 
            redir.Text = "1"
            Response.Write("<script language='javascript'>alert('No hay contratos pagados');</script>")
            Response.Write("<script>location.href='misContra.aspx';</script>")
            Exit Sub
        End If

    End Sub


    Private Sub regresar()
        lbldescrip.Text = ""
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

    Protected Sub export_Click(sender As Object, e As EventArgs) Handles export.Click
        Dim q
        Dim tabla

        If TreeView1.Nodes.Count = 0 Then
            export.Enabled = True
            Response.Write("<script language='javascript'>alert('Nada que exportar');</script>")
            Exit Sub
        End If

        If (Not System.IO.Directory.Exists("C:\SAT\" + Session("casfim"))) Then
            System.IO.Directory.CreateDirectory("C:\SAT\" + Session("casfim"))
        End If
        Dim arch = "C:\SAT\" + Session("casfim") + "\export" + ejercicio.ToString + "_" + mes.ToString + ".xlsx"
        If File.Exists(arch) Then
            File.Delete(arch)
        End If

        Dim oExcel As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application
        Dim oBook As Microsoft.Office.Interop.Excel.Workbook = oExcel.Workbooks.Add
        Dim oSheet As Microsoft.Office.Interop.Excel.Worksheet = oBook.Sheets(1)

        oSheet.Cells(1, 1).value = "TIPO"
        oSheet.Cells(2, 1).value = "TIT"
        oSheet.Cells(2, 2).value = "RFC"
        oSheet.Cells(2, 3).value = "AP PATERNO"
        oSheet.Cells(2, 4).value = "AP MATERNO"
        oSheet.Cells(2, 5).value = "NOMBRES"
        oSheet.Cells(2, 6).value = "RAZON SOCIAL"
        oSheet.Cells(2, 7).value = "NUM DE CUENTA O CONTRATO"
        oSheet.Cells(2, 8).value = "CURP"
        oSheet.Cells(2, 9).value = "num id Fiscal"
        oSheet.Cells(2, 10).value = "entidadFederativa"
        oSheet.Cells(2, 11).value = "calleAvenidaVia"
        oSheet.Cells(2, 12).value = "numExterior"
        oSheet.Cells(2, 13).value = "numInterior"
        oSheet.Cells(2, 14).value = "codPostal"
        oSheet.Cells(2, 15).value = "colonia"
        oSheet.Cells(2, 16).value = "localidadMunicipio"
        oSheet.Cells(2, 17).value = "CORREO ELECTRONICO (opc.)"
        oSheet.Cells(2, 18).value = "TELEFONO1"
        oSheet.Cells(2, 19).value = "TELEFONO2"
        oSheet.Cells(2, 20).value = "SUMA DE DEPOSITOS EN EFECTIVO"
        oSheet.Cells(2, 21).value = "EXCEDENTE"
        oSheet.Cells(2, 22).value = "moneda"
        oSheet.Cells(2, 23).value = "tipo de cambio"
        oSheet.Cells(2, 24).value = "% DE PROPORCION"
        oSheet.Cells(1, 1).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(1, 1).Font.Bold = True ' Fuente en negrita
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
        oSheet.Cells(2, 19).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 19).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 20).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 20).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 21).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 21).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 22).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 22).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 23).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 23).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(2, 24).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(2, 24).Font.Bold = True ' Fuente en negrita

        oSheet.Cells(3, 1).value = "COT"
        oSheet.Cells(3, 2).value = "RFC"
        oSheet.Cells(3, 3).value = "AP PATERNO"
        oSheet.Cells(3, 4).value = "AP MATERNO"
        oSheet.Cells(3, 5).value = "NOMBRES"
        oSheet.Cells(3, 6).value = "RAZON SOCIAL"
        oSheet.Cells(3, 7).value = ""
        oSheet.Cells(3, 8).value = "CURP"
        oSheet.Cells(3, 9).value = "num id Fiscal"
        oSheet.Cells(3, 10).value = "entidadFederativa"
        oSheet.Cells(3, 11).value = "calleAvenidaVia"
        oSheet.Cells(3, 12).value = "numExterior"
        oSheet.Cells(3, 13).value = "numInterior"
        oSheet.Cells(3, 14).value = "codPostal"
        oSheet.Cells(3, 15).value = "colonia"
        oSheet.Cells(3, 16).value = "localidadMunicipio"
        oSheet.Cells(3, 17).value = "CORREO ELECTRONICO (opc.)"
        oSheet.Cells(3, 18).value = "TELEFONO1"
        oSheet.Cells(3, 19).value = "TELEFONO2"
        oSheet.Cells(3, 20).value = ""
        oSheet.Cells(3, 21).value = ""
        oSheet.Cells(3, 22).value = ""
        oSheet.Cells(3, 23).value = ""
        oSheet.Cells(3, 24).value = "% DE PROPORCION"
        oSheet.Cells(3, 1).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 1).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 2).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 2).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 3).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 3).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 4).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 4).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 5).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 5).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 6).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 6).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 7).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 7).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 8).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 8).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 9).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 9).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 10).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 10).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 11).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 11).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 12).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 12).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 13).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 13).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 14).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 14).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 15).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 15).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 16).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 16).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 17).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 17).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 18).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 18).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 19).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 19).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 20).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 20).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 21).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 21).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 22).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 22).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 23).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 23).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(3, 24).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(3, 24).Font.Bold = True ' Fuente en negrita

        oSheet.Cells(4, 1).value = "CHQ"
        oSheet.Cells(4, 2).value = "RFC"
        oSheet.Cells(4, 3).value = "AP PATERNO"
        oSheet.Cells(4, 4).value = "AP MATERNO"
        oSheet.Cells(4, 5).value = "NOMBRES"
        oSheet.Cells(4, 6).value = "RAZON SOCIAL"
        oSheet.Cells(4, 7).value = ""
        oSheet.Cells(4, 8).value = "CURP"
        oSheet.Cells(4, 9).value = "num id Fiscal"
        oSheet.Cells(4, 10).value = "entidadFederativa"
        oSheet.Cells(4, 11).value = "calleAvenidaVia"
        oSheet.Cells(4, 12).value = "numExterior"
        oSheet.Cells(4, 13).value = "numInterior"
        oSheet.Cells(4, 14).value = "codPostal"
        oSheet.Cells(4, 15).value = "colonia"
        oSheet.Cells(4, 16).value = "localidadMunicipio"
        oSheet.Cells(4, 17).value = "CORREO ELECTRONICO (opc.)"
        oSheet.Cells(4, 18).value = "TELEFONO1"
        oSheet.Cells(4, 19).value = "TELEFONO2"
        oSheet.Cells(4, 20).value = ""
        oSheet.Cells(4, 21).value = ""
        oSheet.Cells(4, 22).value = "moneda"
        oSheet.Cells(4, 23).value = "tipo de cambio"
        oSheet.Cells(4, 24).value = ""
        oSheet.Cells(4, 25).value = "MONTO DEL CHEQUE CAJA"
        oSheet.Cells(4, 1).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 1).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 2).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 2).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 3).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 3).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 4).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 4).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 5).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 5).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 6).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 6).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 7).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 7).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 8).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 8).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 9).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 9).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 10).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 10).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 11).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 11).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 12).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 12).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 13).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 13).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 14).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 14).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 15).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 15).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 16).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 16).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 17).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 17).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 18).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 18).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 19).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 19).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 20).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 20).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 21).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 21).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 22).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 22).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 23).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 23).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 24).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 24).Font.Bold = True ' Fuente en negrita
        oSheet.Cells(4, 25).Font.Size = 12  ' tamaño de letra
        oSheet.Cells(4, 25).Font.Bold = True ' Fuente en negrita

        oSheet.Range("T:T").NumberFormat = "###,###,###,###" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha
        oSheet.Range("U:U").NumberFormat = "###,###,###,###" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha
        'oSheet.Range("W:W").NumberFormat = "###,###,###,##0.00" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha
        oSheet.Range("X:X").NumberFormat = "0.####" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha
        oSheet.Range("Y:Y").NumberFormat = "###,###,###,###" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha

        Dim ren = 5
        q = "SELECT d.*, c.*, d.id as idC FROM titular2 d, contrib2 c WHERE d.idContrib2=c.id AND idIdeMens2=" + id.Text + " order by d.id"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            While dr.Read()
                oSheet.Cells(ren, 1).value = "TIT"
                oSheet.Cells(ren, 2).value = dr("rfc")
                If Not dr("apPat").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 3).value = dr("apPat")
                End If
                If Not dr("apMat").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 4).value = dr("apMat")
                End If
                If Not dr("nom").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 5).value = dr("nom")
                End If
                If Not dr("razonSocial").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 6).value = dr("razonSocial")
                End If
                oSheet.Cells(ren, 7).value = dr("nContratoCta").ToString
                If Not dr("curp").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 8).value = dr("curp")
                End If
                If Not dr("numIdFis").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 9).value = dr("numIdFis")
                End If
                oSheet.Cells(ren, 10).value = getNomEntFed(dr("idEntFed"))
                oSheet.Cells(ren, 11).value = dr("calle")
                oSheet.Cells(ren, 12).value = dr("nExt")
                If Not dr("nInt").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 13).value = dr("nInt")
                End If
                oSheet.Cells(ren, 14).value = dr("cp")
                oSheet.Cells(ren, 15).value = dr("col")
                oSheet.Cells(ren, 16).value = dr("loc")
                oSheet.Cells(ren, 17).value = dr("correo")
                oSheet.Cells(ren, 18).value = dr("tel1").ToString
                If Not dr("tel2").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 19).value = dr("tel2")
                End If
                oSheet.Cells(ren, 20).value = FormatCurrency(dr("sumaDepoEfe"), 0)
                oSheet.Cells(ren, 21).value = FormatCurrency(dr("excedente"), 0)
                oSheet.Cells(ren, 22).value = dr("moneda")
                If Not dr("tipoCamb").Equals(DBNull.Value) Then
                    If dr("tipoCamb") <> 0 Then
                        oSheet.Cells(ren, 23).value = dr("tipoCamb")
                    End If
                End If
                If Not dr("porcProp").Equals(DBNull.Value) Then
                    If dr("porcProp") <> 1 Then
                        oSheet.Cells(ren, 24).value = FormatCurrency(dr("porcProp"))
                    End If
                End If

                ren = ren + 1

                Dim q2
                q2 = "SELECT cot.*, con.* FROM cotit2 cot, contrib2 con WHERE cot.idContrib2=con.id and idTitular2=" + dr("idC").ToString + " order by cot.id"
                myCommand2 = New SqlCommand(q2)
                Using dr2 = ExecuteReaderFunction(myCommand2)
                    While dr2.Read()
                        oSheet.Cells(ren, 1).value = "COT"
                        oSheet.Cells(ren, 2).value = dr2("rfc")
                        If Not dr2("apPat").Equals(DBNull.Value) Then
                            oSheet.Cells(ren, 3).value = dr2("apPat")
                        End If
                        If Not dr2("apMat").Equals(DBNull.Value) Then
                            oSheet.Cells(ren, 4).value = dr2("apMat")
                        End If
                        If Not dr2("nom").Equals(DBNull.Value) Then
                            oSheet.Cells(ren, 5).value = dr2("nom")
                        End If
                        If Not dr2("razonSocial").Equals(DBNull.Value) Then
                            oSheet.Cells(ren, 6).value = dr2("razonSocial")
                        End If
                        oSheet.Cells(ren, 7).value = ""
                        If Not dr2("curp").Equals(DBNull.Value) Then
                            oSheet.Cells(ren, 8).value = dr2("curp")
                        End If
                        If Not dr2("numIdFis").Equals(DBNull.Value) Then
                            oSheet.Cells(ren, 9).value = dr2("numIdFis")
                        End If
                        oSheet.Cells(ren, 10).value = getNomEntFed(dr2("idEntFed"))
                        oSheet.Cells(ren, 11).value = dr2("calle")
                        oSheet.Cells(ren, 12).value = dr2("nExt")
                        If Not dr2("nInt").Equals(DBNull.Value) Then
                            oSheet.Cells(ren, 13).value = dr2("nInt")
                        End If
                        oSheet.Cells(ren, 14).value = dr2("cp")
                        oSheet.Cells(ren, 15).value = dr2("col")
                        oSheet.Cells(ren, 16).value = dr2("loc")
                        oSheet.Cells(ren, 17).value = dr2("correo")
                        oSheet.Cells(ren, 18).value = dr2("tel1").ToString
                        If Not dr2("tel2").Equals(DBNull.Value) Then
                            oSheet.Cells(ren, 19).value = dr2("tel2")
                        End If
                        oSheet.Cells(ren, 20).value = ""
                        oSheet.Cells(ren, 21).value = ""
                        oSheet.Cells(ren, 22).value = ""
                        oSheet.Cells(ren, 23).value = ""
                        oSheet.Cells(ren, 24).value = FormatCurrency(dr2("porcProp"))

                        ren = ren + 1
                    End While
                End Using
            End While
        End Using


        q = "SELECT d.*, c.* FROM chq2 d, contrib2 c WHERE d.idContrib2=c.id AND idIdeMens2=" + id.Text + " order by d.id"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            While dr.Read()
                oSheet.Cells(ren, 1).value = "CHQ"
                oSheet.Cells(ren, 2).value = dr("rfc")
                If Not dr("apPat").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 3).value = dr("apPat")
                End If
                If Not dr("apMat").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 4).value = dr("apMat")
                End If
                If Not dr("nom").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 5).value = dr("nom")
                End If
                If Not dr("razonSocial").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 6).value = dr("razonSocial")
                End If
                oSheet.Cells(ren, 7).value = ""
                If Not dr("curp").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 8).value = dr("curp")
                End If
                If Not dr("numIdFis").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 9).value = dr("numIdFis")
                End If
                oSheet.Cells(ren, 10).value = getNomEntFed(dr("idEntFed"))
                oSheet.Cells(ren, 11).value = dr("calle")
                oSheet.Cells(ren, 12).value = dr("nExt")
                If Not dr("nInt").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 13).value = dr("nInt")
                End If
                oSheet.Cells(ren, 14).value = dr("cp")
                oSheet.Cells(ren, 15).value = dr("col")
                oSheet.Cells(ren, 16).value = dr("loc")
                oSheet.Cells(ren, 17).value = dr("correo")
                oSheet.Cells(ren, 18).value = dr("tel1").ToString
                If Not dr("tel2").Equals(DBNull.Value) Then
                    oSheet.Cells(ren, 19).value = dr("tel2")
                End If
                oSheet.Cells(ren, 20).value = ""
                oSheet.Cells(ren, 21).value = ""
                oSheet.Cells(ren, 22).value = dr("moneda")
                If Not dr("tipoCamb").Equals(DBNull.Value) Then
                    If dr("tipoCamb") <> 0 Then
                        oSheet.Cells(ren, 23).value = dr("tipoCamb")
                    End If
                End If
                oSheet.Cells(ren, 24).value = ""
                oSheet.Cells(ren, 25).value = FormatCurrency(dr("montoChqCajaMens"), 0)

                ren = ren + 1
            End While
        End Using

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
        oSheet.Columns("S:S").EntireColumn.AutoFit()
        oSheet.Columns("T:T").EntireColumn.AutoFit()
        oSheet.Columns("U:U").EntireColumn.AutoFit()
        oSheet.Columns("V:V").EntireColumn.AutoFit()
        oSheet.Columns("W:W").EntireColumn.AutoFit()
        oSheet.Columns("X:X").EntireColumn.AutoFit()
        oSheet.Columns("Y:Y").EntireColumn.AutoFit()


        oExcel.Visible = False
        oExcel.UserControl = True
        oExcel.DisplayAlerts = False
        oBook.SaveAs(arch)
        oBook.Close(True)
        oBook = Nothing
        oExcel.Quit()
        oExcel = Nothing

        export.Enabled = True

        Response.Clear()
        Response.ClearHeaders()
        Response.ClearContent()
        Response.AddHeader("content-disposition", "attachment; filename=export" + ejercicio.ToString + "_" + mes.ToString + ".xlsx")
        Response.ContentType = "application/vnd.ms-excel"
        Response.WriteFile(arch)
        Response.End()

        File.Delete(arch)

        Dim MSG As String = "<script language='javascript'>alert('Descargo exitoso hacia su equipo, revise su carpeta de descargas');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
    End Sub

    Protected Sub setStatusDecla_Click(sender As Object, e As EventArgs) Handles setStatusDecla.Click
        myCommand = New SqlCommand("update ideMens2 set idEstatusDecla=" + estado.SelectedValue.ToString + " where id=" + id.Text)
        ExecuteNonQueryFunction(myCommand)
        Dim MSG As String = "<script language='javascript'>alert('ok');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
    End Sub

    Protected Sub setFechas_Click(sender As Object, e As EventArgs) Handles setFechas.Click
        Response.Clear()
        Response.AddHeader("Content-Disposition", "inline")
        Response.ContentType = "text/html"

        Dim dtnow As DateTime
        Dim regDate As New System.Text.RegularExpressions.Regex("^\d{1,2}\/\d{1,2}\/\d{4}$")
        If fPresentada.Text.Trim <> "" Then
            If regDate.IsMatch(fPresentada.Text.Trim) Then
                If Not DateTime.TryParse(fPresentada.Text.Trim, dtnow) Then
                    fPresentada.Focus()
                    lbldescrip.Text = "fecha presentada invalida"
                    Dim MSG As String = "<script language='javascript'>alert('fecha presentada invalida');</script>"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
                    Exit Sub
                End If
            Else
                fPresentada.Focus()
                lbldescrip.Text = "fecha presentada formato de fecha no valido (dd/mm/aaaa)"
                Dim MSG As String = "<script language='javascript'>alert('fecha presentada formato de fecha no valido (dd/mm/aaaa)');</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
                Exit Sub
            End If
        End If

        If fDescargada.Text.Trim <> "" Then
            If regDate.IsMatch(fDescargada.Text.Trim) Then
                If Not DateTime.TryParse(fDescargada.Text.Trim, dtnow) Then
                    fDescargada.Focus()
                    lbldescrip.Text = "fecha descargada invalida"
                    Dim MSG As String = "<script language='javascript'>alert('fecha descargada invalida');</script>"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
                    Exit Sub
                End If
            Else
                fDescargada.Focus()
                lbldescrip.Text = "fecha descargada formato de fecha no valido (dd/mm/aaaa)"
                Dim MSG As String = "<script language='javascript'>alert('fecha descargada formato de fecha no valido (dd/mm/aaaa)');</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
                Exit Sub
            End If
        End If

        If fCita.Text.Trim <> "" Then
            If regDate.IsMatch(fCita.Text.Trim) Then
                If Not DateTime.TryParse(fCita.Text.Trim, dtnow) Then
                    fCita.Focus()
                    lbldescrip.Text = "fecha Cita invalida"
                    Dim MSG As String = "<script language='javascript'>alert('fecha Cita invalida');</script>"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
                    Exit Sub
                End If
            Else
                fCita.Focus()
                lbldescrip.Text = "fecha Cita formato de fecha no valido (dd/mm/aaaa)"
                Dim MSG As String = "<script language='javascript'>alert('fecha Cita formato de fecha no valido (dd/mm/aaaa)');</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
                Exit Sub
            End If
        End If


        Dim fPresentadav, fCitav
        If fPresentada.Text.Trim = "" Then
            fPresentadav = DBNull.Value
        Else
            fPresentadav = CDate(fPresentada.Text).ToString("yyyy-MM-dd")
        End If

        If fCita.Text.Trim = "" Then
            fCitav = DBNull.Value
        Else
            fCitav = CDate(fCita.Text).ToString("yyyy-MM-dd")
        End If

        myCommand = New SqlCommand("update ideMens2 set fPresentada=@fPresentada, fCita=@fCita, hrCita='" + hrs.SelectedValue.ToString + "', minCita='" + mins.SelectedValue.ToString + "'  where id=" + id.Text)
        myCommand.Parameters.AddWithValue("@fPresentada", fPresentadav)
        myCommand.Parameters.AddWithValue("@fCita", fCitav)
        ExecuteNonQueryFunction(myCommand)

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", "<script language='javascript'>alert('Fechas guardadas');</script>", False)
    End Sub

    Private Function returnID(ByVal tabla As String, ByVal param1 As String, Optional ByVal param2 As Long = 0, Optional ByVal param3 As String = "") As Long
        Dim consulta
        If tabla = "estatusDecla2" Then
            consulta = "SELECT id FROM estatusDecla2 WHERE estatus='" + param1 + "'"
        ElseIf tabla = "contrib2" Then
            consulta = "SELECT id FROM contrib2 WHERE rfc='" + param1 + "'"
        ElseIf tabla = "entFed2" Then
            consulta = "SELECT id FROM entFed2 WHERE descr='" + param1 + "'"
        ElseIf tabla = "ideMens2" Then
            consulta = "SELECT TOP 1 id FROM idMens2 WHERE ejercicio='" + param1 + "' and mes=" + param2.ToString + " order by id desc"
        End If

        Dim retorno = -1
        Dim myCommandE = New SqlCommand(consulta)
        Using drE = ExecuteReaderFunction(myCommandE)
            If drE.HasRows Then
                drE.Read()
                retorno = drE("id")
            Else
                retorno = -1
            End If
        End Using
        Return retorno
    End Function

    Private Function returnUltID(ByVal tabla As String) As Long
        Dim myCommandE = New SqlCommand("SELECT TOP 1 id FROM " + tabla + " ORDER BY id DESC")
        Dim retorno
        Using drE = ExecuteReaderFunction(myCommandE)
            drE.Read()
            retorno = drE("id")
        End Using
        Return retorno
    End Function

    Private Function getNomEntFed(ByVal ident As Integer) As String
        Dim myCommandE = New SqlCommand("SELECT descr FROM entFed2 WHERE id=" + ident.ToString)
        Dim retorno
        Using drE = ExecuteReaderFunction(myCommandE)
            drE.Read()
            retorno = drE("descr")
        End Using
        Return retorno
    End Function

    Private Function getClaveEntFed(ByVal ident As Integer) As String
        Dim myCommandE = New SqlCommand("SELECT clave FROM entFed2 WHERE id=" + ident.ToString)
        Dim retorno
        Using drE = ExecuteReaderFunction(myCommandE)
            drE.Read()
            retorno = drE("clave")
        End Using
        Return retorno
    End Function

    Protected Sub TreeView1_SelectedNodeChanged(sender As Object, e As EventArgs) Handles TreeView1.SelectedNodeChanged


    End Sub

    Protected Sub declaValidar_Click(sender As Object, e As EventArgs) Handles descargaLocal.Click
        nomArchAnualDatos.Value = "C:\SAT\" + Session("casfim") + "\IDE_" + Session("curCorreo") + "_" + ejercicio.ToString + "_" + mes.ToString + ".txt"
        If Not File.Exists(nomArchAnualDatos.Value) Then
            Dim MSG = "<script language='javascript'>alert('aun no ha creado la declaracion');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Exit Sub
        End If
        Dim file1 As System.IO.FileInfo = New System.IO.FileInfo(nomArchAnualDatos.Value)
        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" + file1.Name)
        Response.AddHeader("Content-Length", file1.Length.ToString())
        Response.ContentType = "application/octet-stream"
        Response.WriteFile(file1.FullName)
        Response.End()

        myCommand = New SqlCommand("update ideMens2 set fDescargada='" + CDate(fDescargada.Text).ToString("yyyy-MM-dd") + "' where id=" + id.Text)
        ExecuteNonQueryFunction(myCommand)

        Dim myCommandE = New SqlCommand("SELECT orden from estatusDecla2 where id in (select idEstatusDecla FROM ideMens2 WHERE id=" + id.ToString + ")")
        Dim orden
        Using drE = ExecuteReaderFunction(myCommandE)
            drE.Read()
            orden = drE("orden")
        End Using

        Dim elEstatus = returnID("estatusDecla2", "archivo descargado")

        If orden < 4 Then '< arch descargado
            myCommand = New SqlCommand("update ideMens2 set idEstatusDecla=" + elEstatus.ToString + " where id=" + id.Text)
            ExecuteNonQueryFunction(myCommand)
        End If

        Response.Clear()
        Response.AddHeader("Content-Disposition", "inline")
        Response.ContentType = "text/html"
    End Sub

    Protected Sub setValidador_Click(sender As Object, e As EventArgs) Handles acuseSet.Click
        nomArchAnualDatos.Value = "C:\SAT\" + Session("casfim") + "\acuse" + ejercicio.ToString + "_" + mes.ToString
        Dim MSG
        myCommand = New SqlCommand("update ideMens2 set tieneAcuse=" + IIf(chkAcuse.Checked, "1", "0") + " where id=" + id.Text)
        ExecuteNonQueryFunction(myCommand)
        Dim archDest
        If FileUploadAcuse.HasFile And chkAcuse.Checked.Equals(True) Then
            Dim fileSize As Integer = FileUploadAcuse.PostedFile.ContentLength
            Dim fileName As String = Server.HtmlEncode(FileUploadAcuse.FileName)
            Dim extension As String = System.IO.Path.GetExtension(fileName)
            archDest = nomArchAnualDatos.Value + "." + extension
            If File.Exists(archDest) Then
                File.Delete(archDest)
            End If
            If (Not System.IO.Directory.Exists("C:\SAT\" + Session("casfim"))) Then
                System.IO.Directory.CreateDirectory("C:\SAT\" + Session("casfim"))
            End If
            Try
                FileUploadAcuse.SaveAs(archDest)
                chkAcuse.Checked = True

            Catch ex As Exception
                MSG = "<script language='javascript'>alert('" + ex.Message + "');</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Finally
                FileUploadAcuse.PostedFile.InputStream.Flush()
                FileUploadAcuse.PostedFile.InputStream.Close()
                FileUploadAcuse.FileContent.Dispose()
                FileUploadAcuse.Dispose()
            End Try
        End If


        MSG = "<script language='javascript'>alert('guardado');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
    End Sub

    Protected Sub descargarAcuse_Click(sender As Object, e As EventArgs) Handles descargarAcuse.Click
        '****************checar acuse sea tipo html
        nomArchAnualDatos.Value = "C:\SAT\" + Session("casfim") + "\acuse" + ejercicio.ToString + "_" + mes.ToString + ".html"
        If Not File.Exists(nomArchAnualDatos.Value) Then
            Dim MSG = "<script language='javascript'>alert('aun no ha creado la declaracion');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Exit Sub
        End If
        Dim file1 As System.IO.FileInfo = New System.IO.FileInfo(nomArchAnualDatos.Value)
        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" + file1.Name)
        Response.AddHeader("Content-Length", file1.Length.ToString())
        Response.ContentType = "application/octet-stream"
        Response.WriteFile(file1.FullName)
        Response.End()

        Response.Clear()
        Response.AddHeader("Content-Disposition", "inline")
        Response.ContentType = "text/html"
    End Sub

    Protected Sub saveTipoEnvio_Click(sender As Object, e As EventArgs) Handles saveTipoEnvio.Click
        Dim MSG
        If tipoEnvio.SelectedValue = "op" Then
            MSG = "<script language='javascript'>alert('Escoja una opción válida');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Exit Sub
        End If

        myCommand = New SqlCommand("update ideMens2 set metodoPresentac='" + tipoEnvio.SelectedValue + "' where id=" + id.Text)
        ExecuteNonQueryFunction(myCommand)
        MSG = "<script language='javascript'>alert('Guardado');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
    End Sub

    Protected Sub cita_Click(sender As Object, e As EventArgs) Handles cita.Click
        Dim MSG
        If fCita.Text.Trim = "" Or hrs.Text.Trim = "-" Or mins.Text.Trim = "-" Then
            MSG = "<script language='javascript'>alert('Indique la fecha, horas y minutos para solicitar la cita x correo');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Exit Sub
        End If

        Dim elcorreo As New System.Net.Mail.MailMessage
        Using elcorreo
            'elcorreo.To.Add("declaracioneside@gmail.com")
            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
            elcorreo.To.Add("declaracioneside@gmail.com")
            elcorreo.Subject = "IDE: confirmar o modificar solicitud de cita para " + Session("curCorreo") + ", ejercicio " + ejercicio.ToString + " mes " + mes.ToString
            elcorreo.Body = "<html><body>Buen dia<br><br>Fecha " + fCita.Text + ", a las " + hrs.SelectedValue.ToString + ":" + mins.SelectedValue.ToString + " <br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet<br>Tel 443 690 3616<br>Correo declaracioneside@gmail.com<br><a href='https://twitter.com/declaracionesid' target='_blank'><img src='declaracioneside.com/twitter.jpg' alt='Clic aquí, siguenos en twitter' Height='30px' Width='30px' BorderWidth ='0px'></a>&nbsp;<a href='http://www.youtube.com/user/declaracioneside' target='_blank'> <img src='declaracioneside.com/iconoyoutube.png'  alt='Suscribete a nuestro canal declaraciones de depósitos en efectivo e IDE en youtube' Height='30px' Width='30px' BorderWidth ='0px'></a> &nbsp;<a href='http://www.facebook.com/depositosenefectivo' target='_blank'><img src='declaracioneside.com/facebook.jpg' alt='Clic aquí para seguirnos en facebook' Height='30px' Width='30px' BorderWidth ='0px'></a>&nbsp;&nbsp;<a href='https://mx.linkedin.com/in/declaraciones-depósitos-en-efectivo-1110125b' target='_blank'><img src='declaracioneside.com/linkedin.png' alt='Siguenos en linkedin' Height='30px' Width='30px' BorderWidth ='0px'></a>&nbsp;<a href='http://plus.google.com/107594546767340388428?prsrc=3'><img src='http://ssl.gstatic.com/images/icons/gplus-32.png' alt='Google+' Height='30px' Width='30px' BorderWidth ='0px'></a><br /> </body></html>"
            elcorreo.IsBodyHtml = True
            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
            'elcorreo.Attachments.Add(New System.Net.Mail.Attachment(nomArchAnualDatos.Value))
            'elcorreo.Attachments.Add(New System.Net.Mail.Attachment("C:\SAT\instructivoCargarDeclaracion.pdf"))
            Dim smpt As New System.Net.Mail.SmtpClient
            smpt.Host = "smtp.gmail.com"
            smpt.Port = "587"
            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
            smpt.EnableSsl = True 'req p server gmail
            Try
                smpt.Send(elcorreo)
                elcorreo.Dispose()

                lbldescrip.Text = "Se envio solicitud por correo al proveedor, espere confirmacion o propuesta de otra fecha/hora por el mismo medio"
                MSG = "<script language='javascript'>alert('Se envio solicitud por correo al proveedor, espere confirmacion o propuesta de otra fecha/hora por el mismo medio');</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Catch ex As Exception
                lbldescrip.Text = "Error enviando solicitud por correo al proveedor: " & ex.Message
                MSG = "<script language='javascript'>alert('" + lbldescrip.Text + "');</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
                Exit Sub
            Finally
                'File.Delete(Left(nomArchMens, Len(nomArchMens) - 4) + ".ZIP")
            End Try
        End Using

        Dim elEstatus = returnID("estatusDecla2", "agendado para declarar")
        myCommand = New SqlCommand("update ideMens2 set idEstatusDecla=" + elEstatus.ToString + " where id=" + id.Text)
        ExecuteNonQueryFunction(myCommand)

        MSG = "<script language='javascript'>alert('Se ha enviado correo al proveedor solicitando la cita, espere confirmacion o propuesta de otra fecha/hora por correo');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
    End Sub

    Protected Sub instructivo_Click(sender As Object, e As EventArgs) Handles instructivo.Click
        nomArchAnualDatos.Value = "C:\SAT\instructivoCargarDeclaracion.pdf"
        If Not File.Exists(nomArchAnualDatos.Value) Then
            Dim MSG = "<script language='javascript'>alert('no se encontro');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Exit Sub
        End If
        Dim file1 As System.IO.FileInfo = New System.IO.FileInfo(nomArchAnualDatos.Value)
        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" + file1.Name)
        Response.AddHeader("Content-Length", file1.Length.ToString())
        Response.ContentType = "application/octet-stream"
        Response.WriteFile(file1.FullName)
        Response.End()

        Response.Clear()
        Response.AddHeader("Content-Disposition", "inline")
        Response.ContentType = "text/html"
    End Sub

    Private Sub Crear_Click(sender As Object, e As EventArgs) Handles Crear.Click

    End Sub
End Class