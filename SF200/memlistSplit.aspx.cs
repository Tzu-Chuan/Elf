using System;
using System.Data;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

/*==========================================*/
/*說明：modify by asamchang, 2013-5-21 */
/*==========================================*/
public partial class SF200_memlistSplit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /*get req*/
        string xml = Request["xml"] == null ? "" : Request["xml"].ToString();
        string orgno = string.IsNullOrEmpty(Request["orgno"]) ? "" : Request["orgno"].ToString().Trim();/*上一次no*/
        string orgnm = string.IsNullOrEmpty(Request["orgnm"]) ? "" : Request["orgnm"].ToString().Trim();/*上一次name*/

        //parpare new emp
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(xml);

        //parpare old emp
        string[] orgno_array = orgno.Split(',');
        string[] orgnm_array = orgnm.Split(',');
        Dictionary<string, string> orgDi = new Dictionary<string, string>();
        for (int i = 0; i < orgno_array.Length; i++)
        {
            orgDi.Add(orgno_array[i], orgnm_array[i]);
        }

        /*get data*/
        if (xDoc.SelectNodes("/*/*").Count >= 1)
        {
            StringBuilder empnostr = new StringBuilder();
            StringBuilder empnamestr = new StringBuilder();
            StringBuilder empgroupstr = new StringBuilder();
            StringBuilder emptitlestr = new StringBuilder();
            StringBuilder empaddnostr = new StringBuilder();
            StringBuilder empdelnostr = new StringBuilder();

            XmlNodeList xList = xDoc.SelectNodes("/*/*");
            string empno = "", empname = "", email = "", group = "";
            foreach (XmlElement xe in xList)
            {
                empno = xe["com_empno"].InnerText;
                empname = xe["com_cname"].InnerText;
                email = xe["com_email"] == null ? "" : xe["com_email"].InnerText;/*說明：節點可能會沒有出現*/
                group = getEmpGroup(xe["com_deptid"].InnerText);

                empnostr.Append("," + empno);
                empnamestr.Append("," + empname);
                empgroupstr.Append("," + group);
                emptitlestr.AppendFormat(";" + "({{'SamAccountName':'{0}','DisplayName':'{1}','Email':'{2}','Group':'{3}'}})", empno, empname, email, group);

                /*說明：此次的工號不在orgDi內的為此次新增工號, 有在orgDi內的為此次仍保留的工號(將其從orgDi remove)*/
                if (!orgDi.ContainsKey(empno)) { empaddnostr.Append("," + empno); }
                else { orgDi.Remove(empno); }
            }

            /*說明：剩下在orgDi內的為此次被刪除的工號*/
            foreach (KeyValuePair<string, string> pair in orgDi)
            {
                empdelnostr.Append("," + pair.Key);
            }
            orgDi.Clear();

            /*output*/
            Response.Write(empnostr.ToString(1, empnostr.Length - 1)
                + "$$" + empnamestr.ToString(1, empnamestr.Length - 1)
                + "$$" + empgroupstr.ToString(1, empgroupstr.Length - 1)
                + "$$" + emptitlestr.ToString(1, emptitlestr.Length - 1)
                + "$$" + (empaddnostr.Length > 0 ? empaddnostr.ToString(1, empaddnostr.Length - 1) : "")
                + "$$" + (empdelnostr.Length > 0 ? empdelnostr.ToString(1, empdelnostr.Length - 1) : "")
                );
            Response.End();
        }

    }


    public string getEmpGroup(string deptid)
    {
        string strSQL = "";
        SqlCommand oCmd = null;
        try
        {
            strSQL = @"select rtrim(co.org_abbr_egnm)+'-'+rtrim(cd.dep_deptcd) from common..orgcod co inner join common..depcod cd on co.org_orgcd=cd.dep_orgcd where cd.dep_deptid=@deptid";

            oCmd = new SqlCommand(strSQL, DbUtil.GetConn(ConfigUtil.DSN_Common));
            oCmd.Parameters.Add("@deptid", SqlDbType.NVarChar).Value = deptid;
            //return DbUtil.execCmdGetResult(oCmd).ToString();

            Object obj = DbUtil.ExecCmdGetResult(oCmd);
            if (obj != null) return obj.ToString();
            else return "";

        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }




}
