﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PjDetail.aspx.cs" Inherits="project_PjDetail" %>

<!DOCTYPE html>

<html>

<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta name="description" content="請填寫"/>
    <meta name="keywords" content="請填寫"/>
    <meta name="author" content="請填寫"/>

    <!--=====Favicon-->
    <link rel="shortcut icon" href="favicon.ico" />
    <!--=====CSS Global Compulsory -->
    <link rel="stylesheet" href="../assets/plugins/bootstrap/css/bootstrap.css" />
    <link rel="stylesheet" href="../assets/css/style.css" />
    <!--======CSS Implementing Plugins -->
    <link rel="stylesheet" href="../assets/plugins/font-awesome/css/font-awesome.min.css" />
    <!--icon的元件-->
    <link rel="stylesheet" href="../assets/plugins/sky-forms/css/custom-sky-forms.css" />
    <!--=====CSS Customization -->
    <link rel="stylesheet" href="../assets/css/custom.css" />
    <!--=====CSS Customization by ochison  -->
    <link rel="stylesheet" href="../assets/css/iekicon.css" />
    <link rel="stylesheet" href="../assets/css/scrollbar.css" />
    <link rel="stylesheet" href="../assets/css/ochi.css" />

    <!-- JS Global Compulsory -->
    <script type="text/javascript" src="../assets/plugins/jquery/jquery.js"></script>
    <script type="text/javascript" src="../assets/plugins/jquery/jquery-migrate.min.js"></script>
    <!-- JS Implementing Plugins -->
    <script type="text/javascript" src="../assets/plugins/back-to-top.js"></script>
    <!-- JS Customization -->
    <script type="text/javascript" src="../assets/js/custom.js"></script>
    <!-- JS Page Level -->
    <script type="text/javascript" src="../assets/js/app.js"></script>
    <!--看圖-->
    <script type="text/javascript" src="../assets/js/jquery.scrollbar.min.js"></script>
    <script type="text/javascript" src="../assets/js/ochiJS.js"></script>

    <!--[if lt IE 9]>
        <script src="../assets/plugins/respond.js"></script>
        <script src="../assets/plugins/html5shiv.js"></script>
        <script src="../assets/js/plugins/placeholder-IE-fixes.js"></script>
        <![endif]-->

    <!--===my d3-->
    <script src="../js/textCloud/d3.js"></script>
    <script src="../js/textCloud/d3.layout.cloud.js"></script>

    <!-- Nick.JS -->
    <script type="text/javascript" src="../js/NickCommon.js"></script>
    <script type="text/javascript" src="../js/PageList.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.1.12.1.js"></script>
    <script>$.widget.bridge('uitooltip', $.ui.tooltip);</script> <!--JQueryUI & bootstrap tooltip 會打架 重新設定 JQueryUI tooltip function name-->
    <!-- bootstrap -->
    <script type="text/javascript" src="../assets/plugins/bootstrap/js/bootstrap.min.js"></script>

    <!--===my js-->
    <script type="text/javascript" src="pjdetail.js"></script>
    <script type="text/javascript" src="wordclould.js"></script>
    <title>IEKElf</title>
    <script>
        $(document).ready(function () {
            getData(0);

            $(document).on("show.bs.collapse", ".collapse", function () {
                if ($('input[name="cbResource"]:checked').val() == "all") {
                    getData(0);
                }
                else {
                    $("#articleByWebsite").find(".collapse.in").collapse("hide");
                    $("#tmpWebsite").val($(this).attr("wguid"));
                    getWebsiteArticle(0);
                }
            });

            // Tag Btn
            $(document).on("click", "#tagbtn", function () {
                doTagSelect($(this).attr("pjguid"), $(this).attr("articleguid"));
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
        });
        
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
                                    str += $(this).children("itemNo").text().trim() + "&nbsp;";
                                    str += '<a href="articleDetail.aspx?pjGuid=' + $(this).children("project_guid").text().trim() + '&atGuid=' + $(this).children("article_guid").text().trim() + '">' + $(this).children("title").text().trim() + '</a>&nbsp;&nbsp;';
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
                                str += $(this).children("itemNo").text().trim() + "&nbsp;";
                                str += '<a href="articleDetail.aspx?pjGuid=' + $(this).children("project_guid").text().trim() + '&atGuid=' + $(this).children("article_guid").text().trim() + '">' + $(this).children("title").text().trim() + '</a>&nbsp;&nbsp;';
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
                                str += '<a id="score" name="sortbtn" href="javascript:void(0);" class="btn-u" role="button">order by Score</a>';
                                str += '<a id="get_time" name="sortbtn" href="javascript:void(0);" class="btn-u btn-u-default" role="button">order by Date</a>';
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
    </script>
</head>
<body class="header-fixed boxed-layout">
    <div class="wrapper">
         <!--#include file="../templates/Header.html"-->
          <div class="breadcrumbs">
            <div class="container">
              <div class="row padding10TB">
                <div class="col-lg-6 col-md-6 col-sm-6">
                  <span class="font-size4">Project Name：</span><b></b>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6">
                  <span class="font-size4">Technology item：</span><b></b>
                </div>
              </div>

            </div>
          </div>

          <div class="container">
              <div class="text-right margin10T"><a data-toggle="modal" data-target="#block_abs" class="btn-u" >Abstract</a></div>
              <!--文字雲-->
              <div class="twocol margin10T"><div class="left"><H4>At a glance: word cloud</H4></div></div>
              <div class="maxheightB BoxBorderSa BoxBgWa padding5ALL"><div id="blockTag" class="width100"></div></div>

              <div class="row margin20T margin10B">
                  <div class="col-sm-12">
                      <!--<div class="col-lg-7 col-md-7 col-sm-12">-->
                      <div class="twocol">
                          <div class="left"><h4>Recommended articles from monitored websites <i class="fa fa-info-circle white-tooltip" id="WebsiteDesc" data-html="true" style="cursor: pointer;"></i></h4></div>
                          <div class="right"></div>
                      </div>

                      <div class="BoxBorderSa maxheightA">
                          <div class="padding5ALL ">
                              <!-- Search Block-->
                              <div class="gentable">
                                  <table width="100%">
                                      <tr>
                                          <td width="70" valign="middle">Resources</td>
                                          <td>
                                              <ul class="ks-cboxtags">
                                                  <li><input type="radio" id="checkboxTwo" value="all" class="" name="cbResource" checked="checked" /><label for="checkboxTwo">All resources</label></li>
                                                  <li><input type="radio" id="checkboxThree" value="site" class="" name="cbResource" /><label for="checkboxThree">By website</label></li>
                                              </ul>
                                          </td>
                                      </tr>
                                      <tr>
                                          <td valign="middle">Topics</td>
                                          <td><ul id="topic_ul" class="ks-cboxtags"></ul></td>
                                      </tr>
                                      <tr>
                                          <td valign="middle">Period</td>
                                          <td>
                                              <ul class="ks-cboxtags">
                                                  <li><input type="radio" id="checkboxOne3" value="0" class="" name="cbDate" checked="checked" /><label for="checkboxOne3">Max period</label></li>
                                                  <li><input type="radio" id="checkboxTwo3" value="1" class="" name="cbDate" /><label for="checkboxTwo3">1 Day</label></li>
                                                  <li><input type="radio" id="checkboxThree3" value="7" class="" name="cbDate" /><label for="checkboxThree3">1 Week</label></li>
                                                  <li><input type="radio" id="checkboxFour3" value="30" class="" name="cbDate" /><label for="checkboxFour3">1 Month</label></li>
                                                  <li><input type="radio" id="checkboxFive3" value="180" class="" name="cbDate" /><label for="checkboxFive3">6 Months</label></li>
                                                  <li><input type="radio" id="checkboxsix3" value="365" class="" name="cbDate" /><label for="checkboxsix3">1 Year</label></li>
                                              </ul>
                                          </td>
                                      </tr>
                                      <tr>
                                          <td valign="middle">Tag</td>
                                          <td><ul id="tag_ul" class="ks-cboxtags"></ul></td>
                                      </tr>
                                  </table>
                              </div>

                              <!--Articles Block-->
                              <input id="sortname" type="hidden" value="score" />
                              <div class="scroll-wrapper maxheightA2 scrollbar-outer" style="position: relative;">
                                <div id="allArticleBlock" style="max-height: 488px; overflow-y:scroll;">
                                    <div id="accordion_1" class="panel-group acc-v1">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <!--總筆數-->
                                                <div class="pull-right col-sm-4 col-md-3 margin-top-2">
                                                    <span id="resultCount" class="badge badge-sea rounded-2x col-sm-12 col-md-12"></span>
                                                </div>

                                                <!--功能-連結-->
                                                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion_1" href="#ArticleListBlock_all">all articles</a>
                                            </div>

                                            <div id="ArticleListBlock_all" class="panel-collapse collapse in">
                                                <!--排序-->
                                                <div class="row">
                                                    <div class="btn-group margin-bottom-30 col-md-6 col-md-offset-4" role="group" aria-label="">
                                                        <a id="score" name="sortbtn" href="javascript:void(0);" class="btn-u" role="button">order by Score</a>
                                                        <a id="get_time" name="sortbtn" href="javascript:void(0);" class="btn-u btn-u-default" role="button">order by Date</a>
                                                    </div>
                                                </div>

                                            <!--pageData-->
                                            <ol id="ArticlesList" style="list-style-type: none;"></ol>
                                                      
                                            <div id="pageblock" style="text-align:center;"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                </div>
                                  
                                <div id="articleByWebsite" class="panel-group acc-v1" style="max-height: 488px; overflow-y: scroll;"></div>
                                <input type="hidden" id="tmpWebsite" />
                              </div>
                              <!--Articles Block End-->
                          </div>
                      </div>
                  </div>
              </div>
          </div>
         <!--#include file="../templates/Footer.html"-->
    </div>

    <!-- Tag Model-->
    <div class="modal fade" id="myTemplate_tagSelect" tabindex="-1" role="dialog" aria-labelledby="myModalLabel_02" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="close"><span aria-hidden="true">&times;</span></button>
                    <h2 class="modal-title" id="myModalLabel_02">Tag Select</h2>
                </div>
                <div class="modal-body" style="margin-bottom:50px;">

                    <!--===msg-->
                    <div id="tagSelect_message" style="color: red;"></div>
                    <br />

                    <!--===content-->
                    <div id="tagSelect_dataBlock"></div>

                    <!--===btn-->
                    <div class="col-sm-12 col-md-12 text-right">
                        <input type="submit" value="Maintain Tag" id="tagSelect_maintain" class="btn-u btn-u-lg btn-u-sea" style="margin-right: 10px;" onclick="doTagSelect_maintain();" />
                        <input type="submit" value="Cancel" id="tagSelect_cancel" class="btn-u btn-u-lg btn-u-sea" style="margin-right: 10px;" onclick="doTagSelect_cancel();" />
                        <input type="submit" value="Save" id="tagSelect_save" class="btn-u btn-u-lg btn-u-sea" onclick="doTagSelect_save();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
