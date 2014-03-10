<!--
/**** Created by Geoff Groskreutz: groskrg@versifit.com			****
 ***		Version Date: 4/8/2004 - Primary support for IE5.0+  ***
		Updated 10/14/2004 - Added better support for Netscape and persistent Resizing for IE5.0+  ***
 Purpose: To resize content area of modules, based upon overall
   width of module due to auto stretching on table cells,
   or a window resize operation. ***/

function rb_PortalModules(){
   this.Modules = new Array();
   this.Add = rb_AddPortalModule;
   this.ProcessAll = rb_AutoResize_PortalModules;
   this.GetModules = rb_Get_PortalModules;
}
function rb_Get_PortalModules(arrayObj){
	if(arrayObj){
		for(var i = 0; i < arrayObj.length; i++)
			this.Add(arrayObj[i]);
	}
}
function rb_AddPortalModule(moduleId){
	if (document.getElementById && document.getElementById(moduleId)){
		document.getElementById(moduleId)._offset = rb_findOffset_Element;
		this.Modules[this.Modules.length] = document.getElementById(moduleId);
	}
}
document._portalmodules = new rb_PortalModules();
function rb_findOffset_Element()
{
	var offset = 0;
	var curStyle = this.currentStyle;
	if (curStyle) {
		var leftp = parseInt(curStyle.paddingLeft);
		var rightp = parseInt(curStyle.paddingRight);
		offset += ((!isNaN(leftp))?leftp:0)+((!isNaN(rightp))?rightp:0);
	}
	else {
		offset += (this.offsetLeft);
		//if (this.offsetParent)
			//offset -= (this.offsetParent.offsetLeft);
	}
	return offset;
}
function rb_AutoResize_PortalModules(){
	var isNS6 = (document.getElementById && !document.all);
	window.document._resizeobj_container = this;
	window.onresize = function(){ if (window.document._resizeobj_container) window.document._resizeobj_container.ProcessAll();};
	for (var i = 0; i < this.Modules.length; i++){
		//curMod is typically a div element contained in a table cell
		var curMod=this.Modules[i]; 
		
		if (curMod && curMod.offsetParent) { 
		    var ResizeFunc = new Function("event","if(window.onresize && window.onresize.toString().indexOf('if (window.document._resizeobj_container) window.document._resizeobj_container.ProcessAll();') < 1) { var sfunc = window.onresize.toString(); var bIdx = sfunc.indexOf('{'); var eIdx = sfunc.lastIndexOf('}'); var nfunc = sfunc.substring(bIdx+1,eIdx) + '; if (window.document._resizeobj_container) window.document._resizeobj_container.ProcessAll();'; window.onresize = new Function('event',nfunc);  }");
		    if (!isNS6)
				curMod.offsetParent.onresize = ResizeFunc;
		    var prnt = curMod.offsetParent;
		    prnt._offset = rb_findOffset_Element;
			
			var pOffW = prnt.offsetWidth;
			var cOffW = curMod.offsetWidth;
			var cOffH = curMod.offsetHeight;
			var cScrW = curMod.scrollWidth;
			var cScrH = curMod.scrollHeight;
			var cSWidth = curMod.style.width;
			var offsetp = prnt._offset();
			if (pOffW && cOffW){
				if (pOffW-offsetp >= cOffW){
					var offsetw = 0;
					var offseth = isNS6?40:20;
					if (cOffH == cScrH && cScrW && cScrW > cOffW)
						curMod.style.height = cOffH+offseth;
					if(isNS6 && cSWidth && cScrW < pOffW){
						offsetw = -20;
						if (offsetp < 10 && offsetp >= 4)
						  offsetw = 0;
						if (offsetp && offsetp < 4)
						  offsetw = -10;
					}
					if(cSWidth && (pOffW-(offsetp+2)) > cOffW){
						if (!(isNS6 && (cOffW >= (pOffW - 20) && cOffW < pOffW) &&  cScrH > cOffH)){
						   	curMod.style.width=pOffW+offsetw;
						}
					}
					
				}
			}
		}
	}
}
// -->