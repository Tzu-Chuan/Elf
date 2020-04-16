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
        <!--**************************************************************************-->
        <!--//////head start-->
        
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
        <link rel="stylesheet" href="../assets/plugins/sky-forms/css/custom-sky-forms.css"/>
        <!--=====表單物件的元件-->

        <!--=====CSS Theme -->
        <!--<link rel="stylesheet" href="../assets/css/themeColors-default.css">-->

        <!--=====CSS Customization -->
        <link rel="stylesheet" href="../assets/css/custom.css"/>

	<!--=====CSS Customization by ochison  -->
        <link rel="stylesheet" href="../assets/css/iekicon.css"/>
        <link rel="stylesheet" href="../assets/css/ochi.css"/>
        <!--//////head end-->
        <!--**************************************************************************-->
      </head>

      <body class="header-fixed boxed-layout">
        <div class="wrapper">
          <!--**************************************************************************-->
          <!--//////標頭start-->
          <xsl:call-template name ="topbar"/>
          <!--//////標頭end-->
          <!--**************************************************************************-->


          <!--**************************************************************************-->
          <!--//////Container Part start-->
          <div class="container">
            <form name="Form1" method="get">
              <input type="hidden" name="currentPageIndex" value="{$pagerCurrentPageIndex}"/>
            </form>

            <div class="content">
              <!--=====關鍵字搜尋-->
              <!--<div class="margin-bottom-10 animated fadeInDown">-->
              <div class="margin-bottom-10 animated">
                <!--查詢-->
                <div class="btn-group">
                  <div class="btn-group">
                    <input type="text" id="keyword" name="keyword" value="" onkeypress="" class="form-control" placeholder="Please input keyword" />
                  </div>
                  <button type="button" class="btn btn-main" onclick="doSearch();return false;">
                    <i class="fa fa-search"></i>&nbsp;Search
                  </button>
                </div>
              </div>

              <!--PageList-->
              <div class="well">
                <table class="table table-striped table-hover">
                  <thead>
                    <tr>
                      <th nowrap="">NO.</th>
                      <th>
                        Project Name
                      </th>
                      <th>
                        Tech Item
                      </th>
                      <th style="width:90px;">
                        Status
                      </th>
                      <th>
                        Owner
                      </th>
                      <th>
                        Create Time
                      </th>
                      <th>
                        Closed Time
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
                          <a href="./projectDetail.aspx?pjGuid={@project_guid}">
                            <xsl:value-of select="@project_name"/>
                          </a>
                        </td>
                        <td>
                            <xsl:value-of select="@technology"/>
                        </td>
                        <td>
                          <span class="label rounded-2x label-info col-md-9">
                            <xsl:choose>
                              <!--/*X=1,匯入,青色*/-->
                              <xsl:when test="(@status=1)">
                                <xsl:attribute name="class">label rounded-2x label-info col-sm-12 col-md-9</xsl:attribute>
                              </xsl:when>
                              
                              <!--/*10<=X<=22,更新中,橘色*/-->
                              <xsl:when test="(@status>=10 and @status&lt;=22)">
                                <xsl:attribute name="class">label rounded-2x label-orange col-sm-12 col-md-9</xsl:attribute>
                              </xsl:when>
                              
                              <!--/*X=40,已建置,綠色*/-->
                              <xsl:when test="(@status=40)">
                                <xsl:attribute name="class">label rounded-2x label-success col-sm-12 col-md-9</xsl:attribute>
                              </xsl:when>
                              
                              <!--/*X=50,已結案,黑色*/-->
                              <xsl:when test="(@status=50)">
                                <xsl:attribute name="class">label rounded-2x label-dark col-sm-12 col-md-9</xsl:attribute>
                              </xsl:when>
                              
                              <!--/*60<=X<=69,異常,紅色*/-->
                              <xsl:when test="(@status>=60 and @status&lt;=69)">
                                <xsl:attribute name="class">label rounded-2x label-danger col-sm-12 col-md-9</xsl:attribute>
                              </xsl:when>
                            </xsl:choose>
                            <xsl:value-of select="@status_en_name"/>
                          </span>
                        </td>
                        <td>
                          <xsl:value-of select="@owner"/>
                        </td>
                        <td>
                          <xsl:value-of select="@create_time"/>
                        </td>
                        <td>
                          <xsl:value-of select="@stop_time"/>
                        </td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>

                <!--**********************************[換頁碼]-->
                <xsl:call-template name="pager"/>

                <!--/*專案狀態圖示說明*/-->
                <p class="text-info">
                  <i class="fa fa-info-circle"></i>Status Explain：
                </p>
                <table>
                  <tr>
                    <td>
                      <span class="label rounded-2x label-info col-md-9">Inception</span>：
                    </td>
                    <td>
                      The project is initiated and will not be analyzed before STARTing it. The project manager can still DELETE the project at this phase.
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <span class="label rounded-2x label-orange col-md-9">Analyzing</span>：
                    </td>
                    <td>
                      The tech item related articles are being searched, categorized, ranked, and recommended. The outcome will be ready for reading after finishing the first time of analyzing.
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <span class="label rounded-2x label-success col-md-9">Ready</span>：
                    </td>
                    <td>
                      The project is ready to be read.
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <span class="label rounded-2x label-dark col-md-9">Closed</span>：
                    </td>
                    <td>
                      The project is closed. Data won’t be updated after being closed.
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <span class="label rounded-2x label-danger col-md-9">Error</span>：
                    </td>
                    <td>
                      Please contact manager to get correction.
                    </td>
                  </tr>
                </table>
          
              </div>
              <!--/well-->
            </div>
            <!--/content-->
          </div>
          <!--/container-->
          <!--//////Container Part end-->
          <!--**************************************************************************-->


          <!--**************************************************************************-->
          <!--//////頁腳start-->
          <xsl:call-template name ="footer"/>
          <!--//////頁腳end-->
          <!--**************************************************************************-->


        </div>

        <!--**************************************************************************-->
        <!--//////javascript start-->
        <!--/wrapper-->
        <!-- JS Global Compulsory -->
        <!--<script type="text/javascript" src="../assets/plugins/jquery/jquery.min.js"></script>-->
        <script type="text/javascript" src="../assets/plugins/jquery/jquery.js"></script>
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

          function doSearch()
          {
          location.href = "default.aspx?q=" + encodeURIComponent($('#keyword').val());
          }
        </script>

        <!--===my js-->
        <!--<script type="text/javascript" src="default.js"></script>-->

        <!--//////javascript end-->
        <!--**************************************************************************-->
      </body>
    </html>


  </xsl:template>
</xsl:stylesheet>
