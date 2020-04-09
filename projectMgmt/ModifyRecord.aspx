﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModifyRecord.aspx.cs" Inherits="projectMgmt_ModifyRecord" %>

<!DOCTYPE html>

<html>
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
            getData(0);

            $(document).on("click", "a[name='undobtn']", function () {
                if (confirm("Confirm to undo this word?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "mgmtHandler/UndoWord.aspx",
                        data: {
                            id: $(this).attr("aid")
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
                            }
                        }
                    });
                }
            });
        }); // end js

        function getData(p) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "mgmtHandler/GetRecordList.aspx",
                data: {
                    PageNo: p,
                    PjGuid: $.getQueryString("pjGuid")
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
                                var descstr = '';
                                switch ($(this).children("status").text().trim()) {
                                    case "add":
                                        descstr = 'Add a word：' + $(this).children("name").text().trim();
                                        break;
                                    case "update":
                                        descstr = 'Update Information<br>';
                                        descstr += 'Research Topic：'+ $(this).children("orgWordCategory").text().trim() + ' → ' + $(this).children("WordCategory").text().trim() + '<br>';
                                        descstr += 'Related Key Word：' + $(this).children("org_name").text().trim() + ' → ' + $(this).children("name").text().trim() + '<br>';
                                        descstr += 'Tag in articles：' + FormatBlacklist($(this).children("org_blacklist").text().trim()) + ' → ' + FormatBlacklist($(this).children("blacklist").text().trim());
                                        break;
                                    case "delete":
                                        descstr = 'Delete a word：' + $(this).children("name").text().trim();
                                        break;
                                }
                                tabstr += '<td nowrap="nowrap">' + descstr + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("status").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("createdate").text().trim())) + '<br>' + $.FormatTime($(this).children("createdate").text().trim()) + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">';
                                tabstr += '<a name="undobtn" class="btn-u btn-u-red margin-right-5" href="javascript:void(0);" aid="'+$(this).children("id").text().trim()+'">Undo</a>';
                                tabstr += '</td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="5">data not found</td></tr>';
                        $("#tablist tbody").append(tabstr);
                        Page.Option.Selector = "#pageblock";
                        Page.CreatePage(p, $("total", data).text());
                        $(window).scrollTop(0);
                        
                    }
                }
            });
        }

        function FormatBlacklist(str) {
            var tmpV = '';
            switch (str) {
                case "0": tmpV = "Whitelist"; break;
                case "1": tmpV = "Blacklist"; break;
                case "2": tmpV = "Candidate"; break;
            }
            return tmpV;
        }
    </script>
</head>
<body>
    <div class="wrapper">
        <!--#include file="../templates/Header.html"-->
        <div class="container">
            <div class="content">
                <div class="well">
                    <table id="tablist" class="table table-striped table-hover">
                        <thead>
                            <tr>
                                <th style="text-align: center;">SN</th>
                                <th style="text-align: center;">Description</th>
                                <th style="text-align: center;">Action</th>
                                <th style="text-align: center;">Time</th>
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