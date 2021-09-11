Imports System.Data
Imports System.Data.SqlClient

Public Class WebForm17
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim myCommand2 As SqlCommand
    Dim dr As SqlDataReader

    Private Sub controlaAcceso()
        Dim idcli
        Dim q
        q = "SELECT id, solSocketEstatus, loginSAT FROM clientes WHERE correo='" + Session("curCorreo") + "'"
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()

        idcli = dr("id")

        If dr("solSocketEstatus") = "VACIA" Then
            dr.Close()
            Response.Write("<script language='javascript'>alert('Es necesario que vaya a su cuenta y suba el archivo de autorización de socket');</script>")
            Response.Write("<script>location.href='cliente.aspx';</script>")
        ElseIf dr("solSocketEstatus") <> "APROBADA" Then
            dr.Close()
            '    Response.Write("<script language='javascript'>alert('Estamos en espera de que el SAT nos asigne su matriz de conexión segura y su socket, para poder acceder a esta sección');</script>")
            '    Response.Write("<script>location.href='cliente.aspx';</script>")
        End If        

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
        myCommand = New SqlCommand(q, myConnection)
        dr = myCommand.ExecuteReader()
        dr.Read()
        If Not dr.HasRows Then
            dr.Close()
            Response.Write("<script language='javascript'>alert('Requiere especificar un representante legal actual');</script>")
            Response.Write("<script>location.href='cliente.aspx';</script>")
        End If
        dr.Close()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNothing(Session("curCorreo")) = True Then
            Response.Write("<script language='javascript'>alert('Expiró su sesión');</script>")
            Session.Abandon()
            Response.Write("<script language='javascript'>location.href='Login.aspx';</script>")
            Exit Sub
        End If

        myConnection = New SqlConnection("server=tcp:.;database=ide;User ID=usuario;Password='SmN+v-XzFy2N;91E170o';MultipleActiveResultSets=true")
        myConnection.Open()
        myCommand = New SqlCommand("set dateformat ymd", myConnection)
        myCommand.ExecuteNonQuery()



        controlaAcceso()

        If Not IsPostBack Then  '1a vez
            Dim q
            q = "SELECT ideAn.id,ideAn.ejercicio, ideAn.estado,ideAn.normalComplementaria, ideAn.idContrato FROM ideAnual ideAn, clientes cli WHERE ideAn.idCliente=cli.id AND cli.correo='" + Session("curCorreo") + "' order by cast(ejercicio as int) DESC,id DESC"
            myCommand = New SqlCommand(q, myConnection)
            dr = myCommand.ExecuteReader()
            While dr.Read()
                Dim ejercicio As New TreeNode()
                ejercicio.Text = dr("ejercicio") + ", Estatus " + dr("estado").ToString.ToLower + ", Id " + dr("id").ToString + ", Tipo " + dr("normalComplementaria").ToString.ToLower
                If dr("estado").ToString <> "VACIA" Then
                    ejercicio.Text = ejercicio.Text + ", Contrato " + dr("idContrato").ToString
                End If
                TreeView1.Nodes.Add(ejercicio)

                Dim q2
                Dim dr2 As SqlDataReader
                q2 = "SELECT ideM.id, ideM.mes, ideM.estado,ideM.normalComplementaria, ideM.idContrato FROM ideMens ideM, ideAnual ideAn WHERE ideAn.id=" + dr("id").ToString + " AND ideM.idAnual=ideAn.id AND ideM.estado<>'VACIA' order by cast(ideM.mes as int),ideM.id"
                myCommand2 = New SqlCommand(q2, myConnection)
                dr2 = myCommand2.ExecuteReader()
                While dr2.Read()
                    Dim mes As New TreeNode()
                    mes.Text = "Mes " + dr2("mes") + ", Estatus " + dr2("estado").ToString.ToLower + ", Id " + dr2("id").ToString + ", Tipo " + dr2("normalComplementaria").ToString.ToLower + ", Contrato " + dr2("idContrato").ToString
                    ejercicio.ChildNodes.Add(mes)
                End While
                dr2.Close()
            End While
            dr.Close()

        End If
    End Sub

    Private Sub WebForm17_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        myConnection.Close()
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
End Class