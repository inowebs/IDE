Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Win32
Imports System.Net.Mail
Imports System.Web.Mail
Imports Microsoft.Office.Interop
Imports System.IO
Imports System
Imports System.Text
Imports System.Security.AccessControl
Imports System.Security
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Xml
Imports Ionic.Zip
Imports System.Diagnostics
Imports Microsoft.Office.Interop.Excel
Imports Microsoft.Office.Interop.Word

Public Class WebForm4
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
    Dim dr As SqlDataReader

    Dim idParam As Integer
    Dim PKnombreCompleto
    Dim bkCasfim

    Dim dataSet As DataSet
    Dim cerrarSesion

    Private Sub cuentaRegistros()
        Dim q
        q = "SELECT COUNT(*) as cuenta FROM reprLegal where idCliente=" + idParam.ToString
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        reprLegNregs.Text = FormatNumber(v.ToString, 0) + " Registros"
        GridView2.SelectedIndex = -1
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("curCorreo")) = True Then
            Response.Write("<script language='javascript'>alert('Expiró su sesión');</script>")
            Session.Abandon()
            Response.Write("<script language='javascript'>location.href='Login.aspx';</script>")
            Exit Sub
        End If

        'myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';")
        'myConnection.Open()

        myCommand = New SqlCommand("set dateformat ymd")
        ExecuteNonQueryFunction(myCommand)



        'If Session("runAsAdmin") = "1" And session("curCorreo").ToString.ToUpper <> "PRUEBASDEIDE@GMAIL.COM" Then
        '    Dim item As System.Web.UI.WebControls.MenuItem = NavigationMenu.FindItem("Mis contratos")
        '    item.Parent.ChildItems.Remove(item)
        '    item = NavigationMenu.FindItem("Mis declaraciones")
        '    item.Parent.ChildItems.Remove(item)
        '    item = NavigationMenu.FindItem("Declarar")
        '    item.Parent.ChildItems.Remove(item)
        'End If

        Page.ClientScript.RegisterStartupScript(GetType(Microsoft.Office.Interop.Excel.Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + divScroll.ClientID + "');", True)

        If Not IsPostBack Then  '1a vez
            If Session("curCorreo") = "" Or IsNothing(Session("curCorreo")) = True Then
                Response.Redirect("Login.aspx")
            End If
            If HttpContext.Current.Request.IsLocal Or Session("runAsAdmin") = "1" Then
                idDistribuidor.Enabled = True
                facRetens.Visible = True
                clientesEstatus.Visible = True
                fielUp.Enabled = True
                fielBajar.Visible = True
            Else
                idDistribuidor.Enabled = False
                facRetens.Visible = False
                clientesEstatus.Visible = False
                fielUp.Enabled = False
                fielBajar.Visible = False
            End If
            frame1.Attributes("src") = "declaAyu22.aspx"
        End If

        Dim q
        q = "SELECT id FROM clientes WHERE correo='" + Session("curCorreo") + "'"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            idParam = v.ToString()
        End If

        If Not IsPostBack Then  '1a vez
            cargaDatos(idParam) 'id
            cerrarSesion = 0
        Else    'refresh after press butons
        End If

        If HttpContext.Current.Request.IsLocal Or Session("runAsAdmin") = "1" Then
            Panel1.Visible = True
        Else
            Panel1.Visible = False
        End If

        'SqlDataSource2.ConnectionString = "$ ConnectionStrings:ideConnectionString "
        SqlDataSource2.SelectCommand = "SELECT rl.id, rl.nombres, rl.ap1, rl.ap2, rl.rfc, rl.curp FROM reprLegal rl, clientes cli WHERE rl.idCliente=cli.id and cli.correo='" + Session("curCorreo") + "' ORDER BY rl.id"
        GridView2.DataBind()
        cuentaRegistros()

        directorioServidor.Enabled = False

        If IsPostBack Then
            If Not String.IsNullOrEmpty(passWeb.Text.Trim()) Then
                passWeb.Attributes.Add("value", passWeb.Text)
            End If
            If Not String.IsNullOrEmpty(passWeb2.Text.Trim()) Then
                passWeb2.Attributes.Add("value", passWeb2.Text)
            End If
        End If


    End Sub

    Private Sub cargaDatos(ByVal id)
        'carga x id
        Dim q, estatuscli

        myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';MultipleActiveResultSets=True")
        myConnection.Open()
        'Encriptacion
        myCommand = New SqlCommand("OPEN SYMMETRIC KEY SYM_KEY DECRYPTION BY PASSWORD ='##Djjcp##'", myConnection)
        myCommand.ExecuteNonQuery()
        'ExecuteNonQueryFunction(myCommand)
        q = "SELECT *, CAST(DECRYPTBYKEY(passWeb) AS VARCHAR(15)) as CripPassWeb FROM clientes WHERE id=" + id.ToString
        myCommand = New SqlCommand(q, myConnection)
        Dim dr2 = myCommand.ExecuteReader()
        If dr2.Read() Then
            correo.Text = dr2("correo").ToString()
            'passWeb.Text = dr2("passWeb").ToString()
            passWeb.Attributes.Add("value", dr2("CripPassWeb").ToString())
            passWeb2.Attributes.Add("value", dr2("CripPassWeb").ToString())
            Session("pkPass") = dr2("CripPassWeb").ToString()
            loginSAT.Text = dr2("loginSAT").ToString()
            directorioServidor.Text = dr2("directorioServidor").ToString()
            razonSoc.Text = dr2("razonSoc").ToString()
            contacto.Text = dr2("contacto").ToString()
            puesto.Text = dr2("puesto").ToString()
            tel.Text = dr2("tel").ToString()
            cel.Text = dr2("cel").ToString()
            paginaWeb.Text = dr2("paginaWeb").ToString()
            rfcDeclarante.Text = dr2("rfcDeclarante").ToString()
            domFiscal.Text = dr2("domFiscal").ToString()
            numSucursales.Text = FormatNumber(dr2("numSucursales"), 0, , , )
            numSociosClientes.Text = FormatNumber(dr2("numSociosClientes"), 0, , , )
            directorioSat.Text = dr2("directorioSat").ToString()
            casfim.Text = dr2("casfim").ToString()
            If dr2("casfimProvisional").Equals(False) Then
                casfimProvisional.Checked = False
            Else
                casfimProvisional.Checked = True
            End If
            fechaSolSocketSat.Text = Left(dr2("fechaSolSocketSat").ToString(), 10)
            fechaPrueba.Text = Left(dr2("fechaPrueba").ToString(), 10)
            fechaRegistro.Text = Left(dr2("fechaRegistro").ToString(), 10)
            ipSat.Text = dr2("ipSat").ToString()
            If dr2("esInstitCredito").Equals(False) Then
                esInstitCredito.Checked = False
            Else
                esInstitCredito.Checked = True
            End If
            solSocketEstatus.Text = dr2("solSocketEstatus")
            If dr2("rfcComodinPm").Equals(False) Then
                rfcComodinPm.Checked = False
            Else
                rfcComodinPm.Checked = True
            End If
            If dr2("nombreFull").Equals(False) Then
                chkNombreFull.Checked = False
            Else
                chkNombreFull.Checked = True
            End If
            idDistribuidor.Text = dr2("idDistribuidor").ToString
            estatuscli = dr2("idEstatus")
            If estatuscli < 1 Then
                clientesEstatus.SelectedValue = 27
            Else
                clientesEstatus.SelectedValue = dr2("idEstatus")
            End If

            If dr2("transmisionOk").Equals(False) Then
                transmisionOk.Checked = False
            Else
                transmisionOk.Checked = True
            End If
            If dr2("recepcionOk").Equals(False) Then
                recepcionOk.Checked = False
            Else
                recepcionOk.Checked = True
            End If
            If dr2("contactadoPguiarDecl").Equals(False) Then
                contactadoPguiarDecl.Checked = False
            Else
                contactadoPguiarDecl.Checked = True
            End If
            If dr2("solSockConfirmadaSAT").Equals(False) Then
                solSockConfirmadaSAT.Checked = False
            Else
                solSockConfirmadaSAT.Checked = True
            End If
            If Not DBNull.Value.Equals(dr2("facTercero")) Then
                If dr2("facTercero").Equals(False) Then
                    facTercero.Checked = False
                    facPanel.Visible = False
                Else
                    facTercero.Checked = True
                    facPanel.Visible = True
                    If Not DBNull.Value.Equals(dr2("facRfc")) Then
                        facRfc.Text = dr2("facRfc")
                    End If
                    If Not DBNull.Value.Equals(dr2("facRazon")) Then
                        facRazon.Text = dr2("facRazon")
                    End If
                    If Not DBNull.Value.Equals(dr2("facUso")) Then
                        facUso.SelectedValue = dr2("facUso")
                    End If
                    'If Not DBNull.Value.Equals(dr2("facFP")) Then
                    '    facFP.SelectedValue = dr2("facFP")
                    'End If
                End If
            Else
                facTercero.Checked = False
                facPanel.Visible = False
            End If
            If DBNull.Value.Equals(dr2("remoto")) Then
                remoto.SelectedIndex = 0
            Else
                remoto.SelectedValue = dr2("remoto")
            End If
            If DBNull.Value.Equals(dr2("formaPresentacion")) Then
                formaPresentacion.SelectedIndex = 0
            Else
                formaPresentacion.SelectedValue = dr2("formaPresentacion")
            End If
            formaPresentacionChange()
            If Not DBNull.Value.Equals(dr2("rutaFiel")) Then
                rutaFiel.Text = dr2("rutaFiel")
            Else
                rutaFiel.Text = ""
            End If
            If Not DBNull.Value.Equals(dr2("whats")) Then
                whats.Text = dr2("whats")
            Else
                whats.Text = ""
            End If
            If Not DBNull.Value.Equals(dr2("idNumRemoto")) Then
                idNumRemoto.Text = dr2("idNumRemoto")
            Else
                idNumRemoto.Text = ""
            End If
            If Not DBNull.Value.Equals(dr2("passRemoto")) Then
                passRemoto.Text = dr2("passRemoto")
            Else
                passRemoto.Text = ""
            End If
            If Not DBNull.Value.Equals(dr2("fuente")) Then
                fuente.Text = dr2("fuente")
            End If
            If Not DBNull.Value.Equals(dr2("dxFac")) Then
                dxFac.Text = dr2("dxFac")
            End If
            If Not DBNull.Value.Equals(dr2("facCorreos")) Then
                facCorreos.Text = dr2("facCorreos")
            End If
            If Not DBNull.Value.Equals(dr2("facRetens")) Then
                facRetens.Checked = dr2("facRetens")
            Else
                facRetens.Checked = 0
            End If
            If Not DBNull.Value.Equals(dr2("fielUp")) Then
                fielUp.Checked = dr2("fielUp")
            Else
                fielUp.Checked = 0
            End If
            If Not DBNull.Value.Equals(dr2("otrosCorreos")) Then
                otrosCorreos.Text = dr2("otrosCorreos")
            End If
        End If
        dr2.Close()

        myCommand = New SqlCommand("CLOSE SYMMETRIC KEY SYM_KEY", myConnection)
        myCommand.ExecuteNonQuery()

        myConnection.Close()

        If estatuscli = 27 Then
            myCommand = New SqlCommand("update clientes set idEstatus=27 where correo='" + Session("curCorreo") + "'")
            ExecuteNonQueryFunction(myCommand)
        End If

        q = "SELECT id FROM reprLegal WHERE idCliente=" + id.ToString + " AND esActual=1"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            actualRepr.Text = v.ToString()
        End If
        cuentaRegistros()

    End Sub

    Function IsValidEmail(ByVal strIn As String) As Boolean
        ' Return true if strIn is in valid e-mail format.
        Return Regex.IsMatch(strIn, ("^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
    End Function

    Private Function validaVacios() As Integer
        If Trim(correo.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el correo');</script>")
            correo.Focus()
            Return 0
        End If
        If IsValidEmail(Trim(correo.Text)) = False Then
            Response.Write("<script language='javascript'>alert('Formato de correo incorrecto');</script>")
            correo.Focus()
            Return 0
        End If

        If Trim(razonSoc.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique la razon social');</script>")
            razonSoc.Focus()
            Return 0
        End If
        If Trim(rfcDeclarante.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el RFC');</script>")
            rfcDeclarante.Focus()
            Return 0
        End If
        If Len(Trim(rfcDeclarante.Text)) < 9 Or Len(Trim(rfcDeclarante.Text)) > 12 Then
            Response.Write("<script language='javascript'>alert('El tamaño del rfc debe estar entre 9-12 caracteres');</script>")
            rfcDeclarante.Focus()
            Return 0
        End If
        If Not Regex.IsMatch(rfcDeclarante.Text.ToUpper.Trim, "^([A-Z\s]{3})\d{6}([A-Z\w]{3})$") Then
            Response.Write("<script language='javascript'>alert('Formato de rfc invalido');</script>")
            rfcDeclarante.Focus()
            Return 0
        End If
        If Trim(passWeb.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el password');</script>")
            passWeb.Focus()
            Return 0
        End If
        If Trim(passWeb.Text).Length < 6 Then
            Response.Write("<script language='javascript'>alert('Longitud minima de password de 6 caracteres');</script>")
            passWeb.Focus()
            Return 0
        End If
        If passWeb.Text.Trim <> passWeb2.Text.Trim Then
            Response.Write("<script language='javascript'>alert('El password y su confirmación no coinciden');</script>")
            passWeb.Focus()
            Return 0
        End If
        If Trim(contacto.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el contacto');</script>")
            contacto.Focus()
            Return 0
        End If
        If Trim(puesto.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el puesto');</script>")
            puesto.Focus()
            Return 0
        End If
        If Trim(tel.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el teléfono');</script>")
            tel.Focus()
            Return 0
        End If
        If Trim(domFiscal.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el domicilio fiscal');</script>")
            domFiscal.Focus()
            Return 0
        End If
        If Trim(casfim.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique la clave CASFIM');</script>")
            casfim.Focus()
            Return 0
        End If
        If Trim(fuente.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique cómo se enteró de nosotros');</script>")
            fuente.Focus()
            Return 0
        End If
        If facTercero.Checked Then
            If Trim(facRfc.Text) = "" Then
                Response.Write("<script language='javascript'>alert('Especifique RFC para facturar');</script>")
                facRfc.Focus()
                Return 0
            Else
                If Len(Trim(facRfc.Text)) < 12 Or Len(Trim(facRfc.Text)) > 13 Then
                    Response.Write("<script language='javascript'>alert('El tamaño del rfc de facturacion debe estar entre 12 y 13 caracteres');</script>")
                    facRfc.Focus()
                    Return 0
                End If
            End If
            If Trim(facRazon.Text) = "" Then
                Response.Write("<script language='javascript'>alert('Especifique razon social para facturar');</script>")
                facRazon.Focus()
                Return 0
            End If
        End If

        Return 1
    End Function

    Private Function validaInyeccion() As Integer

        If InStr(correo.Text.ToUpper, "SELECT ") > 0 Or InStr(correo.Text.ToUpper, "INSERT ") > 0 Or InStr(correo.Text.ToUpper, "UPDATE ") > 0 Or InStr(correo.Text.ToUpper, "DELETE ") > 0 Or InStr(correo.Text.ToUpper, "DROP  ") > 0 Or InStr(correo.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en correo');</script>")
            Return 0
        End If
        If InStr(razonSoc.Text.ToUpper, "SELECT ") > 0 Or InStr(razonSoc.Text.ToUpper, "INSERT ") > 0 Or InStr(razonSoc.Text.ToUpper, "UPDATE ") > 0 Or InStr(razonSoc.Text.ToUpper, "DELETE ") > 0 Or InStr(razonSoc.Text.ToUpper, "DROP ") > 0 Or InStr(razonSoc.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en razonsocial');</script>")
            Return 0
        End If
        If InStr(contacto.Text.ToUpper, "SELECT ") > 0 Or InStr(contacto.Text.ToUpper, "INSERT ") > 0 Or InStr(contacto.Text.ToUpper, "UPDATE ") > 0 Or InStr(contacto.Text.ToUpper, "DELETE ") > 0 Or InStr(contacto.Text.ToUpper, "DROP ") > 0 Or InStr(contacto.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en contacto');</script>")
            Return 0
        End If
        If InStr(puesto.Text.ToUpper, "SELECT ") > 0 Or InStr(puesto.Text.ToUpper, "INSERT ") > 0 Or InStr(puesto.Text.ToUpper, "UPDATE ") > 0 Or InStr(puesto.Text.ToUpper, "DELETE ") > 0 Or InStr(puesto.Text.ToUpper, "DROP ") > 0 Or InStr(puesto.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en puesto');</script>")
            Return 0
        End If
        If InStr(tel.Text.ToUpper, "SELECT ") > 0 Or InStr(tel.Text.ToUpper, "INSERT ") > 0 Or InStr(tel.Text.ToUpper, "UPDATE ") > 0 Or InStr(tel.Text.ToUpper, "DELETE ") > 0 Or InStr(tel.Text.ToUpper, "DROP ") > 0 Or InStr(tel.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en tel');</script>")
            Return 0
        End If
        If InStr(cel.Text.ToUpper, "SELECT ") > 0 Or InStr(cel.Text.ToUpper, "INSERT ") > 0 Or InStr(cel.Text.ToUpper, "UPDATE ") > 0 Or InStr(cel.Text.ToUpper, "DELETE ") > 0 Or InStr(cel.Text.ToUpper, "DROP ") > 0 Or InStr(cel.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en cel');</script>")
            Return 0
        End If
        If InStr(paginaWeb.Text.ToUpper, "SELECT ") > 0 Or InStr(paginaWeb.Text.ToUpper, "INSERT ") > 0 Or InStr(paginaWeb.Text.ToUpper, "UPDATE ") > 0 Or InStr(paginaWeb.Text.ToUpper, "DELETE ") > 0 Or InStr(paginaWeb.Text.ToUpper, "DROP ") > 0 Or InStr(paginaWeb.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en pagina');</script>")
            Return 0
        End If
        If InStr(rfcDeclarante.Text.ToUpper, "SELECT ") > 0 Or InStr(rfcDeclarante.Text.ToUpper, "INSERT ") > 0 Or InStr(rfcDeclarante.Text.ToUpper, "UPDATE ") > 0 Or InStr(rfcDeclarante.Text.ToUpper, "DELETE ") > 0 Or InStr(rfcDeclarante.Text.ToUpper, "DROP ") > 0 Or InStr(rfcDeclarante.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en rfc');</script>")
            Return 0
        End If
        If InStr(domFiscal.Text.ToUpper, "SELECT ") > 0 Or InStr(domFiscal.Text.ToUpper, "INSERT ") > 0 Or InStr(domFiscal.Text.ToUpper, "UPDATE ") > 0 Or InStr(domFiscal.Text.ToUpper, "DELETE ") > 0 Or InStr(domFiscal.Text.ToUpper, "DROP ") > 0 Or InStr(domFiscal.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en domicilio');</script>")
            Return 0
        End If
        If InStr(numSucursales.Text.ToUpper, "SELECT ") > 0 Or InStr(numSucursales.Text.ToUpper, "INSERT ") > 0 Or InStr(numSucursales.Text.ToUpper, "UPDATE ") > 0 Or InStr(numSucursales.Text.ToUpper, "DELETE ") > 0 Or InStr(numSucursales.Text.ToUpper, "DROP ") > 0 Or InStr(numSucursales.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en num suc');</script>")
            Return 0
        End If
        If InStr(numSociosClientes.Text.ToUpper, "SELECT ") > 0 Or InStr(numSociosClientes.Text.ToUpper, "INSERT ") > 0 Or InStr(numSociosClientes.Text.ToUpper, "UPDATE ") > 0 Or InStr(numSociosClientes.Text.ToUpper, "DELETE ") > 0 Or InStr(numSociosClientes.Text.ToUpper, "DROP ") > 0 Or InStr(numSociosClientes.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en num socios');</script>")
            Return 0
        End If
        If InStr(casfim.Text.ToUpper, "SELECT ") > 0 Or InStr(casfim.Text.ToUpper, "INSERT ") > 0 Or InStr(casfim.Text.ToUpper, "UPDATE ") > 0 Or InStr(casfim.Text.ToUpper, "DELETE ") > 0 Or InStr(casfim.Text.ToUpper, "DROP ") > 0 Or InStr(casfim.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en casfim');</script>")
            Return 0
        End If
        If InStr(idDistribuidor.Text.ToUpper, "SELECT ") > 0 Or InStr(idDistribuidor.Text.ToUpper, "INSERT ") > 0 Or InStr(idDistribuidor.Text.ToUpper, "UPDATE ") > 0 Or InStr(idDistribuidor.Text.ToUpper, "DELETE ") > 0 Or InStr(idDistribuidor.Text.ToUpper, "DROP ") > 0 Or InStr(idDistribuidor.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en idDistribuidor');</script>")
            Return 0
        End If
        If InStr(clientesEstatus.Text.ToUpper, "SELECT ") > 0 Or InStr(clientesEstatus.Text.ToUpper, "INSERT ") > 0 Or InStr(clientesEstatus.Text.ToUpper, "UPDATE ") > 0 Or InStr(clientesEstatus.Text.ToUpper, "DELETE ") > 0 Or InStr(clientesEstatus.Text.ToUpper, "DROP ") > 0 Or InStr(clientesEstatus.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en estatus cliente');</script>")
            Return 0
        End If
        If InStr(fuente.Text.ToUpper, "SELECT ") > 0 Or InStr(fuente.Text.ToUpper, "INSERT ") > 0 Or InStr(fuente.Text.ToUpper, "UPDATE ") > 0 Or InStr(fuente.Text.ToUpper, "DELETE ") > 0 Or InStr(fuente.Text.ToUpper, "DROP ") > 0 Or InStr(fuente.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en fuente');</script>")
            Return 0
        End If
        If InStr(dxFac.Text.ToUpper, "SELECT ") > 0 Or InStr(dxFac.Text.ToUpper, "INSERT ") > 0 Or InStr(dxFac.Text.ToUpper, "UPDATE ") > 0 Or InStr(dxFac.Text.ToUpper, "DELETE ") > 0 Or InStr(dxFac.Text.ToUpper, "DROP ") > 0 Or InStr(dxFac.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en datos de facturacion');</script>")
            Return 0
        End If
        If InStr(facCorreos.Text.ToUpper, "SELECT ") > 0 Or InStr(facCorreos.Text.ToUpper, "INSERT ") > 0 Or InStr(facCorreos.Text.ToUpper, "UPDATE ") > 0 Or InStr(facCorreos.Text.ToUpper, "DELETE ") > 0 Or InStr(facCorreos.Text.ToUpper, "DROP ") > 0 Or InStr(facCorreos.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en correos de facturacion');</script>")
            Return 0
        End If
        If InStr(facRazon.Text.ToUpper, "SELECT ") > 0 Or InStr(facRazon.Text.ToUpper, "INSERT ") > 0 Or InStr(facRazon.Text.ToUpper, "UPDATE ") > 0 Or InStr(facRazon.Text.ToUpper, "DELETE ") > 0 Or InStr(facRazon.Text.ToUpper, "DROP ") > 0 Or InStr(facRazon.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en razon de facturacion');</script>")
            Return 0
        End If
        If InStr(facRfc.Text.ToUpper, "SELECT ") > 0 Or InStr(facRfc.Text.ToUpper, "INSERT ") > 0 Or InStr(facRfc.Text.ToUpper, "UPDATE ") > 0 Or InStr(facRfc.Text.ToUpper, "DELETE ") > 0 Or InStr(facRfc.Text.ToUpper, "DROP ") > 0 Or InStr(facRfc.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en rfc de factura');</script>")
            Return 0
        End If
        If InStr(rutaFiel.Text.ToUpper, "SELECT ") > 0 Or InStr(rutaFiel.Text.ToUpper, "INSERT ") > 0 Or InStr(rutaFiel.Text.ToUpper, "UPDATE ") > 0 Or InStr(rutaFiel.Text.ToUpper, "DELETE ") > 0 Or InStr(rutaFiel.Text.ToUpper, "DROP ") > 0 Or InStr(rutaFiel.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en rutafiel');</script>")
            Return 0
        End If
        If InStr(whats.Text.ToUpper, "SELECT ") > 0 Or InStr(whats.Text.ToUpper, "INSERT ") > 0 Or InStr(whats.Text.ToUpper, "UPDATE ") > 0 Or InStr(whats.Text.ToUpper, "DELETE ") > 0 Or InStr(whats.Text.ToUpper, "DROP ") > 0 Or InStr(whats.Text.ToUpper, "ALTER ") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas en whatsapp');</script>")
            Return 0
        End If

        Return 1
    End Function

    Private Sub WebForm4_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

    End Sub



    Protected Sub mod_Click(ByVal sender As Object, ByVal e As EventArgs) Handles [mod].Click
        If validaInyeccion() < 1 Then
            Exit Sub
        End If
        If validaVacios() < 1 Then
            Exit Sub
        End If
        If validaDuplModUsuario(idParam) < 1 Then
            Exit Sub
        End If

        If Session("curCorreo") = "PRUEBASDEIDE@GMAIL.COM" Then 'cuenta de pruebas demo
            If Session("curCorreo") <> correo.Text.ToUpper.Trim Then
                Response.Write("<script language='javascript'>alert('Operacion de Modificar cuenta de correo limitada en cuenta demo');</script>")
                Exit Sub
            End If
            If Session("pkPass") <> passWeb.Text Then
                Response.Write("<script language='javascript'>alert('Operacion de Modificar password limitada en cuenta demo');</script>")
                Exit Sub
            End If
        End If

        Dim esInstitCreditoVal, q, rfcComodinPmVal, chkCasfimProvisional, casfimProvisionalVal, facTerceroVal, nombreFullVal
        If esInstitCredito.Checked = False Then
            esInstitCreditoVal = "0"
        Else
            esInstitCreditoVal = "1"
        End If

        If rfcComodinPm.Checked = False Then
            rfcComodinPmVal = "0"
        Else
            rfcComodinPmVal = "1"
        End If
        If chkNombreFull.Checked = False Then
            nombreFullVal = "0"
        Else
            nombreFullVal = "1"
        End If
        If casfimProvisional.Checked = False Then
            casfimProvisionalVal = "0"
        Else
            casfimProvisionalVal = "1"
        End If
        If facTercero.Checked = False Then
            facTerceroVal = "0"
        Else
            facTerceroVal = "1"
        End If

        Dim q2
        Dim v
        If idDistribuidor.Text.Trim <> "" Then
            q2 = "SELECT id FROM distribuidores WHERE id=@idDis"
            myCommand = New SqlCommand(q2)
            myCommand.Parameters.AddWithValue("@idDis", idDistribuidor.Text.ToUpper.Trim)
            v = ExecuteScalarFunction(myCommand)
        Else
            q2 = "SELECT id FROM distribuidores WHERE nombreFiscal='DEFAULT'"
            myCommand = New SqlCommand(q2)
            v = ExecuteScalarFunction(myCommand)
        End If

        If IsNothing(v) Then
            idDistribuidor.Focus()
            Response.Write("<script language='javascript'>alert('Ese distribuidor no existe o no está autorizado, verifiquelo o dejelo en blanco');</script>")
            Exit Sub
        End If
        idDistribuidor.Text = v

        'tomando casfim original
        q = "SELECT casfim from clientes WHERE id=" + idParam.ToString
        myCommand = New SqlCommand(q)
        bkCasfim = ExecuteScalarFunction(myCommand)

        'encriptacion
        myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';")
        myConnection.Open()
        myCommand = New SqlCommand("DECLARE @KEYID UNIQUEIDENTIFIER SET @KEYID = KEY_GUID('SYM_KEY') OPEN SYMMETRIC KEY SYM_KEY DECRYPTION BY PASSWORD='##Djjcp##' UPDATE clientes SET passWeb=ENCRYPTBYKEY(@KEYID,'" + Trim(passWeb.Text) + "') WHERE id=" + idParam.ToString, myConnection)
        myCommand.ExecuteNonQuery()
        myCommand = New SqlCommand("CLOSE SYMMETRIC KEY SYM_KEY", myConnection)
        myCommand.ExecuteNonQuery()
        myConnection.Close()

        'el pass se act arriba aparte
        q = "UPDATE clientes SET correo=@corr, razonSoc=@raz, contacto=@con, puesto=@pue, tel=@tel, cel=@cel, paginaWeb=@pag, rfcDeclarante=@rfc, domFiscal=@dom, numSucursales=@suc, numSociosClientes=@soc, casfim=@cas, directorioServidor=@cas, esInstitCredito=" + esInstitCreditoVal + ", rfcComodinPm=" + rfcComodinPmVal + ", nombreFull=" + nombreFullVal + ", idDistribuidor=@idDis,  idEstatus=@cli, fuente=@fue, casfimProvisional=" + casfimProvisionalVal + ", dxFac=@dxF, facTercero=@facTercero, facRfc=@facRfc, facRazon=@facRazon, facUso=@facUso, facCorreos=@facCorreos, facRetens=@facRetens, remoto=@remoto, rutaFiel=@rutaFiel, whats=@whats, idNumRemoto=@idNumRemoto, passRemoto=@passRemoto, formaPresentacion=@formaPresentacion WHERE id=" + idParam.ToString
        myCommand = New SqlCommand(q)
        myCommand.Parameters.AddWithValue("@corr", correo.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@raz", razonSoc.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@con", contacto.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@pue", puesto.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@tel", tel.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@cel", cel.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@pag", paginaWeb.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@rfc", rfcDeclarante.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@dom", domFiscal.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@suc", numSucursales.Text.ToUpper.Trim.Replace(",", ""))
        myCommand.Parameters.AddWithValue("@soc", numSociosClientes.Text.ToUpper.Trim.Replace(",", ""))
        myCommand.Parameters.AddWithValue("@cas", casfim.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@idDis", idDistribuidor.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@cli", clientesEstatus.SelectedValue)
        myCommand.Parameters.AddWithValue("@fue", fuente.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@dxF", dxFac.Text.Trim)
        myCommand.Parameters.AddWithValue("@facCorreos", facCorreos.Text.Trim)
        myCommand.Parameters.AddWithValue("@facTercero", facTerceroVal.ToString)
        myCommand.Parameters.AddWithValue("@facRfc", facRfc.Text)
        myCommand.Parameters.AddWithValue("@facRazon", facRazon.Text)
        myCommand.Parameters.AddWithValue("@facUso", facUso.SelectedValue)
        myCommand.Parameters.AddWithValue("@facRetens", facRetens.Checked)
        myCommand.Parameters.AddWithValue("@rutaFiel", rutaFiel.Text.Trim)
        myCommand.Parameters.AddWithValue("@whats", whats.Text.Trim)
        myCommand.Parameters.AddWithValue("@remoto", remoto.SelectedValue)
        myCommand.Parameters.AddWithValue("@idNumRemoto", idNumRemoto.Text.Trim)
        myCommand.Parameters.AddWithValue("@passRemoto", passRemoto.Text.Trim)
        myCommand.Parameters.AddWithValue("@formaPresentacion", formaPresentacion.SelectedValue)
        'myCommand.Parameters.AddWithValue("@facFP", facFP.SelectedValue)
        ExecuteReaderFunction(myCommand)

        If casfim.Text.Trim <> "" Then
            If (Not System.IO.Directory.Exists("C:\SAT")) Then
                System.IO.Directory.CreateDirectory("C:\SAT")
            End If
            If (Not System.IO.Directory.Exists("C:\SAT\" + casfim.Text.Trim)) Then
                System.IO.Directory.CreateDirectory("C:\SAT\" + casfim.Text.Trim)
            End If
            directorioServidor.Enabled = True
            directorioServidor.Text = casfim.Text.Trim
            directorioServidor.Enabled = False
        End If
        If Session("curCorreo") <> correo.Text.ToUpper.Trim Then
            Session("curCorreo") = correo.Text.ToUpper.Trim
            FormsAuthentication.SignOut()
            Session.Abandon()
            Response.Cache.SetCacheability(HttpCacheability.Private)
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.Write("<script language='javascript'>alert('Actualizado correctamente, cerrando sesión');</script>")
            Response.Write("<script>location.href = 'Login.aspx';</script>")
        Else
            Response.Write("<script language='javascript'>alert('Actualizado correctamente');</script>")
        End If

        'Response.Write("<script language='javascript'>alert('" + bkCasfim + ", " + casfim.Text.Trim + "');</script>")
        If bkCasfim <> casfim.Text.Trim Then
            If bkCasfim <> "" And IsNothing(bkCasfim) = False Then
                If (System.IO.Directory.Exists("C:\SAT\" + bkCasfim)) Then
                    Dim oDir As New System.IO.DirectoryInfo("C:\SAT\" + bkCasfim)
                    oDir.Attributes = oDir.Attributes And Not IO.FileAttributes.ReadOnly
                    Dim di As New IO.DirectoryInfo("C:\SAT\" + bkCasfim + "\")
                    Dim diar1 As IO.FileInfo() = di.GetFiles("*.*")
                    Dim fichero As FileInfo
                    For Each fichero In diar1
                        File.Copy(fichero.FullName, "C:\SAT\" + casfim.Text.Trim + "\" + fichero.Name)
                        File.Delete(fichero.FullName)
                    Next
                    System.IO.Directory.Delete("C:\SAT\" + bkCasfim, False)
                End If
            End If
        End If
    End Sub

    Private Function validaDuplModUsuario(ByVal id) As Integer
        Dim q
        q = "SELECT * FROM clientes WHERE id<>" + id.ToString + " and (correo=@corr or razonSoc=@razon or rfcDeclarante=@rfc or casfim=@casfim)" 'or->1 col NO puede repet en otro reg
        myCommand = New SqlCommand(q)
        myCommand.Parameters.AddWithValue("@corr", correo.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@razon", razonSoc.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@rfc", rfcDeclarante.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@casfim", casfim.Text.ToUpper.Trim)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('Ese correo, razon social, rfc o clave casfim ya están en uso x otro usuario');</script>")
            Return 0
        End If

        Return 1
    End Function

    Private Function validaDuplModAdmin(ByVal id) As Integer
        Dim q, v
        If Trim(loginSAT.Text.ToUpper) <> "" Then
            q = "SELECT id FROM clientes WHERE id<>" + id.ToString + " and loginSAT='" + Trim(loginSAT.Text.ToUpper) + "'"
            myCommand = New SqlCommand(q)
            v = ExecuteScalarFunction(myCommand)
            If Not IsNothing(v) Then
                loginSAT.Focus()
                Response.Write("<script language='javascript'>alert('Ese loginSAT ya está en uso x otro usuario');</script>")
                Return 0
            End If
        End If

        If Trim(directorioSat.Text.ToUpper) <> "" Then
            q = "SELECT id FROM clientes WHERE id<>" + id.ToString + " and directorioSat='" + Trim(directorioSat.Text.ToUpper) + "'"
            myCommand = New SqlCommand(q)
            v = ExecuteScalarFunction(myCommand)
            If Not IsNothing(v) Then
                directorioSat.Focus()
                Response.Write("<script language='javascript'>alert('Ese directorioSat ya está en uso x otro usuario');</script>")
                Return 0
            End If
        End If

        If fechaSolSocketSat.Text.Trim <> "" Then
            Dim dtnow As DateTime
            Dim regDate As New System.Text.RegularExpressions.Regex("^\d{1,2}\/\d{1,2}\/\d{4}$")
            If regDate.IsMatch(fechaSolSocketSat.Text.Trim) Then
                If Not DateTime.TryParse(fechaSolSocketSat.Text.Trim, dtnow) Then
                    fechaSolSocketSat.Focus()
                    Response.Write("<script language='javascript'>alert('fechaSolSocketSat fecha invalida');</script>")
                    Return 0
                End If
            Else
                fechaSolSocketSat.Focus()
                Response.Write("<script language='javascript'>alert('fechaSolSocketSat formato de fecha no valido (dd/mm/aaaa)');</script>")
                Return 0
            End If
        End If

        If fechaPrueba.Text.Trim <> "" Then
            Dim dtnow As DateTime
            Dim regDate As New System.Text.RegularExpressions.Regex("^\d{1,2}\/\d{1,2}\/\d{4}$")
            If regDate.IsMatch(fechaPrueba.Text.Trim) Then
                If Not DateTime.TryParse(fechaPrueba.Text.Trim, dtnow) Then
                    fechaPrueba.Focus()
                    Response.Write("<script language='javascript'>alert('fechaPrueba fecha invalida');</script>")
                    Return 0
                End If
            Else
                fechaPrueba.Focus()
                Response.Write("<script language='javascript'>alert('fechaPrueba formato de fecha no valido (dd/mm/aaaa)');</script>")
                Return 0
            End If
        End If


        Return 1
    End Function

    Protected Sub modAdmin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles modAdmin.Click
        If validaDuplModAdmin(idParam) < 1 Then
            Exit Sub
        End If
        Dim q, fechaSolSocketSatVal, fechaPruebaVal
        If fechaSolSocketSat.Text.Trim = "" Then
            fechaSolSocketSatVal = ""
        Else
            fechaSolSocketSatVal = ",fechaSolSocketSat='" + Format(Convert.ToDateTime(Trim(fechaSolSocketSat.Text)), "yyyy-MM-dd") + "'"
        End If
        If fechaPrueba.Text.Trim = "" Then
            fechaPruebaVal = ""
        Else
            fechaPruebaVal = ",fechaPrueba='" + Format(Convert.ToDateTime(Trim(fechaPrueba.Text)), "yyyy-MM-dd") + "'"
        End If

        'loginSAT, directorioSAT tal cual lo da hacienda
        q = "UPDATE clientes SET loginSAT='" + Trim(loginSAT.Text) + "',directorioServidor='" + Trim(directorioServidor.Text.ToUpper) + "', directorioSat='" + Trim(directorioSat.Text) + "'" + fechaSolSocketSatVal + fechaPruebaVal + ", ipSat='" + Trim(ipSat.Text.ToUpper) + "', solSocketEstatus='" + solSocketEstatus.Text + "' WHERE id=" + idParam.ToString
        myCommand = New SqlCommand(q)
        ExecuteReaderFunction(myCommand)

        If FileUpload2.HasFile Then
            Dim savePath As String = "C:\SocketGrl\etc\"
            Dim fileSize As Integer = FileUpload2.PostedFile.ContentLength
            Dim fileName As String = Server.HtmlEncode(FileUpload2.FileName)
            Dim extension As String = System.IO.Path.GetExtension(fileName)
            If (extension = ".conf" Or extension = ".CONF") Then
                savePath += Server.HtmlEncode(fileName & "Nuevo.conf")
                FileUpload2.SaveAs(savePath)
                Dim fechaanterior = Left(FileDateTime("C:\SocketGrl\etc\cli.conf").ToString, 10).Replace("/", "")
                File.Replace(savePath, "C:\SocketGrl\etc\cli.conf", "C:\SocketGrl\etc\cli.conf." & fechaanterior)
                File.Delete(savePath)
                'AddFileSecurity(savePath, Session("identidad"), FileSystemRights.ReadData, AccessControlType.Allow)
            Else
                Response.Write("<script language='javascript'>alert('El archivo debe ser de tipo .conf');</script>")
                Exit Sub
            End If
        End If

        If fechaPrueba.Text.Trim <> "" Then
            Dim elcorreo As New System.Net.Mail.MailMessage
            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
            elcorreo.To.Add("declaracioneside@gmail.com")
            elcorreo.Subject = "Probar socket el dia " + fechaPrueba.Text.Trim + " para " + Session("curCorreo")
            elcorreo.Body = "<html><body>Si no se ha llegado la fecha y no has hecho la prueba, marca como no leído este correo, pero si ya es la fecha, haz la prueba del socket</body></html>"
            elcorreo.IsBodyHtml = True
            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
            Dim smpt As New System.Net.Mail.SmtpClient
            smpt.Host = "smtp.gmail.com"
            smpt.Port = "587"
            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
            smpt.EnableSsl = True 'req p server gmail
            Try
                smpt.Send(elcorreo)
                Response.Write("<script language='javascript'>alert('Notificación enviada');</script>")
            Catch ex As Exception
                Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
                Exit Sub
            End Try
        End If

        Response.Write("<script language='javascript'>alert('Actualizado correctamente');</script>")
    End Sub

    Private Sub GridView2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView2.SelectedIndexChanged
        Dim row As GridViewRow = GridView2.SelectedRow
        idReprLeg.Text = row.Cells(1).Text
        nombres.Text = Server.HtmlDecode(row.Cells(2).Text)
        ap1.Text = Server.HtmlDecode(row.Cells(3).Text)
        ap2.Text = Server.HtmlDecode(row.Cells(4).Text)
        rfc.Text = Server.HtmlDecode(row.Cells(5).Text)
        curp.Text = Server.HtmlDecode(row.Cells(6).Text)
    End Sub

    Private Sub add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles add.Click
        If InStr(nombres.Text.ToUpper, "SELECT") > 0 Or InStr(nombres.Text.ToUpper, "INSERT") > 0 Or InStr(nombres.Text.ToUpper, "UPDATE") > 0 Or InStr(nombres.Text.ToUpper, "DELETE") > 0 Or InStr(nombres.Text.ToUpper, "DROP") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas');</script>")
            Exit Sub
        End If
        If InStr(ap1.Text.ToUpper, "SELECT") > 0 Or InStr(ap1.Text.ToUpper, "INSERT") > 0 Or InStr(ap1.Text.ToUpper, "UPDATE") > 0 Or InStr(ap1.Text.ToUpper, "DELETE") > 0 Or InStr(ap1.Text.ToUpper, "DROP") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas');</script>")
            Exit Sub
        End If
        If InStr(ap2.Text.ToUpper, "SELECT") > 0 Or InStr(ap2.Text.ToUpper, "INSERT") > 0 Or InStr(ap2.Text.ToUpper, "UPDATE") > 0 Or InStr(ap2.Text.ToUpper, "DELETE") > 0 Or InStr(ap2.Text.ToUpper, "DROP") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas');</script>")
            Exit Sub
        End If
        If InStr(rfc.Text.ToUpper, "SELECT") > 0 Or InStr(rfc.Text.ToUpper, "INSERT") > 0 Or InStr(rfc.Text.ToUpper, "UPDATE") > 0 Or InStr(rfc.Text.ToUpper, "DELETE") > 0 Or InStr(rfc.Text.ToUpper, "DROP") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas');</script>")
            Exit Sub
        End If
        If InStr(curp.Text.ToUpper, "SELECT") > 0 Or InStr(curp.Text.ToUpper, "INSERT") > 0 Or InStr(curp.Text.ToUpper, "UPDATE") > 0 Or InStr(curp.Text.ToUpper, "DELETE") > 0 Or InStr(curp.Text.ToUpper, "DROP") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas');</script>")
            Exit Sub
        End If

        Dim esActualVal
        If validaVaciosRL() < 1 Then
            Exit Sub
        End If

        If validaDuplRL() < 1 Then
            Exit Sub
        End If
        Dim q As String
        If reprLegNregs.Text = "0 Registros" Then
            esActualVal = "1"
        Else
            esActualVal = "0"
        End If
        q = "INSERT INTO reprLegal(idCliente,nombres,ap1,ap2,nombrecompleto,rfc,esActual,curp) VALUES(" + Trim(idParam.ToString) + ",@nom,@ap1,@ap2,@nomc,@rfc," + esActualVal + ",@curp)"
        myCommand = New SqlCommand(q)
        myCommand.Parameters.AddWithValue("@nom", nombres.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@ap1", ap1.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@ap2", ap2.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@nomC", nombres.Text.ToUpper.Trim + " " + ap1.Text.ToUpper.Trim + " " + ap2.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@rfc", rfc.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@curp", curp.Text.ToUpper.Trim)
        ExecuteReaderFunction(myCommand)

        If esActualVal = "1" Then
            q = "select id from reprLegal where idCliente=" + Trim(idParam.ToString)
            myCommand = New SqlCommand(q)
            Dim v = ExecuteScalarFunction(myCommand)
            actualRepr.Text = v.ToString
        End If

        'refrescar grid
        idReprLeg.Text = "ID"
        nombres.Text = ""
        ap1.Text = ""
        ap2.Text = ""
        rfc.Text = ""
        curp.Text = ""
        GridView2.DataBind()
        cuentaRegistros()
        Response.Write("<script language='javascript'>alert('Registro exitoso');</script>")

    End Sub

    Private Function validaVaciosRL() As Integer
        If Trim(nombres.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el nombre');</script>")
            nombres.Focus()
            Return 0
        End If

        If Trim(ap1.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el apellido paterno');</script>")
            ap1.Focus()
            Return 0
        End If
        If Trim(ap2.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el apellido materno');</script>")
            ap2.Focus()
            Return 0
        End If
        If Trim(curp.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique la curp');</script>")
            curp.Focus()
            Return 0
        End If
        If Trim(rfc.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el rfc');</script>")
            rfc.Focus()
            Return 0
        End If
        If Len(Trim(rfc.Text)) < 9 Or Len(Trim(rfc.Text)) > 13 Then
            Response.Write("<script language='javascript'>alert('Tamaño de rfc debe estar entre 9 y 13 caracteres');</script>")
            rfc.Focus()
            Return 0
        End If
        If Len(Trim(curp.Text)) <> 18 Then
            Response.Write("<script language='javascript'>alert('Tamaño de curp debe ser de 18 caracteres');</script>")
            curp.Focus()
            Return 0
        End If
        If Not Regex.IsMatch(rfc.Text.ToUpper.Trim, "^([A-Z\s]{4})\d{6}([A-Z\w]{3})$") Then
            Response.Write("<script language='javascript'>alert('Formato de rfc invalido');</script>")
            rfc.Focus()
            Return 0
        End If
        If Not Regex.IsMatch(curp.Text.ToUpper.Trim, "[A-Z]{4}[0-9]{6}[H,M][A-Z]{5}[0-9]{2}") Then
            Response.Write("<script language='javascript'>alert('Formato de curp invalido');</script>")
            curp.Focus()
            Return 0
        End If

        Return 1
    End Function
    Private Function validaDuplRL() As Integer
        Dim q, elnombrecompleto
        elnombrecompleto = nombres.Text.ToUpper.Trim + " " + ap1.Text.ToUpper.Trim + " " + ap2.Text.ToUpper.Trim
        q = "SELECT id FROM reprLegal WHERE idCliente=" + idParam.ToString + " AND nombreCompleto=@nom"
        myCommand = New SqlCommand(q)
        myCommand.Parameters.AddWithValue("@nom", elnombrecompleto)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('Ya existe ese representante legal para ese cliente');</script>")
            Return 0
        End If

        Return 1
    End Function

    Private Function validaDuplModRL() As Integer
        Dim q
        q = "SELECT nombreCompleto FROM reprLegal WHERE ID='" + Trim(idReprLeg.Text) + "'"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        PKnombreCompleto = v.ToString()

        Dim elnombrecompleto
        elnombrecompleto = nombres.Text.ToUpper.Trim + " " + ap1.Text.ToUpper.Trim + " " + ap2.Text.ToUpper.Trim
        q = "SELECT id FROM reprLegal WHERE idCliente=" + idParam.ToString + "AND nombreCompleto='" + elnombrecompleto + "'"
        myCommand = New SqlCommand(q)
        v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) And (PKnombreCompleto <> elnombrecompleto) Then
            Response.Write("<script language='javascript'>alert('Ese representante legal ya está en uso');</script>")
            Return 0
        End If

        Return 1
    End Function

    Protected Sub defActual_Click(ByVal sender As Object, ByVal e As EventArgs) Handles defActual.Click
        If idReprLeg.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If
        Dim q
        If actualRepr.Text <> "" Then
            q = "UPDATE reprLegal SET esActual=0 where id=" + actualRepr.Text
            myCommand = New SqlCommand(q)
            ExecuteNonQueryFunction(myCommand)
        End If

        q = "UPDATE reprLegal SET esActual=1 where idCliente=" + idParam.ToString + " AND id=" + idReprLeg.Text.ToString
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)

        Response.Write("<script language='javascript'>alert('Actualizado correctamente');</script>")
        actualRepr.Text = Trim(idReprLeg.Text)
        PKnombreCompleto = nombres.Text.ToUpper.Trim + " " + ap1.Text.ToUpper.Trim + " " + ap2.Text.ToUpper.Trim
    End Sub

    Protected Sub edit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles edit.Click
        If idReprLeg.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If
        If validaVaciosRL() < 1 Then
            Exit Sub
        End If
        If InStr(nombres.Text.ToUpper, "SELECT") > 0 Or InStr(nombres.Text.ToUpper, "INSERT") > 0 Or InStr(nombres.Text.ToUpper, "UPDATE") > 0 Or InStr(nombres.Text.ToUpper, "DELETE") > 0 Or InStr(nombres.Text.ToUpper, "DROP") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas');</script>")
            Exit Sub
        End If
        If InStr(ap1.Text.ToUpper, "SELECT") > 0 Or InStr(ap1.Text.ToUpper, "INSERT") > 0 Or InStr(ap1.Text.ToUpper, "UPDATE") > 0 Or InStr(ap1.Text.ToUpper, "DELETE") > 0 Or InStr(ap1.Text.ToUpper, "DROP") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas');</script>")
            Exit Sub
        End If
        If InStr(ap2.Text.ToUpper, "SELECT") > 0 Or InStr(ap2.Text.ToUpper, "INSERT") > 0 Or InStr(ap2.Text.ToUpper, "UPDATE") > 0 Or InStr(ap2.Text.ToUpper, "DELETE") > 0 Or InStr(ap2.Text.ToUpper, "DROP") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas');</script>")
            Exit Sub
        End If
        If InStr(rfc.Text.ToUpper, "SELECT") > 0 Or InStr(rfc.Text.ToUpper, "INSERT") > 0 Or InStr(rfc.Text.ToUpper, "UPDATE") > 0 Or InStr(rfc.Text.ToUpper, "DELETE") > 0 Or InStr(rfc.Text.ToUpper, "DROP") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas');</script>")
            Exit Sub
        End If
        If InStr(curp.Text.ToUpper, "SELECT") > 0 Or InStr(curp.Text.ToUpper, "INSERT") > 0 Or InStr(curp.Text.ToUpper, "UPDATE") > 0 Or InStr(curp.Text.ToUpper, "DELETE") > 0 Or InStr(curp.Text.ToUpper, "DROP") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas');</script>")
            Exit Sub
        End If
        If validaDuplModRL() < 1 Then
            Exit Sub
        End If
        Dim q As String
        q = "UPDATE reprLegal SET nombres=@nom,ap1=@ap1,ap2=@ap2, nombreCompleto=@nomC, rfc=@rfc, curp=@curp WHERE id=" + idReprLeg.Text
        myCommand = New SqlCommand(q)
        myCommand.Parameters.AddWithValue("@nom", nombres.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@ap1", ap1.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@ap2", ap2.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@nomC", nombres.Text.ToUpper.Trim + " " + ap1.Text.ToUpper.Trim + " " + ap2.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@rfc", rfc.Text.ToUpper.Trim)
        myCommand.Parameters.AddWithValue("@curp", curp.Text.ToUpper.Trim)
        ExecuteNonQueryFunction(myCommand)

        'refrescar grid
        idReprLeg.Text = "ID"
        nombres.Text = ""
        ap1.Text = ""
        ap2.Text = ""
        rfc.Text = ""
        curp.Text = ""
        GridView2.DataBind()
        GridView2.SelectedIndex = -1
        Response.Write("<script language='javascript'>alert('Actualizacion exitosa');</script>")
    End Sub

    Protected Sub del_Click(ByVal sender As Object, ByVal e As EventArgs) Handles del.Click
        If idReprLeg.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro ');</script>")
            Exit Sub
        End If

        'validar si esta siendo usado x FKs
        Dim q As String
        q = "SELECT idRepresentanteLegal FROM ideAnual WHERE idRepresentanteLegal=" + Trim(idReprLeg.Text)
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('No se puede eliminar este registro pues esta siendo usado en una declaracion anual');</script>")
            Exit Sub
        End If

        q = "SELECT idRepresentanteLegal FROM ideMens WHERE idRepresentanteLegal=" + Trim(idReprLeg.Text)
        myCommand = New SqlCommand(q)
        dr = myCommand.ExecuteReader()
        v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('No se puede eliminar este registro pues esta siendo usado en una declaracion mensual');</script>")
            Exit Sub
        End If

        'del cascadas
        If Trim(idReprLeg.Text) = actualRepr.Text And GridView2.Rows.Count > 1 Then 'borrando la actual
            Response.Write("<script language='javascript'>alert('Este es el registro en uso, para eliminarlo marque 1o otro como el actual');</script>")
            Exit Sub
        End If

        q = "DELETE FROM reprLegal WHERE id=" + Trim(idReprLeg.Text)
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)
        idReprLeg.Text = "ID"
        nombres.Text = ""
        ap1.Text = ""
        ap2.Text = ""
        curp.Text = ""
        rfc.Text = ""
        cuentaRegistros()
        GridView2.DataBind()
        Response.Write("<script language='javascript'>alert('Se ha eliminado');</script>")

    End Sub


    Protected Sub loginSAT_TextChanged(sender As Object, e As EventArgs) Handles loginSAT.TextChanged

    End Sub

    Private Function validaRequisitos() As Integer
        Dim q
        q = "SELECT id, casfim, loginSAT FROM clientes cli WHERE cli.id=" + idParam.ToString
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If v = "" Or IsNothing(v) = True Then
            Response.Write("<script language='javascript'>alert('Requiere especificar su clave CASFIM');</script>")
            Return 0
        End If


        q = "select solSocketEstatus from clientes WHERE id=" + idParam.ToString
        myCommand = New SqlCommand(q)
        v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            If v <> "VALIDADA" Then
                Response.Write("<script language='javascript'>alert('Se ocupa que el estatus del socket sea carta validada o bien suba y valide la carta');</script>")
                Return 0
            End If
        End If


        Return 1
    End Function

    Protected Sub solSocket_Click(sender As Object, e As EventArgs) Handles solSocket.Click
        If validaRequisitos() < 1 Then
            Exit Sub
        End If

        fechaSolSocketSat.Text = Left(Format(Now(), "dd/MM/yyyy"), 10)
        Dim q
        q = "UPDATE clientes SET fechaSolSocketSat='" + Format(Now(), "yyyy-MM-dd") + "' WHERE id=" + idParam.ToString
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)

        Dim oDir As New System.IO.DirectoryInfo(Server.MapPath("~"))
        oDir.Attributes = oDir.Attributes And Not IO.FileAttributes.ReadOnly

        If (File.Exists(Server.MapPath("~/Solicitud de Matrices IDE formato copia.doc"))) Then
            File.Delete(Server.MapPath("~/Solicitud de Matrices IDE formato copia.doc"))
        End If

        'si lanzo un proceso no guarda el archivo word

        Dim WDoc As New Microsoft.Office.Interop.Word.Document()
        Dim AppWord As New Microsoft.Office.Interop.Word.Application()
        Try
            WDoc = AppWord.Documents.Open(Server.MapPath("~/Solicitud de Matrices IDE formato.dotx"))
            AppWord.Visible = False
            WDoc.FormFields("fechaSolSocketSat").Result = fechaSolSocketSat.Text
            WDoc.FormFields("razonSoc").Result = razonSoc.Text
            WDoc.FormFields("casfim").Result = casfim.Text
            WDoc.FormFields("rutaSAT").Result = "C:\SAT\" + casfim.Text
            WDoc.SaveAs(Server.MapPath("~/Solicitud de Matrices IDE formato copia.doc"))
        Catch ex As COMException
            Response.Write("<script language='javascript'>alert('" + ex.InnerException.ToString() + "');</script>")

        Finally
            WDoc.Close(False) 'cierra plantilla
            AppWord.Quit()
            WDoc = Nothing
            AppWord = Nothing
        End Try

        oDir.Attributes = oDir.Attributes And IO.FileAttributes.ReadOnly

        Dim myCommand2 As New SqlCommand
        With myCommand2
            .Connection = myConnection
            .CommandType = CommandType.StoredProcedure
            .CommandText = "clientesAutorizacionDescarga"
            .Parameters.AddWithValue("@ID", idParam.ToString)
            dr = .ExecuteReader()
            If Not dr Is Nothing Then
                If dr.Read Then
                    If DBNull.Value.Equals(dr("solSocketArch")) Then
                        Response.Write("<script language='javascript'>alert('Aun no ha subido su autorización');</script>")
                        dr.Close()
                        Exit Sub
                    End If
                    Dim writeStream As FileStream
                    writeStream = New FileStream(Server.MapPath("~/autorizaciones/" + Session("curCorreo") + ".pdf.ZIP"), FileMode.Create)
                    Dim writeBinay As New BinaryWriter(writeStream)
                    writeBinay.Write(dr("solSocketArch"))
                    writeBinay.Close()

                    Dim ZipToUnpack As String = Server.MapPath("~/autorizaciones/" + Session("curCorreo") + ".pdf.ZIP")
                    Dim TargetDir As String = Server.MapPath("~/autorizaciones")
                    Using zip1 As ZipFile = ZipFile.Read(ZipToUnpack)
                        'AddHandler zip1.ExtractProgress, AddressOf MyExtractProgress
                        Dim e1 As ZipEntry
                        For Each e1 In zip1
                            e1.Extract(TargetDir, ExtractExistingFileAction.OverwriteSilently)
                        Next
                    End Using
                End If
            End If
        End With

        Dim elcorreo As New System.Net.Mail.MailMessage
        Using elcorreo
            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com", "Declaraciones IDE")
            elcorreo.To.Add("guadalupe.hernandezr@sat.gob.mx") 'declaracioneside@gmail.com
            elcorreo.CC.Add("ana.arroyo@sat.gob.mx") 'declaracioneside@gmail.com
            elcorreo.CC.Add("brenda.gordillo@sat.gob.mx") 'declaracioneside@gmail.com
            elcorreo.Bcc.Add("declaracioneside@gmail.com") 'declaracioneside@gmail.com
            elcorreo.Subject = "Solicitud de matriz para Conexión Segura"
            elcorreo.Body = "<html><body>Buen día, <br><br>Anexo la autorización recibida para tramitar y el formato para solicitarle matriz para Conexión Segura para declaraciones del IDE de la institución: " + razonSoc.Text.ToUpper.Trim + " cuyo RFC es " + rfcDeclarante.Text + "<br><br>Reciba un cordial saludo,<br><br>Gracias</body></html>"
            elcorreo.IsBodyHtml = True
            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
            elcorreo.Attachments.Add(New System.Net.Mail.Attachment(Server.MapPath("~/autorizaciones/" + Session("curCorreo") + ".pdf")))
            elcorreo.Attachments.Add(New Attachment(Server.MapPath("~/Solicitud de Matrices IDE formato copia.doc")))
            Dim smpt As New System.Net.Mail.SmtpClient
            smpt.DeliveryMethod = SmtpDeliveryMethod.Network
            smpt.UseDefaultCredentials = False
            smpt.Host = "smtp.gmail.com"
            smpt.Port = 587 ' 465 
            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")  '("admon.declaracioneside", "a1declaracioneside")
            smpt.EnableSsl = True 'req p server gmail

            Try
                smpt.Send(elcorreo)
                elcorreo.Dispose()
                Response.Write("<script language='javascript'>alert('Envío exitoso de solicitud por correo, espere un minuto y llame al SAT para validar recepción de solicitud de socket con Brenda Gordillo Sanchez, Ana Lilia Arroyo Gonzalez (55) 12 03 10 00 ext. 43881, si no la recibió reenviarla o redactarla hasta que le llegue, está en Enviados de declaracioneside@gmail.com');</script>")
            Catch ex As Exception
                Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
                Exit Sub
            Finally
                'If (File.Exists(Server.MapPath("~/Solicitud de Matrices IDE formato copia.doc"))) Then
                '    File.Delete(Server.MapPath("~/Solicitud de Matrices IDE formato copia.doc"))
                'End If
            End Try
        End Using

    End Sub

    Protected Sub notifica_Click(sender As Object, e As EventArgs) Handles notifica.Click
        Dim q
        q = "UPDATE clientes SET solSocketEstatus='APROBADA' WHERE id=" + idParam.ToString
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)

        Dim elcorreo As New System.Net.Mail.MailMessage
        elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
        elcorreo.To.Add(Session("curCorreo"))
        elcorreo.Subject = "Notificación: su socket para enviar declaraciones de depositos en efectivo al SAT ya está listo para usarse en nuestro portal web"
        elcorreo.Body = "<html><body>Buen día,<br> A partir de este momento puede enviar declaraciones del IDE y bajar sus respectivos acuses, siempre que tenga contrato vigente y pagado, verifique sus datos fiscales en su cuenta antes de crear sus contratos pues con esos datos (razon social, RFC) se le facturará<br> Los contratos son un paquete de un número de declaraciones (plan básico o ceros) o de un rango de fechas (plan premium) bajo un plan, sean para declaraciones retrasadas o para declaraciones próximas, Tú defines la cantidad o el periodo a contratar y puedes combinar contratos de distintos planes a la vez. Para planes básico y ceros, tú decides para cual año y mes utilizar cada declaración, las contratadas no utilizadas podrás usarlas cuando lo requieras, no las pierdes. Para el plan premium las declaraciones se aplican exclusivamente a los periodos contratados. Cada contrato un plan distinto. Las declaraciones puedes irlas presentando conforme obtengas los datos de cada una, no es necesario que para declarar la primera tengas la informacion de todas <br>Reciba un cordial saludo, <br>Atte. <a href='declaracioneside.com'>declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet. Tel 4436903616, 4432180237</body></html>"
        elcorreo.IsBodyHtml = True
        elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        Dim smpt As New System.Net.Mail.SmtpClient
        smpt.Host = "smtp.gmail.com"
        smpt.Port = "587"
        smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
        smpt.EnableSsl = True 'req p server gmail
        Try
            smpt.Send(elcorreo)
            Response.Write("<script language='javascript'>alert('Notificación enviada');</script>")
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
            Exit Sub
        End Try
    End Sub

    Sub AddFileSecurity(ByVal fileName As String, ByVal account As String,
        ByVal rights As FileSystemRights, ByVal controlType As AccessControlType)

        Dim fSecurity As FileSecurity = File.GetAccessControl(fileName)
        Dim accessRule As FileSystemAccessRule =
        New FileSystemAccessRule(account, rights, controlType)
        fSecurity.AddAccessRule(accessRule)
        File.SetAccessControl(fileName, fSecurity)

    End Sub


    Protected Sub solSubir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles solSubir.Click
        If InStr(Request.ServerVariables("HTTP_USER_AGENT"), "MSIE") Then
            Response.Write("<script language='javascript'>alert('Requiere iniciar sesión con un navegador distinto a Internet Explorer, puede descargar e instalar Chrome desde la sección inferior de descargas de esta página');</script>")
            Exit Sub
        End If

        If FileUpload1.HasFile Then

            Dim q
            q = "select solSocketEstatus from clientes WHERE id=" + idParam.ToString
            myCommand = New SqlCommand(q)
            Dim v = ExecuteScalarFunction(myCommand)
            If Not IsNothing(v) Then
                If v = "VALIDADA" Then
                    Response.Write("<script language='javascript'>alert('El archivo anterior ya es el válido');</script>")
                    Exit Sub
                End If
            End If
            Dim savePath As String = Server.MapPath("~/")
            Dim fileSize As Integer = FileUpload1.PostedFile.ContentLength
            Dim fileName As String = Server.HtmlEncode(FileUpload1.FileName)
            Dim extension As String = System.IO.Path.GetExtension(fileName)
            If (fileSize < 350000) Then '350kb
                If (extension = ".pdf" Or extension = ".PDF") Then
                    savePath += Server.HtmlEncode(Session("curCorreo") & ".pdf")
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
                Else
                    Response.Write("<script language='javascript'>alert('El archivo debe ser de tipo .pdf');</script>")
                    Exit Sub
                End If
            Else
                Response.Write("<script language='javascript'>alert('El tamaño del archivo debe ser máximo 350Kb (0.35 Mb), pruebe a guardarlo con menor tamaño o envielo por correo a declaracioneside@gmail.com');</script>")
                Exit Sub
            End If

            Try
                Using zip As ZipFile = New ZipFile
                    zip.AddFile(savePath, "")
                    zip.Save(savePath & ".ZIP")
                End Using
            Catch ex1 As Exception

            Finally
                If File.Exists(savePath) Then 'borro original
                    File.Delete(savePath)
                End If
            End Try

            'AddFileSecurity(savePath & ".ZIP", Session("identidad"), FileSystemRights.ReadData, AccessControlType.Allow)
            'AddFileSecurity(savePath, "IIS_WPG", FileSystemRights.ReadData, AccessControlType.Allow)


            'subir archivo a la BD
            Dim fstream As FileStream
            Dim imgdata As Byte()
            Dim data As Byte()
            Dim finfo As FileInfo
            finfo = New FileInfo(savePath & ".ZIP")
            Dim numbyte As Long
            Dim br As BinaryReader
            numbyte = finfo.Length
            fstream = New FileStream(savePath & ".ZIP", FileMode.Open, FileAccess.Read)
            br = New BinaryReader(fstream)
            data = br.ReadBytes(numbyte)
            imgdata = data

            Dim myCommand2 As New SqlCommand
            With myCommand2
                .Connection = myConnection
                .CommandType = CommandType.StoredProcedure
                .CommandText = "clientesAutorizacion"
                .Parameters.AddWithValue("@ID", idParam.ToString)
                .Parameters.AddWithValue("@Logo", imgdata)
                dr = .ExecuteReader()
            End With
            br.Close()
            fstream.Close()
            dr.Close()

            'enviarme correo avisando
            Dim elcorreo As New System.Net.Mail.MailMessage
            Using elcorreo
                elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                elcorreo.To.Add("declaracioneside@gmail.com")
                elcorreo.Subject = "Autorizacion de socket recibida por " + Session("curCorreo")
                elcorreo.Body = "<html><body>Validar contenido<br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
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
                    'If (File.Exists("C:\inetpub\wwwroot\Solicitud de Matrices IDE formato copia.doc")) Then
                    '    File.Delete("C:\inetpub\wwwroot\Solicitud de Matrices IDE formato copia.doc")
                    'End If
                End Try
            End Using


            File.Delete(savePath & ".ZIP")
            If solSocketEstatus.SelectedValue <> "APROBADA" Then
                solSocketEstatus.SelectedValue = "RECIBIDA"
                q = "update clientes set solSocketEstatus = 'RECIBIDA' WHERE id=" + idParam.ToString
                Dim myCommand3 = New SqlCommand(q)
                ExecuteNonQueryFunction(myCommand3)
            End If

            Response.Write("<script language='javascript'>alert('Archivo recibido correctamente, en breve será notificado sobre la validez de esta carta de autorización');</script>")
        Else
            Response.Write("<script language='javascript'>alert('1o especifique el archivo a subir');</script>")
        End If
    End Sub

    Protected Sub solVerFormato_Click(ByVal sender As Object, ByVal e As EventArgs) Handles solVerFormato.Click
        Dim q, nombreRL
        q = "SELECT rl.nombreCompleto FROM reprLegal rl, clientes cli WHERE cli.id=" + idParam.ToString + " AND cli.id=rl.idCliente AND rl.esActual=1"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('Requiere especificar un representante legal actual');</script>")
            Exit Sub
        End If
        nombreRL = v

        Dim oDir As New System.IO.DirectoryInfo(Server.MapPath("~/"))
        oDir.Attributes = oDir.Attributes And Not IO.FileAttributes.ReadOnly

        'If (File.Exists(Server.MapPath("~/autorizacion tramite socket copia.doc"))) Then
        '    File.Delete(Server.MapPath("~/autorizacion tramite socket copia.doc"))
        'End If

        oDir.Attributes = oDir.Attributes And IO.FileAttributes.ReadOnly

        Dim p As New Process
        p.StartInfo.FileName = Server.MapPath("~/mailMergeAutoriza.exe")
        p.StartInfo.Arguments = casfim.Text.Trim.ToUpper + " " + Server.MapPath("~")
        p.Start()
        p.WaitForExit()

        'descarga archivo, file download
        Dim filename As String = "autorizacion tramite socket copia.doc"
        Dim path As String = Server.MapPath("~/" + filename)
        Dim file1 As System.IO.FileInfo = New System.IO.FileInfo(path)
        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" + file1.Name)
        Response.AddHeader("Content-Length", file1.Length.ToString())
        Response.ContentType = "application/octet-stream"
        Response.WriteFile(file1.FullName)
        Response.End()

        Response.Write("<script language='javascript'>alert('Guarde este docto. en su computador (Guardar como), reemplace en él los datos enmarcados entre los símbolos <>, agregue su membrete, imprimalo y firma de su repres. legal, en seguida guarde en su computador la imagen escaneada que contiene el documento con la firma como tipo .pdf y un tamaño maximo de 150Kb (0.15Mb)');</script>")

    End Sub

    Protected Sub mostrarSol_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mostrarSol.Click
        Dim q
        q = "select casfim from clientes WHERE id=" + idParam.ToString
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('Es necesario que guarde la clave casfim en el sistema');</script>")
            Exit Sub
        Else
            If v = "" Then
                Response.Write("<script language='javascript'>alert('Es necesario que guarde la clave casfim en el sistema');</script>")
                Exit Sub
            End If
        End If

        'bajar de la BD
        Dim myCommand2 As New SqlCommand
        With myCommand2
            .Connection = myConnection
            .CommandType = CommandType.StoredProcedure
            .CommandText = "clientesAutorizacionDescarga"
            .Parameters.AddWithValue("@ID", idParam.ToString)
            dr = .ExecuteReader()
            If Not dr Is Nothing Then
                If dr.Read Then
                    If DBNull.Value.Equals(dr("solSocketArch")) Then
                        Response.Write("<script language='javascript'>alert('Aun no ha subido su autorización');</script>")
                        dr.Close()
                        Exit Sub
                    End If
                    Dim writeStream As FileStream
                    writeStream = New FileStream(Server.MapPath("~/autorizaciones/" + Session("curCorreo") + ".pdf.ZIP"), FileMode.Create)
                    Dim writeBinay As New BinaryWriter(writeStream)
                    writeBinay.Write(dr("solSocketArch"))
                    writeBinay.Close()

                    'descarga archivo, file download
                    Dim filename As String = Session("curCorreo") + ".pdf.ZIP"
                    Dim path As String = Server.MapPath("./autorizaciones/" & filename)
                    Dim file1 As System.IO.FileInfo = New System.IO.FileInfo(path)
                    Response.Clear()
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file1.Name)
                    Response.AddHeader("Content-Length", file1.Length.ToString())
                    Response.ContentType = "application/octet-stream"
                    Response.WriteFile(file1.FullName)
                    Response.End()

                    'Response.Write("<script language='javascript'>window.open('autorizaciones/" + session("curCorreo") + ".pdf');</script>")
                    'System.Diagnostics.Process.Start("C:\SAT\" + casfim.Text.ToUpper.Trim + "\autorizacion.jpg")
                End If
            End If
        End With
    End Sub


    Protected Sub solSocketEstatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles solSocketEstatus.SelectedIndexChanged

    End Sub

    Protected Sub validaAutorizacion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles validaAutorizacion.Click
        If solSocketEstatus.Text = "RECIBIDA" Then
            Dim q
            q = "update clientes set solSocketEstatus='VALIDADA' where id=" + idParam.ToString
            myCommand = New SqlCommand(q)
            ExecuteNonQueryFunction(myCommand)

            q = "SELECT inscrip FROM planes"
            myCommand = New SqlCommand(q)
            Dim v = ExecuteScalarFunction(myCommand)
            Dim inscripBase = v
            q = "SELECT ivaPorcen FROM actuales"
            myCommand = New SqlCommand(q)
            v = ExecuteScalarFunction(myCommand)
            Dim ivaActual = v
            Dim inscripNeto = inscripBase * (1 + ivaActual / 100)

            solSocketEstatus.SelectedValue = "VALIDADA"

            Dim elcorreo As New System.Net.Mail.MailMessage
            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
            elcorreo.To.Add(Session("curCorreo"))
            elcorreo.Subject = "Su autorización para declarar depositos en efectivo mediante nuestro sistema está validada, ahora proceda a realizar los contratos que desee"
            elcorreo.Body = "<html><body>Buen día " + razonSoc.Text + ",<br> A partir de este momento, puede realizar los contratos que desee en el submenú Mis Contratos, una vez que ud. cuente con su clave casfim / clave ide / clave de institucion financiera y nos la notifique, procederemos a tramitar, configurar y realizar las pruebas de su matriz de conexión segura con el SAT para aperturar el canal para el envío de sus declaraciónes cuyo proceso toma 2-3 semanas aprox. tras lo cual será notificado en su correo para que acceda a la sección de declaraciones. Videotutorial para hacer contratos en linea: <a href='http://www.youtube.com/watch?v=ZpeOxQg9SKo' target='_blank'>http://www.youtube.com/watch?v=ZpeOxQg9SKo</a><br><br>Hasta que reciba nuestra notificacion de que su socket ya esta listo, entonces podra proceder al pago indicado en sus contratos para que le activemos su contrato y envienos el comprobante de pago, al hacer sus contratos recibira por correo instrucciones de pago<br><br>Los formatos en que requiere generar la información para enviar desde nuestra página sus declaraciones con reporte de Depósitos en efectivo puede descargarlos en seguida: <a href='declaracioneside.com/ejemploMensual.xlsx'>mensual previa al 2014</a>, <a href='declaracioneside.com/ejemploAnual.xlsx'>anual previa al 2014</a>, <a href='declaracioneside.com/ejemploMensual2.xlsx'>mensual desde 2014</a>, <a href='declaracioneside.com/ejemploAnual2.xlsx'>anual desde 2014</a>, si su declaración es en ceros no requiere dichos formatos y en un par de clics enviara su declaración guiado por nuestros videotutoriales <br><br>Reciba un cordial saludo, <br>Atte. <a href='declaracioneside.com'>declaracioneside.com</a><br><br><br>Tu solución en declaraciones de depósitos en efectivo por internet. Tel 4436903616, 4432180237</body></html>"
            elcorreo.IsBodyHtml = True
            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
            Dim smpt As New System.Net.Mail.SmtpClient
            smpt.Host = "smtp.gmail.com"
            smpt.Port = "587"
            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
            smpt.EnableSsl = True 'req p server gmail
            Try
                smpt.Send(elcorreo)
                Response.Write("<script language='javascript'>alert('Notificación de autorización enviada');</script>")
            Catch ex As Exception
                Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
                Exit Sub
            End Try
        End If
    End Sub

    Protected Sub NavigationMenu_MenuItemClick(sender As Object, e As System.Web.UI.WebControls.MenuEventArgs) Handles NavigationMenu.MenuItemClick
        If NavigationMenu.SelectedItem.Text = "Declarar" Then
            Session("misEjercicio") = ""
            Session("misNdecla") = ""
            Session("misContrato") = ""
            Session("misMes") = ""
            Session("misTipo") = ""
        End If

    End Sub

    Protected Sub prueba_Click(sender As Object, e As EventArgs) Handles prueba.Click
        Dim loginSAT, archivoLocal, directorioServidor, casfim, ipSAT, directorioSAT, archivoLocalSinDir
        Dim q = "SELECT loginSAT,directorioServidor,casfim,ipSAT, directorioSAT FROM clientes WHERE id=" + Session("GidCliente").ToString
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            dr.Read()
            loginSAT = dr("loginSAT")
            casfim = dr("casfim")
            directorioServidor = "C:\SAT\" + dr("directorioServidor")
            ipSAT = dr("ipSAT")
            directorioSAT = dr("directorioSAT")
        End Using

        archivoLocal = "C:\SAT\prueba.txt"
        archivoLocalSinDir = "prueba.txt"
        If File.Exists(archivoLocal) Then
            File.Delete(archivoLocal)
        End If
        Dim archivo As StreamWriter = File.CreateText(archivoLocal)
        archivo.WriteLine(razonSoc.Text.Trim.ToUpper)
        archivo.Close()

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

        'Dim imp As New RunAs_Impersonator
        'imp.ImpersonateStart(".", "Administrator", passS) ' usar . si el usuario es local sin dominio

        Dim proceso As Process
        Dim p As New ProcessStartInfo("C:\SAT\Soky_nt_bank.exe") '("C:\SAT\TestAcuseVB.exe")
        p.Arguments = ipSAT + " " + loginSAT + " " + archivoLocal + " " + directorioSAT + "/" + archivoLocalSinDir
        p.UseShellExecute = False
        p.RedirectStandardOutput = True
        Dim std_out As StreamReader
        Try
            proceso = Process.Start(p)
            std_out = proceso.StandardOutput()
            proceso.WaitForExit()
            pruebaResultado.Text = std_out.ReadToEnd
            std_out.Close()
        Catch ex As Exception
            acuseResultado.Text = ex.Message
        End Try

        If InStr(pruebaResultado.Text, "ERROR") Or InStr(pruebaResultado.Text, "FALLA") Or InStr(pruebaResultado.Text, "Falla") Or InStr(pruebaResultado.Text, "Atencion") Or InStr(pruebaResultado.Text, "errno") Then
        Else
            Dim NewCopy As String
            NewCopy = directorioServidor + "\prueba.txt"
            If Not System.IO.File.Exists(NewCopy) = True Then
                System.IO.File.Copy(archivoLocal, NewCopy)
            End If
        End If
        'pruebaErr.Text = Environment.UserName 

        'Wnd_name = "Declaraciones y Acuses IDE ver 3.0"
        'nWnd = FindWindow(Nothing, Wnd_name)
        ''hWnd6 = FindWindowEx(nWnd, IntPtr.Zero, "ThunderRT6CommandButton", "cancel") 'cancel
        ''If Not hWnd6.Equals(ceroIntPtr) Then
        ''    SetActiveWindow(nWnd)
        ''    retval6 = SendNotifyMessage(hWnd6, BM_CLICK, IntPtr.Zero, 0)
        ''End If

        'If nWnd.Equals(ceroIntPtr) Then
        '    pruebaResultado.Text = "Aplicación de hacienda no se lanzó"
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
        '                    retval4 = SendMessage(hWnd4, WM_SETTEXT, IntPtr.Zero, loginSAT) 'loginSAT
        '                    hWnd5 = FindWindowEx(nWnd, hWnd4, "ThunderRT6TextBox", "") 'tx: archivo local
        '                    If Not hWnd5.Equals(ceroIntPtr) Then
        '                        retval5 = SendMessage(hWnd5, WM_SETTEXT, IntPtr.Zero, archivoLocal)
        '                        SetActiveWindow(nWnd)
        '                        hWnd6 = FindWindowEx(nWnd, IntPtr.Zero, "ThunderRT6CommandButton", "&Procesar") 'Procesar (&Subrayado)
        '                        If Not hWnd6.Equals(ceroIntPtr) Then
        '                            retval6 = SendNotifyMessage(hWnd6, BM_CLICK, IntPtr.Zero, 0)   'HABILITAR CLIC TX: aqui marca ERROR: 2012.08.21  12:20; Host Remoto=200.33.74.165  (11) +ERROR+ Transmision, Seguridad Afectada: Cliente: No se localiza el dueto (IDESERVER$) -> (cajapoix5240) en la Matriz Cliente. xq si lanza la aplicacion, envia los datos, los lee, pero en el boton Procesar valida algun valor/variable/parametro/registro/sesion/etc no se que sea que toma el nombre del equipo con un $ al final ( IDESERVER$ ) cuando deberia poner ( Administrator ) ¿de donde leen este valor? para nosotros ponerle Administrator a esa propiedad , siendo que antes de lanzar la aplicacion ya hice la impersonasion/ ejecucion de programa como usuario Administrador durante todo su procesamiento
        '                            Dim Handle As IntPtr = Marshal.AllocHGlobal(500)
        '                            Dim resultado As String
        '                            Dim numText As IntPtr
        '                            Dim tam As IntPtr
        '                            tam = 500
        '                            Do
        '                                numText = SendMessage(hWnd3, WM_GETTEXT, tam, Handle)    'resultados del comando                                        
        '                                resultado = Marshal.PtrToStringUni(Handle)
        '                            Loop While resultado.Equals("")     'vs tiempo fijo
        '                            pruebaResultado.Text = resultado
        '                            retval = SendMessage(nWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero)    'cerrar
        '                        Else
        '                            pruebaResultado.Text = "Componente Enviar declaracion/Procesar no localizado"
        '                        End If
        '                    Else
        '                        pruebaResultado.Text = "Componente cuenta sat / login transmisor no localizado"
        '                    End If
        '                Else
        '                    pruebaResultado.Text = "Componente archivo declaracion / archivo local no localizado"
        '                End If
        '            Else
        '                pruebaResultado.Text = "Componente de mensajes de aplicacion SAT no localizado"
        '            End If
        '        Else
        '            pruebaResultado.Text = "Componente cuenta sat / login acuses no localizado"
        '        End If
        '    Else
        '        pruebaResultado.Text = "Componente repositorio/directorio acuses no localizado"
        '    End If
        'End If

        'imp.ImpersonateStop()
    End Sub

    Protected Sub pruebaAcuse_Click(sender As Object, e As EventArgs) Handles pruebaAcuse.Click
        Dim directorioServidor
        Dim q = "SELECT directorioServidor FROM clientes WHERE id=" + Session("GidCliente").ToString
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        directorioServidor = "C:\SAT\" + v

        Dim NewCopy As String
        NewCopy = directorioServidor + "\prueba.txt"
        If Not System.IO.File.Exists(NewCopy) = True Then
            acuseResultado.Text = "Antes realice la prueba de envío"
        Else
            Dim dirs As String() = Directory.GetFiles(directorioServidor, "*.xml")
            If dirs.Length > 0 Then
                acuseResultado.Text = "Acuses encontrados"
            Else
                acuseResultado.Text = "No hay acuses todavia"
            End If
        End If

        'Dim loginSAT, directorioServidor, casfim, tipo, idArch
        'Dim q = "SELECT loginSAT,directorioServidor,casfim FROM clientes WHERE id=" + session("GidCliente").ToString
        'myCommand = New SqlCommand(q, myConnection)
        'dr = myCommand.ExecuteReader()
        'dr.Read()
        'loginSAT = dr("loginSAT")
        'directorioServidor = "C:\SAT\" + dr("directorioServidor")
        'casfim = dr("casfim")
        'dr.Close()

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
        'System.Threading.Thread.Sleep(3000)

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
        '    acuseResultado.Text = "Aplicación de hacienda no se lanzó"
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
        '                            elcorreo.Subject = "Decl. de prueba ERROR_ACUSE"
        '                            elcorreo.Body = "<html><body>cliente=" + session("curCorreo") + ", error=" + resultado + "+. Validar haber hecho 1o la prueba de envio</body></html>"
        '                            elcorreo.IsBodyHtml = True
        '                            elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        '                            Dim smpt As New System.Net.Mail.SmtpClient
        '                            smpt.Host = "smtp.gmail.com"
        '                            smpt.Port = "587"
        '                            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
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
        '                        acuseResultado.Text = "Error bajando acuses: " + resultado

        '                    Else
        '                        acuseResultado.Text = "Acuses se ejecutó correctamente" 'ver acuses en su directorio
        '                    End If
        '                    retval = SendMessage(nWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero)    'cerrar
        '                Else
        '                    'Response.Write("<script language='javascript'>alert('Componente Acuses no localizado');</script>")
        '                    acuseResultado.Text = "Componente Boton Recibe Acuses no localizado"
        '                End If
        '            Else
        '                'Response.Write("<script language='javascript'>alert('Componente Resultados no localizado');</script>")
        '                acuseResultado.Text = "Componente de mensajes de aplicacion SAT no localizado"
        '            End If

        '            'el campo de resultados en la vers ant del testacusevb era un text, aqui es un caption/label/static
        '        Else
        '            'Response.Write("<script language='javascript'>alert('Componente login acuses no localizado');</script>")
        '            acuseResultado.Text = "Componente cuenta sat / login remoto acuses no localizado"
        '        End If
        '    Else
        '        'Response.Write("<script language='javascript'>alert('Componente directorio no localizado');</script>")
        '        acuseResultado.Text = "Componente repositorio/ruta/directorio acuses no localizado"
        '    End If
        'End If
    End Sub




    Private Function referencia(ByVal nCliente) As String
        Try
            'AddFileSecurity("C:\SAT\referencias.xlsx", Session("identidad"), FileSystemRights.ReadData, AccessControlType.Allow)
            Dim excel As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application
            Dim w As Workbook = excel.Workbooks.Open("C:\SAT\referencias.xlsx")
            Dim sheet As Worksheet = w.Sheets(1)
            Dim r As Microsoft.Office.Interop.Excel.Range = sheet.UsedRange
            Dim array(,) As Object = r.Value(XlRangeValueDataType.xlRangeValueDefault)
            w.Close(False)   'cierro excel y trabajo con la var
            If array IsNot Nothing Then
                Return array(nCliente, 3).ToString.Trim()
            End If

        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('" + ex.Message + "');</script>")
        End Try
        Return ""
    End Function

    Protected Sub aprobarEnvio_Click(sender As Object, e As EventArgs) Handles aprobarEnvio.Click
        Dim q
        q = "update clientes set transmisionOk=1 where id=" + idParam.ToString
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)
        transmisionOk.Checked = True
    End Sub

    Protected Sub aprobarRecepcion_Click(sender As Object, e As EventArgs) Handles aprobarRecepcion.Click
        Dim q
        q = "update clientes set recepcionOk=1 where id=" + idParam.ToString
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)
        recepcionOk.Checked = True
    End Sub

    Protected Sub guiarDecla_Click(sender As Object, e As EventArgs) Handles guiarDecla.Click
        myCommand = New SqlCommand("update clientes set contactadoPguiarDecl=1 where id=" + idParam.ToString)
        ExecuteNonQueryFunction(myCommand)
        contactadoPguiarDecl.Checked = True
    End Sub

    Protected Sub confirmarRecibida_Click(sender As Object, e As EventArgs) Handles confirmarRecibida.Click
        Dim q
        q = "update clientes set solSockConfirmadaSAT=1 where id=" + idParam.ToString
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)
        solSockConfirmadaSAT.Checked = True
    End Sub

    Protected Sub recepcionOk_CheckedChanged(sender As Object, e As EventArgs) Handles recepcionOk.CheckedChanged

    End Sub
    Protected Sub ayuda_Click(sender As Object, e As EventArgs) Handles ayuda.Click
        frame1.Attributes("src") = "declaAyu.aspx"
    End Sub
    Protected Sub ayuda2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ayuda2.Click
        frame1.Attributes("src") = "declaAyu2.aspx"
    End Sub

    Protected Sub mod_Click1(sender As Object, e As EventArgs) Handles [mod].Click

    End Sub

    Protected Sub mod_Click2(sender As Object, e As EventArgs) Handles [mod].Click

    End Sub

    Protected Sub mod_Click3(sender As Object, e As EventArgs) Handles [mod].Click

    End Sub

    Protected Sub mod_Click4(sender As Object, e As EventArgs) Handles [mod].Click

    End Sub

    Private Sub btnActOtros_Click(sender As Object, e As EventArgs) Handles btnActOtros.Click
        Dim q = "UPDATE clientes SET otrosCorreos='" + otrosCorreos.Text.Trim.ToUpper + "' WHERE id=" + idParam.ToString
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)
        Response.Write("<script language='javascript'>alert('ok');</script>")
    End Sub

    Protected Sub mod_Click5(sender As Object, e As EventArgs) Handles [mod].Click

    End Sub

    Protected Sub mod_Click6(sender As Object, e As EventArgs) Handles [mod].Click

    End Sub

    Private Sub facTercero_CheckedChanged(sender As Object, e As EventArgs) Handles facTercero.CheckedChanged
        If facTercero.Checked Then
            facPanel.Visible = True
        Else
            facPanel.Visible = False
        End If
    End Sub

    Protected Sub desValidaCarta_Click(sender As Object, e As EventArgs) Handles desValidaCarta.Click
        myCommand = New SqlCommand("UPDATE clientes set solSocketEstatus='VACIA' where id=" + idParam.ToString)
        ExecuteNonQueryFunction(myCommand)
        Response.Write("<script language='javascript'>alert('ok, ahora borra el archivo " + Session("curCorreo") + ".pdf.ZIP y el .pdf" + "');</script>")
    End Sub

    Protected Sub facRetens_CheckedChanged(sender As Object)

    End Sub

    Protected Sub mod_Click7(sender As Object, e As EventArgs) Handles [mod].Click

    End Sub

    Protected Sub ayuda22_Click(sender As Object, e As EventArgs) Handles ayuda22.Click
        frame1.Attributes("src") = "declaAyu22.aspx"
    End Sub

    Private Sub formaPresentacionChange()
        If formaPresentacion.SelectedValue = "Subir FIEL" Then
            FileUploadFiel.Visible = True
            subirFiel.Visible = True
            remoto.Visible = False
            idNumRemoto.Visible = False
            passRemoto.Visible = False
            rutaFiel.Visible = False
            whats.Visible = False
            fielUp.Visible = True
            'fielBajar.Visible = True
            lblConexion.Visible = False
            lblIdNum.Visible = False
            lblContra.Visible = False
            lblRuta.Visible = False
            LabelW.Visible = False
        ElseIf formaPresentacion.SelectedValue = "Conexion remota" Then
            FileUploadFiel.Visible = False
            subirFiel.Visible = False
            remoto.Visible = True
            idNumRemoto.Visible = True
            passRemoto.Visible = True
            rutaFiel.Visible = True
            whats.Visible = True
            fielUp.Visible = False
            'fielBajar.Visible = False
            lblConexion.Visible = True
            lblIdNum.Visible = True
            lblContra.Visible = True
            lblRuta.Visible = True
            LabelW.Visible = True
        Else 'cliente
            FileUploadFiel.Visible = False
            subirFiel.Visible = False
            remoto.Visible = False
            idNumRemoto.Visible = False
            passRemoto.Visible = False
            rutaFiel.Visible = False
            whats.Visible = False
            fielUp.Visible = False
            'fielBajar.Visible = False
            lblConexion.Visible = False
            lblIdNum.Visible = False
            lblContra.Visible = False
            lblRuta.Visible = False
            LabelW.Visible = False
        End If
    End Sub

    Protected Sub formaPresentacion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles formaPresentacion.SelectedIndexChanged
        formaPresentacionChange()
    End Sub

    Protected Sub passRemoto_TextChanged(sender As Object, e As EventArgs) Handles passRemoto.TextChanged

    End Sub

    Protected Sub subirFiel_Click(sender As Object, e As EventArgs) Handles subirFiel.Click
        If casfim.Text.Trim = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", "<script language='javascript'>alert('no esta guardada la casfim');</script>", False)
            Exit Sub
        End If
        Dim nomArchAnualDatos = "C:\SAT\" + casfim.Text + "\fiel"
        Dim MSG
        myCommand = New SqlCommand("update clientes set fielUp=" + IIf(fielUp.Checked, "1", "0") + " where correo='" + Session("curCorreo") + "'")
        ExecuteNonQueryFunction(myCommand)
        Dim archDest
        If FileUploadFiel.HasFile Then
            Dim fileSize As Integer = FileUploadFiel.PostedFile.ContentLength
            Dim fileName As String = Server.HtmlEncode(FileUploadFiel.FileName)
            Dim extension As String = System.IO.Path.GetExtension(fileName)
            If extension.ToUpper <> ".ZIP" Then
                MSG = "<script language='javascript'>alert('el archivo debe ser comprimido tipo .zip e incluir el .key, .cer y la contrasena de la fiel');</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
                Exit Sub
            End If
            archDest = nomArchAnualDatos + extension
            If File.Exists(archDest) Then
                File.Delete(archDest)
            End If
            If (Not System.IO.Directory.Exists("C:\SAT\" + casfim.Text)) Then
                System.IO.Directory.CreateDirectory("C:\SAT\" + casfim.Text)
            End If
            Try
                FileUploadFiel.SaveAs(archDest)
                fielUp.Checked = True
                myCommand = New SqlCommand("update clientes set fielUp=1 where correo='" + Session("curCorreo") + "'")
                ExecuteNonQueryFunction(myCommand)
            Catch ex As Exception
                MSG = "<script language='javascript'>alert('" + ex.Message + "');</script>"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Finally
                FileUploadFiel.PostedFile.InputStream.Flush()
                FileUploadFiel.PostedFile.InputStream.Close()
                FileUploadFiel.FileContent.Dispose()
                FileUploadFiel.Dispose()
            End Try
        End If
        MSG = "<script language='javascript'>alert('guardado, lo encriptaremos');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
    End Sub

    Protected Sub fielBajar_Click(sender As Object, e As EventArgs) Handles fielBajar.Click
        If casfim.Text.Trim = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", "<script language='javascript'>alert('no esta guardada la casfim');</script>", False)
            Exit Sub
        End If
        Dim nomArchAnualDatos = "C:\SAT\" + casfim.Text + "\fiel.zip"
        If Not File.Exists(nomArchAnualDatos) Then
            Dim MSG = "<script language='javascript'>alert('no se encontro la fiel');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Exit Sub
        End If
        Dim file1 As System.IO.FileInfo = New System.IO.FileInfo(nomArchAnualDatos)
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
End Class