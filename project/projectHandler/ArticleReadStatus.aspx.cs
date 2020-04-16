using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class project_projectHandler_ArticleReadStatus : System.Web.UI.Page
{
    ReadArticle_DB r_db = new ReadArticle_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 已讀文章功能處理
        ///說    明:
        /// * Request["area"]: 文章所屬區塊
        /// * Request["gid"]: 文章 Guid
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string area = (string.IsNullOrEmpty(Request["area"])) ? "" : Request["area"].ToString().Trim();
            string gid = (string.IsNullOrEmpty(Request["gid"])) ? "" : Request["gid"].ToString().Trim();

            r_db._R_ArticleGuid = gid;
            r_db._R_Empno = SSOUtil.GetCurrentUser().工號;
            DataTable dt = r_db.getReadArticle();

            if (area == "article" && dt.Rows.Count == 0)
            {
                r_db.addReadArticle();
            }
            else
            {

            }

            string xmlstr = string.Empty;
            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response></Response></root>";
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