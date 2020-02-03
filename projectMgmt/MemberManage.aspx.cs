using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class projectMgmt_MemberManage : System.Web.UI.Page
{
    Member m_db = new Member();
    public string PjName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(Request.QueryString["pj"]))
            {
                dt = m_db.getProjectInfo(Request.QueryString["pj"].ToString());
                if (dt.Rows.Count > 0)
                    PjName = dt.Rows[0]["project_name"].ToString();
            }
            else
            {
                Response.Redirect("~/projectMgmt/default.aspx");
            }
        }
    }
}