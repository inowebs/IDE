<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="admon.aspx.vb" Inherits="WebApplication1.WebForm32" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="Server">

    <style type="text/css">
        /* TAB MENU   
----------------------------------------------------------*/

        div.hideSkiplink {
            background-color: #3a4f63;
            width: 100%;
            height: 0px;
        }

        div.menu {
            padding: 4px 0px 4px 8px;
            width: 100%;
            background-color: #465c71;
        }

            div.menu ul {
                list-style: none;
                margin: 0px;
                padding: 0px;
            }


                div.menu ul li a, div.menu ul li a:visited {
                    background-color: #465c71;
                    /*border: 1px #4e667d solid;*/
                    color: #dde4ec;
                    display: block;
                    line-height: 1.35em;
                    padding: 4px 15px;
                    text-decoration: none;
                    white-space: nowrap;
                    font-family: Arial;
                    position: relative;
                    z-index: 100;
                    font-size: medium;
                    font-weight: normal;
                }

                    div.menu ul li a:hover {
                        background-color: #bfcbd6;
                        color: #465c71;
                        text-decoration: none;
                    }

                    div.menu ul li a:active {
                        background-color: #465c71;
                        color: #cfdbe6;
                        text-decoration: none;
                    }

        .modalBackground {
            background-color: white;
            filter: alpha(opacity=90);
            opacity: 0.99;
        }

        .modalPopup {
            background-color: #ffffff;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 300px;
            height: 140px;
        }
    </style>

</asp:Content>


<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="container">
        <div class="row">

            <strong>
                <br />
                <span class="style15admon">Administración</span>
            </strong>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            <asp:Timer ID="Timer1" runat="server" Interval="1000" Enabled="False" />

            <ajaxToolkit:ModalPopupExtender ID="panel1_ModalPopupExtender"
                runat="server" DropShadow="True" PopupControlID="Panel5"
                TargetControlID="btnOculto" BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Button ID="btnOculto" runat="server" Text="oculto" Height="0px" Width="0px" Style="display: none" />

            <asp:Panel ID="Panel5" runat="server" CssClass="modalPopup" align="center" Style="display: none">
                <span class="style14">Contraseña de acceso:</span>
                <asp:TextBox ID="pass" runat="server" TextMode="Password"></asp:TextBox>
                <asp:Button ID="ingresar" runat="server" Text="Ingresar" />
                <br />

            </asp:Panel>
            <br />
            <br />
            <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/clienteList.aspx?lan=1">Clientes</asp:HyperLink>
            &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/contratos.aspx?lan=1"
                    ForeColor="#009999" CssClass="style9master">Contratos</asp:HyperLink>
            &nbsp;
&nbsp;&nbsp;&nbsp;      
            <asp:Menu ID="MenuTabs1" runat="server" CssClass="menu" EnableViewState="false" IncludeStyleBlock="false" Orientation="Horizontal" OnMenuItemClick="MenuTabs1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="AutoCotizador" Value="5" NavigateUrl="admon.aspx?v=5" />
                    <asp:MenuItem Text="UUID" Value="6" NavigateUrl="admon.aspx?v=6" />
                    <asp:MenuItem Text="IVA" Value="0" NavigateUrl="admon.aspx?v=0" />
                    <asp:MenuItem Text="IDE" Value="1" NavigateUrl="admon.aspx?v=1" />
                    <asp:MenuItem Text="PLANES" Value="2" NavigateUrl="admon.aspx?v=2" />
                    <asp:MenuItem Text="DESCUENTOS" Value="3" NavigateUrl="admon.aspx?v=3" />
                    <asp:MenuItem Text="Prospectos y Asesorias" Value="4" NavigateUrl="admon.aspx?v=4" />
                    
                </Items>
            </asp:Menu>
        </div>

        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="View1" runat="server">
                <div class="row">
                    <asp:Panel ID="Panel1" runat="server" CssClass="container">
                        <div class="row">
                            <div class="col-sm-12">
                                <h3>
                                    <asp:Label ID="Label1" runat="server" Text="IVA"></asp:Label>
                                </h3>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <p>
                                    <asp:Label ID="Label3" runat="server" Text="Porcentaje en uso:"></asp:Label>
                                    <asp:Label ID="actualIva" runat="server" Text="Label"></asp:Label>
                                </p>
                            </div>
                        </div>
                        <div class="row pb-1">
                            <div class="col-sm-1">
                                ID
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="id" runat="server" Text="ID"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                Porcentaje
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox ID="porcen" runat="server" onkeypress="return numerosDec()" onblur="Javascript:formatoNumero(this,2,'.',',');" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>

                            <div class="col-sm-2">

                            </div>
                            <div class="col-sm-5">
                                <div class="row">
                                    <div class="col-sm-3">
                                        <asp:Button ID="add" runat="server" Text="Agregar" CssClass="btn btn-sm btn-success"/>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Button ID="edit" runat="server" Text="Modificar" CssClass="btn btn-sm btn-main p-1"/>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Button ID="del" runat="server" Text="Eliminar" CssClass="btn btn-sm btn-danger"                                           OnClientClick="return confirm('¿Esta seguro de eliminar este registro?');" />
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Button ID="defActualIva" runat="server" Text="Definir como actual" CssClass="btn btn-sm btn-info"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row pb-1">
                            <div style="max-height: 300px; width:100%" runat="server" id="divScroll" class="col-sm-12 scroll scroll4">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="porcen" DataSourceID="SqlDataSource1" AlternatingRowStyle-BackColor="#C2D69B" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True">
                                        </asp:CommandField>
                                        <asp:BoundField DataField="id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="id">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="porcen" HeaderText="Porcentaje" ReadOnly="True" SortExpression="porcen"  DataFormatString="{0:N}" HtmlEncode="false" >
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                    <SelectedRowStyle BackColor="#CC3333" ForeColor="white" Font-Bold="True" />
                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                    <HeaderStyle BackColor="#333333"  Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                    <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                    <SortedDescendingHeaderStyle BackColor="#242121" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ideConnectionString2 %>" SelectCommand="SELECT [id], [porcen] FROM [iva] ORDER BY [id]"></asp:SqlDataSource>
                            </div>

                        </div>
                        <div class="row pb-1">
                            <div class="col-sm-12">
                                <asp:Label ID="ivaNregs" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </asp:View>
            <asp:View ID="View2" runat="server">
                <asp:Panel ID="Panel2" runat="server" Style="position: relative; top: 25px; left: 0px; height: 528px;"
                    Width="360px" BorderColor="#006666" BorderStyle="Solid" BorderWidth="1px">
                    &nbsp;
                                    <asp:Label ID="Label4" runat="server" ForeColor="Maroon"
                                        Style="font-weight: 900; font-size: large;" Text="IDE"></asp:Label>
                    <br />
                    &nbsp;<asp:Label ID="Label5" runat="server" Text="Id En uso:"></asp:Label>
                    &nbsp;&nbsp;
                                    <asp:Label ID="actualIde" runat="server"
                                        Style="color: #000000; background-color: #DDDDDD" Text="Label"></asp:Label>
                    <br />
                    <br />
                    <table class="style1">
                        <tr>
                            <td class="style4">ID</td>
                            <td>
                                <i><b>Límite $</b></i></td>
                            <td>
                                <strong><em>Porcentaje</em></strong></td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:Label ID="idIde" runat="server" Text="ID"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="limite" runat="server" Width="125px"
                                    onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');"
                                    onkeypress="return numerosDec()"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="idePorcen" runat="server" Width="56px" onkeypress="return numerosDec()" onblur="Javascript:formatoNumero(this,2,'.',',');"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style3">&nbsp;</td>
                            <td>
                                <asp:Button ID="addIde" runat="server" Style="height: 26px; font-size: small;"
                                    Text="+ Agregar" />
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style3">&nbsp;</td>
                            <td>
                                <asp:Button ID="editIde" runat="server" Text="(..) Modificar" />
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style3">&nbsp;</td>
                            <td>
                                <asp:Button ID="delIde" runat="server" Text="- Eliminar"
                                    OnClientClick="return confirm('¿Esta seguro de eliminar este registro?');" />
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style3">&nbsp;</td>
                            <td>
                                <asp:Button ID="defActualIde" runat="server" Text="_/ Definir como actual" />
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <br />
                    <div style="height: 25px; width: 309px; margin: 0; padding: 0">
                        <table id="tblHeaderIde" bgcolor="#EDEDED" border="1" cellpadding="0"
                            cellspacing="0" rules="all" style="border-collapse: collapse; height: 100%;">
                            <tr>
                                <td style="width: 79px; text-align: center"></td>
                                <td style="width: 50px; text-align: center">ID</td>
                                <td style="width: 100px; text-align: center">Límite</td>
                                <td style="width: 80px; text-align: center">Porcentaje</td>
                            </tr>
                        </table>
                    </div>
                    <input type="hidden" id="scrollPos2" runat="server" value="0" />
                    <div style="overflow: auto; height: 200px; width:100%" runat="server" id="divScroll2" onscroll="javascript:document.getElementById('scrollPos2').value = document.getElementById('divScroll2').scrollTop;">
                        <asp:GridView ID="GridView2" runat="server"
                            AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False"
                            DataKeyNames="limite,porcen" DataSourceID="SqlDataSource2"
                            ShowHeader="False">
                            <AlternatingRowStyle BackColor="#C2D69B" />
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" ItemStyle-Width="75px" />
                                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False"
                                    ItemStyle-Width="60px" ReadOnly="True" SortExpression="id"></asp:BoundField>
                                <asp:BoundField DataField="limite" HeaderText="limite" ItemStyle-Width="100px"
                                    ReadOnly="True" SortExpression="limite" DataFormatString="{0:C}" HtmlEncode="false"></asp:BoundField>
                                <asp:BoundField DataField="porcen" HeaderText="porcen" ItemStyle-Width="80px"
                                    ReadOnly="True" SortExpression="porcen" DataFormatString="{0:N}" HtmlEncode="false"></asp:BoundField>
                            </Columns>
                            <SelectedRowStyle BackColor="#990000" Font-Bold="false" ForeColor="white" />
                            <HeaderStyle BackColor="#EDEDED" Height="26px" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                            ConnectionString="<%$ ConnectionStrings:ideConnectionString2 %>"
                            SelectCommand="SELECT [id], [limite], [porcen] FROM [ideConf] ORDER BY [id]"></asp:SqlDataSource>
                    </div>
                    <br />
                    &nbsp;<asp:Label ID="ideNregs" runat="server" Text=""></asp:Label>
                </asp:Panel>
            </asp:View>
            <asp:View ID="View3" runat="server">
                <asp:Panel ID="Panel3" runat="server"
                    Style="position: relative; top: 25px; left: 0px; height: 528px; width: 580px;"
                    BorderColor="#006666" BorderStyle="Solid" BorderWidth="1px">
                    &nbsp;
                                    <asp:Label ID="Label2" runat="server" ForeColor="Maroon"
                                        Style="font-weight: 900; font-size: large;" Text="PLANES"></asp:Label>
                    <br />
                    &nbsp;&nbsp;&nbsp;
                                    <br />
                    <br />
                    <table class="style1">
                        <tr>
                            <td class="style4">ID</td>
                            <td>
                                <b><i>Fecha</i></b></td>
                            <td>
                                <strong><em>Plan</em></strong></td>
                            <td>
                                <b><i>PrecioBaseMes</i></b></td>
                            <td>
                                <b><i>Iva</i></b></td>
                            <td>
                                <b><i>Inscripción</i></b></td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:Label ID="idPlan" runat="server" Text="ID"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="fecha" runat="server" Text="ID" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="elPlan" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="precioBaseMes" runat="server"
                                    onblur="Javascript:formatoNumero(this,2,'.',',');"
                                    onkeypress="return numerosDec()" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="ivaPlan" runat="server" Text="ID" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="inscrip" runat="server"
                                    onblur="Javascript:formatoNumero(this,2,'.',',');"
                                    onkeypress="return numerosDec()" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">&nbsp;</td>
                            <td>
                                <asp:Button ID="addPlan" runat="server" Style="height: 26px; font-size: small;"
                                    Text="+ Agregar" />
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style3">&nbsp;</td>
                            <td>
                                <asp:Button ID="editPlan" runat="server" Text="(..) Modificar" />
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style3">&nbsp;</td>
                            <td>
                                <asp:Button ID="delPlan" runat="server" Text="- Eliminar"
                                    OnClientClick="return confirm('¿Esta seguro de eliminar este registro?');" />
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <br />
                    <div style="height: 25px; width: 555px; margin: 0; padding: 0">
                        <table id="Table1" bgcolor="#EDEDED" border="1" cellpadding="0"
                            cellspacing="0" rules="all" style="border-collapse: collapse; height: 100%;">
                            <tr>
                                <td style="width: 79px; text-align: center"></td>
                                <td style="width: 35px; text-align: center">ID</td>
                                <td style="width: 100px; text-align: center">Fecha</td>
                                <td style="width: 150px; text-align: center">Plan</td>
                                <td style="width: 80px; text-align: center">PrecioBaseMes</td>
                                <td style="width: 35px; text-align: center">Iva</td>
                                <td style="width: 80px; text-align: center">Inscripcion</td>
                            </tr>
                        </table>
                    </div>
                    <input type="hidden" id="scrollPos3" runat="server" value="0" />
                    <div style="overflow: auto; height: 200px; width:100%" runat="server" id="divScroll3" onscroll="javascript:document.getElementById('scrollPos3').value = document.getElementById('divScroll3').scrollTop;">
                        <asp:GridView ID="GridView3" runat="server"
                            AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False"
                            DataKeyNames="fecha,elplan" DataSourceID="SqlDataSource3"
                            ShowHeader="False" Width="555px">
                            <AlternatingRowStyle BackColor="#C2D69B" />
                            <SelectedRowStyle BackColor="#990000" Font-Bold="false" ForeColor="white" />
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" ItemStyle-Width="75" />
                                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False"
                                    ReadOnly="True" SortExpression="id" ItemStyle-Width="30" />
                                <asp:BoundField DataField="fecha" HeaderText="fecha" ReadOnly="True"
                                    SortExpression="fecha" ItemStyle-Width="100" />
                                <asp:BoundField DataField="elplan" HeaderText="elplan" ReadOnly="True"
                                    SortExpression="elplan" ItemStyle-Width="150" />
                                <asp:BoundField DataField="precioBaseMes" HeaderText="precioBaseMes"
                                    SortExpression="precioBaseMes" ItemStyle-Width="80" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="iva" HeaderText="iva" SortExpression="iva" ItemStyle-Width="40" DataFormatString="{0:N}" />
                                <asp:BoundField DataField="inscrip" HeaderText="inscrip" ItemStyle-Width="80"
                                    SortExpression="inscrip" DataFormatString="{0:C}" />
                            </Columns>
                            <HeaderStyle BackColor="#EDEDED" Height="26px" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                            ConnectionString="<%$ ConnectionStrings:ideConnectionString2 %>"
                            SelectCommand="SELECT [id], [fecha], [elplan], [precioBaseMes], [iva], [inscrip] FROM [planes] ORDER BY [id]"></asp:SqlDataSource>
                    </div>
                    <br />
                    &nbsp;
                </asp:Panel>
            </asp:View>
            <asp:View ID="View4" runat="server">
                <asp:Panel ID="Panel4" runat="server"
                    Style="position: relative; top: 25px; left: 0px; height: 523px; width: 741px;"
                    BorderColor="#006666" BorderStyle="Solid" BorderWidth="1px">
                    &nbsp;
                                    <asp:Label ID="Label10" runat="server" ForeColor="Maroon"
                                        Style="font-weight: 900; font-size: large;" Text="DESCUENTOS"></asp:Label>
                    <br />
                    &nbsp;&nbsp;&nbsp;
                                    <br />
                    <br />
                    <table class="style1">
                        <tr>
                            <td class="style22">
                                <span class="style19">ID</span></td>
                            <td>
                                <i><b>Código</b></i></td>
                            <td>
                                <strong><em>Caduca</em></strong></td>
                            <td>
                                <strong><em>FechaCaducidad&nbsp; </em></strong></td>
                            <td>
                                <i><b>Porcentaje</b></i></td>
                            <td class="style18">Tipo</td>
                            <td>
                                <i><b>Plan</b></i></td>
                            <td>
                                <i><b>Inscrip.Gratis</b></i></td>
                            <td>
                                <strong><em>$Inscrip</em></strong></td>
                            <td>
                                <i><b>Regularizaciones</b></i></td>
                            <td>
                                <i><b>Anticipadas</b></i></td>
                            <td>
                                <i><b>#Declaraciones</b></i></td>
                            <td>
                                <i><b>#Meses</b></i></span></td>
                            <td>
                                <strong><em>IdPreRquisito</em></strong></td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:Label ID="idDescto" runat="server" Text="ID" CssClass="style21"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="cod" runat="server" MaxLength="70"
                                    Width="209px" CssClass="style21"></asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="Caduca" runat="server" CssClass="style21" />
                            </td>
                            <td style="margin-left: 40px">
                                <asp:TextBox ID="fechaCaducidad" runat="server" MaxLength="10"
                                    Width="104px" CssClass="style21"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="desctoPorcen" runat="server"
                                    onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');"
                                    onkeypress="return numerosDec()" Width="56px" CssClass="style21"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList ID="tipo" runat="server" CssClass="style21">
                                    <asp:ListItem Value="VACIO">VACIO</asp:ListItem>
                                    <asp:ListItem Value="PROMO">PROMOCION</asp:ListItem>
                                    <asp:ListItem Value="REF">REFERENCIA</asp:ListItem>
                                    <asp:ListItem Value="REG">REGULARIZACION</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="plan" runat="server" CssClass="style21">
                                    <asp:ListItem Value="VACIO">VACIO</asp:ListItem>
                                    <asp:ListItem Value="BASICO">BASICO</asp:ListItem>
                                    <asp:ListItem Value="CEROS">CEROS</asp:ListItem>
                                    <asp:ListItem Value="PREMIUM">PREMIUM</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="inscripGratis" runat="server" CssClass="style21" />
                            </td>
                            <td>
                                <asp:TextBox ID="inscripMonto" runat="server"
                                    onkeypress="return numerosDec()" Width="56px" ToolTip="antes de iva"
                                    CssClass="style21"></asp:TextBox>
                            </td>
                            <td>
                                <span class="style21">
                                    <asp:CheckBox ID="regularizacion" runat="server" />
                                </span>
                            </td>
                            <td>
                                <asp:CheckBox ID="anticipadas" runat="server" />
                                </span>
                            </td>
                            <td>
                                <asp:TextBox ID="nDeclContratadas" runat="server"
                                    onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');"
                                    onkeypress="return numerosDec()" Width="30px" CssClass="style21"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="duracionMeses" runat="server"
                                    onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');"
                                    onkeypress="return numerosDec()" Width="30px" CssClass="style21"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="idPreRequisito" runat="server"
                                    onkeypress="return numerosDec()" Width="30px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style20">&nbsp;</td>
                            <td>
                                <asp:Button ID="addDescto" runat="server" Style="height: 26px;"
                                    Text="+ Agregar" CssClass="style21" />
                            </td>
                            <td>
                                <span class="style19"></span></td>
                            <td>
                                <span class="style16"></span></td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td></span></td>
                            <td></span></td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style20">&nbsp;</td>
                            <td>
                                <asp:Button ID="editDescto" runat="server" Text="(..) Modificar"
                                    CssClass="style21" />
                            </td>
                            <td>
                                <span class="style19"></span></td>
                            <td>
                                <span class="style16"></span></td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td></span></td>
                            <td></span></td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style20">&nbsp;</td>
                            <td>
                                <asp:Button ID="delDescto" runat="server" Text="- Eliminar"
                                    OnClientClick="return confirm('¿Esta seguro de eliminar este registro?');"
                                    CssClass="style21" />
                            </td>
                            <td>
                                <span class="style19"></span></td>
                            <td>
                                <span class="style16"></span></td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td></span></td>
                            <td></span></td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <br />
                    <div style="height: 50px; width: 1295px; margin: 0; padding: 0">
                        <table id="Table2" bgcolor="#EDEDED" border="1" cellpadding="0"
                            cellspacing="0" rules="all" style="border-collapse: collapse; height: 100%;">
                            <tr>
                                <td style="width: 75px; text-align: center"></td>
                                <td style="width: 60px; text-align: center">
                                    <span class="style16">ID</span></td>
                                <td style="width: 200px; text-align: center">Código</td>
                                <td style="width: 80px; text-align: center">Caduca</td>
                                <td style="width: 80px; text-align: center">Fecha Caducidad</td>
                                <td style="width: 80px; text-align: center">Porcentaje</td>
                                <td style="width: 80px; text-align: center">Tipo</td>
                                <td style="width: 80px; text-align: center">Plan</td>
                                <td style="width: 80px; text-align: center">Inscrip Gratis</td>
                                <td style="width: 80px; text-align: center">Regularizaciones</td>
                                <td style="width: 80px; text-align: center">Anticipadas</td>
                                <td style="width: 80px; text-align: center">nDecl</td>
                                <td style="width: 80px; text-align: center">nMeses</td>
                                <td style="width: 80px; text-align: center">id Pre Requisito</td>
                                <td style="width: 80px; text-align: center">Inscrip Monto</span></td>
                            </tr>
                        </table>
                    </div>
                    <input type="hidden" id="scrollPos4" runat="server" value="0" />
                    <div style="overflow: auto; height: 400px; width:100%" runat="server" id="divScroll4" onscroll="javascript:document.getElementById('scrollPos4').value = document.getElementById('divScroll4').scrollTop;">
                        <asp:GridView ID="GridView4" runat="server"
                            AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False"
                            DataKeyNames="porcen" DataSourceID="SqlDataSource4"
                            ShowHeader="False" Height="229px" Style="font-size: small">
                            <AlternatingRowStyle BackColor="#C2D69B" />
                            <SelectedRowStyle BackColor="#990000" Font-Bold="false" ForeColor="white" />
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" ItemStyle-Width="75" />
                                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False"
                                    ItemStyle-Width="60" ReadOnly="True" SortExpression="id"></asp:BoundField>
                                <asp:BoundField DataField="cod" HeaderText="cod" ItemStyle-Width="200"
                                    ReadOnly="True" SortExpression="cod"></asp:BoundField>
                                <asp:BoundField DataField="caduca" HeaderText="caduca" ItemStyle-Width="80"
                                    ReadOnly="True" SortExpression="caduca"></asp:BoundField>
                                <asp:BoundField DataField="fechaCaducidad" HeaderText="fechaCaducidad" ItemStyle-Width="80"
                                    ReadOnly="True" SortExpression="fechaCaducidad" DataFormatString="{0:d}"></asp:BoundField>
                                <asp:BoundField DataField="porcen" HeaderText="porcen" ItemStyle-Width="80"
                                    ReadOnly="True" SortExpression="porcen" DataFormatString="{0:N}"></asp:BoundField>
                                <asp:BoundField DataField="tipo" HeaderText="tipo" ItemStyle-Width="80"
                                    ReadOnly="True" SortExpression="tipo"></asp:BoundField>
                                <asp:BoundField DataField="elplan" HeaderText="plan" ItemStyle-Width="80"
                                    ReadOnly="True" SortExpression="elplan"></asp:BoundField>
                                <asp:BoundField DataField="inscripGratis" HeaderText="inscripGratis" ItemStyle-Width="80"
                                    ReadOnly="True" SortExpression="inscripGratis"></asp:BoundField>
                                <asp:BoundField DataField="regularizacion" HeaderText="regularizacion" ItemStyle-Width="80"
                                    ReadOnly="True" SortExpression="regularizacion"></asp:BoundField>
                                <asp:BoundField DataField="anticipadas" HeaderText="anticipadas" ItemStyle-Width="80"
                                    ReadOnly="True" SortExpression="anticipadas"></asp:BoundField>
                                <asp:BoundField DataField="nDeclContratadas" HeaderText="nDeclContratadas" ItemStyle-Width="80"
                                    ReadOnly="True" SortExpression="nDeclContratadas" DataFormatString="{0:N}"></asp:BoundField>
                                <asp:BoundField DataField="duracionMeses" HeaderText="duracionMeses" ItemStyle-Width="80"
                                    ReadOnly="True" SortExpression="duracionMeses" DataFormatString="{0:N}"></asp:BoundField>
                                <asp:BoundField DataField="idPreRequisito" HeaderText="idPreRequisito" ItemStyle-Width="80"
                                    ReadOnly="True" SortExpression="idPreRequisito" DataFormatString="{0:N}"></asp:BoundField>
                                <asp:BoundField DataField="inscripMonto" HeaderText="inscripMonto" ItemStyle-Width="80"
                                    ReadOnly="True" SortExpression="inscripMonto" DataFormatString="{0:N}"></asp:BoundField>
                            </Columns>
                            <HeaderStyle BackColor="#EDEDED" Height="26px" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                            ConnectionString="<%$ ConnectionStrings:ideConnectionString2 %>"
                            SelectCommand="SELECT * FROM [desctos] ORDER BY [id]"></asp:SqlDataSource>
                    </div>
                    <br />
                    &nbsp;<asp:Label ID="desctoNregs" runat="server"></asp:Label>
                </asp:Panel>
            </asp:View>
            <asp:View ID="View5" runat="server">

                <br />
                <br />
                <br />
                <span class="style15admon"><strong>Prospectos y Asesorias<br />
                </strong></span>
                <br />
                <br />
                ID
                        <asp:Label ID="prosId" runat="server">ID</asp:Label>
                &nbsp; Estatus
                        <asp:DropDownList ID="prosEstatus" runat="server" ToolTip="VAcio, NOtificado, LLamado">
                            <asp:ListItem Value="VA">VA</asp:ListItem>
                            <asp:ListItem Value="NO">NO</asp:ListItem>
                            <asp:ListItem Value="LL">LL</asp:ListItem>
                        </asp:DropDownList>
                &nbsp;<asp:CheckBox ID="factTx" runat="server" Text="Factura enviada" />
                <asp:Button ID="prosModEstatus" runat="server" Text="Modificar elegido" />
                <br />
                <br />
                correo:
                        <asp:TextBox ID="correo" runat="server" Width="198px"></asp:TextBox>
                <asp:Button ID="enviarAsesoria" runat="server" Text="Enviarle asesoría" OnClientClick="return confirm('¿Ya confirmó el pago de este correo?');" />
                <br />
                <br />
                <br />
                <asp:TextBox ID="prosTextoNotificar" runat="server" Height="101px"
                    TextMode="MultiLine" Width="487px">Estimado visitante a declaracioneside.com, agradecemos tu interés en informarte respecto al IDE y en consultar nuestros servicios, será un placer poder servirte, te invitamos a probar nuestros servicios, consulta nuestros planes (sin olvidar los descuentos que tenemos para ti) en declaracioneside.com/planes.aspx Registrate para poder comenzar a enviar tus declaraciones de IDE en declaracioneside.com/registro.aspx Consulta nuestra sección de preguntas frecuentes en declaracioneside.com/preguntas.aspx Si deseas que te llamemos envianos un correo a declaracioneside@gmail.com o comunicate con nosotros al tel. (443) 690 3616, Si conoces a mas instituciones que recauden IDE haz equipo con nosotros como distribuidor en declaracioneside.com/distribuidores.aspx y al registrarte como tal obtendras comisiones por las instituciones que logres afiliar</asp:TextBox>
                <asp:Button ID="notificar" runat="server" Text="Notificarles" />
                <br />
                <br />
                <table id="Table3" bgcolor="#EDEDED" border="1" cellpadding="0" cellspacing="0"
                    rules="all" style="border-collapse: collapse; height: 100%;">
                    <tr>
                        <td style="width: 75px; text-align: center"></td>
                        <td style="width: 60px; text-align: center">ID</td>
                        <td style="width: 150px; text-align: center">Nombre</td>
                        <td style="width: 150px; text-align: center">Correo</td>
                        <td style="width: 150px; text-align: center">Tels</td>
                        <td style="width: 50px; text-align: center">Estatus</td>
                        <td style="width: 50px; text-align: center">Motivo</td>
                        <td style="width: 100px; text-align: center">EstadoAsesoria</td>
                        <td style="width: 100px; text-align: center">FacturaEnviada</td>
                    </tr>
                </table>
                <input type="hidden" id="scrollPos5" runat="server" value="0" />
                <div style="overflow: auto; height: 672px; width:100%" runat="server"
                    id="divScroll5"
                    onscroll="javascript:document.getElementById('scrollPos5').value = document.getElementById('divScroll5').scrollTop;">
                    <asp:GridView ID="GridView5" runat="server"
                        AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False"
                        DataSourceID="SqlDataSource5" ShowHeader="False" Height="547px"
                        Width="887px">
                        <AlternatingRowStyle BackColor="#C2D69B" />
                        <SelectedRowStyle BackColor="#990000" Font-Bold="false" ForeColor="white" />
                        <Columns>
                            <asp:CommandField ItemStyle-Width="75" ShowSelectButton="True" />
                            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False"
                                ItemStyle-Width="60" ReadOnly="True" SortExpression="id" />
                            <asp:BoundField DataField="nombre" HeaderText="nombre" ItemStyle-Width="150"
                                ReadOnly="True" SortExpression="nombre" />
                            <asp:BoundField DataField="correo" HeaderText="correo" ItemStyle-Width="150"
                                ReadOnly="True" SortExpression="correo" />
                            <asp:BoundField DataField="tels" HeaderText="tels" ItemStyle-Width="150"
                                ReadOnly="True" SortExpression="tels" />
                            <asp:BoundField DataField="estatus" HeaderText="estatus" ItemStyle-Width="50"
                                ReadOnly="True" SortExpression="estatus" />
                            <asp:BoundField DataField="motivo" HeaderText="motivo" ItemStyle-Width="50"
                                ReadOnly="True" SortExpression="motivo" />
                            <asp:BoundField DataField="edoAsesoria" HeaderText="edoAsesoria" ItemStyle-Width="100"
                                ReadOnly="True" SortExpression="edoAsesoria" />
                            <asp:BoundField DataField="factTx" HeaderText="factTx" ItemStyle-Width="100"
                                ReadOnly="True" SortExpression="factTx" />
                        </Columns>
                        <HeaderStyle BackColor="#EDEDED" Height="26px" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource5" runat="server"
                        ConnectionString="<%$ ConnectionStrings:ideConnectionString2 %>"
                        SelectCommand="SELECT * FROM [prospectos] ORDER BY [id]"></asp:SqlDataSource>
                    <br />
                    <asp:Label ID="prosNregs" runat="server"></asp:Label>
                </div>
            </asp:View>
            <asp:View ID="View6" runat="server">
                <br />
                <br />
                <span class="style15admon"><strong>Auto-Cotizador<br />
                    <br />
                </strong></span>
                <table class="style1master">
                    <tr>
                        <td class="style17">
                            <b>Cargar excel</b></td>
                        <td>
                            </td>
                    </tr>
                    <tr>
                        <td class="style17">
                            <asp:FileUpload ID="FileUpload1" runat="server"
                                Width="273px" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="crearCot" runat="server" Text="Tx Cotizaciones" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="generico" runat="server" Text="Tx Mailing" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="campos" runat="server" Text="ListaCampos" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <span class="style17">Avance:</span>
                                    <asp:UpdatePanel ID="up" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblAvance" runat="server" />
                                            <br />
                                            <div style="width: 100px; height: 10px; border: 1px solid black; position: relative; top: 0px; left: 0px;">
                                                <div id="progressbar1" runat="server" style="width: 0px; height: 10px; background-color: green; position: relative" class="estatusstyle18">
                                                </div>
                                            </div>
                                            <asp:Label ID="statusImport" runat="server"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                            </td>
                    </tr>
                </table>
                <br />
                <br />
            </asp:View>
            <asp:View ID="ViewUuid" runat="server">
                        <span class="style15admon"><strong>UUID</strong></span><br />
                        <table style="width: 345px">
                            <tr>
                                <td class="style4">
                                    #Contrato</td>
                                <td class="style4">
                                    UUID</td>
                            </tr>
                            <tr>
                                <td class="style23">
                                    <asp:TextBox ID="uuidNumContrato" runat="server" Width="81px"></asp:TextBox>
                                </td>
                                <td class="style26">
                                    <asp:TextBox ID="uuid" runat="server" Width="340px" Columns="36" MaxLength="36"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style23" colspan="3">
                                    <asp:Button ID="uuidGuardar" runat="server" style="font-size: small;" 
                                        Text="Guardar" />
                                </td>
                            </tr>
                        </table>

                    </asp:View>
        </asp:MultiView>
    </div>

    &nbsp;
        <br />
    <br />

    <br />
    <br />

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
            if ((AsciiValue >= 48 && AsciiValue <= 57) || (AsciiValue == 8 || AsciiValue == 127 || AsciiValue == 46 || AsciiValue == 45))
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
