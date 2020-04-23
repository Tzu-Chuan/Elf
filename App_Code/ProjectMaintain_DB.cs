using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// ProjectMaintain_DB 的摘要描述
/// </summary>
public class ProjectMaintain_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord
    {
        set { KeyWord = value; }
    }

    int manager_sn;
    string manager_guid = string.Empty;
    string role_id = string.Empty;
    string empno = string.Empty;
    string empname = string.Empty;
    string orgcd = string.Empty;
    string deptid = string.Empty;
    DateTime create_time;
    string create_empno = string.Empty;
    string create_empname = string.Empty;

    public int _manager_sn { set { manager_sn = value; } }
    public string _manager_guid { set { manager_guid = value; } }
    public string _role_id { set { role_id = value; } }
    public string _empno { set { empno = value; } }
    public string _empname { set { empname = value; } }
    public string _orgcd { set { orgcd = value; } }
    public string _deptid { set { deptid = value; } }
    public DateTime _create_time { set { create_time = value; } }
    public string _create_empno { set { create_empno = value; } }
    public string _create_empname { set { create_empname = value; } }



    public DataSet GetManagerList(string pStart, string pEnd,string sortStr)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * into #tmp
from sys_manager_right
where 1=1 ");

        // 關鍵字
        if (KeyWord != "")
        {
            sb.Append(@"and (lower(
                                isnull(role_id,'')+isnull(empno,'')+isnull(empname,'')+isnull(orgcd,'')+isnull(deptid,'')
                                ) like '%" + KeyWord + "%') ");
        }

        sb.Append(@"select count(*) as total from #tmp
select * from (
select ROW_NUMBER() over (order by " + sortStr + @") itemNo,#tmp.*
from #tmp 
)#t where itemNo between @pStart and @pEnd ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);

        oda.Fill(ds);
        return ds;
    }

    public DataSet GetEmpList(string mode, string pStart, string pEnd, string sortStr)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Common"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
SELECT 
com_orgcd,
org_abbr_chnm1,
com_empno,
com_cname,
com_deptid,
com_mailadd
into #tmp
FROM comper
left join orgcod on org_orgcd=com_orgcd
where 1=1 ");

        if (mode == "manager")
            sb.Append(@"and com_orgcd='58' ");

        // 關鍵字
        if (KeyWord != "")
        {
            sb.Append(@"and (lower(
                                isnull(org_abbr_chnm1,'')+isnull(com_empno,'')+isnull(com_cname,'')+isnull(com_deptid,'')+isnull(com_mailadd,'')
                                ) like '%" + KeyWord + "%') ");
        }

        sb.Append(@"select count(*) as total from #tmp
select * from (
select ROW_NUMBER() over (order by " + sortStr + @") itemNo,#tmp.*
from #tmp 
)#t where itemNo between @pStart and @pEnd ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);

        oda.Fill(ds);
        return ds;
    }

    public void addManager()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        oCmd.CommandText = @"insert into sys_manager_right (
manager_guid,
role_id,
empno,
empname,
orgcd,
deptid,
create_time,
create_empno,
create_empname
) values (
@manager_guid,
@role_id,
@empno,
@empname,
@orgcd,
@deptid,
@create_time,
@create_empno,
@create_empname
) ";
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        oCmd.Parameters.AddWithValue("@manager_guid", Guid.NewGuid().ToString());
        oCmd.Parameters.AddWithValue("@role_id", role_id);
        oCmd.Parameters.AddWithValue("@empno", empno);
        oCmd.Parameters.AddWithValue("@empname", empname);
        oCmd.Parameters.AddWithValue("@orgcd", orgcd);
        oCmd.Parameters.AddWithValue("@deptid", deptid);
        oCmd.Parameters.AddWithValue("@create_time", DateTime.Now);
        oCmd.Parameters.AddWithValue("@create_empno", create_empno);
        oCmd.Parameters.AddWithValue("@create_empname", create_empname);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }

    public DataTable getManagerByEmpno()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"Select * from sys_manager_right where empno=@empno ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@empno", empno);

        oda.Fill(ds);
        return ds;
    }

    public void deleteManager()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        oCmd.CommandText = @"delete from sys_manager_right where manager_guid=@manager_guid ";
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);

        oCmd.Parameters.AddWithValue("@manager_guid", manager_guid);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }
}