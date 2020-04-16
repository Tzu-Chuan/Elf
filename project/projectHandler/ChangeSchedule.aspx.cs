using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class project_projectHandler_ChangeSchedule : System.Web.UI.Page
{
    Dao_ProjectMgmt db = new Dao_ProjectMgmt();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: Update Schedule 狀態
        ///說    明:
        /// * Request["r_guid"]: 字詞 Guid
        /// * Request["r_sche"]: Schedule 狀態
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string r_guid = (string.IsNullOrEmpty(Request["r_guid"])) ? "" : Request["r_guid"].ToString().Trim();
            string r_sche = (string.IsNullOrEmpty(Request["r_sche"])) ? "" : Request["r_sche"].ToString().Trim();

            string xmlstr = string.Empty;
            db.exec_relatedWord_schedule_update(r_guid, r_sche);

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