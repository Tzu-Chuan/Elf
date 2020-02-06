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
  <xsl:param name="xmlProject"></xsl:param>
  <xsl:param name="xmlWebsiteList"></xsl:param>
  <xsl:param name="xmlDirectionList"></xsl:param>
  <xsl:param name="xmlAskComList"></xsl:param>
  <xsl:param name="xmlMyTagList"></xsl:param>

  <xsl:param name="pjGuid"></xsl:param>
  <xsl:param name="def_date0"></xsl:param>
  <xsl:param name="def_viewMode"></xsl:param>
  <xsl:param name="def_researchGuid"></xsl:param>
  <xsl:param name="def_myTag"></xsl:param>

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

        <!--======CSS Implementing Plugins -->
        <link rel="stylesheet" href="../assets/plugins/font-awesome/css/font-awesome.min.css"/>
        <!--icon的元件-->
        <link rel="stylesheet" href="../assets/plugins/sky-forms/css/custom-sky-forms.css"/>
        <!--=====表單物件的元件-->

        <!--=====CSS Theme -->
        <!--<link rel="stylesheet" href="../assets/css/themeColors-default.css">-->

        <!--=====CSS Customization -->
        <link rel="stylesheet" href="../assets/css/custom.css"/>

        <!--=====CSS Customization by ochison  -->
        <link rel="stylesheet" href="../assets/css/iekicon.css"/>
        <link rel="stylesheet" href="../assets/css/scrollbar.css"/>
        <link rel="stylesheet" href="../assets/css/ochi.css"/>
        <!--=====文字雲pop-->
        <!--
        <style>
          div.tooltip {
  position: absolute;
  text-align: center;
  width: 60px;
  height: 28px;
  padding: 2px;
  font: 12px sans-serif;
  background: lightsteelblue;
  border: 0px;
  border-radius: 8px;
  pointer-events: none;
}
        </style>-->

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
          <xsl:call-template name ="MyTemplate_01"/>
          <xsl:call-template name ="myTemplate_tagSelect"/>
          <xsl:call-template name ="myTemplate_tagMaintain"/>
          <xsl:call-template name ="myTemplate_abs"/>
          <!--//////遮罩end-->
          <!--**************************************************************************-->


          <!--**************************************************************************-->
          <!--//////麵包屑start-->
          <div class="breadcrumbs">
            <div class="container">
              <!--
              <ul class="breadcrumb">
                <li>技術項目</li>
                <li class="active">
                  <b>Silicon Photonic</b>
                </li>
              </ul>
              -->

              <div class="row padding10TB">
                <div class="col-lg-6 col-md-6 col-sm-6">
                  <span class="font-size4">Project Name：</span><b><xsl:value-of select="$xmlProject/rec/@project_name"/></b>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6">
                  <span class="font-size4">Technology item：</span><b><xsl:value-of select="$xmlProject/rec/@technology"/></b>
                </div>
              </div>

            </div>
          </div>
          <!--//////麵包屑end-->
          <!--**************************************************************************-->


          <!--**************************************************************************-->
          <!--//////Container Part start-->
          <div class="container">
            <div class="text-right margin10T">
              <a data-toggle="modal" data-target="#block_abs" class="btn-u" >Abstract</a>
            </div>
            <div class="twocol margin10T">
              <div class="left"><H4>At a glance: word cloud</H4></div>
              <!--<div class="right textright width70">
                <a href="javascript:void(0);" class="btn-u btn-u-dark itemcontrolbtn">Maintain</a>
                <form action="" method="post" enctype="multipart/form-data" id="switchsearchblock" style="display:none;" class="sky-form">
                  <div class="row">

                    (a)時間
                    <span class="col col-4">
                      <label class="select">
                        <select name="date0" id="cloud_date0">
                          <option value="0" selected="selected">Max period</option>
                          <option value="1">1 Day</option>
                          <option value="7">1 Week</option>
                          <option value="30">1 Month</option>
                          <option value="180">6 Months</option>
                          <option value="365">1 Year</option>

                        </select>
                        <i></i>
                      </label>
                    </span>

                    (b)研究方向
                    <span class="col col-4">
                      <label class="select">
                        <select name="researchGuid" id="cloud_researchGuid">
                          <option value="all" selected="selected">All research topics</option>
                          <xsl:for-each select="$xmlDirectionList/*">
                            <option value="{@research_guid}">
                              <xsl:value-of select="@name"/>
                            </option>
                          </xsl:for-each>
                        </select>
                        <i></i>
                      </label>
                    </span>

                    (c)search btn
                    <div class="col col-4">
                      <button id="searBtn" class="btn-u btn-block btn-u-dark" type="button" onclick="doClouldSearch()">
                        <i class="fa fa-search"></i>&nbsp;Search
                      </button>
                    </div>
                  </div>
                </form>
              </div>-->
            </div>

            <div class="maxheightB BoxBorderSa BoxBgWa padding5ALL">


              <div id="blockTag" class="width100" />
            </div>

            <div class="row margin20T margin10B">
              <div class="col-sm-12">
                <!--<div class="col-lg-7 col-md-7 col-sm-12">-->
                <div class="twocol">
                  <div class="left">
                    <H4>
                      Recommended articles from monitored websites 
                      <i class="fa fa-info-circle white-tooltip" id="WebsiteDesc" data-html="true" style="cursor: pointer;"></i>
                    </H4>
                  </div>
                  <div class="right"></div><!-- right -->
                </div><!-- twocol -->

                <div  class="BoxBorderSa maxheightA">
                  <div class="padding5ALL ">

                    <div class="gentable">
                      <table width="100%">
                        <tr>
                          <td width="70" valign="middle">Resources</td>
                          <td>
                            <ul class="ks-cboxtags">
                              <li><input type="radio" id="checkboxTwo" value="all" class="" name="cbResource" checked="checked" /><label for="checkboxTwo">All resources</label></li>
                              <li><input type="radio" id="checkboxThree" value="site" class="" name="cbResource"/><label for="checkboxThree">By website</label></li>
                            </ul>
                          </td>
                        </tr>
                        <tr>
                          <td valign="middle">Topics</td>
                          <td>
                            <ul id="topic_ul" class="ks-cboxtags"></ul>
                          </td>
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
                          <td>
                            <ul id="tag_ul" class="ks-cboxtags"></ul>
                          </td>
                        </tr>
                      </table>
                    </div>



                    <!--(2)查詢條件========================-->
                    <form action="" method="post" enctype="multipart/form-data" id="" class="sky-form">
                      <input type="hidden" id="pjGuid" name="pjGuid" value="{$pjGuid}"/>
                      <input type="hidden" id="def_date0" name="def_date0" value="{$def_date0}"/>
                      <input type="hidden" id="def_viewMode" name="def_viewMode" value="{$def_viewMode}"/>
                      <input type="hidden" id="def_researchGuid" name="def_researchGuid" value="{$def_researchGuid}"/>
                      <input type="hidden" id="def_myTag" name="def_myTag" value="{$def_myTag}"/>
                      <input type="hidden" id="tpGuid" name="tpGuid" />
                      <input type="hidden" id="tpid" name="tpid" />


                      <!--(2a)日期-->
                      <section class="col-sm-2 col-md-2 searchitem1" style="display:none">
                        <label class="select">
                          <select name="date0" id="search_date0">
                            <option value="0" selected="selected">Max period</option>
                            <option value="1">1 Day</option>
                            <option value="7">1 Week</option>
                            <option value="30">1 Month</option>
                            <option value="180">6 Months</option>
                            <option value="365">1 Year</option>

                          </select>
                          <i></i>
                        </label>
                      </section>


                      <!--<div class="input-daterange">
                      <div class="col col-2">
                        <label class="input">
                          <i class="icon-append fa fa-calendar"></i>
                          <input type="text" name="start" id="search_date1" placeholder="Start Date" />
                        </label>
                      </div>
                      <div class="col col-2">
                        <label class="input">
                          <i class="icon-append fa fa-calendar"></i>
                          <input type="text" name="finish" id="search_date2" placeholder="Finish Date" />
                        </label>
                      </div>
                    </div>-->


                      <!--(2b)來源-->
                      <!--<section class="col-sm-3 col-md-3 searchitem1" style="display:none">
                        <label class="select">
                          <select name="viewMode" id="search_viewMode">
                            <option value="all" selected="selected">All resources</option>
                            <option value="site">By website</option>
                          </select>
                          <i></i>
                        </label>
                      </section>-->

                      <!--(2c)研究方向-->
                      <!--<section class="col-sm-3 col-md-3 searchitem1" style="display:none">
                        <label class="select">
                          <select name="researchGuid" id="search_researchGuid">
                            <option value="all" selected="selected">All topics</option>
                            <xsl:for-each select="$xmlDirectionList/*">
                              <option value="{@research_guid}">
                                <xsl:value-of select="@name"/>
                              </option>
                            </xsl:for-each>
                          </select>
                          <i></i>
                        </label>
                      </section>-->

                      <!--(2d)myTag-->
                      <!--<section class="col-sm-2 col-md-2 searchitem1" style="display:none">
                        <label class="select">
                          <select name="myTag" id="search_myTag">
                            <option value="all" selected="selected">No Tag</option>
                            <xsl:for-each select="$xmlMyTagList/*">
                              <option value="{@tagtype_guid}">
                                <xsl:value-of select="@tagtype_name"/>
                              </option>
                            </xsl:for-each>
                          </select>
                          <i></i>
                        </label>
                      </section>-->


                      <!--(2d)搜尋鍵-->
                      <!--<div class="twocol">
                        <div class="right">
                          <section class=" searchitem1">
                            <button class="btn-u btn-block btn-u-dark" type="button" onclick="doDetailSearch()">
                              <i class="fa fa-search"></i>&nbsp;Search
                            </button>
                          </section>
                        </div>
                      </div>-->



                    </form>
                    <div class="clearfix"></div>
                    <!--(3)結果========================-->
                    <!--(3a)固定來源全部-->
                    <div class="maxheightA2 scrollbar-outer">

                    <div id="myBlock_allArticles">

                      <div id="accordion_1" class="panel-group acc-v1">
                        <xsl:variable name="typeId" select="'1'"/>
                        <xsl:variable name="typeGuid" select="'all'"/>
                        <xsl:variable name="typeName" select="'all'"/>
                        <xsl:variable name="resultCount" select="$xmlProject/rec/@result_count"/>
                        <div class="panel panel-default">
                          <!--區塊-pageHead-->
                          <div class="panel-heading" id="pageHead{$typeId}{$typeGuid}">
                            <div class="pull-right col-sm-4 col-md-3 margin-top-2">
                              <!--資訊-筆數-->
                              <xsl:choose>
                                <xsl:when test="$resultCount>'0'">
                                  <span class="badge badge-sea rounded-2x col-sm-12 col-md-12">
                                    <xsl:value-of select="$resultCount"/> results
                                  </span>
                                </xsl:when>
                                <xsl:otherwise>
                                  <span class="badge badge-dark rounded-2x col-sm-12 col-md-12">
                                    <xsl:value-of select="$resultCount"/> results
                                  </span>
                                </xsl:otherwise>
                              </xsl:choose>
                            </div>

                            <!--功能-連結-->
                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion_1" href="#pageBlock{$typeId}{$typeGuid}">
                              all articles
                            </a>
                          </div>

                          <!--區塊-pageBlock-->
                          <div id="pageBlock{$typeId}{$typeGuid}" class="panel-collapse collapse in" typeId="{$typeId}" typeGuid="{$typeGuid}">
                            <!--orderBy-->
                            <div class="row">
                              <div class="btn-group margin-bottom-30 col-md-6 col-md-offset-4" role="group" aria-label="">
                                <sapn onclick="doChangeOrderField(this, 'score', '1', '{$typeId}', '{$typeGuid}');">
                                  <a name="score" href="javascript:void(0);" class="btn-u" role="button">order by Score</a>
                                </sapn>
                                <sapn onclick="doChangeOrderField(this, 'date', '1', '{$typeId}', '{$typeGuid}');">
                                  <a name="date" href="javascript:void(0);" class="btn-u btn-u-default" role="button">order by Date</a>
                                </sapn>
                                <!--<button type="button" class="btn btn-default">order by date</button>
                                    <button type="button" class="btn btn-default">order by score</button>-->
                              </div>
                            </div>

                            <!--pageData-->
                            <span id="pageData{$typeId}{$typeGuid}"/>
                          </div>
                        </div>
                      </div>
                      <br/>
                    </div>


                    <!--(3b)固定來源依網站-->
                    <div id="myBlock_articlesBySite" style="display:none">

                      <div id="accordion_2" class="panel-group acc-v1">
                        <xsl:for-each select="$xmlWebsiteList/*">
                          <xsl:variable name="typeId" select="'2'"/>
                          <xsl:variable name="typeGuid" select="@website_guid"/>
                          <xsl:variable name="typeName" select="@website_name"/>
                          <xsl:variable name="resultCount" select="@result_count"/>
                          <div class="panel panel-default">
                            <!--區塊-pageHead-->
                            <div class="panel-heading" id="pageHead{$typeId}{$typeGuid}">
                              <div class="pull-right col-sm-4 col-md-3 margin-top-2">
                                <!--資訊-筆數-->
                                <xsl:choose>
                                  <xsl:when test="$resultCount>'0'">
                                    <span class="badge badge-sea rounded-2x col-sm-12 col-md-12">
                                      <xsl:value-of select="$resultCount"/> results
                                    </span>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <span class="badge badge-dark rounded-2x col-sm-12 col-md-12">
                                      <xsl:value-of select="$resultCount"/> results
                                    </span>
                                  </xsl:otherwise>
                                </xsl:choose>
                              </div>

                              <!--功能-連結-->
                              <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion_2" href="#pageBlock{$typeId}{$typeGuid}">
                                website > <xsl:value-of select="$typeName"/>
                              </a>
                            </div>

                            <!--區塊-pageBlock-->
                            <div id="pageBlock{$typeId}{$typeGuid}" class="panel-collapse collapse" typeId="{$typeId}" typeGuid="{$typeGuid}">
                              <!--orderBy-->
                              <div class="row">
                                <div class="btn-group margin-bottom-30 col-md-4 col-md-offset-5" role="group" aria-label="">
                                  <sapn onclick="doChangeOrderField(this, 'score', '1', '{$typeId}', '{$typeGuid}');">
                                    <a name="score" href="javascript:void(0);" class="btn-u" role="button">order by Score</a>
                                  </sapn>
                                  <sapn onclick="doChangeOrderField(this, 'date', '1', '{$typeId}', '{$typeGuid}');">
                                    <a name="date" href="javascript:void(0);" class="btn-u btn-u-default" role="button">order by Date</a>
                                  </sapn>
                                  <!--<button type="button" class="btn btn-default">order by date</button>
                                    <button type="button" class="btn btn-default">order by score</button>-->
                                  <input id="hidden_Order" type="hidden"  value="score" />
                                </div>
                              </div>

                              <!--pageData-->
                              <span id="pageData{$typeId}{$typeGuid}"/>
                            </div>
                          </div>
                        </xsl:for-each>
                      </div>
                    </div>

                    </div><!-- scrollbar-outer -->


                  </div><!-- padding5ALL -->
                </div>


              </div><!-- col -->
              
              <!--Ask.com-->
              <div class="col-lg-5 col-md-5 col-sm-12" style="display:none;">
                <div class="twocol">
                  <div class="left"><H4>Search from ask.com</H4></div>
                  <div class="right"></div><!-- right -->
                </div><!-- twocol -->

                <div class="BoxBorderSa maxheightA scrollbar-outer">
                  <div class="padding5ALL">
                    <div class="twocol">
                      <!--(2e)維護askCom排程鍵-->
                      <section class="col-sm-4 col-md-6 col-sm-offset-6 col-md-offset-2 searchitem1" style="display:none;">
                        <button class="btn-u btn-block btn-u-blue" type="button" onclick="doAskComManage()">
                          <i class="fa fa-wrench"></i>&nbsp;Modulate schedule
                        </button>
                      </section>

                      <!--(2f)結束維護askCom排程鍵-->
                      <section class="col-sm-4 col-md-7 col-sm-offset-6 col-md-offset-1 searchitem2" style="display:none;">
                        <button class="btn-u btn-block btn-u-dark-blue" type="button" onclick="doEndAskComManage()">
                          <i class="fa fa-wrench"></i>&nbsp;End modulate schedule
                        </button>
                      </section>

                      <!--(2g)summary遮罩-->
                      <div class="col-sm-2 col-md-4" style="display:none;">
                        <a data-toggle="modal" data-target="#black_summary" class="btn-u btn-block btn-u-sea" >
                          <i class="fa fa-flask"></i>Summary
                        </a>
                      </div>
                    </div>
                    <!-- twocol -->

                    <!--(3c)askCom-->
                    <div id="myBlock_askComData">

                      <div id="acco rdion_3" class="panel-group acc-v1">
                        <xsl:for-each select="$xmlDirectionList/*">
                          <xsl:variable name="research_guid" select="@research_guid"/>

                          <div class="askCom_item" researchGuid="{@research_guid}">
                            <br/>
                            Research topic：<span class="color-sea">
                              <xsl:value-of select="@name"/>
                            </span>
                          </div>

                          <xsl:for-each select="$xmlAskComList/*[@research_guid=$research_guid]">
                            <xsl:variable name="typeId" select="'3'"/>
                            <xsl:variable name="typeGuid" select="@related_guid"/>
                            <xsl:variable name="typeName" select="@related_name"/>
                            <!--researchGuid：過濾研究方向用的-->
                            <xsl:variable name="researchGuid" select="@research_guid"/>
                            <xsl:variable name="researchName" select="@research_name"/>
                            <xsl:variable name="schedule" select="@schedule"/>
                            <xsl:variable name="analystGive" select="@analyst_give"/>
                            <xsl:variable name="resultCount" select="@result_count"/>

                            <div class="panel panel-default askCom_item" researchGuid="{$researchGuid}">
                              <!--區塊-pageHead-->
                              <div class="panel-heading" id="pageHead{$typeId}{$typeGuid}">
                                <div class="pull-right col-sm-6 col-md-6 margin-top-2">
                                  <!--資訊-筆數-->
                                  <xsl:choose>
                                    <xsl:when test="$resultCount>'0'">
                                      <span class="badge badge-sea rounded-2x col-sm-4 col-md-4" style="margin-left:-20px;">
                                        <xsl:value-of select="$resultCount"/> results
                                      </span>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <span class="badge badge-dark rounded-2x col-sm-4 col-md-4" style="margin-left:-20px;">
                                        <xsl:value-of select="$resultCount"/> results
                                      </span>
                                    </xsl:otherwise>
                                  </xsl:choose>

                                  <!--資訊-關鍵字來源-->
                                  <xsl:choose>
                                    <xsl:when test="$analystGive='0'">
                                      <span class="badge badge-dark rounded-2x col-sm-4 col-md-4">default</span>
                                    </xsl:when>
                                    <xsl:when test="$analystGive='1'">
                                      <span class="badge badge-dark-blue rounded-2x col-sm-4 col-md-4">customized</span>
                                    </xsl:when>
                                    <xsl:when test="$analystGive='2' or $analystGive='3'">
                                      <span class="badge badge-blue rounded-2x col-sm-4 col-md-4">learning</span>
                                    </xsl:when>
                                  </xsl:choose>

                                  <!--資訊-是否排程-->
                                  <xsl:choose>
                                    <xsl:when test="@schedule = '0'">
                                      <!--<span class="badge badge-dark rounded-2x col-sm-4 col-md-4">not schedule</span>-->
                                      <span class="col-sm-4 col-md-4">
                                        <button name="NScheBtn" aid="{@related_guid}" class="btn-u btn-u-xs btn-u-dark rounded-2x" style="margin-left:-15px;">not schedule</button>
                                      </span>
                                    </xsl:when>
                                    <xsl:when test="@schedule = '1'">
                                      <!--<span class="badge badge-purple rounded-2x col-sm-4 col-md-4">schedule</span>-->
                                      <span class="col-sm-4 col-md-4">
                                        <button name="ScheBtn" aid="{@related_guid}" class="btn-u btn-u-xs btn-u-purple rounded-2x" style="margin-left:-15px;">schedule</button>
                                      </span>
                                    </xsl:when>
                                  </xsl:choose>

                                </div>
                                <!--/.pull-right-->
                                <!--功能-連結-->
                                <div class="">
                                  <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion_3" href="#pageBlock{$typeId}{$typeGuid}"><xsl:value-of select="$typeName"/></a>
                                </div>

                              </div>
                              <!--/.panel-heading-->

                              <!--區塊-pageBlock-->
                              <div id="pageBlock{$typeId}{$typeGuid}" class="panel-collapse collapse" typeId="{$typeId}" typeGuid="{$typeGuid}">
                                <!--pageData-->
                                <span id="pageData{$typeId}{$typeGuid}"/>
                              </div>
                            </div>

                          </xsl:for-each>
                        </xsl:for-each>
                      </div>
                    </div>


                    <!--(3d)維護關連詞是否納入每日排程-->
                    <div id="myBlock_askComManage" style="display:none">
                      <div id="accordion_4" class="panel-group acc-v1">
                        <xsl:for-each select="$xmlDirectionList/*">
                          <xsl:variable name="research_guid" select="@research_guid"/>

                          <div>
                            <br/>
                            Research topic：<xsl:value-of select="@name"/>
                          </div>

                          <xsl:for-each select="$xmlAskComList/*[@research_guid=$research_guid]">
                            <xsl:variable name="typeId" select="'4'"/>
                            <xsl:variable name="typeGuid" select="@related_guid"/>
                            <xsl:variable name="typeName" select="@related_name"/>
                            <!--researchGuid：過濾研究方向用的-->
                            <xsl:variable name="researchGuid" select="@research_guid"/>
                            <xsl:variable name="researchName" select="@research_name"/>
                            <xsl:variable name="schedule" select="@schedule"/>
                            <xsl:variable name="analystGive" select="@analyst_give"/>
                            <xsl:variable name="resultCount" select="@result_count"/>

                            <div class="panel panel-default">
                              <!--區塊-pageHead-->
                              <div class="panel-heading" id="pageHead{$typeId}{$typeGuid}">

                                <div class="pull-right col-sm-4 col-md-3 margin-top-2">
                                  <!--資訊-筆數-->
                                  <xsl:choose>
                                    <xsl:when test="$resultCount>'0'">
                                      <span class="badge badge-sea rounded-2x col-sm-4 col-md-4">
                                        <xsl:value-of select="$resultCount"/> results
                                      </span>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <span class="badge badge-dark rounded-2x col-sm-4 col-md-4">
                                        <xsl:value-of select="$resultCount"/> results
                                      </span>
                                    </xsl:otherwise>
                                  </xsl:choose>

                                  <!--資訊-關鍵字來源-->
                                  <xsl:choose>
                                    <xsl:when test="$analystGive='0'">
                                      <span class="badge badge-dark rounded-2x col-sm-4 col-md-4">default</span>
                                    </xsl:when>
                                    <xsl:when test="$analystGive='1'">
                                      <span class="badge badge-dark-blue rounded-2x col-sm-4 col-md-4">customized</span>
                                    </xsl:when>
                                    <xsl:when test="$analystGive='2' or $analystGive='3'">
                                      <span class="badge badge-blue rounded-2x col-sm-4 col-md-4">learning</span>
                                    </xsl:when>
                                  </xsl:choose>

                                  <!--功能-是否排程-->
                                  <div class="pull-right">
                                    <a class="tooltips col-sm-4 col-md-3" data-original-title="關鍵字收藏" data-toggle="tooltip" data-placement="left" href="#" typeId="{$typeId}" typeGuid="{$typeGuid}" schedule="{$schedule}" onclick="doAskComUpdate(this);return false;">
                                      <xsl:choose>
                                        <xsl:when test="$schedule='0'">
                                          <i class="fa fa-heart-o"/>
                                        </xsl:when>
                                        <xsl:when test="$schedule='1'">
                                          <i class="fa fa-heart"/>
                                        </xsl:when>
                                      </xsl:choose>
                                    </a>
                                  </div>
                                </div>

                                <!--功能-連結-->
                                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion_4" href="#pageBlock{$typeId}{$typeGuid}">
                                  <xsl:value-of select="$typeName"/>
                                </a>
                              </div>

                              <!--區塊-pageBlock-->
                              <div id="pageBlock{$typeId}{$typeGuid}" class="panel-collapse collapse" typeId="{$typeId}" typeGuid="{$typeGuid}">
                                <!--pageData-->
                                <span id="pageData{$typeId}{$typeGuid}"/>
                              </div>

                            </div>

                          </xsl:for-each>
                        </xsl:for-each>
                      </div>
                    </div>
                  </div>
                  <!-- padding5ALL -->
                </div>
                <!-- BoxBorderSa -->
              </div><!-- col -->
            </div><!-- row -->
            
            <div class="clearfix"></div>
            <div class="margin-bottom-30"></div>
          </div>
          <!--/.container-->

          <!--//////Container Part end-->
          <!--**************************************************************************-->


          <!--**************************************************************************-->
          <!--//////頁腳start-->
          <xsl:call-template name ="footer"/>
          <!--//////頁腳end-->
          <!--**************************************************************************-->


        </div>
        <!--/.wrapper-->


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
        <!--看圖-->
        <!--<script type="text/javascript" src="../assets/plugins/fancybox/jquery.fancybox.pack.js"></script>-->
        <!-- JS Customization -->
        <script type="text/javascript" src="../assets/js/custom.js"></script>
        <!-- JS Page Level -->
        <script type="text/javascript" src="../assets/js/app.js"></script>
        <!--看圖-->
        <!--<script type="text/javascript" src="../assets/js/plugins/fancy-box.js"></script>-->
        <script type="text/javascript" src="../assets/js/jquery.scrollbar.min.js"></script>
        <script type="text/javascript" src="../assets/js/ochiJS.js"></script>

        <!--[if lt IE 9]>
        <script src="../assets/plugins/respond.js"></script>
        <script src="../assets/plugins/html5shiv.js"></script>
        <script src="../assets/js/plugins/placeholder-IE-fixes.js"></script>
        <![endif]-->


        <!--===jquery dp-->
        <!--<link rel="stylesheet" type="text/css" href="../js/j_dp/css/bootstrap-datepicker.standalone.min.css" />
        <script language="javascript" type="text/javascript" src="../js/j_dp/bootstrap-datepicker.min.js">;</script>
        <script language="javascript" type="text/javascript" src="../js/j_dp/bootstrap-datepicker.zh-TW.min.js">;</script>-->

        <!--===my d3-->
        <script src="../js/textCloud/d3.js"></script>
        <script src="../js/textCloud/d3.layout.cloud.js"></script>

        <!--===my js-->
        <script type="text/javascript" src="projectDetail_main.js"></script>
        <script type="text/javascript" src="projectDetail_clould.js"></script>
        
        <!-- Nick.JS -->
        <script src="../js/NickCommon.js"></script>


        <!--//////javascript end-->
        <!--**************************************************************************-->
      </body>
    </html>
  </xsl:template>


  <!--=======================
template：
========================-->
  <xsl:template name="MyTemplate_01">
    <!-- 遮罩：研究方向 -->
    <div class="modal fade" id="black_summary" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
            <h2 class="modal-title" id="myModalLabel">Summary</h2>
          </div>
          <div class="modal-body">

            <!--=======================-->
            <h3>Word cloud</h3>
            <div class="center-block">

              <!--
              <div class="sky-form pull-right">
                <a href="http://61.61.246.46:8100/entities/project/{$pjGuid}/page/1" target="_blank">
                  <button type="button" class="btn-u btn-u-sea">
                    維護專案詞庫
                  </button>
                </a>
              </div>
              -->


              <div class="clearfix"></div>
              <div id="blockMessage">&nbsp;</div>
              <div id="blockPopUp"/>
              <!--<div id="blockTag" style="border: 1px solid rgb(0, 0, 0); border-image: none; width: 500px; height: 300px;"/>-->

            </div>
            <br />

            <!--=======================-->
            <h3>research topics and related word</h3>
            <ol>
              <xsl:for-each select="$xmlDirectionList/*">
                <xsl:variable name="research_guid" select="@research_guid"/>

                <li>
                  <xsl:value-of select="@name"/>：
                  <blockquote>
                    <dl class="dl-horizontal">

                      <xsl:for-each select="$xmlAskComList/*[@research_guid=$research_guid]">
                        <dt>
                          <div>
                            <xsl:value-of select="@related_name"/>
                          </div>
                        </dt>
                        <dd>
                          <!--資訊-關鍵字來源-->
                          <xsl:choose>
                            <xsl:when test="@analyst_give='0'">
                              <span class="badge badge-dark rounded-2x">default</span>
                            </xsl:when>
                            <xsl:when test="@analyst_give='1'">
                              <span class="badge badge-blue rounded-2x">customized</span>
                            </xsl:when>
                            <xsl:when test="@analyst_give='2' or @analyst_give='3'">
                              <span class="badge badge-blue rounded-2x">learning</span>
                            </xsl:when>
                          </xsl:choose>

                          <!--資訊-是否排程-->
                          <xsl:choose>
                            <xsl:when test="@schedule = '0'">
                              <span class="badge badge-dark rounded-2x">not schedule</span>
                            </xsl:when>
                            <xsl:when test="@schedule = '1'">
                              <span class="badge badge-purple rounded-2x">schedule</span>
                            </xsl:when>
                          </xsl:choose>

                          <!--資訊-黑、白、中立名單-->
                          <xsl:choose>
                            <xsl:when test="@blacklist = '0'">
                              <span class="badge badge-dark rounded-2x">whitelist</span>
                            </xsl:when>
                            <xsl:when test="@blacklist = '1'">
                              <span class="badge badge-sea rounded-2x">blacklist</span>
                            </xsl:when>
                            <xsl:when test="@blacklist = '2'">
                              <span class="badge badge-dark rounded-2x">neutral</span>
                            </xsl:when>
                          </xsl:choose>

                        </dd>
                      </xsl:for-each>

                    </dl>
                  </blockquote>
                </li>

              </xsl:for-each>
            </ol>


          </div>
        </div>
      </div>
    </div>
  </xsl:template>


  <!-- 遮罩：tag挑選 -->
  <xsl:template name="myTemplate_tagSelect">
    <div class="modal fade" id="myTemplate_tagSelect" tabindex="-1" role="dialog" aria-labelledby="myModalLabel_02" aria-hidden="true">
      <div class="modal-dialog modal-sm">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="close">
              <span aria-hidden="true">&times;</span>
            </button>
            <h2 class="modal-title" id="myModalLabel_02">Tag Select</h2>
          </div>
          <div class="modal-body">

            <!--===msg-->
            <div id="tagSelect_message" style="color:red;"></div>
            <br/>

            <!--===content-->
            <div id="tagSelect_dataBlock"></div>

            <!--===btn-->
            <div class="col-sm-12 col-md-12 text-right">
              <input type="submit" value="Maintain Tag" id="tagSelect_maintain" class="btn-u btn-u-lg btn-u-sea" style="margin-right:10px;" onclick="doTagSelect_maintain();"/>
              <input type="submit" value="Cancel" id="tagSelect_cancel" class="btn-u btn-u-lg btn-u-sea" style="margin-right:10px;" onclick="doTagSelect_cancel();"/>
              <input type="submit" value="Save" id="tagSelect_save" class="btn-u btn-u-lg btn-u-sea" onclick="doTagSelect_save();"/>
            </div>


            <br/>
            <br/>

          </div>
        </div>
      </div>
    </div>
  </xsl:template>



  <!-- 遮罩：tag維護 -->
  <xsl:template name="myTemplate_tagMaintain">
    <div class="modal fade" id="myTemplate_tagMaintain" tabindex="-1" role="dialog" aria-labelledby="myModalLabel_03" aria-hidden="true">
      <div class="modal-dialog modal-sm">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="close">
              <span aria-hidden="true">&times;</span>
            </button>
            <h2 class="modal-title" id="myModalLabel_03">Tag Maintain</h2>
          </div>
          <div class="modal-body">

            <!--===msg-->
            <div id="tagMaintain_message" style="color:red;"></div>
            <br/>

            <!--===content-->
            <div id="tagMaintain_dataBlock"></div>

            <!--===btn-->
            <div class="col-sm-12 col-md-12 text-right">
              <input type="submit" value="Cancel" id="tagMaintain_cancel" class="btn-u btn-u-lg btn-u-sea" style="margin-right:10px;" onclick="doTagMaintain_cancel();"/>
            </div>


            <br/>
            <br/>

          </div>
        </div>
      </div>
    </div>
  </xsl:template>


  <xsl:template name="myTemplate_abs">
    <div class="modal fade" id="block_abs" tabindex="-1" role="dialog" aria-labelledby="myModalLabel_abs" aria-hidden="true">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="close">
              <span aria-hidden="true">&times;</span>
            </button>
            <h2 class="modal-title" id="myModalLabel_03">Abstract</h2>
          </div>
          <div class="modal-body">Under construction</div>
        </div>
      </div>
    </div>
  </xsl:template>
</xsl:stylesheet>
