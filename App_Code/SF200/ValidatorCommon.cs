using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;


namespace ISCSF200
{
    /// <summary>
    /// 資料驗證底層
    /// </summary>
    public class ValidatorCommon : System.Web.UI.Page
    {
        public ValidatorCommon()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        #region 判斷是否為數字
        /// <summary>
        /// 判斷是否為數字(不使用小數點)(0~9)
        /// </summary>
        /// <param name="str">欲判斷的值</param>
        /// <returns></returns>
        public bool IsNumeric(string str)
        {
            char[] tmp = str.ToCharArray();
            for (int i = 0; i < tmp.Length; i++)
            {
                if ((int)tmp[i] < 48 || (int)tmp[i] > 57)
                {
                    return false;
                }

            }
            return true;
        }

        #endregion

        #region 判斷是否為英數字
        /// <summary>
        /// 判斷是否為英數字
        /// </summary>
        /// <param name="str">欲判斷的值</param>
        /// <returns></returns>
        public bool IsEng(string str)
        {
            char[] tmp = str.ToCharArray();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!(((int)tmp[i] >= 48 && (int)tmp[i] <= 57) || ((int)tmp[i] >= 65 && (int)tmp[i] <= 90) || ((int)tmp[i] >= 97 && (int)tmp[i] <= 122)))//0~9  A~Z a~z
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 檢查mail正確性
        /// <summary>
        /// 判斷Email的正確性
        /// </summary>
        /// <param name="txt_mail">欲判斷mail</param>
        /// <returns></returns>

        public bool MailCheck(string txt_mail)
        {
            Regex myEmailRegex = new Regex(@"([a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+)", RegexOptions.IgnoreCase);

            if (!myEmailRegex.IsMatch(txt_mail, 0))
            {
                return false;
            }

            //!
            char[] tmp = txt_mail.ToCharArray();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i] == 33)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region 檢查特殊字元
        /// <summary>
        /// 檢查特殊字元
        /// </summary>
        /// <param name="checkValue">欲檢查的值</param>
        /// <returns></returns>
        public bool CheckSQLInjection(string checkValue)
        {
            //「%27」:「'」(單引號)
            //「%2B」:「+」(加號)
            //「alert(」:
            if (checkValue.Length > 0 && (checkValue.ToUpper().IndexOf("%27") >= 0 || checkValue.ToUpper().IndexOf("%2B") >= 0
                || checkValue.ToUpper().IndexOf("'") >= 0 || checkValue.ToUpper().IndexOf("ALERT(") >= 0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region URL檢查特殊字元
        /// <summary>
        /// URL檢查特殊字元
        /// </summary>
        /// <param name="checkValue">URL檢查特殊字元</param>
        /// <returns></returns>
        public bool URLCheckSQLInjection(string checkValue)
        {
            //「%27」:「'」(單引號)
            //「%2B」:「+」(加號)
            //「alert(」:
            if (checkValue.Length > 0 &&
                (checkValue.ToUpper().IndexOf("%27") >= 0 || checkValue.ToUpper().IndexOf("%2B") >= 0
                || checkValue.ToUpper().IndexOf("'") >= 0 || checkValue.ToUpper().IndexOf("ALERT(") >= 0)
                || checkValue.ToUpper().IndexOf("+") >= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 檢查控制項有沒有特殊字元
        /// <summary>
        /// 檢查控制項有沒有特殊字元
        /// </summary>
        /// <param name="controls">控制項集合</param>
        public void CheckControls(ControlCollection controls)
        {
            string errUrl = "Error.aspx?err=err";
            foreach (Control obj in controls)
            {
                string objTypeName = obj.GetType().Name;
                if (objTypeName == "TextBox")
                {
                    if (!CheckSQLInjection(((TextBox)obj).Text))
                    {
                        Response.Redirect(errUrl);
                    }
                }
                else if (objTypeName == "CheckBox")
                {
                    CheckBox cb = (CheckBox)obj;

                    if (cb.Attributes["value"] != null)
                    {
                        if (!CheckSQLInjection(cb.Attributes["value"].ToString()))
                        {
                            Response.Redirect(errUrl);
                        }
                    }
                }
                else if (objTypeName == "RadioButtonList")
                {
                    if (!CheckSQLInjection(((RadioButtonList)obj).SelectedValue))
                    {
                        Response.Redirect(errUrl);
                    }
                }
                else if (objTypeName == "DropDownList")
                {
                    if (!CheckSQLInjection(((DropDownList)obj).SelectedValue))
                    {
                        Response.Redirect(errUrl);
                    }
                }
                else if (objTypeName.IndexOf("usercontrol") >= 0)//針對UserControl
                {
                    CheckControls(obj.Controls);
                }
            }
        }
        #endregion


    }

}