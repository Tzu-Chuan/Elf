using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class project_setTagMaintainDelete : System.Web.UI.Page
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
            int count_add = dao.execMyTagDelete(req.tagid, req.empno);
            Response.Write("OK.");



        }
        catch (Exception ex)
        {
           //////Response.Write(ex.Message);
            //////throw new Exception(CommonUtil.GetCurrLocationMsg(ex));

           Response.Write("Error message, tag delete exception!!");
        }
    }


    private LocalReq GetRequest(HttpRequest Request)
    {
        LocalReq req = new LocalReq();

        req.tagid = string.IsNullOrEmpty(Request["tagid"]) ? req.tagid : Request["tagid"].ToString().Trim();
        req.empno = SSOUtil.GetCurrentUser().工號;

        return req;
    }

    private class LocalReq
    {
        public LocalReq()
            : base()
        {
        }

        public string tagid = "";
        public string empno = "";
    }
}