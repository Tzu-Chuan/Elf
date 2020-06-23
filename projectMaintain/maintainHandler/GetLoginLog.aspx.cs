using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class projectMaintain_maintainHandler_GetLoginLog : System.Web.UI.Page
{
    ProjectMaintain_DB pm_db = new ProjectMaintain_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢登入資訊
        ///說明:
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string PageNo = (string.IsNullOrEmpty(Request["PageNo"])) ? "0" : Request["PageNo"].ToString().Trim();
            int PageSize = (string.IsNullOrEmpty(Request["PageSize"])) ? 20 : int.Parse(Request["PageSize"].ToString().Trim());
            string keyword = (string.IsNullOrEmpty(Request["keyword"])) ? "" : Request["keyword"].ToString().Trim();

            //計算起始與結束
            int pageEnd = (int.Parse(PageNo) + 1) * PageSize;
            int pageStart = pageEnd - PageSize + 1;

            pm_db._KeyWord = keyword;
            DataSet ds = pm_db.GetLoginLog(pageStart.ToString(), pageEnd.ToString());

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