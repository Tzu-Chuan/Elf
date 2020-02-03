using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace ISCSF200
{

    /// <summary>
    /// Summary description for Cls_Data
    /// </summary>
    public class DDListItem
    {
        private string _dataValue;
        private string _dataText;
        public DDListItem(string dataText, string dataValue)
        {
            _dataText = dataText;
            _dataValue = dataValue;
        }

        public string DataValue
        {
            get
            {
                return _dataValue;
            }
            set
            {
                _dataValue = value;
            }
        }

        public string DataText
        {
            get
            {
                return _dataText;
            }
            set
            {
                _dataText = value;
            }
        }
    }


    public class Cls_Data
    {
        public Cls_Data()
        {
        }

        private static string _Basinfowinvalue;
        private static string _Mbrinfowinvalue;
        private static string _Basinfogrptypevalue;
        public static string Basinfogrptypevalue
        {
            get { return _Basinfogrptypevalue; }
            set { _Basinfogrptypevalue = value; }
        }
        public static string Basinfowinvalue
        {
            get
            {
                return _Basinfowinvalue;
            }
            set
            {
                _Basinfowinvalue = value;
            }
        }
        public static string Mbrinfowinvalue
        {
            get
            {
                return _Mbrinfowinvalue;
            }
            set
            {
                _Mbrinfowinvalue = value;
            }
        }

        public static string Getloginname(Page page)
        {
            //login-姓名(工號)
            string com_empno = Utils.GetITRILogOnUser(page);
            SqlCommand sqlCmd_loginname = new SqlCommand("select rtrim(com_cname) + '(' + rtrim(com_empno) + ')' from common..comper where com_empno = @com_empno");
            sqlCmd_loginname.Parameters.AddWithValue("@com_empno", com_empno);
            object obj = Common.Data.runScalar(sqlCmd_loginname);
            if (obj == null)
            {
                return null;
            }
            else
            {
                return obj.ToString();
            }
        }

        public static DataView Getdeptcontact(string depcd)// depcd 就是查聯絡人窗口用的工號
        {
            //查詢同一部門成員
            SqlCommand sqlCmd_depcd = new SqlCommand("select rtrim(com_empno) as com_empno from common..comper where com_depcd <> 'Y' and com_deptcd=@deptcd");
            //SqlCommand sqlCmd_depcd = new SqlCommand("select rtrim(com_empno) as com_empno, com_cname + '(' + com_empno + ')' as com_cname from common..comper where com_depcd <> 'Y' and com_deptcd=@deptcd");
            sqlCmd_depcd.Parameters.AddWithValue("@deptcd", depcd);
            DataView dv = Common.Data.runParaCmd(sqlCmd_depcd);
            if (dv.Count == 0)
            {
                return null;
            }
            else
            {
                return dv;
            }

        }

        public static ListItem[] GetOrgcd(string D4orgcd, string changeorgcd)
        {
            //changeorgcd是否為單選項(固定)，1：固定，0：變動
            //單位挑選清單
            string SQL = "select rtrim(org_orgcd)as org_orgcd ,rtrim(org_orgchnm) + '(' + rtrim(org_abbr_egnm) + ')' as org_orgchnm from common..orgcod where org_status='A' ";
            if (changeorgcd == "1" && !string.IsNullOrEmpty(D4orgcd))
                SQL += " and org_orgcd=" + D4orgcd;

            DataView dv = Common.Data.runParaCmd(new SqlCommand(SQL));
            ListItem[] lis;
            if (dv.Count.Equals(0))
            {
                lis = new ListItem[1];
                lis[0] = new ListItem("無資料", "0");
            }
            else
            {
                lis = new ListItem[dv.Count];
                for (int i = 0; i < dv.Count; i++)
                {
                    lis[i] = new ListItem(dv[i]["org_orgchnm"].ToString(), dv[i]["org_orgcd"].ToString());
                }
            }
            return lis;
        }

        public static ListItem[] GetOrgcd1()
        {
            //查詢全部單位
            DataView dv = Common.Data.runParaCmd(new SqlCommand("select rtrim(org_orgcd)as org_orgcd ,rtrim(org_abbr_chnm1) as org_abbr_chnm1 from common..orgcod where org_status='A'"));
            ListItem[] lis;
            if (dv.Count.Equals(0))
            {
                lis = new ListItem[1];
                lis[0] = new ListItem("無資料", "0");
            }
            else
            {
                lis = new ListItem[dv.Count];
                for (int i = 0; i < dv.Count; i++)
                {
                    lis[i] = new ListItem(dv[i]["org_abbr_chnm1"].ToString(), dv[i]["org_orgcd"].ToString());
                }
            }
            return lis;
        }

        public static ListItem[] GetDept(string orgcd)
        {
            //查詢部門
            SqlCommand sqlCmd_dept = new SqlCommand("select rtrim(dep_deptcd) as dep_deptcd, rtrim(dep_deptcd)+rtrim(dep_deptname) as dep_deptname  from common..depcod where dep_orgcd=@orgcd");
            sqlCmd_dept.Parameters.AddWithValue("@orgcd", orgcd);
            DataView dv = Common.Data.runParaCmd(sqlCmd_dept);
            ListItem[] lis;
            if (dv.Count.Equals(0))
            {
                lis = new ListItem[1];
                lis[0] = new ListItem("無部門資料", "0");
            }
            else
            {
                lis = new ListItem[dv.Count];
                for (int i = 0; i < dv.Count; i++)
                {
                    lis[i] = new ListItem(dv[i]["dep_deptname"].ToString(), dv[i]["dep_deptcd"].ToString());
                }
            }
            return lis;
        }

        public static ListItem[] GetDept1(string orgcd)
        {
            //查詢部門
            SqlCommand sqlCmd_dept = new SqlCommand("select rtrim(dep_deptcd) as dep_deptcd, rtrim(dep_deptname) as dep_deptname from common..depcod where dep_orgcd=@orgcd");
            sqlCmd_dept.Parameters.AddWithValue("@orgcd", orgcd);
            DataView dv = Common.Data.runParaCmd(sqlCmd_dept);
            ListItem[] lis;
            if (dv.Count.Equals(0))
            {
                lis = new ListItem[1];
                lis[0] = new ListItem("無部門資料", "0");
            }
            else
            {
                lis = new ListItem[dv.Count];
                for (int i = 0; i < dv.Count; i++)
                {
                    lis[i] = new ListItem(dv[i]["dep_deptname"].ToString(), dv[i]["dep_deptcd"].ToString());
                }
            }
            return lis;
        }

        public static ListItem[] GetBody(string orgcd, string deptcd)
        {
            //查詢部門人員
            SqlCommand sqlCmd_dept = new SqlCommand("select rtrim(com_empno) as com_empno, rtrim(com_cname) as com_cname from common..comper where com_depcd <> 'Y' and com_orgcd = @orgcd and com_deptcd = @deptcd");
            sqlCmd_dept.Parameters.AddWithValue("@orgcd", orgcd);
            sqlCmd_dept.Parameters.AddWithValue("@deptcd", deptcd);
            DataView dv = Common.Data.runParaCmd(sqlCmd_dept);
            ListItem[] lis;
            if (dv.Count.Equals(0))
            {
                lis = new ListItem[1];
                lis[0] = new ListItem("無部門人員資料", "0");
            }
            else
            {
                lis = new ListItem[dv.Count];
                for (int i = 0; i < dv.Count; i++)
                {
                    lis[i] = new ListItem(dv[i]["com_cname"].ToString(), dv[i]["com_empno"].ToString());
                }
            }
            return lis;
        }

    }
}
