<?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE stylesheet[
  <!ENTITY nbsp "&#160;">
  <!ENTITY copy "&#169;">
  <!ENTITY times "&#215;">
  <!ENTITY emsp "&#8195;">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../templates/pager.xslt"/>
  <xsl:output method="html" indent="yes" standalone="yes" omit-xml-declaration="yes" />


  <!--=======================
param：
========================-->
  <xsl:param name="AppRoot" />
  <xsl:param name="AppTitle" />

  <xsl:param name="xmlList" />
  <xsl:param name="pjid" />
  <xsl:param name="arcid" />



  <!--=======================
template：
========================-->
  <xsl:template match="/">
    <div>
      <xsl:choose>
        <xsl:when test="count($xmlList/*) > 0">
          <ol>
            <xsl:for-each select="$xmlList/*">
              <li>
                <div class="checkbox">
                  <label>
                    <input type="checkbox" name="tag_opt" value="{@tagtype_guid}">
                      <xsl:if test="@selected = '1'">
                        <xsl:attribute name="checked">checked</xsl:attribute>
                      </xsl:if>
                    </input>
                    <xsl:value-of select="@tagtype_name"/>
                  </label>
                </div>
              </li>
            </xsl:for-each>
          </ol>
          <br/>
          <input id="pjid" type="hidden" value="{$pjid}"/>
          <input id="arcid" type="hidden" value="{$arcid}"/>
        </xsl:when>

        <xsl:otherwise>
          Do not have tag.
          Please maintain tag first.
          <br/>
          <br/>
        </xsl:otherwise>
      </xsl:choose>


    </div>

  </xsl:template>
  
  
  
  
  
</xsl:stylesheet>
