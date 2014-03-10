/**
 * If the web browser is IE6 or below, display a close-able little notice bar 
 * on the top of the web page, to notify user to upgrade to a better browser.
 *
 * Usage: Simply include this file in the pages that you want the notice bar appear.
 * 
 * Hesky Ji<hji003@gmail.com>
 * Maikel Daloo<maikel.daloo@gmail.com> 
 *
 * public-beta-0.1
 * 11/08/2008
 */

var ua = navigator.userAgent.toLowerCase();
var client = {
    isStrict:   document.compatMode == 'CSS1Compat',
    isOpera:    ua.indexOf('opera') > -1,
    isIE:       ua.indexOf('msie') > -1,
    isIE7:      ua.indexOf('msie 7') > -1,
	isIE8:      ua.indexOf('msie 8') > -1,	
	isIE9:      ua.indexOf('msie 9') > -1,	
	isIE10:      ua.indexOf('msie 10') > -1,	
	isIE11:      ua.indexOf('msie 11') > -1,	
    isSafari:   /webkit|khtml/.test(ua),
    isWindows:  ua.indexOf('windows') != -1 || ua.indexOf('win32') != -1,
    isMac:      ua.indexOf('macintosh') != -1 || ua.indexOf('mac os x') != -1,
    isLinux:    ua.indexOf('linux') != -1
};
client.isBorderBox = client.isIE && !client.isStrict;
client.isSafari3 = client.isSafari && !!(document.evaluate);
client.isGecko = ua.indexOf('gecko') != -1 && !client.isSafari;

/**
 * You're not sill using IE6 are you?
 *
 * @var         Boolean
 * @private
 */
var ltIE7 = client.isIE && !client.isIE7 && !client.isIE8 && !client.isIE9 && !client.isIE10 && !client.isIE11;

if(ltIE7){
if ($.cookie('IE6ALERT') != 1 ){
  addLoadEvent(display_warning);
  }
}

function addLoadEvent(func) {
  var oldonload = window.onload;
  if (typeof window.onload != 'function') {
    window.onload = func;
  } else {
    window.onload = function() {
      func();
      if (oldonload) {
        oldonload();
      }
    }
  }
}
  
function display_warning(){
    
	var html =  '<div id="browserModal" class="modal_container">';
	html +='<div class="alert_box">';
		html +='<span class="advice_txt">ADVERTENCIA!</span><br /><br />';
     html +='<span class="modal_text">Su navegador actual no es soportado por<br />';
				 html +='  este y otros sitios. Descargue la versi&oacute;n<br />';
								 html +='actualizada de su navegador favorito<br />';
							   html +='para una &oacute;ptima navegabilidad.  <br />';
   html +='</span>';
   html +='</div>';
   html +='<div class="browsers_link">';
	    
         html +='<div class="ie_link_div">';
    	 html +='<a class="browser" href="http://www.microsoft.com/spain/windows/internet-explorer/" target="_blank">Internet <br/>';
         html +='		Explorer';
         html +='</a>';
    	 html +='</div>';
        
 html +='        <div class="ff_link_div">';
		html +='<a class="browser" href="http://www.mozilla-europe.org/es/firefox/" target="_blank">Mozilla <br />';
 html +='																		        			Firefox';
         html +='</a>';
    	 html +='</div>';
        
   	 html +='<div class="ch_link_div">';
       	 html +='<a class="browser" href=" http://www.google.com/chrome" target="_blank">Google <br />';
 html +='																	Chrome';
		 html +='</a>';
     html +='</div>';
     html +='</div>';
	 html +='</div>';
	
	
	$(html).appendTo('#superPrincipalMenu');
  $('.modal_container').dialog({ autoOpen: true, modal:true, width: 410} );
  
}