<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="contrato.aspx.vb" Inherits="WebApplication1.WebForm8" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="HeadContent">


    <script type="text/javascript">

        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-33257770-1']);
        _gaq.push(['_trackPageview']);

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();

        function callConfirm2() {
            if (confirm('Ya existe una factura, deseas Re Timbrar?')) {
                $("#btnConfirm").click();
            }
        }

        function callConfirm(ped) {
            var mensaje = confirm("Ya existe una factura, deseas Re Timbrar?");            
            if (mensaje) {
                let valHiden = $("#<%:refactPedido.ClientID%>");
                valHiden.val(ped);
                __doPostBack("<%:refactPedido.ClientID%>", "");
            }
            else {
                return true;
            }
        }


    </script>

    <style type="text/css">
        .auto-style1 {
            position: relative;
            width: 100%;
            min-height: 1px;
            -webkit-box-flex: 0;
            -ms-flex: 0 0 25%;
            flex: 0 0 25%;
            max-width: 25%;
            left: 0px;
            top: 0px;
            padding-left: 15px;
            padding-right: 15px;
        }
    </style>

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
        <div class="row">
            <div class="col-sm-12">
                <h4 class="text-center">Contrato</h4>
            </div>
        </div>

        <asp:UpdatePanel runat="server" ID="mipanel">
            <ContentTemplate>       
        <asp:HiddenField ID="refactPedido" runat="server" />
        <div class="row pb-1">
            <div class="col-sm-2 text-right">
                Número de Contrato: 
            </div>
            <div class="col-sm-2">
                <asp:Label ID="id" runat="server" Font-Bold="true"></asp:Label>
            </div>
            <div class="col-sm-2">
                <asp:Button ID="Sugerencias" CssClass="btn btn-info btn-sm rounded p-1" runat="server" Text="Ver Sugerencias" Visible="false" />
                <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>
            </div>
            <div class="col-sm-6"></div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-3 text-right">
                <label for="idCliente">Cliente ID:</label>
            </div>
            <div class="col-sm-3">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-sm-10 pl-0">
                            <asp:TextBox ID="idCliente" runat="server" AutoPostBack="True" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:Button ID="selCliente" runat="server" Text="..." OnClientClick="document.getElementById('form1').target = '_self';"
                                PostBackUrl="~/contrato.aspx" CssClass="btn btn-dark btn-sm" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-3 text-right">
                Nombre:
            </div>
            <div class="col-sm-3">
                <asp:TextBox runat="server" ID="cliente" Columns="30" MaxLength="50" AutoPostBack="True" CssClass="form-control form-control-sm"></asp:TextBox>
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-3 text-right">
                Plan :
            </div>
            <div class="col-sm-3">
                <asp:DropDownList ID="elPlan" runat="server" DataSourceID="SqlDataSource2" DataTextField="elplan" DataValueField="elplan" AutoPostBack="True" CssClass="form-control form-control-sm " Font-Size="Small">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ideConnectionString %>" SelectCommand="SELECT [elplan] FROM [planes]"></asp:SqlDataSource>
            </div>
            <div class="col-sm-3">
                <asp:Label ID="idPlan" runat="server" Visible="false"></asp:Label>
            </div>
            <div class="col-sm-3">
            </div>
        </div>
        <div class="row pb-1">            
            <div class="col-sm-3 text-right">
                Periodo inicial:
            </div>
            <div class="col-sm-3">
                <asp:TextBox runat="server" ID="periodoInicial" Columns="30" MaxLength="10" AutoPostBack="True" ToolTip="Fecha inicial de los periodos a declarar en este contrato: dd/mm/aaaa" CssClass="form-control form-control-sm">
                </asp:TextBox>
                <div class="container-fluid">
                    <div class="row pb-1">
                        <div class="col-sm-12">
                            <ajaxToolkit:CalendarExtender ID="periodoInicial_CalendarExtender" runat="server"
                                Enabled="True" TargetControlID="periodoInicial" CssClass="MyCalendar bg-white rounded " Format="dd/MM/yyyy">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                            <asp:Label ID="lblFF" runat="server" Text="Fecha Final:" Visible="false"></asp:Label>
                            <asp:Label ID="fechaFinal" runat="server"></asp:Label>
                            <asp:HiddenField ID="mesesRegularizacion" runat="server" Visible="False" />
                            <asp:HiddenField ID="mesesAnticipados" runat="server" Visible="False" />            
            </div>
        </div>
        
        <div class="row pb-1" style="display:none;">
            <div class="col-sm-3 text-right">
                * Descuentos Total =
            </div>
            <div class="col-sm-3">
                <asp:Label ID="desctoPesos" runat="server"></asp:Label>
            </div>
            <div class="col-sm-3 text-right">
                desglose:
            </div>
            <div class="col-sm-3">
                <asp:Label ID="desglose" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-3 text-right">
                <asp:Label ID="lblDeclContra" runat="server" Text="Declaraciones Contratadas:"></asp:Label>
                
            </div>
            <div class="col-sm-3">
                <asp:TextBox runat="server" ID="nDeclContratadas" Columns="30" MaxLength="40"
                    onblur="Javascript:ceros(this);formatoNumero(this,0,'.',',');"
                    onkeypress="return numeros()" AutoPostBack="True"
                    ToolTip="Presione la tecla Tab (tabulación) despues de introducir este dato"
                    CssClass="form-control form-control-sm" Text="0"></asp:TextBox>
            </div>
            <div class="col-sm-3 text-right">
                <asp:Label ID="lblDurMes" runat="server" Text="Duración (Meses) :" Visible="false"></asp:Label>                
                
            </div>
            <div class="col-sm-3">
                <asp:TextBox runat="server" ID="duracionMeses" Columns="30" MaxLength="40"
                    onblur="Javascript:ceros(this);formatoNumero(this,0,'.',',');"
                    onkeypress="return numeros()" AutoPostBack="True"
                    ToolTip="Presione la tecla Tab (tabulación) despues de introducir este dato"
                    CssClass="form-control form-control-sm" Text="0" Visible="false"></asp:TextBox>                
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-3 text-right">
                <asp:Label ID="lblDeclHech" runat="server" Text="Declaraciones Hechas:"></asp:Label>
                
            </div>
            <div class="col-sm-3">
                <asp:Label ID="nDeclHechas" runat="server"></asp:Label>
            </div>
            <div class="col-sm-3 text-right">
                <asp:Button ID="actNdeclsHechas" runat="server" Text="Actualizar hechas a" CssClass="btn btn-info btn-info-sm rounded p-1" />
            </div>
            <div class="col-sm-3">
                <asp:TextBox ID="nDeclHechasCaptura" runat="server" onkeypress="return numeros()" CssClass="form-control form-control-sm"></asp:TextBox>
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-3 text-right">
                <p>
                    <asp:Label ID="lblNvoPrec" runat="server" Text="Precio neto Pactado:"></asp:Label>                    
                </p>
            </div>
            <div class="col-sm-3 text-right">
                <asp:TextBox ID="nvoPrecNeto" runat="server" onkeypress="return numerosDec()" CssClass="form-control form-control-sm"></asp:TextBox>
            </div>
            <div class="col-sm-3 text-right">
                
            </div>
            <div class="col-sm-3">
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-3 text-right">
                Precio neto contrato:
            </div>
            <div class="col-sm-3">
                <asp:Label ID="precioNetoContrato" runat="server"></asp:Label>
            </div>
            <div class="col-sm-6" style="Font-Size:Small">
                <asp:CheckBox ID="acepto" runat="server"  />
                Acepto los <a href="politicas.aspx">Términos del servicio y políticas de uso</a>, el 
                <a href="privacidad.aspx">Aviso de Privacidad</a> y 
                <a href="contrato.aspx#caracs">Características del 
                contrato</a>, del sitio web declaracioneside.com
            </div>

        </div>
        <div class="row pb-1" style="display:none;">
            <div class="col-sm-3 text-right">
                <asp:Label Text="Código de cortesia:" ID="lblcortes" runat="server" />
            </div>
            <div class="col-sm-3">
                <asp:TextBox runat="server" ID="codCliente" Columns="30" MaxLength="70" AutoPostBack="True" ToolTip="(opcional) Sí posee un código de cortesía otorgado por su distribuidor introdúzcalo aquí" PlaceHolder="(OPCIONAL)"
                    CssClass="form-control form-control-sm"></asp:TextBox>
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-12" style="Font-Size:Small">
                <asp:CheckBox ID="esRegularizacion" runat="server"
                                Text="Regulariza periodos anteriores"
                                ToolTip="En este contrato sólo podrá realizar declaraciones de periodos previos al mes actual en que intente enviar declaraciones" AutoPostBack="True" Font-Size="Small" />
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-12" style="Font-Size:Small">
                (Si es un nuevo contrato, primero guardelo para habilitar el contenido del botón &#39;pago con tarjeta&#39;)
            </div>
        </div>

        </ContentTemplate>
        </asp:UpdatePanel>

        <div class="row pb-1 bg-light">
            <div class="col-sm-12">
                <h4 class="text-center">Acciones
                </h4>
            </div>
            <div class="col-sm-2">
                <asp:Button runat="server" ID="addEdit" Text="Guardar"
                    OnClientClick="document.getElementById('form1').target = '_self';"
                    PostBackUrl="~/contrato.aspx" CssClass="btn btn-info btn-info-sm rounded p-1" />
            </div>
            <div class="col-sm-2">
                <asp:Button ID="instruccPago" runat="server" Text="Instrucc. Pago"
                    ToolTip="Recibir por correo instrucciones de pago" CssClass="btn btn-info btn-info-sm rounded p-1" />
            </div>
            <div class="col-sm-2">
                <asp:Button ID="misContra" runat="server" Text="Mis Contratos"
                    OnClientClick="document.getElementById('form1').target = '_self';"
                    PostBackUrl="~/contrato.aspx" ToolTip="Regresar a mi lista de contratos"
                    CssClass="btn btn-info btn-info-sm rounded p-1" />
            </div>
            <div class="col-sm-2">
                <asp:Button ID="pagos" runat="server" Text="Pagar con tarjeta"
                    Style="margin-right: 0px"
                    ToolTip="Pagar con tarjeta de crédito, débito, paypal" CssClass="btn btn-info btn-info-sm rounded p-1" />
            </div>
            <div class="col-sm-2">
                <a href="#" onclick="window.print();">Imprimir</a>
            </div>
            <div class="col-sm-2">
                <asp:Button ID="cotizar" runat="server" CssClass="btn btn-info btn-info-sm rounded p-1" Text="Ajustar precio" ToolTip="ve a Guardar" />
            </div>
            
            <div class="col-sm-12">
                &nbsp;
            </div>
        </div>
        <hr class="bg-dark" style="border-width: 1px" />
            <div class="card">
                <div class="card-body">
        <div class="row pb-1 pt-1">
                    <div class="col-sm-2 text-right ">
                        <asp:Label ID="lblfechaPagado" runat="server" Text="* Fecha pagado:" />                        
                    </div>
                    <div class="col-sm-2 text-left">
                        <asp:TextBox runat="server" ID="fechaPago" Columns="30" MaxLength="10" ToolTip="dd/mm/aaaa" CssClass="form-control form-control-sm"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="fechaPago_CalendarExtender" runat="server"
                            Enabled="True" TargetControlID="fechaPago" CssClass="MyCalendar bg-white rounded" Format="dd/MM/yyyy">
                        </ajaxToolkit:CalendarExtender>
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="lblformapago" runat="server" Text="FormaPago:" Font-Bold="true" ForeColor="#990000"></asp:Label> &nbsp;
                        <asp:DropDownList ID="pagoRealizado" runat="server" CssClass="form-control form-control-sm" DataSourceID="SqlDataSourceFP" DataTextField="descripcion" DataValueField="clave" ></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSourceFP" runat="server" ConnectionString="<%$ ConnectionStrings:ideConnectionString %>" SelectCommand="SELECT * FROM [c_formaPago] order by id"></asp:SqlDataSource>
                    </div>
                    <div class="auto-style1">
                        <asp:CheckBox ID="yFac" runat="server" Text="y facturar" />
                        <asp:Button runat="server" ID="actPago" Text="Registrar Pago"
                            PostBackUrl="~/contrato.aspx"
                            ToolTip="y notificarlo al cliente, lo establece o lo limpia"
                            CssClass="btn btn-sm btn-info rounded p-1" />                                        
                    </div>  
                    <div class="col-sm-2">
                        <asp:Button runat="server" ID="soloFac" Text="Solo facturar FP" ToolTip="Verifica 1o formaPago" PostBackUrl="~/contrato.aspx" CssClass="btn btn-sm btn-info rounded p-1" />
                        <asp:Button ID="btnConfirm" runat="server" Text="" Style="display: none" />
                    </div>                        
        </div>
      </div>
     </div>

        <div class="row pb-1">
            <div class="col-sm-2">
                <asp:CheckBox ID="chkPostpago" runat="server" Text="Postpago" />
            </div>
            <div class="col-sm-2">
                <asp:CheckBox ID="deCortesia" runat="server" Text="De cortesia" />
            </div>            

            <div class="col-sm-2">
                <asp:CheckBox ID="comisionPagada" runat="server" Text="Comisión Pagada"
                    ToolTip="Administrador" />
            </div>

            <div class="col-sm-3">
                <asp:CheckBox ID="vencido" runat="server" Text="Contr.Vencido"
                    ToolTip="Contrato Vencido" />
            </div>
            <div class="col-sm-3">
                <asp:CheckBox ID="sinCorr" runat="server" Text="Sin enviar correo" />
            </div>

        </div>
        <div class="row pb-1">
            <div class="col-sm-2">
                <asp:CheckBox ID="parcialidades" runat="server" Text="Paga parcialidades"
                    ToolTip="Administrador" />
            </div>
            <div class="col-sm-3 text-right">
                <asp:Label ID="lblNadeudos" runat="server" Text="No. Adeudos: "></asp:Label>
                <asp:TextBox runat="server" ID="nAdeudos" Columns="3" MaxLength="3"
                    onblur="Javascript:ceros(this);formatoNumero(this,0,'.',',');"
                    onkeypress="return numeros()" AutoPostBack="True"
                    CssClass="form-control form-control-sm">0</asp:TextBox>
            </div>
            <div class="col-sm-3 text-right">
                <asp:Label ID="lblMontoAdeudos" runat="server" Text="$ Adeudos"></asp:Label>
                <asp:TextBox runat="server" ID="montoAdeudos" Columns="10" MaxLength="10"
                    onblur="Javascript:ceros(this);formatoNumero(this,0,'.',',');"
                    onkeypress="return numeros()" AutoPostBack="True"
                    CssClass="form-control form-control-sm">0</asp:TextBox>
            </div>
            <div class="col-sm-4 text-right">
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-2">
                <asp:Button ID="factTx" runat="server" Text="Factura enviada" CssClass="btn btn-info btn-info-sm rounded p-1" /> 
                <asp:CheckBox ID="enviada" runat="server" Enabled="False" />               
            </div>
            <div class="col-sm-2 text-right">
                <asp:Label ID="piDiferenciaLbl" runat="server" Text="Diferencia en pesos ($)"></asp:Label>
            </div>
            <div class="col-sm-2">
                <asp:TextBox runat="server" ID="piDiferencia" Columns="30" CssClass="form-control form-control-sm" AutoPostBack="True"></asp:TextBox>
            </div>
            <div class="col-sm-2">
                <asp:Button ID="pagosInsuficiente" runat="server" Text="PagoInsuficiente"
                    ToolTip="Notifica al cliente que haga nuevo contrato y pague la diferencia e indicarle periodo para pagar"
                    CssClass="btn btn-info btn-sm rounded p-1" />
            </div>
            <div class="col-sm-2">
                <asp:Button ID="atras" runat="server" Text="Contratos"
                    OnClientClick="document.getElementById('form1').target = '_self';"
                    PostBackUrl="~/contrato.aspx" CssClass="btn btn-sm btn-info rounded p-1" />
            </div>
            <div class="col-sm-2">
                <asp:Button ID="del" runat="server" Text="Eliminar"
                    OnClientClick="document.getElementById('form1').target = '_self'; return confirm('¿Esta seguro de eliminar este registro?');"
                    PostBackUrl="~/contrato.aspx" CssClass="btn btn-sm btn-danger rounded p-1"  />
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-12">
                <asp:Label ID="lbluuid" runat="server" Text="UUID:"></asp:Label> &nbsp;
                <asp:TextBox runat="server" ID="uuid" Columns="36" MaxLength="36" Width="363px" Enabled="false" ></asp:TextBox>
            </div>
        </div>
    </div>
    <hr class="bg-dark" style="border-width: 1px" />
    <div class="container">
        <div class="card">
            <div class="card-header">
                <h5>Caracteristicas del contrato</h5>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="container-fluid">
                        <div class="col-sm-12">
                            <span class="style1master1">
                                <span class="style17">Este sistema NO está diseñado para Instituciones de crédito que son 
    auxiliares de la TESOFE, pero si es una institución de crédito no auxiliar de la 
    TESOFE o es una <strong>Institución financiera</strong> distinta a las instituciones de crédito(bancos) 
    este sistema es para Usted.<br />
                                    <br />
                                    * Administrados por sistema

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

                        function cambiaCliente(IDclienteId, clienteId, IDcliente, cliente) {
                            document.getElementById(IDclienteId).value = clienteId;
                            document.getElementById(IDcliente).value = cliente;
                        }
                    </script>

                                    <br />
                                    <br />
                                    Para mas detalles, coloque el ratón sobre los cuadros o botones deseados.<br />
                                    <br />
                                    Periodo inicial: Fecha inicial de los periodos a declarar en este contrato, solo 
    se considerará el mes y el año: dd/mm/aaaa<br />
                                    <br />
                                    Una declaración se considera de periodos anteriores o retrasados, si es de una 
    fecha menor al mes anterior del momento de contratación, en cuyo caso ocuparía 
    palomear &#39;Regularización de periodos anteriores&#39;.
    <br />
                                    <br />
                                    Solo puede tener un plan 
    por contrato, elija el mas adecuado a sus necesidades, y si ocupa distintos 
    planes realice distintos contratos por los periodos deseados; para el caso de 
    planes Premium podrá realizar todas las declaraciones ilimitadas que necesite correspondientes 
    al 
    rango de fechas contratado.<br />
                                    <br />
                                    Declaración anual:<br />
                                    &nbsp;&nbsp;&nbsp; * Plan premium &gt;=12 meses.&nbsp; Incluye <strong>declaraciones 
anuales normales y complementarias</strong> del periodo. Por ejemplo si su contrato 
es de junio 2012 a junio 2014, esto le incluye las anuales de 2012 y 2013<br />
                                    &nbsp;&nbsp;&nbsp; * Plan basico o ceros. Cada <strong>declaración anual normal o 
complementaria</strong> cuenta como un envío<br />

                                    <br />
                                    Los traslapes de fechas de contrato solo se validan para planes premium<br />
                                    <br />
                                    Los descuentos se aplican sobre el precio neto del contrato<br />

                                    <br />
                                    No hay cancelaciones, si requiere asesoría no dude en contactarnos<br />

                                    <br />
                                    Verificaremos a la brevedad su pago, una vez que lo confirmemos observará la 
    fecha de pago y podrá hacer uso de su plan, lo cual le será notificado 
    <br />
                                    <br />
                                </span></span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
    </div>
</asp:Content>
