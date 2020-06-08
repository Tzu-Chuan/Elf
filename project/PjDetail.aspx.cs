using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class project_PjDetail : System.Web.UI.Page
{
    Member m_db = new Member();
    ProjectMGMT_DB mgmt_db = new ProjectMGMT_DB();
    public string ProjectName, Technology, GlobalStatus;
    protected void Page_Load(object sender, EventArgs e)
    {
        string pjGuid = (string.IsNullOrEmpty(Request["pjGuid"])) ? "" : Request["pjGuid"].ToString().Trim();

        GlobalStatus = "N";
        #region 瀏覽權限 (是否為專案成員)
        if (!RightUtil.Get_BaseRight().角色是系統或專案管理人員)
        {
            m_db._PM_ProjectGuid = pjGuid;
            m_db._PM_Empno = SSOUtil.GetCurrentUser().工號;
            DataTable dt = m_db.getMemberByEmpno();
            if (dt.Rows.Count == 0)
            {
                // 2020/6/4 暫時修改全院可看的權限 -by Nick
                if (pjGuid != "2b4b7012-503d-4888-a42c-696eea00e66c")
                {
                    Response.Write("Error message：do not have read right.");
                    Response.End();
                }
                else
                    GlobalStatus = "Y";

            }
            #endregion
        }

        #region 參數錯誤
        if (pjGuid == "")
        {
            Response.Write("message：parameter error!!");
            Response.End();
        }
        #endregion

        DataTable prodt = mgmt_db.getProjectInfo(pjGuid);
        if (prodt.Rows.Count > 0)
        {
            ProjectName = prodt.Rows[0]["project_name"].ToString();
            Technology = prodt.Rows[0]["technology"].ToString();
        }
    }
}