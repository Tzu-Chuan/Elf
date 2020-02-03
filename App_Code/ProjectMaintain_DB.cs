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
}