using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    /// SitePathRegEx compares the attribute value to the fully qualified path of the Site's
    /// AppDomain Path.
    /// </summary>
    internal class SitePathRegEx : IConfigMap
    {
        public bool Execute(string configMapAttribute)
        {
            bool rtnValue = false;

            if (configMapAttribute.Length != 0)
            {
                var sitePath = HttpRuntime.AppDomainAppPath;
                Trace.TraceInformation("SitePathRegEx: matching to {0}", sitePath);
                var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);
                rtnValue = re.IsMatch(sitePath);
            }

            return rtnValue;
        }
    }
}