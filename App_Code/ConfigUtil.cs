using System;
using System.Configuration;
using System.Web;

// ====================================================================================
// 異動時間: 2017/10/20
// 作    者: 聖宏
// 功    能: 
// ====================================================================================
public class ConfigUtil
{
    /*============================================================================*/
    /*=====SSO Config*/
    public static readonly bool DebugSSOMode = (GetConfig("DebugSSOMode").ToLower() == "true") ? true : false;
    public static readonly string DebugSSOAccount = GetConfig("DebugSSOAccount");

    /*=====Db Config*/
    public static readonly string DSN_Default = GetConfig("DSN.Default");
    public static readonly string DSN_Common = GetConfig("DSN.Common");

    /*=====App Config*/
    public static readonly string AppRoot = (HttpRuntime.AppDomainAppVirtualPath == "/") ? "" : HttpRuntime.AppDomainAppVirtualPath;
    //public static readonly string AppUrl = GetConfig("AppUrl");
    public static readonly string AppTitle = GetConfig("AppTitle");

    public static readonly string AppWordHouse = GetConfig("AppWordHouse");
    public static readonly string AppArticle = GetConfig("AppArticle");
    public static readonly string AppWordCloud = GetConfig("AppWordCloud");

    public static readonly string AppIEKOrgcd = GetConfig("AppIEKOrgcd");

    public static readonly string ElfTemplateFile = GetConfig("ElfTemplateFile");
    public static readonly string ElfSampleTemplateFile = GetConfig("ElfSampleTemplateFile");


    /*=====Page Config*/
    public static readonly int PageSize5 = Convert.ToInt32(GetConfig("PageSize5"));
    public static readonly int PageSize10 = Convert.ToInt32(GetConfig("PageSize10"));
    public static readonly int PageSize20 = Convert.ToInt32(GetConfig("PageSize20"));
    public static readonly int PageSize50 = Convert.ToInt32(GetConfig("PageSize50"));
    public static readonly int PageSize100 = Convert.ToInt32(GetConfig("PageSize100"));

    public static readonly int PageCount = Convert.ToInt32(GetConfig("PageCount"));

    /*=====FilePath Config*/
    //public static readonly string UploadPathRoot = GetConfig("UploadPathRoot");

    ///*=====Email Config*/
    //public static bool Email_DebugSendMode = (GetConfig("Email_DebugSendMode").ToLower() == "true") ? true : false;
    //public static string Email_DebugSendAddress = GetConfig("Email_DebugSendAddress");
    //public static string Email_DebugSendSymbol = GetConfig("Email_DebugSendSymbol");
    //public static bool Email_DebugBccMode = (GetConfig("Email_DebugBccMode").ToLower() == "true") ? true : false;
    //public static string Email_DebugBccAddress = GetConfig("Email_DebugBccAddress");
    //public static string Email_SmtpServer = GetConfig("Email_SmtpServer");
    //public static string Email_SmtpAccount = GetConfig("Email_SmtpAccount");
    //public static string Email_SmtpPassword = GetConfig("Email_SmtpPassword");
    //public static string Email_FromAddress = GetConfig("Email_FromAddress");
    //public static string Email_FromName = GetConfig("Email_FromName");
    //public static string Email_AdminAddress = GetConfig("Email_AdminAddress");
    //public static string Email_AdminSubject = GetConfig("Email_AdminSubject");
    //public static string Email_ErrorAddress = GetConfig("Email_ErrorAddress");
    //public static string Email_ErrorSubject = GetConfig("Email_ErrorSubject");

    /*============================================================================*/
    static ConfigUtil()
    {
    }

    /*============================================================================*/
    private static string GetConfig(string keyName)
    {
        try
        {
            string retStr = ConfigurationManager.AppSettings[keyName];
            if (retStr == null)
            {
                throw new Exception(string.Format("config key不存在({0})!", keyName));
            }
            return retStr;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*============================================================================*/
}


