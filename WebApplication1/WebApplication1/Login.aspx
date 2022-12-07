<%@ Page Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="WebApplication1.WebForm24" Title="Declaraciones IDE, Solución para declaración informativa del impuesto IDE" MetaDescription="Solución para declarar el Impuesto a los Depósitos en Efectivo (IDE). Envía declaraciones de IDE mensuales y anuales aquí" MaintainScrollPositionOnPostback="true" %>

<script runat="server">        

</script>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="canonical" href="https://www.declaracioneside.com/Login.aspx" />
     <script src="https://code.jquery.com/jquery-3.0.0.min.js"></script>
    <script src="https://code.jquery.com/jquery-migrate-3.0.0.js"></script> 
    <script src="https://www.google.com/recaptcha/api.js?onload=onloadCallbackLogin&render=explicit"
        async defer>
    </script>
    <script>
        var onloadCallbackLogin = function () {
            login = grecaptcha.render('captcha', {
                'sitekey': '6LcjR4AUAAAAAD5QlcCBcXVU0QZOTlfepnXKdoD-',
                'callback': verifyCallback,
                'theme': 'light'
            });
        }
        var verifyCallback = function (response) {
            /*alert(response);*/
            $("#<%=responseRe.ClientID%>").val(response);
        }             
       
    </script>
    <style type="text/css">
        .style3 {
            font-size: medium;
            color: #800000;
            font-weight: bold;
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
   <asp:HiddenField ID="responseRe" runat="server" />
     <section class="signin-page account pt-3">
        <div class="container">
            <div class="row">
                <div class="col-md-12 mx-auto">
                    <h4 class="text-center">Iniciar sesión</h4>

                    <div class="block">
                        <p class="text-center text-dark">
                            <asp:Label ID="Label1" runat="server" Text="Solución para declaraciones de Depósitos en Efectivo (ISR, IDE). Envía declaraciones mensuales y anuales de Depósitos en Efectivo (ISR, IDE) aquí."></asp:Label></p>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4"></div>
                <div class="col-md-4 text-center bg-white p-4 rounded">
                    <asp:Label ID="Label2" runat="server" Text="Prueba el DEMO o ingresa tu cuenta"></asp:Label>
                    <%--<img style="border-radius: 4px 4px;" src="probarDemoIDE.png" alt="demo,instrucciones para demo" />--%>
                    <asp:Login ID="LoginUser" CssClass="table text-center" runat="server" EnableViewState="false" RenderOuterTable="true" OnAuthenticate="OnAuthenticate" OnLoggedIn="OnLoggedIn">
                        <LayoutTemplate>
                            <span class="failureNotification">
                                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                            </span>
                            <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification"
                                ValidationGroup="LoginUserValidationGroup" />
                            <fieldset class="login">

                                <p>
                                    <asp:TextBox ID="UserName" runat="server" placeHolder="Correo registrado (usuario):" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                        CssClass="failureNotification" ErrorMessage="El correo es obligatorio." ToolTip="El correo es obligatorio."
                                        ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="Password" runat="server" placeHolder="Contraseña de cliente:" CssClass="form-control" TextMode="Password" TabIndex="1"></asp:TextBox>
                                </p>
                                <div id="captcha"></div>
                                
                                <%--<p class="text-center">
                                    <asp:Image ID="imgCaptcha" runat="server" />
                                    <asp:TextBox ID="txtCaptcha" placeHolder="Introduce el texto de la imágen:" runat="server" CssClass="form-control" TabIndex="2" />
                                    <asp:Label ID="lblMessage" runat="server" CssClass="red" Style="font-size: small; font-family: Arial" />
                                    &nbsp;<asp:LinkButton ID="otraImagen" CssClass="text-dark" runat="server" OnClick="otraImagen_Click1" TabIndex="5">Probar otra imagen</asp:LinkButton>
                                <p>--%>
                                    <asp:Button ID="LoginButton" runat="server" CommandName="Login"
                                        OnClick="LoginButton_Click" Text="Ingresar"
                                        ValidationGroup="LoginUserValidationGroup" CssClass="btn btn-main" TabIndex="3" />
                                </p>
                                <p>
                                    <asp:HyperLink ID="HyperLink2" runat="server"
                                        NavigateUrl="recoverPassword.aspx" CssClass="text-dark" Width="200px" TabIndex="6">¿Olvidó su contraseña?</asp:HyperLink>
                                </p>
                                <p>
                                    <asp:CheckBox ID="RememberMe" runat="server" TabIndex="4" />
                                    <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline text-dark">Mantenerme conectado</asp:Label>
                                </p>
                            </fieldset>
                        </LayoutTemplate>
                    </asp:Login>
                    <asp:HyperLink ID="RegisterHyperLink" CssClass="text-dark" runat="server" EnableViewState="false">Registrarse</asp:HyperLink>
                    &nbsp;si no tiene una cuenta.                    
                </div>
                <div class="col-md-5"></div>
            </div>
        </div>
    </section>
</asp:Content>
