<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="faq.aspx.vb" Inherits="WebApplication1.faq" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="canonical" href="https://www.declaracioneside.com/faq.aspx" />
    <style type="text/css">
        /***********************************************/
        /***************** Accordion ********************/
        /***********************************************/
        @import url('https://fonts.googleapis.com/css?family=Tajawal');
        @import url('https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css');       

     .accordion h1,
        .accordion a {
            color: #0056B3;
        }

        .accordion .btn-link {
          font-weight: 400;
            color: #0056B3;
            background-color: transparent;
            text-decoration: none !important;
            font-size: 16px;
            font-weight: bold;
            padding-left: 25px;
        }
         
        .accordion .btn-link:focus {
            color: #007b5e;
        }
        .accordion .card-body {
            border-top: 2px solid #007b5e;
        }

        .accordion .card-header .btn.collapsed .fa.main {
            display: none;            
        }

        .accordion .card-header .btn .fa.main {
            background: #007b5e;
            padding: 13px 11px;
            color: #ffffff;
            width: 35px;
            height: 41px;
            position: absolute;
            left: -1px;
            top: 10px;
            border-top-right-radius: 7px;
            border-bottom-right-radius: 7px;
            display: block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="page-wrapper section pt-0">
        <div class="container">
            <div class="row">
                <div class="col-md-4">
                    <h4>Preguntas Frecuentes</h4>
                    <p>Bienvenido a las preguntas frecuentes de declaraciones de depósitos en efectivo e IDE, donde aclaramos dudas para presentar aquí tu declaración informativa de depósitos en efectivo. </p>
                    <p>Puedes enviarnos un correo con tu pregunta o solicitud haciendo clic <a href="#" data-toggle="modal" data-target="#contactanosModal">Aqui</a></p>
                </div>
                <div class="col-md-8">
                    <div class="accordion" id="accordionExample">
                        <div class="card">
                            <div class="card-header" id="headingOne">
                                <h5 class="mb-0">
                                    <button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                        <i class="fa fa-user"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Qué servicios y recursos me ofrecen? 
						
                                    </button>
                                </h5>
                            </div>

                            <div id="collapseOne" class="collapse show fade" aria-labelledby="headingOne" data-parent="#accordionExample">
                                <div class="card-body">
                                    Esencialmente es un servicio en línea para que las instituciones financieras que reciben depósitos en efectivo puedan realizar sus declaraciones informativas de depósitos en efectivo y del impuesto IDE satisfaciendo los lineamientos tecnológicos requeridos por el SAT .<a href="Default.aspx" class="ml-3" target="_blank"><strong><i class="fa fa-angle-double-right"></i></strong>Toda la información aquí</a>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Quienes pueden usar este servicio?
						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Todas aquellas instituciones del sistema financiero que por decreto del SAT tengan obligación de informar depósitos en efectivo o bien de recaudar el impuesto del IDE, entre otras se incluyen:</p>
                                    <ul style="list-style: none">

                                        <li>* Sociedades cooperativas de ahorro y préstamos (cajas populares y eclesiásticas).</li>
                                        <li>* Uniones de crédito.</li>
                                        <li>* Instituciones de seguros.</li>
                                        <li>* Sociedades financieras.</li>
                                        <li>* Instituciones de crédito(bancos no auxiliares de la TESOFE).</li>
                                        <li>* Administradoras de fondos para el retiro (afores).</li>
                                        <li>*Sociedades de inversión </li>
                                        <li>*Organismos de integración financiera rural</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo2">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo2" aria-expanded="false" aria-controls="collapseTwo2">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Qué tipo de declaraciones puedo presentar en declaracioneside.com? 
						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo2" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Declaraciones informativas mensuales y anuales de depósitos en efectivo y de IDE, en ceros o con datos, normales y complementarias.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo3">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo3" aria-expanded="false" aria-controls="collapseTwo3">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Cuáles son los pasos para registrarse?						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo3" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>a) Tener o tramitar la clave CASFIM de la institución financiera y su dirección de correo electrónico, si no la tienes, te asesoramos.</p>
                                    <p>b) Registrarse y subir una carta de autorización para que seamos su proveedor para el envío de declaraciones y para trámitarle de su matríz de conexión segura, según se detalla cuando inicia sesión e ingresa a 'Mi cuenta'.</p>
                                    <p>c) Una vez validemos tu carta de autorización, podrás contratar en línea lo que necesites.</p>
                                    <p>d) Procedemos a tramitarle ante el SAT la matriz de conexión y a configurar el socket de comunicación para su uso, dicho tramite dura una semana aprox., lo cuál le será notificado vía correo electrónico para que comience a declarar inmediatamente con nuestro sistema. </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo4">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo4" aria-expanded="false" aria-controls="collapseTwo4">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Puedo enviar declaraciones anteriores a través de este sistema?						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo4" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Sí, esto les permite poner al día su situación fiscal y evitarse mas multas. <a href="#" class="ml-3" data-toggle="modal" data-target="#contactanosModal"><strong><i class="fa fa-angle-double-right"></i></strong>Contáctanos</a></p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo5">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo5" aria-expanded="false" aria-controls="collapseTwo5">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Es necesario que tenga contratada una IP fija para declarar depósitos en efectivo y/o IDE?						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo5" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>En declaracioneside.com te ofrecemos el servicio en línea, no requires invertir en infraestructura ni en una IP fija.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo6">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo6" aria-expanded="false" aria-controls="collapseTwo6">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Requiero elaborar el archivo XML para la declaración de depósitos en efectivo?						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo6" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Declaracioneside.com se encarga de generar la información hacia un formato XML válido por el SAT ya sea que tu declaración sea en ceros o bien que la importes desde tus archivos de excel. Opcionalmente si ya dispones de tu archivo XML puedes subirlo y enviarlo directamente usando nuestra infraestructura. </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo7">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo7" aria-expanded="false" aria-controls="collapseTwo7">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Por que requiero un servicio como este?						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo7" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>¿Cuál es el fundamento legal? </p>

                                    <p>Depositos en efectivo 2014 ISR:</p>
                                    <ul style="list-style: none">
                                        <li>- Artículo 55, fracción IV de la Ley del Impuesto sobre la Renta</li>
                                        <li>- Regla I.3.5.8. de la Resolución Miscelánea Fiscal para 2014</li>
                                        <li>- Artículo Vigésimo Sexto Transitorio de la RMF para 2014</li>
                                    </ul>
                                    <p>
                                        IDE:
                                    </p>
                                    <p>
                                        La declaración provisional o definitiva que las instituciones financieras realizan vía la página del SAT por línea de captura de pago referenciado es previo a pagar el IDE en caso de que tuviesen que pagarlo.
Posteriormente tienen la obligación de enviar las declaraciones informativas mensuales y anuales del IDE, sean con datos o en ceros, en la lista de obligaciones del siguiente enlace, en el punto cuatro 'informar mensualmente al SAT' es donde entra nuestra solución, obsérvese el aviso referente a declaraciones de IDE en ceros: 'Tratándose de instituciones del sistema financiero que durante uno o varios meses no recauden IDE, ni tengan IDE pendiente de recaudar, deberán informar mensualmente mediante la forma electrónica denominada IDE-M “Declaración informativa mensual del impuesto a los depósitos en efectivo”, sin operaciones, por el periodo de que se trate'.
                                    </p>
                                    <p>
                                        Dicha forma electrónica fue reemplazada por un sistema informático para envío de declaraciones como el nuestro y el punto cinco se refiere a informar anualmente, donde también entra nuestra solución, vea <a href="http://www.sat.gob.mx/sitio_internet/informacion_fiscal/rf2008/137_12265.html">http://www.sat.gob.mx/sitio_internet/informacion_fiscal/rf2008/137_12265.html</a>
                                        En el punto cuatro, está el siguiente enlace donde dice cómo deben realizar las declaraciones informativas (nuestra solución se apega a ello) Declaraciones Informativas del IDE por parte de las Instituciones del Sistema Financiero 
en donde se señala el mecanismo para llevarlo a cabo (nuestra solución) Mecanismo para presentar las declaraciones informativas
                                    </p>
                                    <p>
                                        Todo lo anterior en cumplimiento a la ley del IDE articulo 4, fracciones III y VII que puede consultarse en  
                                        <a href="http://www.diputados.gob.mx/LeyesBiblio/pdf/LIDE.pdf">http://www.diputados.gob.mx/LeyesBiblio/pdf/LIDE.pdf</a>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo8">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo8" aria-expanded="false" aria-controls="collapseTwo8">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿El pago referenciado sustituye a la declaración informativa de IDE?						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo8" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Nó, el pago referenciado y su pago son solo parte del proceso fiscal, éste debe completarse con el envío de declaraciones informativas del IDE, véase este <a href="#" onclick="videoman_onClick('images/blog/pagoReferenciadoVsDeclaracion.jpg', 'Extracto 1')" data-toggle="modal" data-target="#modalvideo">extracto 1</a>, <a href="#" onclick="videoman_onClick('images/blog/pagoReferenciadoVsDeclaracion1.jpg', 'Extracto 2')" data-toggle="modal" data-target="#modalvideo">extracto 2</a> </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo10">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo10" aria-expanded="false" aria-controls="collapseTwo10">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Cómo puedo tramitar la clave Casfim, clave IDE o clave de institución financiera? 						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo10" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Solicita una <a href="#" data-toggle="modal" data-target="#contactanosModal">asesoría especializada</a>, este servicio es gratuito para quienes se registren en este portal. Esta clave de 5 dígitos la asigna el SAT para dar de alta la institución en su catálogo, es indispensabe para enviar las declaraciones de IDE. En muchos casos es posible que no conozcas que clave CASFIM o clave de institución financiera asignó el SAT a tu institución, nosotros realizamos una investigación de ello para ti o bien te orientamos para tramitarla. La clave IDE, es normalmente de 5 digitos, no la confundas con la clave de institucion financiera de 6 digitos que se usa para los reportes que se usan con la CNBV comision nacional bancaria y de valores</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo11">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo11" aria-expanded="false" aria-controls="collapseTwo11">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Requerimiento de IDE, requerimiento de depósitos en efectivo? 						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo11" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>En ocasiones llegan cartas invitación para cumplir con la obligación de enviar informativas de IDE, normalmente dan un periodo de gracia de 15 dias, por lo que si tomas acción inmediata, es posible evitar las multas que oscilan en $10,000 por mes o año no declarado, comienza <a href="registro.aspx">registrandote </a>considera que el socket en promedio se tramita en 3 semanas y las claves CASFIM en 2 semanas .</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo12">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo12" aria-expanded="false" aria-controls="collapseTwo12">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Cuánto tiempo tarda en llegar el acuse de aceptación o rechazo de mi declaración?  						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo12" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Un promedio de 24 a 72 hrs, lo cual depende del tiempo de respuesta del SAT, pero no debes preocuparte ya que una vez recibido lo tendrás disponible a través de nuestro servicio.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo13">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo13" aria-expanded="false" aria-controls="collapseTwo13">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Si se vence mi contrato, puedo acceder a las declaraciones de ese periodo?   						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo13" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Definitivamente, pero no podrán enviar declaraciones sino solo operaciones de consulta y descarga de acuses.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo14">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo14" aria-expanded="false" aria-controls="collapseTwo14">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Qué operaciones puedo realizar con una declaración?   						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo14" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Enviar/editar declaración, Crear y enviar declaración en ceros, Consultar declaración, Bajar acuses, Maneja declaraciones normales y complementarias.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo15">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo15" aria-expanded="false" aria-controls="collapseTwo15">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Qué formas de pago hay disponibles?    						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo15" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Transferencia, depósito bancario, cheque, efectivo, tarjeta de crédito, débito,  cuyas instrucciones se les proporcionan al contratar o solicitar asesoría escrita especializada.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo16">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo16" aria-expanded="false" aria-controls="collapseTwo16">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Cuantos contratos puedo tener a la vez?     						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo16" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Todos los que necesites, puedes combinar diversos planes y periodos, solo considera las fechas de vencimiento para los planes premium.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo20">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo20" aria-expanded="false" aria-controls="collapseTwo20">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Qué tipos de declaraciones puedo incluir en los contratos?     						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo20" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Un contrato es para un tipo de declaracion (plan) en específico, puedes tener contrato(s) para enviar en ceros, otro(s) contrato(s) con plan básico para enviar con datos, y otro(s) con plan premium para envíos con datos incluidas las complementarias ilimitadas; y puedes tener a la vez distintos contratos activos según tus necesidades. </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo21">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo21" aria-expanded="false" aria-controls="collapseTwo21">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Cómo se manejan los periodos anteriores, actuales y próximos a declarar en los contratos?      						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo21" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>En cada contrato debe especificarse si se trata de periodos anteriores (para regularizarse) o bien de periodos actuales/próximos, de modo que si tienes ambas necesidades, ocuparás unos contratos para manejar periodos anteriores, y otros contratos para manejar los actuales/próximos, de forma simultánea independientemente del plan.  </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo22">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo22" aria-expanded="false" aria-controls="collapseTwo22">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿En qué formatos debo generar la información de las declaraciones  para que sean aceptadas en este sistema?       						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo22" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Los archivos los generas tu mismo en excel con la información que va a declarar tu institución o empresa, se maneja un formato para la declaración mensual y otro para la anual, éstos te son enviados por correo automáticamente una vez que te has registrado en nuestro sistema y que nos has enviado tu carta de autorización validada para tramitarle el canal de conexión para transmisión de tus declaraciones ante el SAT, también pueden serte proporcionados por alguno de nuestros distribuidores o bien <a href="#" data-toggle="modal" data-target="#contactanosModal">contáctanos</a> para solicitarlos. Si ya dispones de la versión XML de IDE o XML de depósitos en efectivo también puedes enviarla desde nuestro sitio.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo23">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo23" aria-expanded="false" aria-controls="collapseTwo23">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Quién realiza el proceso de enviar mi declaración de depósitos en efectivo y del IDE?       						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo23" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Tú mismo lo haces sin ningún intermediario, una vez que te has registrado , iniciado sesión en tu cuenta, y una vez que has cubierto tu inscripción y los contratos que desees, basta que elijas el contrato con el cual deseas declarar, sube/importa tus archivos al sistema e inmediatamente son enviados a los servidores del SAT desde nuestra página por ti mismo</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo24">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo24" aria-expanded="false" aria-controls="collapseTwo24">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Qué opciones tengo para declarar depósitos en efectivo desde 2014?         						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo24" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>
                                        Pueden optar por declarar mensualmente o anualmente:
                                    </p>

                                    <p>Si optan por mensuales:</p>
                                    <ul style="list-style: none">
                                        <li>- El 10 de Abril 2014 es el límite para declarar Enero, Febrero y Marzo de 2014</li>
                                        <li>- Se exime de la anual siempre que haya presentado todas las mensuales en tiempo (a mas tardar el dia 10 del mes próximo al que declara incluso en modo contingencia) en caso contrario deberá presentar también la anual (el SAT aún no publica si será solo de los meses omitidos o extemporáneos).</li>
                                        <li>- El sistema cubre todas las especificaciones que marca el SAT</li>
                                        <li>- Los meses que no rebasen los $15,000 en efectivo y que no tengan cheques de caja adquiridos en efectivo, deben declararse en ceros.</li>
                                    </ul>
                                    Si optan por las anuales:
                                    <ul style="list-style: none">
                                        <li>- Deberá tener concentrada una gran cantidad de información de todo el año pudiendo experimentar sobrecargas de datos</li>
                                        <li>- El SAT aún no emite las reglas aplicables, pero se presume que requerirá muchos mas datos a profundo detalle que en las mensuales (como se manejaba anteriormente en el IDE) los cuales deberá tenerlos TODOS totalmente recabados de TODOS sus clientes, implicando una tarea de mayor carga en su cálculo y generación, y disponer de menor tiempo para posibles correcciones.</li>
                                    </ul>
                                    <p>La sugerencia es declarar mensualmente.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo25">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo25" aria-expanded="false" aria-controls="collapseTwo25">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Cuando es una declaración extemporánea?          						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo25" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Para instituciones financieras que optan por declarar IDE mensualmente, deben hacerlo antes del día 10 del mes de calendario inmediato siguiente al que está declarando; con un mes que se declare extemporaneo deberá presentar la declaración anual IDE; ambas opciones pueden declararse en este portal. (ide extemporáneo) </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo26">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo26" aria-expanded="false" aria-controls="collapseTwo26">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Que és y para que se usa el socket o matriz de conexión segura?           						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo26" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>El socket es un canal de comunicación que asigna el SAT para poder presentar las declaraciones informativas de IDE o de depósitos en efectivo por medios electrónicos, nosotros nos encargamos de gestionar un socket para tu institución, lo configuramos y lo podrás usar de forma transparente desde tu cuenta registrada en nuestro portal al momento de enviar tus informativas IDE. </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo27">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo27" aria-expanded="false" aria-controls="collapseTwo27">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>La Ley del IDE se abrogó pero ¿ El IDE Aún se declarara ?            						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo27" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Sí, varias instituciones financieras se confundieron y dejaron de declarar debido a la derogación del impuesto IDE y han recibido multas por omitir el seguir declarando. El impuesto ya no se retiene pero se deben seguir enviando las informativas de acuerdo al Art. 55, Fracción IV de la Ley del Impuesto sobre la Renta. Aquí en declaracioneside.com puedes presentar declaraciones por depósitos en efectivo del periodo que necesites. </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo28">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo28" aria-expanded="false" aria-controls="collapseTwo28">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Puedo dar de baja la obligación del IDE?             						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo28" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Existen instituciones financieras que al darse de alta en el SAT en automático tienen la obligación de declarar el IDE, pero como muchas no reciben ni rebasan depósitos en efectivo superiores a los 15 mp mensuales, suponen que no deben declarar y son multadas; lo que les corresponde hacer es presentar informativas de ide en ceros y dar de baja la obligación de declarar el IDE en el SAT; pero en caso que si hayan recibido tales depósitos deberán presentar informativas de ide con datos, para lo cual tenemos para ti la mejor solución desde este portal. </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo29">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo29" aria-expanded="false" aria-controls="collapseTwo29">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Cuales son los formatos para informativas de IDE, informativas de depósitos en efectivo?              						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo29" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>El SAT maneja layouts que deben presentarse en formato XML. Tenemos para ti plantillas (layouts) con ejemplos en formato excel para declaraciones con datos faciles de llenar y que son validados por el sistema al momento de que cargues tu información de la declaración informativa de IDE o depósitos en efectivo. Para declaraciones de IDE en ceros, el sistema genera el XML en base a tu institución y periodo a declarar sin necesidad de subir archivos. </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo30">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo30" aria-expanded="false" aria-controls="collapseTwo30">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Declaré pero recibo avisos de incumplimiento?               						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo30" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Si ya presentó su declaración de depósitos en efectivo y tiene su acuse de aceptación que bajó desde nuestra página, realice una aclaración a través de 'Mi portal' del SAT adjuntando el acuse XML del periodo que le solicitan. Las cartas invitación normalmente las emite el SAT por parejo tanto a instituciones cumplidoras como a las que nó. </p>
                                </div>
                            </div>
                        </div>

                        <div class="card">
                            <div class="card-header" id="headingTwo30a">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo30a" aria-expanded="false" aria-controls="collapseTwo30a">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Qué no se considera depósito en efectivo?               						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo30a" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>No se considerarán depósitos en efectivo, los que se efectúen a favor de personas físicas o morales mediante transferencias electrónicas, traspasos de cuenta, títulos de crédito o cualquier otro documento o sistema pactado con Instituciones del Sistema Financiero en los términos de las leyes aplicables, aun cuando sean a cargo de la misma Institución que los reciba. </p>
                                </div>
                            </div>
                        </div>
                        
                        <div class="card">
                            <div class="card-header" id="headingTwo30b">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo30b" aria-expanded="false" aria-controls="collapseTwo30b">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Qué se entiende por depósitos en efectivo?               						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo30b" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Los depósitos en moneda nacional o extranjera que se realicen en cualquier tipo de cuenta que las personas físicas o morales tengan a su nombre en las Instituciones del Sistema Financiero, así como las adquisiciones en efectivo de cheques de caja.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo30c">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo30c" aria-expanded="false" aria-controls="collapseTwo30c">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Qué obligación se cumple al presentar la Declaración anual de depósitos en efectivo?               						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo30c" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>La obligación de informar los depósitos en  efectivo que se realicen en las cuentas abiertas a nombre de los contribuyentes en las Instituciones del Sistema Financiero. </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo30d">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo30d" aria-expanded="false" aria-controls="collapseTwo30d">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Quiénes están obligados al pago del impuesto a los depósitos en efectivo?               						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo30d" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Las personas físicas y morales respecto de todos los depósitos en efectivo, en moneda nacional o extranjera, que realicen en cualquier tipo de cuenta que tengan en las instituciones del sistema financiero, cuya suma exceda 15,000 pesos en un mes. Por las adquisiciones en efectivo de cheques de caja, independientemente del monto del cheque adquirido.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo30e">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo30e" aria-expanded="false" aria-controls="collapseTwo30e">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Qué depósitos no se consideran en efectivo?
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo30e" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Los que se efectúen a favor de personas físicas y morales mediante transferencias electrónicas, traspasos de cuenta, títulos de crédito o cualquier otro documento o sistema pactado con instituciones del sistema financiero, aun cuando sean a cargo de la misma institución que los reciba</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo30f">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo30f" aria-expanded="false" aria-controls="collapseTwo30f">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i> ¿Sobre qué depósitos no se paga el IDE?               						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo30f" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p> Los de transferencias electrónicas, traspasos de cuenta, títulos de crédito (cheques, pagarés) o cualquier otro documento o sistema pactado con las instituciones del sistema financiero, aun cuando sean a cargo de la misma institución que los reciba.
<br/> Depósitos en efectivo que se realicen en las cuentas hasta por un monto que no exceda de 15,000 pesos en cada mes de calendario.
<br/> Los pagos efectuados que no sean en efectivo, sino a través de depósitos por traspaso o transferencias electrónicas, aun cuando éstos sean mayores de 15,000 pesos. Por las transferencias electrónicas de fondos que hacen los residentes en el extranjero, ya que no se considera un depósito en efectivo </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo30g">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo30g" aria-expanded="false" aria-controls="collapseTwo30g">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Pueden considerarse como personas morales con fines no lucrativos, para efectos de que no se les recaude IDE, los fideicomisos autorizados para recibir donativos deducibles del ISR?               						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo30g" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Sí, estos fideicomisos se consideran como personas morales con fines no lucrativos y no están sujetos al pago del IDE.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo30h">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo30h" aria-expanded="false" aria-controls="collapseTwo30h">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Cómo se debe efectuar el pago del IDE?               						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo30h" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Mediante recaudación que deberán hacer las instituciones del sistema financiero.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo30i">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo30i" aria-expanded="false" aria-controls="collapseTwo30i">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i> ¿Puede considerarse el estado de cuenta que emiten las instituciones financieras como constancia de recaudación y entero del IDE?               						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo30i" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Sí, siempre que contenga la información establecida en el anexo 1, rubro A, numeral 7de la RMF. Se tendrá por cumplida esta obligación cuando se expidan dichos estados de cuenta en forma electrónica a petición de los contribuyentes y contengan la información citada.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo30j">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo30j" aria-expanded="false" aria-controls="collapseTwo30j">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Qué obligaciones tienen las instituciones del sistema financiero con relación al IDE?               						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo30j" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Recaudar el impuesto el último día del mes de que se trate de cualquiera de las cuentas que tenga abiertas el contribuyente.
<br/> Enterar el impuesto en el plazo y en los términos que mediante reglas establezca la SHCP, el cual no debe exceder de tres días hábiles a aquel en el que se haya recaudado.
<br/> Informar mensualmente al SAT el importe del impuesto recaudado y el pendiente de recaudar por falta de fondos en las cuentas de los contribuyentes o por omisión de la institución de que se trate.
<br/> Recaudar el impuesto que no hubiera sido recaudado en el último día del mes de que se trate por falta de fondos, en el momento en el que se haga algún depósito durante el ejercicio fiscal en cualquiera de las cuentas que tenga el contribuyente en la institución financiera.
<br/> Entregar al contribuyente de forma mensual y anual las constancias que acrediten el entero del impuesto a los depósitos en efectivo.
<br/> Llevar un registro de los depósitos en efectivo que reciban.
<br/> Proporcionar anualmente, a más tardar el 15 de febrero, la información del impuesto recaudado y del pendiente por recaudar.
<br/> Informar a los titulares de las cuentas concentradoras, sobre los depósitos en efectivo realizados en ellas </p>
                                </div>
                            </div>
                        </div>

                         <div class="card">
                            <div class="card-header" id="headingTwo30k">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo30k" aria-expanded="false" aria-controls="collapseTwo30k">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Cómo concentran las instituciones del sistema financiero el IDE recaudado a sus clientes en la Tesorería de la Federación?               						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo30k" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>A partir del 1 de junio de 2012 las instituciones del sistema financiero deberán efectuar la concentración del entero del IDE que recauden a la Tesorería de la Federación, a través de depósito referenciado vía internet. Las instituciones del sistema financiero deberán contar previamente con el acuse de recibo electrónico que contiene la línea de captura, para lo cual deberán ingresar al portal de internet del SAT, Mi portal, Pago referenciado, de conformidad con el procedimiento establecido en las reglas II.2.8.8.1. y II.2.8.8.2. de la RMF para 2013 </p>
                                </div>
                            </div>
                        </div>

                         <div class="card">
                            <div class="card-header" id="headingTwo30l">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo30l" aria-expanded="false" aria-controls="collapseTwo30l">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Causarán el impuesto los pagos en efectivo para cubrir deudas con motivo de tarjetas de crédito?               						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo30l" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>No, hasta por el monto adeudado, ya que se trata de cuentas abiertas con motivo del otorgamiento de créditos. </p>
                                </div>
                            </div>
                        </div>

                         <div class="card">
                            <div class="card-header" id="headingTwo30m">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo30m" aria-expanded="false" aria-controls="collapseTwo30m">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>¿Qué pasa cuándo se efectúa un depósito a tarjeta de crédito y este excede el adeudo?               						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo30m" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Se deberá pagar el IDE por el excedente que supere la cantidad de 15,000 pesos </p>
                                </div>
                            </div>
                        </div>



                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- Modal -->
    <div id="modalvideo" class="modal fade bd-example-modal-lg" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header" id="titulo" style="padding-bottom: 0px">
                </div>
                <div class="modal-body" id="video" style="padding: 0px">
                    <p>Some text in the modal.</p>
                </div>

            </div>
        </div>
    </div>
    <script type="text/javascript">
        function videoman_onClick(url, titulo) {
            document.getElementById("video").innerHTML = "<div class=\"row\"><div class=\"col-sm-12\">" + "<img  style=\"width: 100%; height: 500px;\"  src=\"" + url + "\" ></img></div >";
            document.getElementById("titulo").innerHTML = "<h5 class=\"modal-title\" >" + titulo + "</h5> <button type=\"button\" onclick=\"cerrar_onClick()\" class=\"close btn-danger\" data-dismiss=\"modal\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>";
        }
        function cerrar_onClick() {//funcion para detener el iframe de youtube en caso de que se este reproduciendo        
            $("iframe").each(function () {
                var src = $(this).attr('src');
                $(this).attr('src', src);
            });
            document.getElementById("contenido").innerHTML = "";
        }
    </script>
</asp:Content>
