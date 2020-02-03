using System;
using System.Data;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

public partial class project_articleDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LocalReq req = GetRequest(Request);

        ////string url = "http://61.61.246.46:8100/articles/tagging_tool/";
        string url = ConfigUtil.AppArticle;
        string empno = SSOUtil.GetCurrentUser().工號;
        string startTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        string isProjectOwner = "Y";
        string isSystemManager = "Y";
        //req.pjGuid = "A4BC1360-7B3D-4AE3-A1C8-8196FF878BA9";
        //req.atGuid = "320C0F20-7437-4C50-8BED-6912BB396880";

        ///string keyid = string.Format(@"930424^2018/01/03 14:55^Y^Y^A4BC1360-7B3D-4AE3-A1C8-8196FF878BA9^320C0F20-7437-4C50-8BED-6912BB396880");

        /*參數為：工號、目前時間、專案管理者、系統管理者、專案guid、文章guid*/
        string keyid = string.Format(@"{0}^{1}^{2}^{3}^{4}^{5}",
                ToBase64String(empno)
                , ToBase64String(startTime)
                , ToBase64String(isProjectOwner)
                , ToBase64String(isSystemManager)
                , ToBase64String(req.pjGuid)
                , ToBase64String(req.atGuid)
            );


        ////Response.Write("<br/>" + req.pjGuid);
        ////Response.Write("<br/>" + req.atGuid);
        ////return;

        keyid = ToBase64String(keyid);
        //Response.Write(keyid);

        Response.Redirect(url + keyid, true);
    }


    public static string ToBase64String(string str)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
    }

    public static string FromBase64String(string str)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(str));
    }


    private LocalReq GetRequest(HttpRequest Request)
    {
        LocalReq req = new LocalReq();

        /*==========param*/
        req.pjGuid = string.IsNullOrEmpty(Request["pjGuid"]) ? "" : Request["pjGuid"].ToString().ToUpper().Trim();
        req.atGuid = string.IsNullOrEmpty(Request["atGuid"]) ? "" : Request["atGuid"].ToString().ToUpper().Trim();

        return req;
    }

    private class LocalReq
    {
        public LocalReq()
            : base()
        {
        }

        public string pjGuid = "";/*專案guid*/
        public string atGuid = "";/*文章guid*/
    }
}