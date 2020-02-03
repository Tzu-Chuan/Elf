using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISCSF200;


public partial class Qry_projno : ValidatorCommon
{
    #region �����ܼ�
    /// <summary>
    /// strCnword
    /// </summary>
    public string strCnword
    {
        set { ViewState["strCnword"] = value; }
        get { return (string)ViewState["strCnword"]; }
    }

    #endregion

    #region �t�Ψƥ�

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //'HIS'-�t���v�p��prs020; 'NOW'-�{��p��pro020
            //DEFAULT �{��p��pro020
            strCnword = Request["Cnword"] ?? "NOW";

            if ((!uc_regex.Match(Request["p1"] + "", uc_regex.OidEmpty) || !CheckSQLInjection(Request["p1"] + "")) || (!uc_regex.Match(Request["p2"] + "", uc_regex.OidEmpty) || !CheckSQLInjection(Request["p2"] + "")) ||
                (!uc_regex.Match(Request["p3"] + "", uc_regex.OidEmpty) || !CheckSQLInjection(Request["p3"] + "")) || (!uc_regex.Match(Request["p4"] + "", uc_regex.OidEmpty) || !CheckSQLInjection(Request["p4"] + "")) ||
                (!CheckSQLInjection(Request["Cnword"] + "")))
            {
                Response.Redirect(string.Format(@"ErrorPage.aspx"));
            }


            if (Request.QueryString["keyword"] != null)
            {
                tbx_keyword.Text = Request.QueryString["keyword"].ToString();
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", "clearValue('','','','');", true);
            }
            

            ddl_orgcd.DataBind();
            DataView dv = BindData();

            // if only one record is retrieved, return this record
            if (dv != null && dv.Count == 1)
                ClientScript.RegisterStartupScript(this.GetType(), "select", string.Format(@"setValue('{0}', '{1}', '{2}', '{3}');", dv[0]["s20_pojno"].ToString(), dv[0]["s20_pojcname"].ToString().Replace("'", "\\'"), dv[0]["s20_year"].ToString(), dv[0]["s20_orgcd"].ToString()), true);
        }
    }
    #endregion

    #region btn_Query_Click
    protected void btn_Query_Click(object sender, EventArgs e)
    {
        BindData();
    }
    #endregion

    #region gv_proj_RowCommand
    // ����p�e�N��
    protected void gv_proj_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            ClientScript.RegisterStartupScript(this.GetType(), "select", string.Format(@"setValue('{0}', '{1}', '{2}', '{3}');", ((LinkButton)e.CommandSource).Text, row.Cells[3].Text.Replace("'", "\\'"), row.Cells[0].Text, row.Cells[5].Text), true);
        }
    }
    #endregion

    #region gv_proj_RowCreated
    // �������
    protected void gv_proj_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[5].Visible = false;
        }
    }
    #endregion

    #endregion

    #region �ۭq�禡

    #region BindData
    // ���ô��
    private DataView BindData()
    {
        if (!ValidateFields())
            return null;

        #region select parameters

        ds_proj.SelectParameters.Clear();

        ds_proj.SelectParameters.Add("year", TypeCode.String, tbx_year.Text.Trim());
        ds_proj.SelectParameters["year"].ConvertEmptyStringToNull = false;

        ds_proj.SelectParameters.Add("orgcd", TypeCode.String, ddl_orgcd.SelectedValue);

        ds_proj.SelectParameters.Add("keyword", TypeCode.String, tbx_keyword.Text);
        ds_proj.SelectParameters["keyword"].ConvertEmptyStringToNull = false;

        ds_proj.SelectParameters.Add("CnWord", TypeCode.String, strCnword);
        ds_proj.SelectParameters["CnWord"].ConvertEmptyStringToNull = false;

        #endregion

        DataView dv;

        try
        {
            dv = (DataView)ds_proj.Select(new DataSourceSelectArguments());
        }
        catch (Exception ex)
        {
            throw new Exception("�d�߭p�e�N���ɵo�Ϳ��~: " + ex.Message, ex);
        }

        return dv;
    }
    #endregion

    #region ValidateFields
    // validate inputs
    private bool ValidateFields()
    {
        if (!uc_regex.Match(tbx_year.Text.Trim(), uc_regex.CYearEmpty))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "error", "alert('�п�J���T���褸�~��!');", true);
            return false;
        }

        return true;
    }
    #endregion

    #endregion
}
