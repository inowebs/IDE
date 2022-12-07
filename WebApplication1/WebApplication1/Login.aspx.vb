Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Imaging
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Security.AccessControl
Imports System.IO
Imports System.Net
Imports Newtonsoft.Json.Linq

Public Class WebForm24
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim dr As SqlDataReader

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

    Sub cargaCaptcha()
        Dim imgCaptcha As System.Web.UI.WebControls.Image
        imgCaptcha = LoginUser.FindControl("imgCaptcha")

        Dim strPathToImage As String = "captchaimg/captcha.gif" ' Make sure the directory is writable by your web server!
        Dim strText As String = fncDrawCaptcha(Server.MapPath(strPathToImage))
        imgCaptcha.ImageUrl = strPathToImage
        Session.Add("strText", strText)
        imgCaptcha.Width = 200
        imgCaptcha.Height = 100
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString("ReturnUrl"))
        RegisterHyperLink.NavigateUrl = "~/registro.aspx"
        'myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';")
        'myConnection.Open()
        myCommand = New SqlCommand("set dateformat ymd")
        ExecuteNonQueryFunction(myCommand)

        SetFocus(LoginUser.FindControl("UserName"))

        Dim v As TextBox
        Dim txtPass As TextBox
        v = LoginUser.FindControl("UserName")
        txtPass = LoginUser.FindControl("Password")

        If Not IsPostBack Then
            Dim so_language = System.Globalization.CultureInfo.InstalledUICulture.ThreeLetterWindowsLanguageName
            If so_language.StartsWith("ENU") Then
                Session("identidad") = "NETWORK SERVICE"
            Else
                Session("identidad") = "Servicio de red"
            End If
            'cargaCaptcha()

            If Request.QueryString("id") IsNot Nothing Then 'viene de admon            
                Dim q
                myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';MultipleActiveResultSets=True")
                myConnection.Open()
                'Encriptacion
                myCommand = New SqlCommand("OPEN SYMMETRIC KEY SYM_KEY DECRYPTION BY PASSWORD ='##Djjcp##'", myConnection)
                myCommand.ExecuteNonQuery()
                q = "SELECT correo, CAST(DECRYPTBYKEY(passWeb) AS VARCHAR(15)) as passWeb FROM clientes WHERE id='" + Trim(Request.QueryString("id")) + "'"
                myCommand = New SqlCommand(q, myConnection)
                dr = myCommand.ExecuteReader()
                If dr.Read() Then
                    v.Text = dr("correo")
                    txtPass.Text = dr("passWeb")
                    txtPass.Attributes.Add("value", dr("passWeb"))
                End If
                dr.Close()
                myCommand = New SqlCommand("CLOSE SYMMETRIC KEY SYM_KEY", myConnection)
                myCommand.ExecuteNonQuery()

                myConnection.Close()
            Else
                v.Text = "pruebasdeide@gmail.com"
                txtPass.Text = "sistema"
                txtPass.Attributes.Add("value", "sistema")
            End If
        Else
            'If Not String.IsNullOrEmpty(txtPass.Text.Trim()) Then
            '    txtPass.Attributes.Add("value", txtPass.Text)
            'End If
        End If



    End Sub

    Sub OnAuthenticate(ByVal sender As Object, ByVal e As AuthenticateEventArgs)
        'Dim txtCaptcha As TextBox
        'Dim lblMessage As Label
        'Dim imgCaptcha As System.Web.UI.WebControls.Image
        'txtCaptcha = LoginUser.FindControl("txtCaptcha")
        'lblMessage = LoginUser.FindControl("lblMessage")
        'imgCaptcha = LoginUser.FindControl("imgCaptcha")

        'If HttpContext.Current.Request.IsLocal Or Session("runAsAdmin") = "1" Then
        'Else
        '    If Not txtCaptcha.Text = Session.Item("strText") Then
        '        cargaCaptcha()
        '        lblMessage.Text = "No coincidieron los caracteres"
        '        txtCaptcha.Text = ""
        '        LoginUser.FailureText = ""
        '        e.Authenticated = False
        '        Return
        '    End If
        'End If

        'lblMessage.Text = ""

        If Not String.IsNullOrEmpty(Request.QueryString("lan")) Then
            If Request.QueryString("lan") = "1" Then
                Session("runAsAdmin") = "1"
            End If
        End If

        If (HttpContext.Current.Request.IsLocal And String.IsNullOrEmpty(Request.QueryString("user"))) Or Session("runAsAdmin") = "1" Then
        Else
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

        Dim Authenticated As Boolean
        Authenticated = SiteSpecificAuthenticationMethod(LoginUser.UserName.Trim, LoginUser.Password.Trim)
        e.Authenticated = Authenticated
    End Sub

    Function SiteSpecificAuthenticationMethod(ByVal UserName As String, ByVal Password As String) As Boolean
        Dim q

        If InStr(Password.ToUpper, "SELECT") > 0 Or InStr(Password.ToUpper, "INSERT") > 0 Or InStr(Password.ToUpper, "UPDATE") > 0 Or InStr(Password.ToUpper, "DELETE") > 0 Or InStr(Password.ToUpper, "DROP") > 0 Then
            Return False
        End If
        If InStr(UserName.ToUpper, "SELECT") > 0 Or InStr(UserName.ToUpper, "INSERT") > 0 Or InStr(UserName.ToUpper, "UPDATE") > 0 Or InStr(UserName.ToUpper, "DELETE") > 0 Or InStr(UserName.ToUpper, "DROP") > 0 Then
            Return False
        End If

        myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';MultipleActiveResultSets=True")
        myConnection.Open()
        'Encriptacion
        myCommand = New SqlCommand("OPEN SYMMETRIC KEY SYM_KEY DECRYPTION BY PASSWORD ='##Djjcp##'", myConnection)
        myCommand.ExecuteNonQuery()

        If (Request.QueryString("id") IsNot Nothing) Or (HttpContext.Current.Request.IsLocal And String.IsNullOrEmpty(Request.QueryString("user"))) Or Session("runAsAdmin") = "1" Then 'viene de admon            
            q = "SELECT id FROM clientes WHERE correo=@corr" 'parametrizando para evitar inyeccion sql
        Else
            q = "Select id FROM clientes WHERE correo=@corr And CAST(DECRYPTBYKEY(passWeb) As VARCHAR(15))=@pass" 'parametrizando para evitar inyeccion sql
        End If
        myCommand = New SqlCommand(q, myConnection)
        myCommand.Parameters.AddWithValue("@corr", Trim(UserName.ToUpper.Trim))
        If Request.QueryString("id") Is Nothing Then
            myCommand.Parameters.AddWithValue("@pass", Trim(Password.Trim))
        End If
        dr = myCommand.ExecuteReader()
        If dr.HasRows Then
            myCommand = New SqlCommand("CLOSE SYMMETRIC KEY SYM_KEY", myConnection)
            myCommand.ExecuteNonQuery()

            myConnection.Close()
            Return True
        Else
            myCommand = New SqlCommand("CLOSE SYMMETRIC KEY SYM_KEY", myConnection)
            myCommand.ExecuteNonQuery()

            myConnection.Close()
            LoginUser.FailureText = "Correo o contraseña incorrecta ¿los olvidó?"
            Return False
        End If
    End Function

    Protected Sub OnLoggedIn(ByVal sender As Object, ByVal e As EventArgs)
        'ya validado en el autenticate, if todo ok sigue aqui:
        Session("curCorreo") = LoginUser.UserName.ToUpper.Trim
        LoginUser.ForeColor = Drawing.Color.White

        Dim q
        q = "SELECT id,razonSoc FROM clientes WHERE correo='" + Session("curCorreo") + "'"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            If dr.Read() Then
                Session("GidCliente") = dr("id").ToString()
                Session("Gcliente") = dr("razonSoc").ToString()
            End If
        End Using

        'If myConnection.State <> ConnectionState.Closed Then
        '    myConnection.Close()
        'End If

        Response.Redirect("~/cliente.aspx")
    End Sub

    Sub OnLoginError(ByVal sender As Object, ByVal e As EventArgs)
        LoginUser.PasswordRecoveryText = "¿Contraseña incorrecta, la olvidó?"
        LoginUser.FailureText = "¿Contraseña incorrecta, la olvidó?"
    End Sub


    Protected Sub LoginButton_Click(sender As Object, e As EventArgs)

    End Sub



    Protected Sub otraImagen_Click1(sender As Object, e As EventArgs)
        cargaCaptcha()
    End Sub
End Class