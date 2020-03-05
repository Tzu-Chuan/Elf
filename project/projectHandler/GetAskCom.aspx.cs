using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class project_projectHandler_GetAskCom : System.Web.UI.Page
{
    ProjectMGMT_DB MGMT_db = new ProjectMGMT_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢 Ask.com 字詞
        ///說明:
        /// * Request["PjGuid"]: 專案Guid
        /// * Request["topics"]: Topics
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string PjGuid = (string.IsNullOrEmpty(Request["PjGuid"])) ? "" : Request["PjGuid"].ToString().Trim();
            string Topics = (string.IsNullOrEmpty(Request["topics"])) ? "" : Request["topics"].ToString().Trim();
            int Period = (string.IsNullOrEmpty(Request["period"])) ? 0 : int.Parse(Request["period"].ToString().Trim());

            DataTable dt = MGMT_db.GetAskCom(PjGuid, Topics, Period);

            string xmlstr = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXML(dt, "dataList", "data_item");
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