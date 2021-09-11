Imports System.Data
Imports System.Data.SqlClient

Public Class WebForm2
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim dr As SqlDataReader

    Dim adapter As New SqlDataAdapter(myCommand)
    Dim dataSet As DataSet
    Dim tb As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        myConnection = New SqlConnection("server=job-pc;database=ide;Integrated Security=SSPI;")
        myConnection.Open()

        If Not IsPostBack Then  '1a vez
            'numSucursales.Attributes.Add("onBlur", "ceros(" + numSucursales.ClientID + ")")

        End If

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click

        If validaVacios() < 1 Then
            Exit Sub
        End If

        If validaDupl() < 1 Then
            Exit Sub
        End If

        Dim esInstitCreditoVal, q
        If esInstitCredito.Checked = False Then
            esInstitCreditoVal = "0"
        Else
            esInstitCreditoVal = "1"
        End If
        q = "INSERT INTO clientes(correo, passWeb, razonSoc, contacto, puesto, tel, cel, paginaWeb, rfcDeclarante, domFiscal, numSucursales, numSociosClientes, casfim, esInstitCredito, fechaRegistro, solSocketEstatus, directorioServidor) VALUES('" + Trim(correo.Text.ToUpper) + "','" + Trim(passWeb.Text) + "','" + Trim(razonSoc.Text.ToUpper) + "','" + Trim(contacto.Text.ToUpper) + "','" + Trim(puesto.Text.ToUpper) + "','" + Trim(tel.Text.ToUpper) + "','" + Trim(cel.Text) + "','" + Trim(paginaWeb.Text.ToUpper) + "','" + Trim(rfcDeclarante.Text.ToUpper) + "','" + Trim(domFiscal.Text.ToUpper) + "','" + Trim(Replace(numSucursales.Text, ",", "")) + "','" + Trim(Replace(numSociosClientes.Text, ",", "")) + "','" + Trim(casfim.Text.ToUpper) + "'," + esInstitCreditoVal + ",'" + Format(Now(), "yyyy-MM-dd") + "','VACIA','" + Trim(casfim.Text.ToUpper) + "')"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        'If dr.Count Then
        dr.Close()
        MsgBox("Registro exitoso", , "")
        Response.Redirect("~/Account/Login.aspx")
    End Sub

    Function IsValidEmail(ByVal strIn As String) As Boolean
        ' Return true if strIn is in valid e-mail format.
        Return Regex.IsMatch(strIn, ("^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
    End Function

    Private Function validaVacios() As Integer
        If Trim(correo.Text) = "" Then
            MsgBox("Especifique el correo", , "")
            correo.Focus()
            Return 0
        End If

        If IsValidEmail(Trim(correo.Text)) = False Then
            MsgBox("Formato de correo incorrecto", , "")
            correo.Focus()
            Return 0
        End If

        If Trim(razonSoc.Text) = "" Then
            MsgBox("Especifique la razon social", , "")
            razonSoc.Focus()
            Return 0
        End If
        If Trim(rfcDeclarante.Text) = "" Then
            MsgBox("Especifique el RFC", , "")
            rfcDeclarante.Focus()
            Return 0
        End If
        If Trim(rfcDeclarante.Text).Length < 9 Or Trim(rfcDeclarante.Text).Length > 12 Then
            MsgBox("Longitud de rfc Declarante entre 9-12 caracteres", , "")
            rfcDeclarante.Focus()
            Return 0
        End If
        If Not Regex.IsMatch(rfcDeclarante.Text.ToUpper.Trim, "^([A-Z\s]{3})\d{6}([A-Z\w]{3})$") Then
            MsgBox("Formato de rfc invalido", , "")
            rfcDeclarante.Focus()
            Return 0
        End If
        If Trim(passWeb.Text) = "" Then
            MsgBox("Especifique el password", , "")
            passWeb.Focus()
            Return 0
        End If
        If Trim(passWeb.Text).Length < 6 Then
            MsgBox("Longitud minima de password de 6 caracteres", , "")
            passWeb.Focus()
            Return 0
        End If
        If passWeb.Text.Trim <> passWeb2.Text.Trim Then
            MsgBox("El password y su confirmación no coinciden", , "")
            passWeb.Focus()
            Return 0
        End If
        If Trim(contacto.Text) = "" Then
            MsgBox("Especifique el contacto", , "")
            contacto.Focus()
            Return 0
        End If
        If Trim(puesto.Text) = "" Then
            MsgBox("Especifique el puesto", , "")
            puesto.Focus()
            Return 0
        End If
        If Trim(tel.Text) = "" Then
            MsgBox("Especifique el teléfono", , "")
            tel.Focus()
            Return 0
        End If
        If Trim(domFiscal.Text) = "" Then
            MsgBox("Especifique el domicilio fiscal", , "")
            domFiscal.Focus()
            Return 0
        End If
        If Trim(casfim.Text) = "" Then
            MsgBox("Especifique la clave CASFIM", , "")
            casfim.Focus()
            Return 0
        End If

        Return 1
    End Function


    Private Function validaDupl() As Integer
        Dim q
        q = "SELECT correo, razonSoc, rfcDeclarante, casfim FROM clientes WHERE correo='" + Trim(correo.Text.ToUpper) + "' OR razonSoc='" + Trim(razonSoc.Text.ToUpper) + "' OR rfcDeclarante='" + Trim(rfcDeclarante.Text.ToUpper) + "' OR casfim='" + Trim(casfim.Text.ToUpper) + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            'dr(0).ToString()
            'grid1.datasource=dr
            'grid1.DataBind()
            MsgBox("Ya existe un usuario registrado con esas llaves", , "")
            Return 0
        End If
        dr.Close()

        Return 1
    End Function

    Private Sub WebForm2_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        myConnection.Close()
    End Sub

    Protected Sub numSucursales_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles numSucursales.TextChanged

    End Sub
End Class