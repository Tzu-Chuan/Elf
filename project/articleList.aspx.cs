using System;
using System.Data;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

public partial class project_articleList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LocalReq req = GetRequest(Request);
        showPage(req);
    }


    private void showPage(LocalReq req)
    {
        /*#################################################*/
        /*log處理*/
        /*#################################################*/
        /*#################################################*/
        /*資料處理*/
        /*#################################################*/
        /*==========取得列表資料*/
        /*起始筆數*/
        int startRec = (req.currentPageIndex - 1) * ConfigUtil.PageSize20;
        startRec = (startRec < 0) ? 0 : startRec;
        /*一頁幾筆*/
        int maxRec = ConfigUtil.PageSize20;

        /*get data*/
        Dao_Project dao = new Dao_Project();
        XmlDocument xmlResultList = dao.getArticleList("", req.orderField, "", startRec, maxRec
            , req.pjGuid, req.date0, req.viewMode, req.researchGuid, req.myTag, req.typeId, req.typeGuid);
        //////xmlResultList.Save(Response.Output);
        //////return;

        /*==========set pager*/
        /*總共幾筆*/
        int totalCount = Convert.ToInt32(((XmlElement)xmlResultList.SelectSingleNode("/*")).GetAttribute("TotalCount"));
        /*分頁*/
        PagerUtil pager = new PagerUtil(totalCount, ConfigUtil.PageSize20, ConfigUtil.PageCount);
        pager.SetCurrentPage(req.currentPageIndex);

        /*==========set output xDoc*/
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml("<root><rec></rec></root>");

        /*#################################################*/
        /*參數處理*/
        /*#################################################*/
        /*==========common args*/
        XsltArgumentList xArgs = XmlUtil.GetXsltArguments();
        //base.AddPageTemplateXsltParam(xArgs);

        /*==========pager args*/
        pager.AddXsltArguments(xArgs);

        /*==========content args*/
        xArgs.AddParam("xmlResultList", "", xmlResultList.CreateNavigator().Select("/*"));
        xArgs.AddParam("pjGuid", "", req.pjGuid);

        /*分頁用*/
        xArgs.AddParam("typeId", "", req.typeId);
        xArgs.AddParam("typeGuid", "", req.typeGuid);
        xArgs.AddParam("orderField", "", req.orderField);

        /*#################################################*/
        /*輸出處理*/
        /*#################################################*/
        XslCompiledTransform xslDoc = XmlUtil.GetXslTransform(Server.MapPath("articleList.xslt"));
        xslDoc.Transform(xDoc, xArgs, Response.Output);
    }

    private LocalReq GetRequest(HttpRequest Request)
    {
        LocalReq req = new LocalReq();

        /*==========page*/
        req.currentPageIndex = string.IsNullOrEmpty(Request["currentPageIndex"]) ? 1 : int.Parse(Request["currentPageIndex"].ToString().Trim());
        req.orderField = string.IsNullOrEmpty(Request["orderField"]) ? "" : Request["orderField"].ToString().Trim();

        /*==========param*/
        req.pjGuid = string.IsNullOrEmpty(Request["pjGuid"]) ? "" : Request["pjGuid"].ToString().Trim();

        req.date0 = string.IsNullOrEmpty(Request["date0"]) ? 0 : int.Parse(Request["date0"].ToString().Trim());
        req.viewMode = string.IsNullOrEmpty(Request["viewMode"]) ? "" : Request["viewMode"].ToString().Trim();
        req.researchGuid = string.IsNullOrEmpty(Request["researchGuid"]) ? "" : Request["researchGuid"].ToString().Trim();
        req.myTag = string.IsNullOrEmpty(Request["myTag"]) ? "" : Request["myTag"].ToString().Trim();


        req.typeId = string.IsNullOrEmpty(Request["typeId"]) ? "" : Request["typeId"].ToString().Trim();
        req.typeGuid = string.IsNullOrEmpty(Request["typeGuid"]) ? "" : Request["typeGuid"].ToString().Trim();

        return req;
    }

    private class LocalReq
    {
        public LocalReq()
            : base()
        {
        }

        /*==========page*/
        public int currentPageIndex;/*page-目前頁碼*/
        public string orderField;      /*排序項目*/

        /*==========param*/
        public string pjGuid = "";/*專案guid*/

        public int date0 = 0;           /*時間*/
        public string viewMode = "";    /*瀏覽方式*/
        public string researchGuid = "";/*研究方向guid*/
        public string myTag = "";/*myTag*/

        public string typeId = "";/*查詢資料種類,1=固定來源全部,2=固定來源依網站, 3=askCom*/
        public string typeGuid = "";/*網站、或關連詞guid*/
    }
}