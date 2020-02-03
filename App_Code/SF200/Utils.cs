using System;
using System.Text.RegularExpressions;
using System.Web.UI;


namespace ISCSF200
{
    /// <summary>
    /// Summary description for Utils
    /// </summary>
    public class Utils
    {
        public Utils()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //取得 TIRI SSO ID 用於(Manage、Candidate)
        #region ITRI Single Sign On

        public static string GetITRILogOnUser(Page sender)
        {
            if (GetSSO(sender) != "")
            {  //取得 SSO
                return GetSSO(sender);
            }
            else
            {   //NT Login
                return sender.User.Identity.Name.Substring(sender.User.Identity.Name.IndexOf("\\", 0) + 1);
            }
        }

        private static string GetAttribute(Page sender, string AttrName)
        {
            string AllHttpAttrs, FullAttrName;
            int AttrLocation;

            AllHttpAttrs = sender.Request.ServerVariables["ALL_HTTP"];
            FullAttrName = "HTTP_" + AttrName.ToUpper();
            AttrLocation = AllHttpAttrs.IndexOf(FullAttrName + ":");

            if (AttrLocation > 0)
            {
                string Result;
                Result = AllHttpAttrs.Substring(AttrLocation + FullAttrName.Length + 1);
                AttrLocation = Result.IndexOf("\n");
                if (AttrLocation <= 0)
                    AttrLocation = Result.Length + 1;
                return Result.Substring(0, AttrLocation - 1);
            }
            return "";
        }

        private static string GetSSO(System.Web.UI.Page sender)
        {
            return GetAttribute(sender, "SM_USER");
        }

        #endregion

        //取得 Form 驗證 ID 用於(Preview、Review)
        public static string GetFormLogOnUser(Page page)
        {
            return page.User.Identity.Name;
        }

        public static Int32 ToInt32(string str_Integer)
        {
            return Convert.ToInt32(str_Integer);
        }

        public static bool IsInteger(string inputString)
        {
            if (inputString.Length == 0)
            {
                return false;
            }
            else
            {
                return Regex.IsMatch(inputString, @"^\d+$");
            }
        }

        public static string Get10CharDate()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        //public static bool IsPlusInt(string NumString)
        //{
        //    return Regex.IsMatch(NumString, @"^\d+$");
        //}

        public static bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.只能輸入這些字元
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        //查詢條件不可輸入 ' -- / * 等字元
        public static bool IsLegal(string striIn)
        {
            if (striIn.IndexOf("'") != -1 ||
                 striIn.IndexOf("--") != -1 ||
                 striIn.IndexOf("*") != -1 ||
                 striIn.IndexOf("/") != -1)
                return true;
            return false;
        }

        //textbox針對%做處理
        public static bool IsChange(string strInput)
        {
            if (strInput.IndexOf("%") != -1) return true;
            return false;
        }

        //檢查日期格式是否為 "yyyyMMdd
        public static bool Is8CharDatet(string inputDate)
        {
            try
            {
                DateTime DT = DateTime.ParseExact(inputDate, "yyyyMMdd", null);
                if (DT.ToString("yyyyMMdd").Equals(inputDate) && DT.Year > 1900 && DT.Year < 2999)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //檢查日期格式是否為 "yyyy/MM/dd
        public static bool CheckDateFormat(string inputDate)
        {
            try
            {
                DateTime DT = DateTime.ParseExact(inputDate, "yyyy/MM/dd", null);
                if (DT.ToString("yyyy/MM/dd").Equals(inputDate) && DT.Year > 1900 && DT.Year < 2999)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string Date_8_10(string eightCodeDate)
        {
            if (eightCodeDate.Length == 8)
            {
                return eightCodeDate.Substring(0, 4) + "/" + eightCodeDate.Substring(4, 2) + "/" + eightCodeDate.Substring(6, 2);
            }
            else
            {
                return eightCodeDate;
            }
        }

        public static string Date_10_8(string tenCodeDate)
        {
            try
            {
                if (tenCodeDate.Length != 0)
                {
                    System.DateTime DT = System.DateTime.Parse(tenCodeDate);
                    return DT.ToString("yyyyMMdd");
                }
                else
                {
                    return tenCodeDate;
                }
            }
            catch
            {
                return "19000101";
            }
        }


    }
}