using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

public partial class project_GetTechItemList : System.Web.UI.Page
{
    Dao_Project db = new Dao_Project();
    protected void Page_Load(object sender, EventArgs e)
    {
        XmlDocument xDoc = new XmlDocument();
        try
        {
            DataTable dt = db.getDashBoardTechItemList();

            string xmlstr = string.Empty;
            xmlstr = DataTableToXml.ConvertDatatableToXmlByAttribute(dt, "root", "rec");
            xmlstr = "<?xml version='1.0' encoding='utf-8'?>" + xmlstr;
            xDoc.LoadXml(xmlstr);
        }
        catch (Exception ex)
        {
            xDoc = ExceptionUtil.GetExceptionDocument(ex);
        }
        Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
        xDoc.Save(Response.Output);
    }
}