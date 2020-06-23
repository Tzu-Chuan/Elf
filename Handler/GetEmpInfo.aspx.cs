using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class Handler_GetEmpInfo : System.Web.UI.Page
{
    ProjectMaintain_DB pm_db = new ProjectMaintain_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢員工資訊
        ///說明:
        /// * Request["empno"]: 工號
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string empno = (string.IsNullOrEmpty(Request["empno"])) ? "" : Request["empno"].ToString().Trim();

            string xmlstr = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXML(pm_db.GetEmpInfo(empno), "dataList", "data_item");
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