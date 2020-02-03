using System;
using System.Data;
using System.Data.SqlClient;

public partial class SF200_getEmpGroup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string com_deptid = Request["com_deptid"] == null ? "" : Request["com_deptid"].ToString();
        Response.Write(getEmpGroup(com_deptid));
        Response.End();
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
            return DbUtil.ExecCmdGetResult(oCmd).ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }
}
