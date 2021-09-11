<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="pagos.aspx.vb" Inherits="WebApplication1.WebForm19" MasterPageFile="~/Site.Master"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script runat="server">
    Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("hp") = "1" Then 'panel1 habilitado
            Panel1.Enabled = True
        Else
            Panel1.Enabled = False
        End If
    End Sub
    </script>

<style type="text/css">
    .style4
    {
        color: #800000;
        font-size: medium;
    }
    .style5
    {
        color: #000000;
    }
    .style6
    {
        text-align: center;
    }
    .style13
    {
        color: #996600;
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
    <span class="style4"><strong>Pagos


    con tarjeta de crédito, débito, paypal


    </strong></span>


    <br />
    <br />
                Puede pagar inmediatamente en línea con su 
    <span class="style5">tarjeta de crédito</span>(Visa 
    y MasterCard), <span class="style5">débito</span>(Visa y MasterCard emitidas por 
    Bancomer, Banamex, Santander y HSBC) o su cuenta 
                <span class="style5">paypal</span>, haciendo 
<span class="style13"><strong><em>clic en la imágen</span></em></strong></span> 
siguiente<br />
                <br />
                <asp:Panel ID="Panel1" runat="server">
                        <div class="style6">
                            <input name='shopping_url' type='hidden' />
                            <input name='return' type='hidden' value='~/contrato.aspx?id=<%=session("GidContrato") %>' />
                            <input name='cmd' type='hidden' value='_xclick' />
                            <input name='business' type='hidden' value='job001@hotmail.com' />
                            <input name='lc' type='hidden' value='MX' />
                            <input name='item_name' type='hidden' value='Declaraciones IDE, referencia o concepto C<%=Session("GidContrato")%>' />
                            <input name='amount' type='hidden' value='<%=session("GmontoContra") %>' />
                            <input name='currency_code' type='hidden' value='MXN' />
                            <input name='button_subtype' type='hidden' value='services' />
                            <input name='no_note' type='hidden' value='0' />
                            <input name='add' type='hidden' value='1' />
                            <input name='bn' type='hidden' value='PP-BuyNowBF:btn_paynowCC_LG.gif:NonHostedGuest' />
                            <asp:ImageButton ID="ImageButton1" runat="server" Height="159px" 
                                ImageUrl="pagos.jpg" 
                                OnClientClick="document.getElementById('form1').target = '_blank';" 
                                PostBackUrl="https://www.paypal.com/cgi-bin/webscr" 
                                ToolTip="Clic aquí para pagar" Width="194px" 
                                AlternateText="Clic aquí para pagar ahora" />
                            <span style="font-size: 11px; font-family: Arial, Verdana;">&nbsp; Un modo más seguro 
                            y sencillo de pagar</span>
                        </div>
                </asp:Panel>
                <br />
                O bien, puede optar por <span class="style5">depósito o transferencia bancaria</span>, cuyos detalles se le 
                enviaran por correo al momento de guardar su contrato, del cual una vez pagado envíenos un correo adjuntando el comprobante del deposito o de la transferencia para acreditarlo.
<br />
             <br />
    Por el momento no está disponible el procesamiento de pagos con tarjetas &#39;<span 
    class="style5">Visa 
    Electron</span>&#39;<br />
    <br />
    <asp:Button ID="Button1" runat="server" onclientclick="document.getElementById('form1').target = '_self';" 
        Text="Regresar" PostBackUrl="~/contrato.aspx" />
    <br />
    <br />


</asp:Content>