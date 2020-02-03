<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="pagerRecordCount"/>
  <xsl:param name="pagerPageCount"/>
  <xsl:param name="pagerCurrentPageIndex"/>
  <xsl:param name="pagerStartPos"/>
  <xsl:param name="pagerEndPos"/>
  <xsl:param name="pagerStartPage"/>
  <xsl:param name="pagerEndPage"/>
  <xsl:param name="pagerCount"/>

  <xsl:template name="pager">
    <div class="num">  
      <!--第一頁-->
      <xsl:if test="$pagerPageCount&gt;0">
        <xsl:if test="$pagerCurrentPageIndex+(-1) &gt;= 1">
          <a href="javascript:on_PageChanged(1)" class="btn-first">
            fir
          </a>
        </xsl:if>
      </xsl:if>
      <!--前一頁-->
      <xsl:if test="$pagerCurrentPageIndex+(-1) &gt;= 1">
        <a href="javascript:on_PageChanged({$pagerCurrentPageIndex+(-1)})" class="btn-prev">
          prev
        </a>
      </xsl:if>
      <!--列印頁碼，呼叫本身的template pagernum-->
      <xsl:if test="$pagerPageCount&gt;0">
        <xsl:call-template name="pagernum">
          <xsl:with-param name="pageIndex" select="$pagerStartPage"/>
        </xsl:call-template>
      </xsl:if>      
      <!--下一頁-->
      <xsl:if test="$pagerCurrentPageIndex+1 &lt;= $pagerPageCount">
        <a href="javascript:on_PageChanged({$pagerCurrentPageIndex+1})" class="btn-next">
          next
        </a>
      </xsl:if>
      <!--最後一頁-->
      <xsl:if test="$pagerPageCount&gt;0">
        <xsl:if test="$pagerCurrentPageIndex+1 &lt;= $pagerPageCount">
          <a href="javascript:on_PageChanged({$pagerPageCount})" class="btn-last">
            last
          </a>
        </xsl:if>
      </xsl:if>
    </div>
    <!--頁數資訊-->
    <div class="total">共 <xsl:value-of select="$pagerRecordCount"/> 筆，<xsl:value-of select="$pagerPageCount"/> 頁</div>
    
  </xsl:template>

  <xsl:template name="pagernum">
    <xsl:param name="pageIndex"/>
    <xsl:if test="$pageIndex&lt;=$pagerEndPage">
      <xsl:choose>
        <xsl:when test="$pageIndex=$pagerCurrentPageIndex">
          <span class="cur">
            <a href="javascript:return false;">
              <xsl:value-of select="$pageIndex"/>
            </a>
          </span>
        </xsl:when>
        <xsl:otherwise>
          <span class="cur2">
            <a href="javascript:on_PageChanged({$pageIndex})">
              <xsl:value-of select="$pageIndex"/>
            </a>
          </span>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:call-template name="pagernum">
        <xsl:with-param name="pageIndex" select="$pageIndex+1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>






