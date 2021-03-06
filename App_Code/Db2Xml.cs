using System;
using System.Data;
using System.Xml;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Globalization;
using System.Data.SqlClient;

/// <summary>
/// 本程式碼版權屬Dick所有, 任何人非經同意不得任意散發或修改。
/// </summary>
public class Db2Xml : IDisposable
{
    // ------------------------------------------------------------------------------------
    // 異動時間: 2007/01/31
    // 作    者: Dick
    // 完 成 度: 99%
    // 功    能: Db2Xml Instance
    //	備    註:
    //       *. 
    // ------------------------------------------------------------------------------------
    //	程式說明: 
    //       *. 
    //       *. 
    //       *. 
    // ------------------------------------------------------------------------------------
    // 主要參數說明:
    //       *. recStart: 資料開始必須大於等於零
    //       *. recCount: 如果資料筆數小於等於零, 表示要讀取全部資料
    //       *. rootName: 根節點名稱預設值為 "root"
    //       *. recordName: 每筆資料的預設標籤名稱為 "rec"
    //       *. 
    // 回傳資料參數說明:
    //       *. @RecordStart: 資料開始位置
    //       *. @MaxRecord: 最大的資料擷取筆數
    //       *. @RecordCount: 實際擷取的資料筆數
    //       *. @TotalCount: 總資料筆數
    //       *. 
    //       *. 
    //       *. 
    // ------------------------------------------------------------------------------------

    #region /* 建構子 */

    #endregion

    public Db2Xml()
    {
        // ------------------------------------------------------------------------------------
        // 異動時間: 2007/01/31
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 建構函式的程式碼
        // 回 傳 值: 無
        // 參    數: 
        //       *. 
        //	備    註:
        //       *. 
        // ------------------------------------------------------------------------------------
        //	程式說明: 
        //       *. 
        // ------------------------------------------------------------------------------------
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbConnection">Default DbConnection</param>
    public Db2Xml(SqlConnection dbConnection)
    {
        // ------------------------------------------------------------------------------------
        // 異動時間: 2007/01/31
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 建構函式的程式碼
        // 回 傳 值: 無
        // 參    數: 
        //       *. SqlConnection dbConnection: DataBase Connection (SqlConnection)
        //	備    註:
        //       *. 
        // ------------------------------------------------------------------------------------
        //	程式說明: 
        //       *. 
        // ------------------------------------------------------------------------------------
        this.DbConnection = dbConnection;
    }



    /// <summary>
    /// 預設的 DbConnection, 當 DbCommand 沒有 DbConnection 值時, 會以此執行
    /// </summary>
    SqlConnection _oConn = null; // 
    public SqlConnection DbConnection
    {
        // ------------------------------------------------------------------------------------
        // 異動時間: 2007/01/31
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 取得或設定預設的 DbConnection
        // 回 傳 值: SqlConnection value
        // 參    數: SqlConnection value
        // ------------------------------------------------------------------------------------
        //	摘    要: 
        //	備    註: 
        // ------------------------------------------------------------------------------------
        get
        {
            return _oConn;
        }
        set
        {
            _oConn = value;
        }
    }



    /// <summary>
    /// 根節點的預設名稱
    /// </summary>
    string _NameOfRoot = "root";
    public string NameOfRoot
    {
        get
        {
            return _NameOfRoot;
        }
        set
        {
            _NameOfRoot = XmlConvert.VerifyNCName(value);
        }
    }


    /// <summary>
    /// 記錄節點的預設名稱
    /// </summary>
    string _NameOfRecord = "rec";
    public string NameOfRecord
    {
        get
        {
            return _NameOfRecord;
        }
        set
        {
            _NameOfRecord = XmlConvert.VerifyNCName(value);
        }
    }


    /// <summary>
    /// 是否統計全部的資料筆數, 預設為 fasle, 在大量資料時, 計算總筆數會有效能的考量
    /// </summary>
    public bool CountTotal = false;

    /// <summary>
    /// 使用 Element 來表現欄位資料, Element 的名稱即為欄位名稱
    /// </summary>
    public bool UseElement = false;

    /// <summary>
    /// 是否把資料開始位置, 最大的資料擷取筆數, 實際擷取的資料筆數, 總資料筆數等相關訊息填入指定的根元素中
    /// </summary>
    public bool FillInfo = true;

    /// <summary>
    /// 是否顯示資料序號, 起始值為 1, 預設為 false
    /// </summary>
    public bool ShowSeriesNo = false; // 

    /// <summary>
    /// 當顯示資料序號功能啟動時, 資料序號的預設欄位名稱, 預設為 No
    /// </summary>
    string _SeriesName = "No"; // 
    public string SeriesName // 資料序號的欄位名稱
    {
        // ------------------------------------------------------------------------------------
        //	異動時間: 2006/05/16
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 資料序號的欄位名稱
        // 回 傳 值: 無
        // 參    數: 
        //       *. 
        //	備    註:
        //       *. 
        // ------------------------------------------------------------------------------------
        //	程式說明: 
        //       *. 
        // ------------------------------------------------------------------------------------

        get
        {
            return _SeriesName;
        }
        set
        {
            _SeriesName = XmlConvert.VerifyNCName(value);
        }
    }


    /// <summary>
    /// 是否顯示值為 null 的資料, 預設為 true
    /// </summary>
    public bool ShowNull = true;

    /// <summary>
    /// 如果欄位值為 null 要顯示的字串, 預設為空字串
    /// </summary>
    string _NullString = "";
    public string NullString // 當資料為 null 時的顯示字串
    {
        // ------------------------------------------------------------------------------------
        // 異動時間: 2006/06/12
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 當資料為 null 時的替代字串
        // 回 傳 值: string value
        // 參    數: string value
        // ------------------------------------------------------------------------------------
        //	摘    要: 
        //	備    註: 
        // ------------------------------------------------------------------------------------
        get
        {
            return _NullString;
        }
        set
        {
            _NullString = (value != null) ? value : "";
        }
    }


    /// <summary>
    /// 是否把不合格的 XML 字元濾除, 預設為不要以免降低效能
    /// </summary>
    public bool FilterInvalidXmlCahrs = false;

    /// <summary>
    /// Field's Patterns, use FieldName-PatternValue, No data type check
    /// </summary>
    public StringDictionary FieldPatterns = new StringDictionary();

    /// <summary>
    /// 資料輸出時否執行格式化, 預設為 true
    /// </summary>
    public bool FormatData = true;

    /// <summary>
    /// 是否利用指定欄位名稱的方式來格式化資料
    /// </summary>
    public bool FormatWithPatterns = false;

    /// <summary>
    /// 是否加入型別描述的資料
    /// </summary>
    public bool WithSchema = false;



    /// <summary>
    /// 在現有條件下, 是否還有更多的資料未擷取
    /// </summary>
    bool _MoreData = false;
    public bool MoreData
    {
        get
        {
            return _MoreData;
        }
    }



    #region /* properties */
    #endregion


    /// <summary>
    /// 設定時間顯示的格式, 與 DateTime.ToString("FormatString") 相同
    /// </summary>
    string _DateTimeFormat = null;
    public string DataTimePattern
    {
        // ------------------------------------------------------------------------------------
        // 異動時間: 2006/06/12
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 設定時間顯示的格式, 與 DateTime.ToString("FormatString") 相同
        // 回 傳 值: string value
        // 參    數: string value
        // ------------------------------------------------------------------------------------
        //	摘    要: 
        //	備    註: 
        // ------------------------------------------------------------------------------------
        get
        {
            return _DateTimeFormat;
        }
        set
        {
            _DateTimeFormat = value;
        }
    }


    /// <summary>
    /// 設定整數值顯示的格式, 與 Number.ToString("FormatString") 相同
    /// </summary>
    string _IntegerFormat = null;
    public string IntegerPattern
    {
        // ------------------------------------------------------------------------------------
        // 異動時間: 2006/06/12
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 設定整數值顯示的格式, 與 Number.ToString("FormatString") 相同
        // 回 傳 值: string value
        // 參    數: string value
        // ------------------------------------------------------------------------------------
        //	摘    要: 
        //	備    註: 
        // ------------------------------------------------------------------------------------
        get
        {
            return _IntegerFormat;
        }
        set
        {
            _IntegerFormat = value;
        }
    }


    /// <summary>
    /// 設定Byte型別資料的格式, 與 Byte.ToString("FormatString") 相同
    /// </summary>
    string _ByteFormat = null;
    public string BytePattern
    {
        // ------------------------------------------------------------------------------------
        // 異動時間: 2006/06/12
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 設定整數值顯示的格式, 與 Number.ToString("FormatString") 相同
        // 回 傳 值: string value
        // 參    數: string value
        // ------------------------------------------------------------------------------------
        //	摘    要: 
        //	備    註: 
        // ------------------------------------------------------------------------------------
        get
        {
            return _ByteFormat;
        }
        set
        {
            _ByteFormat = value;
        }
    }


    /// <summary>
    /// 設定實數(浮點數)值顯示的格式, 與 Number.ToString("FormatString") 相同
    /// </summary>
    string _FloatFormat = null;
    public string FloatPattern
    {
        // ------------------------------------------------------------------------------------
        // 異動時間: 2006/06/12
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 設定實數(浮點數)值顯示的格式, 與 Number.ToString("FormatString") 相同
        // 回 傳 值: string value
        // 參    數: string value
        // ------------------------------------------------------------------------------------
        //	摘    要: 
        //	備    註: 
        // ------------------------------------------------------------------------------------
        get
        {
            return _FloatFormat;
        }
        set
        {
            _FloatFormat = value;
        }
    }


    /// <summary>
    /// 設定GUIDGUID 型別資料的格式, 與 GUID.ToString("FormatString") 相同
    /// </summary>
    string _GuidFormat = null;
    public string GuidPattern
    {
        // ------------------------------------------------------------------------------------
        // 異動時間: 2006/06/12
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 設定實數(浮點數)值顯示的格式, 與 Number.ToString("FormatString") 相同
        // 回 傳 值: string value
        // 參    數: string value
        // ------------------------------------------------------------------------------------
        //	摘    要: 
        //	備    註: 
        // ------------------------------------------------------------------------------------
        get
        {
            return _GuidFormat;
        }
        set
        {
            _GuidFormat = value;
        }
    }




    #region /* methods */
    #endregion
    //aaaaa
    private XmlDocument NewXmlDocument(string rootName)
    {
        // ------------------------------------------------------------------------------------
        // 異動時間: 2007/01/31
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 產生新的 XmlDocument 
        // 回 傳 值: XmlDOcument
        // 參    數: string rootName
        // ------------------------------------------------------------------------------------
        //	摘    要: 
        //	備    註: 
        XmlDocument xDoc = new XmlDocument();
        xDoc.PreserveWhitespace = false;
        xDoc.AppendChild(xDoc.CreateElement(rootName));
        return xDoc;
    }


    void SetFormatInfo(IDataRecord oRs, out string[] fieldNames, out FieldType[] types, out string[] patterns)
    {

        int fieldCount = oRs.FieldCount;

        // 設定資料欄位名稱資訊
        fieldNames = new string[fieldCount];
        for (int i = 0; i < fieldCount; i++)
        {
            fieldNames[i] = oRs.GetName(i);
        }

        // 設定資料欄位型態資訊
        types = GetFieldTypes(oRs);

        // 設定資料欄位格式化字串
        if (this.FormatWithPatterns)
        {
            patterns = GetFieldPatterns(fieldNames);
        }
        else
        {
            patterns = GetSimplePattern(types);
        }
    }


    string[] GetFieldPatterns(string[] fieldNames)
    {
        int fieldCount = fieldNames.Length; // 欄位總數

        string[] patterns = new string[fieldCount]; // 
        for (int i = 0; i < fieldCount; i++)
        {
            patterns[i] = FieldPatterns[fieldNames[i]];
        }
        return patterns;
    }


    string[] GetSimplePattern(FieldType[] types)
    {
        // ------------------------------------------------------------------------------------
        // 異動時間: 2007/01/31
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 取得欄位的格式化資訊
        // 回 傳 值: void
        // 參    數: 
        //       *. 
        //	備    註:
        //       *. 
        // ------------------------------------------------------------------------------------
        //	程式說明: 
        //       *. 
        // ------------------------------------------------------------------------------------
        int fieldCount = types.Length; // 欄位總數

        string[] patterns = new string[fieldCount]; // 

        for (int i = 0; i < fieldCount; i++)
        {
            if (!this.FormatData)
            {
                // 資料輸出不要格式化
                patterns[i] = null;
                continue;
            }

            switch (types[i])
            {
                case FieldType.DateTime:
                    {
                        patterns[i] = this.DataTimePattern;
                        break;
                    }
                case FieldType.Int16:
                    {
                        patterns[i] = this.IntegerPattern;
                        break;
                    }
                case FieldType.Int32:
                    {
                        patterns[i] = this.IntegerPattern;
                        break;
                    }
                case FieldType.Int64:
                    {
                        patterns[i] = this.IntegerPattern;
                        break;
                    }
                //					case FieldType.UInt16:
                //					{
                //						patterns[i] = this.IntegerPattern;
                //						break;
                //					}
                //					case FieldType.UInt32:
                //					{
                //						patterns[i] = this.IntegerPattern;
                //						break;
                //					}
                //					case FieldType.UInt64:
                //					{
                //						patterns[i] = this.IntegerPattern;
                //						break;
                //					}
                case FieldType.Single:
                    {
                        patterns[i] = this.FloatPattern;
                        break;
                    }
                case FieldType.Double:
                    {
                        patterns[i] = this.FloatPattern;
                        break;
                    }
                case FieldType.Decimal:
                    {
                        patterns[i] = this.FloatPattern;
                        break;
                    }
                case FieldType.Byte:
                    {
                        patterns[i] = this.BytePattern;
                        break;
                    }
                case FieldType.Guid:
                    {
                        patterns[i] = this.GuidPattern;
                        break;
                    }
                default:
                    {
                        patterns[i] = null;
                        break;
                    }
            }
        }
        return patterns;
    }


    FieldType[] GetFieldTypes(IDataRecord oRs)
    {
        // ------------------------------------------------------------------------------------
        // 異動時間: 2007/01/31
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 取得每個欄位 FieldType
        // 回 傳 值: FieldType[]
        // 參    數: 
        //     *. 
        //	備    註:
        //     *. 
        // ------------------------------------------------------------------------------------
        //	程式說明: 
        //     *. 
        // ------------------------------------------------------------------------------------
        int fieldCount = oRs.FieldCount; // 欄位總數
        FieldType[] types = new FieldType[fieldCount];
        for (int i = 0; i < fieldCount; i++)
        {
            types[i] = GetFieldType(oRs.GetFieldType(i));
        }
        return types;
    }


    /// <summary>
    /// 以 attribute 的方式把資料填入 XML 節點文件中  
    /// </summary>
    /// <param name="xNode"></param>
    /// <param name="RecNo"></param>
    /// <param name="oRs"></param>
    /// <param name="fieldName"></param>
    /// <param name="types"></param>
    /// <param name="patterns"></param>
    //aaaaa
    void FillAsAttribute(XmlElement xNode, int RecNo, IDataRecord oRs, string[] fieldName, FieldType[] types, string[] patterns)
    {
        //	------------------------------------------------------------------------------------
        // 異動時間: 2007/01/31
        // 完 成 度: 99%
        // 作    者: Dick
        // 功    能: 以 attribute 的方式把資料填入 XML 節點文件中
        //	摘    要: 
        //	備    註: 
        //	------------------------------------------------------------------------------------
        int fieldCount = fieldName.Length; // 資料欄位數目
        if (this.ShowSeriesNo) // 顯示資料編號
        {
            xNode.SetAttribute(_SeriesName, RecNo.ToString());
        }
        if (FormatData)
        {
            for (int i = 0; i < fieldCount; i++)
            {
                if (oRs.IsDBNull(i))
                {
                    if (this.ShowNull)
                    {
                        xNode.SetAttribute(fieldName[i], _NullString);
                    }
                }
                else
                {
                    xNode.SetAttribute(fieldName[i], ConvertToString(oRs, i, types, patterns)); // null value will fill empty string
                    //						xNode.SetAttribute(fieldName[i], ConvertToString(types[i], patterns[i], oRs.GetValue(i))); // null value will fill empty string
                }
            }
        }
        else
        {
            for (int i = 0; i < fieldCount; i++)
            {
                if (oRs.IsDBNull(i))
                {
                    if (this.ShowNull)
                    {
                        xNode.SetAttribute(fieldName[i], _NullString);
                    }
                }
                else
                {
                    xNode.SetAttribute(fieldName[i], oRs.GetValue(i).ToString()); // null value will fill empty string
                }
            }
        }
    }


    /// <summary>
    /// 以 element 的方式把資料填入 XML 節點文件中 
    /// </summary>
    /// <param name="xNode"></param>
    /// <param name="RecNo"></param>
    /// <param name="oRs"></param>
    /// <param name="fieldName"></param>
    /// <param name="types"></param>
    /// <param name="patterns"></param>
    void FillAsElement(XmlElement xNode, int RecNo, IDataRecord oRs, string[] fieldName, FieldType[] types, string[] patterns)
    {
        //	------------------------------------------------------------------------------------
        // 異動時間: 2007/01/31
        // 完 成 度: 99%
        // 作    者: Dick
        // 功    能: 以 element 的方式把資料填入 XML 節點文件中 
        //	摘    要: 
        //	備    註: 
        //	------------------------------------------------------------------------------------
        XmlDocument xDoc = xNode.OwnerDocument;
        XmlElement xNew = null;
        int fieldCount = fieldName.Length; // 資料欄位數目
        if (this.ShowSeriesNo) // 顯示資料編號
        {
            xNew = xDoc.CreateElement(_SeriesName);
            xNode.AppendChild(xNew);
            xNew.InnerText = RecNo.ToString();
        }
        if (FormatData)
        {
            for (int i = 0; i < fieldCount; i++)
            {
                xNew = xDoc.CreateElement(fieldName[i]);
                xNode.AppendChild(xNew);
                if (oRs.IsDBNull(i))
                {
                    if (this.ShowNull)
                    {
                        xNew.InnerText = _NullString;
                    }
                }
                else
                {
                    xNew.InnerText = ConvertToString(oRs, i, types, patterns); // null value will fill empty string
                    //						xNew.InnerText = ConvertToString(types[i], patterns[i], oRs.GetValue(i)); // null value will fill empty string
                }
            }
        }
        else
        {
            for (int i = 0; i < fieldCount; i++)
            {
                xNew = xDoc.CreateElement(fieldName[i]);
                xNode.AppendChild(xNew);
                if (oRs.IsDBNull(i))
                {
                    if (this.ShowNull)
                    {
                        xNew.InnerText = _NullString;
                    }
                }
                else
                {
                    xNew.InnerText = oRs.GetValue(i).ToString(); // null value will fill empty string
                }
            }
        }
    }


    /// <summary>
    /// 把資料轉換成字串輸出
    /// </summary>
    /// <param name="oRs"></param>
    /// <param name="fieldIndex"></param>
    /// <param name="fieldTypes"></param>
    /// <param name="patterns"></param>
    /// <returns></returns>
    string ConvertToString(IDataRecord oRs, int fieldIndex, FieldType[] fieldTypes, string[] patterns)
    {
        int i = fieldIndex;
        switch (fieldTypes[i])
        {
            case FieldType.Boolean:
                {
                    return oRs.GetBoolean(i).ToString();
                }
            case FieldType.Byte:
                {
                    return oRs.GetByte(i).ToString(patterns[i], NumberFormatInfo.InvariantInfo);
                }
            case FieldType.Bytes:
                {
                    return oRs.GetValue(i).ToString();
                }
            case FieldType.DateTime:
                {
                    return oRs.GetDateTime(i).ToString(patterns[i], DateTimeFormatInfo.InvariantInfo);
                }
            case FieldType.Decimal:
                {
                    return oRs.GetDecimal(i).ToString(patterns[i], NumberFormatInfo.InvariantInfo);
                }
            case FieldType.Double:
                {
                    return oRs.GetDouble(i).ToString(patterns[i], NumberFormatInfo.InvariantInfo);
                }
            case FieldType.Guid:
                {
                    return oRs.GetGuid(i).ToString(patterns[i], NumberFormatInfo.InvariantInfo);
                }
            case FieldType.Int16:
                {
                    return oRs.GetInt16(i).ToString(patterns[i], NumberFormatInfo.InvariantInfo);
                }
            case FieldType.Int32:
                {
                    return oRs.GetInt32(i).ToString(patterns[i], NumberFormatInfo.InvariantInfo);
                }
            case FieldType.Int64:
                {
                    return oRs.GetInt64(i).ToString(patterns[i], NumberFormatInfo.InvariantInfo);
                }
            case FieldType.Single:
                {
                    return oRs.GetFloat(i).ToString(patterns[i], NumberFormatInfo.InvariantInfo);
                }
            case FieldType.String:
                {
                    return oRs.GetString(i);
                }
            case FieldType.UnKnown:
                {
                    return oRs.GetValue(i).ToString();
                }

        }
        return null;
    }


    /// <summary>
    /// 移除不合格的 XML 字元
    /// </summary>
    /// <param name="xmlText"></param>
    /// <returns></returns>
    string FilterXmlString(string xmlText)
    {
        // ------------------------------------------------------------------------------------
        // 異動時間: 2007/01/31
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 把不合法 (XML) 的字元移除
        // 回 傳 值: String: 移除不合法字元後的字串
        // 參    數: 
        //       *. 
        //	備    註: 
        // 備    註: 
        //       *. XML合格字元範圍如下:
        //          [9 : 0x9], [10 : 0xA], [13 : 0xD], [32 : 0x20] - [55295 : 0xD7FF], 
        //          [57344 : 0xE000] - [65533 : 0xFFFD], [65536 : 0x10000] - [1114111 : 0x10FFFF] 
        //       *. 由於現存文字區均未超過 65535, 所以只要以 int32 來處理即可
        // ------------------------------------------------------------------------------------
        //	程式說明: 
        //	x < 9, 10 < x < 13, 13 < x < 32, 55295 < x < 57344, 57344 < x < 65536, 1114111 < x
        // ------------------------------------------------------------------------------------

        int xChar;
        StringReader sr = new StringReader(xmlText);
        StringBuilder sb = new StringBuilder();
        xChar = sr.Read();
        while (xChar > -1)
        {
            if (xChar <= 31)
            {
                if ((xChar != 9) && (xChar != 10) && (xChar != 13))
                {
                    xChar = sr.Read();
                    continue;
                }
            }
            if ((xChar > 55295) && (xChar < 57344))
            {
                xChar = sr.Read();
                continue;
            }
            if ((xChar > 65533) && (xChar < 65536))
            {
                xChar = sr.Read();
                continue;
            }
            if (xChar > 1114111)
            {
                xChar = sr.Read();
                continue;
            }
            sb.Append((char)xChar);
            xChar = sr.Read();
        }
        sr.Close();
        return sb.ToString();
    }


    /// <summary>
    /// 填入資料的 Schema 資料
    /// </summary>
    /// <param name="xRoot"></param>
    /// <param name="oRs"></param>
    void FillSchema(XmlElement xRoot, SqlDataReader oRs)
    {
        //	------------------------------------------------------------------------------------
        //	異動時間: 2006/05/15
        // 完 成 度: 99%
        // 功    能: 在回傳的結果中填入資料結構
        //	摘    要: 
        //	備    註: 
        //	------------------------------------------------------------------------------------
        XmlDocument xDoc = xRoot.OwnerDocument;
        XmlElement xSchema = xDoc.CreateElement("Schema");
        xRoot.AppendChild(xSchema);

        XmlElement xNew = null;
        DataTable tblSchema = oRs.GetSchemaTable();
        DataColumnCollection oCols = tblSchema.Columns;
        DataRowCollection oRows = tblSchema.Rows;

        for (int i = 0; i < oRows.Count; i++)
        {
            xNew = xDoc.CreateElement("Column");
            xNew.SetAttribute("BaseName", oRows[i]["BaseColumnName"].ToString());
            xNew.SetAttribute("Name", oRows[i]["ColumnName"].ToString());
            xNew.SetAttribute("DataType", oRows[i]["DataType"].ToString());
            xNew.SetAttribute("Size", oRows[i]["ColumnSize"].ToString());
            xSchema.AppendChild(xNew);
        }
        //	------------------------------------------------------------------------------------
        ////	SchemaTable information list
        ////	<Schema ColumnName="id" ColumnOrdinal="0" ColumnSize="8" NumericPrecision="19" NumericScale="255" 
        ////		IsUnique="False" IsKey="" BaseServerName="" BaseCatalogName="" 
        ////		BaseColumnName="id" BaseSchemaName="" BaseTableName="" 
        ////		DataType="System.Int64" AllowDBNull="False" ProviderType="0" 
        ////		IsAliased="" IsExpression="" IsIdentity="True" IsAutoIncrement="True" 
        ////		IsRowVersion="False" IsHidden="" IsLong="False" IsReadOnly="True" /> 
        //	------------------------------------------------------------------------------------
        ////	for(int i=0; i<oRows.Count; i++)
        ////	{
        ////		xNew = xDoc.CreateElement("Schema");
        ////		for(int j=0; j<oCols.Count; j++)
        ////		{
        ////			xNew.SetAttribute(oCols[j].ColumnName, oRows[i][j].ToString());
        ////		}
        ////		xSchema.AppendChild(xNew);
        ////	}
        //	------------------------------------------------------------------------------------
    }


    /// <summary>
    /// 把資料填入 XML Element 之中
    /// </summary>
    /// <param name="xRoot"></param>
    /// <param name="oRs"></param>
    /// <param name="recStart"></param>
    /// <param name="recCount"></param>
    /// <param name="recordName"></param>
    //aaaaa
    void FillData(XmlElement xRoot, SqlDataReader oRs, int recStart, int recCount, string recordName)
    {
        //	------------------------------------------------------------------------------------
        // 異動時間: 2007/01/31
        // 完 成 度: 99%
        // 作    者: Dick
        // 功    能: 把資料填入 XML 節點之中
        //	摘    要: 
        //	備    註: 
        //       1. 如果 CountTotal = false, _MoreData (HasMore) 會自動調整
        //       2. 如果 CountTotal = true, _MoreData=false (HasMore)
        //	------------------------------------------------------------------------------------
        // 程式說明:
        //     *. 取得資料轉換及格式化用的相關資訊
        //     *. 
        //     *. 
        //     *. 
        //     *. 
        //     *. 
        //	------------------------------------------------------------------------------------
        // 取得資料轉換及格式化用的相關資訊
        string[] fieldNames = null; // 資料欄名稱
        //			TypeCode[] types = null; // 資料型別
        FieldType[] types = null; // 資料型別
        string[] patterns = null; // 格式化字串
        SetFormatInfo(oRs, out fieldNames, out types, out patterns);

        // get all field name
        XmlDocument xDoc = xRoot.OwnerDocument;
        int recEnd = (recCount <= 0) ? int.MaxValue : recStart + recCount; // 資料結束位置
        int realCount = 0; // 實際擷取的資料筆數
        int TotalCount = 0; // 總資料筆數
        XmlElement xNew = null; // 

        if (this.WithSchema)
        {
            FillSchema(xRoot, oRs);
        }

        if (this.CountTotal)
        {
            // 利用 DataReader 計算全部的資料筆數
            if (this.UseElement)
            {
                while (oRs.Read())
                {
                    if ((TotalCount >= recStart) && (TotalCount < recEnd))
                    {
                        xNew = xDoc.CreateElement(recordName);
                        FillAsElement(xNew, TotalCount + 1, oRs, fieldNames, types, patterns);
                        xRoot.AppendChild(xNew);
                        realCount++;
                    }
                    TotalCount++;
                }
            }
            else
            {
                while (oRs.Read())
                {
                    if ((TotalCount >= recStart) && (TotalCount < recEnd))
                    {
                        xNew = xDoc.CreateElement(recordName);
                        FillAsAttribute(xNew, TotalCount + 1, oRs, fieldNames, types, patterns);
                        xRoot.AppendChild(xNew);
                        realCount++;
                    }
                    TotalCount++;
                }
            }


            // 檢查是否還有資料未讀取 (因為資料開始為零)
            if ((recStart + realCount) < TotalCount)
            {
                _MoreData = true;
            }
            else
            {
                _MoreData = false;
            }
        }
        else
        {
            // 不計算全部的資料筆數, 這個情形會自動檢視是否有未讀取的資料, 並影響 _MoreData 值
            if (this.UseElement)
            {
                while ((TotalCount < recEnd) && oRs.Read())
                {
                    if (TotalCount >= recStart)
                    {
                        xNew = xDoc.CreateElement(recordName);
                        FillAsElement(xNew, TotalCount + 1, oRs, fieldNames, types, patterns);
                        xRoot.AppendChild(xNew);
                        realCount++;
                    }
                    TotalCount++;
                }
            }
            else
            {
                while ((TotalCount < recEnd) && oRs.Read())
                {
                    if (TotalCount >= recStart)
                    {
                        xNew = xDoc.CreateElement(recordName);
                        FillAsAttribute(xNew, TotalCount + 1, oRs, fieldNames, types, patterns);
                        xRoot.AppendChild(xNew);
                        realCount++;
                    }
                    TotalCount++;
                }
            }

            // 檢查是否還有資料未讀取
            if (oRs.Read())
            {
                _MoreData = true;
            }
            else
            {
                _MoreData = false;
            }
        }

        if (this.FillInfo)
        {
            /// <summary>
            /// 把資料開始位置, 最大的資料擷取筆數, 實際擷取的資料筆數, 總資料筆數等相關訊息填入指定的根元素中
            /// </summary>
            xRoot.SetAttribute("RecordStart", recStart.ToString()); // 資料開始位置
            xRoot.SetAttribute("MaxRecord", recCount.ToString()); // 最大的資料擷取筆數
            xRoot.SetAttribute("RecordCount", realCount.ToString()); // 實際擷取的資料筆數
            xRoot.SetAttribute("TotalCount", TotalCount.ToString()); // 總資料筆數
            //				xRoot.SetAttribute("HasMore", _MoreData.ToString()); // 是否還有資料未讀取
        }
    }


    #region /* 外部程式資料存取功能區 */
    #endregion

    #region /* GetXmlDocument from DbCommand */
    #endregion

    //aaaaa
    public XmlDocument GetXmlDocument(SqlCommand dbCommand)
    {
        return GetXmlDocument(dbCommand, 0, 0);
    }

    public XmlDocument GetXmlDocument(SqlCommand dbCommand, int recStart, int recCount)
    {
        return GetXmlDocument(dbCommand, recStart, recCount, this.NameOfRoot, this.NameOfRecord);
    }

    public XmlDocument GetXmlDocument(SqlCommand dbCommand, int recStart, int recCount, string rootName, string recordName)
    {
        //	------------------------------------------------------------------------------------
        // 異動時間: 2007/01/31
        // 完 成 度: 99%
        // 功    能: 由資料庫擷取資料
        //	摘    要: 
        //	備    註: 
        //	------------------------------------------------------------------------------------
        // 程式說明:
        //     *. 選定 DbConnection 以便存取資料
        //     *. 保留 DbConnection 狀態
        //     *. 產生一份空的 XmlDocument
        //     *. 執行資料存取指令
        //     *. 把結果填入 XmlDocument
        //     *. 還原 DbConnection 狀態
        //	------------------------------------------------------------------------------------
        SqlDataReader oRs = null;
        XmlDocument xDoc = null;
        XmlElement xRoot = null;
        bool IsConnectionClosed = true;

        if (dbCommand.Connection == null)
        {
            dbCommand.Connection = _oConn;
        }

        IsConnectionClosed = (dbCommand.Connection.State == ConnectionState.Closed);

        // 產生一份空的 XmlDocument
        xDoc = NewXmlDocument(rootName);
        xRoot = xDoc.DocumentElement;

        if (IsConnectionClosed)
        {
            dbCommand.Connection.Open();
        }

        try
        {
            // 取得資料
            oRs = dbCommand.ExecuteReader();

            // 填入資料
            FillData(xRoot, oRs, recStart, recCount, recordName);

            oRs.Close();

            if (IsConnectionClosed)
            {
                dbCommand.Connection.Close();
            }
        }
        catch (Exception err)
        {
            // 釋放佔用的資源
            if (oRs != null) oRs.Close();

            if (dbCommand.Connection.State == ConnectionState.Open)
            {
                dbCommand.Connection.Close();
            }

            throw (new Exception("Db2Xml.GetXmlDocument exception!" + err.Message, err));
        }

        return xDoc;
    }



    #region /* Fill data to XmlDocument from DbCommand */
    #endregion

    public void FillData(XmlNode xRoot, SqlCommand dbCommand)
    {
        FillData(xRoot, dbCommand, 0, 0, NameOfRecord);
    }

    public void FillData(XmlNode xRoot, SqlCommand dbCommand, int recStart, int recCount)
    {
        FillData(xRoot, dbCommand, recStart, recCount, NameOfRecord);
    }

    public void FillData(XmlNode xNode, SqlCommand dbCommand, int recStart, int recCount, string recordName)
    {
        //	------------------------------------------------------------------------------------
        //	異動時間: 2006/05/15
        // 完 成 度: 99%
        // 功    能: 由資料庫擷取資料, 並將資料填入指定的 XmlElement 之下
        //	摘    要: 
        //	備    註: 
        //	------------------------------------------------------------------------------------
        XmlElement xRoot;
        switch (xNode.NodeType)
        {
            case XmlNodeType.Element:
                {
                    xRoot = (XmlElement)xNode;
                    break;
                }
            case XmlNodeType.Document:
                {
                    xRoot = ((XmlDocument)xNode).CreateElement(this.NameOfRoot);
                    xNode.AppendChild(xRoot);
                    break;
                }
            default:
                {
                    throw new ArgumentException("目前只支援 XmlElement, XmlDocument 類型的節點!");
                }
        }

        if (dbCommand.Connection == null) dbCommand.Connection = _oConn;

        bool IsConnectionClosed = (dbCommand.Connection.State == ConnectionState.Closed);

        SqlDataReader oRs = null;

        if (IsConnectionClosed) dbCommand.Connection.Open();

        try
        {
            oRs = dbCommand.ExecuteReader();

            FillData(xRoot, oRs, recStart, recCount, recordName);

            oRs.Close();
        }
        catch (Exception err)
        {
            throw (err);
        }
        finally
        {
            if (IsConnectionClosed) dbCommand.Connection.Close();
        }
    }



    #region /* Part of Version II */
    #endregion

    FieldType GetFieldType(Type tp)
    {
        switch (tp.Name)
        {
            case "Boolean":		// bit 
                {
                    return FieldType.Boolean;
                }
            case "Byte":		// tinyint 
                {
                    return FieldType.Byte;
                }
            case "Byte[]":		// binary, image, varbinary, timestamp
                {
                    return FieldType.Bytes;
                }
            case "DateTime":	// datetime, smalldatetime 
                {
                    return FieldType.DateTime;
                }
            case "Decimal":		// decimal, money, decimal, smallmoney 
                {
                    return FieldType.Decimal;
                }
            case "Double":		// float 
                {
                    return FieldType.Double;
                }
            case "Guid":			// uniqueidentifier 
                {
                    return FieldType.Guid;
                }
            case "Int16":		// smallint 
                {
                    return FieldType.Int16;
                }
            case "Int32":		// int 
                {
                    return FieldType.Int32;
                }
            case "Int64":		// bigint 
                {
                    return FieldType.Int64;
                }
            case "Single":		// real 
                {
                    return FieldType.Single;
                }
            case "String":		// char, nchar, ntext, text, varchar, text, ntext, nvarchar, ntext 
                {
                    return FieldType.String;
                }
        }
        return FieldType.UnKnown;
    }



    #region IDisposable 成員

    void IDisposable.Dispose()
    {
        // TODO:  加入 Db2Xml.Dispose 實作


        if ((this.DbConnection != null) && (this.DbConnection.State == ConnectionState.Open))
        {
            this.DbConnection.Close();
        }
    }

    #endregion


    enum FieldType
    {
        // ------------------------------------------------------------------------------------
        // 異動時間: 2006/xx/xx
        // 作    者: Dick
        // 完 成 度: 99%
        // 功    能: 
        //	備    註:
        //       *. 
        // ------------------------------------------------------------------------------------
        //	程式說明: 
        //       *. 
        // ------------------------------------------------------------------------------------
        UnKnown,		//	Boolean,		// bit 
        Boolean,		//	Boolean,		// bit 
        Byte,			//	Byte,			// tinyint 
        Bytes,		//	Byte[],		// binary, image, varbinary, timestamp
        DateTime,	//	DateTime,	// datetime, smalldatetime 
        Decimal,		//	Decimal,		// decimal, money, decimal, smallmoney 
        Double,		//	Double,		// float 
        Guid,			//	Guid,			// uniqueidentifier 
        Int16,		//	Int16,		// smallint 
        Int32,		//	Int32,		// int 
        Int64,		//	Int64,		// bigint 
        Single,		//	Single,		// real 
        String		//	String,		// char, nchar, ntext, text, varchar, text, ntext, nvarchar, ntext 
    }

}