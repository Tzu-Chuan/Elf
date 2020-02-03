using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISCSF200;

public partial class Qry_customer : ValidatorCommon
{

    #region 系統事件

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            //１．先做參數驗證（是否為ｎｕｌｌ或含有非法字元）
            if ((uc_regex.Match(Request["p1"]+"", uc_regex.OidEmpty) || !CheckSQLInjection(Request["p1"] + "")) && (uc_regex.Match(Request["p2"]+"", uc_regex.OidEmpty) || !CheckSQLInjection(Request["p2"] + "")) &&
               (uc_regex.Match(Request["p3"] + "", uc_regex.OidEmpty) || !CheckSQLInjection(Request["p3"] + "")) && (uc_regex.Match(Request["p4"] + "", uc_regex.OidEmpty) || !CheckSQLInjection(Request["p4"] + "")) &&
               (uc_regex.Match(Request["p5"] + "", uc_regex.OidEmpty) || !CheckSQLInjection(Request["p5"] + "")) && (uc_regex.Match(Request["p6"] + "", uc_regex.OidEmpty) || !CheckSQLInjection(Request["p6"] + "")) &&
               (uc_regex.Match(Request["p7"] + "", uc_regex.OidEmpty) || !CheckSQLInjection(Request["p7"] + "")) && (uc_regex.Match(Request["p8"] + "", uc_regex.OidEmpty) || !CheckSQLInjection(Request["p8"] + "")) &&
               (uc_regex.Match(Request["keyword"] + "", uc_regex.CompidEmpty) || uc_regex.Match(Request["keyword"] + "", uc_regex.CompCNameEmpty) || !CheckSQLInjection(Request["keyword"] + "")) &&
                CheckSQLInjection(Request["oricountry"] + ""))
            {
                tbx_keyword.Text = Request["keyword"] + "";
            }
            else
            {
                //２．若參數錯誤－導向錯誤頁面
                Response.Redirect(string.Format(@"ErrorPage.aspx"));
            }

            DataView dv = BindData();

            //４．一筆結果，直接帶回原視窗，關閉子視窗
            #region // 若搜尋結果只有一筆，則將資料直接帶回原視窗
            if (dv != null && dv.Count == 1)
            {
                string script = string.Format(@"setValue(""{0}"",""{1}"",""{2}"",""{3}"",""{4}"",""{5}"",""{6}"",""{7}"");",
                                dv[0]["comp_idno"].ToString().Trim(),
                                dv[0]["comp_cname"].ToString().Trim(),
                                dv[0]["comp_postno"].ToString().Trim(),
                                dv[0]["comp_phone"].ToString().Trim(),
                                dv[0]["comp_fax"].ToString().Trim(),
                                dv[0]["addr"].ToString().Trim(),
                                dv[0]["comp_chairman"].ToString().Trim(),
                                dv[0]["comp_cmtitle"].ToString().Trim(),
                                "");

                ClientScript.RegisterStartupScript(this.GetType(), "Msg", script, true);
            }
            else if (Request["keyword"] != null)//５．沒有任何資料筆數就清空回傳的參數資料（避免查詢不到值時，畫面還留有原始資料）
            {
                string script = string.Format("clearValue('','','','','','','','');");
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", script, true);
            }
            #endregion

            string url = ConfigurationManager.AppSettings["CustAdd"].ToString();
            btn_Add.OnClientClick = string.Format("window.close();WinOpen('{0}');", url);
        }
    }
    #endregion

    #region btn_Query_Click
    protected void btn_Query_Click(object sender, EventArgs e)
    {
        BindData();
    }
    #endregion

    #region gv_Cust_RowDataBound
    protected void gv_Cust_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //６．資料bind到gridview時，就先串好linkButton，user點該連結，直接可以把資料帶回原畫面

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkbtn = (LinkButton)e.Row.FindControl("LinkButton1");
            string script = string.Format(@"setValue(""{0}"",""{1}"",""{2}"",""{3}"",""{4}"",""{5}"",""{6}"",""{7}"");",
                                DataBinder.Eval(e.Row.DataItem, "comp_idno"),
                                DataBinder.Eval(e.Row.DataItem, "comp_cname"),
                                DataBinder.Eval(e.Row.DataItem, "comp_postno"),
                                DataBinder.Eval(e.Row.DataItem, "comp_phone"),
                                DataBinder.Eval(e.Row.DataItem, "comp_fax"),
                                DataBinder.Eval(e.Row.DataItem, "addr"),
                                DataBinder.Eval(e.Row.DataItem, "comp_chairman"),
                                DataBinder.Eval(e.Row.DataItem, "comp_cmtitle"));
            lnkbtn.OnClientClick = script;
        }
    }
    #endregion

    #endregion

    #region 自訂函式

    #region ValidateFields
    // validate inputs
    private bool ValidateFields()
    {
        if (!uc_regex.Match(tbx_keyword.Text.Trim(), uc_regex.CompidEmpty) && !uc_regex.Match(tbx_keyword.Text.Trim(), uc_regex.CompCNameEmpty))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "error", "alert('關鍵字錯誤!');", true);
            return false;
        }

        return true;
    }

    protected void btn_Add_Click(object sender, EventArgs e)
    {

    }


    #endregion

    #region BindData
    private DataView BindData()
    {
        //３．資料撈出
        if (!ValidateFields())
            return null;

        string tmp_oricountry = Request["oricountry"] + "";

        ds_custquery.SelectParameters.Clear();
        ds_custquery.SelectParameters.Add("keyword", TypeCode.String, tbx_keyword.Text.Trim());
        ds_custquery.SelectParameters["keyword"].ConvertEmptyStringToNull = false;
        ds_custquery.SelectParameters.Add("oricountry", TypeCode.String, tmp_oricountry);
        ds_custquery.SelectParameters["oricountry"].ConvertEmptyStringToNull = false;        

        DataView dv;

        try
        {
            dv = (DataView)ds_custquery.Select(new DataSourceSelectArguments());
        }
        catch (Exception ex)
        {
            throw new Exception("查詢代碼及參數資料時發生錯誤: " + ex.Message, ex);
        }
        return dv;
    }
    #endregion

    #endregion


}
