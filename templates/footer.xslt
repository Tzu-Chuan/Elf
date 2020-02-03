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

  <xsl:param name="tabName" />

  <!--=======================
template：
========================-->
  <xsl:template name="footer">

    <div class="footer-default">
      <div class="copyright">
        <div class="container">
          <div class="row">
            <div class="col-md-12">
              <!--
              <p>
                2017 &copy; 工業技術研究院  ｜連絡窗口：｜
                最佳瀏覽解析度：1280X1024
              </p>
              -->
              <p>
                2017 &copy; ITRI IEK  ｜Contact us：03-5912293
              </p>

            </div>
          </div>
        </div>
      </div>
    </div>


    <!-- Piwik -->
    <script type="text/javascript">
      <![CDATA[
      var _paq = _paq || [];
      /* tracker methods like "setCustomDimension" should be called before "trackPageView" */
      _paq.push(['trackPageView']);
      _paq.push(['enableLinkTracking']);
      (function() {
      var u="//piwik.itri.org.tw/piwik/";
      _paq.push(['setTrackerUrl', u+'piwik.php']);
      _paq.push(['setSiteId', '40']);
      var d=document, g=d.createElement('script'), s=d.getElementsByTagName('script')[0];
      g.type='text/javascript'; g.async=true; g.defer=true; g.src=u+'piwik.js'; s.parentNode.insertBefore(g,s);
      })();
      ]]>
    </script>
    <!-- End Piwik Code -->


  </xsl:template>


</xsl:stylesheet>
