/* Write here your custom javascript codes */
$('.scrollbar-outer').scrollbar();

$("ul.ks-cboxtags input.kschoiceall").change(function(){
    $(this).parent("li").nextAll("li").children("input[type=checkbox]").prop('checked', $(this).prop("checked")); //change all ".checkbox" checked status
});

$("ul.ks-cboxtags").change(function(){
    var listnum = $(this).children("li").children("input").not('.kschoiceall').length;
    var hascheck = $(this).children("li").children("input:checked").not('.kschoiceall').length;
    //console.log(hascheck);
    if(hascheck < listnum)
    {
        $(this).children("li").children("input[class=kschoiceall]").prop("checked", false);
    }
    else
    {
        $(this).children("li").children("input[class=kschoiceall]").prop("checked", true);
    }


});
$("#switchsearchblock").hide();
$(".itemcontrolbtn").click(function () {
    if ($(this).html() == "Close") {
        $(this).html("Maintain");
        $("#switchsearchblock").hide();
    }
    else {
        $(this).html("Close");
        $("#switchsearchblock").show();

    }
    //切換子項顯示與隱藏
    //$("#switchsearchblock").show();
    //文字切換  ?:運算式是if else的快捷方式
    //$(this).text($(this).text() == 'Open' ? 'Close' : 'Open');
});
