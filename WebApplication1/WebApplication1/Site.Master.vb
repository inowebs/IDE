Imports Microsoft.Win32
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO
Imports Newtonsoft.Json.Linq
Public Class Site
    Inherits System.Web.UI.MasterPage
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim dr As SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then '1a vez  
            'If IsNothing(Session("curCorreo")) = True Then
            '    Response.Write("<script language='javascript'>alert('Expiró su sesión');</script>")
            '    Session.Abandon()
            '    Response.Write("<script language='javascript'>location.href='Login.aspx';</script>")
            '    Exit Sub
            'End If
            nombreServidor = "tcp:."
            myCommand = New SqlCommand("set dateformat ymd")
            ExecuteNonQueryFunction(myCommand)

            'If Not Request.QueryString("runAsAdmin") Is Nothing Then 'implementando hopads de administrador
            '    myCommand = New SqlCommand("SELECT id FROM admin WHERE namebre='" + Request.QueryString("runAsAdmin").ToString + "'", myConnection)
            '    dr = myCommand.ExecuteReader()
            '    If Not dr.Read() Then
            '        dr.Close()
            '        Session("runAsAdmin") = "0"
            '    Else
            '        dr.Close()
            '        Session("runAsAdmin") = "1"
            '    End If
            '    myConnection.Close()
            'Else
            '    If Session("runAsAdmin") Is Nothing Or Session("runAsAdmin") <> "1" Then
            '        Session("runAsAdmin") = "0"
            '    Else
            '        Session("runAsAdmin") = "1"
            '    End If
            'End If

            'Else
            '    If HyperLink3.Visible = True Then
            '        Session("runAsAdmin") = "1"
            '    Else
            '        Session("runAsAdmin") = "0"
            '    End If
        End If

        If Session("runAsAdmin") = "1" Then
            admin.Visible = True
            distri.Visible = True
        Else
            admin.Visible = False
            distri.Visible = False
        End If

    End Sub

    Sub HeadLoginStatus_LoggedOut(ByVal sender As Object, ByVal e As System.EventArgs)
        Session("curCorreo") = Nothing
        Session("GidCliente") = Nothing
        Session("GidAnual") = Nothing
        Session("GidContrato") = Nothing
        Session("GidMens") = Nothing
        If Session("runAsAdmin") = "1" Then
            Response.Redirect("~/login.aspx?lan=1")
        Else
            Response.Redirect("~/default.aspx")
        End If
    End Sub
    Private Function validaDuplSite() As Integer
        Dim q
        q = "SELECT id FROM prospeccion WHERE correo='" + Trim(email.Text.ToUpper) + "'"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            'Response.Write("<script language='javascript'>alert('Ya se había anotado');</script>")
            Return 0
        End If
        Return 1
    End Function
    Sub subSubmitForm()
        'Your code in here
        'Response.Write("Captcha ok")
        ''LLENANDO PETICION DE CAPTCHA

        Dim responseCaptcha = respHidden.Value 'primera forma mostrando el cptcha       
        'Dim responseHiden = respHidden.Value

        'If responseCaptcha Is Nothing Or responseCaptcha = "" Then
        '    'Procesamiento en el back del archivo json del response
        Dim secretKey = ConfigurationManager.AppSettings("CaptchaSecret").ToString
        Dim urlCapt = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}"
        Dim requestUri = String.Format(urlCapt, secretKey, responseCaptcha)
        Dim req = CType(WebRequest.Create(requestUri), HttpWebRequest)
        Using respuesta As WebResponse = req.GetResponse
            Using stream = New StreamReader(respuesta.GetResponseStream())
                Dim jresponse As JObject = JObject.Parse(stream.ReadToEnd)
                Dim isSuccess = jresponse.Value(Of Boolean)("success")
                If isSuccess = False Then
                    Response.Write("<script language='javascript'>alert('Confirma el captcha');</script>")
                    Exit Sub
                End If
            End Using
        End Using


        If Trim(name.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el namebre');</script>")
            name.Focus()
            Exit Sub
        End If
        If Trim(email.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el correo');</script>")
            email.Focus()
            Exit Sub
        End If
        If Trim(tele.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el teléfono');</script>")
            tele.Focus()
            Exit Sub
        ElseIf tele.Text.Trim.Length < 10 Then
            Response.Write("<script language='javascript'>alert('Favor de indicar la clave lada en el teléfono');</script>")
            tele.Focus()
            Exit Sub
        End If
        If Trim(message.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el mensaje');</script>")
            message.Focus()
            Exit Sub
        End If

        myCommand = New SqlCommand("set dateformat ymd")
        ExecuteNonQueryFunction(myCommand)

        If validaDuplSite() > 0 Then
            Dim q = "INSERT INTO prospeccion(cliente, idDistribuidor, estatusActual, fecha, notas, correo) VALUES('" + Trim(name.Text.ToUpper) + "',1,'VA','" + Format(Now, "yyyy-MM-dd") + "','BAJO BONO, " + tele.Text + "','" + Trim(email.Text.ToUpper) + "')"
            myCommand = New SqlCommand(q)
            ExecuteNonQueryFunction(myCommand)
        End If

        myCommand.Dispose()
        'Response.Write("<script>alert('Validado');</script>")
        Dim elcorreo As New System.Net.Mail.MailMessage
        If depto.SelectedValue.Equals("Ventas") Then
            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
            elcorreo.To.Add("declaracioneside@gmail.com")
        Else
            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
            elcorreo.To.Add("declaracioneside@gmail.com")
        End If
        elcorreo.Subject = "Contactado por " + Trim(name.Text)
        elcorreo.Body = "<html><body>Nombre: " + Trim(name.Text) + "<br>Correo: " + Trim(email.Text) + "<br> Tels: " + Trim(tele.Text) + "<br>Mensaje: <br>" + Trim(message.Text) + "</body></html>"
        elcorreo.IsBodyHtml = True
        elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        Dim smpt As New System.Net.Mail.SmtpClient
        smpt.Host = "smtp.gmail.com"
        smpt.Port = "587"
        smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
        smpt.EnableSsl = True 'req p server gmail
        Try
            smpt.Send(elcorreo)
            Response.Write("<script language='javascript'>alert('Mensaje enviado, pronto estaremos en contacto contigo');</script>")
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
            'Exit Sub
        End Try
        'Response.Write("<script>location.href = 'Default.aspx';</script>")
    End Sub

    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        subSubmitForm()
    End Sub
    Function IsValidEmail(ByVal strIn As String) As Boolean
        ' Return true if strIn is in valid e-mail format.
        Return Regex.IsMatch(strIn, ("^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
    End Function

    Private Sub recomendar_Click(sender As Object, e As EventArgs) Handles recomendar.Click
        'Dim responseCaptcha = Request.Form("g-recaptcha-response") 'primera forma mostrando el cptcha       
        'Dim responseHiden = respHidden.Value

        'If responseCaptcha Is Nothing Or responseCaptcha = "" Then
        '    'Procesamiento en el back del archivo json del response
        '    Dim secretKey = ConfigurationManager.AppSettings("CaptchaSecret").ToString
        '    Dim urlCapt = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}"
        '    Dim requestUri = String.Format(urlCapt, secretKey, responseCaptcha)
        '    Dim req = CType(WebRequest.Create(requestUri), HttpWebRequest)
        '    Using respuesta As WebResponse = req.GetResponse
        '        Using stream = New StreamReader(respuesta.GetResponseStream())
        '            Dim jresponse As JObject = JObject.Parse(stream.ReadToEnd)
        '            Dim isSuccess = jresponse.Value(Of Boolean)("success")
        '            If isSuccess = False Then
        '                Response.Write("<script language='javascript'>alert('Confirma el captcha');</script>")
        '                Exit Sub
        '            End If
        '        End Using
        '    End Using
        '    Response.Write("<script language='javascript'>alert('Confirma el captcha');</script>")
        '    Exit Sub
        'End If

        If Trim(amigoCorr.Text.Trim) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el correo');</script>")
            amigoCorr.Focus()
            Exit Sub
        End If
        If IsValidEmail(Trim(amigoCorr.Text.Trim)) = False Then
            Response.Write("<script language='javascript'>alert('Formato de correo incorrecto');</script>")
            amigoCorr.Focus()
            Exit Sub
        End If
        If amigoTel.Text <> "" And amigoTel.Text.Trim.Length < 10 Then
            Response.Write("<script language='javascript'>alert('Favor de incluir clave lada en el teléfono');</script>")
            amigoTel.Focus()
            Exit Sub
        End If

        If validaDupl() > 0 Then
            myCommand = New SqlCommand("set dateformat ymd")
            ExecuteNonQueryFunction(myCommand)
            Dim q = "INSERT INTO prospeccion(cliente, idDistribuidor, estatusActual, fecha, notas, correo) VALUES('" + Trim(amigoNom.Text.ToUpper) + "',1,'VA','" + Format(Now, "yyyy-MM-dd") + "','RECOMENDADO EN LINEA, " + amigoTel.Text.Trim + "','" + Trim(amigoCorr.Text.ToUpper) + "')"
            myCommand = New SqlCommand(q)
            ExecuteNonQueryFunction(myCommand)

        End If
        myCommand.Dispose()


        Dim elcorreo As New System.Net.Mail.MailMessage
        elcorreo.From = New System.Net.Mail.MailAddress("ventas@inowebs.com")
        elcorreo.To.Add(amigoCorr.Text.Trim.ToUpper)
        elcorreo.Bcc.Add("ventas@inowebs.com")
        elcorreo.Subject = amigoNom.Text.Trim.ToUpper + ": Un amigo te recomienda esta excelete página para tu facturación electrónica, nómina y contabilidad electrónica"
        elcorreo.Body = "<html><body>Hola " + amigoNom.Text.Trim.ToUpper + "<br><br>Revisa nuestra información con datos muy importantes para ti, Somos Tu solución en Facturas electrónicas, nómina, CFDI, contabilidad electrónica<br><br><a href='facturaselectronicascfdi.com/presentacion.ppsx'>Acceder esta información ahora</a><br><br>Te mantendremos al tanto de mas infomación relevante, si no deseas seguirlas recibiendo haz clic en <a href='facturaselectronicascfdi.com/unsuscribe.aspx?u=" + amigoNom.Text.Trim.ToUpper + "&c=" + amigoCorr.Text.Trim.ToUpper + "'>darme de baja</a><br><br>Atentamente <a href='facturaselectronicascfdi.com'>Facturación Electrónica Inowebs</a><br>Innovación en Facturación electrónica</body></html>"
        elcorreo.IsBodyHtml = True
        elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        Dim smpt As New System.Net.Mail.SmtpClient
        smpt.Host = "mail.inowebs.com"
        smpt.Port = "26"
        smpt.Credentials = New System.Net.NetworkCredential("soporte@inowebs.com", "sinowebs")
        smpt.EnableSsl = True 'req p server gmail
        Try
            smpt.Send(elcorreo)
            Response.Write("<script language='javascript'>alert('Invitación enviada');</script>")
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('Error enviando envitación: " & ex.Message + ", intente mas tarde');</script>")
            'Exit Sub
        End Try
    End Sub
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
        'myConnection = New SqlConnection("Data Source=.;User ID=usuario;Password='SmN+v-XzFy2N;91E170o'; Initial Catalog=ide;Integrated Security=True;MultipleActiveResultSets=True;")
        q = "SELECT id FROM prospectos WHERE correo='" + Trim(correo.Text.ToUpper) + "'"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If Not IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('Ya se había anotado');</script>")
            Return 0
        End If
        Return 1
    End Function
    Private Sub btnOptin_Click(sender As Object, e As EventArgs) Handles btnOptin.Click
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
        myCommand = New SqlCommand("set dateformat ymd")
        ExecuteNonQueryFunction(myCommand)
        Dim q = "INSERT INTO prospeccion(cliente, idDistribuidor, estatusActual, fecha, notas, correo) VALUES('" + Trim(nombre.Text.ToUpper) + "',1,'VA','" + Format(Now, "yyyy-MM-dd") + "','BAJO BONO, " + lostels + "','" + Trim(correo.Text.ToUpper) + "')"
        myCommand = New SqlCommand(q)
        ExecuteNonQueryFunction(myCommand)

        Dim elcorreo As New System.Net.Mail.MailMessage
        elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
        elcorreo.To.Add(correo.Text.Trim.ToUpper)
        elcorreo.Bcc.Add("declaracioneside@gmail.com")
        elcorreo.Subject = "Información de primera mano del IDE"
        elcorreo.Body = "<html><body>Hola " + nombre.Text.Trim.ToUpper + ", " + tel.Text.Trim + "<br><br>Bienvenido,<br><br> Haga <a href='declaracioneside.com/bono1.docx'>clic aquí</a> para descargar un archivo con información valiosa <br><br><br>Le mantendremos al tanto de mas infomación relevante, si no desea seguirlas recibiendo haga clic en <a href='declaracioneside.com/unsuscribe.aspx?u=" + nombre.Text.Trim.ToUpper + "&contact=" + correo.Text.Trim.ToUpper + "'>darme de baja</a><br><br>Atentamente <a href='declaracioneside.com'>declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
        elcorreo.IsBodyHtml = True
        elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        Dim smpt As New System.Net.Mail.SmtpClient
        smpt.Host = "smtp.gmail.com"
        smpt.Port = "587"
        smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
        smpt.EnableSsl = True 'req p server gmail
        Try
            smpt.Send(elcorreo)
            Response.Write("<script language='javascript'>alert('Se ha enviado la información a su correo');</script>")
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('Error enviando información: " & ex.Message + ", intente mas tarde');</script>")
            Exit Sub
        End Try

    End Sub
End Class