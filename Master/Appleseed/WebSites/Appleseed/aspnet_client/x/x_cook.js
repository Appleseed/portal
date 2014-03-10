// x_cook.js, part of X, a Cross-Browser.com Javascript Library
// Copyright (C) 2001,2002,2003,2004,2005 Michael Foster - Distributed under the terms of the GNU LGPL - OSI Certified
// File Rev: 4

// cookie implementations based on code from Netscape Javascript Guide

function xSetCookie(name, value, expire, path)
{
  document.cookie = name + "=" + escape(value) +
                    ((!expire) ? "" : ("; expires=" + expire.toGMTString())) +
                    "; path=" + ((!path) ? "/" : path);
}

function xGetCookie(name)
{
  var value=null, search=name+"=";
  if (document.cookie.length > 0) {
    var offset = document.cookie.indexOf(search);
    if (offset != -1) {
      offset += search.length;
      var end = document.cookie.indexOf(";", offset);
      if (end == -1) end = document.cookie.length;
      value = unescape(document.cookie.substring(offset, end));
    }
  }
  return value;
}

// Thanks to Jeff Rose for fixing this one:

function xDeleteCookie(name, path)
{
  if (xGetCookie(name)) {
    document.cookie = name + "=" +
                    "; path=" + ((!path) ? "/" : path) +
                    "; expires=" + new Date(0).toGMTString();
  }
}
