using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// Member 的摘要描述
/// </summary>
public class Member
{
    string KeyWord = string.Empty;
    public string _KeyWord
    {
        set { KeyWord = value; }
    }
    #region 私用
    string PM_ID = string.Empty;
    string PM_Guid = string.Empty;
    string PM_ProjectGuid = string.Empty;
    string PM_Empno = string.Empty;
    string PM_Name = string.Empty;
    string PM_Deptid = string.Empty;
    DateTime PM_CreateDate;
    string PM_CreateEmpno = string.Empty;
    string PM_CreateName = string.Empty;
    #endregion
    #region 公用
    public string _PM_ID { set { PM_ID = value; } }
    public string _PM_Guid { set { PM_Guid = value; } }
    public string _PM_ProjectGuid { set { PM_ProjectGuid = value; } }
    public string _PM_Empno { set { PM_Empno = value; } }
    public string _PM_Name { set { PM_Name = value; } }
    public string _PM_Deptid { set { PM_Deptid = value; } }
    public DateTime _PM_CreateDate { set { PM_CreateDate = value; } }
    public string _PM_CreateEmpno { set { PM_CreateEmpno = value; } }
    public string _PM_CreateName { set { PM_CreateName = value; } }
    #endregion

    public DataTable getProjectInfo(string project_guid)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"Select project_name from input_project where project_guid=@project_guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@project_guid", project_guid);

        oda.Fill(ds);
        return ds;
    }

    public DataSet getMemberList(string pStart, string pEnd)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"Select PM_ID,PM_Guid,PM_Empno,PM_Name,PM_Deptid 
into #tmpAll from pj_member 
where PM_ProjectGuid=@PM_ProjectGuid ");

        if (KeyWord != "")
            sb.Append(@"and ((upper(PM_Empno) LIKE '%' + upper(@KeyWord) + '%') or (upper(PM_Name) LIKE '%' + upper(@KeyWord) + '%')) ");

        sb.Append(@"
--總筆數
select count(*) as total from #tmpAll
--分頁資料
select * from (
select ROW_NUMBER() over (order by PM_Empno) itemNo,#tmpAll.*
from #tmpAll
)#tmp where itemNo between @pStart and @pEnd

drop table #tmpAll  ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@PM_ProjectGuid", PM_ProjectGuid);
        oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);
        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);

        oda.Fill(ds);
        return ds;
    }


    public DataSet getCommon(string pStart, string pEnd)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Common"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"Select com_orgcd,com_empno,com_cname,com_deptid,com_deptcd 
into #tmpAll from comper 
where com_depcd<>'Y' ");

        //sb.Append(@"and com_orgcd='58'");

        if (KeyWord != "")
            sb.Append(@"and ((upper(com_empno) LIKE '%' + upper(@KeyWord) + '%') or (upper(com_cname) LIKE '%' + upper(@KeyWord) + '%')) ");

        sb.Append(@"
--總筆數
select count(*) as total from #tmpAll
--分頁資料
select * from (
select ROW_NUMBER() over (order by com_empno) itemNo,#tmpAll.*
from #tmpAll
)#tmp where itemNo between @pStart and @pEnd

drop table #tmpAll  ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@KeyWord", KeyWord);
        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);

        oda.Fill(ds);
        return ds;
    }


    public void addMember()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        oCmd.CommandText = @"insert into pj_member (
PM_Guid,
PM_ProjectGuid,
PM_Empno,
PM_Name,
PM_Deptid,
PM_CreateEmpno,
PM_CreateName
) values (
@PM_Guid,
@PM_ProjectGuid,
@PM_Empno,
@PM_Name,
@PM_Deptid,
@PM_CreateEmpno,
@PM_CreateName
) ";
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        oCmd.Parameters.AddWithValue("@PM_Guid", Guid.NewGuid().ToString("N"));
        oCmd.Parameters.AddWithValue("@PM_ProjectGuid", PM_ProjectGuid);
        oCmd.Parameters.AddWithValue("@PM_Empno", PM_Empno);
        oCmd.Parameters.AddWithValue("@PM_Name", PM_Name);
        oCmd.Parameters.AddWithValue("@PM_Deptid", PM_Deptid);
        oCmd.Parameters.AddWithValue("@PM_CreateEmpno", PM_CreateEmpno);
        oCmd.Parameters.AddWithValue("@PM_CreateName", PM_CreateName);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }


    public void deleteMember()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        oCmd.CommandText = @"delete from pj_member where PM_ID=@PM_ID ";
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);

        oCmd.Parameters.AddWithValue("@PM_ID", PM_ID);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }

    public DataTable getMemberByEmpno()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"Select * from pj_member where PM_ProjectGuid=@PM_ProjectGuid and PM_Empno=@PM_Empno ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@PM_ProjectGuid", PM_ProjectGuid);
        oCmd.Parameters.AddWithValue("@PM_Empno", PM_Empno);

        oda.Fill(ds);
        return ds;
    }
}