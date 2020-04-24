/*pageList*/


/*############################################################################*/
/*===文字雲搜尋*/
function doClouldSearch()
{
    getCloudSearchData();
}

/*===文字雲取資料*/
function getCloudSearchData()
{
    /*===clear cloud*/
    $('#blockTag').html("");

    /*===get param*/
    var __pjguid = $.getQueryString("pjGuid");

    /*===get url*/
    var __url = "http://61.61.246.46/archives/project_word_cloud/project/" + __pjguid;

    /*===get ajax*/
    $("#blockMessage").html("<img src='../images/loading1.gif'><font color='red'>資料查詢中...</font></img>");
    /* 測試資料 start*/
    //var jsonval = {"coronavirus": 1.0, "tech": 0.13, "times": 0.03, "technology": 0.02, "spreads": 0.02, "including": 0.06, "dashboard": 0.02, "financial": 0.05, "chinese": 0.02, "china": 0.02, "live data": 0.01, "cases": 0.17, "security": 0.09, "confirmed": 0.05, "traveler": 0.01, "outbreak": 0.01, "recoveries": 0.01, "health": 0.24, "messonnier": 0.01, "bugprevents": 0.01, "australia": 0.01, "amid": 0.01, "reports": 0.01, "centerfor": 0.01, "sms": 0.01, "safecybersecurity": 0.01, "world": 0.01, "province": 0.01, "affecting": 0.01, "tracking": 0.1, "disease": 0.05, "controland": 0.01, "areas": 0.01, "eastrespiratory syndrome": 0.01, "ofinfected": 0.01, "learning": 0.01, "withchinese investigators": 0.01, "techrepublic": 0.12, "conference": 0.01, "online": 0.05, "deadly": 0.01, "boston": 0.01, "harvardmedical": 0.01, "virus": 0.03, "ntruth": 0.05, "people": 0.04, "infected": 0.07, "media": 0.02, "posts": 0.02, "nthe information": 0.01}
    //setDataResult(jsonval);
    /* 測試資料 end*/
	
    $.ajax({
        url: __url,
        type: "get",
        dataType: 'json',
        success: function (jsonData)
        {
            setDataResult(jsonData);
        },
        error: function (jqXHR, textStatus, exception)
        {
            $("#blockMessage").html("結果訊息：" + jqXHR.responseText);
        }
    });

    function setDataResult(jsonData)
    {
        var __newData = new Array();
        var __obj = null;

        $.each(jsonData, function (key1, value1)
        {
            //舊API
            //if (key1.toLowerCase() == __cloud_researchGuid.toLowerCase())
            //{
                //$.each(value1, function (key2, value2)
                //{
                //    __obj = { 'text': key2, 'size': value2 }
                //    __newData.push(__obj);
                //});
            //}

            //新API 
            __obj = { 'text': key1, 'size': value1 }
            __newData.push(__obj);
        });

        /*===check array length*/
        if (__newData.length == 0)
        {
            ////alert(__newData.length);
            $("#blockMessage").html("<font color='red'>訊息：沒有此條件的文字雲,請調整時間.</font>");
        }
        else
        {
            $("#blockMessage").html("&nbsp;");
        }

        /*===修正為v4語法*/
        //var fill = d3.scale.category20();
        var fill = d3.scaleOrdinal(d3.schemeCategory20);
        ////var defaultFont = '"微軟正黑體",Impact';
        ////var defaultFont = '"微軟正黑體",Impact';
        var defaultFont = 'Roboto Condensed,Microsoft Jhenghei,Helvetica Neue,Arial,Geneva,sans-serif';
        ////var defaultFont = 'Impact';

        var tooltip = d3.select("#blockPopUp").append("div")
        .attr("class", "tooltip")
        .style("opacity", 0);


        d3.layout.cloud()
        .size([1000, 150])
        .words(
           __newData.map(function (arg)
           {
               //////var newsize = d.size;
               //////if (d.size <= 0.1) { newsize = 0.1; }
               //////else if (d.size <= 0.2 && d.size > 0.1) { newsize = 0.2; }
               //return 10 + d.size * 50;

               //return { text: arg.text, size: 10 + Math.random() * 50 };
               return { text: arg.text, size: 10 + arg.size * 35, sizeorg: arg.size };
           })
        )
        .padding(3)
        .rotate(0)
        //.font('"微軟正黑體",Impact')
        .font(defaultFont)
        .fontSize(function (d)
        {
            return d.size;
        })
        .on("end", draw)
        .start();

        function draw(words)
        {
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
            .style("fill", function (d, i)
            {
                /*紅,橙,藍,綠*/
                ////if (d.sizeorg >= 0.8) { return fill(i); }
                if (d.sizeorg >= 0.9) { return "red"; }
                else if (d.sizeorg >= 0.8 && d.sizeorg < 0.9) { return "#FF7F0E"; }
                else if (d.sizeorg >= 0.7 && d.sizeorg < 0.8) { return "#ADC7EF"; }
                else if (d.sizeorg >= 0.6 && d.sizeorg < 0.7) { return "#9CDF8C"; }
                    ////else { return "#AEC7E8"; }
                else { return "grey"; }
            })
            .attr("text-anchor", "middle")
            .attr("transform", function (d)
            {
                return "translate(" + [d.x, d.y] + ")rotate(" + d.rotate + ")";
            })
            .text(function (d)
            {
                return d.text;
            })
            //.on('click', function (d)
            //{
            //    //window.open(d.url);
            //})
            .on('mouseover', function (d)
            {
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
             .on("mouseout", function (d)
             {
                 tooltip.transition().duration(500).style("opacity", 0);
                 ////$('div#blockPopUp').hide()
             });
        }
    }
}
