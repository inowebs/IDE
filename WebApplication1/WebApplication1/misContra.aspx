<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="misContra.aspx.vb" Inherits="WebApplication1.WebForm10" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" %>

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
        <div class="row ">
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
    </div>
    <div class="row">
        <div class="container">
            <div class="col-sm-12">
                <p class="style7">
                    <span class="style1master1">
                        <strong>Mis Contratos. </strong></span><span class="style13">
                            <a href="http://youtu.be/ZpeOxQg9SKo" target="_blank" style="color:#007bff">VideoTutorial</a></span>
                </p>
                <p class="style8">
                    <asp:LinkButton ID="nuevo" runat="server" style="color:#007bff">Solicitar nuevo contrato</asp:LinkButton>
                </p>
                <p class="style14">
                    Para pagos, solicite un nuevo contrato o bien elija uno
                </p>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="col-md-12 scroll scroll4 " style="max-height: 500px; width:100%">
                <asp:GridView ID="GridView3" runat="server"
                    AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False"
                    DataKeyNames="fechaPago,periodoInicial" DataSourceID="SqlDataSource3" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal" Font-Size="Small">
                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="white" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" ButtonType="Button">
                            <ItemStyle></ItemStyle>
                        </asp:CommandField>
                        <asp:BoundField DataField="id" HeaderText="Número de contrato" InsertVisible="False"
                            ReadOnly="True" SortExpression="id" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="precioNetoContrato" HeaderText="Precio Neto" ReadOnly="True"
                            SortExpression="precioNetoContrato" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="fechaPago" HeaderText="Fecha de Pago" ReadOnly="True"
                            SortExpression="fechaPago" DataFormatString="{0:d}">
                            <ItemStyle></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="periodoInicial" HeaderText="Periodo Inicial" ReadOnly="True"
                            SortExpression="periodoInicial" DataFormatString="{0:d}">
                            <ItemStyle></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="elplan" HeaderText="Plan"
                            SortExpression="elplan">
                            <ItemStyle></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="fechaFinal" HeaderText="Fecha Final" SortExpression="fechaFinal"
                            DataFormatString="{0:d}">
                            <ItemStyle></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="nDeclHechas" HeaderText="Declaraciones Hechas"
                            SortExpression="nDeclHechas" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="nDeclContratadas" HeaderText="Declaraciones Contratadas"
                            SortExpression="nDeclContratadas" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="uuid" HeaderText="uuid"
                            SortExpression="uuid" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                            <ItemStyle></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#242121" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ideConnectionString %>"
                    SelectCommand=""></asp:SqlDataSource>
            </div>
        </div>
    </div>


    <asp:Label ID="nRegs" runat="server" Text="0 Registros"></asp:Label>

</asp:Content>
