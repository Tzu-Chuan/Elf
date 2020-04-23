<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WordList.aspx.cs" Inherits="projectMgmt_WordList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="請填寫" />
    <meta name="keywords" content="請填寫" />
    <meta name="author" content="請填寫" />

    <!--=====Favicon-->
    <link rel="shortcut icon" href="favicon.ico" />
    <!--=====CSS Global Compulsory -->
    <link rel="stylesheet" href="../assets/plugins/bootstrap/css/bootstrap.css" />
    <link rel="stylesheet" href="../assets/css/style.css" />
    <!--=====CSS Implementing Plugins -->
    <link rel="stylesheet" href="../assets/plugins/font-awesome/css/font-awesome.min.css" />
    <!--=====icon的元件-->
    <link rel="stylesheet" href="../assets/plugins/sky-forms/css/custom-sky-forms.css" />
    <!--=====CSS Customization -->
    <link rel="stylesheet" href="../assets/css/custom.css" />
    <!--=====CSS Customization by ochison  -->
    <link rel="stylesheet" href="../assets/css/iekicon.css" />
    <link rel="stylesheet" href="../assets/css/ochi.css" />

    <!-- JS Global Compulsory -->
    <script type="text/javascript" src="../assets/plugins/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../assets/plugins/jquery/jquery-migrate.min.js"></script>
    <script type="text/javascript" src="../assets/plugins/bootstrap/js/bootstrap.min.js"></script>
    <!-- JS Implementing Plugins -->
    <script type="text/javascript" src="../assets/plugins/back-to-top.js"></script>
    <!-- JS Customization -->
    <script type="text/javascript" src="../assets/js/custom.js"></script>
    <!-- JS Page Level -->
    <script type="text/javascript" src="../assets/js/app.js"></script>
    <!-- my js -->
    <script type="text/javascript" src="../js/PageList.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.1.12.1.js"></script>
    <script type="text/javascript" src="../js/NickCommon.js"></script>
    <title>IEKElf</title>
    <script>
        $(document).ready(function () {
            //分頁設定
            Page.Option.SortMethod = "-";
            Page.Option.SortName = "analyst_give";

            getData(0);
            getTopics();

            /// 表頭排序
            $(document).on("click", "a[name='sortbtn']", function () {
                if (Page.Option.SortName != $(this).attr("sortname")) {
                    var nowMethod = $(this).find("i").attr("class");
                    if (nowMethod == "fa fa-angle-up")
                        Page.Option.SortMethod = "+";
                    else
                        Page.Option.SortMethod = "-";
                }
                Page.Option.SortName = $(this).attr("sortname");
                if (Page.Option.SortMethod == "-") {
                    Page.Option.SortMethod = "+";
                    $(this).find("i").attr("class", 'fa fa-angle-up');
                }
                else {
                    Page.Option.SortMethod = "-";
                    $(this).find("i").attr("class", 'fa fa-angle-down');
                }
                getData(0);
            });

            $(document).on("click", "#RecordBtn", function () {
                window.open("ModifyRecord.aspx?pjGuid=" + $.getQueryString("pjGuid"));
            });

            $(document).on("click", "input[name='cbTopic'],input[name='cbBlacklist'],input[name='cbSource']", function () {
                getData(0);
            });

            // 新增字詞跳窗
            $(document).on("click", "#newWord", function () {
                $("#addbtn").val("Add This Word");
                $("#tmpGuid").val("");
                $("#tmpTopic").val("");
                $("#tmpWord").val("");
                $("#tmpBlacklist").val("");
                $("#m_ddlTopics").val($("#m_ddlTopics").find('option').first().val());
                $("#m_word").val("");
                $("#m_blacklist").val("0");
                $('#addModal').modal('show');
            });

            // 新增字詞
            $(document).on("click", "#addbtn", function () {
                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "mgmtHandler/addWord.aspx",
                    data: {
                        pjGuid: $.getQueryString("pjGuid"),
                        wGuid: $("#tmpGuid").val(),
                        TopicID: $("#m_ddlTopics").val(),
                        Word: $("#m_word").val(),
                        Blacklist: $("#m_blacklist").val(),
                        OrgTopic: $("#tmpTopic").val(),
                        OrgWord: $("#tmpWord").val(),
                        OrgBlacklist: $("#tmpBlacklist").val(),
                        OrgAnalysis: $("#tmpAnalysis").val()
                    },
                    error: function (xhr) {
                        alert(xhr.responseText);
                    },
                    success: function (data) {
                        if ($(data).find("Error").length > 0) {
                            alert($(data).find("Error").attr("Message"));
                        }
                        else {
                            //alert($("Response", data).text());
                            getData(0);
                            $('#addModal').modal('hide');
                        }
                    }
                });
            });

            // 刪除字詞
            $(document).on("click", "a[name='delbtn']", function () {
                if (confirm("Confirm to delete this word?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "mgmtHandler/deleteWord.aspx",
                        data: {
                            pjGuid: $.getQueryString("pjGuid"),
                            WordGuid: $(this).attr("aid")
                        },
                        error: function (xhr) {
                            alert(xhr.responseText);
                        },
                        success: function (data) {
                            if ($(data).find("Error").length > 0) {
                                alert($(data).find("Error").attr("Message"));
                            }
                            else {
                                getData(0);
                            }
                        }
                    });
                }
            });

            // 編輯字詞
            $(document).on("click", "a[name='editbtn']", function () {
                $("#addbtn").val("Save");
                $("#tmpGuid").val($(this).attr("aid"));
                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "mgmtHandler/GetWord.aspx",
                    data: {
                        WordGuid: $(this).attr("aid")
                    },
                    error: function (xhr) {
                        alert(xhr.responseText);
                    },
                    success: function (data) {
                        if ($(data).find("Error").length > 0) {
                            alert($(data).find("Error").attr("Message"));
                        }
                        else {
                            $("#tmpTopic").val($("research_guid", data).text());
                            $("#tmpWord").val($("name", data).text());
                            $("#tmpBlacklist").val($("blacklist", data).text());
                            $("#tmpAnalysis").val($("analyst_give", data).text());
                            $("#m_ddlTopics").val($("research_guid", data).text());
                            $("#m_word").val($("name", data).text());
                            $("#m_blacklist").val($("blacklist", data).text());
                            $('#addModal').modal('show');
                        }
                    }
                });
            });
        }); // end js

        function getData(p) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "mgmtHandler/GetWordList.aspx",
                data: {
                    PageNo: p,
                    PjGuid: $.getQueryString("pjGuid"),
                    keyword : $("#keyword").val(),
                    Topic: $('input[name="cbTopic"]:checked').val(),
                    Blacklist: $('input[name="cbBlacklist"]:checked').val(),
                    Source: $('input[name="cbSource"]:checked').val(),
                    SortName: Page.Option.SortName,
                    SortMethod: Page.Option.SortMethod
                },
                error: function (xhr) {
                    alert(xhr.responseText);
                },
                success: function (data) {
                    if ($(data).find("Error").length > 0) {
                        alert($(data).find("Error").attr("Message"));
                    }
                    else {
                        $("#tablist tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("itemNo").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("name").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("Topic").text().trim() + '</td>';
                                var blackStr = '';
                                switch ($(this).children("blacklist").text().trim()) {
                                    case "0": blackStr = "Whitelist"; break;
                                    case "1": blackStr = "Blacklist"; break;
                                    case "2": blackStr = "Candidate"; break;
                                }
                                tabstr += '<td align="center" nowrap="nowrap">' + blackStr + '</td>';
                                var analysisStr = '';
                                switch ($(this).children("analyst_give").text().trim()) {
                                    case "0": analysisStr = "Default from excel"; break;
                                    case "1": analysisStr = "User Added"; break;
                                    case "2": analysisStr = "Learning-edited"; break;
                                    case "3": analysisStr = "Learning-unedited"; break;
                                }
                                tabstr += '<td align="center" nowrap="nowrap">' + analysisStr + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">';
                                tabstr += '<a name="editbtn" class="btn-u btn-u-default margin-right-5" href="javascript:void(0);" aid="'+$(this).children("related_guid").text().trim()+'">Edit</a>';
                                tabstr += '<a name="delbtn" class="btn-u btn-u-red margin-right-5" href="javascript:void(0);" aid="'+$(this).children("related_guid").text().trim()+'">Delete</a>';
                                tabstr += '</td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="6">data not found</td></tr>';
                        $("#tablist tbody").append(tabstr);
                        Page.Option.Selector = "#pageblock";
                        Page.CreatePage(p, $("total", data).text());
                        $(window).scrollTop(0);
                    }
                }
            });
        }

        function getTopics() {
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
                        var ddlstr = '';
                        var ULstr = '<li><input type="radio" id="TopicRadio" value="all" name="cbTopic" checked="checked" /><label for="TopicRadio">All topics</label></li>';
                        $(data).find("topic_item").each(function (i) {
                            ddlstr += '<option value="' + $(this).children("research_guid").text().trim() + '">' + $(this).children("name").text().trim() + '</option>';
                            if (i == 0)
                                ULstr += '<li><input type="radio" id="TopicRadio' + i + '" value="' + $(this).children("research_guid").text().trim() + '" name="cbTopic" /><label for="TopicRadio' + i + '" style="margin-left: 4px;">' + $(this).children("name").text().trim() + '</label></li>';
                            else
                                ULstr += '<li><input type="radio" id="TopicRadio' + i + '" value="' + $(this).children("research_guid").text().trim() + '" name="cbTopic" /><label for="TopicRadio' + i + '" style="margin-left: 4px;">' + $(this).children("name").text().trim() + '</label></li>';
                        });
                        $("#topic_ul").empty();
                        $("#topic_ul").append(ULstr);
                        $("#m_ddlTopics").empty();
                        $("#m_ddlTopics").append(ddlstr);
                    }
                }
            });
        }
    </script>
</head>
<body class="header-fixed boxed-layout">
    <div class="wrapper">
        <!--#include file="../templates/Header.html"-->

        <!-- container -->
        <div class="container">
            <div class="content">
                <div class="row margin-bottom-10 animated">
                    <div class="col-md-2 col-md-push-10 text-right"></div>
                    <div class="col-md-10 col-md-pull-2">
                        <div class="btn-group">
                            <div class="btn-group"><input type="text" id="keyword" name="keyword" value="" onkeypress="" class="form-control" placeholder="Please input keyword" /></div>
                            <button type="button" class="btn btn-main" id="SearchBtn" onclick="getData(0);"><i class="fa fa-search"></i>&nbsp;Search</button>
                        </div>
                    </div>
                </div>

                <div class="gentable">
                    <table width="100%">
                        <tr>
                            <td valign="middle">Research Topic</td>
                            <td>
                                <ul id="topic_ul" class="ks-cboxtags"></ul>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">Tag in articles</td>
                            <td>
                                <ul class="ks-cboxtags">
                                    <li><input type="radio" id="checkboxOne" value="all" class="" name="cbBlacklist" checked="checked" /><label for="checkboxOne">All</label></li>
                                    <li><input type="radio" id="checkboxTwo" value="0" class="" name="cbBlacklist" /><label for="checkboxTwo">Whitelist</label></li>
                                    <li><input type="radio" id="checkboxThree" value="1" class="" name="cbBlacklist" /><label for="checkboxThree">Blacklist</label></li>
                                    <li><input type="radio" id="checkboxFour" value="2" class="" name="cbBlacklist" /><label for="checkboxFour">Candidate</label></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">Source</td>
                            <td>
                                <ul class="ks-cboxtags">
                                    <li><input type="radio" id="checkboxOne2" value="all" class="" name="cbSource" checked="checked" /><label for="checkboxOne2">All</label></li>
                                    <li><input type="radio" id="checkboxTwo2" value="0" class="" name="cbSource" /><label for="checkboxTwo2">Default from excel</label></li>
                                    <li><input type="radio" id="checkboxThree2" value="1" class="" name="cbSource" /><label for="checkboxThree2">User Added</label></li>
                                    <li><input type="radio" id="checkboxFour2" value="2" class="" name="cbSource" /><label for="checkboxFour2">Learning-edited</label></li>
                                    <li><input type="radio" id="checkboxFive2" value="3" class="" name="cbSource" /><label for="checkboxFive2">Learning-unedited</label></li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                </div>

                <!--PageList-->
                <div style="margin-bottom:35px;">
                    <div style="float:left;"><a id="newWord" class="btn-u btn-u-sea" href="javascript:void(0);">Add New Word</a></div>
                    <div style="float:right;"><a id="RecordBtn" class="btn-u btn-u-sea" href="javascript:void(0);">Maintain History</a></div>
                </div>
                <div class="well">
                    <div style="overflow:auto;">
                        <table id="tablist" class="table table-striped table-hover">
                        <thead>
                            <tr>
                                <th style="text-align: center;">SN</th>
                                <th style="text-align: center;">Related Key Word&nbsp;<a href="javascript:void(0);" name="sortbtn" sortname="name"><i class="fa fa-angle-up"></i></a></th>
                                <th style="text-align: center;">Research Topic&nbsp;<a href="javascript:void(0);" name="sortbtn" sortname="Topic"><i class="fa fa-angle-up"></i></a></th>
                                <th style="text-align: center;">Tag in articles&nbsp;<a href="javascript:void(0);" name="sortbtn" sortname="blacklist"><i class="fa fa-angle-up"></i></a></th>
                                <th style="text-align: center;">Source&nbsp;<a href="javascript:void(0);" name="sortbtn" sortname="analyst_give"><i class="fa fa-angle-down"></i></a></th>
                                <th style="text-align: center;">Function</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                    </div>
                    <div id="pageblock" style="text-align: center;"></div>
                </div><!--/well-->
            </div><!--/content-->
        </div>

        <!--#include file="../templates/Footer.html"-->
    </div>

    <input id="tmpGuid" type="hidden" />
    <input id="tmpTopic" type="hidden" />
    <input id="tmpWord" type="hidden" />
    <input id="tmpBlacklist" type="hidden" />
    <input id="tmpAnalysis" type="hidden" />
    <div class="modal fade" id="addModal" tabindex="-1" role="dialog" aria-labelledby="addLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h2 class="modal-title" id="addLabel">Add New Word</h2>
                </div>
                <div class="modal-body">
                    <table id="addtab" class="table table-striped">
                        <tr>
                            <th style="text-align: right;">Related Key Word</th>
                            <td><input id="m_word" type="text" class="form-control" /></td>
                        </tr>
                        <tr>
                            <th style="text-align: right; width:200px;">Research Topic</th>
                            <td><select id="m_ddlTopics" class="form-control"></select></td>
                        </tr>
                        <tr>
                            <th style="text-align: right;">Tag in articles?</th>
                            <td>
                                <select id="m_blacklist" class="form-control">
                                    <option value="0">Whitelist</option>
                                    <option value="1">Blacklist</option>
                                    <option value="2">Candidate</option>
                                </select>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <input type="button" value="Cancel" class="btn-u btn-u-lg btn-u-sea" data-dismiss="modal" />
                    <input type="button" id="addbtn" class="btn-u btn-u-lg btn-u-sea" />
                </div>
            </div>
        </div>
    </div>
</body>
</html>
