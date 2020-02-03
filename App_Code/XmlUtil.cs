using System;
using System.Xml.Xsl;
using System.Xml;

public class XmlUtil
{
    public XmlUtil()
    {

    }

    /*============================================================================*/
    /*new xArgs object */
    public static XslCompiledTransform GetXslTransform(string xslPath)
    {
        XmlReaderSettings xrs = new XmlReaderSettings();
        xrs.ProhibitDtd = false;
        XmlReader xr = XmlReader.Create(xslPath, xrs);

        XslCompiledTransform xslDoc = new XslCompiledTransform(false);
        XsltSettings xs = new XsltSettings(true, true);

        xslDoc.Load(xr, xs, new XmlUrlResolver());
        xr.Close();
        xr = null;
        xrs = null;
        xs = null;
        //xslDoc.TemporaryFiles.Delete();
        return xslDoc;
    }

    public static XslCompiledTransform GetXslTransform(XmlDocument xDoc)
    {
        XslCompiledTransform xslDoc = new XslCompiledTransform(false);

        xslDoc.Load(xDoc, null, new XmlUrlResolver());
        return xslDoc;
    }


    /*============================================================================*/
    /*set common xArgs */
    public static XsltArgumentList GetXsltArguments()
    {
        XsltArgumentList xArgs = new XsltArgumentList();
        xArgs.AddParam("AppToday", "", DateTime.Now.ToString("yyyy/MM/dd"));
        xArgs.AddParam("AppNow", "", DateTime.Now.ToString("HH:mm:ss"));
        xArgs.AddParam("AppUtcToday", "", DateTime.UtcNow.ToString("yyyy/MM/dd"));
        xArgs.AddParam("AppUtcNow", "", DateTime.UtcNow.ToString("HH:mm:ss"));
        xArgs.AddParam("AppRoot", "", ConfigUtil.AppRoot);
        xArgs.AddParam("AppTitle", "", ConfigUtil.AppTitle);

        xArgs.AddParam("empno", "", SSOUtil.GetCurrentUser().工號);
        xArgs.AddParam("isSysMgr", "", RightUtil.Get_BaseRight().角色是系統管理人員);
        xArgs.AddParam("isPjMgr", "", RightUtil.Get_BaseRight().角色是專案管理人員);

        return xArgs;
    }

    /*============================================================================*/
}
