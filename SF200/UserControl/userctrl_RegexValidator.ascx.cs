using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Text.RegularExpressions;


public partial class UserCtrl_userctrl_RegexValidator : System.Web.UI.UserControl
{
    #region local variables

    #region 登錄流水號(不可為空值)

    private string _seqsn = "^\\d+$";

    /// <summary>
    /// 登錄流水號(不可為空值)
    /// </summary>
    public string Seqsn
    {
        get
        {
            return _seqsn;
        }
    }

    #endregion

    #region 國合案件編號(可為空值)

    private string _casenoempty = "^(\\w{7})?$";

    /// <summary>
    /// 國合案件編號(可為空值)
    /// </summary>
    public string CaseNoEmpty
    {
        get
        {
            return _casenoempty;
        }
    }

    #endregion

    #region 西元年度(可為空值)

    private string _cyearempty = "^(\\d{4})?$";

    /// <summary>
    /// 西元年度(可為空值)
    /// </summary>
    public string CYearEmpty
    {
        get
        {
            return _cyearempty;
        }
    }

    #endregion

    #region 西元年度(不可為空值)

    private string _cyear = "^\\d{4}$";

    /// <summary>
    /// 西元年度(不可為空值)
    /// </summary>
    public string CYear
    {
        get
        {
            return _cyear;
        }
    }

    #endregion

    #region 西元日期(可為空值)

    private string _cdateempty = "^(\\d{8})?$";

    /// <summary>
    /// 西元日期(可為空值)
    /// </summary>
    public string CDateEmpty
    {
        get
        {
            return _cdateempty;
        }
    }

    #endregion

    #region 西元日期(不可為空值)

    private string _cdate = "^\\d{8}$";

    /// <summary>
    /// 西元日期(不可為空值)
    /// </summary>
    public string CDate
    {
        get
        {
            return _cdate;
        }
    }

    #endregion

    #region 西元日期格式字串(不可為空值)

    private string _cdateformat = "^y{4}([ /-]?)M{2}\\1d{2}$";

    /// <summary>
    /// 西元日期格式字串(不可為空值)
    /// </summary>
    public string CDateFormat
    {
        get
        {
            return _cdateformat;
        }
    }

    #endregion

    #region 員工姓名(可為空值,可為中英日法德義葡文)

    private string _empnameempty = "^[\\s\\u4e00-\\ud7a3\\u00c0-\\u00ff\\u0152-\\u0178\\u3040-\\u309f\\u30a0-\\u30ffA-Za-z.']{0,30}$";

    /// <summary>
    /// 員工姓名(可為空值,可為中英文)
    /// </summary>
    public string EmpnameEmpty
    {
        get
        {
            return _empnameempty;
        }
    }

    #endregion

    #region 員工姓名(不可為空值,可為中英日法德義葡文)

    private string _empname = "^[\\s\\u4e00-\\ud7a3\\u00c0-\\u00ff\\u0152-\\u0178\\u3040-\\u309f\\u30a0-\\u30ffA-Za-z.']{1,30}$";

    /// <summary>
    /// 員工姓名(不可為空值,可為中英文)
    /// </summary>
    public string Empname
    {
        get
        {
            return _empname;
        }
    }

    #endregion

    #region 員工工號(可為空值)

    private string _empnoempty = "^(\\d{6})?$";

    /// <summary>
    /// 員工工號(可為空值)
    /// </summary>
    public string EmpnoEmpty
    {
        get
        {
            return _empnoempty;
        }
    }

    #endregion

    #region 員工工號(不可為空值)

    private string _empno = "^\\d{6}$";

    /// <summary>
    /// 員工工號(不可為空值)
    /// </summary>
    public string Empno
    {
        get
        {
            return _empno;
        }
    }

    #endregion

    #region 員工工號清單(可為空值)

    private string _empnolistempty = "^(\\d{6},?)*$";

    /// <summary>
    /// 員工工號清單(可為空值)
    /// </summary>
    public string EmpnoListEmpty
    {
        get
        {
            return _empnolistempty;
        }
    }

    #endregion

    #region 客戶名稱(可為空值,可為中英文)

    private string _compcnameempty = "^[^%\\\\\"]{0,60}$";

    /// <summary>
    /// 客戶名稱(可為空值,可為中英文)
    /// </summary>
    public string CompCNameEmpty
    {
        get
        {
            return _compcnameempty;
        }
    }

    #endregion

    #region 客戶名稱(不可為空值,可為中英文)

    private string _compcname = "^[^%\\\\\"]{1,60}$";

    /// <summary>
    /// 客戶名稱(不可為空值,可為中英文)
    /// </summary>
    public string CompCName
    {
        get
        {
            return _compcname;
        }
    }

    #endregion

    #region 客戶代號(可為空值)

    private string _compidempty = "^\\w{0,10}$";

    /// <summary>
    /// 客戶代號(可為空值)
    /// </summary>
    public string CompidEmpty
    {
        get
        {
            return _compidempty;
        }
    }

    #endregion

    #region 客戶代號(不可為空值)

    private string _compid = "^\\w{8,10}$";

    /// <summary>
    /// 客戶代號(不可為空值)
    /// </summary>
    public string Compid
    {
        get
        {
            return _compid;
        }
    }

    #endregion

    #region 客戶代號清單(可為空值)

    private string _custlistempty = "^(\\w{8,10},?)*$";

    /// <summary>
    /// 客戶代號清單(可為空值)
    /// </summary>
    public string CustListEmpty
    {
        get
        {
            return _custlistempty;
        }
    }

    #endregion

    #region 單位(可為空值)

    private string _orgcdempty = "^\\d{0,2}$";

    /// <summary>
    /// 單位(可為空值)
    /// </summary>
    public string OrgcdEmpty
    {
        get
        {
            return _orgcdempty;
        }
    }

    #endregion

    #region 部門代號(不可為空值)

    private string _deptid = "^\\d{2}[\\dA-Z]{5}$";

    /// <summary>
    /// 部門代號(不可為空值)
    /// </summary>
    public string Deptid
    {
        get
        {
            return _deptid;
        }
    }

    #endregion

    #region 分機號碼(可為空值)

    private string _extempty = "^[\\d-() #]*$";

    /// <summary>
    /// 分機號碼(可為空值)
    /// </summary>
    public string ExtEmpty
    {
        get
        {
            return _extempty;
        }
    }

    #endregion

    #region 國家名稱(不可為空值,可為中英文)

    private string _countryname = "^[\\s\\u4e00-\\ud7a3A-Za-z]{1,50}$";

    /// <summary>
    /// 國家名稱(不可為空值,可為中英文)
    /// </summary>
    public string CountryName
    {
        get
        {
            return _countryname;
        }
    }

    #endregion

    #region 國家名稱(可為空值,可為中英文)

    private string _countrynameempty = "^[\\s\\u4e00-\\ud7a3A-Za-z]{0,50}$";

    /// <summary>
    /// 國家名稱(可為空值,可為中英文)
    /// </summary>
    public string CountryNameEmpty
    {
        get
        {
            return _countrynameempty;
        }
    }

    #endregion

    #region 計畫代號(不可為空值,prs020.s20_pojno)

    private string _projno = "^\\w{10}$";

    /// <summary>
    /// 計畫代號(不可為空值)
    /// </summary>
    public string Projno
    {
        get
        {
            return _projno;
        }
    }

    #endregion

    #region object id(可為空值)

    private string _oidempty = "^[\\w_]*$";

    /// <summary>
    /// object id(不可為空值)
    /// </summary>
    public string OidEmpty
    {
        get
        {
            return _oidempty;
        }
    }

    #endregion

    #region object id(不可為空值)

    private string _oid = "^[\\w_]+$";

    /// <summary>
    /// object id(不可為空值)
    /// </summary>
    public string Oid
    {
        get
        {
            return _oid;
        }
    }

    #endregion

    #region 整數(正整數,不可為空值)

    private string _int = "^\\d+$";

    /// <summary>
    /// 整數(正整數,可為空值)

    /// </summary>
    public string Int
    {
        get
        {
            return _int;
        }
    }

    #endregion

    #region 浮點數(正浮點數,不可為空值)

    private string _float = "^\\d+.\\d+$";

    /// <summary>
    /// 浮點數(正浮點數,不可為空值)

    /// </summary>
    public string Float
    {
        get
        {
            return _float;
        }
    }

    #endregion

    #region Boolean(不可為空值)

    private string _boolean = "^true|false$";

    /// <summary>
    /// Boolean(不可為空值)

    /// </summary>
    public string Boolean
    {
        get
        {
            return _boolean;
        }
    }

    #endregion

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    public bool Match(string text, string key)
    {
        Regex regex = new Regex(key);
        Match m = regex.Match(text);

        return m.Success;
    }

    public bool MatchInt(string text, int length, bool allowempty)
    {
        string key = string.Format(@"^\\d{{0}{1}}$", (allowempty ? "0," : ""), length.ToString());

        Regex regex = new Regex(key);
        Match m = regex.Match(text);

        return m.Success;
    }
}
