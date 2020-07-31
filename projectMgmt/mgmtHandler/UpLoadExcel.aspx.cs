using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using FlexCel.Core;
using FlexCel.XlsAdapter;

public partial class projectMgmt_mgmtHandler_UpLoadExcel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        XmlDocument xDoc = new XmlDocument();

        #region 權限
        if (!RightUtil.Get_BaseRight().角色是系統或專案管理人員)
        {
            xDoc = ExceptionUtil.GetErrorMassageDocument("Error message：do not have read right.");
            Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
            xDoc.Save(Response.Output);
            return;
        }
        #endregion

        try
        {
            HttpFileCollection uploadFiles = Request.Files;//檔案集合
            HttpPostedFile aFile = uploadFiles[0];
            
            ExcelFile excelObj = new XlsFile(true); //new xls object
            excelObj.Open(aFile.InputStream);   //開啟上傳的 excel檔
            //excelObj.ActiveSheetByName = "工作表1"; //設定使用的工作表名稱為 source	
            excelObj.ActiveSheet = 1; //設定使用的工作表的數字,開始為1	

            XmlDocument xmldoc = new XmlDocument();
            XmlElement pInfo = xmldoc.CreateElement("ProjectInfo");
            pInfo.SetAttribute("project_guid", Guid.NewGuid().ToString());
            xmldoc.AppendChild(pInfo);

            XmlElement xCol = null;
            XmlElement xRow = null;
            string cellStr = string.Empty;
            string[] tmpAry = null;

            for (int col = 2; col <= excelObj.ColCount; col++)
            {
                xCol = xmldoc.CreateElement("Category");
                xCol.SetAttribute("col_num", col.ToString());
                xCol.SetAttribute("item_guid", Guid.NewGuid().ToString());

                for (int row = 3; row <= excelObj.RowCount; row++)
                {
                    cellStr = (excelObj.GetCellValue(row, col) == null) ? "" : excelObj.GetCellValue(row, col).ToString().Trim();
                    tmpAry = cellStr.Split('(');
                    cellStr = tmpAry[0].Trim();/*只取()內容前面的文字, 再去1次頭尾空白*/

                    if (row == 3)
                    {
                        xCol.SetAttribute("item_name", cellStr);
                    }
                    else
                    {
                        if (cellStr != "")
                        {
                            xRow = xmldoc.CreateElement("rec");
                            // 加上excel第幾行屬性
                            xRow.SetAttribute("row_num", row.ToString());
                            xRow.InnerText = cellStr;
                            xCol.AppendChild(xRow);
                        }
                    }
                }

                if (xCol.GetAttribute("item_name") != "" && xCol.ChildNodes.Count > 0)
                    pInfo.AppendChild(xCol);
            }

            // 將excel xml物件放入session
            Session["__Session_xmlDoc"] = xmldoc;

            string xmlStr = "<?xml version='1.0' encoding='UTF-8'?><root>" + xmldoc.OuterXml.ToString() + "</root>";
            xDoc.LoadXml(xmlStr);
        }
        catch (Exception ex)
        {
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }
}