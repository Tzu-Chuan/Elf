/*pageList*/

/*===============*/
/*init*/
var __isFirstLoad = 1;


jQuery(document).ready(function () {
    if ($("#ProjectOpen").val() == "Y") {
        $("#Menu").hide();
    }
    // 撈出查詢條件
    getResources();
    // 文章列表
    getData(0);
    // Ask.com
    //getAskCom();
    // 文字雲
    getCloudSearchData();

    $("#WebsiteDesc").attr("title", "<div style='text-align:left;'>．Monitored websites include: cnet, zdnet, RD World Online, CompositesWorld<br>．Automatically search all articles with the \"technology item\" indicated in this project, scored by your personal preference</div>");
    $("#WebsiteDesc").tooltip({
        placement: 'top'
    });

    // 展開文章列表動作
    $(document).on("show.bs.collapse", ".collapse", function () {
        if (this.id == "ArticleListBlock_all") {
            getData(0);
        }
        else if (this.id.indexOf("website") > -1) {
            $("#articleByWebsite").find(".collapse.in").collapse("hide");
            $("#tmpWebsite").val($(this).attr("wguid"));
            getWebsiteArticle(0);
        }
        else if (this.id.indexOf("askcom") > -1) {
            $("#AskComList").find(".collapse.in").collapse("hide");
            $("#tmpAskCom").val($(this).attr("aguid"));
            getAskComArticle(0);
        }
    });

    // Sort Btn
    $(document).on("click", "a[name='sortbtn']", function () {
        $("a[name='sortbtn']").removeClass("btn-u-default");
        if (this.id == "score")
            $("#get_time").addClass("btn-u-default");
        else
            $("#score").addClass("btn-u-default");

        $("#sortname").val(this.id);
        getData(0);
    });

    // Website Sort Btn
    $(document).on("click", "a[name='website_sortbtn']", function () {
        $("a[name='website_sortbtn']").removeClass("btn-u-default");
        if ($(this).attr("aid") == "score")
            $("a[aid='get_time']").addClass("btn-u-default");
        else
            $("a[aid='score']").addClass("btn-u-default");

        $("#sortname").val($(this).attr("aid"));
        getWebsiteArticle(0);
    });

    // Tag Btn
    $(document).on("click", "#tagbtn", function () {
        doTagSelect($(this).attr("pjguid"), $(this).attr("articleguid"));
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
            }
        });
    });

    $(document).on("click", "input[name='cbResource'],input[name='cbTopic'],input[name='cbDate'],input[name='cbTag']", function () {
        getData(0);
        //getAskCom();
    });

    $(document).on("click", "a[name='alink']", function () {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "projectHandler/ArticleReadStatus.aspx",
            data: {
                area: "article",
                gid: $(this).attr("atGuid")
            },
            error: function (xhr) { alert(xhr.responseText); },
            success: function (data) {
                if ($(data).find("Error").length > 0) {
                    alert($(data).find("Error").attr("Message"));
                }
            }
        });
    });
});

// 主要文章列表
function getData(p) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "projectHandler/GetArticleList.aspx",
        data: {
            PageNo: p,
            PageSize: "20",
            PjGuid: $.getQueryString("pjGuid"),
            resources: $('input[name="cbResource"]:checked').val(),
            topics: $('input[name="cbTopic"]:checked').val(),
            period: $('input[name="cbDate"]:checked').val(),
            mytag: $('input[name="cbTag"]:checked').val(),
            SortName: $("#sortname").val()
        },
        error: function (xhr) {
            alert(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                if ($('input[name="cbResource"]:checked').val() == "all") {
                    $("#allArticleBlock").show();
                    $("#articleByWebsite").hide();
                    $("#ArticlesList").empty();
                    var str = '';
                    $("#resultCount").html($("total", data).text() + " results");
                    if ($(data).find("data_item").length > 0) {
                        $(data).find("data_item").each(function (i) {
                            str += "<li>";
                            str += $(this).children("itemNo").text().trim() + '.&nbsp;';
                            //建立專案後第一批進來的文章不顯示 new
                            var atGetDate = $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("get_time").text().trim()));
                            if (parseInt($(this).children("DaysDiff").text().trim()) < 3 && $(this).children("MinTime").text().trim() != atGetDate)
                                str += '<img src="../images/new_article.png" width="25px" />&nbsp;';
                            var aColor = (parseInt($(this).children("HaveRead").text().trim()) > 0) ? "color:#609;" : "";
                            str += '<a name="alink" target="_blank" atGuid="' + $(this).children("article_guid").text().trim() + '" href="articleDetail.aspx?pjGuid=' + $(this).children("project_guid").text().trim() + '&atGuid=' + $(this).children("article_guid").text().trim() + '" style="' + aColor + '">' + $(this).children("title").text().trim() + '</a>&nbsp;&nbsp;';
                            //str += '<a name="alink" target="_blank" atGuid="' + $(this).children("article_guid").text().trim() + '" href="ArticleContent.aspx?atGuid=' + $(this).children("article_guid").text().trim() + '" style="' + aColor + '">' + $(this).children("title").text().trim() + '</a>&nbsp;&nbsp;';
                            str += '<a id="tagbtn" href="javascript:void(0);" pjguid="' + $(this).children("project_guid").text().trim() + '" articleguid="' + $(this).children("article_guid").text().trim() + '">[tag]</a>';
                            str += '<blockquote><small><em>';
                            str += 'date:' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("get_time").text().trim())) + ' | score:<font color="red">' + $(this).children("score").text().trim() + '</font>';
                            str += '</em></small>';
                            str += '<p>' + $(this).children("articledesc").text().trim() + '</p>';
                            str += '</blockquote>';
                            str += '</li>';
                        });
                    }
                    else
                        str += '<li>data not found</li>';
                    $("#ArticlesList").append(str);
                    $("#allArticleBlock").scrollTop(0);
                    Page.Option.FunctionName = "getData";
                    Page.Option.Selector = "#pageblock";
                    Page.CreatePage(p, $("total", data).text());
                }
                else {
                    $("#allArticleBlock").hide();
                    $("#articleByWebsite").show();
                    getWebSite();
                }
            }
        }
    });
}

// WebSite 列表
function getWebSite() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "projectHandler/GetWebsite.aspx",
        data: {
            PjGuid: $.getQueryString("pjGuid"),
            topics: $('input[name="cbTopic"]:checked').val(),
            period: $('input[name="cbDate"]:checked').val(),
            mytag: $('input[name="cbTag"]:checked').val()
        },
        error: function (xhr) {
            alert(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                var str = '';
                $("#resultCount").html($("total", data).text() + " results");
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        str += '<div class="panel panel-default" >';
                        str += '<div class="panel-heading">';
                        str += '<div class="pull-right col-sm-4 col-md-3 margin-top-2">';
                        str += '<span class="badge badge-sea rounded-2x col-sm-12 col-md-12">' + $(this).children("results").text().trim() + ' results</span>';
                        str += '</div>';
                        str += '<a class="accordion-toggle" data-toggle="collapse" data-parent="#articleByWebsite" href="#ArticleListBlock_website_' + $(this).children("website_guid").text().trim() + '">website > ' + $(this).children("website_name").text().trim() + '</a>';
                        str += '</div>';
                        str += '<div id="ArticleListBlock_website_' + $(this).children("website_guid").text().trim() + '" wguid="' + $(this).children("website_guid").text().trim() + '" class="panel-collapse collapse">';
                        str += '<div class="row">';
                        str += '<div class="btn-group margin-bottom-30 col-md-6 col-md-offset-4" role="group">';
                        str += '<a aid="score" name="website_sortbtn" href="javascript:void(0);" class="btn-u" role="button">order by Score</a>';
                        str += '<a aid="get_time" name="website_sortbtn" href="javascript:void(0);" class="btn-u btn-u-default" role="button">order by Date</a>';
                        str += '</div></div>';
                        str += '<ol id="ArticlesList_' + $(this).children("website_guid").text().trim() + '" style="list-style-type: none;"></ol>';
                        str += '<div name="pageblock_bywebsite" style="text-align:center;"></div>';
                        str += '</div></div>';
                    });
                }

                $("#articleByWebsite").empty();
                $("#articleByWebsite").append(str);
            }
        }
    });
}

// Website 文章列表
function getWebsiteArticle(p) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "projectHandler/GetArticleList.aspx",
        data: {
            PageNo: p,
            PageSize: "20",
            PjGuid: $.getQueryString("pjGuid"),
            WebsiteGuid: $("#tmpWebsite").val(),
            resources: $('input[name="cbResource"]:checked').val(),
            topics: $('input[name="cbTopic"]:checked').val(),
            period: $('input[name="cbDate"]:checked').val(),
            mytag: $('input[name="cbTag"]:checked').val(),
            SortName: $("#sortname").val()
        },
        error: function (xhr) {
            alert(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                $("#ArticlesList_" + $("#tmpWebsite").val()).empty();
                var str = '';
                $("#resultCount").html($("total", data).text() + " results");
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        str += "<li>";
                        str += $(this).children("itemNo").text().trim() + ".&nbsp;";
                        //建立專案後第一批進來的文章不顯示 new
                        var atGetDate = $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("get_time").text().trim()));
                        if (parseInt($(this).children("DaysDiff").text().trim()) < 3 && $(this).children("MinTime").text().trim() != atGetDate)
                            str += '<img src="../images/new_article.png" width="25px" />&nbsp;';
                        var aColor = (parseInt($(this).children("HaveRead").text().trim()) > 0) ? "color:#609;" : "";
                        str += '<a name="alink" target="_blank" atGuid="' + $(this).children("article_guid").text().trim() + '" href="articleDetail.aspx?pjGuid=' + $(this).children("project_guid").text().trim() + '&atGuid=' + $(this).children("article_guid").text().trim() + '">' + $(this).children("title").text().trim() + '</a>&nbsp;&nbsp;';
                        //str += '<a name="alink" target="_blank" atGuid="' + $(this).children("article_guid").text().trim() + '" href="ArticleContent.aspx?atGuid=' + $(this).children("article_guid").text().trim() + '" style="' + aColor + '">' + $(this).children("title").text().trim() + '</a>&nbsp;&nbsp;';
                        str += '<a id="tagbtn" href="javascript:void(0);" pjguid="' + $(this).children("project_guid").text().trim() + '" articleguid="' + $(this).children("article_guid").text().trim() + '">[tag]</a>';
                        str += '<blockquote><small><em>';
                        str += 'date:' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("get_time").text().trim())) + ' | score:<font color="red">' + $(this).children("score").text().trim() + '</font>';
                        str += '</em></small>';
                        str += '<p>' + $(this).children("articledesc").text().trim() + '</p>';
                        str += '</blockquote>';
                        str += '</li>';
                    });
                }
                else
                    str += '<li>data not found</li>';

                $("#ArticlesList_" + $("#tmpWebsite").val()).append(str);
                $("#articleByWebsite").scrollTop(0);
                Page.Option.FunctionName = "getWebsiteArticle";
                Page.Option.Selector = "div[name='pageblock_bywebsite']";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}

// Ask.com 列表
function getAskCom() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "projectHandler/GetAskCom.aspx",
        data: {
            PjGuid: $.getQueryString("pjGuid"),
            topics: $('input[name="cbTopic"]:checked').val(),
            period: $('input[name="cbDate"]:checked').val()
        },
        error: function (xhr) {
            alert(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                var str = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        if (i == 0 || $(this).prev().children("research_name").text().trim() != $(this).children("research_name").text().trim())
                            str += '<div class="askCom_item"><br>Research topic：<span class="color-sea">' + $(this).children("research_name").text().trim() + '</span></div>';
                        str += '<div class="panel panel-default askCom_item">';
                        str += '<div class="panel-heading">';
                        str += '<div class="pull-right col-sm-6 col-md-6 margin-top-2">';
                        str += '<span class="badge badge-dark rounded-2x col-sm-4 col-md-4" style="margin-left: -20px;">' + $(this).children("result_count").text().trim() + ' results</span>';
                        switch ($(this).children("analyst_give").text().trim()) {
                            case "0":
                                str += '<span class="badge badge-dark rounded-2x col-sm-4 col-md-4">default</span>';
                                break;
                            case "1":
                                str += '<span class="badge badge-dark-blue rounded-2x col-sm-4 col-md-4">customized</span>';
                                break;
                            case "2":
                            case "3":
                                str += '<span class="badge badge-blue rounded-2x col-sm-4 col-md-4">learning</span>';
                                break;
                        }
                        str += '<span class="col-sm-4 col-md-4">';
                        if ($(this).children("schedule").text().trim() == 0)
                            str += '<button name="NScheBtn" aid="' + $(this).children("related_guid").text().trim() + '" class="btn-u btn-u-xs btn-u-dark rounded-2x" style="margin-left: -15px;">not schedule</button>';
                        else
                            str += '<button name="ScheBtn" aid="' + $(this).children("related_guid").text().trim() + '" class="btn-u btn-u-xs btn-u-purple rounded-2x" style="margin-left: -15px;">schedule</button>';
                        str += '</span>';
                        str += '</div>';
                        str += '<div><a class="accordion-toggle" data-toggle="collapse" data-parent="#AskComList" href="#askcom_' + $(this).children("related_guid").text().trim() + '">' + $(this).children("related_name").text().trim() + '</a></div>';
                        str += '</div>';
                        str += '<div id="askcom_' + $(this).children("related_guid").text().trim() + '" aguid="' + $(this).children("related_guid").text().trim() + '" class="panel-collapse collapse">';
                        str += '<ol id="askcomArticle_' + $(this).children("related_guid").text().trim() + '" style="list-style-type:none; margin-top:10px;"></ol>';
                        str += '<div name="pageblock_byaskcom" style="text-align:center;"></div>';
                        str += '</div>';
                        str += '</div>';
                    });
                }

                $("#AskComList").empty();
                $("#AskComList").append(str);
            }
        }
    });
}

// Ask.com 文章列表
function getAskComArticle(p) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "projectHandler/GetAskComArticleList.aspx",
        data: {
            PageNo: p,
            PageSize: "20",
            Related_guid: $("#tmpAskCom").val(),
            period: $('input[name="cbDate"]:checked').val()
        },
        error: function (xhr) {
            alert(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                $("#askcomArticle_" + $("#tmpAskCom").val()).empty();
                var str = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        str += "<li>";
                        str += $(this).children("itemNo").text().trim() + ".&nbsp;";
                        //建立專案後第一批進來的文章不顯示 new
                        var atGetDate = $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("get_time").text().trim()));
                        if (parseInt($(this).children("DaysDiff").text().trim()) < 3 && $(this).children("MinTime").text().trim() != atGetDate)
                            str += '<img src="../images/new_article.png" width="25px" />&nbsp;';
                        str += '<a href="' + $(this).children("url").text().trim() + '" target="_blank">' + $(this).children("title").text().trim() + '</a>';
                        str += '<blockquote><small><em>';
                        str += 'date:' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("get_time").text().trim()));
                        str += '</em></small>';
                        str += '<p>' + $(this).children("describe_text").text().trim() + '</p>';
                        str += '</blockquote>';
                        str += '</li>';
                    });
                }
                else
                    str += '<li>data not found</li>';

                $("#askcomArticle_" + $("#tmpAskCom").val()).append(str);
                Page.Option.FunctionName = "getAskComArticle";
                Page.Option.Selector = "div[name='pageblock_byaskcom']";
                Page.CreatePage(p, $("total", data).text());
            }
        }
    });
}

// Tag Select
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

// Tag Save
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
                    ULstr += '<li><input type="radio" id="TopicRadio' + i + '" value="' + $(this).children("research_guid").text().trim() + '" name="cbTopic" /><label for="TopicRadio' + i + '" style="margin-left: 4px;">' + $(this).children("name").text().trim() + '</label></li>';
                });
                $("#topic_ul").empty();
                $("#topic_ul").append(ULstr);

                var tagStr = '<li><input type="radio" id="TagRadio" value="all" name="cbTag" checked="checked" /><label for="TagRadio">No Tag</label></li>';
                $(data).find("mytag_item").each(function (i) {
                    tagStr += '<li><input type="radio" id="TagRadio' + i + '" value="' + $(this).children("tagtype_guid").text().trim() + '" name="cbTag" /><label for="TagRadio' + i + '" style="margin-left: 4px;">' + $(this).children("tagtype_name").text().trim() + '</label></li>';
                });
                $("#tag_ul").empty();
                $("#tag_ul").append(tagStr);
            }
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