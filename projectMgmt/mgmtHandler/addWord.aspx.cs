﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Xml;

public partial class projectMgmt_mgmtHandler_addWord : System.Web.UI.Page
{
    ProjectMGMT_DB mgmt_db = new ProjectMGMT_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增字詞
        ///說    明:
        /// * Request["pjGuid"]: Project Guid
        /// * Request["wGuid"]: Word Guid
        /// * Request["TopicID"]: Topic Guid
        /// * Request["Word"]: 字詞
        /// * Request["Blacklist"]: 黑/白名單
        /// * Request["OrgTopic"]: 原 分類
        /// * Request["OrgWord"]: 原 字詞
        /// * Request["OrgBlacklist"]: 原 黑/白名單
        /// * Request["OrgAnalysis"]: 原 字詞來源
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
            string wGuid = (string.IsNullOrEmpty(Request["wGuid"])) ? "" : Request["wGuid"].ToString().Trim();
            string TopicID = (string.IsNullOrEmpty(Request["TopicID"])) ? "" : Request["TopicID"].ToString().Trim();
            string Word = (string.IsNullOrEmpty(Request["Word"])) ? "" : Request["Word"].ToString().Trim();
            string Word_stem = (string.IsNullOrEmpty(Request["Word_stem"])) ? "" : Request["Word_stem"].ToString().Trim();
            string Blacklist = (string.IsNullOrEmpty(Request["Blacklist"])) ? "" : Request["Blacklist"].ToString().Trim();
            string OrgTopic = (string.IsNullOrEmpty(Request["OrgTopic"])) ? "" : Request["OrgTopic"].ToString().Trim();
            string OrgWord = (string.IsNullOrEmpty(Request["OrgWord"])) ? "" : Request["OrgWord"].ToString().Trim();
            string OrgBlacklist = (string.IsNullOrEmpty(Request["OrgBlacklist"])) ? "" : Request["OrgBlacklist"].ToString().Trim();
            string OrgAnalysis = (string.IsNullOrEmpty(Request["OrgAnalysis"])) ? "" : Request["OrgAnalysis"].ToString().Trim();

            

            string xmlstr = string.Empty;
            if (wGuid == "")
            {
				#region 檢查字詞是否存在
				DataTable dt = mgmt_db.CheckWordExist(pjGuid, Word, TopicID);
				if (dt.Rows.Count > 0)
				{
					xDoc = ExceptionUtil.GetErrorMassageDocument("The word is already in the list.");
					Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
					xDoc.Save(Response.Output);
					return;
				}
				#endregion

				string newGuid = Guid.NewGuid().ToString();
                mgmt_db.addWord(oConn, myTrans, newGuid, TopicID, Word, Word_stem, Blacklist);
                mgmt_db.InsertWordLog(oConn, myTrans, pjGuid, newGuid,"add");
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>Add Success</Response></root>";
            }
            else
            {
                if (Word != OrgWord)
                {
                    #region 檢查字詞是否存在
                    DataTable dt = mgmt_db.CheckWordExist(pjGuid, Word, TopicID);
                    if (dt.Rows.Count > 0)
                    {
                        xDoc = ExceptionUtil.GetErrorMassageDocument("The word is already in the list.");
                        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
                        xDoc.Save(Response.Output);
                        return;
                    }
                    #endregion
                }

                mgmt_db.UpdateWord(oConn, myTrans, wGuid, TopicID, Word, Blacklist, OrgAnalysis);
                mgmt_db.InsertWordLog(oConn, myTrans, pjGuid, wGuid, "update", OrgTopic, OrgWord, OrgBlacklist, OrgAnalysis);
                xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>Save Done</Response></root>";
            }

            myTrans.Commit();
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

    private string GetBlacklistName(string status)
    {
        string tmpV = string.Empty;
        switch (status)
        {
            case "0":
                tmpV = "Whitelist";
                break;
            case "1":
                tmpV = "Blacklist";
                break;
            case "2":
                tmpV = "Candidate";
                break;
        }
        return tmpV;
    }
}