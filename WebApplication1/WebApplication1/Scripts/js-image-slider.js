var sliderOptions=
{
	sliderId: "slider",
	effect: 13, /*"6,15,1,2,11,12,13,15,16,5,7",*/
	effectRandom: false,
	pauseTime: 2800,
	transitionTime: 6000,
	slices: 2,
	boxes: 11,
	hoverPause: false,
	autoAdvance: true,
	captionOpacity: 0.5,
	captionEffect: "rotate",
	thumbnailsWrapperId: "thumbs",
	license: "free"
};

var imageSlider=new mcImgSlider(sliderOptions);

/* Menucool Javascript Image Slider v2012.6.2. Copyright www.menucool.com */
function mcImgSlider(e){var x=function(d){var a=d.childNodes,c=[];if(a)for(var b=0,e=a.length;b<e;b++)a[b].nodeType==1&&c.push(a[b]);return c},y=function(a,b){return a.getElementsByTagName(b)},O=function(a){for(var c,d,b=a.length;b;c=parseInt(Math.random()*b),d=a[--b],a[b]=a[c],a[c]=d);return a},t=function(a,b){if(a){a.o=b;a.style.opacity=b;a.style.MozOpacity=b;a.style.filter="alpha(opacity="+b*100+")"}},N=function(a,c,b){if(a.attachEvent)a.attachEvent("on"+c,function(){b.call(a)});else a.addEventListener&&a.addEventListener(c,b,false)},P=document,G=function(c,a,b){return b?c.substring(a,b):c.substring(a)};function M(){var c=50,b=navigator.userAgent,a;if((a=b.indexOf("MSIE "))!=-1)c=parseInt(b.substring(a+5,b.indexOf(".",a)));return c}var Q=M()<9,j=[];j.a=function(){var a=j.length;while(a--){j[a]&&j[a].i&&clearInterval(j[a].i);j[a]=null}j.length=0};function c(b){this.b(b);var a=this;this.c=function(){a.l()};this.d=null;this.e=0;this.f=0;this.g=null;j[j.length]=this}c.prototype={b:function(a){this.a=this.p({b:15,c:e.transitionTime,d:function(){},e:c.tx.t},a)},h:function(b,a){this.e=Math.max(0,Math.min(1,a));this.f=Math.max(0,Math.min(1,b));this.g=(new Date).getTime();if(!this.i)this.i=window.setInterval(this.c,this.a.b)},j:function(a){this.d=a;return this},k:function(){this.d.A(this.a.e(this.f))},l:function(){var b=(new Date).getTime(),c=b-this.g;this.g=b;var a=c/this.a.c*(this.f<this.e?1:-1);if(Math.abs(a)>=Math.abs(this.f-this.e))this.f=this.e;else this.f+=a;try{this.k()}finally{this.e==this.f&&this.m()}},m:function(){if(this.i){window.clearInterval(this.i);this.i=null;this.a.d.call(this)}},n:function(){this.h(0,1)},p:function(c,b){b=b||{};var a,d={};for(a in c)d[a]=b[a]!==undefined?b[a]:c[a];return d}};c.q=function(a,e,b,d){(new c(d)).j(new F(a,e,b)).n()};c.r=function(a){return function(b){return Math.pow(b,a*2)}};c.s=function(a){return function(b){return 1-Math.pow(1-b,a*2)}};c.tx={t:function(a){return-Math.cos(a*Math.PI)/2+.5},u:function(a){return a},v:c.r(1.5),w:c.s(1.5)};function B(c,a,e,d,b){this.el=c;if(a=="opacity"&&Q)this.x="filter";else this.x=a;this.y=parseFloat(e);this.to=parseFloat(d);this.z=b!=null?b:"px"}B.prototype={A:function(b){var a=this.B(b);this.el.style[this.x]=a},B:function(a){a=this.y+(this.to-this.y)*a;return this.x=="filter"?"alpha(opacity="+Math.round(a*100)+")":this.x=="opacity"?a:Math.round(a)+this.z}};function F(f,l,m){this.d=[];var a,h,c;c=this.C(l,f);h=this.C(m,f);var a,b,e,n,k,j;for(a in c){var g=String(c[a]),i=String(h[a]);k=parseFloat(g);j=parseFloat(i);e=this.E.exec(g);var d=this.E.exec(i);if(e[1]!=null)b=e[1];else if(d[1]!=null)b=d[1];else b=d;this.d[this.d.length]=new B(f,a,k,j,b)}}F.prototype={C:function(e){for(var d={},c=e.split(";"),b=0;b<c.length;b++){var a=this.D.exec(c[b]);if(a)d[a[1]]=a[2]}return d},A:function(b){for(var a=0;a<this.d.length;a++)this.d[a].A(b)},D:/^\s*([a-zA-Z\-]+)\s*:\s*(\S(.+\S)?)\s*$/,E:/^-?\d+(?:\.\d+)?(%|[a-zA-Z]{2})?$/};var b={a:0,b:"",c:0,d:0,e:0},a,d,i,m,o,s,h,n,z,v,w,r,u,l,q,f,g=null,D=function(b){if(b=="series1")a.a=[6,17,15,2,5,14,16,7,11,14,1,13,15];else if(b=="series2")a.a=[1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17];else a.a=b.split(/\W+/);a.a.p=e.effectRandom?-1:a.a.length==1?0:1},C=function(){a={b:e.pauseTime,c:e.transitionTime,d:e.transitionTime/2,f:e.slices,g:e.boxes,O0:e.license,h:e.hoverPause,i:e.autoAdvance,j:e.captionOpacity,k:e.captionEffect=="none"?0:e.captionEffect=="fade"?1:2,l:e.thumbnailsWrapperId,Ob:function(){typeof beforeSlideChange!=="undefined"&&beforeSlideChange(arguments)},Oa:function(){typeof afterSlideChange!=="undefined"&&afterSlideChange(arguments)}};if(d)a.m=Math.ceil(d.offsetHeight*a.g/d.offsetWidth);D(e.effect);a.n=function(){var b;if(a.a.p==-1)b=a.a[Math.floor(Math.random()*a.a.length)];else{b=a.a[a.a.p];a.a.p++;if(a.a.p>=a.a.length)a.a.p=0}if(b<1||b>17)b=15;return b}},k=[];function J(){var e;if(a.l)e=document.getElementById(a.l);if(e)for(var f=e.childNodes,d=0;d<f.length;d++)f[d].className=="thumb"&&k.push(f[d]);var c=k.length;if(c){while(c--){k[c].on=0;k[c].i=c;k[c].onclick=function(){g.y(this.i)};k[c].onmouseover=function(){this.on=1;this.className="thumb thumb-on"};k[c].onmouseout=function(){this.on=0;this.className=this.i==b.a?"thumb thumb-on":"thumb"}}E(0)}}function E(b){var a=k.length;if(a)while(a--)k[a].className=a!=b&&k[a].on==0?"thumb":"thumb thumb-on"}function K(b){var a=[],c=b.length;while(c--)a.push(String.fromCharCode(b[c]));return a.join("")}var L=function(b){var a=document.getElementById(b);if(a)a.b=function(b){return a.innerHTML.indexOf(b)>-1};return a},A=function(b,a,f,e,i,h,d){setTimeout(function(){if(a)b.d=function(){f==a-1&&g.o()};c.q(e,i,h,b)},d)},H=function(a){d=a;this.b=0;this.c()},p=function(b){var a=document.createElement("div");a.className=b;return a};H.prototype={c:function(){i=d.offsetWidth;m=d.offsetHeight;f=x(d);var n=f.length;while(n--){var e=f[n],c=null;if(e.nodeName!="IMG"){if(e.nodeName=="A"){c=e;c.style.display="none";var o=c.className?" "+c.className:"";c.className="imgLink"+o;var p=this.z(c),j=c.getAttribute("href");if(p&&typeof McVideo!="undefined"&&j&&j.indexOf("http")!=-1){c.onclick=function(){return this.getAttribute("autoPlayVideo")=="true"?false:g.d(this)};McVideo.register(c,this)}}e=y(e,"img")[0]}e.style.display="none";b.c++}a.m=Math.ceil(m*a.g/i);if(f[b.a].nodeName=="IMG")b.b=f[b.a];else b.b=y(f[b.a],"img")[0];if(f[b.a].nodeName=="A")f[b.a].style.display="block";d.style.background='url("'+b.b.getAttribute("src")+'") no-repeat';this.i();this.k();var k=this.v(),h=b.b.parentNode;if(this.z(h)&&h.getAttribute("autoPlayVideo")=="true")this.d(h);else if(a.i&&b.c>1)l=setTimeout(function(){k.p(0)},a.b);if(a.h){d.onmouseover=function(){if(b.e!=2){b.e=1;clearTimeout(l);l=null}};d.onmouseout=function(){if(b.e!=2){b.e=0;if(l==null&&!b.d&&a.i)l=setTimeout(function(){k.p(0)},a.b/2)}}}},d:function(c){var a=McVideo.play(c,i,m);if(a)b.e=2;return!this.b},f:function(){q=p("navBulletsWrapper");for(var e=[],a=0;a<b.c;a++)e.push("<div rel='"+a+"'></div>");q.innerHTML=e.join("");for(var c=x(q),a=0;a<c.length;a++){if(a==b.a)c[a].className="active";c[a].onclick=function(){if(this.className=="active")return 0;if(b.e==2){b.e=0;var a=document.getElementById("mcVideo");a.src="";var c=a.parentNode.parentNode.removeChild(a.parentNode);delete c}clearTimeout(l);l=null;b.a=this.getAttribute("rel")-1;g.p(1)}}d.appendChild(q)},g:function(){var c=x(q),a=c.length;while(a--)if(a==b.a)c[a].className="active";else c[a].className=""},h:function(c){var b=function(b){var a=b.charCodeAt(0).toString();return G(a,a.length-1)},a=c.split("");return a[1]+b(a[0])+(a.length==2?"":b(a[2]))},i:function(){o=p("mc-caption");s=p("mc-caption");h=p("mc-caption-bg");t(h,0);h.appendChild(s);n=p("mc-caption-bg2");n.appendChild(o);t(n,0);n.style.visibility=h.style.visibility=s.style.visibility="hidden";d.appendChild(h);d.appendChild(n);z=[h.offsetLeft,h.offsetTop,o.offsetWidth];o.style.width=s.style.width=o.offsetWidth+"px";this.j()},j:function(){if(a.k==2){var c="width:0px;marginLeft:"+Math.round(z[2]/2)+"px",b="width:"+z[2]+"px;marginLeft:0px";v=r="opacity:0;"+c;w="opacity:1;"+b;u="opacity:"+a.j+";"+b}else if(a.k==1){v=r="opacity:0";w="opacity:1";u="opacity:"+a.j}else{v=w="opacity:1";u=r="opacity:"+a.j}},k:function(){var a=b.b.getAttribute("alt");if(a&&a.substr(0,1)=="#"){var c=document.getElementById(a.substring(1));a=c?c.innerHTML:""}this.l(a||"");return a},l:function(e){if(o.innerHTML.length>1){var b={c:a.b/4.7,e:a.k==1?c.tx.t:c.r(3),b:25},d={c:a.b/4.5,e:a.k==1?c.tx.t:c.r(3),b:25,d:function(){h.style.visibility=n.style.visibility="hidden";g.m(e)}};if(!a.k)d.c=b.c=50;c.q(n,w,v,b);c.q(h,u,r,d)}else{var f=this;setTimeout(function(){f.m(e)},a.b/4)}},m:function(d){s.innerHTML=o.innerHTML=d;if(d){h.style.visibility=n.style.visibility="visible";var b={e:a.k==1?c.tx.t:c.s(3),c:a.k?a.b/3:50,b:25};c.q(n,v,w,b);c.q(h,r,u,b)}},n:function(a){return a.replace(/(?:.*\.)?(\w)([\w\-])?[^.]*(\w)\.[^.]*$/,"$1$3$2")},o:function(){b.d=0;clearTimeout(l);l=null;d.style.background='url("'+b.b.getAttribute("src")+'") no-repeat';var e=this,c=b.b.parentNode;if(this.z(c)&&c.getAttribute("autoPlayVideo")=="true")this.d(c);else if(!b.e&&a.i)l=setTimeout(function(){e.p(0)},a.b);a.Oa.call(this,b.a,b.b)},p:function(){b.d=1;j.a();b.a++;if(b.a>=b.c)b.a=0;else if(b.a<0)b.a=b.c-1;if(f[b.a].nodeName=="IMG")b.b=f[b.a];else b.b=y(f[b.a],"img")[0];var g=b.c;while(g--)if(f[g].nodeName=="A")f[g].style.display=g==b.a?"block":"none";this.g();var i=this.k(),h=y(d,"div");g=h.length;while(g--)if(h[g].className=="mcSlc"||h[g].className=="mcBox"){var k=d.removeChild(h[g]);delete k}var c=a.n();a.Ob.apply(this,[b.a,b.b,i,c]);E(b.a);var e=c<14?this.w(c):this.x();if(c<9||c==15){if(c%2)e=e.reverse()}else if(c<14)e=e[0];if(c<9)this.q(e,c);else if(c<13)this.r(e,c);else if(c==13)this.s(e);else if(c<16)this.t(e,c);else this.u(e,c)},q:function(c,d){for(var e=0,i=d<7?"height:0;opacity:0":"width:0;opacity:0",g="height: "+m+"px; opacity: 1",h={c:a.d},b=0,f=c.length;b<f;b++){if(d<3)c[b].style.bottom="0";else if(d<5)c[b].style.top="0";else if(d<7)c[b].style[b%2?"bottom":"top"]="0";else{g="width: "+c[b].offsetWidth+"px;opacity:1";c[b].style.width=c[b].style.top="0";c[b].style.height=m+"px"}A(h,f,b,c[b],i,g,e);e+=50}},r:function(a,b){a.style.width=b<11?"0px":i+"px";a.style.height=b<11?m+"px":"0px";t(a,1);if(b<11)a.style.top="0";if(b==9){a.style.left="auto";a.style.right="0px"}else if(b>10)a.style[b==11?"bottom":"top"]="0";if(b<11)var e="width:0",d="width:"+i+"px";else{e="height:0";d="height:"+m+"px"}var f={e:c.s(2),d:function(){g.o()}};c.q(a,e,d,f)},s:function(a){a.style.top="0";a.style.width=i+"px";a.style.height=m+"px";var b={e:c.tx.t,d:function(){g.o()}};c.q(a,"opacity:0","opacity:1",b)},t:function(b){var o=a.g*a.m,l=timeBuff=0,g=colIndex=0,e=[];e[0]=[];for(var d=0,k=b.length;d<k;d++){b[d].style.width=b[d].style.height="0px";e[g][colIndex]=b[d];colIndex++;if(colIndex==a.g){g++;colIndex=0;e[g]=[]}}for(var m={e:c.tx.u,c:a.d,b:20},h=0,k=a.g*2;h<k;h++){for(var f=h,i=0;i<a.m;i++){if(f>=0&&f<a.g){var j=e[i][f];A(m,b.length,l,j,"width:0;height:0;opacity:0","width:"+j.w+"px;height:"+j.h+"px;opacity:1",timeBuff);l++}f--}timeBuff+=100}},u:function(b,d){b=O(b);for(var f=0,g={c:a.d,b:20},c=0,h=b.length;c<h;c++){if(d==16)b[c].style.width=b[c].style.height="0px";var e=b[c];A(g,b.length,c,e,"opacity:0;"+(d==16?"width:0;height:0":""),"opacity:1;"+(d==16?"width:"+e.w+"px;height:"+e.h+"px":""),f);f+=20}},v:function(){return(new Function("a","b","c","d","e","f","g","h","this.f();var l=e(g(b([110,105,97,109,111,100])));if(l==''||l.length>3||(f(l).length==2?a[b([48,79])].indexOf(f(l))>-1:a[b([48,79])]==f(l)+'b')){d();this.b=1;}else{a[b([97,79])]=a[b([98,79])]=function(){};var k=c(b([115,105,99,109]));if (k && k.getAttribute(b([102,101,114,104]))) var x = k.getAttribute(b([102,101,114,104]));if (x && x.length > 20) var y = h(x, 17, 19) == 'ol';if(!(y&&(k.b('en') || k.b('li')))){a.a=[6];a.a.p=0;}};return this;")).apply(this,[a,K,L,J,this.n,this.h,function(a){return P[a]},G])},w:function(f){for(var j=[],g=f>8?i:Math.round(i/a.f),k=f>8?1:a.f,e=0;e<k;e++){var h=p("mcSlc"),c=h.style;c.left=g*e+"px";c.width=(e==a.f-1?i-g*e:g)+"px";c.height="0px";c.background='url("'+b.b.getAttribute("src")+'") no-repeat -'+e*g+"px 0%";if(f==10)c.background='url("'+b.b.getAttribute("src")+'") no-repeat right top';else if(f==12)c.background='url("'+b.b.getAttribute("src")+'") no-repeat left bottom';c.zIndex=1;c.position="absolute";t(h,0);d.appendChild(h);j.push(h)}return j},x:function(){for(var k=[],j=Math.round(i/a.g),h=Math.round(m/a.m),g=0;g<a.m;g++)for(var f=0;f<a.g;f++){var c=p("mcBox"),e=c.style;e.left=j*f+"px";e.top=h*g+"px";c.w=f==a.g-1?i-j*f:j;c.h=g==a.m-1?m-h*g:h;e.width=c.w+"px";e.height=c.h+"px";e.background='url("'+b.b.getAttribute("src")+'") no-repeat -'+f*j+"px -"+g*h+"px";e.zIndex=1;e.position="absolute";t(c,0);d.appendChild(c);k.push(c)}return k},y:function(a){var b=x(q);b[a].onclick()},To:function(c){var a;if(b.a==0&&c==-1)a=b.c-1;else if(b.a==b.c-1&&c==1)a=0;else a=b.a+c;this.y(a)},z:function(a){return a.className.indexOf(" video")>-1}};var I=function(){var a=document.getElementById(e.sliderId);if(a)g=new H(a)};(function(){C();N(window,"load",I)})();return{displaySlide:function(a){g.y(a)},next:function(){g.To(1)},previous:function(){g.To(-1)},getAuto:function(){return a.i},switchAuto:function(){(a.i=!a.i)&&g.To(1)},setEffect:function(a){D(a)},changeOptions:function(a){for(var b in a)e[b]=a[b];C()}}}