//<script language="Javascript" type="text/javascript">
/* 
This script is part of the EasyListBox server control.
Purchase and licensing information can be found at EasyListBox.com.
*/
var ELB_ChildLists = new Array();
var ELB_ChildListsItems = new Array();
var ELB_SelectedValues = new Array();
var ELB_Attributes = new Array();
var ELB_Selected = new Array();

function ELB_FilterCombo(ctlID) {
	var sFilter = comboText.value;
	}

function ELB_FilterMultiSelect(ctlID) {

	var oList = getObj(ctlID);
	var oSearch = getObj(ctlID + "_searchBox");
	var oTemp;
	var sText = oSearch.value.toLowerCase();

	if(sText != "") {
		for(var i=0;i<oList.options.length;i++) {
			oTemp = oList.options[i];
			if(oTemp.text.toLowerCase().indexOf(sText) > -1){
				oTemp.selected = true;
				}
			}
		ELB_OnChange(ctlID);
		}
	}

function getObj(ctlID) {
	var oTemp; // Pointer to the object (for browser compatibility)
	var i = 0;
	var j = 0;

	if(document.getElementById) {
		oTemp = document.getElementById(ctlID);
		}
	else {
		for(i=0;i<document.forms.length;i++) {
			var oForm = document.forms[i];
			for(j=0;j<document.forms[i].elements.length;j++) {
				var oElement = document.forms[i].elements[j];
				if(oElement.name == ctlID) { oTemp = oElement; }
				}
			}
		}
	if(oTemp) { return oTemp; }
    }

function ELB_focus(ctlID) {
	getObj(ctlID).focus();
	}

function ELB_init(ctlID, autopostback, selMode, childLists, parentList, swapTarget) {
	var arrSelections = new Array();
	var elbTemp = getObj(ctlID);

	ELB_Attributes[ctlID] = new Array();
	ELB_Attributes[ctlID]["autopostback"] = autopostback;

	if(swapTarget) {
		ELB_Attributes[ctlID]["swapTarget"] = true;
		ELB_RollUp(ctlID);
		}
	else if(parentList != "" && getObj(parentList) && getObj(parentList).selectedIndex > -1) {
		var valTemp = getObj(ctlID + "_SelectedValue").value;
		if(selMode == 1) {
			var sTemp;
			var iTemp;
			while(valTemp.length > 0) {
				valTemp = valTemp.substring(1, valTemp.length);
				iTemp = valTemp.indexOf(",");
				sTemp = valTemp.substring(0, iTemp);
				arrSelections[sTemp] = true;
				valTemp = valTemp.substring(iTemp, valTemp.length);
				if(valTemp == ",") { valTemp = ""; }
				}
			ELB_Selected[ctlID] = arrSelections;
			}
		ELB_FillChildList(ctlID, getObj(parentList).options[getObj(parentList).selectedIndex].value, true);
		if(selMode == 0) { 
			elbTemp.selectedIndex = parseInt(valTemp);
			ELB_OnChange(ctlID, true);
			}
		}

	if(selMode == 1) {
		for(var iList=0;iList<elbTemp.options.length;iList++) {
			var theOpt = elbTemp.options[iList];
			if(theOpt.selected) {
				arrSelections[iList] = true;
				}
			}
		ELB_Selected[ctlID] = arrSelections;
		ELB_displaySelected(ctlID, arrSelections);
		}

	if(childLists) {
		var selIndex = elbTemp.selectedIndex;
		var itemValue = selIndex == -1 ? "aybabtuaybabtuaybabtu" : elbTemp.options[selIndex].value;

		ELB_ChildLists[ctlID] = childLists;
		ELB_SetChildren(childLists, itemValue, true);
		}
    }

function ELB_GetValue(ctlID) {
	var tmpList = getObj(ctlID);
	if(tmpList.multiple) {
		var sVal;
		for(var i=0; i < tmpList.options.length; i++) {
			if(tmpList.options[i].selected) { sVal += "," + tmpList.options.value; }
			}
		tmpList = tmpList.substring(1, tmpList.length - 1);
		}
	else {
		return tmpList.options[tmpList.selectedIndex].value;
		}
	}

function ELB_OnChange(ctlID, binInit) {
	var elbTemp = getObj(ctlID);
    var theIndex = elbTemp.selectedIndex;
    var sValue = "";

    if(theIndex == -1) { itemValue = "aybabtuaybabtuaybabtu"; }
	else {var itemValue = elbTemp.options[theIndex].value; }

	if(ELB_Selected[ctlID]) {
		var arrSelections = ELB_Selected[ctlID];
		var iSelected = 0;
		sValue = ",";

		if(!binInit) {
			for(var iList=0;iList<elbTemp.options.length;iList++) {
				var theOpt = elbTemp.options[iList];
				if(theOpt.selected) {iSelected++}
				}
			if(iSelected == 1) {
				if(arrSelections[theIndex]) {
					arrSelections[theIndex] = false;
					elbTemp.options[theIndex].selected = false;
					}
				else {
					arrSelections[theIndex] = true;
					}
				}
			}

		for(var iList=0;iList<elbTemp.options.length;iList++) {
			var theOpt = elbTemp.options[iList];
			if(arrSelections[iList] == true || theOpt.selected == true) {
				arrSelections[iList] = true;
				theOpt.selected = true;
				sValue = sValue + (iList + ",");
				}
			else {
				arrSelections[iList] == false;
				theOpt.selected = false;
				}
			}

		getObj(ctlID + "_SelectedValue").value = sValue;

		if(theIndex == -1) {
			}
		else {
			theOpt = elbTemp.options[theIndex];
			if(arrSelections[theIndex]) {theOpt.selected = false; theOpt.selected = true;}
			else {theOpt.selected = true; theOpt.selected = false; }

			ELB_Selected[ctlID] = arrSelections;
			ELB_displaySelected(ctlID, ELB_Selected[ctlID]);
			}
		}
	else { 
		getObj(ctlID + "_SelectedValue").value = itemValue;
		getObj(ctlID + "_SelectedIndex").value = theIndex;
		}
	if(ELB_Attributes[ctlID]["swapTarget"]) { ELB_RollUp(ctlID); }
	if(ELB_ChildLists[ctlID]) { ELB_SetChildren(ELB_ChildLists[ctlID], itemValue); }
	eval(getObj(ctlID + "_OnChange").value);
	if(ELB_Attributes["autopostback"] == true) {
		__doPostBack(ctlID, '');
		}
	//__doPostBack('theSimpsons','')
	//;
	}

function ELB_SetChildren(arrChildren, filterVal, binInit) {
	for(var iChild = 0;iChild < arrChildren.length;iChild++) {
		if(arrChildren[iChild] != "" && getObj(arrChildren[iChild])) {
			ELB_FillChildList(arrChildren[iChild], filterVal, binInit);
			}
		}
    }

function ELB_FillChildList(ctlID, filterVal, binInit) {

	var sFilter = getObj(ctlID);
	var arrListItems = ELB_ChildListsItems[ctlID];
	var arrItem;
    var iSourceRows = 0;
	var iCount;
	var sText;

	if(!binInit) { ELB_ClearSelection(ctlID); }
	
	sFilter.options.length = 0;

	if(filterVal == "aybabtuaybabtuaybabtu") { return; }

	for(iSourceRows=1;iSourceRows<arrListItems.length;iSourceRows++) {
		arrItem = arrListItems[iSourceRows];

		if(arrItem[0] == filterVal) {
			iCount = sFilter.options.length;
			sFilter.options.length++;
			sFilter.options[iCount].value = arrItem[1];
			sText = arrItem[2];
			if(arrItem[3]) { sText += " " + arrItem[3]; }
			if(arrItem[4]) { sText += " " + arrItem[4]; }
			sFilter.options[iCount].text = sText;
			}
		}

	ELB_OnChange(ctlID, binInit);
	}

function ELB_SwapItem(sourceID, targetID) {
	var selSource = getObj(sourceID);
	var selTarget = getObj(targetID);
	var optSource;
	var optTarget;

	ELB_ClearSelection(targetID);

	for(var i=0;i<selSource.options.length;i++) {
		optSource = selSource.options[i];
		if(optSource.selected) {
			selTarget.options.length++;
			optTarget = selTarget.options[selTarget.options.length - 1];
			optTarget.value = optSource.value;
			optTarget.text = optSource.text;
			optTarget.selected = true;
			optSource.selected = false;
			ELB_RemoveItem(sourceID, i);
			i--;
			}
		}
	if(ELB_Attributes[sourceID]["swapTarget"]) { ELB_RollUp(sourceID); }
	if(ELB_Attributes[targetID]["swapTarget"]) { ELB_RollUp(targetID); }
	}

function ELB_MoveItem(ctlID, moveIndex) {
	var oList = getObj(ctlID);
	var iMin = -1;
	var iMax = -1;
	var currentIndex = 0;
	var arrSelections = ELB_Selected[ctlID];
	var binComplete = false;

	if(!oList.multiple) {
		iMin = oList.selectedIndex;
		iMax = iMin;
		}
	else { 
		for(iCurrent=0; iCurrent < oList.options.length; iCurrent++) {
			if(iMin == -1 && ELB_Selected[ctlID][iCurrent]) { iMin = iCurrent; }
			else if(iMin != -1 && ELB_Selected[ctlID][iCurrent]) { iMax = iCurrent; }
			}
		if(iMin == -1) { return; }
		if(iMax == -1) { iMax = iMin; }
		}

		iCurrent = (moveIndex < 0) ? 0 : oList.options.length - 1;

		while(!binComplete) {
			if(oList.options[iCurrent].selected) {
				var oCurrent = oList.options[iCurrent];
				if((iCurrent + moveIndex) > -1) { var oNext = oList.options[iCurrent + moveIndex]; }
				if(!oNext) { }
				else if(!oNext.selected) {
					var valTemp = oNext.value;
					var txtTemp = oNext.text;
					var selTemp = oNext.selected;

					oNext.value = oCurrent.value;
					oNext.text = oCurrent.text;
					oNext.selected = oCurrent.selected;

					oCurrent.value = valTemp;
					oCurrent.text = txtTemp;
					oCurrent.selected = selTemp;
					arrSelections[iCurrent] = oCurrent.selected;
					arrSelections[iCurrent + moveIndex] = oNext.selected;
					}
				}
			iCurrent -= moveIndex;
			if(moveIndex < 0 && (iCurrent > iMax || iCurrent == oList.options.length)) { binComplete = true; }
			else if(moveIndex > 0 && (iCurrent < iMin || iCurrent == -1)) { binComplete = true; }
			}

	ELB_RollUp(ctlID);
	}

function ELB_RemoveItem(ctlID, theIndex) {
	var oList = getObj(ctlID);
	var oCurrent;
	var oNext;

	for(i=theIndex;i<oList.options.length-1;i++) {
		oCurrent = oList.options[i];
		oNext = oList.options[i+1];
		oCurrent.value = oNext.value;
		oCurrent.text = oNext.text;
		oCurrent.selected = oNext.selected;
		}
	oList.options.length--;
	}

function ELB_RollUp(ctlID) {
	var sValue = "";
	var sText = "";
	var oList = getObj(ctlID);
	var oCurrent;

	for(var iRow=0;iRow<oList.options.length;iRow++) {
		oCurrent = oList.options[iRow];
		sText += oCurrent.text;
		sValue += oCurrent.value;
		if(sValue != "" && iRow != (oList.options.length - 1)) { 
			sValue += "|"; 
			sText += "|";
			}
		if(ELB_Selected[ctlID]) {
			if(oCurrent.selected) { ELB_Selected[ctlID][iRow] = true; }
			else { ELB_Selected[ctlID][iRow] = false; }
			}
		}
	if(ELB_Selected[ctlID]) { ELB_Selected[ctlID].length = iRow + 1; }
	getObj(ctlID + "_SelectedValue").value = sValue;
	getObj(ctlID + "_SelectedText").value = sText;
	}

function ELB_ClearSelection(ctlID, binInit) {
	var oList = getObj(ctlID);

	var arrSelections = new Array();
	arrSelections = ELB_Selected[ctlID];

	if(oList.multiple) {
		for(var i=0;i<oList.options.length;i++) {
			oList.options[i].selected = false;
			arrSelections[i] = false;
			}
		ELB_displaySelected(ctlID, arrSelections);
		ELB_Selected[ctlID] = arrSelections;
		}
	else {
		oList.selectedIndex = -1;
		}
	}

function ELB_SelectAll(ctlID) {
	var oList = getObj(ctlID);
	var arrSelections = new Array();
	arrSelections = ELB_Selected[ctlID];

	if(oList.multiple) {
		for(var i=0;i<oList.options.length;i++) {
			oList.options[i].selected = true;
			if(!ELB_Attributes[ctlID]["swapTarget"]) { arrSelections[i] = true; }
			}
		}
	else {
		oList.selectedIndex = 0;
		}
	ELB_Selected[ctlID] = arrSelections;
	ELB_displaySelected(ctlID, ELB_Selected[ctlID]);
	}

function ELB_displaySelected(ctlID, arrSelected) {
	if(getObj(ctlID + "_displayField")) { 
		var selectionList = "";

		if(arrSelected) {
			if(getObj(ctlID + "_displayField")) {
				for(var i=0;i<arrSelected.length;i++) {
					if(arrSelected[i] && selectionList.length > 1) {
						selectionList += "\n" + getObj(ctlID).options[i].text;
						}
					else if(arrSelected[i]) {
						selectionList += getObj(ctlID).options[i].text;
						}
					}
				getObj(ctlID + "_displayField").value = selectionList;
				}
			}
		else if(getObj(ctlID)){ 
			var oList = getObj(ctlID);
			for(var i=0;i<oList.options.length;i++) {
				if(oList.options[i].selected) {
					selectionList = (selectionList == "") ? oList.options[i].text : selectionList + "\n" + oList.options[i].text;
					}
				}
			getObj(ctlID + "_displayField").value = selectionList;
			}
		}
	}

function ELB_FilterCombo(ctlID, theVal) {
	var theList = getObj(ctlID);

	if(window.event) {
		if(window.event.keyCode) {
			var theCode = window.event.keyCode;
			if((theCode > 96 && theCode < 122) || (theCode > 64 && theCode < 91)) {
				for(var i=0;i<theList.options.length;i++) {
					if(theList.options[i].text.toLowerCase().indexOf(theVal.toLowerCase()) == 0) {
						theList.options[i].selected = true;
						i = theList.options.length;
						ELB_OnChange(ctlID);
						}
					}
				}
			}
		}
	else {
		for(var i=0;i<theList.options.length;i++) {
			if(theList.options[i].text.toLowerCase().indexOf(theVal.toLowerCase()) == 0) {
				theList.options[i].selected = true;
				i = theList.options.length;
				ELB_OnChange(ctlID);
				}
			}
		}
	}

function ELB_CheckFilter(evt) {
	//alert(evt.charCode);
	if(evt.charCode == 13) { 
		//alert("It's the enter key");
		var targetList = evt.target.id;
		var ctlID = targetList.substring(0, targetList.indexOf("_"));
		ELB_FilterMultiSelect(ctlID);
		this.focus();
		evt.cancelBubble = true; 
		}
	}

// End EasyListBox script
//</script>