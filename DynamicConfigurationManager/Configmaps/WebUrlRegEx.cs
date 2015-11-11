using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DynamicConfigurationManager.ConfigMaps
{
    internal class WebUrlRegEx : IConfigMap
    {
        public bool Execute(string configMapAttribute)
        {
            var reqUri = HttpContext.Current.Request.Url;

            var mWebUrl = new StringBuilder();
            mWebUrl.Append(reqUri.Host);

            if (!reqUri.IsDefaultPort)
            {
                mWebUrl.Append(":");
                mWebUrl.Append(reqUri.Port);
            }

            var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);
            return re.IsMatch(mWebUrl.ToString());
        }
    }
}