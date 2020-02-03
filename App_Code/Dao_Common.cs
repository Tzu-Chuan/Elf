using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.Xml;

/// <summary>
/// Dao_Common 的摘要描述
/// </summary>
public class Dao_Common
{
    /*============================================================================*/
    /*儲存登入記錄(save login log)*/
    public static int Save_Loginlog(string log_empno, string log_ip, DateTime log_datetime, string log_modulename, string log_systemname, string log_systemid, string log_userdeptid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
insert into [sys_loginlog]([log_empno],[log_ip],[log_datetime],[log_modulename],[log_systemname],[log_systemid],[log_userdeptid])
values(@log_empno,@log_ip,@log_datetime,@log_modulename,@log_systemname,@log_systemid,@log_userdeptid)
;");

            oCmd.CommandText = sb.ToString();
            oCmd.Connection = DbUtil.GetConn();
            oCmd.Parameters.AddWithValue("@log_empno", log_empno);
            oCmd.Parameters.AddWithValue("@log_ip", log_ip);
            oCmd.Parameters.AddWithValue("@log_datetime", log_datetime);
            oCmd.Parameters.AddWithValue("@log_modulename", log_modulename);
            oCmd.Parameters.AddWithValue("@log_systemname", log_systemname);
            oCmd.Parameters.AddWithValue("@log_systemid", log_systemid);
            oCmd.Parameters.AddWithValue("@log_userdeptid", log_userdeptid);

            int count = DbUtil.ExecCmdNoResult(oCmd);
            return count;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }


    /*============================================================================*/
}