﻿$(document).ready(function () {
    // 文字雲
    WordCloud();
    // RWD 依視窗大小重繪文字雲
    d3.select(window).on('resize', WordCloud);

    getResources();
    
    getData();
    GetFeedBack();
    
    $(window).scroll(function () {
        if ($(this).scrollTop() >= $("#TopDistance").val()) {          /* 要滑動到選單的距離 */
            $('.dropdowns').addClass('navFixed');   /* 幫選單加上固定效果 */
        } else {
            $('.dropdowns').removeClass('navFixed'); /* 移除選單固定效果 */
        }
    });

    $(window).resize(function () {
        $('.dropdowns').css("width", $('#ArticleContent').width() + 20 + "px");
    });

    //checkbox check all
    $(document).on("click", "#Topic_all", function () {
        if ($("#Topic_all").prop("checked")) {
            // IE
            if ($("#tmpBrowser").val() == "internetexplorer") {
                // Topic
                $("input[name='cbTopic']").each(function () {
                    var color = $(this).closest("li").find("label").attr("colorstr");
                    $(this).closest("li").find("label").css("background-color", color);
                    $(this).prop("checked", true);
                });

                // 標記文字
                $(".tagword").each(function () {
                    $(this).css("background-color", $(this).attr("colorstr"));
                });
            }
            // Chrome & FireFox
            else {
                // Topic
                $("input[name='cbTopic']").each(function () {
                    var color = $(this).closest("li").find("label").attr("colorstr");
                    $(this).closest("li").find("label").removeClass("white_print");
                    $(this).closest("li").find("label").addClass(color);
                    $(this).prop("checked", true);
                });

                // 標記文字
                $(".tagword").each(function () {
                    $(this).addClass($(this).attr("colorstr"));
                });
            }
        }
        else {
            $("input[name='cbTopic']").prop("checked", false);
            if ($("#tmpBrowser").val() == "internetexplorer") {
                // Topic
                $("input[name='cbTopic']").closest("li").find("label").css("background-color", "");
                // 標記文字
                $(".tagword").css("background-color", "");
            }
            else {
                // Topic
                $("input[name='cbTopic']").closest("li").find("label").removeClass();
                $("input[name='cbTopic']").closest("li").find("label").addClass("white_print");
                // 標記文字
                $(".tagword").each(function () {
                    $(this).removeClass($(this).attr("colorstr"));
                });
            }
        }
    });

    $(document).on("click", "input[name='cbTopic']", function () {
        var color = $(this).closest("li").find("label").attr("colorstr");
        if ($(this).is(":checked")) {
            if ($("#tmpBrowser").val() == "internetexplorer")
                $(this).closest("li").find("label").css("background-color", color);
            else {
                $(this).closest("li").find("label").removeClass("white_print");
                $(this).closest("li").find("label").addClass(color);
            }

            // 標記文字
            $("span[name='" + this.value + "']").each(function () {
                if ($("#tmpBrowser").val() == "internetexplorer")
                    $(this).css("background-color", color);
                else {
                    $(this).removeClass("white_print");
                    $(this).addClass(color);
                }
            });
        }
        else {
            if ($("#tmpBrowser").val() == "internetexplorer") {
                // Topic
                $(this).closest("li").find("label").css("background-color", "");
                // 標記文字
                $("span[name='" + this.value + "']").css("background-color", "");
            }
            else {
                // Topic
                $(this).closest("li").find("label").removeClass(color);
                $(this).closest("li").find("label").addClass("white_print");
                // 標記文字
                $("span[name='" + this.value + "']").removeClass(color);
                //$("span[name='" + this.value + "']").addClass("white_print");
            }
        }
    });

    // 列印
    $(document).on("click", "#PrintBtn", function () {
        printHtml();
    });

    // 評分 mouseover
    $(document).on("mouseover", "a[name='star']", function () {
        var rank = $(this).attr("rank");
        $("a[name='star']").each(function (i) {
            if ((i + 1) <= rank)
                $(this).find("span").addClass("StarFull");
            else
                $(this).find("span").removeClass("StarFull");
        });
    });

    // 評分 mouseout
    $(document).on("mouseout", "a[name='star']", function () {
        $("a[name='star'] span").removeClass("StarFull");
    });

    // Star Ranking
    $(document).on("click", "a[name='star']", function () {
        var score = $(this).attr("rank");
        $("a[name='star']").each(function (i) {
            if ((i + 1) <= score)
                $(this).find("span").addClass("StarChecked");
            else
                $(this).find("span").removeClass("StarChecked");
        });
        $("#ScoreBlock").show();
        $("#RankScore").html(score);
    });

    // feedback submit
    $(document).on("click", "#SubBtn", function () {
        if ($("#RankScore").html() == "") {
            alert("送出前，請先對文章評分\nThank you!");
            return false;
        }

        if ($("#ranked").val() == "Y") {
            if (!confirm("Sure about changing your feedback?")) {
                return false;
            }
        }

        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
            url: "projectHandler/RankingFeedback.aspx",
            data: {
                atGuid: $.getQueryString("atGuid"),
                score: $("#RankScore").html(),
                feedback: $("#feedbackStr").val()
            },
            error: function (xhr) {
                alert(xhr.responseText);
            },
            success: function (data) {
                if ($(data).find("Error").length > 0) {
                    alert($(data).find("Error").attr("Message"));
                }
                else {
                    alert($("Response", data).text());
                }
            }
        });
    });

});// end js

function getData() {
    $.ajax({
        type: "POST",
        async: true, //在沒有返回值之前,不會執行下一步動作
        url: "projectHandler/GetArticleDetail.aspx",
        data: {
            atGuid: $.getQueryString("atGuid")
        },
        error: function (xhr) {
            $("#ArticleContent").html('<span style="color:red;">Error Message：<br>' + xhr.status + '</span>');
            console.log(xhr.responseText);
        },
        beforeSend: function () {
            $.blockUI({
                message: $('#msgblock'),
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#ffffff',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .6,
                    color: '#000000'
                }
            });
        },
        complete: function () {
            $("#TopDistance").val($(".dropdowns").offset().top);
            $('.dropdowns').css("width", $('#ArticleContent').width() + 20 + "px");
            $.unblockUI();
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                $("#ArticleContent").html('<span style="color:red;">Error Message：<br>' + $(data).find("Error").attr("Message") + '</span>');
            }
            else {
                var NewContent = '';
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        $("#Summary").html($(this).children("abstract_iekelf").text().trim());
                        $("#ArticleTitle").html($(this).children("title").text().trim());
                        $("#WebSite").html('Source: ' + $(this).children("website_name").text().trim() + '.  click <a target="_blank" href="' + $(this).children("url").text().trim() + '">here</a>' + ' to view the original article.');
                        NewContent = $(this).children("full_text").text().trim().replace(/\n/g, " ");
                    });
                }

                //  **************** 文章段落處理 Start *******************
                var point_index = NewContent.indexOf(".");
                var h_index = 0;
                var e_index = 0;
                var tmpPointStr = "";
                var twoWord = ["Dr", "Mr", "Ms"];
                var threeWord = ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];
                if (point_index > -1) {
                    while (point_index >= 0) {
                        tmpPointStr += NewContent.substring(h_index, point_index) + ".";
                        if ($.inArray(NewContent.substr(point_index - 2, 2).trim(), twoWord) == -1
                            && $.inArray(NewContent.substr(point_index - 3, 3).toUpperCase().trim(), threeWord) == -1) {
                            tmpPointStr += "<br><br>";
                        }
                        e_index = NewContent.indexOf(" ", point_index + 1);
                        h_index = e_index;
                        point_index = NewContent.indexOf(".", point_index + 1);
                    }
                }

                NewContent = tmpPointStr;
                // **************** 文章段落處理 End *******************

                var ResearchAry=$("#TypeAry").val().split(",");
                if ($(data).find("word_item").length > 0) {
                    // 單字原型判斷套件設定語言
                    language = snowballFactory.newStemmer("english");
                    $(data).find("word_item").each(function (i) {
                        var color = $('input[name="cbTopic"][value="' + $(this).children("research_guid").text() + '"]').closest("li").find("label").attr("colorstr");

                        // 全文文字取代(比對用)
                        //var word = new RegExp($(this).children("name").text(), 'g');
                        //NewContent = NewContent.replace(word, '<span class="tagword" name="' + $(this).children("research_guid").text() + '" colorstr="' + color + '" style="background-color:' + color + ';">' + $(this).children("name").text() + '</span>');

                        // debug 用
                        var stop = '';
                        if ($(this).children("name").text() == "coronavirus")
                            stop = "OK";

                        // **************** 文章處理 Start *******************
                        var index = 0;
                        var headIndex = 0;
                        var name = $(this).children("name").text();  //字詞
                        var name_stem = $(this).children("name_stem").text(); //字詞原型
                        var tmpContent = '';
                        var mergeLength = 0;
                        // 判斷是單字還是句子
                        var spaceCont = 0;
                        if (name.indexOf(" ") > -1)
                            spaceCont = name.split(" ").length;

                        // 第一個字 index
                        var tmpIndex = NewContent.indexOf(name, index);

                        // 迴圈開始
                        while (tmpIndex >= 0) {
                            if (mergeWord == true)
                                index = index + mergeLength;

                            tmpIndex = NewContent.indexOf(name, index);

                            // 重覆字詞狀態
                            var repeatWord = false;
                            // 判斷文字是否為複合字
                            var mergeWord = false;
                            if (NewContent.substr((tmpIndex - 1), 1).trim() != "" &&
                                NewContent.substr((tmpIndex - 1), 1).trim() != "?" &&
                                NewContent.substr((tmpIndex - 1), 1).trim() != "!" &&
                                NewContent.substr((tmpIndex - 1), 1).trim() != ":" &&
                                NewContent.substr((tmpIndex - 1), 1).trim() != "," &&
                                NewContent.substr((tmpIndex - 1), 1).trim() != "." &&
                                NewContent.substr((tmpIndex - 1), 1).trim() != ">" && // 文字重複
                                tmpIndex > 0) {
                                mergeWord = true;
                                do {
                                    tmpIndex = tmpIndex - 1;
                                } while (NewContent.substr(tmpIndex, 1).trim() != "" && NewContent.substr(tmpIndex, 1) != ".")
                                // +1 去掉前面的空白
                                tmpIndex += 1;
                            }
                            else {
                                // 字詞重覆出現時
                                if (NewContent.substr((tmpIndex - 1), 1).trim() == ">")
                                    repeatWord = true;
                            }

                            index = tmpIndex + 1;

                            // 單字結尾位置
                            var endIndex='';
                            if (repeatWord)
                                endIndex = NewContent.indexOf("<", index);
                            else
                                endIndex = NewContent.indexOf(" ", index);

                            // 句子處理
                            if (spaceCont > 0) {
                                for (var i = 1; i < spaceCont; i++) {
                                    index = endIndex + 1;
                                    endIndex = NewContent.indexOf(" ", index);
                                }
                            }

                            // 取單字,去標點符號
                            var tmpstr = NewContent.substring(tmpIndex, endIndex);
                            if (tmpstr.indexOf(",") > -1)
                                endIndex = NewContent.indexOf(",", index);
                            if (tmpstr.indexOf(".") > -1)
                                endIndex = NewContent.indexOf(".", index);
                            if (tmpstr.indexOf("!") > -1)
                                endIndex = NewContent.indexOf("!", index);
                            if (tmpstr.indexOf("?") > -1)
                                endIndex = NewContent.indexOf("?", index);
                            if (tmpstr.indexOf(":") > -1)
                                endIndex = NewContent.indexOf(":", index);

                            tmpstr = NewContent.substring(tmpIndex, endIndex);

                            // 複合字長度(字數)
                            if (mergeWord == true)
                                mergeLength = tmpstr.length;

                            // 判斷單字原型
                            var stem_str = language.stem(tmpstr);
                            var boolWord = false;
                            if (tmpstr == $(this).children("name").text())
                                boolWord = true;
                            else {
                                if (stem_str.toLowerCase() == name_stem.toLowerCase())
                                    boolWord = true;
                            }

                            // 加入標籤
                            // 文章有對應文字
                            if (tmpIndex >= 0) {
                                if (boolWord) {
                                    if ($("#tmpBrowser").val() == "internetexplorer") // IE
                                        tmpContent += NewContent.substring(headIndex, tmpIndex) + '<span class="tagword" name="' + $(this).children("research_guid").text() + '" colorstr="' + color + '" style="background-color:' + color + '">' + tmpstr + '</span>';
                                    else 
                                        tmpContent += NewContent.substring(headIndex, tmpIndex) + '<span class="tagword ' + color + '" name="' + $(this).children("research_guid").text() + '" colorstr="' + color + '">' + tmpstr + '</span>';
                                    
                                    if ($.inArray($(this).children("research_guid").text(), ResearchAry) >= 0)
                                        ResearchAry.splice($.inArray($(this).children("research_guid").text(), ResearchAry), 1);
                                }
                                else
                                    tmpContent += NewContent.substring(headIndex, endIndex);

                            }
                            // 文章無對應文字
                            else
                                tmpContent += NewContent.substring(headIndex);

                            headIndex = endIndex;
                        }

                        if (tmpContent != "")
                            NewContent = tmpContent;
                        // **************** 文章處理 End *******************
                    });

                   
                    //NewContent = NewContent.replace(/\./g, ".<br><br>");//.replace(/\?/g, "?<br><br>");
                    
                    $("#ArticleContent").html(NewContent);

                    RemoveResources(ResearchAry);
                }
            }
        }
    });
}


// 文字雲
function WordCloud() {
    if ($("#tmpCloud").val() == "") {
        $.ajax({
            type: "POST",
            async: true, //在沒有返回值之前,不會執行下一步動作
            url: "projectHandler/GetArticleWordCloud.aspx",
            data: {
                atGuid: $.getQueryString("atGuid")
            },
            success: function (data) {
                setDataResult($("textrank_keyword", data).text());
                $("#tmpCloud").val(JSON.stringify($("textrank_keyword", data).text()));
            },
            error: function (jqXHR, textStatus, exception) {
                $("#blockMessage").html("<font color='red'>Word Cloud message：" + exception + "</font>");
            }
        });
    }
    else
        setDataResult($.parseJSON($("#tmpCloud").val()));
}

// 繪出文字雲
function setDataResult(jsonData) {
    $("#blockTag").html("");
    // RWD 抓DIV當下width
    var BlockWidth = parseInt($('#blockTag').width());

    var __newData = new Array();
    var __obj = null;
    jsonData = jsonData.replace(/\'/g,"\"")
    jsonData = $.parseJSON(jsonData);
    $.each(jsonData, function (i,value) {
        __obj = { 'text': value.text, 'size': value.weight }
        __newData.push(__obj);
    });

    /*===check array length*/
    if (__newData.length == 0) {
        $("#blockMessage").html("<font color='red'>訊息：沒有此條件的文字雲,請調整時間.</font>");
    }
    else {
        $("#blockMessage").html("&nbsp;");
    }

    /*===修正為v4語法*/
    var fill = d3.scaleOrdinal(d3.schemeCategory20);
    var defaultFont = 'Roboto Condensed,Microsoft Jhenghei,Helvetica Neue,Arial,Geneva,sans-serif';

    var tooltip = d3.select("#blockPopUp").append("div")
        .attr("class", "tooltip")
        .style("opacity", 0);


    d3.layout.cloud()
        .size([BlockWidth, 150])
        .words(
            __newData.map(function (arg) {
                return { text: arg.text, size: 10 + arg.size * 50, sizeorg: arg.size };
            })
        )
        .padding(3)
        .rotate(0)
        .font(defaultFont)
        .fontSize(function (d) {
            return d.size;
        })
        .on("end", draw)
        .start();

    function draw(words) {
        d3.select("#blockTag")
            .append("svg")
            .attr("width", BlockWidth)
            .attr("height", 150)
            .append("g")
            .attr("transform", "translate(" + BlockWidth / 2 + ",75)")
            .selectAll("text")
            .data(words)
            .enter()
            .append("text")
            .style("font-size", function (d) { return d.size + "px"; })
            .style("font-family", defaultFont)
            .style("fill", function (d, i) {
                /*紅,橙,藍,綠*/
                if (d.sizeorg >= 0.3) { return "red"; }
                else if (d.sizeorg >= 0.2 && d.sizeorg < 0.3) { return "#FF7F0E"; }
                else if (d.sizeorg >= 0.1 && d.sizeorg < 0.2) { return "#ADC7EF"; }
                else if (d.sizeorg >= 0 && d.sizeorg < 0.1) { return "#9CDF8C"; }
                else { return "grey"; }
            })
            .attr("text-anchor", "middle")
            .attr("transform", function (d) {
                return "translate(" + [d.x, d.y] + ")rotate(" + d.rotate + ")";
            })
            .text(function (d) {
                return d.text;
            })
            .on('mouseover', function (d) {

            })
            .on("mouseout", function (d) {
                tooltip.transition().duration(500).style("opacity", 0);
            });
    }
}

// 字詞分類項目
function getResources(color) {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../Handler/GetResources.aspx",
        data: {
            ProjectGuid: $("#tmpPjGuid").val()
        },
        error: function (xhr) {
            alert(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                var researchAry = [];
                var ULstr = '<li><input type="checkbox" id="Topic_all" value="" name="cbTopic" checked="checked" /><label for="Topic_all" colorstr="#FFFFFF !important;" style="margin-right: 5px; font-weight: bold; font-size: 18px; font-family: Segoe UI; background-color:#FFFFFF !important;">All</label></li>';
                if ($("#tmpBrowser").val() == "internetexplorer") // IE
                    var color = ["lightpink !important", "sandybrown !important", "yellow !important", "lightgreen !important", "lightskyblue !important", "mediumpurple !important"];
                else //Chrome & FireFox & Edge
                    var color = ["red_print", "orange_print", "yellow_print", "green_print", "blue_print", "purple_print"];
                //var color = ["#FF7575", "#FFA042", "#F9F900", "#02DF82", "#46A3FF", "#CA8EFF"];
                $(data).find("topic_item").each(function (i) {
                    var ColorStr = (i > 5) ? GetRandomColor() : color[i];
                    if ($("#tmpBrowser").val() == "internetexplorer") // IE
                        ULstr += '<li><input type="checkbox" id="Topic' + i + '" value="' + $(this).children("research_guid").text().trim() + '" name="cbTopic" checked="checked" /><label for="Topic' + i + '" colorstr="' + ColorStr + '" style="margin-right: 5px; font-weight: bold; font-size: 18px; background-color:' + ColorStr + '">' + $(this).children("name").text().trim() + '</label></li>';
                    else
                        ULstr += '<li><input type="checkbox" id="Topic' + i + '" value="' + $(this).children("research_guid").text().trim() + '" name="cbTopic" checked="checked" /><label class="' + ColorStr + '" for="Topic' + i + '" colorstr="' + ColorStr + '" style="margin-right: 5px; font-weight: bold; font-size: 18px;">' + $(this).children("name").text().trim() + '</label></li>';
                    researchAry.push($(this).children("research_guid").text().trim());
                });
                $("#topicTag").empty();
                $("#topicTag").append(ULstr);
                $("#TypeAry").val(researchAry);
            }
        }
    });
}

// 刪除沒出現關鍵字的分類
function RemoveResources(Ary) {
    $.each(Ary, function (key, value) {
        $('#topicTag li input[value="' + value + '"]').parent().remove();
    });
}

// 隨機顏色
function GetRandomColor() {
    var letters = '0123456789ABCDEF'.split('');
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

// 使用者回饋資訊
function GetFeedBack() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "projectHandler/GetFeedBack.aspx",
        data: {
            atGuid: $.getQueryString("atGuid")
        },
        error: function (xhr) {
            alert(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                // 若為專案管理員或系統管理員
                if ($("#tmpComp").val() == "Y") {
                    if ($(data).find("data_item").length > 0) {
                        $(data).find("data_item").each(function () {
                            var score = parseInt($(this).attr("star_rating"))
                            $("a[name='star']").each(function (i) {
                                if ((i + 1) <= score)
                                    $(this).find("span").addClass("StarChecked");
                                else
                                    $(this).find("span").removeClass("StarChecked");
                            });
                            $("#ScoreBlock").show();
                            $("#RankScore").html($(this).attr("star_rating"));
                            $("#feedbackStr").val($(this).attr("user_feedback"));
                            if (score > 0)
                                $("#ranked").val("Y");
                        });
                    }
                }
                else {
                    $("#FeedBack").hide();
                }
            }
        }
    });
}

function printstem(word) {
    return language.stem(word);
}

//列印功能
function printHtml() {
    //var bodyHtml = document.body.innerHTML;
    //var newHtml = $("#printarea").html();
    //document.body.innerHTML = newHtml;
    window.print();
    //document.body.innerHTML = bodyHtml;
    
}
