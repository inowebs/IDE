Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Office.Interop.Excel
Imports System.Security.AccessControl
Imports System.Security
Imports System.IO


Public Class WebForm9
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim myCommand2 As SqlCommand
    Dim myCommand3 As SqlCommand
    Dim myCommand4 As SqlCommand
    Dim myCommand5 As SqlCommand
    Dim dr As SqlDataReader
    Dim dr2 As SqlDataReader
    Dim dr3 As SqlDataReader
    Dim dr4 As SqlDataReader
    Dim dr5 As SqlDataReader

    Protected Sub ingresar_Click(sender As Object, e As EventArgs) Handles ingresar.Click
        myCommand = New SqlCommand("SELECT id FROM admin WHERE nombre='" + pass1.Text.Trim + "'", myConnection)
        dr = myCommand.ExecuteReader()
        If Not dr.HasRows Then
            dr.Close()
            Response.Write("<script language='javascript'>alert('Contraseña incorrecta');</script>")
            btnOculto_Click(sender, e)
        Else
            dr.Close()
            Session("admonIn") = "1"
            'UpdatePanel3.Update()
            panel1_ModalPopupExtender.Hide()
        End If

    End Sub
    Protected Sub btnOculto_Click(sender As Object, e As EventArgs) Handles btnOculto.Click
        '                UpdatePanel3.Update()
        panel1_ModalPopupExtender.Show()
        Panel2.Style.Remove("display")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ToolkitScriptManager1.RegisterPostBackControl(ingresar)
        ToolkitScriptManager1.RegisterPostBackControl(btnOculto)
        If Not IsPostBack Then
            If Not String.IsNullOrEmpty(Request.QueryString("lan")) Then
                If Request.QueryString("lan") = "1" Then 'red local
                    If Session("admonIn") = "1" Then

                    Else
                        Session("admonIn") = "1"
                        btnOculto_Click(sender, e)
                    End If
                Else
                    Response.Write("<script language='javascript'>alert('Acceso denegado por su ubicación/forma de acceso');</script>")
                    Response.Write("<script>location.href='Login.aspx';</script>")
                End If
            Else
                If Session("admonIn") <> "1" Then
                    Response.Write("<script language='javascript'>alert('Acceso denegado por su ubicación/forma de acceso');</script>")
                    Response.Write("<script>location.href='Login.aspx';</script>")
                End If

            End If
        End If
        myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';MultipleActiveResultSets=true")
        myConnection.Open()
        myCommand = New SqlCommand("set dateformat ymd", myConnection)
        myCommand.ExecuteNonQuery()


        nRegs.Text = FormatNumber(GridView3.Rows.Count.ToString, 0) + " Registros"

        If Not IsPostBack Then  '1a vez
            
        End If

    End Sub

    Private Sub WebForm9_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'myConnection.Close()
    End Sub

    Private Function referenciaInversa(ByVal ref) As String
        Try
            'AddFileSecurity("C:\SAT\referencias.xlsx", Session("identidad"), FileSystemRights.ReadData, AccessControlType.Allow)
            Dim excel As Application = New Application
        Dim w As Workbook = excel.Workbooks.Open("C:\SAT\referencias.xlsx")
        Dim sheet As Worksheet = w.Sheets(1)
        Dim r As Range = sheet.UsedRange
        Dim array(,) As Object = r.Value(XlRangeValueDataType.xlRangeValueDefault)
        Dim i As Long
        If array IsNot Nothing Then
            For i = 1 To sheet.Range("A1").CurrentRegion.Rows.Count
                If array(i, 3).ToString = ref.ToString Then
                    w.Close()   'cierro excel y trabajo con la var
                    Return array(i, 1).ToString.Trim()
                End If
            Next
        End If
            w.Close(False)   'cierro excel y trabajo con la var
            Return ""
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('" + ex.Message + "');</script>")
        End Try
    End Function

    Sub AddFileSecurity(ByVal fileName As String, ByVal account As String, _
        ByVal rights As FileSystemRights, ByVal controlType As AccessControlType)

        Dim fSecurity As FileSecurity = File.GetAccessControl(fileName)
        Dim accessRule As FileSystemAccessRule = _
        New FileSystemAccessRule(account, rights, controlType)
        fSecurity.AddAccessRule(accessRule)
        File.SetAccessControl(fileName, fSecurity)

    End Sub

    Protected Sub obtPass_Click(sender As Object, e As EventArgs) Handles obtPass.Click
        If correo.Text.Trim = "" Then
            Exit Sub
        End If

        Dim q
        'Encriptacion
        myCommand = New SqlCommand("OPEN SYMMETRIC KEY SYM_KEY DECRYPTION BY PASSWORD ='##Djjcp##'", myConnection)
        myCommand.ExecuteNonQuery()

        q = "SELECT CAST(DECRYPTBYKEY(passWeb) AS VARCHAR(15)) as passWeb FROM clientes WHERE correo='" + Trim(correo.Text.Trim.ToUpper) + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        If dr.Read() Then
            pass.Text = dr("passWeb")
            dr.Close()
            myCommand = New SqlCommand("CLOSE SYMMETRIC KEY SYM_KEY", myConnection)
            myCommand.ExecuteNonQuery()
        Else
            dr.Close()
            myCommand = New SqlCommand("CLOSE SYMMETRIC KEY SYM_KEY", myConnection)
            myCommand.ExecuteNonQuery()
            pass.Text = "Correo no encontrado"
        End If

    End Sub

    Protected Sub Buscar_Click(sender As Object, e As EventArgs) Handles Buscar.Click
        SqlDataSource3.SelectCommand = "SELECT DISTINCT cli.[id], [correo], cli.[razonSoc], cli.[rfcDeclarante], cli.[casfim], cli.[loginsat], cli.[tel], cli.contacto, e.estatus, c.cel, c.fechaSolSocketSAT, c.fechaRegistro, c.fuente, c.dxFac, c.otrosCorreos FROM [clientes] cli, contratos co, ideAnual a, estatusCliente e WHERE e.id=cli.idEstatus AND cli.idEstatus=" + edoCli.SelectedValue.ToString
        'If pendientes.SelectedValue = "a" Then 'pend autoriz
        '    SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + "cli.solSocketEstatus='VACIA'"
        'ElseIf pendientes.SelectedValue = "s" Then 'pend socket
        '    SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + "cli.solSocketEstatus<>'APROBADA' AND cli.solSocketEstatus<>'VACIA'"
        'ElseIf pendientes.SelectedValue = "c" Then 'pend contratos
        '    SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + "cli.loginSAT IS NOT NULL AND cli.solSocketEstatus='APROBADA' AND cli.id NOT IN (SELECT idCliente FROM CONTRATOS)"
        'ElseIf pendientes.SelectedValue = "p" Then 'pend pagos
        '    SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + "co.idCliente=cli.id AND co.fechaPago IS NULL AND cli.solSocketEstatus='APROBADA'"
        'ElseIf pendientes.SelectedValue = "e" Then 'estatus
        '    SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + "cli.estatus='" + clientesEstatus.Text + "'"
        'ElseIf pendientes.SelectedValue = "d" Then 'sin decl ya pago
        '    SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + "co.idCliente=cli.id AND co.fechaPago IS NOT NULL AND cli.loginSAT IS NOT NULL AND cli.id NOT IN (SELECT idCliente FROM ideAnual)"
        'End If

        SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand + " ORDER BY cli.[razonSoc]"
        GridView3.DataBind()

        nRegs.Text = FormatNumber(GridView3.Rows.Count.ToString, 0) + " Registros"
    End Sub

    Private Function bajarAcuse(ByVal mensAn, ByVal idCliente, ByVal idDecla, ByVal ejercicio, ByVal normalComplementaria, ByVal mes) As String
        Dim loginSAT, directorioServidor, casfim, tipo, idArch, q
        q = "SELECT loginSAT,directorioServidor,casfim FROM clientes WHERE id=" + idCliente.ToString
        myCommand = New SqlCommand(q, myConnection)
        dr5 = myCommand.ExecuteReader()
        dr5.Read()
        loginSAT = dr5("loginSAT")
        directorioServidor = "C:\SAT\" + dr5("directorioServidor")
        casfim = dr5("casfim")
        dr5.Close()
        If normalComplementaria = "NORMAL" Then
            tipo = "N"
            idArch = ""
        Else
            tipo = "C"
            idArch = idDecla.ToString
        End If

        Dim nomArchMens, nomArchMensSinPath
        If mensAn = "m" Then
            nomArchMens = "C:\SAT\" + casfim + "\" + "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML"
            nomArchMensSinPath = "M-" + ejercicio.ToString + "-" + mes.ToString + tipo + idArch + ".XML"
        Else
            nomArchMens = "C:\SAT\" + casfim + "\" + "A-" + ejercicio.ToString + tipo + idArch + ".XML"
            nomArchMensSinPath = "A-" + ejercicio.ToString + tipo + idArch + ".XML"
        End If

        Dim di As New IO.DirectoryInfo(directorioServidor + "\")
        Dim diar1 As IO.FileInfo() = di.GetFiles("*.xml")
        Dim dra As IO.FileInfo
        Dim fName As String
        Dim allRead As String
        Dim regMatch As String 'string to search for inside of text file. It is case sensitive.
        regMatch = nomArchMensSinPath  'buscando el nomArchMensSinPath como texto dentro del archivo        

        Dim c As Integer
        c = 0
        For Each dra In diar1   'busca aceptaciones y rechachazos del archivo
            fName = dra.FullName 'path to text file                    
            Dim testTxt As StreamReader = New StreamReader(fName)
            allRead = testTxt.ReadToEnd() 'Reads the whole text file to the end
            testTxt.Close() 'Closes the text file after it is fully read.
            If (Regex.IsMatch(allRead, regMatch)) Then 'If match found in allRead
                c = 1
                Exit For
            End If
        Next
        If c = 0 Then
            Return "Sin Acuse"
        Else
            Return "Con Acuse"
        End If
    End Function

    Protected Sub verDecl_Click(sender As Object, e As EventArgs) Handles verDecl.Click
        Dim q, q2, q4, q3
        q = "SELECT id, correo, razonSoc FROM clientes"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        While dr.Read()
            Dim padre As New TreeNode()

            q4 = "select id,ejercicio,normalComplementaria from ideAnual where idCliente = " + dr("id").ToString + " and ejercicio in (select max(CAST (ejercicio AS INT)) as ejercicio from ideAnual a where idCliente=" + dr("id").ToString + ")"
            myCommand4 = New SqlCommand(q4, myConnection)
            dr4 = myCommand4.ExecuteReader()
            If dr4.Read() Then

                If dr4("id") Is DBNull.Value Or String.IsNullOrEmpty(dr4("id").ToString.Trim) Then
                Else
                    q2 = "select max(CAST(m.mes AS INT)) as mes from ideMens m, ideAnual a where m.idAnual=a.id and idAnual=" + dr4("id").ToString + " and (m.estado='ACEPTADA' or m.estado='CONTINGENCIA')"
                    myCommand2 = New SqlCommand(q2, myConnection)
                    dr2 = myCommand2.ExecuteReader()
                    dr2.Read()
                    If dr2("mes") Is DBNull.Value Or String.IsNullOrEmpty(dr2("mes").ToString.Trim) Then 'solo anual
                        padre.Text = dr("id").ToString + ", " + dr4("ejercicio").ToString + ", " + dr("razonSoc").ToString + ", " + bajarAcuse("a", dr("id").ToString, dr4("id").ToString, dr4("ejercicio").ToString, dr4("normalComplementaria"), "0")
                    Else
                        q3 = "select m.normalComplementaria, m.id from ideMens m, ideAnual a where m.mes='" + dr2("mes").ToString + "' and m.idAnual=" + dr4("id").ToString
                        myCommand2 = New SqlCommand(q3, myConnection)
                        dr3 = myCommand2.ExecuteReader()
                        dr3.Read()
                        padre.Text = dr("id").ToString + ", " + dr2("mes").ToString + "/" + dr4("ejercicio").ToString + ", " + dr("razonSoc").ToString + ", " + bajarAcuse("m", dr("id").ToString, dr3("id").ToString, dr4("ejercicio").ToString, dr3("normalComplementaria"), dr2("mes").ToString)
                    End If
                    TreeView1.Nodes.Add(padre)

                    Dim decls As New TreeNode()
                    decls.Text = "Declaraciones"
                    padre.ChildNodes.Add(decls)

                    Dim dr8 As SqlDataReader
                    q = "SELECT ideAn.id,ideAn.ejercicio, ideAn.estado,ideAn.normalComplementaria, ideAn.idContrato, ideAn.fechaEnvio, ideAn.acuseDescargado FROM ideAnual ideAn, clientes cli WHERE ideAn.idCliente=cli.id AND cli.id=" + dr("id").ToString + " order by cast(ejercicio as int),id"
                    myCommand = New SqlCommand(q, myConnection)
                    dr8 = myCommand.ExecuteReader()
                    If dr8.HasRows Then
                        While dr8.Read()
                            Dim ejercicio As New TreeNode()
                            ejercicio.Text = dr8("ejercicio") + ", Estatus " + dr8("estado").ToString.ToLower + ", Id " + dr8("id").ToString + ", Tipo " + dr8("normalComplementaria").ToString.ToLower
                            If dr8("estado").ToString <> "VACIA" Then
                                ejercicio.Text = ejercicio.Text + ", Contrato " + dr8("idContrato").ToString
                                If dr8("estado").ToString = "ACEPTADA" Then
                                    ejercicio.Text = ejercicio.Text + ", FechaEnvío " + dr8("fechaEnvio").ToString + ", " + bajarAcuse("a", dr("id").ToString, dr8("id").ToString, dr8("ejercicio").ToString, dr8("normalComplementaria"), "0") + ", AcuseDescargado " + IIf(dr8("acuseDescargado").Equals(True), "si", "no")
                                End If
                            End If
                            decls.ChildNodes.Add(ejercicio)

                            Dim dr6 As SqlDataReader
                            q2 = "SELECT ideM.id, ideM.mes, ideM.estado,ideM.normalComplementaria, ideM.idContrato, ideM.fechaEnvio, ideM.acuseDescargado FROM ideMens ideM, ideAnual ideAn WHERE ideAn.id=" + dr8("id").ToString + " AND ideM.idAnual=ideAn.id AND ideM.estado<>'VACIA' order by cast(ideM.mes as int),ideM.id"
                            myCommand2 = New SqlCommand(q2, myConnection)
                            dr6 = myCommand2.ExecuteReader()
                            While dr6.Read()
                                Dim mes As New TreeNode()
                                mes.Text = "Mes " + dr6("mes") + ", Estatus " + dr6("estado").ToString.ToLower + ", Id " + dr6("id").ToString + ", Tipo " + dr6("normalComplementaria").ToString.ToLower + ", Contrato " + dr6("idContrato").ToString
                                If dr6("estado").ToString = "ACEPTADA" Then
                                    mes.Text = mes.Text + ", FechaEnvío " + dr6("fechaEnvio").ToString + ", " + bajarAcuse("m", dr("id").ToString, dr6("id").ToString, dr8("ejercicio").ToString, dr6("normalComplementaria"), dr6("mes").ToString) + ", AcuseDescargado " + IIf(dr6("acuseDescargado").Equals(True), "si", "no")
                                End If
                                ejercicio.ChildNodes.Add(mes)
                            End While
                            dr6.Close()
                        End While
                    End If
                    dr8.Close()
etqContratos:
                    Dim contras As New TreeNode()
                    contras.Text = "Contratos"
                    padre.ChildNodes.Add(contras)

                    Dim dr7 As SqlDataReader
                    q2 = "SELECT co.id,co.fechaPago,co.periodoInicial,pla.elplan,co.fechaFinal,co.nDeclHechas,co.nDeclContratadas,co.esRegularizacion, co.precioNetoContrato FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.id=" + dr("id").ToString + " ORDER BY co.periodoInicial"
                    myCommand2 = New SqlCommand(q2, myConnection)
                    dr7 = myCommand2.ExecuteReader()
                    If dr7.HasRows Then
                        While dr7.Read()
                            Dim contra As New TreeNode()
                            contra.Text = "id " + dr7("id").ToString + ", $" + FormatNumber(dr7("precioNetoContrato"), 2) + ", FechaPago " + Left(dr7("fechaPago").ToString, 10) + ", PeriodoInicial " + Left(dr7("periodoInicial").ToString.ToString, 10) + ", Plan " + dr7("elplan") + ", Fecha final " + Left(dr7("fechaFinal").ToString, 10) + ", Declaraciones " + dr7("nDeclHechas").ToString + "/" + dr7("nDeclContratadas").ToString
                            contras.ChildNodes.Add(contra)
                        End While
                    End If
                    dr7.Close()
                End If
            End If
        End While
        dr.Close()
        dr3.Close()
        dr2.Close()
        dr4.Close()
    End Sub

    Protected Sub TreeView1_SelectedNodeChanged(sender As Object, e As EventArgs) Handles TreeView1.SelectedNodeChanged
        Dim pos, tam, idCliSelected
        pos = TreeView1.SelectedNode.Text.IndexOf(",")
        tam = pos
        idCliSelected = TreeView1.SelectedNode.Text.Substring(0, tam)
        Response.Redirect("Login.aspx?id=" + idCliSelected)
    End Sub

    Private Sub GridView3_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView3.RowCommand

    End Sub

    Protected Sub GridView3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView3.SelectedIndexChanged
        Dim row As GridViewRow = GridView3.SelectedRow
        idCli.Text = row.Cells(1).Text
        correo.Text = row.Cells(2).Text
        nomCli.Text = row.Cells(3).Text

        Dim q = "SELECT id from estatusCliente WHERE estatus='" + row.Cells(11).Text + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        edoCli.SelectedValue = dr("id")
        dr.Close()

    End Sub

    Protected Sub modEstatus_Click(sender As Object, e As EventArgs) Handles modEstatus.Click
        Dim q = "update clientes set idEstatus=" + edoCli.SelectedValue.ToString + " where id=" + idCli.Text
        myCommand = New SqlCommand(q, myConnection)
        myCommand.ExecuteNonQuery()
        GridView3.DataBind()
        Dim MSG = "<script language='javascript'>alert('Modificación exitosa');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
    End Sub

    Private Sub download_Click(sender As Object, e As EventArgs) Handles download.Click
        Dim q = "SELECT correo FROM clientes ORDER BY correo"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        correo.Text = ""
        While dr.Read()
            correo.Text = correo.Text + dr("correo") + ", "
        End While
        dr.Close()
        Dim MSG = "<script language='javascript'>alert('Se envio al texto de correo');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
    End Sub

    Private Sub exportarexcel_Click(sender As Object, e As EventArgs) Handles exportarexcel.Click
        If GridView3.Rows.Count < 1 Then
            Dim MSG As String = "<script language='javascript'>alert('Nada que exportar');</script>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
            Exit Sub
        End If

        Dim arch = "C:\SAT\clientesIDE.xlsx"

        If File.Exists(arch) Then
            File.Delete(arch)
        End If

        Try

            Dim oExcel As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application
            Dim oBook As Microsoft.Office.Interop.Excel.Workbook = oExcel.Workbooks.Add
            Dim oSheet As Microsoft.Office.Interop.Excel.Worksheet = oBook.Sheets(1)

            oSheet.Cells(2, 1).value = "ID"
            oSheet.Cells(2, 2).value = "Correo"
            oSheet.Cells(2, 3).value = "Razon social"
            oSheet.Cells(2, 4).value = "RFC"
            oSheet.Cells(2, 5).value = "Casfim"
            oSheet.Cells(2, 6).value = "login"
            oSheet.Cells(2, 7).value = "tel"
            oSheet.Cells(2, 8).value = "contacto"
            oSheet.Cells(2, 9).value = "estatus"
            oSheet.Cells(2, 10).value = "cel"
            oSheet.Cells(2, 11).value = "fechaSocket"
            oSheet.Cells(2, 12).value = "fechaReg"
            oSheet.Cells(2, 13).value = "fuente"
            oSheet.Cells(2, 14).value = "dxFac"
            oSheet.Cells(2, 15).value = "otrosCorreos"

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
            oSheet.Cells(2, 11).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 11).Font.Bold = True ' Fuente en negrita
            oSheet.Cells(2, 12).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 12).Font.Bold = True ' Fuente en negrita
            oSheet.Cells(2, 13).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 13).Font.Bold = True ' Fuente en negrita
            oSheet.Cells(2, 14).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 14).Font.Bold = True ' Fuente en negrita
            oSheet.Cells(2, 15).Font.Size = 12  ' tamaño de letra
            oSheet.Cells(2, 15).Font.Bold = True ' Fuente en negrita

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
            oSheet.Columns("K:K").EntireColumn.AutoFit()
            oSheet.Columns("L:L").EntireColumn.AutoFit()
            oSheet.Columns("M:M").EntireColumn.AutoFit()
            oSheet.Columns("N:N").EntireColumn.AutoFit()
            oSheet.Columns("O:O").EntireColumn.AutoFit()

            oSheet.Range("K:K").NumberFormat = "dd/MM/yyyy" ' "###,###,###,##0.000000" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha
            oSheet.Range("L:L").NumberFormat = "dd/MM/yyyy" ' "###,###,###,##0.000000" 'moneda; "@" p texto; "dd/MM/yyyy" p fecha

            Dim ren = 3
            For Each row As GridViewRow In GridView3.Rows
                oSheet.Cells(ren, 1).value = row.Cells(1).Text
                oSheet.Cells(ren, 2).value = Server.HtmlDecode(row.Cells(2).Text)
                oSheet.Cells(ren, 3).value = Server.HtmlDecode(row.Cells(3).Text)
                oSheet.Cells(ren, 4).value = row.Cells(4).Text
                oSheet.Cells(ren, 5).value = row.Cells(5).Text
                If row.Cells(6).Text <> "&nbsp;" And row.Cells(6).Text <> "NULL" Then
                    oSheet.Cells(ren, 6).value = Server.HtmlDecode(row.Cells(6).Text)
                End If
                oSheet.Cells(ren, 7).value = Server.HtmlDecode(row.Cells(7).Text)
                oSheet.Cells(ren, 8).value = Server.HtmlDecode(row.Cells(8).Text)
                oSheet.Cells(ren, 9).value = row.Cells(9).Text
                If row.Cells(10).Text <> "&nbsp;" And row.Cells(10).Text <> "NULL" Then
                    oSheet.Cells(ren, 10).value = Server.HtmlDecode(row.Cells(10).Text)
                End If
                If row.Cells(11).Text <> "&nbsp;" And row.Cells(11).Text <> "NULL" Then
                    oSheet.Cells(ren, 11).value = Left(Server.HtmlDecode(row.Cells(11).Text), 10)
                End If
                oSheet.Cells(ren, 12).value = Left(Server.HtmlDecode(row.Cells(12).Text), 10)
                If row.Cells(13).Text <> "&nbsp;" And row.Cells(13).Text <> "NULL" Then
                    oSheet.Cells(ren, 13).value = Server.HtmlDecode(row.Cells(13).Text)
                End If
                If row.Cells(14).Text <> "&nbsp;" And row.Cells(14).Text <> "NULL" Then
                    oSheet.Cells(ren, 14).value = Server.HtmlDecode(row.Cells(14).Text)
                End If
                If row.Cells(15).Text <> "&nbsp;" And row.Cells(15).Text <> "NULL" Then
                    oSheet.Cells(ren, 15).value = Server.HtmlDecode(row.Cells(15).Text)
                End If
                If row.Cells(16).Text <> "&nbsp;" And row.Cells(16).Text <> "NULL" Then
                    oSheet.Cells(ren, 16).value = Server.HtmlDecode(row.Cells(16).Text)
                End If
                If row.Cells(17).Text <> "&nbsp;" And row.Cells(17).Text <> "NULL" Then
                    oSheet.Cells(ren, 17).value = Server.HtmlDecode(row.Cells(17).Text)
                End If
                If row.Cells(18).Text <> "&nbsp;" And row.Cells(18).Text <> "NULL" Then
                    oSheet.Cells(ren, 18).value = Server.HtmlDecode(row.Cells(18).Text)
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
            Response.AddHeader("content-disposition", "attachment; filename=" + Path.GetFileName("C:\SAT\clientesIDE.xlsx"))
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

    Private Sub delCta_Click(sender As Object, e As EventArgs) Handles delCta.Click
        myCommand = New SqlCommand("DELETE From ideMens Where idAnual IN (SELECT ID FROM IDEANUAL WHERE idCliente=" + idCli.Text + ")", myConnection)
        myCommand.ExecuteNonQuery()
        myCommand = New SqlCommand("DELETE From ideAnual Where idCliente=" + idCli.Text, myConnection)
        myCommand.ExecuteNonQuery()
        myCommand = New SqlCommand("DELETE From reprLegal Where idCliente=" + idCli.Text, myConnection)
        myCommand.ExecuteNonQuery()
        myCommand = New SqlCommand("DELETE From contratos Where idCliente=" + idCli.Text, myConnection)
        myCommand.ExecuteNonQuery()
        myCommand = New SqlCommand("DELETE From clientes Where id=" + idCli.Text, myConnection)
        myCommand.ExecuteNonQuery()

        GridView3.DataBind()
        GridView3.SelectedIndex = -1
        idCli.Text = ""
        Dim MSG = "<script language='javascript'>alert('ok');</script>"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clientScript", MSG, False)
    End Sub

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(14).Text = "baja obligacion" Or e.Row.Cells(14).Text = "no localizable o no contestan" Or e.Row.Cells(14).Text = "ahora lo hacen ellos" Or e.Row.Cells(14).Text = "ya no llevan ese cliente" Then 'estatus
                e.Row.BackColor = System.Drawing.Color.Silver
            End If
            If e.Row.Cells(21).Text = "True" Then 'estatus
                e.Row.Cells(21).Text = "si"
            Else
                e.Row.Cells(21).Text = "no"
            End If
        End If
    End Sub

    Private Sub GridView3_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView3.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(18).Text = "True" Then 'facTercero
                e.Row.Cells(18).Text = "si"
            Else
                e.Row.Cells(18).Text = "no"
            End If
            'If e.Row.Cells(6).Text = "True" Then ' casfimProvisional
            '    e.Row.Cells(6).Text = "si"
            'Else
            '    e.Row.Cells(6).Text = "no"
            'End If
        End If
    End Sub
End Class