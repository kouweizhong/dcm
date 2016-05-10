using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     SitePathRegEx compares the attribute value to the fully qualified path of the Site's
    ///     AppDomain Path.
    /// </summary>
    internal class SitePathRegEx : IConfigMap
    {
        /// <summary>
        ///     Determines if the given configuration map attribute matches the HTTP host's application
        ///     domain path.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool Execute(string configMapAttribute)
        {
            if (configMapAttribute.Length == 0) return false;
            var sitePath = HttpRuntime.AppDomainAppPath;

            Trace.TraceInformation($"SitePathRegEx: matching to {sitePath}");

            var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);

            return re.IsMatch(sitePath);
        }
    }
}