using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Xml;
using ISCSF200;

public partial class Grp_Qry_member : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string D4orgcd = Request["D4orgcd"];
            string changeorgcd = Request["changeorgcd"];

            //單位挑選清單
            lisb_org.DataSource = Cls_Data.GetOrgcd(D4orgcd,changeorgcd);
            lisb_org.DataTextField = "text";
            lisb_org.DataValueField = "value";
            lisb_org.DataBind();
            
            ddl_orgcdlist.DataSource = Cls_Data.GetOrgcd(D4orgcd, changeorgcd);
            ddl_orgcdlist.DataTextField = "text";
            ddl_orgcdlist.DataValueField = "value";
            ddl_orgcdlist.DataBind();

            if (!string.IsNullOrEmpty(D4orgcd))
            {
                lisb_org.SelectedValue = D4orgcd;
                ddl_orgcdlist.SelectedValue = D4orgcd;
                lisb_org_SelectedIndexChanged(sender, e);
            }

            ReBindGridViewBody();
            ReBindGridViewContact();
            ReBindGridViewAddrBook();

            //建立目前成員清單
            char[] seq ={ ',' };
            if (Request.Form["empno"] != "")
            {
                string[] empno = Request["empno"].ToString().Split(seq);
                string[] empname = Request["empname"].ToString().Split(seq);
                ListItem li = new ListItem();
                for (int i = 0; i < empno.Length; i++)
                {
                    lisb_body.Items.Add(new ListItem(empname[i], empno[i]));
                }
            }
            gv_body.Sort("com_cname", SortDirection.Descending);
            gv_contact.Sort("com_cname", SortDirection.Descending);
            gv_addrbook.Sort("ab_name", SortDirection.Descending);
        }
    }

    protected void lik_body_Click(object sender, EventArgs e)
    {
        //姓名
        pan_body.Visible = true;
        pan_btnBody.Visible = true;
        btn_body_add.Visible = true;
        btn_body_del.Visible = true;
        //部門
        pan_dept.Visible = false;
        pan_btnDep.Visible = false;
        btn_dept_add.Visible = false;
        btn_dept_del.Visible = false;
        //我的聯絡人
        pan_cont.Visible = false;
        pan_btnCont.Visible = false;
        btn_cont_add.Visible = false;
        btn_cont_del.Visible = false;
        //群組聯絡人
        pan_Addrbook.Visible = false;
        pan_btnaddrbook.Visible = false;
        btn_addrbook_add.Visible = false;
        btn_addrbook_del.Visible = false;
    }
    protected void lik_dept_Click(object sender, EventArgs e)
    {
        //姓名
        pan_body.Visible = false;
        pan_btnBody.Visible = false;
        btn_body_add.Visible = false;
        btn_body_del.Visible = false;
        //部門
        pan_dept.Visible = true;
        pan_btnDep.Visible = true;
        btn_dept_add.Visible = true;
        btn_dept_del.Visible = true;
        //我的聯絡人
        pan_cont.Visible = false;
        pan_btnCont.Visible = false;
        btn_cont_add.Visible = false;
        btn_cont_del.Visible = false;
        //群組聯絡人
        pan_Addrbook.Visible = false;
        pan_btnaddrbook.Visible = false;
        btn_addrbook_add.Visible = false;
        btn_addrbook_del.Visible = false;
    }
    protected void lik_comanager_Click(object sender, EventArgs e)
    {
        //顯示已完成階段的群組成員清單
        //ReBindGridViewGrpMember();
        //姓名
        pan_body.Visible = false;
        pan_btnBody.Visible = false;
        btn_body_add.Visible = false;
        btn_body_del.Visible = false;
        //部門
        pan_dept.Visible = false;
        pan_btnDep.Visible = false;
        btn_dept_add.Visible = false;
        btn_dept_del.Visible = false;
        //我的聯絡人
        pan_cont.Visible = true;
        pan_btnCont.Visible = true;
        btn_cont_add.Visible = true;
        btn_cont_del.Visible = true;
        //群組聯絡人
        pan_Addrbook.Visible = false;
        pan_btnaddrbook.Visible = false;
        btn_addrbook_add.Visible = false;
        btn_addrbook_del.Visible = false;
    }
    protected void lik_addrbook_Click(object sender, EventArgs e)
    {
        //姓名
        pan_body.Visible = false;
        pan_btnBody.Visible = false;
        btn_body_add.Visible = false;
        btn_body_del.Visible = false;
        //部門
        pan_dept.Visible = false;
        pan_btnDep.Visible = false;
        btn_dept_add.Visible = false;
        btn_dept_del.Visible = false;
        //我的聯絡人
        pan_cont.Visible = false;
        pan_btnCont.Visible = false;
        btn_cont_add.Visible = false;
        btn_cont_del.Visible = false;
        //群組聯絡人
        pan_Addrbook.Visible = true;
        pan_btnaddrbook.Visible = true;
        btn_addrbook_add.Visible = true;
        btn_addrbook_del.Visible = true;
    }

    protected void btn_body_serch_Click(object sender, EventArgs e)
    {
        //查詢
        //show不可輸入 ' -- / * 等字元當查詢條件
        if (Utils.IsLegal(tbx_keyword.Text))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alter", "alert(\"不可輸入 ' -- /* 等字元當查詢條件\");", true);
            return;
        }
        ReBindGridViewBody();
        gv_body.PageIndex = 0;
    }
    protected void btn_cont_search_Click(object sender, EventArgs e)
    {
        //show不可輸入 ' -- / * 等字元當查詢條件
        if (Utils.IsLegal(tbx_keyword_cont.Text))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alter", "alert(\"不可輸入 ' -- /* 等字元當查詢條件\");", true);
            return;
        }
        ReBindGridViewContact();
        gv_contact.PageIndex = 0;
    }
    protected void btn_addrbook_search_Click(object sender, EventArgs e)
    {
        //查詢
        //show不可輸入 ' -- / * 等字元當查詢條件
        if (Utils.IsLegal(tbx_keyword.Text))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alter", "alert(\"不可輸入 ' -- /* 等字元當查詢條件\");", true);
            return;
        }
        ReBindGridViewAddrBook();
        gv_addrbook.PageIndex = 0;
    }

    private void ReBindGridViewBody()
    {
        //關鍵字查詢
        string keyword = tbx_keyword.Text;
        ds_memberlist.SelectParameters.Clear();
        ds_memberlist.SelectParameters.Add("keyword", TypeCode.String, keyword);
        ds_memberlist.SelectParameters.Add("orgcd", TypeCode.String, ddl_orgcdlist.SelectedValue == "00" ? "" : ddl_orgcdlist.SelectedValue);
        ds_memberlist.Select(new DataSourceSelectArguments());
    }
    private void ReBindGridViewContact()
    {
        //關鍵字查詢
        string keyword = tbx_keyword_cont.Text.Trim();

        ds_contact.SelectParameters.Clear();
        ds_contact.SelectParameters.Add("empno", TypeCode.String, Utils.GetITRILogOnUser(this));
        ds_contact.SelectParameters.Add("keyword", TypeCode.String, keyword);
        ds_contact.Select(new DataSourceSelectArguments());

    }
    private void ReBindGridViewAddrBook()
    {
        string keyword = tbx_keyword_addrbook.Text.Trim();

        ds_addrbook.SelectParameters.Clear();
        ds_addrbook.SelectParameters.Add("empno", Utils.GetITRILogOnUser(this));
        ds_addrbook.SelectParameters.Add("keyword", TypeCode.String, keyword);
        ds_addrbook.Select(new DataSourceSelectArguments());
    }

    protected void btn_body_add_Click(object sender, EventArgs e)
    {
        //人-增加
        //把換頁前的項目記下來,
        foreach (GridViewRow gvr in gv_body.Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                CheckBox cbx = (CheckBox)gvr.Cells[0].FindControl("ckb_body");
                if (cbx != null)
                {
                    string com_came = cbx.Text;
                    string com_empno = ((HiddenField)gvr.Cells[0].FindControl("hf_com_empno")).Value;
                    ListItem li = new ListItem(com_came, com_empno);
                    if (cbx.Checked)
                    {
                        if (!lbxTempItem.Items.Contains(li))
                        {
                            lbxTempItem.Items.Add(li);
                        }
                        cbx.Checked = false;
                    }
                    else
                    {
                        lbxTempItem.Items.Remove(li);
                    }
                }
            }
        }

        //lbxTempItem  => lisb_body
        foreach (ListItem li in lbxTempItem.Items)
        {
            if (lisb_body.Items.Contains(li))
            {
                //項目重複的處理
                lbl_Message.Text += li.Text + ",";
            }
            else
            {
                lisb_body.Items.Add(li);
            }
        }

        if (lbxTempItem.Items.Count.Equals(0))
        {
            MessageBox.Show("請選擇一個要新增的群組成員");
        }

        else
        {
            lbxTempItem.Items.Clear();
            // MessageBox.Show(lbl_Message.Text + "已存在群組成員列表");
            lbl_Message.Text = string.Empty;
        }
       // Get_memberXML();
    }
    protected void btn_body_del_Click(object sender, EventArgs e)
    {
        if (lisb_body.SelectedIndex != -1)
        {
            foreach (ListItem li in lisb_body.Items)
            {
                if (li.Selected)
                {
                    ListItem newLi = new ListItem(li.Text, li.Value);
                    lisb_bodydel.Items.Add(newLi);
                }
            }
            //多筆刪除
            while (lisb_body.SelectedIndex != -1)
            {
                lisb_body.Items.Remove(lisb_body.SelectedItem);
            }
        }
        else
        {
            MessageBox.Show("請選擇一個要移除的成員");
        }
        //Get_memberXML();
    }
    protected void btn_dept_add_Click(object sender, EventArgs e)
    {
        if (lisb_cname.SelectedIndex != -1)
        {
            foreach (ListItem li in lisb_cname.Items)
            {
                if (li.Selected)
                {
                    ListItem newLib = new ListItem(li.Text, li.Value);
                    lisb_body.Items.Add(newLib);
                    lisb_cname.Items.FindByValue(li.Value).Enabled = false;
                }
            }
        }
        else
        {
            MessageBox.Show("請選擇一個要新增的群組成員");
        }
        //Get_memberXML();
    }
    protected void btn_dept_del_Click(object sender, EventArgs e)
    {
        if (lisb_body.SelectedIndex != -1)
        {
            foreach (ListItem li in lisb_body.Items)
            {
                if (li.Selected)
                {
                    ListItem newLi = new ListItem(li.Text, li.Value);
                    lisb_bodydel.Items.Add(newLi);
                    if (lisb_cname.Items.Contains(li))
                    {
                        //lisb_cname.Items.Add(newLi);
                        lisb_cname.Items.FindByValue(li.Value).Enabled = true;
                    }
                }
            }
            //多筆刪除
            while (lisb_body.SelectedIndex != -1)
            {
                lisb_body.Items.Remove(lisb_body.SelectedItem);
            }
        }
        else
        {
            MessageBox.Show("請選擇一個要移除的成員");
        }
        //Get_memberXML();
    }
    protected void btn_cont_add_Click(object sender, EventArgs e)
    {
        //人-增加
        //把換頁前的項目記下來,
        foreach (GridViewRow gvr in gv_contact.Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                CheckBox cbx = (CheckBox)gvr.Cells[0].FindControl("ckb_cont");
                if (cbx != null)
                {
                    //判斷勾選的選項是否為數字：工號一定是數字
                    if (Utils.IsInteger(((HiddenField)gvr.Cells[0].FindControl("hf_cont_empno")).Value))
                    {
                        string com_came = cbx.Text;
                        string com_empno = ((HiddenField)gvr.Cells[0].FindControl("hf_cont_empno")).Value;
                        ListItem li = new ListItem(com_came, com_empno);
                        if (cbx.Checked)
                        {
                            if (!lbxTempItem_cont.Items.Contains(li))
                            {
                                lbxTempItem_cont.Items.Add(li);
                            }
                            cbx.Checked = false;
                        }
                        else
                        {
                            lbxTempItem_cont.Items.Remove(li);
                        }
                    }
                    else
                    {
                        Response.Redirect(string.Format(@"GrpErrorPage.aspx?msg={0}", Server.HtmlEncode("Msg.無對應資料")));
                    }
                }
            }
        }

        //lbxTempItem  => lisb_body
        foreach (ListItem li in lbxTempItem_cont.Items)
        {
            if (lisb_body.Items.Contains(li))
            {
                //項目重複的處理
                lbl_Message_cont.Text += li.Text + ",";
            }
            else
            {
                lisb_body.Items.Add(li);
            }
        }

        if (lbxTempItem_cont.Items.Count.Equals(0))
        {
            MessageBox.Show("請選擇一個要新增的群組成員");
        }

        else
        {
            lbxTempItem_cont.Items.Clear();
            // MessageBox.Show(lbl_Message.Text + "已存在群組成員列表");
            lbl_Message_cont.Text = string.Empty;
        }
    }
    protected void btn_cont_del_Click(object sender, EventArgs e)
    {
        //所選擇的項目正常
        if (lisb_body.SelectedIndex != -1)
        {
            foreach (ListItem li in lisb_body.Items)
            {
                if (li.Selected)
                {
                    ListItem newLi = new ListItem(li.Text, li.Value);
                    lisb_bodydel.Items.Add(newLi);
                }
            }


            //多筆刪除
            while (lisb_body.SelectedIndex != -1)
            {
                lisb_body.Items.Remove(lisb_body.SelectedItem);
            }

        }
        else
        {
            MessageBox.Show("請選擇一個要移除的群組成員");
        }
    }
    protected void btn_addrbook_add_Click(object sender, EventArgs e)
    {
        //人-增加
        //把換頁前的項目記下來,
        foreach (GridViewRow gvr in gv_addrbook.Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                CheckBox cbx = (CheckBox)gvr.Cells[0].FindControl("ckb_addrbook");
                if (cbx != null)
                {
                    //判斷勾選的選項是否為數字：工號一定是數字
                    if (Utils.IsInteger(((HiddenField)gvr.Cells[0].FindControl("hf_addrbook_empno")).Value))
                    {
                        string com_came = cbx.Text;
                        string com_empno = ((HiddenField)gvr.Cells[0].FindControl("hf_addrbook_empno")).Value;
                        ListItem li = new ListItem(com_came, com_empno);
                        if (cbx.Checked)
                        {
                            if (!lbxTempItem_addrbook.Items.Contains(li))
                            {
                                lbxTempItem_addrbook.Items.Add(li);
                            }
                            cbx.Checked = false;
                        }
                        //else
                        //{
                        //    lbxTempItem_addrbook.Items.Remove(li);
                        //}
                    }
                    else
                    {
                        Response.Redirect(string.Format(@"GrpErrorPage.aspx?msg={0}", Server.HtmlEncode("Msg.無對應資料")));
                    }
                }
            }
        }

        //lbxTempItem_addrbook  => lisb_body
        foreach (ListItem li in lbxTempItem_addrbook.Items)
        {
            if (lisb_body.Items.Contains(li))
            {
                //項目重複的處理
                lbl_Message_addrbook.Text += li.Text + ",";
            }
            else
            {
                lisb_body.Items.Add(li);
            }
        }

        if (lbxTempItem_addrbook.Items.Count.Equals(0))
        {
            MessageBox.Show("請選擇一個要新增的群組成員");
        }

        else
        {
            lbxTempItem_addrbook.Items.Clear();
            lbl_Message_addrbook.Text = string.Empty;
        }
    }
    protected void btn_addrbook_del_Click(object sender, EventArgs e)
    {
        //所選擇的項目正常
        if (lisb_body.SelectedIndex != -1)
        {
            foreach (ListItem li in lisb_body.Items)
            {
                if (li.Selected)
                {
                    ListItem newLi = new ListItem(li.Text, li.Value);
                    lisb_bodydel.Items.Add(newLi);
                }
            }


            //多筆刪除
            while (lisb_body.SelectedIndex != -1)
            {
                lisb_body.Items.Remove(lisb_body.SelectedItem);
            }

        }
        else
        {
            MessageBox.Show("請選擇一個要移除的群組成員");
        }
    }

    private void Get_memberXML()
    {
        string strsellist = "";
        for (int i = 0; i < lisb_body.Items.Count; i++)
        {
            strsellist += lisb_body.Items[i].Value + ",";
        }
        if (strsellist.Length > 0) strsellist = strsellist.Substring(0, strsellist.Length - 1);

        System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection();
        sqlConnection1.ConnectionString = ConfigurationManager.ConnectionStrings["GrbWebConn"].ConnectionString;
        string select = string.Format("select com_empno,rtrim(com_cname)com_cname,rtrim(com_telext)com_telex,com_deptid,(select rtrim(t52_mailbox)+'@itri.org.tw' from common..te520 where t52_empno=com_empno)com_email  from common..comper Where (CHARINDEX(com_empno,'{0}')>0)", strsellist);
        SqlDataAdapter da = new SqlDataAdapter(select, sqlConnection1);
        DataSet ds = new DataSet();
        da.Fill(ds, "Member");

        XmlDataDocument result = new XmlDataDocument(ds);
        h_memlist.Value = result.InnerXml;
    }

    protected void btn_body_finish_Click(object sender, EventArgs e)
    {
        //傳XML型式的工號資料


        string strsellist = "";
        for (int i = 0; i < lisb_body.Items.Count; i++)
        {
            strsellist += lisb_body.Items[i].Value + ",";
        }
        if (strsellist.Length > 0) strsellist = strsellist.Substring(0, strsellist.Length - 1);

        System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection();
        sqlConnection1.ConnectionString = ConfigurationManager.ConnectionStrings["GrbWebConn"].ConnectionString;
        string select = string.Format("select com_empno,rtrim(com_cname)com_cname,rtrim(com_telext)com_telex,com_deptid,(select rtrim(t52_mailbox)+'@itri.org.tw' from common..te520 where t52_empno=com_empno)com_email  from common..comper Where (CHARINDEX(com_empno,'{0}')>0)", strsellist);
        SqlDataAdapter da = new SqlDataAdapter(select, sqlConnection1);
        DataSet ds = new DataSet();
        da.Fill(ds, "Member");

        XmlDataDocument result = new XmlDataDocument(ds);
        
        ClientScript.RegisterStartupScript(this.GetType(), "select", string.Format(@"setValue('{0}');", result.InnerXml), true);
       // ClientScript.RegisterStartupScript(this.GetType(), "select", string.Format(@"setValue('{0}');", strsellist), true);
        //Session["Member"] = ds;
        //Session["xmlMem"] = result.InnerXml;
        h_memlist.Value = result.InnerXml;
    }

    protected void gv_body_PageIndexChanged(object sender, EventArgs e)
    {
        //把換後要把之前的項目勾回去,
        foreach (GridViewRow gvr in ((GridView)sender).Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                CheckBox cbx = (CheckBox)gvr.Cells[0].FindControl("ckb_body");
                if (cbx != null)
                {
                    string com_came = cbx.Text;
                    string com_empno = ((HiddenField)gvr.Cells[0].FindControl("hf_com_empno")).Value;
                    ListItem li = new ListItem(com_came, com_empno);
                    if (lbxTempItem.Items.Contains(li))
                    {
                        cbx.Checked = true;
                    }
                }
            }
        }
    }
    protected void gv_body_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //把換頁前的項目記下來,
        foreach (GridViewRow gvr in ((GridView)sender).Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                CheckBox cbx = (CheckBox)gvr.Cells[0].FindControl("ckb_body");
                if (cbx != null)
                {
                    string com_came = cbx.Text;
                    string com_empno = ((HiddenField)gvr.Cells[0].FindControl("hf_com_empno")).Value;
                    ListItem li = new ListItem(com_came, com_empno);
                    if (cbx.Checked)
                    {
                        if (!lbxTempItem.Items.Contains(li))
                        {
                            lbxTempItem.Items.Add(li);
                        }
                    }
                    else
                    {
                        lbxTempItem.Items.Remove(li);
                    }
                }
            }
        }
        gv_body.PageIndex = e.NewPageIndex;
        ReBindGridViewBody();
    }
    protected void gv_body_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)//只有在頁首時才進行
        {
            foreach (DataControlField field in gv_body.Columns)
            {
                if (field.SortExpression == gv_body.SortExpression && gv_body.SortExpression != "")//當點選進行排序時才放圖
                {
                    Image obj = new Image();//定義新的圖片物件
                    obj.ImageUrl = ((gv_body.SortDirection == SortDirection.Ascending) ? "./Images/icon-arrowdesc.gif" : "./Images/icon-arrowasc.gif");//看是升冪還是降冪
                    obj.AlternateText = "ASC";

                    e.Row.Cells[gv_body.Columns.IndexOf(field)].Controls.Add(obj);//加入圖示
                }
            }
        }
    }

    protected void lisb_org_SelectedIndexChanged(object sender, EventArgs e)
    {
        //清空部門
        lisb_dept.Items.Clear();
        //清空部門人員
        lisb_cname.Items.Clear();
        //產生部門
        string orgcd = lisb_org.SelectedValue;
        lisb_dept.DataSource = Cls_Data.GetDept(orgcd);
        lisb_dept.DataTextField = "text";
        lisb_dept.DataValueField = "value";
        lisb_dept.DataBind();
    }
    protected void lisb_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        //清空部門人員
        lisb_cname.Items.Clear();
        //產生部門人員
        string orgcd = lisb_org.SelectedValue;
        string deptcd = lisb_dept.SelectedValue;
        lisb_cname.DataSource = Cls_Data.GetBody(orgcd,deptcd);
        lisb_cname.DataTextField = "text";
        lisb_cname.DataValueField = "value";
        lisb_cname.DataBind();
    }
    protected void lisb_cname_DataBound(object sender, EventArgs e)
    {
        foreach (ListItem li in lisb_cname.Items)
        {
            if (lisb_body.Items.Contains(li))
            {
                li.Enabled = false;
            }
        }
    }

    protected void gv_contact_PageIndexChanged(object sender, EventArgs e)
    {
        //把換後要把之前的項目勾回去,
        foreach (GridViewRow gvr in ((GridView)sender).Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                CheckBox cbx = (CheckBox)gvr.Cells[0].FindControl("ckb_cont");
                if (cbx != null)
                {
                    string com_came = cbx.Text;
                    string com_empno = ((HiddenField)gvr.Cells[0].FindControl("hf_cont_empno")).Value;
                    ListItem li = new ListItem(com_came, com_empno);
                    if (lbxTempItem_cont.Items.Contains(li))
                    {
                        cbx.Checked = true;
                    }
                }
            }
        }
    }
    protected void gv_contact_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //把換頁前的項目記下來,
        foreach (GridViewRow gvr in ((GridView)sender).Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                CheckBox cbx = (CheckBox)gvr.Cells[0].FindControl("ckb_cont");
                if (cbx != null)
                {
                    string com_came = cbx.Text;
                    string com_empno = ((HiddenField)gvr.Cells[0].FindControl("hf_cont_empno")).Value;
                    ListItem li = new ListItem(com_came, com_empno);
                    if (cbx.Checked)
                    {
                        if (!lbxTempItem_cont.Items.Contains(li))
                        {
                            lbxTempItem_cont.Items.Add(li);
                        }
                    }
                    else
                    {
                        lbxTempItem_cont.Items.Remove(li);
                    }
                }
            }
        }
        gv_contact.PageIndex = e.NewPageIndex;
        ReBindGridViewContact();
    }
    protected void gv_contact_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)//只有在頁首時才進行
        {
            foreach (DataControlField field in gv_contact.Columns)
            {
                if (field.SortExpression == gv_contact.SortExpression && gv_contact.SortExpression != "")//當點選進行排序時才放圖
                {
                    Image obj = new Image();//定義新的圖片物件
                    obj.ImageUrl = ((gv_contact.SortDirection == SortDirection.Ascending) ? "./Images/icon-arrowdesc.gif" : "./Images/icon-arrowasc.gif");//看是升冪還是降冪
                    obj.AlternateText = "ASC";

                    e.Row.Cells[gv_contact.Columns.IndexOf(field)].Controls.Add(obj);//加入圖示
                }
            }
        }
    }
    
    protected void gv_addrbook_PageIndexChanged(object sender, EventArgs e)
    {
        //把換後要把之前的項目勾回去,
        foreach (GridViewRow gvr in ((GridView)sender).Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                CheckBox cbx = (CheckBox)gvr.Cells[0].FindControl("ckb_addrbook");
                if (cbx != null)
                {
                    string com_came = cbx.Text;
                    string com_empno = ((HiddenField)gvr.Cells[0].FindControl("hf_addrbook_empno")).Value;
                    ListItem li = new ListItem(com_came, com_empno);
                    if (lbxTempItem_addrbook.Items.Contains(li))
                    {
                        cbx.Checked = true;
                    }
                }
            }
        }
    }
    protected void gv_addrbook_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //把換頁前的項目記下來,
        foreach (GridViewRow gvr in ((GridView)sender).Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                CheckBox cbx = (CheckBox)gvr.Cells[0].FindControl("ckb_addrbook");
                if (cbx != null)
                {
                    string com_came = cbx.Text;
                    string com_empno = ((HiddenField)gvr.Cells[0].FindControl("hf_addrbook_empno")).Value;
                    ListItem li = new ListItem(com_came, com_empno);
                    if (cbx.Checked)
                    {
                        if (!lbxTempItem_addrbook.Items.Contains(li))
                        {
                            lbxTempItem_addrbook.Items.Add(li);
                        }
                    }
                    else
                    {
                        lbxTempItem_addrbook.Items.Remove(li);
                    }
                }
            }
        }
        gv_addrbook.PageIndex = e.NewPageIndex;
        ReBindGridViewAddrBook();
    }
    protected void gv_addrbook_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)//只有在頁首時才進行
        {
            foreach (DataControlField field in gv_addrbook.Columns)
            {
                if (field.SortExpression == gv_addrbook.SortExpression && gv_addrbook.SortExpression != "")//當點選進行排序時才放圖
                {
                    Image obj = new Image();//定義新的圖片物件
                    obj.ImageUrl = ((gv_addrbook.SortDirection == SortDirection.Ascending) ? "./Images/icon-arrowdesc.gif" : "./Images/icon-arrowasc.gif");//看是升冪還是降冪
                    obj.AlternateText = "ASC";

                    e.Row.Cells[gv_addrbook.Columns.IndexOf(field)].Controls.Add(obj);//加入圖示
                }
            }
        }
    }

    protected void ds_memberlist_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        for (int i = 0; i < e.Command.Parameters.Count; i++)//當參數為空字串時，Parameters會將它設為「Null」，所以需改成「空字串」
        {
            if (e.Command.Parameters[i].Value == null)
                e.Command.Parameters[i].Value = "";
        }
    }
    protected void ds_contact_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        for (int i = 0; i < e.Command.Parameters.Count; i++)//當參數為空字串時，Parameters會將它設為「Null」，所以需改成「空字串」
        {
            if (e.Command.Parameters[i].Value == null)
                e.Command.Parameters[i].Value = "";
        }
    }
    protected void ds_addrbook_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        for (int i = 0; i < e.Command.Parameters.Count; i++)//當參數為空字串時，Parameters會將它設為「Null」，所以需改成「空字串」
        {
            if (e.Command.Parameters[i].Value == null)
                e.Command.Parameters[i].Value = "";
        }
    }
}
