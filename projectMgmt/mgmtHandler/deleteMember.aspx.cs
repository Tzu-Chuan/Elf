using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class projectMgmt_mgmtHandler_deleteMember : System.Web.UI.Page
{
    Member m_db = new Member();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 刪除成員
        ///說    明:
        /// * Request["id"]: 成員ID
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
            
            string id = (string.IsNullOrEmpty(Request["id"])) ? "" : Request["id"].ToString().Trim();

            string xmlstr = string.Empty;
            m_db._PM_ID = id;
            m_db.deleteMember();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>Delete Success</Response></root>";
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