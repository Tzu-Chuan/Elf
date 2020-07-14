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

        sb.Append(@"Select project_name,technology,create_time from input_project where project_guid=@project_guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@project_guid", project_guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetProjectOwner(string project_guid)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from sys_project_right where project_guid=@project_guid ");

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
            sb.Append(@"and (PR.empno=@empno or input_project.project_guid in (select * from #tmpMemberProj)) ");

        // 關鍵字
        if (KeyWord != "")
        {
            sb.Append(@"and (lower(
                                isnull(project_name,'')+isnull(technology,'')+isnull(tn_related_word,'')+isnull(status_en_name,'')+isnull(PR.empname+'('+PR.empno+')','')
                                ) like '%" + KeyWord.ToLower() + "%') ");
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

    public DataSet GetArticleList(string project_guid, string website_guid, string topic, int date, string myTag, string SortName, string pStart, string pEnd)
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
        , website_guid
		, project_guid
		, title
		, CONVERT(nvarchar(10),get_time,23) as get_time
		,articledesc = paragraph_xml.value('(/paragraph/key[@name=''" + topic + @"''])[1]', 'varchar(max)')
		,score =  category_score_xml.value('(/score/key[@name=''" + topic + @"''])[1]', 'float')
        ,(SELECT DATEDIFF(DAY,get_time,getdate())) as DaysDiff
        ,(select CONVERT(nvarchar(10),min(get_time),23) from result_article where project_guid=@project_guid) as MinTime
        ,(select count(*) from ReadArticle where R_ArticleGuid=article_guid and R_Empno=@empno) as HaveRead
		 from result_article
	) as tb 
	where (1=1)
	and project_guid=@project_guid
	and score > 0 ");

        // 時間
        if (website_guid !="")
        {
            sb.Append(@" and website_guid=@website_guid ");
        }

        // 時間
        if (date > 0)
        {
            sb.Append(@" and get_time >= DateAdd(day,@date, CONVERT(VARCHAR(10) ,GETDATE(),111) ) ");
        }

        // MyTag
        if (myTag != "all")
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
	select ROW_NUMBER() over (order by " + SortName + @") itemNo,#tmp.*
	from #tmp 
)#t where itemNo between @pStart and @pEnd");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@empno", SSOUtil.GetCurrentUser().工號);
        oCmd.Parameters.AddWithValue("@project_guid", project_guid);
        oCmd.Parameters.AddWithValue("@website_guid", website_guid);
        oCmd.Parameters.AddWithValue("@topic", topic);
        oCmd.Parameters.AddWithValue("@myTag", myTag);
        oCmd.Parameters.Add("@date", SqlDbType.Int).Value = date * -1;
        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);


        oda.Fill(ds);
        return ds;
    }

    public DataSet GetArticleDetail(string article_guid)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
declare @pjguid nvarchar(50);
select @pjguid=project_guid from result_article where article_guid=@article_guid;

select article.*,web.website_name,optsite_url from result_article as article
  left join input_website as web on web.website_guid=article.website_guid
  left join sys_opt_site on optsite_name=web.website_name
where article_guid=@article_guid 

select d.research_guid,d.name as topic,w.name,w.name_stem from input_research_direction as d 
left join input_related_word as w on w.research_guid=d.research_guid and blacklist='0'
where project_guid=@pjguid
order by d.name,w.name ");
        

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@article_guid", article_guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable CheckWordRepeat(string article_guid)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
declare @pjguid nvarchar(50);
select @pjguid=project_guid from result_article where article_guid=@article_guid;


select w.name,count(w.name) as total into #tmp from input_research_direction as d 
left join input_related_word as w on w.research_guid=d.research_guid and blacklist='0'
where project_guid=@pjguid
group by w.name

select ISNULL(max(total),0) as MaxNum from #tmp

drop table #tmp ");


        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@article_guid", article_guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetWebsite(string project_guid, string topic, int date, string myTag)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
select * ,(select count(*) from result_article where website_guid = tb.website_guid and category_score_xml.value('(/score/key[@name=(''"+ topic + @"'')])[1]', 'float') > 0 ");

        if (date > 0)/*條件：時間*/
        {
            sb.Append(@" and get_time >= DateAdd(day,@date, CONVERT(VARCHAR(10) ,GETDATE(),111)) ");
        }

        if (myTag != "all")/*條件：有myTag條件時*/
        {
            sb.Append(@" and article_guid in 
                                    (select article_guid from sys_tagrec
                                    where project_guid = @project_guid 
                                    and tagtype_guid=@myTag) ");
        }

        sb.Append(@") as results ");
        sb.Append(@"from input_website as tb
where project_guid=@project_guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@project_guid", project_guid);
        oCmd.Parameters.AddWithValue("@topic", topic);
        oCmd.Parameters.AddWithValue("@myTag", myTag);
        oCmd.Parameters.Add("@date", SqlDbType.Int).Value = date * -1;

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetAskCom(string project_guid, string topic,int date)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
Select distinct a.related_guid --//關連詞id
, a.related_sn --//關連詞新增時順序
, a.name as related_name --//關連詞名稱
, a.blacklist --//關連詞狀態
, a.schedule --//是否納入每日排程
, a.analyst_give--//關連詞來源
, b.research_guid--//研究方向id
, b.name as research_name --//研究方向名稱
, b.research_sn  --//研究方向新增時順序
, (select count(*) from result_search where related_guid = a.related_guid ");

        if (date > 0)/*條件：時間*/
        {
            sb.Append(@" and get_time >= DateAdd(day,@date, CONVERT(VARCHAR(10) ,GETDATE(),111)) ");
        }

        sb.Append(@") as result_count --//各關連詞結果筆數
From input_related_word a
inner join input_research_direction b on (b.research_guid = a.research_guid) and (b.project_guid = @project_guid)
where a.blacklist !=1--//排除黑名單(0=白,1=黑,2=候選詞)
        and a.analyst_give !=2--//排除未審核(0=excel匯入時,1=使用者新增,2=系統產生未審核,3=系統產生已審核)  
 ");

        if (topic != "all")
        {
            sb.Append(@" and a.research_guid=@topic ");
        }

        sb.Append(@" order by research_sn, schedule desc, related_name asc--//改為有排程+關連詞. 2018/4/12,asam ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@project_guid", project_guid);
        oCmd.Parameters.AddWithValue("@topic", topic);
        oCmd.Parameters.Add("@date", SqlDbType.Int).Value = date * -1;

        oda.Fill(ds);
        return ds;
    }

    public DataSet GetAskComArticles(string related_guid, int date, string pStart, string pEnd)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select title, url, describe_text, score, get_time,(SELECT DATEDIFF(DAY,get_time,getdate())) as DaysDiff
                    ,(select CONVERT(nvarchar(10),min(get_time),23) from result_search where related_guid=@related_guid) as MinTime
                    into #tmp
                    from result_search
                    where related_guid=@related_guid ");

        if (date > 0)/*條件：時間*/
        {
            sb.Append(@" and get_time >= DateAdd(day,@date, CONVERT(VARCHAR(10) ,GETDATE(),111)) ");
        }

        sb.Append(@"
select count(*) as total from #tmp

select * from (
	select ROW_NUMBER() over (order by score desc) itemNo,#tmp.*
	from #tmp 
)#t where itemNo between @pStart and @pEnd");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@related_guid", related_guid);
        oCmd.Parameters.Add("@date", SqlDbType.Int).Value = date * -1;
        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);

        oda.Fill(ds);
        return ds;
    }

    public DataSet GetWordList(string project_guid, string topic, string blacklist, string source, string pStart, string pEnd,string sortStr)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
SELECT 
direction.name as Topic,
word.related_guid,
word.research_guid,
word.name,
word.blacklist,
word.schedule,
word.analyst_give,
word.similarity
into #tmp
FROM input_research_direction as direction
  left join input_related_word as word on word.research_guid=direction.research_guid
  where project_guid=@project_guid ");

        if (topic != "all")
        {
            sb.Append(@" and direction.research_guid=@topic ");
        }

        if (blacklist != "all")
        {
            sb.Append(@" and word.blacklist=@blacklist ");
        }

        if (source != "all")
        {
            sb.Append(@" and word.analyst_give=@source ");
        }

        // 關鍵字
        if (KeyWord != "")
        {
            sb.Append(@"and (lower(isnull(word.name,'')) like '%" + KeyWord.ToLower() + "%') ");
        }

        sb.Append(@"select count(*) as total from #tmp
select * from (
select ROW_NUMBER() over (order by " + sortStr + @") itemNo,#tmp.*
from #tmp 
)#t where itemNo between @pStart and @pEnd  ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@project_guid", project_guid);
        oCmd.Parameters.AddWithValue("@topic", topic);
        oCmd.Parameters.AddWithValue("@blacklist", blacklist);
        oCmd.Parameters.AddWithValue("@source", source);
        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);

        oda.Fill(ds);
        return ds;
    }

    public DataTable GetArticleByGuid(string article_guid)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from result_article where article_guid=@article_guid");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@article_guid", article_guid);

        oda.Fill(ds);
        return ds;
    }

    public void addWord(SqlConnection oConn, SqlTransaction oTrans, string guid, string research_guid, string name,string name_stem, string blacklist)
    {
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = @"insert into input_related_word (
related_guid,
research_guid,
name,
name_stem,
blacklist,
schedule,
analyst_give,
create_time
) values (
@related_guid,
@research_guid,
@name,
@name_stem,
@blacklist,
'1',
'1',
@create_time
) ";
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        oCmd.Parameters.AddWithValue("@related_guid", guid);
        oCmd.Parameters.AddWithValue("@research_guid", research_guid);
        oCmd.Parameters.AddWithValue("@name", name);
        oCmd.Parameters.AddWithValue("@name_stem", name_stem);
        oCmd.Parameters.AddWithValue("@blacklist", blacklist);
        oCmd.Parameters.AddWithValue("@create_time", DateTime.Now);

        oCmd.Transaction = oTrans;
        oCmd.ExecuteNonQuery();
    }

    public void deleteWord(SqlConnection oConn, SqlTransaction oTrans, string related_guid)
    {
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = @"delete from input_related_word where related_guid=@related_guid ";
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);

        oCmd.Parameters.AddWithValue("@related_guid", related_guid);
        
        oCmd.Transaction = oTrans;
        oCmd.ExecuteNonQuery();
    }

    public DataTable GetWordByGuid(string related_guid)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from input_related_word where related_guid=@related_guid");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@related_guid", related_guid);

        oda.Fill(ds);
        return ds;
    }

    public DataTable CheckWordExist(string project_guid,string name)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from input_related_word where
research_guid in (SELECT research_guid FROM input_research_direction where project_guid=@project_guid) and name=@name ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@project_guid", project_guid);
        oCmd.Parameters.AddWithValue("@name", name);

        oda.Fill(ds);
        return ds;
    }

    public void UpdateWord(SqlConnection oConn, SqlTransaction oTrans, string related_guid, string research_guid, string name, string blacklist, string org_analysis)
    {
        SqlCommand oCmd = oConn.CreateCommand();
        StringBuilder sb = new StringBuilder();

        sb.Append(@"update input_related_word set
research_guid=@research_guid,
name=@name,
blacklist=@blacklist,
update_time=@update_time ");

        if (org_analysis == "3")
            sb.Append(@",analyst_give='2' ");

        sb.Append(@"where related_guid=@related_guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        oCmd.Parameters.AddWithValue("@related_guid", related_guid);
        oCmd.Parameters.AddWithValue("@research_guid", research_guid);
        oCmd.Parameters.AddWithValue("@name", name);
        oCmd.Parameters.AddWithValue("@blacklist", blacklist);
        oCmd.Parameters.AddWithValue("@update_time", DateTime.Now);

        oCmd.Transaction = oTrans;
        oCmd.ExecuteNonQuery();
    }

    public void InsertWordLog(SqlConnection oConn, SqlTransaction oTrans, string pjGuid, string related_guid, string status)
    {
        InsertWordLog(oConn, oTrans, pjGuid, related_guid, status, "", "", "","");
    }

    public void InsertWordLog(SqlConnection oConn, SqlTransaction oTrans, string pjGuid, string related_guid, string status, string orgtopic, string orgname, string orgblacklist, string organalyst_give)
    {
        SqlCommand oCmd = oConn.CreateCommand();
        oCmd.CommandText = @"insert into WordLog
select 
@pjGuid,
related_guid,
research_guid,
name,
blacklist,
schedule,
analyst_give,
name_stem,
score,
q3_score,
silimar_related_word,
similarity,
create_time,
update_time,
@orgtopic,
@orgname,
@orgblacklist,
@organalyst_give,
@status,
getdate()
from input_related_word
where related_guid=@related_guid ";

        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);

        oCmd.Parameters.AddWithValue("@pjGuid", pjGuid);
        oCmd.Parameters.AddWithValue("@related_guid", related_guid);
        oCmd.Parameters.AddWithValue("@status", status);
        oCmd.Parameters.AddWithValue("@orgtopic", orgtopic);
        oCmd.Parameters.AddWithValue("@orgname", orgname);
        oCmd.Parameters.AddWithValue("@orgblacklist", orgblacklist);
        oCmd.Parameters.AddWithValue("@organalyst_give", organalyst_give);

        oCmd.Transaction = oTrans;
        oCmd.ExecuteNonQuery();
    }

    public string GetTopicName(string research_guid)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select name from input_research_direction where research_guid=@research_guid");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@research_guid", research_guid);

        oda.Fill(ds);
        string tmpstr = string.Empty;
        if (ds.Rows.Count > 0)
            tmpstr = ds.Rows[0]["name"].ToString();

        return tmpstr;
    }

    public DataSet GetRecordList(string project_guid,string action,string sday,string eday, string pStart, string pEnd)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"
SELECT a.*,
b.name as WordCategory,
c.name as orgWordCategory
into #tmp
FROM WordLog as a
left join input_research_direction as b on b.research_guid=a.research_guid 
left join input_research_direction as c on c.research_guid=case when a.org_topic <>'' then a.org_topic else (select NEWID()) end 
where a.project_guid=@project_guid ");

        // 關鍵字
        if (KeyWord != "")
            sb.Append(@"and (lower(isnull(a.name,'')+isnull(org_name,'')) like '%" + KeyWord + "%') ");

        if (action != "")
            sb.Append(@"and status=@action ");

        if (sday != "" && eday != "")
            sb.Append(@"and createdate between CONVERT(datetime,@sday) and DATEADD(day,1,CONVERT(datetime,@eday)) ");
        else if (sday != "")
            sb.Append(@"and createdate >= CONVERT(datetime,@sday) ");
        else if (eday != "")
            sb.Append(@"and createdate <= DATEADD(day,1, CONVERT(datetime,@eday)) ");

        sb.Append(@"select count(*) as total from #tmp
select * from (
select ROW_NUMBER() over (order by createdate desc) itemNo,#tmp.*
from #tmp 
)#t where itemNo between @pStart and @pEnd  ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();

        oCmd.Parameters.AddWithValue("@project_guid", project_guid);
        oCmd.Parameters.AddWithValue("@action", action);
        oCmd.Parameters.AddWithValue("@sday", sday);
        oCmd.Parameters.AddWithValue("@eday", eday);
        oCmd.Parameters.AddWithValue("@pStart", pStart);
        oCmd.Parameters.AddWithValue("@pEnd", pEnd);

        oda.Fill(ds);
        return ds;
    }

    public void UndoWord(string id)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        oCmd.CommandText = @"
declare @mode nvarchar(20)
select @mode=status from WordLog where id=@id

declare @wGuid nvarchar(50)
select @wGuid=related_guid from WordLog where id=@id

if @mode='add'
	begin
		delete from input_related_word where related_guid=@wGuid
	end
else if @mode='update'
	begin
		delete from input_related_word where related_guid=@wGuid
		insert into input_related_word
		select 
		related_guid,
		org_topic,
		org_name,
		org_blacklist,
		schedule,
		org_analyst_give,
		name_stem,
		score,
		q3_score,
		silimar_related_word,
		similarity,
		create_time,
		update_time
		from WordLog
		where id=@id 
	end
else if @mode='delete'
	begin
		insert into input_related_word
		select 
		related_guid,
		research_guid,
		name,
		blacklist,
		schedule,
		analyst_give,
		name_stem,
		score,
		q3_score,
		silimar_related_word,
		similarity,
		create_time,
		update_time
		from WordLog
		where id=@id 
	end

	
delete from WordLog where related_guid=@wGuid and createdate >= (select createdate from WordLog where id=@id)
";
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);

        oCmd.Parameters.AddWithValue("@id", id);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }


    public DataTable GetProjectGuid_By_ArticleGuid(string article_guid)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select * from result_article where article_guid=@article_guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@article_guid", article_guid);

        oda.Fill(ds);
        return ds;
    }

    public void RankingFeedBack(string atGuid , int score, string desc)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        oCmd.CommandText = @"
update result_article set
star_rating=@score,
user_feedback=@desc
where article_guid=@atGuid
";
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);

        oCmd.Parameters.AddWithValue("@atGuid", atGuid);
        oCmd.Parameters.AddWithValue("@score", score);
        oCmd.Parameters.AddWithValue("@desc", desc);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }

    public DataTable GetArticleFeedback(string article_guid)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select star_rating,user_feedback from result_article where article_guid=@article_guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@article_guid", article_guid);

        oda.Fill(ds);
        return ds;
    }

    public void ReOpenProjcet(string PjGuid)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        oCmd.CommandText = @"
update input_project set
stop_time=NULL,
update_time=@gettime,
status=40
where project_guid=@PjGuid
";
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);

        oCmd.Parameters.AddWithValue("@PjGuid", PjGuid);
        oCmd.Parameters.AddWithValue("@gettime", DateTime.Now);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }
}