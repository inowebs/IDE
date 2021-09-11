<%@ Import Namespace="System.Data.OleDb" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="cliente.aspx.vb" Inherits="WebApplication1.WebForm4" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript" language="javascript">
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
    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ToolkitScriptManager1"></ajaxToolkit:ToolkitScriptManager>
    <div class="container">
        <div class="row">
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
                <h3 class="text-center">Cuenta
                </h3>
            </div>
            <div class="col-sm-12">
                <p>
                    Video de ayuda
                <a href="http://youtu.be/ZpeOxQg9SKo" target="_blank" style="color:#007bff">VideoTutorial</a>
                </p>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <p>
                    Para mas info de los cuadros, botones, enlaces, listas: coloque el ratón sobre ellos
                </p>
            </div>
        </div>
        <div class="row">
            <div class="container-fluid">
                <div class="row pb-1">
                    <div class="col-sm-3 text-right">Correo</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="correo" runat="server" MaxLength="50" ToolTip="Introduzca el correo que usara para acceder a este sistema y recibir notificaciones" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-3 text-right">Elija su Contraseña de cliente</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="passWeb" runat="server" MaxLength="15" TextMode="Password" ToolTip="seis caracteres o numeros mínimo" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-3 text-right">Denominación / Razón Social</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="razonSoc" runat="server" MaxLength="250" ToolTip="Razón social con la que está dado de alta en el SAT" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-3 text-right">Repita su Contraseña de cliente</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="passWeb2" runat="server" MaxLength="15" TextMode="Password" ToolTip="seis caracteres o numeros mínimo" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-3 text-right">Nombre de contacto</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="contacto" runat="server" MaxLength="100" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-3 text-right">Puesto</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="puesto" runat="server" MaxLength="50" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-3 text-right">Teléfono</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="tel" runat="server" MaxLength="40" ToolTip="Incluya clave lada" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-3 text-right">Celular</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="cel" runat="server" MaxLength="20" onkeypress="return numeros()" ToolTip="Incluya clave lada" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-3 text-right">PáginaWeb</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="paginaWeb" runat="server" MaxLength="200" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-3 text-right">RFC empresa declarante</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="rfcDeclarante" runat="server" MaxLength="12" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-3 text-right">Domicilio fiscal completo</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="domFiscal" runat="server" MaxLength="100" ToolTip="incluya colonia, C.P., población, mpio." CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-3 text-right">clave CASFIM o de Institución Financiera</div>
                    <div class="col-sm-3">
                        <div class="row p-1">
                            <div class="col-sm-12">
                                <asp:TextBox ID="casfim" runat="server" MaxLength="10" CssClass="form-control form-control-sm"
                                    onBlur="Javascript:actualizaDirectorioServidor(this);"
                                    ToolTip="La clave CASFIM de su institución, especifíquela correctamente, pues de lo contrario el SAT no podrá autorizarnos su socket para que pueda enviar declaraciones, si aún no lo tiene, introduzca provisionalmente cualquier clave numerica pero no olvide actualizarla por la oficial de su institución al ingresar en 'Mi cuenta'"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row p-1">
                            <div class="col-sm-12">
                                <asp:CheckBox ID="casfimProvisional" runat="server" Text="¿es clave Provisional?" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-3 text-right">¿Son un banco?</div>
                    <div class="col-sm-3">
                        <p>
                            <asp:CheckBox ID="esInstitCredito" runat="server" />
                            si no son un banco déjelo en blanco
                        </p>
                    </div>
                    <div class="col-sm-3 text-right">¿Como te enteraste de nosotros?</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="fuente" runat="server" MaxLength="120" TextMode="MultiLine" ToolTip="Sea lo mas específico posible, max. 120 caracteres" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-6"> Otros correos de facturación: <asp:TextBox ID="facCorreos" runat="server" MaxLength="200" CssClass="form-control form-control-sm"></asp:TextBox></div>
                    <div class="col-sm-3"> <asp:CheckBox ID="facRetens" runat="server" Text="Factura con Retenciones" /></div>
                    <div class="col-sm-3"> <asp:CheckBox ID="facTercero" runat="server" Text="Factura un tercero" ToolTip="Habilitela si no se va a facturar a los datos de la institucion financiera de las declaraciones" AutoPostBack="true" /></div>                    
                </div>

                <asp:Panel runat="server" ID="facPanel" Visible="false" GroupingText="Datos de facturación del tercero:" BorderWidth="1px">
                    <div class="row pb-1">                   
                           <div class="col-sm-12"><asp:TextBox ID="dxFac" runat="server" MaxLength="500" TextMode="MultiLine" ToolTip="500 caracteres max." CssClass="form-control form-control-sm" Visible="false"></asp:TextBox></div>                    
                    </div>
                    <div class="row pb-1">                   
                            <div class="col-sm-3">RFC:<asp:TextBox ID="facRfc" runat="server" MaxLength="13" CssClass="form-control form-control-sm"></asp:TextBox></div>
                            <div class="col-sm-6">RazonSocial:<asp:TextBox ID="facRazon" runat="server" MaxLength="250" CssClass="form-control form-control-sm"></asp:TextBox></div>
                            <div class="col-sm-3">Uso CFDI:<asp:DropDownList ID="facUso" runat="server" CssClass="form-control form-control-sm" DataSourceID="SqlDataSourceUso" DataTextField="descripcion" DataValueField="clave" ></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSourceUso" runat="server" ConnectionString="<%$ ConnectionStrings:ideConnectionString %>" SelectCommand="SELECT * FROM [c_UsoCFDI] order by id"></asp:SqlDataSource>
                            </div>
<%--                            <div class="col-sm-3"><asp:DropDownList ID="facFP" runat="server" CssClass="form-control form-control-sm" DataSourceID="SqlDataSourceFP" DataTextField="descripcion" DataValueField="clave" ></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSourceFP" runat="server" ConnectionString="<%$ ConnectionStrings:ideConnectionString %>" SelectCommand="SELECT * FROM [c_formaPago] order by id"></asp:SqlDataSource>
                            </div>                    --%>
                    </div>
                </asp:Panel>
                <div class="row pb-1">
                    <div class="col-sm-3 text-right"># Sucursales</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="numSucursales" runat="server" onblur="Javascript:ceros(this);formatoNumero(this,0,'.',',');" onkeypress="return numeros()" CssClass="form-control form-control-sm">0</asp:TextBox>
                    </div>
                    <div class="col-sm-3 text-right"># Socios o Clientes</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="numSociosClientes" runat="server" onblur="Javascript:ceros(this);formatoNumero(this,0,'.',',');" onkeypress="return numeros()" CssClass="form-control form-control-sm">0</asp:TextBox>
                    </div>                    
                </div>
                <div class="row pb-1">
                    <div class="col-sm-3 text-right" style="display:none">Número de Distribuidor (opcional)</div>
                    <div class="col-sm-3">
                        <div class="row pb-1">
                            <div class="col-sm-12">
                                <asp:TextBox ID="idDistribuidor" runat="server" AutoPostBack="True" ToolTip="Le es proporcionado por el personal que lo introdujo al sistema" onkeypress="return numeros()" Enabled="False" CssClass="form-control form-control-sm" Visible="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row pb-1">
                            <div class="col-sm-12">
                                <asp:CheckBox ID="rfcComodinPm" runat="server" Text="¿Reemplazar el RFC de personas morales a declarar cuya longitud sea  de 9 caracteres por el RFC comodín del SAT de 12 caracteres? " Style="font-size: x-small" />
                                <asp:CheckBox ID="chkNombreFull" runat="server" Text="¿Usar nombre completo en 1 sola columna en layouts anuales con datos? " Style="font-size: x-small" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        
                    </div>
                    <div class="col-sm-3">
                        
                    </div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-3 text-right">
                        
                    </div>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="clientesEstatus" runat="server" DataSourceID="SqlDataSourceEdoCli" DataTextField="estatus" DataValueField="id" CssClass="form-control form-control-sm"></asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceEdoCli" runat="server" ConnectionString="<%$ ConnectionStrings:ideConnectionString %>" SelectCommand="SELECT * FROM [estatusCliente] order by id"></asp:SqlDataSource>
                    </div>
                    <div class="col-sm-6 text-center">
                        <asp:Button ID="mod" runat="server" Text="Modificar" ToolTip="Guardar cambios" CssClass="btn btn-lg btn-info p-1 rounded" />
                    </div>
                </div>
            </div>
        </div>
        <hr class="bg-dark" style="border-width: 1px" />
        <asp:Panel ID="Panel1" runat="server" CssClass="row">
            <div class="container-fluid">
                <div class="row pb-1">
                    <div class="col-sm-3">
                        <asp:Button ID="validaAutorizacion" runat="server" Text="Validar carta" ToolTip="Notificar cliente." CssClass="btn btn-sm btn-info p-1 rounded" />
                        
                        &nbsp;&nbsp;
                        
                        <asp:Button ID="desValidaCarta" runat="server" Text="DesValidar carta"  CssClass="btn btn-sm btn-info p-1 rounded" />
                    </div>
                    <div class="col-sm-3">Otros correos: </div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="otrosCorreos" runat="server" MaxLength="200" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-3">
                        <asp:Button ID="btnActOtros" runat="server" Text="Act.Otros" CssClass="btn btn-sm btn-info p-1 rounded" />
                    </div>
                </div>
                <div class="row  pb-1">
                    <div class="col-sm-3 text-right">Directorio Servidor</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="directorioServidor" runat="server" AutoPostBack="True" MaxLength="20" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-3 text-right">Fecha de Registro</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="fechaRegistro" runat="server" MaxLength="10" ToolTip="dd/mm/aaaa" Enabled="False" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-3 text-right">solSocketStatus</div>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="solSocketEstatus" runat="server" AutoPostBack="True" CssClass="form-control form-control-sm">
                            <asp:ListItem Value="VACIA">Vacía</asp:ListItem>
                            <asp:ListItem Value="RECIBIDA">Recibida</asp:ListItem>
                            <asp:ListItem Value="VALIDADA">Validada</asp:ListItem>
                            <asp:ListItem Value="RECHAZADA">Rechazada</asp:ListItem>
                            <asp:ListItem Value="APROBADA">Aprobada</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-3">
                        <div class="row pb-1">
                            <div class="col-sm-12 text-right">fechaSolSocketSat</div>
                        </div>
                        <div class="row pb-1">
                            <div class="col-sm-12 text-right">
                                <asp:Button ID="solSocket" runat="server" Text="Solicitar Socket" CssClass="btn btn-sm btn-info p-1 rounded" />
                            </div>
                        </div>
                        <div class="row pb-1">
                            <div class="col-sm-12 text-right">
                                <asp:Button ID="confirmarRecibida" runat="server" Text="confirmarRecibida" ToolTip="se llamo al SAT p validar recepcion sol socket" CssClass="btn btn-sm btn-info p-1 rounded" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="row">
                            <div class="col-sm-6">
                                <asp:TextBox ID="fechaSolSocketSat" runat="server" Enabled="False" MaxLength="10" ToolTip="dd/mm/aaaa" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6">
                                <asp:CheckBox ID="solSockConfirmadaSAT" runat="server" Enabled="False" Text="confirmado SAT" />
                            </div>
                        </div>
                    </div>
                </div>
                <hr class="bg-dark" style="border-width: 1px" />
                <div class="row pb-1">
                    <div class="col-sm-3 text-right">ipRemotaSAT</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="ipSat" runat="server" MaxLength="15" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-3 text-right">usuarioRemotoSAT</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="loginSAT" runat="server" MaxLength="20" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-3 text-right">directorioRemoSAT</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="directorioSat" runat="server" MaxLength="40" ToolTip="sin / al final (usuarioRemoto/Declaraciones)" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-3 text-right">
                        Archivo .conf del socket:
                    </div>
                    <div class="col-sm-3">
                        <asp:FileUpload ID="FileUpload2" runat="server" ToolTip="Descargar el .conf del socket recibido por correo, renombrarlo quitando lo que sigue del .conf, importante hacerlo en el orden que vayan llegando los sockets" />
                    </div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-3 text-right">fecha próx Prueba</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="fechaPrueba" runat="server" MaxLength="10" ToolTip="calcular 3 dias habiles posteriores a la recepción del socket, dd/mm/aaaa" CssClass="form-control form-control-sm"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="fechaPrueba_CalendarExtender" runat="server" TargetControlID="fechaPrueba" CssClass="MyCalendar bg-white rounded" Format="dd/MM/yyyy">
                        </ajaxToolkit:CalendarExtender>
                    </div>
                    <div class="col-sm-6" />
                </div>
                <hr class="bg-dark" style="border-width: 1px" />
                <div class="row pb-1">
                    <div class="col-sm-6">
                        <div class="row">
                            <div class="col-sm-12">
                                <p>
                                    <asp:Button ID="modAdmin" runat="server" Text="(..) Modificar" ToolTip="Nos envia correo de la proxima prueba, si la hay" CssClass="btn btn-sm btn-info p-1 rounded" />
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <p>

                                    <asp:Button ID="prueba" runat="server" Text="PruebaSocketEnvio" CssClass="btn btn-sm btn-info p-1 rounded" />
                                    <asp:Button ID="aprobarEnvio" runat="server" Text="aprobarEnvio" CssClass="btn btn-sm btn-info p-1 rounded" />
                                    <asp:CheckBox ID="transmisionOk" runat="server" Text="Aprobado" Enabled="False" />
                                </p>
                            </div>
                            <div class="col-sm-12">
                                <asp:TextBox ID="pruebaResultado" runat="server" TextMode="MultiLine" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row">
                            <div class="col-sm-12">
                                <br>
                                <br>
                                <br></br>
                                <br></br>
                                </br>
                                </br>                                 
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <p>
                                    <asp:Button ID="pruebaAcuse" runat="server" Text="PruebaSocketAcuse" CssClass="btn btn-sm btn-info p-1 rounded" />
                                    <asp:Button ID="aprobarRecepcion" runat="server" Text="aprobarRecepcion" CssClass="btn btn-sm btn-info p-1 rounded" />
                                    <asp:CheckBox ID="recepcionOk" runat="server" Text="Aprobado" Enabled="False" />
                                </p>
                            </div>
                            <div class="col-sm-12">
                                <asp:TextBox ID="acuseResultado" runat="server" TextMode="MultiLine" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row pb-1">
                    <div class="col-sm-6">
                        <asp:Button ID="notifica" runat="server" Text="Notificar cliente" ToolTip="Actualiza estatus socket" CssClass="btn btn-sm btn-info p-1 rounded" />
                    </div>
                    <div class="col-sm-3">
                        <asp:CheckBox ID="contactadoPguiarDecl" runat="server" Text="Contactado p guiar declaración" Enabled="False" />
                    </div>
                    <div class="col-sm-3">
                        <asp:Button ID="guiarDecla" runat="server" Text="Si" CssClass="btn btn-sm btn-info p-1 rounded" />
                    </div>
                </div>
            </div>
        </asp:Panel>
        <hr class="bg-dark" style="border-width: 1px" />
    </div>
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <div class="container">
                <asp:Panel ID="Panel2" runat="server" CssClass="row">
                    <div class="container-fluid">
                        <div class="row pb-1">
                            <div class="col-sm-12">
                                <h4 class="text-center">Representante Legal
                                </h4>
                            </div>
                            <div class="col-sm-12">
                                <p>
                                    (Si después cambia el representante legal, anotelo abajo, Agregar, Definir como actual)
                                </p>
                            </div>
                        </div>
                        <div class="row pb-1">
                            <div class="col-sm-2">
                                Id Actual:
                            </div>
                            <div class="col-sm-10">
                                <asp:Label ID="actualRepr" runat="server" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div class="row pb-1">
                            <div class="col-sm-10 pb-1">
                                <div class="row">
                                    <div class="col-sm-2">Nombre(s)</div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="nombres" runat="server" MaxLength="40" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">Apellido Paterno</div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="ap1" runat="server" MaxLength="40" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">Apellido Materno</div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="ap2" runat="server" MaxLength="40" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="row">
                                    <p>
                                        <asp:Button ID="add" runat="server" Text="Agregar" CssClass="btn btn-sm btn-info p-1 rounded" />
                                        <asp:Button ID="edit" runat="server" Text="Modificar" CssClass="btn btn-sm btn-info p-1 rounded" />
                                    </p>
                                </div>
                            </div>
                            <div class="col-sm-10">
                                <div class="row">
                                    <div class="col-sm-2">ID</div>
                                    <div class="col-sm-2">
                                        <asp:Label ID="idReprLeg" runat="server" Text="# Ninguno" Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="col-sm-2">RFC</div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="rfc" runat="server" MaxLength="13" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">CURP</div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="curp" runat="server" MaxLength="30" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="row">
                                    <p>
                                        <asp:Button ID="del" runat="server" OnClientClick="return confirm('¿Confirma eliminación?');" Text="Eliminar" CssClass="btn btn-sm btn-info p-1 rounded" />
                                        <asp:Button ID="defActual" runat="server" Text="Definir actual" CssClass="btn btn-sm btn-info p-1 rounded" />
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="row pb-1">
                            <div id="divScroll" runat="server" class="col-sm-12 scroll scroll4" style="max-height: 300px; width:100%">
                                <asp:GridView ID="GridView2" runat="server" Width="100%"
                                    AlternatingRowStyle-BackColor="#C2D69B" AutoGenerateColumns="False"
                                    DataSourceID="SqlDataSource2"
                                    Style="font-family: Arial; font-size: small" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                                    <Columns>
                                        <asp:CommandField ItemStyle-Width="75" ShowSelectButton="True" ButtonType="Button" />
                                        <asp:BoundField DataField="id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                                        <asp:BoundField DataField="nombres" HeaderText="Nombre(s)" SortExpression="nombres" />
                                        <asp:BoundField DataField="ap1" HeaderText="Apellido Paterno" SortExpression="ap1" />
                                        <asp:BoundField DataField="ap2" HeaderText="Apellido Materno" SortExpression="ap2" />
                                        <asp:BoundField DataField="rfc" HeaderText="RFC" SortExpression="rfc" />
                                        <asp:BoundField DataField="curp" HeaderText="CURP" SortExpression="curp" />
                                    </Columns>
                                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="white" />
                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                    <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                    <SortedDescendingHeaderStyle BackColor="#242121" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:ideConnectionString2 %>"
                                    SelectCommand="SELECT * FROM [reprLegal]"></asp:SqlDataSource>
                                <span class="style9master">
                                    <asp:Label ID="reprLegNregs" runat="server"></asp:Label>
                                </span>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </asp:View>
    </asp:MultiView>
    <div class="container">
        <hr />
        <div class="row pb-1">
            <div class="col-sm-12">
                <h5>Autorización de socket
                </h5>
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-2">
                <asp:Button ID="solVerFormato" runat="server" Text="1. Bajar formato" ToolTip="Guarde este docto. en su computador (Guardar como), reemplace en él los datos enmarcados entre los símbolos &lt;&gt;, agregue su membrete, imprimalo y firma de su repres. legal, en seguida guarde en su computador la imagen escaneada que contiene el documento con la firma como tipo .pdf y un tamaño maximo de 350Kb (0.35Mb)" CssClass="btn btn-sm btn-info p-1 rounded" />
            </div>
            <div class="col-sm-6">
                <p>
                    Subir archivo 
                <asp:FileUpload ID="FileUpload1" runat="server" ToolTip="Elija un archivo .pdf de máximo 350Kb (0.35Mb). Elija el archivo escaneado según se indicó en el boton 'Ver formato', una vez recibida y validada esta solicitud, tramitaremos ante el SAT la asignación de su socket y matriz de conexion segura para poder iniciar el envío de las declaraciones del IDE, cuyo proceso lleva aprox. de 1 a 2 semanas" />
                </p>
            </div>
            <div class="col-sm-2">
                <asp:Button ID="solSubir" runat="server" Text="2. Subir autorización" CssClass="btn btn-sm btn-info p-1 rounded" />
            </div>
            <div class="col-sm-2">
                <asp:Button ID="mostrarSol" runat="server" Text="3. Mostrar" ToolTip="Muestra el archivo que ud. subió" CssClass="btn btn-sm btn-info p-1 rounded" />
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-12"><strong>Descargas</strong></div>
            <div class="col-sm-12">
                <p>
                    Utilice el navegador<strong>Chrome o Firefox </strong>para subir su autorización y para declarar, puede <a href="https://www.google.com/intl/es/chrome/browser/?hl=es" target="_blank" style="color:#007bff" title="Tras instalar el navegador, abralo y en la barra de direcciones superior, ingrese a declaracioneside.com y vuelva a esta sección tras iniciar sesión para subir su autorización o envíela a nuestro correo">descargarlo aquí</a> e instalarlo, ingrese nuevamente a esta página desde ese navegador.
                </p>
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-12">
                <p>
                    <asp:LinkButton ID="ayuda2" runat="server" style="color:#007bff" ToolTip="Cómo declarar">Ayuda desde 2014</asp:LinkButton>                    
                    <br/> 
                    <asp:LinkButton ID="ayuda" runat="server" style="color:#007bff" ToolTip="Cómo declarar" >Ayuda previa a 2014</asp:LinkButton>
                </p>
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-12">
                <iframe id="frame1" runat="server" frameborder="0" height="900px" style="max-height: 1200px" name="I1" scrolling="auto" width="100%"></iframe>
            </div>
            <div class="col-sm-6">
                
            </div>
            <div class="col-sm-6">
                <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="politicas.aspx" Visible="False">Términos del servicio, políticas de uso</asp:HyperLink>
            </div>
        </div>
        <div class="row pb-1">
            <div class="col-sm-12">
                <p>
                    Cuando Ud. presenta inactividad una vez iniciada su sesión, es posible que el sistema lo detecte y observe errores, por lo que Ud. requerirá ir al menú -> Inicio, y luego cerrar e iniciar sesión.
                </p>
            </div>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        function ceros(campo) {
            if (document.getElementById(campo.id).value == "") {
                document.getElementById(campo.id).value = "0";
            }
        }

        function actualizaDirectorioServidor(campo) {
            document.getElementById('<%=directorioServidor.clientID %>').value = document.getElementById(campo.id).value;
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

</asp:Content>
