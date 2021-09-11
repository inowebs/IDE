<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="misDecla.aspx.vb" Inherits="WebApplication1.WebForm17" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" %>

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
    <style type="text/css">
        a.ContentPlaceHolder1_TreeView1_0 {
            color:blue;
        }
    </style>
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
        <div class="row pb-1">
            <div class="col-sm-12">
                <h3 class="text-center">Mis Declaraciones
                </h3>
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-12">
                <p class="alert alert-info">
                    Seleccione su declaración para cargarla inmediatamente
                </p>
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-12">
                <asp:TreeView ID="TreeView1" runat="server"
                    Style="font-family:blue; font-family:Calibri, 'Trebuchet MS', sans-serif" >
                </asp:TreeView>
            </div>
        </div>
    </div>
</asp:Content>
