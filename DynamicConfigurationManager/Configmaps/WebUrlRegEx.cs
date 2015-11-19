namespace DynamicConfigurationManager.ConfigMaps
{
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// WebUrlRegEx performs a regular expression search of the fully qualified path of the web host
    /// URI and port number.
    /// </summary>
    internal class WebUrlRegEx : IConfigMap
    {
        /// <summary>
        /// Determines if the configuration map attribute matches the web host URI including port number.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool Execute(string configMapAttribute)
        {
            var reqUri = HttpContext.Current.Request.Url;

            var webUri = new StringBuilder();
            webUri.Append(reqUri.Host);

            if (!reqUri.IsDefaultPort)
            {
                webUri.Append(":");
                webUri.Append(reqUri.Port);
            }

            var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);
            return re.IsMatch(webUri.ToString());
        }
    }
}