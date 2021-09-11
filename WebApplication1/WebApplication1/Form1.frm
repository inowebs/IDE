VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   2685
   ClientLeft      =   60
   ClientTop       =   375
   ClientWidth     =   5145
   LinkTopic       =   "Form1"
   ScaleHeight     =   2685
   ScaleWidth      =   5145
   StartUpPosition =   3  'Windows Default
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim WithEvents oApp As Word.Application
Attribute oApp.VB_VarHelpID = -1
Dim cn          As ADODB.Connection

Private Sub Form_Load()
    Dim Data() As String
    Data = Split(Command, " ") 'lee argumentos

    Dim rst As ADODB.Recordset
    Set cnn = New ADODB.Connection
    cnn.Open "Provider=SQLOLEDB;data Source=tcp:IDESERVER;Initial Catalog=ide;User Id=usuario;Password=USUARIO;"
    Set rst = New ADODB.Recordset
    rst.Open "Select * from clientes WHERE casfim='" + Data(0) + "'", cnn, adOpenStatic, adLockReadOnly

    Dim WDoc As Word.Document
    Dim AppWord As New Word.Application
    Set WDoc = AppWord.Documents.Open(FileName:="C:\inetpub\wwwroot\Solicitud de Matrices IDE formato.dotx")
    AppWord.Visible = False
    WDoc.FormFields("fechaSolSocketSat").Result = Format(rst!fechaSolSocketSat, "dd/mm/yyyy")
    WDoc.FormFields("razonSoc").Result = rst!razonSoc
    WDoc.FormFields("casfim").Result = rst!casfim
    WDoc.FormFields("rutaSAT").Result = "C:\SAT\" + rst!casfim
    WDoc.SaveAs "C:\inetpub\wwwroot\Solicitud de Matrices IDE formato copia.doc", wdFormatDocument
    WDoc.Close False 'cierra plantilla
    AppWord.Quit
   
   rst.Close
   cnn.Close
   End
End Sub
