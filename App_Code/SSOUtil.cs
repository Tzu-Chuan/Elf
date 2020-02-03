using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Xml;

// ==========================================================================================================
// 異動時間: 2012/9
// 作    者: 
// 功    能: 
// ==========================================================================================================
public class SSOUtil
{
    /*explain:get current user info*/
    public static UserInfo GetCurrentUser()
    {
        try
        {
        string __UserInfo = "UserInfo";
        if (HttpContext.Current.Session[__UserInfo] == null)
        {
            string empno = GetSsoUser();
            if (string.IsNullOrEmpty(empno))
            {
                    //HttpContext.Current.Response.Write("Error,取得帳號失敗,請通知管理者,謝謝!");
                    //HttpContext.Current.Response.End();
                    throw new Exception("取得SSO帳號失敗");
            }
            HttpContext.Current.Session[__UserInfo] = new UserInfo(empno);
        }
        return (UserInfo)HttpContext.Current.Session[__UserInfo];
    }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*explain:get current sso id*/
    public static string GetSsoUser()
    {
        try
        {
            if (ConfigUtil.DebugSSOMode == true)
        {
            return ConfigUtil.DebugSSOAccount;
        }
        else
        {
            return (HttpContext.Current.Request.ServerVariables["HTTP_SM_USER"] == null ? "" : HttpContext.Current.Request.ServerVariables["HTTP_SM_USER"]).ToUpper();
        }
    }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }
}


// ==========================================================================================================
// 異動時間: 2012/9
// 作    者: 
// 功    能: 
// ==========================================================================================================
public class UserInfo
{
    public readonly string 工號 = "";
    public string 姓名 = "";
    public string 分機 = "";
    public string Email = "";
    public string 部門 = "";
    public string 部門代碼 = "";
    public string 部門名稱 = "";
    public string 單位代碼 = "";
    public string 單位名稱 = "";
    public string 上次登入時間 = "";
    public bool 是否為系統管理者 = false;
    public XmlDocument 成員登入資訊檔 = null;

    /// <summary>
    /// 
    /// </summary>
    public UserInfo(string empno)
    {
        XmlElement xe = null;
        XmlDocument xDoc_UserWithEmpno = GetData_UserWithEmpno(empno);
        XmlNode xNode = xDoc_UserWithEmpno.SelectSingleNode("/*/*");
        if (xNode != null)
        {
            xe = (XmlElement)xNode;

            /*取得-個人登入資訊*/
            this.工號 = xe.GetAttribute("工號");
            this.姓名 = xe.GetAttribute("中文名");
            this.分機 = xe.GetAttribute("分機");
            this.Email = xe.GetAttribute("Email");
            this.部門 = xe.GetAttribute("部門");
            this.部門代碼 = xe.GetAttribute("部門代碼");
            this.部門名稱 = xe.GetAttribute("部門名稱");
            this.單位代碼 = xe.GetAttribute("單位代碼");
            this.單位名稱 = xe.GetAttribute("單位名稱");

            /*取得-是否為系統管理者*/
            //XmlDocument xDoc_Admin = new XmlDocument();
            //xDoc_Admin.Load( HttpContext.Current.Server.MapPath( "~/App_Data/UserList.xml" ) );
            //if( xDoc_Admin.SelectSingleNode( string.Format( @"/*/group[@id='administrators']/*[@id='{0}']", this.工號 ) ) != null )
            //{
            //    this.是否為系統管理者 = true;
            //}

            /*取得-上次登入時間*/
            //.....
        }

        /*取得-成員登入資訊檔*/
        XmlDocument xDoc_UserXml = new XmlDocument();
        xDoc_UserXml.LoadXml("<root><rec/></root>");
        xe = (XmlElement)xDoc_UserXml.SelectSingleNode("/*/*");
        xe.SetAttribute("工號", this.工號);
        xe.SetAttribute("姓名", this.姓名);
        xe.SetAttribute("分機", this.分機);
        xe.SetAttribute("Email", this.Email);
        xe.SetAttribute("部門", this.部門);
        xe.SetAttribute("部門代碼", this.部門代碼);
        xe.SetAttribute("部門名稱", this.部門名稱);
        xe.SetAttribute("單位代碼", this.單位代碼);
        xe.SetAttribute("單位名稱", this.單位名稱);
        xe.SetAttribute("上次登入時間", this.上次登入時間);
        xe.SetAttribute("是否為系統管理者", (this.是否為系統管理者 ? "1" : "0"));
        this.成員登入資訊檔 = xDoc_UserXml;
    }

    /// <summary>
    /// 
    /// </summary>
    XmlDocument GetData_UserWithEmpno(string empno)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"
                        SELECT
                        RTRIM(comper.com_orgcd) AS [單位代碼]
                        ,RTRIM(orgcod.org_abbr_chnm2) AS [單位名稱]
                        ,RTRIM(comper.com_deptcd) AS [部門代碼]
                        ,RTRIM(depcod.dep_deptname) AS [部門名稱]
                        ,RTRIM(comper.com_deptid) AS [部門]
                        ,RTRIM(comper.com_empno) AS [工號]
                        ,RTRIM(comper.com_cname) AS [中文名]
                        ,RTRIM(comper.com_ename) AS [英文名]
                        ,RTRIM(comper.com_telext) AS [分機]
                        ,RTRIM(comper.com_mailadd) AS [Email]
                        FROM comper
                        INNER JOIN depcod ON comper.com_deptid = depcod.dep_deptid
                        INNER JOIN orgcod ON comper.com_orgcd = orgcod.org_orgcd
                        WHERE 1=1
                        AND comper.com_depcd='N'
                        AND orgcod.org_status='A'
                        AND comper.com_empno = @com_empno
                        ORDER BY com_empno
                     ;");

        SqlCommand oCmd = new SqlCommand(sb.ToString(), DbUtil.GetConn(ConfigUtil.DSN_Common));
        oCmd.Parameters.Add("@com_empno", SqlDbType.NVarChar, 10).Value = empno;
        return DbUtil.GetDB2Xml(oCmd, "yyyy/MM/dd HH:mm");
    }

}



