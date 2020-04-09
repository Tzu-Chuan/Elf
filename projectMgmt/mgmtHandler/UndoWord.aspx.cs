using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class projectMgmt_mgmtHandler_UndoWord : System.Web.UI.Page
{
    ProjectMGMT_DB mgmt_db = new ProjectMGMT_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 刪除字詞
        ///說    明:
        /// * Request["id"]: undo id
        /// * Request["WordGuid"]: Word Guid
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string id = (string.IsNullOrEmpty(Request["id"])) ? "" : Request["id"].ToString().Trim();

            string xmlstr = string.Empty;
            mgmt_db.UndoWord(id);
            
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>Success</Response></root>";
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