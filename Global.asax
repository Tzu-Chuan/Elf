<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // 在應用程式啟動時執行的程式碼

    }

    void Application_End(object sender, EventArgs e)
    {
        //  在應用程式關閉時執行的程式碼

    }

    void Application_Error(object sender, EventArgs e)
    {
        // 在發生未處理的錯誤時執行的程式碼

    }

    void Session_Start(object sender, EventArgs e)
    {
        // 在新的工作階段啟動時執行的程式碼

        try
        {
            /*當測試階段可不執行，可於webconfig修改設定值*/
            if (ConfigUtil.DebugSSOMode == false)
            {
                /*若Session Lost則會重新啟動Session_Start*/
                bool tFlag = false;
                if (HttpContext.Current.Request.Headers["Cookie"] != null && HttpContext.Current.Session.IsNewSession && HttpContext.Current.Request.InputStream.Length != 0)
                {
                    tFlag = true;
                    HttpContext.Current.Session.Abandon();
                }
                else
                {
                    tFlag = false;
                }

                /*啟動新工作階段時執行的程式碼*/
                if (tFlag == false)
                {
                    /*add system login log */
                    Dao_Common.Save_Loginlog(SSOUtil.GetCurrentUser().工號, CommonUtil.GetCurrentUserIP(), DateTime.Now, "登入", "IEKElf", "", SSOUtil.GetCurrentUser().部門);
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("message：session_start error.");
            Response.End();
        }
    }

    void Session_End(object sender, EventArgs e)
    {
        // 在工作階段結束時執行的程式碼
        // 注意: 只有在  Web.config 檔案中將 sessionstate 模式設定為 InProc 時，
        // 才會引起 Session_End 事件。如果將 session 模式設定為 StateServer 
        // 或 SQLServer，則不會引起該事件。

    }
       
</script>
