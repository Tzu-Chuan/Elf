using System;
using System.Data;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

public partial class project_projectDetail : System.Web.UI.Page
{
    Member m_db = new Member();
    protected void Page_Load(object sender, EventArgs e)
    {
        LocalReq req = GetRequest(Request);
        showPage(req);
    }


    private void showPage(LocalReq req)
    {
        /*#################################################*/
        /*check處理*/
        /*#################################################*/

        #region /* 瀏覽權限*/
        if (!RightUtil.Get_BaseRight().角色是系統或專案管理人員)
        {
            m_db._PM_ProjectGuid = req.pjGuid;
            m_db._PM_Empno = SSOUtil.GetCurrentUser().工號;
            DataTable dt = m_db.getMemberByEmpno();
            if (dt.Rows.Count == 0)
            {
                Response.Write("Error message：do not have read right.");
                Response.End();
            }
        }
        #endregion

        if (req.pjGuid == "")
        {
            Response.Write("message：parameter error!!");
            Response.End();
        }

        /*#################################################*/
        /*log處理*/
        /*#################################################*/


        /*#################################################*/
        /*資料處理*/
        /*#################################################*/
        /*get data*/
        Dao_Project dao = new Dao_Project();

        /*專案內容*/
        XmlDocument xmlProject = dao.getProject(req.pjGuid, req.date0, req.researchGuid, req.myTag);
        ////xmlProject.Save(Response.Output);
        ////return;

        /*監控網站*/
        XmlDocument xmlWebsiteList = dao.getWebsite(req.pjGuid, req.date0, req.researchGuid, req.myTag);
        ////xmlWebsiteList.Save(Response.Output);
        ////return;

        /*研究方向*/
        XmlDocument xmlDirectionList = dao.getDirection(req.pjGuid);
        ////xmlDirectionList.Save(Response.Output);
        ////return;

        /*關連詞*/
        XmlDocument xmlAskComList = dao.getAskCom(req.pjGuid, req.date0);
        ////xmlAskComList.Save(Response.Output);
        ////return;


        /*myTag*/
        XmlDocument xmlMyTagList = dao.getMyTagMaintain(req.empno);


        /*==========set output xDoc*/
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml("<root><rec></rec></root>");
        //xDoc.LoadXml(ds.GetXml());

        /*#################################################*/
        /*參數處理*/
        /*#################################################*/

        /*==========common args*/
        XsltArgumentList xArgs = XmlUtil.GetXsltArguments();
        //base.AddPageTemplateXsltParam(xArgs);

        /*==========content args*/
        xArgs.AddParam("xmlProject", "", xmlProject.CreateNavigator().Select("/*"));
        xArgs.AddParam("xmlWebsiteList", "", xmlWebsiteList.CreateNavigator().Select("/*"));
        xArgs.AddParam("xmlDirectionList", "", xmlDirectionList.CreateNavigator().Select("/*"));
        xArgs.AddParam("xmlAskComList", "", xmlAskComList.CreateNavigator().Select("/*"));
        xArgs.AddParam("xmlMyTagList", "", xmlMyTagList.CreateNavigator().Select("/*"));

        xArgs.AddParam("pjGuid", "", req.pjGuid);
        xArgs.AddParam("def_date0", "", req.date0);
        xArgs.AddParam("def_viewMode", "", req.viewMode);
        xArgs.AddParam("def_researchGuid", "", req.researchGuid);
        xArgs.AddParam("def_myTag", "", req.myTag);

        xArgs.AddParam("tabName", "", "projectList");/*topbar tab用*/

        /*#################################################*/
        /*輸出處理*/
        /*#################################################*/
        XslCompiledTransform xslDoc = XmlUtil.GetXslTransform(Server.MapPath("projectDetail.xslt"));
        xslDoc.Transform(xDoc, xArgs, Response.Output);
    }

    private LocalReq GetRequest(HttpRequest Request)
    {
        LocalReq req = new LocalReq();

        req.pjGuid = string.IsNullOrEmpty(Request["pjGuid"]) ? "" : Request["pjGuid"].ToString().Trim();

        req.date0 = string.IsNullOrEmpty(Request["date0"]) ? 0 : int.Parse(Request["date0"].ToString().Trim());
        req.viewMode = string.IsNullOrEmpty(Request["viewMode"]) ? "all" : Request["viewMode"].ToString().Trim();
        req.researchGuid = string.IsNullOrEmpty(Request["researchGuid"]) ? "all" : Request["researchGuid"].ToString().Trim();
        req.myTag = string.IsNullOrEmpty(Request["myTag"]) ? "all" : Request["myTag"].ToString().Trim();

        req.empno = SSOUtil.GetCurrentUser().工號;

        return req;
    }

    private class LocalReq
    {
        public LocalReq()
            : base()
        {
        }

        public string pjGuid = "";/*專案guid*/

        public int date0 = 0;           /*時間*/
        public string viewMode = "";    /*瀏覽方式*/
        public string researchGuid = "";/*研究方向guid*/
        public string myTag = "";/*myTag*/

        public string empno = "";
    }
}