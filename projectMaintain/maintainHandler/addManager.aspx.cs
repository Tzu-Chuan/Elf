using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class projectMaintain_maintainHandler_addManager : System.Web.UI.Page
{
    ProjectMaintain_DB pm_db = new ProjectMaintain_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        ///-----------------------------------------------------
        ///功    能: 新增管理員
        ///說    明:
        /// * Request["role_id"]: 身分
        /// * Request["orgcd"]: 所代碼
        /// * Request["empno"]: 工號
        /// * Request["name"]: 姓名
        /// * Request["deptid"]: 部門代碼
        ///-----------------------------------------------------
        XmlDocument xDoc = new XmlDocument();
        try
        {
            #region 權限判斷
            if (!RightUtil.Get_BaseRight().角色是系統管理人員)
            {
                xDoc = ExceptionUtil.GetErrorMassageDocument("do not have read right.");
                Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
                xDoc.Save(Response.Output);
                return;
            }
            #endregion

            string role_id = (string.IsNullOrEmpty(Request["role_id"])) ? "" : Request["role_id"].ToString().Trim();
            string orgcd = (string.IsNullOrEmpty(Request["orgcd"])) ? "" : Request["orgcd"].ToString().Trim();
            string empno = (string.IsNullOrEmpty(Request["empno"])) ? "" : Request["empno"].ToString().Trim();
            string name = (string.IsNullOrEmpty(Request["name"])) ? "" : Request["name"].ToString().Trim();
            string deptid = (string.IsNullOrEmpty(Request["deptid"])) ? "" : Request["deptid"].ToString().Trim();


            string xmlstr = string.Empty;
            pm_db._role_id = role_id;
            pm_db._orgcd = orgcd;
            pm_db._empno = empno;
            pm_db._empname = name;
            pm_db._deptid = deptid;
            pm_db._create_empno = SSOUtil.GetCurrentUser().工號;
            pm_db._create_empname = SSOUtil.GetCurrentUser().姓名;
            
            DataTable mdt = pm_db.getManagerByEmpno();
            if (mdt.Rows.Count > 0)
            {
                xDoc = ExceptionUtil.GetErrorMassageDocument("is already a list manager.");
                Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
                xDoc.Save(Response.Output);
                return;
            }
            else
                pm_db.addManager();

            xmlstr = "<?xml version='1.0' encoding='utf-8'?><root><Response>Save Success</Response></root>";
            xDoc.LoadXml(xmlstr);
        }
        catch (Exception ex)
        {
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }
}