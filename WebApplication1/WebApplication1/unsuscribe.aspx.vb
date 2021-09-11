Imports System.Data
Imports System.Data.SqlClient

Public Class WebForm22
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

    Function IsValidEmail(ByVal strIn As String) As Boolean
        ' Return true if strIn is in valid e-mail format.
        Return Regex.IsMatch(strIn, ("^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
    End Function

    Private Sub baja_Click(sender As Object, e As EventArgs) Handles baja.Click
        If mail.Text.Trim = "" Then
            mail.Focus()
            Response.Write("<script language='javascript'>alert('ingrese el correo a dar de baja');</script>")
            Exit Sub
        End If

        If IsValidEmail(Trim(mail.Text)) = False Then
            Response.Write("<script language='javascript'>alert('Formato de correo incorrecto');</script>")
            mail.Focus()
            Exit Sub
        End If

        Dim elcorreo As New System.Net.Mail.MailMessage
        Using elcorreo
                elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                elcorreo.To.Add("declaracioneside@gmail.com")
                elcorreo.Subject = "Dar de Baja de lista de correos del IDE a: " + mail.Text.Trim
                elcorreo.Body = "<html><body>Actualizar lista excel en linea y lista negra</body></html>"
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
                Finally
                End Try
            End Using
            Dim MSG As String = "<script language='javascript'>alert('Registro de solicitud de baja enviado exitosamente');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)

    End Sub
End Class