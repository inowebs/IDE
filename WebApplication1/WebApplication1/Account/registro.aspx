<%@ Import Namespace="System.Data.OleDb" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="registro.aspx.vb" Inherits="WebApplication2.WebForm2" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1Registro
        {
            width: 85%;
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
        }
        .style4Registro
        {
            height: 26px;
            text-align: right;
        }
        .style5
        {
            font-size: medium;
            color: #800000;
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
        Así de rápido, podrá comenzar a reportar sus declaraciones del IDE al SAT por 
        usted mismo; pruebe nuestro servicio.<br />
        (Si desea ser nuestro distribuidor, registrese en <a href="distribuidores.aspx">Distribuidores</a>)<br />
        <br />
        Si es Ud. un distribuidor y está registrando a otras instituciones financieras, 
        introduzca abajo los datos de cada una de ellas (los datos de facturación son 
        los de dichas instituciones), y si desea que la facturación salga a nombre suyo 
        envíenos un correo indicandonos los datos de facturación.&nbsp;
        <br />
        <br />
        </span><br />
        <table class="style1Registro">
            <tr>
                <td class="style3Registro">
                    &nbsp;</td>
                <td>
                    Para más detalles, coloque el mouse sobre recuadros no obvios</td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label1" runat="server" Text="Correo"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="correo" runat="server" Width="237px" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style4Registro">
        <asp:Label ID="Label2" runat="server" Text="Elija su contraseña de cliente"></asp:Label>
                </td>
                <td class="style2Registro">
        <asp:TextBox ID="passWeb" runat="server" Width="237px" MaxLength="15" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label17" runat="server" Text="Repita su Contraseña de cliente"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="passWeb2" runat="server" Width="237px" MaxLength="15" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label3" runat="server" Text="Razón Social para facturarle"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="razonSoc" runat="server" Width="237px" MaxLength="250" 
                        ToolTip="Razón social con la que está dado de alta en el SAT y a la que le facturaremos"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label16" runat="server" Text="Nombre de contacto"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="contacto" runat="server" Width="237px" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label4" runat="server" Text="Puesto"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="puesto" runat="server" Width="237px" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label5" runat="server" Text="Teléfono"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="tel" runat="server" Width="237px" MaxLength="40"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label6" runat="server" Text="Celular"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="cel" runat="server" Width="237px" MaxLength="10" onkeypress="return numeros()"></asp:TextBox>        
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label7" runat="server" Text="PáginaWeb"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="paginaWeb" runat="server" Width="237px" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label8" runat="server" Text="RFC empresa declarante para facturarle"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="rfcDeclarante" runat="server" Width="237px" MaxLength="12"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label10" runat="server" Text="Domicilio fiscal para facturarle"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="domFiscal" runat="server" Width="237px" MaxLength="100"></asp:TextBox>
                &nbsp;incluya colonia, C.P., población, mpio., edo.</td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label11" runat="server" Text="clave CASFIM o de Institución Financiera"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="casfim" runat="server" Width="237px" MaxLength="10" 
                        
                        ToolTip="La clave CASFIM de su institución, especifíquela correctamente, pues de lo contrario el SAT no podrá autorizarnos su socket para que pueda enviar declaraciones, si aún no lo tiene, introduzca provisionalmente cualquier clave numerica pero no olvide actualizarla por la oficial de su institución al ingresar en 'Mi cuenta'"></asp:TextBox>
                &nbsp;si aún no la tiene, puede indicar una clave<br />
                    numérica provisional al azar de 5 dígitos, y después poner la que le asigne el 
                    SAT</td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label13" runat="server" Text="¿Son un banco?"></asp:Label>
                </td>
                <td>
        <asp:CheckBox ID="esInstitCredito" runat="server" />
                    &nbsp;Banca múltiple, de fomento o desarrollo<br />
                    </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label14" runat="server" Text="# Sucursales"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="numSucursales" runat="server" onblur="Javascript:ceros(this);formatoNumero(this,0,'.',',');" Width="237px" onkeypress="return numeros()">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label15" runat="server" Text="# Socios o Clientes"></asp:Label>
                </td>
                <td>
        <asp:TextBox ID="numSociosClientes" runat="server" onblur="Javascript:ceros(this);formatoNumero(this,0,'.',',');" Width="237px" onkeypress="return numeros()">0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3Registro">
        <asp:Label ID="Label18" runat="server" Text="# Distribuidor"></asp:Label>
                </td>
                <td>
                <asp:TextBox ID="idDistribuidor" runat="server" AutoPostBack="True" Width="68px" 
                        
                        ToolTip="(opcional) Le es proporcionado por el personal que lo introdujo al sistema" 
                        onkeypress="return numeros()"></asp:TextBox>
                &nbsp;</td>
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
    
</asp:Content>