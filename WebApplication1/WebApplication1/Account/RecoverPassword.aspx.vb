Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Web.Mail

Public Class WebForm25
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim dr As SqlDataReader


    Protected Sub SendEmail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SendEmail.Click

        If Trim(UsersEmail.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el correo');</script>")
            UsersEmail.Focus()
            Exit Sub
        End If

        Dim q, elpass, larazonsoc
        q = "SELECT passWeb,razonSoc FROM clientes WHERE correo='" + UsersEmail.Text.ToUpper.Trim + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            elpass = dr("passWeb").ToString()
            larazonsoc = dr("razonSoc").ToString()
        Else
            Response.Write("<script language='javascript'>alert('Correo no esta registrado en nuestro sistema');</script>")
            Exit Sub
        End If
        dr.Close()


        Dim elcorreo As New System.Net.Mail.MailMessage
        elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
        elcorreo.To.Add(UsersEmail.Text.Trim.ToUpper)
        elcorreo.Subject = "Recuperacion de password para su cuenta en declaracioneside.com"
        elcorreo.Body = "<html><body>Hola <b>" + larazonsoc + " (" + UsersEmail.Text + ")</b><br><br> De acuerdo a nuestros registros, Usted solicitó recuperar su password, el cual es: " + elpass + " <br><br> Si tiene preguntas o problemas contactenos<br><br> !Gracias! <br><br><b> <a href='http://declaracioneside.com'>declaracioneside.com</a><br>Lider en envío de declaraciones del IDE por internet </b></body></html>"
        elcorreo.IsBodyHtml = True
        elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        Dim smpt As New System.Net.Mail.SmtpClient
        smpt.Host = "smtp.gmail.com"
        'smpt.Port = "465"
        smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2")
        smpt.EnableSsl = True 'req p server gmail
        Try
            smpt.Send(elcorreo)
            Response.Write("<script language='javascript'>alert('Se ha enviado la informacion requerida a su correo');</script>")
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
            Exit Sub
        End Try
        Response.Write("<script>location.href = 'Login.aspx';</script>")
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        myConnection = New SqlConnection("server=" + nombreServidor + ";database=ide;User ID=usuario;Password=USUARIO;")
        myConnection.Open()
        myCommand = New SqlCommand("set dateformat ymd", myConnection)
        myCommand.ExecuteNonQuery()

    End Sub

    Private Sub WebForm25_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        myConnection.Close()
    End Sub
End Class