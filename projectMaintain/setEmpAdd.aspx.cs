using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class projectMaintain_setEmpAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            /*#################################################*/
            /*check處理*/
            /*#################################################*/
            #region /* 權限*/
            if (!RightUtil.Get_BaseRight().角色是系統管理人員)
            {
                //Common.saveSecureLog();
                Response.Write("Error message：do not have read right.");
                Response.End();
            }
            #endregion

            /*===get req*/
            LocalReq req = GetRequest(Request);

            /*===check*/
            ////////if (req.role_id <= 0 || req.role_id >= 3)
            ////////{
            ////////    Response.Write("message：role parameter error.");
            ////////    return;
            ////////}
            if (req.empno == "" || req.empname == "" || req.deptid == "")
            {
                Response.Write("message：empno parameter error.");
                return;
            }


            /*===exec*/
            Dao_ProjectMaintain dao = new Dao_ProjectMaintain();
            int count = dao.exec_empadd(req.role_id.ToString(), req.empno, req.empname, req.orgcd, req.deptid);

            if (count != 1)
            {
                throw new Exception(string.Format("message：add error(correct=1,error={0}).", count));
            }
            else
            {
                Response.Write("OK.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }


    private LocalReq GetRequest(HttpRequest Request)
    {
        LocalReq req = new LocalReq();

        /*==========param*/
        req.role_id = string.IsNullOrEmpty(Request["role_id"]) ? "" : Request["role_id"].ToString().Trim();
        req.empno = string.IsNullOrEmpty(Request["empno"]) ? "" : Request["empno"].ToString().Trim();
        req.empname = string.IsNullOrEmpty(Request["empname"]) ? "" : Request["empname"].ToString().Trim();
        req.orgcd = string.IsNullOrEmpty(Request["orgcd"]) ? "" : Request["orgcd"].ToString().Trim();
        req.deptid = string.IsNullOrEmpty(Request["deptid"]) ? "" : Request["deptid"].ToString().Trim();
        return req;
    }

    private class LocalReq
    {
        public LocalReq()
            : base()
        {
        }

        public string role_id = "";
        public string empno = "";
        public string empname = "";
        public string orgcd = "";
        public string deptid = "";

    }
}