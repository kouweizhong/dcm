namespace DynamicConfigurationManager.ConfigMaps
{
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// SitePathRegEx compares the attribute value to the fully qualified path of the Site's
    /// AppDomain Path.
    /// </summary>
    internal class SitePathRegEx : IConfigMap
    {
        /// <summary>
        /// Determines if the given configuration map attribute matches the HTTP host's application
        /// domain path.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
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