using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;

public partial class projectMgmt_mgmtHandler_deleteWord : System.Web.UI.Page
{
    ProjectMGMT_DB mgmt_db = new ProjectMGMT_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 刪除字詞
        ///說    明:
        /// * Request["pjGuid"]: Project Guid
        /// * Request["WordGuid"]: Word Guid
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        SqlConnection oConn = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        oConn.Open();
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = oConn;
        SqlTransaction myTrans = oConn.BeginTransaction();
        oCmd.Transaction = myTrans;
        try
        {
            string pjGuid = (string.IsNullOrEmpty(Request["pjGuid"])) ? "" : Request["pjGuid"].ToString().Trim();
            string WordGuid = (string.IsNullOrEmpty(Request["WordGuid"])) ? "" : Request["WordGuid"].ToString().Trim();

            string xmlstr = string.Empty;
            mgmt_db.InsertWordLog(oConn, myTrans, pjGuid, WordGuid, "delete");
            mgmt_db.deleteWord(oConn, myTrans, WordGuid);

            myTrans.Commit();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>Success</Response></root>";
            xDoc.LoadXml(xmlstr);
        }
        catch (Exception ex)
        {
            myTrans.Rollback();
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }

        oCmd.Connection.Close();
        oConn.Close();

        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }
}