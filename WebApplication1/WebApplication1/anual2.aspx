﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="anual2.aspx.vb" Inherits="WebApplication1.anual2" MaintainScrollPositionOnPostback="true" SmartNavigation="true" %>

<%@ Register Assembly="FastReport.Web, Version=2015.2.0.0, Culture=neutral, PublicKeyToken=db7e5ce63278458c" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Declaración Anual</title>
    <link rel="Stylesheet" href="plugins/bootstrap/dist/css/bootstrap.min.css" />
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
        function f() {
            var timer = $find(" <%=Timer1.ClientID %>");
            timer._stopTimer();
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

</head>
<body>
    <form id="form1" runat="server">
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
                    - Si cambia de año u operación a realizar, recuerde pulsar Aplicar.<asp:Label ID="redir" runat="server"></asp:Label>                         
                        <asp:TextBox ID="fenvio" runat="server" Visible="False" ToolTip="fechaEnvio" ></asp:TextBox>
                        <asp:TextBox ID="resul" runat="server" Visible="False" ToolTip="evidenciaEnvio"></asp:TextBox>
                        <asp:Button ID="lost" runat="server" Text="SetLost" Visible="False" />
                                                                                       
                </div>
            </div>
        </div>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View1" runat="server">
                <asp:HiddenField ID="h1" runat="server" />
                <asp:HiddenField ID="ejercicio" runat="server" />
                <asp:HiddenField ID="idContrato" runat="server" />
                <asp:HiddenField ID="pl" runat="server" />
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:Timer ID="Timer1" runat="server" Interval="1000" Enabled="False" />
                <div class="container">
                    <div class="row">
                        <div class="col-sm-12">
                            <h5>Creación de Declaración, vía Importar de Excel <a href="http://youtu.be/H9jjSI-oZAY" target="_blank">Ver Videotutorial</a></h5>
                            <p>
                                (Puede reimportar la información de excel sin costo adicional tantas veces 
            como necesite antes de enviar la declaración)
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="row col-sm-12">
                                <asp:FileUpload ID="FileUpload1" runat="server" ToolTip="Importar detalles declaración IDE anual" Width="900px" />
                                <br />
                                <asp:Button ID="b1" runat="server" Text="Button" Visible="False" />
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
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
                                </div>

                            </div>

                        </div>
                        <div class="col-sm-4">
                            <asp:Button ID="importMensXls" runat="server" CssClass="btn btn-sm btn-info"
                                Text="Importar"
                                ToolTip="Este proceso puede tardar algunos minutos dependiendo de la cantidad de datos a importar, espere a que cambie el estatus a Importada" />
                        </div>
                    </div>
                    <%--bloque de errores--%>
                    <div class="row pt-1 pb-2">
                        <div class="col-sm-2 text-right">
                            <asp:Label ID="lblErrImport" runat="server" CssClass="style21" Text="Errores encontrados:" Visible="False" ForeColor="#996600" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="col-sm-10">
                            <asp:TextBox ID="errImport" Width="100%" runat="server" Height="113px" Rows="8" TextMode="MultiLine" Visible="False" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <asp:Button ID="ver" runat="server" Text="Ver datos y Acuses" CssClass="btn btn-sm btn-info" />
                        </div>
                    </div>

                </div>

            </asp:View>
            <asp:View ID="View2" runat="server">
                <div class="container">
                    <div class="row">
                        <div class="col-sm-12">
                            <h5>Creación de Declaración, vía Importar de XML</h5>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <p>
                                * Defina en su xml el siguiente esquema&nbsp;con la ruta 
            indicada&nbsp;&nbsp; xmlns:xsi=&#39;http://www.w3.org/2001/XMLSchema-instance&#39; 
            xsi:noNamespaceSchemaLocation=&#39;C:\SAT\ide_20130430.xsd&#39;
                            </p>
                            <p>
                                (Puede reimportar la información de xml tantas veces como 
            necesite antes de enviar la declaración,es su responsabilidad que este archivo esté estructurado  con la norma del SAT, se enviará tal cual lo suba)
                            </p>
                        </div>
                    </div>
                    <div class="row pb-2">
                        <div class="col-sm-4">
                            <asp:FileUpload
                                ID="FileUpload2" runat="server"
                                ToolTip="Importar detalles declaración IDE anual" Width="955px" />
                        </div>
                        <div class="col-sm-4">
                            <asp:Button ID="importarXml" runat="server" CssClass="btn btn-sm btn-info btn-block" Text="Importar ahora" />
                        </div>
                        <div class="col-sm-4">
                            <div class="row">
                                <div class="col-sm-12"><span class="style17">Avance:</span></div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
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
                                <div class="col-sm-6">
                                    <asp:Label ID="statusImportXml" runat="server" Style="font-weight: 700"
                                        CssClass="style18"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row pb-2">
                        <div class="col-sm-12">
                            <asp:Button ID="verXml" runat="server" Text="Ver datos y acuses" CssClass="btn btn-sm btn-info" />
                        </div>
                    </div>
                </div>

            </asp:View>
            <asp:View ID="View3" runat="server">
                <span class="style18">Creación de Declaración, vía Editar</span><br class="style18" />
            </asp:View>
            <asp:View ID="View4" runat="server">
                <div class="container">
                    <div class="row">
                        <div class="col-sm-12">
                            <h5>Creación de Declaración en Ceros y Enviar <a href="http://youtu.be/Gq5BxiZi0AI" target="_blank">Ver Videotutorial</a>  </h5>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:Button ID="Crear" runat="server" Text="Crear" CssClass="btn btn-info btn-sm btn-block" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:Button ID="verCeros" runat="server" Text="Ir a Acuses" CssClass="btn btn-info btn-sm btn-block" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="View5" runat="server">
                <div class="container">
                    <div class="row">
                        <div class="col-sm-12">
                            <h5>Consulta de Declaración</h5>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Button ID="back" runat="server" Text="Regresar" CssClass="btn btn-sm btn-info" />
                        </div>
                        <div class="col-sm-3">
                            <asp:Button ID="verDatos" runat="server" CssClass="btn btn-sm btn-info" Text="Ver datos" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="verEvidEnvio" runat="server" CssClass="btn btn-sm btn-info" Text="Evidencia de Envio" />
                        </div>
                        <div class="col-sm-3">
                            <asp:Button ID="export" runat="server" CssClass="btn btn-sm btn-info" Text="Exportar a excel" ToolTip="Se lleva en promedio 2-24 hrs para recibir el acuse del SAT, tras dar clic, vea el mensaje mostrado abajo en Descripción" />
                        </div>
                        <div class="col-sm-3">
                            <asp:Button ID="bajarAcuseExcel" runat="server" Text="Bajar Acuse"
                                ToolTip="Se lleva en promedio 2-24 hrs para recibir el acuse del SAT, tras dar clic, vea el mensaje mostrado abajo en Descripción"
                                CssClass="btn btn-sm btn-info" />
                            <asp:CheckBox ID="acuSinCorr" runat="server" Text="Sin enviar correo" />
                            <div class="alert alert-warning">
                                Tras bajar acuse, Vea el mensaje mostrado abajo en Descripción
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:Label ID="Label1" runat="server" Style="font-family: Arial, Helvetica, sans-serif; font-size: small" Text="Meses:"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:TreeView ID="TreeView1" runat="server"
                                Style="font-family: Arial; font-size: small">
                            </asp:TreeView>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <span class="style9master" style="font-family: Arial, Helvetica, sans-serif">
                                <cc1:WebReport ID="WebReport1" runat="server" Height="57px" PdfA="True" ShowExports="False" ShowPdfExport="False" ShowPrint="False" ShowRefreshButton="False" ShowZoomButton="False" Visible="False" Width="193px"></cc1:WebReport>
                            </span>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="View6" runat="server">
                <div class="container">
                    <div class="row">
                        <div class="col-sm-4">
                            <h5>Consulta de Declaración: </h5>
                        </div>
                    </div>
                    <div class="container">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="row">
                                    <div class="col-sm-4 ">
                                        <asp:Button ID="backXml" runat="server" Text="Regresar" CssClass="btn btn-sm btn-info " />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Button ID="consultarXml" runat="server" Text="Descargar xml subido" CssClass="btn btn-sm btn-info" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Button ID="bajaAcuseXml" runat="server"
                                            Text="Bajar Acuse"
                                            ToolTip="Se lleva en promedio 2-24 hrs para recibir el acuse del SAT"
                                            CssClass="btn btn-info btn-sm" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>

            <asp:View ID="View7" runat="server">
                <div class="container">
                    <div class="row">
                        <div class="col-sm-12">
                            <h3>Creación de declaración anual vía 12 mensuales registradas del ejercicio</h3>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <asp:Button ID="Button1" runat="server" Text="Crear" CssClass="btn btn-sm btn-info" />
                        </div>
                        <div class="col-sm-4">
                            <asp:Button ID="Button2" runat="server" Text="Enviar Declaración"
                                ToolTip="(Requiere 1o clic en Crear) Tras enviar, espere a que cambie el estatus para finalizar esta operación"
                                CssClass="btn btn-info btn-sm" />
                        </div>
                        <div class="col-sm-4">
                            <asp:Button ID="Button3" runat="server" Text="Ver datos y Acuses" CssClass="btn btn-sm btn-info" />
                        </div>
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>


        <div class="container">
            <hr class="bg-dark" style="border-width: 1px" />
            <div class="row">
                <div class="col-sm-12">
                    <ul>
                        <li>* = Datos calculados por sistema, podría editarlos <strong>después</strong> de importar</li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="container">
            <div class="card">

                <div class="card-body">
                    <div class="row">
                        <div class="col-sm-2 text-right">Estatus: </div>
                        <div class="col-sm-10 text-left" style="color:darkgoldenrod">
                            <asp:Label ID="estado" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 text-right">Descripción: </div>
                        <div class="col-sm-10 text-left">
                            <asp:Label ID="descrip" runat="server" Style="color: #996600; font-weight: 700;"></asp:Label>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-header">Totales de año</div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-sm-4 text-right">Número de Operaciones</div>
                                <div class="col-sm-4 text-center">
                                    <asp:Label ID="nOpers" runat="server" Text="0" Width="150px"></asp:Label>
                                </div>
                                <div class="col-sm-4"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">Excedente</div>
                                <div class="col-sm-4 text-center">
                                    <asp:Label ID="impteExcedente" runat="server" Text="0" Width="150px"></asp:Label>
                                </div>
                                <div class="col-sm-4"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">Cheque de caja en efectivo</div>
                                <div class="col-sm-4 text-center">
                                    <asp:Label ID="impteCheque" runat="server" Text="0"
                                        Width="150px"></asp:Label>
                                </div>
                                <div class="col-sm-4"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="card">
                <div class="card-body">
                    <div class="row">
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="lblFechaPresentacionAnt" runat="server"
                                Text="Fecha presentación anterior"></asp:Label>
                        </div>
                        <div class="col-sm-4 text-center">
                            <asp:Label ID="fechaPresentacionAnt" runat="server" Text="0"
                                Width="150px"></asp:Label>
                        </div>
                        <div class="col-sm-4"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="lblNumOperAnt" runat="server" Text="Num. de Operación anterior"
                                CssClass="style17"></asp:Label>
                        </div>
                        <div class="col-sm-4 text-center">
                            <asp:Label ID="numOperAnt" runat="server" Text="0"
                                Width="150px" CssClass="style17"></asp:Label>
                        </div>
                        <div class="col-sm-4"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right">Tipo</div>
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="normalComplementaria" runat="server" CssClass="style17"></asp:Label>
                        </div>
                        <div class="col-sm-4"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right">Representante legal</div>
                        <div class="col-sm-4 text-right">
                            [<asp:Label ID="idRepresentanteLegal" runat="server" Text="0" CssClass="style17"></asp:Label>]
                            <asp:Label ID="RepresentanteLegal" runat="server" Width="300"
                                CssClass="style17"></asp:Label>
                        </div>
                        <div class="col-sm-4"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right">Límite excedente $</div>
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="ideConfLimite" runat="server"
                                Text="0" Width="150px" CssClass="style17"></asp:Label>
                        </div>
                        <div class="col-sm-4"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right">Fecha envío</div>
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="fechaEnvio" runat="server" Width="150px"
                                CssClass="style17"></asp:Label>
                        </div>
                        <div class="col-sm-4"></div>
                    </div>
                    <div class="card">
                        <div class="card-header">Acuse</div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-sm-4 text-right">Fecha presentación</div>
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="fechaPresentacion" runat="server" Width="150px"
                                        CssClass="style17"></asp:Label>
                                </div>
                                <div class="col-sm-4"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">Num. de Operación</div>
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="numOper" runat="server" Width="150px"
                                        CssClass="style17"></asp:Label>
                                </div>
                                <div class="col-sm-4"></div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right">ID</div>
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="id" runat="server" Text="0" CssClass="style17"></asp:Label>
                        </div>
                        <div class="col-sm-4"></div>
                    </div>
                    <div class="row pb-2">
                        <div class="col-sm-4">
                        </div>
                        <div class="col-sm-4">
                            <asp:Button ID="btnMod" runat="server" Text="Validar" CssClass=" btn btn-sm btn-block btn-info" />
                        </div>
                        <div class="col-sm-4"></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4"></div>
                        <div class="col-sm-4">
                            <asp:Button ID="enviarDeclaracionExcel" runat="server"
                                Text="Enviar Declaración"
                                ToolTip="Tras enviar, espere a que cambie el estatus" CssClass="btn btn-block btn-sm btn-info" />
                        </div>
                        <div class="col-sm-4">
                            <asp:CheckBox ID="chkSinCorreo" runat="server" Text="sin enviar correo cliente" />
                            <div class="alert alert-warning">
                                Después de enviarla, espere hasta recibir un mensaje, pues cada que pulse 
                    &#39;Enviar&#39; se descuenta una declaración.
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                        </div>
                        <div class="col-sm-4">
                            Quién presentará la contingencia:
                            <asp:DropDownList ID="quienContin" CssClass="form-control form-control-sm" runat="server">
                                <asp:ListItem>Proveedor</asp:ListItem>
                                <asp:ListItem>Contribuyente</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Button ID="btnContingencia" runat="server" Text="Descargar contingencia"
                                ToolTip="Cuenta como un envío"
                                OnClientClick="if (confirm('¿Esta 100% seguro de descargar esta contingencia? \n Le sugerimos esperar unas horas a que se reestablezca el servidor del SAT y vuelva intentar Enviar declaración pulsando ahora Cancelar, primer alerta')==true){return confirm('¿Confirma descargar esta contingencia ahora? \n Le sugerimos esperar unas horas a que se reestablezca el servidor del SAT y vuelva intentar Enviar declaración pulsando ahora Cancelar, última alerta');}else{return false;}"
                                CssClass="btn btn-sm btn-block btn-info" />
                            <asp:CheckBox ID="chkPostpago" runat="server" Enabled="False" Text="Postpago" />
                        </div>
                        <div class="col-sm-4">
                            <div class="alert alert-warning">
                                Si por alguna razón no pudo enviar su declaración y le urge, puede presentar 
                    directamente al SAT el archivo de contingencia<br />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <br />
</body>
</html>
