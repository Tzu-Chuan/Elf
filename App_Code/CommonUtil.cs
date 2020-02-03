using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;

// ====================================================================================
// 異動時間: 2013/02
// 作    者: asamchnag
// 功    能: 
// ====================================================================================
public class CommonUtil
{
    /*============================================================================*/
    public static string GetCurrentUserIP()
    {
        return HttpContext.Current.Request.UserHostAddress;
    }

    /*ex msg*/
    public static string GetCurrLocationMsg(Exception ex)
    {
        string strDllName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Module.Name;
        string strClassName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().ReflectedType.Name;
        string strMethodName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
        return string.Format(@"(in {0}\{1}) {2}", strClassName, strMethodName, ex.Message);
    }

    public static string GetCurrLocationMsg(Exception ex, string selfErrorDesc)
    {
        string strDllName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Module.Name;
        string strClassName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().ReflectedType.Name;
        string strMethodName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
        return string.Format(@"(in {0}\{1}) {2} {3}", strClassName, strMethodName, selfErrorDesc, ex.Message);
    }

    /*explain:特殊字元處理-SQL LIKE*/
    public static string FilterSqlLikeStr(string keyword)
    {
        string newKeyword = keyword;
        if (newKeyword != "")
        {
            newKeyword = newKeyword.Replace("[", "[[]");/*只需處理左邊中括號,右邊自然失效*/
            newKeyword = newKeyword.Replace("%", "[%]");
            newKeyword = newKeyword.Replace("_", "[_]");
        }
        return newKeyword;
    }


    /*============================================================================*/
}

