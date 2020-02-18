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
    var __pjguid = $('#pjGuid').val();
    var __cloud_researchGuid = $('#cloud_researchGuid').val();
    var __cloud_date0 = $('#cloud_date0').val();

    var __endDate = "";
    var __startDate = "";

    if (__cloud_date0 == 0)
    {
        /*===表示不限時間時*/
        __endDate = "9"
        __startDate = "9";
    }
    else
    {
        /*===get end date*/
        var __today = new Date();
        var __endDate = "" + __today.getFullYear()
            + ((__today.getMonth() + 1) < 10 ? "0" + (__today.getMonth() + 1) : (__today.getMonth() + 1))
            + (__today.getDate() < 10 ? "0" + __today.getDate() : __today.getDate())
        ;

        /*===get start date*/
        var __newDay = new Date();
        __newDay.setDate(__newDay.getDate() - __cloud_date0);
        var __startDate = "" + __newDay.getFullYear()
            + ((__newDay.getMonth() + 1) < 10 ? "0" + (__newDay.getMonth() + 1) : (__newDay.getMonth() + 1))
            + (__newDay.getDate() < 10 ? "0" + __newDay.getDate() : __newDay.getDate())
        ;
    }

    /*===get url*/
    //__url="http://61.61.246.46/archives/word_cloud/project/69A0128B-057D-4F9C-B589-943E18916237/start_date/9/end_date/9/"; //舊API
    //__url ="http://61.61.246.46/archives/project_word_cloud/project/D4BF39D3-0A0E-429B-A89F-0D20A7BBEE7B"; //新API  (2020/1/16 by nick)
    var __url = "http://61.61.246.46/archives/project_word_cloud/project/"
        + __pjguid
    //    + "/start_date/" + __startDate
    //    + "/end_date/" + __endDate;
    //alert(__url);

    /*===get ajax*/
    $("#blockMessage").html("<img src='../images/loading1.gif'><font color='red'>資料查詢中...</font></img>");
    /* 測試資料 start*/
    var jsonval = { "ces": 1.0, "nvidia partners": 0.1, "car": 0.17, "drive": 0.26, "vehicles": 0.39, "driving smart": 0.12, "autonomous": 0.05, "ceo": 0.48, "power": 0.17, "mercedes": 0.04, "unveils": 0.09, "assistance": 0.02, "advanced": 0.02, "control": 0.07, "samsung": 0.85, "systems": 0.02, "driver": 0.02, "huang said": 0.02, "powerful use": 0.02, "announced": 0.25, "expands": 0.13, "launched": 0.21, "data": 0.72, "hit": 0.02, "shapiro": 0.02, "processors": 0.21, "intelligence": 0.25, "automotive": 0.39, "technologies": 0.12, "quantum": 0.19, "mobileye": 0.13, "compute": 0.11, "lenovo": 0.17, "chipsetrobotic": 0.17, "business tech": 0.07, "mobility": 0.09, "windows": 0.01, "network technology": 0.01, "techrepublic": 0.24, "cheaper": 0.19, "surface": 0.19, "conference": 0.09, "sunday": 0.01, "services": 0.11, "mapping": 0.17, "outlines": 0.14, "phone": 0.1, "preview": 0.11, "partnered": 0.01, "deals": 0.01 }
    setDataResult(jsonval);
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
            //alert(jqXHR.responseText);
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
