using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class projectMgmt_mgmtHandler_GetManagerRight : System.Web.UI.Page
{
    ProjectMGMT_DB db = new ProjectMGMT_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 查詢管理者權限
        ///說明:
        /// * Request["empno"]: 工號
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            //string empno = (string.IsNullOrEmpty(Request["empno"])) ? "" : Request["empno"].ToString().Trim();
            string empno = SSOUtil.GetCurrentUser().工號;

            DataTable dt = db.GetManagerList(empno);

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