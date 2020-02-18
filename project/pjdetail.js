/*pageList*/

/*===============*/
/*init*/
var __isFirstLoad = 1;


jQuery(document).ready(function () {
    $("#WebsiteDesc").attr("title", "<div style='text-align:left;'>．Monitored websites include: cnet, zdnet, RD World Online, CompositesWorld<br>．Automatically search all articles with the \"technology item\" indicated in this project, scored by your personal preference</div>");
    $("#WebsiteDesc").tooltip({
        placement: 'top'
    });

    // 文字雲
    getCloudSearchData();

    // Change Schedule
    $(document).on("click", "button[name='ScheBtn'],button[name='NScheBtn']", function (e) {
        var btn_obj = $(this);
        var RSche = "";
        if (btn_obj.attr("name") == "ScheBtn") {
            RSche = "0";
            btn_obj.attr("name", "NScheBtn");
            btn_obj.text("not schedule");
            btn_obj.removeClass("btn-u-purple");
            btn_obj.addClass("btn-u-dark");
        }
        else {
            RSche = "1";
            btn_obj.attr("name", "ScheBtn");
            btn_obj.text("schedule");
            btn_obj.removeClass("btn-u-dark");
            btn_obj.addClass("btn-u-purple");
        }

        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "ChangeSchedule.aspx",
            data: {
                r_guid: $(this).attr("aid"),
                r_sche: RSche
            },
            error: function (xhr) { alert(xhr.responseText); },
            success: function (data) {
                if ($(data).find("Error").length > 0) {
                    alert($(data).find("Error").attr("Message"));
                }
                else {
                }
            }
        });
    });

    $(document).on("click", "input[name='cbResource'],input[name='cbTopic'],input[name='cbDate'],input[name='cbTag']", function () {
        getData(0);
    });

    /// 撈出查詢條件
    getResources();
});

/*===tag select*/

function doTagSelect(a, b) {
    var dataObj = {};
    dataObj["pjid"] = a;
    dataObj["arcid"] = b;
    $.ajax({
        url: "getTagSelect.aspx"
        , type: "post"
        , dataType: 'html'
        , cache: false
        , async: true
        , data: dataObj
        , error: function () { alert("tag select error"); }
        , success: function (data, ts, xhr) {
            $("#tagSelect_dataBlock").html(data);
            $("#myTemplate_tagSelect").modal('show');
        }
    });

}

function doTagSelect_save() {

    var cbSelected = "";
    var cbPartObjs = $("#tagSelect_dataBlock").find("input[type=checkbox]:checked");
    for (var i = 0; i < cbPartObjs.length; i++) {
        cbSelected += $(cbPartObjs[i]).val() + ",";
    }
    cbSelected = cbSelected.substring(0, cbSelected.length - 1);


    var dataObj = {};
    dataObj["pjid"] = $("#pjid").val();
    dataObj["arcid"] = $("#arcid").val();
    dataObj["cbSelected"] = cbSelected;
    $.ajax({
        url: "setTagSelectSave.aspx"
        , type: "post"
        , dataType: "html"
        , cache: false
        , async: true
        , data: dataObj
        , error: function () { alert("tag save error"); }
        , success: function (data, ts, xhr) {
            if (data == "OK.") {
                $("#myTemplate_tagSelect").modal('hide');
                getResources();
                getData(0);
            }
            else {
                alert(data);
            }
        }
    });
}

function doTagSelect_cancel() {
    $("#myTemplate_tagSelect").modal('hide');
}


function doTagSelect_maintain() {
    doTagSelect_cancel();

    var dataObj = {};
    $.ajax({
        url: "getTagMaintain.aspx"
        , type: "post"
        , dataType: 'html'
        , cache: false
        , async: true
        , data: dataObj
        , error: function () { alert("tag maintain error"); }
        , success: function (data, ts, xhr) {
            $("#tagMaintain_dataBlock").html(data);
            $("#myTemplate_tagMaintain").modal('show');
        }
    });
}

/*############################################################################*/
/*===tag maintain*/

function doTagMaintain_cancel() {
    $("#myTemplate_tagMaintain").modal('hide');
}

function doTagMaintain_add() {
    var dataObj = {};
    dataObj["newTagName"] = $('#newTagName').val();
    $.ajax({
        url: "setTagMaintainAdd.aspx"
        , type: "post"
        , dataType: "html"
        , cache: false
        , async: true
        , data: dataObj
        , error: function () { alert("tag add error"); }
        , success: function (data, ts, xhr) {
            if (data == "OK.") {
                doTagSelect_maintain();
            }
            else {
                alert(data);
            }
        }
    });
}

function doTagMaintain_delete(a) {
    var dataObj = {};
    dataObj["tagid"] = a;

    $.ajax({
        url: "setTagMaintainDelete.aspx"
        , type: "post"
        , dataType: "html"
        , cache: false
        , async: true
        , data: dataObj
        , error: function () { alert("tag delete error"); }
        , success: function (data, ts, xhr) {
            if (data == "OK.") {
                doTagSelect_maintain();
            }
            else {
                alert(data);
            }
        }
    });
}


function getResources() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "GetResources.aspx",
        data: {
            ProjectGuid: $.getQueryString("pjGuid")
        },
        error: function (xhr) {
            alert(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                var ULstr = '<li><input type="radio" id="TopicRadio" value="all" name="cbTopic" checked="checked" /><label for="TopicRadio">All topics</label></li>';
                $(data).find("topic_item").each(function (i) {
                    if (i == 0)
                        ULstr += '<li><input type="radio" id="TopicRadio' + i + '" value="' + $(this).children("research_guid").text().trim() + '" name="cbTopic" /><label for="TopicRadio' + i + '">' + $(this).children("name").text().trim() + '</label></li>';
                    else
                        ULstr += '<li><input type="radio" id="TopicRadio' + i + '" value="' + $(this).children("research_guid").text().trim() + '" name="cbTopic" /><label for="TopicRadio' + i + '">' + $(this).children("name").text().trim() + '</label></li>';
                });
                $("#topic_ul").empty();
                $("#topic_ul").append(ULstr);

                var tagStr = '<li><input type="radio" id="TagRadio" value="all" name="cbTag" checked="checked" /><label for="TagRadio">No Tag</label></li>';
                $(data).find("mytag_item").each(function (i) {
                    if (i == 0)
                        tagStr += '<li><input type="radio" id="TagRadio' + i + '" value="' + $(this).children("tagtype_guid").text().trim() + '" name="cbTag" /><label for="TagRadio' + i + '">' + $(this).children("tagtype_name").text().trim() + '</label></li>';
                    else
                        tagStr += '<li><input type="radio" id="TagRadio' + i + '" value="' + $(this).children("tagtype_guid").text().trim() + '" name="cbTag" /><label for="TagRadio' + i + '">' + $(this).children("tagtype_name").text().trim() + '</label></li>';
                });
                $("#tag_ul").empty();
                $("#tag_ul").append(tagStr);
            }

            $('input[name="cbResource"][value="' + $.getQueryString("viewMode") + '"]').prop("checked", true);
            $('input[name="cbTopic"][value="' + $.getQueryString("researchGuid") + '"]').prop("checked", true);
            $('input[name="cbDate"][value="' + $.getQueryString("date0") + '"]').prop("checked", true);
            $('input[name="cbTag"][value="' + $.getQueryString("myTag") + '"]').prop("checked", true);
        }
    });
}






/*############################################################################*/
/*===btn:askCom,維護排程鍵*/
function doAskComManage() {
    $("#myBlock_allArticles").hide();
    $("#myBlock_articlesBySite").hide();
    $("#myBlock_askComData").hide();
    $("#myBlock_askComManage").show();

    $(".searchitem1").hide();
    $(".searchitem2").show();
}

/*===btn:askCom,結束維護排程鍵*/
function doEndAskComManage() {
    doDetailSearch();
    $(".searchitem1").show();
    $(".searchitem2").hide();
}

/*===btn:askCom,更新排程鍵*/
function doAskComUpdate(myobj) {
    $.ajax({
        url: "../projectMgmt/setRelatedWordSchedule.aspx",
        type: "get",
        dataType: "html",
        //////cache: false,
        //////async: true,
        data: {
            typeId: $(myobj).attr("typeId")
            , typeGuid: $(myobj).attr("typeGuid")
            , schedule: $(myobj).attr("schedule")
        },
        success: function (data) {
            if (data == "OK.") {
                setDataResult(data);
            }
            else {
                alert(data);
            }
        },
        error: function (jqXHR, textStatus, exception) {
            alert(jqXHR.responseText);
        }
    });

    function setDataResult(result) {
        var schedule = $(myobj).attr("schedule");

        if (schedule == "0")/*不納入改為納入*/ {
            $(myobj).attr("schedule", "1")
            $(myobj).find("i").removeClass("fa-heart-o")
            $(myobj).find("i").addClass("fa-heart")
        }
        else if (schedule == "1")/*納入改為不納入*/ {
            $(myobj).attr("schedule", "0")
            $(myobj).find("i").removeClass("fa-heart")
            $(myobj).find("i").addClass("fa-heart-o")
        }
    }
}