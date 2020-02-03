using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class projectMgmt_setPjClose : System.Web.UI.Page
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

            /*===get req*/
            LocalReq req = GetRequest(Request);

            /*===check*/


            /*===exec*/
            Dao_ProjectMgmt dao = new Dao_ProjectMgmt();
            int count = dao.exec_project_close(req.pjGuid);

            if (count != 1)
            {
                throw new Exception(string.Format("訊息：異動錯誤(correct=1,error={0}).", count));
            }
            else
            {
                Response.Write("OK.");
            }
        }
        catch (Exception ex)
        {
            //throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
            Response.Write("Error message, project close exception!!");
        }
    }


    private LocalReq GetRequest(HttpRequest Request)
    {
        LocalReq req = new LocalReq();

        /*==========param*/
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