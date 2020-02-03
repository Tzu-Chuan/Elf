using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;

// ====================================================================================
// 異動時間: 2013/02
// 作    者: asamchang
// 功    能: 
// ====================================================================================
public class DbUtil
{
    /*============================================================================*/
    private static string SelfDataTimePattern = "yyyy-MM-dd";

    /*============================================================================*/
    public static SqlConnection GetConn()
    {
        try
        {
            return new SqlConnection(ConfigUtil.DSN_Default);
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }
   
    public static SqlConnection GetConn(string DataSourceString)
    {
        try
        {
            return new SqlConnection(DataSourceString);
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
    }

    //////public static SqlConnection GetConn()
    //////{
    //////    return new SqlConnection(ConfigUtil.DSN_Default);
    //////}

    /*============================================================================*/
    public static XmlDocument GetDB2Xml(SqlCommand oCmd)
    {
        return GetDB2Xml(oCmd, SelfDataTimePattern, 0, 0);
    }
    public static XmlDocument GetDB2Xml(SqlCommand oCmd, string DataTimePattern)
    {
        return GetDB2Xml(oCmd, DataTimePattern, 0, 0);
    }
    public static XmlDocument GetDB2Xml(SqlCommand oCmd, int recStart, int recCount)
    {
        return GetDB2Xml(oCmd, SelfDataTimePattern, recStart, recCount);
    }
    public static XmlDocument GetDB2Xml(SqlCommand oCmd, string DataTimePattern, int recStart, int recCount)
    {
        try
        {
            /*step1:check connection*/
            if (oCmd.Connection == null)
            {
                throw new Exception("發生錯誤, Connection is null");
            }

            /*step2:call Db2Xml and return xml doc*/
            Db2Xml d2x = new Db2Xml();
            d2x.DataTimePattern = DataTimePattern;
            d2x.CountTotal = true;
            d2x.ShowSeriesNo = true;
            return d2x.GetXmlDocument(oCmd, recStart, recCount);
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
        finally
        {
            if (oCmd.Connection.State != ConnectionState.Closed)
            {
                oCmd.Connection.Close();
            }
        }
    }

    /*============================================================================*/
    public static XmlDocument GetDB2Xml(SqlCommand oCmd, SqlConnection oConn)
    {
        return GetDB2Xml(oCmd, oConn, SelfDataTimePattern, 0, 0);
    }
    public static XmlDocument GetDB2Xml(SqlCommand oCmd, SqlConnection oConn, string DataTimePattern)
    {
        return GetDB2Xml(oCmd, oConn, DataTimePattern, 0, 0);
    }
    public static XmlDocument GetDB2Xml(SqlCommand oCmd, SqlConnection oConn, int recStart, int recCount)
    {
        return GetDB2Xml(oCmd, oConn, SelfDataTimePattern, recStart, recCount);
    }
    public static XmlDocument GetDB2Xml(SqlCommand oCmd, SqlConnection oConn, string DataTimePattern, int recStart, int recCount)
    {
        try
        {
            /*step1:check connection*/
            if (oConn == null)
            {
                throw new Exception("發生錯誤, Connection is null");
            }

            /*step2:call Db2Xml and return xml doc*/
            Db2Xml d2x = new Db2Xml(oConn);
            d2x.DataTimePattern = DataTimePattern;
            d2x.CountTotal = true;
            d2x.ShowSeriesNo = true;
            return d2x.GetXmlDocument(oCmd, recStart, recCount);
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
        finally
        {
            if (oCmd.Connection.State != ConnectionState.Closed)
            {
                oCmd.Connection.Close();
            }
        }
    }

    /*============================================================================*/
    public static int ExecCmdNoResult(SqlCommand oCmd)
    {
        int effectNum = 0;
        try
        {
            /*step1:check connection*/
            if (oCmd.Connection == null)
            {
                throw new Exception("發生錯誤, Connection is null");
            }

            /*step2:exec*/
            oCmd.Connection.Open();
            effectNum = oCmd.ExecuteNonQuery();
            oCmd.Connection.Close();

            return effectNum;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
        finally
        {
            if (oCmd.Connection.State != ConnectionState.Closed)
            {
                oCmd.Connection.Close();
            }
        }
    }
    public static Object ExecCmdGetResult(SqlCommand oCmd)
    {
        Object obj = null;
        try
        {
            /*step1:check connection*/
            if (oCmd.Connection == null)
            {
                throw new Exception("發生錯誤, Connection is null");
            }
            /*step3:exec and get object result*/
            oCmd.Connection.Open();
            obj = oCmd.ExecuteScalar();
            oCmd.Connection.Close();

            return obj;
        }
        catch (Exception ex)
        {
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
        finally
        {
            if (oCmd.Connection.State != ConnectionState.Closed)
            {
                oCmd.Connection.Close();
            }
        }

    }
    public static int ExecCmdTranNoResult(SqlCommand[] oCmd, SqlConnection oConn)
    {
        int[] effectNumObj = new int[oCmd.Length];
        oConn.Open();
        SqlTransaction oTrans = oConn.BeginTransaction();
        try
        {
            for (int i = 0; i < oCmd.Length; i++)
            {
                if (oCmd[i] != null && oCmd[i].CommandText != "")
                {
                    oCmd[i].Connection = oConn;
                    oCmd[i].Transaction = oTrans;
                    effectNumObj[i] = oCmd[i].ExecuteNonQuery();
                }
            }

            oTrans.Commit();
            oConn.Close();
            oTrans.Dispose();

            int effectNum = 0;
            for (int i = 0; i < effectNumObj.Length; i++) { effectNum += Convert.ToInt32(effectNumObj[i]); }
            return effectNum;
        }
        catch (Exception ex)
        {
            oTrans.Rollback();
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
        finally
        {
            if (oConn.State != ConnectionState.Closed)
            {
                oConn.Close();
            }
        }


    }
    public static Object[] ExecCmdTranGetResult(SqlCommand[] oCmd, SqlConnection oConn)
    {
        Object[] obj = new object[oCmd.Length];
        oConn.Open();
        SqlTransaction oTrans = oConn.BeginTransaction();
        try
        {
            for (int i = 0; i < oCmd.Length; i++)
            {
                if (oCmd[i] != null && oCmd[i].CommandText != "")
                {
                    oCmd[i].Connection = oConn;
                    oCmd[i].Transaction = oTrans;
                    obj[i] = oCmd[i].ExecuteScalar();
                }
            }
            oTrans.Commit();
            oConn.Close();
            oTrans.Dispose();
            return obj;
        }
        catch (Exception ex)
        {
            oTrans.Rollback();
            throw new Exception(CommonUtil.GetCurrLocationMsg(ex));
        }
        finally
        {
            if (oConn.State != ConnectionState.Closed)
            {
                oConn.Close();
            }
        }

    }

    /*============================================================================*/
    private static SqlCommand CmdStr2Obj(string CommandText)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = CommandText;
        return cmd;
    }

    /*============================================================================*/
}

