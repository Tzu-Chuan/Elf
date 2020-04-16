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
  <xsl:param name="xmlList"></xsl:param>

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
          <!--//////遮罩start-->
          <xsl:call-template name ="subFunction_01"/>
          <xsl:call-template name ="subFunction_02"/>
          <!--//////遮罩end-->
          <!--**************************************************************************-->


          <!--**************************************************************************-->
          <!--//////Container Part start-->
          <div class="container">
            <form name="Form1" method="get">
              <input type="hidden" name="currentPageIndex" value="{$pagerCurrentPageIndex}"/>
            </form>

            <div class="content">
              <!--<div class="row margin-bottom-10 animated fadeInDown">-->
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
                    <button type="button" class="btn btn-main" onclick="doSearch();return false;">
                      <i class="fa fa-search"></i>&nbsp;Search
                    </button>
                  </div>
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
                      <th>Function</th>
                    </tr>
                  </thead>
                  <tbody>
                    <xsl:for-each select="$xmlList/*">
                      <tr>
                        <td nowrap="nowrap">
                          <xsl:value-of select="@No"/>
                        </td>
                        <td>
                          <a href="../project/projectDetail.aspx?pjGuid={@project_guid}">
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
                          <div class="text-nowrap">
                            <xsl:value-of select="@create_time"/>
                          </div>
                        </td>
                        <td>
                          <div class="text-nowrap">
                            <xsl:value-of select="@stop_time"/>
                          </div>
                        </td>
                        <td>
                          <!--/*專案匯入階段*/-->
                          <xsl:if test="(@status=1)">
                            <button type="button" class="btn-u btn-u-red margin-right-5" onclick="doPjStart('{@project_guid}')">
                              <i class="fa  fa-power-off margin-right-5"></i>Start
                            </button>
                          </xsl:if>
                          <!--/*專案匯入階段*/-->
                          <xsl:if test="(@status=1)">
                            <button type="button" class="btn-u btn-u-default margin-right-5" onclick="doPjDelete('{@project_guid}')">
                              <i class="fa fa-trash-o margin-right-5"></i>Delete
                            </button>
                          </xsl:if>
                          <!--/*專案已建置階段*/-->
                          <xsl:if test="(@status=40)">
                            <button type="button" class="btn-u btn-u-sea margin-right-5" onclick="doPjClose('{@project_guid}')">
                              <i class="fa  fa-power-off margin-right-5"></i>Close
                            </button>
                          </xsl:if>
                          <!--/*專入匯入,已建置階段*/-->
                          <xsl:if test="(@status!=1 and @status!=50)">
                            <button type="button" class="btn-u btn-u-dark-blue margin-right-5" onclick="doRelatedWordMaintain('{@project_guid}')">
                              <i class="fa fa-wrench margin-right-5"></i>Maintain
                            </button>
                          </xsl:if>
                          <!--/*不分階段*/-->
                          <button type="button" class="btn-u btn-u-dark margin-right-5" onclick="doExport('{@project_guid}')">
                            <i class="fa fa-tasks margin-right-5"></i>Export
                          </button>
                          <xsl:if test="(@compstatus>0)">
                            <a class="btn-u margin-right-5" href="MemberManage.aspx?pj={@project_guid}">
                              <i class="fa fa-wrench margin-right-5"></i>Member
                            </a>
                          </xsl:if>
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

        <!--//////javascript end-->
        <!--**************************************************************************-->
      </body>
    </html>
  </xsl:template>





  <!--=======================
template：
========================-->
  <xsl:template name="subFunction_01">
    <!-- 遮罩：匯入專案01 -->
    <div class="modal fade" id="InitiateProject_01" tabindex="-1" role="dialog" aria-labelledby="myModalLabel_01" aria-hidden="true">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="close">
              <span aria-hidden="true">&times;</span>
            </button>
            <h2 class="modal-title" id="myModalLabel_01">Step1：Upload Project Excel</h2>
          </div>
          <div class="modal-body">

            <!--<h3>Upload a excel file：</h3>-->

            <form id="formFileUplad" name="formFileUplad" method="post" encType="multipart/form-data">
              <!-- ===btn -->
              <div class="col-sm-12 col-md-12 text-right">
                <input type="submit" value="Cancel" id="btn_01_cencel" class="btn-u btn-u-lg btn-u-sea" style="margin-right:10px;"/>
                <input type="submit" value="NextStep" id="btn_01_next" class="btn-u btn-u-lg btn-u-sea"/>
              </div>

              <!-- ===msg -->
              <div id="message_01" style="color:red;"></div>
              <br/>

              <!-- ===template -->
              <h3>download template excel：</h3>
              <a href="../dl/excelTemplate.aspx" target="_self">template v201910</a> (view <a href="../dl/excelSampleTemplate.aspx" target="_self">sample</a>)
              <br/>
              <br/>

              <!-- ===choose excel -->
              <h3>choose project excel：</h3>
              <input type="file" id="file1" name="file1" size="50"/>
            </form>
            
            <br/>
            <br/>

          </div>
        </div>
      </div>
    </div>
  </xsl:template>



  <xsl:template name="subFunction_02">
    <!-- 遮罩：匯入專案02 -->
    <div class="modal fade" id="InitiateProject_02" tabindex="-1" role="dialog" aria-labelledby="myModalLabel_02" aria-hidden="true">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="close">
              <span aria-hidden="true">&times;</span>
            </button>
            <h2 class="modal-title" id="myModalLabel_02">Step2：Check Excel</h2>
          </div>
          <div class="modal-body">

            <!-- ===btn -->
            <div class="col-sm-12 col-md-12 text-right">
              <input type="submit" value="Cancel" id="btn_02a_cencel" class="btn-u btn-u-lg btn-u-sea" style="margin-right:10px;"/>
              <input type="submit" value="Save" id="btn_02a_save" class="btn-u btn-u-lg btn-u-sea"/>
            </div>

            <!-- ===msg -->
            <div id="message_02" style="color:red;"></div>
            <br/>

            <!-- ===excel content -->
            <div id="excelContentBlock_02"></div>

            <!-- ===btn -->
            <div class="col-sm-12 col-md-12 text-right">
              <input type="submit" value="Cancel" id="btn_02b_cencel" class="btn-u btn-u-lg btn-u-sea" style="margin-right:10px;"/>
              <input type="submit" value="Save" id="btn_02b_save" class="btn-u btn-u-lg btn-u-sea"/>
            </div>


            <br/>
            <br/>

          </div>
        </div>
      </div>
    </div>
  </xsl:template>


</xsl:stylesheet>