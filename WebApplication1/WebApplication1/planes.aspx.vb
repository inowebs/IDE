Imports System.Runtime.InteropServices
Imports System.Security
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Public Class WebForm2
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim dr As SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';")
        myConnection.Open()

        myCommand = New SqlCommand("set dateformat ymd", myConnection)
        myCommand.ExecuteNonQuery()

    End Sub

    Protected Sub btnOptin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOptin.Click
        'no se guardan dx de facturacion en la BD xq es cliente eventual
        If validaVacios() < 1 Then
            Exit Sub
        End If

        If validaDupl() < 1 Then
            Exit Sub
        End If

        Dim lostels
        If tel.Text = "" Then
            lostels = " "
        Else
            lostels = tel.Text
        End If

        'pago sin referencia, no todos son clientes o antes de serlo
        Dim q = "SELECT ivaPorcen FROM actuales"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        Dim ivaActual = dr("ivaPorcen")
        dr.Close()
        Dim asesoriaPrecioNeto = asesoriaPrecioBase * (1 + ivaActual / 100)
        Session("Gasesoria") = Math.Round(asesoriaPrecioNeto) 'redondea al entero mas proximo

        q = "INSERT INTO prospectos(correo, nombre, tels, motivo, edoAsesoria) VALUES('" + Trim(correo.Text.ToUpper) + "','" + Trim(nombre.Text.ToUpper) + "', '" + lostels + "','A','SO')"
        myCommand = New SqlCommand(q, myConnection)
        myCommand.ExecuteNonQuery()

        Dim elcorreo As New System.Net.Mail.MailMessage
        elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
        elcorreo.To.Add(correo.Text.Trim.ToUpper)
        elcorreo.Bcc.Add("declaracioneside@gmail.com")
        elcorreo.Subject = "Solicitud de asesoria especializada para declaraciones e informes por depósitos en efectivo"
        elcorreo.Body = "<html><body>Hola " + nombre.Text.Trim.ToUpper + ", Tel. " + tel.Text.Trim + "<br><br>Bienvenido,<br><br> Hemos recibodo su solicitud, en breve nos pondremos en contacto con Ud. Le mantendremos al tanto de mas infomación relevante, si no desea seguirlas recibiendo haga clic en <a href='declaracioneside.com/unsuscribe.aspx?u=" + nombre.Text.Trim.ToUpper + "&c=" + correo.Text.Trim.ToUpper + "'>darme de baja</a><br><br>Atentamente <a href='declaracioneside.com'>declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
        elcorreo.IsBodyHtml = True
        elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        Dim smpt As New System.Net.Mail.SmtpClient
        smpt.Host = "smtp.gmail.com"
        smpt.Port = "587"
        smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a.")
        smpt.EnableSsl = True 'req p server gmail
        Try
            smpt.Send(elcorreo)
            Response.Write("<script language='javascript'>alert('Se ha enviado copia de la solicitud a su correo');</script>")
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('Error enviando solicitud: " & ex.Message + ", intente mas tarde');</script>")
            Exit Sub
        End Try

        'Response.Write("<script>location.href='pagoAsesoria.aspx';</script>")

    End Sub

    Function IsValidEmail(ByVal strIn As String) As Boolean
        ' Return true if strIn is in valid e-mail format.
        Return Regex.IsMatch(strIn, ("^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
    End Function

    Private Function validaVacios() As Integer
        If Trim(correo.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique su correo');</script>")
            correo.Focus()
            Return 0
        End If

        If IsValidEmail(Trim(correo.Text)) = False Then
            Response.Write("<script language='javascript'>alert('Formato de correo incorrecto');</script>")
            correo.Focus()
            Return 0
        End If

        If Trim(nombre.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique su nombre');</script>")
            nombre.Focus()
            Return 0
        End If

        Return 1
    End Function


    Private Function validaDupl() As Integer
        Dim q
        q = "SELECT * FROM prospectos WHERE correo='" + Trim(correo.Text.ToUpper) + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            Response.Write("<script language='javascript'>alert('Ya se había anotado');</script>")
            Return 0
        End If
        dr.Close()

        Return 1
    End Function

    Private Sub WebForm15_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        myConnection.Close()
    End Sub
End Class