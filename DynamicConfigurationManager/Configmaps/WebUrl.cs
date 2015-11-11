using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Web;

namespace DynamicConfigurationManager.ConfigMaps
{
    internal class WebUrl : IConfigMap
    {
        public bool Execute(string configMapAttribute)
        {
            int port;
            string hostname;
            if (System.ServiceModel.OperationContext.Current != null
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
                var reqUri = HttpContext.Current.Request.Url;
                Trace.TraceInformation("HttpContext.Current.Request.Url: {0}", reqUri);
                hostname = reqUri.Host;
                port = reqUri.Port;
            }
            else
            {
                Trace.TraceError("Error: Unknown Request Context");
                throw new ApplicationException("Unknown Request Context");
            }

            string mWebUrl = hostname + (port == 80 ? string.Empty : ":" + port);
            return mWebUrl.Equals(configMapAttribute, StringComparison.OrdinalIgnoreCase);
        }
    }
}