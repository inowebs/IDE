<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="mensual22.aspx.vb" Inherits="WebApplication1.mensual22" %>
<%@ Register Assembly="FastReport.Web, Version=2015.2.0.0, Culture=neutral, PublicKeyToken=db7e5ce63278458c" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html>

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
            <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server" />--%>    <%--no usar el ajaxToolkit:ToolkitScriptManager con updatepanel ni timer xq no lo toma--%>
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
                    <asp:Label ID="redir" runat="server"></asp:Label>
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
                            <asp:HiddenField ID="h1" runat="server" />
                            Avance:
                        </div>
                        <div class="col-sm-10">
                            <asp:Timer ID="Timer1" runat="server" Enabled="False" Interval="1000" />
                            <asp:HiddenField ID="nomArchAnualDatos" runat="server" />
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
                                <asp:TextBox ID="errImport" Width="100%" runat="server" Height="113px" Rows="14" TextMode="MultiLine" Visible="False" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                    <div class="row pb-1">
                        <div class="col-sm-12">
                            <asp:Button ID="ver" runat="server" Text="Ver datos y Acuses" CssClass="btn btn-sm btn-info" />
                        </div>
                    </div>
                </div>
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
                            <asp:CheckBox ID="chkAcuse" runat="server" Text="Acuse" /> 
                            &nbsp;<asp:FileUpload ID="FileUploadAcuse" runat="server" ToolTip="" />
                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="acuseSet" runat="server" Text="Guardar y subir" CssClass=" btn btn-sm btn-info" />
                            &nbsp;&nbsp; <asp:Button ID="descargarAcuse" runat="server" Text="Descargar acuse"  CssClass="btn btn-sm btn-info"/>
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
                            <asp:Label ID="nRegs" runat="server" Text="0 Registros ordenados por nombre/razón social" Font-Size="Small" Visible="false"></asp:Label>
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-sm-12">
                            <asp:TreeView ID="TreeView1" runat="server"
                                Style="font-family: Arial; font-size: small">
                            </asp:TreeView>
                        </div>
                    </div>
                    <cc1:WebReport ID="WebReport1" runat="server" Height="27px" PdfA="True" ShowExports="False" ShowPdfExport="False" ShowPrint="False" ShowRefreshButton="False" ShowZoomButton="False" Visible="False" Width="104px" />
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
                <div class="col-sm-12">
                    Descripción: <asp:Label ID="lbldescrip" runat="server" Style="color: #996600; font-weight: 700;"></asp:Label>
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-12 text-right">
                    Estatus: <asp:DropDownList ID="estado" runat="server" AutoPostBack="true" DataSourceID="SqlDataSourceEdoDecla" DataTextField="estatus" DataValueField="id">
                        </asp:DropDownList>
                    <asp:Button ID="setStatusDecla" runat="server" Text="SETestatus" CssClass="btn btn-sm btn-info"/>
                            <asp:SqlDataSource ID="SqlDataSourceEdoDecla" runat="server" ConnectionString="<%$ ConnectionStrings:ideConnectionString %>" SelectCommand="SELECT * FROM [estatusDecla2] order by orden"></asp:SqlDataSource>
                            <asp:HiddenField ID="hidNivelDecla" runat="server" Visible="False" />                    
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-sm-12">
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
                            <div class="row pb-1">
                                <div class="col-sm-6 text-right">
                                *Suma Depositos en efectivo:
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox ID="sumaDepEfe" runat="server" onkeypress="return numerosDec()" onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');" CssClass="form-control form-control-sm">0</asp:TextBox>
                            </div>
                            </div>
                            <div class="row pb-1">
                                <div class="col-sm-6 text-right">
                                *Monto Cheque Caja:
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox ID="montoChqCaja" runat="server" onkeypress="return numerosDec()" onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');" CssClass="form-control form-control-sm">0</asp:TextBox>
                            </div>
                            </div>
                            <div class="row pb-1">
                                <div class="col-sm-6 text-right">
                                *No. de titulares
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox ID="ntit2" runat="server" onkeypress="return numerosDec()" onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');" CssClass="form-control form-control-sm">0</asp:TextBox>
                            </div>
                            </div>
                            <div class="row pb-1">
                                <div class="col-sm-6 text-right">
                                *No. de cheques de caja
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox ID="nChq2" runat="server" onkeypress="return numerosDec()" onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');" CssClass="form-control form-control-sm">0</asp:TextBox>
                            </div>
                            </div>
                        </div>
                    </div>

                </div>
                
            </div>          
          
            <div class="row pb-1">
                <div class="col-sm-12">
                    Monto Límite de efectivo: <asp:Label ID="ideConfLimite" runat="server" Text="0" CssClass="form-control form-control-sm"></asp:Label>
                    <br />
                    Fecha presentada: <asp:TextBox ID="fPresentada" runat="server" MaxLength="10" Columns="10" ></asp:TextBox>&nbsp;&nbsp;&nbsp;
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true" TargetControlID="fPresentada" CssClass="MyCalendar bg-white rounded" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
                    Fecha descargada: <asp:TextBox ID="fDescargada" runat="server" MaxLength="10" Columns="10" ></asp:TextBox>&nbsp;&nbsp;&nbsp;
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true" TargetControlID="fDescargada" CssClass="MyCalendar bg-white rounded" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
                    <asp:Label ID="lblCita" runat="server" Text="Cita proveedor:" ></asp:Label><asp:TextBox ID="fCita" runat="server" MaxLength="10" Columns="10" ToolTip="Fecha para declarar con el proveedor DeclaracionesIDE,o bien suba su fiel en cuenta" ></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="true" TargetControlID="fCita" CssClass="MyCalendar bg-white rounded" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
                    &nbsp;<asp:DropDownList  ID="hrs" runat="server" AutoPostBack="True"  style="text-align:right" ToolTip="Hora" >
                                <asp:ListItem Value="-">-</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>                                
                                <asp:ListItem Value="10">10</asp:ListItem>                                
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                            </asp:DropDownList>:                            
                            <asp:DropDownList  ID="mins" runat="server" AutoPostBack="True"  style="text-align:right" ToolTip="Minutos" >
                                <asp:ListItem Value="-">-</asp:ListItem>
                                <asp:ListItem Value="00">00</asp:ListItem>
                                <asp:ListItem Value="01">01</asp:ListItem>
                                <asp:ListItem Value="02">02</asp:ListItem>
                                <asp:ListItem Value="03">03</asp:ListItem>
                                <asp:ListItem Value="04">04</asp:ListItem>
                                <asp:ListItem Value="05">05</asp:ListItem>
                                <asp:ListItem Value="06">06</asp:ListItem>
                                <asp:ListItem Value="07">07</asp:ListItem>
                                <asp:ListItem Value="08">08</asp:ListItem>
                                <asp:ListItem Value="09">09</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="16">16</asp:ListItem>
                                <asp:ListItem Value="17">17</asp:ListItem>
                                <asp:ListItem Value="18">18</asp:ListItem>
                                <asp:ListItem Value="19">19</asp:ListItem>
                                <asp:ListItem Value="20">20</asp:ListItem>
                                <asp:ListItem Value="21">21</asp:ListItem>
                                <asp:ListItem Value="22">22</asp:ListItem>
                                <asp:ListItem Value="23">23</asp:ListItem>
                                <asp:ListItem Value="24">24</asp:ListItem>
                                <asp:ListItem Value="25">25</asp:ListItem>
                                <asp:ListItem Value="26">26</asp:ListItem>
                                <asp:ListItem Value="27">27</asp:ListItem>
                                <asp:ListItem Value="28">28</asp:ListItem>
                                <asp:ListItem Value="29">29</asp:ListItem>
                                <asp:ListItem Value="30">30</asp:ListItem>
                                <asp:ListItem Value="31">31</asp:ListItem>
                                <asp:ListItem Value="32">32</asp:ListItem>
                                <asp:ListItem Value="33">33</asp:ListItem>
                                <asp:ListItem Value="34">34</asp:ListItem>
                                <asp:ListItem Value="35">35</asp:ListItem>
                                <asp:ListItem Value="36">36</asp:ListItem>
                                <asp:ListItem Value="37">37</asp:ListItem>
                                <asp:ListItem Value="38">38</asp:ListItem>
                                <asp:ListItem Value="39">39</asp:ListItem>
                                <asp:ListItem Value="40">40</asp:ListItem>
                                <asp:ListItem Value="41">41</asp:ListItem>
                                <asp:ListItem Value="42">42</asp:ListItem>
                                <asp:ListItem Value="43">43</asp:ListItem>
                                <asp:ListItem Value="44">44</asp:ListItem>
                                <asp:ListItem Value="45">45</asp:ListItem>
                                <asp:ListItem Value="46">46</asp:ListItem>
                                <asp:ListItem Value="47">47</asp:ListItem>
                                <asp:ListItem Value="48">48</asp:ListItem>
                                <asp:ListItem Value="49">49</asp:ListItem>
                                <asp:ListItem Value="50">50</asp:ListItem>
                                <asp:ListItem Value="51">51</asp:ListItem>
                                <asp:ListItem Value="52">52</asp:ListItem>
                                <asp:ListItem Value="53">53</asp:ListItem>
                                <asp:ListItem Value="54">54</asp:ListItem>
                                <asp:ListItem Value="55">55</asp:ListItem>
                                <asp:ListItem Value="56">56</asp:ListItem>
                                <asp:ListItem Value="57">57</asp:ListItem>
                                <asp:ListItem Value="58">58</asp:ListItem>
                                <asp:ListItem Value="59">59</asp:ListItem>
                            </asp:DropDownList>                            &nbsp;<asp:Button ID="setFechas" runat="server" Text="Guardar fechas" CssClass="btn btn-sm btn-info"/>
                &nbsp;&nbsp;
                    <asp:Button ID="cita" runat="server" Text="Avisar cita al proveedor" CssClass="btn btn-sm btn-info"/>
                </div>
            </div>
            
            <div class="row pb-1">
                <div class="col-sm-12">
                    ID: <asp:Label ID="id" runat="server" Text="0"></asp:Label>,&nbsp;
                </div>
            </div>
            
            <div class="row pb-1">
                <div class="col-sm-12">
                    <asp:Panel ID="Panel1" runat="server" GroupingText="Declaración Mensual" BorderWidth="1" >
                        <asp:Button ID="crearDecla" runat="server" Text=" Crear " ToolTip="Crea el archivo de la declaracion que ocupa el SAT, no disponible en cuenta demo, esta operación consume una declaración" CssClass="btn btn-sm btn-info" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="descargaLocal" runat="server"
                            Text="Descargar" CssClass="btn btn-sm btn-info" /> &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkSinCorreo" runat="server" Text="sin enviar correo" Visible="False" />
                        &nbsp;
                                    <asp:Button ID="instructivo" runat="server" Text="Instructivo"  CssClass="btn btn-sm btn-info" Visible="false"/>
                        </asp:Panel>
                            <asp:CheckBox ID="validada" runat="server" Text="Validada" />                             
                            &nbsp;&nbsp;&nbsp;&nbsp;                             
                            Forma de presentación :
                            <asp:DropDownList  ID="tipoEnvio" runat="server" style="text-align:right" Enabled="false">
                                <asp:ListItem Value="Subir FIEL">Subir FIEL</asp:ListItem>                                
                                <asp:ListItem Value="Conexion remota">Conexion remota a mi equipo con la fiel</asp:ListItem>                                
                                <asp:ListItem Value="Por Cliente">El Cliente lo hara por su cuenta</asp:ListItem>                           
                            </asp:DropDownList>                            &nbsp;&nbsp;           &nbsp;&nbsp;
                            <asp:Button ID="saveTipoEnvio" runat="server" Text="Guardar método" CssClass=" btn btn-sm btn-info" Visible="false" />   &nbsp;&nbsp;&nbsp;   <asp:CheckBox ID="chkPostpago" runat="server" Enabled="False" Text="Postpago" />
                </div>
               
            </div>          
        </div>
    </form>
</body>
</html>
