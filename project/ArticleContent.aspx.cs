using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class project_ArticleContent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(Request["atGuid"]))
        {
            Response.Write("Message：Parameter Error !!");
            Response.End();
        }
    }
}