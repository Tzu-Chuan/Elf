using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class project_projectHandler_GetAskComArticleList : System.Web.UI.Page
{
    ProjectMGMT_DB MGMT_db = new ProjectMGMT_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢Ask.com文章列表
        ///說明:
        /// * Request["PageNo"]: 所在頁面
        /// * Request["PageSize"]: 一頁幾筆資料
        /// * Request["Related_guid"]: Word Guid
        /// * Request["Period"]: Period
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string PageNo = (string.IsNullOrEmpty(Request["PageNo"])) ? "0" : Request["PageNo"].ToString().Trim();
            int PageSize = (string.IsNullOrEmpty(Request["PageSize"])) ? 20 : int.Parse(Request["PageSize"].ToString().Trim());
            string Related_guid = (string.IsNullOrEmpty(Request["Related_guid"])) ? "" : Request["Related_guid"].ToString().Trim();
            int Period = (string.IsNullOrEmpty(Request["period"])) ? 0 : int.Parse(Request["period"].ToString().Trim());

            //計算起始與結束
            int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
            int pageStart = pageEnd - PageSize + 1;

            DataSet ds = MGMT_db.GetAskComArticles(Related_guid, Period, pageStart.ToString(), pageEnd.ToString());

            string xmlstr = string.Empty;
            string xmlstr2 = string.Empty;
            xmlstr = "<total>" + ds.Tables[0].Rows[0]["total"].ToString() + "</total>";
            xmlstr2 = DataTableToXml.ConvertDatatableToXML(ds.Tables[1], "dataList", "data_item");
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