using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class project_projectHandler_GetArticleDetail : System.Web.UI.Page
{
    ProjectMGMT_DB MGMT_db = new ProjectMGMT_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢文章內容
        ///說明:
        /// * Request["atGuid"]: 文章 Guid
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string atGuid = (string.IsNullOrEmpty(Request["atGuid"])) ? "" : Request["atGuid"].ToString().Trim();

            DataSet ds = MGMT_db.GetArticleDetail(atGuid);

            string xmlstr = string.Empty;
            string xmlstr2 = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXML(ds.Tables[0], "dataList", "data_item");
            xmlstr2 = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "WordList", "word_item");
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