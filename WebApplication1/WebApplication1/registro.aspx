<%@ Import Namespace="System.Data.OleDb" %>

<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="registro.aspx.vb" Inherits="WebApplication1.registro" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="Server"> 
    <link rel="canonical" href="https://www.declaracioneside.com/registro.aspx" />
     <script src="https://code.jquery.com/jquery-3.0.0.min.js"></script>
    <script src="https://code.jquery.com/jquery-migrate-3.0.0.js"></script> 
    <script src="https://www.google.com/recaptcha/api.js?onload=onloadCallbackReg&render=explicit"
        async defer>
    </script>  
    <script>
           var verifyCallbackRegistro = function (response) {
            /*alert(response);*/
            $("#<%=responseRe.ClientID%>").val(response);
           }
        var onloadCallbackReg = function () {
            registro = grecaptcha.render('captchaRegistro', {
                'sitekey': '6LcjR4AUAAAAAD5QlcCBcXVU0QZOTlfepnXKdoD-',
                'callback': verifyCallbackRegistro,
                'theme': 'light'
            });
        }
    </script>
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
    <section class="signin-page account pt-2">
        <div class="container">
            <div class="row">
                <div class="col-sm-12">
                    <h4>Registro de institución financiera para envío de Declaraciones de Depósitos en Efectivo</h4>
                    <p>Envío de declaraciones informativas de IDE al SAT</p>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 mx-auto">
                    <div class="block text-center">
                        <div class="form-group"><asp:TextBox ID="correo" runat="server" placeHolder="Correo" MaxLength="50" CssClass="form-control" TabIndex="1" type="email"></asp:TextBox></div>
                        <div class="form-group"> <asp:TextBox ID="passWeb" runat="server" placeHolder="Elija su contraseña de cliente"  MaxLength="15" TextMode="Password" CssClass="form-control" ToolTip="seis caracteres o numeros mínimo" TabIndex="2"></asp:TextBox></div>
                        <div class="form-group"> <asp:TextBox ID="passWeb2" runat="server" placeHolder="Repita su contraseña de cliente" MaxLength="15" TextMode="Password" CssClass="form-control" ToolTip="seis caracteres o numeros mínimo" TabIndex="3"></asp:TextBox></div>
                        <div class="form-group"> <asp:TextBox ID="razonSoc" runat="server" placeHolder="Nombre completo de la institución financiera" MaxLength="250" ToolTip="Razón social con la que está dado de alta en el SAT" CssClass="form-control" TabIndex="4"></asp:TextBox></div>
                        <div class="form-group"><asp:TextBox ID="contacto" runat="server" placeHolder="Nombre de contacto" MaxLength="100" CssClass="form-control" TabIndex="5"></asp:TextBox></div>
                        <div class="form-group"><asp:TextBox ID="puesto" runat="server" placeHolder="Puesto" MaxLength="50" CssClass="form-control" TabIndex="6"></asp:TextBox></div>
                        <div class="form-group"><asp:TextBox ID="tel" runat="server" placeHolder="Teléfono (Incluir lada)" MaxLength="40" CssClass="form-control" TabIndex="7"></asp:TextBox></div>
                        <div class="form-group"><asp:TextBox ID="cel" runat="server" placeHolder="Celular (Incluir lada)" MaxLength="10" onkeypress="return numeros()" CssClass="form-control" TabIndex="8"></asp:TextBox></div>
                        <div class="form-group"><asp:TextBox ID="paginaWeb" runat="server" placeHolder="Página Web (Opcional)" MaxLength="200" CssClass="form-control" TabIndex="9"></asp:TextBox></div>
                        <div class="form-group"><asp:TextBox ID="rfcDeclarante" runat="server" placeHolder="RFC empresa declarante" MaxLength="12" CssClass="form-control" TabIndex="10"></asp:TextBox></div>
                        <div class="form-group"><asp:TextBox ID="domFiscal" runat="server" placeHolder="Domicilio fiscal completo (Incluir colonia,C.P.,localidad,edo.)" CssClass="form-control" MaxLength="100" TabIndex="11"></asp:TextBox></div>
                        <div class="form-group">
                            <asp:TextBox ID="casfim" CssClass="form-control" placeHolder="Clave CASFIM (Si aun no la tiene, indique una clave numerica provisional al azar de 5 o 6 digitos, marque la casilla de abajo)" runat="server" MaxLength="6" ToolTip="Clave CASFIM correcta, si aún no lo tiene, introduzca provisionalmente cualquier clave numerica de 5 o 6 digitos" TabIndex="12" Type="number"></asp:TextBox>                             
                        </div>
                        <div class="form-group">
                            <label class="form-control">
                                ¿Es clave Provisional?
                            <asp:CheckBox ID="casfimProvisional" CssClass="form-check-label" runat="server"  TabIndex="13" />
                            </label>
                        </div>
                        <div class="form-group">
                            <label class="form-control">
                            <asp:Label ID="Label13" runat="server" Text="¿Son un banco? " CssClass="style14"></asp:Label> 
                            <asp:CheckBox ID="esInstitCredito" runat="server" CssClass="style1master1" TabIndex="14" />   
                                (si no lo es deje en blanco la casilla)
                                </label>
                        </div>
                        <div class="form-group">
                            <label>¿Como te entreaste de nosostros?</label>
                              <asp:TextBox ID="fuente" runat="server" CssClass="form-control w-100" MaxLength="120"
                        TextMode="MultiLine"
                        ToolTip="Sea lo mas detallado posible, max. 120 caracteres" Width="237px" TabIndex="15"></asp:TextBox>
                               <asp:TextBox ID="numSucursales" runat="server"
                        onblur="Javascript:ceros(this);formatoNumero(this,0,'.',',');"
                        onkeypress="return numeros()" Visible="False" Width="31px">0</asp:TextBox>
                    <asp:TextBox ID="numSociosClientes" runat="server"
                        onblur="Javascript:ceros(this);formatoNumero(this,0,'.',',');"
                        onkeypress="return numeros()" Visible="False" Width="57px">0</asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="idDistribuidor" CssClass="form-control" Visible="false" placeHolder="Número de distribuidor(opcional)" runat="server" ToolTip="(opcional) Le es proporcionado por el personal que lo introdujo al sistema"  onkeypress="return numeros()" TabIndex="16"></asp:TextBox>
                        </div>
                        <%--<div class="form-inline text-center">                            
                            <asp:Image ID="imgCaptcha" runat="server" CssClass="img-fluid pr-5 pb-2" />  <asp:Button ID="otraImagen" runat="server" OnClick="otraImagen_Click" CssClass="btn btn-info" Text="Probar otra imagen"  TabIndex="19" />                                
                        </div>--%>
                        <div class="form-group">
                            <asp:HiddenField ID="responseRe" runat="server" />
                           <%-- <asp:TextBox ID="txtCaptcha" placeHolder="Introduce el texto de la imagen" runat="server" CssClass="form-control" TabIndex="17" />--%>
                            <div id="captchaRegistro"></div>
                        </div>
                        <div class="form-group">
                             <asp:Button ID="Button1" CssClass="btn btn-main" runat="server" Text="Registrarme"  TabIndex="18" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </section>
    <script type="text/javascript">
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
