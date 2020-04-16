using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class Handler_GetResources : System.Web.UI.Page
{
    Dao_Project db = new Dao_Project();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢 Resources 類別
        ///說明:
        /// * Request["ProjectGuid"]: 專案 Guid
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string ProjectGuid = (string.IsNullOrEmpty(Request["ProjectGuid"])) ? "" : Request["ProjectGuid"].ToString().Trim();
            
            DataTable dt = db.getProjectTopic(ProjectGuid);
            DataTable dt2 = db.getMyTag(ProjectGuid, SSOUtil.GetCurrentUser().工號);

            string xmlstr = string.Empty;
            string xmlstr2 = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "topic_item");
            xmlstr2 = DataTableToXml.ConvertDatatableToXML(dt2, "dataList", "mytag_item");
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root>" + xmlstr + xmlstr2 + "</root>";
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