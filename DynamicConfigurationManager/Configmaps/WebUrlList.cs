namespace DynamicConfigurationManager.ConfigMaps
{
    using System;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.Web;

    /// <summary>
    /// WebUrlList compares the configuration map attribute value to the fully qualified path of the
    /// web host URI and port number.
    /// </summary>
    internal class WebUrlList : IConfigMap
    {
        /// <summary>
        /// Determines if the configuration map attribute matches the web host URI including port number.
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
                Trace.TraceInformation("OperationContext.Current.Channel.LocalAddress.Uri: {0}", uri);
                hostname = uri.Host;
                port = uri.Port;
            }
            else if (HttpContext.Current != null)
            {
                try
                {
                    var reqUri = HttpContext.Current.Request.Url;
                    Trace.TraceInformation("HttpContext.Current.Request.Url: {0}", reqUri);
                    hostname = reqUri.Host;
                    port = reqUri.Port;
                }
                catch (HttpException ex)
                {
                    Trace.TraceError(
                        "Error: Cannot retrieve Request Url nor port; likely caused by calling DCM from Application_OnStart: {0}",
                        ex.Message);
                    hostname = Environment.MachineName;
                    port = 9999;
                }
            }
            else
            {
                Trace.TraceInformation("Error: Unknown Request Context");
                throw new ApplicationException("Unknown Request Context");
            }

            string requestHostPort = hostname + ":" + port;
            string hostnamePort = Environment.MachineName + ":" + port;

            Trace.TraceInformation("Original requestHostPort: {0}", requestHostPort);
            Trace.TraceInformation("Original hostnamePort: {0}", hostnamePort);

            // allow an optional substring to remove from the compared strings--to make matches
            // domain name agnostic i.e. webUrlList="gus-sit:8087,gus-uat:8087;.bankofamerica.com"
            // thus, the entry will match when running from http://gus-sit.bankofamerica.com:8087 or http://gus-sit:8087
            int semicolon = configMapAttribute.IndexOf(";", StringComparison.OrdinalIgnoreCase);
            if (semicolon >= 0 && (semicolon + 1) <= configMapAttribute.Length)
            {
                string repl = configMapAttribute.Substring(semicolon + 1).ToLower();
                configMapAttribute = configMapAttribute.Substring(0, semicolon);
                configMapAttribute = configMapAttribute.ToLower().Replace(repl, string.Empty);
                requestHostPort = requestHostPort.ToLower().Replace(repl, string.Empty);
            }

            Trace.TraceInformation("Resulting ConfigMapAttribute: {0}", configMapAttribute);
            Trace.TraceInformation("Resulting requestHostPort: {0}", requestHostPort);

            // Walk the list of items split by ',' character, don't forget to trim the spaces
            foreach (string webUrl in configMapAttribute.Split(','))
            {
                Trace.TraceInformation("Comparing {0} with {1} and {2}", webUrl.Trim(), requestHostPort, hostnamePort);
                if (requestHostPort.Equals(webUrl.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    Trace.TraceInformation("Matched Web URL");
                    return true;
                }

                if (hostnamePort.Equals(webUrl.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    Trace.TraceInformation("Matched Machine Name");
                    return true;
                }
            }

            Trace.TraceInformation("No match!");
            return false;
        }
    }
}