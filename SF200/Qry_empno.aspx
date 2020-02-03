<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Qry_empno.aspx.cs" Inherits="Sub_Sys_Qry_empno" %>

<%@ Register Src="./UserControl/userctrl_RegexValidator.ascx" TagName="userctrl_RegexValidator"
    TagPrefix="uc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>查詢員工資料</title>
    <base target="_self" />
    <link href="Css/itriap.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
<!--
/* 傳入相關參數說明
// ==============================
// orgcd   = 只能挑選某單位內的人
// keyword = 搜尋關鍵字
// depcd   =          此參數=”Y” 加入com_depcd<> 'Y' 為查詢條件 (查離職員工)
// p1 = com_empno		員工工號
// p2 = com_cname		員工姓名
// p3 = com_telext		電話
// p4 = com_orgcd		單位編號(例：17)
// p5 = com_deptcd		部門編號(例：SF200)
// p6 = com_deptid		完整部門代號(例：17SF200)
// p7 = com_dept_name	部門名稱(例：網際網路應用系統整合二部)
// p8 = com_mailadd     email
// ==============================
*/

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

//-->
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="content-sub">
            <table>
                <tr>
                    <td>
                        關鍵字(以下欄位搜尋)：
                        <asp:TextBox ID="tbx_keyword" runat="server"></asp:TextBox><asp:TextBox ID="tbx_orgcd"
                            runat="server" Visible="False"></asp:TextBox>&nbsp;<asp:Button ID="btn_Query" runat="server"
                                CssClass="button" onmouseout="this.className='btn_mouseout'" onmouseover="this.className='btn_mouseover'"
                                Text="查詢" OnClick="btn_Query_Click" /></td>
                </tr>
                <tr>
                    <td><hr />
                        <asp:datagrid id="dg" runat="server" allowpaging="True" AllowSorting="false"
                            autogeneratecolumns="False" cellspacing="2" gridlines="None" OnPageIndexChanged="dg_PageIndexChanged" OnSortCommand="dg_SortCommand">
								<PagerStyle Mode="NumericPages"></PagerStyle>
								<ItemStyle Height="18px"></ItemStyle>
								<AlternatingItemStyle CssClass="TRowEven" Height="18px"></AlternatingItemStyle>
								<HeaderStyle HorizontalAlign="Center" CssClass="Theader"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="org_abbr_chnm2" HeaderText="單位" SortExpression="com_orgcd">
										<HeaderStyle Width="25%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="員工工號" SortExpression="com_empno">
										<HeaderStyle Width="25%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<ItemTemplate>
											<a href="#" onclick='setValue("<%#DataBinder.Eval(Container.DataItem,"com_empno")%>","<%#Convert.ToString(DataBinder.Eval(Container.DataItem,"com_cname")).Trim()%>","<%#DataBinder.Eval(Container.DataItem,"com_telext")%>","<%#DataBinder.Eval(Container.DataItem,"com_orgcd")%>","<%#DataBinder.Eval(Container.DataItem,"com_deptcd")%>","<%#DataBinder.Eval(Container.DataItem,"com_deptid")%>","<%#DataBinder.Eval(Container.DataItem,"com_dept_name")%>","<%#DataBinder.Eval(Container.DataItem,"com_mailadd")%>")'>
												<%#DataBinder.Eval(Container.DataItem,"com_empno")%>
											</a>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="com_cname" HeaderText="姓名" SortExpression="com_cname">
										<HeaderStyle Width="25%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="com_deptid" HeaderText="單位" SortExpression="com_deptid">
										<HeaderStyle Width="25%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="com_telext" HeaderText="電話" SortExpression="com_telext">
										<HeaderStyle Width="25%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="com_mailadd" HeaderText="Email" SortExpression="com_mailadd">
										<HeaderStyle Width="25%"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
							</asp:datagrid>
                    </td>
                </tr>
            </table>
        </div>
        <uc1:userctrl_RegexValidator ID="uc_regex" runat="server" />
    </form>
</body>
</html>
