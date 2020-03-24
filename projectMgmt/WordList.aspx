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

            $(document).on("click", "input[name='cbTopic'],input[name='cbBlacklist'],input[name='cbSource']", function () {
                getData(0);
            });
        });

        function getData(p) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "mgmtHandler/GetWordList.aspx",
                data: {
                    PageNo: p,
                    PjGuid: $.getQueryString("pjGuid"),
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
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("Topic").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("name").text().trim() + '</td>';
                                var blackStr = '';
                                switch ($(this).children("blacklist").text().trim()) {
                                    case "0": blackStr = "Whitelist"; break;
                                    case "1": blackStr = "Blacklist"; break;
                                    case "2": blackStr = "Candidate"; break;
                                }
                                tabstr += '<td align="center" nowrap="nowrap">' + blackStr + '</td>';
                                var analysisStr = '';
                                switch ($(this).children("blacklist").text().trim()) {
                                    case "0": analysisStr = "Default from excel"; break;
                                    case "1": analysisStr = "User Added"; break;
                                    case "2": analysisStr = "Learning-edited"; break;
                                    case "3": analysisStr = "Learning-unedited"; break;
                                }
                                tabstr += '<td align="center" nowrap="nowrap">' + analysisStr + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">';
                                tabstr += '<a class="btn-u btn-u-default margin-right-5" href="javascript:void(0);">Edit</a>';
                                tabstr += '<a class="btn-u btn-u-red margin-right-5" href="javascript:void(0);">Delete</a>';
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
                url: "../project/GetResources.aspx",
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
                                ULstr += '<li><input type="radio" id="TopicRadio' + i + '" value="' + $(this).children("research_guid").text().trim() + '" name="cbTopic" /><label for="TopicRadio' + i + '" style="margin-left: 4px;">' + $(this).children("name").text().trim() + '</label></li>';
                            else
                                ULstr += '<li><input type="radio" id="TopicRadio' + i + '" value="' + $(this).children("research_guid").text().trim() + '" name="cbTopic" /><label for="TopicRadio' + i + '" style="margin-left: 4px;">' + $(this).children("name").text().trim() + '</label></li>';
                        });
                        $("#topic_ul").empty();
                        $("#topic_ul").append(ULstr);
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
                <%--<div class="row margin-bottom-10 animated">
                    <div class="col-md-2 col-md-push-10 text-right"></div>
                    <div class="col-md-10 col-md-pull-2">
                        <div class="btn-group">
                            <div class="btn-group"><input type="text" id="keyword" name="keyword" value="" onkeypress="" class="form-control" placeholder="Please input keyword" /></div>
                            <button type="button" class="btn btn-main" id="SearchBtn" onclick="getData(0);"><i class="fa fa-search"></i>&nbsp;Search</button>
                        </div>
                    </div>
                </div>--%>

                <div class="gentable">
                    <table width="100%">
                        <tr>
                            <td valign="middle">Topics</td>
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
                <div>
                    <a class="btn-u btn-u-sea margin-right-5" href="javascript:void(0);">Add</a>
                </div>
                <div class="well">
                    <table id="tablist" class="table table-striped table-hover">
                        <thead>
                            <tr>
                                <th style="text-align: center;">SN</th>
                                <th style="text-align: center;">Research Topic</th>
                                <th style="text-align: center;">Related Key Word</th>
                                <th style="text-align: center;">Tag in articles?</th>
                                <th style="text-align: center;">Source</th>
                                <th style="text-align: center;">Function</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                    <div id="pageblock" style="text-align: center;"></div>
                </div><!--/well-->
            </div><!--/content-->
        </div>

        <!--#include file="../templates/Footer.html"-->
    </div>
</body>
</html>
