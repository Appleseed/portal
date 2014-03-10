// xmenu4.js
// xMenu4 v0.08, Cascading menus from nested ULs!
// Copyright (C) 2002,2003,2004,2005 Michael Foster (cross-browser.com)
// This code is distributed under the terms of the LGPL (gnu.org)

////--- xMenuManager (not complete and really not needed - basically just a stub)
var xMnuMgr = new xMenuManager();

function xMenuManager() {
  /// Properties
  this.activeMenu = null;
  this.firstLd = true;
  this.menus = new Array();
  /// Methods
  this.add = function(xm)
  {
    this.activeMenu = xm;
    this.menus[this.menus.length] = xm;
  }
  this.load = function()
  {
    var i;
    for (i=0; i<this.menus.length; ++i) {
      this.menus[i].load();
    }
    if (this.firstLd) {
      this.firstLd = false;
      xAddEventListener(document, 'mousemove', xmDocOnMousemove, false);
    }
  }
  this.paint = function()
  {
    var i;
    for (i=0; i<this.menus.length; ++i) {
      this.menus[i].paint();
    }
  }
} // end xMenuManager object prototype

////--- Document Mousemove Event Listener

function xmDocOnMousemove(oEvent)
{
  if (xMnuMgr.activeMenu) {
    xMnuMgr.activeMenu.onMousemove(oEvent);
  }
}

////--- xMenu4 Object Prototype (still experimental)

function xMenu4(
  sULId,                      // id str or ele obj of outermost UL
  bAbsolute,                  // outer UL position: true=absolute, false=static
  bHorizontal,                // main label positioning: true=horizontal, false=vertical
  iOfsX, iOfsY,               // box horizontal and vertical offsets
  aLblClp, aBoxClp,           // lbl and box focus clip arrays
  // css class names:
  clsBar, clsBox,             // bar and box
  clsBarLbl, clsBarLblHvr,    // bar lbl LI
  clsBarItm, clsBarItmHvr,    // bar itm LI
  clsBoxLbl, clsBoxLblHvr,    // box lbl LI
  clsBoxItm, clsBoxItmHvr)    // box itm LI
{
  /// Properties
  this.ele = xGetElementById(sULId);
  this.active = null; // active box
  this.item = null; // active item
  this.ori1 = bHorizontal ? 1 : 2; // 1==under/over, 2==right/left (not complete)
  this.ori2 = 2;
  this.abs = bAbsolute;
  this.ofsX = iOfsX;
  this.ofsY = iOfsY;
  this.lblClp = aLblClp;
  this.boxClp = aBoxClp;
  this.clsBar = clsBar;
  this.clsBox = clsBox;
  this.clsBarLbl = clsBarLbl;
  this.clsBarLblHvr = clsBarLblHvr;
  this.clsBarItm = clsBarItm;
  this.clsBarItmHvr = clsBarItmHvr;
  this.clsBoxLbl = clsBoxLbl;
  this.clsBoxLblHvr = clsBoxLblHvr;
  this.clsBoxItm = clsBoxItm;
  this.clsBoxItmHvr = clsBoxItmHvr;
} // end xMenu4 object prototype

////--- xMenu4.traversePre()  Pre-order traversal on ULs (and LIs if bLI) starting with oUL

xMenu4.prototype.traversePre = function(oUL, sMthd, bLI, iLvl)
{
  if (!iLvl) iLvl = 0;
  var isUL = oUL.nodeName.toUpperCase()=='UL';
  if (bLI || isUL) {
    this[sMthd](oUL, iLvl);
    if (isUL) ++iLvl;
  }
  var c = oUL.firstChild;
  while (c) {
    if (c.nodeName.toUpperCase()=='UL' || c.nodeName.toUpperCase()=='LI') {
      this.traversePre(c, sMthd, bLI, iLvl);
    }
    c = c.nextSibling;
  }
}

////--- xMenu4.load()  Initialize the menu data structure

xMenu4.prototype.load = function()
{
  this.traversePre(this.ele, 'loadUL');
  this.traversePre(this.ele, 'loadLI', true);
//  alert(this.ele.innerHTML);
//  xShow(this.ele);
}
xMenu4.prototype.loadUL = function(ele, iLvl)
{
  ele.xmLvl = iLvl;
  ele.isBox = true;
  if (iLvl == 0) { // root ul (bar)
    ele.xmLbl = null;
  }
  else {
    var li = xParent(ele,1);
    ele.xmLbl = li;
    ele.xmLbl.xmBox = ele;
    ele.xmLbl.isLbl = true;
  }
}
xMenu4.prototype.loadLI = function(ele, iLvl)
{
  ele.xmLvl = iLvl;
  if (ele.nodeName.toUpperCase() == 'LI' && !ele.isLbl) {
    ele.isItm = true;
  }
}

////--- xMenu4.paint()  Apply all css then hide and position all menu boxes

xMenu4.prototype.paint = function()
{
  this.applyAllCss();
  this.traversePre(this.ele, 'paintV');
}
xMenu4.prototype.paintV = function(oUL, iLvl)
{
  if (iLvl > 0) {
    xHide(oUL);
    var li = oUL.xmLbl; // xParent(oUL,1);
    var ori = iLvl == 1 ? this.ori1 : this.ori2;
    if (this.abs) { // bar UL = absolute, box UL = absolute, LI = static
      if (ori==1) xMoveTo(oUL, xOffsetLeft(li), xHeight(xParent(li,1)) + this.ofsY);
//      else if (ori==2) xMoveTo(oUL, xWidth(xParent(li,1)) + this.ofsX, xOffsetTop(li));
      else if (ori==2) xMoveTo(oUL, xOffsetLeft(li) + xWidth(li) + this.ofsX, xOffsetTop(li));
    }
    else { // bar UL = static, box UL = absolute, LI = static
      if (ori==1) xMoveTo(oUL, xPageX(li), xPageY(li /* xParent(li,1) */) + xHeight(li /* xParent(li,1) */) + this.ofsY);
      else if (ori==2) xMoveTo(oUL, xWidth(xParent(li,1)) + this.ofsX, xOffsetTop(li));
    }
  }
  else xShow(oUL);
}

////--- xMenu4.activate()  Activate box oUL

xMenu4.prototype.activate = function(oUL)
{
  if (oUL) {
    if (oUL.xmLvl > 0) {
      this.applyLICss(oUL.xmLbl, true);
      xShow(oUL);
    }
    this.active = oUL;
  }
}

////--- xMenu4.deactivate()  Deactivate box oUL

xMenu4.prototype.deactivate = function(oUL)
{
  if (oUL) {
    if (oUL.xmLvl > 0) {
      xHide(oUL);
      this.applyLICss(oUL.xmLbl, false);
    }
    this.active = oUL.xmLvl == 1 ? null : xParent(oUL.xmLbl, 1);
  }
}

////--- xMenu4.deactivateAll()  Deactivate all boxes

xMenu4.prototype.deactivateAll = function()
{
  this.traversePre(this.ele, 'deactivate');
  this.active = null;
}

////--- xMenu4.applyAllCss()

xMenu4.prototype.applyAllCss = function()
{
  this.traversePre(this.ele, 'applyAllCssV', true);
}
xMenu4.prototype.applyAllCssV = function(ele)
{
  if (ele.isBox) this.applyULCss(ele);
  else this.applyLICss(ele);
}

////--- xMenu4.applyULCss()

xMenu4.prototype.applyULCss = function(oUL)
{
  if (oUL && xStr(oUL.className)) {
    if (oUL.xmLvl == 0) { // bar
      oUL.className = this.clsBar;
    }
    else { // box
      oUL.className = this.clsBox;
    }
  }
}

////--- xMenu4.applyLICss()

xMenu4.prototype.applyLICss = function(oLI, bActive)
{
  if (oLI && oLI.xmLvl > 0 && xStr(oLI.className)) {
    if (bActive) { // active state
      if (oLI.xmLvl == 1) {
        oLI.className = oLI.isLbl ? this.clsBarLblHvr : this.clsBarItmHvr;
      }
      else {
        oLI.className = oLI.isLbl ? this.clsBoxLblHvr : this.clsBoxItmHvr;
      }
    }
    else { // inactive state
      if (oLI.xmLvl == 1) {
        oLI.className = oLI.isLbl ? this.clsBarLbl : this.clsBarItm;
      }
      else {
        oLI.className = oLI.isLbl ? this.clsBoxLbl : this.clsBoxItm;
      }
    } // end else inactive state
  }
}

////--- xMenu4.onMousemove()  Handle all user interaction

xMenu4.prototype.onMousemove = function(oEvent)
{
  var evt = new xEvent(oEvent);
  var ele = evt.target;
  while (ele && !ele.isLbl && !ele.isBox && !ele.isItm) {
    ele = xParent(ele,1);
  }

  if (!ele) { // not on lbl nor box nor itm
    if (
      this.active &&
      !xHasPoint(this.active, evt.pageX, evt.pageY, this.boxClp[0], this.boxClp[1], this.boxClp[2], this.boxClp[3])
    ) {
      this.deactivateAll();
    }
  }
  else if (ele.isLbl) { // on lbl
    if (ele.xmBox != this.active) {
      if (this.active && xParent(ele,1) != this.active) {
        this.deactivate(this.active);
      }
      else {
        this.activate(ele.xmBox);
      }
    }
  }
  else if (ele.isBox || ele.isItm) { // on box or itm
    var box = ele;
    if (ele.isItm) box = xParent(ele,1);
    if (this.active && /* box.xmLvl > 0 && */ box != this.active) {
      if (
        // if not going from lbl to box
        !xHasPoint(this.active.xmLbl, evt.pageX, evt.pageY, this.lblClp[0], this.lblClp[1], this.lblClp[2], this.lblClp[3])
        // The following line shouldn't be needed, but in Opera it seems that sometimes
        // when you move the mouse fast the target and coord properties are out of sync... ?
        && !xHasPoint(this.active, evt.pageX, evt.pageY, 0, null, null, null)
      ) {
        this.deactivate(this.active);
      }
    }
  }
  if (ele && ele.isItm) { // on itm
    if (ele != this.item) {
      if (this.item) {
        this.applyLICss(this.item, false);
      }
      this.applyLICss(ele, true);
      this.item = ele;
    }
  }
  else if (this.item) { // not ele or not on itm
    this.applyLICss(this.item, false);
    this.item = null;
  }

/*
  window.status =
    'Menu[ele: ' + xName(ele) +
    ',  aLbl: ' + (this.active ? xName(this.active.xmLbl):'null') +
    '(' + (this.active ? (xPageX(this.active.xmLbl)+','+xPageY(this.active.xmLbl)):'') +
    '),  aBox: ' + xName(this.active)  + '(' + (this.active ? (xPageX(this.active)+','+xPageY(this.active)):'') + ')' +
    (ele && ele.isItm ? '  isItm' : (ele && ele.isLbl ? '  isLbl' : (ele && ele.isBox ? '  isBox' : ''))) + (ele && xNum(ele.xmLvl) ? ',  ' + ele.xmLvl:'') + ']' +
    '  Mouse[xy: ' + evt.pageX + ':' + evt.pageY +
    ',  ele: ' + xName(evt.target) + '(' + xPageX(evt.target) + ',' + xPageY(evt.target) + ')]';
*/
} // end xMenu4.onMousemove()

                                                                                               
////--- dbgMsg()

var firstDbgMsg = true;
function dbgMsg(msg)
{
  if (firstDbgMsg) {
    firstDbgMsg = false;
    xGetElementById('dbg').innerHTML = '<p>' + msg + '</p>';
  }
  else {
    xGetElementById('dbg').innerHTML += '<p>' + msg + '</p>';
  }
}

// end xMenu4.js