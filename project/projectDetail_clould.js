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
	//var jsonval = { "65CBB871-813C-43CA-A9B1-C73819BC182D": { "Driver": 0.36, "Development": 0.54, "heat": 0.03, "device": 0.17, "Packaging": 0.05, "Repairing": 0.01, "Chip": 0.37, "production": 0.26, "process": 1.0 }, "3D635AD5-424C-4EC1-8B4C-D38FCB271D61": { "university": 0.31, "department": 0.76, "Columbia University": 0.01, "Intel": 1.0, "Microsoft": 0.64, "IBM": 0.1 }, "5B8C2BE8-8D07-480D-8A75-98700FC0E334": { "quantum": 0.72, "mobility": 0.43, "particularity": 0.05, "performance": 0.01, "conductivity": 0.17, "feature": 1.0, "property": 0.72 }, "ACC8256F-8943-4E76-A6A4-1A89C0DF9936": { "robot": 0.09, "vehicle": 0.33, "claim": 0.18, "Gaming": 0.06, "industry": 0.5, "smartphone": 0.01, "computer": 0.66, "IoT": 0.03, "car": 1.0, "Display": 0.08, "Monitor": 0.23, "AI": 0.11, "application": 0.15, "Watch": 0.09, "Automotive": 0.19 }, "EADC3D9F-7204-4FBC-BA98-CBFAFB2DFC02": { "infrastructure": 0.39, "demand": 0.01, "billion": 1.0, "global": 0.17, "future": 0.45, "million": 0.56, "generation": 0.17 }, "all": { "Driver": 0.36, "Development": 0.54, "heat": 0.03, "device": 0.17, "Packaging": 0.05, "Repairing": 0.01, "Chip": 0.37, "production": 0.26, "process": 1.0, "university": 0.31, "department": 0.76, "Columbia University": 0.01, "Intel": 1.0, "Microsoft": 0.64, "IBM": 0.1, "quantum": 0.72, "mobility": 0.43, "particularity": 0.05, "performance": 0.01, "conductivity": 0.17, "feature": 1.0, "property": 0.72, "robot": 0.09, "vehicle": 0.33, "claim": 0.18, "Gaming": 0.06, "industry": 0.5, "smartphone": 0.01, "computer": 0.66, "IoT": 0.03, "car": 1.0, "Display": 0.08, "Monitor": 0.23, "AI": 0.11, "application": 0.15, "Watch": 0.09, "Automotive": 0.19, "infrastructure": 0.39, "demand": 0.01, "billion": 1.0, "global": 0.17, "future": 0.45, "million": 0.56, "generation": 0.17 } }
 //   setDataResult(jsonval);
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
