using System;
using System.Data;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

using FlexCel.Core;
using FlexCel.XlsAdapter;

public partial class projectMgmt_inputExcelCheck : System.Web.UI.Page
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
            /*===將excel內容轉至xml*/
            XmlDocument xmlDoc = GetExcelData(req);
            //////xmlDoc.Save(Response.Output);
            //////Response.End();

            /*===將excel xml物件放入session*/
            HttpContext.Current.Session["__Session_InputExcelCheck_xmlDoc"] = xmlDoc;

            /*===取挑選網站資料*/
            Dao_ProjectMgmt dao = new Dao_ProjectMgmt();
            XmlDocument xmlOptSite = dao.selectCmd_optsite();
            //////xmlOptSite.Save(Response.Output);
            //////Response.End();

            /*#################################################*/
            /*參數處理*/
            /*#################################################*/
            /*==========common args*/
            XsltArgumentList xArgs = XmlUtil.GetXsltArguments();
            xArgs.AddParam("xmlDoc", "", xmlDoc.CreateNavigator().Select("/*"));
            xArgs.AddParam("xmlOptSite", "", xmlOptSite.CreateNavigator().Select("/*"));

            ///////xArgs.AddParam("tabName", "", "projectMgmt");/*topbar tab用*/


            /*#################################################*/
            /*輸出處理*/
            /*#################################################*/
            XslCompiledTransform xslDoc = XmlUtil.GetXslTransform(Server.MapPath("inputExcelCheck.xslt"));
            xslDoc.Transform(xmlDoc, xArgs, Response.Output);
        }
        catch (Exception ex)
        {
            //Response.Write("Error message, upload excel exception!!");
            Response.Write("Error message, " + ex.Message);
        }

    }

    private XmlDocument GetExcelData(LocalReq req)
    {
        /*#################################################*/
        /*宣告excel obj*/
        /*#################################################*/
        ExcelFile excelObj = new XlsFile(true); //new xls object
        excelObj.Open(req.pFile.InputStream);   //開啟上傳的 excel檔
        //excelObj.ActiveSheetByName = "工作表1"; //設定使用的工作表名稱為 source	
        excelObj.ActiveSheet = 1; //設定使用的工作表的數字,開始為1	


        /*#################################################*/
        /*excel資料轉入xml doc*/
        /*#################################################*/
        //===宣告參數
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(@"<root/>");

        int xx_row_count = excelObj.RowCount;//excel的筆(數)
        int xx_col_count = excelObj.ColCount;//excel的欄位(數)

        //===存入excel筆數資訊
        xDoc.DocumentElement.SetAttribute("xx_row_count", xx_row_count.ToString());/*excel行資料筆數資訊*/
        xDoc.DocumentElement.SetAttribute("xx_col_count", xx_col_count.ToString());/*excel欄資料筆數資訊*/
        xDoc.DocumentElement.SetAttribute("xx_project_guid", Guid.NewGuid().ToString());/*專案用guid*/
        //////xDoc.SelectSingleNode("/*/technology").InnerText = excelObj.GetCellValue(2, 1).ToString().Trim();
        //////xDoc.SelectSingleNode("/*/research_direction").InnerText = excelObj.GetCellValue(2, 2).ToString().Trim();

        XmlElement xeRec = null;
        XmlElement xeChild = null;
        string cellStr = "";
        string[] tmpAry = null;
        char[] tmpChar = new char[1] { '(' };

        //===存入excel資料資訊
        /*###excel是從(1,1)位置開始取得資料*/
        /*###c表示為excel欄位數,parser從B2 cell開始處理*/
        for (int col = 2; col <= xx_col_count; col++)
        {
            xeRec = xDoc.CreateElement("rec");
            /*###加入excel第幾欄屬性*/
            xeRec.SetAttribute("xx_col_num", col.ToString());
            /*###加入guid(可以當作研究方向的guid)*/
            xeRec.SetAttribute("xx_item_guid", Guid.NewGuid().ToString());


            /*###r表示為excel筆數,從第2行開始*/
            for (int row = 2; row <= xx_row_count; row++)
            {
                /*###取得cell資料*/
                cellStr = (excelObj.GetCellValue(row, col) == null) ? "" : excelObj.GetCellValue(row, col).ToString().Trim();/*去頭尾空白*/
                tmpAry = cellStr.Split(tmpChar);
                cellStr = tmpAry[0].Trim();/*只取()內容前面的文字, 再去1次頭尾空白*/

                /*###行2：系統項目說明*/
                if (row == 2)
                {
                    /*###將說明改名，方便insert時比對名稱*/
                    if (cellStr.StartsWith("專案名稱"))
                    {
                        cellStr = "__專案名稱";
                    }
                    else if (cellStr.StartsWith("觀測項目之英文全名"))
                    {
                        cellStr = "__觀測項目名稱";
                    }
                    else if (cellStr.StartsWith("觀測項目之英文簡寫"))
                    {
                        cellStr = "__觀測項目簡寫";
                    }
                    else if (cellStr.StartsWith("Research category"))
                    {
                            cellStr = "__研究方向";
                    }


                    /*###加入屬性*/
                    xeRec.SetAttribute("xx_item_explain", cellStr);
                }
                /*###行3：系統項目*/
                else if (row == 3)
                {
                    /*###加入屬性*/
                    xeRec.SetAttribute("xx_item_name", cellStr);
                }
                /*###行4之後的：內容*/
                else
                {
                    /*###cell資料空白時不建節點*/
                    //if (cellStr != "")
                    if (cellStr != "" && cellStr.ToLower().IndexOf("blank below this line") < 0)/*轉小寫比對*/
                    {
                        /*###建立節點*/
                        xeChild = xDoc.CreateElement("rec");
                        /*###加上excel第幾行屬性*/
                        xeChild.SetAttribute("xx_row_num", row.ToString());
                        /*###加入text*/
                        xeChild.InnerText = cellStr;
                        /*###將child節點加入rec*/
                        xeRec.AppendChild(xeChild);
                    }
                }
            }

            /*###加入rec節點*/
            /*###系統項目說明要有值*/
            if (
                (xeRec.GetAttribute("xx_item_explain") != "" && xeRec.GetAttribute("xx_item_name") != "")
                || (xeRec.GetAttribute("xx_item_explain") != "" && xeRec.ChildNodes.Count > 0)
               )
            {
                xDoc.DocumentElement.AppendChild(xeRec);
            }
        }

        return xDoc;
    }


    private XmlDocument CheckExcel(XmlDocument xdoc)
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(@"<root/>");

        /*===取得專案名稱*/
        string val_projectname = xdoc.SelectSingleNode("/*/*[@xx_item_explain='__專案名稱']/*") == null ? "" : xdoc.SelectSingleNode("/*/*[@xx_item_explain='__專案名稱']/*[1]").InnerText;
        if (val_projectname == "")
        {
            xDoc.DocumentElement.SetAttribute("msg_project_name", "Project Name is null.");
        }

        /*===取得觀測項目名稱*/
        string val_techname = xdoc.SelectSingleNode("/*/*[@xx_item_explain='__觀測項目名稱']/*") == null ? "" : xdoc.SelectSingleNode("/*/*[@xx_item_explain='__觀測項目名稱']/*[1]").InnerText;
        if (val_techname == "")
        {
            xDoc.DocumentElement.SetAttribute("msg_project_name", "Project Item is null.");
        }

        /*===取得研究方向*/
        XmlNodeList xlist = xdoc.SelectNodes("/*/*[@xx_item_explain='__研究方向']");
        string strData = "";
        for (int i = 0; i < xlist.Count; i++)
        {

        }

        return xDoc;
    }


    private LocalReq GetRequest(HttpRequest Request)
    {
        LocalReq req = new LocalReq();

        req.pFile = Request.Files[0];
        return req;
    }

    private class LocalReq
    {
        public LocalReq()
            : base()
        {
        }

        public HttpPostedFile pFile;
    }
}
