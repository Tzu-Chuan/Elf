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
    <!-- Star Ranking CSS-->
    <link rel="stylesheet" href="../assets/css/font-awesome.min.css" />

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
    <script type="text/javascript" src="../js/snowball.babel.js"></script>

    <link rel="stylesheet" href="ArticleContent.css" />

    <title>IEKElf</title>
    <style>
        .navFixed {
            z-index: 10;
            position: fixed;
            top: 0;
            margin-top: 0;
            /*min-width: 1140px;*/
        }

        .aStar:hover {
            color: #FFD700 !important;
        }

        .StarFull {
            color: #FFD700 !important;
        }

        .StarChecked {
            color: #FFD700 !important;
        }
    </style>
</head>
<body class="header-fixed boxed-layout" >
    <input type="hidden" id="TopDistance" />
    <input type="hidden" id="TypeAry" />
    <input type="hidden" id="tmpPjGuid" value="<%= PjGuid %>" />
    <input type="hidden" id="tmpComp" value="<%= Competence %>" />
    <input type="hidden" id="tmpBrowser" value="<%= BrowserName %>" />
    <div class="wrapper">
         <!--#include file="../templates/Header.html"-->
        <div id="printarea" class="container" style="font-family: Segoe UI;">
            <!--文字雲-->
            <div class="twocol margin10T">
                <div class="left"><h1>See First : Word Cloud</h1></div>
                <div class="right"><input type="button" id="PrintBtn" class="btn btn-info" value="Print" style="color:black; font-size:16px;" /></div>
            </div>
            <div class="maxheightB BoxBorderSa BoxBgWa padding5ALL"><div id="blockTag" class="width100"></div></div>
            <div id="blockMessage"></div>
            <input type="hidden" id="tmpCloud" />

            <div class="BoxBgWa margin-bottom-20 margin5T" style="padding:5px 10px;">
                <div style="color: #2196F3 !important; font-size: 2.42rem; margin-bottom:5px;">Auto Summary</div>
                <div id="Summary" style="font-size: 18px; "></div>
            </div>
            <div id="ArticleTitle" style="font-size: 2.42rem; margin-bottom: 10px; "></div>
            <div id="WebSite" style="font-size: 1rem; margin-bottom: 10px; "></div>

            <div class="margin5TB dropdowns" style="background-color: #b0bec5; padding: 5px 0px;">
                <ul id="topicTag" class="ks-cboxtags"></ul>
            </div>

            <div id="ArticleContent" class="BoxBgWa margin-bottom-20" style="font-size: 18px; padding:10px 10px;"></div>
        </div>

        <!-- Ranking Feedback -->
        <div class="container" style="font-family: Segoe UI;">
            <div id="FeedBack" class="BoxBgWa margin-bottom-20" style="font-size: 18px; padding:10px 10px;">
                <input id="ranked" type="hidden" value="N" />
                <div style="color: #2196F3 !important; font-size: 2.92rem; ">Ranking Feedback</div>
                <%--<div style="font-size: 2.28rem;">Rating</div>--%>
                <div id="ScoreBlock" style="font-size:1.64rem; color:#4CAF50 !important; display:none;">score: <span id="RankScore" style="color:#4CAF50 !important;"></span></div>
                <div style="font-size:30px; margin-bottom:10px;">
                    <a href="javascript:void(0);" class="aStar" name="star" rank="1"><span class="fa fa-star"></span></a>
                    <a href="javascript:void(0);" class="aStar" name="star" rank="2"><span class="fa fa-star"></span></a>
                    <a href="javascript:void(0);" class="aStar" name="star" rank="3"><span class="fa fa-star"></span></a>
                    <a href="javascript:void(0);" class="aStar" name="star" rank="4"><span class="fa fa-star"></span></a>
                    <a href="javascript:void(0);" class="aStar" name="star" rank="5"><span class="fa fa-star"></span></a>
                </div>
                <div style="font-size: 2.28rem;">Comments</div>
                <div style="margin-bottom:20px;"><input type="text" id="feedbackStr" name="keyword" value="" onkeypress="" class="form-control" placeholder="Write your feedback down" /></div>
                <div><input type="button" id="SubBtn" class="btn btn-info" value="SUBMIT" style="color:black; font-size:16px;" /></div>
            </div>
        </div>
		<div class="container col-12" style="margin-bottom:10px;">
			<div style="font-size:16pt;">To maintain key words：<a href="../projectMgmt/WordList.aspx?pjGuid=<%= PjGuid %>" target="_blank">Click Here</a></div>
			<%--<a href="../projectMgmt/WordList.aspx?pjGuid=<%= PjGuid %>" target="_blank" class="btn btn-info btn-lg btn-block" style="color:black;">To maintain key words</a>--%>
		</div>
        <!--#include file="../templates/Footer.html"-->
    </div>
</body>
</html>
