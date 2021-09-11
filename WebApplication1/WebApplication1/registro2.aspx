<%@ Import Namespace="System.Data.OleDb" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="registro2.aspx.vb" Inherits="WebApplication1.WebForm2" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1Registro
        {
            width: 98%;
            height: 489px;
            margin-right: 0px;
        }
        .style2Registro
        {
            height: 26px;
        }
        .style3Registro
        {
        text-align: right;
        width: 303px;
    }
        .style4Registro
        {
            height: 26px;
            text-align: right;
        width: 303px;
    }
        .style5
        {
            font-size: 13pt;
            color: #800000;
        }
    .style14
    {
        font-size: small;
        font-family: Arial;
    }
    .style15
    {
        font-size: x-small;
        font-family: Arial;
    }
    .style16
    {
        font-size: small;
    }
        .style17
        {
            font-size: x-small;
        }
    .style18
    {
        text-align: right;
        visibility: hidden;
        width: 303px;
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


<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">   
    <div>   
        <strong><span class="style5">Registro de la institución financiera de la que 
        enviará declaraciones del IDE</span></strong><br />
        <br />
        <span style="color: rgb(34, 34, 34); font-family: verdana, arial, helvetica, sans-serif; font-size: 13px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: justify; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; background-color: rgb(255, 255, 255); display: inline !important; float: none; ">
        Así de rápido, podrás comenzar a <strong>enviar</strong> tú mismo las <strong>declaraciones 
        informativas de IDE</strong> al <strong>SAT</strong>&nbsp;
        <br />
        <br />
        </span><br />
        <table class="style1Registro">
            <tr class="style14">
                <td class="style3Registro">
                    &nbsp;</td>
                <td>
                    Para más detalles, coloque el mouse sobre recuadros no obvios</td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label1" runat="server" Text="Correo" CssClass="style14"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="correo" runat="server" Width="237px" MaxLength="50" CssClass="style14" 
                        AutoPostBack="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style4Registro">
        <asp:Label ID="Label2" runat="server" Text="Elija su contraseña de cliente" 
                        CssClass="style14"></asp:Label>
                </td>
                <td class="style2Registro">
        <asp:TextBox ID="passWeb" runat="server" Width="237px" MaxLength="15" TextMode="Password" 
                        CssClass="style14" ToolTip="seis caracteres o numeros mínimo"></asp:TextBox>
                &nbsp;</td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label17" runat="server" Text="Repita su Contraseña de cliente" 
                        CssClass="style14"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="passWeb2" runat="server" Width="237px" MaxLength="15" TextMode="Password" 
                        CssClass="style14" ToolTip="seis caracteres o numeros mínimo"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label3" runat="server" Text="Denominación / Razón Social" 
                        CssClass="style14"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="razonSoc" runat="server" Width="237px" MaxLength="250" 
                        
                        ToolTip="Razón social con la que está dado de alta en el SAT" 
                        CssClass="style14"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label16" runat="server" Text="Nombre de contacto" CssClass="style14"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="contacto" runat="server" Width="237px" MaxLength="100" CssClass="style14"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label4" runat="server" Text="Puesto" CssClass="style14"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="puesto" runat="server" Width="237px" MaxLength="50" CssClass="style14"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label5" runat="server" Text="Teléfono" CssClass="style14"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="tel" runat="server" Width="237px" MaxLength="40" CssClass="style14"></asp:TextBox>
                &nbsp;<span class="style14">incluir lada</span></td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label6" runat="server" Text="Celular" CssClass="style14"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="cel" runat="server" Width="237px" MaxLength="10" 
                        onkeypress="return numeros()" CssClass="style14"></asp:TextBox>        
                    <span class="style14">&nbsp;incluir lada</span></td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label7" runat="server" Text="PáginaWeb" CssClass="style14"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="paginaWeb" runat="server" Width="237px" MaxLength="200" CssClass="style14"></asp:TextBox>
                &nbsp;<span class="style15">(opcional)</span></td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label8" runat="server" Text="RFC empresa declarante" 
                        CssClass="style14"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="rfcDeclarante" runat="server" Width="237px" MaxLength="12" 
                        CssClass="style14"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label10" runat="server" Text="Domicilio fiscal completo" 
                        CssClass="style14"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="domFiscal" runat="server" Width="237px" MaxLength="100"></asp:TextBox>
                    &nbsp;<span class="style15">incluir colonia, C.P., localidad., edo.</span></td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label11" runat="server" Text="clave CASFIM &lt;br&gt; (clave de Institución Financiera)" 
                        style="font-weight: 700" CssClass="style14"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="casfim" runat="server" Width="237px" MaxLength="10" 
                        
                        ToolTip="La clave CASFIM de su institución, especifíquela correctamente, pues de lo contrario el SAT no podrá autorizarnos su socket para que pueda enviar declaraciones, si aún no lo tiene, introduzca provisionalmente cualquier clave numerica pero no olvide actualizarla por la oficial de su institución al ingresar en 'Mi cuenta'"></asp:TextBox>
                &nbsp;<asp:Label ID="Label19" runat="server" Height="30px" 
                        style="font-size: x-small; font-family: Arial;" 
                        Text="si aún no la tiene, puede indicar una clave numérica provisional al azar de 5 dígitos, y después poner la que le asigne el SAT" 
                        Width="322px"></asp:Label>
                    <br />
                    <asp:CheckBox ID="casfimProvisional" runat="server" 
                        Text="¿es clave Provisional?" style="font-size: small" />
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label13" runat="server" Text="¿Son un banco?" CssClass="style14"></asp:Label>
                </td>
                <td>
                    <span class="style16">
        <asp:CheckBox ID="esInstitCredito" runat="server" CssClass="style1master1" />
                    </span><span class="style1master1">
                    <span class="style17">si no son un banco déjelo en blanco</span></span></td>
            </tr>
            <tr>
                <td class="style3Registro">
                    <asp:Label ID="Label20" runat="server" style="font-size: small" 
                        Text="¿Como te enteraste de nosotros?"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="fuente" runat="server" Height="48px" MaxLength="120" 
                        TextMode="MultiLine" 
                        ToolTip="Sea lo mas específico posible, max. 120 caracteres" Width="237px"></asp:TextBox>
                </td>
            </tr>
            <tr style="visibility: hidden">
                <td class="style18" style="visibility: hidden">
        <asp:Label ID="Label14" runat="server" Text="# Sucursales" CssClass="style14"></asp:Label>
                </td>
                <td style="visibility: hidden">
        <asp:TextBox ID="numSucursales" runat="server" onblur="Javascript:ceros(this);formatoNumero(this,0,'.',',');" Width="237px" onkeypress="return numeros()">0</asp:TextBox>
                &nbsp;<span class="style15">(opcional)</span></td>
            </tr>
            <tr style="visibility: hidden">
                <td class="style18" style="visibility: hidden">
        <asp:Label ID="Label15" runat="server" Text="# Socios o Clientes" CssClass="style14"></asp:Label>
                </td>
                <td style="visibility: hidden">
        <asp:TextBox ID="numSociosClientes" runat="server" onblur="Javascript:ceros(this);formatoNumero(this,0,'.',',');" Width="237px" onkeypress="return numeros()">0</asp:TextBox>
                &nbsp;<span class="style15">(opcional)</span></td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label18" runat="server" Text="Número de Distribuidor" CssClass="style14"></asp:Label>
                </td>
                <td>
                <asp:TextBox ID="idDistribuidor" runat="server" AutoPostBack="True" Width="68px" 
                        
                        ToolTip="(opcional) Le es proporcionado por el personal que lo introdujo al sistema" 
                        onkeypress="return numeros()"></asp:TextBox>
                &nbsp;<span class="style15">(opcional)</span></td>
            </tr>
            <tr>
                <td class="style3Registro">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style3Registro">
                    &nbsp;</td>
                <td>
        <asp:Button ID="Button1" runat="server" Text="Registrarme" Height="26px" />
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style3Registro">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>    
    </div>

<script type="text/javascript" language="javascript">
function ceros(campo) {
    if (document.getElementById(campo.id).value == "") {
        document.getElementById(campo.id).value = "0";        
    }
}

function numeros() {
    var AsciiValue = event.keyCode
    if ((AsciiValue >= 48 && AsciiValue <= 57) || (AsciiValue == 8 || AsciiValue == 127))
        event.returnValue = true;
    else
        event.returnValue = false;
}

function numerosDec() {
    var AsciiValue = event.keyCode
    if ((AsciiValue >= 48 && AsciiValue <= 57) || (AsciiValue == 8 || AsciiValue == 127 || AsciiValue == 46))
        event.returnValue = true;
    else
        event.returnValue = false;
}

function formatoNumero(campo, decimales, separador_decimal, separador_miles) { // v2007-08-06
    numero = document.getElementById(campo.id).value;
    numero = numero.replace(/,/g, '');
    numero = parseFloat(numero);
    if (isNaN(numero)) {
        return "";
    }
    if (decimales !== undefined) {
        numero = numero.toFixed(decimales);
    }
    numero = numero.toString().replace(".", separador_decimal !== undefined ? separador_decimal : ",");
    if (separador_miles) {
        var miles = new RegExp("(-?[0-9]+)([0-9]{3})");
        while (miles.test(numero)) {
            numero = numero.replace(miles, "$1" + separador_miles + "$2");
        }
    }
    document.getElementById(campo.id).value = numero;
    return numero;
}
</script>
    
    <!-- Google Code for conversion Conversion Page -->
    <script type="text/javascript">
    /* <![CDATA[ */
    var google_conversion_id = 997292121;
    var google_conversion_language = "en";
    var google_conversion_format = "2";
    var google_conversion_color = "ffffff";
    var google_conversion_label = "K5s-CO-yxgQQ2fDF2wM";
    var google_conversion_value = 0;
    /* ]]> */
    </script>
    <script type="text/javascript" src="http://www.googleadservices.com/pagead/conversion.js">
    </script>
    <noscript>
    <div style="display:inline;">
    <img height="1" width="1" style="border-style:none;" alt="" src="http://www.googleadservices.com/pagead/conversion/997292121/?value=0&amp;label=K5s-CO-yxgQQ2fDF2wM&amp;guid=ON&amp;script=0"/>
    </div>
    </noscript>

</asp:Content>