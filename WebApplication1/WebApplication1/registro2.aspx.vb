Imports System.Data
Imports System.Data.SqlClient

Public Class WebForm2
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim dr As SqlDataReader

    Dim dataSet As DataSet
    Dim tb As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        myConnection = New SqlConnection("server=" + nombreServidor + ";database=ide;User ID=usuario;Password=USUARIO;MultipleActiveResultSets=True")
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
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click

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

        'el pass se inserta vacio pero se actualiza en seguida al encriptar
        q = "INSERT INTO clientes(correo, razonSoc, contacto, puesto, tel, cel, paginaWeb, rfcDeclarante, domFiscal, numSucursales, numSociosClientes, casfim, esInstitCredito, fechaRegistro, solSocketEstatus, directorioServidor,idDistribuidor,inscripcionPagada,casfimProvisional,fuente) VALUES('" + Trim(correo.Text.ToUpper) + "','" + Trim(razonSoc.Text.ToUpper) + "','" + Trim(contacto.Text.ToUpper) + "','" + Trim(puesto.Text.ToUpper) + "','" + Trim(tel.Text.ToUpper) + "','" + Trim(cel.Text) + "','" + Trim(paginaWeb.Text.ToUpper) + "','" + Trim(rfcDeclarante.Text.ToUpper) + "','" + Trim(domFiscal.Text.ToUpper) + "','" + Trim(Replace(numSucursales.Text, ",", "")) + "','" + Trim(Replace(numSociosClientes.Text, ",", "")) + "','" + Trim(casfim.Text.ToUpper) + "'," + esInstitCreditoVal + ",'" + Format(Now(), "yyyy-MM-dd") + "','VACIA','" + Trim(casfim.Text.ToUpper) + "'," + idDistribuidor.Text.Trim + ",0," + casfimProvisionalVal + ",'" + fuente.Text.Trim + "')"
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
        elcorreo.Subject = "Registro en DeclaracionesIde.com"
        elcorreo.Body = "<html><body>Hola " + razonSoc.Text.Trim.ToUpper + "<br><br>Bienvenido,<br><br> Su registro se procesó exitosamente en nuestro servidor, ahora puede acceder su cuenta desde el menú 'Mi cuenta' o bien en 'Iniciar sesión' de nuestra página<br>Los datos de su cuenta son: Correo = " + correo.Text.Trim.ToUpper + ", Contraseña = " + passWeb.Text.Trim + " <br><br>Una vez haya iniciado sesión, estando dentro del submenú Cuenta, desplacece a la sección 'Representante Legal' e introduzca los datos del representante legal actual de su institución y pulse en '+ Agregar', si en algun momento cambian de representante legal, agreguelo en mi cuenta y definalo como el actual; desplacece a la parte inferior hasta la sección autorización de socket, haga clic en 'Ver formato' para descargar el formato que requiere adecuar, firmar y enviarnos por el sistema para tramitarle y configurarle con el SAT su canal/matriz de conexión segura para transmisión de datos , luego clic en 'Seleccionar archivo o Examinar' para seleccionar un archivo escaneado en PDF con los datos rellenos en base al formato del paso anterior, luego clic en 'Subir solicitud' para enviarnolo por sistema, si este paso fue exitoso haga clic en 'Mostrar' para ver el archivo que recién subió.<br><br> Si recién acaba de tramitar su clave CASFIM envienos adjunta en un correo la pantalla donde le asignan dicha clave, que se vea la clave y el nombre o RFC de su empresa<br><br> Habiendo completado estos pasos nosotros validemos su solicitud/formato de autorización, le notificaremos para que proceda a realizar y pagar los contratos que desee desde el submenú 'Mis contratos', una vez que tengamos su carta de autorización y su clave de IDE o de institucion financiera nosotros realizamos ante el SAT la gestión y configuración de su socket, lo cual se lleva aprox. 1-2 semanas<br><br>Una vez esté lista dicha configuración, Ud. será notificado para poder ingresar a su cuenta y así pueda comenzar a enviar sus declaraciones de IDE. Es necesario que instale y descargue el navegador Chrome o Firefox <br><br><br>Atentamente <a href='http://declaracioneside.com'>declaracioneside.com</a><br>Tu solución en declaraciones informativas del impuesto IDE por internet</body></html>"
        elcorreo.IsBodyHtml = True
        elcorreo.Priority = System.Net.Mail.MailPriority.Normal
        Dim smpt As New System.Net.Mail.SmtpClient
        smpt.Host = "smtp.gmail.com"
        'smpt.Port = "465"
        smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2")
        smpt.EnableSsl = True 'req p server gmail
        Try
            smpt.Send(elcorreo)
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('Error notificando registro: " & ex.Message + ", intente mas tarde');</script>")
            Exit Sub
        End Try

        Response.Write("<script language='javascript'>alert('Registro exitoso');</script>")
        Response.Write("<script>location.href = 'Login.aspx';</script>")
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
            Response.Write("<script language='javascript'>alert('Especifique como se entero de nosotros');</script>")
            fuente.Focus()
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
            Response.Write("<script language='javascript'>alert('Ya existe un usuario registrado con ese correo, razon, rfc o casfim');</script>")
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