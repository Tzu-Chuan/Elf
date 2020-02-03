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
  <xsl:param name="xmlDoc"></xsl:param>
  <xsl:param name="xmlOptSite"></xsl:param>



  <!--=======================
template：
========================-->
  <xsl:template match="/">

    <!--=======================-->
    <h3>Choose monitored websites：</h3>
    <ol>
      <xsl:for-each select="$xmlOptSite/*">
        <li>
          <div class="checkbox">
            <label>
              <input type="checkbox" name="optsite" value="{@optsite_name}"/>
              <a href="{@optsite_url}" target="_black">
                <xsl:value-of select="@optsite_name"/>
              </a>
            </label>
          </div>
        </li>
      </xsl:for-each>
    </ol>
    <br/>

    <!--=======================-->
    <h3>Project Information：</h3>
    <div style="margin-left:20px">
      <table class="table table-striped">
        <tbody>
          <tr>
            <th style="width:130px;">
              Project Name：
            </th>
            <td>
              <xsl:value-of select="$xmlDoc/*[@xx_item_explain='__專案名稱']/*[1]"/>
            </td>
          </tr>
          <tr>
            <th>
              Item：
            </th>
            <td>
              <xsl:value-of select="$xmlDoc/*[@xx_item_explain='__觀測項目名稱']/*[1]"/>
            </td>
          </tr>
          <tr>
            <th>
              Abbreviation：
            </th>
            <td>
              <xsl:for-each select="$xmlDoc/*[@xx_item_explain='__觀測項目簡寫']/*">
                <xsl:value-of select="./text()"/>、
              </xsl:for-each>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <br/>

    <!--=======================-->
    <h3>
      Project research topics and related word：
    </h3>
    <ol>
      <xsl:for-each select="$xmlDoc/*[@xx_item_explain='__研究方向']">

        <li>
          <div>
            <xsl:value-of select="@xx_item_name"/>：
          </div>

          <!--列表-->
          <div class="bg-color-grey2" style="padding:10px;">
            <div class="row">
              <xsl:for-each select="./*">
                <ul>
                  <li class="col-xs-6 col-md-3">
                    <span>
                      <xsl:value-of select="./text()"/>
                    </span>
                  </li>
                </ul>
              </xsl:for-each>
            </div>
          </div>

          <div class="clearfix"></div>
          <!--/結束浮動區塊-->
          <br/>
        </li>
      </xsl:for-each>
    </ol>
    <br/>  
  
  
  </xsl:template>




</xsl:stylesheet>