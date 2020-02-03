using System;
using System.Data;


public partial class Grp_Grp_ChooseMember_test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //提供現有成員工號和姓名清單
        //h_mem_num.Value = 工號清單
        //h_mem_name.Value = 姓名清單
        //開啟挑選成員頁面
        btn_choose.OnClientClick = String.Format(@"return Get_member('{0}');", h_memlist.ClientID);
    }

    protected void btnDetonate_Click(object sender, EventArgs e)
    {
        #region 將取得的xml轉成DataSet
        if (h_memlist.Value.Length == 0 || h_memlist.Value == "undefined") return;
        DataSet dataSet = new DataSet();
        System.IO.StringReader reader = new System.IO.StringReader(h_memlist.Value);
        dataSet.ReadXml(reader);

        gv_body.DataSource = dataSet;
        gv_body.DataBind();
        #endregion

        string strmem_num = "";
        string strmem_name = "";
        for (int i = 0; i < gv_body.Rows.Count; i++)
        {
            strmem_num += gv_body.Rows[i].Cells[0].Text + ",";
            strmem_name += gv_body.Rows[i].Cells[1].Text + ",";
        }
        if (strmem_num.Length != 0) strmem_num = strmem_num.Substring(0, strmem_num.Length - 1);
        if (strmem_name.Length != 0) strmem_name = strmem_name.Substring(0, strmem_name.Length - 1);
        //提供最新的工號及姓名清單
        h_mem_num.Value = strmem_num;
        h_mem_name.Value = strmem_name;
    }
}
