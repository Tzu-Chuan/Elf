using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class project_projectHandler_GetWebsite : System.Web.UI.Page
{
    ProjectMGMT_DB MGMT_db = new ProjectMGMT_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢爬蟲網站
        ///說明:
        /// * Request["PageNo"]: 所在頁面
        /// * Request["PageSize"]: 一頁幾筆資料
        /// * Request["keyword"]: 關鍵字
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string PjGuid = (string.IsNullOrEmpty(Request["PjGuid"])) ? "" : Request["PjGuid"].ToString().Trim();
            string Topics = (string.IsNullOrEmpty(Request["topics"])) ? "" : Request["topics"].ToString().Trim();
            int Period = (string.IsNullOrEmpty(Request["period"])) ? 0 : int.Parse(Request["period"].ToString().Trim());
            string MyTag = (string.IsNullOrEmpty(Request["mytag"])) ? "" : Request["mytag"].ToString().Trim();

            DataTable dt = MGMT_db.GetWebsite(PjGuid, Topics, Period, MyTag);

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