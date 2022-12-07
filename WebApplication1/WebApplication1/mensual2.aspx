<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="mensual2.aspx.vb" Inherits="WebApplication1.mensual2" MaintainScrollPositionOnPostback="true" SmartNavigation="true" Debug="true" %>

<%@ Register Assembly="FastReport.Web, Version=2015.2.0.0, Culture=neutral, PublicKeyToken=db7e5ce63278458c" Namespace="FastReport.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        function scrollTo(what) {
            if (what != "0")
                document.getElementById(what).scrollTop = document.getElementById("scrollPos").value;
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
    <style type="text/css">
        .scroll {
            width: 20px;
            overflow: auto;
            float: left;
            margin: 0 10px;
        }

        .scroll4::-webkit-scrollbar {
            width: 7px;
        }

        .scroll4::-webkit-scrollbar-thumb {
            background: #666;
            border-radius: 20px;
        }

        .scroll4::-webkit-scrollbar-track {
            background: #ddd;
            border-radius: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <input type="hidden" id="Hidden1" runat="server" value="0" />
        <asp:ScriptManager ID="ScriptManager1" runat="server" />    <%--no usar el ajaxToolkit:ToolkitScriptManager con updatepanel ni timer xq no lo toma--%>
        <div class="container">
            <div class="row pb-1">
                <div class="col-sm-12">
                    <h4 class="text-center">
                        <asp:Label ID="encab" runat="server"></asp:Label>
                    </h4>
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-12" style="color:darkgoldenrod">
                    - Si cambia de mes, año u operación a realizar, pulse el boton &quot;Aplicar&quot; de arriba.
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-12">
                    <asp:Label ID="redir" runat="server"></asp:Label>
                    <asp:TextBox ID="fenvio" runat="server" Visible="False" ToolTip="fechaEnvio"></asp:TextBox>
                        <asp:TextBox ID="resul" runat="server" Visible="False" ToolTip="evidenciaEnvio" ></asp:TextBox>
                        <asp:Button ID="lost" runat="server" Text="SetLost" Visible="False" />
                </div>
            </div>
        </div>

        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View1" runat="server">
                <div class="container">
                    <div class="row pb-1">
                        <div class="col-sm-12">
                            <h5>Creación de Declaración, vía Importar de Excel<a href="http://youtu.be/zp6M0zIdYkc" target="_blank"> Ver Videoturorial</a>
                            </h5>
                        </div>
                        <div class="col-sm-12">
                            <p>
                                Puede reimportar la información de excel sin costo adicional tantas veces como necesite antes de enviar la declaración. Los cheques de caja adquiridos en efectivo solo son considerados para bancos.
                            </p>
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-4">
                            <asp:FileUpload ID="FileUpload1" runat="server" ToolTip="Importar detalles declaración IDE mensual" Width="919px" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Button ID="importMensXls" runat="server" Text="Importar" ToolTip="Este proceso puede tardar algunos minutos dependiendo de la cantidad de datos a importar, espere a que cambie el estatus a Importada" CssClass="btn btn-sm btn-info" />
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-2">
                            <asp:Button ID="Button1" runat="server" Text="." Visible="False" />
                            <asp:HiddenField ID="h1" runat="server" />
                            Avance:
                        </div>
                        <div class="col-sm-10">
                            <asp:Timer ID="Timer1" runat="server" Enabled="False" Interval="1000" />
                            <asp:UpdatePanel ID="up" runat="server">
                                <ContentTemplate>                                    
                                    <asp:Label ID="lblAvance" runat="server" />
                                    <div style="width: 100px; height: 10px; border: 1px solid black; position: relative; top: 0px; left: 0px;">
                                        <div id="progressbar1" runat="server" style="width: 0px; height: 10px; background-color: green; position: relative">
                                        </div>
                                    </div>
                                    <asp:Label ID="statusImport" runat="server"
                                        CssClass="style25"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                        <div class="row pt-1 pb-2">
                            <div class="col-sm-2 text-right">
                                <asp:Label ID="lblErrImport" runat="server" CssClass="style21" Text="Errores encontrados:" Visible="False" ForeColor="#996600" Font-Size="Small"></asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <asp:TextBox ID="errImport" Width="100%" runat="server" Height="113px" Rows="8" TextMode="MultiLine" Visible="False" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                    <div class="row pb-1">
                        <div class="col-sm-12">
                            <asp:Button ID="ver" runat="server" Text="Ver datos y Acuses" CssClass="btn btn-sm btn-info" />
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="View2" runat="server">
                <span class="style35"><strong>Creación de Declaración, vía Importar de XML</strong></span><br class="style16" />
                <br class="style16" />
                <span class="style20">* Defina en su xml el siguiente esquema&nbsp;con la ruta 
            indicada&nbsp;&nbsp; xmlns:xsi=&#39;http://www.w3.org/2001/XMLSchema-instance&#39; 
            xsi:noNamespaceSchemaLocation=&#39;C:\SAT\ide_20130430.xsd&#39;</span><br class="style20" />
                <span class="style20">(Puede reimportar la información de xml tantas veces como 
            necesite antes de enviar la declaración,</span><br class="style20" />
                <span class="style20">es su responsabilidad que este archivo esté estructurado 
            con la norma del SAT, se enviará tal cual lo suba)</span><br class="style20" />
                <br class="style16" />
                <span class="style16">&nbsp;&nbsp;</span><asp:FileUpload
                    ID="FileUpload2" runat="server"
                    ToolTip="Importar detalles declaración IDE mensual" Width="761px"
                     />
                <span class="style16">&nbsp;&nbsp;&nbsp; </span>
                <asp:Button ID="importarXml" runat="server" Text="Importar"
                    CssClass="btn btn-sm btn-info" />
                <span class="style16">&nbsp;&nbsp; </span>
                <br class="style16" />
                <span class="style20">Avance: </span>
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
                <asp:Label ID="statusImportXml" runat="server" Style="font-weight: 700"
                    CssClass="style16"></asp:Label>
                <br class="style16" />
                <asp:Button ID="verXml" runat="server" Text="Ver datos y acuses"
                    CssClass="btn btn-sm btn-info" />
                <span class="style16">&nbsp;&nbsp;&nbsp; </span>
                <span class="style16">&nbsp;&nbsp;&nbsp; </span>
                <br class="style16" />
                <br class="style16" />
            </asp:View>
            <asp:View ID="View3" runat="server">
                <span class="style20">Creación de Declaración, vía Editar</span><br class="style16" />
            </asp:View>
            <asp:View ID="View4" runat="server">
                <div class="container">
                    <div class="row pb-1">
                        <div class="col-sm-4">
                            <strong>Creación de Declaración en Ceros y Enviar</strong>
                        </div>
                        <div class="col-sm-2">
                            <asp:Button ID="Crear" runat="server" Text="Crear" CssClass="btn btn-sm btn-info" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Button ID="verCeros" runat="server" Text="Ir a Acuses" CssClass="btn btn-sm btn-info" />
                        </div>
                        <div class="col-sm-4">
                        </div>
                    </div>
                </div>


            </asp:View>
            <asp:View ID="View5" runat="server">
                <div class="container">
                    <div class="row pb-1">
                        <div class="col-sm-12">Consulta de Declaración</div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-2">
                            <asp:Button ID="back" runat="server" Text="Regresar" CssClass="btn btn-sm btn-info" />
                        </div>
                        <div class="col-sm-2">                            
                            <asp:Button ID="verEvidEnvio" runat="server" CssClass="btn btn-sm btn-info" Text="Evidencia de Envio" />
                        
                            &nbsp;
                        
                            <asp:Button ID="bajarAcuseExcel" runat="server" Text="Bajar Acuse" ToolTip="Se lleva en promedio 2-24 hrs para recibir el acuse del SAT, tras dar clic, vea el mensaje mostrado abajo en Descripción" CssClass="btn btn-sm btn-info" />
                            <asp:CheckBox ID="acuSinCorr" runat="server" Text="Sin enviar correo" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Button ID="export" runat="server" Text="Exportar a excel" ToolTip="Se lleva en promedio 2-24 hrs para recibir el acuse del SAT, tras dar clic, vea el mensaje mostrado abajo en Descripción" CssClass="btn btn-sm btn-info" />
                        </div>
                        <div class="col-sm-6">
                            Tras bajar acuse, Vea el mensaje mostrado abajo en Descripción.
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-12">
                            <asp:Label ID="nRegs" runat="server" Text="0 Registros ordenados por nombre/razón social" Font-Size="Small"></asp:Label>
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-12 scroll scroll4" style="max-height: 400px; width:100%">
                            <asp:GridView ID="GridView3" runat="server"
                                AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False"
                                DataKeyNames="id" DataSourceID="SqlDataSource3" Width="100%" Font-Size="Small" CssClass="style16" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="white" />
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" Visible="false"></asp:CommandField>
                                    <asp:BoundField DataField="id" HeaderText="ID" InsertVisible="False"
                                        ReadOnly="True" SortExpression="id"
                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="nombres" HeaderText="Nombres"
                                        ReadOnly="True" SortExpression="nombres" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ap1" HeaderText="Ap. Paterno"
                                        SortExpression="ap1" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ap2" HeaderText="Ap. Materno"
                                        SortExpression="ap2" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="razonSocial" HeaderText="Razón Social"
                                        SortExpression="razonSocial" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="rfc"
                                        HeaderText="RFC"
                                        SortExpression="rfc" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Dom"
                                        HeaderText="Domicilio"
                                        SortExpression="Dom" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="telefono1"
                                        HeaderText="Tel1"
                                        SortExpression="telefono1" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="telefono2"
                                        HeaderText="Tel2"
                                        SortExpression="telefono2" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="numSocioCliente"
                                        HeaderText="No. Socio (Cliente)"
                                        SortExpression="numSocioCliente" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sumaDeposEfe" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Depósitos" SortExpression="sumaDeposEfe" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="exedente" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Exedente" SortExpression="exedente" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="chqCajaMonto" DataFormatString="{0:N}" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Cheques Caja" ItemStyle-Width="80"
                                        SortExpression="chqCajaMonto" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
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
                                ConnectionString="<%$ ConnectionStrings:ideConnectionString2 %>"
                                SelectCommand="SELECT * FROM [ideDet]"></asp:SqlDataSource>

                        </div>
                    </div>
                    <cc1:WebReport ID="WebReport1" runat="server" Height="27px" PdfA="True" ShowExports="False" ShowPdfExport="False" ShowPrint="False" ShowRefreshButton="False" ShowZoomButton="False" Visible="False" Width="104px" />
                </div>
            </asp:View>

            <asp:View ID="View6" runat="server">
                <div class="container">
                    <div class="row pb-1">
                        <div class="col-sm-2">
                            Consulta de Declaración
                        </div>
                        <div class="col-sm-2">
                            <asp:Button ID="backXml" runat="server" Text="Regresar" CssClass="btn btn-sm btn-info" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Button ID="consultarXml" runat="server" Text="Descargar xml subido" CssClass="btn btn-sm btn-info" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Button ID="bajaAcuseXml" runat="server" Text="Bajar Acuse" ToolTip="Se lleva en promedio 2-24 hrs para recibir el acuse del SAT" CssClass="btn btn-sm btn-info" />
                        </div>
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
        <div class="container">
            <div class="row pb-1">
                <div class="col-sm-12">
                    * = Datos calculados por sistema, podría editarlos después de importar.
                    
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-2 text-right">
                    Descripción:
                </div>
                <div class="col-sm-10 text-left" style="color:darkgoldenrod">
                    <asp:Label ID="lbldescrip" runat="server"></asp:Label>
                </div>
                </div>
            <div class="row pb-1">
                <div class="col-sm-2 text-right">
                    Estatus:
                </div>
                <div class="col-sm-10 text-left" style="color:darkgoldenrod">
                    <asp:Label ID="estado" runat="server" ></asp:Label>
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-6">
                    <div class="card">
                        <div class="card-header p-0 text-center">
                            Totales del mes
                        </div>
                        <div class="card-body p-0 form-inline">
                            <div class="col-sm-6 text-right">
                                *Excedente en efectivo
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox ID="impteExcedente" runat="server" onkeypress="return numerosDec()" onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');" CssClass="form-control form-control-sm">0</asp:TextBox>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="col-sm-6">
                    <div class="card">
                        <div class="card-header p-0 text-center">Normalmente es el último dia del mes reportado</div>
                        <div class="card-body form-inline p-0">
                            <div class="col-sm-3 text-right ">

                                <asp:Label ID="lblFechaCorte" runat="server" Text="Fecha corte"></asp:Label>
                            </div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="fechaCorte" runat="server" MaxLength="10"
                                    ToolTip="formato dd/mm/aaaa" CssClass="form-control form-control-sm" placeholder="Normalmente es el último dia del mes reportado"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="fechaCorte_CalendarExtender" runat="server"
                                    Enabled="True" TargetControlID="fechaCorte" CssClass="MyCalendar bg-white rounded" Format="dd/MM/yyyy">
                                </ajaxToolkit:CalendarExtender>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-3 text-right ">
                    <asp:Label ID="lblFechaPresentacionAnt" runat="server"
                        Text="Fecha presentación anterior" CssClass="style20"></asp:Label>
                </div>
                <div class="col-sm-3 text-right">
                    <asp:Label ID="fechaPresentacionAnt" runat="server" Text="0" Style="text-align: right"
                        Width="150px" CssClass="style20"></asp:Label>
                </div>
                <div class="col-sm-3 ">
                    <asp:Label ID="lblNumOperAnt" runat="server" Text="# Operación anterior"
                        CssClass="style20"></asp:Label>
                </div>
                <div class="col-sm-3">
                    <asp:Label ID="numOperAnt" runat="server" Text="0" Style="text-align: right"
                        Width="150px" CssClass="style20"></asp:Label>
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
                    <asp:Label ID="idRepresentanteLegal" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="RepresentanteLegal" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-3 text-right">
                    Monto Límite de efectivo
                </div>
                <div class="col-sm-3">
                    <asp:Label ID="ideConfLimite" runat="server" Text="0" CssClass="form-control form-control-sm"></asp:Label>
                </div>
                <div class="col-sm-3 text-right">
                    Fecha envío
                </div>
                <div class="col-sm-3">
                    <asp:Label ID="fechaEnvio" runat="server"></asp:Label>
                </div>
            </div>
            <div class="card">
                <div class="card-header p-1 text-center">Acuse</div>
                <div class="card-body p-1">
                    <div class="row pb-1">
                        <div class="col-sm-3 text-right">
                            * Fecha presentación
                        </div>
                        <div class="col-sm-3">
                            <asp:Label ID="fechaPresentacion" runat="server"></asp:Label>
                        </div>
                        <div class="col-sm-3 text-right">
                            * Núm. de Operación
                        </div>
                        <div class="col-sm-3">
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
                <div class="col-sm-3 text-right">
                    idAnual
                </div>
                <div class="col-sm-3">
                    <asp:Label ID="idAnual" runat="server" Text="0"></asp:Label>
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-3">
                    <asp:Button ID="modi" runat="server" Text="Validar" ToolTip="Guarda los cambios de la tabla izquierda" CssClass="btn btn-sm btn-info" />
                </div>
                <div class="col-sm-3">
                    <asp:Button ID="btnEnviarDeclaracion" runat="server"
                        Text="Enviar Declaración"
                        ToolTip="Tras enviar, espere a que cambie el estatus" CssClass="btn btn-sm btn-info" />
                    <br />
                            <asp:CheckBox ID="chkSinCorreo" runat="server" Text="sin enviar correo cliente" />
                </div>
                <div class="col-sm-6">
                    <div class="alert alert-warning">
                        Después de enviarla, espere hasta recibir un mensaje, pues 
                    cada que pulse &#39;Enviar&#39; se descuenta una declaración.
                    </div>
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-12">
                    <div class="alert alert-warning">
                        Si por alguna razón no pudo enviar su declaración y le urge, puede presentar 
                    directamente al SAT el archivo de contingencia<br />
                    </div>
                </div>
                <div class="col-sm-3">
                    Quién presentará la contingencia:
                </div>
                <div class="col-sm-3">
                    <asp:DropDownList ID="quienContin" runat="server" CssClass="form-control form-control-sm">
                        <asp:ListItem>Proveedor</asp:ListItem>
                        <asp:ListItem>Contribuyente</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-6">
                    <asp:Button ID="btnContingencia" runat="server" Text="Descargar contingencia"
                        ToolTip="Cuenta como un envío"
                        OnClientClick="if (confirm('¿Esta 100% seguro de descargar esta contingencia? \n Le sugerimos esperar unas horas a que se reestablezca el servidor del SAT y vuelva intentar Enviar declaración pulsando ahora Cancelar, primer alerta')==true){return confirm('¿Confirma descargar esta contingencia ahora? \n Le sugerimos esperar unas horas a que se reestablezca el servidor del SAT y vuelva intentar Enviar declaración pulsando ahora Cancelar, última alerta');}else{return false;}" CssClass="btn btn-sm btn-info" />
                    <asp:CheckBox ID="chkPostpago" runat="server" Enabled="False" Text="Postpago" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
