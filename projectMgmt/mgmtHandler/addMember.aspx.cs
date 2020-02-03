using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class projectMgmt_mgmtHandler_addMember : System.Web.UI.Page
{
    Member m_db = new Member();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增成員
        ///說    明:
        /// * Request["pjid"]: 專案Guid
        /// * Request["empno"]: 工號
        /// * Request["name"]: 姓名
        /// * Request["deptid"]: 部門代碼
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            #region 權限判斷
            if (!RightUtil.Get_BaseRight().角色是系統或專案管理人員)
            {
                xDoc = ExceptionUtil.GetErrorMassageDocument("do not have read right.");
                Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
                xDoc.Save(Response.Output);
                return;
            }
            #endregion

            string pjid = (string.IsNullOrEmpty(Request["pjid"])) ? "" : Request["pjid"].ToString().Trim();
            string empno = (string.IsNullOrEmpty(Request["empno"])) ? "" : Request["empno"].ToString().Trim();
            string name = (string.IsNullOrEmpty(Request["name"])) ? "" : Request["name"].ToString().Trim();
            string deptid = (string.IsNullOrEmpty(Request["deptid"])) ? "" : Request["deptid"].ToString().Trim();
            

            string xmlstr = string.Empty;
            m_db._PM_Guid = Guid.NewGuid().ToString("N");
            m_db._PM_ProjectGuid = pjid;
            m_db._PM_Empno = empno;
            m_db._PM_Name = name;
            m_db._PM_Deptid = deptid;
            m_db._PM_CreateEmpno = SSOUtil.GetCurrentUser().工號;
            m_db._PM_CreateName = SSOUtil.GetCurrentUser().姓名;

            m_db._PM_ProjectGuid = pjid;
            DataTable mdt = m_db.getMemberByEmpno();
            if (mdt.Rows.Count > 0)
            {
                xDoc = ExceptionUtil.GetErrorMassageDocument("is already a list member.");
                Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
                xDoc.Save(Response.Output);
                return;
            }
            else
                m_db.addMember();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>Save Success</Response></root>";
            xDoc.LoadXml(xmlstr);
        }
        catch (Exception ex)
        {
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }
}