using System;
using System.Web;


namespace ISCSF200
{
    /// <summary>
    /// Summary description for QueryString
    /// </summary>
    public class QueryString : IDisposable
    {
        private string[] _queryStringItems = new string[] { };

        public string this[string ordinal]
        {
            get
            {
                if (ordinal == "")
                {
                    return null;
                }

                if (HttpContext.Current.Request.QueryString["q"] == null)
                {
                    return null;
                }

                // decodes to name/value pairs: id=1&something=what&test=true
                string decode;
                try
                {
                    decode = Base64.Decode(HttpContext.Current.Request.QueryString["q"]);
                }
                catch (Exception)
                {
                    return null;
                }

                string[] pairs = decode.Split('&');

                foreach (string pair in pairs)
                {
                    string[] item = pair.Split('=');

                    if (item[0].ToLower() == ordinal.ToLower())
                    {
                        return item[1];
                    }
                }

                return null;
            }
        }

        public QueryString()
        { }

        public string EncodePairs(string data)
        {
            if (data == "")
            {
                return "";
            }

            return Base64.Encode(data);
        }

        #region IDisposable Members

        public void Dispose()
        { }

        #endregion
    }

}