Imports System.Runtime.InteropServices
Imports System.Security
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Public Class _Default
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim dr As SqlDataReader
    Dim dr2 As SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ipServidor = "200.77.146.31" 'ip publica fija asignada por proveedor telmex o megacable
        nombreServidor = "tcp:." 'nombre asignado al servidor
        'nombreServidor = "tcp:job-PC" 'nombre asignado al servidor
        platafServer = "WINDOWS SERVER 2008 R2"
        loginTx = "Administrator"
        rutaSAT = "C:\SAT"
        loginRxSAT = "SAT"
        passS = "Djobjosue2"
        asesoriaPrecioBase = 350

        myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';")
        myConnection.Open()

        myCommand = New SqlCommand("set dateformat ymd", myConnection)
        myCommand.ExecuteNonQuery()

        If (Not System.IO.Directory.Exists("C:\inetpub\wwwroot\autorizaciones")) Then
            System.IO.Directory.CreateDirectory("C:\inetpub\wwwroot\autorizaciones")
        End If
        If (Not System.IO.Directory.Exists("C:\inetpub\wwwroot\xmlSubidos")) Then
            System.IO.Directory.CreateDirectory("C:\inetpub\wwwroot\xmlSubidos")
        End If

        For Each FileFound As String In Directory.GetFiles("C:\inetpub\wwwroot\autorizaciones", "*.*")
            File.Delete(FileFound) 'borra locales
        Next

        If Not IsPostBack Then '1a vez                        
            If Not Request.QueryString("id") Is Nothing Then 'implementando hopads de distribuidores
                myCommand = New SqlCommand("SELECT id FROM distribuidores WHERE id=" + Request.QueryString("id").ToString + " and doctos=1", myConnection)
                dr = myCommand.ExecuteReader()
                If Not dr.Read() Then
                    dr.Close()
                    Session("refDistribuidor") = "1"
                Else
                    dr.Close()
                    Session("refDistribuidor") = Request.QueryString("id")
                End If
            Else
                If Not Session("refDistribuidor") Is Nothing And Session("refDistribuidor") <> "1" Then

                Else
                    Session("refDistribuidor") = "1"
                End If
            End If

            If Not (Left(Request.ServerVariables("REMOTE_ADDR"), 10) = "192.168.0." Or Left(Request.ServerVariables("REMOTE_ADDR"), 10) = "127.0.0.1" Or Request.QueryString("lan") IsNot Nothing Or HttpContext.Current.Request.IsLocal) Then 'red local
                Session("runAsAdmin") = "0"
            Else
                If Request.QueryString("lan") IsNot Nothing Then
                    If Request.QueryString("lan") <> "1" Then
                        Session("runAsAdmin") = "0"
                    Else
                        Session("runAsAdmin") = "1"
                    End If
                Else
                    Session("runAsAdmin") = "1"
                End If
            End If




            Session("curCorreo") = ""

            If DatePart(DateInterval.Day, Now) = 1 Then 'dia 1o de cada mes, revisa enviar correo p recordatorio de gen. facts d parcialidades
                'myCommand = New SqlCommand("SELECT msgFactParciales FROM actuales", myConnection)
                'dr = myCommand.ExecuteReader()
                'dr.Read()
                'If dr("msgFactParciales").Equals(False) Then
                '    dr.Close()
                '    Dim elcorreo As New System.Net.Mail.MailMessage
                '    Using elcorreo
                '        elcorreo.From = New System.Net.Mail.MailAddress("declaracioneside@gmail.com")
                '        elcorreo.To.Add("declaracioneside@gmail.com")
                '        elcorreo.Subject = "Enviar facturas de pagos parciales"
                '        elcorreo.Body = "<html><body>"
                '        myCommand = New SqlCommand("SELECT id, parcialidades, nAdeudos, montoAdeudos, vencido FROM contratos WHERE parcialidades=1 AND vencido=0", myConnection)
                '        dr2 = myCommand.ExecuteReader()
                '        While dr2.Read()
                '            elcorreo.Body = elcorreo.Body + "Contrato No. " + dr2("id").ToString() + "<br><br>"
                '        End While
                '        dr2.Close()
                '        elcorreo.Body = elcorreo.Body + "</body></html>"
                '        elcorreo.IsBodyHtml = True
                '        elcorreo.Priority = System.Net.Mail.MailPriority.Normal
                '        Dim smpt As New System.Net.Mail.SmtpClient
                '        smpt.Host = "smtp.gmail.com"
                '        smpt.Port = "587"
                '        smpt.Credentials = New System.Net.NetworkCredential("declaracioneside", "declaracioneside2a.")
                '        smpt.EnableSsl = True 'req p server gmail
                '        Try
                '            smpt.Send(elcorreo)
                '            elcorreo.Dispose()
                '        Catch ex As Exception
                '            Response.Write("<script language='javascript'>alert('Error enviando recordatorio de facturas de pagos parciales: " & ex.Message + "');</script>")
                '            Exit Sub
                '        Finally
                '            myCommand = New SqlCommand("UPDATE actuales SET msgFactParciales=1", myConnection)
                '            myCommand.ExecuteNonQuery()                            
                '        End Try
                '    End Using
                'Else
                '    dr.Close()
                'End If
            Else
                myCommand = New SqlCommand("UPDATE actuales SET msgFactParciales=0", myConnection) 'restablezco var p recordatorio
                myCommand.ExecuteNonQuery()
            End If
            Session("refDistribuidor") = "1"

            Dim numAn, numMens
            Dim q = "select COUNT(id) as numAn from ideAnual where archivoXML is not null"
            myCommand = New SqlCommand(q, myConnection)
            dr = myCommand.ExecuteReader()
            dr.Read()
            numAn = dr("numAn")
            dr.Close()

            q = "select COUNT(id) as numMens from ideMens where archivoXML is not null"
            myCommand = New SqlCommand(q, myConnection)
            dr = myCommand.ExecuteReader()
            dr.Read()
            numMens = dr("numMens")
            dr.Close()

            nAcuses.Text = FormatNumber(CDbl(numAn) + CDbl(numMens), 0) 'FormatNumber(Directory.GetFiles("C:\SAT", "AA*.xml", SearchOption.AllDirectories).Length.ToString(), 0)

        End If
    End Sub


    Function IsValidEmail(ByVal strIn As String) As Boolean
        ' Return true if strIn is in valid e-mail format.
        Return Regex.IsMatch(strIn, ("^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
    End Function


End Class