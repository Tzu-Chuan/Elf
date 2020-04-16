<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArticleContent.aspx.cs" Inherits="project_ArticleContent" %>

<!DOCTYPE html>

<html>

<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta name="description" content="請填寫"/>
    <meta name="keywords" content="請填寫"/>
    <meta name="author" content="請填寫"/>

    <!--=====Favicon-->
    <link rel="shortcut icon" href="favicon.ico" />
    <!--=====CSS Global Compulsory -->
    <link rel="stylesheet" href="../assets/plugins/bootstrap/css/bootstrap.css" />
    <link rel="stylesheet" href="../assets/css/style.css" />
    <!--======CSS Implementing Plugins -->
    <link rel="stylesheet" href="../assets/plugins/font-awesome/css/font-awesome.min.css" />
    <!--icon的元件-->
    <link rel="stylesheet" href="../assets/plugins/sky-forms/css/custom-sky-forms.css" />
    <!--=====CSS Customization -->
    <link rel="stylesheet" href="../assets/css/custom.css" />
    <!--=====CSS Customization by ochison  -->
    <link rel="stylesheet" href="../assets/css/iekicon.css" />
    <link rel="stylesheet" href="../assets/css/scrollbar.css" />
    <link rel="stylesheet" href="../assets/css/ochi.css" />

    <!-- JS Global Compulsory -->
    <script type="text/javascript" src="../assets/plugins/jquery/jquery.js"></script>
    <script type="text/javascript" src="../assets/plugins/jquery/jquery-migrate.min.js"></script>
    <!-- JS Implementing Plugins -->
    <script type="text/javascript" src="../assets/plugins/back-to-top.js"></script>
    <!-- JS Customization -->
    <script type="text/javascript" src="../assets/js/custom.js"></script>
    <!-- JS Page Level -->
    <script type="text/javascript" src="../assets/js/app.js"></script>
    <!--看圖-->
    <script type="text/javascript" src="../assets/js/jquery.scrollbar.min.js"></script>
    <script type="text/javascript" src="../assets/js/ochiJS.js"></script>

    <!--[if lt IE 9]>
        <script src="../assets/plugins/respond.js"></script>
        <script src="../assets/plugins/html5shiv.js"></script>
        <script src="../assets/js/plugins/placeholder-IE-fixes.js"></script>
        <![endif]-->

    <!--===my d3-->
    <script src="../js/textCloud/d3.js"></script>
    <script src="../js/textCloud/d3.layout.cloud.js"></script>

    <!-- Nick.JS -->
    <script type="text/javascript" src="../js/NickCommon.js"></script>
    <script type="text/javascript" src="../js/PageList.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.1.12.1.js"></script>
    <script>$.widget.bridge('uitooltip', $.ui.tooltip);</script> <!--JQueryUI & bootstrap tooltip 會打架 重新設定 JQueryUI tooltip function name-->
    <!-- bootstrap -->
    <script type="text/javascript" src="../assets/plugins/bootstrap/js/bootstrap.min.js"></script>

    <!--===my js-->
    <script type="text/javascript" src="articleContent.js"></script>
    <script type="text/javascript" src="wordcloud.js"></script>
    <title>IEKElf</title>
    <style>

</style>
</head>
<body class="header-fixed boxed-layout">
    <div class="wrapper">
         <!--#include file="../templates/Header.html"-->
          <div class="container">
              <div id="top-bar">test</div>
              <!--文字雲-->
              <div class="twocol margin10T"><div class="left"><H3>See First : Word Cloud</H3></div></div>
              <div class="maxheightB BoxBorderSa BoxBgWa padding5ALL dropdowns"><div id="blockTag" class="width100"></div></div>
                 
                <div class="twocol">
                    <div class="left"><h3>Summary</h3></div>
                </div>

                <div class="BoxBgWa margin-bottom-20">
                    <div style="font-size:14pt; margin-bottom:10px;"><b>Auto Summary</b></div>
                    <div id="Summary"></div>
                </div>

               <div id="ArticleTitle"></div>
               <div id="WebSite"></div>

              <div id="ArticleContent" class="BoxBgWa margin-bottom-20"></div>
          </div>
         <!--#include file="../templates/Footer.html"-->
    </div>
</body>
</html>
