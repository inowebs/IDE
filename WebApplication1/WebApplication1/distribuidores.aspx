<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="distribuidores.aspx.vb" Inherits="WebApplication1.WebForm18" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">

        .style5distr
        {
            font-size: medium;
            color: #800000;
        }
        .style7
        {
            height: 30px;
        }
        .style8
        {
            height: 30px;
        }
        .style14
        {
        }
        .style19
        {
            width: 56px;
            text-align: center;
            font-size: small;
        }
        .style20
        {
            text-align: center;
            width: 221px;
        }
        .style21
        {
            width: 225px;
        }
        .style22
        {
            width: 173px;
            font-size: small;
        }
        .style23
        {
            width: 233px;
        }
        .style24
        {
            height: 30px;
            width: 233px;
        }
        .style25
        {
            font-size: medium;
        }
        .style26
        {
            color: #000000;
        }
        .style46
        {
            font-family: arial;
            font-size: small;
            text-align: left;
        }
        .style47
        {
            font-size: small;
        }
        .style48
        {
            width: 225px;
            font-size: small;
        }
        .style49
        {
            width: 233px;
            font-size: small;
        }
        .style51
        {
            background-color: #FEFFFF;
        }
        .style53
        {
            height: 30px;
            font-size: small;
        }
        .style54
        {
            font-weight: bold;
            font-size: small;
            font-family: Arial;
        }
        .style55
        {
            width: 1px;
            text-align: center;
            font-size: small;
        }
        .style56
        {
            width: 302px;
            text-align: center;
            font-size: small;
        }
        .style57
        {
            color: #003366;
        }
        .style59
        {
            width: 1px;
        }
        .style60
        {
            width: 56px;
        }
        .style61
        {
            font-family: arial;
            font-size: small;
            text-align: left;
        }
        .style62
        {
            color: #003399;
            font-size: small;
        }
        .style63
        {
            text-align: center;
            width: 221px;
            font-size: small;
        }
        .MyCalendar .ajax__calendar_container {
            border:1px solid #646464;
            background-color: white;
            color: black;
        }
        </style>
    <script type="text/javascript" language="javascript">
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

</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">   
    <span itemscope itemtype="http://schema.org/SoftwareApplication">
        <strong itemprop="name">
        <asp:Label ID="Label2" runat="server"     
            Text="Solución para declaraciones de Depósitos en Efectivo (ISR, IDE). Envía declaraciones mensuales y anuales de Depósitos en Efectivo (ISR, IDE) aquí." 
            style="color: #FFFFFF; font-size: x-small;" ></asp:Label>
            </strong></span>
            <br />  
        <strong><span class="style5distr">Distribuidores</span></strong>
        <br />
        <br />
        <asp:Image ID="Image2" runat="server" ImageUrl="~/dist1.jpg" 
    AlternateText="Distribuidores" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Image ID="Image4" runat="server" imageurl="~/dist4.jpg" 
    Height="148px" Width="207px" AlternateText="Gana dinero con nosotros"/>
        <br />
        <br />
        <br />
         <span 
            class="style47">
&nbsp;&nbsp;&nbsp;&nbsp;
         
&nbsp;&nbsp;&nbsp;&nbsp;
         <strong><asp:LinkButton ID="LinkButton1" runat="server" 
            ToolTip="Tras dar clic, desplace la página hacia abajo">Registrarse</asp:LinkButton></strong>
&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="LinkButton2" runat="server" 
            ToolTip="Tras dar clic, desplace la página hacia abajo">Iniciar sesión como distribuidor</asp:LinkButton>
&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="listado" runat="server" 
            ToolTip="Tras dar clic, desplace la página hacia abajo">ListaDistr</asp:LinkButton>
        &nbsp;&nbsp;&nbsp;
             <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>
             <asp:Panel runat="server" ID="getPros" GroupingText="BD prospec." BorderWidth="1px">
                 <asp:CheckBox ID="chkClientes" runat="server" Text="IncluirClientes" />
                 <asp:CheckBox ID="chkPeriodo" runat="server" AutoPostBack="true" Text="Periodo" />
                 <asp:TextBox ID="perDesde" runat="server" Visible="false"></asp:TextBox>
                 <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
                                        Enabled="True" TargetControlID="perDesde" CssClass="MyCalendar" Format="dd/MM/yyyy" ></ajaxToolkit:CalendarExtender>
                 <asp:TextBox ID="perHasta" runat="server" Visible="false"></asp:TextBox>
                 <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" 
                                        Enabled="True" TargetControlID="perHasta" CssClass="MyCalendar" Format="dd/MM/yyyy" ></ajaxToolkit:CalendarExtender>
                 <asp:LinkButton ID="exportarBD" runat="server">GetCorreos</asp:LinkButton>
             </asp:Panel>
        
        </span>
        <br />
        <br />
        
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="View5" runat="server" >
                <strong><span class="style25">Gana dinero:</span></strong> <br />
                <br />
                <span class="style47">Dos esquemas de comisiones (no aplica concepto de 
                inscripción) a escoger:<br /> a) Gana un 10% sobre contratos (inicial y 
                renovaciones) durante un año, a partir de la fecha de <span class="style51">registro de cada cliente </span>
                <span class="style51" 
                    style="font-family: Arial; font-size: small; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: auto; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; display: inline !important; float: none;">
                que se registre con el número de distribuidor de EL DISTRIBUIDOR</span><span 
                    class="style51">. </span>
                <br />
                b) Gana un 15% por contrato inicial (antes de impuestos) con cada cliente que 
                logres y mantén un 15% de comisiones sobre renovaciones de contratos de los 
                clientes que introdujiste siempre y cuando en ese mes y año de renovación hayas 
                ingresado(registrado) a nuevos clientes.
                <br />
                <br />
                <span style="mso-ascii-font-family: Calibri; mso-fareast-font-family: +mn-ea; color: #002060; mso-font-kerning: 12.0pt;language:
es-MX">Los contratos son un paquete de un número de</span><span 
                    style="mso-ascii-font-family: Calibri; mso-fareast-font-family: +mn-ea; color: #002060; mso-font-kerning: 12.0pt;language:es-MX">
                </span>
                <span style="mso-ascii-font-family: Calibri; mso-fareast-font-family: +mn-ea; color: #002060; mso-font-kerning: 12.0pt;language:
es-MX">declaraciones (plan básico o ceros) o de un rango de fechas (plan premium) bajo un </span>
                <span style="mso-ascii-font-family: Calibri; mso-fareast-font-family: +mn-ea; mso-bidi-font-family: +mn-cs; color: #002060; mso-font-kerning: 12.0pt;language:
es-MX"><a href="planes.aspx"><span class="style26">plan</span></a></span></span><span 
                    style="mso-ascii-font-family: Calibri; mso-fareast-font-family: +mn-ea; color: #002060; mso-font-kerning: 12.0pt;language:es-MX"><span 
                    class="style47">, sean para declaraciones retrasadas o para declaraciones 
                próximas, cada cliente define la cantidad o el periodo a contratar y puede 
                combinar contratos de distintos planes a la vez. Para planes básico y ceros, el 
                cliente decide en que año y mes utilizar cada declaración, las contratadas no 
                utilizadas podrá usarlas cuando lo requiera, no las pierde. Para el plan premium 
                las declaraciones se aplican exclusivamente a los periodos contratados. Los 
                contratos son electrónicos. </span>
                <span class="style25"><span class="style1master1"><span class="style46" 
                    style="mso-ascii-font-family: Calibri; mso-fareast-font-family: +mn-ea; color: #002060; mso-font-kerning: 12.0pt;language:es-MX">
                Cada contrato un plan distinto.</span></span></span></span><br class="style47" />
                <span class="style47">Estas comisiones no aplican a la cuota de inscripción de 
                cliente nuevo por concepto de procesamiento y configuración de su 
                matriz/canal/socket de conexión segura.
                <br />
                <br />
                Este sistema puedes ofrecerlo a los siguientes <strong>prospectos (mercado)</strong> 
                <strong>instituciones del sistema financiero</strong> de todo el país (México):<br />
                <br />
                &nbsp;&nbsp;&nbsp; Instituciones de crédito(bancos no auxiliares de la TESOFE), instituciones 
                de seguros, administradoras de fondos para el retiro, uniones de crédito, 
                sociedades financieras, sociedades de inversión, sociedades cooperativas de 
                ahorro y préstamos (cajas populares y eclesiásticas), organismos de integración 
                financiera rural.<br />
                </span>
                <br />
                <strong>
                <br />
                Requisitos</strong>:
                <br />
                <br />
                <span class="style47">* Correo electrónico,
                <br />
                * <a href="distribuidores.aspx/#" 
                    title="Si no tienes una, en bancomer puedes abrir una cuenta express con $100, tu IFE y # de celular, y pideles tu 'clabe interbancaria', cuyo tope de cuenta es $13,500 pero despues puedes abrir una de otro tipo y re-definirla aqui en la sección de tu cuenta de distribuidor">
                Cuenta bancaria</a>
                <br />
                * Si no estás inscrito en el SAT, conseguir (por cuenta propia y con terceros) y 
                enviarnos facturas con nuestros datos por montos iguales o superiores a las 
                comisiones que recibas.
                <br />
                * O bien si ya estás inscrito y activo, tu cédula fiscal para emitirnos facturas 
                o recibos de honorarios (electrónicos o escaneados en PDF) como comprobantes de 
                tus ganancias (si decides darte de alta por primera vez en el SAT, te sugerimos 
                elegir el régimen de Persona física con actividad empresarial y profesional 
                (para facturar) o bien un régimen que te permita ganar ingresos por comisiones u 
                honorarios), y agregue la actividad fiscal &#39;Apoyo para negocios&#39;. * NO requieres 
                adquirir ninguna licencia ni realizar ningún pago, sino recibir comisiones por 
                los clientes que logres afiliar.
                <br />
                <br />
                Recibirás transferencia bancaria directo a tu cuenta al emitirnos los 
                comprobantes fiscales correspondientes, dispones de 30 dias naturales para 
                enviarlos y poder recibir comisiones a partir de la fecha de recepción del 
                correo donde se le solicitan las facturas. El monto total (despues de impuestos) 
                de la(s) factura(s) que nos enviará, equivale al % que estás recibiendo.
                <br />
                <br />
                En el caso de estar dado de alta en el SAT, el concepto de las facturas puede 
                ser &quot;Comisión de renta de servicio informático logrado con el contrato número 
                &lt;numeroDeContrato&gt;&quot;. Pero si no estás dado de alta en el SAT, los conceptos de 
                las facturas pueden ser de: papeleria, gasolina, publicidad(impresos, 
                materiales, servicios publicitarios, anuncios), articulos de oficina, 
                viáticos(pasajes, hospedaje, alimentos), cursos de informática, empresariales o 
                contables, articulos y servicios relacionados a la 
                informatica/computación/internet, mantenimiento y servicios de automovil, entre 
                otros.
                <br />
                <br />
                Es necesario que verifiques tu <strong>autorización</strong> en la sección 
                &quot;Iniciar sesión como distribuidor&quot; (la cual emitiremos tras validar tu 
                documentación), pues si no estás autorizado no se te acreditarán tus comisiones.
                <br />
                <br />
                <strong>Regístrate</strong> como distribuidor y provee información verídica.
                <br />
                Recuerda proporcionar tu <strong>número de distribuidor</strong> a tus clientes 
                que se vayan o vayas a registrar, para que en dicho proceso lo introduzcan, y 
                así vincularte tus comisiones.
                <br />
                <br />
                <strong>Documentación</strong> para persona física: Escaneados tu IFE, CURP, 
                comprobante de domicilio reciente (y si optaste por el esquema de inscripción al 
                SAT tu hoja de inscripción/registro en el RFC)
                <br />
                Documentación para persona moral: Escaneados comprobante de domicilio reciente, 
                (y si optaste por el esquema de inscripción al SAT la hoja de 
                inscripción/registro en el RFC)
                <br />
                En el proceso de registro podrás subir dicha documentación.<br />
                <br />
                Si optaste por inscripción en el SAT, y no cuentas con <strong>facturación</strong>, puedes 
                <strong>emitir tus facturas electrónicas</strong> registrandote en
                <a href="facturaselectronicascfdi.com" target="_blank">facturaselectronicascfdi.com</a>,
                <br />
                <br />
                Quienes se inscriben en el SAT, tienen puertas abiertas para recibir créditos al 
                disponer de sus declaraciones fiscales como comprobante de ingresos y meter sus 
                gastos como deducibles para pagar el mínimo de impuestos.&nbsp;Si te encuentras en 
                Morelia y requieres asesoría de un contador, puedes contactar al 443-3152068 
                cuyo despacho te asesorará y te manejará precios especiales indicando la 
                referencia de DeclaracionesIDE<br />
                <br />
                En registrarse tendrás acceso al <strong>contrato de distribuidores</strong>, 
                así mismo podrás disponer de los recursos que hemos diseñado para los 
                distribuidores.<br />
                <br />
                El registro de clientes así como la presentación de sus declaraciones en el 
                sistema, puede realizarla el cliente por si mismo o bien puede estar a cargo del 
                distribuidor, en este último caso, el distribuidor deberá usar un correo 
                distinto para administrar cada institución. Si es Ud. un distribuidor y desea 
                registrar Ud. mismo a sus instituciones financieras clientes, deberá registrar 
                cada una por separado (los datos de facturación son los de dichas 
                instituciones), y si desea que la facturación salga a nombre suyo envíenos un 
                correo indicandonos los datos de facturación.<br /> </span>
            </asp:View>
            <asp:View ID="View1" runat="server">
                <span class="style5distr"><strong><a name="registrarse">Registro de Distribuidor</a></strong></span><br />
                <br />
                <table class="style1" style="width: 88%">
                    <tr>
                        <td class="style48">
                            Nombre Fiscal</td>
                        <td>
                            <asp:TextBox ID="nombreFiscal" runat="server" Width="360px" MaxLength="250" 
                                CssClass="style46"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style48">
                            Banco</td>
                        <td>
                            <asp:TextBox ID="banco" runat="server" Width="360px" MaxLength="40" 
                                CssClass="style46"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style48">
                            Clabe interbancaria</td>
                        <td>
                            <asp:TextBox ID="clabe" runat="server" Width="360px" MaxLength="30" 
                                ToolTip="Se la proporciona su banco" CssClass="style46"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style48">
                            Número de cuenta</td>
                        <td>
                            <asp:TextBox ID="numCuenta" runat="server" CssClass="style46" MaxLength="30" 
                                ToolTip="Se la proporciona su banco" Width="360px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style48">
                            Ciudad y estado</td>
                        <td>
                            <asp:TextBox ID="ciudadYestado" runat="server" CssClass="style46" 
                                MaxLength="80" Width="360px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style48">
                            Teléfono con Lada</td>
                        <td>
                            <asp:TextBox ID="tel" runat="server" Width="360px" MaxLength="21" 
                                
                                ToolTip="solo use numeros y la coma sin espacios para agregar un segundo teléfono" 
                                CssClass="style46"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style48">
                            Correo</td>
                        <td>
                            <asp:TextBox ID="correo" runat="server" Width="360px" MaxLength="50" 
                                CssClass="style46"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style48">
                            Elije tu Contraseña de distribuidor</td>
                        <td>
                            <asp:TextBox ID="pass" runat="server" TextMode="Password" Width="360px" 
                                MaxLength="15" CssClass="style46"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style48">
                            Repite tu Contraseña de distribuidor</td>
                        <td>
                            <asp:TextBox ID="pass2" runat="server" TextMode="Password" Width="360px" 
                                MaxLength="15" CssClass="style46"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style21">
                            <span class="style47">Documentación</span></td>
                        <td>
                            <br class="style47" />
                            <span class="style47">Elije un archivo comprimido .zip de tu computadora de 
                            tamaño máximo 2Mb el cual contenga escaneados tu hoja de inscripción/registro en 
                            el RFC, IFE, CURP y comprobante de domicilio reciente para personas fisicas, o 
                            únicamente su hoja de inscripción/registro en el RFC y comprobante de domicilio 
                            reciente para personas morales.<br /> </span>(puedes 
                            especificar esta infomación posteriormente en &#39;Iniciar sesión como distribuidor&#39; 
                            si no dispones de ella ahora)<br /> 
                            </span>
                            </span>
                            <asp:FileUpload ID="FileUpload1" runat="server" Width="367px" 
                                CssClass="style46" />
                        </td>
                    </tr>
                    <tr class="style47">
                        <td class="style21">
                            &nbsp;</td>
                        <td>
                            <asp:CheckBox ID="acepto" runat="server" />
                            &nbsp;Acepto el <a href="contratoDistribuidores.aspx">Contrato para distribuidores</a>, del sitio web 
                            declaracioneside.com</td>
                    </tr>
                    <tr>
                        <td class="style48">
                            Comisiones</td>
                        <td>
                            <asp:RadioButtonList ID="clisForzosos" runat="server" style="font-size: small">
                                <asp:ListItem>15% sobre contratos nuevos y renovaciones, sujeto a haber ingresado a 1 cliente en el mes de renovaciones de contratos de clientes anteriores</asp:ListItem>
                                <asp:ListItem Selected="True">10% sobre contratos durante un año a partir de la fecha de registro de cada cliente prospectado</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style21">
                            <span class="style47">Facturas de clientes</span></td>
                        <td>
                            <asp:CheckBox ID="facturarAdistrib" runat="server" 
                                Text="¿Deseas que las facturas de tus clientes salgan a tu nombre?" 
                                ToolTip="¿y tu despues les facturas a ellos?" CssClass="style47" />
                            <br class="style47" />
                            <span class="style47">Datos de facturación (RFC y domicilio fiscal completo con 
                            cod. postal, ciudad y estado) :<br /> </span>
                            </span>
                            <asp:TextBox ID="datosFacturacion" runat="server" Height="41px" MaxLength="700" 
                                TextMode="MultiLine" Width="690px" CssClass="style46"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style21">
                            <span class="style47"></span></td>
                        <td>
                            <br />
                            </span>
                            <asp:Button ID="registrarme" runat="server" Text="Registrarme" 
                                CssClass="style47" />
                        </td>
                    </tr>
                </table>
                <br />
            </asp:View>
            <asp:View ID="View2" runat="server">
                <span class="style5distr"><strong><a name="iniciar"> Iniciar sesión como 
                distribuidor</a></strong></span><br />
                <br />
                <table class="style1" style="width: 88%">
                    <tr>
                        <td class="style22">
                            Correo</td>
                        <td>
                            &nbsp;<asp:TextBox ID="correo1" runat="server" Width="222px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style22">
                            Contraseña de distribuidor</td>
                        <td>
                            &nbsp;<asp:TextBox ID="pass5" runat="server" TextMode="Password" Width="218px" 
                                MaxLength="15"></asp:TextBox>
                            &nbsp;&nbsp;
                            <asp:Button ID="identificarme" runat="server" Text="Identificarme" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="Panel1" runat="server" Visible="False">
                    <table class="style1" style="width: 88%">
                        <tr class="style47">
                            <td class="style23">
                                # Distribuidor</td>
                            <td>
                                <asp:Label ID="id" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style49">
                                Nombre Fiscal</td>
                            <td>
                                <asp:TextBox ID="nombreFiscal1" runat="server" MaxLength="250" Width="360px" 
                                    CssClass="style46"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style49">
                                Banco</td>
                            <td>
                                <asp:TextBox ID="banco1" runat="server" Width="360px" MaxLength="40" 
                                    CssClass="style46"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style49">
                                Clabe interbancaria</td>
                            <td>
                                <asp:TextBox ID="clabe1" runat="server" Width="360px" MaxLength="30" 
                                    ToolTip="Se la proporciona su banco" CssClass="style46"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style49">
                                Número de cuenta</td>
                            <td>
                                <asp:TextBox ID="numCuenta1" runat="server" CssClass="style46" MaxLength="30" 
                                    ToolTip="Se la proporciona su banco" Width="360px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style49">
                                Ciudad y estado</td>
                            <td>
                                <asp:TextBox ID="ciudadYestado1" runat="server" CssClass="style46" 
                                    MaxLength="80" Width="360px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style49">
                                Teléfono con Lada</td>
                            <td>
                                <asp:TextBox ID="tel1" runat="server" Width="360px" MaxLength="21" 
                                    
                                    ToolTip="solo use numeros y la coma sin espacios para agregar un segundo teléfono" 
                                    CssClass="style46"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style49">
                                Correo</td>
                            <td>
                                <asp:TextBox ID="correo2" runat="server" Width="360px" MaxLength="50" 
                                    CssClass="style46"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style49">
                                Contraseña de distribuidor</td>
                            <td>
                                <asp:TextBox ID="pass6" runat="server" TextMode="Password" Width="360px" 
                                    MaxLength="15" CssClass="style46"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style49">
                                Confirmar contraseña de distribuidor</td>
                            <td>
                                <asp:TextBox ID="pass7" runat="server" TextMode="Password" Width="360px" 
                                    MaxLength="15" CssClass="style46"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style24">
                                <span class="style47">Documentación</span></td>
                            <td class="style53">
                                </span class="style47">Elije un archivo comprimido .zip de tu computadora de tamaño máximo 2Mb el cual contenga escaneados 
                                tu hoja de inscripción/registro en el RFC, IFE, CURP y comprobante de domicilio reciente para personas fisicas, o únicamente tu hoja de inscripción/registro en el RFC y comprobante de domicilio reciente para personas morales.<br />
                                </span>
                                <asp:FileUpload ID="FileUpload2" runat="server" Width="367px" 
                                    CssClass="style46" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style48">
                                Comisiones</td>
                            <td>
                                <asp:RadioButtonList ID="clisForzosos1" runat="server">
                                    <asp:ListItem>15% sobre contratos nuevos y renovaciones, sujeto a haber ingresado a 1 cliente en el mes de renovaciones de contratos de clientes anteriores</asp:ListItem>
                                    <asp:ListItem Selected="True">10% sobre contratos durante un año a partir de la fecha de registro de cada cliente prospectado</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style21">
                                <span class="style47">Facturas de clientes</span></td>
                            <td class="style47">
                                </span class="style47">
                                <asp:CheckBox ID="facturarAdistrib0" runat="server" 
                                    Text="¿Deseas que las facturas de tus clientes salgan a tu nombre?" 
                                    ToolTip="¿y tu despues les facturas a ellos?" />
                                <br />
                                Datos de facturación (RFC y domicilio fiscal completo con cod. postal, ciudad y 
                                estado) :<br />
                                </span>
                                <asp:TextBox ID="datosFacturacion0" runat="server" Height="41px" 
                                    MaxLength="700" TextMode="MultiLine" Width="690px" CssClass="style46"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="style47">
                            <td class="style24">
                                &nbsp;</td>
                            <td class="style8">
                                <asp:CheckBox ID="doctos" runat="server" Enabled="False" Text="Autorizado" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style24">
                            </td>
                            <td class="style8">
                                <asp:Button ID="mod" runat="server" Text="Modificar" CssClass="style47" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style24">
                                &nbsp;</td>
                            <td class="style8">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style24">
                                &nbsp;</td>
                            <td class="style8">
                                &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="linkProspeccion" runat="server">PROSPECCION</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:HyperLink ID="HyperLink15" runat="server" 
                                    NavigateUrl="~/guiaDistribuidores.docx">Guía para distribuidores</asp:HyperLink>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <br />
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style7" colspan="2">
                                &nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <br />
            </asp:View>
            <asp:View ID="View3" runat="server">
                <span class="style5distr"><strong><a name="listado"> Listado</a></strong></span><br />
                <br />
                <asp:Button ID="autorizar" runat="server" Text="Autorizar" 
                    ToolTip="y notificar" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="desAutorizar" runat="server" Text="desAutorizar" />
                &nbsp; <span class="style5distr"><strong>
                <asp:TextBox ID="titulo" runat="server">titulo</asp:TextBox>
                </strong></span>&nbsp;<asp:TextBox ID="mensaje" runat="server" Height="88px" 
                    style="margin-top: 0px" TextMode="MultiLine" Width="399px"></asp:TextBox>
                <asp:Button ID="enviarCorreo" runat="server" Text="Enviarles correo" />
                <br />
                <br />
                <asp:Button ID="esEmpleado" runat="server" Text="Set empleado" 
                    ToolTip="Marcar como empleado de nuestra empresa, Trabaja para DI" 
                    Width="83px" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="eliminar" runat="server" Text="Eliminar" />
                <br />
                <strong><span class="style47">COMISIONES<em>:&nbsp;&nbsp; </em></span></strong>
                <span class="style47">
                <asp:CheckBox ID="comisCaduca" runat="server" Checked="True" Text="Caduca" />
                </span><em><span class="style47"><strong>&nbsp; </strong>MesesCaducidad </span></em>
                <asp:TextBox ID="comisMesesCaducidad" runat="server" CssClass="style54" 
                    MaxLength="10" 
                    ToolTip="Meses iniciales tras cada registro de clientes por los que gozara de comisiones" 
                    Width="27px"></asp:TextBox>
                <strong><span class="style47">&nbsp; </span>
                <asp:TextBox ID="comisPorcen" runat="server" CssClass="style54" 
                    onblur="Javascript:ceros(this);formatoNumero(this,2,'.',',');" 
                    onkeypress="return numerosDec()" Width="56px"></asp:TextBox>
                <span class="style47">% </span> </strong>
                <asp:Button ID="modComision" runat="server" Text="Modificar" 
                    CssClass="style46" />
                <br class="style47" />
                <input type="hidden" id="scrollPos" runat="server" value="0" class="style46"/>
                <div ID="divScroll" runat="server" 
                    onscroll="javascript:document.getElementById('scrollPos').value = document.getElementById('divScroll').scrollTop;" 
                    style="overflow-y: auto;overflow-x: auto; height: 500px;width:100%">
                    <asp:GridView ID="GridView3" runat="server" 
                        AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False" 
                        DataKeyNames="nombreFiscal,clabe" DataSourceID="SqlDataSource3" 
                        ShowHeader="true" style="font-family: Arial; font-size: small">
                        <AlternatingRowStyle BackColor="#C2D69B" />
                        <selectedrowstyle backcolor="#990000" font-bold="false" forecolor="white" />
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" 
                                ReadOnly="True" SortExpression="id" />
                            <asp:BoundField DataField="nombreFiscal" HeaderText="nombre Fiscal" 
                                ReadOnly="True" SortExpression="nombreFiscal" />
                            <asp:BoundField DataField="banco" HeaderText="banco" ReadOnly="True" 
                                SortExpression="banco" />
                            <asp:BoundField DataField="clabe" HeaderText="clabe" SortExpression="clabe" />
                            <asp:BoundField DataField="ciudadYestado" HeaderText="ciudad Y estado" 
                                SortExpression="ciudadYestado" />
                            <asp:BoundField DataField="tel" HeaderText="tel" SortExpression="tel" />
                            <asp:BoundField DataField="correo" HeaderText="correo" 
                                SortExpression="correo" />
                            <asp:BoundField DataField="doctos" HeaderText="docs" SortExpression="doctos" />
                            <asp:BoundField DataField="clisForzosos" HeaderText="clisForzosos" 
                                SortExpression="clisForzosos" />
                            <asp:BoundField DataField="comisCaduca" HeaderText="comisCaduca" 
                                SortExpression="comisCaduca" />
                            <asp:BoundField DataField="comisMesesCaducidad" 
                                HeaderText="comisMesesCaducidad" SortExpression="comisMesesCaducidad" />
                            <asp:BoundField DataField="comisPorcen" HeaderText="comisPorcen" 
                                SortExpression="comisPorcen" />
                            <asp:BoundField DataField="esEmpleado" HeaderText="esEmpleado" 
                                SortExpression="esEmpleado" />
                            <asp:BoundField DataField="numCuenta" HeaderText="numCuenta" 
                                SortExpression="numCuenta" />
                        </Columns>
                        <HeaderStyle BackColor="#EDEDED" Height="26px" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:ideConnectionString2 %>" 
                        SelectCommand="SELECT id,nombreFiscal,banco,clabe,ciudadYestado,tel,correo,doctos,clisForzosos,comisCaduca,comisMesesCaducidad,comisPorcen,esEmpleado,numCuenta  FROM distribuidores ORDER BY nombreFiscal">
                    </asp:SqlDataSource>
                </div>
                <br />
                <asp:Label ID="nRegs" runat="server" Text="0 Registros"></asp:Label>
                <br />
                <br />
                Sin contraseña, dado que su información bancaria es sensible<br />
                <asp:Panel ID="Panel4" runat="server" BorderColor="#006666" BorderStyle="Solid" 
                    BorderWidth="1px" Height="1000px" 
                    style="position:relative; top: 25px; left: -1px; height: 906px; width: 867px;">
                    &nbsp;
                    <asp:LinkButton ID="LinkButton4" runat="server">Candidatos a Distribuidores</asp:LinkButton>
                    <br />
                    <br />
                    <table class="style1">
                        <tr>
                            <td class="style3">
                                &nbsp;</td>
                            <td>
                                ID&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="candId" runat="server" Text="ID"></asp:Label>
                                <br />
                                Nombre<asp:TextBox ID="candNombre" runat="server" Height="19px" MaxLength="60" 
                                    ToolTip="Usar nombre completo" Width="342px"></asp:TextBox>
                                <br />
                                Correo&nbsp;
                                <asp:TextBox ID="candCorreo" runat="server" Height="19px" MaxLength="50" 
                                    ToolTip="si no tiene correo, poner un 0" Width="342px"></asp:TextBox>
                                <br />
                                Tels&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:TextBox ID="candTels" runat="server" Height="19px" MaxLength="21" 
                                    
                                    ToolTip="solo use numeros y la coma sin espacios para agregar un segundo teléfono" 
                                    Width="342px"></asp:TextBox>
                                <br />
                                Ciudad&nbsp;
                                <asp:TextBox ID="candCiudad" runat="server" Height="16px" MaxLength="50" 
                                    Width="342px"></asp:TextBox>
                                <br />
                                Estatus&nbsp;
                                <asp:DropDownList ID="candEstatus" runat="server" 
                                    ToolTip="VAcio, NOtificado, LLamado">
                                    <asp:ListItem Value="VA">VA</asp:ListItem>
                                    <asp:ListItem Value="NO">NO</asp:ListItem>
                                    <asp:ListItem Value="LL">LL</asp:ListItem>
                                </asp:DropDownList>

                                <br />
                                Obs<asp:TextBox ID="candObservacion" runat="server" Height="95px" MaxLength="100" 
                                    TextMode="MultiLine" 
                                    
                                    ToolTip="caracteres prohibidos: ()',&quot;" 
                                    Width="363px"></asp:TextBox>
                                <br />
                            </td>
                            <td class="style14">
                                <br />
                                <br />
                                <br />
                                <asp:Button ID="addCand" runat="server" 
                                    style="height: 26px; font-size: small;" Text="+ Agregar" />
                                <br />
                                <br />
                                <asp:Button ID="editCand" runat="server" Text="(..) Modificar" />
                                <br />
                                <br />
                                <asp:Button ID="delCand" runat="server" 
                                    onclientclick="return confirm('¿Esta seguro de eliminar este registro?');" 
                                    Text="- Eliminar" />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td class="style14">
                                <asp:Button ID="invitar" runat="server" Text="Invitarlos" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="candidatosNregs" runat="server"></asp:Label>
                    <br />
                    <br />
                    <div style="height:25px;width:838px; margin:0; padding:0">
                        <table ID="Table2" bgcolor="#EDEDED" border="1" cellpadding="0" cellspacing="0" 
                            rules="all" style="border-collapse:collapse;height:100%;">
                            <tr>
                                <td style="width:75px;text-align:center">
                                </td>
                                <td style="width:30px;text-align:center">
                                    ID</td>
                                <td style="width:80px;text-align:center">
                                    Nombre</td>
                                <td style="width:80px;text-align:center">
                                    Correo</td>
                                <td style="width:80px;text-align:center">
                                    Tel</td>
                                <td style="width:80px;text-align:center">
                                    Ciudad</td>
                                <td style="width:50px;text-align:center">
                                    Estatus</td>
                                <td style="width:363px;text-align:center">
                                    Obs</td>
                            </tr>
                        </table>
                    </div>
                    <input type="hidden" id="scrollPos4" runat="server" value="0"/>
                    <div ID="divScroll4" runat="server" 
                        onscroll="javascript:document.getElementById('scrollPos4').value = document.getElementById('divScroll4').scrollTop;" 
                        style="overflow: auto; height: 498px; width:100%">
                        <asp:GridView ID="GridView4" runat="server" 
                            AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False" 
                            DataKeyNames="nombre" DataSourceID="SqlDataSource4" Height="498px" 
                            ShowHeader="False" Width="838px" 
                            style="font-family: Arial; font-size: small">
                            <AlternatingRowStyle BackColor="#C2D69B" />
                            <selectedrowstyle backcolor="#990000" font-bold="false" forecolor="white" />
                            <Columns>
                                <asp:CommandField ItemStyle-Width="75" ShowSelectButton="True" />
                                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" 
                                    ItemStyle-Width="30" ReadOnly="True" SortExpression="id" />
                                <asp:BoundField DataField="nombre" HeaderText="nombre" ItemStyle-Width="80" 
                                    ReadOnly="True" SortExpression="nombre" />
                                <asp:BoundField DataField="correo" HeaderText="correo" 
                                    ItemStyle-Width="80" ReadOnly="True" SortExpression="correo" />
                                <asp:BoundField DataField="tels" HeaderText="tels" 
                                    ItemStyle-Width="80" SortExpression="tels" />
                                <asp:BoundField DataField="ciudad" HeaderText="ciudad" 
                                    ItemStyle-Width="80" SortExpression="ciudad" />
                                <asp:BoundField DataField="estatus" HeaderText="estatus" ItemStyle-Width="50" 
                                    SortExpression="estatus" />
                                <asp:BoundField DataField="obs" HeaderText="obs" ItemStyle-Width="363" 
                                    SortExpression="obs" />
                            </Columns>
                            <HeaderStyle BackColor="#EDEDED" Height="26px" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:ideConnectionString %>" 
                            SelectCommand="SELECT * FROM [candidatoDistrib] ORDER BY nombre">
                        </asp:SqlDataSource>
                    </div>
                    <br />
                    &nbsp;
                </asp:Panel>
                <br />
                <br />
                <br />

            </asp:View>
            <asp:View ID="View4" runat="server">
                <asp:Panel ID="Panel3" runat="server" 
                            style="position:relative; top: 25px; left: -1px; height: 1660px; width: 100%;" 
                            BorderColor="#006666" BorderStyle="Solid" BorderWidth="1px">
                                    <span class="style47">&nbsp; <a name="prospeccion">
                                    <asp:LinkButton ID="LinkButton3" runat="server">PROSPECCION</asp:LinkButton>
                                    </a>&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; ID Dist&nbsp;<asp:Label ID="iddistribuidorLogged" runat="server"></asp:Label>
                                    &nbsp;&nbsp; <b><i style="text-align: left">Conceptos
                                    <asp:DropDownList ID="concepto" runat="server" CssClass="style46" 
                                        DataSourceID="SqlDataSource6" DataTextField="concepto" 
                                        DataValueField="concepto">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource6" runat="server" 
                                        ConnectionString="Data Source=IDESERVER;Initial Catalog=ide;Persist Security Info=True;User ID=usuario;Password='SmN+v-XzFy2N;91E170o'" 
                                        ProviderName="System.Data.SqlClient" 
                                        SelectCommand="SELECT [concepto] FROM [prospeccionConceptos]">
                                    </asp:SqlDataSource>
                                    </i></b>
                                    <br />

                                    </span>
                                    <asp:Panel ID="PanelBus" runat="server" GroupingText="Búsquedas" BackColor="#CCCCFF">                
                                    <span 
                                        class="style47">:
                                    <asp:CheckBox ID="chkProspecto" runat="server" style="font-size: small" 
                                        Text="Cliente (prospecto) con el texto" />
                                    </span>&nbsp;<asp:TextBox ID="prosNom" runat="server" CssClass="style46" 
                                        ToolTip="Introduzca todo o parte del nombre del prospecto" Width="343px"></asp:TextBox>
                                    <span class="style47">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox 
                                        ID="chkProgramadas" runat="server" style="font-size: small" 
                                        Text="Programadas p" />
                                    &nbsp;<asp:TextBox ID="fechaProg" runat="server" Width="82px"></asp:TextBox>
&nbsp;
                                    </span>
                                    &nbsp;&nbsp;
                                    <ajaxToolkit:CalendarExtender ID="fechaProg_CalendarExtender" runat="server" 
                                        Enabled="True" TargetControlID="fechaProg" CssClass="MyCalendar" Format="dd/MM/yyyy" ></ajaxToolkit:CalendarExtender>
                                    <br />
                                    <span class="style47">
                                    <asp:CheckBox ID="mios" runat="server" Text="Mios" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkNotas" runat="server" style="font-size: small" 
                                        Text="Notas con el texto" />
                                    <asp:TextBox ID="notasBus" runat="server" CssClass="style46" 
                                        ToolTip="ene, feb, mar, abr, may, jun, jul, ago, sep, oct, nov, dic. Opcionalmente digitos del año 13, 14, etc. Ejem: Ene 13 o Ene" 
                                        Width="246px"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkEstatus" runat="server" style="font-size: small" 
                                        Text="Estatus" />
                                    <asp:DropDownList ID="estatusBus" runat="server" CssClass="style46" 
                                        ToolTip="VA vacio, CO correo falta explicación, VL correo validado, LL llamado y explicado, RE renovar contrato, OK cliente al corriente, CC cierre caso cerrado, BA cliente dio baja">
                                        <asp:ListItem Value="VA">Vacio</asp:ListItem>
                                        <asp:ListItem Value="CO">Correo enviado</asp:ListItem>
                                        <asp:ListItem Value="VL">Correo confirmado</asp:ListItem>
                                        <asp:ListItem Value="LL">Llamando</asp:ListItem>
                                        <asp:ListItem Value="RE">Renovar contrato</asp:ListItem>
                                        <asp:ListItem Value="OK">OK al corriente</asp:ListItem>
                                        <asp:ListItem Value="CC">Caso cerrado</asp:ListItem>
                                        <asp:ListItem Value="BA">Baja</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkCorreo" runat="server" style="font-size: small" 
                                        Text="Correo" />
                                    <asp:TextBox ID="correoBus" runat="server" Width="177px"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
                                    <asp:CheckBox ID="chkDistr" runat="server" Text="Distrib" />
                                    <asp:TextBox ID="txtDistr" runat="server" Width="28px"></asp:TextBox>
                                    &nbsp;&nbsp;
                                    <asp:CheckBox ID="ultFecha" runat="server" Text="Ultima fecha" />
                                    &nbsp;<asp:TextBox ID="ultFechaBus" runat="server" Width="82px"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="ultFechaBus_CalendarExtender" runat="server" 
                                        Enabled="True" TargetControlID="ultFechaBus" CssClass="MyCalendar" Format="dd/MM/yyyy" ></ajaxToolkit:CalendarExtender>
&nbsp;
                                    <asp:CheckBox ID="sinCC" runat="server" style="font-size: small" Text="Sin CC" />
                                    &nbsp;
                                    <asp:CheckBox ID="sinVA" runat="server" style="font-size: small" Text="Sin VA" />
                                    &nbsp;
                                    <asp:CheckBox ID="sinCO" runat="server" style="font-size: small" Text="Sin CO" />
                                    &nbsp;
                                    <asp:CheckBox ID="sinLL" runat="server" style="font-size: small" Text="Sin LL" />
                                    &nbsp;
                                    <asp:CheckBox ID="sinRE" runat="server" style="font-size: small" Text="Sin RE" />
                                    &nbsp;
                                    <asp:CheckBox ID="sinVL" runat="server" style="font-size: small" Text="Sin VL" />
                                    &nbsp;
                                    <asp:CheckBox ID="sinOK" runat="server" style="font-size: small" Text="Sin OK" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="Buscar" runat="server" BackColor="#33CC33" CssClass="style46" 
                                        Text="Buscar" ToolTip="El resultado de carga en la tabla de abajo" />
                                    </span>
                                    </asp:Panel>
                                    
                                    <h3>Ordenar por:</h3>&nbsp; &nbsp;<asp:RadioButtonList 
                                        ID="orden" runat="server" RepeatDirection="Horizontal" Width="855px" 
                                        Height="26px" Style="font-size: small" BackColor="#CCCCFF">
                                        <asp:ListItem Value="1">Próx. llamada DESC</asp:ListItem>
                                        <asp:ListItem Value="4">Próx. llamada ASC</asp:ListItem>
                                        <asp:ListItem Value="2">Nombre prospecto</asp:ListItem>
                                        <asp:ListItem Value="3">Ultima Modificación DESC</asp:ListItem>
                                        <asp:ListItem Value="5">Ultima Modificación ASC</asp:ListItem>
                                    </asp:RadioButtonList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    Descr: <asp:TextBox ID="descr" runat="server" Width="544px"></asp:TextBox>
                                    <asp:HiddenField ID="hiddenBus" runat="server" />
                                    <asp:Button ID="ordenar" runat="server" BackColor="#33CC33" Text="Ordenar" />
                                        <asp:Panel ID="Panelreg" runat="server" GroupingText="Registro" BorderWidth="2pt">                
                                    <table>
                                        <tr>
                                            <td class="style47">
                                                ID</td>
                                            <td class="style63">
                                                <strong><em>Prospecto a Cliente</em></strong></td>
                                            <td class="style55">
                                                <b><i>Estatus</i></b></td>
                                            <td class="style19">
                                                <strong><em>Próxima llamada</em></strong></td>
                                            <td class="style47">
                                                <b><i># Distr.</i></b></td>
                                            <td class="style56">
                                                <em><strong>Notas</strong></em></td>
                                        </tr>
                                        <tr>
                                            <td class="style3">
                                                <asp:Label ID="idProspeccion" runat="server" Text="ID"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="cliente" runat="server" CssClass="style46" Height="150px" 
                                                    MaxLength="250" TextMode="MultiLine" 
                                                    ToolTip="Usar nombre completo, no usar acentos ya que generaría errores" 
                                                    Width="292px"></asp:TextBox>
                                                <br />
                                                <span class="style47"><strong><em>
                                                <br />
                                                </em></strong></span>
                                            </td>
                                            <td class="style59">
                                                <asp:DropDownList ID="estatusActual" runat="server" CssClass="style46" 
                                                    ToolTip="">
                                                    <asp:ListItem Value="VA">Vacio</asp:ListItem>
                                                    <asp:ListItem Value="CO">Correo enviado</asp:ListItem>
                                                    <asp:ListItem Value="VL">Correo confirmado</asp:ListItem>
                                                    <asp:ListItem Value="LL">Llamando</asp:ListItem>
                                                    <asp:ListItem Value="RE">Renovar contrato</asp:ListItem>
                                                    <asp:ListItem Value="OK">OK al corriente</asp:ListItem>
                                                    <asp:ListItem Value="CC">Caso cerrado</asp:ListItem>
                                                    <asp:ListItem Value="BA">Baja</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td >
                                                <span class="style47">
                                                <asp:TextBox ID="fechaIr" runat="server" Columns="10" MaxLength="10" Width="82px"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="fechaIr_CalendarExtender" runat="server" CssClass="MyCalendar" Enabled="True" Format="dd/MM/yyyy" TargetControlID="fechaIr">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:Button ID="irFecha" runat="server" Text="Ir fecha" />
                                                &nbsp; </span>
                                                <asp:Button ID="irHoy" runat="server" Text="Ir hoy" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="prevYear" runat="server" Text="&lt;&lt;" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="nextYear" runat="server" Text="&gt;&gt;" />
                                                <asp:Calendar ID="fechaProgramada" runat="server" DayNameFormat="Shortest" 
                                                    SelectionMode="Day" ShowGridLines="True" style="font-size: x-small">
                                                    <TodayDayStyle BackColor="silver" />
                                                    <SelectedDayStyle BackColor="Yellow" ForeColor="Red" />
                                                </asp:Calendar>
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <br />
                                                <br />
                                                <br />
                                                <span class="style47">
                                                <asp:Label ID="idDistribuidor" runat="server" Height="16px" 
                                                    style="text-align: center" Text="ID" Width="60px"></asp:Label>
                                                </span>
                                                <br />
                                                <br />
                                                <br />
                                            </td>
                                            <td>
                                                <span class="style47">
                                                <asp:TextBox ID="notas" runat="server" CssClass="style46" Height="150px" 
                                                    MaxLength="500" TextMode="MultiLine" 
                                                    ToolTip="Guardar siempre solo la última y vital información" Width="400px"></asp:TextBox>
                                                <br />
                                                <br />
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style47">
                                                &nbsp;</td>
                                            <td>
                                                <strong><em><span class="style47">Correos</span> </em></strong>
                                                <asp:TextBox ID="correoProspeccion" runat="server" CssClass="style46" 
                                                    Height="58px" MaxLength="100" TextMode="MultiLine" ToolTip="separados por coma" 
                                                    Width="276px"></asp:TextBox>
                                                <br />
                                            </td>
                                            <td class="style59">
                                                &nbsp;</td>
                                            <td class="style60">
                                                <span class="style47">
                                                <div class="style20">
                                                    <b><i style="text-align: left"><span class="style47">Tels/Correos inválidos:<br />
                                                    <asp:TextBox ID="telsInvalidos" runat="server" CssClass="style46" Height="64px" 
                                                        MaxLength="100" TextMode="MultiLine" ToolTip="separados por coma" Width="204px"></asp:TextBox>
                                                    </span></i></b>
                                                </div>
                                                </span>
                                            </td>
                                            <td colspan="2">
                                                <span class="style47"><strong><em>Tipo</em></strong>
                                                <asp:DropDownList ID="tipo" runat="server" CssClass="style46">
                                                    <asp:ListItem Value="1">PROSPECTO DIRECTO</asp:ListItem>
                                                    <asp:ListItem Value="2">INTERMEDIARIO</asp:ListItem>
                                                    <asp:ListItem Value="3">PUBLICIDAD CORREO</asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                                <br />
                                                <b><i>Ult.Modificación&nbsp;</i></b>
                                                <asp:Label ID="fecha" runat="server" style="text-align: center" Text="ID" 
                                                    Width="60px"></asp:Label>
                                                <br />
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                    </hr>
                                    </span>
                                    </td>
                                    <td>
                                    </td>
                                    <td colspan="3">
                                    </td>
                                    </tr>
                                    </table>
                                    </hr>
                                    </span>
                                    </td>
                                    <td class="style56">
                                        <span class="style47">
                                        <br />
                                        </i></b>
                                        <asp:Button ID="addPros" runat="server" BackColor="#99CCFF" CssClass="style61" 
                                            style="height: 26px; " Text=" + " />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="editPros" runat="server" BackColor="#99CCFF" CssClass="style61" 
                                            Text=" Mod " />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="delPros" runat="server" BackColor="#99CCFF" CssClass="style61" 
                                            onclientclick="return confirm('¿Esta seguro de eliminar este registro?');" 
                                            Text=" - " />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="Transferira" runat="server" BackColor="#99CCFF" 
                                            CssClass="style61" Text="Transferir a" />
                                        <asp:TextBox ID="transferido" runat="server" CssClass="style46" Width="49px"></asp:TextBox>
                                        </span>
                                    </td>
                                    </span>
                                    </td>
                                    </span>
                                    </tr>
                                    <tr>
                                        <td class="style47">
                                            &nbsp;</td>
                                        <td>
                                            <span class="style47">
                                            &nbsp;&nbsp;
                                            <asp:Button ID="anterior" runat="server" BackColor="#99CCFF" CssClass="style61" onclientclick="return confirm('¿Esta seguro de eliminar este registro?');" Text="&lt; Ant" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="siguiente" runat="server" BackColor="#99CCFF" CssClass="style61" onclientclick="return confirm('¿Esta seguro de eliminar este registro?');" Text="Sig &gt;" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            </span>
                                            <asp:Button ID="lim" runat="server" BackColor="#99CCFF" CssClass="style61" Style="height: 26px;" Text="Limp" />
                                            <br />
                                        </td>
                                    </tr>
                                    </table>
                                    </asp:Panel>
                                    <h3>Resultados:</h3>
                                    <asp:Label ID="prospeccionNregs" runat="server"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="exportarexcel" runat="server" Style="font-size: x-small; font-family: Arial, Helvetica, sans-serif;" Text="export Excel" />
                                    <br />
                                    <br />
                                    
                                    <input type="hidden" id="scrollPos3" runat="server" value="0" class="style46"/>
                                    <div ID="divScroll3" runat="server" 
                                        onscroll="javascript:document.getElementById('scrollPos3').value = document.getElementById('divScroll3').scrollTop;" 
                                        style="overflow: auto; height: 990px; width:100%">
                                        <asp:GridView ID="GridView1" runat="server" 
                                            AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False" 
                                            DataKeyNames="cliente" DataSourceID="SqlDataSource1" Height="498px" 
                                            ShowHeader="True" Width="1730px" 
                                            style="font-family: Arial; font-size: x-small" 
                                            onrowdatabound="OnRowDataBound" AllowPaging="False" 
                                            >
                                            <AlternatingRowStyle BackColor="#C2D69B" />
                                            <selectedrowstyle backcolor="#990000" font-bold="false" forecolor="white" />
                                            <Columns>
                                                <asp:CommandField ItemStyle-Width="75" ShowSelectButton="True" SelectText="Mostrar" ButtonType="Button"   ControlStyle-BackColor="#CCCCFF" >
                                                <ControlStyle Height="100%" Width="100%" />
                                                </asp:CommandField>
                                                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" 
                                                    ItemStyle-Width="30" ReadOnly="True" SortExpression="id" />
                                                <asp:BoundField DataField="cliente" HeaderText="cliente" ItemStyle-Width="200" 
                                                    ReadOnly="True" SortExpression="cliente" />
                                                <asp:BoundField DataField="estatusActual" HeaderText="estatus" ItemStyle-Width="50" 
                                                    SortExpression="estatusActual" />
                                                <asp:BoundField DataField="fecha" HeaderText="Ult.Modif" 
                                                    ItemStyle-Width="80" ReadOnly="True" SortExpression="fecha" />
                                                <asp:BoundField DataField="iddistribuidor" HeaderText="Distr." 
                                                    ItemStyle-Width="70" SortExpression="iddistribuidor" />
                                                <asp:BoundField DataField="notas" HeaderText="notas" 
                                                    ItemStyle-Width="150" SortExpression="notas" />
                                                <asp:BoundField DataField="correo" HeaderText="correo" 
                                                    ItemStyle-Width="150" ReadOnly="True" SortExpression="correo" />                                                                                                
                                                <asp:BoundField DataField="fechaProgramada" HeaderText="Prox. Llamada" ItemStyle-Width="80" 
                                                    SortExpression="fechaProgramada" />
                                                <asp:BoundField DataField="telsinvalidos" HeaderText="Tels invalidos" ItemStyle-Width="150" 
                                                    SortExpression="telsinvalidos" />
                                                <asp:BoundField DataField="tipo" HeaderText="tipo" ItemStyle-Width="100" 
                                                    SortExpression="tipo" />
                                            </Columns>
                                            <HeaderStyle BackColor="#EDEDED" Height="26px" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:ideConnectionString2 %>" 
                                            SelectCommand="SELECT id,cliente,idDistribuidor,estatusActual,fecha,notas,correo,fechaProgramada,telsInvalidos,tipo FROM prospeccion where id=-1">
                                        </asp:SqlDataSource>
                                    </div>
                                    <br />
                                    &nbsp;&nbsp;</asp:Panel>                        
            </asp:View>
            <asp:View ID="View6" runat="server">
                Exportando prospección en server con nombre = fecha de hoy, restaria 
                validarla/depurarla
                <asp:Button ID="exportar" runat="server" Text="Exportar" />
                <br />

        </asp:View>
        </asp:MultiView>
    </asp:Content>
