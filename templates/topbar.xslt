<?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE stylesheet[
  <!ENTITY nbsp "&#160;">
  <!ENTITY copy "&#169;">
  <!ENTITY times "&#215;">
  <!ENTITY emsp "&#8195;">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!--=======================
param：
========================-->
  <xsl:param name="AppRoot" />
  <xsl:param name="AppTitle" />
  <xsl:param name="empno" />
  <xsl:param name="isSysMgr" />
  <xsl:param name="isPjMgr" />

  <xsl:param name="tabName" />

  <!--=======================
template：
========================-->
  <xsl:template name="topbar">

    <div class="header header-sticky" >
      <!-- Topbar -->
      <div class="topbar">
        <div class="container">
          <!-- Topbar Navigation -->
          <ul class="loginbar pull-right">
            <li>
              <a href="https://itriweb.itri.org.tw/" target="_blank">itriweb</a>
            </li>
            <li class="topbar-devider"></li>
            <li>
              <a href="https://msx.itri.org.tw/owa/auth/logon.aspx" target="_blank">itrimail</a>
            </li>
            <li class="topbar-devider"></li>
            <li>
              <a href="https://empfinder.itri.org.tw/WebPage/ED_QueryIndex.aspx" target="_blank">Search itri employee</a>
            </li>
          </ul>
          <!-- End Topbar Navigation -->
        </div>
      </div>
      <!-- End Topbar -->
      <!-- Navbar -->
      <div class="navbar navbar-default mega-menu" role="navigation">
        <div class="container">
          <!-- Brand and toggle get grouped for better mobile display -->
          <div class="navbar-header">
            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-responsive-collapse">
              <span class="sr-only">Toggle navigation</span>
              <span class="fa fa-bars"></span>
            </button>
            <a class="navbar-brand" href="{$AppRoot}/project/default.aspx">
              <img id="logo-header" src="../images/logo.png" alt="Logo"/>
            </a>
          </div>

          <!-- Collect the nav links, forms, and other content for toggling -->
          <div class="collapse navbar-collapse navbar-responsive-collapse">
            <ul class="nav navbar-nav">
              <!-- Home -->
              <li>
                <xsl:choose>
                  <xsl:when test="$tabName='projectList'">
                    <xsl:attribute name="class">active</xsl:attribute>
                  </xsl:when>
                </xsl:choose>
                <div class="textcenter font-size6 deskonly ochimenuicon">
                  <a href="{$AppRoot}/project/default.aspx" style="text-decoration: none;">
                    <i class="iekeif-iek_list"><span class="path1"></span><span class="path2"></span><span class="path3"></span><span class="path4"></span><span class="path5"></span><span class="path6"></span></i>
                  </a>
                </div>
                <a href="{$AppRoot}/project/default.aspx">Project List</a>
              </li>

              <!--===當使用者為「系統管理人員、或專案管理人員」時顯示-->
              <!--<xsl:if test="$isSysMgr = 1 or $isPjMgr = 1">-->
                <li>
                  <xsl:choose>
                    <xsl:when test="$tabName='projectMgmt'">
                      <xsl:attribute name="class">active</xsl:attribute>
                    </xsl:when>
                  </xsl:choose>
                  <div class="textcenter font-size6 deskonly ochimenuicon">
                    <a href="{$AppRoot}/projectMgmt/default.aspx" style="text-decoration: none;">
                      <i class="iekeif-iek_tool"><span class="path1"></span><span class="path2"></span><span class="path3"></span><span class="path4"></span></i>
                    </a>
                  </div>
                  <a href="{$AppRoot}/projectMgmt/default.aspx">Project mgmt</a>
                </li>
              <!--</xsl:if>-->
                
              <!--===當使用者為「系統管理人員」時顯示-->
              <xsl:if test="$isSysMgr = 1">
                <li>
                  <xsl:choose>
                    <xsl:when test="$tabName='projectMaintain'">
                      <xsl:attribute name="class">active</xsl:attribute>
                    </xsl:when>
                  </xsl:choose>
                  <div class="textcenter font-size6 deskonly ochimenuicon">
                    <a href="{$AppRoot}/projectMaintain/default.aspx" style="text-decoration: none;">
                      <i class="iekeif-iek_role"><span class="path1"></span><span class="path2"></span><span class="path3"></span></i>
                    </a>
                  </div>
                  <a href="{$AppRoot}/projectMaintain/default.aspx">Project maintain</a>
                </li>
              </xsl:if>
		          <li>
                  <xsl:choose>
                    <xsl:when test="$tabName='DataExplore'">
                      <xsl:attribute name="class">active</xsl:attribute>
                    </xsl:when>
                  </xsl:choose>
                  <div class="textcenter font-size6 deskonly ochimenuicon">
                    <a href="https://exploreelftest.itri.org.tw" target="_blank" style="text-decoration: none;">
                      <i class="iekeif-pie-chart"><span class="path1"></span><span class="path2"></span><span class="path3"></span><span class="path4"></span></i>
                    </a>
                  </div>
                  <a href="https://exploreelftest.itri.org.tw" target="_blank" style="text-transform:none;">askElf</a>
                </li>
            </ul>
          </div>
          <!--/navbar-collapse-->
        </div>
      </div>
      <!-- End Navbar -->
    </div>
  </xsl:template>


</xsl:stylesheet>
