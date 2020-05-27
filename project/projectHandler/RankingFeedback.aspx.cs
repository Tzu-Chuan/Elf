using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class project_projectHandler_RankingFeedback : System.Web.UI.Page
{
    ProjectMGMT_DB db = new ProjectMGMT_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: Update Schedule 狀態
        ///說    明:
        /// * Request["r_guid"]: 字詞 Guid
        /// * Request["r_sche"]: Schedule 狀態
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            string atGuid = (string.IsNullOrEmpty(Request["atGuid"])) ? "" : Request["atGuid"].ToString().Trim();
            int score = (string.IsNullOrEmpty(Request["score"])) ? 0 : Int32.Parse(Request["score"].ToString().Trim());
            string feedback = (string.IsNullOrEmpty(Request["feedback"])) ? "" : Request["feedback"].ToString().Trim();

            string xmlstr = string.Empty;
            db.RankingFeedBack(atGuid, score, feedback);

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>The rating is submitted</Response></root>";
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