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

    #region �n���y����(���i���ŭ�)

    private string _seqsn = "^\\d+$";

    /// <summary>
    /// �n���y����(���i���ŭ�)
    /// </summary>
    public string Seqsn
    {
        get
        {
            return _seqsn;
        }
    }

    #endregion

    #region ��X�ץ�s��(�i���ŭ�)

    private string _casenoempty = "^(\\w{7})?$";

    /// <summary>
    /// ��X�ץ�s��(�i���ŭ�)
    /// </summary>
    public string CaseNoEmpty
    {
        get
        {
            return _casenoempty;
        }
    }

    #endregion

    #region �褸�~��(�i���ŭ�)

    private string _cyearempty = "^(\\d{4})?$";

    /// <summary>
    /// �褸�~��(�i���ŭ�)
    /// </summary>
    public string CYearEmpty
    {
        get
        {
            return _cyearempty;
        }
    }

    #endregion

    #region �褸�~��(���i���ŭ�)

    private string _cyear = "^\\d{4}$";

    /// <summary>
    /// �褸�~��(���i���ŭ�)
    /// </summary>
    public string CYear
    {
        get
        {
            return _cyear;
        }
    }

    #endregion

    #region �褸���(�i���ŭ�)

    private string _cdateempty = "^(\\d{8})?$";

    /// <summary>
    /// �褸���(�i���ŭ�)
    /// </summary>
    public string CDateEmpty
    {
        get
        {
            return _cdateempty;
        }
    }

    #endregion

    #region �褸���(���i���ŭ�)

    private string _cdate = "^\\d{8}$";

    /// <summary>
    /// �褸���(���i���ŭ�)
    /// </summary>
    public string CDate
    {
        get
        {
            return _cdate;
        }
    }

    #endregion

    #region �褸����榡�r��(���i���ŭ�)

    private string _cdateformat = "^y{4}([ /-]?)M{2}\\1d{2}$";

    /// <summary>
    /// �褸����榡�r��(���i���ŭ�)
    /// </summary>
    public string CDateFormat
    {
        get
        {
            return _cdateformat;
        }
    }

    #endregion

    #region ���u�m�W(�i���ŭ�,�i�����^��k�w�q����)

    private string _empnameempty = "^[\\s\\u4e00-\\ud7a3\\u00c0-\\u00ff\\u0152-\\u0178\\u3040-\\u309f\\u30a0-\\u30ffA-Za-z.']{0,30}$";

    /// <summary>
    /// ���u�m�W(�i���ŭ�,�i�����^��)
    /// </summary>
    public string EmpnameEmpty
    {
        get
        {
            return _empnameempty;
        }
    }

    #endregion

    #region ���u�m�W(���i���ŭ�,�i�����^��k�w�q����)

    private string _empname = "^[\\s\\u4e00-\\ud7a3\\u00c0-\\u00ff\\u0152-\\u0178\\u3040-\\u309f\\u30a0-\\u30ffA-Za-z.']{1,30}$";

    /// <summary>
    /// ���u�m�W(���i���ŭ�,�i�����^��)
    /// </summary>
    public string Empname
    {
        get
        {
            return _empname;
        }
    }

    #endregion

    #region ���u�u��(�i���ŭ�)

    private string _empnoempty = "^(\\d{6})?$";

    /// <summary>
    /// ���u�u��(�i���ŭ�)
    /// </summary>
    public string EmpnoEmpty
    {
        get
        {
            return _empnoempty;
        }
    }

    #endregion

    #region ���u�u��(���i���ŭ�)

    private string _empno = "^\\d{6}$";

    /// <summary>
    /// ���u�u��(���i���ŭ�)
    /// </summary>
    public string Empno
    {
        get
        {
            return _empno;
        }
    }

    #endregion

    #region ���u�u���M��(�i���ŭ�)

    private string _empnolistempty = "^(\\d{6},?)*$";

    /// <summary>
    /// ���u�u���M��(�i���ŭ�)
    /// </summary>
    public string EmpnoListEmpty
    {
        get
        {
            return _empnolistempty;
        }
    }

    #endregion

    #region �Ȥ�W��(�i���ŭ�,�i�����^��)

    private string _compcnameempty = "^[^%\\\\\"]{0,60}$";

    /// <summary>
    /// �Ȥ�W��(�i���ŭ�,�i�����^��)
    /// </summary>
    public string CompCNameEmpty
    {
        get
        {
            return _compcnameempty;
        }
    }

    #endregion

    #region �Ȥ�W��(���i���ŭ�,�i�����^��)

    private string _compcname = "^[^%\\\\\"]{1,60}$";

    /// <summary>
    /// �Ȥ�W��(���i���ŭ�,�i�����^��)
    /// </summary>
    public string CompCName
    {
        get
        {
            return _compcname;
        }
    }

    #endregion

    #region �Ȥ�N��(�i���ŭ�)

    private string _compidempty = "^\\w{0,10}$";

    /// <summary>
    /// �Ȥ�N��(�i���ŭ�)
    /// </summary>
    public string CompidEmpty
    {
        get
        {
            return _compidempty;
        }
    }

    #endregion

    #region �Ȥ�N��(���i���ŭ�)

    private string _compid = "^\\w{8,10}$";

    /// <summary>
    /// �Ȥ�N��(���i���ŭ�)
    /// </summary>
    public string Compid
    {
        get
        {
            return _compid;
        }
    }

    #endregion

    #region �Ȥ�N���M��(�i���ŭ�)

    private string _custlistempty = "^(\\w{8,10},?)*$";

    /// <summary>
    /// �Ȥ�N���M��(�i���ŭ�)
    /// </summary>
    public string CustListEmpty
    {
        get
        {
            return _custlistempty;
        }
    }

    #endregion

    #region ���(�i���ŭ�)

    private string _orgcdempty = "^\\d{0,2}$";

    /// <summary>
    /// ���(�i���ŭ�)
    /// </summary>
    public string OrgcdEmpty
    {
        get
        {
            return _orgcdempty;
        }
    }

    #endregion

    #region �����N��(���i���ŭ�)

    private string _deptid = "^\\d{2}[\\dA-Z]{5}$";

    /// <summary>
    /// �����N��(���i���ŭ�)
    /// </summary>
    public string Deptid
    {
        get
        {
            return _deptid;
        }
    }

    #endregion

    #region �������X(�i���ŭ�)

    private string _extempty = "^[\\d-() #]*$";

    /// <summary>
    /// �������X(�i���ŭ�)
    /// </summary>
    public string ExtEmpty
    {
        get
        {
            return _extempty;
        }
    }

    #endregion

    #region ��a�W��(���i���ŭ�,�i�����^��)

    private string _countryname = "^[\\s\\u4e00-\\ud7a3A-Za-z]{1,50}$";

    /// <summary>
    /// ��a�W��(���i���ŭ�,�i�����^��)
    /// </summary>
    public string CountryName
    {
        get
        {
            return _countryname;
        }
    }

    #endregion

    #region ��a�W��(�i���ŭ�,�i�����^��)

    private string _countrynameempty = "^[\\s\\u4e00-\\ud7a3A-Za-z]{0,50}$";

    /// <summary>
    /// ��a�W��(�i���ŭ�,�i�����^��)
    /// </summary>
    public string CountryNameEmpty
    {
        get
        {
            return _countrynameempty;
        }
    }

    #endregion

    #region �p�e�N��(���i���ŭ�,prs020.s20_pojno)

    private string _projno = "^\\w{10}$";

    /// <summary>
    /// �p�e�N��(���i���ŭ�)
    /// </summary>
    public string Projno
    {
        get
        {
            return _projno;
        }
    }

    #endregion

    #region object id(�i���ŭ�)

    private string _oidempty = "^[\\w_]*$";

    /// <summary>
    /// object id(���i���ŭ�)
    /// </summary>
    public string OidEmpty
    {
        get
        {
            return _oidempty;
        }
    }

    #endregion

    #region object id(���i���ŭ�)

    private string _oid = "^[\\w_]+$";

    /// <summary>
    /// object id(���i���ŭ�)
    /// </summary>
    public string Oid
    {
        get
        {
            return _oid;
        }
    }

    #endregion

    #region ���(�����,���i���ŭ�)

    private string _int = "^\\d+$";

    /// <summary>
    /// ���(�����,�i���ŭ�)

    /// </summary>
    public string Int
    {
        get
        {
            return _int;
        }
    }

    #endregion

    #region �B�I��(���B�I��,���i���ŭ�)

    private string _float = "^\\d+.\\d+$";

    /// <summary>
    /// �B�I��(���B�I��,���i���ŭ�)

    /// </summary>
    public string Float
    {
        get
        {
            return _float;
        }
    }

    #endregion

    #region Boolean(���i���ŭ�)

    private string _boolean = "^true|false$";

    /// <summary>
    /// Boolean(���i���ŭ�)

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
