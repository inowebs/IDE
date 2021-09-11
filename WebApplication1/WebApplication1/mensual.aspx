<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="mensual.aspx.vb" Inherits="WebApplication1.WebForm12" MaintainScrollPositionOnPostback="true" SmartNavigation="true" %>

<%@ Register Assembly="FastReport.Web, Version=2015.2.0.0, Culture=neutral, PublicKeyToken=db7e5ce63278458c" Namespace="FastReport.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Declaración Mensual</title>
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
    <!-- bootstrap.min css -->
    <link rel="stylesheet" href="plugins/bootstrap/dist/css/bootstrap.min.css" />
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>
        <div class="container">
            <div class="row pb-1">
                <div class="col-sm-12">
                    <h4 class="text-center">
                        <asp:Label ID="encab" runat="server"></asp:Label>
                        <asp:Label ID="redir" runat="server"></asp:Label>
                    </h4>
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-12">
                    <ul>
                        <li>Si cambia de mes, año u operación a 
        realizar, recuerde pulsar Aplicar.
                        </li>
                        <li>Después de importar, realice el pago de lo recaudado indicado abajo en &#39;impuesto pagado&#39; y 
                        luego complete datos del pago
                        </li>
                    </ul>
                </div>
            </div>
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="View1" runat="server">

                    <span class="style12">
                        <strong>Creación de Declaración, vía 
            Importar de Excel&nbsp; </span><span class="style16">
                <a href="http://youtu.be/zp6M0zIdYkc" target="_blank">Ver Videoturorial</a></span></strong></span><br class="style16" />
                    <br class="style16" />
                    <span class="style20">(Puede reimportar la información de excel tantas veces 
            como necesite antes de enviar la declaración)</span><br class="style20" />
                    <span class="style20">&nbsp;&nbsp;</span><asp:FileUpload ID="FileUpload1" runat="server"
                        ToolTip="Importar detalles declaración IDE mensual" CssClass="style20"
                        Width="360px" />
                    <span class="style20">&nbsp;&nbsp;&nbsp; </span>
                    <asp:Button ID="importMensXls" runat="server" Text="Importar ahora"
                        ToolTip="Este proceso puede tardar algunos minutos dependiendo de la cantidad de datos a importar, espere a que cambie el estatus a Importada"
                        CssClass="btn btn-sm btn-info" />
                    <span class="style20">&nbsp; </span>
                    <br class="style20" />
                    <span class="style20">Avance: </span>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div style="width: 100px; height: 10px; border: 1px solid black; position: relative">
                                <div id="progressbar" runat="server"
                                    style="width: 0px; height: 10px; background-color: green; position: relative">
                                    &nbsp;
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label ID="statusImport" runat="server" Style="font-weight: 700"
                        CssClass="style16"></asp:Label>
                    <span class="style16">&nbsp;&nbsp; </span>
                    <br class="style16" />
                    <br />
                    <asp:Label ID="lblErrImport" runat="server" CssClass="style21" Text="Errores encontrados:" Visible="False" ForeColor="#996600" Font-Size="Small"></asp:Label>
                    <asp:TextBox ID="errImport" Width="100%" runat="server" Height="113px" Rows="8" TextMode="MultiLine" Visible="False" CssClass="form-control form-control-sm"></asp:TextBox>
<%--                    <div class="row pt-1 pb-2">
                        <div class="col-sm-2 text-right">
                            
                        </div>
                        <div class="col-sm-10">
                            
                        </div>
                    </div>--%>
                    
                    <br class="style16" />
                    <asp:Button ID="ver" runat="server" Text="Ver datos y Acuses"
                        CssClass="btn btn-sm btn-info" />
                    <span class="style16">&nbsp;&nbsp;&nbsp; </span>
                    <span class="style16">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>
                    <br class="style16" />
                    <br class="style16" />
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <div class="row pb-1">
                        <div class="col-sm-12">
                            <strong>Creación de Declaración, vía Importar de XML</strong>
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-12">
                            <p>
                                * Defina en su xml el siguiente esquema&nbsp;con la ruta indicada&nbsp;&nbsp; xmlns:xsi=&#39;http://www.w3.org/2001/XMLSchema-instance&#39; xsi:noNamespaceSchemaLocation=&#39;C:\SAT\ide_20130430.xsd&#39;
                                <br />
                                (Puede reimportar la información de xml tantas veces como necesite antes de enviar la declaración, es su responsabilidad que este archivo esté estructurado con la norma del SAT, se enviará tal cual lo suba)
                            </p>
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-4">
                            <asp:FileUpload ID="FileUpload2" runat="server" ToolTip="Importar detalles declaración IDE mensual" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Button ID="importarXml" runat="server" Text="Importar ahora" CssClass="btn btn-sm btn-info" />
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-1">
                            Avance: 
                        </div>
                        <div class="col-sm-11">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div style="width: 100px; height: 10px; border: 1px solid black; position: relative">
                                        <div id="progressbarXml" runat="server"
                                            style="width: 0px; height: 10px; background-color: green; position: relative">
                                            &nbsp;
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-12">
                            <asp:Label ID="statusImportXml" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-12">
                            <asp:Button ID="verXml" runat="server" Text="Ver datos y acuses" CssClass="btn btn-sm btn-info" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="View3" runat="server">
                    <span class="style20">Creación de Declaración, vía Editar</span><br class="style16" />
                </asp:View>
                <asp:View ID="View4" runat="server">
                    <span class="style17"><strong>Creación de Declaración en Ceros y Enviar</strong></span><br class="style16" />
                    <br class="style16" />
                    <asp:Button ID="Crear" runat="server"
                        Text="Crear" CssClass="btn btn-sm btn-info" />
                    <span class="style16">&nbsp;&nbsp; </span>
                    <span class="style16">&nbsp;&nbsp;&nbsp;</span><asp:Button ID="verCeros" runat="server"
                        Text="Ir a Acuses"
                        CssClass="btn btn-sm btn-info" />
                    <span class="style16">&nbsp;&nbsp;&nbsp;</span><br class="style16" />
                </asp:View>
                <asp:View ID="View5" runat="server">
                    <span class="style17"><strong>Consulta de Declaración</strong></span><span
                        class="style16">&nbsp;&nbsp;&nbsp; &nbsp; </span>



                    <asp:Button ID="back" runat="server" Text="Regresar"
                        CssClass="btn btn-sm btn-info" Width="89px" />



                    <span class="style16">&nbsp;&nbsp;&nbsp;&nbsp; </span>
                    <asp:Button ID="bajarAcuseExcel" runat="server" Text="Bajar Acuse"
                        ToolTip="Se lleva en promedio 2-24 hrs para recibir el acuse del SAT, tras dar clic, vea el mensaje mostrado abajo en Descripción"
                        CssClass="btn btn-sm btn-info" />



                    &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="export" runat="server" CssClass="btn btn-sm btn-info"
                Text="Exportar a excel"
                ToolTip="Se lleva en promedio 2-24 hrs para recibir el acuse del SAT, tras dar clic, vea el mensaje mostrado abajo en Descripción"
                Width="117px" />
                    &nbsp; <span class="style27">Tras bajar acuse, Vea el mensaje mostrado abajo en 
            Descripción</span><br class="style16" />
                    <br class="style16" />
                    <asp:Label ID="nRegs" runat="server"
                        Text="0 Registros ordenados por nombre/razón social" Font-Size="Small"
                        CssClass="style16"></asp:Label>
                    <br class="style16" />
                    <table class="style1">
                        <tr>
                            <td class="style4">

                                <div style="overflow-y: auto; overflow-x: hidden; height: 400px;width:100%" runat="server"
                                    id="divScroll"
                                    onscroll="javascript:document.getElementById('scrollPos').value = document.getElementById('divScroll').scrollTop;">
                                    <asp:GridView ID="GridView3" runat="server"
                                        AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False"
                                        DataKeyNames="id" DataSourceID="SqlDataSource3"
                                        ShowHeader="True" Width="100%" Font-Size="10pt" CssClass="style16">
                                        <AlternatingRowStyle BackColor="#C2D69B" />
                                        <SelectedRowStyle BackColor="#990000" Font-Bold="false" ForeColor="white" />
                                        <Columns>
                                            <asp:CommandField ShowSelectButton="True" Visible="false" />
                                            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False"
                                                ReadOnly="True" SortExpression="id"
                                                ItemStyle-HorizontalAlign="Right">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="nombres" HeaderText="Nombres"
                                                ReadOnly="True" SortExpression="nombres"></asp:BoundField>
                                            <asp:BoundField DataField="ap1" HeaderText="Ap. Paterno"
                                                SortExpression="ap1"></asp:BoundField>
                                            <asp:BoundField DataField="ap2" HeaderText="Ap. Materno"
                                                ItemStyle-Width="120" SortExpression="ap2">
                                                <ItemStyle Width="120px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="razonSocial" HeaderText="Razón Social"
                                                ItemStyle-Width="200" SortExpression="razonSocial">
                                                <ItemStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="rfc"
                                                HeaderText="Rfc" ItemStyle-Width="120"
                                                SortExpression="rfc">
                                                <ItemStyle Width="120px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Dom"
                                                HeaderText="Domicilio" ItemStyle-Width="200"
                                                SortExpression="Dom">
                                                <ItemStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="telefono1"
                                                HeaderText="telefono1" ItemStyle-Width="100"
                                                SortExpression="telefono1">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="telefono2"
                                                HeaderText="telefono2" ItemStyle-Width="100"
                                                SortExpression="telefono2">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="numSocioCliente"
                                                HeaderText="# Socio / Cliente" ItemStyle-Width="100"
                                                SortExpression="numSocioCliente">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="sumaDeposEfe" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Depósitos" ItemStyle-Width="80" SortExpression="sumaDeposEfe">
                                                <ItemStyle HorizontalAlign="Right" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="exedente" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Exedente" ItemStyle-Width="80" SortExpression="exedente">
                                                <ItemStyle HorizontalAlign="Right" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="determinado" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Determinado" ItemStyle-Width="80"
                                                SortExpression="determinado">
                                                <ItemStyle HorizontalAlign="Right" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="recaudado" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Recaudado" ItemStyle-Width="80"
                                                SortExpression="recaudado">
                                                <ItemStyle HorizontalAlign="Right" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="pendienteRecaudar" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Pendiente Recaudar" ItemStyle-Width="80"
                                                SortExpression="pendienteRecaudar">
                                                <ItemStyle HorizontalAlign="Right" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="remanente" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Remanente" ItemStyle-Width="80"
                                                SortExpression="remanente">
                                                <ItemStyle HorizontalAlign="Right" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="impteSaldoPendienteRecaudar" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Saldo Pendiente Recaudar" ItemStyle-Width="80"
                                                SortExpression="impteSaldoPendienteRecaudar">
                                                <ItemStyle HorizontalAlign="Right" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="chqCajaMonto" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="ChequeCaja Monto" ItemStyle-Width="80"
                                                SortExpression="chqCajaMonto">
                                                <ItemStyle HorizontalAlign="Right" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="chqCajaMontoRecaudado" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="ChequeCaja Recaudado" ItemStyle-Width="80"
                                                SortExpression="chqCajaMontoRecaudado">
                                                <ItemStyle HorizontalAlign="Right" Width="80px" />
                                            </asp:BoundField>
                                        </Columns>
                                        <HeaderStyle BackColor="#EDEDED" Height="26px" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:ideConnectionString2 %>"
                                        SelectCommand="SELECT * FROM [ideDet]"></asp:SqlDataSource>
                                    <br />
                                    <span class="style9master" style="font-family: Arial, Helvetica, sans-serif">
                                        <cc1:WebReport ID="WebReport1" runat="server" Height="26px" PdfA="True" ShowExports="False" ShowPdfExport="False" ShowPrint="False" ShowRefreshButton="False" ShowZoomButton="False" Visible="False" Width="106px" />
                                    </span>
                                </div>
                            </td>
                            <td class="style3">&nbsp;</td>
                            <td class="style16">&nbsp;</td>
                        </tr>
                    </table>
                    <br class="style16" />
                </asp:View>

                <asp:View ID="View6" runat="server">
                    <span class="style17"><strong>Consulta de Declaración</strong></span><span
                        class="style16">&nbsp;&nbsp;&nbsp; &nbsp; </span>



                    <asp:Button ID="backXml" runat="server" Text="Regresar"
                        CssClass="btn btn-sm btn-info" />



                    <span class="style16">&nbsp;&nbsp;&nbsp;&nbsp; </span>
                    <asp:Button ID="consultarXml" runat="server" Text="Descargar xml subido"
                        CssClass="btn btn-sm btn-info" Width="141px" />
                    <span class="style16">&nbsp;&nbsp;&nbsp; </span>
                    <asp:Button ID="bajaAcuseXml" runat="server" Text="Bajar Acuse"
                        ToolTip="Se lleva en promedio 2-24 hrs para recibir el acuse del SAT"
                        CssClass="btn btn-sm btn-info" Width="89px" />



                    <br class="style16" />
                    <br class="style16" />
                </asp:View>
            </asp:MultiView>
            <div class="row pb-1">
                <div class="col-sm-12">
                    * = Datos calculados por sistema, podría editarlos después de importar.
                    
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-2 text-right">
                    Descripción:
                </div>
                <div class="col-sm-10 text-left">
                    <asp:Label ID="descrip" runat="server"></asp:Label>
                </div>
                </div>
            <div class="row pb-1">
                <div class="col-sm-2 text-right">
                    Estatus:
                </div>
                <div class="col-sm-10 text-left">
                    <asp:Label ID="estado" runat="server"></asp:Label>
                </div>
            </div>
            <div class="card">
                <div class="card-header text-center">
                    Totales del mes
                </div>
                <div class="card-body">
                    <div class="row pb-1">
                        <div class="col-sm-3 text-right">
                            Excedente
                        </div>
                        <div class="col-sm-3 ">
                            <asp:TextBox ID="impteExcedente" runat="server" onkeypress="return numerosDec()"
                                onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');" CssClass="form-control form-control-sm text-right">0</asp:TextBox>
                        </div>
                        <div class="col-sm-3 text-right">
                            Determinado
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="impteDeterminado" runat="server" onkeypress="return numerosDec()"
                                onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');" CssClass="form-control form-control-sm text-right">0</asp:TextBox>
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-3 text-right">
                            Recaudado
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="impteRecaudado" runat="server" onkeypress="return numerosDec()"
                                onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');" CssClass="form-control form-control-sm text-right">0</asp:TextBox>
                        </div>
                        <div class="col-sm-3 text-right">
                            Pendiente recaudar
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="imptePendienteRecaudar" runat="server" onkeypress="return numerosDec()"
                                onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');" CssClass="form-control form-control-sm text-right">0</asp:TextBox>
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-3 text-right">
                            Remanente
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="impteRemanente" runat="server" onkeypress="return numerosDec()" onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');" CssClass="form-control form-control-sm text-right">0</asp:TextBox>
                        </div>
                        <div class="col-sm-3 text-right">
                            * Saldo pendiente de recaudar
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="impteSaldoPendienteRecaudar" runat="server" onkeypress="return numerosDec()"
                                onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');" ToolTip="saldo pendiente acumulado del ejercicio a la fecha de presentación de la declaración informativa mensual" CssClass="form-control form-control-sm text-right">0</asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row pb-1">
                <div class="col-sm-3 text-right">
                    <asp:Label ID="lblimpteCheques" runat="server" Text="* Monto recaudado Cheques de caja"></asp:Label>
                </div>
                <div class="col-sm-3">
                    <asp:TextBox ID="impteCheques" runat="server" onkeypress="return numerosDec()"
                        onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');" ToolTip="Debe aplicarse la tasa de % de IDE correspondiente del ejercicio fiscal respectivo" CssClass="form-control form-control-sm text-right">0</asp:TextBox>
                </div>
                <div class="col-sm-3 text-right">
                    <asp:Label ID="lblFechaPresentacionAnt" runat="server" Text="Fecha presentación anterior"></asp:Label>
                </div>
                <div class="col-sm-3 text-right">
                    <asp:Label ID="fechaPresentacionAnt" runat="server" Text="0"></asp:Label>
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-3 text-right">
                    <asp:Label ID="lblNumOperAnt" runat="server" Text="# Operación anterior"></asp:Label>
                </div>
                <div class="col-sm-3 text-right">
                    <asp:Label ID="numOperAnt" runat="server" Text="0"></asp:Label>
                </div>
                <div class="col-sm-6">
                    <div class="card">
                        <div class="card-header p-0 text-center">Normalmente último dia del mes reportado</div>
                        <div class="card-body form-inline p-0">
                            <div class="col-sm-3 text-right">
                                <asp:Label ID="lblFechaCorte" runat="server" Text="Fecha corte"></asp:Label>
                            </div>
                            <div class="col-sm-3 text-right">
                                <asp:TextBox ID="fechaCorte" runat="server" MaxLength="10"
                                    ToolTip="formato dd/mm/aaaa" placeholder="Normalmente último dia del mes reportado" CssClass="form-control form-control-sm"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="fechaCorte_CalendarExtender" runat="server"
                                    Enabled="True" TargetControlID="fechaCorte" CssClass="MyCalendar bg-white rounded" Format="dd/MM/yyyy">
                                </ajaxToolkit:CalendarExtender>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class="row pb-1">
                <div class="col-sm-3 text-right">
                    Tipo
                </div>
                <div class="col-sm-3">
                    <asp:Label ID="normalComplementaria" runat="server"></asp:Label>
                </div>
                <div class="col-sm-3 text-right">
                    Representante legal
                </div>
                <div class="col-sm-3">
                    <p>
                        <asp:Label ID="idRepresentanteLegal" runat="server" Text="0"></asp:Label><asp:Label ID="RepresentanteLegal" runat="server"></asp:Label>
                    </p>
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-3 text-right">
                    Límite Ide $
                </div>
                <div class="col-sm-3 text-right">
                    <asp:Label ID="ideConfLimite" runat="server" Text="0"></asp:Label>
                </div>
                <div class="col-sm-3 text-right">
                    % Ide
                </div>
                <div class="col-sm-3 text-right">
                    <asp:Label ID="ideConfPorcen" runat="server" Text="0"></asp:Label>
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-3 text-right">
                    <asp:Label ID="lblfedFechaRecaudacion" runat="server" Text="Fecha de recaudación"></asp:Label>
                </div>
                <div class="col-sm-3">
                    <asp:TextBox ID="fedFechaRecaudacion" runat="server" MaxLength="10" ToolTip="formato dd/mm/aaaa" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>
            <div class="card">
                <div class="card-header text-center">Datos del pago (entero propio)</div>
                <div class="card-body">
                    <div class="row pb-1">
                        <div class="col-sm-3 text-right">
                            <asp:Label ID="lblfedFechaEntero" runat="server" Text="Fecha de pago"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="fedFechaEntero" runat="server" MaxLength="10" ToolTip="formato dd/mm/aaaa" CssClass="form-control form-control-sm text-right"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-3 text-right">
                            <asp:Label ID="lblfedImpto" runat="server" Text="Impuesto pagado"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="fedImpto" runat="server" onkeypress="return numerosDec()" onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');" ToolTip="Impuesto pagado(enterado) = Recaudado + Remanente + RecaudadoDeChequesDeCaja" CssClass="form-control form-control-sm text-right">0</asp:TextBox>
                        </div>
                        <div class="col-sm-3 text-right">
                            <asp:Label ID="lblfedNumOper" runat="server" Text="Núm. de Oper. bancaria o línea de captura"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="fedNumOper" runat="server" MaxLength="20" onblur="Javascript:ceros(this);" ToolTip="o bien línea de captura" CssClass="form-control form-control-sm text-right"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-3 text-right">
                            <asp:Label ID="lblenteroPropInstit" runat="server" Text="Institución de pago"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="enteroPropInstit" runat="server" MaxLength="250" onblur="Javascript:ceros(this);" ToolTip="Institución (nombre Fiscal) Bancaria (Auxiliar de la Tesofe) en la que hizo el pago del IDE que ustedes recaudaron" CssClass="form-control form-control-sm text-right"></asp:TextBox>
                        </div>
                        <div class="col-sm-3 text-right">
                            <asp:Label ID="lblenteroPropInstitRfc" runat="server" Text="Rfc Institución de pago"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="enteroPropInstitRfc" runat="server" MaxLength="12" onblur="Javascript:ceros(this);" ToolTip="RFC de la Institución Bancaria (Auxiliar de la Tesofe) en la que hizo el pago del IDE que ustedes recaudaron" CssClass="form-control form-control-sm text-right"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row pb-1">
                <div class="col-sm-3 text-right">
                    Fecha envío
                </div>
                <div class="col-sm-3">
                    <asp:Label ID="fechaEnvio" runat="server"></asp:Label>
                </div>
            </div>
            <div class="card">
                <div class="card-header text-center">Acuse</div>
                <div class="card-body">
                    <div class="row pb-1">
                        <div class="col-sm-3 text-right">
                            *Fecha presentación (acuse)
                        </div>
                        <div class="col-sm-3">
                            <asp:Label ID="fechaPresentacion" runat="server"></asp:Label>
                        </div>
                        <div class="col-sm-3 text-right">
                            * Núm. de Operación (acuse)
                        </div>
                        <div class="col-sm-3 text-right">
                            <asp:Label ID="numOper" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row pb-1">
                <div class="col-sm-3 text-right">
                    ID
                </div>
                <div class="col-sm-3">
                    <asp:Label ID="id" runat="server" Text="0"></asp:Label>
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-3 text-right">
                    idAnual
                </div>
                <div class="col-sm-3">
                    <asp:Label ID="idAnual" runat="server" Text="0"></asp:Label>
                </div>
                <div class="col-sm-3">
                    <asp:Button ID="modi" runat="server" Text="Modificar" ToolTip="Guarda los cambios de la tabla izquierda" CssClass="btn btn-sm btn-info" />
                </div>
                <div class="col-sm-3">
                    <asp:Button ID="btnEnviarDeclaracion" runat="server" Text="Enviar Declaración" ToolTip="Tras enviar, espere a que cambie el estatus" CssClass="btn btn-sm btn-info" />
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-12">
                    Después de enviarla, espere hasta recibir un mensaje, pues cada que pulse 
                    &#39;Enviar&#39; se descuenta una declaración.
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-12">
                    Si por alguna razón no pudo enviar su declaración y le urge, puede presentar 
                    directamente al SAT el archivo de contingencia.
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-3 text-right">
                    Quién presentará la contingencia:
                </div>
                <div class="col-sm-3">
                    <asp:DropDownList ID="quienContin" runat="server" CssClass="form-control form-control-sm">
                        <asp:ListItem>Proveedor</asp:ListItem>
                        <asp:ListItem>Contribuyente</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-3">
                    <asp:Button ID="btnContingencia" runat="server" Text="Descargar contingencia" CssClass="btn btn-sm btn-info"
                        ToolTip="Cuenta como un envío"
                        OnClientClick="if (confirm('¿Esta 100% seguro de descargar esta contingencia? \n Le sugerimos esperar unas horas a que se reestablezca el servidor del SAT y vuelva intentar Enviar declaración pulsando ahora Cancelar, primer alerta')==true){return confirm('¿Confirma descargar esta contingencia ahora? \n Le sugerimos esperar unas horas a que se reestablezca el servidor del SAT y vuelva intentar Enviar declaración pulsando ahora Cancelar, última alerta');}else{return false;}" />
                </div>
                <div class="col-sm-3">
                    <asp:CheckBox ID="chkPostpago" runat="server" Enabled="False" Text="Postpago" />
                </div>
            </div>
    </form>
</body>
</html>
