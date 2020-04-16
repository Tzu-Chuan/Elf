using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class projectMgmt_mgmtHandler_GetWordList : System.Web.UI.Page
{
    ProjectMGMT_DB MGMT_db = new ProjectMGMT_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢辭庫維護列表
        ///說明:
        /// * Request["PageNo"]: 所在頁面
        /// * Request["PageSize"]: 一頁幾筆資料
        /// * Request["SortName"]: 排序欄位
        /// * Request["SortMethod"]: 排序方式
        /// * Request["PjGuid"]: 專案Guid
        /// * Request["keyword"]: 關鍵字
        /// * Request["Topic"]: input_research_direction.name
        /// * Request["Blacklist"]: input_related_word.blacklist
        /// * Request["Source"]: input_related_word.analyst_give
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string PageNo = (string.IsNullOrEmpty(Request["PageNo"])) ? "0" : Request["PageNo"].ToString().Trim();
            int PageSize = (string.IsNullOrEmpty(Request["PageSize"])) ? 20 : int.Parse(Request["PageSize"].ToString().Trim());
            string SortName = (string.IsNullOrEmpty(Request["SortName"])) ? "" : Request["SortName"].ToString().Trim();
            string SortMethod = (string.IsNullOrEmpty(Request["SortMethod"])) ? "-" : Request["SortMethod"].ToString().Trim();
            SortMethod = (SortMethod == "+") ? "asc" : "desc";
            string SortCommand = SortName + " " + SortMethod;

            string PjGuid = (string.IsNullOrEmpty(Request["PjGuid"])) ? "" : Request["PjGuid"].ToString().Trim();
            string keyword = (string.IsNullOrEmpty(Request["keyword"])) ? "" : Request["keyword"].ToString().Trim();
            string Topic = (string.IsNullOrEmpty(Request["Topic"])) ? "all" : Request["Topic"].ToString().Trim();
            string Blacklist = (string.IsNullOrEmpty(Request["Blacklist"])) ? "all" : Request["Blacklist"].ToString().Trim();
            string Source = (string.IsNullOrEmpty(Request["Source"])) ? "all" : Request["Source"].ToString().Trim();

            //計算起始與結束
            int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
            int pageStart = pageEnd - PageSize + 1;

            MGMT_db._KeyWord = keyword;
            DataSet ds = MGMT_db.GetWordList(PjGuid, Topic, Blacklist, Source, pageStart.ToString(), pageEnd.ToString(), SortCommand);

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