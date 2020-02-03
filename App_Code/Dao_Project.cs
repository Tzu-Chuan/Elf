using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.Xml;

/// <summary>
/// Dao_Project 的摘要描述
/// </summary>
public class Dao_Project
{
    /*============================================================================*/
    /*default.aspx*/
    /*list專案*/
    public XmlDocument getProjectList(string q, string orderField, string orderBy, int recStart, int recCount,string empno)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();

            /*===cmd*/
            sb.Append(@"select project_guid into #tmp
from input_project
left join pj_member on PM_ProjectGuid=project_guid
where PM_Empno=@empno ");

            sb.Append(@"
                select * from 
                (
                    Select * 
                    ,status_en_name = (select status_en_name from [pj_status] where status_id=tb.status)
                    ,owner = (select top 1 empname+'('+empno+')' from [sys_project_right] where project_guid=tb.project_guid and role_id='owner')
		            ,empno = (select top 1 empno from sys_project_right where project_guid=tb.project_guid and role_id='owner') ");

            if (empno == "admin")
                sb.Append(@",compstatus = 1 ");
            else
                sb.Append(@",compstatus = (select count(*) from sys_manager_right where role_id='pj_mgr' and empno=@empno and (select sys_project_right.empno from sys_project_right where project_guid=tb.project_guid)=@empno) ");

            sb.Append(@" From [input_project] tb
                ) as a
                WHERE (1=1)
                ");

            if (empno != "admin")
            {
                sb.Append(@" and empno=@empno");
                sb.Append(@" or project_guid in (select * from #tmp)");
            }

            /*===關鍵字*/
            if (q != "")
            {
                sb.Append(@" and (lower(
                                isnull([technology],'')
                                +isnull([tn_related_word],'')
                                +isnull([status_en_name],'')
                                +isnull([owner],'')
                                ) like @q)
                ");
                oCmd.Parameters.AddWithValue("@q", "%" + q + "%");/*關鍵字*/
            }

            /*===排序*/
            if (orderField != "")
            {
                //sb.AppendFormat(" ORDER BY {0} {1};", pIndexField, pIndexOrder);
                sb.AppendFormat(" ORDER BY {0} {1};", orderField, orderBy);
            }

            /*===exec & return*/
            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.AddWithValue("@empno", empno);  /*工號*/
            XmlDocument xDoc = DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn(), recStart, recCount);
            return xDoc;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }



    /*============================================================================*/
    /*articleList.aspx*/
    /*抓取文章分頁*/
    /*https://docs.microsoft.com/en-us/sql/t-sql/xml/value-method-xml-data-type*/

    public XmlDocument getArticleList(string pKeyword, string orderField, string orderBy, int recStart, int recCount
        , string pjGuid, int date0, string viewMode, string researchGuid, string myTag, string typeId, string typeGuid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();
            //////////string str_timeCondition = "";

            //////////if (date0 > 0)/*有時間條件時*/
            //////////{
            //////////    str_timeCondition = " and [get_time] >= DateAdd(day,@date0, CONVERT(VARCHAR(10) ,GETDATE(),111) )";
            //////////    oCmd.Parameters.Add("@date0", SqlDbType.Int).Value = date0 * -1;/*條件：時間*/
            //////////}




            /*===固定來源-全部網站*/
            if (typeId == "1")
            {
                sb.AppendFormat(@"
select * from 
(  
select
[article_guid]
, [project_guid]
, [title]
, [get_time]
, [full_text]   
,[desc] = paragraph_xml.value('(/paragraph/key[@name=sql:variable(''@researchGuid'')])[1]', 'varchar(max)')
,[score] =  category_score_xml.value('(/score/key[@name=sql:variable(''@researchGuid'')])[1]', 'float')
 from [result_article]
) as tb 
where (1=1)
and [project_guid]=@project_guid
and [score] > 0
");

                //DB寫法
                //,[desc] = paragraph_xml.value('(/paragraph/key[@name=''all''])[1]', 'varchar(max)')
                //,[score] =  category_score_xml.value('(/score/key[@name=''all''])[1]', 'float')


                oCmd.Parameters.AddWithValue("@project_guid", pjGuid);/*條件：專案guid*/
                oCmd.Parameters.AddWithValue("@researchGuid", researchGuid);/*條件：研究方向guid*/

                
                if (date0 > 0)/*條件：時間*/
                {
                    sb.Append(@" and [get_time] >= DateAdd(day,@date0, CONVERT(VARCHAR(10) ,GETDATE(),111) )");
                    oCmd.Parameters.Add("@date0", SqlDbType.Int).Value = date0 * -1;
                }

                if (myTag != "all")/*條件：有myTag條件時*/
                {
                    sb.Append(@" and [article_guid] in 
                                    (select [article_guid] from [sys_tagrec]
                                    where [project_guid] = @project_guid 
                                    and [tagtype_guid]=@tagtype_guid)
                              ");
                    oCmd.Parameters.Add("@tagtype_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(myTag);
                    /////oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
                }

                if (orderField == "date") { sb.Append(@" order by [get_time] desc;"); }
                else if (orderField == "score") { sb.Append(@" order by [score] desc;"); }
                else { sb.Append(@" order by [score] desc;"); }
            }
            /*===固定來源-依網站分類*/
            else if (typeId == "2")
            {
                sb.AppendFormat(@"
                    select * from 
                    (  
                        select
                        [article_guid], [project_guid], [title], [get_time], [website_guid]   
                        ,[desc] = paragraph_xml.value('(/paragraph/key[@name=sql:variable(''@researchGuid'')])[1]', 'varchar(max)')
                        ,[score] =  category_score_xml.value('(/score/key[@name=''" + @researchGuid + @"''])[1]', 'float')
                        from [result_article]
                    ) as tb 
                    where (1=1)
                    and [project_guid]=@project_guid
                    and [website_guid]=@website_guid                    
                    and [score] > 0
                    ");
                oCmd.Parameters.AddWithValue("@project_guid", pjGuid);/*條件：專案guid*/
                oCmd.Parameters.AddWithValue("@website_guid", typeGuid);/*條件：網站來源guid*/
                oCmd.Parameters.AddWithValue("@researchGuid", researchGuid);/*條件：研究方向guid*/

                if (date0 > 0)/*條件：時間*/
                {
                    sb.Append(@" and [get_time] >= DateAdd(day,@date0, CONVERT(VARCHAR(10) ,GETDATE(),111) )");
                    oCmd.Parameters.Add("@date0", SqlDbType.Int).Value = date0 * -1;
                }

                if (myTag != "all")/*條件：有myTag條件時*/
                {
                    sb.Append(@" and [article_guid] in 
                                    (select [article_guid] from [sys_tagrec]
                                    where [project_guid] = @project_guid 
                                    and [tagtype_guid]=@tagtype_guid)
                              ");
                    oCmd.Parameters.Add("@tagtype_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(myTag);
                    /////oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
                }

                if (orderField == "date") { sb.Append(@" order by [get_time] desc;"); }
                else if (orderField == "score") { sb.Append(@" order by [score] desc;"); }
                else { sb.Append(@" order by [score] desc;"); }
            }
            /*===askCom*/
            else if (typeId == "3")
            {
                sb.AppendFormat(@"
                    select [title], [url], [describe_text], [score], [get_time]
                    from [result_search]
                    where (1=1)
                    and [related_guid]=@related_guid
                    ");
                oCmd.Parameters.AddWithValue("@related_guid", typeGuid);/*條件：關連詞guid*/

                if (date0 > 0)
                {
                    sb.Append(@" and [get_time] >= DateAdd(day,@date0, CONVERT(VARCHAR(10) ,GETDATE(),111) )");
                    oCmd.Parameters.Add("@date0", SqlDbType.Int).Value = date0 * -1;/*條件：時間*/
                }

                sb.Append(@" order by [score] desc;");
            }
            /*===askComManage*/
            /*說明：不用時間條件*/
            else if (typeId == "4")
            {
                sb.AppendFormat(@"
                    select [title], [url], [describe_text], [score], [get_time]
                    from [result_search]
                    where (1=1)
                    and [related_guid]=@related_guid
                    ");
                oCmd.Parameters.AddWithValue("@related_guid", typeGuid);/*條件：關連詞guid*/

                //////////if (date0 > 0)
                //////////{
                //////////    sb.Append(@" and [get_time] >= DateAdd(day,@date0, CONVERT(VARCHAR(10) ,GETDATE(),111) )");
                //////////    oCmd.Parameters.Add("@date0", SqlDbType.Int).Value = date0 * -1;
                //////////}

                sb.Append(@" order by [score] desc;");
            }



            /*===exec & return*/
            oCmd.CommandText = sb.ToString();
            XmlDocument xDoc = DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn(), recStart, recCount);
            return xDoc;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }



    /*============================================================================*/
    /*projectDetail.aspx*/
    /*抓取專案內容-全部網站 by pjguid*/
    public XmlDocument getProject(string pjGuid, int date0, string researchGuid, string myTag)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();
            StringBuilder sbtimeCondition = new StringBuilder();

            if (date0 > 0)/*有時間條件時*/
            {
                sbtimeCondition.Append(@" and [get_time] >= DateAdd(day,@date0, CONVERT(VARCHAR(10) ,GETDATE(),111))" );
                oCmd.Parameters.Add("@date0", SqlDbType.Int).Value = date0 * -1;
            }
            if (myTag != "all")/*有myTag條件時*/
            {
                sbtimeCondition.Append(@" and [article_guid] in (select [article_guid] from [sys_tagrec] 
                                          where [project_guid] = @project_guid 
                                          and [tagtype_guid]=@tagtype_guid)
                                        ");
                oCmd.Parameters.Add("@tagtype_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(myTag);
                //////////oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            }


            sb.AppendFormat(@"
Select *
,[result_count] = (
                  select count(*)
                  from [result_article] 
                  where project_guid = tb.project_guid
                  and category_score_xml.value('(/score/key[@name=sql:variable(''@researchGuid'')])[1]', 'float') > 0
                  {0}
                  )--//文章筆數(分數>0)
From [input_project] tb
where [project_guid] = @project_guid
;", sbtimeCondition.ToString() );

           


            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            oCmd.Parameters.AddWithValue("@researchGuid", researchGuid);/*條件：研究方向guid*/


            /*result*/
            XmlDocument xDoc = DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn());
            return xDoc;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*抓取監測網站-依網站分類 by pjguid*/
    public XmlDocument getWebsite(string pjGuid, int date0, string researchGuid, string myTag)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();
            StringBuilder sbtimeCondition = new StringBuilder();

            if (date0 > 0)/*有時間條件時*/
            {
                sbtimeCondition.Append(@" and [get_time] >= DateAdd(day,@date0, CONVERT(VARCHAR(10) ,GETDATE(),111))");
                oCmd.Parameters.Add("@date0", SqlDbType.Int).Value = date0 * -1;/*條件：時間*/
            }
            if (myTag != "all")/*有myTag條件時*/
            {
                sbtimeCondition.Append(@" and [article_guid] in (select [article_guid] from [sys_tagrec] 
                                          where [project_guid] = @project_guid 
                                          and [tagtype_guid]=@tagtype_guid)
                                        ");
                oCmd.Parameters.Add("@tagtype_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(myTag);
                //////////oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            }


            sb.AppendFormat(@"
Select * 
,[result_count] = (
                  select count(*)
                  from [result_article] 
                  where website_guid = tb.website_guid
                  and category_score_xml.value('(/score/key[@name=sql:variable(''@researchGuid'')])[1]', 'float') > 0
                  {0}
                  )--//文章筆數(分數>0)
From [input_website] tb
where [project_guid] = @project_guid
;", sbtimeCondition.ToString());

            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            oCmd.Parameters.AddWithValue("@researchGuid", researchGuid);/*條件：研究方向guid*/

            /*result*/
            XmlDocument xDoc = DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn());
            return xDoc;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*抓取研究方向 by pjguid*/
    public XmlDocument getDirection(string pjGuid)
    {
        try
        {
            string sqlStr = @"
Select * From [input_research_direction] tb
where [project_guid] = @project_guid
order by [research_sn] asc--//研究方向的新增時的順序
;";

            SqlCommand oCmd = new SqlCommand();
            oCmd.CommandText = sqlStr;
            oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);

            /*result*/
            XmlDocument xDoc = DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn());
            return xDoc;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*抓取搜尋ask.com結果的關連詞(排除黑名單、未審核) by pjguid*/
    /*說明1：排除黑名單、未審核*/
    /*說明2：排序為有排程在上、依關連詞a-z排*/
    public XmlDocument getAskCom(string pjGuid, int date0)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();
            StringBuilder sbtimeCondition = new StringBuilder();

            if (date0 > 0)/*有時間條件時*/
            {
                sbtimeCondition.Append(@" and [get_time] >= DateAdd(day,@date0, CONVERT(VARCHAR(10) ,GETDATE(),111))");
                oCmd.Parameters.Add("@date0", SqlDbType.Int).Value = date0 * -1;/*條件：時間*/
            }

            sb.AppendFormat(@"
            select * from 
            (  
                Select distinct a.[related_guid] --//關連詞id
                , a.[related_sn] --//關連詞新增時順序
                , a.[name] as related_name --//關連詞名稱
                , a.[blacklist] --//關連詞狀態
                , a.[schedule] --//是否納入每日排程
                , a.[analyst_give]--//關連詞來源
                , b.[research_guid]--//研究方向id
                , b.[name] as research_name --//研究方向名稱
                , b.[research_sn]  --//研究方向新增時順序
                , [result_count] = (
                                    select count(*) from [result_search] where [related_guid] = a.[related_guid]
                                    {0}
                                   ) --//各關連詞結果筆數
                From [input_related_word] a
                inner join [input_research_direction] b on (b.research_guid = a.research_guid) and (b.project_guid = @project_guid)
                where a.[blacklist] !=1--//排除黑名單(0=白,1=黑,2=候選詞)
                      and a.[analyst_give] !=2--//排除未審核(0=excel匯入時,1=使用者新增,2=系統產生未審核,3=系統產生已審核)  
            ) as tb
            --order by [research_sn] asc, [related_sn] asc--//新增時的順序(研究方向+關連詞)
            order by [schedule] desc, [related_name] asc--//改為有排程+關連詞. 2018/4/12,asam
            ;", sbtimeCondition.ToString());

            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);






            //////            string sqlStr = "";
            //////            if (date0 == 0)
            //////            {
            //////                sqlStr = @"
            //////            select * from 
            //////            (  
            //////                Select distinct a.[related_guid] --//關連詞id
            //////                , a.[related_sn] --//關連詞新增時順序
            //////                , a.[name] as related_name --//關連詞名稱
            //////                , a.[blacklist] --//黑,白,中立名單關連詞
            //////                , a.[schedule] --//是否納入每日排程
            //////                , a.[analyst_give]--//0為分析師第1階段給的 , 1為使用者新增 , 2透過分析產生的新相關詞-未審核 , 3透過分析產生的新相關詞-已審核*/
            //////                , b.[research_guid]--//研究方向id
            //////                , b.[name] as research_name --//研究方向名稱
            //////                , b.[research_sn]  --//研究方向新增時順序
            //////                , 'result_count' = (select count(*) from [result_search] where [related_guid] = a.[related_guid]) --//各關連詞結果筆數
            //////                From [input_related_word] a
            //////                inner join [input_research_direction] b on (b.research_guid = a.research_guid) and (b.project_guid = @pjGuid)
            //////                where a.[blacklist] !=1--//排除黑名單關連詞 
            //////            ) as tb
            //////            order by [research_sn] asc, [related_sn] asc--//新增時的順序(研究方向+關連詞)
            //////            ;";
            //////            }
            //////            else if (date0 > 0)/*有時間條件時*/
            //////            {
            //////                sqlStr = @"
            //////            select * from 
            //////            (  
            //////                Select distinct a.[related_guid] --//關連詞id
            //////                , a.[related_sn] --//關連詞新增時順序
            //////                , a.[name] as related_name --//關連詞名稱
            //////                , a.[blacklist] --//黑,白,中立名單關連詞
            //////                , a.[schedule] --//是否納入每日排程
            //////                , a.[analyst_give]--//0為分析師第1階段給的 , 1為使用者新增 , 2透過分析產生的新相關詞-未審核 , 3透過分析產生的新相關詞-已審核*/
            //////                , b.[research_guid]--//研究方向id
            //////                , b.[name] as research_name --//研究方向名稱
            //////                , b.[research_sn]  --//研究方向新增時順序
            //////                , 'result_count' = (
            //////                                    select count(*) from [result_search] where [related_guid] = a.[related_guid]
            //////                                    and [get_time] >= DateAdd(day,@date0, CONVERT(VARCHAR(10) ,GETDATE(),111) )
            //////                                   ) --//各關連詞結果筆數
            //////                From [input_related_word] a
            //////                inner join [input_research_direction] b on (b.research_guid = a.research_guid) and (b.project_guid = @pjGuid)
            //////                where a.[blacklist] !=1--//排除黑名單關連詞 
            //////            ) as tb
            //////            order by [research_sn] asc, [related_sn] asc--//新增時的順序(研究方向+關連詞)
            //////            ;";
            //////            }


            //////            SqlCommand oCmd = new SqlCommand();
            //////            oCmd.CommandText = sqlStr;
            //////            oCmd.Parameters.Add("@pjGuid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            //////            oCmd.Parameters.Add("@date0", SqlDbType.Int).Value = date0 * -1;/*條件：時間*/

            /*result*/
            XmlDocument xDoc = DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn());
            return xDoc;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*============================================================================*/
    /*抓取文章的tab */
    public XmlDocument getArticleTagSelect(string pjid, string arcid, string empno)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"
Select a.*
, 'selected' = case when b.tagrec_sn is not null then '1'
else '0' end
From [sys_tagtype] a
left join 
(
select *
from [sys_tagrec]
where [project_guid] = @project_guid
and [article_guid]=@article_guid
and [tagtype_empno]=@tagtype_empno
)as b
on a.[tagtype_guid] = b.[tagtype_guid]
where a.[tagtype_empno] = @tagtype_empno
order by a.tagtype_sn asc
;");

            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjid);
            oCmd.Parameters.Add("@article_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(arcid);
            oCmd.Parameters.Add("@tagtype_empno", SqlDbType.VarChar).Value = empno;

            /*result*/
            XmlDocument xDoc = DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn());
            return xDoc;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*============================================================================*/
    /*save tag*/
    public int exec_tagSelectSave(string pjid, string arcid, string empno, string[] cbSelectedArray)
    {
        try
        {
            SqlCommand[] oCmd = new SqlCommand[]
            {
                  deleteCmd_tag(pjid, arcid, empno, cbSelectedArray)
                  ,addCmd_tag(pjid, arcid, empno, cbSelectedArray)
             };
            int resultCount = DbUtil.ExecCmdTranNoResult(oCmd, DbUtil.GetConn());
            return resultCount;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }


    /*1,刪除該文章原來的tag*/
    SqlCommand deleteCmd_tag(string pjid, string arcid, string empno, string[] cbSelectedArray)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(
@"delete from [sys_tagrec]
where [project_guid] = @project_guid
and [article_guid]=@article_guid
and [tagtype_empno]=@tagtype_empno
;");
            oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjid);
            oCmd.Parameters.Add("@article_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(arcid);
            oCmd.Parameters.Add("@tagtype_empno", SqlDbType.VarChar).Value = empno;
            oCmd.CommandText = sb.ToString();
            return oCmd;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*1,新增tag*/
    SqlCommand addCmd_tag(string pjid, string arcid, string empno, string[] cbSelectedArray)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < cbSelectedArray.Length; i++)
            {
                if (cbSelectedArray[i] != "")
                {
                    sb.AppendFormat(
    @"insert into [sys_tagrec]([project_guid], [article_guid], [tagtype_guid], [tagtype_empno], [create_time])
values(@project_guid, @article_guid, @tagtype_guid{0}, @tagtype_empno, getDate())
;", i);
                    oCmd.Parameters.Add("@tagtype_guid" + i, SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(cbSelectedArray[i]);
                }
            }
            oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjid);
            oCmd.Parameters.Add("@article_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(arcid);
            oCmd.Parameters.Add("@tagtype_empno", SqlDbType.VarChar).Value = empno;

            oCmd.CommandText = sb.ToString();
            return oCmd;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*============================================================================*/
    /*抓取我的tab選單*/
    public XmlDocument getMyTagMaintain(string empno)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"
Select *
From [sys_tagtype]
where [tagtype_empno] = @tagtype_empno
order by tagtype_sn asc
;");

            oCmd.CommandText = sb.ToString();
            oCmd.Parameters.Add("@tagtype_empno", SqlDbType.VarChar).Value = empno;

            /*result*/
            XmlDocument xDoc = DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn());
            return xDoc;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*抓取我的tab count*/
    public int getMyTagCount(string empno)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"
Select count(*)
From [sys_tagtype]
where [tagtype_empno] = @tagtype_empno
;");

            oCmd.CommandText = sb.ToString();
            oCmd.Connection = DbUtil.GetConn();
            oCmd.Parameters.Add("@tagtype_empno", SqlDbType.VarChar).Value = empno;

            /*result*/
            int resultCount = int.Parse(DbUtil.ExecCmdGetResult(oCmd).ToString());
            return resultCount;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }


    /*新增我的tag*/
    public int execMyTagAdd(string newTagName, string empno)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"
insert into [sys_tagtype](tagtype_guid,[tagtype_name], [tagtype_empno], [create_time])
values(@tagtype_guid,@tagtype_name, @tagtype_empno, getDate())
; ");

            oCmd.CommandText = sb.ToString();
            oCmd.Connection = DbUtil.GetConn();
            oCmd.Parameters.Add("@tagtype_guid", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
            oCmd.Parameters.Add("@tagtype_name", SqlDbType.VarChar).Value = newTagName;
            oCmd.Parameters.Add("@tagtype_empno", SqlDbType.VarChar).Value = empno;

            /*result*/
            int resultCount = DbUtil.ExecCmdNoResult(oCmd);
            return resultCount;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*刪除我的tag*/
    public int execMyTagDelete(string tagid, string empno)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"
delete from[sys_tagtype]
where [tagtype_guid] = @tagtype_guid
and [tagtype_empno] = @tagtype_empno
; ");

            oCmd.CommandText = sb.ToString();
            oCmd.Connection = DbUtil.GetConn();
            oCmd.Parameters.Add("@tagtype_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(tagid);
            oCmd.Parameters.Add("@tagtype_empno", SqlDbType.VarChar).Value = empno;

            /*result*/
            int resultCount = DbUtil.ExecCmdNoResult(oCmd);
            return resultCount;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }


    /*============================================================================*/
    /*explain: utility - get where in param */
    /*============================================================================*/
    public string getWhereInParam(ref SqlCommand oCmd, string[] paramArray, string paramName)
    {
        string paramStr = "";
        for (int i = 0; i < paramArray.Length; i++)
        {
            paramStr = paramStr + "@" + paramName + i + ",";
            oCmd.Parameters.AddWithValue("@" + paramName + i, Convert.ToString(paramArray[i].Trim()));
        }
        return paramStr.Substring(0, paramStr.Length - 1);
    }
    

    public DataTable getProjectTopic(string project_guid)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = DbUtil.GetConn();
        StringBuilder sb = new StringBuilder();

        sb.Append(@"Select * from input_research_direction where project_guid=@project_guid ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@project_guid", project_guid);

        oda.Fill(ds);
        return ds;
    }


    public DataTable getMyTag(string project_guid,string empno)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = DbUtil.GetConn();
        StringBuilder sb = new StringBuilder();

        sb.Append(@"  Select tp.tagtype_guid,tp.tagtype_name from sys_tagrec as rec
  left join sys_tagtype tp on tp.tagtype_guid=rec.tagtype_guid
  where rec.project_guid=@project_guid and rec.tagtype_empno=@empno
  group by tp.tagtype_guid,tp.tagtype_name ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@project_guid", project_guid);
        oCmd.Parameters.AddWithValue("@empno", empno);

        oda.Fill(ds);
        return ds;
    }

    public DataTable getDashBoardTechItemList()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = DbUtil.GetConn();
        StringBuilder sb = new StringBuilder();

        sb.Append(@"select technology as TechItem into #tmp from input_project group by technology

select project_guid,TechItem,create_time from #tmp
left join input_project on technology=TechItem and create_time=(select max(create_time) from input_project where technology=TechItem)
order by create_time desc


drop table #tmp ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();
        

        oda.Fill(ds);
        return ds;
    }
}