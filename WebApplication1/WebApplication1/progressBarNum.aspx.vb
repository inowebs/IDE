Public Class progressBarNum
    Inherits System.Web.UI.Page

    Protected Sub TimerControl1_Tick(sender As Object, e As EventArgs) Handles TimerControl1.Tick
        Label1.Text = FormatNumber(Session("barraIteracion").ToString, 0) + " de " + FormatNumber(Session("barraN").ToString, 0) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; " + FormatNumber(Session("barraIteracion") / Session("barraN") * 100, 4).ToString + " %"
        If Session("barraIteracion") = Session("barraN") Or Session("error") <> "" Then
            TimerControl1.Enabled = False

            If Session("error") <> "" Then
                lblErr.Text = Session("error") + ". Se procesaron exitosamente los primeros " + Session("barraIteracion").ToString + " registros"
            Else
                Label2.Text = "Ahora puede cerrar esta ventana, se procesaron exitosamente todas las operaciones"
            End If
            Session("eoBar") = 1
        End If

    End Sub

End Class