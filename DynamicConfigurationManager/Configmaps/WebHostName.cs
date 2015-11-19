namespace DynamicConfigurationManager.ConfigMaps
{
    using System;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// WebHostName compares the configuration map attribute value to the fully qualified path of
    /// the web host URI.
    /// </summary>
    internal class WebHostName : IConfigMap
    {
        /// <summary>
        /// Determines if the configuration map attribute matches the web host URI.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool Execute(string configMapAttribute)
        {
            string hostname;
            if (OperationContext.Current != null
                && OperationContext.Current.Channel != null
                && OperationContext.Current.Channel.LocalAddress != null
                && OperationContext.Current.Channel.LocalAddress.Uri != null)
            {
                var uri = OperationContext.Current.Channel.LocalAddress.Uri;

                Trace.TraceInformation("OperationContext.Current.Channel.LocalAddress.Uri: {0}", uri);

                hostname = uri.Host;
            }
            else if (HttpContext.Current != null)
            {
                var reqUri = HttpContext.Current.Request.Url;

                Trace.TraceInformation("HttpContext.Current.Request.Url: {0}", reqUri);

                hostname = reqUri.Host;
            }
            else
            {
                Trace.TraceError("Error: Unknown Http Request Context");

                throw new ApplicationException("Unknown Http Request Context");
            }

            Trace.TraceInformation("Hostname from context Uri: {0}", hostname);

            var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);

            return re.IsMatch(hostname);
        }
    }
}