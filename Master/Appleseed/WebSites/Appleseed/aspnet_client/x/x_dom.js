// x_dom.js, part of X, a Cross-Browser.com Javascript Library
// Copyright (C) 2001,2002,2003,2004,2005 Michael Foster - Distributed under the terms of the GNU LGPL - OSI Certified
// File Rev: 5

/* xWalkEleTree

   Function Prototype:

     xWalkEleTree(oNode, fnVisit, oData, iLevel, iBranch);
       oNode:   The Element object at which to begin traversal.
       fnVisit: This function will be called for each Element in the tree.
       oData:   Object or variable to be passed to fnVisit.
       iLevel:  The starting level number (omit for 0).
       iBranch: The starting branch number (omit for 0).
       
   Visit Function Prototype:

     ret = fnVisit(node, level, branch, data);
       node:   The current Element object in the traversal.
       level:  The current depth of the traversal.
       branch: The number of the current level 0 subtree.
       data:   The oData object originally passed to xWalkEleTree.
       ret:    0 = stop, 1 = continue, 2 = skip subtree.
*/
function xWalkEleTree(oNode, fnVisit, oData, iLevel, iBranch)
{
  if (typeof iLevel == 'undefined') iLevel = 0;
  if (typeof iBranch == 'undefined') iBranch = 0;
  var v = fnVisit(oNode, iLevel, iBranch, oData);
  if (!v) return 0;
  if (v == 1) {
    for (var c = oNode.firstChild; c; c = c.nextSibling) {
      if (c.nodeType == 1) {
        if (!iLevel) ++iBranch;
        if (!xWalkEleTree(c, fnVisit, oData, iLevel + 1, iBranch)) return 0;
      }
    }
  }
  return 1;
}

// 10Jan05
function xWalkTree(oNode, fnVisit)
{
  fnVisit(oNode);
  for (var c = oNode.firstChild; c; c = c.nextSibling) {
    if (c.nodeType == 1) xWalkTree(c, fnVisit);
  }
}
/* original implementation:
function xWalkTree(oNode, fnVisit)
{
  if (oNode) {
    if (oNode.nodeType == 1) {fnVisit(oNode);}
    for (var c = oNode.firstChild; c; c = c.nextSibling) {
      xWalkTree(c, fnVisit);
    }
  }
}
*/
function xGetComputedStyle(oEle, sProp, bInt)
{
  var s, p = 'undefined';
  if(document.defaultView && document.defaultView.getComputedStyle){
    s = document.defaultView.getComputedStyle(oEle,'');
    if (s) p = s.getPropertyValue(sProp);
  }
  else if(oEle.currentStyle) {
    // convert css property name to object property name for IE
    var a = sProp.split('-');
    sProp = a[0];
    for (var i=1; i<a.length; ++i) {
      c = a[i].charAt(0);
      sProp += a[i].replace(c, c.toUpperCase());
    }   
    p = oEle.currentStyle[sProp];
  }
  return bInt ? (parseInt(p) || 0) : p;
}
function xGetElementsByClassName(clsName, parentEle, tagName, fn)
{
  var found = new Array();
  var re = new RegExp('\\b'+clsName+'\\b', 'i');
  var list = xGetElementsByTagName(tagName, parentEle);
  for (var i = 0; i < list.length; ++i) {
    if (list[i].className.search(re) != -1) {
      found[found.length] = list[i];
      if (fn) fn(list[i]);
    }
  }
  return found;
}
function xGetElementsByTagName(tagName, parentEle)
{
  var list = null;
  tagName = tagName || '*';
  parentEle = parentEle || document;
  if (xIE4 || xIE5) {
    if (tagName == '*') list = parentEle.all;
    else list = parentEle.all.tags(tagName);
  }
  else if (parentEle.getElementsByTagName) list = parentEle.getElementsByTagName(tagName);
  return list || new Array();
}
function xGetElementsByAttribute(sTag, sAtt, sRE, fn)
{
  var a, list, found = new Array(), re = new RegExp(sRE, 'i');
  list = xGetElementsByTagName(sTag);
  for (var i = 0; i < list.length; ++i) {
    a = list[i].getAttribute(sAtt);
    if (!a) {a = list[i][sAtt];}
    if (typeof(a)=='string' && a.search(re) != -1) {
      found[found.length] = list[i];
      if (fn) fn(list[i]);
    }
  }
  return found;
}
// If tag is defined, returns previous sibling with nodeName == tag, else with nodeType == 1.
function xPrevSib(ele, tag)
{
  var s = ele ? ele.previousSibling : null;
  if (tag) while (s && s.nodeName != tag) { s = s.previousSibling; }
  else while (s && s.nodeType != 1) { s = s.previousSibling; }
  return s;
}
// If tag is defined, returns next sibling with nodeName == tag, else with nodeType == 1.
function xNextSib(ele, tag)
{
  var s = ele ? ele.nextSibling : null;
  if (tag) while (s && s.nodeName != tag) { s = s.nextSibling; }
  else while (s && s.nodeType != 1) { s = s.nextSibling; }
  return s;
}
// If tag is defined, returns first child (or first child's sibling)
// with nodeName == tag, else with nodeType == 1.
function xFirstChild(ele, tag)
{
  var c = ele ? ele.firstChild : null;
  if (tag) while (c && c.nodeName != tag) { c = c.nextSibling; }
  else while (c && c.nodeType != 1) { c = c.nextSibling; }
  return c;
}
