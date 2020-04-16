<?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE stylesheet[
  <!ENTITY nbsp "&#160;">
  <!ENTITY copy "&#169;">
  <!ENTITY times "&#215;">
  <!ENTITY emsp "&#8195;">
]>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../templates/pager.xslt"/>
  <xsl:import href="../templates/topbar.xslt"/>
  <xsl:import href="../templates/footer.xslt"/>
  <xsl:output method="html" indent="yes" standalone="yes" omit-xml-declaration="yes" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />

  <!--=======================
param：
========================-->
  <xsl:param name="AppRoot" />
  <xsl:param name="AppTitle" />
  <xsl:param name="xmlList" />

  <!--=======================
template：
========================-->
  <xsl:template match="/">
    <html>
      <head>
        <!--/////////////////////////////////-->
        <!--=====head-->

        <title>
          <xsl:value-of select="$AppTitle"/>
        </title>
        <!--=====Meta-->
        <meta charset="utf-8"/>
        <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
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
        <link rel="stylesheet" href="../assets/plugins/cube-portfolio/css/cubeportfolio.css"/>
        <!--篩選的元件-->
        <link rel="stylesheet" href="../assets/plugins/sky-forms/css/custom-sky-forms.css"/>
        <!--=====表單物件的元件-->

        <!-- CSS Theme -->
        <link rel="stylesheet" href="../assets/css/themeColors-default.css"/>

        <!--=====CSS Customization -->
        <link rel="stylesheet" href="../assets/css/custom.css"/>

	<!--=====CSS Customization by ochison  -->
        <link rel="stylesheet" href="../assets/css/iekicon.css"/>
        <link rel="stylesheet" href="../assets/css/ochi.css"/>
        <!--/////////////////////////////////-->
      </head>

      <body class="header-fixed boxed-layout">
        <div class="wrapper">
          <!--/////////////////////////////////-->
          <!--=====標頭-->
          <xsl:call-template name ="topbar"/>
          <!--/////////////////////////////////-->


          <!--/////////////////////////////////-->
          <!--=====遮罩-->
          <xsl:call-template name ="subAddMember"/>
          <!--/////////////////////////////////-->


          <!--/////////////////////////////////-->
          <!--Container Part start-->
          <div class="container">
            <form name="Form1" method="get">
              <input type="hidden" name="currentPageIndex" value="{$pagerCurrentPageIndex}"/>
            </form>

            <div class="content">
              <!--=====關鍵字搜尋-->
              <!--<div class="margin-bottom-10 animated fadeInDown">-->
              <div class="margin-bottom-10 animated">
                <!--新增-->
                <div class="col-md-4 col-md-push-8 text-right">
                  <a href="#AddManager" data-toggle="modal" data-target="#AddManager" class="btn-u btn-u-sea btn-u-lg">Add manager</a>
                </div>
                <!--查詢-->
                <div class="col-md-8 col-md-pull-4">
                  <div class="btn-group">
                    <div class="btn-group">
                      <input type="text" id="keyword" name="keyword" value="" onkeypress="" class="form-control" placeholder="Please input keyword" />
                    </div>
                    <button type="button" class="btn btn-main" onclick="doSearch();return false;">
                      <i class="fa fa-search"></i>&nbsp;Search
                    </button>
                  </div>
                </div>
                <div class="clearfix"></div>
              </div>

              <!--PageList-->
              <div class="well">
                <table class="table table-striped table-hover">
                  <thead>
                    <tr>
                      <th nowrap="">NO.</th>
                      <th nowrap="">
                        Role&nbsp;<a href="#">
                          <i class="fa fa-angle-down"></i>
                        </a>
                      </th>
                      <th nowrap="">
                        Deptid&nbsp;<a href="#">
                          <i class="fa fa-angle-up"></i>
                        </a>
                      </th>
                      <th nowrap="">
                        Name&nbsp;<a href="#">
                          <i class="fa fa-angle-up"></i>
                        </a>
                      </th>
                      <th nowrap="">
                        Create Time&nbsp;<a href="#">
                          <i class="fa fa-angle-up"></i>
                        </a>
                      </th>
                      <th nowrap="">
                        Function
                      </th>
                    </tr>
                  </thead>
                  <tbody>
                    <xsl:for-each select="$xmlList/*">
                      <tr>
                        <td nowrap="nowrap">
                          <xsl:value-of select="@No"/>
                        </td>
                        <td>
                          <xsl:value-of select="@role_enname"/>
                        </td>
                        <td>
                          <xsl:value-of select="@deptid"/>
                        </td>
                        <td>
                          <xsl:value-of select="@empname"/>(<xsl:value-of select="@empno"/>)
                        </td>
                        <td>
                          <xsl:value-of select="@create_time"/>
                        </td>
                        <!--<td class="text-right">-->
                        <td>
                          <!--
                          <a href="#會員編輯" data-target="#會員編輯" data-toggle="modal" class="btn-u btn-u-sea">Delete</a>
                          -->
                          <button type="button" class="btn-u btn-u-default" onclick="doEmpDelete('{@manager_guid}')">
                            <i class="fa fa-trash-o"></i>&nbsp;Delete
                          </button>
                        </td>
                      </tr>

                    </xsl:for-each>
                  </tbody>
                </table>

                <!--/////////////////////////////////-->
                <!--=====換頁碼-->
                <xsl:call-template name="pager"/>
                <!--/////////////////////////////////-->

              </div>
              <!--/well-->
            </div>
            <!--/content-->
          </div>
          <!--/container-->
          <!--Container Part end-->
          <!--/////////////////////////////////-->


          <!--/////////////////////////////////-->
          <!--=====頁腳-->
          <xsl:call-template name ="footer"/>
          <!--/////////////////////////////////-->


        </div>



        <!--/////////////////////////////////-->
        <!--=====javascript-->
        <!--/wrapper-->
        <!-- JS Global Compulsory -->
        <script type="text/javascript" src="../assets/plugins/jquery/jquery.min.js"></script>
        <script type="text/javascript" src="../assets/plugins/jquery/jquery-migrate.min.js"></script>
        <script type="text/javascript" src="../assets/plugins/bootstrap/js/bootstrap.min.js"></script>
        <!-- JS Implementing Plugins -->
        <script type="text/javascript" src="../assets/plugins/back-to-top.js"></script>
        <!--<script type="text/javascript" src="../assets/plugins/cube-portfolio/js/jquery.cubeportfolio.min.js"></script>-->
        <!-- JS Customization -->
        <script type="text/javascript" src="../assets/js/custom.js"></script>
        <!-- JS Page Level -->
        <script type="text/javascript" src="../assets/js/app.js"></script>
        <!--<script type="text/javascript" src="../assets/js/plugins/cube-portfolio.js"></script>-->
        <script type="text/javascript">
          jQuery(document).ready(function () {
          });
        </script>
        <!--[if lt IE 9]>
        <script src="../assets/plugins/respond.js"></script>
        <script src="../assets/plugins/html5shiv.js"></script>
        <script src="../assets/js/plugins/placeholder-IE-fixes.js"></script>
        <![endif]-->

        <script type="text/javascript">
          function on_PageChanged( page )
          {
          document.Form1.currentPageIndex.value=page;
          document.Form1.submit();
          }
        </script>

        <!--===my js-->
        <script type="text/javascript" src="default_old.js"></script>

        <!--/////////////////////////////////-->
      </body>
    </html>


  </xsl:template>

  
  
  
  
  <!--=======================
template：
========================-->
  <xsl:template name="subAddMember">
    <!-- 遮罩：新增管理人員 -->
    <div class="modal fade" id="AddManager" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
      <div class="modal-dialog ">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
            <h2 class="modal-title" id="myModalLabel">Add manager</h2>
          </div>
          <div class="modal-body">

            <form action="" class="sky-form">

              <p class="text-info">
                <i class="fa fa-info-circle"></i>
                【System Manager】Right：1.add project manager、2.add project、3.maintain all projects<br />
                <i class="fa fa-info-circle"></i>
                【Project Manager】Right：1.add project、2.maintain project
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

                <input type="text" id="empname1" name="empname1"  value="" size="30" maxlength="20" readonly="readonly" placeholder="Please select empno" class="form-control"/>
                <input type="hidden" id="empno1" name="empno1" value="" size="20"/>
                <input type="hidden" id="empgroup1" name="empgroup1" value="" size="20"/>
                <input type="hidden" id="emptitle1" name="emptitle1" value="" size="20"/>
                <input type="hidden" id="com_cname" name="com_cname" value="" size="20"/>
                <input type="hidden" id="com_empno" name="com_empno" value="" size="20"/>
                <input type="hidden" id="com_email" name="com_email" value="" size="20"/>
                <input type="hidden" id="com_deptid" name="com_deptid" size="20"/>
                <input type="hidden" id="com_orgcd" name="com_orgcd" value="" size="20"/>

                <span class="input-group-btn">
                  <button type="button" class="btn btn-guidence" onclick="getITRIemp('1',''); return false;">
                    <i class="fa fa-binoculars"></i>
                  </button>
                </span>

              </div>
              <!--/input-group-->
            </form>
          </div>
          <div class="modal-footer">
            <!--<button type="button" class="btn-u btn-u-red">取　消</button>-->
            <button type="button" class="btn-u btn-u-sea" onclick="doEmpAdd(this);return false;">Send</button>
          </div>
        </div>
      </div>
    </div>
  </xsl:template>

</xsl:stylesheet>
