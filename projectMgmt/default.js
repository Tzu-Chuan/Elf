/*pageList*/

/*===============*/
/*===init*/
jQuery(document).ready(function () {
    GetManager();

    /*當InitiateProject_01 開啟時執行*/
    $("#InitiateProject_01").on("show.bs.modal", function (e) {
        document.getElementById("formFileUplad").reset();/*重置*/
        $("#message_01").text("");/*清空*/
    });

    /*InitiateProject_01*/
    $("#btn_01_cencel").click(function (event) {
        $("#InitiateProject_01").modal('hide');
    });

    $("#btn_01_next").click(function (event) {
        excelUpload(event);
    });

    /*InitiateProject_02*/
    $("#btn_02a_cencel, #btn_02b_cencel").click(function (event) {
        $("#InitiateProject_02").modal('hide');
    });
    $("#btn_02a_save, #btn_02b_save").click(function (event) {
        excelSave();
    });

});


/*===匯入*/
/*https://dotblogs.com.tw/cross/2010/09/21/17840 */
/*https://www.mkyong.com/jquery/jquery-ajax-submit-a-multipart-form */
function doUpload(event) {
    var fileSize = 0; //檔案大小
    var sizeLimit = 1024000;  //上傳上限，單位:byte，設定為1024k(1m)
    var f = document.getElementById("file1");
    var re = /\.(xlsx|xls)$/i;  //允許的副檔名

    var isIE = window.ActiveXObject || "ActiveXObject" in window
    //var isEdge = userAgent.indexOf("Edge") > -1;
    //var isFF = userAgent.indexOf("Firefox") > -1; 
    //var isSafari = userAgent.indexOf("Safari") > -1 && userAgent.indexOf("Chrome") == -1;
    //var isChrome = userAgent.indexOf("Chrome") > -1 && userAgent.indexOf("Safari") > -1 && !isEdge;


    //FOR IE
    if ($.browser.msie) {
        //檢查副檔名
        if (!re.test(f.value)) {
            alert("please upload excel format file!");
        }
        else {
            var img = new Image();
            img.src = f.value;
            img.onload = checkSize;
        }
    }
    //FOR Firefox,Chrome
    else {
        ////if (!re.test(f.files.item(0).name))
        ////if (re.test(f.files.item(0) == null))
        ////{
        ////    alert("please upload excel format file!");
        ////}
        ////else
        {
            fileSize = f.files.item(0).size;
            checkSize();
        }
    }

    //檢查檔案大小
    function checkSize() {
        ////////FOR IE FIX
        //////if ($.browser.msie)
        //////{
        //////    fileSize = this.fileSize;
        //////}

        ////////check
        //////if (fileSize > sizeLimit)
        //////{
        //////    Message((fileSize / 1024).toFixed(1), (sizeLimit / 1024));
        //////}
        //////else
        {
            //////////obj.form.action = "inputExcelCheck.aspx";
            //////////obj.form.submit();

            ajaxUpload(event);
        }
    }

    function Message(file, limit) {
        ////        var msg = "您所選擇的檔案大小為 " + file + " kB\n已超過上傳上限 " + limit + " kB\n不允許上傳！"
        var msg = "upload file size:" + file + " kB \nhad more than limit size: " + limit + " kB \nnot allowed to upload！"
        alert(msg);
    }
}

function excelUpload(event) {
    //stop submit the form, we will post it manually.
    event.preventDefault();

    $("#message_01").empty();

    // Get form
    var form = $('#formFileUplad')[0];

    // Create an FormData object 
    var data = new FormData(form);

    // If you want to add an extra field for the FormData
    ////data.append("CustomField", "This is some extra data, testing");

    // disabled the submit button
    $("#btn_01_next").prop("disabled", true);

    $.ajax({
        type: "POST",
        enctype: 'multipart/form-data',
        url: "mgmtHandler/UpLoadExcel.aspx",
        data: data,
        processData: false,
        contentType: false,
        cache: false,
        timeout: 600000,
        error: function (xhr) {
            $("#btn_01_next").prop("disabled", false);
            $("#message_01").text(xhr.responseText);
        },
        success: function (data) {
            $("#btn_01_next").prop("disabled", false);
            if ($(data).find("Error").length > 0) {
                $("#message_01").html($(data).find("Error").attr("Message"));
            }
            else {
                var contentStr = '';
                // websites
                contentStr += '<h3>Choose monitored websites：</h3>';
                contentStr += '<ol>';
                if ($(data).find("optsite_item").length > 0) {
                    $(data).find("optsite_item").each(function () {
                        contentStr += '<li><div class="checkbox"><label><input type="checkbox" name="optsite" value="' + $(this).children("optsite_name").text().trim() + '" />';
                        contentStr += '<a href="' + $(this).children("optsite_url").text().trim() + '" target="_black">' + $(this).children("optsite_name").text().trim() + '</a></label></div></li>';
                    });
                }
                contentStr += '</ol><br />';

                // Project Information
                contentStr += '<h3>Project Information：</h3>';
                contentStr += '<div style="margin-left:20px">';
                contentStr += '<table class="table table-striped"><tbody>';
                contentStr += '<tr><th style="width:130px;">Project Name：</th><td>' + $(data).find("Category[col_num='2']").children("rec").text().trim()+'</td></tr>';
                contentStr += '<tr><th>Item：</th><td>' + $(data).find("Category[col_num='3']").children("rec").text().trim() + '</td></tr>';
                var abbrStr = '';
                $(data).find("Category[col_num='4']").children("rec").each(function () {
                    if (abbrStr != "") abbrStr += '、';
                    abbrStr += $(this).text().trim();
                });
                contentStr += '<tr><th>Abbreviation：</th><td>' + abbrStr + '</td></tr>';
                contentStr += '</tbody></table></div><br />';

                // Project research topics and related word
                contentStr += '<h3>Project research topics and related word：</h3>';
                contentStr += '<ol>';
                $(data).find("Category").each(function (i) {
                    if (parseInt($(this).attr("col_num")) >= 5) {
                        contentStr += '<li>';
                        contentStr += '<div>' + $(this).attr("item_name") + '：</div>';
                        contentStr += '<div class="bg-color-grey2" style="padding:10px;">';
                        contentStr += '<div class="row">';
                        contentStr += '<ul style="list-style-type: decimal;">';
                        $(this).children("rec").each(function () {
                            contentStr += '<li class="col-xs-6 col-md-3"><span>' + $(this).text().trim() + '</span></li>';
                        });
                        contentStr += '</ul>';
                        contentStr += '</div></div>';
                        contentStr += '</li><br/>';
                    }
                });
                contentStr += '</ol>';

                $("#excelContent").html(contentStr);

                $("#InitiateProject_01").modal('hide');
                $("#InitiateProject_02").modal('show');
            }
        }
    });
}


/*===上載2*/
function excelSave() {
    $("#message_02").empty();

    var websiteStr = '';
    if ($("input[name='optsite']:checked").length == 0) {
        $("#message_02").text("Error message: please choose monitored websites!!");
        return false;
    }
    else {
        $("input[name='optsite']:checked").each(function () {
            if (websiteStr != '') websiteStr += ",";
            websiteStr += this.value;
        });
    }

    // disabled the submit button
    $("#btn_02a_save, #btn_02b_save").prop("disabled", true);
    
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "mgmtHandler/SaveExcel.aspx",
        data: {
            optsite: websiteStr
        },
        error: function (xhr) {
            $("#btn_02a_save, #btn_02b_save").prop("disabled", false);
            $("#message_02").text(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                $("#message_02").html($(data).find("Error").attr("Message"));
            }
            else {
                $("#btn_02a_save, #btn_02b_save").prop("disabled", false);
                getData(0);
                $("#InitiateProject_02").modal('hide');
            }
        }
    });
}


/*===func-啟動*/
function doPjStart(pjGuid) {
    if (confirm("Do you sure want to start this project?")) {
        execDelete();
    }

    function execDelete() {
        $.ajax({
            url: "setPjStart.aspx",
            type: "get",
            dataType: "html",
            //////cache: false,
            //////async: true,
            data: {
                pjGuid: pjGuid
            },
            success: function (data) {
                if (data == "OK.") {
                    getData(0);
                }
                else {
                    alert(data);
                }
            },
            error: function (jqXHR, textStatus, exception) {
                alert(jqXHR.responseText);
            }
        });
    }
}

/*===func-刪除*/
function doPjDelete(pjGuid) {
    if (confirm("Do you sure want to delete this project?")) {
        execDelete();
    }

    function execDelete() {
        $.ajax({
            url: "setPjDelete.aspx",
            type: "get",
            dataType: "html",
            //////cache: false,
            //////async: true,
            data: {
                pjGuid: pjGuid
            },
            success: function (data) {
                if (data == "OK.") {
                    getData(0);
                }
                else {
                    alert(data);
                }
            },
            error: function (jqXHR, textStatus, exception) {
                alert(jqXHR.responseText);
            }
        });
    }
}

/*===func-結案*/
function doPjClose(pjGuid) {
    if (confirm("Do you sure want to close this project?")) {
        execClose();
    }

    function execClose() {
        $.ajax({
            url: "setPjClose.aspx",
            type: "get",
            dataType: "html",
            data: {
                pjGuid: pjGuid
            },
            success: function (data) {
                if (data == "OK.") {
                    getData(0);
                }
                else {
                    alert(data);
                }
            },
            error: function (jqXHR, textStatus, exception) {
                alert(jqXHR.responseText);
            }
        });
    }
}

/*===func-匯出*/
function doExport(pjGuid) {
    location.href = "outputExcel.aspx?pjGuid=" + pjGuid;
}

function GetManager() {
    $.ajax({
        type: "POST",
        async: false, //在沒有返回值之前,不會執行下一步動作
        url: "../Handler/GetManagerRight.aspx",
        error: function (xhr) {
            alert(xhr.responseText);
        },
        success: function (data) {
            if ($(data).find("Error").length > 0) {
                alert($(data).find("Error").attr("Message"));
            }
            else {
                if ($(data).find("data_item").length > 0)
                    $("#newProject").show();
                else
                    $("#newProject").hide();
            }
        }
    });
}
