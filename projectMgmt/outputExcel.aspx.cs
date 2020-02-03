using System;
using System.Data;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

using FlexCel.Core;
using FlexCel.XlsAdapter;


public partial class projectMgmt_outputExcel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
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
        /*宣告excel obj*/
        /*#################################################*/
        string template_filename = ConfigUtil.ElfTemplateFile;
        string fpath = Path.Combine(HttpRuntime.AppDomainAppPath, string.Format(@"App_Data\{0}", template_filename));
        ////string templateFile = Path.Combine(HttpRuntime.AppDomainAppPath, @"App_Data\test.xlsx");/*excel template*/

        ExcelFile excelObj = new XlsFile(fpath); //new xls object
        //excelObj.ActiveSheetByName = "工作表1"; //設定使用的工作表名稱為 source	
        excelObj.ActiveSheet = 1; //設定使用的工作表的數字,開始為1	


        /*#################################################*/
        /*將資料寫入excel obj*/
        /*#################################################*/
        Dao_ProjectMgmt dao = new Dao_ProjectMgmt();
        //XmlDocument xDoc = null;

        /*============專案名稱、項目、同義詞*/
        XmlDocument xDoc_pj = dao.selectCmd_project(req.pjGuid);
        ////xDoc_pj.Save(Response.Output);
        ////Response.End();

        string project_name = xDoc_pj.SelectSingleNode("/*/*/@project_name").Value;
        excelObj.SetCellValue(4, 2, project_name);/*填入r4-c2*/

        string technology = xDoc_pj.SelectSingleNode("/*/*/@technology").Value;
        excelObj.SetCellValue(4, 3, technology);/*填入r4-c3*/

        string tnKey = xDoc_pj.SelectSingleNode("/*/*/@tn_related_word").Value;
        string[] separators = { "^" };
        string[] tnKeys = tnKey.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < tnKeys.Length; i++)
        {
            excelObj.SetCellValue(4 + i, 4, tnKeys[i]);/*填入(r4~r?)-c4*/
        }


        /*============*/
        ///////////////XmlDocument xDoc_dirt = dao.selectCmd_researchDirection("A4BC1360-7B3D-4AE3-A1C8-8196FF878BA9");
        ////////////////xDoc2.Save(Response.Output);
        ////////////////Response.End();

        ////////XmlNodeList xlist = xDoc2.SelectNodes("/*/*");
        ////////for (int i = 0; i < xlist.Count; i++)
        ////////{
        ////////    excelObj.SetCellValue(3, 6 + i, ((XmlElement)xlist[i]).GetAttribute("name"));
        ////////}

        /*============研究方向和關連詞*/
        XmlDocument xDoc_dt = dao.selectCmd_researchDirection(req.pjGuid);
        XmlDocument xDoc_rel = dao.selectCmd_relatedWord(req.pjGuid);
        ////xDoc_rel.Save(Response.Output);
        ////Response.End();

        XmlNodeList xlist_dt = xDoc_dt.SelectNodes("/*/*");
        XmlNodeList xlist_rel = null;
        string directionName= null;
        string relatedwordName = null;
        for (int c = 0; c < xlist_dt.Count; c++)
        {
            directionName = ((XmlElement)xlist_dt[c]).GetAttribute("name");
            excelObj.SetCellValue(3, 5 + c, directionName);/*填入r3-(c5~c?)*/


            xlist_rel = xDoc_rel.SelectNodes(string.Format("/*/*[@direction='{0}']", directionName));
            for (int r = 0; r < xlist_rel.Count; r++)
            {
                relatedwordName = ((XmlElement)xlist_rel[r]).GetAttribute("relatedword");
                excelObj.SetCellValue(4 + r, 5 + c, relatedwordName);/*填入(r4~r?)-(c5~c?)*/
            }        
        }

        /*#################################################*/
        /*輸出*/
        /*#################################################*/
        /*===處理中文檔名問題，因ie與非ie browser對編碼方式不同所以需分別處理*/
        string org_filename = string.Format("IEKELF_{0}_{1}.xlsx", project_name, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
        string chi_filename = (CheckBrowserIsMS(Request) == true)
            ? string.Format("{0}", HttpUtility.UrlPathEncode(org_filename)) : string.Format("\"{0}\"", org_filename);

        /*===判斷副檔名*/
        bool IsXls = Path.GetExtension(org_filename).Equals(".xls", StringComparison.OrdinalIgnoreCase);

        /*===判斷excel格式*/
        TFileFormats fileFormat;
        if (IsXls) fileFormat = TFileFormats.Xls;
        else fileFormat = TFileFormats.Xlsx;


   
        using (MemoryStream ms = new MemoryStream())
        {
            excelObj.Save(ms, TFileFormats.Xlsx);
            ms.Position = 0;

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + chi_filename);
            Response.AddHeader("Content-Length", ms.Length.ToString());
            
            if (IsXls) Response.ContentType = StandardMimeType.Xls;
            else Response.ContentType = StandardMimeType.Xlsx;

            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }

        

    }

    private bool CheckBrowserIsMS(HttpRequest Request)
    {
        /*2015/09/02 by 聖宏:處理ie與非ie browser對編碼方式不同的問題*/
        /*2015/12/25 by 聖宏:增加對ms edge的檢查*/
        bool isMS = false;
        if (Request.Browser.Browser == "IE"
           || Request.Browser.Browser == "InternetExplorer"
           || Request.UserAgent.IndexOf("Edge") > -1
           )
        {
            isMS = true;
        }
        else
        {
            isMS = false;
        }
        return isMS;
    }




    private LocalReq GetRequest(HttpRequest Request)
    {
        LocalReq req = new LocalReq();

        /*==========param*/
        //////req.pjGuid = "A4BC1360-7B3D-4AE3-A1C8-8196FF878BA9";
        req.pjGuid = string.IsNullOrEmpty(Request["pjGuid"]) ? "" : Request["pjGuid"].ToString().Trim();
        return req;
    }

    private class LocalReq
    {
        public LocalReq()
            : base()
        {
        }

        public string pjGuid = "";/*專案guid*/
    }
}