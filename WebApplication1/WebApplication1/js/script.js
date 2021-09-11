                           

(function ($) { "use strict";
	
	/* ========================================================================= */
	/*	Page Preloader
	/* ========================================================================= */
	
	// window.load = function () {
	// 	document.getElementById('preloader').style.display = 'none';
	// }

	$(window).on("load",function(){
		$('#preloader').fadeOut('slow',function(){$(this).remove();});
	});




	/* ========================================================================= */
	/*	Portfolio Filtering Hook
	/* =========================================================================  */
	$('.play-icon i').click(function() {
		var video = '<iframe allowfullscreen src="' + $(this).attr('data-video') + '"></iframe>';
		$(this).replaceWith(video);
	});







	/* ========================================================================= */
	/*	Portfolio Filtering Hook
	/* =========================================================================  */

	var portfolio_item = $('.portfolio-items-wrapper');
	if (portfolio_item.length) {
		var mixer = mixitup(portfolio_item);
	};



	
	
	/* ========================================================================= */
	/*	Testimonial Carousel
	/* =========================================================================  */
 
	//Init the slider
	$('.testimonial-slider').slick({
		slidesToShow: 3,
		slidesToScroll: 1,
		infinite: true,
        dots:true,
		arrows:false,
		autoplay: true,
  		autoplaySpeed: 2000,
  		responsive: [
		    {
		      breakpoint: 600,
		      settings: {
		        slidesToShow: 1,
		        slidesToScroll: 2
		      }
		    },
		    {
		      breakpoint: 480,
		      settings: {
		        slidesToShow: 1,
		        slidesToScroll: 1
		      }
		    }
		  ]
	});


	/* ========================================================================= */
	/*	Clients Slider Carousel
	/* =========================================================================  */
 
	//Init the slider
	$('.clients-logo-slider').slick({
		infinite: true,
		arrows:false,
		autoplay: true,
  		autoplaySpeed: 2000,
  		slidesToShow: 5,
  		slidesToScroll: 1,
	});




	/* ========================================================================= */
	/*	Company Slider Carousel
	/* =========================================================================  */
	$('.company-gallery').slick({
		infinite: true,
		arrows:false,
		autoplay: true,
  		autoplaySpeed: 2000,
  		slidesToShow: 5,
  		slidesToScroll: 1,
	});
	
	
	/* ========================================================================= */
	/*	Awars Counter Js
	/* =========================================================================  */
	




	/* ========================================================================= */
	/*   Contact Form Validating
	/* ========================================================================= */


    $('#btnSubmit').click(function (e) {

		

		/* declare the variables, var error is the variable that we use on the end
		to determine if there was an error or not */
		var error = false;
		var name = $('#name').val();
		var email = $('#email').val();
        var subject = $('#tele').val().toString();
		var message = $('#message').val().toString();

		/* in the next section we do the checking by using VARIABLE.length
		where VARIABLE is the variable we are checking (like name, email),
		length is a JavaScript function to get the number of characters.
		And as you can see if the num of characters is 0 we set the error
		variable to true and show the name_error div with the fadeIn effect. 
		if it's not 0 then we fadeOut the div( that's if the div is shown and
		the error is fixed it fadesOut. 
		
		The only difference from these checks is the email checking, we have
		email.indexOf('@') which checks if there is @ in the email input field.
		This JavaScript function will return -1 if no occurrence have been found.*/
        if (name.length == 0) {
            //stop the form from being submitted
            e.preventDefault();
			var error = true;            
            $("#mensaje1").fadeIn("slow");
		} else {            
            $("#mensaje1").fadeOut();
		}
        if (email.length == 0 || email.indexOf('@') == '-1') {
            e.preventDefault();
			var error = true;           
            $("#mensaje2").fadeIn("slow");
		} else {            
            $("#mensaje2").fadeOut();
		}
        if (subject.length < 10) {
            e.preventDefault();
			var error = true;            
            $("#mensaje3").fadeIn("slow");
		} else {           
            $("#mensaje3").fadeOut();
		}
        if (message.length == 0) {
            e.preventDefault();
			var error = true;            
            $("#mensaje4").fadeIn("slow");
		} else {            
            $("#mensaje4").fadeOut();
		}

		//now when the validation is done we check if the error variable is false (no errors)
        if (error == false) {		
            $('#contact-submit').attr({
                'disabled': 'false',
                'value': 'Enviando...'
            });
            $(e.currentTarget).trigger('submit', { 'lots_of_stuff_done': true });
          
			/* using the jquery's post(ajax) function and a lifesaver
			function serialize() which gets all the data from the form
			we submit it to send_email.php */			
		}
    });
   
       $('#btnSubmit2').click(function (e) {

		//stop the form from being submitted
		e.preventDefault();

		/* declare the variables, var error is the variable that we use on the end
		to determine if there was an error or not */
		var error2 = false;
		var name2 = $('#name2').val();
		var email2 = $('#email2').val();
        var subject2 = $('#tele2').val().toString();
		var message2 = $('#message2').val().toString();

		/* in the next section we do the checking by using VARIABLE.length
		where VARIABLE is the variable we are checking (like name, email),
		length is a JavaScript function to get the number of characters.
		And as you can see if the num of characters is 0 we set the error
		variable to true and show the name_error div with the fadeIn effect. 
		if it's not 0 then we fadeOut the div( that's if the div is shown and
		the error is fixed it fadesOut. 
		
		The only difference from these checks is the email checking, we have
		email.indexOf('@') which checks if there is @ in the email input field.
		This JavaScript function will return -1 if no occurrence have been found.*/
		if (name2.length == 0) {
			var error2 = true;            
            $("#mensaje11").fadeIn("slow");
		} else {            
            $("#mensaje11").fadeOut();
		}
		if (email2.length == 0 || email2.indexOf('@') == '-1') {
			var error2 = true;           
            $("#mensaje21").fadeIn("slow");
		} else {            
            $("#mensaje21").fadeOut();
		}
		if (subject2.length < 10) {
			var error = true;            
            $("#mensaje31").fadeIn("slow");
		} else {           
            $("#mensaje3").fadeOut();
		}
		if (message2.length == 0) {
			var error = true;            
            $("#mensaje41").fadeIn("slow");
		} else {            
            $("#mensaje41").fadeOut();
		}

		//now when the validation is done we check if the error variable is false (no errors)
		if (error == false) {
			//disable the submit button to avoid spamming
			//and change the button text to Sending...
            $('#btnSubmit2').attr({
				'disabled': 'false',
				'value': 'Enviando...'
			});

			/* using the jquery's post(ajax) function and a lifesaver
			function serialize() which gets all the data from the form
			we submit it to send_email.php */			
		}
    });
   



/* ========================================================================= */
/*	On scroll fade/bounce effect
/* ========================================================================= */
	var scroll = new SmoothScroll('a[href*="#"]');
	




/* ========================================================================= */
	/*	Header Scroll Background Change
	/* ========================================================================= */	
	
$(window).scroll(function() {    
var scroll = $(window).scrollTop();
 //console.log(scroll);
if (scroll > 200) {
    //console.log('a');
    $(".navigation").addClass("sticky-header");
} else {
    //console.log('a');
    $(".navigation").removeClass("sticky-header");
}});


})(jQuery);



//window.marker = null;

//function initialize() {
//    var map;

//    var nottingham = new google.maps.LatLng(51.507351, -0.127758);

//    var style = [
//    {
//        "stylers": [
//            {
//                "hue": "#ff61a6"
//            },
//            {
//                "visibility": "on"
//            },
//            {
//                "invert_lightness": true
//            },
//            {
//                "saturation": 40
//            },
//            {
//                "lightness": 10
//            }
//        ]
//    }
//];

//    var mapOptions = {
//        // SET THE CENTER
//        center: nottingham,

//        // SET THE MAP STYLE & ZOOM LEVEL
//        mapTypeId: google.maps.MapTypeId.ROADMAP,
//        zoom:9,

//        // SET THE BACKGROUND COLOUR
//        backgroundColor:"#000",

//        // REMOVE ALL THE CONTROLS EXCEPT ZOOM
//        zoom:17,
//        panControl:false,
//        zoomControl:true,
//        mapTypeControl:false,
//        scaleControl:false,
//        streetViewControl:false,
//        overviewMapControl:false,
//        zoomControlOptions: {
//            style:google.maps.ZoomControlStyle.LARGE
//        }

//    }
//    map = new google.maps.Map(document.getElementById('map'), mapOptions);

//    // SET THE MAP TYPE
//    var mapType = new google.maps.StyledMapType(style, {name:"Grayscale"});
//    map.mapTypes.set('grey', mapType);
//    map.setMapTypeId('grey');

//    //CREATE A CUSTOM PIN ICON
//    var marker_image ='plugins/google-map/images/marker.png';
//    var pinIcon = new google.maps.MarkerImage(marker_image,null,null, null,new google.maps.Size(74, 73));

//    marker = new google.maps.Marker({
//        position: nottingham,
//        map: map,
//        icon: pinIcon,
//        title: 'eventre'
//    });
//}

//google.maps.event.addDomListener(window, 'load', initialize);




                            