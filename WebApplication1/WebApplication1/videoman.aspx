<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="videoman.aspx.vb" Inherits="WebApplication1.videoman" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="canonical" href="https://www.declaracioneside.com/videoman.aspx" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">   
    <section class="account pt-2">
        <div class="container">
            <div class="row">
                <div class="col-md-12">                  
                        <h4 class="text-center">Videos</h4>
                        <p class="text-center">Clic en cualquier imagen para videos</p>       
                  <p class="text-center"><a href="registro.aspx" class="btn btn-main">Quiero registrarme</a></p>
                </div>               
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="video-player bg-white rounded ">
                        <a href="#" onclick="videoman_onClick('https://www.youtube.com/embed/ixe94ZD_x18', 'Declaración de depósitos en efectivo Mensual 2022')" data-toggle="modal" data-target="#modalvideo">
                            <img class="img-fluid rounded" src="images/videoman/mensualExcel.png" style="width: 350px; height: 157.4px;" alt="NUEVO: Declaración de depósitos en efectivo Mensual 2022 con datos importando desde Excel">
                            <h5 class="text-dark text-center p-2">NUEVO: Declaración mensual de depósitos en efectivo 2022, 2023</h5>
                        </a>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="video-player bg-white rounded ">
                        <a href="#" onclick="videoman_onClick('https://www.youtube.com/embed/O_thKyhRqW4', 'Fundamento legal ide 2022')" data-toggle="modal" data-target="#modalvideo">
                            <img class="img-fluid rounded" src="images/videoman/fundamento.jpg" style="width: 350px; height: 157.4px;" alt="Fundamento legal ide 2022">
                            <h5 class="text-dark text-center p-2">Fundamento legal ide 2022</h5>
                        </a>
                        
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="video-player bg-white rounded" >
                        <a href="#" onclick="videoman_onClick('https://www.youtube.com/embed/BqSzXcjmYlA', 'Nuestra oferta de servicio para declaraciones de depósitos en efectivo')" data-toggle="modal" data-target="#modalvideo">
                            <img class="img-fluid rounded" src="images/videoman/oferta.png" style="width: 350px; height: 157.4px;" alt="Declaraciones ide video demostrativo">
                            <h5 class="text-dark text-center p-2">Oferta de servicio</h5>
                        </a>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="video-player  bg-white rounded">
                        <a href="#" onclick="videoman_onClick('https://www.youtube.com/embed/98becf3N4mw', 'Servicio de declaraciones de depósitos en efectivo y de IDE')" data-toggle="modal" data-target="#modalvideo">
                            <img class="img-fluid rounded" src="images/videoman/servicio.jpg" style="width: 350px; height: 157.4px;" alt="Declaraciones ide video demostrativo">
                            <h5 class="text-dark text-center p-2">Servicio declaraciones informativas IDE</h5>
                        </a>                                                
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="video-player  bg-white rounded">
                        <a href="#" onclick="videoman_onClick('https://www.youtube.com/user/declaracioneside', 'Layout mensual 2022 para declaraciones de depósitos en efectivo')" data-toggle="modal" data-target="#modalvideo">
                            <img class="img-fluid rounded" src="images/videoman/excel.jpg" style="width: 350px; height: 157.4px;" alt="Layout mensual 2022 para declaraciones de depósitos en efectivo">
                            <h5 class="text-dark text-center p-2">Layout mensual 2022 para declaraciones de depósitos en efectivo</h5>
                        </a>                        
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="video-player  bg-white rounded">
                        <a href="#" onclick="videoman_onClick('https://www.youtube.com/embed/JvYJSxrl69Q', 'Declaración Mensual de depósitos en efectivo (e IDE) importando desde XML < 2022, recuperacion de acuses')" data-toggle="modal" data-target="#modalvideo">
                            <img class="img-fluid rounded" src="images/videoman/mensualXML.png" style="width: 350px; height: 157.4px;" alt="Declaracion mensual de depositos en efectivo en IDE">
                            <h5 class="text-dark text-center p-2">Declaración Mensual de depósitos en efectivo (e IDE) desde XML < 2022, recuperacion de acuses</h5>
                        </a>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="video-player  bg-white rounded">
                        <a href="#" onclick="videoman_onClick('https://www.youtube.com/embed/H9jjSI-oZAY', 'Declaración Anual de depósitos en efectivo (e IDE) importando desde Excel <=2013')" data-toggle="modal" data-target="#modalvideo">
                            <img class="img-fluid rounded" src="images/videoman/anualCeros_excel.jpg" style="width: 350px; height: 157.4px;" alt="Declaración Anual de depósitos en efectivo (e IDE) importando desde Excel <=2013">
                            <h5 class="text-dark text-center p-2">Declaración Anual de depósitos en efectivo (e IDE) importando desde Excel <=2013</h5>
                        </a>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="video-player bg-white rounded">
                        <a href="#" onclick="videoman_onClick('https://www.youtube.com/embed/Gq5BxiZi0AI', 'Declaración informativa Anual de depósitos en efectivo en Ceros < 2022, recuperacion de acuses')" data-toggle="modal" data-target="#modalvideo">
                            <img class="img-fluid rounded" src="images/videoman/anualCeros.jpg" style="width: 350px; height: 157.4px;" alt="Declaracion anual de depositos en efectivo en IDE">
                            <h5 class="text-dark text-center p-2">Declaración informativa Anual de depósitos en efectivo en Ceros < 2022, recuperacion de acuses</h5>
                        </a>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="video-player  bg-white rounded">
                        <a href="#" onclick="videoman_onClick('https://www.youtube.com/embed/dsCn47OGG0A', 'Declaración Anual de depósitos en efectivo (e IDE) importando desde XML < 2022, recuperacion de acuses')" data-toggle="modal" data-target="#modalvideo">
                            <img class="img-fluid rounded" src="images/videoman/anualXML.jpg" style="width: 350px; height: 157.4px;" alt="Declaracion anual de depositos en efectivo en IDE">
                            <h5 class="text-dark text-center p-2">Declaración Anual de depósitos en efectivo (e IDE) desde XML < 2022, recuperacion de acuses</h5>
                        </a>
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
            document.getElementById("video").innerHTML = "<div class=\"row\"><div class=\"col-sm-12\">" + "<iframe class=\"p-3\" style=\"width: 100%; height: 500px;\"  src=\"" + url + "?rel=0" + "\" frameborder=\"0\" allow=\"autoplay; encrypted-media\"allowfullscreen></iframe></div >";
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
