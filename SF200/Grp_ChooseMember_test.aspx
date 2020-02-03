<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeFile="Grp_ChooseMember_test.aspx.cs"
    Inherits="Grp_Grp_ChooseMember_test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>未命名頁面</title>

    <script language="javascript" type="text/javascript">
     function Get_member(memberlist)
    {
        var value = document.getElementById(memberlist).value;
        var url = 'temp_empchoose.aspx?p1=' + memberlist;
                      
        window.open(url);
    }
    </script>

</head>
<body>
    <form id="oForm" runat="server">
        <div>
            <asp:Button ID="btn_choose" runat="server" Text="挑選成員" />
            <asp:TextBox ID="tbx_List" runat="server" Width="505px" Height="101px" TextMode="MultiLine"></asp:TextBox>
            <p>
            </p>
            <asp:GridView ID="gv_body" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField HeaderText="工號" DataField="com_empno" />
                    <asp:BoundField HeaderText="姓名" DataField="com_cname" />
                    <asp:BoundField HeaderText="電話" DataField="com_telex" />
                    <asp:BoundField HeaderText="部門" DataField="com_deptid" />
                    <asp:BoundField HeaderText="EMail" DataField="com_email" />
                </Columns>
            </asp:GridView>
        </div>
        <div style="display: none;">
            <asp:Button ID="btnDetonate" runat="server" Text="觸發POSTBACK" OnClick="btnDetonate_Click"></asp:Button>
            <input id="h_memlist" type="text" runat="server" size="100" />
            <input id="h_mem_num" type="hidden" runat="server" />
            <input id="h_mem_name" type="hidden" runat="server" /></div>
    </form>
</body>
</html>
