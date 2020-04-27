using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class project_ArticleContent : System.Web.UI.Page
{
    ProjectMGMT_DB mgmt_db = new ProjectMGMT_DB();
    public string PjGuid;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(Request["atGuid"]))
        {
            Response.Write("Message：Parameter Error !!");
            Response.End();
        }
        else
        {
            DataTable dt = mgmt_db.GetProjectGuid_By_ArticleGuid(Request["atGuid"].ToString());
            if (dt.Rows.Count > 0)
            {
                PjGuid = dt.Rows[0]["project_guid"].ToString();
            }
        }
    }
}