<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="temas.aspx.vb" Inherits="WebApplication1.temas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link rel="canonical" href="https://www.declaracioneside.com/temas.aspx" />
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
                <div class="col-md-12">
                    <h4>Temas interesantes</h4>
                    <p>Bienvenido a ésta sección donde encontrarás temas de interés sobre el depósitos en efectivo e IDE, puedes presentar y enviar aquí tu declaración informativa de depósitos en efectivo e IDE</p>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="accordion" id="accordionExample">
                        <div class="card">
                            <div class="card-header" id="headingCero1">
                                <h5 class="mb-0">
                                    <button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseCero1" aria-expanded="true" aria-controls="collapseCero1">
                                        <i class="fa fa-user"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i> Obligación a cumplir
						
                                    </button>
                                </h5>
                            </div>

                            <div id="collapseCero1" class="collapse show fade" aria-labelledby="headingCero1" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>
                                        Declarar los depósitos en  efectivo que se realicen en las cuentas abiertas a nombre de los contribuyentes en las Instituciones del Sistema Financiero.
                                    </p>
                                </div>
                            </div>
                        </div>
                         <div class="card">
                            <div class="card-header" id="headingCero2">
                                <h5 class="mb-0">
                                    <button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseCero2" aria-expanded="true" aria-controls="collapseCero2">
                                        <i class="fa fa-user"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Objetivo 
						
                                    </button>
                                </h5>
                            </div>

                            <div id="collapseCero2" class="collapse show fade" aria-labelledby="headingCero2" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>
                                        Cumplir con la obligación establecida en la Ley del Impuesto sobre la Renta de informar sobre los depósitos en efectivo que se realicen en todas las cuentas de las que el contribuyente sea titular en una misma Institución del Sistema Financiero.
                                    </p>
                                </div>
                            </div>
                        </div>
                        
                        <div class="card">
                            <div class="card-header" id="headingCero">
                                <h5 class="mb-0">
                                    <button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseCero" aria-expanded="true" aria-controls="collapseCero">
                                        <i class="fa fa-user"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Fundamento legal vigente 2021 
						
                                    </button>
                                </h5>
                            </div>

                            <div id="collapseCero" class="collapse show fade" aria-labelledby="headingCero" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>
                                        Ley del Impuesto sobre la Renta para 2021: Articulo 55, fracción IV, establece una obligación a las instituciones del sistema financiero, consistente en proporcionar anualmente a más tardar el 15 de febrero la información de depósitos en efectivo que se realicen en las cuentas abiertas a nombre de los contribuyentes en dichas instituciones, cuando el monto mensual acumulado por los depósitos en efectivo que se realicen en todas las cuentas de las que el contribuyente sea titular en una misma institución del sistema financiero exceda de $15,000.00, así como respecto de todas las adquisiciones en efectivo de cheques de caja.
                                        <br/>Resolución Miscelánea Fiscal para 2021: Reglas 3.5.11 y 3.5.14, Anexo 1, rubro A, numeral 2 y Anexo 1-A Trámite 74/ISR.
                                        <br/>Resolución Miscelánea Fiscal para 2021: Reglas 3.5.12 y 3.5.14, Anexo 1, rubro A, numeral 2 y Anexo 1-A Trámite 75/ISR.
                                        <br/>Reglamento de la  Ley del Impuesto sobre la Renta para 2021: Artículo 94.
                                        <br/>Resolución Miscelánea Fiscal para 2021: Regla 3.5.13.
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingOne1">
                                <h5 class="mb-0">
                                    <button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseOne1" aria-expanded="true" aria-controls="collapseOne1">
                                        <i class="fa fa-user"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Solicitud para ratificar y/o solicitar la "Clave de Institución Financiera" para presentar declaraciones de depósitos en efectivo.
						
                                    </button>
                                </h5>
                            </div>

                            <div id="collapseOne1" class="collapse show fade" aria-labelledby="headingOne1" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>
                                    Lo presentan Instituciones del Sistema Financiero, cuando Tratándose de la ratificación de la clave, en el momento que los contribuyentes lo requieran. En caso de solicitud de una nueva clave, cuando cumpla alguno de los supuestos que den lugar a la obligación de proporcionar la información a que se refiere el artículo 55, fracción IV de la Ley del ISR, "cuando el monto mensual acumulado por los depósitos en efectivo que se realicen en todas las cuentas de las que el contribuyente sea titular en una misma institución del sistema financiero exceda de $15,000.00, así como respecto de todas las adquisiciones en efectivo de cheques de caja, en los términos que establezca el Servicio de Administración Tributaria mediante reglas de carácter general", en términos de las reglas 3.5.11 y 3.5.12. de la RMF. Se obtienen Acuse de recepción y Acuse de respuesta
                                    </p>
                                </div>
                            </div>
                        </div>

                        <div class="card">
                            <div class="card-header" id="headingTwo24a">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo24a" aria-expanded="false" aria-controls="collapseTwo24a">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Porcentaje IDE
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo24a" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>
                                        Los porcentaje de impuesto IDE que se aplica sobre el excedente a los depósitos en efectivo acumulados en cada mes son:                                        
                                        <br/>Desde 2010,  3% sobre el excedente de $15,000.
                                        <br/>Año 2008 y 2009,  2% sobre el excedente de $25,000.
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo24b">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo24b" aria-expanded="false" aria-controls="collapseTwo24b">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Inicio del IDE
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo24b" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>                                        
                                        1o de Julio del 2008
                                    </p>
                                </div>
                            </div>
                        </div>
                         <div class="card">
                            <div class="card-header" id="headingTwo24c">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo24c" aria-expanded="false" aria-controls="collapseTwo24c">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Redondeos, decimales y ajustes de la declaración del IDE
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo24c" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>                                        
                                        El Código Fiscal de la Federación (CFF) en su artículo 20, décimo párrafo, establece
lo siguiente:
                                        <br/>
                                        “Para determinar las contribuciones se considerarán, inclusive, las fracciones del
peso. No obstante lo anterior, para efectuar su pago, el monto se ajustará para que
las que contengan cantidades que incluyan de 1 hasta 50 centavos se ajusten a la
unidad inmediata anterior y las que contengan cantidades de 51 a 99 centavos, se
ajusten a la unidad inmediata superior.”
                                        <br/>En resumen: 
                                        <br/>La base del impuesto se calcula con centavos (con decimales). 
                                        <br/>El IDE se recauda, se entera y se informa con montos ajustados. 
                                    </p>
                                </div>
                            </div>
                        </div>

                        <div class="card">
                            <div class="card-header" id="headingOne">
                                <h5 class="mb-0">
                                    <button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                        <i class="fa fa-user"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Información de depósitos por parte de las instituciones del sistema financiero a partir del 2014 
						
                                    </button>
                                </h5>
                            </div>

                            <div id="collapseOne" class="collapse show fade" aria-labelledby="headingOne" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>
                                        El artículo 55, fracción IV de la nueva Ley del Impuesto sobre la Renta 2021, establece una obligación a las instituciones del sistema financiero, consistente en proporcionar anualmente a más tardar el 15 de febrero la información de depósitos en efectivo que se realicen en las cuentas abiertas a nombre de los contribuyentes en dichas instituciones, cuando el monto mensual acumulado por los depósitos en efectivo que se realicen en todas las cuentas de las que el contribuyente sea titular en una misma institución del sistema financiero exceda de $15,000.00, así como respecto de todas las adquisiciones en efectivo de cheques de caja.
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Declaraciones de depósitos en efectivo a partir del 2014						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>La regla I.3.5.8. de la Resolución Miscelánea Fiscal para 2014 permite a las instituciones del sistema financiero optar por dar cumplimiento con la obligación a que se refiere el artículo 55, fracción IV de la Ley del Impuesto sobre la Renta, de manera mensual como alternativa a la anual.</p>
                                    <p>
                                        A continuación la regla aplicable de la RMF 2014
                                    </p>
                                    <p>
                                        I.3.5.10.           Para los efectos del artículo 55, fracción IV de la Ley del ISR, las instituciones del sistema financiero que no reciban depósitos en efectivo o cuando los que reciban sean inferiores a los 15 mil pesos mensuales, por cliente, deberán presentar en los plazos y términos señalados en el Reglamento del CFF y el Anexo 1-A, aviso de actualización de actividades económicas y obligaciones en el RFC, en el que se informe dicha circunstancia.
                                    </p>
                                    <p>
                                        Las instituciones a que se refiere el párrafo anterior, deberán igualmente manifestar mediante aviso de actualización de actividades económicas y obligaciones al RFC, cuando comiencen a recibir depósitos en efectivo superiores a los 15 mil pesos mensuales por cliente en los plazos y términos señalados en el Reglamento del CFF y el Anexo 1-A.
                                    </p>
                                    <p>
                                        Tratándose de instituciones del sistema financiero, cuyos clientes en el ejercicio, o bien, durante uno o varios meses no reciban depósitos en efectivo que excedan del monto acumulado mensual de 15 mil pesos, éstas deberán informar mediante las formas electrónicas IDE-A “Declaración anual de depósitos en efectivo” o IDE-M “Declaración mensual de depósitos en efectivo”, ambas contenidas en el Anexo 1, rubro A, numeral 2, según sea el caso; sin operaciones, por el ejercicio o periodo de que se trate.
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo2">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo2" aria-expanded="false" aria-controls="collapseTwo2">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Declaración Mensual de depósitos en efectivo a partir del 2014.
						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo2" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>
                                        De la RMF 2014 la regla es:
                                    </p>
                                    <p>
                                        I.3.5.7.              Para los efectos del artículo 55, fracción IV de la Ley del ISR, la información se proporcionará a través de la forma electrónica IDE-A “Declaración anual de depósitos en efectivo”, contenida en el Anexo 1, rubro A, numeral 2.
                                    </p>
                                    <p>
                                        La citada declaración se obtendrá en la página de Internet del SAT y se podrá presentar a través de los medios señalados en dicha página, utilizando la FIEL de la institución de que se trate.
                                    </p>
                                    <p>
                                        Tratándose de la adquisición en efectivo de cheques de caja, la información se deberá proporcionar cualquiera que sea el monto de los mismos.
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo3">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo3" aria-expanded="false" aria-controls="collapseTwo3">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Mecanismo de contingencia para la presentación de la información al SAT de depósitos en efectivo						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo3" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>De la RMF 2014 la regla es:</p>
                                    <p>
                                        I.3.5.9.              Para los efectos del artículo 55, fracción IV de la Ley del ISR, cuando no sea posible para las instituciones del sistema financiero realizar el envío de la información, conforme a lo señalado en las reglas I.3.5.7. y I.3.5.8., según sea el caso, derivado de fallas tecnológicas no imputables a éstas; la información se podrá presentar bajo el esquema de contingencia establecido en el “Procedimiento de Contingencia por falla en comunicación en el envío de Declaraciones de depósitos en efectivo”, publicado en la página de Internet del SAT, en los mismos plazos establecidos en el artículo 55, fracción IV de la Ley del ISR o en la regla I.3.5.9., según corresponda.
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo4">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo4" aria-expanded="false" aria-controls="collapseTwo4">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Abrogación del IDE
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo4" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>En virtud del “Decreto por el que se reforman, adicionan y derogan diversas disposiciones de la Ley del Impuesto al Valor Agregado; de la Ley del Impuesto Especial sobre Producción y Servicios; de la Ley Federal de Derechos, se expide la Ley del Impuesto sobre la Renta, y se abrogan la Ley del Impuesto Empresarial a Tasa Única, y la Ley del Impuesto a los Depósitos en Efectivo”, publicado en el Diario Oficial de la Federación del 11 de diciembre de 2013, quedó abrogada la Ley del Impuesto a los Depósitos en Efectivo a partir del 01 de enero de 2014. Sin embargo, la obligacion de declarar mas no de retener impuesto, persiste ahora en la ley del ISR, artitulo 55 fracc IV.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo5">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo5" aria-expanded="false" aria-controls="collapseTwo5">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Obligaciones pendientes de IDE después del 2013						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo5" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Acorde con lo dispuesto en el artículo Décimo Tercero, fracciones II, III, IV, V y VI del citado Decreto, así como el Artículo Noveno transitorio de la Resolución Miscelánea Fiscal  para 2014, las obligaciones y derechos que hubieran nacido durante la vigencia de la Ley del Impuesto a los Depósitos en Efectivo,  deberán ser cumplidas en las formas, plazos y términos establecidos en las disposiciones fiscales vigentes hasta el 31 de diciembre de 2013. Para la presentación de declaraciones complementarios o extemporáneas puede usar los servicios ofrecidos en www.declaracioneside.com.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo6">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo6" aria-expanded="false" aria-controls="collapseTwo6">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Impuesto a los depósitos en efectivo (Impuesto IDE)	
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo6" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>
                                        El impuesto IDE es conocido como el Impuesto a los Depósitos en Efectivo. Lógicamente, grava a aquellos depósitos en efectivo, ya sea por uno o por la suma de varios depósitos cuyo monto en el mes exceda de $15,000 pesos (vigencia a partir del 1 de enero de 2010, anteriormente eran montos superiores a los $25.000). La tasa a aplicar es del 3% sobre el excedente de mencionado monto al mes. 
Para determinar la base imponible, se debe realizar la suma del acumulado de todos losdepósitos en efectivoque recibe un contribuyente en todas las cuentas en las cuales sea titular de una misma institución del sistema financiero (no solo bancos). 
Una vez realizada la suma, si la cantidad de depósitos dentro del mes de que se trate, excede $15,000, ese excedente causará la imposición del impuesto sobre los depósitos en efectivo.
                                    </p>
                                    <p>Asimismo, para los caso de Tarjetas de Crédito se recaudará sólo cuando exista saldo a favor generado por depósitos en efectivo. Quedan alcanzadas todas las personas físicas o morales que efectúen depósitos en efectivo en cuentas abiertas a su nombre.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo7">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo7" aria-expanded="false" aria-controls="collapseTwo7">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Acreditar IDE (Acreditamiento del IDE) en pago mensual					
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo7" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>
                                        Los contribuyentes pueden acreditar (restar) el monto del IDE efectivamente pagado en el mismo mes contra el monto del pago provisional del impuesto sobre la renta (ISR) del mes de que se trate. El remanente del IDE que en su caso resulte podrá restarse o acreditarse contra el ISR retenido a terceros en el mismo mes, por ejemplo, el retenido a los trabajadores. Si después de hacer el acreditamiento anterior existiera una diferencia de IDE, se podrá compensar contra otras contribuciones federales a cargo por ejemplo, el Impuesto Empresarial a Tasa Única (IETU) y el Impuesto al Valor Agregado (IVA). 
La diferencia que en su caso resulte después de la compensación podrá solicitarse en devolución, siempre que sea dictaminada por contador público registrado. El IDE efectivamente pagado también puede restarse del impuesto sobre la renta anual, salvo que haya sido acreditado contra el impuesto sobre la renta retenido a terceros, compensado contra otras contribuciones federales a su cargo o solicitado en devolución.
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo8">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo8" aria-expanded="false" aria-controls="collapseTwo8">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Calcular el IDE						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo8" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>
                                        Cuando se hagan depósitos a plazo cuyo monto individual exceda de 15,000 pesos, el impuesto se recaudará en el momento en que se realicen tales depósitos. 
Para determinar la cantidad que excede de 15<li>,000 pesos, deben considerarse los montos de los depósitos que se hagan en el mes de calendario. </li>
                                        Cuando la institución financiera no pueda hacer la recaudación del impuesto IDE en el mes por falta de fondos en las cuentas del contribuyente, debe efectuar la recaudación en el momento en que se realice algún depósito en cualquiera de las cuentas que tenga el contribuyente en dicha institución. 
Por las cantidades excedentes a 15,000 pesos la recaudación se realizará el último día del mes.
                                    </p>
                                    Ejemplo:
                                    <ul style="list-style: none">
                                        <li>Suma de depósitos mensuales:  $30,000</li>
                                        <li>Monto Exento: $15,000</li>
                                        <li>Excedente: $15,000</li>
                                        <li>Tasa: 3%</li>
                                        <li>IDE determinado: Excedente x Tasa = $15,000 x 3% = $450 </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo10">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo10" aria-expanded="false" aria-controls="collapseTwo10">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Clave de Institución Financiera
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo10" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Es la referencia del número o clave que identifique a la institución financiera, conforme al listado de instituciones financieras recaudadoras del impuesto a los depósitos en efectivo emitido por el SAT, para tramitar la solicitud para ratificar y/o solicitar la Clave de institución financiera para el entero del IDE recaudado en la modalidad de contingencia; presentación de declaraciones informativas del IDE y validación de contribuyentes personas físicas y morales exentas del IDE. </p>
                                    <p>
                                        * ¿Quiénes lo presentan?
Instituciones del Sistema Financiero.
                                    </p>
                                    <p>
                                        * ¿Dónde se presenta?
A través de la página de Internet del SAT, en la sección “Mi Portal”.
                                    </p>
                                    <p>
                                        * ¿Qué documentos se obtienen?
Acuse de recibo.
                                    </p>
                                    <p>
                                        * ¿Cuándo se presenta?
-   Tratándose de la ratificación de la clave, dentro de los siguientes tres meses a la fecha de
    publicación del listado de “Claves de Instituciones Financieras” que realice el SAT en su
    página de Internet.
                                    </p>
                                    <p>
                                        -  En caso de solicitud de una nueva clave, cuando cumpla alguno de los supuestos que den
    lugar a la recaudación y entero o concentración del IDE, de acuerdo con la Ley del IDE y
    con lo establecido en la regla I.7.15. de la RMF.
                                    </p>
                                    <p>Requisitos:</p>
                                    <p>
                                        Para entrar a la aplicación en la página del SAT, inicie sesión en la sección “Mi portal”, para acceder deberá capturar su RFC y CIECF.
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo11">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo11" aria-expanded="false" aria-controls="collapseTwo11">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Constancia de IDE
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo11" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>La entrega de las constancias de retención del impuesto IDE, es una obligación de las instituciones financieras, pero no su envío personalizado mediante el correo. La constancia de retención del IDE se entrega mensualmente y a más tardar el 10 de cada mes y la anual, a más tardar el día 15 de febrero del año de calendario siguiente al año de que se trate. En este caso, el cliente es el que debe acudir a pedirla. Como contribuyente deberás asegurarte de que la institución financiera te entregue cada mes una constancia de retención de IDE para poder acreditar o solicitar devolución del 3% que pagues por concepto del Impuesto a los Depósitos en Efectivo (IDE).</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo12">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo12" aria-expanded="false" aria-controls="collapseTwo12">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Declaración informativa anual del IDE (IDE anual)  						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo12" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>
                                        * ¿Quiénes lo presentan?
Instituciones del Sistema Financiero.
                                    </p>
                                    <p>
                                        * ¿Dónde se presenta?
Por medios electrónicos, puede hacerlo ahora en declaracioneside.com
                                    </p>
                                    <p>
                                        * ¿Qué documentos se obtienen?
Acuse de recibo
                                    </p>
                                    <p>
                                        * ¿Cuándo se presenta?
A más tardar el 15 de febrero de cada año.
                                    </p>
                                    <p>Requisitos:</p>
                                    <p>
                                        Matriz de conexión segura (socket del SAT)
Clave de institución financiera
                                    </p>
                                    <p>En contingencia:</p>
                                    <p>
                                        La información se entregará en la Administración Local de Servicios al Contribuyente que corresponda (Sur del D.F., Guadalupe N.L., Celaya o Zapopan) mediante dispositivos de almacenamiento óptico CD-disco compacto o DVD, en ambos casos no reescribibles, acompañadas de escrito por duplicado dirigido al Administrador Local de Servicios al Contribuyente
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo13">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo13" aria-expanded="false" aria-controls="collapseTwo13">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>IDE Bancario
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo13" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Éste impuesto es aplicado a quienes hacen depósitos en efectivo a cuentas bancarias. Entró en vigor el 1 de julio de 2008 con la finalidad de recaudar un impuesto del 3% sobre el excedente del monto de los depósitos en efectivo en las cuentas de banco que superen los 15 mil pesos. El impuesto será retenido automáticamente por las instituciones bancarias quienes reportarán al gobierno federal a más tardar el día 10 del mes siguiente al que recaudó; éste impuesto es acreditable como pago anticipado del ISR. El monto de 15,000.00 pesos será el acumulado de todos los depósitos en efectivo que se realicen en un mes en las cuentas que el contribuyente sea titular. También se aplicará éste impuesto en los depósitos en efectivo a inversiones a plazo que excedan el mismo límite.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo14">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo14" aria-expanded="false" aria-controls="collapseTwo14">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>IDE en el pago referenciado / pago bancario por parte de instituciones que retienen IDE
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo14" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Haga el pago de IDE por transferencia electrónica de fondos en el portal de su banco, a través del servicio bancario de Depósito Referenciado. Ingrese al portal del banco en el que tenga servicio para realizar pagos a través de internet. Capture la(s) contraseña(s) proporcionada(s) por el banco para ingresar a la aplicación para pago de impuestos federales por línea de captura. </p>
                                    <p>
                                        REQUISITOS COMUNES PARA ABRIR LA CUENTA BANCARIA:
                                    </p>
                                    <ul style="list-style: none">
                                        <li>
                                        - Contrato de Apertura. </i>
                                        <li>- Acta Constitutiva. </li>
                                        <li>- Poderes. </li>
                                        <li>- Comprobante de domicilio Fiscal. </li>
                                        <li>- Identificación Representante Legal.</li>
                                        <li>- Registro Federal de Contribuyentes. </li>
                                    </ul>
                                    <p>
                                        PARA EL PAGO/ENTERO DEL IDE REQUIERE
                                    </p>
                                    <ul style="list-style: none">
                                        <li>- Abrir cuenta bancaria.</li>
                                        <li>- Contratar el servicio de Banca Electrónica adecuado</li>
                                        <li>- Proporcionar clave CASFIM y clave de institución financiera</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo15">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo15" aria-expanded="false" aria-controls="collapseTwo15">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Clave CASFIM
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo15" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Definición. Clave del Catálogo del Sistema Financiero Mexicano. Medios de identificación clave numérica que permite identificar a cada una de las instituciones financieras la cual se compone de cinco caracteres numéricos, correspondiendo de izquierda a derecha, los dos primeros para identificar al sector al que pertenece la entidad y los tres últimos para identificar a la propia Institución Financieras. Número o clave de la entidad financiera, que la va a identificar como Institución Financiera ante el público usuario y demás autoridades financieras.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo16">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo16" aria-expanded="false" aria-controls="collapseTwo16">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Pago de IDE   						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo16" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Pago de impuestos, se efectuará en bancos, utilizando el servicio de pago referenciado, es decir, a través de una línea de captura generada previamente en el SAT y posteriormente el contribuyente debe pagar en la institución bancaria, sin exceder el plazo establecido en dicha línea</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo20">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo20" aria-expanded="false" aria-controls="collapseTwo20">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Plazos para enterar (pagar) y declarar el IDE
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo20" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>
                                        Las instituciones de crédito (bancos exclusivamente) autorizadas deberán realizar el entero el mismo día en que se recaude dicho impuesto.
Las instituciones del sistema financiero deberán realizar el entero a más tardar el día hábil bancario siguiente a aquel en que realicen la recaudación.
Las sociedades cooperativas de ahorro y préstamo y las sociedades financieras populares autorizadas para operar como entidades de ahorro y crédito popular en los términos de la Ley de Ahorro y Crédito Popular, así como las sociedades o asociaciones a que se refiere el artículo segundo transitorio del Decreto por el que se expidió la Ley del Impuesto a los Depósitos en Efectivo, deberán efectuar el entero a más tardar el tercer día hábil bancario siguiente a la fecha en que se efectúe la recaudación. El entero o concentración del IDE recaudado se deberá realizar en las instituciones de crédito autorizadas para recaudar impuestos federales, a través de medios electrónicos que éstas indiquen, proporcionando la información que le soliciten y señalando como concepto de la operación el IDE y la referencia o clave del Catálogo del Sistema Financiero Mexicano (CASFIM). 
Informar mensualmente al SAT el importe del IDE recaudado y pendiente de recaudar por falta de fondos en las cuentas de los contribuyentes, a más tardar el día 10 del mes de calendario inmediato siguiente, a través de la Declaración Informativa Mensual del impuesto a los depósitos en efectivo, lo cual puede realizar desde www.declaracioneside.com
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo21">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo21" aria-expanded="false" aria-controls="collapseTwo21">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Información requerida para enviar las declaraciones de IDE     						
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo21" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Ya sea que su institución haya o no recaudado el IDE y haya dado de alta la obligación de retener IDE, debe notificarlo ya sea mediante una declaración en ceros o con los detalles correspondientes. Determine por cada cliente o socio que genere IDE, lo siguiente: </p>
                                    <ul style="list-style: none">
                                        <li>- Fecha,</li>
                                        <li>- nombres, apellido paterno, apellido materno, (o razón social), </li>
                                        <li>- RFC,</li>
                                        <li>- domicilio,</li>
                                        <li>- telefonos, </li>
                                        <li>- #de socio o cliente, </li>
                                        <li>- suma de depósitos en efectivo, </li>
                                        <li>- cantidad gravada,</li>
                                        <li>- monto determinado, </li>
                                        <li>- monto recaudado,</li>
                                        <li>- monto pendiente de recaudar, </li>
                                        <li>- monto remanente, </li>
                                        <li>- monto de cheques de caja, </li>
                                        <li>- monto recaudado de cheques de caja</li>
                                        <li>- saldo pendiente de recaudar </li>
                                    </ul>
                                    Si su cliente o socio maneja varias cuentas o contratos en su institución, la información proporcionada arriba corresponde a la suma del total de sus cuentas o contratos en el mismo periodo. Por cada cuenta/contrato deberá especificarse:
                                    <ul style="list-style: none">
                                        - # de cuenta, 
                                        <li>- #cotitulares, </li>
                                        <li>- % proporción que corresponde a este cliente/socio, </li>
                                        <li>- impuesto recaudado.</li>
                                        <li>- tipo de cuenta,</li>
                                        <li>- tipo de moneda. </li>
                                    </ul>
                                    Los movimientos efectuados en cada cuenta o contrato de su cliente o socio, pueden llevar una bitácora donde se indique:
                                    <ul style="list-style: none">
                                        <li>- Tipo de operación (depósito o retiro), </li>
                                        <li>- fecha de la operación, </li>
                                        <li>- monto de la operación</li>
                                        <li>- monto de la operación en moneda nacional </li>
                                    </ul>
                                    Regístrese ahora en declaracioneside.com para aprovechar sus ofertas y contrate el plan que más se adecúe a sus necesidades, donde una vez tenga estos datos en formato Excel, el sistema los importará para generar y enviar su información de manera segura a los servidores del SAT.

                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo22">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo22" aria-expanded="false" aria-controls="collapseTwo22">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>IDE Enterado
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo22" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Las instituciones financieras recaudadoras del IDE, después de calcular y retener este impuesto a sus clientes, deben enterarlo mensualmente a la federación mediante instituciones bancarias autorizadas por el SAT. Después de ello deben presentar sus declaraciones informativas mensuales y anuales de IDE, lo cual puede hacer registrandose en declaracioneside.com</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo23">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo23" aria-expanded="false" aria-controls="collapseTwo23">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>IDE Pendiente de Recaudar
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo23" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Pueden darse los casos donde a los contribuyentes se les calcula el IDE determinado, pero por alguna razón es imposible hacerle la retención/recaudación del mismo (sea por falta de fondos etc.) resultando un IDE pendiente de recaudar, el cual debe señalarse como parte de la declaración informativa mensual de IDE por cada contribuyente que presente dicha situación.</p>
                                </div>
                            </div>
                        </div>
                        
                        <div class="card">
                            <div class="card-header" id="headingTwo25">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo25" aria-expanded="false" aria-controls="collapseTwo25">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Informativas de IDE (presentación del IDE)
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo25" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>
                                        El SAT ha dictaminado por ley a partir del 2008 la obligación de presentar Declaraciones informativas del IDE por parte de las instituciones recaudadoras con base a sus especificaciónes técnicas para la presentación de las Declaraciones Informativas de Recaudación de IDE Anuales y Mensuales por medios electrónicos. Así mismo ha detallado un Procedimiento de contingencia por falla en comunicación en el envío de Declaraciones Informativas del Impuesto a los Depósitos en Efectivo por parte de las instituciones recaudadoras. El Mecanismos para presentar las declaraciones informativas de IDE se describe a continuación: 
Para los efectos del artículo 4, fracciones III y VII de la Ley del IDE y las correspondientes Reglas I.7.14 y I.7.22 de la Resolución Miscelánea Fiscal 2013,  la presentación de las declaraciones informativas mensuales y anuales por parte de las Instituciones de Sistema Financiero, se hará conforme a lo establecido a continuación. De la presentación de la declaración informativa mensual y anual del IDE
                                    </p>
                                    <ol>
                                        <li>Las Instituciones del Sistema Financiero deberán desarrollar las aplicaciones que permitan generar la información de declaración informativa, en archivo formato XML
                                        </li>
                                        <li>La información que debe enviarse al Servicio de Administración Tributaria deberá contener lo siguiente:

Listado de conceptos de la Declaración informativa mensual del impuesto a los depósitos en efectivo 
Listado de conceptos de la Declaración informativa anual del impuesto a los depósitos en efectivo</li>

                                        <li>La información deberá ser estructurada y contener las validaciones que se especifican en los criterios señalados en el documento ide_20130430.xsd</li>

                                        <li>El envío de las declaraciones informativas, deberá realizarse vía CECOBAN para las Instituciones del Sistema Financiero que tienen acceso a dicho canal, o bien vía Internet para el resto de dichas instituciones, utilizando en ambos casos, el socket de seguridad establecido en el documento de  “Especificaciones Técnicas para la presentación de la Declaración Informativa Mensual y Anual del IDE” generado por el Servicio de Administración Tributaria</li>

                                        <li>
                                        En caso de enfrentar problemas con el envío de la información a través de los medios electrónicos especificados en los puntos anteriores, la Institución del Sistema Financiero podrá presentar la declaración, bajo el esquema de contingencia, en CD-disco compacto o DVD, en ambos casos no re-escribibles. En este caso la información tendrá que ser firmada y encriptada con criptografía de llave pública y algoritmos RSA, conforme a lo señalado en el documento de Especificaciones Técnicas, citado el en punto anterior

                                        <li>Por la presentación de la Declaración Informativa del IDE, mensual o anual, ya sea por la vía normal o por contingencia, se generará y enviará el correspondiente Acuse de Aceptación o Rechazo también en formato XML, mismo que será entregado por el SAT a través del canal con el que cuente la Institución del Sistema Financiero.</li>
                                    </ol>

                                    <p>
                                        De la conectividad: A efecto de poder realizar el envío de la Declaración Informativa por la vía segura señalada en el punto 4 del apartado anterior “De la información de la declaración informativa”, se deberá contactar a las áreas técnicas del Servicio de Administración Tributaria a efecto de generar las matrices de seguridad y la conectividad de acuerdo a los procedimientos que para el efecto determine dicha autoridad.

De la contingencia: En caso de que sea decretada la contingencia por parte de la institución del Sistema Financiero, debido a que por alguna causa el servicio de comunicaciones se haya caído o bien le sea imposible transmitir la información al Servicio de Administración Tributaria por los canales normales, dichas Instituciones deberán apegarse a lo establecido en el “Procedimiento de Contingencia por falla en comunicación en el envío de Declaraciones Informativas del Impuesto a los Depósitos en Efectivo por parte de las Instituciones del Sistema Financiero”.
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo26">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo26" aria-expanded="false" aria-controls="collapseTwo26">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Ley del IDE
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo26" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>La ley del impuesto a los depósitos en efectivo(LIDE) entró en vigor desde el 2008, a la fecha se han realizado algunas modificaciones y resoluciones por miscelaneas fiscales que se pueden consultar en la página del SAT </p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo27">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo27" aria-expanded="false" aria-controls="collapseTwo27">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Matriz de conexión segura (Socket del SAT)
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo27" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>Para declarar depósitos en efectivo e IDE es necesario establecer una conexión punto a punto con los servidores del SAT, mediante una matriz de conexión segura, para la configuración de la conexión segura será necesario solicitar una matriz de conexión segura proporcionando la información contenida en los siguientes datos, misma que deberá ser remitida al Servicio de Administración Tributaria mediante correo electrónico: Fecha, Área Solicitante, IP Equipo Cliente, Nombre Equipo Cliente, Plataforma Equipo Cliente, Login del usuario que transmite del Eq. Cliente, IP Equipo Servidor, Nombre Equipo Servidor, Plataforma Equipo Servidor, Login del usuario del SAT en el equipo Servidor. En declaracioneside.com nos encargamos de configurar y procesar esta matriz para tu institución, déjanos el trabajo duro, tu solo usa nuestro sistema para presentar y enviar tus declaraciones informativas de depósitos en efectivo e IDE.</p>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header" id="headingTwo28">
                                <h5 class="mb-0">
                                    <button class="btn btn-link collapsed btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseTwo28" aria-expanded="false" aria-controls="collapseTwo28">
                                        <i class="fa fa-plus main"></i>&nbsp;<i class="fa fa-angle-double-right mr-3"></i>Solución a los depósitos en efectivo e IDE, Servicios, Aplicaciones y sistema para enviar informativa de depósitos en efectivo e IDE
                                    </button>
                                </h5>
                            </div>
                            <div id="collapseTwo28" class="collapse fade" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                <div class="card-body">
                                    <p>La Ley del IDE y la ley del ISR señala que debe presentar sus declaraciones informativas mensuales y anuales apegandose a especificaciones técnicas rígidas emitidas por el SAT, para ello las instituciones financieras requieren que su área de informática o sistemas les desarrolle la aplicación para ello, modificarla en caso necesaria e invertir en la infraestructura para ello, o bien pueden contratar dicho servicio con proveedores externos, en declaracioneside.com contamos con una sólida solución probada para declarar depósitos en efectivo e IDE por internet, que puedes contratar en cualquier momento y ahorrar significativamente tanto en la implementación como en el mantenimiento mensual o anual de una aplicación como esta, Regístrate para declarar.  </p>
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
