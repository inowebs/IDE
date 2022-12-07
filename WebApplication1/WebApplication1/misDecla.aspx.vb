Imports System.Data
Imports System.Data.SqlClient

Public Class WebForm17
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim myCommand2 As SqlCommand
    Dim dr As SqlDataReader
    Dim dr2 As SqlDataReader

    Private Sub controlaAcceso()
        Dim idcli
        Dim q
        q = "SELECT id, solSocketEstatus, loginSAT FROM clientes WHERE correo='" + Session("curCorreo") + "'"
        myCommand = New SqlCommand(q)
        Using dr = ExecuteReaderFunction(myCommand)
            dr.Read()

            idcli = dr("id")

            If dr("solSocketEstatus") = "VACIA" Then
                Response.Write("<script language='javascript'>alert('Es necesario que vaya a su cuenta y suba el archivo de autorización de socket');</script>")
                Response.Write("<script>location.href='cliente.aspx';</script>")
            ElseIf dr("solSocketEstatus") <> "APROBADA" Then
                '    Response.Write("<script language='javascript'>alert('Estamos en espera de que el SAT nos asigne su matriz de conexión segura y su socket, para poder acceder a esta sección');</script>")
                '    Response.Write("<script>location.href='cliente.aspx';</script>")
            End If
        End Using


        'q = "SELECT co.id, pla.elplan FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' AND co.fechaPago IS NOT NULL"
        'myCommand = New SqlCommand(q, myConnection)
        'dr = myCommand.ExecuteReader()
        'dr.Read()
        'If (Not dr.HasRows) Then 'sin contratos pagados 
        '    dr.Close()
        '    Response.Write("<script language='javascript'>alert('No hay contratos pagados');</script>")
        '    Response.Write("<script>location.href='misContra.aspx';</script>")
        'End If
        'dr.Close()

        q = "SELECT rl.id FROM reprLegal rl, clientes cli WHERE cli.id=" + idcli.ToString + " AND cli.id=rl.idCliente AND rl.esActual=1"
        myCommand = New SqlCommand(q)
        Dim v = ExecuteScalarFunction(myCommand)
        If IsNothing(v) Then
            Response.Write("<script language='javascript'>alert('Requiere especificar un representante legal actual');</script>")
            Response.Write("<script>location.href='cliente.aspx';</script>")
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(Session("curCorreo")) = True Then
            Response.Write("<script language='javascript'>alert('Expiró su sesión');</script>")
            Session.Abandon()
            Response.Write("<script language='javascript'>location.href='Login.aspx';</script>")
            Exit Sub
        End If

        myCommand = New SqlCommand("set dateformat ymd")
        ExecuteNonQueryFunction(myCommand)



        controlaAcceso()

        If Not IsPostBack Then  '1a vez
            Dim q, q2, dr2
            q = "SELECT ideAn.id,ideAn.ejercicio, ideAn.estado,ideAn.normalComplementaria, ideAn.idContrato FROM ideAnual ideAn, clientes cli WHERE ideAn.idCliente=cli.id AND cli.correo='" + Session("curCorreo") + "' and ideAn.ejercicio<'2022' order by cast(ejercicio as int) DESC,id DESC"
            myCommand = New SqlCommand(q)
            Using dr = ExecuteReaderFunction(myCommand)
                While dr.Read()
                    Dim ejercicio As New TreeNode()
                    ejercicio.Text = dr("ejercicio") + ", Estatus " + dr("estado").ToString.ToLower + ", Id " + dr("id").ToString + ", Tipo " + dr("normalComplementaria").ToString.ToLower
                    If dr("estado").ToString <> "VACIA" Then
                        ejercicio.Text = ejercicio.Text + ", Contrato " + dr("idContrato").ToString
                    End If
                    TreeView1.Nodes.Add(ejercicio)

                    q2 = "SELECT ideM.id, ideM.mes, ideM.estado,ideM.normalComplementaria, ideM.idContrato FROM ideMens ideM, ideAnual ideAn WHERE ideAn.id=" + dr("id").ToString + " AND ideM.idAnual=ideAn.id AND ideM.estado<>'VACIA' order by cast(ideM.mes as int),ideM.id"
                    myCommand2 = New SqlCommand(q2)
                    Using dr3 = ExecuteReaderFunction(myCommand2)
                        While dr3.Read()
                            Dim mes As New TreeNode()
                            mes.Text = "Mes " + dr3("mes") + ", Estatus " + dr3("estado").ToString.ToLower + ", Id " + dr3("id").ToString + ", Tipo " + dr3("normalComplementaria").ToString.ToLower + ", Contrato " + dr3("idContrato").ToString
                            ejercicio.ChildNodes.Add(mes)
                        End While
                    End Using
                End While
            End Using

            Dim v = returnID("estatusDecla2", "pendiente recibir archivos").ToString
            'mensuales 2022
            'Dim dr2 As SqlDataReader
            q2 = "SELECT id, mes, ejercicio, idEstatusDecla,idContrato FROM ideMens2 WHERE idEstatusDecla<>" + v + " and idCliente IN (SELECT id FROM clientes WHERE correo='" + Session("curCorreo") + "') order by ejercicio,cast(mes as int)"
            myCommand2 = New SqlCommand(q2)
            Using dr3 = ExecuteReaderFunction(myCommand2)
                While dr3.Read()
                    Dim mes As New TreeNode()
                    mes.Text = "Ejercicio " + dr3("ejercicio") + ", Mes " + dr3("mes") + ", Estatus " + getEstatusDeclaNomById(dr3("idEstatusDecla")) + ", Id " + dr3("id").ToString + ", Contrato " + dr3("idContrato").ToString
                    TreeView2.Nodes.Add(mes)
                End While
            End Using
        End If
    End Sub

    Private Function returnID(ByVal tabla As String, ByVal param1 As String, Optional ByVal param2 As Long = 0, Optional ByVal param3 As String = "") As Long
        Dim consulta
        If tabla = "estatusDecla2" Then
            consulta = "SELECT id FROM estatusDecla2 WHERE estatus='" + param1 + "'"
        ElseIf tabla = "contrib2" Then
            consulta = "SELECT id FROM contrib2 WHERE rfc='" + param1 + "'"
        ElseIf tabla = "entFed2" Then
            consulta = "SELECT id FROM entFed2 WHERE descr='" + param1 + "'"
        ElseIf tabla = "ideMens2" Then
            consulta = "SELECT TOP 1 id FROM idMens2 WHERE ejercicio='" + param1 + "' and mes=" + param2.ToString + " order by id desc"
        End If

        Dim retorno = -1
        Dim myCommandE = New SqlCommand(consulta)
        Using drE = ExecuteReaderFunction(myCommandE)
            If drE.HasRows Then
                drE.Read()
                retorno = drE("id")
            Else
                retorno = -1
            End If
        End Using
        Return retorno
    End Function

    Private Function getEstatusDeclaNomById(ByVal param1) As String
        Dim consulta = "SELECT estatus FROM estatusDecla2 WHERE id=" + param1.ToString + ""

        Dim retorno = ""
        Dim myCommandE = New SqlCommand(consulta)
        Using drE = ExecuteReaderFunction(myCommandE)
            If drE.HasRows Then
                drE.Read()
                retorno = drE("estatus")
            Else
                retorno = ""
            End If
        End Using
        Return retorno
    End Function

    Private Sub WebForm17_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload

    End Sub

    Protected Sub TreeView1_SelectedNodeChanged(sender As Object, e As EventArgs) Handles TreeView1.SelectedNodeChanged
        Dim pos, pos2, contrato, ejercicio, mes, ndecla, tam

        pos = TreeView1.SelectedNode.Text.IndexOf("Mes")
        If pos <> -1 Then 'mensual
            pos2 = TreeView1.SelectedNode.Text.IndexOf(",", pos)
            tam = pos2 - pos - 4
            mes = TreeView1.SelectedNode.Text.Substring(pos + 4, tam)
            Session("misTipo") = "Mensual"
            Session("misMes") = mes
            ejercicio = Left(TreeView1.SelectedNode.Parent.Text, 4)

            pos = TreeView1.SelectedNode.Parent.Text.IndexOf("Id")
            pos2 = TreeView1.SelectedNode.Parent.Text.IndexOf(",", pos)
            tam = pos2 - pos - 3
            Session("misIdAnual") = TreeView1.SelectedNode.Parent.Text.Substring(pos + 3, tam)
        Else ' anual
            Session("misTipo") = "Anual"
            ejercicio = Left(TreeView1.SelectedNode.Text, 4)

            pos = TreeView1.SelectedNode.Text.IndexOf("Id")
            pos2 = TreeView1.SelectedNode.Text.IndexOf(",", pos)
            tam = pos2 - pos - 3
            Session("misIdAnual") = TreeView1.SelectedNode.Text.Substring(pos + 3, tam)
        End If

        pos = TreeView1.SelectedNode.Text.IndexOf("Id")
        pos2 = TreeView1.SelectedNode.Text.IndexOf(",", pos)
        tam = pos2 - pos - 3
        ndecla = TreeView1.SelectedNode.Text.Substring(pos + 3, tam)

        pos = TreeView1.SelectedNode.Text.IndexOf("Contrato")
        If pos <> -1 Then
            pos2 = Len(TreeView1.SelectedNode.Text)
            tam = pos2 - pos - 9
            contrato = TreeView1.SelectedNode.Text.Substring(pos + 9, tam)
        Else
            contrato = ""
        End If

        Session("misEjercicio") = ejercicio
        Session("misNdecla") = ndecla
        Session("misContrato") = contrato

        Response.Write("<script>location.href='decla.aspx?m=1';</script>")

    End Sub

    Protected Sub NavigationMenu_MenuItemClick(sender As Object, e As System.Web.UI.WebControls.MenuEventArgs) Handles NavigationMenu.MenuItemClick
        If NavigationMenu.SelectedItem.Text = "Declarar" Then
            Session("misEjercicio") = ""
            Session("misNdecla") = ""
            Session("misContrato") = ""
            Session("misMes") = ""
            Session("misTipo") = ""
        End If
    End Sub

    Private Sub TreeView2_SelectedNodeChanged(sender As Object, e As EventArgs) Handles TreeView2.SelectedNodeChanged
        Dim pos, pos2, contrato, ejercicio, mes, ndecla, tam

        ejercicio = TreeView2.SelectedNode.Text.IndexOf(" ")
        Session("misEjercicio") = TreeView2.SelectedNode.Text.Substring(ejercicio + 1, 4)
        pos = TreeView2.SelectedNode.Text.IndexOf("Mes")
        pos2 = TreeView2.SelectedNode.Text.IndexOf(",", pos)
        tam = pos2 - pos - 4
        Session("misMes") = TreeView2.SelectedNode.Text.Substring(pos + 4, tam)
        Session("misTipo") = "Mensual"
        pos = TreeView2.SelectedNode.Text.IndexOf("Estatus")
        pos2 = TreeView2.SelectedNode.Text.IndexOf(",", pos)
        tam = pos2 - pos - 8
        Session("misEstatus") = TreeView2.SelectedNode.Text.Substring(pos + 8, tam)
        pos = TreeView2.SelectedNode.Text.IndexOf("Id")
        pos2 = TreeView2.SelectedNode.Text.IndexOf(",", pos)
        tam = pos2 - pos - 3
        Session("misNdecla") = TreeView2.SelectedNode.Text.Substring(pos + 3, tam)
        pos = TreeView2.SelectedNode.Text.IndexOf("Contrato")
        If pos <> -1 Then
            pos2 = Len(TreeView2.SelectedNode.Text)
            tam = pos2 - pos - 9
            Session("misContrato") = TreeView2.SelectedNode.Text.Substring(pos + 9, tam)
        Else
            Session("misContrato") = ""
        End If
        Response.Write("<script>location.href='decla.aspx?m=1';</script>")
    End Sub
End Class