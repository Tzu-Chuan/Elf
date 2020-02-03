<?xml version="1.0" encoding="UTF-8" ?>
<!DOCTYPE stylesheet[
  <!ENTITY nbsp "&#160;">
  <!ENTITY copy "&#169;">
  <!ENTITY times "&#215;">
  <!ENTITY emsp "&#8195;">
]>
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
    <nav class="text-center">
      <ul class="pagination">
        <li class="disabled">
          <span aria-hidden="true">
            <xsl:value-of select="$pagerRecordCount"/> results / <xsl:value-of select="$pagerPageCount"/> pages
          </span>
        </li>
      </ul>
      <ul class="pagination pagination-lg">
        <!--前一頁-->
        <xsl:if test="$pagerCurrentPageIndex+(-1) &gt;= 1">
          <li>
            <a href="javascript:on_PageChanged({$pagerCurrentPageIndex+(-1)})" aria-label="Next">
              <i class="fa fa-caret-left"></i>&nbsp; Previous
            </a>
          </li>
        </xsl:if>

        <!--列印頁碼，呼叫本身的template pagernum-->
        <xsl:if test="$pagerPageCount&gt;0">
          <xsl:call-template name="pagernum">
            <xsl:with-param name="pageIndex" select="$pagerStartPage"/>
          </xsl:call-template>
        </xsl:if>

        <!--下一頁-->
        <xsl:if test="$pagerCurrentPageIndex+1 &lt;= $pagerPageCount">
          <li>
            <a href="javascript:on_PageChanged({$pagerCurrentPageIndex+1})" aria-label="Next">
              Next &nbsp;<i class="fa fa-caret-right"></i>
            </a>
          </li>
        </xsl:if>

      </ul>
    </nav>
  </xsl:template>


  <xsl:template name="pagernum">
    <xsl:param name="pageIndex"/>
    <xsl:if test="$pageIndex&lt;=$pagerEndPage">
      <xsl:choose>
        <xsl:when test="$pageIndex=$pagerCurrentPageIndex">
          <li class="active">
            <a href="javascript:return false;">
              <xsl:value-of select="$pageIndex"/>
            </a>
          </li>
        </xsl:when>
        <xsl:otherwise>
          <li>
            <a href="javascript:on_PageChanged({$pageIndex})">
              <xsl:value-of select="$pageIndex"/>
            </a>
          </li>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:call-template name="pagernum">
        <xsl:with-param name="pageIndex" select="$pageIndex+1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>






