<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="contratos.aspx.vb" Inherits="WebApplication1.WebForm7" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

    <style type="text/css">
        .style3 {
            width: 15px;
        }

        .style4 {
            width: 659px;
        }

        .style7 {
            font-size: medium;
            color: #800000;
        }

        .style11 {
            width: 101px;
        }

        .style13 {
            font-family: arial;
            font-size: small;
        }

        .modalBackground {
            background-color: white;
            filter: alpha(opacity=90);
            opacity: 0.99;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 300px;
            height: 140px;
        }
    </style>
    <%-- <script type="text/javascript" language="javascript">
        function scrollTo(what) {
            if (what != "0")
                document.getElementById(what).scrollTop = document.getElementById("scrollPos").value;
        }
    </script>--%>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
    <ajaxToolkit:ModalPopupExtender ID="panel1_ModalPopupExtender" runat="server" DropShadow="True" PopupControlID="Panel2" TargetControlID="btnOculto" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>
    <asp:Button ID="btnOculto" runat="server" Text="oculto" Height="0px" Width="0px" Style="display: none" />
    <asp:Panel ID="Panel2" runat="server" align="center" Style="display: none">
        <div class="card">
            <div class="card-header">
                <h5>Acceso</h5>
            </div>
            <div class="card-body">
                <div class="container">
                    <div class="row">
                        <div class="col-sm-6">
                            <label for="pass1">Contraseña de acceso</label>
                        </div>
                        <div class="col-sm-6">
                            <asp:TextBox ID="pass1" runat="server" TextMode="Password" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:Button ID="ingresar" CssClass="btn btn-primary" runat="server" Text="Ingresar" />
            </div>
        </div>
    </asp:Panel>
    <div class="container">
        <div class="row">
            <h5>Contratos</h5>
        </div>
        <div class="row">
            <asp:LinkButton ID="add" runat="server">Agregar</asp:LinkButton>,Buscar
        </div>
        <div class="row">
            <div class="col-sm-1">
                <asp:CheckBox ID="chkRazon" runat="server" Text="Institución" Style="font-size: x-small" />
            </div>
            <div class="col-sm-2">
                <asp:TextBox ID="razon" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
            </div>
            <div class="col-sm-1">
                <asp:CheckBox ID="chkFechaPago" runat="server" Text="FechPago" Style="font-size: x-small" />
            </div>
            <div class="col-sm-2">
                <asp:TextBox ID="fechaPago" runat="server" MaxLength="10" ToolTip="dd/mm/aaaa" CssClass="form-control form-control-sm"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="fechaPago_CalendarExtender" Enabled="True"
                    runat="server" TargetControlID="fechaPago" CssClass="MyCalendar bg-white rounded" Format="dd/MM/yyyy">
                </ajaxToolkit:CalendarExtender>
            </div>
            <div class="col-sm-2">
                <asp:TextBox ID="fechaPago2" runat="server" MaxLength="10"
                    ToolTip="dd/mm/aaaa" CssClass="form-control form-control-sm"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="fechaPago2_CalendarExtender" runat="server"
                    TargetControlID="fechaPago2" CssClass="MyCalendar" Format="dd/MM/yyyy">
                </ajaxToolkit:CalendarExtender>
            </div>
            <div class="col-sm-1">
                <asp:CheckBox ID="chkPlan" runat="server" Text="Plan" Style="font-size: small" />
            </div>
            <div class="col-sm-2">
                <asp:DropDownList ID="elPlan" runat="server" DataSourceID="SqlDataSource1" DataTextField="elplan" DataValueField="elplan" CssClass="form-control form-control-sm"></asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-1">
                <asp:CheckBox ID="chkCorreo"
                    runat="server" Text="Correo" Style="font-size: small" />
            </div>
            <div class="col-sm-2">
                <asp:TextBox ID="correo" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
            </div>
            <div class="col-sm-1">
                <asp:CheckBox ID="chkIni"
                    runat="server" Text="Iniciales"
                    Style="font-size: small" />
            </div>
            <div class="col-sm-2">
                <asp:CheckBox ID="chkNum"
                    runat="server" Text="Num. contrato"
                    Style="font-size: small" />
            </div>
            <div class="col-sm-2">
                <asp:TextBox ID="num" runat="server" MaxLength="10" CssClass="form-control form-control-sm"></asp:TextBox>
            </div>
            <div class="col-sm-1">
                <asp:CheckBox ID="chkStatus" runat="server" Text="Status" />
            </div>
            <div class="col-sm-2">
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ideConnectionString %>" SelectCommand="SELECT [elplan] FROM [planes]"></asp:SqlDataSource>
                <asp:DropDownList ID="Status" CssClass="form-control form-control-sm" runat="server">
                    <asp:ListItem>VIGENTES</asp:ListItem>
                    <asp:ListItem>VENCIDOS</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row pt-3">
            <div class="col-sm-1">
                <asp:CheckBox ID="chkReg" runat="server" Text="Regulariza" Font-Size="x-Small" />
            </div>
            <div class="col-sm-2">
                <asp:DropDownList ID="Reg" CssClass="form-control form-control-sm" runat="server">
                    <asp:ListItem>CON</asp:ListItem>
                    <asp:ListItem>SIN</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-sm-1">
                <asp:CheckBox ID="chkPagado" runat="server" Text="Pagado" Font-Size="Small" />
            </div>
            <div class="col-sm-1">
                <asp:CheckBox ID="chkuuid" runat="server" Text="UUID" Font-Size="Small" />
            </div>
            <div class="col-sm-2">
                <asp:TextBox ID="uuid" runat="server" Columns="36" MaxLength="36"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-1">
                <asp:CheckBox ID="chkFormapago" runat="server" Text="FormaPago" Font-Size="x-Small" />
            </div>
            <div class="col-sm-3">
                <asp:DropDownList ID="pagoRealizado" runat="server">
                                                <asp:ListItem Value="Efectivo">Efectivo</asp:ListItem>
                                                <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                                                <asp:ListItem Value="Transferencia">Transferencia</asp:ListItem>
                                                <asp:ListItem Value="TarjetaCredito">TarjetaCredito</asp:ListItem>
                                                <asp:ListItem Value="DineroElectronico">DineroElectronico</asp:ListItem>
                                            </asp:DropDownList>
            </div>
            <div class="col-sm-1">
                <asp:CheckBox ID="chkFechaContra" runat="server" Text="FechaContrato" Style="font-size: x-small" />
            </div>
            <div class="col-sm-1">
                <asp:TextBox ID="fechaContra1" runat="server" MaxLength="10" ToolTip="dd/mm/aaaa" CssClass="form-control form-control-sm"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" Enabled="True"
                    runat="server" TargetControlID="fechaContra1" CssClass="MyCalendar bg-white rounded" Format="dd/MM/yyyy">
                </ajaxToolkit:CalendarExtender>
            </div>
            <div class="col-sm-1">
                <asp:TextBox ID="fechaContra2" runat="server" MaxLength="10"
                    ToolTip="dd/mm/aaaa" CssClass="form-control form-control-sm"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server"
                    TargetControlID="fechaContra2" CssClass="MyCalendar" Format="dd/MM/yyyy">
                </ajaxToolkit:CalendarExtender>
            </div>
            <div class="col-sm-1">
                <asp:CheckBox ID="chkMonto"
                    runat="server" Text="Monto"
                    Style="font-size: small" />
            </div>
            <div class="col-sm-2">
                <asp:TextBox ID="monto1" runat="server" MaxLength="10" CssClass="form-control form-control-sm"></asp:TextBox>
            </div>
            <div class="col-sm-2">
                <asp:TextBox ID="monto2" runat="server" MaxLength="10" CssClass="form-control form-control-sm"></asp:TextBox>
            </div>
         </div>
         <div class="row">
            <div class="col-sm-3">
                <asp:Button ID="Buscar" runat="server" Text="Buscar" CssClass="btn btn-info" />
            </div>
            <div class="col-sm-3">
                <asp:Label ID="nRegs" runat="server" Text="0 Registros, PrecioNetoTotal = 0"></asp:Label>
            </div>

            </div>
        <div class="row pb-2">
            <p>
                Seleccione el contrato:
            </p>
            <div class="container">
                <div class="row">
                    <div class="col-sm-12 scroll scroll4" style="max-height: 400px; width:100%">
                        <asp:GridView ID="GridView3" runat="server" AllowSorting="true"
                            AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False" 
                            DataKeyNames="id" DataSourceID="SqlDataSource3" Width="100%" CssClass="style9master" ShowHeaderWhenEmpty="True" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal" Font-Size="Small">
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="white" />
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" ItemStyle-Width="75">
                                </asp:CommandField>
                                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False"
                                    ReadOnly="True" SortExpression="id" ItemStyle-Width="100">
                                </asp:BoundField>
                                <asp:BoundField DataField="precioNetoContrato" HeaderText="precioNeto" ReadOnly="True"
                                    SortExpression="precioNetoContrato" ItemStyle-Width="80" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right">
                                    <ItemStyle HorizontalAlign="Right" ></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="fechaPago" HeaderText="fechaPago" ReadOnly="True"
                                    SortExpression="fechaPago" ItemStyle-Width="100" DataFormatString="{0:d}">
                                </asp:BoundField>
                                <asp:BoundField DataField="elplan" HeaderText="Plan"
                                    SortExpression="elplan" ItemStyle-Width="80">
                                </asp:BoundField>
                                <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="fecha"
                                    ItemStyle-Width="100" DataFormatString="{0:d}">
                                </asp:BoundField>
                                <asp:BoundField DataField="nDeclHechas" HeaderText="Hechas" ItemStyle-Width="80"
                                    SortExpression="nDeclHechas" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                    <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="nDeclContratadas" HeaderText="Contratadas" ItemStyle-Width="80"
                                    SortExpression="nDeclContratadas" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Right">
                                    <ItemStyle HorizontalAlign="Right" Width="80px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="correo" HeaderText="Correo" ItemStyle-Width="100"
                                    SortExpression="correo">
                                </asp:BoundField>
                                <asp:BoundField DataField="esRegularizacion" HeaderText="Regul." ItemStyle-Width="50"
                                    SortExpression="esRegularizacion">
                                </asp:BoundField>
                                <asp:BoundField DataField="uuid" HeaderText="UUID" 
                                    SortExpression="uuid">
                                </asp:BoundField>
                                <asp:BoundField DataField="pagoRealizado" HeaderText="formaPago" 
                                    SortExpression="pagoRealizado">
                                </asp:BoundField>
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                            <HeaderStyle BackColor="#333333" Height="26px" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#242121" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                            ConnectionString="<%$ ConnectionStrings:ideConnectionString2 %>"
                            SelectCommand="SELECT co.id,co.precioNetoContrato,co.fechaPago,pla.elplan,co.fecha,co.nDeclHechas,co.nDeclContratadas,cli.correo,co.esRegularizacion, co.uuid, co.pagoRealizado FROM contratos co, clientes cli, planes pla WHERE co.idCliente=cli.id AND co.idPlan=pla.id and cli.id=-1 ORDER BY co.id"></asp:SqlDataSource>
                    </div>
                </div>
            </div>

        </div>
    </div>
    
</asp:Content>
