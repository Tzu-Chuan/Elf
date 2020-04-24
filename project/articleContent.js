$(document).ready(function () {
    // 文字雲
    WordCloud();

    getData();
    
    var TopDistance = $(".dropdowns").offset().top;
    $(window).scroll(function () {
        if ($(this).scrollTop() > TopDistance) {          /* 要滑動到選單的距離 */
            $('.dropdowns').addClass('navFixed');   /* 幫選單加上固定效果 */
        } else {
            $('.dropdowns').removeClass('navFixed'); /* 移除選單固定效果 */
        }
    });

    $(document).on("click", "input[name='cbDate']", function () {
        if ($(this).is(":checked"))
            $(this).closest("li").find("label").css("background-color", "#FF2D2D");
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
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "projectHandler/GetArticleWordCloud.aspx",
        data: {
            atGuid: $.getQueryString("atGuid")
        },
        success: function (data) {
            setDataResult($("textrank_keyword", data).text());
        },
        error: function (jqXHR, textStatus, exception) {
            //alert(jqXHR.responseText);
            $("#blockMessage").html("結果訊息：" + jqXHR.responseText);
        }
    });
}

function setDataResult(jsonData) {
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
        .size([1000, 150])
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
            .attr("width", 1000)
            .attr("height", 150)
            .append("g")
            .attr("transform", "translate(500,75)")
            .selectAll("text")
            .data(words)
            .enter()
            .append("text")
            .style("font-size", function (d) { return d.size + "px"; })
            .style("font-family", defaultFont)
            ////.style("cursor", 'pointer')//當滑鼠移上去時，變換cursor
            ////.style("fill", function (d, i) { return fill(i); })
            .style("fill", function (d, i) {
                /*紅,橙,藍,綠*/
                ////if (d.sizeorg >= 0.8) { return fill(i); }
                if (d.sizeorg >= 0.3) { return "red"; }
                else if (d.sizeorg >= 0.2 && d.sizeorg < 0.3) { return "#FF7F0E"; }
                else if (d.sizeorg >= 0.1 && d.sizeorg < 0.2) { return "#ADC7EF"; }
                else if (d.sizeorg >= 0 && d.sizeorg < 0.1) { return "#9CDF8C"; }
                ////else { return "#AEC7E8"; }
                else { return "grey"; }
            })
            .attr("text-anchor", "middle")
            .attr("transform", function (d) {
                return "translate(" + [d.x, d.y] + ")rotate(" + d.rotate + ")";
            })
            .text(function (d) {
                return d.text;
            })
            //.on('click', function (d)
            //{
            //    //window.open(d.url);
            //})
            .on('mouseover', function (d) {
                //////var matrix = this.getScreenCTM();
                //////tooltip.transition().duration(200).style("opacity", .9);
                //////tooltip.html("ggg")
                //////  .style("left", (window.pageXOffset + matrix.e) + "px")
                //////  .style("top", (window.pageYOffset + matrix.f) + "px");

                //window.open(d.url);
                //alert(d.size)

                ////$('div#blockPopUp').html("score:" + d.sizeorg)
                ////$('div#blockPopUp').show()
                ////.css('top', d3.event.clientY-150)
                ////.css('left', d3.event.clientX-140)
            })
            .on("mouseout", function (d) {
                tooltip.transition().duration(500).style("opacity", 0);
                ////$('div#blockPopUp').hide()
            });
    }
}