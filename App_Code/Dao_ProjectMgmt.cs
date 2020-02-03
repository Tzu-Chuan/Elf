using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.Xml;

/// <summary>
/// Dao_ProjectMgmt 的摘要描述
/// </summary>
public class Dao_ProjectMgmt
{
    /*============================================================================*/
    /*list專案*/
    public XmlDocument getProjectList(string q, string orderField, string orderBy, int recStart, int recCount, string empno)
    {
        return new Dao_Project().getProjectList(q, orderField, orderBy, recStart, recCount, empno);
    }


    /*============================================================================*/
    /*功能：匯入excel*/

    /*執行匯入*/
    public int exec_project_add(XmlDocument xdoc, string optsite)
    {
        string project_guid = "";
        try
        {
            project_guid = xdoc.DocumentElement.GetAttribute("xx_project_guid");

            SqlCommand[] oCmd = new SqlCommand[]
            {
                  insertCmd_right(project_guid)
                  ,insertCmd_project(xdoc, project_guid)
                  ,insertCmd_website(project_guid, optsite)
                  ,insertCmd_researchDirection(xdoc, project_guid)
                  ,insertCmd_relatedWord(xdoc, project_guid)
             };
            int resultCount = DbUtil.ExecCmdTranNoResult(oCmd, DbUtil.GetConn());
            return resultCount;
        }
        catch (Exception ex)
        {
            /////throw new Exception(CommonUtil.GetCurrLocationMsg(ex)

            XmlNodeList xlist = xdoc.SelectNodes("/*/*[@xx_item_explain='__研究方向']/*");
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex) + "relatedWordCount=" + xlist.Count);
        }
    }


    ////////    public int insertExcelData1(XmlDocument xml)
    ////////    {
    ////////        try
    ////////        {
    ////////            string __technology = xml.SelectSingleNode("/*/rec[@name='技術項目']") == null ? "" : xml.SelectSingleNode("/*/rec[@name='技術項目']").ChildNodes[0].InnerText;
    ////////            string __tn_related_word = xml.SelectSingleNode("/*/rec[@name='關鍵字']") == null ? "" : xml.SelectSingleNode("/*/rec[@name='關鍵字']").ChildNodes[0].InnerText;

    ////////            string sqlStr =
    ////////@"insert into input_project([project_guid], [technology], [tn_related_word], [status], [create_time], [start_time], [stop_time])
    ////////values(@project_guid, @technology, @tn_related_word, @status, @create_time, @start_time, @stop_time);
    ////////";
    ////////            SqlCommand oCmd = new SqlCommand();
    ////////            oCmd.CommandText = sqlStr;
    ////////            oCmd.Connection = DbUtil.GetConn();
    ////////            oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
    ////////            oCmd.Parameters.Add("@technology", SqlDbType.VarChar).Value = __technology;
    ////////            oCmd.Parameters.Add("@tn_related_word", SqlDbType.VarChar).Value = __tn_related_word;
    ////////            oCmd.Parameters.Add("@status", SqlDbType.Int).Value = 1;
    ////////            oCmd.Parameters.Add("@create_time", SqlDbType.DateTime).Value = DateTime.Now;
    ////////            oCmd.Parameters.Add("@start_time", SqlDbType.DateTime).Value = DBNull.Value;
    ////////            oCmd.Parameters.Add("@stop_time", SqlDbType.DateTime).Value = DBNull.Value;
    ////////            /*===exec & return*/
    ////////            int resultCount = DbUtil.ExecCmdNoResult(oCmd);
    ////////            return resultCount;

    ////////        }
    ////////        catch (Exception ex)
    ////////        {
    ////////            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
    ////////        }
    ////////    }
   
    
    
    
    /*1,新增_權限*/
    SqlCommand insertCmd_right(string pjGuid)
    {
        try
        {
            List<rightObj> rightObjs = new List<rightObj>();
            rightObjs.Add(
                new rightObj
                {
                    role_id = "owner",
                    empno = SSOUtil.GetCurrentUser().工號,
                    empname = SSOUtil.GetCurrentUser().姓名,
                    orgcd = SSOUtil.GetCurrentUser().單位代碼,
                    deptid = SSOUtil.GetCurrentUser().部門代碼
                }
            );/*專案負責人權限*/

            //////rightObjs.Add(
            //////    new rightObj
            //////    {
            //////        role_id = "alliek",
            //////        empno = "",
            //////        empname = "",
            //////        orgcd = "",
            //////        deptid = ""
            //////    }
            //////);/*讀取權限：全iek人員*/

            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < rightObjs.Count; i++)
            {
                sb.AppendFormat(
    @"insert into sys_project_right([right_guid], [project_guid], [role_id], [empno], [empname], [orgcd], [deptid], [create_time], [create_empno], [create_empname])
values(@right_guid{0}, @project_guid{0}, @role_id{0}, @empno{0}, @empname{0}, @orgcd{0}, @deptid{0}, @create_time{0}, @create_empno{0}, @create_empname{0});", i
                );
                oCmd.Parameters.Add("@right_guid" + i, SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                oCmd.Parameters.Add("@project_guid" + i, SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
                oCmd.Parameters.Add("@role_id" + i, SqlDbType.VarChar).Value = rightObjs[i].role_id;
                oCmd.Parameters.Add("@empno" + i, SqlDbType.VarChar).Value = rightObjs[i].empno;
                oCmd.Parameters.Add("@empname" + i, SqlDbType.NVarChar).Value = rightObjs[i].empname;
                oCmd.Parameters.Add("@orgcd" + i, SqlDbType.Char).Value = rightObjs[i].orgcd;
                oCmd.Parameters.Add("@deptid" + i, SqlDbType.VarChar).Value = rightObjs[i].deptid;
                oCmd.Parameters.Add("@create_time" + i, SqlDbType.DateTime).Value = DateTime.Now;
                oCmd.Parameters.Add("@create_empno" + i, SqlDbType.VarChar).Value = SSOUtil.GetCurrentUser().工號;
                oCmd.Parameters.Add("@create_empname" + i, SqlDbType.NVarChar).Value = SSOUtil.GetCurrentUser().姓名;

            }

            oCmd.CommandText = sb.ToString();
            return oCmd;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    private class rightObj
    {
        public string role_id { get; set; }/*角色權限*/
        public string empno { get; set; }/*工號*/
        public string empname { get; set; }/*姓名*/
        public string orgcd { get; set; }/*單位代碼*/
        public string deptid { get; set; }/*部門代碼*/
    }


    /*2,新增_專案*/
    SqlCommand insertCmd_project(XmlDocument xdoc, string pjGuid)
    {
        string val_projectname = "";
        string val_techname = "";
        string val_relword = "";

        try
        {
            ////string __technology = xdoc.SelectSingleNode("/*/rec[@item_no='1']") == null ? "" : xdoc.SelectSingleNode("/*/rec[@item_no='1']").ChildNodes[0].InnerText;/*技術項目*/
            ////string __tn_related_word = xdoc.SelectSingleNode("/*/rec[@item_no='2']") == null ? "" : xdoc.SelectSingleNode("/*/rec[@item_no='2']").ChildNodes[0].InnerText;/*關鍵字*/

            /*===取得專案名稱*/
            val_projectname = xdoc.SelectSingleNode("/*/*[@xx_item_explain='__專案名稱']/*") == null ? "" : xdoc.SelectSingleNode("/*/*[@xx_item_explain='__專案名稱']/*[1]").InnerText;

            /*===取得觀測項目名稱*/
            val_techname = xdoc.SelectSingleNode("/*/*[@xx_item_explain='__觀測項目名稱']/*") == null ? "" : xdoc.SelectSingleNode("/*/*[@xx_item_explain='__觀測項目名稱']/*[1]").InnerText;

            /*===取得觀測項目簡稱*/
            XmlNodeList xlist_relword = xdoc.SelectNodes("/*/*[@xx_item_explain='__觀測項目簡寫']/*");
            for (int i = 0; i < xlist_relword.Count; i++)
            {
                val_relword = val_relword + "^" + xlist_relword[i].InnerText;
            }

            if (val_relword != "")
            {
                val_relword = val_relword.Substring(1);
            }

            /*===insert*/
            SqlCommand oCmd = new SqlCommand();
            string sqlStr =
@"insert into input_project([project_guid], [project_name], [technology], [tn_related_word], [status], [create_time])
values(@project_guid, @project_name, @technology, @tn_related_word, '1', getdate());
";

            oCmd.CommandText = sqlStr;
            oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            oCmd.Parameters.Add("@project_name", SqlDbType.VarChar).Value = val_projectname;
            oCmd.Parameters.Add("@technology", SqlDbType.VarChar).Value = val_techname;
            oCmd.Parameters.Add("@tn_related_word", SqlDbType.VarChar).Value = val_relword;
            //oCmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = 1;
            //oCmd.Parameters.Add("@create_time", SqlDbType.DateTime).Value = DateTime.Now;
            //oCmd.Parameters.Add("@start_time", SqlDbType.DateTime).Value = DBNull.Value;
            //oCmd.Parameters.Add("@stop_time", SqlDbType.DateTime).Value = DBNull.Value;

            return oCmd;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*3,新增_監測網站*/
    SqlCommand insertCmd_website(string pjGuid, string optsite)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();

            string[] optsiteArray = optsite.Split(',');

            for (int i = 0; i < optsiteArray.Length; i++)
            {
                sb.AppendFormat(
@"insert into input_website([website_guid], [project_guid], [schedule], [website_name], [create_time])
    values(newid(), @project_guid{0}, '1', @website_name{0}, getdate());", i
                );
                //oCmd.Parameters.Add("@website_guid" + i, SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                oCmd.Parameters.Add("@project_guid" + i, SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
                //oCmd.Parameters.Add("@schedule" + i, SqlDbType.Int).Value = 1;
                oCmd.Parameters.Add("@website_name" + i, SqlDbType.VarChar).Value = optsiteArray[i];
                //oCmd.Parameters.Add("@create_time" + i, SqlDbType.DateTime).Value = DateTime.Now;
            }

            oCmd.CommandText = sb.ToString();
            return oCmd;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*4,新增_研究方向*/
    SqlCommand insertCmd_researchDirection(XmlDocument xdoc, string pjGuid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();
            XmlNodeList xlist = xdoc.SelectNodes("/*/*[@xx_item_explain='__研究方向']");
            string tmpResearchGuid = "";/*研究方向guid*/

            for (int i = 0; i < xlist.Count; i++)
            {
                tmpResearchGuid = xlist[i].Attributes["xx_item_guid"].Value;

                sb.AppendFormat(
@"insert into input_research_direction([research_guid], [name], [project_guid], [create_time])
values('{1}', @name{0}, @project_guid{0}, getdate());", i, tmpResearchGuid
    );
                //oCmd.Parameters.Add("@research_guid" + i, SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(xlist[i].Attributes["xx_item_guid"].Value);/*研究方向guid*/
                oCmd.Parameters.Add("@name" + i, SqlDbType.VarChar).Value = xlist[i].Attributes["xx_item_name"].Value;
                oCmd.Parameters.Add("@project_guid" + i, SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
                //oCmd.Parameters.Add("@create_time" + i, SqlDbType.DateTime).Value = DateTime.Now;
            }
            oCmd.CommandText = sb.ToString();
            return oCmd;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*5,新增_關連詞*/
    SqlCommand insertCmd_relatedWord(XmlDocument xdoc, string pjGuid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();
            XmlNodeList xlist = xdoc.SelectNodes("/*/*[@xx_item_explain='__研究方向']/*");
            string tmpResearchGuid = "";/*研究方向guid*/
            string tmpName = "";/*相關詞名稱*/
            int tmpBlacklist = 0;/*詞類:0為白名單, 1為黑名單*/

            for (int i = 0; i < xlist.Count; i++)
            {
                tmpResearchGuid = xlist[i].ParentNode.Attributes["xx_item_guid"].Value;/*此處要取上一層節點*/

                /*說明：當相關詞前面有負號時，取負號後的字串、以及詞的種類值設定為1,表示黑名單*/
                if (xlist[i].InnerText.StartsWith("-") == true)
                {
                    /*黑名單*/
                    tmpName = xlist[i].InnerText.Substring(1);
                    tmpBlacklist = 1;
                }
                else
                {
                    /*白名單*/
                    tmpName = xlist[i].InnerText;
                    tmpBlacklist = 0;
                }

                sb.AppendFormat(
@"insert into input_related_word([related_guid], [research_guid], [name], [blacklist], [schedule], [analyst_give], [create_time])
values(newid(), '{1}', @name{0}, '{2}', '1', '0', getdate() );", i, tmpResearchGuid, tmpBlacklist
    );
                //oCmd.Parameters.Add("@related_guid" + i, SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();/*相關詞guid*/
                //oCmd.Parameters.Add("@research_guid" + i, SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(xlist[i].ParentNode.Attributes["xx_item_guid"].Value);/*研究方向guid*/
                oCmd.Parameters.Add("@name" + i, SqlDbType.VarChar).Value = tmpName;            /*相關詞名稱*/
                //oCmd.Parameters.Add("@blacklist" + i, SqlDbType.TinyInt).Value = tmpBlacklist;  /*(0=白,1=黑,2=候選詞)*/
                //oCmd.Parameters.Add("@schedule" + i, SqlDbType.TinyInt).Value = 1;              /*(0=不納入每日排成, 1=納入每日排程[def])*/
                //oCmd.Parameters.Add("@analyst_give" + i, SqlDbType.TinyInt).Value = 0;          /*(0=excel匯入時[def],1=使用者新增,2=系統產生未審核,3=系統產生已審核)*/
                //oCmd.Parameters.Add("@name_stem" + i, SqlDbType.VarChar).Value = DBNull.Value;  /*原型化後的相關詞*/
                //oCmd.Parameters.Add("@score" + i, SqlDbType.Decimal).Value = DBNull.Value;      /*相關度分數*/
                //oCmd.Parameters.Add("@create_time" + i, SqlDbType.DateTime).Value = DateTime.Now;
            }
            oCmd.CommandText = sb.ToString();
            return oCmd;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

  


    /*============================================================================*/
    /*功能：匯入excel*/
    /*1,查詢_觀測網站*/
    public XmlDocument selectCmd_optsite()
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            string sqlStr =
@"select * from sys_opt_site where disable=0;";
            oCmd.CommandText = sqlStr;
            return DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn(ConfigUtil.DSN_Default), "yyyy/MM/dd");
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }


    /*============================================================================*/
    /*功能：修改關連詞納入或不納入每日排程*/

    public int exec_relatedWord_schedule_update(string typeGuid, string newSchedule)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            string sqlStr =
@"update input_related_word set schedule = @schedule
where related_guid = @related_guid
";

            oCmd.CommandText = sqlStr;
            oCmd.Connection = DbUtil.GetConn();
            oCmd.Parameters.AddWithValue("@schedule", newSchedule);/*0為不納入每日排成, 1為納入每日排程*/
            oCmd.Parameters.AddWithValue("@related_guid", typeGuid);/*關連詞guid*/

            int count = DbUtil.ExecCmdNoResult(oCmd);
            return count;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }


    /*============================================================================*/
    /*功能：專案啟動*/

    public int exec_project_start(string pjGuid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            string sqlStr =
@"update [input_project] set [status] = 10
where [status]=1 and [project_guid] = @pjGuid
";

            oCmd.CommandText = sqlStr;
            oCmd.Connection = DbUtil.GetConn();
            oCmd.Parameters.Add("@pjGuid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);

            int count = DbUtil.ExecCmdNoResult(oCmd);
            return count;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }


    /*============================================================================*/
    /*功能：專案刪除*/
    
    /*執行專案刪除*/
    public int exec_project_delete(string pjGuid)
    {
        try
        {
            SqlCommand[] oCmd = new SqlCommand[]
            {
                  deleteCmd_relatedWord(pjGuid)
                  ,deleteCmd_researchDirection(pjGuid)
                  ,deleteCmd_website(pjGuid)
                  ,deleteCmd_project(pjGuid)
                  ,deleteCmd_right(pjGuid)
             };
            int resultCount = DbUtil.ExecCmdTranNoResult(oCmd, DbUtil.GetConn());
            return resultCount;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    
    /*1,刪除_關連詞, input_related_word*/
    SqlCommand deleteCmd_relatedWord(string pjGuid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(
@"delete a from [input_related_word] a
inner join [input_research_direction] b
on a.[research_guid] = b.[research_guid]
where b.[project_guid] = @pjGuid
");
            oCmd.Parameters.Add("@pjGuid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            oCmd.CommandText = sb.ToString();
            return oCmd;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*2,刪除_研究方向, input_research_direction*/
    SqlCommand deleteCmd_researchDirection(string pjGuid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(
@"delete from [input_research_direction]
where [project_guid] = @pjGuid
");
            oCmd.Parameters.Add("@pjGuid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            oCmd.CommandText = sb.ToString();
            return oCmd;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*3,刪除_監測網站, input_website*/
    SqlCommand deleteCmd_website(string pjGuid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(
@"delete from [input_website]
where [project_guid] = @pjGuid
");
            oCmd.Parameters.Add("@pjGuid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            oCmd.CommandText = sb.ToString();
            return oCmd;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*4,刪除_專案, input_project*/
    SqlCommand deleteCmd_project(string pjGuid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(
@"delete from [input_project]
where [project_guid] = @pjGuid
");

            oCmd.Parameters.Add("@pjGuid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            oCmd.CommandText = sb.ToString();
            return oCmd;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*5,刪除_專案權限, sys_project_right*/
    SqlCommand deleteCmd_right(string pjGuid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(
@"delete from [sys_project_right]
where [project_guid] = @pjGuid
");

            oCmd.Parameters.Add("@pjGuid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            oCmd.CommandText = sb.ToString();
            return oCmd;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }


    /*============================================================================*/
    /*功能：專案結案*/

    public int exec_project_close(string pjGuid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            string sqlStr =
@"update [input_project] set 
[status] = @status
,[stop_time] = @stop_time
where [project_guid] = @pjGuid
";

            oCmd.CommandText = sqlStr;
            oCmd.Connection = DbUtil.GetConn();
            oCmd.Parameters.Add("@pjGuid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            oCmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = 50;
            oCmd.Parameters.Add("@stop_time", SqlDbType.DateTime).Value = DateTime.Now;

            int count = DbUtil.ExecCmdNoResult(oCmd);
            return count;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }


    /*============================================================================*/
    /*功能：匯出excel*/

    /*1,匯出_專案*/
    public XmlDocument selectCmd_project(string pjGuid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            string sqlStr =
@"select * from input_project where project_guid=@project_guid;";
            oCmd.CommandText = sqlStr;
            oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            return DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn(ConfigUtil.DSN_Default), "yyyy/MM/dd");
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*2,匯出_研究方向*/
    public XmlDocument selectCmd_researchDirection(string pjGuid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();
            string sqlStr =
@"select * from input_research_direction where project_guid=@project_guid
order by [research_sn] asc
;";
            oCmd.CommandText = sqlStr;
            oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            return DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn(ConfigUtil.DSN_Default), "yyyy/MM/dd");
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    /*3,匯出_關連詞*/
    /*說明1：匯出關連詞如為黑名單，則在詞前面加上(-)*/
    /*說明2：排除候選詞、未審核*/
    /*說明3：排序為白名單在上黑名單在下、依關連詞字母a-z排*/
    public XmlDocument selectCmd_relatedWord(string pjGuid)
    {
        try
        {
            SqlCommand oCmd = new SqlCommand();

            //////            string sqlStr =
            //////@"select name from input_related_word 
            //////where [research_guid] in (select [research_guid] from input_research_direction where project_guid=@project_guid);
            //////";

            string sqlStr =
@"select 

'direction' = a.name

,'relatedword' = 
case when blacklist='1' then '-'+b.name
else b.name
end

from input_research_direction a
left join input_related_word b on (a.research_guid = b.research_guid)

where a.project_guid=@project_guid
and b.blacklist !=2--//排除候選詞(0=白,1=黑,2=候選詞)
and b.analyst_give !=2--//排除未審核(0=excel匯入時,1=使用者新增,2=系統產生未審核,3=系統產生已審核)

--order by a.[research_sn] asc, b.[related_sn] asc
--order by a.[research_sn], b.[blacklist], (CASE WHEN ASCII(b.name)>90 THEN 0 ELSE 1 END), lower(b.name)
order by a.[research_sn], b.[blacklist], lower(b.name)
;";
            oCmd.CommandText = sqlStr;
            oCmd.Parameters.Add("@project_guid", SqlDbType.UniqueIdentifier).Value = XmlConvert.ToGuid(pjGuid);
            return DbUtil.GetDB2Xml(oCmd, DbUtil.GetConn(ConfigUtil.DSN_Default), "yyyy/MM/dd");
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }


    /*============================================================================*/
}