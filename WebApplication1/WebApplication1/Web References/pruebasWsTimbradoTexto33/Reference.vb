﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
'
Namespace pruebasWsTimbradoTexto33
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2558.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="wsTimbradoTexto33Soap", [Namespace]:="www.facturaselectronicascfdi.com")>  _
    Partial Public Class wsTimbradoTexto33
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private TimbrarPorTexto33OperationCompleted As System.Threading.SendOrPostCallback
        
        Private SellarTimbrarPorTexto33OperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.WebApplication1.My.MySettings.Default.WebApplication1_pruebasWsTimbradoTexto33_wsTimbradoTexto33
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event TimbrarPorTexto33Completed As TimbrarPorTexto33CompletedEventHandler
        
        '''<remarks/>
        Public Event SellarTimbrarPorTexto33Completed As SellarTimbrarPorTexto33CompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("www.facturaselectronicascfdi.com/TimbrarPorTexto33", RequestNamespace:="www.facturaselectronicascfdi.com", ResponseNamespace:="www.facturaselectronicascfdi.com", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function TimbrarPorTexto33(ByVal correo As String, ByVal pass As String, ByVal Proceso As String, ByVal contenidoArchivo As String) As resultado33
            Dim results() As Object = Me.Invoke("TimbrarPorTexto33", New Object() {correo, pass, Proceso, contenidoArchivo})
            Return CType(results(0),resultado33)
        End Function
        
        '''<remarks/>
        Public Overloads Sub TimbrarPorTexto33Async(ByVal correo As String, ByVal pass As String, ByVal Proceso As String, ByVal contenidoArchivo As String)
            Me.TimbrarPorTexto33Async(correo, pass, Proceso, contenidoArchivo, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub TimbrarPorTexto33Async(ByVal correo As String, ByVal pass As String, ByVal Proceso As String, ByVal contenidoArchivo As String, ByVal userState As Object)
            If (Me.TimbrarPorTexto33OperationCompleted Is Nothing) Then
                Me.TimbrarPorTexto33OperationCompleted = AddressOf Me.OnTimbrarPorTexto33OperationCompleted
            End If
            Me.InvokeAsync("TimbrarPorTexto33", New Object() {correo, pass, Proceso, contenidoArchivo}, Me.TimbrarPorTexto33OperationCompleted, userState)
        End Sub
        
        Private Sub OnTimbrarPorTexto33OperationCompleted(ByVal arg As Object)
            If (Not (Me.TimbrarPorTexto33CompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent TimbrarPorTexto33Completed(Me, New TimbrarPorTexto33CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("www.facturaselectronicascfdi.com/SellarTimbrarPorTexto33", RequestNamespace:="www.facturaselectronicascfdi.com", ResponseNamespace:="www.facturaselectronicascfdi.com", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function SellarTimbrarPorTexto33(ByVal correo As String, ByVal pass As String, ByVal Proceso As String, ByVal contenidoArchivo As String, <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")> ByVal cerFile() As Byte, <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")> ByVal keyFile() As Byte, ByVal passCer As String) As resultado33
            Dim results() As Object = Me.Invoke("SellarTimbrarPorTexto33", New Object() {correo, pass, Proceso, contenidoArchivo, cerFile, keyFile, passCer})
            Return CType(results(0),resultado33)
        End Function
        
        '''<remarks/>
        Public Overloads Sub SellarTimbrarPorTexto33Async(ByVal correo As String, ByVal pass As String, ByVal Proceso As String, ByVal contenidoArchivo As String, ByVal cerFile() As Byte, ByVal keyFile() As Byte, ByVal passCer As String)
            Me.SellarTimbrarPorTexto33Async(correo, pass, Proceso, contenidoArchivo, cerFile, keyFile, passCer, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub SellarTimbrarPorTexto33Async(ByVal correo As String, ByVal pass As String, ByVal Proceso As String, ByVal contenidoArchivo As String, ByVal cerFile() As Byte, ByVal keyFile() As Byte, ByVal passCer As String, ByVal userState As Object)
            If (Me.SellarTimbrarPorTexto33OperationCompleted Is Nothing) Then
                Me.SellarTimbrarPorTexto33OperationCompleted = AddressOf Me.OnSellarTimbrarPorTexto33OperationCompleted
            End If
            Me.InvokeAsync("SellarTimbrarPorTexto33", New Object() {correo, pass, Proceso, contenidoArchivo, cerFile, keyFile, passCer}, Me.SellarTimbrarPorTexto33OperationCompleted, userState)
        End Sub
        
        Private Sub OnSellarTimbrarPorTexto33OperationCompleted(ByVal arg As Object)
            If (Not (Me.SellarTimbrarPorTexto33CompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent SellarTimbrarPorTexto33Completed(Me, New SellarTimbrarPorTexto33CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2634.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="www.facturaselectronicascfdi.com")>  _
    Partial Public Class resultado33
        
        Private idComprobanteField As Integer
        
        Private selloField As String
        
        Private erroresField As String
        
        Private mensajeField As String
        
        Private xmlSelladoField() As Byte
        
        Private acuseField() As Byte
        
        Private arregloAcuseField()() As Byte
        
        Private cadenaOriginalField As String
        
        Private fechaHoraTimbradoField As Date
        
        Private fechaHoraTimbradoFieldSpecified As Boolean
        
        Private fechaHoraTimbradoSpecified1Field As Boolean
        
        Private folioUUIDField As String
        
        Private pDFField() As Byte
        
        Private selloDigitalEmisorField As String
        
        Private selloDigitalTimbreSATField As String
        
        Private xMLField() As Byte
        
        '''<remarks/>
        Public Property idComprobante() As Integer
            Get
                Return Me.idComprobanteField
            End Get
            Set
                Me.idComprobanteField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property sello() As String
            Get
                Return Me.selloField
            End Get
            Set
                Me.selloField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property errores() As String
            Get
                Return Me.erroresField
            End Get
            Set
                Me.erroresField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property mensaje() As String
            Get
                Return Me.mensajeField
            End Get
            Set
                Me.mensajeField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")>  _
        Public Property xmlSellado() As Byte()
            Get
                Return Me.xmlSelladoField
            End Get
            Set
                Me.xmlSelladoField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")>  _
        Public Property acuse() As Byte()
            Get
                Return Me.acuseField
            End Get
            Set
                Me.acuseField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property arregloAcuse() As Byte()()
            Get
                Return Me.arregloAcuseField
            End Get
            Set
                Me.arregloAcuseField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property cadenaOriginal() As String
            Get
                Return Me.cadenaOriginalField
            End Get
            Set
                Me.cadenaOriginalField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property fechaHoraTimbrado() As Date
            Get
                Return Me.fechaHoraTimbradoField
            End Get
            Set
                Me.fechaHoraTimbradoField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property fechaHoraTimbradoSpecified() As Boolean
            Get
                Return Me.fechaHoraTimbradoFieldSpecified
            End Get
            Set
                Me.fechaHoraTimbradoFieldSpecified = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("fechaHoraTimbradoSpecified")>  _
        Public Property fechaHoraTimbradoSpecified1() As Boolean
            Get
                Return Me.fechaHoraTimbradoSpecified1Field
            End Get
            Set
                Me.fechaHoraTimbradoSpecified1Field = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property folioUUID() As String
            Get
                Return Me.folioUUIDField
            End Get
            Set
                Me.folioUUIDField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")>  _
        Public Property PDF() As Byte()
            Get
                Return Me.pDFField
            End Get
            Set
                Me.pDFField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property selloDigitalEmisor() As String
            Get
                Return Me.selloDigitalEmisorField
            End Get
            Set
                Me.selloDigitalEmisorField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property selloDigitalTimbreSAT() As String
            Get
                Return Me.selloDigitalTimbreSATField
            End Get
            Set
                Me.selloDigitalTimbreSATField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")>  _
        Public Property XML() As Byte()
            Get
                Return Me.xMLField
            End Get
            Set
                Me.xMLField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2558.0")>  _
    Public Delegate Sub TimbrarPorTexto33CompletedEventHandler(ByVal sender As Object, ByVal e As TimbrarPorTexto33CompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2558.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class TimbrarPorTexto33CompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As resultado33
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),resultado33)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2558.0")>  _
    Public Delegate Sub SellarTimbrarPorTexto33CompletedEventHandler(ByVal sender As Object, ByVal e As SellarTimbrarPorTexto33CompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2558.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class SellarTimbrarPorTexto33CompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As resultado33
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),resultado33)
            End Get
        End Property
    End Class
End Namespace