<?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE stylesheet[
  <!ENTITY nbsp "&#160;">
  <!ENTITY copy "&#169;">
  <!ENTITY times "&#215;">
  <!ENTITY emsp "&#8195;">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" indent="yes" standalone="yes" omit-xml-declaration="yes" />


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
    <div>

      <input type="text" id="newTagName" value="" size="30" maxlength="15"></input>
      <input type="submit" value="Add Tag" id="tagMaintain_add" class="btn-u btn-u-lg btn-u-sea" style="margin-left:10px;" onclick="doTagMaintain_add();"/>

      <br/>
      <br/>
      <ol>
        <xsl:for-each select="$xmlList/*">
          <li>
            <div class="checkbox">
              <input type="submit" value="Delete" id="tagMaintain_delete" class="btn-u btn-u-lg btn-u-sea" style="margin-right:10px;" onclick="doTagMaintain_delete('{@tagtype_guid}');"/>

              <label>
                <xsl:value-of select="@tagtype_name"/>
              </label>

            </div>
          </li>
        </xsl:for-each>
      </ol>
      <br/>
  
    </div>

  </xsl:template>
</xsl:stylesheet>
