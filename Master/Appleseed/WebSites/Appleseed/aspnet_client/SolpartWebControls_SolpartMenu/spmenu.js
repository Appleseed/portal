//------------------------------------------------------//
// Solution Partner's ASP.NET Hierarchical Menu Control //
// Copyright (c) 2002-2003                              //
// Jon Henning - Solution Partner's Inc                 //  
// jhenning@solpart.com   -   http://www.solpart.com    //
// Compatible Menu Version: 1.1.0.0+										//
// Script Version: 1101																  //
//------------------------------------------------------//

var m_oSolpartMenu = new Array(); //stores all menu objects (SolpartMenu) in array 
function spm_initMyMenu(oXML, oCtl)   //Creates SolpartMenu object and calls generate method
{
  m_oSolpartMenu[oCtl.id] = new SolpartMenu(oCtl);
  m_oSolpartMenu[oCtl.id].GenerateMenuHTML(oXML);
}
  
//------- Constructor -------//
function SolpartMenu(o)
{
__db(o.id + ' - constructor');
//  var me = this;  //allow attached events to reference this
  //--- Data Properties ---//
  this.systemImagesPath = spm_getAttr(o, 'SystemImagesPath', '');  
  this.iconImagesPath = spm_getAttr(o, 'IconImagesPath', this.systemImagesPath);
  this.xml = spm_getAttr(o, 'XML', '');
  this.xmlFileName = spm_getAttr(o, 'XMLFileName', '');

  //--- Appearance Properties ---//
  this.fontStyle=spm_getAttr(o, 'FontStyle', 'font-family: arial;');
  this.backColor=spm_getAttr(o, 'BackColor');  
  this.foreColor=spm_getAttr(o, 'ForeColor');
  this.iconBackColor=spm_getAttr(o, 'IconBackgroundColor');
  this.hlColor=spm_getAttr(o, 'HlColor', '');
  this.shColor=spm_getAttr(o, 'ShColor', ''); 
  this.selColor=spm_getAttr(o, 'SelColor');
  this.selForeColor=spm_getAttr(o, 'SelForeColor');
  this.selBorderColor=spm_getAttr(o, 'SelBorderColor');
  this.menuAlignment = spm_getAttr(o, 'MenuAlignment', 'Left');
  this.display=spm_getAttr(o, 'Display', 'horizontal');
  this.MBLeftHTML=spm_getAttr(o, 'MBLHTML', '');
  this.MBRightHTML=spm_getAttr(o, 'MBRHTML', '');

  this.rootArrow = spm_getAttr(o, 'RootArrow', '0');
  this.rootArrowImage = spm_getAttr(o, 'RootArrowImage', '');
  this.arrowImage = spm_getAttr(o, 'ArrowImage', '');
  this.backImage=spm_getAttr(o, 'BackImage', '');

  //--- Transition Properteis ---//
  //this.menuEffectsStyle=spm_getAttr(o, 'MenuEffectsStyle', '');
  this.menuTransitionLength=spm_getAttr(o, 'MenuTransitionLength', .3);
  this.menuTransition=spm_getAttr(o, 'MenuTransition', 'None');
  this.menuTransitionStyle=spm_getAttr(o, 'MenuTransitionStyle', '');
  this.SolpartMenuTransitionObject = new SolpartMenuTransitionObject();
  
  //--- Behavior Properteis ---//
  this.moveable = spm_getAttr(o, 'Moveable', '0');
  this.moDisplay=spm_getAttr(o, 'MODisplay', 'HighLight');
  this.moExpand=spm_getAttr(o, 'MOExpand', "-1");
  this.moutDelay=spm_getAttr(o, 'MOutDelay', "0");

  //--- Sizing Properties ---//
  this.menuBarHeight=spm_fixUnit(spm_getAttr(o, 'MenuBarHeight', '0'));
  this.menuItemHeight=spm_fixUnit(spm_getAttr(o, 'MenuItemHeight', '0'));
  this.iconWidth=spm_fixUnit(spm_getAttr(o, 'IconWidth', '0'));
  this.borderWidth=spm_getAttr(o, 'BorderWidth', '1');

  //--- CSS Properties ---//
  this.cssMenuContainer=spm_getAttr(o, 'CSSMenuContainer', '');
  this.cssMenuBar=spm_getAttr(o, 'CSSMenuBar', '');
  this.cssMenuItem=spm_getAttr(o, 'CSSMenuItem', '');
  this.cssMenuIcon=spm_getAttr(o, 'CSSMenuIcon', '');
  this.cssSubMenu=spm_getAttr(o, 'CSSSubMenu', '');
  this.cssMenuBreak=spm_getAttr(o, 'CSSMenuBreak', '');
  this.cssMenuItemSel=spm_getAttr(o, 'CSSMenuItemSel', '');
  this.cssMenuArrow=spm_getAttr(o, 'CSSMenuArrow', '');
  this.cssMenuRootArrow=spm_getAttr(o, 'CSSRootMenuArrow', '');
  
  //---- methods ---//
  //this.GenerateMenuHTML=__GenerateMenuHTML;

  //----- private ----//
  this._m_sNSpace = o.id;               //stores namespace for menu
  this._m_sOuterTables = '';            //stores HTML for sub menus
  this._m_oDOM;                         //stores XML DOM object
	this._m_oMenu = o;                    //stores container
  this._m_oMenuMove;                    //stores control that is used for moving menu
  
  this._m_oTblMenuBar;                  //stores menu container
	this._m_aOpenMenuID = new Array();	  //stores list of menus that are currently displayed
	this._m_bMoving=false;                //flag to determine menu is being dragged
  this._m_dHideTimer = null;            //used to time when mouse out occured to auto hide menu based on mouseoutdelay

	//--- Exposed Events ---//
/*
	this.onMenuComplete=spm_getAttr(o, 'OnMenuComplete', null);						//fires once menu is done loading
	this.onMenuBarClick=spm_getAttr(o, 'OnMenuBarClick', null);						//fires once menu bar is clicked
	this.onMenuItemClick=spm_getAttr(o, 'OnMenuItemClick', null);         //fires once menu item is clicked
	this.onMenuBarMouseOver=spm_getAttr(o, 'OnMenuBarMouseOver', null);		//fires once mouse moves over menu bar
	this.onMenuBarMouseOut=spm_getAttr(o, 'OnMenuBarMouseOut', null);			//fires once mouse moves out of menu bar
	this.onMenuItemMouseOver=spm_getAttr(o, 'OnMenuItemMouseOver', null);	//fires once mouse moves over menu item
	this.onMenuItemMouseOut=spm_getAttr(o, 'OnMenuItemMouseOut', null);		//fires once mouse moves out of menu bar
*/

//--- Menu Moving currently disabled ---//
/*
  this._menuhook_MouseMove=__menuhook_MouseMove;
  this._menuhook_MouseDown=__menuhook_MouseDown;
  this._menuhook_MouseUp=__menuhook_MouseUp;
  this._document_MouseMove=__document_MouseMove;
  this._document_MouseDown=__document_MouseDown;
  this._document_MouseUp=__document_MouseUp;
  this._bodyclick=__bodyclick;

  this.menuhook_MouseMove=function(e) {me._menuhook_MouseMove(e);};
  this.menuhook_MouseDown=function(e) {me._menuhook_MouseDown(e);};
  this.menuhook_MouseUp=function(e) {me._menuhook_MouseUp(e);};
  this.document_MouseMove=function(e) {me._document_MouseMove(e);};
  this.document_MouseDown=function(e) {me._document_MouseDown(e);};
  this.menuhook_MouseUp=function(e) {me._menuhook_MouseUp(e);};
  this.bodyclick=function() {me._bodyclick();};
*/ 
__db(this._m_oMenu.id + ' - constructor end');
}

//--- Destroys interrnal object references ---//
SolpartMenu.prototype.destroy = function ()
{
  this.systemImagesPath = null;  
  this.iconImagesPath = null;
  this.xml = null;
  this.xmlFileName = null;

  //--- Appearance Properties ---//
  this.fontStyle = null;
  this.backColor = null;  
  this.foreColor = null;
  this.iconBackColor = null;
  this.hlColor = null;
  this.shColor = null; 
  this.selColor = null;
  this.selForeColor = null;
  this.selBorderColor = null;
  this.menuAlignment = null;
  this.display = null;

  this.rootArrow = null;
  this.rootArrowImage = null;
  this.arrowImage = null;
  this.backImage = null;

  //--- Transition Properteis ---//
  //this.menuEffectsStyle = null;
  this.menuTransitionLength = null;
  this.menuTransition = null;
  this.SolpartMenuTransitionObject = null;
  
  //--- Behavior Properteis ---//
  this.moveable = null;
  this.moDisplay = null;
  this.moExpand = null;
  this.moutDelay = null;

  //--- Sizing Properties ---//
  this.menuBarHeight = null;
  this.menuItemHeight = null;
  this.iconWidth = null;
  this.borderWidth = null;

  //--- CSS Properties ---//
  this.cssMenuContainer = null;
  this.cssMenuBar = null;
  this.cssMenuItem = null;
  this.cssMenuIcon = null;
  this.cssSubMenu = null;
  this.cssMenuBreak = null;
  this.cssMenuItemSel = null;
  this.cssMenuArrow = null;
  this.cssMenuRootArrow = null;
  
  //---- methods ---//
  //this.GenerateMenuHTML=__GenerateMenuHTML = null;

  //----- private ----//
  m_oSolpartMenu[this._m_sNSpace] = null;

  this._m_sNSpace = null;                 //stores namespace for menu
  this._m_sOuterTables = null;            //stores HTML for sub menus
  this._m_oDOM = null;                    //stores XML DOM object
	this._m_oMenu = null;                   //stores container
  this._m_oMenuMove = null;               //stores control that is used for moving menu
  
  this._m_oTblMenuBar = null;             //stores menu container
	this._m_aOpenMenuID = null;	            //stores list of menus that are currently displayed
	this._m_bMoving = null;                 //flag to determine menu is being dragged
  this._m_dHideTimer = null;              //used to time when mouse out occured to auto hide menu based on mouseoutdelay
}

//--- static/shared members ---//
/*
SolpartMenu.prototype.menuhook_MouseMove=__menuhook_MouseMove;
SolpartMenu.prototype.menuhook_MouseDown=__menuhook_MouseDown;
SolpartMenu.prototype.menuhook_MouseUp=__menuhook_MouseUp;

SolpartMenu.prototype.document_MouseMove=__document_MouseMove;
SolpartMenu.prototype.document_MouseDown=__document_MouseDown;
SolpartMenu.prototype.document_MouseUp=__document_MouseUp;
*/

//--- xml document loaded (non-dataisland) ---//
SolpartMenu.prototype.onXMLLoad = function ()
{
  this.GenerateMenuHTML(this._m_oDOM);
}

//--- Generates menu HTML through passed in XML DOM ---//
SolpartMenu.prototype.GenerateMenuHTML = function (oXML) 
{
__db(this._m_oMenu.id + ' - GenerateMenuHTML');
    //'Generates the main menu bar
  var sHTML = '';
  this._m_sOuterTables = '';
  //this._m_oMenu.insertAdjacentElement('beforeBegin', );

  
	//if (oXML.readyState != 'complete')
	//	return;

	if (oXML == null)
	{
	  if (this._m_oDOM == null)
	  {
	    oXML = spm_createDOMDoc();//document.implementation.createDocument("", "", null);
	    this._m_oDOM = oXML;
        	  
	    if (this.xml.length)
	      oXML.loadXML(this.xml);
  	  
	    if (this.xmlFileName.length)
	    {
	      //alert(m_oSolpartMenu.length);
	      oXML.onload = eval('onxmlload' + this._m_sNSpace); //'m_oSolpartMenu["' + this._m_sNSpace + '"].onXMLLoad'; this.onXMLLoad;
	      oXML.load(this.xmlFileName);
	      return; //async load
	    }
    }
	}
	else
	  this._m_oDOM = oXML;
  if (this.display == "vertical")
  {
      sHTML += '<table ID="tbl' + this._m_sNSpace + 'MenuBar" CELLPADDING=\'0\' CELLSPACING=\'0\' BORDER="0" CLASS="' + spm_fixCSSForMac(this.getIntCSSName('spmbctr') + this.cssMenuContainer) + '" HEIGHT="100%" STYLE="position: relative; vertical-align: center; display: block;">\n';
      sHTML += MyIIf(this.MBLeftHTML.length, '<tr>\n       <td>' + this.MBLeftHTML + '</td>\n</tr>\n', '');
      sHTML += MyIIf(Number(this.moveable), '<tr>\n       <td ID="td' + this._m_sNSpace + 'MenuMove" height=\'3px\' style=\'cursor: move; ' + spm_getMenuBorderStyle(this) + '\'>' + spm_getSpacer(this) + '</td>\n</tr>\n', '');
      sHTML +=         this.GetMenuItems(this._m_oDOM.documentElement);
      sHTML += '       <tr><td HEIGHT="100%">' + spm_getSpacer(this) + '</td>\n' ;
      sHTML += '   </tr>\n';
      sHTML += MyIIf(this.MBRightHTML.length, '<tr>\n       <td>' + this.MBRightHTML + '</td>\n</tr>\n', '');
      sHTML += '</table>\n';
  }
  else
  {
      sHTML += '<table ID="tbl' + this._m_sNSpace + 'MenuBar" CELLPADDING=\'0\' CELLSPACING=\'0\' BORDER="0" CLASS="' + spm_fixCSSForMac(this.getIntCSSName('spmbctr') + this.cssMenuContainer) + '" WIDTH="100%" STYLE="position: relative; vertical-align: center; display: block;">\n';
      sHTML += '	<tr>\n';
      sHTML += MyIIf(this.MBLeftHTML.length, '<td>' + this.MBLeftHTML + '</td>\n', '');
      sHTML += MyIIf(Number(this.moveable), '       <td ID="td' + this._m_sNSpace + 'MenuMove" width=\'3px\' style=\'cursor: move; ' + spm_getMenuBorderStyle(this) + '\'>' + spm_getSpacer(this) + '</td>\n', '');
      sHTML += spm_getMenuSpacingImage('left', this);
      sHTML +=         this.GetMenuItems(this._m_oDOM.documentElement);
      sHTML += spm_getMenuSpacingImage('right', this);
      sHTML += MyIIf(this.MBRightHTML.length, '<td>' + this.MBRightHTML + '</td>\n', '');
      sHTML += '   </tr>\n';
      sHTML += '</table>\n';
  }
  
	if (isOpera())
	{
		this._m_oMenu.innerHTML = sHTML;
		var oDiv = document.createElement('div');
		oDiv.innerHTML = this._m_sOuterTables;
		document.body.appendChild(oDiv);
	}
	else  
		sHTML = '<SPAN>' + this._m_sOuterTables + '</SPAN>' + sHTML;
  //sHTML = "<table><tr><td>THIS IS A TEST</td></tr></table>";

  this._m_oMenu.innerHTML = sHTML;
  //spm_getById('txtDebug').value = sHTML;
  //if (spm_browserType() != 'ie')
  //  this._m_oMenu.innerHTML = sHTML;  //Mozilla/NS issue with events not firing... why?
    
  //return '';
  this._m_oMenuMove = spm_getById('td' + this._m_sNSpace + 'MenuMove')

  //if (spm_browserType() == 'ie' && isMac() == false)
  //  window.attachEvent("onunload", this.destroy);
  //else
  //  window.addEventListener("onunload", this.destroy, true);

/*
this._m_oMenu.insertAdjacentHTML("afterend", "<TEXTAREA TYPE='txt' id='txtDebug' rows=50 cols=100></TEXTAREA>");
document.all('txtDebug').innerText = this._m_oMenu.outerHTML;
*/
/*
  //--- attach events for menu moving ---//
  if (Number(this.moveable))
  {
    var oCtl = this._m_oMenuMove;  //this._m_oMenu
    oCtl.onmousedown = this.menuhook_MouseDown;
    oCtl.onmouseup = this.menuhook_MouseUp;
    oCtl.onmousemove = this.menuhook_MouseMove;

    if (spm_browserType() == 'ie')
    {
      document.onmousemove = this.document_MouseMove;
      document.onmousedown = this.document_MouseDown;
      //spm_getTags("BODY")[0].onclick = this.bodyclick;
      spm_getTags("BODY")[0].attachEvent('onclick', this.bodyclick);
    }
    else
    {
	    window.addEventListener("click", this.bodyclick, true);
	    window.addEventListener("mousemove", this.document_MouseMove, true);
	    window.addEventListener("mousedown", this.document_MouseDown, true);
	    window.addEventListener("mouseup", this.document_MouseUp, true);
    }

  }
*/
  //if (spm_browserType() == 'ie')
		spm_getTags("BODY")[0].onclick = spm_appendFunction(spm_getTags("BODY")[0].onclick, 'm_oSolpartMenu["' + this._m_sNSpace + '"].bodyclick();'); //document.body.onclick = this.bodyclick;
	//else
	//	window.addEventListener("click", this.bodyclick, true);

  this._m_oTblMenuBar = spm_getById('tbl' + this._m_sNSpace + 'MenuBar'); //this._m_oMenu
  
  this.fireEvent('onMenuComplete');

__db(this._m_oMenu.id + ' - GenerateMenuHTML end');    
}

function spm_getMenuBarEvents(sCtl)
{
  return 'onmouseover="m_oSolpartMenu[\'' + sCtl + '\'].onMBMO(this);" onmouseout="m_oSolpartMenu[\'' + sCtl + '\'].onMBMOUT(this);" onclick="m_oSolpartMenu[\'' + sCtl + '\'].onMBC(this, event);" onmousedown="m_oSolpartMenu[\'' + sCtl + '\'].onMBMD(this);" onmouseup="m_oSolpartMenu[\'' + sCtl + '\'].onMBMU(this);"';
}

function spm_getMenuItemEvents(sCtl)
{
  return 'onmouseover="m_oSolpartMenu[\'' + sCtl + '\'].onMBIMO(this);" onmouseout="m_oSolpartMenu[\'' + sCtl + '\'].onMBIMOUT(this);" onclick="m_oSolpartMenu[\'' + sCtl + '\'].onMBIC(this, event);"';
}

//--- Returns HTML for menu items (recursive function) ---//
SolpartMenu.prototype.GetMenuItems = function (oParent)
{
//return '';
  var oNode;
  var sHTML = '';
  var sID;
  var sParentID;
  var sClickAction;
  
	for (var i = 0; i < oParent.childNodes.length; i++)
	{
		oNode = oParent.childNodes[i];

		if (oNode.nodeType != 3)  //exclude nodeType of Text (Netscape/Mozilla) issue!
		{
		  //'determine if root level item and set parent id accordingly
		  if (oNode.parentNode.nodeName != "menuitem")
			  sParentID = "-1";
		  else
			  sParentID = oNode.parentNode.getAttribute("id");

		  if (oNode.nodeName == "menuitem")
			  sID = oNode.getAttribute("id");
		  else
			  sID = "";

  __db(sID + ' getmenuitems');

			sClickAction = spm_getMenuClickAction(oNode);

		  if (sParentID == "-1")	//'if top level menu item
		  {
			  if (this.display == "vertical")
				  sHTML += "<tr>\n"; //'if vertical display then add rows for each top menuitem
  				
			  sHTML += '<td>\n<table width="100%" CELLPADDING="0" CELLSPACING="0">\n<tr id="td' + this._m_sNSpace + sID + '" ' + spm_getMenuBarEvents(this._m_sNSpace) + '  class="' + spm_fixCSSForMac(this.getIntCSSName('spmbar spmitm') + this.cssMenuBar + ' ' + this.cssMenuItem + ' ' + spm_getMenuItemCSS(oNode)) + '" savecss="' + spm_getMenuItemCSS(oNode) + '" menuclick="' + sClickAction + '" style="' + spm_getMenuItemStyle('item', oNode) + '">\n<td NOWRAP="NOWRAP">' + spm_getImage(oNode, this) + spm_getItemHTML(oNode, 'left', '&nbsp;') + oNode.getAttribute('title') + spm_getItemHTML(oNode, 'right') + MyIIf(Number(this.rootArrow) && spm_nodeHasChildren(oNode), '</td>\n<td align="right" class="' + spm_fixCSSForMac(this.getIntCSSName('spmrarw') + this.cssMenuRootArrow) + '">' + spm_getArrow(this.rootArrowImage, this) + "", '&nbsp;') + '\n</td>\n</tr>\n</table>\n</td>\n';
  	    
	      //this._m_aMenuBarItems[this._m_aMenuBarItems.length] = 'td' + this._m_sNSpace + sID;
  	    
			  if (this.display == "vertical")
				  sHTML += "</tr>\n";
		  
		  }
		  else                        //'submenu - not top level menu item
		  {
			  switch(oNode.nodeName)
			  {
				  case "menuitem":
				  {
					  sHTML +=		'   <tr ID="tr' + this._m_sNSpace + sID + '" ' + spm_getMenuItemEvents(this._m_sNSpace) + ' parentID="' + sParentID + '" class="' + spm_fixCSSForMac(this.getIntCSSName('spmitm') + this.cssMenuItem + ' ' + spm_getMenuItemCSS(oNode)) + '" savecss="' + spm_getMenuItemCSS(oNode) + '" menuclick="' + sClickAction + '" style="' + spm_getMenuItemStyle('item', oNode) + '">\n';
					  sHTML +=		'       <td id="icon' + this._m_sNSpace + sID + '" class="' + spm_fixCSSForMac(this.getIntCSSName('spmicn') + this.cssMenuIcon) + '" style="' + spm_getMenuItemStyle('image', oNode) + '">' + spm_getImage(oNode, this) + '</td>\n';
					  sHTML +=		'       <td id="td' + this._m_sNSpace + sID + '" class="' + spm_fixCSSForMac(this.getIntCSSName('spmitm') + this.cssMenuItem + ' ' + spm_getMenuItemCSS(oNode)) + '" savecss="' + spm_getMenuItemCSS(oNode) + '" NOWRAP="NOWRAP" >' + oNode.getAttribute('title') + '</td>\n';
					  sHTML +=		'       <td id="arrow' + this._m_sNSpace + sID + '" width="15px" CLASS="' + spm_fixCSSForMac(this.getIntCSSName('spmarw') + this.cssMenuArrow) + '">' + MyIIf(spm_nodeHasChildren(oNode), spm_getArrow(this.arrowImage, this), spm_getSpacer(this)) + '</td>\n';
					  sHTML +=		'   </tr>\n';

    	      //this._m_aMenuItems[this._m_aMenuItems.length] = 'tr' + this._m_sNSpace + sID;

					  break;
				  }
				  case "menubreak":
				  {
					  //if (this._m_oMenu.IconBackgroundColor == this.backColor)
						//  sHTML += '   <tr><td NOWRAP height="0px" colspan="3" class="spmbrk ' + this.cssMenuBreak + '">' + spm_getMenuImage('spacer.gif', this, true) + '</td></tr>';
					  //else
						  sHTML += '   <tr>\n<td style="height: 1px" class="' + spm_fixCSSForMac(this.getIntCSSName('spmicn') + this.cssMenuIcon) + '">' + spm_getMenuImage('spacer.gif', this, true) + '</td>\n<td colspan="2" class="' + spm_fixCSSForMac(this.getIntCSSName('spmbrk') + this.cssMenuBreak) + '">' + spm_getMenuImage('spacer.gif', this, true) + '</td>\n</tr>\n';

					  break;
				  }
			  }
		  }

		  //'Generate sub menu - note: we are recursively calling ourself
		  //'netscape renders tables with display: block as having cellpadding!!! therefore using div outside table - LAME!
		  if (oNode.childNodes.length > 0)
			  this._m_sOuterTables = '\n<DIV ID="tbl' + this._m_sNSpace + sID + '" CLASS="' + spm_fixCSSForMac(this.getIntCSSName('spmsub') + this.cssSubMenu) + '" STYLE="display:none; position: absolute;' + this.menuTransitionStyle + '">\n<table CELLPADDING="0" CELLSPACING="0">\n' + this.GetMenuItems(oNode) + '\n</table>\n</DIV>\n' + this._m_sOuterTables;

    }
	}
	return sHTML;
}

	//--------------- Event Functions ---------------//
  //--- menubar click event ---//
	SolpartMenu.prototype.onMBC = function (e, evt)
	{
		var oCell = e; //event.srcElement;
		var sID = oCell.id.substr(2);

		var oMenu = spm_getById("tbl" + sID);
    //var oMenu = spm_getById("td" + sID);

		if (oMenu != null)
		{
			if (oMenu.style.display == '')
			{
				this.hideAllMenus();		
				spm_showElement("SELECT");
			}
			else
			{
				spm_positionMenu(this, oMenu, oCell);
				
				this.doTransition(oMenu);
				oMenu.style.display = "";
				this._m_aOpenMenuID[0] = sID;
				spm_hideElement("SELECT",oMenu);
			}
		}
		
    this.fireEvent('onMenuBarClick', oCell);
    
    oMenu = spm_getById("td" + sID);
    if (spm_getAttr(oMenu, "menuclick", '').length)
    {
      eval(spm_getAttr(oMenu, "menuclick", ''));
      this.hideAllMenus();
    }
		spm_stopEventBubbling(evt);
	}
	
  //--- menubar mousedown event ---//
	SolpartMenu.prototype.onMBMD = function (e)
	{
		var oCell = e; //event.srcElement;
		this.applyBorder(oCell, 1, this.shColor, this.hlColor);
	}
  
  //--- menubar mouseup event ---//
	SolpartMenu.prototype.onMBMU = function (e)
	{
		var oCell = e; //event.srcElement;
		this.applyBorder(oCell, 1, this.hlColor, this.shColor);
	}
  
  //--- menubar mouseover event ---//
	SolpartMenu.prototype.onMBMO = function (e)
	{
		var oCell = e; //event.srcElement;
		
		if (oCell.id.length == 0) //cancelBubble
		  return;
		var sID = oCell.id.substr(2);
		var oMenu = spm_getById("tbl" + sID);

		if (this._m_aOpenMenuID.length || this.moExpand != '0')
		{
			//--- if menu is shown then mouseover triggers the showing of all menus ---//
			this.hideAllMenus();

			if (oMenu != null)
			{
				spm_positionMenu(this, oMenu, oCell);
				this.doTransition(oMenu);
				oMenu.style.display = "";
				this._m_aOpenMenuID[0] = sID;
				spm_hideElement("SELECT",oMenu);
			}
			this.applyBorder(oCell, 1, this.shColor, this.hlColor);
		}
		else
		{
			this.applyBorder(oCell, 1, this.hlColor, this.shColor);
		}
		oCell.className =  spm_fixCSSForMac(this.getIntCSSName('spmitmsel spmbar') + this.cssMenuBar + ' ' + this.cssMenuItemSel);
		
		this._m_dHideTimer = null;
		
		this.fireEvent('onMenuBarMouseOver', oCell);
		
	}
  //--- menubar mouseout event ---//
	SolpartMenu.prototype.onMBMOUT = function (e)
	{
		var oCell = e; //event.srcElement;
		var sID = oCell.id.substr(2);
		this.applyBorder(oCell, 1, spm_getCellBackColor(oCell), spm_getCellBackColor(oCell), "none");	
		this._m_dHideTimer = new Date();
		//setTimeout(this.hideMenuTime, this.moutDelay);
		if (this.moutDelay != 0)
		  setTimeout('m_oSolpartMenu["' + this._m_sNSpace + '"].hideMenuTime()', this.moutDelay);
		  
    oCell.className = spm_fixCSSForMac(this.getIntCSSName('spmbar spmitm') + this.cssMenuBar + ' ' + this.cssMenuItem + spm_getAttr(e, 'savecss', ''));
    this.stopTransition();
    
    this.fireEvent('onMenuBarMouseOut', oCell);
	}
	
  //--- menuitem click ---//
	SolpartMenu.prototype.onMBIC = function (e, evt)
	{
		var oRow = spm_getSourceTR(e, this._m_sNSpace);  //event.srcElement
		var sID = oRow.id.substr(2);
		if (spm_itemHasChildren(sID, this._m_sNSpace) == false)
			this.hideAllMenus();

		this.fireEvent('onMenuItemClick', oRow);

    if (spm_getAttr(oRow, "menuclick", '').length)
    {
      eval(spm_getAttr(oRow, "menuclick", ''));
      this.hideAllMenus();
		}
		//window.event.cancelBubble = true;
		spm_stopEventBubbling(evt);
	}

  //--- menuitem mouseover event ---//
	SolpartMenu.prototype.onMBIMO = function (e)
	{		
		this.handlembi_mo(spm_getSourceTR(e, this._m_sNSpace)); //event.srcElement
		this._m_dHideTimer = null;
	}
  //--- menuitem mouseout event ---//
	SolpartMenu.prototype.onMBIMOUT = function (e)
	{	
		this.handlembi_mout(spm_getSourceTR(e, this._m_sNSpace));  //event.srcElement
		this._m_dHideTimer = new Date;
		//setTimeout(this.hideMenuTime, this.moutDelay);
		if (this.moutDelay != 0)
		  setTimeout('m_oSolpartMenu["' + this._m_sNSpace + '"].hideMenuTime()', this.moutDelay);
	}
	
/*
	function menuhook_KeyPress()
	{
    //not yet
	}
	function menuhook_KeyDown()
	{
    //not yet
	}
	
	function menuhook_MenuFocus()
	{
		var tbl = event.srcElement;
		mb_c(tbl.rows[0].cells[0]);
	}
*/
/*	
	function __menuhook_MouseMove(e) 
	{
		var iNewLeft=0, iNewTop = 0

if (this._m_bMoving)
{
			if (spm_browserType() == 'ie')
			{
//		if ((event.button==1)) 
//		{
			  this.hideAllMenus();
			  if (this._m_oTblMenuBar.startLeft == null)
				  this._m_oTblMenuBar.startLeft = this._m_oTblMenuBar.offsetLeft;
			  iNewLeft=event.clientX - this._m_oTblMenuBar.startLeft - 3;
			  this._m_oTblMenuBar.style.pixelLeft= iNewLeft;
			  if (this._m_oTblMenuBar.startTop == null)
				  this._m_oTblMenuBar.startTop = this._m_oTblMenuBar.offsetTop;
			  iNewTop=event.clientY - this._m_oTblMenuBar.startTop;
			  this._m_oTblMenuBar.style.pixelTop = iNewTop - 10;
			  event.returnValue = false
			  event.cancelBubble = true
//      }
		}
    else
    {
			this.hideAllMenus();
  		
			if (this._m_oTblMenuBar.startLeft == null)
				this._m_oTblMenuBar.startLeft = this._m_oTblMenuBar.offsetLeft;

			iNewLeft=e.clientX - this._m_oTblMenuBar.startLeft - 3;
  		    
			//if (iNewLeft&lt;0) 
			//	iNewLeft=0;
  		
			this._m_oTblMenuBar.style.left = iNewLeft;
  					    
			if (this._m_oTblMenuBar.startTop == null)
				this._m_oTblMenuBar.startTop = this._m_oTblMenuBar.offsetTop;

			iNewTop=e.clientY - this._m_oTblMenuBar.startTop;
			//if (iNewTop&lt;0) 
			//	iNewTop=0;
  			
			this._m_oTblMenuBar.style.top = iNewTop - 10;    
    }
}

	}
	function __menuhook_MouseDown()
	{
		this._m_bMoving = true;
	}
	function __menuhook_MouseUp()
	{
	  this._m_bMoving = false;
	}
	function __document_MouseMove(e)
	{
		if (this._m_bMoving)
		{
			this.menuhook_MouseMove(e);
	  }
	}
	function __document_MouseDown()
	{
		//this._m_bMoving = null;
	}
	function __document_MouseUp()
	{
		this._m_bMoving=false;
	}
*/

	SolpartMenu.prototype.bodyclick = function()
	{
		this.hideAllMenus();
	}

  //--- handles display of newly opened menu ---//
	SolpartMenu.prototype.handleNewItemSelect = function (sID)
	{
		var i;
		var iNewLength=-1;
		var bDeleteRest=false; 
		for (i=0; i<this._m_aOpenMenuID.length; i++)
		{		
			if (bDeleteRest)
				spm_getById("tbl" + this._m_aOpenMenuID[i]).style.display = "none";
			if (this._m_aOpenMenuID[i] == this._m_sNSpace + sID)
			{
				bDeleteRest=true;
				iNewLength = i;
			}				
		}
		if (iNewLength != -1)
			this._m_aOpenMenuID.length = iNewLength+1;
	}
	
  //--- hides all menus that are currently displayed ---//
	SolpartMenu.prototype.hideAllMenus = function ()
	{
		var i;
		for (i=0; i<this._m_aOpenMenuID.length; i++)
		{		
			spm_getById("tbl" + this._m_aOpenMenuID[i]).style.display = "none";
		}
		this._m_aOpenMenuID.length = 0;
		spm_showElement("SELECT");
	}		
  
  
  function SolpartMenuTransitionObject()
  {
    this.id=null;
    this.stop = false;
  } 

  //--- stops menu transition effect ---//
  SolpartMenu.prototype.stopTransition = function ()
  {
    this.SolpartMenuTransitionObject.stop = true;
    this.doFilter();
    this.SolpartMenuTransitionObject = new SolpartMenuTransitionObject();
  }
  
  //--- starts menu transition effect ---//
  SolpartMenu.prototype.doTransition = function (oMenu)
  {
    if (this.menuTransition == 'None' || spm_browserType() != 'ie')
      return;

    var sID = this.SolpartMenuTransitionObject.id;
    
    switch (this.menuTransition)
    {
      case 'AlphaFade':
      {
        if (this.SolpartMenuTransitionObject.id != oMenu.id) 
        {
          this.SolpartMenuTransitionObject.id = oMenu.id;
          this.SolpartMenuTransitionObject.opacity = 0;
          this.doFilter();
        }
        break;
      }
      case 'Wave':
      {
        if (this.SolpartMenuTransitionObject.id != oMenu.id) 
        {        
          this.SolpartMenuTransitionObject.id = oMenu.id;
          this.SolpartMenuTransitionObject.phase = 0;
          this.doFilter();
        }
        break;
      }
      case 'ConstantWave':
      {
        if (sID != oMenu.id) 
        {        
          this.SolpartMenuTransitionObject.id = oMenu.id;
          this.SolpartMenuTransitionObject.phase = 0;
          this.SolpartMenuTransitionObject.constant=true;
          this.doFilter();
        }
        break;
      }
      case 'Inset': case 'RadialWipe': case 'Slide': case 'Spiral': case 'Stretch': case 'Strips': case 'Wheel': case 'GradientWipe': case 'Zigzag': case 'Barn': case 'Blinds': case 'Checkerboard': case 'Fade': case 'Iris': case 'RandomBars':
      {
        oMenu.filters('DXImageTransform.Microsoft.' + this.menuTransition).apply();
        oMenu.filters('DXImageTransform.Microsoft.' + this.menuTransition).duration = this.menuTransitionLength;
        oMenu.filters('DXImageTransform.Microsoft.' + this.menuTransition).play();
        break;
      }
    }
  }

  //--- applys transition filter ---//
  SolpartMenu.prototype.doFilter = function (bStop) 
  {      
    if (this.SolpartMenuTransitionObject.id == null)
      return;
      
    var o = spm_getById(this.SolpartMenuTransitionObject.id);
    window.status = new Date();
    switch (this.menuTransition)
    {
      case 'AlphaFade':
      {
        if (this.SolpartMenuTransitionObject.stop)
        {
          o.filters('DXImageTransform.Microsoft.Alpha').opacity = 100;
        }
        else
        {
          o.filters('DXImageTransform.Microsoft.Alpha').opacity = this.SolpartMenuTransitionObject.opacity;
          if (this.SolpartMenuTransitionObject.opacity < 100)
          {
            setTimeout('m_oSolpartMenu["' + this._m_sNSpace + '"].doFilter()', 50);
            this.SolpartMenuTransitionObject.opacity += (100/20* this.menuTransitionLength);
          }
        }
        break;
      }
      case 'Wave': case 'ConstantWave':
      {
        if (this.SolpartMenuTransitionObject.stop)
        {
            o.filters("DXImageTransform.Microsoft.Wave").freq = 0;
            o.filters("DXImageTransform.Microsoft.Wave").lightstrength = 0;
            o.filters("DXImageTransform.Microsoft.Wave").strength = 0;
            o.filters("DXImageTransform.Microsoft.Wave").phase = 0;
        }
        else
        {
          o.filters("DXImageTransform.Microsoft.Wave").freq = 1;
          o.filters("DXImageTransform.Microsoft.Wave").lightstrength = 20;
          o.filters("DXImageTransform.Microsoft.Wave").strength = 5;
          o.filters("DXImageTransform.Microsoft.Wave").phase = this.SolpartMenuTransitionObject.phase;

          if (this.SolpartMenuTransitionObject.phase < 100 * this.menuTransitionLength || this.SolpartMenuTransitionObject.constant == true)
          {
            setTimeout('m_oSolpartMenu["' + this._m_sNSpace + '"].doFilter()', 50);
            this.SolpartMenuTransitionObject.phase += 5;
          }
          else
          {
            o.filters("DXImageTransform.Microsoft.Wave").freq = 0;
            o.filters("DXImageTransform.Microsoft.Wave").lightstrength = 0;
            o.filters("DXImageTransform.Microsoft.Wave").strength = 0;
            o.filters("DXImageTransform.Microsoft.Wave").phase = 0;
          }
        }
        break;
      }
    }
  }          
  



  //--- handles mouseover for menu item ---//
	SolpartMenu.prototype.handlembi_mo = function (oRow)
	{
		var sID = oRow.id.substr(2);
		//oRow.className = 'spmitmsel';
		spm_getById("icon" + sID).className = spm_fixCSSForMac(this.getIntCSSName('spmitmsel spmicn') + this.cssMenuIcon + ' ' + this.cssMenuItemSel);
		spm_getById("td" + sID).className = spm_fixCSSForMac(this.getIntCSSName('spmitmsel') + this.cssMenuItemSel);
		spm_getById("arrow" + sID).className = spm_fixCSSForMac(this.getIntCSSName('spmitmsel spmarw') + this.cssMenuItemSel + ' ' + this.cssMenuArrow);
		
		//oRow.style.backgroundColor = this.selColor;
		//oRow.style.color = this.selForeColor;
		//oRow.style.color = this.selForeColor;
    //setClassColor(oRow, 'spmitm', this.selForeColor);
    
		//spm_getById("icon" + sID).style.backgroundColor = this.selColor;
		spm_applyRowBorder(oRow, 1, this.selBorderColor, true);

		//if (this._m_aOpenMenuID.join(',').indexOf(oRow.id.replace('tr', '')) == -1)
		if (this._m_aOpenMenuID[this._m_aOpenMenuID.length - 1] != oRow.id.replace('tr', ''))
		{
			this.handleNewItemSelect(spm_getAttr(oRow, "parentID", ""));
		
			if (spm_getById("tbl" + sID) != null)
			{
				oMenu = spm_getById("tbl" + sID);
        //oMenu.style.position = "absolute";  //CSS FIX!				
				oMenu.style.left = spm_elementLeft(oRow) + oRow.offsetWidth;
				oMenu.style.top = spm_elementTop(oRow);
				
				this.doTransition(oMenu);

				oMenu.style.display = "";
				if (spm_elementLeft(oRow) + oRow.offsetWidth + oMenu.offsetWidth > document.body.clientWidth)
				{
					oMenu.style.left = spm_elementLeft(oRow) - oMenu.offsetWidth;
					//oMenu.style.top = spm_elementTop(oRow);					
				}

			  if (spm_elementTop(oMenu) + oMenu.offsetHeight > document.body.clientHeight)
			  {
				  oMenu.style.top = document.body.clientHeight - oMenu.offsetHeight;
			  }
				this._m_aOpenMenuID[this._m_aOpenMenuID.length] = sID;
				spm_hideElement("SELECT",oMenu);
			}	
		}
		this.fireEvent('onMenuItemMouseOver', oRow);
		
	}
	
  //--- handles mouseout for menu item ---//
	SolpartMenu.prototype.handlembi_mout = function (oRow)
	{
			var sID = oRow.id.substr(2);
			//oRow.style.backgroundColor = this.backColor;
			//oRow.style.color = this.foreColor;	
			//oRow.style.color = '';
			//setClassColor(oRow, 'spmitm', '');
			oRow.className = spm_fixCSSForMac(this.getIntCSSName('spmitm') + this.cssMenuItem + spm_getAttr(oRow, 'savecss', ''));
		  spm_getById("icon" + sID).className = spm_fixCSSForMac(this.getIntCSSName('spmicn') + this.cssMenuIcon);
		  spm_getById("td" + sID).className = spm_fixCSSForMac(this.getIntCSSName('spmitm') + this.cssMenuItem + spm_getAttr(oRow, 'savecss', ''));
		  spm_getById("arrow" + sID).className = spm_fixCSSForMac(this.getIntCSSName('spmarw') + this.cssMenuArrow);
			
			//spm_getById("icon" + sID).style.backgroundColor = this.iconBackColor;
			spm_applyRowBorder(oRow, 1, "", false);

      this.stopTransition();
	}

  //used for raising events to client javascript
  SolpartMenu.prototype.fireEvent = function (sEvent, src) 
  {
		return; //disabled for now
    if (eval('this.' + sEvent + ' != null'))
		{
			var e = new Object();
			if (src != null)
				e.srcElement = src;
			else
				e.srcElement = this._m_oMenu;
				
				eval('this.' + sEvent + '(e)');
		}
  }

//--- called by setTimeOut to check mouseout hide delay ---//
SolpartMenu.prototype.hideMenuTime = function ()
  {
    if (this._m_dHideTimer != null && this.moutDelay > 0)
    {
      if (new Date() - this._m_dHideTimer >= this.moutDelay)
      {
        this.hideAllMenus();
        this._m_dHideTimer = null;
      }
      else
        setTimeout(this.hideMenuTime, this.moutDelay);
    }
  }



/*
  function setClassColor(oCtl, sClass, sColor)
  {
    var o;
    for (var i=0; i<oCtl.childNodes.length; i++)
    {
      o = oCtl.childNodes[i];
      if (o.className == sClass)
        o.style.color = sColor;
      
      if (o.childNodes.length)
        setClassColor(o, sClass, sColor)
    }
  }
*/

//global

	function spm_showElement(elmID)
	{
		// Display any element that was hidden
		for (i = 0; i < spm_getTags(elmID).length; i++)
		{
			obj = spm_getTags(elmID)[i];
			if (! obj || ! obj.offsetParent)
				continue;
			obj.style.visibility = "";
		}
	}

	function spm_hideElement(elmID, eMenu)
	{
		var obj;
		// Hide any element that overlaps with the dropdown menu
		for (i = 0; i < spm_getTags(elmID).length; i++)
		{
			obj = spm_getTags(elmID)[i];
			if (spm_elementTop(obj) > parseInt(eMenu.style.top) + eMenu.offsetHeight)
			{
				//if element is below bottom of menu then do nothing
			}
			else if (spm_elementLeft(obj) > parseInt(eMenu.style.left) + eMenu.offsetWidth)
			{
				//if element is to the right of menu then do nothing
			}
			else if (spm_elementLeft(obj) + obj.offsetWidth < parseInt(eMenu.style.left))
			{
				//if element is to the left of menu then do nothing
			}
			else if (spm_elementTop(obj) + obj.offsetHeight < parseInt(eMenu.style.top))
			{
				//if element is to the top of menu then do nothing
			}
			else
			{
				obj.style.visibility = "hidden";
			}
		}
	}

	function spm_positionMenu(me, oMenu, oCell)
	{
		if (me.display == 'vertical')
		{
			oMenu.style.left = spm_elementLeft(oCell) + oCell.offsetWidth;
			oMenu.style.top = spm_elementTop(oCell);
			oMenu.style.display = "";
		}
		else
		{
			oMenu.style.left = spm_elementLeft(oCell);
			oMenu.style.top = spm_elementTop(oCell) + oCell.offsetHeight;
			oMenu.style.display = "";
			if (spm_elementLeft(oMenu) + oMenu.offsetWidth > document.body.clientWidth)
			{
			  if (document.body.clientWidth - oMenu.offsetWidth > 0)  //only do this if it fits
				  oMenu.style.left = document.body.clientWidth - oMenu.offsetWidth;
//				oMenu.style.top = spm_elementTop(oCell) + oCell.offsetHeight;
			}
			if (spm_elementTop(oMenu) + oMenu.offsetHeight > document.body.clientHeight)
			{
			  if (spm_elementTop(oCell) - oMenu.offsetHeight > 0) //only do this if it fits
				  oMenu.style.top = spm_elementTop(oCell) - oMenu.offsetHeight;
			}
			oMenu.style.display = "none";
		}
	}

	//--------- Internal (private) Functions --------//
	//--- Applies border to cell ---//
	SolpartMenu.prototype.applyBorder = function (oCell, iSize, sTopLeftColor, sBottomRightColor, sStyle)
	{
		if (this.moDisplay == 'Outset')
		{
			if (sStyle == null)
				sStyle = "solid";

			if (sTopLeftColor.length > 0 && sBottomRightColor.length > 0)
			{
				if (oCell.tagName == 'TR')
					oCell = oCell.childNodes(0);
				
				oCell.style.borderTop = sStyle + " " + iSize + "px " + sTopLeftColor;
				oCell.style.borderLeft = sStyle + " " + iSize + "px " + sTopLeftColor;
				oCell.style.borderRight = sStyle + " " + iSize + "px " + sBottomRightColor;
				oCell.style.borderBottom = sStyle + " " + iSize + "px " + sBottomRightColor;	
				
			}
		}
		if (this.moDisplay == 'HighLight')
		{
			if (sTopLeftColor == this.backColor)
			{
				//oCell.style.backgroundColor = '';
        //setClassColor(oCell, 'spmitm', '');
        oCell.className = spm_fixCSSForMac(this.getIntCSSName('spmbar spmitm') + this.cssMenuItem + spm_getAttr(oCell, 'savecss', ''));
			}
			else
			{
				//oCell.style.backgroundColor = this.selColor;
        //setClassColor(oCell, 'spmitm', this.selForeColor);
        oCell.className = spm_fixCSSForMac(this.getIntCSSName('spmbar spmitmsel') + this.cssMenuItemSel);
			}
		}		
	}

	function spm_applyRowBorder(oRow, iSize, sColor, bSelected, sStyle)
	{
		var sColor2=sColor;
		if (sStyle == null)
			sStyle = "solid";

		if (sColor == "")
		{
			//if (bSelected)
			//	sColor2 = this.selColor;
			//else
				sColor2 = spm_getCurrentStyle(oRow.cells[0], 'background-Color');
		}

		spm_applyBorders(oRow.cells[0], sStyle, iSize, sColor2, true, true, false, true);

		if (sColor == "" && bSelected == false)
      sColor2 = spm_getCellBackColor(oRow.cells[1]);
    			
		spm_applyBorders(oRow.cells[1], sStyle, iSize, sColor2, true, false, false, true);
		spm_applyBorders(oRow.cells[2], sStyle, iSize, sColor2, true, false, true, true);
	}
	
	function spm_getCellBackColor(o)
	{
		var sColor = spm_getCurrentStyle(o, 'background-Color');  
    if (spm_browserType() == 'ie')
    {
      //--- fix IE transparent border issue ---//
      while (sColor == 'transparent')
      {
        sColor = spm_getCurrentStyle(o, 'background-Color');  
        o = o.parentElement;
      }
    }
    return sColor;
	}
	
	function spm_applyBorders(o, sStyle, iSize, sColor, t, l, r, b)
	{

/*
		if (t && sColor=='') o.style.paddingTop = iSize + "px ";
		if (b && sColor=='') o.style.paddingBottom = iSize + "px ";
		if (r && sColor=='') o.style.paddingRight = iSize + "px ";
		if (l && sColor=='') o.style.paddingLeft = iSize + "px ";
    if (sColor=='')
      iSize = 0;
 */     
		if (t) o.style.borderTop = sStyle + " " + iSize + "px " + sColor;
		if (b) o.style.borderBottom = sStyle + " " + iSize + "px " + sColor;
		if (r) o.style.borderRight = sStyle + " " + iSize + "px " + sColor;
		if (l) o.style.borderLeft = sStyle + " " + iSize + "px " + sColor;

	}
	
	function spm_elementTop(eSrc)
	{
	if (isMac())
		return spm_elementTopMac(eSrc);
		
	var iTop = 0;
		var eParent;
		eParent = eSrc;
		while (eParent.tagName.toUpperCase() != "BODY")
		{
			iTop += eParent.offsetTop;
			eParent = eParent.offsetParent;
			if (eParent == null)
				break;
		}
		return iTop;
	}

	function spm_elementTopMac(eSrc)
	{
		var iTop = 0;
		var eParent;
		var sDebug = new Array();

		eParent = eSrc;
		while (eParent.tagName.toUpperCase() != "BODY")
		{
			if ((eParent.tagName=="TABLE") && (eParent.offsetParent.tagName=="TD"))
			{
				iTop += eParent.clientTop;
				sDebug[sDebug.length] = eParent.tagName + ' clientTop: ' + eParent.clientTop + ' offsetTop: ' + eParent.offsetTop + ' total: ' + iTop;
			}
			else//else if (eParent.tagName == 'TD')			//	iTop += eParent.parentElement.offsetTop;
			{
				iTop += eParent.offsetTop;
				sDebug[sDebug.length] = eParent.tagName + ' offsetTop: ' + eParent.offsetTop + ' clientTop: ' + eParent.clientTop + ' total: ' + iTop;
			}
			
			eParent = eParent.offsetParent;
			if (eParent == null)
				break;
		}
displayDebug(sDebug);
		return iTop;
	}

	function spm_elementLeft(eSrc)
	{	
		if (isMac())
			return spm_elementLeftMac(eSrc);

		var iLeft = 0;
		var eParent;
		eParent = eSrc;
		while (eParent.tagName.toUpperCase() != "BODY")
		{
			iLeft += eParent.offsetLeft;
			eParent = eParent.offsetParent;
			if (eParent == null)
				break;
		}
		return iLeft;
	}

	function spm_elementLeftMac(eSrc)
	{
		var iLeft = 0;
		var eParent;
		var sDebug = new Array();
				
		eParent = eSrc;
		while (eParent.tagName.toUpperCase() != "BODY")
		{
			if ((eParent.tagName=="TABLE") && (eParent.offsetParent.tagName=="TD"))
			{
				iLeft += eParent.clientLeft;
				sDebug[sDebug.length] = eParent.tagName + ' clientLeft: ' + eParent.clientLeft + ' offsetLeft: ' + eParent.offsetLeft + ' total: ' + iLeft;
			}
			else
			{
				iLeft += eParent.offsetLeft;
				sDebug[sDebug.length] = eParent.tagName + ' offsetLeft: ' + eParent.offsetLeft + ' clientLeft: ' + eParent.clientLeft + ' total: ' + iLeft;
			}
			eParent = eParent.offsetParent;
		}
displayDebug(sDebug);
		return iLeft;	
	}

function displayDebug(sDebug)
{
var sDebugDisp=''
var sIndent='-----------------------------------------';
for (var i=sDebug.length-1; i>=0; i--)
{
	sDebugDisp += sIndent.substr(0, sDebug.length - i) + sDebug[i] + '\n';
}
alert(sDebugDisp);
}
	
	function spm_getElement(e, sID) 
	{
		var o=e;
		var i=0;
		while (o.id != sID)
		{
			o=o.parentNode;
			i++;
		}
		return o;
	}

	function spm_getSourceTR(e, ns)
	{
		while (e.id == "")
		{
			e= e.parentElement;
		}
		if (e.id.indexOf("arrow") != -1)
		{
			var sID = e.id.substr(5);
			return spm_getById("tr" + sID);
		}
		else if (e.id.indexOf("td") != -1)
		{
			var sID = e.id.substr(2);
			return spm_getById("tr" + sID);
		}	
		else if (e.id.indexOf("icon") != -1)
		{
			var sID = e.id.substr(4);
			return spm_getById("tr" + sID);
		}	
		else if (e.id.indexOf("img") != -1)
		{
			var sID = e.id.substr(3);
			return spm_getById("tr" + sID);
		}	
		else
		{
			return e;
		}
	}

	function spm_itemHasChildren(sID, ns)
	{
		objTable = spm_getById(ns + "tbl" + sID);
		if (objTable != null)
		{
			if (objTable.rows != null)
			{
				if (objTable.rows.length > 0)
					return true;
				else
					return false;
			}		
		}
	}

function spm_getMenuItemStyle(sType, oNode)
{
  return spm_getAttr(oNode, sType + "style", '');
}

function spm_getMenuItemCSS(oNode)
{
  return spm_getAttr(oNode, "css", '');
}

SolpartMenu.prototype.getIntCSSName =  function(sClass)
{
  var ary = sClass.split(' ');
  var s='';
  for (var i=0; i<ary.length; i++)
    s += this._m_sNSpace.toLowerCase() + '_' + ary[i] + ' ';
  
  return s;
}

function spm_fixCSSForMac(s)
{
	var ary = s.split(' ');
	var sRet='';
	for (var i=0; i<ary.length; i++)
	{
		if (ary[i].rtrim().length > 0)
		{
			if (sRet.length)
				sRet += ' ' + ary[i];
			else
				sRet = ary[i];
		}
	}
	//alert("'" + s + "'\n'" + sRet + "'");
	return sRet;
}

function spm_getMenuClickAction(oNode)
{
  //'function to determine if menu item has action associated (URL)
  if (spm_getAttr(oNode, "runat", '').length)
    return "__doPostBack(this._m_sNSpace, '" + spm_getAttr(oNode, "id", "") + "');";
  if (spm_getAttr(oNode, "server", '').length)
    return "__doPostBack(this._m_sNSpace, '" + spm_getAttr(oNode, "id", "") + "');";

  var sURL = spm_getAttr(oNode, "url", "");
  if (sURL.length)
	{
		if (sURL.toLowerCase().substr(0, "javascript:".length) == "javascript:")
			return sURL.substr("javascript:".length) + ";";
		else
			return "document.location.href='" + sURL + "';"
	}
	return '';
	
}

function spm_getMenuSpacingImage(sPos, me)
{
  var sAlign = me.menuAlignment;

  if ((sPos == 'left' && sAlign == 'Right') || (sPos == 'right' && sAlign == 'Left'))
		return "       <td width=\"100%\">" + spm_getSpacer(me) + "</td>";

  if ((sPos == 'right' && sAlign == 'Left') || (sPos == 'left' && sAlign == 'Right'))
		return "       <td width=\"3px\">" + spm_getSpacer(me) + "</td>";

	if (sAlign == 'Center')
		return "       <td width=\"33%\">" + spm_getSpacer(me) + "</td>";
	
	return '';   
}

function spm_getSpacer(me) 
{
  return spm_getMenuImage('spacer.gif', me, false);
    //return '&nbsp;'; //"<IMG SRC=\"" + me.systemImagesPath + "spacer.gif\">";
}

function spm_getImage(oAttr, me)
{
  //'retrieves an image for a passed in XMLAttribute
  var sImage = spm_getAttr(oAttr, 'image', '');

  if (sImage.length)
  {
    return spm_getHTMLImage(sImage, spm_getAttr(oAttr, 'imagepath', me.iconImagesPath));
  }
  else
    return spm_getMenuImage('spacer.gif', me);
}

function spm_getItemHTML(oNode, sSide, sDef)
{
  if (sDef == null) sDef = '';
  return spm_getAttr(oNode, sSide + "html", sDef);
}

function spm_getMenuImage(sImage, me, bForce)
{
    //'generates html for image using the SystemImagesPath property
    return spm_getHTMLImage(sImage, me.systemImagesPath, bForce);
}

function spm_getHTMLImage(sImage, sPath, bForce)
{
    //'generates html for image using the SystemImagesPath property
    if (spm_browserNeedsSpacer() == false && sImage == 'spacer.gif' && bForce == null)
        return '&nbsp;'; 
    else
        return "<IMG SRC=\"" + sPath + sImage + "\" ALT=\"" + sImage + "\">";
}

function spm_browserNeedsSpacer()
{
  return true;
}

function MyIIf(bFlag, sTrue, sFalse) 
{
    if (bFlag)
		return sTrue;
	else
		return sFalse;
}

function spm_getArrow(sImg, me) 
{
  //FIX
    if (sImg.length)
        return spm_getMenuImage(sImg, me);
    else
        return "4"; //'defaults to using wingdings font (4 = arrow)
    
}

function spm_getMenuBorderStyle(me, shColor, hlColor, width)
{
  if (shColor == null) shColor = me.shColor;
  if (hlColor == null) hlColor = me.hlColor;
  if (width == null) width = me.borderWidth;
  
  //border-bottom: Gray 1px solid; border-left: White 1px solid; border-top: White 1px solid; border-right: Gray 1px solid;
  //return 'border-bottom: ' + shColor + ' ' + width + 'px solid; border-left: ' + hlColor + ' ' + width + 'px solid;  border-top: ' + hlColor + ' ' + width + 'px solid; border-right: ' + shColor + ' ' + width + 'px solid;';
  return getBorderStyle('border-bottom', shColor, width) + getBorderStyle('border-left', hlColor, width) + getBorderStyle('border-top', hlColor, width) + getBorderStyle('border-right', shColor, width);
}

function getBorderStyle(type, color, width)
{  
  return type + ': ' + color + ' ' + width + 'px solid; ';
}



//------------------------//
String.prototype.ltrim = function () { return this.replace(/^\s*/, "");}
String.prototype.rtrim = function () { return this.replace(/\s*$/, "");}
String.prototype.trim  = function () { return this.ltrim().rtrim(); }

if (spm_browserType() != 'ie')
{
  Document.prototype.loadXML = function (s) 
    {
    
      // parse the string to a new doc
      var doc2 = (new DOMParser()).parseFromString(s, "text/xml");

      // remove all initial children
      while (this.hasChildNodes())
      this.removeChild(this.lastChild);

      // insert and import nodes
      for (var i = 0; i < doc2.childNodes.length; i++) 
      {
      this.appendChild(this.importNode(doc2.childNodes[i], true));
      }
    }

    function _Node_getXML() 
    {
      //create a new XMLSerializer
      var objXMLSerializer = new XMLSerializer;
      
      //get the XML string
      var strXML = objXMLSerializer.serializeToString(this);
      
      //return the XML string
      return strXML;
    }
    Node.prototype.__defineGetter__("xml", _Node_getXML);
}

function spm_createDOMDoc()
{
	if (spm_browserType() == 'ie')
	{
		var o = new ActiveXObject('MSXML.DOMDocument');
		o.async = false;
		return o;
	}
	else
		return document.implementation.createDocument("", "", null);
}

function spm_getById(sID)
{
  if (document.all == null)
    return document.getElementById(sID);
  else
    return document.all(sID);
}

function spm_getTags(sTag, oCtl)
{
	if (oCtl == null)
		oCtl = document;
	
	if (spm_browserType() == 'ie')
    return oCtl.all.tags(sTag);
  else
    return oCtl.getElementsByTagName(sTag);
}

function spm_browserType()
{
  var agt=navigator.userAgent.toLowerCase();

  if (agt.indexOf('netscape') != -1) 
    return 'ns'
  if (agt.indexOf('msie') != -1)
    return 'ie';
  
  return 'mo';  
}

function isMac()
{
//return true;
  var agt=navigator.userAgent.toLowerCase();
  if (agt.indexOf('mac') != -1) 
    return true;
  else
    return false;
  
}

function isOpera()
{
//return true;
  var agt=navigator.userAgent.toLowerCase();
  if (agt.indexOf('opera') != -1) 
    return true;
  else
    return false;
  
}

//taken from http://groups.google.com/groups?hl=en&lr=&ie=UTF-8&oe=UTF-8&safe=off&threadm=b42qj3%24r8s1%40ripley.netscape.com&rnum=1&prev=/groups%3Fq%3Dmozilla%2B%2522currentstyle%2522%26hl%3Den%26lr%3D%26ie%3DUTF-8%26oe%3DUTF-8%26safe%3Doff%26scoring%3Dd 
function spm_getCurrentStyle(el, property) {
  if (document.defaultView) 
  {
    // Get computed style information:
    if (el.nodeType != el.ELEMENT_NODE) return null;
    return document.defaultView.getComputedStyle(el,'').getPropertyValue(property.split('-').join(''));
  }
  if (el.currentStyle) 
  {
    // Get el.currentStyle property value:
    return el.currentStyle[property.split('-').join('')];
    //return el.currentStyle.getAttribute(property.split('-').join(''));  //We need to get rid of slashes
  }
  if (el.style) 
  {
    // Get el.style property value:
    return el.style.getAttribute(property.split('-').join(''));  // We need to get rid of slashes
  } return  null;
}

function spm_getAttr(o, sAttr, sDef)
{
  if (sDef == null)
    sDef = '';
  var s = o.getAttribute(sAttr);
  if (s != null && s.length > 0)
    return o.getAttribute(sAttr);
  else
    return sDef;
}

function spm_setAttr(o, sAttr, sVal)
{
	if (sVal.length > 0)
		o.setAttribute(sAttr, sVal);
	else
		o.removeAttribute(sAttr);
}


function spm_fixUnit(s)
{
  if (s.length && isNaN(s) == false)
    return s + 'px';

}

function spm_nodeHasChildren(node)
{
  if (typeof(node.selectSingleNode) != 'undefined') //(node.selectSingleNode != null) //(spm_browserType() == 'ie')
    return node.selectSingleNode('./menuitem') != null;
  else
  {
    if (node.childNodes.length > 0)
    {
      //Netscape/Mozilla counts an empty <menuitem id></menuitem> as having a child...
      for (var i=0; i< node.childNodes.length; i++)
      {
        if (node.childNodes[i].nodeName == 'menuitem')
            return true;
      }
    }
  }
  return false;  
}

function spm_findNode(oParent, sID)
{
	for (var i = 0; i < oParent.childNodes.length; i++)
	{
		oNode = oParent.childNodes[i];

		if (oNode.nodeType != 3)  //exclude nodeType of Text (Netscape/Mozilla) issue!
		{

			if ((oNode.nodeName == "menuitem" || oNode.nodeName == "menubreak") && oNode.getAttribute("id") == sID)
				return oNode;

			if (oNode.childNodes.length > 0)
			{
				var o = spm_findNode(oNode, sID);
				if (o != null)
					return o;
			}
		}
	}
}

function spm_getSibling(oNode, iOffset)
{
	var sID = spm_getAttr(oNode, 'id');
	var o;
	for (var i=0; i<oNode.parentNode.childNodes.length; i++)
	{
		o = oNode.parentNode.childNodes[i];
		if (o.nodeType != 3)
		{
			if (spm_getAttr(o, 'id') == sID)
				return getOffsetNode(o.parentNode, i, iOffset);
		}
	}
}

function spm_stopEventBubbling(e)
{
    if (spm_browserType() == 'ie')
			window.event.cancelBubble = true;
		else
			e.stopPropagation();
}

//--- if you have a better solution send me an email - jhenning@solpart.com ---//
function spm_appendFunction(from_func, to_func)
{
  if (from_func == null)
    return new Function ( to_func ); 
  return new Function ( spm_parseFunctionContents(from_func) + '\n' + spm_parseFunctionContents(to_func) );
}
function spm_parseFunctionContents(fnc)
{
  var s =String(fnc).trim();
  if (s.indexOf('{') > -1)
		s = s.substring(s.indexOf('{') + 1, s.length - 1);
  return s;
}




//--- For JS DOM ---//
function SPJSXMLNode(sNodeName, sID, oParent, sTitle, sURL, sImage, sImagePath, sRightHTML, sLeftHTML, sRunAtServer, sItemStyle, sImageStyle) 
{ 
  this.nodeName = sNodeName;
  this.id=sID;
  this.childNodes = new Array();
  //this.nodeType = 3;
  
  
  this.parentNode = oParent;            
  if (oParent != null)
  {
    oParent.childNodes[oParent.childNodes.length] = this;
    
    if (oParent.documentElement == null)
      this.documentElement = oParent;
    else
      this.documentElement = oParent.documentElement;
  }
  else
    this.documentElement = this;
    
  this.title = sTitle;
  this.url = sURL;
  this.image = sImage;
  this.imagepath = sImagePath;
  this.righthtml = sRightHTML;
  this.lefthtml = sLeftHTML;
  this.server = sRunAtServer;
  this.itemstyle = sItemStyle;
  this.imagestyle = sImageStyle;
}      
SPJSXMLNode.prototype.getAttribute = function(s)
{
  return this[s];
}


  var m_iSPTimer;
  var m_iSPTotalTimer=0;
  var m_sSPDebugText;
  var m_oSPDebugCtl;
  var m_bSPDebug = false;
  
  function __db(s)
  {
    if (spm_browserType() != 'ie' || m_bSPDebug == false)
      return;
     
    var sT = new Date() - m_iSPTimer;
    if (sT > 120000)
    {
      sT = ''
      m_oSPDebugCtl.value = '---reset---';
      m_iSPTotalTimer=0;
    }
    else if (sT > 100)
    {
      m_iSPTotalTimer+= sT;
      sT = ' *** [' + sT + '] *** ';
    }
    else if (sT > 0)
    {
      m_iSPTotalTimer+= sT;
      sT = ' [' + sT + ']';
    }
    else
      sT = '';
      
    if (document.forms.length > 0 && m_oSPDebugCtl == null)
    {      
      document.forms(0).all(0).insertAdjacentHTML('afterEnd', '<TEXTAREA ID="my__Debug" STYLE="WIDTH: 100%; HEIGHT: 300px"></TEXTAREA>');
      m_oSPDebugCtl = document.all('my__Debug');
    }

    if (m_oSPDebugCtl != null)
      m_oSPDebugCtl.value += '[' + m_iSPTotalTimer + '] ' + s + sT + '\n';
    else
      m_sSPDebugText += '[' + m_iSPTotalTimer + '] ' + s + sT + '\n'; 
      
    m_iSPTimer = new Date();
  }

