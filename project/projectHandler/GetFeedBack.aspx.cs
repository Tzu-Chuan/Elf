using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class project_projectHandler_GetFeedBack : System.Web.UI.Page
{
    ProjectMGMT_DB MGMT_db = new ProjectMGMT_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢 FeedBack 資訊
        ///說明:
        /// * Request["atGuid"]: 文章Guid
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string atGuid = (string.IsNullOrEmpty(Request["atGuid"])) ? "" : Request["atGuid"].ToString().Trim();

            DataTable dt = MGMT_db.GetArticleFeedback(atGuid);

            string xmlstr = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXmlByAttribute(dt, "dataList", "data_item");
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + "</root>";
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