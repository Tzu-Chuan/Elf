<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MemberManage.aspx.cs" Inherits="projectMgmt_MemberManage" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>IEKElf</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="請填寫">
    <meta name="keywords" content="請填寫">
    <meta name="author" content="請填寫">
    <link rel="shortcut icon" href="favicon.ico">
    <link rel="stylesheet" href="<%=ResolveUrl("~/assets/plugins/bootstrap/css/bootstrap.css") %>">
    <link rel="stylesheet" href="<%=ResolveUrl("~/assets/css/style.css") %>">
    <link rel="stylesheet" href="<%=ResolveUrl("~/assets/plugins/font-awesome/css/font-awesome.min.css") %>">
    <link rel="stylesheet" href="<%=ResolveUrl("~/assets/plugins/sky-forms/css/custom-sky-forms.css") %>">
    <link rel="stylesheet" href="<%=ResolveUrl("~/assets/css/custom.css") %>">
    <!--=====CSS Customization by ochison  -->
    <link rel="stylesheet" href="<%=ResolveUrl("~/assets/css/iekicon.css") %>"/>
    <link rel="stylesheet" href="<%=ResolveUrl("~/assets/css/ochi.css") %>"/>

    <script type="text/javascript" src="<%=ResolveUrl("~/assets/plugins/jquery/jquery.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/assets/plugins/jquery/jquery-migrate.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/assets/plugins/bootstrap/js/bootstrap.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/assets/plugins/back-to-top.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/assets/js/custom.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/assets/js/app.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/assets/js/jquery.scrollbar.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/assets/js/ochiJS.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/js/PageList.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/js/NickCommon.js") %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            getData(0);

            /// 表頭排序
            $(document).on("click", "a[name='emp_sortbtn']", function () {
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
                getEmployeeList(0);
            });

            // 查詢員工跳窗
            $(document).on("click", "#addMember", function () {
                $("#EmpList").modal("show");

                //分頁設定
                Page.Option.SortMethod = "+";
                Page.Option.SortName = "com_orgcd";
                getEmployeeList(0);
            });

            // 查詢員工
            $(document).on("click", "#emp_SearchBtn", function () {
                getEmployeeList(0);
            });

            // 新增成員
            $(document).on("click", "a[name='checkbtn']", function () {
                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "mgmtHandler/addMember.aspx",
                    data: {
                        pjid: $.getQueryString("pj"),
                        empno: $(this).text(),
                        name: $(this).attr("empname"),
                        deptid: $(this).attr("deptid")
                    },
                    error: function (xhr) {
                        alert(xhr.responseText);
                    },
                    success: function (data) {
                        if ($(data).find("Error").length > 0) {
                            alert($(data).find("Error").attr("Message"));
                        }
                        else {
                            $("#EmpList").modal("hide");
                            //alert($("Response", data).text());
                            getData(0);
                        }
                    }
                });
            });

            // 刪除成員
            $(document).on("click", "input[name='delbtn']", function () {
                var empno = $(this).attr("aid");
                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "mgmtHandler/deleteMember.aspx",
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
                            alert($("Response", data).text());
                            getData(0);
                        }
                    }
                });
            });
        });// end js

        function getData(p) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "mgmtHandler/GetMember.aspx",
                data: {
                    PageNo: p,
                    pjGuid: $.getQueryString("pj"),
                    keyword: $("#KeywordStr").val()
                },
                error: function (xhr) {
                    alert(xhr.responseText);
                },
                success: function (data) {
                    if ($(data).find("Error").length > 0) {
                        alert($(data).find("Error").attr("Message"));
                    }
                    else {
                        $("#MemberList tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("itemNo").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("PM_Deptid").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("PM_Name").text().trim() + '(' + $(this).children("PM_Empno").text().trim() + ')</td>';
                                tabstr += '<td align="center" nowrap="nowrap">';
                                tabstr += '<input class="btn-u btn-u-red margin-right-5" type="button" name="delbtn" value="Delete" aid="' + $(this).children("PM_ID").text().trim() + '">';
                                tabstr += '</td></tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="4">Not Found</td></tr>';
                        $("#MemberList tbody").append(tabstr);
                        Page.Option.Selector = "#pageblock";
                        Page.CreatePage(p, $("total", data).text());
                    }
                }
            });
        }

        function getEmployeeList(p) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetEmpList.aspx",
                data: {
                    PageNo: p,
                    mode: "member",
                    keyword: $("#emp_keyword").val(),
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
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("itemNo").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("org_abbr_chnm1").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap"><a name="checkbtn" href="javascript:void(0);" empname="' + $(this).children("com_cname").text().trim() + '" deptid="'
                                    + $(this).children("com_deptid").text().trim() + '">' + $(this).children("com_empno").text().trim() + '</a></td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("com_cname").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("com_deptid").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("com_mailadd").text().trim() + '</td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="6">查詢無資料</td></tr>';

                        $("#emptab tbody").empty();
                        $("#emptab tbody").append(tabstr);
                        Page.Option.FunctionName = "getEmployeeList";
                        Page.Option.Selector = "#emp_pageblock";
                        Page.CreatePage(p, $("total", data).text());
                    }
                }
            });
        }
    </script>
    <style type="text/css">
        .header .navbar-default .navbar-nav > li > a {
            padding-top: 0px;
            padding-bottom: 0px;
        }
    </style>
</head>
<body class="header-fixed boxed-layout">
    <form id="form1">
        <div class="wrapper">
            <!--#include file="../templates/Header.html"-->
            <!--Content-->
            <div style="padding: 10px 0 6px;border-bottom: solid 1px #eee;background: rgba(0, 0, 0, 0.03);">
                <div class="container">
                    <div class="row padding10TB">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            Tech Item：<b><%= PjName %></b>
                        </div>
                    </div>
                </div>
            </div>
            <div class="container content">
                <h4>Project Member List</h4>
                <div class="twocol">
                    <div class="right"><input id="addMember" type="button" class="btn-u btn-u-sea" value="Add Member" /></div>
                </div>
                <div class="well margin5T" style="margin-bottom:100px;">
                    <div style="overflow:auto;">
                        <table id="MemberList" class="table table-striped table-hover">
                        <thead>
                            <tr>
                                <th style="text-align:center;">No</th>
                                <th style="text-align:center;">Deptid</th>
                                <th style="text-align:center;">Name</th>
                                <th style="text-align:center;">Function</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                    </div>
                    <div id="pageblock" style="text-align:center;"></div>
                </div>
            </div>
            <!--#include file="../templates/Footer.html"-->
        </div>

        <div class="modal fade" id="EmpList" tabindex="-1" role="dialog" aria-labelledby="myModalLabel2" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h2 class="modal-title" id="myModalLabel2">查詢員工資料</h2>
                    </div>
                    <div class="modal-body">
                        <div class="btn-group">
                            <div class="btn-group">
                                <input type="text" id="emp_keyword" name="keyword" value="" onkeypress="" class="form-control" placeholder="Please input keyword" />
                            </div>
                            <a href="javascript:void(0);" class="btn btn-main" id="emp_SearchBtn"><i class="fa fa-search"></i>&nbsp;Search</a>
                        </div>
                        <div class="well margin10T">
                            <div style="overflow:auto;">
                                <table id="emptab" class="table table-striped table-hover">
                                <thead>
                                    <tr>
                                        <th nowrap="" style="text-align: center;">No.</th>
                                        <th nowrap="" style="text-align: center;">所別&nbsp;<a href="javascript:void(0);" name="emp_sortbtn" sortname="com_orgcd"><i class="fa fa-angle-up"></i></a></th>
                                        <th nowrap="" style="text-align: center;">工號&nbsp;<a href="javascript:void(0);" name="emp_sortbtn" sortname="com_empno"><i class="fa fa-angle-down"></i></a></th>
                                        <th nowrap="" style="text-align: center;">姓名&nbsp;<a href="javascript:void(0);" name="emp_sortbtn" sortname="com_cname"><i class="fa fa-angle-down"></i></a></th>
                                        <th nowrap="" style="text-align: center;">單位&nbsp;<a href="javascript:void(0);" name="emp_sortbtn" sortname="com_deptid"><i class="fa fa-angle-down"></i></a></th>
                                        <th nowrap="" style="text-align: center;">E-Mail&nbsp;<a href="javascript:void(0);" name="emp_sortbtn" sortname="com_mailadd"><i class="fa fa-angle-down"></i></a></th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                            </div>
                            <div id="emp_pageblock" style="text-align: center;"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
