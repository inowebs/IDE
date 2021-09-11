<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="precios.aspx.vb" Inherits="WebApplication1.precios" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" Runat="server">
    <style type="text/css">
        .style5
        {
            color: #800000;
            font-size: large;
        }
        .style7
        {
            background-color: #003399;
        }
        .style8
        {
            color: #FFFFFF;
        }
        .style9
        {
            color: #FFFFFF;
            background-color: #003399;
            width: 66px;
        }
        .style10
        {
            color: #996600;
        }
        .style12
        {
            color: #996600;
            font-size: medium;
        }
        .style13
        {
            background-color: #003399;
            width: 245px;
        }
        .style14
        {
            width: 245px;
        }
        .style15
        {
            background-color: #003399;
            width: 573px;
        }
        .style16
        {
            width: 573px;
        }
        .style17
        {
            color: #FFFFFF;
            background-color: #003399;
            width: 549px;
        }
        .style18
        {
            width: 549px;
        }
        .style19
        {
            color: #FFFFFF;
            background-color: #003399;
            width: 243px;
        }
        .style20
        {
            width: 243px;
        }
        .style21
        {
            width: 66px;
        }
        .style22
        {
            font-size: small;
        }
        .style28
        {
            color: #000000;
            font-size: medium;
        }
        .style29
        {
            font-size: medium;
        }
        .style31
        {
            color: #003366;
            font-size: medium;
        }
        .style32
        {
            color: #003366;
        }
        .style24
        {
            width: 195px;
        }
        .style33
        {
            color: #996600;
            font-size: large;
        }
        .style34
        {
            text-align: center;
            color: #000066;
        font-family: Arial;
        font-size: medium;
    }
        .style36
    {
        color: #003399;
        font-size: medium;
    }
    .style37
    {
        color: #003399;
        font-size: small;
    }
        .style38
        {
            color: #666666;
            font-size: small;
        }
        .style41
        {
        color: #FF0066;
    }
        .style42
        {
            color: #000099;
        }
        .style43
        {
            color: #000000;
        }
        .style46
        {
            font-family: arial;
            font-size: small;
        }
        .style49
        {
            height: 36px;
        }
        .style50
        {
            width: 573px;
            height: 36px;
        }
        .style51
        {
            color: #666666;
        }
        .style52
        {
            font-family: arial;
            font-size: small;
            color: #666666;
        }
        .style53
        {
            color: #FFFFFF;
            font-size: small;
        }
        .style55
        {
            color: #000000;
            font-size: small;
        }
        .style56
        {
            color: #FF0066;
            font-size: medium;
        }
        .style58
    {
        font-size: small;
        color: #FF0066;
    }
        .style59
        {
            font-size: small;
            color: #996600;
        }
        .style57
    {
        font-size: large;
    }
        .style39
        {
            color: #003399;
        }
        .style40
        {
            color: #808080;
        }
        .style60
        {
            font-weight: normal;
        }
        .style61
        {
            color: #003366;
            font-size: small;
        }
        .auto-style2 {
            color: #800000;
            font-size: medium;
        }
        .auto-style52 {
            height: 36px;
            width: 268px;
        }
        .auto-style53 {
            background-color: white;
            width: 268px;
        }
        .auto-style54 {
            width: 268px;
        }
        .auto-style55 {
        color: #660033;
        font-size: large;
    }
        .auto-style58 {
            width: 221px;
            background-color: #003399;
        }
        .auto-style59 {
            height: 33px;
            width: 221px;
            display: block;
            text-align: right;
            color: #666666;
            padding: 0px;
        }
        .auto-style60 {
        width: 221px;
    }
        .auto-style61 {
            font-size: large;
            color: #666666;
        }
        .auto-style62 {
            color: #000066;
            font-size: large;
        }
        .auto-style63 {
            color: #996600;
            font-size: medium;
        }
        .auto-style64 {
            color: #009999;
            font-size: medium;
        }
        .auto-style65 {
        font-family: arial;
        font-size: medium;
        color: #666666;
    }
    .auto-style66 {
        font-family: arial;
        font-size: medium;
    }
    .auto-style67 {
        background-color: #003399;
        width: 364px;
    }
    .auto-style68 {
        width: 364px;
    }
    .auto-style69 {
        background-color: #003399;
        width: 396px;
    }
    .auto-style70 {
        width: 396px;
    }
    .auto-style71 {
        width: 573px;
        height: 33px;
    }
    .auto-style72 {
        height: 33px;
    }
        </style>

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
        <br />
        <asp:Label ID="Label1" runat="server"     
            Text="Solución para declaraciones de Depósitos en Efectivo (ISR, IDE). Envía declaraciones mensuales y anuales de Depósitos en Efectivo (ISR, IDE) aquí." 
            style="color: #FFFFFF; font-size: xx-small;"></asp:Label>
            </strong></span>
            <br />
    <strong>   
    <span class="auto-style2">Precios y Planes</span><em><span class="style5"> </span>
    </em>
    </strong><span class="style28">
    <div style="text-align:right"> 

    </div>
    </span><strong><span class="style31">
    Te mejoramos cualquier cotización:&nbsp; </span><span class="auto-style55">
    ! Garantizado !</span></strong><br class="style29" />
    <br />      <div style="text-align:center">
                <img src="socket50.png"                      
                    alt="Declaraciones de depósitos en efectivo" 
                    border="0" />
                </div>
        <br />
    <br />
    <span class="style1master1">
    <strong><span class="style29">
    <span class="auto-style61">
    Socket de conexión</span></span>
    </strong><span class="style39"><span class="style38">Solo aplica a clientes nuevos. Es un proceso que realizamos para trámitar y configurar por cada institución del sector financiero de forma exclusiva un <strong>socket o matriz de conexión segura</strong>* ante el 
    <strong>SAT </strong>para que pueda enviar su declaración por internet. </span></span>
    </span>
    <br 
        class="style40" />
    <br />
    <span class="style59"><strong>Valores agregados:</strong></span><br />
    
    <table style="border: thin solid #000000; font-family: Arial; font-size: small;" >
        <tr class="style8">
            <td class="style13">
                Concepto</td>
            <td class="style15">
                Descripción</td>
            <td class="style7" align="right">
                Precio</td>
        </tr>
        <tr>
            <td class="style14">
                
                Capacitación 
                de uso de la aplicación</td>
            <td class="style16">
                
                Tutoriales, Presencial en nuestras oficinas<span class="Apple-converted-space">, Vía telefónica, </td>
            <td align="right">
                $ 0.00</td>
        </tr>
        <tr>
            <td class="style14">
                
                Soporte técnico</td>
            <td class="style16">
                
                Por correo electrónico</td>
            <td align="right">
                $ 0.00</td>
        </tr>
        <tr>
            <td class="style14">
                
                Actualizaciones</td>
            <td class="style16">
                
                Gratuitas</td>
            <td align="right">
                $ 0.00</td>
        </tr>
    </table>
    
    <br />
    <br />
    <span class="style1master1"><span class="style37">Presenta declaraciones de cualquier mes o año<strong>
    <br />
    </strong></span></span><span class="style36">
    <span __designer:mapid="18ba" class="style38" style="mso-ascii-font-family: Calibri; mso-fareast-font-family: +mn-ea; mso-font-kerning: 12.0pt;language:
es-MX">
    <span class="style1master1">Contrata lo que necesites</span></span></span><span class="style1master1"><br />
    <br />
    </span>
    <br />
    <strong><span class="auto-style62">Declaración de Depósitos en efectivo en <a name="ceros"> Ceros</a></span><br />
    <br />
    Declara</strong><span 
    class="style1master1"><span class="style22"><strong>ciones mensuales o anuales <span class="style42">en ceros</span></strong> 
    en un solo clic, tu decide que años y meses declarar.<br />
    </span></span>
    <br />
    
    <table style="border: thin solid #000000; font-family: Arial; font-size: small;" >
        <tr class="style8">
            <td class="auto-style67">
                Concepto</td>
            <td class="auto-style69">
                Descripción</td>
            <td class="style7" align="right">
                Precio</td>
        </tr>
        <tr>
            <td class="auto-style68">
                
                <strong>Declaración Mensual 
                de depósitos en efectivo en ceros</strong></td>
            <td class="auto-style70">
                
                Plataforma para enviar declaración..No 
                incluye complementarias</td>
            <td align="right">
                Contáctanos</td>
        </tr>
        <tr>
            <td class="auto-style68">
                
                <strong>Declaración Anual 
                de depósitos en efectivo en ceros</strong></td>
            <td class="auto-style70">
                
                Plataforma para enviar declaración... No 
                incluye complementarias</td>
            <td align="right">
                Contáctanos</td>
        </tr>
        <tr>
            <td class="auto-style68">
                
                Capacitación</td>
            <td class="auto-style70">
                
                Tutoriales, Presencial en nuestras oficinas<span class="Apple-converted-space">, Vía telefónica, </td>
            <td align="right">
                $ 0.00</td>
        </tr>
        <tr>
            <td class="auto-style68">
                
                Soporte técnico</td>
            <td class="auto-style70">
                
                Por correo electrónico</td>
            <td align="right">
                $ 0.00</td>
        </tr>
        <tr>
            <td class="auto-style68">
                
                Actualizaciones</td>
            <td class="auto-style70">
                
                Gratuitas</td>
            <td align="right">
                $ 0.00</td>
        </tr>
    </table>
    
    <br />
    <br />
    <span class="auto-style62"><strong><a name="basico"> Declaración de depósitos en efectivo con 
    datos</a> (plan básico)</strong></span><br />
    <span class="style1master1">
    <span class="style22"><strong>
<br />
Declaraciones mensuales, anuales o complementarias </strong>
    <span class="style42">con datos (captaciones de efectivo exedentes a $15,000</span>), tu decide que años y meses declarar.</span></span><br />
    <br />
    <table style="border: thin solid #000000" >
        <tr class="style53">
            <td class="auto-style58">
                Concepto</td>
            <td class="style15">
                Descripción</td>
            <td class="style7" align="right">
                Precio</td>
        </tr>
        <tr class="style22">
            <td class="auto-style59">Declaración Mensual de depósitos en efectivo con datos</td>
            <td class="auto-style71">
                
                Importación, generación y <strong>envío de </strong> una<strong> declaración</strong> 
                <strong>informativa mensual</strong>. No 
                incluye complementarias</td>
            <td align="right" class="auto-style72">
                Contáctanos</tr>
        <tr class="style22">
            <td class="auto-style60">Declaración Anual de depósitos en efectivo con datos</td>
            <td class="style16">
                
                Importación, generación y <strong>envío de </strong> una<strong> declaración</strong> 
                <strong>informativa anual</strong>. No 
                incluye complementarias</td>
            <td align="right">
                Contáctanos</tr>
        <tr class="style22">
            <td class="auto-style60">Declaración complementaria</td>
            <td class="style16">
                
                Importación, generación y <strong>envío de </strong>una<strong> declaración 
                complementaria</strong></td>
            <td align="right">
                Contáctanos</tr>
        <tr class="style22">
            <td class="auto-style60">
                
                Capacitación 
                de uso de la aplicación</td>
            <td class="style16">
                
                Tutoriales, Presencial en nuestras oficinas,
                <span class="Apple-converted-space">Vía telefónica.</td>
            <td align="right">
                $ 0.00</td>
        </tr>
        <tr class="style22">
            <td class="auto-style60">
                
                Soporte técnico</td>
            <td class="style16">
                
                Por correo electrónico</td>
            <td align="right">
                $ 0.00</td>
        </tr>
        <tr class="style22">
            <td class="auto-style60">
                
                Actualizaciones</td>
            <td class="style16">
                
                Gratuitas</td>
            <td align="right">
                $ 0.00</span></span></td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    <strong>
    <span class="auto-style62">Plan <a name="premium"> Premium </a></span>
    <span class="style33">&nbsp;<br />
    </span>
    </strong>
    <span class="style1master1"><span class="style22">
<br />
Para 
</span> <span class="style51"> <strong><span class="style22">enviar</span></strong></span><span 
        class="style58"> </span>
    <span class="auto-style63"> 
    <strong>declaraciones</strong></span><span class="style59"> </span> <span class="style10"><span class="style29">
<strong>ilimitadas</strong></span></span><span class="style22"> 
    del&nbsp; mes, sean<strong> normales o complementarias, </strong>con o sin datos, tu decides 
    cuantos meses.</span></span><br />
    <br />
    <table style="border-style: solid; border-width: thin; font-size: small;" 
        cellspacing="0">
        <tr class="style10">
            <td class="style19" style="border: 1px solid white;">
                Concepto</td>
            <td class="style17" style="border: 1px solid white;">
                Descripción</td>
            <td class="style9" align="right" style="border: 1px solid white;">
                Precio</td>
        </tr>
        <td style="border: 1px solid #f0f0f0;"><strong>Declaración Mensual</strong></td>
            <td class="style18" style="border: 1px solid #f0f0f0;">
                
                Importación, generación y <strong>envío</strong> de:<br />
                    
                
                - Declaración mensual normal <br />
                    
                
                - Declaraciones complementarias del mes ilimitadas sin costo adicional </td>
            <td align="right" class="style21" style="border: 1px solid #f0f0f0;">
                Contáctanos</td>
        </tr>
        <tr>
            <td class="style20" style="border: 1px solid #f0f0f0;">
                
                <strong>Declaración Anual</strong></td>                
                </td>
            <td style="border: 1px solid #f0f0f0;">
                    
                
                Al contratar 12 meses 
                o mas, se incluye sin costo la generación y envío de la 
                declaración anual:<br />
&nbsp;&nbsp; - Declaración anual normal<br />
                    
                
               &nbsp;&nbsp;
                    
                
               - Declaraciones complementarias del año ilimitadas<br />
                    
                Si contratas menos de 12 meses, considera un plan básico o ceros para la 
                declaración anual<td align="right" class="style21">
                $ 0.00</td>
        </tr>
        <tr>
            <td class="style20" style="border: 1px solid #f0f0f0;">
                
                <strong>Declaración complementaria</strong></td>
            <td class="style18" style="border: 1px solid #f0f0f0;">
                
                Incluye declaraciones complemetarias del mes ilimitadas.
                <br />
                Para las complementarias anuales: si se contrata un periodo de mínimo de 12 
                meses éstas están incluidas e ilimitadas dentro del periodo contratado, de otra forma considere un plan básico 
                o ceros.&nbsp; </td>
            <td align="right" class="style21" style="border: 1px solid #f0f0f0;">
                $&nbsp; 0.00</td>
        </tr>
        <tr>
            <td class="style20" style="border: 1px solid #f0f0f0;">
                
                Capacitación 
                de uso de la aplicación</td>
            <td class="style18" style="border: 1px solid #f0f0f0;">
                
                Tutoriales, Presencial en nuestras oficinas, Vía telefónica</td>
            <td align="right" class="style21" style="border: 1px solid #f0f0f0;">
                $ 0.00</td>
        </tr>
        <tr>
            <td class="style20" style="border: 1px solid #f0f0f0;">
                
                Soporte técnico</td>
            <td class="style18" style="border: 1px solid #f0f0f0;">
                
                Ilimitado en horario de oficina</td>
            <td align="right" class="style21" style="border: 1px solid #f0f0f0;">
                $ 0.00</td>
        </tr>
        <tr>
            <td class="style14">
                
                Actualizaciones</td>
            <td class="style16">
                
                Gratuitas</td>
            <td align="right">
                $ 0.00</td>
        </tr>
    </table>
    <span 
    class="style1master1">
    <br />
    </span>
    <br />    
    <a name="servicios"><strong>   
    <span class="auto-style62"><em>Asesoria</em></span></strong> <strong>
    <span class="style28">(</span><span class="auto-style64">Sin costo</span><span class="style28"> </span> </strong><span class="style38"> 
    para instituciones que se </span> <span class="style28"> 
    </span> <span class="style28"> 
    <strong>  
    <a href="registro.aspx">registren</a>)</strong></span></a>
    <br class="style29" />
    <br />
    <table align="center" bgcolor="#ffffff" border="0" cellpadding="4" 
        cellspacing="0" style="width: 200px">
          <tr>
            <td valign="top" width="200">            
                <table border="0" cellpadding="0" cellspacing="0">
                  <tbody><tr>
                    <td width="30"><img src="images/box_01.gif" alt="Introduce tus datos"></td>
                    <td background="images/box_top.gif" class="style34"><strong>Solicitud de Asesoría</strong></td>
                    <td><img src="images/box_02.gif" alt="Introduce tus datos"></td>
                  </tr>
                  <tr valign="top">
                    <td background="images/box_left.gif"></td>
                    <td background="images/box_middle.gif">
			                <div >
                      <table align="center" border="0" cellpadding="0" cellspacing="0">
                          <tbody>
                          <tr>
                            <td class="style24">
	                            <span class="auto-style65">Nombre:</span><strong class="bodytext"><br /><asp:TextBox 
                                    ID="nombre" runat="server" BackColor="lightgray" Width="170px" 
                                    MaxLength="100" CssClass="style46"></asp:TextBox>
	                            </strong><span class="style46"><br />
	                            &nbsp;<strong class="bodytext"><br />
	                            </strong>
	                            </span>
	                            <span class="auto-style66">Correo:</span><span class="style46"><br>
	                            </span>
	                            <asp:TextBox ID="correo" runat="server" BackColor="lightgray" Width="170px" 
                                    MaxLength="50" CssClass="style46"></asp:TextBox>
	                            <span class="style46">
	                            <br />
	                            <br />
	                            </span>
	                            <span class="auto-style66">
	                            Teléfonos (con Lada):</span><span class="style46"><br>
	                            </span>
	                            <asp:TextBox ID="tel" runat="server" BackColor="lightgray" Width="170px" 
                                    MaxLength="21" 
                                    
                                    ToolTip="solo use números y la coma sin espacios para agregar un segundo teléfono" 
                                    CssClass="style46"></asp:TextBox>
	                            <br />
                                </td>
                          </tr>
                        </tbody>
                      </table>
                        <br>
                        <table width="80%" align="center" border="0" cellpadding="3" cellspacing="0">
                        <tbody>
                        <tr>
                        <td style="background-color:#009900;" onMouseOver="this.style.backgroundColor='#cccc00'" 
                        onMouseOut="this.style.backgroundColor='#009900'" align="center">
                            <asp:Button ID="btnOptin" runat="server" style="font-weight: 700" 
                                Text="Enviar solicitud" Width="218px" />
                            </td>
                        </tr>
                        </tbody>
                        </table>			         
          
                        </div>
	                </td>
                    <td background="images/box_right.gif"></td>
                    </tr>
                      <tr>
                        <td><img src="images/box_03.gif" alt="Introduce tus datos"></td>
                        <td background="images/box_bottom.gif"></td>
                        <td><img src="images/box_04.gif" alt="Introduce tus datos"></td>
                      </tr>
                </tbody>
                </table>
           </td>
           </tr>
       </table>
	<br />
    <span class="style46">Cuando el Servicio de Administración Tributaria presenta problemas técnicos 
    con sus servidores que le impiden recibir las <strong>declaraciones</strong> vía <strong>socket</strong>, se 
    declara en estado de <strong>CONTINGENCIA</strong>, en cuyo caso podrías optar por esperar un poco a que el <strong>SAT</strong> restablezca el servicio electrónico o bien, podrás descargar la <strong>contingencia</strong> para presentarla en un módulo del <strong>SAT</strong> en caso de urgencia.<br />
    <br />
    <span class="style51">Precios sujetos a cambio sin previo aviso</span><br />
    </span>
    <br />
    


    

</asp:Content>
