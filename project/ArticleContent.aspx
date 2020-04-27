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
    <title>IEKElf</title>
    <style>
        .navFixed {
            z-index: 10;
            position: fixed;
            top: 0;
            margin-top: 0;
            min-width: 1140px;
        }
    </style>
</head>
<body class="header-fixed boxed-layout" >
    <input type="hidden" id="tmpPjGuid" value="<%= PjGuid %>" />
    <div class="wrapper">
         <!--#include file="../templates/Header.html"-->
        <div class="container">
            <!--文字雲-->
            <div class="twocol margin10T"><div class="left"><h1>See First : Word Cloud</h1></div></div>
            <div class="maxheightB BoxBorderSa BoxBgWa padding5ALL"><div id="blockTag" class="width100"></div></div>
            <div id="blockMessage"></div>
            <input type="hidden" id="tmpCloud" />

            <div class="BoxBgWa margin-bottom-20 margin10T">
                <div style="color: #2196F3; font-size: 2.92rem; font-family: Segoe UI;">Summary</div>
                <div style="font-size: 2.28rem; margin-bottom: 10px; font-family: Segoe UI;">Auto Summary</div>
                <div id="Summary" style="font-size: 18px; font-family: Segoe UI;"></div>
            </div>
            <div id="ArticleTitle" style="font-size: 2.92rem; margin-bottom: 10px; font-family: Segoe UI;"></div>
            <div id="WebSite" style="font-size: 1rem; margin-bottom: 10px; font-family: Segoe UI;"></div>

            <div class="margin5TB dropdowns" style="background-color: #b0bec5; padding: 5px 0px;">
                <ul id="topicTag" class="ks-cboxtags">
                    <li><input type="checkbox" id="checkboxOne" value="0" name="cbDate" checked="checked" /><label for="checkboxOne" style="margin-right: 5px; font-weight: bold; font-size: 18px; font-family: Segoe UI;">All</label></li>
                    <li><input type="checkbox" id="checkboxTwo" value="1" name="cbDate" /><label for="checkboxTwo" style="margin-right: 5px; font-weight: bold; font-size: 18px; font-family: Segoe UI;">AAA</label></li>
                    <li><input type="checkbox" id="checkboxThree" value="7" name="cbDate" /><label for="checkboxThree" style="margin-right: 5px; font-weight: bold; font-size: 18px; font-family: Segoe UI;">BBB</label></li>
                    <li><input type="checkbox" id="checkboxFour" value="30" name="cbDate" /><label for="checkboxFour" style="margin-right: 5px; font-weight: bold; font-size: 18px; font-family: Segoe UI;">CCC</label></li>
                    <li><input type="checkbox" id="checkboxFive" value="180" name="cbDate" /><label for="checkboxFive" style="margin-right: 5px; font-weight: bold; font-size: 18px; font-family: Segoe UI;">DDD</label></li>
                    <li><input type="checkbox" id="checkboxsix" value="365" name="cbDate" /><label for="checkboxsix" style="margin-right: 5px; font-weight: bold; font-size: 18px; font-family: Segoe UI;">EEE</label></li>
                </ul>
            </div>
            <div id="ArticleContent" class="BoxBgWa margin-bottom-20" style="font-size: 18px; font-family: Segoe UI;"></div>
        </div>
         <!--#include file="../templates/Footer.html"-->
    </div>
</body>
</html>
