
//<script language="Javascript" type="text/javascript">
/*
This script is part of the EasyListBox server control.
Purchase and licensing information can be found at EasyListBox.com.
*/
var ELB_listState = new Array();
var ELB_listMode = new Array();
var ELB_BackColorRoll = new Array();
var ELB_ForeColorRoll = new Array();
var ELB_ForeColor = new Array();
var ELB_BackColor = new Array();
var ELB_ChildLists = new Array();
var ELB_ChildListsItems = new Array();

var ELB_PageSelects;
var ELB_AllSelects = new Array();

var ELB_AllItems = new Array();

var elbTip = false;
var sCurrentDrag = "";
var iCurrentDrag = -1;
var aCurrentDrag = new Array();
var sSearch;
var sSpeedSearch = "";
var sSpeedSearchID = "";
var currentKeyDown = 0;
var binGotSelects = false;

// Here's your NoChange variable:
var sNoSelect = "asdfqweupodfnsfencv";

function getObj(objName) {
    var theObj = document.getElementById(objName);
    return theObj;
    }

function ELB_focus(ctlID) {
    if(sSpeedSearchID != ctlID) { sSpeedSearchID = ctlID; sSpeedSearch = ""; elbTip.innerText = sSpeedSearch; }
    var oHead = getObj(ctlID + "_listHead");
    if(oHead.style.display == "none") {
        getObj(ctlID + "_dropList").focus();
        }
    else if(getObj(ctlID + "_comboText")) {
        getObj(ctlID + "_comboText").select();
        }
    else {
        getObj(ctlID + "_listHeadText").focus();
        }
    }

function ELB_init(ctlID, postBackVal, selMode, rollBackColor, rollForeColor, childLists, parentList, swapTarget) {
    ELB_listState[ctlID] = 0;
    ELB_listMode[ctlID] = selMode;
    ELB_BackColorRoll[ctlID] = rollBackColor;
    ELB_ForeColorRoll[ctlID] = rollForeColor;


    var oList = getObj(ctlID + "_dropList");
    var oHead = getObj(ctlID + "_listHead");
	var oListTable = getObj(ctlID + "_dropListTable");
    var binClipHead = oHead.clipHead;
    var selIndex = parseInt(getObj(ctlID + "_SelectedIndex").value);
    var displayMode = oHead.displayMode;

    ELB_ForeColor[ctlID] = oHead.style.color;
    ELB_BackColor[ctlID] = oHead.style.background;

    var hdRow = getObj(ctlID + "_dropListTable").rows[0];
    hdRow.style.display = "none";
    hdRow.selected = false;

    if((parentList != "") && (getObj(parentList)) ) {
        var theParent = getObj(parentList);
        ELB_FillChildList(ctlID, theParent.value);
        }
/* Added (begin)
	else {
		ELB_AllItems[ctlID] = new Array();
		var tmpArray = ELB_AllItems[ctlID];
		var xItem = 1;
		var tmpRows = getObj(ctlID + "_dropListTable").rows;
		while(tmpRows[xItem]) {
			tmpArray[xItem] = tmpRows[xItem].cells[0].innerText.toLowerCase() + "|" + tmpRows[xItem].id;
			xItem++;
			}
		ELB_AllItems[ctlID] = tmpArray;
		}
//  Added (end) */
    if(oHead.style.borderStyle == "solid") {
        oList.style.scrollbarArrowColor = oHead.buttonForeground;
        oList.style.scrollbar3dLightColor = oHead.buttonForeground;
        oList.style.scrollbarShadowColor = oHead.buttonForeground;

        oList.style.scrollbarFaceColor = oHead.buttonBackground;
        oList.style.scrollbarHighlightColor = oHead.buttonBackground;
        oList.style.scrollbarDarkShadowColor = oHead.buttonBackground;
        oList.style.scrollbarTrackColor = oHead.buttonBackground;
        }
    if(swapTarget) { getObj(ctlID + "_dropListTable").swapTarget = true; ELB_RollUp(ctlID); }
    else if(selMode == 1) { // Multiple
        getObj(ctlID + "_listHeadText").onKeyUp = "";
        ELB_MultiSetup(ctlID);
        }
    else { // Single
        oHead.style.height = "10px";
        oList.style.width = oHead.offsetWidth;
        if(selIndex > -1) {
            if(parentList != "") { selIndex = parseInt(getObj(ctlID + "_SelectedIndex").value); }
            //oList.childNodes[0].rows[selIndex].selected = true;
			oListTable.rows[selIndex].selected = true;
            ELB_Rollover(oListTable.rows[selIndex]);
            getObj(ctlID + "_SelectedText").value = oListTable.rows[selIndex].cells[0].innerText;
            oListTable.title = "Selected:  " + oListTable.rows[selIndex].cells[0].innerText;
            }
        }
    if(displayMode == "listbox") {
        if(oHead.style.position == "absolute") {
            oList.style.top = oHead.style.top;
            oList.style.left = oHead.style.left;
            }
        else { oList.style.position = "relative"; }
        oList.style.display = "inline";
        oList.style.visibility = "visible";
        oList.style.border = oHead.style.border;
        oList.tabIndex = getObj(ctlID + "_listHeadText").tabIndex;
        oList.hideFocus = false;
        getObj(ctlID + "_listHeadText").tabIndex = -1;
        oList.style.width = oHead.style.width;
        oHead.style.display = "none";

        if(selMode == 0) {
            ELB_ListBoxSetup(ctlID, swapTarget);
            }
        }
    else if(displayMode == "combo") {
        oList.onmouseover = "";
		var theText = getObj(ctlID + "_comboText");
		theText.style.className = getObj(ctlID + "_dropListTable").rows[0].style.className;
		theText.style.fontFamily = getObj(ctlID + "_dropListTable").rows[0].style.fontFamily;
		theText.style.fontSize = getObj(ctlID + "_dropListTable").rows[0].style.fontSize;
        if(selIndex != -1 && getObj(ctlID).value != "") {
            getObj(ctlID + "_comboText").value = getObj(ctlID + "_dropListTable").rows[selIndex].cells[0].innerText;
            getObj(ctlID + "_listHead").title = "Selected:  " + getObj(ctlID + "_comboText").value;
            getObj(ctlID + "_dropListTable").title = "Selected:  " + getObj(ctlID + "_comboText").value;
            }
        getObj(ctlID + "_listHeadText").onKeyUp = "";
        getObj(ctlID + "_listHeadText").tabIndex = -1;
        }
    else {
        oList.onmouseover = "";
        }
    if(childLists) {
        ELB_ChildLists[ctlID] = childLists;
        ELB_SetChildren(childLists, getObj(ctlID).value);
        }
    getObj(ctlID).autopostback = postBackVal;
/*
    var theBanner = document.getElementById(ctlID + "_bannerTop");
    if(theBanner) {
        theBanner.style.width = "100%";
        //theBanner = document.getElementById(ctlID + "_bannerBottom");
        //theBanner.style.pixelWidth = oList.style.pixelWidth - 5;
        window.status = theBanner.style.width + "     " + theBanner.parentElement.style.width;
        }
*/
    }

function ELB_GetAllSelects() {
    var iSelectCount = 0;
    ELB_PageSelects = document.getElementsByTagName("SELECT");
    ELB_AllSelects[iSelectCount] = ELB_PageSelects;
    var allIFrames = document.frames;
    if(allIFrames) {
        iSelectCount += 1;
        ELB_AllSelects[iSelectCount] = document.getElementsByTagName("IFRAME");
        for(iFrames = 0; iFrames < allIFrames.length; iFrames++) {
            iSelectCount += 1;
            ELB_PageSelects = allIFrames[iFrames].document.getElementsByTagName("SELECT");
            ELB_AllSelects[iSelectCount] = ELB_PageSelects;
            }
        }
    if(window.parent) {
        for(iFrame = 0; iFrame < window.parent.frames.length; iFrame++) {
            iSelectCount += 1;
            ELB_PageSelects = window.parent.frames[iFrame].document.getElementsByTagName("SELECT");
            ELB_AllSelects[iSelectCount] = ELB_PageSelects;
            }
        }
    binGotSelects = true;
    }

function ELB_ListBoxSetup(ctlID, swapTarget) {
    var elbTemp = getObj(ctlID);
    var selIndex = getObj(ctlID + "_SelectedIndex");
    var tblTemp = getObj(ctlID + "_dropListTable");
    var oHead = getObj(ctlID + "_listHead");

    for(var i=1;i<tblTemp.rows.length;i++) {
        var rowTemp = tblTemp.rows[i];

        if(elbTemp.value.indexOf(rowTemp.selectvalue) >= 0 && !swapTarget) {
            binSelected = true;
            rowTemp.selected = true;
            if(oHead.showCheckBoxes != "true") { rowTemp.style.background = ELB_BackColorRoll[ctlID]; }
            }
        for(var j=0;j<rowTemp.cells.length;j++) {
            var celTemp = rowTemp.cells[j];
            celTemp.onclick = celTemp.onblur;
            celTemp.onblur = "";
            if(rowTemp.selected && !swapTarget) {
                celTemp.style.color = ELB_ForeColorRoll[ctlID]
                }
            }
        }
    }

function ELB_MultiSetup(ctlID) {
    var binSelected = false;
    var elbTemp = getObj(ctlID);
    var oHead = getObj(ctlID + "_listHead");
    var sTitle = "";

    var tblTemp = getObj(ctlID + "_dropListTable");
    for(var i=1;i<tblTemp.rows.length;i++) {
        var rowTemp = tblTemp.rows[i];
        if(elbTemp.value.indexOf("," + rowTemp.selectvalue + ",") >= 0 || elbTemp.value.indexOf(rowTemp.selectvalue + ",") == 0) {
            binSelected = true;
            rowTemp.selected = true;
            rowTemp.onmouseup = tblTemp.parentElement.onmouseup;
            if(oHead.showCheckBoxes != "true") { rowTemp.style.background = ELB_BackColorRoll[ctlID]; }
            else { rowTemp.cells[0].childNodes(0).checked = true; }
            }
        for(var j=0;j<rowTemp.cells.length;j++) {
            var celTemp = rowTemp.cells[j];
            celTemp.unselectable = "yes";
            celTemp.onclick = celTemp.onblur;
            celTemp.onblur = "";
            if(rowTemp.selected) {
            if(oHead.showCheckBoxes != "true") { celTemp.style.color = ELB_ForeColorRoll[ctlID]; }
            else { rowTemp.cells[0].childNodes(0).checked = true; }
                if(j==0) {
                    sTitle += "<br />" + celTemp.innerText;
                    }
                }
            }
        }
    if(!binSelected) {
        elbTemp.value = "";
        getObj(ctlID + "_SelectedIndex").value = "-1";
        getObj(ctlID + "_SelectedText").value = "";
        }
    if(sTitle != "") {
        ELB_displaySelected(ctlID, sTitle);
        }
    }

function ELB_FilterCombo(ctlID) {
    var comboText = getObj(ctlID + "_comboText");
    var oList = getObj(ctlID + "_dropListTable");
    var sFilter = comboText.value;
    var newText = "";
    var selIndex = parseInt(getObj(ctlID + "_SelectedIndex").value);
    var maxIndex = oList.rows.length - 1;
    var moveIndex = 0;

    currentKeyDown--;

    if(window.event.altKey && window.event.keyCode == 40) { // Alt + down arrow
        ELB_expandList(ctlID);
        return;
        }
    else if(window.event.keyCode == 36 || window.event.keyCode == 16) {
        return;
        }
    else if(window.event.keyCode == 37) {
        return;
        }
    else if(window.event.keyCode == 27) {
        ELB_ClearSelection(ctlID);
		while(oList.scrollTop != 0) {
			oList.doScroll("scrollBarPageUp");
			}
        return;
        }
    else if(window.event.keyCode == 18) { // Letting up the Alt key
        window.event.cancelBubble = true;
        return;
        }
    else if(window.event.keyCode == 38 || window.event.keyCode == 40) { // Up or down arrow
        moveIndex = -39 + window.event.keyCode;

        if(selIndex > maxIndex) { selIndex = maxIndex }
        else if(selIndex < 0) { selIndex = 0 }
        else {
            ELB_SelectItem(ctlID, oList.rows[selIndex].id);
            getObj(ctlID + "_comboText").select();
            }
        return;
        }
    else if(sFilter == "" ) { // Clear it out if the field is empty
        if(getObj(ctlID).value != "") {
            ELB_ClearSelection(ctlID);
			while(oList.scrollTop != 0) {
				oList.doScroll("scrollBarPageUp");
				}
            }
        return;
        }
    else if(window.event.keyCode == 8) { // let the user backspace
        if(parseInt(getObj(ctlID + "_SelectedIndex").value) > -1) {
            sFilter = sFilter.substring(0, sFilter.length - 1);
            }
        if(sFilter.length == 0) {
            comboText.value = "";
            if(getObj(ctlID).value != "") {
                ELB_ClearSelection(ctlID);
                }
			while(oList.scrollTop != 0) {
				oList.doScroll("scrollBarPageUp");
				}
            return };
        }

	var tmpEnum = new Enumerator(oList.rows);
	var theRow;
	var theText;

	for (;!tmpEnum.atEnd();tmpEnum.moveNext()) { // And if it's an alphanumeric...
        if(currentKeyDown > 0) { return; }
        var rowTemp = tmpEnum.item();

        if(rowTemp.cells[0].innerText.toLowerCase().indexOf(sFilter.toLowerCase()) == 0) {
            ELB_SelectItem(ctlID, rowTemp.id);

            newText = comboText.createTextRange();
            newText.moveStart("character", sFilter.length);
            newText.select();
            ELB_expandList(ctlID);
            return;
            }
/* Old-school "find each row" method
    for(var i=1;i<oList.rows.length;i++) { // And if it's an alphanumeric...
        if(currentKeyDown > 0) { return; }
        var rowTemp = oList.rows[i];

        if(rowTemp.cells[0].innerText.toLowerCase().indexOf(sFilter.toLowerCase()) == 0) {
            ELB_SelectItem(ctlID, rowTemp.id);

            newText = comboText.createTextRange();
            newText.moveStart("character", sFilter.length);
            newText.select();
            return;
            }
*/
        }
    ELB_ClearSelection(ctlID);
	while(oList.scrollTop != 0) {
			window.status = oList.scrollTop;
		oList.doScroll("scrollBarPageUp");
		}
    comboText.value = sFilter;
    comboText.focus();

    getObj(ctlID + "_SelectedIndex").value = -1;
    getObj(ctlID).value = sFilter;
    getObj(ctlID).text = sFilter;
    getObj(ctlID + "_SelectedText").value = sFilter;
    }

function ELB_OnKeyDown(ctlID) {
    if(window.event.keyCode == 9) {    }
    else if(getObj(ctlID + "_dropList").style.visibility == "visible" &&
        window.event.keyCode == 13 && getObj(ctlID + "_listHead").displayMode != "listbox") { // Pressing Enter on the list
        ELB_retractList(ctlID);
        window.event.returnValue = false;
        return;
        }
    else if(getObj(ctlID + "_listHead").displayMode == "combo") {
        if(!window.event.repeat) { currentKeyDown++; }
        window.event.cancelBubble = true;
        return;
        }
//    else if(window.event.keyCode == 38 || window.event.keyCode == 40) { ELB_FilterSelect(ctlID) }
    else if(window.event.keyCode != 17 && ELB_listMode[ctlID] != 1) {
        ELB_FilterSelect(ctlID);
        try {
            window.event.keyCode = 123;
            }
        catch(e) {
            }
        finally {
            window.event.returnValue = false;
            window.event.cancelBubble = true;
            }
        return;
        }
    }

function ELB_FilterSelect(ctlID) {
/* Prior method
    if(!elbTip) {
        elbTip = document.createElement('<span id="elbSpanTip" style="position:absolute;visibility:hidden;background:lightyellow; border:1px solid gray;padding:2px;font-size:8pt;font-family:Verdana;z-index:999;height:10px;filter:progid:DXImageTransform.Microsoft.Alpha(opacity=70);" >');
        //document.body.appendChild(elbTip);
        var oHead = getObj(ctlID + "_listHead");
        oHead.parentElement.appendChild(elbTip);

        }
// */
    elbTip = document.getElementById(ctlID + "_spanTip");
	if(!elbTip) {
		elbTip = document.createElement('<span id="' + ctlID + '_spanTip" style="position:absolute;visibility:hidden;background:lightyellow; border:1px solid gray;padding:2px;font-size:8pt;font-family:Verdana;z-index:999;height:10px;filter:progid:DXImageTransform.Microsoft.Alpha(opacity=70);" >');
		//document.body.appendChild(elbTip);
		var oHead = getObj(ctlID + "_listHead");
		oHead.parentElement.appendChild(elbTip);
	 	}

    var tblList = getObj(ctlID + "_dropListTable");
    var selIndex = parseInt(getObj(ctlID + "_SelectedIndex").value);
    var maxIndex = tblList.rows.length - 1;
    var moveIndex = 0;
    var theKey = window.event.keyCode;
    var binFilter = false;

    if(theKey == 8) {
        if(window.event.keyCode == 8 && sSpeedSearch != "") {
            sSpeedSearch = sSpeedSearch.substring(0, sSpeedSearch.length - 1);
            elbTip.innerText = sSpeedSearch;
            if(sSpeedSearch == "") { 
				ELB_ClearSelection(ctlID); elbTip.style.visibility = "hidden";
				var oList = getObj(ctlID + "_dropList");
				while(oList.scrollTop != 0) {
					oList.doScroll("scrollBarPageUp");
					}
				}
            }
        binFilter = true;
        }
    else if((theKey >= 48 && theKey <= 57) || (theKey >= 65 && theKey <= 90) || (theKey == 32) ||
     (theKey >= 96 && theKey <= 105)) { // Alphanumeric key (including keypad)
        sSpeedSearch += String.fromCharCode(window.event.keyCode).toLowerCase();
        elbTip.innerText = sSpeedSearch;
        if(elbTip.style.visibility == "hidden") {
            ELB_showSearch(ctlID);
            }
        binFilter = true;
        }

    ELB_focus(ctlID);

    if(binFilter && sSpeedSearch != "") {

// / * New "enumerate through rows" approach 
		var tmpEnum = new Enumerator(tblList.rows);
		var theRow;
		var theText;

		for (;!tmpEnum.atEnd();tmpEnum.moveNext()) {
            theRow = tmpEnum.item();
            theText = theRow.innerText.toLowerCase();

            if(theText.indexOf(sSpeedSearch) == 0) {
                ELB_SelectItem(ctlID, theRow.id);
   		        ELB_expandList(ctlID);
                if(getObj(ctlID + "_listHead").displayMode == "listbox") { theRow.cells[0].focus(); }
                return;
                }
            }
// */	
/* Experimental "run through array" method
		var tmpArray = ELB_AllItems[ctlID];

        for(var i=1;i<tmpArray.length;i++) {
            var theText = tmpArray[i];

            if(theText.indexOf(sSpeedSearch) == 0) {
                ELB_SelectItem(ctlID, tmpArray[i].split("|")[1]);
                if(getObj(ctlID + "_listHead").displayMode == "listbox") { theRow.cells[0].focus(); }
                return;
                }
            }
// */
/* Old "find rows by indices" method
        for(var i=1;i<tblList.rows.length;i++) {
            var theRow = tblList.rows[i];
            var theText = theRow.innerText.toLowerCase();

            if(theText.indexOf(sSpeedSearch) == 0) {
                ELB_SelectItem(ctlID, theRow.id);
                if(getObj(ctlID + "_listHead").displayMode == "listbox") { theRow.cells[0].focus(); }
                return;
                }
            }
// */
        window.event.keyCode = 123;
        return;
        }

    if(window.event.altKey && theKey == 40) { ELB_expandList(ctlID); } // Alt + down arrow
    else if(theKey == 38) { moveIndex -= 1; window.event.keyCode = 123; window.event.returnValue = false; sSpeedSearch = ""; elbTip.style.visibility = "hidden"; } // Up arrow
    else if(theKey == 40) { moveIndex += 1; window.event.keyCode = 123; window.event.returnValue = false; sSpeedSearch = ""; elbTip.style.visibility = "hidden"; } // Down arrow
    else if(theKey == 9) { return }
    else if(theKey !== 35 && theKey !== 36) { return }

    selIndex = parseInt(selIndex) + parseInt(moveIndex);
    if(selIndex == 0) { selIndex = 1; }

    if(selIndex > maxIndex) {
        selIndex = maxIndex;
        sSpeedSearch = "";
        return;
        }
    else if(selIndex < 1) {
        selIndex = 1;
        sSpeedSearch = "";
        return;
        }
    else if(moveIndex != 0) {
        sSpeedSearch = "";
        if(theKey == 36) { ELB_SelectItem(ctlID, tblList.rows[1].id); window.event.returnValue = false; } // Home button
        else if(theKey == 35) { ELB_SelectItem(ctlID, tblList.rows[maxIndex].id); window.event.returnValue = false;  } // End button
        else {ELB_SelectItem(ctlID, tblList.rows[selIndex].id) } // Up or down movement
        }
    else {
        }

    if(tblList.parentElement.style.visibility == "visible") {
        tblList.rows[selIndex].cells[0].focus();
        }

    if(getObj(ctlID + "_listHead").displayMode == "dropdown") {
        getObj(ctlID + "_listHeadText").focus();
        }
    else if(getObj(ctlID + "_listHead").displayMode == "combo") {
            if(tblList.parentElement.style.visibility == "visible") { }
            else { getObj(ctlID + "_comboText").select() }
        }
    }

function ELB_showSearch(ctlID) {
    var oHead;
    var binIsHead;

    if(getObj(ctlID + "_listHead").style.display != "none") {
        oHead = getObj(ctlID + "_listHead");
        }
    else {
        oHead = getObj(ctlID + "_dropList");
        }

    var topCorrection = 0;
    var oParent = oHead.parentElement;

    if(oParent.tagName == "TD") {
        if(oParent.vAlign == "bottom") {
            if(oParent.style.borderTopWidth) {
                 topCorrection += parseInt(oParent.style.borderTopWidth)*2 + parseInt(oHead.style.borderWidth);
                 }
            }
        if(oParent.vAlign == "middle" || oParent.vAlign == "") {
            if(oParent.style.borderTopWidth) {
                topCorrection += parseInt(oParent.style.borderTopWidth) + parseInt(oHead.style.borderWidth);
                }
            }
        }

    elbTip.style.left = oHead.offsetLeft;
    elbTip.style.pixelTop = oHead.offsetTop + oHead.offsetHeight; // + topCorrection;
    elbTip.style.zIndex = oHead.style.zIndex + 100;

    var listBottom = elbTip.style.pixelTop + 40;
    var screenBottom = parseInt(document.body.clientHeight) + parseInt(document.body.scrollTop);
    var listTop = -(parseInt(oHead.offsetTop) - 40);

    if((listBottom > screenBottom) && (listTop < (listBottom - screenBottom))) {
        elbTip.style.top = parseInt(oHead.offsetTop) - 40 + topCorrection;
        }

    elbTip.style.visibility = "visible";
    }

function ELB_FilterMultiSelect(ctlID) {
    sSearch = ctlID;
    if(window.event.keyCode == 9) { return } // Just tabbed in; let it go

    var oSearch = getObj(ctlID + "_searchBox");
    var oTable = getObj(ctlID + "_dropListTable");
    var rowTemp;
    var sText = oSearch.value.toLowerCase();

    if(sText != "") {
        for(var i=1;i<oTable.rows.length;i++) {
            rowTemp = oTable.rows[i];
            if((rowTemp.cells[0].innerText.toLowerCase().indexOf(sText) > -1) && !rowTemp.selected) {
                ELB_SelectItem(ctlID, rowTemp.id);
                }
            }
        }
    sSearch = "";
    }

function ELB_doResize(ctlID) {
    if(ctlID == "[object]" || !ctlID) {
        ctlID = window.event.srcElement.id;
        ctlID = ctlID.substring(0, ctlID.indexOf("_"));
        }

    var oHead = getObj(ctlID + "_listHead");
    var oList = getObj(ctlID + "_dropList");
    var oHeadText = getObj(ctlID + "_listHeadText");
    var oClipText;
    if(oHead.clipHead == "true") { oClipText = oHeadText.getElementsByTagName("SPAN")[0]; }

    //window.status = "Resizing... list head width = " + oHead.style.pixelWidth + ";  clip text width = " + oClipText.style.pixelWidth +
    //    ";  SrcElement = " + window.event.srcElement.id;

    oClipText.style.pixelWidth = oHead.style.pixelWidth - 32;

    if(oList.style.visibility == "visible") { ELB_expandList(ctlID); }
    else { oList.style.pixelWidth = oHead.style.pixelWidth; }
    }

function ELB_SwapItem(sourceID, targetID) {
    var tblSource = getObj(sourceID + "_dropListTable");
    var tblTarget = getObj(targetID + "_dropListTable");
    var rowTempOld;
    var rowTempNew;
    var celTempNew;
    var celTempOld;

    ELB_ClearSelection(targetID);

    for(var i=1;i<tblSource.rows.length;i++) {
        rowTempOld = tblSource.rows[i];
        rowTempOld.id = sourceID + "_Row" + i;
        if(rowTempOld.selected) {

            var rowTempNew = tblTarget.insertRow();
            var celTempNew = rowTempNew.insertCell();
            var celTempOld = rowTempOld.cells[0];
            rowTempNew.mergeAttributes(rowTempOld);
            rowTempNew.id = targetID + "_Row" + (tblTarget.rows.length - 1);
            celTempNew.innerHTML = celTempOld.innerHTML;
            celTempNew.mergeAttributes(tblTarget.rows[0].cells[0]);
            celTempNew.id = targetID + "_sC1";

            if(rowTempOld.cells.length > 1) {
                celTempNew = rowTempNew.insertCell();
                celTempOld = rowTempOld.cells[1];
                rowTempNew.mergeAttributes(rowTempOld);
                rowTempNew.id = targetID + "_Row" + (tblTarget.rows.length - 1);
                celTempNew.innerHTML = celTempOld.innerHTML;
                celTempNew.mergeAttributes(tblSource.rows[0].cells[1]);
                celTempNew.id = targetID + "_sC2";

                }
            if(rowTempOld.cells.length > 2) {
                celTempNew = rowTempNew.insertCell();
                celTempOld = rowTempOld.cells[2];
                rowTempNew.mergeAttributes(rowTempOld);
                rowTempNew.id = targetID + "_Row" + (tblTarget.rows.length - 1);
                celTempNew.innerHTML = celTempOld.innerHTML;
                celTempNew.mergeAttributes(tblSource.rows[0].cells[2]);
                celTempNew.id = targetID + "_sC3";
                }

            tblSource.deleteRow(i);


            ELB_SelectItem(targetID, rowTempNew.id);
            if(tblSource.swapTarget) { ELB_RollUp(sourceID); }
            if(tblTarget.swapTarget) { ELB_RollUp(targetID); }
            i--;
            }
        }
    ELB_ClearSelection(sourceID);
    }

function ELB_MoveItem(ctlID, moveIndex) {
    var tblTemp = getObj(ctlID + "_dropListTable");
    var rowTemp;
    if(moveIndex < 0) {
        for(var iRow=1;iRow<tblTemp.rows.length;iRow++) {
            rowTemp = tblTemp.rows[iRow];
            if(rowTemp.selected && (iRow + moveIndex > 0) && (iRow + moveIndex < tblTemp.rows.length)) {
                tblTemp.moveRow(iRow, (iRow + moveIndex));
                }
            }
        }
    else {
        for(var iRow=tblTemp.rows.length-1;iRow>0;iRow--) {
            rowTemp = tblTemp.rows[iRow];
            if(rowTemp.selected && (iRow + moveIndex > 0) && (iRow + moveIndex < tblTemp.rows.length)) {
                tblTemp.moveRow(iRow, (iRow + moveIndex));
                }
            }
        }

    if(tblTemp.swapTarget) { ELB_RollUp(ctlID); }
    }

function ELB_RollUp(ctlID) {
    var sValue = "";
    var sText = "";
    var sText2 = "";
    var sText3 = "";
    var tblTemp = getObj(ctlID + "_dropListTable");
    var rowTemp;

    for(var iRow=1;iRow<tblTemp.rows.length;iRow++) {
        rowTemp = tblTemp.rows[iRow];
        rowTemp.id = ctlID + "_Row" + iRow;
        sText += rowTemp.cells[0].innerText;
        if(rowTemp.cells.length > 1) { sText2 += rowTemp.cells[1].innerText; }
        if(rowTemp.cells.length > 2) { sText3 += rowTemp.cells[2].innerText; }

        sValue += rowTemp.selectvalue;
        if(sValue != "" && iRow != (tblTemp.rows.length - 1)) {
            sValue += "|";
            sText += "|";
            if(rowTemp.cells.length > 1) { sText2 += "|"; }
            if(rowTemp.cells.length > 2) { sText3 += "|"; }
            }
        }
    getObj(ctlID).value = sValue;
    getObj(ctlID + "_SelectedText").value = sText;
    getObj(ctlID + "_SelectedText2").value = sText2;
    getObj(ctlID + "_SelectedText3").value = sText3;
    }

function ELB_AddItem(ctlID, arrValues) {
    var tblTarget = getObj(ctlID + "_dropListTable");
    var rowTempOld;
    var rowTempNew;
    var celTempNew;
    var celTempOld;

    for(var i=1;i<tblSource.rows.length;i++) {
        rowTempOld = tblSource.rows[i];
        rowTempOld.id = sourceID + "_Row" + i;
        if(rowTempOld.selected) {

            var rowTempNew = tblTarget.insertRow();
            var celTempNew = rowTempNew.insertCell();
            var celTempOld = rowTempOld.cells[0];
            rowTempNew.mergeAttributes(rowTempOld);
            rowTempNew.id = targetID + "_Row" + (tblTarget.rows.length - 1);
            celTempNew.innerHTML = celTempOld.innerHTML;
            celTempNew.mergeAttributes(tblTarget.rows[0].cells[0]);
            celTempNew.id = targetID + "_sC1";

            if(rowTempOld.cells.length > 1) {
                celTempNew = rowTempNew.insertCell();
                celTempOld = rowTempOld.cells[1];
                rowTempNew.mergeAttributes(rowTempOld);
                rowTempNew.id = targetID + "_Row" + (tblTarget.rows.length - 1);
                celTempNew.innerHTML = celTempOld.innerHTML;
                celTempNew.mergeAttributes(tblSource.rows[0].cells[1]);
                celTempNew.id = targetID + "_sC2";

                }
            if(rowTempOld.cells.length > 2) {
                celTempNew = rowTempNew.insertCell();
                celTempOld = rowTempOld.cells[2];
                rowTempNew.mergeAttributes(rowTempOld);
                rowTempNew.id = targetID + "_Row" + (tblTarget.rows.length - 1);
                celTempNew.innerHTML = celTempOld.innerHTML;
                celTempNew.mergeAttributes(tblSource.rows[0].cells[2]);
                celTempNew.id = targetID + "_sC3";
                }

            tblSource.deleteRow(i);


            ELB_SelectItem(targetID, rowTempNew.id);
            if(tblSource.swapTarget) { ELB_RollUp(sourceID); }
            if(tblTarget.swapTarget) { ELB_RollUp(targetID); }
            i--;
            }
        }
    }

function ELB_RemoveItem(ctlID, rowIndex) {
    var tblList = getObj(ctlID + "_dropListTable");
    tblList.deleteRow(tblList.rows[rowIndex]);
    if(tblList.swapTarget) { ELB_RollUp(ctlID); }
    }

function ELB_ClearSelection(ctlID) {
    var oHead = getObj(ctlID + "_listHead");
    var oList = getObj(ctlID + "_dropListTable");

    if(oList.title.length > 0) { oList.title = ""; }
    if(oHead.title.length > 0) { oHead.title = ""; }

    if(ELB_ChildLists[ctlID]) { // Clear out the children
        ELB_SetChildren(ELB_ChildLists[ctlID], "aybabtuaybabtuaybabtu");
        }
    if(oList.swapTarget) {
        for(var i=0;i<oList.rows.length;i++) {
            if(oList.rows[i].selected) {
                oList.rows[i].selected = false;
                ELB_Rollout(oList.rows[i]);
                }
            }
        }
    else if(ELB_listMode[ctlID] == 1) {
        for(var i=0;i<oList.rows.length;i++) {
            if(oList.rows[i].selected) {
                ELB_SelectItem(ctlID, oList.rows[i].id);
                ELB_Rollout(oList.rows[i]);
                }
            }
//        if(getObj(ctlID + "_listHead").style.display != "none") { getObj(ctlID + "_listHead")
        }
    else {
        var selIndex = parseInt(getObj(ctlID + "_SelectedIndex").value);
        var myListHead = getObj(ctlID + "_listHead");
        if(myListHead == null) return;
        var displayMode = getObj(ctlID + "_listHead").displayMode;
        if(displayMode == "combo") { getObj(ctlID + "_comboText").value = "" }
        else if(oHead.clipHead == "true") { getObj(ctlID + "_clipText").innerText = ""; }
        else { getObj(ctlID + "_listHeadText").innerText = "" }
        if(selIndex > -1) {
            var tempRow = oList.rows[selIndex];
            tempRow.selected = false;
            ELB_Rollout(tempRow);
            }
        getObj(ctlID).value = "";
        getObj(ctlID + "_SelectedIndex").value = -1;
        getObj(ctlID + "_SelectedText").value = "";
        getObj(ctlID + "_SelectedText2").value = "";
        getObj(ctlID + "_SelectedText3").value = "";

        }
    if((oHead.promptText != "") && (oHead.displayMode != "combo")) { getObj(ctlID + "_listHeadText").innerText = oHead.promptText; }
    }

function ELB_SelectAll(ctlID) {
    var oList = getObj(ctlID + "_dropListTable");
    if(ELB_listMode[ctlID] == 1) {
        for(var i=1;i<oList.rows.length;i++) {
            if(!oList.rows[i].selected) {
                ELB_SelectItem(ctlID, oList.rows[i].id);
                ELB_Rollover(oList.rows[i]);
                }
            }
        }
    }

function ELB_FocusHead(ctlID) {
    var oHeadText = getObj(ctlID + "_listHeadText");
    var oHead = getObj(ctlID + "_listHead");
    currentKeyDown = 0;

    if(ctlID != sSpeedSearchID) { sSpeedSearchID = ctlID; sSpeedSearch = ""; }

    if(oHead.displayMode == "combo") {
        currentKeyDown = 0;
		getObj(ctlID + "_comboText").select();
        }
    else {
        oHeadText.style.color = ELB_ForeColorRoll[ctlID];
        oHeadText.parentElement.parentElement.style.background = ELB_BackColorRoll[ctlID];
        }
    }

function ELB_GetValue(ctlID) {
    return getObj(ctlID).value;
    }

function ELB_SelectValue(ctlID, sVal) {
    var tblList = getObj(ctlID + "_dropListTable");
    var rowName;
    var rowVal;
    for(var i=1; i<tblList.rows.length;i++) {
        rowName = ctlID + "_Row" + i;
        rowVal = tblList.rows[i].selectvalue;
        if(rowVal == sVal) {
            ELB_SelectItem(ctlID, rowName);
            return true;
            }
        }
        return false;
    }

function ELB_SelectText(ctlID, sText) {
    var tblList = getObj(ctlID + "_dropListTable");
    var rowName;
    var rowText;
    for(var i=1; i<tblList.rows.length;i++) {
        rowName = ctlID + "_Row" + i;
        rowText = tblList.rows[i].cells[0].innerText;
        if(rowText == sText) {
            ELB_SelectItem(ctlID, rowName);
            return true;
            }
        }
        return false;
    }

function ELB_SelectItem(ctlID, rowID) { // Select an item for posting or just keeping around

    //document.all.myText.value += "\n" + ctlID + " - " + rowID;
    var noChange = getObj(ctlID).noChange;

    var binChange = false;
    var itemRow = getObj(rowID);

    if(rowID.indexOf("sC") > 0) { itemRow = itemRow.parentElement; } // in case of checkboxes

    rowID = itemRow.id;

    var selectIndex = parseInt(rowID.replace(ctlID + "_Row", ""));

    var changeProc = getObj(ctlID).myChange;

    var itemValue = getObj(rowID).selectvalue;
    if(itemValue.indexOf(sNoSelect) >= 0) { return; }

    var itemName;
    var selText;
    var oHead = getObj(ctlID + "_listHead");
    var displayMode = oHead.displayMode;
    var tblTemp = getObj(ctlID + "_dropListTable");

    var currentVal = getObj(ctlID);
    var currentText = getObj(ctlID + "_SelectedText");
    var currentText2 = getObj(ctlID + "_SelectedText2");
    var currentText3 = getObj(ctlID + "_SelectedText3");

    if(displayMode == "combo") { itemName = getObj(rowID).childNodes[0].innerText; selText = itemName; } // No formatting for combo box
    else { itemName = getObj(rowID).childNodes(0).innerHTML; selText = getObj(rowID).childNodes(0).innerText; }

     if(ELB_listMode[ctlID] == 1) { // Multiple
        var currentSelIndex = getObj(ctlID + "_SelectedIndex");

        if(!itemRow.selected) { // Add selected value
            if(currentVal.value != "") {
                currentSelIndex.value = currentSelIndex.value + selectIndex + ",";
                currentVal.value = currentVal.value + itemValue + ",";
                currentText.value = currentText.value + selText + ",";
                if(itemRow.cells[1]) { currentText2.value = currentText2.value + itemRow.cells[1].innerText + ","; }
                if(itemRow.cells[2]) { currentText3.value = currentText3.value + itemRow.cells[2].innerText + ","; }
                }
            else {
                currentSelIndex.value = "," + selectIndex + ",";
                currentVal.value = "," + itemValue + ",";
                currentText.value = "," + selText + ",";
                if(itemRow.cells[1]) { currentText2.value = "," + itemRow.cells[1].innerText + ","; }
                if(itemRow.cells[2]) { currentText3.value = "," + itemRow.cells[2].innerText + ","; }
                }
            itemRow.selected = true;
            if(oHead.showCheckBoxes != "true") { ELB_Rollover(itemRow, true); }
            }
        else { // Remove selected value
            currentVal.value = currentVal.value.replace("," + itemValue, "");
            currentSelIndex.value = currentSelIndex.value.replace("," + selectIndex, "");
            currentText.value = currentText.value.replace("," + selText, "");
            if(itemRow.cells[1]) { currentText2.value = itemRow.cells[1].innerText; }
            if(itemRow.cells[2]) { currentText3.value = itemRow.cells[2].innerText; }
            itemRow.selected = false;
            ELB_Rollout(itemRow);
            }

        var sTitle = "";
        var cRows = tblTemp.rows;
        var i = 0;
        var binSelected = false;
        for(i=1;i<cRows.length;i++) {
            if(cRows[i].selected) {
                binSelected = true;
                sTitle = sTitle + "<br />" + cRows[i].cells[0].innerText;
                }
            }
        ELB_displaySelected(ctlID, sTitle);
        if(binSelected == false) {
            currentSelIndex.value = -1;
            currentVal.value = "";
            currentText.value = "";
            }

        if (window.event != null){
            var oEvent = document.createEventObject(window.event);
            if(oEvent.ctrlKey) {
                document.selection.empty();
                }
            }
        if(sSearch == ctlID) { getObj(ctlID + "_searchBox").focus() }
         else if(getObj(ctlID + "_dropList").style.visibility == "visible") { getObj(ctlID + "_dropList").focus() }
        }
    else { // Single, dropdown format
        if(rowID == "nothing") {
            getObj(ctlID).value = "";
            getObj(ctlID + "_SelectedIndex").value = -1;
            getObj(ctlID + "_SelectedText").value = "";
            return;
            }
        var prevRow = getObj(ctlID + "_dropListTable").rows[parseInt(getObj(ctlID + "_SelectedIndex").value)];
        if(prevRow) {
            prevRow.selected = false;
            ELB_Rollout(prevRow);
            }

        itemRow.selected = true;
        ELB_Rollover(itemRow, true);

        getObj(ctlID + "_SelectedIndex").value = selectIndex;
        getObj(ctlID).selectedIndex = selectIndex;

        if(itemValue != getObj(ctlID).value) {
            // Check for new value to trigger the onChange event
            binChange = true;
            }
        if(itemValue != "") {
            getObj(ctlID).value = itemValue;
            getObj(ctlID).text = selText;
            getObj(ctlID + "_SelectedText").value = selText;
            if(itemRow.cells[1]) { currentText2.value = itemRow.cells[1].innerText; }
            if(itemRow.cells[2]) { currentText3.value = itemRow.cells[2].innerText; }
            if(!noChange) {
                if(displayMode == "combo") {
                    getObj(ctlID + "_comboText").value = itemName;
                    }
                else {
                    if(oHead.clipHead == "true") {
                        getObj(ctlID + "_listHeadText").childNodes(0).innerHTML = itemName;
                        }
                    else {
                        getObj(ctlID + "_listHeadText").innerHTML = itemName;
                        }
                    }
                }
            }
        if(displayMode != "listbox" && window.event.keyCode != 38 && window.event.keyCode != 40) {
            // Only retract the list and focus the head if we didn't just do an up or down arrow (and if it's a dropdown/combo).
            ELB_retractList(ctlID);
            if(displayMode != "combo") {
                getObj(ctlID + "_listHeadText").focus();
                }
            else { selText = itemName; }
            }
        else if(displayMode == "listbox") {
            getObj(ctlID + "_dropList").focus();
            }
        getObj(ctlID + "_dropListTable").title = "Selected:  " + selText;
        oHead.title = "Selected:  " + selText;
        if(displayMode == "combo") { getObj(ctlID + "_comboText").title = "Selected:  " + selText; }
        }
    if(binChange) {
        eval(getObj(ctlID).myChange);
        }
    if(getObj(ctlID).autopostback && document.activeElement.id != (ctlID + "_comboText") && getObj(ctlID).value != getObj(ctlID + "_old").value) {
        //__doPostBack('myListBox','')
        if(getObj(ctlID).autopostback == true) { __doPostBack(getObj(ctlID).postbackname, ''); }
        }
    if(oHead.showCheckBoxes == "true") {
        if(itemRow.selected) {
            itemRow.cells[0].childNodes(0).checked = true;
            }
        else {
            itemRow.cells[0].childNodes(0).checked = false;
            }
        }

    if(ELB_ChildLists[ctlID]) { ELB_SetChildren(ELB_ChildLists[ctlID], itemValue); }

    if(tblTemp.swapTarget) { ELB_RollUp(ctlID); }

    if(noChange) {
        getObj(ctlID + "_SelectedIndex").value = 0;
        getObj(ctlID).selectedIndex = 0;
        getObj(ctlID).value = "";
        getObj(ctlID + "_SelectedText").value = "";

        getObj(ctlID).text = "";
        itemRow.selected = false;
        ELB_Rollout(itemRow);
        }
    }

function ELB_SetChildren(arrChildren, filterVal) {
    for(var iChild = 0;iChild < arrChildren.length;iChild++) {
        if(arrChildren[iChild] != "" && getObj(arrChildren[iChild])) {
            ELB_FillChildList(arrChildren[iChild], filterVal);
            }
        }
    }

function ELB_FillChildList(ctlID, filterVal) {
    //var sFilter = getObj(ctlID);
    var arrListItems = ELB_ChildListsItems[ctlID];
    var tblList = getObj(ctlID + "_dropListTable");
    var selMode = ELB_listMode[ctlID];
    var guideRow = tblList.rows[0];
    var guideCell = guideRow.cells[0];

    var arrItem;
    var rowTemp;
    var celTemp;

    var iChildRows = 0;
    var iSourceRows = 0;
    var iNewRowIndex = 0;
    var iCount = tblList.rows.length;

    if(tblList.rows.length > 1) { ELB_ClearSelection(ctlID); }

    for(iChildRows=iCount-1;iChildRows>0;iChildRows--) { tblList.deleteRow(iChildRows); }

    if(getObj(ctlID + "_listHead").displayMode == "dropdown") { tblList.rows[0].style.display = "block"; }

    for(iSourceRows=1;iSourceRows<arrListItems.length;iSourceRows++) {
        arrItem = arrListItems[iSourceRows];
        if(arrItem[0] == filterVal) {
            iNewRowIndex++;
            if(selMode == 0 && arrItem[1] == getObj(ctlID).value) { getObj(ctlID + "_SelectedIndex").value = iNewRowIndex; }
            rowTemp = tblList.insertRow();
            rowTemp.id = ctlID + "_Row" + iNewRowIndex;
            rowTemp.onmouseover = ELB_Rollover;
            rowTemp.onmouseout = ELB_Rollout;
            rowTemp.onmousedown = ELB_BeginDrag;

            rowTemp.selectvalue = arrItem[1];

            //alert("Current row display set to " + rowTemp.style.display);

            celTemp = rowTemp.insertCell();
            celTemp.mergeAttributes(guideCell);
            celTemp.id = ctlID + "_sC" + 0;

            celTemp.innerHTML = arrItem[2];
            celTemp.className = guideCell.className;
            celTemp.onblur = guideCell.onblur;

            if(arrItem[3]) {
                if(iNewRowIndex==1 && !tblList.rows[0].cells[1]) {
                    celTemp = tblList.rows[0].insertCell();
                    celTemp.mergeAttributes(guideCell);
                    celTemp.id = ctlID + "_sC" + 1;
                    }
                celTemp = rowTemp.insertCell();
                celTemp.mergeAttributes(guideCell);
                celTemp.id = ctlID + "_sC" + 1;
                celTemp.innerHTML = arrItem[3];
                }
            if(arrItem[4]) {
                if(iNewRowIndex==1 && !tblList.rows[0].cells[2]) {
                    celTemp = tblList.rows[0].insertCell();
                    celTemp.mergeAttributes(guideCell);
                    celTemp.id = ctlID + "_sC" + 2;
                    }
                celTemp = rowTemp.insertCell();
                celTemp.mergeAttributes(guideCell);
                celTemp.id = ctlID + "_sC" + 2;
                celTemp.innerHTML = arrItem[4];
                }
            }
        }
        tblList.rows[0].style.display = "none";
    }

function ELB_displaySelected(ctlID, strSelections) {
    var selectedList = strSelections;

    if(getObj(ctlID + "_displayField") && getObj(ctlID)) {
        var myDisplay = getObj(ctlID + "_displayField");
        if(strSelections) {
            if(selectedList.indexOf("<br />") == 0) { selectedList = selectedList.substr(6) }
            myDisplay.innerHTML = selectedList;
            }
        else {
            selectedList = "";
            var myRows = getObj(ctlID + "_dropListTable").rows;
            for(var iRow=1;iRow<myRows.length;iRow++) {
                if(myRows[iRow].selected && selectedList == "") { selectedList = myRows[iRow].cells[0].innerHTML
                    }
                else if(myRows[iRow].selected) { selectedList += "<br />" + myRows[iRow].cells[0].innerHTML; }
                }
            myDisplay.innerHTML = selectedList;
            }
        }
    else if(getObj(ctlID)) {
        var oHead = getObj(ctlID + "_listHead");
        getObj(ctlID + "_dropListTable").title = "Selected:\n" + selectedList.split("<br />").join("\n");
        oHead.title = getObj(ctlID + "_dropListTable").title;
        if(oHead.displayMode == "dropdown" && ELB_listMode[ctlID] == 1)  {
            var sNewText = getObj(ctlID + "_dropListTable").title.replace("Selected:\n\n", "").split("\n").join(",");
            if(sNewText.replace("Selected:,", "").length < 3) { sNewText = ""; }
            if(oHead.clipHead == "true") {
                getObj(ctlID + "_listHeadText").childNodes(0).innerHTML = sNewText;
                }
            else {
                getObj(ctlID + "_listHeadText").innerHTML = sNewText;
                }
            }
        }
    }

function ELB_checkActive(ctlID) {
    if(!ctlID) {
        ctlID = window.event.srcElement.id;
        ctlID = ctlID.substring(0, ctlID.indexOf("_"));
        }

    var theID = document.activeElement.id;
    var parentID = document.activeElement.parentElement.id
    var myListHead = getObj(ctlID + "_listHead");
    if(myListHead == null) return;
    var displayMode = getObj(ctlID + "_listHead").displayMode;
    var tblTemp = getObj(ctlID + "_dropListTable");
    if(ELB_listMode[ctlID] == 1) { document.selection.empty() }

    theID = "  element: " + theID + "   parent: " + parentID;

    if(theID.indexOf(sSpeedSearchID) < 0) { sSpeedSearch = ""; }

    /*if(displayMode == "combo" && theID.indexOf("comboText") >= 0) {
        }
    else*/
    if(theID.indexOf(ctlID) == -1 && theID.indexOf(ctlID + "_listHead") == -1) {
        var oHeadText = getObj(ctlID + "_listHeadText");
        var oHead = getObj(ctlID + "_listHead");
        var comboText = getObj(ctlID + "_comboText");

        if(oHead.displayMode == "combo") {
            if(comboText.limitToList == "true" && comboText.value != "" &&
                parseInt(getObj(ctlID + "_SelectedIndex").value) == -1) {
                alert(comboText.restrictionMessage);
                comboText.select();
                return;
                }
            else if(comboText.value != "" && parseInt(getObj(ctlID + "_SelectedIndex").value) == -1) {
                getObj(ctlID).value = comboText.value;
                if(getObj(ctlID).autopostback == true && (document.activeElement.id.indexOf(ctlID) < 0) && (getObj(ctlID).value != getObj(ctlID + "_old").value)) {
                    __doPostBack(ctlID, '');
                    }
                }
            else {
                if(getObj(ctlID).autopostback == true && (document.activeElement.id.indexOf(ctlID) < 0) && (getObj(ctlID).value != getObj(ctlID + "_old").value)) {
                    __doPostBack(ctlID, '');
                    }
                }
            comboText.style.color = ELB_ForeColor[ctlID]
            }
        else {
            oHeadText.style.color = ELB_ForeColor[ctlID];
            oHeadText.parentElement.parentElement.style.background = ELB_BackColor[ctlID];
            }
//        if(ELB_listMode[ctlID] == 0 && displayMode != "listbox") { // Single
        if(displayMode != "listbox") { // Listbox
            ELB_retractList(ctlID);
            }
		if(elbTip) { elbTip.style.visibility = "hidden"; }
        }
    else if(theID.indexOf("sC") >= 0) {
        ELB_SelectItem(ctlID, document.activeElement.parentElement.id);
        }
    }

function ELB_checkChange() {
    var sName = event.propertyName;
    var sID = "ELB";

    switch(sName) {
        case "selectedIndex":
            break;
        default:
            break;
        }
    }

function ELB_alterList(ctlID) { // Expand or retract the list, depending on its current state
    if(ELB_listState[ctlID] == 0) {
        ELB_expandList(ctlID);
        }
    else {
        ELB_retractList(ctlID);
        }
    }

function ELB_hitSelects(sStatus) {
    for(var iCollIndex = 0; iCollIndex < ELB_AllSelects.length; iCollIndex++) {
        //if(!confirm("Index = " + iCollIndex + "; Continue?")) { return; }
        ELB_PageSelects = ELB_AllSelects[iCollIndex];
        if(ELB_PageSelects) {
            for(var iSelectCount = 0;iSelectCount < ELB_PageSelects.length;iSelectCount++) {
                ELB_PageSelects[iSelectCount].style.visibility = sStatus;
                }
            }
        }
    }

var oPop = window.createPopup();
var oSpan;

function ELB_expandList(ctlID) {

    if(binGotSelects == false) { ELB_GetAllSelects(); }
    ELB_hitSelects("hidden");

    var listID = ctlID + "_dropList";
    var headID = ctlID + "_listHead";

    var oHead = getObj(headID);
    var oList = getObj(listID);
    var selIndex = parseInt(getObj(ctlID + "_SelectedIndex").value);

    var topCorrection = 0;
    var oParent = oHead.parentElement;

    if(oParent.tagName == "TD") {
        if(oParent.vAlign == "bottom") {
            //topCorrection = -(oParent.offsetHeight) + oHead.offsetHeight;
            topCorrection = oHead.offsetHeight;
            if(oParent.style.borderTopWidth) {
                 topCorrection += parseInt(oParent.style.borderTopWidth)*2 + parseInt(oHead.style.borderWidth);
                 }
            }
/*
        if(oParent.vAlign == "middle" || oParent.vAlign == "") {
            topCorrection = -(oParent.offsetHeight/2) + oHead.offsetHeight/2;
            if(oParent.style.borderTopWidth) {
                topCorrection += parseInt(oParent.style.borderTopWidth) + parseInt(oHead.style.borderWidth);
                }
            }
*/
        }

    oList.style.left = oHead.offsetLeft;
    oList.style.pixelTop = oHead.offsetTop + oHead.offsetHeight + topCorrection;
    oList.style.width = oHead.offsetWidth;
    oList.style.background = oHead.style.background;
    oList.style.zIndex = oHead.style.zIndex + 100;
/*
    var theParent = oHead;
    var posX = oHead.offsetLeft;
    var posY = oHead.offsetTop;

    while(theParent.offsetParent) {
        theParent = theParent.offsetParent;
        if(theParent.style.position.toLowerCase() == "relative") {
            posX += theParent.offsetLeft/2;
            posY += theParent.offsetTop/2;
            window.status = "relative: " + theParent.tagName;
            }
        else {
            posX += theParent.offsetLeft;
            posY += theParent.offsetTop;
            }
        //alert(theParent.tagName);
        }

    oList.style.pixelTop = posY + oHead.offsetHeight;
    oList.style.pixelLeft = posX;
// */    
    var listBottom = oList.style.pixelTop + oList.style.pixelHeight;
    var screenBottom = parseInt(document.body.clientHeight) + parseInt(document.body.scrollTop);
    var listTop = -(parseInt(oHead.offsetTop) - parseInt(oList.style.height));
//    window.status = listBottom + " " + screenBottom + " " + listTop;

    if((listBottom > screenBottom) && (listTop < (listBottom - screenBottom))) {
        oList.style.pixelTop = oHead.offsetTop - oList.style.pixelHeight + topCorrection;
        }

    if(selIndex > 0) {
        var selectedRow = getObj(ctlID + "_dropListTable").rows[selIndex];
        var selectedTop = selectedRow.offsetTop;
        var selectedHeight = parseInt(selectedRow.offsetHeight);
        oList.scrollTop = ((selIndex - 1) * selectedHeight);
        //if((selIndex * selectedHeight) > oList.style.pixelHeight
        }
    oList.style.visibility = "visible";
    //oList.focus();

    ELB_listState[ctlID] = 1;

/*
    oSpan = document.createElement("<SPAN>");
    oSpan.mergeAttributes(oList);
    oSpan.style.top = "0px";
    oSpan.style.left = "0px";
    
    oSpan.innerHTML = oList.innerHTML;
    oPop.document.body.innerHTML = oSpan.outerHTML.replace("ELB_", "window.parent.ELB_");
    //alert(oPop.document.body.innerHTML);
    oPop.document.body.style.border = oList.style.border;
    oPop.document.body.style.background = oList.style.background;
    //oPop.show(oList.style.pixelLeft + 2, oList.style.pixelTop, oList.offsetWidth, oList.offsetHeight, document.body);
    oPop.hide();
*/
//    window.status = oList.offsetLeft + "  " + oList.offsetTop + "  " + oList.offsetWidth + "  " + oList.offsetHeight;
    }

function ELB_retractList(ctlID) {
    ELB_hitSelects("visible");

    var listID = ctlID + "_dropList";
    var oList = getObj(listID);
    if(oList == null)
        return; 
    oList.style.zIndex = 0;
    oList.style.visibility = "hidden";
    ELB_listState[ctlID] = 0;
    if(document.activeElement.id.indexOf(ctlID) > -1 && document.activeElement.id.indexOf("_listHead") > 0) {
        var displayMode = getObj(ctlID + "_listHead").displayMode;
        if(displayMode == "dropdown") { getObj(ctlID + "_listHeadText").focus() }
        else if(displayMode == "combo") { getObj(ctlID + "_comboText").select() }
        }
    }

function ELB_BeginDrag(ctlID, rowTemp) {
    document.selection.empty();
    if(!rowTemp) { rowTemp = window.event.srcElement; }
    if(rowTemp.id.indexOf("_sC") >= 0) { rowTemp = rowTemp.parentElement; }
    if(!ctlID) { ctlID = rowTemp.id.substring(0, rowTemp.id.indexOf("_")); }
    if(ELB_listMode[ctlID] == 1) {
        aCurrentDrag = new Array();
        sCurrentDrag = ctlID;
        iCurrentDrag = rowTemp.rowIndex;
        aCurrentDrag[iCurrentDrag] = true;
        }
    }

function ELB_EndDrag(ctlID) {
    sCurrentDrag = "";
    iCurrentDrag = -1;
    aCurrentDrag = new Array();
    }

function ELB_DoDrag(ctlID, rowStyle) {
    //window.status = rowStyle.id;
    aCurrentDrag[rowStyle.rowIndex] = true;
    if(!rowStyle.selected) { ELB_SelectItem(ctlID, rowStyle.id) }
    aCurrentDrag[window.event.srcElement.rowIndex] = true;
    var upRow = getObj(ctlID + "_Row" + (rowStyle.rowIndex + 1));
    var downRow = getObj(ctlID + "_Row" + (rowStyle.rowIndex - 1));
    if(rowStyle.rowIndex == iCurrentDrag) { // It's the row where we started
        if(upRow && upRow.selected) {
            ELB_SelectItem(ctlID, upRow.id);
            }
        else if(downRow && downRow.selected) {
            ELB_SelectItem(ctlID, downRow.id);
            }
        }
    else if(rowStyle.rowIndex > iCurrentDrag) { // We're below the start row on the page
        for(var i=iCurrentDrag;i<rowStyle.rowIndex;i++) {
            var rowTemp = getObj(ctlID + "_Row" + i);
            if(!rowTemp.selected && rowTemp.selectvalue.indexOf(sNoSelect) < 0) {
                ELB_SelectItem(ctlID, rowTemp.id);
                ELB_Rollover(rowTemp, true);
                aCurrentDrag[rowTemp.rowIndex] = true;
                }
            }
        if(upRow && upRow.selected) {
            if(aCurrentDrag[upRow.rowIndex]) {
                ELB_SelectItem(ctlID, upRow.id);
                aCurrentDrag[upRow.rowIndex] = false;
                }
            }
        }
    else if(rowStyle.rowIndex < iCurrentDrag) { // We're above the start row on the page
        for(var i=iCurrentDrag;i>rowStyle.rowIndex;i--) {
            var rowTemp = getObj(ctlID + "_Row" + i);
            if(!rowTemp.selected && rowTemp.selectvalue.indexOf(sNoSelect) < 0) {
                ELB_SelectItem(ctlID, rowTemp.id);
                ELB_Rollover(rowTemp, true);
                aCurrentDrag[rowTemp.rowIndex] = true;
                }
            }
        if(downRow && downRow.selected) {
            if(aCurrentDrag[downRow.rowIndex]) {
                ELB_SelectItem(ctlID, downRow.id);
                aCurrentDrag[downRow.rowIndex] = false;
                }
            }
        }
    ELB_checkScroll(ctlID);
    }

var iScrollCount = 0;

function ELB_checkScroll(ctlID) {
    if(sCurrentDrag == ctlID) {
        var oHead = getObj(ctlID + "_listHead");
        var oList = getObj(ctlID + "_dropList");
        var oTable = getObj(ctlID + "_dropListTable");

        var iToScrollUp = oTable.rows[1].offsetHeight;
        var iToScrollDown = (oList.style.pixelHeight - oTable.rows[1].offsetHeight);
        if(ELB_listMode[ctlID] == 1 && oHead.style.display != "none") {
            iToScrollUp = oList.style.pixelTop - 2*oHead.offsetHeight + parseInt(document.body.scrollTop);
            iToScrollDown = oList.style.pixelTop + oList.style.pixelHeight - oHead.offsetHeight - parseInt(document.body.scrollTop);
            }
//        window.status = "top: " + iToScrollUp + "; bottom: " + iToScrollDown + "; mouse: " + window.event.y;
        if(window.event.y < iToScrollUp) {
            if(iScrollCount > -1) { iScrollCount = -1; }
            else { oList.doScroll("scrollBarUp"); iScrollCount = 0; }
            }
        else if(window.event.y > iToScrollDown) {
            if(iScrollCount < 1) { iScrollCount = 1; }
            else { oList.doScroll("scrollBarDown"); iScrollCount = 0; }
            }
        }
    document.selection.empty();
    }

function ELB_Rollover(rowStyle, binRedundant) {
    if(!rowStyle) { rowStyle = window.event.srcElement; }
    while(rowStyle.id.indexOf("_Row") < 0) { rowStyle = rowStyle.parentElement; }
    var ctlID = rowStyle.parentElement.parentElement.id.replace("_dropListTable", "");

    if((ctlID == sCurrentDrag) && !binRedundant) { ELB_DoDrag(ctlID, rowStyle) }

    rowStyle.style.background = ELB_BackColorRoll[ctlID];
    for(var iRow=0;iRow<=3;iRow++) {
        if(rowStyle.childNodes[iRow]) {
            rowStyle.childNodes[iRow].style.color = ELB_ForeColorRoll[ctlID];
            }
        }
    if(window.event) { window.event.cancelBubble = true }
    }

function ELB_Rollout(rowStyle) { // Restore an option after highlighting
    if(!rowStyle) { rowStyle = window.event.srcElement; }
    while(rowStyle.id.indexOf("_Row") < 0) { rowStyle = rowStyle.parentElement; }
    var ctlID = rowStyle.parentElement.parentElement.id.replace("_dropListTable", "");
    var iCurrent = rowStyle.id.replace(ctlID + "_Row");
    var oHead = getObj(ctlID + "_listHead");

    if(!rowStyle.selected || oHead.showCheckBoxes == "true") {
        // If the row isn't selected, roll it back
        rowStyle.style.background = ELB_BackColor[ctlID];
        for(var iRow=0;iRow<=3;iRow++) {
            if(rowStyle.childNodes[iRow]) {
                rowStyle.childNodes[iRow].style.color = ELB_ForeColor[ctlID];
                }
            }
        }
    if(window.event) { window.event.cancelBubble = true; }
    }

function ELB_ScrollList(oList) {
    if(window.event.wheelDelta < 0) { // Scroll the list by the wheel
        oList.doScroll("scrollBarDown");
        }
    else {
        oList.doScroll("scrollBarUp");
        }
    window.event.returnValue = false; // Keep from scrolling the window
    }

// End EasyListBox script
//</script>
