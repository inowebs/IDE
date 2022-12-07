<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="clienteList.aspx.vb" Inherits="WebApplication1.WebForm9" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

    <style type="text/css">
        .style3clienteList
        {
            font-size: medium;
            color: #800000;
        }
        .style10
        {
            color: #000000;
            font-size: small;
        }
                        .modalBackground
        {
            background-color: white;
            filter: alpha(opacity=90);
            opacity: 0.99;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 300px;
            height: 140px;
        }
        .scrolling {  
                position: absolute;  
            }  
              
            .gvWidthHight {  
                overflow: scroll;  
                height: 700px;  
                width: 1200px;  
            }  

        </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <ajaxToolkit:ToolkitScriptManager 
        ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <ajaxToolkit:ModalPopupExtender ID="panel1_ModalPopupExtender" runat="server" DropShadow="True" PopupControlID="Panel2"  TargetControlID="btnOculto" BackgroundCssClass="modalBackground"> </ajaxToolkit:ModalPopupExtender>                
                        <asp:Button ID="btnOculto" runat="server" Text="oculto" Height="0px" Width="0px" style = "display:none" />                
                        <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" align="center" style = "display:none">
                                        <span class="style14">Contraseña de acceso:</span> <asp:TextBox ID="pass1" runat="server" TextMode="Password"></asp:TextBox> 
                                    <asp:Button ID="ingresar" runat="server" Text="Ingresar" />
                                    <br />
                                    
                       </asp:Panel>

    <p class="style3clienteList">
        <strong>Listado de Clientes&nbsp;&nbsp;&nbsp; </strong>
        <asp:LinkButton ID="verDecl" runat="server">Ultimas decla de Todos</asp:LinkButton>
    </p>
    <asp:Panel ID="Panel3" runat="server" BorderStyle="Groove">
        <span class="style10">Cliente seleccionado</span>: 
        <asp:Label ID="nomCli" runat="server" CssClass="style9master"></asp:Label>
        &nbsp; , id=<asp:Label ID="idCli" runat="server" CssClass="style9master"></asp:Label>
        &nbsp;&nbsp;
        <br />
        Correo: 
        <asp:TextBox ID="correo" runat="server" Width="249px"></asp:TextBox>
&nbsp;<asp:Button ID="obtPass" runat="server" Text="Buscar Contraseña" />
&nbsp;&nbsp;<asp:Label ID="pass" runat="server" 
            style="color: #000000; font-size: small"></asp:Label>
        <asp:Button ID="delCta" runat="server" Text="Eliminar cuenta" BackColor="Red" />
    <p class="style3clienteList">
        <asp:DropDownList ID="edoCli" runat="server" DataSourceID="SqlDataSourceEdoCli" DataTextField="estatus" DataValueField="id">
        </asp:DropDownList>
    &nbsp;
        <asp:Button ID="modEstatus" runat="server" Text="Guardar estatus" />
        &nbsp;&nbsp;
        <asp:Button ID="Buscar" runat="server" Text="Buscar" /> &nbsp;&nbsp; <asp:Button ID="download" runat="server" Text="GetCorreos" /> <asp:Button ID="exportarexcel" runat="server" Style="font-size: x-small; font-family: Arial, Helvetica, sans-serif;" Text="export Excel" />
        <asp:SqlDataSource ID="SqlDataSourceEdoCli" runat="server" ConnectionString="<%$ ConnectionStrings:ideConnectionString %>" SelectCommand="SELECT * FROM [estatusCliente] order by id"></asp:SqlDataSource>
        &nbsp;</p>
        
                                            <br />        
    Clientes order desc
    <br />
    <asp:Label ID="nRegs" runat="server" Text="0 Registros"></asp:Label>
    <div id="agrupaGV3" class="gvWidthHight">
                                            <asp:GridView ID="GridView3" runat="server" 
                                                
        AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False" 
                                                
        DataKeyNames="correo,razonSoc,rfcDeclarante" DataSourceID="SqlDataSource3" 
                                                ShowHeader="True" Width="960px" 
        CssClass="style9master" Font-Size="X-Small" style="width:100%">
                                                <AlternatingRowStyle BackColor="#C2D69B" />
                                                <selectedrowstyle backcolor="#990000" font-bold="false" forecolor="white" />
                                                <Columns>
                                                    <asp:CommandField ShowSelectButton="True" ItemStyle-Width="75" />
                                                    <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                                                    <asp:BoundField DataField="correo" HeaderText="correo" ReadOnly="True" SortExpression="correo" />
                                                    <asp:BoundField DataField="razonSoc" HeaderText="razonSoc" ReadOnly="True" SortExpression="razonSoc"/>
                                                    <asp:BoundField DataField="rfcDeclarante" HeaderText="rfcDeclarante" SortExpression="rfcDeclarante" />
                                                    <asp:BoundField DataField="casfim" HeaderText="casfim" SortExpression="casfim"  ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="casfimOK" HeaderText="casfimOK" SortExpression="casfimOK"  ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="cartaAutorizacion" HeaderText="cartaAutorizacion" SortExpression="cartaAutorizacion"  ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="loginSAT" HeaderText="login" SortExpression="loginSAT" />
                                                    <asp:BoundField DataField="tel" HeaderText="tel" SortExpression="tel" />
                                                    <asp:BoundField DataField="contacto" HeaderText="contacto" SortExpression="contacto" />
                                                    <asp:BoundField DataField="estatus" HeaderText="estatus" SortExpression="estatus" HtmlEncode="false" />
                                                    <asp:BoundField DataField="cel" HeaderText="cel" SortExpression="cel" />
                                                    <asp:BoundField DataField="fechaSolSocketSAT" HeaderText="fechaSolSocketSAT" SortExpression="fechaSolSocketSAT"  DataFormatString="{0:d}"  ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="fechaRegistro" HeaderText="fechaRegistro" SortExpression="fechaRegistro"  DataFormatString="{0:d}"  ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="fuente" HeaderText="fuente" SortExpression="fuente" />
                                                    <asp:BoundField DataField="facCorreos" HeaderText="facCorreos" SortExpression="facCorreos" />
                                                    <asp:BoundField DataField="otrosCorreos" HeaderText="otrosCorreos" SortExpression="otrosCorreos" />
                                                    <asp:BoundField DataField="facTercero" HeaderText="facTercero" SortExpression="facTercero"  ItemStyle-HorizontalAlign="Center"/>                                                    
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button Text="Logearse" runat="server" CommandName="logearse" CommandArgument="<%# Container.DataItemIndex %>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle BackColor="#EDEDED" Height="26px" />
                                            </asp:GridView>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                                                ConnectionString="<%$ ConnectionStrings:ideConnectionString2 %>" 
                                                
        SelectCommand="SELECT c.id, correo, razonsoc, rfcdeclarante, casfim, loginSAT, tel, contacto, e.estatus, cel, fechaSolSocketSAT, fechaRegistro, fuente, facCorreos, otrosCorreos, facTercero, casfimOK=(case when casfimProvisional=1 then 'no' else 'si' end), cartaAutorizacion=(case when solSocketArch IS NULL then 'no' else 'si' end) FROM clientes c, estatusCliente e where c.idEstatus=e.id ORDER BY c.id DESC">
                                            </asp:SqlDataSource>
        </div>
                                    <br />
    
    <br />
    </asp:Panel>

            <table style="width: 345px">
                            <tr>
                                <td class="style23">
                                    x Periodo: 
                                    <asp:TextBox ID="declaDesde" runat="server" Columns="10" MaxLength="10"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="declaDesde_CalendarExtender" runat="server"
                                Enabled="True" TargetControlID="declaDesde" CssClass="MyCalendar bg-white rounded " Format="dd/MM/yyyy">
                            </ajaxToolkit:CalendarExtender>
                                    <asp:TextBox ID="declaHasta" runat="server" Columns="10" MaxLength="10"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="declaHasta_CalendarExtender" runat="server"
                                Enabled="True" TargetControlID="declaHasta" CssClass="MyCalendar bg-white rounded " Format="dd/MM/yyyy">
                            </ajaxToolkit:CalendarExtender>
                                    <asp:Button ID="declaBusPer" runat="server" style="font-size: small;" Text="Buscar" />
                                </td>
                            </tr>
                        </table>
    <br />

    <span class="style15admon"><strong>Declas y Contratos</strong></span><br />
            Desde 2022:                        
            <asp:TreeView ID="tv22" runat="server" style="font-size: small">
            </asp:TreeView>
            Hasta 2021:                        
            <asp:TreeView ID="tvDeclas" runat="server" style="font-size: small">
            </asp:TreeView>
        Mensuales sin anual:
    <asp:TreeView ID="TreeView2" runat="server" style="font-size: small">
            </asp:TreeView>

    Contratos:    
    <asp:GridView ID="GridViewContras" runat="server"
                    AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False"
                    DataKeyNames="fechaPago,periodoInicial" DataSourceID="SqlDataSourceContras" Width="100%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal" Font-Size="Small">
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
                <asp:SqlDataSource ID="SqlDataSourceContras" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ideConnectionString %>"
                    SelectCommand=""></asp:SqlDataSource>

    <br />
    Ultimas acciones clientes:
    <br />
    <div id="agrupaGV1" class="gvWidthHight">
                                            <asp:GridView ID="GridView1" runat="server" AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False" DataKeyNames="correo" DataSourceID="SqlDataSource1" ShowHeader="True" Font-Size="X-Small" style="width:100%">
                                                <AlternatingRowStyle BackColor="#C2D69B" />
                                                <selectedrowstyle backcolor="#990000" font-bold="false" forecolor="white" />
                                                <Columns>
                                                    <asp:CommandField ShowSelectButton="False" ItemStyle-Width="75" />
                                                    <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" 
                                                        ReadOnly="True" SortExpression="id" />
                                                    <asp:BoundField DataField="correo" HeaderText="correo" ReadOnly="True" 
                                                        SortExpression="correo" />
                                                    <asp:BoundField DataField="razonSoc" HeaderText="razonSoc" ReadOnly="True" 
                                                        SortExpression="razonSoc"/>
                                                    <asp:BoundField DataField="tel" HeaderText="tel" SortExpression="tel" />
                                                    <asp:BoundField DataField="contacto" HeaderText="contacto" SortExpression="contacto" />
                                                    <asp:BoundField DataField="faccorreos" HeaderText="facCorreos" SortExpression="faccorreos" />
                                                    <asp:BoundField DataField="otroscorreos" HeaderText="otrosCorreos" SortExpression="otroscorreos" />
                                                    <asp:BoundField DataField="idContra" HeaderText="ultContra" SortExpression="idContra" ItemStyle-HorizontalAlign="Right"/>
                                                    <asp:BoundField DataField="precioNetoContrato" HeaderText="neto" SortExpression="precioNetoContrato" HtmlEncode="false" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"/>
                                                    <asp:BoundField DataField="pu" HeaderText="pu" SortExpression="pu" DataFormatString="{0:c}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"/>
                                                    <asp:BoundField DataField="puSinIVA" HeaderText="puSinIVA" SortExpression="puSinIVA" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right"/>
                                                    <asp:BoundField DataField="ejercicio" HeaderText="ejercicio" SortExpression="ejercicio" ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="periodoinicial" HeaderText="periodoIni" SortExpression="periodoinicial"  DataFormatString="{0:d}"/>
                                                    <asp:BoundField DataField="elestatus" HeaderText="Estatus" SortExpression="elestatus" HtmlEncode="false" />
                                                    <asp:BoundField DataField="pendsPcorriente" HeaderText="pendsPcorriente" SortExpression="pendsPcorriente" ItemStyle-HorizontalAlign="Right"/>
                                                    <asp:BoundField DataField="ultMensAn" HeaderText="ultMensAn" SortExpression="ultMensAn" ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="nDeclContratadas" HeaderText="contratadas" SortExpression="nDeclContratadas"  ItemStyle-HorizontalAlign="Right"/>
                                                    <asp:BoundField DataField="nDeclHechas" HeaderText="hechas" SortExpression="nDeclHechas"  ItemStyle-HorizontalAlign="Right"/>
                                                    <asp:BoundField DataField="elplan" HeaderText="plan" SortExpression="elplan" HtmlEncode="false" HeaderStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="fechaPago" HeaderText="fechaPago" SortExpression="fechaPago"  DataFormatString="{0:d}"/>
                                                    <asp:BoundField DataField="postpago" HeaderText="postpago" SortExpression="postpago" ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="uuid" HeaderText="uuid" SortExpression="uuid" ItemStyle-HorizontalAlign="Left"/>                                                                                                        
                                                </Columns>
                                                <HeaderStyle BackColor="#EDEDED" Height="26px" />
                                            </asp:GridView>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                                ConnectionString="<%$ ConnectionStrings:ideConnectionString2 %>" 
                                                
        SelectCommand="select cli.id, correo, razonsoc, tel, contacto, faccorreos, otroscorreos, idContra, nDeclContratadas,nDeclHechas,elplan,fechaPago,postpago,uuid,precioNetoContrato,ejercicio,elestatus,periodoinicial,pu,puSinIVA,pendsPcorriente=year(getdate())-ejercicio, ultMensAn=(select case (year(getdate())-ejercicio) when 0 then 'm' else 'a' end) from 
clientes cli, 
(select MAX(id) as idContra, idCliente from contratos co group by idCliente) as maxContra, 
(select precioNetoContrato,id,nDeclContratadas,idplan,fechaPago,nDeclHechas,postpago,uuid,periodoinicial,pu=precioNetoContrato/(select case nDeclContratadas when 0 then 1 else nDeclContratadas end),puSinIVA=(precioNetoContrato/(select case nDeclContratadas when 0 then 1 else nDeclContratadas end))/1.16 from contratos) as contras,
(select id, elplan from planes) as planes,
(select MAX(ejercicio) as ejercicio,idCliente from ideAnual group by idCliente ) as anual,
(select id, estatus as elestatus from estatusCliente) as estatusCli
where maxContra.idCliente=cli.id and 
contras.id=maxContra.idContra and 
planes.id=contras.idplan and 
estatusCli.id=cli.idEstatus and
anual.idCliente=cli.id order by correo">
                                            </asp:SqlDataSource>
        </div>
    <br />
    Ultimas Declas y Contras de todos:
    <br />
    <asp:TreeView ID="TreeView1" runat="server" style="font-size: small">
            </asp:TreeView>
    <br />

    <br />
                                        
</asp:Content>