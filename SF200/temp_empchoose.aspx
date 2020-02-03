<%@ Page Language="C#" AutoEventWireup="true" CodeFile="temp_empchoose.aspx.cs" Inherits="temp_empchoose" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>未命名頁面</title>
     <base target="_self" />
    <meta http-equiv="refresh" content="0" />
</head>
<body>
    <form id="form1" action="Qry_member.aspx?D4orgcd=<%=Request["D4orgcd"] %>&changeorgcd=<%=Request["changeorgcd"] %>" method="post">
        <div style="display: none;">
            <textarea id="empno" cols="1" rows="1" runat="server"></textarea>
            <textarea id="empname" cols="1" rows="1" runat="server"></textarea>
        </div>
    </form>
</body>

<script language="javascript" type="text/javascript">
       
	    var topWin = null;
	    if(window.opener)	
		    topWin = window.opener;
	    else if (window.dialogArguments)
		    topWin = window.dialogArguments;
     
        document.getElementById('empno').value=topWin.document.getElementById('h_mem_num').value;
        document.getElementById('empname').value=topWin.document.getElementById('h_mem_name').value;
        document.getElementById('form1').submit();
        
    	 function setValue(v1)
        {
            returnValue=v1;
	        window.self.close();
	        return false;
        }
</script>

</html>
