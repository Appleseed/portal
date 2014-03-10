// x_tpg.js, part of X, a Cross-Browser.com Javascript Library
// Copyright (C) 2001,2002,2003,2004,2005 Michael Foster - Distributed under the terms of the GNU LGPL - OSI Certified
// File Rev: 5

/* xTabPanelGroup(id, w, h, th, clsTP, clsTG, clsTD, clsTS)
     id - id string of tabPanelGroup element.
     w - overall width.
     h - overall height.
     th - tab height.
     clsTP - tabPanel css class
     clsTG - tabGroup css class
     clsTD - tabDefault css class
     clsTS - tabSelected css class

   Assumes tabPanelGroup element (overall container) has a 1px border.
*/

// xTabPanelGroup, Copyright 2005 Michael Foster (Cross-Browser.com)
// Part of X, a Cross-Browser Javascript Library, Distributed under the terms of the GNU LGPL

function xTabPanelGroup(id, w, h, th, clsTP, clsTG, clsTD, clsTS, selectedPanel) // object prototype
{
  // Private Methods

  function onClick() //r7
  {
    paint(this);
    return false;
  }
  function onFocus() //r7
  {
    paint(this);
  }
  function paint(tab)
  {
    tab.className = clsTS;
    xZIndex(tab, highZ++);
    xDisplay(panels[tab.xTabIndex], 'block'); //r6
  
    if (selectedIndex != tab.xTabIndex) {
      xDisplay(panels[selectedIndex], 'none'); //r6
      tabs[selectedIndex].className = clsTD;
  
      selectedIndex = tab.xTabIndex;
    }
  }

  // Public Methods

  this.select = function(n) //r7
  {
    if (n && n <= tabs.length) {
	  if(n<0)
		n = 0;
      tabs[n].onclick();
    }
  }
  
  this.SelectedPanel = function() //r7
  {
    return selectedIndex;
  }

  this.onUnload = function()
  {
    if (xIE4Up) for (var i = 0; i < tabs.length; ++i) {tabs[i].onclick = null;}
  }

  // Constructor Code (note that all these vars are 'private')

  var panelGrp = xGetElementById(id);
  if (!panelGrp) { return null; }
  var panels = xGetElementsByClassName(clsTP, panelGrp);
  var tabs = xGetElementsByClassName(clsTD, panelGrp);
  var tabGrp = xGetElementsByClassName(clsTG, panelGrp);
  if (!panels || !tabs || !tabGrp || panels.length != tabs.length || tabGrp.length != 1) { return null; }
  var selectedIndex = 0, highZ, x = 0, i;
  xResizeTo(panelGrp, w, h);
  xResizeTo(tabGrp[0], w, th);
  xMoveTo(tabGrp[0], 0, 0);
  w -= 2; // remove border widths
  var tw = w / tabs.length;
  for (i = 0; i < tabs.length; ++i) {
    xResizeTo(tabs[i], tw, th); 
    xMoveTo(tabs[i], x, 0);
    x += tw;
    tabs[i].xTabIndex = i;
    tabs[i].onclick = onClick;
    tabs[i].onfocus = onFocus; //r7
    panels[i].style.overflow ='scroll';
    xDisplay(panels[i], 'none'); //r6
    xResizeTo(panels[i], w, h - th - 2); // -2 removes border widths
    xMoveTo(panels[i], 0, th);
  }
  highZ = i;
  if((selectedPanel) &&  (selectedPanel > 0))
	tabs[selectedPanel].onclick();
  else
	tabs[0].onclick();
}

/*

function xTabPanelGroup(id, w, h, th, clsTP, clsTG, clsTD, clsTS) // object prototype
{
  var panelGrp = xGetElementById(id);
  if (!panelGrp) { return null; }
  var panels = xGetElementsByClassName(clsTP, panelGrp);
//  alert(panels.length);
  var tabs = xGetElementsByClassName(clsTD, panelGrp);
//  alert(tabs.length);
  var tabGrp = xGetElementsByClassName(clsTG, panelGrp);
  if (!panels || !tabs || !tabGrp || panels.length != tabs.length || tabGrp.length != 1) { return null; }
//  alert('have tabs')
  var selectedIndex = 0, highZ, x = 0, i;

//  var settingsTable = xGetElementsByClassName('settings-table');

  
  
//  if(settingsTable){
//	 xHeight(settingsTable[0], xHeight(panelGrp));
//  }


//  xResizeTo(panelGrp, w, null);
  xResizeTo(tabGrp[0], w, th);
  xMoveTo(tabGrp[0], 0, 0);
  
 // w -= 2; // remove border widths
  var tw = w / tabs.length-1;
//alert(h);
  for (i = 0; i < tabs.length; ++i) {
    xResizeTo(tabs[i], tw, th); 
    xMoveTo(tabs[i], x, 0);
    x += tw;
    tabs[i].xTabIndex = i;
    tabs[i].onclick = tabOnClick;
//    xResizeTo(panels[i], w, h - th - 2); // -2 removes border widths
//alert(panels[i].childNodes[0].offsetHeight+(th*22))
    xResizeTo(panels[i], w, (h-20)); // -2 removes border widths
    xMoveTo(panels[i], 0, th);
    if(panels[i].style)
		panels[i].style.display='none';
	//panels[i].style["border"]='1px solid red';
	panels[i].style["overflow"]='scroll';
  }
  
  xResizeTo(panelGrp, w, (th+h+30));
   //xResizeTo(xGetElementById(panelGrp.parentNode.id), w, h);
//alert(th+h);
  highZ = i;
  tabs[0].onclick();
  
  function tabOnClick()
  {
	panels[selectedIndex].style.display='none';
    tabs[selectedIndex].className = clsTD;
//	var prevH = panels[selectedIndex].childNodes[0].clientHeight;
    this.className = clsTS;
    xZIndex(this, highZ++);
    xZIndex(panels[this.xTabIndex], highZ++);
	panels[this.xTabIndex].style.display='block';
    selectedIndex = this.xTabIndex;
	var newH = ( xTop(panels[selectedIndex]) + xHeight(panels[selectedIndex].childNodes[0]) + (th*2) );
	//xClip(e,iTop,iRight,iBottom,iLeft) 
//	alert(newH);
	//xResizeTo(panelGrp, w, newH);
  }

  this.onUnload = function()
  {
    for (var i = 0; i < tabs.length; ++i) {tabs[i].onclick = null;}
  }
}

*/