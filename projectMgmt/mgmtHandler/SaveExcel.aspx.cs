using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Data.SqlClient;

public partial class projectMgmt_mgmtHandler_SaveExcel : System.Web.UI.Page
{
	ProjectMGMT_DB db = new ProjectMGMT_DB();
	protected void Page_Load(object sender, EventArgs e)
	{
		///-----------------------------------------------------
		///功    能: 儲存 Excel
		///說    明:
		/// * Request["optsite"]: monitored websites
		///-----------------------------------------------------
		XmlDocument xDoc = new XmlDocument();

		/// Transaction
		SqlConnection oConn = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"].ToString());
		oConn.Open();
		SqlCommand oCmmd = new SqlCommand();
		oCmmd.Connection = oConn;
		SqlTransaction myTrans = oConn.BeginTransaction();
		oCmmd.Transaction = myTrans;
		try
		{
			#region 權限判斷
			if (!RightUtil.Get_BaseRight().角色是系統或專案管理人員)
				throw new Exception("do not have read right.");
			#endregion

			string[] optsite = (string.IsNullOrEmpty(Request["optsite"])) ? null : Request["optsite"].ToString().Trim().Split(',');

			#region Get Excel XML Session
			object obj = Session["__Session_xmlDoc"];
			XmlDocument xmlDoc = new XmlDocument();
			if (obj != null)
				xmlDoc = (XmlDocument)obj;
			else
				throw new Exception("Error message: excel data overtime!!");
			#endregion

			string pjGuid = Guid.NewGuid().ToString();

			// sys_project_right
			db.InsertPj_Right(pjGuid, oConn, myTrans);

			// input_project
			string pjName = (xmlDoc.SelectSingleNode("/*/*[@col_num='2']/*") == null) ? "" : xmlDoc.SelectSingleNode("/*/*[@col_num='2']/*[1]").InnerText;
			string tech = (xmlDoc.SelectSingleNode("/*/*[@col_num='3']/*") == null) ? "" : xmlDoc.SelectSingleNode("/*/*[@col_num='3']/*[1]").InnerText;
			string tnRelatedWord = string.Empty;
			XmlNodeList xlist_tnRelatedWord = xmlDoc.SelectNodes("/*/*[@col_num='4']/*");
			for (int i = 0; i < xlist_tnRelatedWord.Count; i++)
			{
				if (tnRelatedWord != "") tnRelatedWord += "^";
				tnRelatedWord += xlist_tnRelatedWord[i].InnerText;
			}
			db.InsertPj_Project(pjGuid, pjName, tech, tnRelatedWord, oConn, myTrans);

			// input_website
			if (optsite != null)
			{
				for (int i = 0; i < optsite.Length; i++)
				{
					db.InsertPj_Website(pjGuid, optsite[i], oConn, myTrans);
				}
			}

			// input_research_direction  &  input_related_word
			XmlNodeList xlist = xmlDoc.SelectNodes("/*/Category");
			if (xlist.Count > 0)
			{
				for (int i = 0; i < xlist.Count; i++)
				{
					int col = Int32.Parse(xlist[i].Attributes["col_num"].Value);
					if (col < 5)
						continue;
					else
					{
						string tmpResearchGuid = xlist[i].Attributes["item_guid"].Value;
						string tmpResearchName = xlist[i].Attributes["item_name"].Value;
						db.InsertPj_ResearchDirection(pjGuid, tmpResearchGuid, tmpResearchName, oConn, myTrans);

						XmlNodeList xlist_Word = xlist[i].SelectNodes("rec");
						if (xlist_Word.Count > 0)
						{
							for (int j = 0; j < xlist_Word.Count; j++)
							{
								string tmpWord = xlist_Word[j].InnerText;
								db.InsertPj_RelatedWord(pjGuid, tmpResearchGuid, tmpWord, oConn, myTrans);
							}
						}
					}
				}
			}

			myTrans.Commit();

			string xmlstr = string.Empty;

			xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>Save Success</Response></root>";
			xDoc.LoadXml(xmlstr);
		}
		catch (Exception ex)
		{
			myTrans.Rollback();
			xDoc = ExceptionUtil.GetExceptionDocument(ex);
		}
		finally
		{
			oCmmd.Connection.Close();
			oConn.Close();
		}
		Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
		xDoc.Save(Response.Output);
	}
}