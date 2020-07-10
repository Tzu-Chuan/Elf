<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="project_ProjectList" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta name="description" content="請填寫"/>
    <meta name="keywords" content="請填寫"/>
    <meta name="author" content="請填寫"/>

    <!--=====Favicon-->
    <link rel="shortcut icon" href="favicon.ico"/>
    <!--=====CSS Global Compulsory -->
    <link rel="stylesheet" href="../assets/plugins/bootstrap/css/bootstrap.css"/>
    <link rel="stylesheet" href="../assets/css/style.css"/>
    <!--=====CSS Implementing Plugins -->
    <link rel="stylesheet" href="../assets/plugins/font-awesome/css/font-awesome.min.css"/>
    <!--=====icon的元件-->
    <link rel="stylesheet" href="../assets/plugins/sky-forms/css/custom-sky-forms.css"/>
    <!--=====CSS Customization -->
    <link rel="stylesheet" href="../assets/css/custom.css"/>
    <!--=====CSS Customization by ochison  -->
    <link rel="stylesheet" href="../assets/css/iekicon.css"/>
    <link rel="stylesheet" href="../assets/css/ochi.css"/>

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
    <title>IEKElf</title>
    <script>
		$(document).ready(function () {
			getData(0);

			$(document).on("keypress", "#keyword", function (e) {
				if ((e.keyCode == 13) || (e.key == "Enter") || (e.code == "Enter")) {
					try {
						e.stopPropagation();
						e.preventDefault();
					}
					catch (err) {
						e.cancelBubble = true;
					}
					getData(0);
					return false;
				}
			});
		});
        
        function getData(p) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "projectHandler/GetProjectList.aspx",
                data: {
                    PageNo: p,
                    keyword: $("#keyword").val()
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
                                tabstr += '<td align="center">' + $(this).children("itemNo").text().trim() + '</td>';
                                tabstr += '<td><a href="../project/PjDetail.aspx?pjGuid=' + $(this).children("project_guid").text().trim() + '">' + $(this).children("project_name").text().trim() + '</a></td>';
                                tabstr += '<td>' + $(this).children("technology").text().trim() + '</td>';
                                var StatusColor = '';
                                if ($(this).children("status").text().trim() == 1)
                                    StatusColor = "label rounded-2x label-info col-sm-12 col-md-9";
                                else if (parseInt($(this).children("status").text().trim()) >= 10 && parseInt($(this).children("status").text().trim()) <= 22)
                                    StatusColor = "label rounded-2x label-orange col-sm-12 col-md-9";
                                else if ($(this).children("status").text().trim() == 40)
                                    StatusColor = "label rounded-2x label-success col-sm-12 col-md-9";
                                else if ($(this).children("status").text().trim() == 50)
                                    StatusColor = "label rounded-2x label-dark col-sm-12 col-md-9";
                                else if (parseInt($(this).children("status").text().trim()) >= 60 && parseInt($(this).children("status").text().trim()) <= 69)
                                    StatusColor = "label rounded-2x label-danger col-sm-12 col-md-9";
                                tabstr += '<td align="center" nowrap="nowrap"><span class="' + StatusColor + '">' + $(this).children("status_en_name").text().trim() + '</span></td>';
                                tabstr += '<td align="center">' + $(this).children("OwnerName").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("create_time").text().trim())) + '</td>';
                                var closed_time = ($(this).children("stop_time").text().trim() == "") ? "" : $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("stop_time").text().trim()));
                                tabstr += '<td align="center" nowrap="nowrap">' + closed_time + '</td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="8">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);
                        $("html").scrollTop(0);
                        Page.Option.Selector = "#pageblock";
                        Page.CreatePage(p, $("total", data).text());
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
                <!--匯入excel-->
                <div class="col-md-2 col-md-push-10 text-right">
                    <a id="newProject" data-toggle="modal" data-target="#InitiateProject_01" class="btn-u btn-u-lg btn-u-sea" style="display:none;">Initiate a project</a>
                </div>

                <!--查詢-->
                <div class="col-md-10 col-md-pull-2">
                    <div class="btn-group">
                        <div class="btn-group">
                            <input type="text" id="keyword" name="keyword" value="" onkeypress="" class="form-control" placeholder="Please input keyword" />
                        </div>
                        <button type="button" class="btn btn-main" id="SearchBtn" onclick="getData(0);">
                            <i class="fa fa-search"></i>&nbsp;Search
                        </button>
                    </div>
                </div>
                </div>

                <!--PageList-->
                <div class="well">
                    <div style="overflow:auto;">
                        <table id="tablist" class="table table-striped table-hover">
                            <thead>
                            <tr>
                                <th style="text-align:center;">NO</th>
                                <th style="text-align:center;">Project Name</th>
                                <th style="text-align:center;">Tech Item</th>
                                <th style="text-align:center; width:90px;">Status</th>
                                <th style="text-align:center;">Owner</th>
                                <th style="text-align:center;">Create Time</th>
                                <th style="text-align:center;">Closed Time</th>
                            </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                    <div id="pageblock" style="text-align:center;"></div>

                    <!--/*專案狀態圖示說明*/-->
                    <p class="text-info">
                        <i class="fa fa-info-circle"></i>Status Explain：
                    </p>
                    <table>
                            <tr>
                                <td><span class="label rounded-2x label-info col-md-9">Inception</span>：</td>
                                <td>The project is initiated and will not be analyzed before STARTing it. The project manager can still DELETE the project at this phase.</td>
                            </tr>
                            <tr>
                                <td><span class="label rounded-2x label-orange col-md-9">Analyzing</span>：</td>
                                <td>The tech item related articles are being searched, categorized, ranked, and recommended. The outcome will be ready for reading after finishing the first time of analyzing.</td>
                            </tr>
                            <tr>
                                <td><span class="label rounded-2x label-success col-md-9">Ready</span>：</td>
                                <td>The project is ready to be read.</td>
                            </tr>
                            <tr>
                                <td><span class="label rounded-2x label-dark col-md-9">Closed</span>：</td>
                                <td>The project is closed. Data won’t be updated after being closed.</td>
                            </tr>
                            <tr>
                                <td><span class="label rounded-2x label-danger col-md-9">Error</span>：</td>
                                <td>Please contact manager to get correction.</td>
                            </tr>
                        </table>
                </div><!--/well-->
            </div><!--/content-->
        </div>
        
         <!--#include file="../templates/Footer.html"-->
    </div>
</body>
</html>
