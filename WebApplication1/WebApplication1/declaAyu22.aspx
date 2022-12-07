<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="declaAyu22.aspx.vb" Inherits="WebApplication1.declaAyu22" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <style type="text/css">
        .style1
        {
            color: #800000;
            font-size: large;
        }
        .style2
        {
            text-align: justify;
        }
        .style5
        {
            font-size: large;
        }
        .style6
        {
            font-size: small;
        }
        .style7
        {
            font-family: Arial;
        }
        .style8
        {
            font-size: small;
            font-family: Arial;
        }
        .style9
        {
            color: #800000;
        }
        .style10
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: small;
        }
        .style11
        {
            font-family: Arial, Helvetica, sans-serif;
        }
        </style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="style2">
        <span class="style1"><strong>Cambios para 2022</strong></span><br 
            class="style5" />
        <br />
        <span class="style8">- Declaraciones mensuales de forma obligatoria a mas tardar el dia ultimo del mes posterior al reportado</span>
        <br /><br />
        - Las mensualels del 2022 se podran declarar a partir del 1 de Dic. de 2022. <br class="style8" />
            <br /><br />
        - Se deroga la declaracion anual <br class="style8" />        
        - Anual 2021 y anteriores, se deben presentar a mas tardar el 15 de febrero 2022. <br /><br />
        - Mensuales 2022 de enero a octure, presentarlas durante el mes de diciembre 2022<br /><br />
        - Mensuales 2022 noviembre y posteriores, presentarlas desde el 1 de dic. 2022 hasta el dia ultimo del mes de calendario inmediato siguiente al que corresponda<br /><br />
        - Obligacion a cumplir. Declarar los depositos en efectivo que se realicen en las cuentas abiertas a nombres de los contribuyentes en las instituciones del sistema financiero, asi como las adquisiciones en efectivo de cheques de caja<br /><br />
        - Descarga el layout excel para declaraciones mensuales con datos <a href="ejemploMensual22.xlsx">aqui</a><br /><br />
        - Esta pagina realizara todas las validaciones necesarias para asegurar que la declaracion sea aceptada, y creara los archivos necesarios que pide el SAT para su declaracion<br /><br />
        - Una vez que esta pagina genere los archivos listos para presentarse, podrá descargarlos y presentarlos por su cuenta en el portal del SAT o bien, tenemos la opcion de que nos haga un pedido (contrato) del plan "presentacion ejercicio" para que nosotros se las prsentemos por los 12 meses que cubre dicho plan, una vez vencido dicho plan, podra adquirir otros nuevos variando el periodo de inicio del ejercicio<br /><br />
        - Las declaraciones en ceros nosotros se las presentamos por default<br /><br />
        - Para declaraciones con datos, Cada mes usted ingresara a este portal a subir su archivo de excel en base a nuestro layout, el sistema lo validara y en caso de errores se le indicaran para su correccion, una vez que estan correctos la declaracion cambia a estatus validada, y ya estara lista para su presentacion. 
            En su cuenta al iniciar sesion configure la forma de presentacion de las declaraciones:  <br />
            a) subiendo la fiel de la institucion, la cual sera encriptada por seguridad, para que una vez contratado el plan presentacion anual, nosotros cada mes en base a sus archivos creados le hagamos la presentacion de las declaraciones. Para ello procuren subir sus archivos entre el dia 1 y 5 de cada mes, para nosotros presentarlos despues de ese lapso del mes en curso<br />
            b)conexion remota para conectarnos a su equipo cada vez que le vayamos a presentar la declaracion, tambien indique si la conexion sera por anydesk o teamviewer, el id o numero de estacion de trabajo, el password, la ruta local o de red donde tenga guardada su firma electronica (NO se guarda en esta opcion) para con ello ahorrar tiempo y al conectarnos a su equipo ir directo a esa ruta para presentarle nosotros las declaraciones en caso de habernos contratado el plan correspondiene, dicha ruta ha de contener el archivo .cer, el .key y la contraseña de la fiel. Solo se usara con fines de autentificarse en el SAT para presentar estas declaraciones. <br />            
            - Tenga ya instalado el anydesk o teamviewer en el equipo de computo donde nos conectaremos para declarar y donde tenga la fiel lista<br /><br />
            - Puede instalarlos de aqui <a href="https://anydesk.com/es/downloads/windows" >anydesk</a> o <a href="https://www.teamviewer.com/es/descarga/windows/"> teamviewer</a><br />
            - En el caso que nos contraten el servicio de "presentacion ejercicio", podran solicitar cada mes la fecha y hora de la cita a la que quieren que nos conectemos a su equipo para presentar la declaracion, indicandolo en la declaracion mensual correspondiente de esta pagina, y pulsando por sistema en "avisar al proveedor", y via correo sera confirmada o sugerida otra fecha<br /><br />
            c) presentar por su propia cuenta las declaraciones, para ello basta con que cada mes suba su excel en base a nuestro layout, importarlo, validarlo y crear y descargar la declaracion, para con ella ir a presentala al sat.<br />            
        - Dado que este sistema ahora es independiente al SAT, sugerimos que actualice el estatus de su declaracion en nuestro portal, y suba el acuse correspondiente para su propio control interno<br /><br />
        - <br /><br />
        -&nbsp;
        Al colocar el mouse sobre un control, botón, cuadro, lista, etc. obtendrá una 
        descripción del mismo en caso de haberla</span><br class="style8" />
        <span class="style8">-
        Siempre que vaya a realizar una nueva operación inicie pulsando el menú Declarar 
        para recargar las opciones adecuadas de acuerdo a sus declaraciones, luego elija 
        las opciones y pulse el botón Aplicar.</span><br class="style8" />
        <span class="style8">-
        En promedio se lleva de 2-24 hrs para que se genere el acuse del SAT tras haberla presentado. Si no lo obtiene en ese periodo y nos contrato el servicio de presentacion anual, contactenos para 
        comunicarnos al SAT y gestionar.</span><br class="style8" /><span class="style8">-
</span><br class="style8" />
        <span class="style8">-
        Para acceder a esta ayuda: Ingrese al sistema, vaya al menu cuenta, desplacese abajo hasta ayuda</span><br class="style8" />
        <span class="style8">-
        La complementaria es sobre la última declaracion del periodo, la cual requiere 
        tener sus acuses aceptados para poder complementarse</span><br class="style8" />
            <span class="style8">-
        Un contrato con plan ceros no le dará opción para importar datos para crear una 
        declaración con registros</span><br class="style8" /><span class="style8">-
        Por norma del SAT los importes a declarar omiten decimales, 
        por lo cual podría observar diferencias en los totales de la declaración que el 
        sistema calcula, pero aún así ud. podría modificarlos en caso de desearlo. Para 
        la determinación del monto excedente, los depósitos en efectivo se consideran 
        con centavos (con decimales), así deberá capturarlos Ud. El sistema enviará la 
        información redondeando de 1 hasta 50 centavos se ajusten a la unidad inmediata 
        anterior y las que contengan cantidades de 51 a 99 centavos, se ajusten a la 
        unidad inmediata superior. </span><br 
            class="style8" />
        <span class="style8">-
        No olvide pulsar Aplicar para ejecutar el comando correspondiente a las opciones 
        que elijió bajo el menú Declarar, siempre que cambie tales opciones deberá 
        pulsar aplicar para que se actualice la sección de declaraciones</span><br 
            class="style8" /><span class="style8">-
</span><br class="style8" />
        <br class="style8" />
        <strong><span class="style8">Importación desde excel:&nbsp;
        
        </span>
        <br class="style8" />
        </strong><span class="style8">&nbsp;&nbsp; El archivo contiene en la primer hoja todos los registros</span><br 
            class="style8" />
        <span class="style8">&nbsp;&nbsp; Abajo de los encabezados deben ir los datos correspondientes
        (recuerde dejar los renglones de encabezados) </span>
        <br class="style8" />
        <span class="style8">&nbsp;&nbsp; Si usted lo desea puede utilizar fórmulas en las celdas o bien sin formulas
        </span><br class="style8" />
        <span class="style8">&nbsp;&nbsp; No deje renglones en blanco entre los registros</span><br 
            class="style8" />
        <span class="style8">&nbsp;&nbsp; Cada dato debe ocupar una sola celda&nbsp;&nbsp; 
        </span> <br class="style8" />
        <br class="style8" />
        <span class="style8">&nbsp;&nbsp;&nbsp;<strong>Formato</strong> archivo excel para declaración
        </span>
<asp:HyperLink ID="mensual" runat="server"  NavigateUrl="~/ejemploMensual22.xlsx"
            style="text-decoration: underline; color: #0000CC; font-weight: 700;" 
            CssClass="style8">mensual 2022</asp:HyperLink> <span class="style8">&nbsp;(ejemplo)
        <br />
        Esta declaración requiere la especificación del desglose de los cotitulares de 
        las cuentas que causaron los montos reportados por cada contribuyente.Por cotitulares entendemos aquellos co-propietarios de una misma cuenta donde cada uno de ellos tiene un porcentaje de proporcion 
	(un ejemplo claro son las cuentas mancomunadas)
        <br />
        Los renglones se clasifican en 3 grupos de acuerdo a la columna Descripción, el 
        2° renglón especifica las columnas para indicar un TITULAR, el 3er renglón 
        especifica las columnas para indicar una COTITULAR, el 4° renglón 
        especifica las columnas para indicar un CUENTAS DE CHEQUES; a partir del 5° renglón en la 
        1er columna escriba TIT (titular), COT (cotitular), o bien CHQ (cheque), y 
        los datos asociados en las demás columnas.
        <br />
        Un titular puede tener relacionados uno o varios cotitularesque deben 
        indicarse antes de cambiar de titular&nbsp;
        <br />
        Después de especificar el titular, sus posibles cotitulares, puede proceder con el siguiente titulalr con sus posibles 
        cotitulares. Si su Institución no ha manejado número de contrato o 
        número de cuenta, le recomendamos que lo maneje, o en última instancia 
        especifique el número de socio o de cliente en lugar del número de 
        cuenta/contrato. Hasta el final indique los cheques de caja si es que los usa.
        <br />
        Un titular debe aparecer una sola vez en la declaracion por numero de cuenta o contrato, seguido por todos 
        los cotitulares de sus respectivas cuentas.</span><br class="style8" />
        <br class="style8" />
        <br class="style8" /><em><span class="style8"><span class="style9"><strong>Descripción 
        de columnas de excel</strong></span>:</span><br class="style8" /></em>
        <span class="style8">- <strong>Descripción. </strong>Indica el tipo de registro, cuyos valores son TIT (titular),
        COT 
        (cotitular), 
        </span><span class="style10">CHQ </span></span><span 
            class="style8">(cheque) </span><br class="style8" />
        <span class="style8"- <strong><strong>- Nombres</strong>. Para persona física: el o los nombres del contribuyente reportado</span><br 
            class="style8" />
        - <span class="style8"><strong>Ap paterno</strong>. Para persona física: apellido paterno del contribuyente reportado
        </span> 
        <br class="style8" />
        <span class="style8">- <strong>Ap materno</strong>. Para persona física: apellido materno del contribuyente reportado</span><br 
            class="style8" />
        <span class="style8">- <strong>Razon social</strong>. Para persona moral: Denominación o razón social del 
        contribuyente reportado</span><br class="style8" />
        <span class="style8">- <strong>Razon social</strong>. Si se trata de persona fisica indique nombre, ap paterno, ap materno y deje vacia la razon social. Si se trata de persona moral indique la razon social 
y deje vacios nombre, ap paterno, ap materno
</span><br class="style8" />
        <span class="style8">- <strong>Rfc</strong>. RFC del contribuyente reportado</span><br class="style8" />
        <span class="style8">- <strong>CURP</strong>. CURP de persona fisica </span><br class="style8" />
        <span class="style8">- <strong>CURP</strong>. NumIdFiscal, numero de identificacion fiscal cuando sean extranjeros </span><br class="style8" />
        <span class="style8">- <strong>Domicilio</strong>. Domicilio del contribuyente reportado separado en sus componentes&nbsp;
        <br />Teléfono1</strong>- <strong>Telefono1</strong>. Teléfono del contribuyente reportado. En 
        caso de contar con más de 2 teléfonos, la información a proporcionar será 
        conforme a la siguiente prioridad: 1. Teléfono Particular. 2. Teléfono Móvil. 3. 
        Teléfono de oficina. &nbsp;<br />
       - <strong>Télefono2</strong>. Teléfono del contribuyente reportado&nbsp;</span><br 
            class="style8" />
        <span class="style8"> <strong>Suma de depositos en efectivo</strong>- </span>
        Suma de depositos en efectivo del contribuyente 
        reportado (solo en mensuales)</span><br class="style10" />
        <span class="style8">- <strong>Monto Excedente</strong>. Monto excedente a 
        $15,000 en el mes por el contribuyente (aplica a mensuales)</span><br 
            class="style8" />
        <span class="style8">-  <strong>Monto de cheques de caja</strong></strong>. Monto del 
        cheque de caja, pagado en efectivo. </span><br class="style8" />
        <span class="style8">- <strong>Num. de cuenta o contrato.</strong> Número o clave que identifica la cuenta o 
        contrato reportado. Expresión 
        del(os) número(s) de la(s) cuenta(s) abierta(s) a nombre del contribuyente en 
        la(s) que se realizaron los depósitos en efectivo.&nbsp; . Si no 
        manejan ningún número de cuenta o de contrato: anote aquí el Número de socio. Este dato no debe repetirse en la declaracion. </strong> </span>
        <strong><br class="style8" />
        <span class="style8">- Porcentaje de proporción</span></strong> de la cuenta que corresponde al 
        contribuyente informado ya sea titular o cotitular. En caso de no existir cotitulares, dejelo vacio, puede incluir hasta 4 decimales</span><br class="style8" />
        <br />
        <strong>- Correo electrónico</strong> Se puede especificar el correo del contribuyente <br />
        <strong>- moneda</strong> moneda de la operacion en base al catalogo<br />
        <strong>- tipo de cambio</strong>. cuando la moneda no es MXN, indique el tipo de cambio, maximo 2 decimales<br />
        <br />Existe ayuda adicional sobre los datos/columnas del layout de excel, en la hoja ayuda del archivo de ejemplo mismo.
            <br />
        </span>
        <br class="style8" />
        <span class="style8">(Si Ud. opta por bajar el ejemplo sea mensual o anual, y sobre ese trabajar o 
        generar una copia, asegúrese de eliminar todos los renglones debajo del 
        encabezado antes de iniciar con los suyos, seleccionando todas esas filas sobre 
        la columna que indica los números de renglón, clic derecho y eliminar, hágalo 
        así en lugar de borrar solamente el contenido</span><br class="style8" />
        <br class="style8" />
        <span class="style8">Ud. es el único responsable del contenido de la información que está declarando 
        </span><br class="style8" />
        <span class="style8">Favor de conservar sus archivos de excel de las declaraciones</span><br />
            Si en el archivo de excel se encuentran repetidos el nombre y apellidos o razón 
        social, se actualizarán los datos restantes de dicho registro reemplazando las 
        coincidencias anteriores.</span><br class="style8" />
        </span>
        <span class="style8">Respecto al efectivo captado por concepto de abono a creditos o prestamos, hay que considerar que si el pago entra a una cuenta de la institucion 
entonces no debera incluirse en la declaracion, pero si el efectivo entra a una cuenta del contribuyente si deberia incluirse.
        </span>
        <br />
        <span class="style8">Para el caso donde hay cuentas de ahorro de menores de edad, que no tienen RFC, se pueden tomar los datos del tutor
        </span>
        <br />
        <span class="style8"> Si en el archivo de excel se encuentran repetidos el nombre y apellidos o razón 
        social, se actualizarán los datos restantes de dicho registro reemplazando las 
        coincidencias anterioes<br />
        Si la importación detecta algún error, se le avisará y deberá importar de nuevo 
        realizando los ajustes sugeridos, ya que al reimportar se borra la información 
        anterior del periodo elegido.<br />
        </span>
        <br class="style8" />
        <br class="style8" /><strong><span class="style8">Pasos para crear/editar una Declaración Mensual importando desde 
        Excel </span> </strong><br class="style8" />
        <span class="style6"><span class="style7">En el menu declarar, 
            Elija un contrato vigente correspondiente al periodo que desea declarar, Elija mensual, Elija el ejercicio y el mes, Elija 
        &#39;Crear/Editar declaración&#39;, Si va a editar(modificar) la declaración seleccione 
        el # de Declaración , Clic en Aplicar lo cual cargará la sección de declaraciones, 
        Clic en &#39;Seleccionar archivo&#39; y elija su archivo de excel que contiene la 
        declaración mensual que desea subir, Clic en &#39;Importar ahora&#39;, espere a que 
        cambie el estatus a importada (si tiene algún error en su archivo de excel se le 
        notificará, tras corregirlo y guardarlo vuelva a seleccionarlo e importe de 
        nuevo), verifique los montos marcados con *  Clic en 
        Crear, Clic en Descargar. &#39;Ver datos y acuses&#39; le mostrará 
        los registros importados del mes, de requerirlo ocupe las barras de 
        desplazamiento horizontales y verticales del listado de registros, ubicadas al 
        fondo de la página.
        <br />
        <br />
        <br class="style8" /><span class="style8"><strong>Pasos para Subir y Descargar Acuses de su declaración </strong></span><br class="style8" /><span class="style8"></strong>
            Asegúrese de haber enviado satisfactoriamente la declaración , Elija mensual , Elija el ejercicio y 
        el mes, Elija &#39;Consultar Declaración y Acuses&#39;, Seleccione el # de 
        Declaración de la que desea el acuse , Clic en Aplicar lo cual cargará la sección 
        de declaraciones, palomee acuse, seleccione el archivo de su acuse, clic ahí &#39;Guardar y subir&#39;. Los acuses son su comprobante de 
        envío de la declaración. La idea de subirlos es para su propio control interno</span><br class="style8" />
        <br class="style8" />
</div>
    </form>
</body>
</html>
