using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class projectMgmt_mgmtHandler_GetRecordList : System.Web.UI.Page
{
    ProjectMGMT_DB MGMT_db = new ProjectMGMT_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢辭庫維護列表
        ///說明:
        /// * Request["PageNo"]: 所在頁面
        /// * Request["PageSize"]: 一頁幾筆資料
        /// * Request["PjGuid"]: 專案Guid
        /// * Request["keyword"]: 關鍵字
        /// * Request["Action"]: 更新動作
        /// * Request["StartDate"]: 起始日期
        /// * Request["EndDate"]: 結束日期
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string PageNo = (string.IsNullOrEmpty(Request["PageNo"])) ? "0" : Request["PageNo"].ToString().Trim();
            int PageSize = (string.IsNullOrEmpty(Request["PageSize"])) ? 20 : int.Parse(Request["PageSize"].ToString().Trim());

            string PjGuid = (string.IsNullOrEmpty(Request["PjGuid"])) ? "" : Request["PjGuid"].ToString().Trim();
            string keyword = (string.IsNullOrEmpty(Request["keyword"])) ? "" : Request["keyword"].ToString().Trim();
            string Action = (string.IsNullOrEmpty(Request["Action"])) ? "" : Request["Action"].ToString().Trim();
            string StartDate = (string.IsNullOrEmpty(Request["StartDate"])) ? "" : Request["StartDate"].ToString().Trim();
            string EndDate = (string.IsNullOrEmpty(Request["EndDate"])) ? "" : Request["EndDate"].ToString().Trim();

            //計算起始與結束
            int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
            int pageStart = pageEnd - PageSize + 1;

            MGMT_db._KeyWord = keyword;
            DataSet ds = MGMT_db.GetRecordList(PjGuid, Action, StartDate, EndDate, pageStart.ToString(), pageEnd.ToString());

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