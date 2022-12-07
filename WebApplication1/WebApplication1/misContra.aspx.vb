Imports System.Data
Imports System.Data.SqlClient

Public Class WebForm10
    Inherits System.Web.UI.Page
    Dim myConnection As SqlConnection
    Dim myCommand As SqlCommand
    Dim dr As SqlDataReader

    Private Sub controlaAcceso()
        'Dim q
        'q = "SELECT id, solSocketEstatus, loginSAT,inscripcionPagada FROM clientes WHERE correo='" + session("curCorreo") + "'"
        'myCommand = New SqlCommand(q, myConnection)
        'dr = myCommand.ExecuteReader()
        'dr.Read()
        ''If dr("inscripcionPagada").Equals(False) Then
        ''    dr.Close()
        ''    Response.Write("<script language='javascript'>alert('Es necesario que cubra el pago de su inscripción');</script>")
        ''    Response.Write("<script>location.href='cliente.aspx';</script>")
        ''    Exit Sub
        ''End If

        'If dr("solSocketEstatus") = "VACIA" Then
        '    dr.Close()
        '    Response.Write("<script language='javascript'>alert('Es necesario que vaya a su cuenta y suba el archivo de autorización de socket');</script>")
        '    Response.Write("<script>location.href='cliente.aspx';</script>")
        '    'ElseIf dr("solSocketEstatus") <> "APROBADA" Then
        '    '    dr.Close()
        '    '    Response.Write("<script language='javascript'>alert('Estamos en espera de que el SAT nos asigne su matriz de conexión segura y su socket, para poder acceder a esta sección');</script>")
        '    '    Response.Write("<script>location.href='cliente.aspx';</script>")
        'End If
        'dr.Close()
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

        'SqlDataSource3.ConnectionString = "$ ConnectionStrings:ideConnectionString "
        SqlDataSource3.SelectCommand = "SELECT co.id,co.precionetocontrato,co.fechaPago,co.periodoInicial,pla.elplan,co.fechaFinal,co.nDeclHechas,co.nDeclContratadas,co.uuid FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id AND cli.correo='" + Session("curCorreo") + "' ORDER BY co.id DESC"
        GridView3.DataBind()
        nRegs.Text = FormatNumber(GridView3.Rows.Count.ToString, 0) + " Registros"
    End Sub

    Protected Sub GridView3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView3.SelectedIndexChanged
        Dim row As GridViewRow = GridView3.SelectedRow
        session("GidContrato") = row.Cells(1).Text
        Response.Redirect("~/contrato.aspx")
    End Sub

    Protected Sub nuevo_Click(sender As Object, e As EventArgs) Handles nuevo.Click
        session("GidContrato") = Nothing
        Response.Redirect("~/contrato.aspx")
    End Sub

    Private Sub WebForm10_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload

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