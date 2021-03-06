﻿using System;
using System.Data;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

public partial class project_default_old : System.Web.UI.Page
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
        //////#region /* 瀏覽權限*/
        //////if (!RightUtil.Get_BaseRight())
        //////{
        //////    //Common.saveSecureLog();
        //////    Response.Write("message：do not have read right.");
        //////    Response.End();
        //////}
        //////#endregion

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
        string empno = SSOUtil.GetCurrentUser().工號;
        if (RightUtil.Get_BaseRight().角色是系統管理人員)
            empno = "admin";
        Dao_Project dao = new Dao_Project();
        XmlDocument xmlList = dao.getProjectList(req.q, "create_time", "desc", startRec, maxRec, empno);
        //xmlList.Save(Response.Output);
        //return;


        ////DataSet ds1 = new DataSet();
        ////ds1 = WebDAO.getProjectList(startRec, maxRec);
        
        /////*放入新dsData*/
        ////DataSet dsData = new DataSet();
        ////dsData.Tables.Add(ds1.Tables[0].Copy());

        /////*dsData to xmlDataList*/
        ////XmlDocument xmlDataList = new XmlDocument();
        ////xmlDataList.LoadXml(dsData.GetXml());
        ////xmlDataList.Save(Response.Output);
        ////return;

        /*==========set pager*/
        /*總共幾筆*/
        int totalCount = Convert.ToInt32(((XmlElement)xmlList.SelectSingleNode("/*")).GetAttribute("TotalCount"));
        /*分頁*/
        PagerUtil pager = new PagerUtil(totalCount, ConfigUtil.PageSize20, ConfigUtil.PageCount);
        pager.SetCurrentPage(req.currentPageIndex);

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

        /*==========pager args*/
        pager.AddXsltArguments(xArgs);

        /*==========content args*/
        xArgs.AddParam("xmlList", "", xmlList.CreateNavigator().Select("/*"));
        xArgs.AddParam("tabName", "", "projectList");/*topbar tab用*/

        /*#################################################*/
        /*輸出處理*/
        /*#################################################*/
        XslCompiledTransform xslDoc = XmlUtil.GetXslTransform(Server.MapPath("default_old.xslt"));
        xslDoc.Transform(xDoc, xArgs, Response.Output);
    }

    private LocalReq GetRequest(HttpRequest Request)
    {
        LocalReq req = new LocalReq();

        /*==========page*/
        req.currentPageIndex = string.IsNullOrEmpty(Request["currentPageIndex"]) ? 1 : int.Parse(Request["currentPageIndex"].ToString().Trim());

        /*==========param*/
        req.q = string.IsNullOrEmpty(Request["q"]) ? "" : Request["q"].ToString().Trim().ToLower();/*小寫*/
        return req;
    }

    private class LocalReq
    {
        public LocalReq()
            : base()
        {
        }

        public int currentPageIndex;/*page-目前頁碼*/
        public string q = "";/*關鍵字搜尋*/
    }
}