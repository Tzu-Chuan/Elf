using System;
using System.Data;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

using FlexCel.Core;
using FlexCel.XlsAdapter;

public partial class projectMgmt_inputExcelSave : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            /*#################################################*/
            /*check處理*/
            /*#################################################*/
            #region /* 權限*/
            if (!RightUtil.Get_BaseRight().角色是系統或專案管理人員)
            {
                //Common.saveSecureLog();
                Response.Write("Error message：do not have read right.");
                Response.End();
            }
            #endregion


            LocalReq req = GetRequest(Request);

            /*#################################################*/
            /*資料處理*/
            /*#################################################*/
            /////*===讀取excel xml session*/
            XmlDocument xmlDoc = null;
            object obj = HttpContext.Current.Session["__Session_InputExcelCheck_xmlDoc"];
            if (obj != null)
            {
                xmlDoc = (XmlDocument)obj;
            }
            else
            {
                Response.Write("Error message, excel data overtime!!");
                Response.End();
            }

            /*===將xml內容依所需存至db*/
            Dao_ProjectMgmt dao = new Dao_ProjectMgmt();
            int resultCount = dao.exec_project_add(xmlDoc, req.optsite);

            /////////////*response*/
            //////////////Response.Write(resultCount);

            /*#################################################*/
            /*參數處理*/
            /*#################################################*/


            /*#################################################*/
            /*輸出處理*/
            /*#################################################*/
            Response.Write("OK.");
        }
        catch (Exception ex)
        {
            //Response.Write("Error message, save excel exception!!");
            Response.Write("Error message, " + ex.Message);
            Response.End();
        }

    }

   


    private LocalReq GetRequest(HttpRequest Request)
    {
        LocalReq req = new LocalReq();

        /*==========param*/
        req.optsite = string.IsNullOrEmpty(Request["optsite"]) ? "" : Request["optsite"].ToString().Trim();
        return req;
    }

    private class LocalReq
    {
        public LocalReq()
            : base()
        {
        }

        public string optsite = "";/*固定來源網站id*/
    }
}
