using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class project_setTagMaintainAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            /*===get req*/
            LocalReq req = GetRequest(Request);

            /*===check*/


            /*===exec*/
            Dao_Project dao = new Dao_Project();
            int count_now = dao.getMyTagCount(req.empno);

            if (count_now >= 10)
            {
                Response.Write("Message, can not add new tag, tag max number is 10.");
            }
            else
            {
                int count_add = dao.execMyTagAdd(req.newTagName, req.empno);

                Response.Write("OK.");
            }


        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            //////throw new Exception(CommonUtil.GetCurrLocationMsg(ex));

            
            ////Response.Write("Error message, tag add exception!!");
        }
    }


    private LocalReq GetRequest(HttpRequest Request)
    {
        LocalReq req = new LocalReq();

        req.newTagName = string.IsNullOrEmpty(Request["newTagName"]) ? req.newTagName : Request["newTagName"].ToString().Trim();
        req.empno = SSOUtil.GetCurrentUser().工號;

        return req;
    }

    private class LocalReq
    {
        public LocalReq()
            : base()
        {
        }

        public string newTagName = "";
        public string empno = "";
    }
}