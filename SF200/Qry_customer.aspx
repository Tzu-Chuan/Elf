<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Qry_customer.aspx.cs" Inherits="Qry_customer" %>

<%@ Register Src="UserControl/userctrl_RegexValidator.ascx" TagName="userctrl_RegexValidator"
    TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>客戶資料查詢</title>
    <base target="_self" />
    <link href="CSS/itriap.css" type="text/css" rel="stylesheet" />

    <script language="javascript" type="text/javascript">
    
    function WinOpen(url)
    {
        if (url.indexOf('?') > 0)
        {
            url += "&";
        }
        else
        {
            url += "?";
        }

        url += "rnd=" + Math.random();
		//window.showModalDialog(url, window, "resizable:yes;scroll:yes;status:no;dialogHeight="+height+"pt;dialogWidth="+width+"pt");
		window.open(url);
    }
    
    

        <!--
        /* 傳入相關參數說明
        // ==============================
        // keyword = 搜尋關鍵字
        // oricountry  =國內外客戶 不傳此值 default帶'' 全部客戶
        // p1 = comp_idno		客戶編號
        // p2 = comp_cname		客戶名稱
        // p3 = comp_postno		郵政區號
        // p4 = comp_phone	    聯絡人電話
        // p5 = comp_fax		聯絡人傳真
        // p6 = addr		    聯絡人地址
        // p7 = comp_chairman	聯絡人姓名
        // p8 = comp_cmtitle	職稱
        // ==============================
        

        */
	    //-->	
	    
	    function setValue(v1,v2,v3,v4,v5,v6,v7,v8)
        {
	        var topWin = null;
	        if(window.opener)	
		        topWin = window.opener;
	        else if (window.dialogArguments)
		        topWin = window.dialogArguments;
	        else
		        return false;
		        
		        
		 if(topWin.document.getElementById("<%=Request["p1"]%>")!=null) topWin.document.getElementById("<%=Request["p1"]%>").value=v1;
         if(topWin.document.getElementById("<%=Request["p2"]%>")!=null) topWin.document.getElementById("<%=Request["p2"]%>").value=v2;
         if(topWin.document.getElementById("<%=Request["p3"]%>")!=null) topWin.document.getElementById("<%=Request["p3"]%>").value=v3;
         if(topWin.document.getElementById("<%=Request["p4"]%>")!=null) topWin.document.getElementById("<%=Request["p4"]%>").value=v4;
         if(topWin.document.getElementById("<%=Request["p5"]%>")!=null) topWin.document.getElementById("<%=Request["p5"]%>").value=v5;
         if(topWin.document.getElementById("<%=Request["p6"]%>")!=null) topWin.document.getElementById("<%=Request["p6"]%>").value=v6;
         if(topWin.document.getElementById("<%=Request["p7"]%>")!=null) topWin.document.getElementById("<%=Request["p7"]%>").value=v7;
         if(topWin.document.getElementById("<%=Request["p8"]%>")!=null) topWin.document.getElementById("<%=Request["p8"]%>").value=v8;

	        self.close();
	        return false;
        }

        function clearValue(v1,v2,v3,v4,v5,v6,v7,v8)
        {
	        var topWin = null;
	        if(window.opener)	
		        topWin = window.opener;
	        else if (window.dialogArguments)
		        topWin = window.dialogArguments;
	        else
		        return false;

	     if(topWin.document.getElementById("<%=Request["p1"]%>")!=null) topWin.document.getElementById("<%=Request["p1"]%>").value=v1;
         if(topWin.document.getElementById("<%=Request["p2"]%>")!=null) topWin.document.getElementById("<%=Request["p2"]%>").value=v2;
         if(topWin.document.getElementById("<%=Request["p3"]%>")!=null) topWin.document.getElementById("<%=Request["p3"]%>").value=v3;
         if(topWin.document.getElementById("<%=Request["p4"]%>")!=null) topWin.document.getElementById("<%=Request["p4"]%>").value=v4;
         if(topWin.document.getElementById("<%=Request["p5"]%>")!=null) topWin.document.getElementById("<%=Request["p5"]%>").value=v5;
         if(topWin.document.getElementById("<%=Request["p6"]%>")!=null) topWin.document.getElementById("<%=Request["p6"]%>").value=v6;
         if(topWin.document.getElementById("<%=Request["p7"]%>")!=null) topWin.document.getElementById("<%=Request["p7"]%>").value=v7;
         if(topWin.document.getElementById("<%=Request["p8"]%>")!=null) topWin.document.getElementById("<%=Request["p8"]%>").value=v8;
	        

	        return false;
        }
        
        function htmlDecode(str)
        {
           var div = document.createElement("div");
           div.innerHTML = str;
           
           //alert(div.innerHTML);
           
           return div.innerHTML;
        }
    </script>

</head>
<body>
    <form id="Form1" method="post" runat="server">
        <div class="content-sub">
            <table class="border-none">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td style="width: 50%">
                                    關鍵字：<asp:TextBox ID="tbx_keyword" runat="server" Width="120px"></asp:TextBox></td>
                                <td style="width: 20%">
                                    <asp:Button ID="btn_Query" onmouseover="this.className='btn_mouseover'" onmouseout="this.className='btn_mouseout'"
                                        runat="server" CssClass="button" Text="查詢" OnClick="btn_Query_Click"></asp:Button>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btn_Add" runat="server" CssClass="button" 
                                        onmouseout="this.className='btn_mouseout'" onmouseover="this.className='btn_mouseover'"
                                        Text="新增" OnClick="btn_Add_Click" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                    <asp:GridView ID="gv_Cust" runat="server" DataSourceID="ds_custquery" AllowSorting="True"
                                        AllowPaging="True" CellSpacing="2" GridLines="None" AutoGenerateColumns="False"
                                        OnRowDataBound="gv_Cust_RowDataBound">
                                        <HeaderStyle CssClass="Theader" HorizontalAlign="Center" />
                                        <PagerSettings Mode="NumericFirstLast" FirstPageImageUrl="Images/move_first.gif"
                                            LastPageImageUrl="Images/move_lest.gif" />
                                        <PagerStyle HorizontalAlign="Left" CssClass="PagerStyle" />
                                        <AlternatingRowStyle CssClass="TRowEven" HorizontalAlign="Center" Height="23px" />
                                        <RowStyle HorizontalAlign="Center" Height="23px" />
                                        <EmptyDataRowStyle CssClass="Theader" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:BoundField HeaderText="客戶編號" DataField="comp_idno" SortExpression="comp_idno" >
                                                <ItemStyle Width="12%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="客戶名稱" SortExpression="comp_cname">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Text='<%# Eval("comp_cname") %>'
                                                        OnClientClick='setValue("1","1","1","1","1","1","1","1")'></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="40%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <!--當找不到資料時則顯示「查無資料」-->
                                            <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="查無資料!"></asp:Label>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <uc1:userctrl_RegexValidator ID="uc_regex" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:SqlDataSource ID="ds_custquery" runat="server" ConnectionString="<%$ ConnectionStrings:pubbs %>"
                SelectCommand="pr_comnwebap_customer" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Name="keyword" />
                    <asp:Parameter Name="oricountry" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
