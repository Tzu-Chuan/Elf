using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

/// <summary>
/// ReadArticle_DB 的摘要描述
/// </summary>
public class ReadArticle_DB
{
    string KeyWord = string.Empty;
    public string _KeyWord
    {
        set { KeyWord = value; }
    }
    #region 私用
    string R_ID = string.Empty;
    string R_ArticleGuid = string.Empty;
    string R_Empno = string.Empty;
    DateTime R_CreateDate;
    #endregion
    #region 公用
    public string _R_ID { set { R_ID = value; } }
    public string _R_ArticleGuid { set { R_ArticleGuid = value; } }
    public string _R_Empno { set { R_Empno = value; } }
    public DateTime _R_CreateDate { set { R_CreateDate = value; } }
    #endregion

    public void addReadArticle()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        oCmd.CommandText = @"insert into ReadArticle (
R_ArticleGuid,
R_Empno
) values (
@R_ArticleGuid,
@R_Empno
) ";
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        oCmd.Parameters.AddWithValue("@R_ArticleGuid", R_ArticleGuid);
        oCmd.Parameters.AddWithValue("@R_Empno", R_Empno);

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();
    }

    public DataTable getReadArticle()
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(ConfigurationManager.AppSettings["DSN.Default"]);
        StringBuilder sb = new StringBuilder();

        sb.Append(@"Select * from ReadArticle where R_ArticleGuid=@R_ArticleGuid and R_Empno=@R_Empno ");

        oCmd.CommandText = sb.ToString();
        oCmd.CommandType = CommandType.Text;
        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataTable ds = new DataTable();

        oCmd.Parameters.AddWithValue("@R_ArticleGuid", R_ArticleGuid);
        oCmd.Parameters.AddWithValue("@R_Empno", R_Empno);

        oda.Fill(ds);
        return ds;
    }
}