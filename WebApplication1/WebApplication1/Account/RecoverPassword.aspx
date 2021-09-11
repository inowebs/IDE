<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RecoverPassword.aspx.vb" Inherits="WebApplication2.WebForm25" MasterPageFile="~/Site.Master" %>

<SCRIPT runat="server">

</SCRIPT>


<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" Runat="Server">

 </asp:Content>


<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">   
    <div><strong>¿Olvidó su password?</strong></div>
    <table border="0">
        <tr>
            <td align="right">Ingrese su correo:</td>
            <td align="right"><asp:TextBox runat="server" ID="UsersEmail" Columns="30" MaxLength="50" 
                    Width="237px"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right"></td>
            <td colspan="2" align="right">
                <asp:Button runat="server" ID="SendEmail" Text="Recuperar contraseña" OnClick="SendEmail_Click" />
            </td>
        </tr>
    </table>


 </asp:Content>