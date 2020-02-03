using System;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using ISCSF200;

public partial class Sub_Sys_Qry_empno : ValidatorCommon
{
    #region 全域變數

    /// <summary>
    /// depcd
    /// depcd="Y" 離職員工( 加上 com_depcd<> 'Y' AND 條件)
    /// </summary>
    public string depcd
    {
        set { ViewState["depcd"] = value; }
        get { return (string)ViewState["depcd"]; }
    }




    #endregion


    #region 系統事件

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if ((Request["p1"] == null || uc_regex.Match(Request["p1"], uc_regex.OidEmpty) || !CheckSQLInjection(Request["p1"] + "")) && (Request["p2"] == null || uc_regex.Match(Request["p2"], uc_regex.OidEmpty) || !CheckSQLInjection(Request["p2"] + "")) &&
                (Request["p3"] == null || uc_regex.Match(Request["p3"], uc_regex.OidEmpty) || !CheckSQLInjection(Request["p3"] + "")) && (Request["p4"] == null || uc_regex.Match(Request["p4"], uc_regex.OidEmpty) || !CheckSQLInjection(Request["p4"] + "")) &&
                (Request["p5"] == null || uc_regex.Match(Request["p5"], uc_regex.OidEmpty) || !CheckSQLInjection(Request["p5"] + "")) && (Request["p6"] == null || uc_regex.Match(Request["p6"], uc_regex.OidEmpty) || !CheckSQLInjection(Request["p6"] + "")) &&
                (Request["p7"] == null || uc_regex.Match(Request["p7"], uc_regex.OidEmpty) || !CheckSQLInjection(Request["p7"] + "")) && (Request["p8"] == null || uc_regex.Match(Request["p8"], uc_regex.OidEmpty) || !CheckSQLInjection(Request["p8"] + "")) &&
                (Request["orgcd"] == null || uc_regex.Match(Request["orgcd"], uc_regex.OrgcdEmpty) || !CheckSQLInjection(Request["orgcd"] + "")) && (Request["depcd"] == null || uc_regex.Match(Request["depcd"], uc_regex.OidEmpty) || !CheckSQLInjection(Request["depcd"] + "")) &&
                (Request["keyword"] == null || uc_regex.Match(Request["keyword"], uc_regex.EmpnoEmpty) || uc_regex.Match(Request["keyword"], uc_regex.EmpnameEmpty) || uc_regex.Match(Request["keyword"], uc_regex.ExtEmpty) || !CheckSQLInjection(Request["keyword"] + "")))
            {
                tbx_keyword.Text = Request["keyword"] + "";
                tbx_orgcd.Text = Request["orgcd"] + "";
                depcd = Request["depcd"] + "";
            }
            else
            {
                Response.Redirect(string.Format(@"ErrorPage.aspx"));
            }

            DataSet ds = BindData();

            #region // 若搜尋結果只有一筆，則將資料直接帶回原視窗
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string script = string.Format(@"setValue('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');",
                    dr["com_empno"].ToString().Trim(), dr["com_cname"].ToString().Trim(),
                    dr["com_telext"].ToString().Trim(), dr["com_orgcd"].ToString().Trim(),
                    dr["com_deptcd"].ToString().Trim(), dr["com_deptid"].ToString().Trim(),
                    dr["com_dept_name"].ToString().Trim(),dr["com_mailadd"].ToString().Trim());
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", script, true);

            }
            else if (Request["keyword"] != null)
            {
                string script = string.Format("clearValue('','','','','','','','');");
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", script, true);
            }
            #endregion


        }

    }
    #endregion

    #region btn_Query_Click
    protected void btn_Query_Click(object sender, EventArgs e)
    {
        BindData();
    }
    #endregion

    #region dg_SortCommand
    //排序部份使用datagrid的sortcommand事件，直接再做binddata就好
    protected void dg_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        BindData();
    }
    #endregion

    #region datagrid 上下頁控制

    protected void dg_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
    {
        dg.CurrentPageIndex = e.NewPageIndex;
        BindData();
    }
    #endregion


    #endregion

    #region 自訂函式

    #region GetDbConnection
    private SqlConnection GetDbConnection()
    {
        string connString = ConfigurationManager.AppSettings["DSN.Common"];
        return new SqlConnection(connString);
    }
    #endregion

    #region BindData

    private DataSet BindData()
    {
        string condition = (tbx_orgcd.Text.Length == 2) ?  string.Format(" AND ltrim(com_orgcd)='{0}'", tbx_orgcd.Text) : string.Empty;
        string tmp_depcd="";
        if (depcd.ToUpper() == "Y")
        {
            tmp_depcd = "com_depcd<> 'Y' AND";
        }

        #region SQL
        string SQL = string.Format(@"
select top 500 
com_empno
,com_cname
,com_orgcd,com_deptcd
,com_deptid,com_mailadd
,org_abbr_chnm2,
(select top 1 dep_deptname from common..depcod cd where cd.dep_deptid=com_deptid ) AS com_dept_name
, com_telext
FROM common..comper
INNER JOIN common..orgcod ON com_orgcd = org_orgcd
WHERE 
{0} ((upper(com_empno) like upper(@keyword))
OR (com_cname like @keyword)
OR (com_telext like @keyword)) 
{1}
and com_orgcd = @com_orgcd
ORDER BY com_empno DESC
", tmp_depcd, condition);
        #endregion

        SqlCommand oCmd = new SqlCommand(SQL, GetDbConnection());
        oCmd.CommandText = SQL;
        oCmd.Connection = GetDbConnection();

        string keyword = string.Format("%{0}%", tbx_keyword.Text.Trim());
        oCmd.Parameters.AddWithValue("@keyword", keyword);

        oCmd.Parameters.AddWithValue("@com_orgcd", ConfigUtil.AppIEKOrgcd);//產經中心代碼

        SqlDataAdapter oda = new SqlDataAdapter(oCmd);
        DataSet ds = new DataSet();
        oda.Fill(ds);

        dg.DataSource = ds;
        dg.DataBind();

        return ds;
    }

    #endregion

    #endregion
}
