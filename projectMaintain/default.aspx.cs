using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class projectMaintain_default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 權限判斷
        if (!RightUtil.Get_BaseRight().角色是系統管理人員)
        {
            Response.Write("message：do not have read right.");
            Response.End();
            return;
        }
        #endregion
    }
}