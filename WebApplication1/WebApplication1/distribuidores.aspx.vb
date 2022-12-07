Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Win32
Imports System.Net.Mail
Imports System.Web.Mail
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel
Imports System.IO
Imports System.IO.Directory
Imports System
Imports System.Security.AccessControl
Imports System.Security

Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Runtime.InteropServices
Imports System.Security.Principal


Public Class WebForm18
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim dr As SqlDataReader

    Dim dataSet As DataSet
    Dim tb As System.Data.DataTable
    Dim pkCliente
    Dim pkCandNombre

    Private Sub cuentaRegistros()
        Dim q
        q = "SELECT COUNT(*) as cuenta FROM prospeccion"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        prospeccionNregs.Text = FormatNumber(dr("cuenta").ToString, 0) + " Registros"
        dr.Close()
        GridView1.SelectedIndex = -1
    End Sub

    Private Sub cuentaRegistrosCand()
        Dim q
        q = "SELECT COUNT(*) as cuenta FROM candidatoDistrib"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        candidatosNregs.Text = FormatNumber(dr("cuenta").ToString, 0) + " Registros"
        dr.Close()
        GridView4.SelectedIndex = -1
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If HttpContext.Current.Request.IsLocal Or Session("runAsAdmin") = "1" Then
            listado.Visible = True
            chkDistr.Enabled = True
            exportarBD.Visible = True
            getPros.Visible = True
        Else
            listado.Visible = False
            chkDistr.Enabled = False
            exportarBD.Visible = False
            getPros.Visible = False
        End If


        If Not myConnection Is Nothing Then
            myConnection.Close()
        End If
        myConnection = New SqlConnection("Persist Security Info=True;Data Source=.;User ID=usuario;Password='SmN+v-XzFy2N;91E170o'; Initial Catalog=ide;Integrated Security=True;MultipleActiveResultSets=True;")
        myConnection.Open()
        myCommand = New SqlCommand("set dateformat ymd", myConnection)
        myCommand.ExecuteNonQuery()


        Page.ClientScript.RegisterStartupScript(GetType(Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + divScroll.ClientID + "');", True)
        nRegs.Text = FormatNumber(GridView3.Rows.Count.ToString, 0) + " Registros"
        'MultiView1.ActiveViewIndex = Int32.Parse(0)
        Page.ClientScript.RegisterStartupScript(GetType(Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + divScroll3.ClientID + "','scrollPos3');", True)

        Page.ClientScript.RegisterStartupScript(GetType(Page), System.DateTime.Now.Ticks.ToString(), "scrollTo('" + divScroll4.ClientID + "','scrollPos4');", True)

        GridView4.SelectedIndex = -1
        candidatosNregs.Text = FormatNumber(GridView4.Rows.Count.ToString, 0) + " Registros"
        
    End Sub


    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        MultiView1.ActiveViewIndex = Int32.Parse(1)
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
        MultiView1.ActiveViewIndex = Int32.Parse(2)
        Panel1.Visible = False
    End Sub

    Protected Sub listado_Click(sender As Object, e As EventArgs) Handles listado.Click        
        MultiView1.ActiveViewIndex = Int32.Parse(3)
        GridView3.DataBind()
        GridView3.SelectedIndex = -1

        GridView4.DataBind()
        GridView4.SelectedIndex = -1
    End Sub

    Protected Sub registrarme_Click(sender As Object, e As EventArgs) Handles registrarme.Click

        If acepto.Checked = False Then
            acepto.Focus()
            Response.Write("<script language='javascript'>alert('Si no acepta el contrato para distribuidores, no puede registrarse como distribuidor');</script>")
            Exit Sub
        End If


        If validaVacios() < 1 Then
            Exit Sub
        End If

        If validaDupl() < 1 Then
            Exit Sub
        End If

        If FileUpload1.HasFile Then
            Dim fileSize As Integer = FileUpload1.PostedFile.ContentLength
            Dim fileName As String = Server.HtmlEncode(FileUpload1.FileName)
            Dim extension As String = System.IO.Path.GetExtension(fileName)
            If (fileSize < 2000000) Then '2,000kb: 4 archivos jpg
                If (extension <> ".zip" And extension <> ".ZIP") Then
                    Response.Write("<script language='javascript'>alert('El archivo debe ser de tipo .zip o .ZIP');</script>")
                    Exit Sub
                End If
            Else
                Response.Write("<script language='javascript'>alert('El tamaño del archivo debe ser máximo 2000Kb (2 Mb)');</script>")
                Exit Sub
            End If
        End If

        Dim clisForzososVal
        If clisForzosos.SelectedIndex = 0 Then 'clis forzosos p comis recurrentes
            clisForzososVal = "1"
        Else
            clisForzososVal = "0"
        End If
        Dim facturarAdistribVal
        If facturarAdistrib.Checked = False Then
            facturarAdistribVal = "0"
        Else
            facturarAdistribVal = "1"
        End If

        Dim q = "INSERT INTO distribuidores(nombreFiscal,banco,clabe,ciudadYestado,tel,correo,pass,clisForzosos,facturarAdistrib,datosFacturacion,numCuenta) VALUES('" + nombreFiscal.Text.Trim.ToUpper + "','" + banco.Text.Trim.ToUpper + "','" + clabe.Text.Trim + "','" + ciudadYestado.Text.Trim.ToUpper + "','" + tel.Text.Trim.ToUpper + "','" + correo.Text.Trim.ToUpper + "','" + pass.Text.Trim + "'," + clisForzososVal + "," + facturarAdistribVal + ",'" + datosFacturacion.Text.Trim.ToUpper + "','" + numCuenta.Text.Trim + "')"
        myCommand = New SqlCommand(q, myConnection)
        myCommand.ExecuteNonQuery()
        If clisForzosos.SelectedIndex = 1 Then 'no son forzosos
            myCommand = New SqlCommand("UPDATE distribuidores SET comisCaduca=1,comisMesesCaducidad=12,comisPorcen=10 WHERE correo='" + correo.Text.Trim.ToUpper + "'", myConnection)
            myCommand.ExecuteNonQuery()
        Else
            myCommand = New SqlCommand("UPDATE distribuidores SET comisCaduca=0,comisPorcen=15 WHERE correo='" + correo.Text.Trim.ToUpper + "'", myConnection)
            myCommand.ExecuteNonQuery()
        End If

        q = "SELECT id FROM distribuidores WHERE correo='" + Trim(correo.Text.ToUpper) + "' AND clabe='" + Trim(clabe.Text) + "' AND numCuenta='" + Trim(numCuenta.Text) + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()

        Dim elcorreo2 As New System.Net.Mail.MailMessage
        Using elcorreo2
            elcorreo2.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
            elcorreo2.To.Add(correo.Text.Trim)
            elcorreo2.Bcc.Add("declaracioneside@gmail.com")
            elcorreo2.Subject = "Gracias por registrase como Distribuidor provisional # " + dr("id").ToString
            elcorreo2.Body = "<html><body>Tan pronto nos envíe y validemos la documentación de Usted, será notificado de tal autorización, mientras tanto puede orientar a sus clientes para que se Registren como tales en la página principal en el enlace Registrarse de la parte superior derecha, donde introducirán el # de distribuidor de Usted, una vez completado ese registro el cliente podrá iniciar sesión desde la página principal para acceder a Mi cuenta, y desde ahi el cliente nos envia su carta de autorizacion para que le tramitemos un socket de conexion ante el SAT, que al recibirlo le es notificado para que pueda realizar los contratos y declaraciones que desee. Ingresa a <a href='www.declaracioneside.com/distribuidores.aspx'>www.declaracioneside.com/distribuidores.aspx</a> y dirigete a la sección 'Iniciar sesión como distribuidor' introduce tus datos, ahi encontraras los formatos de declaraciones y el listado de tus instituciones. Es importante que revise todo el sitio, en especial el contrato para distribuidores, los términos del servicio para clientes en el pie de página, planes y precios, los recursos para distribuidores, la sección de preguntas frecuentes para que sepan la información suficiente para responder cualquier pregunta de los prospectos<br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
            elcorreo2.IsBodyHtml = True
            elcorreo2.Priority = System.Net.Mail.MailPriority.Normal
            Dim smpt As New System.Net.Mail.SmtpClient
            smpt.Host = "smtp.gmail.com"
            smpt.Port = "587"
            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
            smpt.EnableSsl = True 'req p server gmail
            Try
                smpt.Send(elcorreo2)
                elcorreo2.Dispose()
            Catch ex As Exception
                Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
                Exit Sub
            Finally
            End Try
        End Using


        If FileUpload1.HasFile Then
            Dim savePath = "C:\SAT\docDistr" + dr("id").ToString + ".zip"
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

            'AddFileSecurity(savePath, Session("identidad"), FileSystemRights.ReadData, AccessControlType.Allow)

            'enviarme doc 
            Dim elcorreo As New System.Net.Mail.MailMessage
            Using elcorreo
                elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                elcorreo.To.Add("declaracioneside@gmail.com")
                elcorreo.Subject = "Documentación del distribuidor # " + dr("id").ToString
                elcorreo.Body = "<html><body>Validar contenido, guardar en carpeta Docs Distribuidores del correo p no saturar al server, si es válida autorizarlo y si no enviarle correo a " + correo.Text.Trim + "<br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
                elcorreo.IsBodyHtml = True
                elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                elcorreo.Attachments.Add(New Attachment(savePath))
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
            File.Delete(savePath)
        Else
            'enviarme doc 
            Dim elcorreo As New System.Net.Mail.MailMessage
            Using elcorreo
                elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                elcorreo.To.Add("declaracioneside@gmail.com")
                elcorreo.To.Add(correo.Text.Trim)
                elcorreo.Subject = "Distribuidor # " + dr("id").ToString + " pendiente de documentación"
                elcorreo.Body = "<html><body>Le recordamos que es necesario nos envíe su documentación de distribuidor a la brevedad posible, para poder autorizarlo y de esa forma pueda cobrar sus comisiones<br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
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
                End Try
            End Using
        End If

        Response.Write("<script language='javascript'>alert('Registro exitoso, su # de Distribuidor provisional (sujeto a autorización lo cual le será notificado) a publicar con sus prospectos de ventas para recibir sus comisiones es " + dr("id").ToString + ". Información importante ha sido enviada a su correo que registró como distribuidor');</script>")
        dr.Close()
        GridView3.DataBind()
        MultiView1.ActiveViewIndex = Int32.Parse(1)

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

        If Trim(nombreFiscal.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique su nombre fiscal');</script>")
            nombreFiscal.Focus()
            Return 0
        End If
        If Trim(banco.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el banco');</script>")
            banco.Focus()
            Return 0
        End If
        If Trim(pass.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el password');</script>")
            pass.Focus()
            Return 0
        End If
        If Trim(pass.Text).Length < 6 Then
            Response.Write("<script language='javascript'>alert('Longitud minima de password de 6 caracteres');</script>")
            pass.Focus()
            Return 0
        End If
        If pass.Text.Trim <> pass2.Text.Trim Then
            Response.Write("<script language='javascript'>alert('El password y su confirmación no coinciden');</script>")
            pass.Focus()
            Return 0
        End If
        If Trim(clabe.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique la clabe interbancaria');</script>")
            clabe.Focus()
            Return 0
        End If
        If Trim(numCuenta.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el número de cuenta');</script>")
            numCuenta.Focus()
            Return 0
        End If
        If Trim(clabe.Text).Length <> 18 Then
            Response.Write("<script language='javascript'>alert('Longitud requerida de clabe de 18 caracteres');</script>")
            clabe.Focus()
            Return 0
        End If

        If Trim(ciudadYestado.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique su ciudad y estado');</script>")
            ciudadYestado.Focus()
            Return 0
        End If
        If Trim(tel.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el teléfono');</script>")
            tel.Focus()
            Return 0
        End If
        If facturarAdistrib.Checked = True Then
            If datosFacturacion.Text.Trim = "" Then
                Response.Write("<script language='javascript'>alert('No ha especificado datos de facturación');</script>")
                datosFacturacion.Focus()
                Return 0
            End If
        End If

        Return 1
    End Function


    Private Function validaDupl() As Integer
        Dim q
        q = "SELECT correo, clabe, numCuenta FROM distribuidores WHERE correo='" + Trim(correo.Text.ToUpper) + "' OR clabe='" + Trim(clabe.Text) + "' OR nombreFiscal='" + Trim(nombreFiscal.Text) + "' OR numCuenta='" + Trim(numCuenta.Text) + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            Response.Write("<script language='javascript'>alert('Ya existe un distribuidor registrado con esas llaves');</script>")
            Return 0
        End If
        dr.Close()

        Return 1
    End Function


    Private Sub WebForm18_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        myConnection.Close()
    End Sub

    Protected Sub identificarme_Click(ByVal sender As Object, ByVal e As EventArgs) Handles identificarme.Click
        If correo1.Text.Trim = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el correo');</script>")
            correo1.Focus()
            Exit Sub
        End If
        If HttpContext.Current.Request.IsLocal Or Session("runAsAdmin") = "1" Then
        Else
            If pass5.Text.Trim = "" Then
                Response.Write("<script language='javascript'>alert('Especifique la contraseña');</script>")
                pass5.Focus()
                Exit Sub
            End If
        End If

        Dim q

        If HttpContext.Current.Request.IsLocal Or Session("runAsAdmin") = "1" Then
            q = "SELECT * FROM distribuidores WHERE correo='" + Trim(correo1.Text.ToUpper) + "'"
        Else
            q = "SELECT * FROM distribuidores WHERE correo='" + Trim(correo1.Text.ToUpper) + "' AND pass='" + Trim(pass5.Text) + "'"
        End If
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If Not dr.Read() Then
            Response.Write("<script language='javascript'>alert('Distribuidor no registrado o contraseña incorrecta');</script>")
            Panel1.Visible = False
        Else
            Panel1.Visible = True
            id.Text = dr("id").ToString
            idDistribuidor.Text = id.Text
            iddistribuidorLogged.Text = id.Text
            nombreFiscal1.Text = dr("nombreFiscal")
            banco1.Text = dr("banco")
            clabe1.Text = dr("clabe")
            numCuenta1.Text = dr("numCuenta")
            ciudadYestado1.Text = dr("ciudadYestado")
            tel1.Text = dr("tel")
            correo2.Text = dr("correo")
            pass6.Attributes.Add("value", dr("pass").ToString())
            pass7.Attributes.Add("value", dr("pass").ToString())
            If dr("clisForzosos").Equals(True) Then
                clisForzosos1.SelectedIndex = 0
            Else
                clisForzosos1.SelectedIndex = 1
            End If
            If dr("doctos").Equals(True) Then
                doctos.Checked = True
                FileUpload2.Enabled = False
            Else
                doctos.Checked = False
                FileUpload2.Enabled = True
            End If
            If dr("facturarAdistrib").Equals(False) Then
                facturarAdistrib0.Checked = False
            Else
                facturarAdistrib0.Checked = True
                datosFacturacion0.Text = dr("datosFacturacion")
            End If

            If dr("esEmpleado").Equals(True) Then
                'HyperLink13.Visible = True
                'HyperLink9.Visible = True
                linkProspeccion.Visible = True
                HyperLink15.Visible = True
                'HyperLink10.Visible = True
            Else
                'HyperLink13.Visible = False
                'HyperLink9.Visible = False
                linkProspeccion.Visible = False
                HyperLink15.Visible = False
                'HyperLink10.Visible = False
            End If
            'If Session("runAsAdmin") = "1" Then
            '    transferir.Visible = True
            '    transferido.Visible = True
            'Else
            '    transferir.Visible = False
            '    transferido.Visible = False
            'End If
        End If
        dr.Close()

    End Sub

    Protected Sub mod_Click(ByVal sender As Object, ByVal e As EventArgs) Handles [mod].Click
        If validaVacios2() < 1 Then
            Exit Sub
        End If

        If validaDupl2() < 1 Then
            Exit Sub
        End If

        If FileUpload2.HasFile Then
            Dim fileSize As Integer = FileUpload2.PostedFile.ContentLength
            Dim fileName As String = Server.HtmlEncode(FileUpload2.FileName)
            Dim extension As String = System.IO.Path.GetExtension(fileName)
            If (fileSize < 2000000) Then '2,000kb: 4 archivos jpg
                If (extension <> ".zip" And extension <> ".ZIP") Then
                    Response.Write("<script language='javascript'>alert('El archivo debe ser de tipo .zip o .ZIP');</script>")
                    Exit Sub
                End If
            Else
                Response.Write("<script language='javascript'>alert('El tamaño del archivo debe ser máximo 2000Kb (2 Mb)');</script>")
                Exit Sub
            End If
        End If

        If FileUpload2.HasFile Then
            Dim savePath = "C:\SAT\docDistr" + id.Text + ".zip"
            FileUpload2.SaveAs(savePath)
            'AddFileSecurity(savePath, Session("identidad"), FileSystemRights.ReadData, AccessControlType.Allow)

            'enviarme doc 
            Dim elcorreo As New System.Net.Mail.MailMessage
            Using elcorreo
                elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                elcorreo.To.Add("declaracioneside@gmail.com")
                elcorreo.Subject = "Documentación del distribuidor # " + id.Text
                elcorreo.Body = "<html><body>Validar contenido, guardar en pc distinta del server en carpetas p no saturar al server, si es válida autorizarlo y si no enviarle correo a " + correo2.Text.Trim + "<br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
                elcorreo.IsBodyHtml = True
                elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                elcorreo.Attachments.Add(New Attachment(savePath))
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
            File.Delete(savePath)
        End If

        Dim facturarAdistribVal
        If facturarAdistrib0.Checked = False Then
            facturarAdistribVal = "0"
        Else
            facturarAdistribVal = "1"
        End If

        Dim clisForzososVal, q
        If clisForzosos1.SelectedIndex = 0 Then 'clis forzosos p comis recurrentes
            clisForzososVal = "1"
            q = "UPDATE distribuidores SET nombreFiscal='" + nombreFiscal1.Text.Trim.ToUpper + "',banco='" + banco1.Text.Trim.ToUpper + "',clabe='" + clabe1.Text.Trim.ToUpper + "',ciudadYestado='" + ciudadYestado1.Text.Trim.ToUpper + "',tel='" + tel1.Text.Trim.ToUpper + "',correo='" + correo2.Text.Trim.ToUpper + "',pass='" + pass6.Text.Trim + "',clisForzosos=" + clisForzososVal + ",facturarAdistrib=" + facturarAdistribVal + ",datosFacturacion='" + datosFacturacion0.Text.Trim.ToUpper + "',numCuenta='" + numCuenta1.Text.Trim.ToUpper + "',comisCaduca=0,comisPorcen=15  WHERE id=" + id.Text
        Else
            clisForzososVal = "0"
            q = "UPDATE distribuidores SET nombreFiscal='" + nombreFiscal1.Text.Trim.ToUpper + "',banco='" + banco1.Text.Trim.ToUpper + "',clabe='" + clabe1.Text.Trim.ToUpper + "',ciudadYestado='" + ciudadYestado1.Text.Trim.ToUpper + "',tel='" + tel1.Text.Trim.ToUpper + "',correo='" + correo2.Text.Trim.ToUpper + "',pass='" + pass6.Text.Trim + "',clisForzosos=" + clisForzososVal + ",facturarAdistrib=" + facturarAdistribVal + ",datosFacturacion='" + datosFacturacion0.Text.Trim.ToUpper + "',numCuenta='" + numCuenta1.Text.Trim.ToUpper + "',comisCaduca=1,comisMesesCaducidad=12,comisPorcen=10 WHERE id=" + id.Text
        End If
        myCommand = New SqlCommand(q, myConnection)
        myCommand.ExecuteNonQuery()
        Response.Write("<script language='javascript'>alert('Datos actualizados');</script>")
        MultiView1.ActiveViewIndex = Int32.Parse(-1)
        GridView3.DataBind()
        'Response.Redirect("~/Default.aspx")

    End Sub

    Private Function validaVacios2() As Integer
        If Trim(correo2.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el correo');</script>")
            correo2.Focus()
            Return 0
        End If

        If IsValidEmail(Trim(correo2.Text)) = False Then
            Response.Write("<script language='javascript'>alert('Formato de correo incorrecto');</script>")
            correo2.Focus()
            Return 0
        End If

        If Trim(nombreFiscal1.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique su nombre fiscal');</script>")
            nombreFiscal1.Focus()
            Return 0
        End If
        If Trim(banco1.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el banco');</script>")
            banco1.Focus()
            Return 0
        End If
        If Trim(pass6.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el password');</script>")
            pass6.Focus()
            Return 0
        End If
        If Trim(pass6.Text).Length < 6 Then
            Response.Write("<script language='javascript'>alert('Longitud minima de password de 6 caracteres');</script>")
            pass6.Focus()
            Return 0
        End If
        If pass6.Text.Trim <> pass7.Text.Trim Then
            Response.Write("<script language='javascript'>alert('El password y su confirmación no coinciden');</script>")
            pass6.Focus()
            Return 0
        End If
        If Trim(clabe1.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique la clabe interbancaria');</script>")
            clabe1.Focus()
            Return 0
        End If
        If Trim(clabe1.Text).Length <> 18 Then
            Response.Write("<script language='javascript'>alert('Longitud requerida de clabe de 18 caracteres');</script>")
            clabe1.Focus()
            Return 0
        End If
        If Trim(numCuenta1.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el número de cuenta');</script>")
            numCuenta1.Focus()
            Return 0
        End If
        If Trim(ciudadYestado1.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique su ciudad y estado');</script>")
            ciudadYestado1.Focus()
            Return 0
        End If
        If Trim(tel1.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el teléfono');</script>")
            tel1.Focus()
            Return 0
        End If
        If facturarAdistrib0.Checked = True Then
            If datosFacturacion0.Text.Trim = "" Then
                Response.Write("<script language='javascript'>alert('No ha especificado datos de facturación');</script>")
                datosFacturacion0.Focus()
                Return 0
            End If
        End If


        Return 1
    End Function


    Private Function validaDupl2() As Integer
        Dim q
        q = "SELECT correo, clabe, numCuenta FROM distribuidores WHERE id<>" + id.Text + " and (correo='" + Trim(correo2.Text.ToUpper) + "' OR clabe='" + Trim(clabe1.Text) + "' OR nombreFiscal='" + Trim(nombreFiscal1.Text) + "' OR numCuenta='" + Trim(numCuenta1.Text) + "')"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            Response.Write("<script language='javascript'>alert('Ya existe otro distribuidor registrado con esas llaves');</script>")
            Return 0
        End If
        dr.Close()

        Return 1
    End Function

    Sub AddFileSecurity(ByVal fileName As String, ByVal account As String, _
            ByVal rights As FileSystemRights, ByVal controlType As AccessControlType)

        Dim fSecurity As FileSecurity = File.GetAccessControl(fileName)
        Dim accessRule As FileSystemAccessRule = _
        New FileSystemAccessRule(account, rights, controlType)
        fSecurity.AddAccessRule(accessRule)
        File.SetAccessControl(fileName, fSecurity)

    End Sub

    Protected Sub autorizar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles autorizar.Click
        If GridView3.SelectedRow.Equals(False) Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If

        Dim row As GridViewRow = GridView3.SelectedRow

        If row.Cells(8).Text = "True" Then
            Response.Write("<script language='javascript'>alert('Ya estaba autorizado');</script>")
            Exit Sub
        End If

        Dim q = "UPDATE distribuidores SET doctos=1 WHERE id=" + row.Cells(1).Text
        myCommand = New SqlCommand(q, myConnection)
        myCommand.ExecuteNonQuery()

        Dim elcorreo As New System.Net.Mail.MailMessage
        Using elcorreo
            elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
            elcorreo.To.Add(row.Cells(7).Text)
            elcorreo.Subject = "Bienvenido " + row.Cells(2).Text + ", Distribuidor de declaracioneside.com"
            elcorreo.Body = "<html><body>Ahora formas oficialmente parte del equipo de distribuidores de <a href='declaracioneside.com'>declaracioneside.com</a> 'Tu solución en declaraciones de depósitos en efectivo por internet' y puedes comenzar a ganar dinero inmediatamente a través de los contratos que consigas con tus prospectos, ingresa a <a href='www.declaracioneside.com/distribuidores.aspx'>www.declaracioneside.com/distribuidores.aspx</a> y dirigete a la sección 'Iniciar sesión como distribuidor' introduce tus datos, y en la parte inferior encontrarás la guía para distribuidores, formatos de declaraciones y 'Prospeccion' donde podrás llevar el seguimiento de prospección en línea de tus clientes y consultar previamente si el cliente que intentas contactar ya está tomado por otro distribuidor<br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
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
            End Try
        End Using
        GridView3.DataBind()
        Response.Write("<script language='javascript'>alert('Autorizado y notificado');</script>")
        GridView3.SelectedIndex = -1
    End Sub


    Protected Sub desAutorizar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles desAutorizar.Click
        If GridView3.SelectedRow.Equals(False) Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If

        Dim row As GridViewRow = GridView3.SelectedRow

        If row.Cells(8).Text = "False" Then
            Response.Write("<script language='javascript'>alert('Estaba sin autorizar');</script>")
            Exit Sub
        End If

        Dim q = "UPDATE distribuidores SET doctos=0 WHERE id=" + row.Cells(1).Text
        myCommand = New SqlCommand(q, myConnection)
        myCommand.ExecuteNonQuery()

        GridView3.DataBind()
        Response.Write("<script language='javascript'>alert('desAutorizado');</script>")
        GridView3.SelectedIndex = -1

    End Sub

    Protected Sub addPros_Click(ByVal sender As Object, ByVal e As EventArgs) Handles addPros.Click
        If validaVaciosPros() < 1 Then
            Exit Sub
        End If

        If validaDuplPros() < 1 Then
            Exit Sub
        End If
        Dim q As String
        Dim correoProspeccionVal As String
        If correoProspeccion.Text.Trim <> "" Then
            correoProspeccionVal = correoProspeccion.Text.Trim.ToUpper
        Else
            correoProspeccionVal = " "
        End If

        Dim notasval
        If notas.Text.Trim <> "" Then
            If Len(notas.Text) > 500 Then
                notas.Focus()
                Response.Write("<script language='javascript'>alert('Depure sus notas, la capacidad es de 500 caracteres');</script>")
                Exit Sub
            End If
            notasval = notas.Text.Trim
        Else
            notasval = " "
        End If
        If Not fechaProgramada.SelectedDate.Date = DateTime.MinValue.Date Then 'se selecciono una fecha
            'Dim tomorrow As Date = Date.Today.AddDays(1)
            'fechaProgramada.TodaysDate = tomorrow
            'fechaProgramada.SelectedDate = fechaProgramada.TodaysDate
            q = "INSERT INTO prospeccion(cliente,estatusActual,fecha,idDistribuidor,notas,correo,fechaProgramada,telsInvalidos,tipo) VALUES('" + Trim(cliente.Text.ToUpper) + "','" + Trim(estatusActual.Text.ToUpper) + "','" + Format(Now(), "yyyy-MM-dd") + "'," + iddistribuidorLogged.Text.Trim + ",'" + notasval + "','" + correoProspeccionVal + "','" + Format(fechaProgramada.SelectedDate, "yyyy-MM-dd") + "','" + telsInvalidos.Text.ToUpper + "','" + tipo.SelectedValue + "')"
        Else
            q = "INSERT INTO prospeccion(cliente,estatusActual,fecha,idDistribuidor,notas,correo,telsInvalidos,tipo) VALUES('" + Trim(cliente.Text.ToUpper) + "','" + Trim(estatusActual.Text.ToUpper) + "','" + Format(Now(), "yyyy-MM-dd") + "'," + iddistribuidorLogged.Text.Trim + ",'" + notasval + "','" + correoProspeccionVal + "','" + telsInvalidos.Text.ToUpper + "','" + tipo.SelectedValue + "')"
        End If

        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Close()
        'refrescar grid
        'SqlDataSource1.SelectCommand = "SELECT id,cliente,estatusActual,fecha,idDistribuidor,notas,correo,fechaProgramada,telsInvalidos,tipo FROM [prospeccion] WHERE idDistribuidor=" + id.Text + " and estatusActual<>'CC' ORDER BY cliente"
        GridView1.DataBind()
        cuentaRegistros()
        idProspeccion.Text = "ID"
        cliente.Text = ""
        estatusActual.Text = "VA"
        fecha.Text = ""
        notas.Text = ""
        correoProspeccion.Text = ""
        GridView1.SelectedIndex = -1

        fechaProgramada.SelectedDates.Clear()
        telsInvalidos.Text = ""
        tipo.SelectedIndex = 0 '1er item
        Response.Write("<script language='javascript'>alert('Registro exitoso');</script>")

    End Sub


    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView1.SelectedIndexChanged
        Dim row As GridViewRow = GridView1.SelectedRow
        idProspeccion.Text = row.Cells(1).Text
        cliente.Text = Server.HtmlDecode(row.Cells(2).Text)
        estatusActual.Text = row.Cells(3).Text
        fecha.Text = Left(row.Cells(4).Text, 10)
        idDistribuidor.Text = row.Cells(5).Text
        notas.Text = Server.HtmlDecode(row.Cells(6).Text)
        correoProspeccion.Text = Server.HtmlDecode(row.Cells(7).Text)
        If row.Cells(8).Text = "" Or row.Cells(8).Text = " " Or row.Cells(8).Text = "&nbsp;" Then
            fechaProgramada.SelectedDates.Clear()
        Else
            fechaProgramada.SelectedDate = Left(row.Cells(8).Text, 10)
            fechaProgramada.VisibleDate = fechaProgramada.SelectedDate
        End If
        If row.Cells(9).Text = "" Or row.Cells(9).Text = " " Or row.Cells(9).Text = "&nbsp;" Then
            telsInvalidos.Text = ""
        Else
            telsInvalidos.Text = row.Cells(9).Text
        End If

        If row.Cells(10).Text = "1" Then
            tipo.Text = "PROSPECTO DIRECTO"
        ElseIf row.Cells(10).Text = "2" Then
            tipo.Text = "INTERMEDIARIO"
        ElseIf row.Cells(10).Text = "3" Then
            tipo.Text = "PUBLICIDAD CORREO"
        End If

        'SqlDataSource1.SelectCommand = "SELECT id,cliente,estatusActual,fecha,idDistribuidor,notas,correo,fechaProgramada, telsInvalidos, tipo FROM [prospeccion] WHERE id=" + idProspeccion.Text
        'GridView1.DataBind()
    End Sub

    Private Function validaVaciosPros() As Integer
        If Trim(cliente.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el cliente');</script>")
            cliente.Focus()
            Return 0
        End If

        Return 1
    End Function

    Private Function validaDuplPros() As Integer
        Dim q
        q = "SELECT id FROM prospeccion WHERE cliente='" + Trim(cliente.Text.ToUpper) + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            Response.Write("<script language='javascript'>alert('Ese cliente ya esta prospectado');</script>")
            Return 0
        End If
        dr.Close()

        Return 1
    End Function

    Private Function validaDuplModPros() As Integer
        Dim q
        q = "SELECT cliente FROM prospeccion WHERE ID='" + Trim(idProspeccion.Text) + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        pkCliente = dr(0).ToString()
        dr.Close()

        q = "SELECT id FROM prospeccion WHERE cliente='" + Trim(cliente.Text.ToUpper) + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() And (pkCliente <> Trim(cliente.Text.ToUpper)) Then   'if dr.Read() and (pkPorcen <> trim(porcen.text) or pk2 <>trim(campo2)) then
            Response.Write("<script language='javascript'>alert('Cliente ya está en uso');</script>")
            dr.Close()
            Return 0
        End If
        dr.Close()

        Return 1
    End Function

    Protected Sub editPros_Click(ByVal sender As Object, ByVal e As EventArgs) Handles editPros.Click
        If idProspeccion.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If
        If validaVaciosPros() < 1 Then
            Exit Sub
        End If

        If validaDuplModPros() < 1 Then
            Exit Sub
        End If

        If Session("runAsAdmin") = "0" Then
            If idDistribuidor.Text <> iddistribuidorLogged.Text Then
                Response.Write("<script language='javascript'>alert('El registro está con otro distribuidor');</script>")
                Exit Sub
            End If
        End If

        Dim q As String
        If notas.Text.Trim = "" Then
            notas.Text = " "
        Else
            If Len(notas.Text) > 500 Then
                notas.Focus()
                Response.Write("<script language='javascript'>alert('Depure sus notas, la capacidad es de 500 caracteres');</script>")
                Exit Sub
            End If
        End If

        Dim correoProspeccionVal As String
        If correoProspeccion.Text.Trim <> "" Then
            correoProspeccionVal = correoProspeccion.Text.Trim.ToUpper
        Else
            correoProspeccionVal = " "
        End If
        If Not fechaProgramada.SelectedDate.Date = DateTime.MinValue.Date Then 'se selecciono una fecha
            'Dim tomorrow As Date = Date.Today.AddDays(1)
            'fechaProgramada.TodaysDate = tomorrow
            'fechaProgramada.SelectedDate = fechaProgramada.TodaysDate
            q = "UPDATE prospeccion SET cliente='" + Trim(cliente.Text.ToUpper) + "',idDistribuidor=" + iddistribuidorLogged.Text + ",estatusActual='" + estatusActual.Text + "',fecha='" + Format(Now(), "yyyy-MM-dd") + "',notas='" + notas.Text.ToUpper.Trim + "',correo='" + correoProspeccionVal + "', fechaProgramada='" + Format(Convert.ToDateTime(fechaProgramada.SelectedDate), "yyyy-MM-dd") + "', telsInvalidos='" + telsInvalidos.Text.ToUpper.Trim + "', tipo='" + tipo.SelectedValue + "' WHERE id=" + idProspeccion.Text
        Else
            q = "UPDATE prospeccion SET cliente='" + Trim(cliente.Text.ToUpper) + "',idDistribuidor=" + iddistribuidorLogged.Text + ",estatusActual='" + estatusActual.Text + "',fecha='" + Format(Now(), "yyyy-MM-dd") + "',notas='" + notas.Text.ToUpper.Trim + "',correo='" + correoProspeccionVal + "', telsInvalidos='" + telsInvalidos.Text.ToUpper.Trim + "', tipo='" + tipo.SelectedValue + "' WHERE id=" + idProspeccion.Text
        End If

        If iddistribuidorLogged.Text = "" Then
            Response.Redirect("www.declaracioneside.com/Default.aspx")
        End If
        'descr.Text = q
        'Exit Sub
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Close()

        pkCliente = Trim(cliente.Text.ToUpper)

        'SqlDataSource1.SelectCommand = "SELECT id,cliente,estatusActual,fecha,idDistribuidor,notas,correo,fechaProgramada,telsInvalidos,tipo FROM [prospeccion] WHERE idDistribuidor=" + id.Text + " and estatusActual<>'CC' ORDER BY cliente"
        SqlDataSource1.SelectCommand = hiddenBus.Value
        GridView1.DataBind()
        'GridView1.UpdateRow(GridView1.SelectedRow.RowIndex, False)
        'GridView1.SelectedIndex = -1

        'refrescar grid
        'idProspeccion.Text = "ID"
        'cliente.Text = ""
        'estatusActual.Text = "VA"
        'fecha.Text = ""
        'notas.Text = ""
        'correoProspeccion.Text = ""
        'fechaProgramada.SelectedDates.Clear()
        'telsInvalidos.Text = ""
        'tipo.SelectedIndex = 0 '1er item
        Response.Write("<script language='javascript'>alert('Cambios guardados');</script>")
    End Sub

    Protected Sub delPros_Click(ByVal sender As Object, ByVal e As EventArgs) Handles delPros.Click
        If idProspeccion.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If

        If Session("runAsAdmin") = "0" Then
            If idDistribuidor.Text <> iddistribuidorLogged.Text Then
                Response.Write("<script language='javascript'>alert('El registro es de otro distribuidor');</script>")
                Exit Sub
            End If
        End If

        'validar si esta siendo usado x FKs

        'del cascadas

        Dim q = "DELETE FROM prospeccion WHERE id=" + Trim(idProspeccion.Text)
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Close()
        idProspeccion.Text = "ID"
        cliente.Text = ""
        estatusActual.Text = "VA"
        fecha.Text = ""
        idDistribuidor.Text = "ID"
        notas.Text = ""
        fechaProgramada.SelectedDates.Clear()
        telsInvalidos.Text = ""
        tipo.SelectedIndex = 0
        cuentaRegistros()
        GridView1.DataBind()
        Response.Write("<script language='javascript'>alert('Se ha eliminado');</script>")

    End Sub

    Protected Sub linkProspeccion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles linkProspeccion.Click
        MultiView1.ActiveViewIndex = Int32.Parse(4)
        'SqlDataSource1.SelectCommand = "SELECT id,cliente,estatusActual,fecha,idDistribuidor,notas,correo,fechaProgramada,telsInvalidos,tipo FROM [prospeccion] WHERE idDistribuidor=" + id.Text + " and estatusActual<>'CC' "
        'hiddenBus.Value = "SELECT id,cliente,estatusActual,fecha,idDistribuidor,notas,correo,fechaProgramada,telsInvalidos,tipo FROM [prospeccion] WHERE idDistribuidor=" + id.Text + " and estatusActual<>'CC' "
        GridView1.DataBind()
        GridView1.SelectedIndex = -1
    End Sub

    Protected Sub LinkButton3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton3.Click
        'Call Page_Load(vbNull, EventArgs.Empty)
        'If cliente.Text.Trim = "" Then
        '    If id.Text <> "" Then
        '        SqlDataSource1.SelectCommand = "SELECT id,cliente,estatusActual,fecha,idDistribuidor,notas FROM [prospeccion] WHERE idDistribuidor=" + id.Text + " and estatusActual<>'CC' ORDER BY cliente"
        '    Else
        '        SqlDataSource1.SelectCommand = "SELECT id,cliente,estatusActual,fecha,idDistribuidor,notas FROM [prospeccion] WHERE estatusActual<>'CC' ORDER BY cliente"
        '    End If
        '    GridView1.DataBind()
        'End If
        'GridView1.SelectedIndex = -1            
        prospeccionNregs.Text = FormatNumber(GridView1.Rows.Count.ToString, 0) + " Registros"
        SqlDataSource1.SelectCommand = "SELECT id,cliente,estatusActual,fecha,idDistribuidor,notas,correo,fechaProgramada,telsInvalidos,tipo FROM [prospeccion] "
        hiddenBus.Value = "SELECT id,cliente,estatusActual,fecha,idDistribuidor,notas,correo,fechaProgramada,telsInvalidos,tipo FROM [prospeccion] "
        GridView1.DataBind()
        GridView1.SelectedIndex = -1
        fechaProgramada.VisibleDate = Now

    End Sub

    Protected Sub enviarCorreo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles enviarCorreo.Click

        If titulo.Text = "" Or titulo.Text = "titulo" Or mensaje.Text = "" Then
            Response.Write("<script language='javascript'>alert('Indique el titulo y el mensaje');</script>")
            Exit Sub
        End If

        Dim row As GridViewRow
        Dim i

        Dim elcorreo2 As New System.Net.Mail.MailMessage
        Using elcorreo2
            elcorreo2.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
            For i = 1 To GridView3.Rows.Count
                row = GridView3.Rows(i - 1)
                elcorreo2.Bcc.Add(row.Cells(7).Text)
            Next i
            elcorreo2.Subject = titulo.Text.Trim
            elcorreo2.Body = "<html><body>" + mensaje.Text + "<br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
            elcorreo2.IsBodyHtml = True
            elcorreo2.Priority = System.Net.Mail.MailPriority.Normal
            Dim smpt As New System.Net.Mail.SmtpClient
            smpt.Host = "smtp.gmail.com"
            smpt.Port = "587"
            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
            smpt.EnableSsl = True 'req p server gmail
            Try
                smpt.Send(elcorreo2)
                elcorreo2.Dispose()
            Catch ex As Exception
                Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
                Exit Sub
            Finally
                Response.Write("<script language='javascript'>alert('Mensaje enviado');</script>")
            End Try
        End Using

    End Sub

    Protected Sub addCand_Click(ByVal sender As Object, ByVal e As EventArgs) Handles addCand.Click
        If validaVacioscand() < 1 Then
            Exit Sub
        End If

        If validaDuplcand() < 1 Then
            Exit Sub
        End If
        Dim q As String
        q = "INSERT INTO candidatoDistrib(nombre,correo,tels,ciudad,estatus,obs) VALUES('" + candNombre.Text.Trim.ToUpper + "','" + candCorreo.Text.Trim.ToUpper + "','" + Trim(candTels.Text.Trim.ToUpper) + "','" + candCiudad.Text.Trim.ToUpper + "','" + candEstatus.Text + "','" + Left(candObservacion.Text.Trim, 100) + "')"

        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Close()

        'refrescar grid
        candId.Text = "ID"
        candNombre.Text = ""
        candCorreo.Text = ""
        candTels.Text = ""
        candCiudad.Text = ""
        candEstatus.Text = "VA"
        candObservacion.Text = ""
        GridView4.DataBind()
        cuentaRegistrosCand()
        Response.Write("<script language='javascript'>alert('Registro exitoso');</script>")

    End Sub

    Protected Sub editCand_Click(ByVal sender As Object, ByVal e As EventArgs) Handles editCand.Click
        If candId.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If
        If validaVacioscand() < 1 Then
            Exit Sub
        End If

        If validaDuplModcand() < 1 Then
            Exit Sub
        End If
        Dim q As String
        q = "UPDATE candidatoDistrib SET nombre='" + candNombre.Text.Trim.ToUpper + "',correo='" + candCorreo.Text.Trim.ToUpper + "',tels='" + Trim(candTels.Text.Trim) + "', ciudad='" + candCiudad.Text.Trim + "', estatus='" + candEstatus.Text + "', obs='" + Left(candObservacion.Text.Trim, 100) + "' WHERE id=" + candId.Text
        myCommand = New SqlCommand(q, myConnection)
        myCommand.ExecuteNonQuery()

        'refrescar grid
        candId.Text = "ID"
        candNombre.Text = ""
        candCorreo.Text = ""
        candTels.Text = ""
        candCiudad.Text = ""
        candEstatus.Text = "VA"
        candObservacion.Text = ""
        GridView4.DataBind()
        cuentaRegistrosCand()
        Response.Write("<script language='javascript'>alert('Actualización exitosa');</script>")

    End Sub

    Protected Sub delCand_Click(ByVal sender As Object, ByVal e As EventArgs) Handles delCand.Click
        If candId.Text = "ID" Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If

        'del cascadas

        Dim q = "DELETE FROM candidatoDistrib WHERE id=" + Trim(candId.Text)
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Close()

        'refrescar grid
        candId.Text = "ID"
        candNombre.Text = ""
        candCorreo.Text = ""
        candTels.Text = ""
        candCiudad.Text = ""
        candEstatus.Text = "VA"
        candObservacion.Text = ""
        GridView4.DataBind()
        cuentaRegistrosCand()
        Response.Write("<script language='javascript'>alert('Se ha eliminado');</script>")

    End Sub

    Protected Sub invitar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles invitar.Click
        Dim q As String
        q = "SELECT nombre, correo FROM candidatoDistrib WHERE estatus IS NULL OR estatus='VA'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()

        Dim elcorreo2 As New System.Net.Mail.MailMessage
        Using elcorreo2
            elcorreo2.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
            While dr.Read()
                If dr("correo") <> "0" Then
                    elcorreo2.Bcc.Add(dr("correo"))
                    myCommand = New SqlCommand("UPDATE candidatoDistrib SET estatus='NO' WHERE correo='" + dr("correo") + "'", myConnection)
                    myCommand.ExecuteNonQuery()
                End If
            End While
            dr.Close()
            elcorreo2.Subject = "Te invitamos a formar parte de nuestra red de distribuidores, genera tus propios ingresos con este gran negocio"
            elcorreo2.Body = "<html><body>Gracias por contactarnos, y darte tu mismo la oportunidad de crecer en una oportunidad de negocios donde todos los prospectos a clientes están hambrientos de nuestro servicio, ayudanos a difundirlo y gana comisiones recurrentes de los contratos de los clientes que logres afiliar a nuestro sistema, <br><br>Ofrecemos la solución para presentar y enviar declaraciones del impuesto a los depositos en efectivo (IDE) por internet, algo que ningun paquete contable ofrece<br>El mercado al que está destinado nuestro servicio esta constituido por toda institucion financiera y empresa que por ley retenga este impuesto por recibir excedentes de depositos en efectivo, que aparte de poder acceder a ellos directamente puedes hacerlo contactando contadores y despachos contables y fiscales que llevan cientos de estos clientes <br><br>Toda la informacion para distribuidores puedes consultarla en <a href='declaracioneside.com/distribuidores.aspx'>declaracioneside.com/distribuidores.aspx</a> donde puedes registrarte de inmediato, así como descargar nuestra propuesta de negocios completa<br>Da un vistazo a nuestra pagina principal <a href='declaracioneside.com'>declaracioneside.com</a><br> No dejes pasar esta oportunidad<br><br>Atentamente,<br><br><a href='declaracioneside.com' target='_blank'>Declaracioneside.com</a><br>Tu solución en declaraciones de depósitos en efectivo por internet</body></html>"
            elcorreo2.IsBodyHtml = True
            elcorreo2.Priority = System.Net.Mail.MailPriority.Normal
            Dim smpt As New System.Net.Mail.SmtpClient
            smpt.Host = "smtp.gmail.com"
            smpt.Port = "587"
            smpt.Credentials = New System.Net.NetworkCredential("declaracioneside@gmail.com", "ywuxdaffpyskcsuv")
            smpt.EnableSsl = True 'req p server gmail
            Try
                smpt.Send(elcorreo2)
                elcorreo2.Dispose()
            Catch ex As Exception
                Response.Write("<script language='javascript'>alert('Error: " & ex.Message + "');</script>")
                Exit Sub
            Finally
                Response.Write("<script language='javascript'>alert('Mensaje enviado');</script>")
            End Try
        End Using

        GridView4.DataBind()

    End Sub

    Private Function validaVacioscand() As Integer
        If Trim(candNombre.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el nombre');</script>")
            candNombre.Focus()
            Return 0
        End If
        If Trim(candCorreo.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el correo');</script>")
            candCorreo.Focus()
            Return 0
        End If

        If Trim(candTels.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique el telefono');</script>")
            candTels.Focus()
            Return 0
        End If
        If Trim(candCiudad.Text) = "" Then
            Response.Write("<script language='javascript'>alert('Especifique la ciudad');</script>")
            candCiudad.Focus()
            Return 0
        End If        
        Return 1
    End Function

    Private Function validaDuplcand() As Integer
        Dim q
        q = "SELECT id FROM candidatoDistrib WHERE nombre='" + Trim(candNombre.Text.Trim.ToUpper) + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            dr.Close()
            Response.Write("<script language='javascript'>alert('Ese candidato ya existe');</script>")
            Return 0
        End If
        dr.Close()

        Return 1
    End Function

    Private Function validaDuplModcand() As Integer
        Dim q
        q = "SELECT nombre FROM candidatoDistrib WHERE ID='" + Trim(candId.Text) + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        pkCandNombre = dr("nombre").ToString()
        dr.Close()

        q = "SELECT * FROM candidatoDistrib WHERE nombre='" + Trim(candNombre.Text.ToUpper) + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() And (pkCandNombre <> Trim(candNombre.Text.ToUpper)) Then   'if dr.Read() and (pkPorcen <> trim(porcen.text) or pk2 <>trim(campo2)) then
            Response.Write("<script language='javascript'>alert('Nombre candidato ya está en uso');</script>")
            dr.Close()
            Return 0
        End If
        dr.Close()

        Return 1
    End Function

    Protected Sub GridView4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView4.SelectedIndexChanged
        Dim row As GridViewRow = GridView4.SelectedRow
        candId.Text = row.Cells(1).Text
        candNombre.Text = Server.HtmlDecode(row.Cells(2).Text)
        candCorreo.Text = Server.HtmlDecode(row.Cells(3).Text)
        candTels.Text = row.Cells(4).Text
        candCiudad.Text = Server.HtmlDecode(row.Cells(5).Text)
        candEstatus.Text = Server.HtmlDecode(row.Cells(6).Text)
        candObservacion.Text = Server.HtmlDecode(row.Cells(7).Text)
    End Sub

    'Protected Sub informacion_Click(sender As Object, e As EventArgs) Handles informacion.Click
    '    MultiView1.ActiveViewIndex = Int32.Parse(0)
    'End Sub

    Protected Sub Buscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Buscar.Click
        If chkCorreo.Checked = False And chkEstatus.Checked = False And chkProgramadas.Checked = False And ultFecha.Checked = False And chkNotas.Checked = False And chkProspecto.Checked = False And mios.Checked = False And chkDistr.Checked = False Then
            Response.Write("<script language='javascript'>alert('Marque criterio(s) de búsqueda ');</script>")
            Exit Sub
        End If

        Dim q

        SqlDataSource1.SelectCommand = "SELECT id,cliente,estatusActual,fecha,idDistribuidor,notas,correo,fechaProgramada,telsInvalidos,tipo FROM [prospeccion] WHERE 1=1"
        If chkCorreo.Checked = True Then
            SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND correo like '%" + correoBus.Text.ToUpper + "%'"
        End If
        If chkEstatus.Checked = True Then
            SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND estatusActual ='" + estatusBus.SelectedValue.ToString + "'"
        End If
        If mios.Checked = True Then
            SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND idDistribuidor=" + id.Text
        End If
        If chkDistr.Checked = True Then
            SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND idDistribuidor=" + txtDistr.Text
        End If
        If chkProgramadas.Checked = True Then
            If IsDate(fechaProg.Text) = False Then
                Response.Write("<script language='javascript'>alert('Formato invalido en fecha programada');</script>")
                Exit Sub
            Else
                SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND CONVERT(VARCHAR(10),fechaProgramada,120)='" + Format(Convert.ToDateTime(fechaProg.Text.Trim), "yyyy-MM-dd") + "'"
            End If            
        End If
        If ultFecha.Checked = True Then
            If IsDate(ultFechaBus.Text) = False Then
                Response.Write("<script language='javascript'>alert('Formato invalido en ultima fecha');</script>")
                Exit Sub
            Else
                SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND CONVERT(VARCHAR(10),fecha,120)='" + Format(Convert.ToDateTime(ultFechaBus.Text.Trim), "yyyy-MM-dd") + "'"
            End If
        End If
        If chkNotas.Checked = True Then
            SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND notas like '%" + notasBus.Text.ToUpper + "%'"
        End If
        If chkProspecto.Checked = True Then
            SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND cliente LIKE '%" + prosNom.Text.Trim.ToUpper + "%'"
        End If

        If sinCC.Checked = True Then
            SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND estatusActual<>'CC'"
        End If
        If sinVA.Checked = True Then
            SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND estatusActual<>'VA'"
        End If
        If sinVL.Checked = True Then
            SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND estatusActual<>'VL'"
        End If
        If sinCO.Checked = True Then
            SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND estatusActual<>'CO'"
        End If
        If sinLL.Checked = True Then
            SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND estatusActual<>'LL'"
        End If
        If sinOK.Checked = True Then
            SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND estatusActual<>'OK'"
        End If
        If sinRE.Checked = True Then
            SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand + " AND estatusActual<>'RE'"
        End If

        hiddenBus.Value = SqlDataSource1.SelectCommand
        GridView1.DataBind()
        q = SqlDataSource1.SelectCommand.Replace("SELECT id,cliente,estatusActual,fecha,idDistribuidor,notas,correo,fechaProgramada,telsInvalidos,tipo", "SELECT COUNT(id) as cuenta")
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        prospeccionNregs.Text = dr("cuenta").ToString + " registros"
        dr.Close()
        GridView1.PageIndex = 0

        If GridView1.Rows.Count = 0 Then
            idProspeccion.Text = "ID"
            cliente.Text = ""
            estatusActual.SelectedIndex = 0
            notas.Text = ""
            correoProspeccion.Text = ""
            telsInvalidos.Text = ""
            tipo.SelectedIndex = 0
            fecha.Text = "ID"
            fechaProgramada.SelectedDates.Clear()
        End If

    End Sub

    Protected Sub GridView3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView3.SelectedIndexChanged
        Dim row As GridViewRow = GridView3.SelectedRow
        If row.Cells(9).Text = "0" Then 'sin clisForzosos
            comisCaduca.Checked = row.Cells(10).Text
            comisMesesCaducidad.Text = row.Cells(11).Text
            comisPorcen.Text = row.Cells(12).Text
        End If
    End Sub

    Protected Sub modComision_Click(sender As Object, e As EventArgs) Handles modComision.Click
        If GridView3.SelectedRow.Equals(False) Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If

        Dim row As GridViewRow = GridView3.SelectedRow
        Dim q
        Dim comisCaducaValue
        If comisCaduca.Checked = True Then
            comisCaducaValue = "1"
        Else
            comisCaducaValue = "0"
        End If
        q = "UPDATE distribuidores SET clisForzosos=0, comisCaduca=" + comisCaducaValue + ",comisMesesCaducidad='" + comisMesesCaducidad.Text.Trim + "',comisPorcen='" + Trim(comisPorcen.Text) + "' WHERE id='" + row.Cells(1).Text + "'"
        myCommand = New SqlCommand(q, myConnection)
        myCommand.ExecuteNonQuery()

        GridView3.DataBind()

        Response.Write("<script language='javascript'>alert('Comisión modificada');</script>")
    End Sub

    Protected Sub transferir_Click(sender As Object, e As EventArgs) Handles Transferira.Click
        If transferido.Text.Trim = "" Then
            Response.Write("<script language='javascript'>alert('Indique el # de distribuidor al que desea transferir la prospección');</script>")
            Exit Sub
        End If

        If idProspeccion.Text = "" Then
            Response.Write("<script language='javascript'>alert('Seleccione primero un registro a transferir');</script>")
            Exit Sub
        End If

        Dim q = "SELECT id FROM distribuidores WHERE ID='" + Trim(transferido.Text.Trim) + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If Not dr.Read() Then
            Response.Write("<script language='javascript'>alert('Distribuidor no localizado');</script>")
            Exit Sub
        End If
        dr.Close()

        q = "UPDATE prospeccion SET idDistribuidor='" + transferido.Text.Trim + "' WHERE id='" + idProspeccion.Text + "'"
        myCommand = New SqlCommand(q, myConnection)
        myCommand.ExecuteNonQuery()

        Response.Write("<script language='javascript'>alert('Transferido');</script>")

    End Sub

    Protected Sub esEmpleado_Click(sender As Object, e As EventArgs) Handles esEmpleado.Click
        If GridView3.SelectedRow.Equals(False) Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If
        myCommand = New SqlCommand("UPDATE distribuidores SET esEmpleado=1 WHERE id=" + id.Text.ToString, myConnection)
        myCommand.ExecuteNonQuery()
    End Sub

    Protected Sub eliminar_Click(sender As Object, e As EventArgs) Handles eliminar.Click
        If GridView3.SelectedRow.Equals(False) Then
            Response.Write("<script language='javascript'>alert('Elija 1o un registro');</script>")
            Exit Sub
        End If

        Dim q = "SELECT id FROM clientes WHERE idDistribuidor=" + GridView3.SelectedRow.Cells(1).Text
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            Response.Write("<script language='javascript'>alert('Tiene clientes vinculados');</script>")
            Exit Sub
        End If
        dr.Close()

        q = "SELECT id FROM prospeccion WHERE idDistribuidor=" + GridView3.SelectedRow.Cells(1).Text
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            Response.Write("<script language='javascript'>alert('Tiene prospecciones');</script>")
            Exit Sub
        End If
        dr.Close()

        myCommand = New SqlCommand("DELETE FROM distribuidores WHERE id=" + GridView3.SelectedRow.Cells(1).Text, myConnection)
        myCommand.ExecuteNonQuery()
        GridView3.DataBind()
        Response.Write("<script language='javascript'>alert('Eliminado ok');</script>")
    End Sub

    'Protected Sub misInstituciones_Click(sender As Object, e As EventArgs) Handles misInstituciones.Click
    '    Dim q = "SELECT esEmpleado FROM distribuidores WHERE id=" + id.Text.ToString
    '    myCommand = New SqlCommand(q, myConnection)
    '    dr = myCommand.ExecuteReader()
    '    dr.Read()
    '    If dr("doctos").Equals(False) Then
    '        dr.Close()
    '        Response.Write("<script language='javascript'>alert('No tienes autorización, ');</script>")
    '        Exit Sub
    '    End If
    '    dr.Close()

    '    es distribuidor oficial
    '    q = "SELECT razonSoc FROM clientes WHERE idDistribuidor=" + id.Text.ToString
    '    myCommand = New SqlCommand(q, myConnection)
    '    dr = myCommand.ExecuteReader()
    '    Dim listaclientes = ""
    '    While dr.Read()
    '        listaclientes = listaclientes + dr("razonSoc") + " / "
    '    End While
    '    dr.Close()
    '    Response.Write("<script language='javascript'>alert('" + listaclientes + "');</script>")

    'End Sub

    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            For Each cell As TableCell In e.Row.Cells
                If e.Row.Cells(3).Text = "CC" Then
                    cell.BackColor = System.Drawing.Color.Silver
                End If
                If InStr(e.Row.Cells(6).Text, "CLIENTE") > 0 Then
                    cell.BackColor = System.Drawing.Color.Gold
                End If
            Next
        End If
    End Sub
    
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ordenar.Click

        If orden.SelectedValue = "1" Then
            SqlDataSource1.SelectCommand = hiddenBus.Value + " ORDER BY fechaProgramada DESC"
        ElseIf orden.SelectedValue = "2" Then
            SqlDataSource1.SelectCommand = hiddenBus.Value + " ORDER BY cliente ASC"
        ElseIf orden.SelectedValue = "3" Then
            SqlDataSource1.SelectCommand = hiddenBus.Value + " ORDER BY fecha DESC"
        ElseIf orden.SelectedValue = "5" Then
            SqlDataSource1.SelectCommand = hiddenBus.Value + " ORDER BY fecha ASC"
        ElseIf orden.SelectedValue = "4" Then
            SqlDataSource1.SelectCommand = hiddenBus.Value + " ORDER BY fechaProgramada ASC"
        End If
        'descr.Text = "hideen=" + hiddenBus.Value + ". SQL=" + SqlDataSource1.SelectCommand
        GridView1.DataBind()
    End Sub

    Protected Sub export_Click(ByVal sender As Object, ByVal e As EventArgs) Handles exportar.Click


        'Dim oExcel As Excel.Application
        'Dim oBook As Excel.Workbook
        'Dim oSheet As Excel.Worksheet
        'oExcel = CreateObject("Excel.Application")
        'oBook = oExcel.Workbooks.Add(Type.Missing)
        'oSheet = oBook.Worksheets(1)

        'Dim colIndex As Integer = 1
        'Dim rowIndex As Integer = 1

        ''Dim q

        'q = "select correo from prospeccion where correo is not null and correo <>''"
        'myCommand = New SqlCommand(q, myConnection)
        'dr = myCommand.ExecuteReader()
        'If dr.HasRows Then
        '    While dr.Read()
        '        Dim campo = Regex.Replace(dr("correo"), "\s+", " ")
        '        If InStr(campo, ",") > 0 Then
        '            Dim words As String() = campo.Split(New Char() {","c})
        '            Dim word As String
        '            For Each word In words
        '                If InStr(word, " ") > 0 Then
        '                    Dim words2 As String() = word.Split(New Char() {" "c})
        '                    Dim word2 As String
        '                    For Each word2 In words2
        '                        If word2.Trim <> "" Then
        '                            oSheet.Cells(rowIndex, 1) = word2.Trim
        '                            rowIndex += 1
        '                        End If
        '                    Next
        '                Else
        '                    If word.Trim <> "" Then
        '                        oSheet.Cells(rowIndex, 1) = word.Trim
        '                        rowIndex += 1
        '                    End If
        '                End If
        '            Next
        '        ElseIf InStr(campo, " ") > 0 Then
        '            Dim words2 As String() = campo.Split(New Char() {" "c})
        '            Dim word2 As String
        '            For Each word2 In words2
        '                If word2.Trim <> "" Then
        '                    oSheet.Cells(rowIndex, 1) = word2.Trim
        '                    rowIndex += 1
        '                End If
        '            Next
        '        Else
        '            oSheet.Cells(rowIndex, 1) = campo
        '            rowIndex += 1
        '        End If
        '    End While
        'End If
        'dr.Close()

        'Dim fileName As String = "C:\SAT\prospeccion" + Now.ToString("dd-MM-yyyy") + ".xls"
        'oSheet.Columns.AutoFit()
        ''Save file in final path
        'oBook.SaveAs(fileName, XlFileFormat.xlWorkbookNormal, Type.Missing,
        'Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive,
        'Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing)

        ''Release the objects
        'ReleaseObject(oSheet)
        'oBook.Close(False, Type.Missing, Type.Missing)
        'ReleaseObject(oBook)
        'oExcel.Quit()
        'ReleaseObject(oExcel)
        ''Some time Office application does not quit after automation: 
        ''so i am calling GC.Collect method.
        'GC.Collect()
        'Response.Write("<script language='javascript'>alert('Exportado en " + fileName + "');</script>")
    End Sub

    Private Sub ReleaseObject(ByVal o As Object)
        Try
            While (System.Runtime.InteropServices.Marshal.ReleaseComObject(o) > 0)
            End While
        Catch
        Finally
            o = Nothing
        End Try
    End Sub

    Protected Sub exportarBD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles exportarBD.Click
        Dim q = "select DISTINCT correo from prospeccion where correo is not null and correo <>''"
        If chkClientes.Checked.Equals(False) Then 'los de prospeccion pero no los de clientes, ni los de otroscorreos de clientes
            'q = q + " AND correo NOT IN (SELECT P.CORREO FROM clientes C, prospeccion P WHERE P.correo LIKE '%'+C.correo+'%')"
            q = "select distinct P.correo FROM prospeccion P left outer join  clientes C on P.correo LIKE '%'+C.correo+'%' where C.correo IS Null and P.correo is not null and P.correo<>'' AND p.correo not in (SELECT distinct P.CORREO FROM prospeccion P INNER JOIN CLIENTES C ON P.correo LIKE '%'+C.otroscorreos+'%')"
        End If
        If chkPeriodo.Checked.Equals(True) Then
            q = q + " AND fecha >= '" + Format(Convert.ToDateTime(CDate(perDesde.Text.Trim)), "yyyy-MM-dd") + "' AND fecha <= '" + Format(Convert.ToDateTime(CDate(perHasta.Text.Trim)), "yyyy-MM-dd") + "'"
        End If
        q = q + " order by correo"
        myCommand = New SqlCommand(q, myConnection)

        dr = myCommand.ExecuteReader()

        Dim var
        var = ""
        If dr.HasRows Then
            While dr.Read()
                var = var + dr("correo") + ", "
            End While
        End If
        dr.Close()

        Dim strFile As String = "C:\SAT\prospeccionIDE_" & DateTime.Today.ToString("dd-MMM-yyyy") & ".txt"
        Dim sw As StreamWriter
        Try
            If (File.Exists(strFile)) Then
                File.Delete(strFile)
            End If
            sw = File.CreateText(strFile)
            sw.WriteLine(var)
            sw.Close()
        Catch ex As IOException
            MsgBox(ex.StackTrace.ToString)
        End Try

        Dim MSG = "<script language='javascript'>alert('Se creo el archivo " + strFile + " en el server');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
        'MultiView1.ActiveViewIndex = Int32.Parse(5)


    End Sub

    Protected Sub prevYear_Click(sender As Object, e As EventArgs) Handles prevYear.Click
        fechaProgramada.VisibleDate = fechaProgramada.VisibleDate.AddYears(-1)
    End Sub

    Protected Sub nextYear_Click(sender As Object, e As EventArgs) Handles nextYear.Click
        fechaProgramada.VisibleDate = fechaProgramada.VisibleDate.AddYears(1)
    End Sub

    Protected Sub irHoy_Click(sender As Object, e As EventArgs) Handles irHoy.Click
        fechaProgramada.SelectedDate = Format(Now(), "yyyy-MM-dd")
        fechaProgramada.VisibleDate = fechaProgramada.SelectedDate
    End Sub

    Protected Sub irFecha_Click(sender As Object, e As EventArgs) Handles irFecha.Click
        If Not IsDate(fechaIr.Text.Trim) Then
            Response.Write("<script language='javascript'>alert('Fecha invalida');</script>")
        End If
        fechaProgramada.SelectedDate = fechaIr.Text.Trim
        fechaProgramada.VisibleDate = fechaProgramada.SelectedDate
    End Sub

    Protected Sub PageIndexChanging(sender As Object, e As GridViewPageEventArgs) 'Handles GridView1.PageIndexChanging
        'GridView1.PageIndex = e.NewPageIndex
        'SqlDataSource1.SelectCommand = hiddenBus.Value
        'GridView1.DataSourceID = "SqlDataSource1"
        'GridView1.DataBind()
    End Sub

    Protected Sub Transferira_Click(sender As Object, e As EventArgs) Handles Transferira.Click

    End Sub

    Protected Sub lim_Click(sender As Object, e As EventArgs) Handles lim.Click
        idProspeccion.Text = "ID"
        cliente.Text = ""
        estatusActual.Text = "VA"
        fecha.Text = ""
        notas.Text = ""
        correoProspeccion.Text = ""
        GridView1.SelectedIndex = -1

        fechaProgramada.SelectedDates.Clear()
        telsInvalidos.Text = ""
        tipo.SelectedIndex = 0 '1er item

    End Sub

    Private Sub chkPeriodo_CheckedChanged(sender As Object, e As EventArgs) Handles chkPeriodo.CheckedChanged
        If chkPeriodo.Checked.Equals(True) Then
            perDesde.Visible = True
            perHasta.Visible = True
        Else
            perDesde.Visible = False
            perHasta.Visible = False
        End If
    End Sub

    Protected Sub exportarexcel_Click(sender As Object, e As EventArgs) Handles exportarexcel.Click
        If GridView1.Rows.Count < 1 Then
            Dim MSG As String = "<script language='javascript'>alert('Nada que exportar');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Exit Sub
        End If

        Dim arch = "C:\SAT\prospeccionIDE.xlsx"

        If File.Exists(arch) Then
            File.Delete(arch)
        End If

        Try

            Dim oExcel As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application
            Dim oBook As Microsoft.Office.Interop.Excel.Workbook = oExcel.Workbooks.Add
            Dim oSheet As Microsoft.Office.Interop.Excel.Worksheet = oBook.Sheets(1)

            oSheet.Cells(2, 1).value = "ID"
            oSheet.Cells(2, 2).value = "Prospecto"
            oSheet.Cells(2, 3).value = "Estatus"
            oSheet.Cells(2, 4).value = "UltModif"
            oSheet.Cells(2, 5).value = "idDistr"
            oSheet.Cells(2, 6).value = "Notas"
            oSheet.Cells(2, 7).value = "Correos"
            oSheet.Cells(2, 8).value = "fechaProgramada"
            oSheet.Cells(2, 9).value = "telsinvalidos"
            oSheet.Cells(2, 10).value = "tipo"

            oSheet.Cells(2, 1).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 1).Font.Bold = True ' Fuente en negrita
            oSheet.Cells(2, 2).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 2).Font.Bold = True ' Fuente en negrita
            oSheet.Cells(2, 3).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 3).Font.Bold = True ' Fuente en negrita
            oSheet.Cells(2, 4).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 4).Font.Bold = True ' Fuente en negrita
            oSheet.Cells(2, 5).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 5).Font.Bold = True ' Fuente en negrita
            oSheet.Cells(2, 6).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 6).Font.Bold = True ' Fuente en negrita
            oSheet.Cells(2, 7).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 7).Font.Bold = True ' Fuente en negrita
            oSheet.Cells(2, 8).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 8).Font.Bold = True ' Fuente en negrita
            oSheet.Cells(2, 9).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 9).Font.Bold = True ' Fuente en negrita
            oSheet.Cells(2, 10).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 10).Font.Bold = True ' Fuente en negrita

            oSheet.Columns("A:A").EntireColumn.AutoFit()
            oSheet.Columns("B:B").EntireColumn.AutoFit()
            oSheet.Columns("C:C").EntireColumn.AutoFit()
            oSheet.Columns("D:D").EntireColumn.AutoFit()
            oSheet.Columns("E:E").EntireColumn.AutoFit()
            oSheet.Columns("F:F").EntireColumn.AutoFit()
            oSheet.Columns("G:G").EntireColumn.AutoFit()
            oSheet.Columns("H:H").EntireColumn.AutoFit()
            oSheet.Columns("I:I").EntireColumn.AutoFit()
            oSheet.Columns("J:J").EntireColumn.AutoFit()

            oSheet.Range("D:D").NumberFormat = "dd/MM/yyyy" ' "###,###,###,##0.000000" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha
            oSheet.Range("H:H").NumberFormat = "dd/MM/yyyy"

            Dim ren = 3
            For Each row As GridViewRow In GridView1.Rows
                oSheet.Cells(ren, 1).value = row.Cells(1).Text
                oSheet.Cells(ren, 2).value = Server.HtmlDecode(row.Cells(2).Text)
                If row.Cells(3).Text = "" Or row.Cells(3).Text = " " Or row.Cells(3).Text = "&nbsp;" Or row.Cells(3).Text = "VA" Then
                    oSheet.Cells(ren, 3).value = "Vacío"
                End If
                If row.Cells(3).Text = "CO" Then
                    oSheet.Cells(ren, 3).value = "Correo enviado"
                End If
                If row.Cells(3).Text = "VL" Then
                    oSheet.Cells(ren, 3).value = "Correo confirmado"
                End If
                If row.Cells(3).Text = "LL" Then
                    oSheet.Cells(ren, 3).value = "Llamando"
                End If
                If row.Cells(3).Text = "RE" Then
                    oSheet.Cells(ren, 3).value = "Renovar contrato"
                End If
                If row.Cells(3).Text = "OK" Then
                    oSheet.Cells(ren, 3).value = "OK al corriente"
                End If
                If row.Cells(3).Text = "CC" Then
                    oSheet.Cells(ren, 3).value = "Caso cerrado"
                End If
                If row.Cells(3).Text = "BA" Then
                    oSheet.Cells(ren, 3).value = "Baja"
                End If

                oSheet.Cells(ren, 4).value = Left(row.Cells(4).Text, 10)
                'oSheet.Cells(ren, 5).value = row.Cells(5).Text
                If row.Cells(6).Text <> "&nbsp;" And row.Cells(6).Text <> "NULL" Then
                    oSheet.Cells(ren, 6).value = Server.HtmlDecode(row.Cells(6).Text)
                End If
                If row.Cells(7).Text <> "&nbsp;" And row.Cells(7).Text <> "NULL" Then
                    oSheet.Cells(ren, 7).value = Server.HtmlDecode(row.Cells(7).Text)
                End If
                If row.Cells(8).Text <> "&nbsp;" And row.Cells(8).Text <> "NULL" Then
                    oSheet.Cells(ren, 8).value = Left(row.Cells(8).Text, 10)
                End If
                If row.Cells(9).Text <> "&nbsp;" And row.Cells(9).Text <> "NULL" Then
                    oSheet.Cells(ren, 9).value = row.Cells(9).Text
                End If

                If row.Cells(10).Text = "1" Then
                    oSheet.Cells(ren, 10).value = "Dir"
                End If
                If row.Cells(10).Text = "2" Then
                    oSheet.Cells(ren, 10).value = "Interm"
                End If
                If row.Cells(10).Text = "3" Then
                    oSheet.Cells(ren, 10).value = "Publici"
                End If

                ren = ren + 1
            Next

            oExcel.Visible = False
            oExcel.UserControl = True
            oExcel.DisplayAlerts = False

            oBook.SaveAs(arch)    'SaveCopyAs(arch)
            oBook.Close(True)
            oBook = Nothing
            oExcel.Quit()
            oExcel = Nothing

            Response.Clear()
            Response.ClearHeaders()
            Response.ClearContent()
            Response.AddHeader("content-disposition", "attachment; filename=" + Path.GetFileName("C:\SAT\prospeccionIDE.xlsx"))
            Response.ContentType = "application/vnd.ms-excel"
            Response.WriteFile(arch)
            Response.End()

            File.Delete(arch)
            Dim MSG As String = "<script language='javascript'>alert('Descarga ok');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)

        Catch ex As Exception
            Dim MSG As String = "<script language='javascript'>alert('Error excepcion: " + ex.Message + "');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)

        End Try
    End Sub
End Class