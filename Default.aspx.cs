using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string 單位代碼 = SSOUtil.GetCurrentUser().單位代碼;
        string 工號 = SSOUtil.GetCurrentUser().工號;


/////        if (單位代碼 == "20")

        if (單位代碼 == ConfigUtil.AppIEKOrgcd)
        {
            Response.Redirect("./project/default.aspx", true);
        }
        else
        {
            if (工號 == "930424"
                || 工號 == "940340"/*李諺泯*/
                || 工號 == "A60114"/*郭維軒*/
                || 工號 == "970040"/*林順傑*/
                || 工號 == "990340"/*李俊輝*/
                || 工號 == "800382"/*許昌仁*/
                || 工號 == "530956"/*劉百祥*/
                || 工號 == "A30284"/*鄞博萱*/
                )
            {
                Response.Redirect("./project/default.aspx", true);
            }
            else
            {
                Response.Write("很抱歉，本系統目前只開放IEK同仁使用.您非本系統的使用人員!");
            }
        }

    }

}