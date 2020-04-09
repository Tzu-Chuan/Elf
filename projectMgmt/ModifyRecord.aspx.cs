using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class projectMgmt_ModifyRecord : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request["pjGuid"]))
        {
            Response.Redirect("~/projectMgmt/MGMT_List.aspx");
        }
    }
}