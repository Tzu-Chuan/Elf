using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// ProjectMGMT_DB 的摘要描述
/// </summary>
public class ProjectMGMT_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord
    {
        set { KeyWord = value; }
    }

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

    public DataSet GetMGMT_List(string empno,string pStart, string pEnd)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select project_guid into #tmpMemberProj
from input_project
left join pj_member on PM_ProjectGuid=project_guid
where PM_Empno=@empno

select input_project.*,
pj_status.status_en_name as status_en_name,
PR.empname+'('+PR.empno+')' as OwnerName,
PR.empno as OwnerEmpno,
case 
	when PR.empno=@empno then 'Y'
	when @empno='admin' then 'Y'
	else 'N'
end as MemberStatus
into #tmp
from input_project
left join sys_project_right as PR on PR.project_guid=input_project.project_guid
left join pj_status on status_id=input_project.status
where 1=1 ");

        if (empno != "admin")
            sb.Append(@"and PR.empno=@empno or input_project.project_guid in (select * from #tmpMemberProj) ");

        // 關鍵字
        if (KeyWord != "")
        {
            sb.Append(@"and (lower(
                                isnull(technology,'')+isnull(tn_related_word,'')+isnull(status_en_name,'')+isnull(PR.empname+'('+PR.empno+')','')
                                ) like '%" + KeyWord + "%') ");
        }

        sb.Append(@"select count(*) as total from #tmp
select * from (
select ROW_NUMBER() over (order by create_time desc) itemNo,#tmp.*
from #tmp 
)#t where itemNo between @pStart and @pEnd ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@empno", empno);
        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetManager(string empno)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"Select * from sys_manager_right where 1=1 ");
        if (empno != "")
            sb.Append(@" and empno=@empno");


        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@empno", empno);

        oda.Fill(ds);
        return ds;
    }

    public DataSet GetArticleList(string project_guid, string topic, int date, string myTag, string SortName, string pStart, string pEnd)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select * into #tmp from (
	select * from 
	(  
		select
		article_guid
		, project_guid
		, title
		, get_time
		, full_text 
		,articledesc = paragraph_xml.value('(/paragraph/key[@name=''" + topic + @"''])[1]', 'varchar(max)')
		,score =  category_score_xml.value('(/score/key[@name=''" + topic + @"''])[1]', 'float')
		 from result_article
	) as tb 
	where (1=1)
	and project_guid=@project_guid
	and score > 0 ");

        if (date > 0)/*條件：時間*/
        {
            sb.Append(@" and get_time >= DateAdd(day,@date, CONVERT(VARCHAR(10) ,GETDATE(),111) ) ");
        }

        if (myTag != "all")/*條件：有myTag條件時*/
        {
            sb.Append(@" and article_guid in 
                                    (select article_guid from sys_tagrec
                                    where project_guid = @project_guid 
                                    and tagtype_guid=@myTag) ");
        }

        sb.Append(@")#d ");

        sb.Append(@"
select count(*) as total from #tmp

select * from (
	select ROW_NUMBER() over (order by " + SortName + @" desc) itemNo,#tmp.*
	from #tmp 
)#t where itemNo between @pStart and @pEnd");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@project_guid", project_guid);
        oCmd.Parameters.AddWithValue("@topic", topic);
        oCmd.Parameters.AddWithValue("@myTag", myTag);
        oCmd.Parameters.Add("@date", SqlDbType.Int).Value = date * -1;
        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);

        oda.Fill(ds);
        return ds;
    }
}