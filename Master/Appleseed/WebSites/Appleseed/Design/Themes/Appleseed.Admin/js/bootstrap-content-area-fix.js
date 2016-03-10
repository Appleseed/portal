// JavaScript source code
$(document).ready(function () {
    $("#leftPane, #contentpane, #rightpane").removeAttr('style');

    var leftHasContent, centerHasContent, rightHasContent;
    
    if ($("#leftpane").html() != undefined && $("#leftpane").html().length > 0) {
        leftHasContent = true;
    }
    else {
        leftHasContent = false;
    }

    if ($("#contentpane").html() != undefined && $("#contentpane").html().length > 0) {
        centerHasContent = true;
    }
    else {
        centerHasContent = false;
    }

    if ($("#rightpane").html() != undefined && $("#rightpane").html().length > 0) {
        rightHasContent = true;
    }
    else {
        rightHasContent = false;
    }

    /*if (leftHasContent == true && centerHasContent == true && rightHasContent == true) {
        $('.LeftCol').addClass('col-md-4');
        $('.CenterCol').addClass('col-md-4');
        $('.RightCol').addClass('col-md-4');
    }
    else if (leftHasContent == true && centerHasContent == true && rightHasContent == false){
        $('.LeftCol').addClass('col-md-6');
        $('.CenterCol').addClass('col-md-6');
    }
    else if (leftHasContent == false && centerHasContent == true && rightHasContent == true){
        $('.CenterCol').addClass('col-md-6');
        $('.RightCol').addClass('col-md-6');
    }
    else if (leftHasContent == true && centerHasContent == false && rightHasContent == true){
        $('.LeftCol').addClass('col-md-4');
        $('.CenterCol').addClass('col-md-4');
        $('.RightCol').addClass('col-md-4');
    }
    else if (leftHasContent == false && centerHasContent == true && rightHasContent == false) {
        $('.CenterCol').addClass('col-md-12');
    }
    else if (leftHasContent == true && centerHasContent == false && rightHasContent == false) {
        $('.LeftCol').addClass('col-md-12');
    }
    else if (leftHasContent == false && centerHasContent == false && rightHasContent == true) {
        $('.RightCol').addClass('col-md-12');
    }*/
});