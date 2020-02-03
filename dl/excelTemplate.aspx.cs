using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

public partial class dl_excelTemplate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string template_filename = ConfigUtil.ElfTemplateFile;
        string fpath = Path.Combine(HttpRuntime.AppDomainAppPath, string.Format(@"App_Data\{0}", template_filename));


        try
        {

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + template_filename);


            /*===判斷副檔名*/
            bool IsXls = Path.GetExtension(template_filename).Equals(".xls", StringComparison.OrdinalIgnoreCase);
            if (IsXls)
            {
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            }
            else
            {
                Response.AddHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }


            byte[] xfile = null;
            xfile = File.ReadAllBytes(fpath);
            Response.BinaryWrite(xfile);
        }
        catch (Exception ex)
        {
            Response.Write("Error message, download template excel exception!!");
        }




    }
}