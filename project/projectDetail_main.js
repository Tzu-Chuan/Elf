/*pageList*/

/*===============*/
/*init*/
var __isFirstLoad = 1;


jQuery(document).ready(function () {
    $("#WebsiteDesc").attr("title", "<div style='text-align:left;'>．Monitored websites include: cnet, zdnet, RD World Online, CompositesWorld<br>．Automatically search all articles with the \"technology item\" indicated in this project, scored by your personal preference</div>");
    $("#WebsiteDesc").tooltip({
        placement: 'top'
    });

    /*設定最新參數*/
    $('#search_date0').val($('#def_date0').val());
    $('#search_viewMode').val($('#def_viewMode').val());
    $('#search_researchGuid').val($('#def_researchGuid').val());
    $('#search_myTag').val($('#def_myTag').val());

    getCloudSearchData();

    /*===點展開、收合時*/
    $(".collapse").on('show.bs.collapse', function () {
        //$(this).parent().find("a").removeClass("btn-u-default");
        //$(this).find("a").eq(0).addClass("btn-u-default");

        ///*===設參數*/
        var orderField = "score";
        var typeId = $(this).attr("typeId");
        var typeGuid = $(this).attr("typeGuid");

        if (typeId != undefined && typeGuid != undefined) {
            /*===取資料*/
            get_articleList("1", orderField, typeId, typeGuid);

            /*===設位置*/
            if (__isFirstLoad == "0")/*非第一次點選時*/ {
                /*===set scroll*/
                var $body = (window.opera) ? (document.compatMode == "CSS1Compat" ? $('html') : $('body')) : $('html,body');
                setTimeout(function () {
                    $body.animate({
                        scrollTop: $('#pageHead' + typeId + typeGuid).offset().top
                    }, 0);
                }, 500);
            }
            else if (__isFirstLoad == "1")/*第一次點選時(不要移動scroll)*/ {
                __isFirstLoad = "0";
            }
        }
    }).on('hide.bs.collapse', function () {
    });

    /*===事件：展開全部來源*/
    ////////$('#pageHead1all a').click();
    //$("#pageBlock1all").collapse('show');

    /*===事件：點選研究方向遮罩時*/
    $('#black_summary').on('shown.bs.modal', function (e) {
        /*get cloud*/
        getCloudSearchData();
    });


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
            url: "projectHandler/ChangeSchedule.aspx",
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
        doDetailSearch();
    });

    /// 撈出查詢條件
    getResources();

    /*===設定查詢結果*/
    set_searchList();
    get_articleList("1", "score", "1", "all");
});


/*############################################################################*/
/*function*/

//openAllPanels = function (aId)
//{
//    $(aId + ' .panel-collapse:not(".in")').collapse('show');
//}
//closeAllPanels = function (aId)
//{
//    $(aId + ' .panel-collapse.in').collapse('hide');
//}

/*===data:設定搜尋結果*/
function set_searchList() {
    /*取得最新參數*/
    var __pjGuid = $('#pjGuid').val();
    var __date0 = $('#search_date0').val();
    var __viewMode = $('#search_viewMode').val()
    var __researchGuid = $('#search_researchGuid').val();

    __date0 = $('input[name="cbDate"]:checked').val();
    __viewMode = $('input[name="cbResource"]:checked').val();
    __researchGuid = $('input[name="cbTopic"]:checked').val();

    /*設定固定來源瀏覽方式*/
    if (__viewMode == "all") {
        $("#myBlock_allArticles").show();
        $("#myBlock_articlesBySite").hide();
        $("#myBlock_askComData").show();
        $("#myBlock_askComManage").hide();
    }
    else {
        $("#myBlock_allArticles").hide();
        $("#myBlock_articlesBySite").show();
        $("#myBlock_askComData").show();
        $("#myBlock_askComManage").hide();
    }

    /*設定研究方向瀏覽方式*/
    if (__researchGuid == "all") {
        /*顯示所有研究方向*/
        $("#myBlock_askComData").find(".askCom_item").each(function () {
            $(this).show();
        });
    }
    else {
        /*顯示條件研究方向*/
        $("#myBlock_askComData").find(".askCom_item").each(function () {
            if ($(this).attr("researchGuid") != __researchGuid) {
                $(this).hide();
            }
            else {
                $(this).show();
            }
        });
    }

    /////////*重取目前展開的分頁list*/
    ////////var len = $(".panel-collapse.in").length;
    ////////var typeId = "";
    ////////var typeGuid = "";
    ////////for (i = 0; i < len; i++)
    ////////{
    ////////    typeId = $(".panel-collapse.in").eq(i).attr("typeId");
    ////////    typeGuid = $(".panel-collapse.in").eq(i).attr("typeGuid");

    ////////    ////alert("len=" + len
    ////////    ////    + "\n typeId=" + typeId
    ////////    ////    + "\n typeGuid=" + typeGuid
    ////////    ////    );

    ////////    if (__viewMode == "all" && (typeId == "1" || typeId == "3"))
    ////////    {
    ////////        get_articleList(1, typeId, typeGuid);
    ////////    }
    ////////    else if (__viewMode == "site" && (typeId == "2" || typeId == "3"))
    ////////    {
    ////////        get_articleList(1, typeId, typeGuid);
    ////////    }
    ////////}
}

/*===btn:搜尋鍵*/
function doDetailSearch() {
    /*取得最新參數*/
    var __pjGuid = $('#pjGuid').val();
    var __date0 = $('#search_date0').val();
    var __viewMode = $('#search_viewMode').val()
    var __researchGuid = $('#search_researchGuid').val();
    var __myTag = $('#search_myTag').val();

    __date0 = $('input[name="cbDate"]:checked').val();
    __viewMode = $('input[name="cbResource"]:checked').val();
    __researchGuid = $('input[name="cbTopic"]:checked').val();
    __myTag = $('input[name="cbTag"]:checked').val();

    location.href = "projectDetail.aspx?pjGuid=" + __pjGuid
        + "&date0=" + __date0
        + "&viewMode=" + __viewMode
        + "&researchGuid=" + __researchGuid
        + "&myTag=" + __myTag
        ;
}


/*===btn:換頁頁數鍵*/
function doDetailSearchByPage(curPage, orderField, typeId, typeGuid) {
    get_articleList(curPage, orderField, typeId, typeGuid);

    /*===set scroll*/
    var $body = (window.opera) ? (document.compatMode == "CSS1Compat" ? $('html') : $('body')) : $('html,body');
    {
        $body.animate({
            scrollTop: $('#pageHead' + typeId + typeGuid).offset().top
        }, 100);
    }
}

/*===btn:變更排序*/
function doChangeOrderField(obj, orderField, curPage, typeId, typeGuid) {
    $(obj).parent().find("a").removeClass("btn-u-default");
    if (orderField == "score")
        $("a[name='date']").addClass("btn-u-default");
    else
        $("a[name='score']").addClass("btn-u-default");
    get_articleList(curPage, orderField, typeId, typeGuid);
    $("#hidden_Order").val(orderField);
}

/*===data:取得分頁list結果*/
function get_articleList(curPage, orderField, typeId, typeGuid) {
    /*取得最新參數*/
    var __pjGuid = $('#pjGuid').val();
    var __date0 = $('#search_date0').val();
    var __viewMode = $('#search_viewMode').val()
    var __researchGuid = $('#search_researchGuid').val();
    var __myTag = $('#search_myTag').val();

    __date0 = $('input[name="cbDate"]:checked').val();
    __viewMode = $('input[name="cbResource"]:checked').val();
    __researchGuid = $('input[name="cbTopic"]:checked').val();
    __myTag = $('input[name="cbTag"]:checked').val();

    $('#pageData' + typeId + typeGuid + ' div').fadeTo(0, 0.3);
    $.ajax({
        url: "articleList.aspx",
        type: "POST",
        dataType: 'html',
        data: {
            currentPageIndex: curPage
            , orderField: orderField
            , pjGuid: __pjGuid
            , date0: __date0
            , viewMode: __viewMode
            , researchGuid: __researchGuid
            , myTag: __myTag
            , typeId: typeId
            , typeGuid: typeGuid
        },
        success: function (data, textStatus, jqXHR) {
            /*===set html result*/
            ////alert(data);
            $("#pageData" + typeId + typeGuid).html(data);
            //document.getElementById("pageData" + typeId+ typeGuid).innerHTML = data;


            /////////*===set iframe*/
            /////////*http://plnkr.co/edit/4XnLTZ557qMegqRmWVwU?p=preview*/
            /////////*https://segmentfault.com/q/1010000005085719*/
            ////////$('#articleContent').on('shown.bs.modal', function (e)
            ////////{
            ////////    //var id = e.relatedTarget.id;
            ////////    var myurl = $(e.relatedTarget).data("myurl");
            ////////    $(this).find('iframe').attr('src', myurl)
            ////////})
        },
        complete: function (jqXHR, textStatus) { xhr = null; },
        error: function (jqXHR, textStatus, exception) {
            //$('#post').html(msg);
            alert(jqXHR.responseText);
        }
    });
}

/*############################################################################*/
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
                get_articleList("1", $("#hidden_Order").val(), "1", "all");
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
        url: "../Handler/GetResources.aspx",
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

/*############################################################################*/

//Bootstrap Accordion example with expand/collapse all  
//https://codepen.io/Sp00ky/pen/zBZZvq