using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class projectMaintain_setEmpDelete : System.Web.UI.Page
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


            /*===exec*/
            Dao_ProjectMaintain dao = new Dao_ProjectMaintain();
            int count = dao.exec_empdel(req.empGuid);

            if (count != 1)
            {
                throw new Exception(string.Format("message：delete error(correct=1,error={0}).", count));
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
        req.empGuid = string.IsNullOrEmpty(Request["empGuid"]) ? "" : Request["empGuid"].ToString().Trim();
        return req;
    }

    private class LocalReq
    {
        public LocalReq()
            : base()
        {
        }

        public string empGuid = "";/*管理者guid*/

    }
}