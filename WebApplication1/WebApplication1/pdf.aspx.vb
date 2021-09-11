Public Class WebForm31
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim casfim = "00000", arch = "acuseMensual.doc", ruta = "C:\SAT\00000"
        'WORD TO PDF
        Dim newApp As Microsoft.Office.Interop.Word.Application = New Microsoft.Office.Interop.Word.Application
        'Dim newApp As New Word.Application()
        Dim Source As Object = "C:\SAT\" + casfim + "\acuseMensual.doc"
        Dim Target As Object = ruta + "\" + arch + ".pdf"
        Dim Unknown As Object = Type.Missing
        descrip.Text = "1=" + Source + " 2=" + Target
        newApp.Documents.Open(Source, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown)
        Dim format As Object = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF
        newApp.ActiveDocument.SaveAs(Target, format, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown)
        newApp.Quit(Unknown, Unknown, Unknown)
        descrip.Text = "ok"
    End Sub
End Class