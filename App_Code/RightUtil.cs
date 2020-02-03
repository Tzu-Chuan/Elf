using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.Xml;

/// <summary>
/// RightUtil 的摘要描述
/// </summary>
public class RightUtil
{
    /*============================================================================*/
    /*人員權限*/

    static readonly string __Session_BaseRightInfo = "__Session_BaseRightInfo";

    public static BaseRightInfo Get_BaseRight()
    {
        try
        {
            if (HttpContext.Current.Session[__Session_BaseRightInfo] == null)
            {
                string empno = SSOUtil.GetCurrentUser().工號;
                BaseRightInfo info = new BaseRightInfo(empno);
                
                info.XML權限檔 = CreateXmlObj(info);
                HttpContext.Current.Session[__Session_BaseRightInfo] = info;
            }
            return (BaseRightInfo)HttpContext.Current.Session[__Session_BaseRightInfo];
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }


    public class BaseRightInfo
    {
        public bool 角色是系統管理人員 = false;
        public bool 角色是專案管理人員 = false;
        public bool 角色是系統或專案管理人員 = false;
        public XmlDocument XML權限檔 = null;

        public BaseRightInfo(string empno)
        {
            /*取得資料*/
            XmlDocument xDoc = GetData_SysManager(empno);

            /*設定權限*/
            this.角色是系統管理人員 = xDoc.SelectNodes("/*/*[@role_id='sys_mgr']").Count == 1;
            this.角色是專案管理人員 = xDoc.SelectNodes("/*/*[@role_id='pj_mgr']").Count == 1;
            this.角色是系統或專案管理人員 = xDoc.SelectNodes("/*/*[@role_id='sys_mgr' or @role_id='pj_mgr']").Count >= 1;
        }

        static XmlDocument GetData_SysManager(string empno)
        {
            try
            {
                string sqlStr = @"
Select distinct role_id From [sys_manager_right] where empno = @empno;";

                SqlCommand oCmd = new SqlCommand();
                oCmd.CommandText = sqlStr;
                oCmd.Parameters.AddWithValue("@empno", empno);

                /*result*/
                XmlDocument xDoc = DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn());
                return xDoc;
            }
            catch (Exception ex)
            {
                throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
            }
        }
    }

    /*============================================================================*/
    /*專案瀏覽權限*/
    public static ReadRightInfo Get_ReadRight(string pjGuid)
    {
        try
        {
            string empno = SSOUtil.GetCurrentUser().工號;
            ReadRightInfo info = new ReadRightInfo(pjGuid, empno);
            info.XML權限檔 = CreateXmlObj(info);
            return info;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }


    public class ReadRightInfo
    {
        public bool 所有人都可瀏覽此專案 = false;
        public bool 是否為此專案負責人員 = false;
        public bool 是否為此專案讀取人員 = false;
        public bool 角色是否可瀏覽此專案 = false;
        public XmlDocument XML權限檔 = null;

        public ReadRightInfo(string pjGuid, string empno)
        {
            /*取得資料*/
            XmlDocument xDoc = GetData_ProjectRight(pjGuid, empno);

            /*設定權限*/
            this.所有人都可瀏覽此專案 = xDoc.SelectNodes("/*/*[@role_id='alliek']").Count == 1;
            this.是否為此專案負責人員 = xDoc.SelectNodes("/*/*[@role_id='owner']").Count == 1;
            this.是否為此專案讀取人員 = xDoc.SelectNodes("/*/*[@role_id='reader']").Count == 1;
            this.角色是否可瀏覽此專案 = this.所有人都可瀏覽此專案
                                || this.是否為此專案負責人員
                                || this.是否為此專案讀取人員;
        }

        static XmlDocument GetData_ProjectRight(string pjGuid, string empno)
        {
            try
            {
                string sqlStr = @"
Select distinct role_id From [sys_project_right] where project_guid = @pjGuid and role_id = 'alliek' 
union    
Select distinct role_id From [sys_project_right] where project_guid = @pjGuid and empno = @empno 
;";

                SqlCommand oCmd = new SqlCommand();
                oCmd.CommandText = sqlStr;
                oCmd.Parameters.Add("@pjGuid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
                oCmd.Parameters.AddWithValue("@empno", empno);

                /*result*/
                XmlDocument xDoc = DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn());
                return xDoc;
            }
            catch (Exception ex)
            {
                throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
            }
        }
    }

    /*============================================================================*/
    static XmlDocument CreateXmlObj(object sObj)
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml("<root><rec/></root>");
        XmlElement xe = (XmlElement)xDoc.SelectSingleNode("/*/*");
        foreach (System.Reflection.FieldInfo f in sObj.GetType().GetFields())
        {
            if (f.FieldType.ToString() == "System.Boolean")
                xe.SetAttribute(f.Name, (Convert.ToBoolean(f.GetValue(sObj)) == true) ? "1" : "0");
        }
        return xDoc;
    }

    /*============================================================================*/
}