<%@ Page Language="c#" CodeFile="Qry_projno.aspx.cs" AutoEventWireup="true" Inherits="Qry_projno" %>

<%@ Register Src="./UserControl/userctrl_RegexValidator.ascx" TagName="userctrl_RegexValidator"
    TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>規劃案號查詢</title>
    <base target="_self" />
    <link href="CSS/itriap.css" type="text/css" rel="stylesheet" />

    <script language="javascript" type="text/javascript">

        <!--
        /* 傳入相關參數說明
        // ==============================
        // keyword = 搜尋關鍵字
        // p1 = s20_pojno		計畫代號
        // p2 = s20_pojcname	計畫名稱
        // p3 = s20_year    	計畫年度(西元)
        // p4 = s20_orgcd   	單位(代號)
        // ==============================
        */
	    //-->	
	    
	    function setValue(v1,v2,v3,v4)
        {
	        var topWin = null;
	        if(window.opener)	
		        topWin = window.opener;
	        else if (window.dialogArguments)
		        topWin = window.dialogArguments;
	        else
		        return false;

	        if(typeof(topWin.document.all["<%=Request["p1"]%>"]) == "object")	{topWin.document.all["<%=Request["p1"]%>"].value = v1;}
	        if(typeof(topWin.document.all["<%=Request["p2"]%>"]) == "object")	{topWin.document.all["<%=Request["p2"]%>"].value = v2;}
	        if(typeof(topWin.document.all["<%=Request["p3"]%>"]) == "object")	{topWin.document.all["<%=Request["p3"]%>"].value = v3;}
	        if(typeof(topWin.document.all["<%=Request["p4"]%>"]) == "object")	{topWin.document.all["<%=Request["p4"]%>"].value = v4;}

	        self.close();
	        return false;
        }

        function clearValue(v1,v2,v3,v4)
        {
	        var topWin = null;
	        if(window.opener)	
		        topWin = window.opener;
	        else if (window.dialogArguments)
		        topWin = window.dialogArguments;
	        else
		        return false;

	        if(typeof(topWin.document.all["<%=Request["p1"]%>"]) == "object")	{topWin.document.all["<%=Request["p1"]%>"].value = v1;}
	        if(typeof(topWin.document.all["<%=Request["p2"]%>"]) == "object")	{topWin.document.all["<%=Request["p2"]%>"].value = v2;}
	        if(typeof(topWin.document.all["<%=Request["p3"]%>"]) == "object")	{topWin.document.all["<%=Request["p3"]%>"].value = v3;}
	        if(typeof(topWin.document.all["<%=Request["p4"]%>"]) == "object")	{topWin.document.all["<%=Request["p4"]%>"].value = v4;}

	        return false;
        }
        
        function chk_year(theobj)
        { 
	        if (isNaN(theobj.value)) 
	        {
		        alert(theobj.value+' 不是有效年度');
		        theobj.value='';
		        theobj.focus();
	        }
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
                                <td style="width: 20%">
                                    年度：<asp:TextBox ID="tbx_year" runat="server" Width="35px" MaxLength="4"></asp:TextBox></td>
                                <td style="width: 30%">
                                    單位：<asp:DropDownList ID="ddl_orgcd" runat="server" DataTextField="orgcd_name" DataValueField="orgcd"
                                        DataSourceID="ds_org">
                                    </asp:DropDownList></td>
                                <td style="width: 43%">
                                    關鍵字：<asp:TextBox ID="tbx_keyword" runat="server" Width="120px"></asp:TextBox></td>
                                <td style="width: 7%">
                                    <asp:Button ID="btn_Query" onmouseover="this.className='btn_mouseover'" onmouseout="this.className='btn_mouseout'"
                                        runat="server" CssClass="button" Text="查詢" OnClick="btn_Query_Click"></asp:Button></td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <hr />
                                    <asp:GridView ID="gv_proj" runat="server" DataSourceID="ds_proj" AllowSorting="true"
                                        AllowPaging="true" CellSpacing="2" GridLines="none" AutoGenerateColumns="false"
                                        OnRowCommand="gv_proj_RowCommand" OnRowCreated="gv_proj_RowCreated">
                                        <HeaderStyle CssClass="Theader" HorizontalAlign="center" />
                                        <PagerSettings Mode="NumericFirstLast" FirstPageImageUrl="Images/move_first.gif"
                                            LastPageImageUrl="Images/move_lest.gif" />
                                        <PagerStyle HorizontalAlign="left" CssClass="PagerStyle" />
                                        <AlternatingRowStyle CssClass="TRowEven" HorizontalAlign="center" Height="23px" />
                                        <RowStyle HorizontalAlign="center" Height="23px" />
                                        <EmptyDataRowStyle CssClass="Theader" HorizontalAlign="center" />
                                        <EmptyDataTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        計畫年度</td>
                                                    <td style="width: 12%">
                                                        單位</td>
                                                    <td style="width: 20%">
                                                        計畫代號</td>
                                                    <td style="width: 40%">
                                                        計畫名稱</td>
                                                    <td style="width: 13%">
                                                        計畫主持人</td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:BoundField HeaderText="計畫年度" DataField="s20_year" SortExpression="s20_year"
                                                ItemStyle-Width="15%" />
                                            <asp:BoundField HeaderText="單位" DataField="org_abbr_chnm2" SortExpression="s20_orgcd"
                                                ItemStyle-Width="12%" />
                                            <asp:TemplateField HeaderText="計畫代號" SortExpression="s20_pojno" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" Text='<%# Eval("s20_pojno") %>' CommandName="Select"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="計畫名稱" DataField="s20_pojcname" SortExpression="s20_pojcname"
                                                ItemStyle-Width="40%" ItemStyle-HorizontalAlign="left" />
                                            <asp:BoundField HeaderText="計畫主持人" DataField="s20_nminchrg" SortExpression="s20_nminchrg"
                                                ItemStyle-Width="13%" />
                                            <asp:BoundField DataField="s20_orgcd" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:SqlDataSource ID="ds_org" runat="server" ConnectionString="<%$ ConnectionStrings:common %>"
                SelectCommand="select org_orgcd AS orgcd, org_orgcd + '-' + org_abbr_chnm2 AS orgcd_name from common..orgcod where org_status = 'A'">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="ds_proj" runat="server" ConnectionString="<%$ ConnectionStrings:pubbs %>"
                SelectCommand="pr_comnwebap_projno" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
            <uc1:userctrl_RegexValidator ID="uc_regex" runat="server" />
        </div>
    </form>
</body>
</html>
