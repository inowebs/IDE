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
    cnn.Open "Provider=SQLOLEDB;data Source=tcp:.;Initial Catalog=ide;", "usuario", "SmN+v-XzFy2N;91E170o"
    'cnn.Open "Provider=SQLOLEDB;data Source=tcp:.;Initial Catalog=ide;User Id=usuario;Password='SmN+v-XzFy2N;91E170o'"
    Set rst = New ADODB.Recordset
    rst.Open "Select cl.*, rl.nombreCompleto, rl.rfc as rfcRL from clientes cl, reprLegal rl WHERE rl.idCliente=cl.id AND casfim='" + Data(0) + "'", cnn, adOpenStatic, adLockReadOnly

    If Dir(App.Path & "\autorizacion tramite socket copia.doc") <> "" Then
        Kill App.Path & "\autorizacion tramite socket copia.doc"
    End If

    Dim WDoc As Word.Document
    Dim AppWord As New Word.Application
    Set WDoc = AppWord.Documents.Open(FileName:=App.Path & "\autorizacion tramite socket.dotx")
    AppWord.Visible = False
    WDoc.FormFields("fechaSol").Result = Format(Now(), "dd/mm/yyyy")
    WDoc.FormFields("represLegalSol").Result = rst!nombreCompleto
    WDoc.FormFields("razonSocialSol").Result = rst!razonSoc
    WDoc.FormFields("domicilioSol").Result = rst!domFiscal
    WDoc.FormFields("telSol").Result = rst!tel
    WDoc.FormFields("rfcRL").Result = rst!rfcRL
    WDoc.FormFields("rfcInstit").Result = rst!rfcDeclarante
    
    WDoc.SaveAs App.Path & "\autorizacion tramite socket copia.doc", wdFormatDocument
    WDoc.Close False 'cierra plantilla
    AppWord.Quit
   
   rst.Close
   cnn.Close
   End
End Sub
