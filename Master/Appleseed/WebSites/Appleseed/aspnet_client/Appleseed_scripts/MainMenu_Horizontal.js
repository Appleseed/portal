// JavaScript Document


////--- Load Event Listener
function xMenuLoadOnload()
{
	
  var me = xGetElementById('Main_Nav_Menu');
  if (!xDef(me.nodeName, me.firstChild, me.nextSibling)) {
    return;
  }
  
  var mo = new xMenu4(
    me,                       // id str or ele obj of outermost UL
    true,                     // outer UL position: true=absolute, false=static
    true,                     // main label positioning: true=horizontal, false=vertical
    0, 1,                     // box horizontal and vertical offsets
    [-3, -10, -6, -10],       // lbl focus clip array
    [-30, null, null, null],  // box focus clip array
    // css class names:
    'xmBar', 'xmBox',
    'xmBarLbl', 'xmBarLblHvr',
    'xmBarItm', 'xmBarItmHvr',
    'xmBoxLbl', 'xmBoxLblHvr',
    'xmBoxItm', 'xmBoxItmHvr'
  );

  xMnuMgr.add(mo);
  xMnuMgr.load();
//  xmWinOnResize();
  xMnuMgr.paint();
//  xAddEventListener(window, 'resize', xmWinOnResize, false);
}

////--- Window Resize Event Listener

function xmWinOnResize()
{
  // !!!
  var me = xMnuMgr.activeMenu.ele;
//  var rc = xGetElementById('rightColumn');
  //var mm = xGetElementById('menuMarker');
//  var mmp = xParent(mm);
//  xMoveTo(me, xPageX(mmp)-xPageX(rc), xPageY(mmp)-xPageY(rc));
//  xMnuMgr.paint();
}