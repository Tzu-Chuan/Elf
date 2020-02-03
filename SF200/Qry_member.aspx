<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Qry_member.aspx.cs" Inherits="Grp_Qry_member"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>挑選成員</title>
    <base target="_self" />
    <link rel="stylesheet" href="css/group.css" type="text/css" />
    <link rel="stylesheet" href="css/RD.css" type="text/css" />

    <script language="javascript" type="text/javascript">
    	function setValue(v1)
        {
//            window.opener.document.getElementById('h_memlist').value=v1;
//            window.opener.document.getElementById('btnDetonate').click() ;
//	        window.self.close();
//	        return false;
	        
	        var topWin = null;
	        if (window.opener)
	            topWin = window.opener;
	        else if (window.dialogArguments)
	            topWin = window.dialogArguments;

	        topWin.document.getElementById('h_memlist').value = v1;
	        topWin.document.getElementById('submitBtn').value = "1";   //whchang btnDetonate  //確認USER的操作是關閉視窗OR清空後按下submit
	        topWin.document.getElementById('btnDetonate').click();
	        self.close();
	        return false;	        
	        
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <p>
        </p>
        <p>
        </p>
        <div>
            &nbsp;&nbsp;&nbsp;挑選成員名單：
            <asp:LinkButton ID="lik_body" runat="server" OnClick="lik_body_Click">姓名</asp:LinkButton>、
            <asp:LinkButton ID="lik_dept" runat="server" OnClick="lik_dept_Click">部門</asp:LinkButton>、
            <asp:LinkButton ID="lik_comanager" runat="server" OnClick="lik_comanager_Click">我的聯絡人</asp:LinkButton>、
            <asp:LinkButton ID="lik_addrbook" runat="server" OnClick="lik_addrbook_Click">群組通訊錄</asp:LinkButton>
            <table class="horz">
                <tr>
                    <td style="width: 500px; vertical-align: top;">
                        <asp:Panel ID="pan_body" runat="server">
                            <table>
                                <tr>
                                    <td style="width: 600px" align="right">
                                        <asp:DropDownList ID="ddl_orgcdlist" runat="server">
                                        </asp:DropDownList>
                                        搜尋關鍵字：<asp:TextBox ID="tbx_keyword" runat="server"></asp:TextBox>
                                        <asp:Button ID="btn_body_serch" runat="server" CssClass="button-purple" OnClick="btn_body_serch_Click"
                                            Text="查詢" /></td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:GridView ID="gv_body" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            OnPageIndexChanged="gv_body_PageIndexChanged" OnPageIndexChanging="gv_body_PageIndexChanging"
                                            Style="vertical-align: top" Width="595px" meta:resourcekey="gv_bodyResource1"
                                            AllowSorting="True" OnRowCreated="gv_body_RowCreated" DataSourceID="ds_memberlist"
                                            CssClass="horz" EmptyDataText="無資料">
                                            <Columns>
                                                <asp:TemplateField HeaderText="姓名" SortExpression="com_cname">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ckb_body" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "com_cname") %>'
                                                            CssClass="checknone" />
                                                        <asp:HiddenField ID="hf_com_empno" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "com_empno") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="工號" SortExpression="com_empno">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("com_empno") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("com_empno") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="70px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="部門" SortExpression="dep_deptname">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("dep_deptname") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("dep_deptname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="200px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="com_telext" HeaderText="分機">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="com_orgname" HeaderText="單位" ItemStyle-HorizontalAlign="center">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="ds_memberlist" runat="server" ConnectionString="<%$ ConnectionStrings:GrbWebConn%>"
                                            SelectCommand="prGrp_choose_member_from_comper_orgcd" SelectCommandType="StoredProcedure"
                                            OnSelecting="ds_memberlist_Selecting"></asp:SqlDataSource>
                                        <asp:ListBox ID="lbxTempItem" runat="server" Height="1px" Visible="False" Width="1px"
                                            meta:resourcekey="lbxTempItemResource1"></asp:ListBox>
                                        <asp:Label ID="lbl_Message" runat="server" meta:resourcekey="lbl_MessageResource1"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pan_dept" runat="server" Visible="false" Width="600px" meta:resourcekey="pan_deptResource1">
                            <table>
                                <tr>
                                    <td colspan="4">
                                        請挑選想加入群組成員</td>
                                </tr>
                                <tr>
                                    <td style="width: 253px">
                                        <asp:ListBox ID="lisb_org" runat="server" AutoPostBack="True" Height="300px" OnSelectedIndexChanged="lisb_org_SelectedIndexChanged"
                                            Width="245px" meta:resourcekey="lisb_orgResource1"></asp:ListBox></td>
                                    <td>
                                        <asp:ListBox ID="lisb_dept" runat="server" AutoPostBack="True" Height="300px" OnSelectedIndexChanged="lisb_dept_SelectedIndexChanged"
                                            Width="245px" meta:resourcekey="lisb_deptResource1"></asp:ListBox>
                                    </td>
                                    <td style="width: 34px">
                                        <asp:ListBox ID="lisb_cname" runat="server" Height="300px" Width="250px" SelectionMode="Multiple"
                                            OnDataBound="lisb_cname_DataBound" meta:resourcekey="lisb_cnameResource1"></asp:ListBox></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pan_cont" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td style="width: 600px" align="right">
                                        搜尋關鍵字：<asp:TextBox ID="tbx_keyword_cont" runat="server" meta:resourcekey="tbx_keywordResource1"></asp:TextBox>
                                        <asp:Button ID="btn_cont_search" runat="server" CssClass="button-purple" Text="查詢"
                                            meta:resourcekey="btn_cont_searchResource1" OnClick="btn_cont_search_Click" /></td>
                                </tr>
                                <tr>
                                    <td style="height: 289px;" valign="top">
                                        <asp:GridView ID="gv_contact" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                             EmptyDataText="無資料" Style="vertical-align: top" Width="595px" meta:resourcekey="gv_contactResource1"
                                            AllowSorting="True" DataSourceID="ds_contact" OnPageIndexChanged="gv_contact_PageIndexChanged"
                                            OnPageIndexChanging="gv_contact_PageIndexChanging" OnRowCreated="gv_contact_RowCreated"
                                            CssClass="horz">
                                            <Columns>
                                                <asp:TemplateField HeaderText="姓名" meta:resourcekey="TemplateFieldResource1" SortExpression="com_cname">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ckb_cont" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "com_cname") %>'
                                                            meta:resourcekey="ckb_bodyResource1" CssClass="checknone" />
                                                        <asp:HiddenField ID="hf_cont_empno" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "com_empno") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="工號" meta:resourcekey="TemplateFieldResource2" SortExpression="com_empno">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("com_empno") %>' meta:resourcekey="TextBox1Resource1"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("com_empno") %>' meta:resourcekey="Label1Resource1"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="70px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="部門" meta:resourcekey="TemplateFieldResource3" SortExpression="dep_deptname">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("dep_deptname") %>' meta:resourcekey="TextBox2Resource1"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("dep_deptname") %>' meta:resourcekey="Label2Resource1"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="280px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="com_telext" HeaderText="分機" meta:resourcekey="BoundFieldResource1">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="ds_contact" runat="server" ConnectionString="<%$ ConnectionStrings:GrbWebConn%>"
                                            SelectCommand="prGrp_choose_member_from_contacts " SelectCommandType="storedprocedure"
                                            OnSelecting="ds_contact_Selecting"></asp:SqlDataSource>
                                        <asp:ListBox ID="lbxTempItem_cont" runat="server" Height="1px" Visible="False" Width="1px"
                                            meta:resourcekey="lbxTempItemResource1"></asp:ListBox>
                                        <asp:Label ID="lbl_Message_cont" runat="server" meta:resourcekey="lbl_MessageResource1"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pan_Addrbook" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td style="width: 600px" align="right">
                                        搜尋關鍵字：<asp:TextBox ID="tbx_keyword_addrbook" runat="server" meta:resourcekey="tbx_keywordResource1"></asp:TextBox>
                                        <asp:Button ID="btn_addrbook_search" runat="server" CssClass="button-purple" Text="查詢"
                                            meta:resourcekey="btn_addrbook_searchResource1" OnClick="btn_addrbook_search_Click" /></td>
                                </tr>
                                <tr>
                                    <td style="height: 289px;" valign="top">
                                        <asp:GridView ID="gv_addrbook" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                             EmptyDataText="無資料" Style="vertical-align: top" Width="595px" meta:resourcekey="gv_contactResource1"
                                            AllowSorting="True" DataSourceID="ds_addrbook" OnPageIndexChanged="gv_addrbook_PageIndexChanged"
                                            OnPageIndexChanging="gv_addrbook_PageIndexChanging" OnRowCreated="gv_addrbook_RowCreated"
                                            CssClass="horz">
                                            <Columns>
                                                <asp:TemplateField HeaderText="姓名" meta:resourcekey="TemplateFieldResource1" SortExpression="ab_name">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ckb_addrbook" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ab_name") %>'
                                                            meta:resourcekey="ckb_bodyResource1" CssClass="checknone" />
                                                        <asp:HiddenField ID="hf_addrbook_empno" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ab_empno") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="工號" meta:resourcekey="TemplateFieldResource2" SortExpression="ab_empno">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ab_empno") %>' meta:resourcekey="TextBox1Resource1"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("ab_empno") %>' meta:resourcekey="Label1Resource1"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="70px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="所屬群組名稱" meta:resourcekey="TemplateFieldResource3"
                                                    SortExpression="ab_grpname">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("ab_grpname") %>' meta:resourcekey="TextBox2Resource1"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("ab_grpname") %>' meta:resourcekey="Label2Resource1"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="280px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ab_tel" HeaderText="分機" meta:resourcekey="BoundFieldResource1">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="ds_addrbook" runat="server" ConnectionString="<%$ ConnectionStrings:GrbWebConn%>"
                                            SelectCommand="prGrp_choose_member_from_addrbook" SelectCommandType="storedprocedure"
                                            OnSelecting="ds_addrbook_Selecting"></asp:SqlDataSource>
                                        <asp:ListBox ID="lbxTempItem_addrbook" runat="server" Height="1px" Visible="False"
                                            Width="1px" meta:resourcekey="lbxTempItemResource1"></asp:ListBox>
                                        <asp:Label ID="lbl_Message_addrbook" runat="server" meta:resourcekey="lbl_MessageResource1"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Label ID="lbl_grpid" runat="server" Visible="False" meta:resourcekey="lbl_grpidResource1"></asp:Label>
                        <asp:Label ID="lbl_mgrid" runat="server" Visible="False" meta:resourcekey="lbl_mgridResource1"></asp:Label>
                        <asp:Label ID="lbl_grpstatus" runat="server" Visible="False" meta:resourcekey="lbl_grpstatusResource1"></asp:Label>
                        <asp:Label ID="lbl_calendarowner" runat="server" Visible="False" meta:resourcekey="lbl_calendarownerResource1"></asp:Label></td>
                    <td style="width: 50px; height: 176px; vertical-align: middle;" align="center">
                        <asp:Panel ID="pan_btnBody" runat="server">
                            <asp:Button ID="btn_body_add" runat="server" CssClass="button-purple" OnClick="btn_body_add_Click"
                                Text="Add >>" Width="80px" />
                            <br />
                            <br />
                            <asp:Button ID="btn_body_del" runat="server" CssClass="button-purple" OnClick="btn_body_del_Click"
                                Text="<< Remove" Width="80px" />
                        </asp:Panel>
                        <asp:Panel ID="pan_btnDep" runat="server" Visible="false">
                            <asp:Button ID="btn_dept_add" runat="server" CssClass="button-purple" OnClick="btn_dept_add_Click"
                                Text="Add >>" Visible="False" Width="80px" />
                            <br />
                            <br />
                            <asp:Button ID="btn_dept_del" runat="server" CssClass="button-purple" OnClick="btn_dept_del_Click"
                                Text="<< Remove" Visible="False" Width="80px" />
                        </asp:Panel>
                        <asp:Panel ID="pan_btnCont" runat="server" Visible="false">
                            <asp:Button ID="btn_cont_add" runat="server" CssClass="button-purple" Text="Add >>"
                                Visible="False" OnClick="btn_cont_add_Click" Width="80px" />
                            <br />
                            <br />
                            <asp:Button ID="btn_cont_del" runat="server" CssClass="button-purple" Text="<< Remove"
                                Visible="False" OnClick="btn_cont_del_Click" Width="80px" />
                        </asp:Panel>
                        <asp:Panel ID="pan_btnaddrbook" runat="server" Visible="false">
                            <asp:Button ID="btn_addrbook_add" runat="server" CssClass="button-purple" Text="Add >>"
                                Visible="False" Width="80px" OnClick="btn_addrbook_add_Click" />
                            <br />
                            <br />
                            <asp:Button ID="btn_addrbook_del" runat="server" CssClass="button-purple" Text="<< Remove"
                                Visible="False" OnClick="btn_addrbook_del_Click" Width="80px" />
                        </asp:Panel>
                    </td>
                    <td style="vertical-align: top; height: 176px; width: 130px;">
                        <table>
                            <tr>
                                <td style="width: 124px">
                                    成員清單</td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; height: 150px; width: 124px;">
                                    <asp:ListBox ID="lisb_body" runat="server" Height="300px" Width="220px" SelectionMode="Multiple"
                                        meta:resourcekey="lisb_bodyResource1"></asp:ListBox>
                                    <asp:ListBox ID="lisb_bodydel" runat="server" Width="100px" Visible="False" meta:resourcekey="lisb_bodydelResource1">
                                    </asp:ListBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="right">
                        <asp:Button ID="btn_body_finish" runat="server" CssClass="button-purple" OnClick="btn_body_finish_Click"
                            Text="Submit" meta:resourcekey="btn_body_finishResource1" /></td>
                </tr>
            </table>
            <input id="h_keyword" type="hidden" runat="server" />
            <input id="h_memlist" type="hidden" runat="server" />
        </div>
    </form>
</body>
</html>
