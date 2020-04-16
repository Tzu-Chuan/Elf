/*pageList*/

/*===============*/
/*init*/

jQuery(document).ready(function ()
{
});


/*===============*/
/*===搜尋*/
function doSearch()
{
    location.href = "default.aspx?q=" + encodeURIComponent($('#keyword').val());;
}

//========================
/*explain：*/
//========================
//SF200--挑工研人單選 (orgcd:限定單位) 
function getITRIemp(id, orgcd)
{
    getObj("com_cname").value = getVal("empname" + id);
    getObj("com_empno").value = getVal("empno" + id);

    var url = '../SF200/Qry_empno.aspx?orgcd=' + orgcd + '&depcd=Y&keyword=&p1=com_empno&p2=com_cname&p4=com_orgcd&p6=com_deptid&p8=com_email';
    window.showModalDialog(url, window, "resizable:yes;scroll:yes;status:no;dialogHeight=320px;dialogWidth=620px");

    getObj("empname" + id).value = getVal("com_cname");
    getObj("empno" + id).value = getVal("com_empno");

    if (getVal("com_deptid") != '')
    {
        //使用deptid再去取得 groupid (ISC-SF100)
        var ReqArgs = new Array();
        ReqArgs["com_deptid"] = getVal("com_deptid");
        var xHttp = new AjaxUtil().postForm("../SF200/getEmpGroup.aspx", ReqArgs);
        var result = xHttp.responseText;
        getObj("empgroup" + id).value = result;
        getObj("emptitle" + id).value = "({'SamAccountName':'" + getVal("com_empno") + "','DisplayName':'" + getVal("com_cname") + "','Email':'" + getVal("com_email") + "','Group':'" + getVal("empgroup" + id) + "'})";
    }
}

//SF200--挑工研人複選
function getITRIemps(id, orgcd)
{
    /*---------------------------------------*/
    /*explain：parpare*/
    getObj("h_mem_num").value = getVal("empno" + id);       /*explain：保留上一次的工號名單*/
    getObj("h_mem_name").value = getVal("empname" + id);    /*explain：保留上一次的姓名名單*/
    getObj("h_memlist").value = "";                         /*explain：result xml obj*/
    getObj("submitBtn").value = "0";                        /*explain：確認USER的操作是關閉視窗OR清空後按下submit*/
    var D4orgcd = orgcd;                                    /*explain：參數1：D4orgcd：若不預設單位，請代「空值」*/
    var changeorgcd = '1';                                  /*explain：參數2：changeorgcd：設定值為1、0，1為固定，0為可變動*/
    var url = '../SF200/temp_empchoose.aspx?D4orgcd=' + D4orgcd + '&changeorgcd=' + changeorgcd;
    window.showModalDialog(url, window, "resizable:yes;scroll:yes;status:no;dialogHeight=470px;dialogWidth=960px");

    //alert(getVal("h_memlist"));
    //alert(getVal("h_mem_num"));
    //alert(getVal("h_mem_name"));
    //alert(getVal("submitBtn"));
    if (getVal("h_memlist") != "")
    {
        /*---------------------------------------*/
        /*explain：Customized result*/
        var ReqArgs = new Array();
        ReqArgs["xml"] = getVal("h_memlist");
        ReqArgs["orgno"] = getVal("h_mem_num");
        ReqArgs["orgnm"] = getVal("h_mem_name");
        var xHttp = new AjaxUtil().postForm("../SF200/memlistSplit.aspx", ReqArgs);
        var empdata = xHttp.responseText.split('$$');
        /*---------------------------------------*/
        /*explain：check總人數限制 add by asamchang*/
        var testEmpArr = empdata[0].split(',');
        if (testEmpArr.length > 100)
        {
            alert("資料異動時,發生系統錯誤,此欄位挑選的總人數已達100人上限，無法新增!");
            return false;
        }
        /*---------------------------------------*/
        getObj("empno" + id).value = empdata[0];
        getObj("empname" + id).value = empdata[1];
        getObj("empgroup" + id).value = empdata[2];
        getObj("emptitle" + id).value = empdata[3];
        setVal("empaddno" + id, empdata[4]);
        setVal("empdelno" + id, empdata[5]);
    } else
    {
        if (getVal("submitBtn") == "1")
        {
            getObj("empno" + id).value = "";
            getObj("empname" + id).value = "";
            getObj("empgroup" + id).value = "";
            getObj("emptitle" + id).value = "";
            setVal("empaddno" + id, "");
            setVal("empdelno" + id, getVal("h_mem_num"));
        }
    }
}


//========================
/*explain：util for get element object、value、attribute*/
/*date:2012/4/v2/by asam*/
//========================
function getObj(idOrName)
{
    var doc = document;
    if (doc.getElementById(idOrName) != undefined)
        return doc.getElementById(idOrName);
    else
        return doc.getElementsByName(idOrName)[0] != undefined ? doc.getElementsByName(idOrName)[0] : undefined;
}

function getObjs(name)
{
    var doc = document;
    var elts = doc.getElementsByTagName("input");
    var count = 0;
    var elements = [];
    for (var i = 0; i < elts.length; i++)
    {
        if (elts[i].getAttribute("name") == name)
        {
            elements[count++] = elts[i];
        }
    }
    return elements;
    //return document.getElementsByName(name);
}

function getVal(idOrName)
{
    try
    {
        var oo = idOrName;
        var obj = getObj(oo);
        if (obj != undefined)
        {
            var tagnm = obj.tagName.toLowerCase();
            if (tagnm == "select")
            {
                return obj.value;
                //return obj.options[obj.selectedIndex].value;
            }
            else if (tagnm == "input")
            {
                var typenm = getAttrVal(oo, "type");
                if (typenm == "text" || typenm == "hidden")
                {
                    return obj.value;
                }
                else if (typenm == "radio")
                {
                    var objs = getObjs(oo);
                    for (i = 0; i < objs.length; i++)
                    {
                        if (objs[i].checked)
                        {
                            return objs[i].value;
                        }
                    }
                    return "";
                }
                else if (typenm == "checkbox")
                {
                    var objs = getObjs(oo);
                    var oArray = new Array();
                    for (i = 0; i < objs.length; i++)
                    {
                        if (objs[i].checked)
                        {
                            oArray.push(objs[i].value);
                        }
                    }
                    return oArray.join(",");
                }
                else
                {
                    throw "type錯誤,idOrName=" + idOrName;
                }
            }
            else if (tagnm == "textarea")
            {
                return obj.value;
            }
            else
            {
                throw "tag錯誤,idOrName=" + idOrName;
            }
        }
    }
    catch (e)
    {
        var result = '訊息：發生系統錯誤,請聯繫系統管理者(in common\\getVal)!';
        if (e != '')
            alert(result + "\nmsg：" + e);
        else
            alert(result + "\nmsg：" + e.message);
        return true;
    }
}

function getAttrVal(idOrName, attrName)
{
    if (getObj(idOrName) != undefined)
        return getObj(idOrName).getAttribute(attrName) != undefined ? getObj(idOrName).getAttribute(attrName) : undefined;
    else
        return undefined;
}

function getDefVal(idOrName)
{
    if (getObj(idOrName) != undefined)
        return getObj(idOrName).defaultValue != undefined ? getObj(idOrName).defaultValue : undefined;
    else
        return undefined;
}

function getSelText(idOrName)
{
    if (getObj(idOrName) != undefined)
        return getObj(idOrName).options != undefined ? getObj(idOrName).options[getObj(idOrName).selectedIndex].text : undefined;
    else
        return undefined;
}

function getSelAttrVal(idOrName, attrName)
{
    if (getObj(idOrName) != undefined)
        return getObj(idOrName).options != undefined ? getObj(idOrName).options[getObj(idOrName).selectedIndex].getAttribute(attrName) : undefined;
    else
        return undefined;
}


function setVal(idOrName, newStrVal)
{
    try
    {
        var oo = idOrName;
        var obj = getObj(oo);
        //var objv = (newStrVal != undefined) ? newStrVal.toString() : undefined;
        var objv = (newStrVal != undefined) ? newStrVal + '' : undefined;
        if (obj != undefined && objv != undefined)
        {
            var tagnm = obj.tagName.toLowerCase();
            if (tagnm == "select")
            {
                obj.value = objv;
            }
            else if (tagnm == "input")
            {
                var typenm = getAttrVal(oo, "type");
                if (typenm == "text" || typenm == "hidden")
                {
                    obj.value = objv;
                }
                else if (typenm == "radio")
                {
                    var objs = getObjs(oo);
                    for (i = 0; i < objs.length; i++)
                    {
                        objs[i].checked = false;
                        if (objs[i].value == objv)
                        {
                            objs[i].checked = true;
                        }
                    }
                }
                else if (typenm == "checkbox")
                {
                    var objs = getObjs(oo);
                    for (i = 0; i < objs.length; i++)
                    {
                        objs[i].checked = false;
                        if ((objv + ',').indexOf(objs[i].value + ',') != -1)
                        {
                            objs[i].checked = true;
                        }
                    }
                }
                else
                {
                    throw "type錯誤,idOrName=" + idOrName + ",newStrVal=" + newStrVal;
                }
            }
            else if (tagnm == "textarea")
            {
                obj.value = objv;
            }
            else
            {
                throw "tag錯誤,idOrName=" + idOrName + ",newStrVal=" + newStrVal;
            }
        }
    }
    catch (e)
    {
        var result = '訊息：發生系統錯誤,請聯繫系統管理者(in common\\setVal)!';
        if (e != '')
            alert(result + "\nmsg：" + e);
        else
            alert(result + "\nmsg：" + e.message);
        return true;
    }
}








/*===增管理人員*/
function doEmpAdd(myobj)
{
    //alert($("#empname1").val());
    //return;

    if ($("#empno1").val() != "")
    {
        execAdd();
    }
    else
    {
        alert("message：please select empno.")
    }

    function execAdd()
    {
        $.ajax({
            url: "setEmpAdd.aspx",
            type: "get",
            dataType: "html",
            //////cache: false,
            //////async: true,
            data: {
                role_id: $("#role_id").val()
                , empno: $("#empno1").val()
                , empname: $("#empname1").val()
                , orgcd: $("#com_orgcd").val()
                , deptid: $("#com_deptid").val()
            },
            success: function (result)
            {
                if (result == "OK.")
                {
                    setDataResult(result);
                }
                else
                {
                    alert(result);
                }
            },
            error: function (jqXHR, textStatus, exception)
            {
                alert(jqXHR.responseText);
            }
        });
    }

    function setDataResult(result)
    {
        location.reload();
    }
}

/*===刪管理人員*/
function doEmpDelete(empGuid)
{
    if (confirm("Do you sure want to delete this manager?"))
    {
        execDelete();
    }

    function execDelete()
    {
        $.ajax({
            url: "setEmpDelete.aspx",
            type: "get",
            dataType: "html",
            //////cache: false,
            //////async: true,
            data: {
                empGuid: empGuid
            },
            success: function (result)
            {
                if (result == "OK.")
                {
                    setDataResult(result);
                }
                else
                {
                    alert(result);
                }
            },
            error: function (jqXHR, textStatus, exception)
            {
                alert(jqXHR.responseText);
            }
        });
    }

    function setDataResult(result)
    {
        alert('Manager delete success.');
        location.reload();
    }
}