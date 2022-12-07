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

        If InStr(UsersEmail.Text, "SELECT") > 0 Or InStr(UsersEmail.Text, "INSERT") > 0 Or InStr(UsersEmail.Text, "UPDATE") > 0 Or InStr(UsersEmail.Text, "DELETE") > 0 Or InStr(UsersEmail.Text, "DROP") > 0 Then
            Response.Write("<script language='javascript'>alert('No use palabras reservadas');</script>")
            Exit Sub
        End If

        Dim q, elpass, larazonsoc

        'Encriptacion
        myCommand = New SqlCommand("OPEN SYMMETRIC KEY SYM_KEY DECRYPTION BY PASSWORD ='##Djjcp##'", myConnection)
        myCommand.ExecuteNonQuery()

        q = "SELECT CAST(DECRYPTBYKEY(passWeb) AS VARCHAR(15)) as CripPassWeb, razonSoc FROM clientes WHERE correo=@corr"
        myCommand = New SqlCommand(q, myConnection)
        myCommand.Parameters.AddWithValue("@corr", UsersEmail.Text.ToUpper.Trim)
        dr = myCommand.ExecuteReader()

        If dr.Read() Then
            elpass = dr("CripPassWeb").ToString()
            larazonsoc = dr("razonSoc").ToString()
        Else
            dr.Close()
            myCommand = New SqlCommand("CLOSE SYMMETRIC KEY SYM_KEY", myConnection)
            myCommand.ExecuteNonQuery()
            Response.Write("<script language='javascript'>alert('Correo no esta registrado en nuestro sistema');</script>")
            Exit Sub
        End If
        dr.Close()
        myCommand = New SqlCommand("CLOSE SYMMETRIC KEY SYM_KEY", myConnection)
        myCommand.ExecuteNonQuery()


        Dim elcorreo As New System.Net.Mail.MailMessage
        elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
        elcorreo.To.Add(UsersEmail.Text.Trim)
        elcorreo.Subject = "Recuperacion de password para su cuenta en declaracioneside.com"
        elcorreo.Body = "<html><body>Hola <b>" + larazonsoc + "</b><br><br> De acuerdo a nuestros registros, Usted solicitó recuperar su password, el cual es: " + elpass + " <br><br> Si tiene preguntas o problemas contactenos<br><br> !Gracias! <br><br><b> <a href='declaracioneside.com'>declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet </b></body></html>"
        elcorreo.IsBodyHtml = True
        elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        Dim smpt As New System.Net.Mail.SmtpClient
        smpt.Host = "smtp.gmail.com"
        smpt.Port = "587"
        smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
        smpt.EnableSsl = True 'req p server gmail
        Try
            smpt.Send(elcorreo)
            Response.Write("<script language='javascript'>alert('Se ha enviado la informacion requerida a su correo, agregue/guarde a declaracioneside@gmail.com en su lista de contactos, y revise su carpeta de spam y marque esta direccion como segura/conocida/no spam');</script>")
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
            Exit Sub
        End Try
        Response.Write("<script>location.href = 'Login.aspx';</script>")
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';")
        myConnection.Open()
        myCommand = New SqlCommand("set dateformat ymd", myConnection)
        myCommand.ExecuteNonQuery()

    End Sub

    Private Sub WebForm25_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        myConnection.Close()
    End Sub
End Class