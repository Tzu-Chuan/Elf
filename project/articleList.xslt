<?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE stylesheet[
  <!ENTITY nbsp "&#160;">
  <!ENTITY copy "&#169;">
  <!ENTITY times "&#215;">
  <!ENTITY emsp "&#8195;">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../templates/pager_for_articleList.xslt"/>
  <xsl:output method="html" indent="yes" standalone="yes" omit-xml-declaration="yes" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />


  <!--=======================
param：
========================-->
  <xsl:param name="AppRoot" />
  <xsl:param name="AppTitle" />
  <xsl:param name="xmlResultList"></xsl:param>
  <xsl:param name="pjGuid"></xsl:param>

  <!--網站guid-->
  <xsl:param name="typeId"/>
  <xsl:param name="typeGuid"/>

  <!--=======================
template：
========================-->
  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="$typeId = '1' or $typeId = '2'">
        <xsl:call-template name="result_articles"/>
      </xsl:when>
      <xsl:when test="$typeId = '3' or $typeId = '4'">
        <xsl:call-template name="result_askCom"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>


  <!--=======================
template：固定網站來源
========================-->
  <xsl:template name="result_articles">
    <div class="panel-body">
      <form name="Form1" method="get">
        <input type="hidden" name="currentPageIndex" value="{$pagerCurrentPageIndex}"/>
      </form>

      <ol style="list-style-type:none;">
        <xsl:for-each select="$xmlResultList/*">
          <li>
            <!--=====編號-->
            <xsl:value-of select="@No"/>.&nbsp;
            <!--=====標題-->
            <a href="articleDetail.aspx?pjGuid={$pjGuid}&amp;atGuid={@article_guid}" target="_blank">
              <xsl:value-of select="@title"/>
            </a>
            &nbsp;<a href ="#" onclick="doTagSelect('{$pjGuid}', '{@article_guid}');">[tag]</a>

            <blockquote>
              <!--=====時間與分數-->
              <small>
                <!--<span class="color-grey">-->
                <em>
                  date:<xsl:value-of select="@get_time"/> | score:<font color="red">
                    <xsl:value-of select="@score"/>
                  </font>
                </em>
                <!--</span>-->
              </small>

              <!--=====摘要-->
              <p>
                <xsl:value-of select="@desc"/>
                
                
                <!--<xsl:choose>
                  <xsl:when test="string-length(@full_text)&gt;=400">
                    <xsl:value-of select="substring(@full_text,1,400)" disable-output-escaping="yes"/>...
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="@full_text" disable-output-escaping="yes"/>
                  </xsl:otherwise>
                </xsl:choose>-->
              </p>
            </blockquote>
          </li>
        </xsl:for-each>
      </ol>

      <!--=====換頁碼-->
      <xsl:call-template name="pager"/>

    </div>


    <!--=====遮罩：文章內容 -->
    <div id="articleContent" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
            <h2 class="modal-title" id="myModalLabel">文章內容</h2>
          </div>
          <div class="modal-body">

            <div class="modal-body edit-content"/>
            <iframe width="90%" height="90%" frameBorder="0"></iframe>

          </div>
        </div>
      </div>
    </div>


  </xsl:template>


  <!--=======================
template：askCom來源
========================-->
  <xsl:template name="result_askCom">
    <div class="panel-body">
      <form name="Form1" method="get">
        <input type="hidden" name="currentPageIndex" value="{$pagerCurrentPageIndex}"/>
      </form>

      <ol style="list-style-type:none;">
        <xsl:for-each select="$xmlResultList/*">
          <li>
            <!--=====編號-->
            <xsl:value-of select="@No"/>.&nbsp;
            <!--=====標題-->
            <a href="{@url}" target="_blank">
              <xsl:value-of select="@title"/>
            </a>
    
            <blockquote>
              <!--=====時間與分數-->
              <small>
                <!--<span class="color-grey">-->
                <em>
                  date:<xsl:value-of select="@get_time"/>
                </em>
                <!--</span>-->
              </small>

              <!--=====摘要-->
              <p>
                <xsl:value-of select="@describe_text"/>
              </p>
            </blockquote>
          </li>
        </xsl:for-each>
      </ol>

      <!--=====換頁碼-->
      <xsl:call-template name="pager"/>
    </div>

  </xsl:template>



</xsl:stylesheet>
