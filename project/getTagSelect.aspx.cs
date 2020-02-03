using System;
using System.Data;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

public partial class project_getTagSelect : System.Web.UI.Page
{
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


        /*#################################################*/
        /*log處理*/
        /*#################################################*/


        /*#################################################*/
        /*資料處理*/
        /*#################################################*/
        /*get data*/
        Dao_Project dao = new Dao_Project();
        XmlDocument xmlList = dao.getArticleTagSelect(req.pjid, req.arcid, req.empno);
        //////xmlList.Save(Response.Output);
        //////return;

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
        xArgs.AddParam("xmlList", "", xmlList.CreateNavigator().Select("/*"));
        xArgs.AddParam("pjid", "", req.pjid);
        xArgs.AddParam("arcid", "", req.arcid);


        /*#################################################*/
        /*輸出處理*/
        /*#################################################*/
        XslCompiledTransform xslDoc = XmlUtil.GetXslTransform(Server.MapPath("getTagSelect.xslt"));
        xslDoc.Transform(xDoc, xArgs, Response.Output);
    }

    private LocalReq GetRequest(HttpRequest Request)
    {
        LocalReq req = new LocalReq();
        req.pjid = string.IsNullOrEmpty(Request["pjid"]) ? req.pjid : Request["pjid"].ToString();
        req.arcid = string.IsNullOrEmpty(Request["arcid"]) ? req.arcid : Request["arcid"].ToString();

        req.empno = SSOUtil.GetCurrentUser().工號;

        return req;
    }

    private class LocalReq
    {
        public LocalReq()
            : base()
        {
        }

        public string pjid = "";
        public string arcid = "";
        public string empno = "";

    }
}