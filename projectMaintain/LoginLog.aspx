<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginLog.aspx.cs" Inherits="projectMaintain_LoginLog" %>

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
<script type="text/javascript" src="../js/NickCommon.js"></script>
<script type="text/javascript" src="../js/jquery-ui.1.12.1.js"></script>
<head>
	<title>IEKElf</title>
	<script type="text/javascript">
		$(document).ready(function () {
			getData(0);

			// 查詢
			$(document).on("click", "#SearchBtn", function () {
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
					getData(0);
					return false;
				}
			});
		});

		function getData(p) {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "maintainHandler/GetLoginLog.aspx",
				data: {
					PageNo: p,
					keyword: $("#emp_keyword").val()
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
								tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("log_userdeptid").text().trim() + '</td>';
								tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("log_empno").text().trim() + '</td>';
								tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("com_cname").text().trim() + '</td>';
								tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("log_ip").text().trim() + '</td>';
								tabstr += '<td align="center" nowrap="nowrap">' + $.datepicker.formatDate('yy-mm-dd', new Date($(this).children("log_datetime").text().trim())) + '&nbsp;&nbsp;' + $.FormatTime($(this).children("log_datetime").text().trim()) + '</td>';
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
	</script>
</head>
<body>
    <div class="wrapper">
         <!--#include file="../templates/Header.html"-->
        <div class="container">
            <div class="content">
              <!--=====關鍵字搜尋-->
              <div class="margin-bottom-10 animated">
                <!--查詢-->
                <div class="col-md-12">
                  <div class="btn-group">
                    <div class="btn-group">
                      <input type="text" id="emp_keyword" name="keyword" value="" onkeypress="" class="form-control" placeholder="Please input Empno" />
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
                                  <th nowrap="" style="text-align: center;">Deptid</th>
                                  <th nowrap="" style="text-align: center;">Empno</th>
                                  <th nowrap="" style="text-align: center;">Name</th>
                                  <th nowrap="" style="text-align: center;">IP</th>
                                  <th nowrap="" style="text-align: center;">Login Time</th>
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
</body>
</html>
