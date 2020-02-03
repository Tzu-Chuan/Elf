using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.Xml;

/// <summary>
/// Dao_ProjectMaintain 的摘要描述
/// </summary>
public class Dao_ProjectMaintain
{
    /*============================================================================*/
    /*list管理者*/
    public XmlDocument getManagerList(string q, string orderField, string orderBy, int recStart, int recCount)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();

            /*===cmd*/
            sb.Append(@"
                select * from 
                (
                    Select * 
                    ,role_enname = (select dc_item_ename from [sys_data_category] where (dc_group_id = '1') and (dc_item_id=tb.role_id))
                    From [sys_manager_right] tb
                ) as a
                WHERE (1=1)
                ");

            /*===關鍵字*/
            if (q != "")
            {
                sb.Append(@" and (lower(
                                isnull([role_enname],'')
                                +isnull([empno],'')
                                +isnull([empname],'')
                                +isnull([deptid],'')
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
            XmlDocument xDoc = DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn(), recStart, recCount);
            return xDoc;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }



    /*============================================================================*/
    /*功能：新增管理人員*/

    public int exec_empadd(string role_id, string empno, string empname, string orgcd, string deptid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            string sqlStr =
@"insert into sys_manager_right([manager_guid], [role_id], [empno], [empname], [orgcd], [deptid], [create_time], [create_empno], [create_empname])
values(@manager_guid, @role_id, @empno, @empname, @orgcd, @deptid, @create_time, @create_empno, @create_empname)
";

            oCmd.CommandText = sqlStr;
            oCmd.Connection = DbUtil.GetConn();
            oCmd.Parameters.Add("@manager_guid", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
            oCmd.Parameters.Add("@role_id", SqlDbType.VarChar).Value = role_id;
            oCmd.Parameters.Add("@empno", SqlDbType.VarChar).Value = empno;
            oCmd.Parameters.Add("@empname", SqlDbType.NVarChar).Value = empname;
            oCmd.Parameters.Add("@orgcd", SqlDbType.VarChar).Value = orgcd;
            oCmd.Parameters.Add("@deptid", SqlDbType.VarChar).Value = deptid;
            oCmd.Parameters.Add("@create_time", SqlDbType.DateTime).Value = DateTime.Now;
            oCmd.Parameters.Add("@create_empno", SqlDbType.VarChar).Value = SSOUtil.GetCurrentUser().工號;
            oCmd.Parameters.Add("@create_empname", SqlDbType.NVarChar).Value = SSOUtil.GetCurrentUser().姓名;

            int count = DbUtil.ExecCmdNoResult(oCmd);
            return count;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*============================================================================*/
    /*功能：刪除管理人員*/

    public int exec_empdel(string empGuid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            string sqlStr =
@"delete from sys_manager_right
where [manager_guid] = @manager_guid
";

            oCmd.CommandText = sqlStr;
            oCmd.Connection = DbUtil.GetConn();
            oCmd.Parameters.Add("@manager_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(empGuid);

            int count = DbUtil.ExecCmdNoResult(oCmd);
            return count;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }



    /*============================================================================*/
    /*抓取文章分頁*/
    /*https://docs.microsoft.com/en-us/sql/t-sql/xml/value-method-xml-data-type*/

    public XmlDocument getArticleList(string pKeyword, string orderField, string orderBy, int recStart, int recCount
        , string pjGuid, int date0, string viewMode, string researchGuid, string typeId, string typeGuid)
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




            /*===固定來源-全部*/
            if (typeId == "1")
            {
                sb.AppendFormat(@"
                                    select * from 
                                    (  
                                        select
                                        [article_guid], [project_guid], [title], [get_time], [full_text]   
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




////////////                sb.AppendFormat(@"
////////////select * from 
////////////(
////////////	select row_number() over(order by score) as rownumber, * from 
////////////    (  
////////////        select
////////////        [article_guid], [project_guid], [title], [get_time], [full_text]   
////////////		,[desc] = paragraph_xml.value('(/paragraph/key[@name=sql:variable(''@researchGuid'')])[1]', 'varchar(max)')
////////////        ,[score] =  category_score_xml.value('(/score/key[@name=sql:variable(''@researchGuid'')])[1]', 'float')from [result_article]
////////////        where [project_guid]=@project_guid
////////////    ) as tb1 
////////////    where [score] > 0
////////////    {0}
////////////) as tb2
////////////where rownumber between {1} and {2}    
////////////", str_timeCondition, recStart, recStart + recCount);



                oCmd.Parameters.AddWithValue("@project_guid", pjGuid);/*條件：專案guid*/
                oCmd.Parameters.AddWithValue("@researchGuid", researchGuid);/*條件：研究方向guid*/

                if (date0 > 0)
                {
                    sb.Append(@" and [get_time] >= DateAdd(day,@date0, CONVERT(VARCHAR(10) ,GETDATE(),111) )");
                    oCmd.Parameters.Add("@date0", SqlDbType.Int).Value = date0 * -1;/*條件：時間*/
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
                        ,[score] =  category_score_xml.value('(/score/key[@name=sql:variable(''@researchGuid'')])[1]', 'float')
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

                if (date0 > 0)
                {
                    sb.Append(@" and [get_time] >= DateAdd(day,@date0, CONVERT(VARCHAR(10) ,GETDATE(),111) )");
                    oCmd.Parameters.Add("@date0", SqlDbType.Int).Value = date0 * -1;/*條件：時間*/
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

}