using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class project_setTagSelectSave : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            /*===get req*/
            LocalReq req = GetRequest(Request);

            ////////////Response.Write(req.pjid);
            ////////////Response.Write("<br/>");
            ////////////Response.Write(req.arcid);
            ////////////Response.Write("<br/>");
            ////////////Response.Write(req.cbSelected);
            ////////////Response.End();

            /*===check*/


            /*===exec*/
            Dao_Project dao = new Dao_Project();
            int count = dao.exec_tagSelectSave(req.pjid, req.arcid, req.empno, req.cbSelectedArray);

            Response.Write("OK.");
        }
        catch (Exception ex)
        {
            //////Response.Write(ex.Message);
            //////throw new Exception(CommonUtil.GetCurrLocationMsg(ex));

            
            Response.Write("Error message, tag save exception!!");
        }
    }


    private LocalReq GetRequest(HttpRequest Request)
    {
        LocalReq req = new LocalReq();

        req.pjid = string.IsNullOrEmpty(Request["pjid"]) ? req.pjid : Request["pjid"].ToString().Trim();
        req.arcid = string.IsNullOrEmpty(Request["arcid"]) ? req.arcid : Request["arcid"].ToString().Trim();

        req.cbSelected = string.IsNullOrEmpty(Request["cbSelected"]) ? req.cbSelected : Request["cbSelected"].ToString().Trim();
        req.cbSelectedArray = req.cbSelected.Split(',');

        req.empno = SSOUtil.GetCurrentUser().工號;

        return req;
    }

    private class LocalReq
    {
        public LocalReq()
            : base()
        {
        }


        public string pjid = "";
        public string arcid = "";

        public string cbSelected = "";
        public string[] cbSelectedArray = null;

        public string empno = "";
    }
}