<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="projectMaintain_default" %>

<!DOCTYPE html>

<html>
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
<!--===my js-->
<script type="text/javascript" src="../js/PageList.js"></script>
<script type="text/javascript" src="../js/jquery-ui.1.12.1.js"></script>
<head>
    <title>IEKElf</title>
    <script type="text/javascript">
        $(document).ready(function () {
            //分頁設定
            Page.Option.SortMethod = "-";
            Page.Option.SortName = "create_time";

            getData(0);

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

            /// 員工列表 表頭排序
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

            // 新增管理員跳窗
            $(document).on("click", "#newManager", function () {
                $("#addemp").val("");
                $("#tmpEmpNo").val("");
                $("#tmpEmpName").val("");
                $("#tmpOrgcd").val("");
                $("#tmpDeptid").val("");
                $("#AddManager").modal("show");
            });

            
            // 新增管理員
            $(document).on("click", "#sendbtn", function () {
                if ($("#addemp").val() != "") {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "maintainHandler/addManager.aspx",
                        data: {
                            role_id: $("#role_id").val(),
                            orgcd: $("#tmpOrgcd").val(),
                            empno: $("#tmpEmpNo").val(),
                            name: $("#tmpEmpName").val(),
                            deptid: $("#tmpDeptid").val()
                        },
                        error: function (xhr) {
                            alert(xhr.responseText);
                        },
                        success: function (data) {
                            if ($(data).find("Error").length > 0) {
                                alert($(data).find("Error").attr("Message"));
                            }
                            else {
                                Page.Option.SortMethod = "-";
                                Page.Option.SortName = "create_time";
                                getData(0);
                                $('#AddManager').modal('hide');
                            }
                        }
                    });
                }
            });

            // 查詢
            $(document).on("click", "#SearchBtn", function () {
                Page.Option.SortMethod = "-";
                Page.Option.SortName = "create_time";
                getData(0);
            });

            $(document).on("keypress", "#emp_keyword", function (e) {
                if ((e.keyCode == 13) || (e.key == "Enter") || (e.code == "Enter")) {
                    try {
                        e.stopPropagation();
                        e.preventDefault();
                    }
                    catch (err) {
                        e.cancelBubble = true;
                    }
                    getEmployeeList(0);
                    return false;
                }
            });

            // 刪除人員
            $(document).on("click", "a[name='DeleteBtn']", function () {
                if (confirm("Confirm to delete this manager?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "maintainHandler/deleteManager.aspx",
                        data: {
                            gid: $(this).attr("mguid")
                        },
                        error: function (xhr) {
                            alert(xhr.responseText);
                        },
                        success: function (data) {
                            if ($(data).find("Error").length > 0) {
                                alert($(data).find("Error").attr("Message"));
                            }
                            else {
                                Page.Option.SortMethod = "-";
                                Page.Option.SortName = "create_time";
                                getData(0);
                            }
                        }
                    });
                }
            });

            // 查詢員工跳窗
            $(document).on("click", "#EmpListBtn", function () {
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
            
            // 選擇員工
            $(document).on("click", "a[name='checkbtn']", function () {
                var stremp = $(this).attr("empname") + "(" + $(this).text() + ")";
                $("#addemp").val(stremp);
                $("#tmpEmpNo").val($(this).text());
                $("#tmpEmpName").val($(this).attr("empname"));
                $("#tmpOrgcd").val($(this).attr("orgcd"));
                $("#tmpDeptid").val($(this).attr("deptid"));
                $("#EmpList").modal("hide");
            });

        });// end js

        function getData(p) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "maintainHandler/GetManagerList.aspx",
                data: {
                    PageNo: p,
                    keyword: $("#keyword").val(),
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
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("role_id").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("deptid").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("empname").text().trim() + '(' + $(this).children("empno").text().trim() + ')</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("create_time").text().trim())) + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap"><a name="DeleteBtn" href="javascript:void(0);" mguid="'+$(this).children("manager_guid").text().trim()+'" class="btn-u btn-u-default"><i class="fa fa-trash-o"></i>&nbsp;Delete</a></td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="6">查詢無資料</td></tr>';

                        Page.Option.FunctionName = "getData";
                        $("#tablist tbody").empty();
                        $("#tablist tbody").append(tabstr);
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
                    mode: "manager",
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
                                tabstr += '<td align="center" nowrap="nowrap"><a name="checkbtn" href="javascript:void(0);" orgcd="' + $(this).children("com_orgcd").text().trim() + '" empname="'
                                    + $(this).children("com_cname").text().trim() + '" deptid="' + $(this).children("com_deptid").text().trim() + '">' + $(this).children("com_empno").text().trim() + '</a></td>';
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
</head>
<body class="header-fixed boxed-layout">
    <div class="wrapper">
         <!--#include file="../templates/Header.html"-->
        <div class="container">
            <div class="content">
              <!--=====關鍵字搜尋-->
              <div class="margin-bottom-10 animated">
                <!--新增-->
                <div class="col-md-4 col-md-push-8 text-right">
                  <a id="newManager" href="javascript:void(0);" class="btn-u btn-u-sea btn-u-lg">Add manager</a>
                </div>
                <!--查詢-->
                <div class="col-md-8 col-md-pull-4">
                  <div class="btn-group">
                    <div class="btn-group">
                      <input type="text" id="keyword" name="keyword" value="" onkeypress="" class="form-control" placeholder="Please input keyword" />
                    </div>
                    <a href="javascript:void(0);" class="btn btn-main" id="SearchBtn"><i class="fa fa-search"></i>&nbsp;Search</a>
                  </div>
                </div>
                <div class="clearfix"></div>
              </div>

              <!--PageList-->
              <div class="well">
                  <div style="overflow:auto;">
                      <table id="tablist" class="table table-striped table-hover">
                          <thead>
                              <tr>
                                  <th nowrap="" style="text-align: center;">NO.</th>
                                  <th nowrap="" style="text-align: center;">Role&nbsp;<a href="javascript:void(0);" name="sortbtn" sortname="role_id"><i class="fa fa-angle-up"></i></a></th>
                                  <th nowrap="" style="text-align: center;">Deptid&nbsp;<a href="javascript:void(0);" name="sortbtn" sortname="deptid"><i class="fa fa-angle-up"></i></a></th>
                                  <th nowrap="" style="text-align: center;">Name&nbsp;<a href="javascript:void(0);" name="sortbtn" sortname="empno"><i class="fa fa-angle-up"></i></a></th>
                                  <th nowrap="" style="text-align: center;">Create Time&nbsp;<a href="javascript:void(0);" name="sortbtn" sortname="create_time"><i class="fa fa-angle-down"></i></a></th>
                                  <th nowrap="" style="text-align: center;">Function</th>
                              </tr>
                          </thead>
                          <tbody></tbody>
                      </table>
                  </div>
                  <div id="pageblock" style="text-align:center;"></div>
              </div><!--/well-->
            </div><!--/content-->
          </div>

         <!--#include file="../templates/Footer.html"-->
    </div>

    <div class="modal fade" id="AddManager" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h2 class="modal-title" id="myModalLabel">Add manager</h2>
                </div>
                <div class="modal-body">
                    <form action="" class="sky-form">
                        <p class="text-info">
                            <i class="fa fa-info-circle" id="ss" title="123123"></i>【System Manager】Right：1.add project manager、2.add project、3.maintain all projects<br />
                            <i class="fa fa-info-circle"></i>【Project Manager】Right：1.add project、2.maintain project
                        </p>

                        <div class="input-group">
                            <span class="input-group-addon">Role</span>
                            <select id="role_id" name="member" class="form-control">
                                <option value="sys_mgr">System Mgr</option>
                                <option value="pj_mgr">Project Mgr</option>
                            </select>
                        </div>

                        <!--/input-group-->
                        <div class="input-group">
                            <span class="input-group-addon">Empno</span>

                            <input type="text" id="addemp"  value="" size="30" maxlength="20" readonly="readonly" placeholder="Please select empno" class="form-control" />
                            <input id="tmpEmpNo" type="hidden" />
                            <input id="tmpEmpName" type="hidden" />
                            <input id="tmpOrgcd" type="hidden" />
                            <input id="tmpDeptid" type="hidden" />
                            <span class="input-group-btn">
                                <button type="button" class="btn btn-guidence" id="EmpListBtn"><i class="fa fa-binoculars"></i></button>
                            </span>

                        </div>
                        <!--/input-group-->
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn-u btn-u-sea" id="sendbtn">Send</button>
                </div>
            </div>
        </div>
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
</body>
</html>
