Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Imaging
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Net
Imports Newtonsoft.Json.Linq
Imports System.IO

Public Class registro
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim dr As SqlDataReader

    Dim dataSet As DataSet
    Dim tb As DataTable

    Function fncDrawCaptcha(ByVal path As String) As String

        '*** Editable Values
        Dim BackgroundColor As [String]() = New [String]() {255, 255, 255} ' The 3 numbers represent in order RED, GREEN, BLUE for the captcha's background color
        Dim RandomBackgroundNoiseColor As Boolean = True ' True / False. If you choose True, BackgroundNoiseColor will not apply 
        Dim RandomTextColor As Boolean = True ' True / False. If you choose True, TextColor will not apply 
        Dim BackgroundNoiseColor As [String]() = New [String]() {150, 150, 150} ' The 3 numbers represent in order RED, GREEN, BLUE
        Dim TextColor As [String]() = New [String]() {200, 200, 200} ' The 3 numbers represent in order RED, GREEN, BLUE
        Dim BackgroundNoiseTexture As HatchStyle = HatchStyle.Min ' replace ".Min" with any of the following: Horizontal, Vertical, ForwardDiagonal, BackwardDiagonal, Cross, DiagonalCross, Percent05, Percent10, Percent20, Percent25, Percent30, Percent40, Percent50, Percent60, Percent70, Percent75, Percent80, Percent90, LightDownwardDiagonal, LightUpwardDiagonal, DarkDownwardDiagonal, DarkUpwardDiagonal, WideDownwardDiagonal, WideUpwardDiagonal, LightVertical, LightHorizontal, NarrowVertical, NarrowHorizontal, DarkVertical, DarkHorizontal, DashedDownwardDiagonal, DashedUpwardDiagonal, DashedHorizontal, DashedVertical, SmallConfetti, LargeConfetti, ZigZag, Wave, DiagonalBrick, HorizontalBrick, Weave, Plaid, Divot, DottedGrid, DottedDiamond, Shingle, Trellis, Sphere, SmallGrid, SmallCheckerBoard, LargeCheckerBoard, OutlinedDiamond, SolidDiamond, LargeGrid, Min, Max
        Dim length As Integer = 6 ' Number of characters to generate
        '*** END Editable Values

        Dim height As Integer = 100
        Dim width As Integer = 200
        width = width + ((length - 6) * 30)
        Dim ranRotate As New Random
        Dim strText As String = Left(Replace(System.Guid.NewGuid().ToString(), "-", ""), length)
        Dim bmpCanvas As New Bitmap(width, height, PixelFormat.Format24bppRgb)
        Dim graCanvas As Graphics = Graphics.FromImage(bmpCanvas)
        Dim recF As New RectangleF(0, 0, width, height)
        Dim bruBackground As Brush
        Dim letterBrush As SolidBrush

        graCanvas.TextRenderingHint = TextRenderingHint.AntiAlias

        If RandomBackgroundNoiseColor = True Then
            bruBackground = New HatchBrush(BackgroundNoiseTexture, Color.FromArgb((ranRotate.Next(0, 255)), (ranRotate.Next(0, 255)), (ranRotate.Next(0, 255))), Color.FromArgb(BackgroundColor(0), BackgroundColor(1), BackgroundColor(2)))
        Else
            bruBackground = New HatchBrush(BackgroundNoiseTexture, Color.FromArgb(BackgroundNoiseColor(0), BackgroundNoiseColor(1), BackgroundNoiseColor(2)), Color.FromArgb(BackgroundColor(0), BackgroundColor(1), BackgroundColor(2)))
        End If

        graCanvas.FillRectangle(bruBackground, recF)

        If RandomTextColor = True Then
            letterBrush = New SolidBrush(Color.FromArgb((ranRotate.Next(0, 255)), (ranRotate.Next(0, 255)), (ranRotate.Next(0, 255))))
        Else
            letterBrush = New SolidBrush(Color.FromArgb(TextColor(0), TextColor(1), TextColor(2)))
        End If

        Dim matRotate As New System.Drawing.Drawing2D.Matrix
        Dim i As Integer
        For i = 0 To Len(strText) - 1
            matRotate.Reset()
            matRotate.RotateAt(ranRotate.Next(-30, 30), New PointF(width / (Len(strText) + 1) * i, height * 0.5))
            graCanvas.Transform = matRotate
            If i = 0 Then
                graCanvas.DrawString(strText.Chars(i), New Font("Comic Sans MS", 25, FontStyle.Italic), letterBrush, width / (Len(strText) + 1) * i, height * 0.4) 'draw ‘the text on our image
            ElseIf i = 1 Then
                graCanvas.DrawString(strText.Chars(i), New Font("Arial", 30, FontStyle.Bold), letterBrush, width / (Len(strText) + 1) * i, height * 0.1) 'draw ‘the text on our image
            ElseIf i = 2 Then
                graCanvas.DrawString(strText.Chars(i), New Font("Times New Roman", 25, FontStyle.Italic), letterBrush, width / (Len(strText) + 1) * i, height * 0.5) 'draw ‘the text on our image
            ElseIf i = 3 Then
                graCanvas.DrawString(strText.Chars(i), New Font("Georgia", 35, FontStyle.Bold), letterBrush, width / (Len(strText) + 1) * i, height * 0.1) 'draw ‘the text on our image
            ElseIf i = 4 Then
                graCanvas.DrawString(strText.Chars(i), New Font("Verdana", 25, FontStyle.Italic), letterBrush, width / (Len(strText) + 1) * i, height * 0.5) 'draw ‘the text on our image
            ElseIf i = 5 Then
                graCanvas.DrawString(strText.Chars(i), New Font("Geneva", 30, FontStyle.Bold), letterBrush, width / (Len(strText) + 1) * i, height * 0.1) 'draw ‘the text on our image
            Else
                graCanvas.DrawString(strText.Chars(i), New Font("Arial", 30, FontStyle.Italic), letterBrush, width / Len(strText) * i, height * 0.5) 'draw ‘the text on our image
            End If
            graCanvas.ResetTransform()
        Next

        bmpCanvas.Save(path, ImageFormat.Gif)
        graCanvas.Dispose()
        bmpCanvas.Dispose()

        Return strText

    End Function

    'Sub cargaCaptcha()
    '    'Dim imgCaptcha As System.Web.UI.WebControls.Image
    '    'imgCaptcha = LoginUser.FindControl("imgCaptcha")

    '    Dim strPathToImage As String = "captchaimg/captcha.gif" ' Make sure the directory is writable by your web server!
    '    Dim strText As String = fncDrawCaptcha(Server.MapPath(strPathToImage))
    '    imgCaptcha.ImageUrl = strPathToImage
    '    Session.Add("strText", strText)
    '    imgCaptcha.Width = 200
    '    imgCaptcha.Height = 100
    'End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';MultipleActiveResultSets=True")
        myConnection.Open()
        myCommand = New SqlCommand("set dateformat ymd", myConnection)
        myCommand.ExecuteNonQuery()

        If Not Session("refDistribuidor") Is Nothing And Session("refDistribuidor") <> "1" Then
            idDistribuidor.Text = Session("refDistribuidor")
            idDistribuidor.Enabled = False
        End If

        'If Not IsPostBack Then  '1a vez
        'numSucursales.Attributes.Add("onBlur", "ceros(" + numSucursales.ClientID + ")")

        'Me.Button1.OnClientClick = "document.getElementById('form1').target = '_self'; return confirm('Verifique, ¿es correcto el correo '+document.getElementById('correo').value+' ?');"
        'End If
        If IsPostBack Then
            If Not String.IsNullOrEmpty(passWeb.Text.Trim()) Then
                passWeb.Attributes.Add("value", passWeb.Text)
            End If
            If Not String.IsNullOrEmpty(passWeb2.Text.Trim()) Then
                passWeb2.Attributes.Add("value", passWeb2.Text)
            End If
        Else
            'cargaCaptcha()
        End If

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        If Not String.IsNullOrEmpty(Request.QueryString("lan")) Then
            If Request.QueryString("lan") = "1" Then
                Session("runAsAdmin") = "1"
            End If
        End If

        If HttpContext.Current.Request.IsLocal Or Session("runAsAdmin") = "1" Then
        Else
            'If Not txtCaptcha.Text = Session.Item("strText") Then
            '    cargaCaptcha()
            '    txtCaptcha.Text = ""
            '    Response.Write("<script language='javascript'>alert('No coincidieron los caracteres de la imagen');</script>")
            '    Exit Sub
            'End If
            Dim secretKey = ConfigurationManager.AppSettings("CaptchaSecret").ToString
            Dim urlCapt = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}"
            Dim requestUri = String.Format(urlCapt, secretKey, responseRe.Value)
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
        End If

        If validaVacios() < 1 Then
            Exit Sub
        End If

        If validaDupl() < 1 Then
            Exit Sub
        End If

        If idDistribuidor.Text.Trim <> "" Then 'valida distribuidor autorizado
            myCommand = New SqlCommand("SELECT id FROM distribuidores WHERE id=" + idDistribuidor.Text.Trim + " and doctos=1", myConnection)
            dr = myCommand.ExecuteReader()
            If Not dr.Read() Then
                dr.Close()
                idDistribuidor.Focus()
                Response.Write("<script language='javascript'>alert('El # Distribuidor no existe o no está autorizado, verifiquelo o dejelo en blanco');</script>")
                Exit Sub
            End If
            dr.Close()
        End If

        Dim esInstitCreditoVal, q, casfimProvisionalVal
        If esInstitCredito.Checked = False Then
            esInstitCreditoVal = "0"
        Else
            esInstitCreditoVal = "1"
        End If

        If casfimProvisional.Checked = False Then
            casfimProvisionalVal = "0"
        Else
            casfimProvisionalVal = "1"
        End If

        Dim q2
        If idDistribuidor.Text.Trim <> "" Then
            q2 = "SELECT id FROM distribuidores WHERE id=" + idDistribuidor.Text.Trim
        Else
            q2 = "SELECT id FROM distribuidores WHERE nombreFiscal='DEFAULT'"
        End If
        myCommand = New SqlCommand(q2, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        idDistribuidor.Text = dr("id")
        dr.Close()

        q2 = "SELECT id FROM estatusCliente WHERE estatus='pend clave y carta'"
        myCommand = New SqlCommand(q2, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        Dim idEstatusCli = dr("id")
        dr.Close()


        'el pass se inserta vacio pero se actualiza en seguida al encriptar
        q = "INSERT INTO clientes(correo, razonSoc, contacto, puesto, tel, cel, paginaWeb, rfcDeclarante, domFiscal, numSucursales, numSociosClientes, casfim, esInstitCredito, fechaRegistro, solSocketEstatus, directorioServidor,idDistribuidor,casfimProvisional,fuente, idEstatus, ipsat) VALUES('" + Trim(correo.Text.ToUpper) + "','" + Trim(razonSoc.Text.ToUpper) + "','" + Trim(contacto.Text.ToUpper) + "','" + Trim(puesto.Text.ToUpper) + "','" + Trim(tel.Text.ToUpper) + "','" + Trim(cel.Text) + "','" + Trim(paginaWeb.Text.ToUpper) + "','" + Trim(rfcDeclarante.Text.ToUpper) + "','" + Trim(domFiscal.Text.ToUpper) + "','" + Trim(Replace(numSucursales.Text, ",", "")) + "','" + Trim(Replace(numSociosClientes.Text, ",", "")) + "','" + Trim(casfim.Text.ToUpper) + "'," + esInstitCreditoVal + ",'" + Format(Now(), "yyyy-MM-dd") + "','VACIA','" + Trim(casfim.Text.ToUpper) + "'," + idDistribuidor.Text.Trim + "," + casfimProvisionalVal + ",'" + fuente.Text.Trim + "'," + idEstatusCli.ToString + ",'200.57.3.165')"
        myCommand = New SqlCommand(q, myConnection)
        myCommand.ExecuteNonQuery()

        'encriptacion
        myCommand = New SqlCommand("DECLARE @KEYID UNIQUEIDENTIFIER SET @KEYID = KEY_GUID('SYM_KEY') OPEN SYMMETRIC KEY SYM_KEY DECRYPTION BY PASSWORD='##Djjcp##' UPDATE clientes SET passWeb=ENCRYPTBYKEY(@KEYID,'" + Trim(passWeb.Text) + "') WHERE correo='" + Trim(correo.Text.ToUpper) + "'", myConnection)
        myCommand.ExecuteNonQuery()
        myCommand = New SqlCommand("CLOSE SYMMETRIC KEY SYM_KEY", myConnection)
        myCommand.ExecuteNonQuery()


        q2 = "SELECT id FROM prospectos WHERE correo='" + correo.Text.Trim.ToUpper + "'" 'lo saco de prospectos 
        myCommand = New SqlCommand(q2, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            myCommand = New SqlCommand("DELETE FROM prospectos WHERE  correo='" + correo.Text.Trim.ToUpper + "'", myConnection)
            myCommand.ExecuteNonQuery()
        End If
        dr.Close()

        Dim iddistribuidorval
        If idDistribuidor.Text.Trim = "" Then
            iddistribuidorval = "1"
        Else
            iddistribuidorval = idDistribuidor.Text.Trim
        End If
        q2 = "SELECT id FROM prospeccion WHERE cliente LIKE '%" + razonSoc.Text.Trim.ToUpper + "%'" 'lo saco de prospectos 
        myCommand = New SqlCommand(q2, myConnection)
        dr = myCommand.ExecuteReader()
        If Not dr.HasRows Then
            dr.Close()
            myCommand = New SqlCommand("INSERT INTO prospeccion (cliente,idDistribuidor,estatusActual,fecha,notas,correo,fechaprogramada,tipo) VALUES ('" + razonSoc.Text.Trim.ToUpper + "'," + iddistribuidorval + ",'VA','" + Format(Now(), "yyyy-MM-dd") + "','RECIEN REGISTRADO Tel " + tel.Text.Trim + " Cel " + cel.Text.Trim + " Con " + contacto.Text.Trim + " Puesto " + puesto.Text.Trim + "','" + correo.Text.Trim.ToUpper + "','" + Format(Now(), "yyyy-MM-dd") + "',1)", myConnection)
            myCommand.ExecuteNonQuery()
        Else
            dr.Close()
        End If

        If casfim.Text.Trim <> "" Then
            If (Not System.IO.Directory.Exists("C:\SAT")) Then
                System.IO.Directory.CreateDirectory("C:\SAT")
            End If
            If (Not System.IO.Directory.Exists("C:\SAT\" + casfim.Text.Trim)) Then
                System.IO.Directory.CreateDirectory("C:\SAT\" + casfim.Text.Trim)
            End If
        End If

        Dim elcorreo As New System.Net.Mail.MailMessage
        elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
        elcorreo.To.Add(correo.Text.Trim.ToUpper)
        elcorreo.Bcc.Add("declaracioneside@gmail.com")
        elcorreo.Bcc.Add("ventas.declaracioneside@gmail.com")
        elcorreo.Subject = "Registro en DeclaracionesIde.com"
        elcorreo.Body = "<html><body>Hola " + razonSoc.Text.Trim.ToUpper + "<br><br>Bienvenido,<br><br> Su registro se procesó exitosamente en nuestro servidor, ahora puede acceder su cuenta desde el menú 'Ingresar->Iniciar sesión' de nuestra página<br>Los datos de su cuenta son: Correo = " + correo.Text.Trim.ToUpper + ", Contraseña = " + passWeb.Text.Trim + " <br><br>Una vez haya iniciado sesión, estando dentro del submenú Cuenta, desplacece a la sección 'Representante Legal' e introduzca los datos del representante legal actual de su institución y pulse en '+ Agregar', si en algun momento cambian de representante legal, agreguelo en mi cuenta y definalo como el actual; desplacece a la parte inferior hasta la sección autorización de socket, haga clic en 'Ver formato' para descargar el formato que requiere adecuar, firmar y enviarnos por el sistema para tramitarle y configurarle con el SAT su canal/matriz de conexión segura para transmisión de datos , luego clic en 'Seleccionar archivo o Examinar' para seleccionar un archivo escaneado en PDF con los datos rellenos en base al formato del paso anterior, luego clic en 'Subir solicitud' para enviarnolo por sistema, si este paso fue exitoso haga clic en 'Mostrar' para ver el archivo que recién subió.<br><br> Si recién acaba de tramitar su clave CASFIM envienos adjunta en un correo la pantalla donde le asignan dicha clave, que se vea la clave y el nombre o RFC de su empresa<br><br> Habiendo Ud. completado estos pasos, nosotros validemos su solicitud/formato de autorización, le notificaremos para que proceda a realizar los contratos que desee desde el submenú 'Mis contratos', una vez que tengamos su carta de autorización y su clave de IDE o de institucion financiera nosotros realizamos ante el SAT la gestión y configuración de su socket, lo cual se lleva aprox. 3 semanas<br><br>Una vez esté lista dicha configuración, Ud. será notificado para poder ingresar a su cuenta, realizar el pago de sus contratos, enviarnos el comprobante de pago y así pueda comenzar a enviar sus declaraciones de IDE. Es necesario que instale y descargue el navegador Chrome o Firefox <br><br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet<br>Tel 4436903616, 4432180237<br>Correo declaracioneside@gmail.com<br><a href='https://twitter.com/declaracionesid' target='_blank'><img src='declaracioneside.com/twitter.jpg' alt='Clic aquí, siguenos en twitter' Height='30px' Width='30px' BorderWidth ='0px'></a>&nbsp;<a href='http://www.youtube.com/user/declaracioneside' target='_blank'> <img src='declaracioneside.com/iconoyoutube.png'  alt='Suscribete a nuestro canal declaraciones de depósitos en efectivo e IDE en youtube' Height='30px' Width='30px' BorderWidth ='0px'></a> &nbsp;<a href='http://www.facebook.com/depositosenefectivo' target='_blank'><img src='declaracioneside.com/facebook.jpg' alt='Clic aquí para seguirnos en facebook' Height='30px' Width='30px' BorderWidth ='0px'></a>&nbsp;&nbsp;<a href='https://mx.linkedin.com/in/declaraciones-depósitos-en-efectivo-1110125b' target='_blank'><img src='declaracioneside.com/linkedin.png' alt='Siguenos en linkedin' Height='30px' Width='30px' BorderWidth ='0px'></a>&nbsp;<a href='http://plus.google.com/107594546767340388428?prsrc=3'><img src='http://ssl.gstatic.com/images/icons/gplus-32.png' alt='Google+' Height='30px' Width='30px' BorderWidth ='0px'></a><br /> </body></html>"
        elcorreo.IsBodyHtml = True
        elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        Dim smpt As New System.Net.Mail.SmtpClient
        smpt.Host = "smtp.gmail.com"
        smpt.Port = "587"
        smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a.")
        smpt.EnableSsl = True 'req p server gmail
        Try
            smpt.Send(elcorreo)
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('Error notificando registro: " & ex.Message + ", intente mas tarde');</script>")
            Exit Sub
        End Try

        Response.Write("<script language='javascript'>alert('Registro exitoso');</script>")
        If Session("runAsAdmin") = "1" Then
            Response.Write("<script>location.href = 'Login.aspx?lan=1';</script>")
        Else
            Response.Write("<script>location.href = 'Login.aspx';</script>")
        End If
        'Response.Redirect("Login.aspx")

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
        If Trim(rfcDeclarante.Text).Length < 9 Or Trim(rfcDeclarante.Text).Length > 12 Then
            Response.Write("<script language='javascript'>alert('Longitud de rfc Declarante entre 9-12 caracteres');</script>")
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
        ElseIf len(tel.Text) < 10 Then
            Response.Write("<script language='javascript'>alert('Longitud minima de 10 digitos para el teléfono, incluya lada');</script>")
            tel.Focus()
            Return 0
        End If
        If Trim(cel.Text) <> "" Then
            If Len(cel.Text) < 10 Then
                Response.Write("<script language='javascript'>alert('Longitud minima de 10 digitos para el celular, incluya lada');</script>")
                cel.Focus()
                Return 0
            End If
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
        ElseIf len(casfim.Text) > 6 Then
            Response.Write("<script language='javascript'>alert('Longitud maxima de 6 digitos para CASFIM');</script>")
            casfim.Focus()
            Return 0
        End If
        If Trim(fuente.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique como se entero de nosotros');</script>")
            fuente.Focus()
            Return 0
        End If

        Return 1
    End Function


    Private Function validaDupl() As Integer
        Dim q
        q = "SELECT correo, razonSoc, rfcDeclarante FROM clientes WHERE correo='" + Trim(correo.Text.ToUpper) + "' OR razonSoc='" + Trim(razonSoc.Text.ToUpper) + "' OR rfcDeclarante='" + Trim(rfcDeclarante.Text.ToUpper) + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            'dr(0).ToString()
            'grid1.datasource=dr
            'grid1.DataBind()
            Response.Write("<script language='javascript'>alert('Ya existe un usuario registrado con ese correo, nombre de institucion, o rfc ');</script>")
            Return 0
        End If
        dr.Close()

        q = "SELECT casfim FROM clientes WHERE casfim='" + Trim(casfim.Text.ToUpper) + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            'dr(0).ToString()
            'grid1.datasource=dr
            'grid1.DataBind()
            Response.Write("<script language='javascript'>alert('Favor de probar con otra clave casfim');</script>")
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

    'Protected Sub otraImagen_Click(sender As Object, e As EventArgs) Handles otraImagen.Click
    '    cargaCaptcha()
    'End Sub

    Protected Sub idDistribuidor_TextChanged(sender As Object, e As EventArgs) Handles idDistribuidor.TextChanged

    End Sub
End Class