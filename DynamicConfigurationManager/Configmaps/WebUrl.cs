using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Web;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     WebUrl compares the configuration map attribute value to the fully qualified path of the web
    ///     host URI and port number.
    /// </summary>
    internal class WebUrl : IConfigMap
    {
        /// <summary>
        ///     Determines if the configuration map attribute matches the web host URI including port number.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool Execute(string configMapAttribute)
        {
            int port;
            string hostname;
            if (OperationContext.Current != null
                && OperationContext.Current.Channel != null
                && OperationContext.Current.Channel.LocalAddress != null
                && OperationContext.Current.Channel.LocalAddress.Uri != null)
            {
                var uri = OperationContext.Current.Channel.LocalAddress.Uri;

                Trace.TraceInformation($"OperationContext.Current.Channel.LocalAddress.Uri: {uri}");

                hostname = uri.Host;

                port = uri.Port;
            }
            else if (HttpContext.Current != null)
            {
                var reqUri = HttpContext.Current.Request.Url;

                Trace.TraceInformation($"HttpContext.Current.Request.Url: {reqUri}");

                hostname = reqUri.Host;

                port = reqUri.Port;
            }
            else
            {
                Trace.TraceError("Error: Unknown Request Context");

                throw new ApplicationException("Unknown Request Context");
            }

            var webUrl = hostname + (port == 80 ? string.Empty : ":" + port);

            return webUrl.Equals(configMapAttribute, StringComparison.OrdinalIgnoreCase);
        }
    }
}