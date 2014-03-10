var timeout         = 500;
var closetimer		= 0;
var ddmenuitem      = 0;

function cpanel_open()
{	cpanel_canceltimer();
	cpanel_close();
	ddmenuitem = $(this).find('ul').eq(0).css('visibility', 'visible');
}

function cpanel_close()
{	if(ddmenuitem) ddmenuitem.css('visibility', 'hidden');
	
}

function cpanel_timer()
{	closetimer = window.setTimeout(cpanel_close, timeout);}

function cpanel_canceltimer()
{	if(closetimer)
	{	window.clearTimeout(closetimer);
		closetimer = null;}}

$(document).ready(function()
{	
	
	
	$('.Control_Panel_Table > li').bind('mouseover', cpanel_open);
	$('.Control_Panel_Table > li').bind('mouseout',  cpanel_timer);
	
	$(".Control_Panel_Table > li").each(
	  function() {
	    var elem = $(this);
	    if (elem.children('ul').children().length == 0) {
	      elem.remove();
	    }
	  }
	);


});

document.onclick = cpanel_close;