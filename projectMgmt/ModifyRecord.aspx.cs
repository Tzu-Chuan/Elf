using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class projectMgmt_ModifyRecord : System.Web.UI.Page
{
    ProjectMGMT_DB mgmt_db = new ProjectMGMT_DB();
    public string ProjectName, Technology;
    protected void Page_Load(object sender, EventArgs e)
    {
        string pjGuid = (string.IsNullOrEmpty(Request["pjGuid"])) ? "" : Request["pjGuid"].ToString().Trim();

        if (pjGuid != "")
        {
            DataTable prodt = mgmt_db.getProjectInfo(pjGuid);
            if (prodt.Rows.Count > 0)
            {
                ProjectName = prodt.Rows[0]["project_name"].ToString();
                Technology = prodt.Rows[0]["technology"].ToString();
            }
        }
        else
        {
            Response.Redirect("~/projectMgmt/default.aspx");
        }
    }
}