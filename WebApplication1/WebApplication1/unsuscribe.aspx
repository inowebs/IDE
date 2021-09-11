<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="unsuscribe.aspx.vb" Inherits="WebApplication1.WebForm22" MasterPageFile="~/Site.Master"%>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link rel="canonical" href="https://www.declaracioneside.com/unsuscribe.aspx" />
    <style type="text/css">
    .style4
    {
        color: #800000;
        font-size: medium;
    }
</style>

<script type="text/javascript">

    var _gaq = _gaq || [];
    _gaq.push(['_setAccount', 'UA-33257770-1']);
    _gaq.push(['_trackPageview']);

    (function () {
        var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
        ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
    })();

</script>

 </asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">   

    <table border="0">
        <tr>
            <td align="right">Ingresa el correo a dar de baja de la lista de comunicados de Declaraciones IDE:</td>
            <td align="right"><asp:TextBox runat="server" ID="mail" Columns="30" MaxLength="150" 
                    Width="237px"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right"></td>
            <td colspan="2" align="right">
                <asp:Button runat="server" ID="baja" Text="Darme de Baja"/>
            </td>
        </tr>
    </table>
 </asp:Content>