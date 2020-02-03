using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class projectMgmt_setRelatedWordSchedule : System.Web.UI.Page
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
            if (req.typeId != "4")
            {
                Response.Write("Error message, typeId error!!");
                return;
            }

            if (req.schedule != "0" && req.schedule != "1")
            {
                Response.Write("Error message, schedule error!!");
                return;
            }

            /*===exec*/
            Dao_ProjectMgmt dao = new Dao_ProjectMgmt();
            req.newSchedule = (req.schedule == "0") ? "1" : "0";
            int count = dao.exec_relatedWord_schedule_update(req.typeGuid, req.newSchedule);

            if (count != 1)
            {
                ////throw new Exception(string.Format("訊息：異動錯誤(correct=1,error={0}).", count));
                Response.Write("Error message, update data count error!!");
            }
            else
            {
                Response.Write("OK.");
            }
        }
        catch (Exception ex)
        {
            //throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
            Response.Write("Error message, setRelatedWordSchedule exception!!");
        }
    }


    private LocalReq GetRequest(HttpRequest Request)
    {
        LocalReq req = new LocalReq();

        /*==========param*/
        req.typeId = string.IsNullOrEmpty(Request["typeId"]) ? "" : Request["typeId"].ToString().Trim();
        req.typeGuid = string.IsNullOrEmpty(Request["typeGuid"]) ? "" : Request["typeGuid"].ToString().Trim();
        req.schedule = string.IsNullOrEmpty(Request["schedule"]) ? "" : Request["schedule"].ToString().Trim();

        return req;
    }

    private class LocalReq
    {
        public LocalReq()
            : base()
        {
        }

        public string typeId = "";/*查詢種類,1=固定來源全部,2=固定來源依網站, 3=askCom*/
        public string typeGuid = "";/*網站、或關連詞guid*/
        public string schedule = "";/*0為不納入每日排成, 1為納入每日排程*/

        public string newSchedule = "";

    }
}