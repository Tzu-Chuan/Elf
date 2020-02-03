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
            getCommonData(0);

            $(document).on("click", "#comSearchBtn", function () {
                getCommonData(0);
            });

            // 新增成員
            $(document).on("click", "input[name='selectbtn']", function () {
                var empno = $(this).attr("aid");
                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "mgmtHandler/addMember.aspx",
                    data: {
                        pjid: $.getQueryString("pj"),
                        empno: $(this).attr("ano"),
                        name: $(this).attr("aname"),
                        deptid: $(this).attr("adeptid")
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
                    pjGuid: $.getQueryString("pj"),
                    keyword: $("#KeywordStr").val(),
                    PageNo: p,
                    PageSize: CommonPage.Option.PageSize
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

        function getCommonData(p) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "mgmtHandler/GetCommon.aspx",
                data: {
                    keyword: $("#comKeywordStr").val(),
                    PageNo: p,
                    PageSize: CommonPage.Option.PageSize
                },
                error: function (xhr) {
                    alert(xhr.responseText);
                },
                success: function (data) {
                    if ($(data).find("Error").length > 0) {
                        alert($(data).find("Error").attr("Message"));
                    }
                    else {
                        $("#CommonList tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("itemNo").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("com_deptid").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("com_cname").text().trim() + '(' + $(this).children("com_empno").text().trim() + ')</td>';
                                tabstr += '<td align="center" nowrap="nowrap">';
                                tabstr += '<input class="btn-u btn-u-orange margin-right-5" type="button" name="selectbtn" value="Select" ano="' + $(this).children("com_empno").text().trim() + '" aname="'
                                    + $(this).children("com_cname").text().trim() + '" adeptid="' + $(this).children("com_deptid").text().trim() + '">';
                                tabstr += '</td></tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="4">Not Found</td></tr>';
                        $("#CommonList tbody").append(tabstr);
                        CommonPage.Option.Selector = "#com_pageblock";
                        CommonPage.CreatePage(p, $("total", data).text());
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
        <!--Header-->
        <div class="wrapper">
            <div class="header header-sticky">
                <div class="topbar">
                    <div class="container">
                        <ul class="loginbar pull-right">
                            <li><a href="https://itriweb.itri.org.tw/" target="_blank">itriweb</a></li>
                            <li class="topbar-devider"></li>
                            <li><a href="https://msx.itri.org.tw/owa/auth/logon.aspx" target="_blank">itrimail</a></li>
                            <li class="topbar-devider"></li>
                            <li><a href="https://empfinder.itri.org.tw/WebPage/ED_QueryIndex.aspx" target="_blank">Search itri employee</a></li>
                        </ul>
                    </div>
                </div>
                <div class="navbar navbar-default mega-menu" role="navigation">
                    <div class="container">
                        <div class="navbar-header">
                            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-responsive-collapse"><span class="sr-only">Toggle navigation</span><span class="fa fa-bars"></span></button><a class="navbar-brand" href="/project/default.aspx"><img id="logo-header" src="../images/logo.png" alt="Logo"></a></div>
                        <div class="collapse navbar-collapse navbar-responsive-collapse">
                            <ul class="nav navbar-nav">
                                <li>
                                    <div class="textcenter font-size6 deskonly ochimenuicon">
                                        <a href="/project/default.aspx" style="text-decoration: none;">
                                            <i class="iekeif-iek_list"><span class="path1"></span><span class="path2"></span><span class="path3"></span><span class="path4"></span><span class="path5"></span><span class="path6"></span></i>
                                        </a>
                                    </div>
                                    <a href="/project/default.aspx">Project List</a></li>
                                <li class="active">
                                    <div class="textcenter font-size6 deskonly ochimenuicon">
                                        <a href="/projectMgmt/default.aspx" style="text-decoration: none;">
                                            <i class="iekeif-iek_tool"><span class="path1"></span><span class="path2"></span><span class="path3"></span><span class="path4"></span></i>
                                        </a>
                                    </div>
                                    <a href="/projectMgmt/default.aspx">Project mgmt</a></li>
                                <li>
                                    <div class="textcenter font-size6 deskonly ochimenuicon">
                                        <a href="/projectMaintain/default.aspx" style="text-decoration: none;">
                                            <i class="iekeif-iek_role"><span class="path1"></span><span class="path2"></span><span class="path3"></span></i>
                                        </a>
                                    </div>
                                    <a href="/projectMaintain/default.aspx">Project maintain</a></li>
                                <li>
                                    <div class="textcenter font-size6 deskonly ochimenuicon">
                                        <a href="/projectMaintain/default.aspx" style="text-decoration: none;">
                                            <i class="iekeif-pie-chart"><span class="path1"></span><span class="path2"></span><span class="path3"></span><span class="path4"></span></i>
                                        </a>
                                    </div>
                                    <a href="/projectMaintain/default.aspx">Data Explore</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
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
                <div class="well" style="margin-bottom:100px;">
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
                    <div id="pageblock" style="text-align:center;"></div>
                </div>

                <h4>Select Member</h4>
                <div class="btn-group">
                    <input type="text" id="comKeywordStr" class="form-control" />
                </div>
                <button type="button" class="btn btn-main" id="comSearchBtn"><i class="fa fa-search"></i>&nbsp;Search</button>
                <div class="well" style="margin-top:10px;">
                    <table id="CommonList" class="table table-striped table-hover">
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
                    <div id="com_pageblock" style="text-align:center;"></div>
                </div>
            </div>
            <!--Footer-->
            <div class="footer-default">
                <div class="copyright">
                    <div class="container">
                        <div class="row">
                            <div class="col-md-12"><p>2017 © ITRI IEK  ｜Contact us：03-5912293</p></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
