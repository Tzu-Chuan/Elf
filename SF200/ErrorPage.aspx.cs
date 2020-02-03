using System;

public partial class ErrorPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.lblMessage.Text = "系統元件發生錯誤，請聯絡系統管理者，謝謝!";

        //switch (Request["err"])
        //{
        //    case "au":
        //        this.lblMessage.Text = "您的權限不足!";
        //        break;

        //    case "err":
        //        this.lblMessage.Text = "系統發生錯誤!";
        //        break;

        //    case "par":
        //        this.lblMessage.Text = "參數錯誤!";
        //        break;
        //}
    }
}
