$(document).ready(function () {
    // 文字雲
    WordCloud();

    // RWD 依視窗大小重繪文字雲
    d3.select(window).on('resize', WordCloud);

    getResources();

    getData();
    
    var TopDistance = $(".dropdowns").offset().top;
    $(window).scroll(function () {
        if ($(this).scrollTop() >= TopDistance) {          /* 要滑動到選單的距離 */
            $('.dropdowns').addClass('navFixed');   /* 幫選單加上固定效果 */
        } else {
            $('.dropdowns').removeClass('navFixed'); /* 移除選單固定效果 */
        }
    });

    //checkbox check all
    $(document).on("click", "#Topic_all", function () {
        if ($("#Topic_all").prop("checked")) {
            $("input[name='cbTopic']").each(function () {
                $(this).prop("checked", true);
            });
        } else {
            $("input[name='cbTopic']").each(function () {
                $(this).prop("checked", false);
            });
        }
    });

    $(document).on("click", "input[name='cbTopic']", function () {
        if ($(this).is(":checked"))
            $(this).closest("li").find("label").css("background-color", "red");
        else
            $(this).closest("li").find("label").css("background-color", "#FFFFFF");
    });
});

function getData() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "projectHandler/GetArticleDetail.aspx",
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
                if ($(data).find("data_item").length > 0) {
                    $(data).find("data_item").each(function (i) {
                        $("#Summary").html($(this).children("abstract_iekelf").text().trim());
                        $("#ArticleTitle").html($(this).children("title").text().trim());
                        $("#WebSite").html('Article from: <a target="_blank" href="' + $(this).children("optsite_url").text().trim() + '">' + $(this).children("website_name").text().trim() + '</a>');
                        $("#ArticleContent").html($(this).children("full_text").text().trim().replace(/\./g,".<br><br>"));
                    });
                }
            }
        }
    });
}

function WordCloud() {
    if ($("#tmpCloud").val() == "") {
        $.ajax({
            type: "POST",
            async: false, //在沒有返回值之前,不會執行下一步動作
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

function getResources() {
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
                var ULstr = '<li><input type="checkbox" id="Topic_all" value="" name="cbTopic" checked="checked" /><label for="Topic_all" style="margin-right: 5px; font-weight: bold; font-size: 18px; font-family: Segoe UI;">All</label></li>';
                $(data).find("topic_item").each(function (i) {
                    ULstr += '<li><input type="checkbox" id="Topic' + i + '" value="' + $(this).children("research_guid").text().trim() + '" name="cbTopic" checked="checked" /><label for="Topic' + i + '" style="margin-right: 5px; font-weight: bold; font-size: 18px; font-family: Segoe UI;">' + $(this).children("name").text().trim() + '</label></li>';
                });
                $("#topicTag").empty();
                $("#topicTag").append(ULstr);
            }
        }
    });
}