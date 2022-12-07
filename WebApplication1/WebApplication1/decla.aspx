<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="decla.aspx.vb" Inherits="WebApplication1.WebForm11" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="Server">

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
    <div class="container">
        <div class="row">
            <div class="col-sm-12">
                <asp:Menu ID="NavigationMenu" runat="server" CssClass="container-fluid" EnableViewState="false" IncludeStyleBlock="false" Orientation="Horizontal" StaticMenuItemStyle-CssClass="nav-item nav-link text-white btn-info" StaticMenuStyle-CssClass="nav navbar-expand-lg">
                    <Items>
                        <asp:MenuItem NavigateUrl="~/cliente.aspx" Text="Cuenta" />
                        <asp:MenuItem NavigateUrl="~/misContra.aspx" Text="Mis contratos" />
                        <asp:MenuItem NavigateUrl="~/misDecla.aspx" Text="Mis declaraciones" />
                        <asp:MenuItem NavigateUrl="~/decla.aspx" Text="Declarar" />
                    </Items>
                </asp:Menu>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="mipanel" ChildrenAsTriggers="true">
            <Triggers>
                <asp:PostBackTrigger ControlID="aplicar" />
                <asp:PostBackTrigger ControlID="restablecer" />
            </Triggers>


            <ContentTemplate>       
                                                     
        <div class="row">
            <div class="col-sm-12">
                
                    <h3 class="text-center">Declarar</h3>                
                <div class="row">
                    <asp:Label ID="idAnual" runat="server" Font-Size="X-Small"></asp:Label><asp:Label ID="var" runat="server" Font-Size="X-Small">0</asp:Label>                                  
                   <asp:Label ID="redir" runat="server" Font-Size="X-Small"></asp:Label>
                </div>
                
                
                    
                    <asp:HiddenField ID="HiddenField1" runat="server" Visible="False" />                
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-2 text-right" style="font-size:small">
                <asp:Label ID="lblContratos" runat="server" Text="Contratos pagados:" ></asp:Label>
            </div>
            <div class="col-sm-2 text-left">
                <asp:DropDownList ID="contratos" runat="server" AutoPostBack="True" ToolTip="Elija el contrato con el que desea operar correspondiente al periodo que desea declarar, se muestran en el orden sugerido, solo los contratos vigentes o de regularización pueden crear declaraciones" CssClass="form-control form-control-sm">
                </asp:DropDownList>
            </div>
            <div class="col-sm-4">
                <p>
                    <asp:Label ID="idContrato" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="elplan" runat="server"></asp:Label>
                    <asp:Label ID="esRegularizacion" runat="server" Font-Size="Small"></asp:Label>
                    <asp:Button ID="restablecer" runat="server" Text="Eliminar" ToolTip="1o elija el contrato corresp., Libera 1 decl y la regresa a vacia" CssClass="btn btn-sm btn-danger" />
                </p>
            </div>
            <div class="col-sm-2">
                
            </div>
            <div class="col-sm-2">
                
            </div>
        </div>
        <div class="row pb-1">            
            <div class="col-sm-2 text-right float-md-right">
                <asp:DropDownList  ID="tipoMensAn" runat="server" AutoPostBack="True" ToolTip="1o declare las mensuales y luego las anuales" CssClass="form-control form-control-sm " style="text-align:right" >
                    <asp:ListItem Value="Mensual">Mensual</asp:ListItem>
                    <asp:ListItem Value="Anual">Anual</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col-sm-2">
                        Ejercicio
                    </div>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="ejercicio" runat="server" AutoPostBack="True" Width="80"   CssClass="form-control form-control-sm">
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-1">
                        <asp:Label ID="lblMes" runat="server"  Text="Mes:"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="mes" runat="server" AutoPostBack="True" Width="120" CssClass="form-control form-control-sm">
                            <asp:ListItem Value="1">Enero</asp:ListItem>
                            <asp:ListItem Value="2">Febrero</asp:ListItem>
                            <asp:ListItem Value="3">Marzo</asp:ListItem>
                            <asp:ListItem Value="4">Abril</asp:ListItem>
                            <asp:ListItem Value="5">Mayo</asp:ListItem>
                            <asp:ListItem Value="6">Junio</asp:ListItem>
                            <asp:ListItem Value="7">Julio</asp:ListItem>
                            <asp:ListItem Value="8">Agosto</asp:ListItem>
                            <asp:ListItem Value="9">Septiembre</asp:ListItem>
                            <asp:ListItem Value="10">Octubre</asp:ListItem>
                            <asp:ListItem Value="11">Noviembre</asp:ListItem>
                            <asp:ListItem Value="12">Diciembre</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-sm-1 pr-0 text-right" >
                <asp:Label ID="Label1" runat="server" Text="Operación:" Font-Size="Small"></asp:Label>
            </div>
            <div class="col-sm-5">
                <div class="row pb-1">
                    <div class="col-sm-7">
                        <asp:DropDownList ID="oper" runat="server" AutoPostBack="True" ToolTip="Operación a realizar" CssClass="form-control form-control-sm text-left">
                            <asp:ListItem Value="0">Crear/Editar Declaración</asp:ListItem>
                            <asp:ListItem Value="1">Crear Declaración en Ceros y Enviar</asp:ListItem>
                            <asp:ListItem Value="2">Consultar Declaración y Acuses</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-5">
                         <asp:Button ID="aplicar" runat="server" Text="Aplicar" CssClass="btn btn-sm btn-info"  />
                      
                    </div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-4">
                        <asp:DropDownList ID="via" runat="server" CssClass="form-control form-control-sm" AutoPostBack="True">
                            <asp:ListItem Value="0" Selected="True">Vía Excel</asp:ListItem>
                            <asp:ListItem Value="1">Vía Xml</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-8">
                        <asp:Label ID="lblVia" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-3 text-right">
                        <asp:Label ID="lblNumDecl" runat="server" Text="#Decl."></asp:Label>                        
                    </div>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="numDecl" runat="server" AutoPostBack="True" CssClass="form-control form-control-sm text-left">
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-3"></div>
                    <div class="col-sm-3"></div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-5">
                         <asp:CheckBox ID="complementaria" runat="server" AutoPostBack="True" Text="Crear Complementaria" Font-Size="Small" Visible="false" />
                    </div>
                    <div class="col-sm-7"></div>
                </div>
            </div>
        </div>

    </div>
    <div class="row pb-1">
        <iframe frameborder="0" scrolling="auto" id="frame1" runat="server" width="100%" height="900px" class="scroll scroll4 col-sm-12"></iframe>
    </div>
    <div class="row pb-1">
        <div class="col-sm-12">
        </div>
    </div>
                </ContentTemplate>
        </asp:UpdatePanel>

</asp:Content>
