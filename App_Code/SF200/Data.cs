using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace ISCSF200
{

    /// <summary>
    /// Summary description for BusiData
    /// </summary>
    /// 
    namespace Common
    {
        public class Data
        {
            private static string pDBRole = "GrbWebConn";

            public Data()
            {
                //
                // TODO: Add constructor logic here
                //
            }
            private static string getConnectionString(string pDBRole)
            {
                string strConnectionString;

                //使用web.config
                //strConnectionString = getAppString(pDBRole);
                strConnectionString = ConfigurationManager.ConnectionStrings[pDBRole].ConnectionString;

                return strConnectionString;
            }

            public static Int32 ToInt32(string str_Integer)
            {
                return Convert.ToInt32(str_Integer);
            }

            private static void setPara(SqlParameter sqlPara, DataRow dr)
            {
                string strLocation = "Data::SetPara";
                sqlPara.ParameterName = dr["COLUMN_NAME"].ToString();
                switch (dr["TYPE_NAME"].ToString())
                {
                    case "bigint":
                        sqlPara.SqlDbType = SqlDbType.BigInt;
                        break;
                    case "binary":
                        sqlPara.SqlDbType = SqlDbType.Binary;
                        break;
                    case "char":
                        sqlPara.SqlDbType = SqlDbType.Char;
                        sqlPara.Size = ToInt32(dr["LENGTH"].ToString());
                        break;
                    case "datetime":
                        sqlPara.SqlDbType = SqlDbType.DateTime;
                        break;
                    case "decimal":
                        sqlPara.SqlDbType = SqlDbType.Decimal;
                        break;
                    case "float":
                        sqlPara.SqlDbType = SqlDbType.Float;
                        break;
                    case "int":
                        sqlPara.SqlDbType = SqlDbType.Int;
                        break;
                    case "nchar":
                        sqlPara.SqlDbType = SqlDbType.NChar;
                        sqlPara.Size = ToInt32(dr["LENGTH"].ToString());
                        break;
                    case "ntext":
                        sqlPara.SqlDbType = SqlDbType.NText;
                        break;
                    case "nvarchar":
                        sqlPara.SqlDbType = SqlDbType.NVarChar;
                        sqlPara.Size = ToInt32(dr["LENGTH"].ToString());
                        break;
                    case "smallint":
                        sqlPara.SqlDbType = SqlDbType.SmallInt;
                        break;
                    case "text":
                        sqlPara.SqlDbType = SqlDbType.Text;
                        break;
                    case "tinyint":
                        sqlPara.SqlDbType = SqlDbType.TinyInt;
                        break;
                    case "varbinary":
                        sqlPara.SqlDbType = SqlDbType.VarBinary;
                        break;
                    case "varchar":
                        sqlPara.SqlDbType = SqlDbType.VarChar;
                        sqlPara.Size = ToInt32(dr["LENGTH"].ToString());
                        break;
                    default:
                        Exception exErr = new Exception("系統未設定此型別 " + dr["info_name"] + ", 請通知系統人員");
                        exErr.Source = strLocation;
                        throw exErr;
                }

                if (ToInt32(dr["COLUMN_TYPE"].ToString()) == 2)
                    sqlPara.Direction = ParameterDirection.Output;

            }

            #region runScalar(有傳回值)
            /// <summary>
            /// 利用此函式來執行SQL statement, 並回傳單一值		 
            /// </summary>
            /// <param name="pDBRole">使用何種權限的資料庫連線</param>
            /// <param name="pSql">SQL statement</param>
            /// <returns>return object</returns>

            public static Object runScalar(SqlCommand sqlCmd)
            {
                string strLocation = "Data::RunScalar";
                Object obj = null;
                SqlConnection usrcn = new SqlConnection();
                usrcn.ConnectionString = ConfigurationManager.ConnectionStrings[pDBRole].ConnectionString;
                sqlCmd.Connection = usrcn;
                // execute SQL 
                try
                {
                    usrcn.Open();
                    obj = sqlCmd.ExecuteScalar();
                }
                catch (Exception err)
                {
                    err.Source = strLocation;
                    throw err;
                }
                finally
                {
                    usrcn.Close();
                }

                return obj;
            }

            /// <summary>
            /// 利用此函式來執行stored procedure, 並回傳單一值		 
            /// </summary>		
            /// <param name="pDBRole">使用何種權限的資料庫連線</param>
            /// <param name="pSPName">name of stored procedure (input)</param>
            /// <param name="pParas">the value of parameters for Stored Procedure</param>
            /// <returns>return object</returns>

            public static object runScalar(string pSPName)
            {
                string strLocation = "Data::RunScalar()";
                object objResult = null;
                SqlConnection usrcn = new SqlConnection();
                usrcn.ConnectionString = getConnectionString(pDBRole);
                SqlCommand cmd = new SqlCommand();
                // set properties of SqlCommand
                cmd.Connection = usrcn;
                cmd.CommandText = pSPName;
                cmd.CommandType = CommandType.StoredProcedure;
                // execute SQL 
                try
                {
                    usrcn.Open();
                    objResult = cmd.ExecuteScalar();
                }
                catch (Exception err)
                {
                    err.Source = strLocation;
                    throw err;
                }
                finally
                {
                    usrcn.Close();
                }
                usrcn = null;
                cmd = null;
                return objResult;
            }

            public static object runScalar(string pSPName, Object[] pParas)
            {
                string strLocation = "Data::RunScalar()";
                object objResult = null;
                SqlConnection usrcn = new SqlConnection();
                usrcn.ConnectionString = getConnectionString(pDBRole);
                SqlCommand cmd = new SqlCommand();
                SqlParameter[] paras;

                //設定Parameters
                paras = getParameters(pSPName, pParas);

                if (paras.Length != pParas.Length)
                    throw new Exception("參數個數不一致!!");

                // Add Parameter into SqlCommand.SqlParameter
                for (int i = 0; i < paras.Length; i++)
                {
                    cmd.Parameters.Add(paras[i]);
                }


                // set properties of SqlCommand
                cmd.Connection = usrcn;
                cmd.CommandText = pSPName;
                cmd.CommandType = CommandType.StoredProcedure;

                // execute SQL 

                try
                {
                    usrcn.Open();

                    objResult = cmd.ExecuteScalar();
                }
                catch (Exception err)
                {
                    err.Source = strLocation;
                    throw err;
                }
                finally
                {
                    usrcn.Close();
                }

                // Release Resource 
                if (paras != null)
                    cmd.Parameters.Clear();

                usrcn = null;
                cmd = null;


                return objResult;
            }

            #endregion

            #region runParaCmd(有傳回值)
            /// <summary>
            /// 利用此函式來執行SQL statement		 
            /// </summary>
            /// <param name="pDBRole">使用何種權限的資料庫連線</param>
            /// <param name="sqlCmd">SQL statement</param>
            /// <returns>return DataView</returns>
            public static DataView runParaCmd(SqlCommand sqlCmd)
            {
                SqlConnection usrcn = new SqlConnection();
                usrcn.ConnectionString = getConnectionString(pDBRole);
                sqlCmd.Connection = usrcn;
                SqlDataAdapter cmdSQL = new SqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                cmdSQL.Fill(ds, "myTable");

                //Release Resource
                usrcn = null;
                cmdSQL = null;

                return ds.Tables["myTable"].DefaultView;

            }
            #endregion

            #region runSp(有傳回值)
            /// <summary>
            /// 利用此函式來執行stored procedure		 
            /// </summary>
            /// <param name="pDBRole">使用何種權限的資料庫連線</param>
            /// <param name="pSPName">name of stored procedure (input)</param>
            /// <param name="pParas">SqlParameters for the Stored Procedure</param>
            /// <returns>return DataView</returns>
            public static DataView runSp(string pSPName, SqlParameter[] pParas)
            {

                SqlConnection usrcn = new SqlConnection();
                SqlCommand cmd = new SqlCommand();

                //設定Connection String
                usrcn.ConnectionString = getConnectionString(pDBRole);

                // Add Parameter into SqlCommand.SqlParameter
                for (int i = 0; i < pParas.Length; i++)
                {
                    cmd.Parameters.Add(pParas[i]);
                }

                // set properties of SqlCommand
                cmd.Connection = usrcn;
                cmd.CommandText = pSPName;
                cmd.CommandType = CommandType.StoredProcedure;

                // execute SQL and return a dataview
                SqlDataAdapter cmdSQL = new SqlDataAdapter(cmd);
                DataSet myds = new DataSet();
                cmdSQL.Fill(myds, "myTable");

                // Release Resource 
                if (pParas != null)
                    cmd.Parameters.Clear();

                usrcn = null;
                cmd = null;



                return myds.Tables["myTable"].DefaultView;
            }

            /// <summary>
            /// 利用此函式來執行stored procedure		 
            /// </summary>		
            /// <param name="pDBRole">使用何種權限的資料庫連線</param>
            /// <param name="pSPName">name of stored procedure (input)</param>
            /// <param name="pParas">the value of parameters for the Stored Procedure </param>
            /// <returns>return DataView</returns>
            public static DataView runSp(string pSPName, Object[] pParas)
            {
                string strLocation = "Data::RunSp";
                SqlConnection usrcn = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                SqlParameter[] paras;

                //設定Connection String
                usrcn.ConnectionString = getConnectionString(pDBRole);

                //to get Parameters
                paras = getParameters(pSPName, pParas);

                //若參數個數不一致, 則丟出Exception
                if (paras.Length != pParas.Length)
                {
                    Exception exErr = new Exception("參數個數不一致!!");
                    exErr.Source = strLocation;
                    throw exErr;
                }

                // Add Parameter into SqlCommand.SqlParameter		
                for (int i = 0; i < paras.Length; i++)
                {
                    cmd.Parameters.Add(paras[i]);
                }

                // set properties of SqlCommand
                cmd.Connection = usrcn;
                cmd.CommandText = pSPName;
                cmd.CommandType = CommandType.StoredProcedure;

                // execute SQL and return a dataview
                SqlDataAdapter cmdSQL = new SqlDataAdapter(cmd);
                DataSet myds = new DataSet();
                cmdSQL.Fill(myds, "myTable");

                // Release Resource 
                if (paras != null)
                    cmd.Parameters.Clear();

                usrcn = null;
                cmd = null;



                return myds.Tables["myTable"].DefaultView;
            }


            /// <summary>
            /// 利用此函式來執行stored procedure		 
            /// </summary>
            /// <param name="pDBRole">使用何種權限的資料庫連線</param>
            /// <param name="pSPName">name of stored procedure (input)</param>
            /// <returns>return DataView</returns>
            public static DataView runSp(string pDBRole, string pSPName)
            {

                SqlConnection usrcn = new SqlConnection();
                SqlCommand cmd = new SqlCommand();

                //設定Connection String
                usrcn.ConnectionString = getConnectionString(pDBRole);


                // set properties of SqlCommand
                cmd.Connection = usrcn;
                cmd.CommandText = pSPName;
                cmd.CommandType = CommandType.StoredProcedure;

                // execute SQL and return a dataview
                SqlDataAdapter cmdSQL = new SqlDataAdapter(cmd);
                DataSet myds = new DataSet();
                cmdSQL.Fill(myds, "myTable");

                // Release Resource 
                usrcn = null;
                cmd = null;

                return myds.Tables["myTable"].DefaultView;
            }
            #endregion

            #region runParaCmd1(無傳回值)
            /// <summary>
            /// 利用此函式來執行SQL statement(update)		 
            /// </summary>
            /// <param name="pDBRole">使用何種權限的資料庫連線</param>
            /// <param name="sqlCmd">SQL statement</param>
            public static void runParaCmd1(SqlCommand sqlCmd)
            {
                SqlConnection usrcn = new SqlConnection();
                usrcn.ConnectionString = getConnectionString(pDBRole);
                sqlCmd.Connection = usrcn;
                usrcn.Open();
                sqlCmd.ExecuteNonQuery();
                usrcn.Close();

                // Release Resource 
                usrcn = null;
                sqlCmd = null;
            }

            #endregion

            #region RunSp1(無傳回值)
            /// <summary>
            /// 利用此函式來執行stored procedure		 
            /// </summary>
            /// <param name="pDBRole">使用何種權限的資料庫連線</param>
            /// <param name="pSPName">name of stored procedure (input)</param>
            /// <param name="pPara">the value of parameters for the Stored Procedure (input)</param>
            public static void runSp1(string pSPName, SqlParameter[] pParas)
            {
                SqlConnection usrcn = new SqlConnection();
                usrcn.ConnectionString = getConnectionString(pDBRole);
                SqlCommand cmd = new SqlCommand();

                // Add Parameter into SqlCommand.SqlParameter
                for (int i = 0; i < pParas.Length; i++)
                {
                    cmd.Parameters.Add(pParas[i]);
                }


                // set properties of SqlCommand
                cmd.Connection = usrcn;
                cmd.CommandText = pSPName;
                cmd.CommandType = CommandType.StoredProcedure;

                // execute SQL 
                usrcn.Open();

                cmd.ExecuteNonQuery();

                usrcn.Close();

                // Release Resource 
                if (pParas != null)
                    cmd.Parameters.Clear();

                usrcn = null;
                cmd = null;
            }

            /// <summary>
            /// 利用此函式來執行stored procedure
            /// </summary>		
            /// <param name="pDBRole">使用何種權限的資料庫連線</param>
            /// <param name="pSPName">name of stored procedure (input)</param>
            /// <param name="pParas">the value of parameters for the Stored Procedure (input)</param>
            public static void runSp1(string pSPName, Object[] pParas)
            {
                string strLocation = "Data::RunSp1";
                SqlConnection usrcn = new SqlConnection();
                usrcn.ConnectionString = getConnectionString(pDBRole);
                SqlCommand cmd = new SqlCommand();
                SqlParameter[] paras;

                //設定Parameters
                paras = getParameters(pSPName, pParas);

                if (paras.Length != pParas.Length)
                {
                    Exception exErr = new Exception("參數個數不一致!!");
                    exErr.Source = strLocation;
                    throw exErr;
                }

                // Add Parameter into SqlCommand.SqlParameter
                for (int i = 0; i < paras.Length; i++)
                {
                    cmd.Parameters.Add(paras[i]);
                }


                // set properties of SqlCommand
                cmd.Connection = usrcn;
                cmd.CommandText = pSPName;
                cmd.CommandType = CommandType.StoredProcedure;

                // execute SQL 
                usrcn.Open();

                cmd.ExecuteNonQuery();

                usrcn.Close();

                // Release Resource 
                if (paras != null)
                    cmd.Parameters.Clear();

                usrcn = null;
                cmd = null;
            }

            /// <summary>
            /// 利用此函式來執行stored procedure
            /// </summary>
            /// <param name="pDBRole">使用何種權限的資料庫連線</param>
            /// <param name="pSPName">name of stored procedure (input)</param>
            public static void runSp1(string pSPName)
            {
                SqlConnection usrcn = new SqlConnection();
                usrcn.ConnectionString = ConfigurationManager.ConnectionStrings[pDBRole].ConnectionString;
                SqlCommand cmd = new SqlCommand();

                // set properties of SqlCommand
                cmd.Connection = usrcn;
                cmd.CommandText = pSPName;
                cmd.CommandType = CommandType.StoredProcedure;

                // execute SQL 
                usrcn.Open();

                cmd.ExecuteNonQuery();

                usrcn.Close();

                // Release Resource 
                usrcn = null;
                cmd = null;
            }
            #endregion

            #region 公用function
            /// <summary>
            /// 利用此函式來get Stored Procedured的參數
            /// </summary>
            /// <param name="pDBRole">使用何種權限的資料庫連線</param>		
            /// <param name="pSPName">Stored Procedure Name</param>
            /// <param name="pParas">Parameter Values</param>
            /// <returns>return SqlParameter[]</returns>	
            public static SqlParameter[] getParameters(string pSPName, object[] pParas)
            {
                string strLocation = "Data::GetParameters";
                SqlParameter[] Paras = new SqlParameter[pParas.Length];
                DataView dvMain;

                SqlParameter[] myParas = new SqlParameter[1];
                myParas[0] = new SqlParameter("@procedure_name", SqlDbType.NVarChar, 128);
                myParas[0].Value = pSPName;


                dvMain = runSp("sp_sproc_columns", myParas);

                //判斷parameters的參數個數是否正確
                if ((dvMain.Count - 1) != pParas.Length)
                {
                    Exception exErr = new Exception("參數個數不一致!!");
                    exErr.Source = strLocation;
                    throw exErr;
                }

                for (int i = 0; i < pParas.Length; i++)
                {
                    Paras[i] = new SqlParameter();
                    setPara(Paras[i], dvMain.Table.Rows[i + 1]);
                    Paras[i].Value = pParas[i];
                }

                //Release Resource
                dvMain = null;
                myParas = null;

                return Paras;
            }

            #endregion

        }
    }
}