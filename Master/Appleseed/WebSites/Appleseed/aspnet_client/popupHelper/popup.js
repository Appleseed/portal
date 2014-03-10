/* Accessible, friendly popups from links
See http://www.alistapart.com/articles/popuplinks/
Caio Chassot - http://v2studio.com/k/code/lib/
Appleseed implementation - Jes1111 
Version 1.1 27/Nov/2004 - dropped use of listeners - commands now embedded in anchor */
/* defaults */
var _POPUP_FEATURES = 'location=0,statusbar=0,directories=0,width=400,height=300';
function raw_popup(url, target, features) {
    if (isUndefined(features)) features = _POPUP_FEATURES;
    if (isUndefined(target  )) target   = '_blank';
    var theWindow = window.open(url, target, features);
    theWindow.focus();
    return theWindow;
}
function link_popup(src, features) {
    return raw_popup(src.getAttribute('href'), src.getAttribute('target') || '_blank', features);
}
/* supporting library */
function isUndefined(v) {
    var undef;
    return v===undef;
}
