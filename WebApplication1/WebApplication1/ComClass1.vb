<ComClass(ComClass1.ClassId, ComClass1.InterfaceId, ComClass1.EventsId)> _
Public Class ComClass1

#Region "GUID de COM"
    ' Estos GUID proporcionan la identidad de COM para esta clase 
    ' y las interfaces de COM. Si las cambia, los clientes 
    ' existentes no podrán obtener acceso a la clase.
    Public Const ClassId As String = "e3679c3a-ef7e-4394-97d9-bee6c019ec30"
    Public Const InterfaceId As String = "bdf3d339-5054-461a-9586-57acc8189bb5"
    Public Const EventsId As String = "7e4ef158-873e-4a57-bd0f-17dbb8b34ef2"
#End Region

    ' Una clase COM que se puede crear debe tener Public Sub New() 
    ' sin parámetros, si no la clase no se 
    ' registrará en el registro COM y no se podrá crear a 
    ' través de CreateObject.
    Public Sub New()
        MyBase.New()
    End Sub

End Class


