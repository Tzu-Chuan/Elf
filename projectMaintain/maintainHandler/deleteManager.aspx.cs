using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class projectMaintain_maintainHandler_deleteManager : System.Web.UI.Page
{
    ProjectMaintain_DB pm_db = new ProjectMaintain_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 刪除人員
        ///說    明:
        /// * Request["gid"]: 人員 GUID
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            #region 權限判斷
            if (!RightUtil.Get_BaseRight().角色是系統管理人員)
            {
                xDoc = ExceptionUtil.GetErrorMassageDocument("do not have read right.");
                Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
                xDoc.Save(Response.Output);
                return;
            }
            #endregion

            string gid = (string.IsNullOrEmpty(Request["gid"])) ? "" : Request["gid"].ToString().Trim();

            string xmlstr = string.Empty;
            pm_db._manager_guid = gid;
            pm_db.deleteManager();

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