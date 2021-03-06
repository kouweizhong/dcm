namespace DynamicConfigurationManager.ConfigMaps
{
    using System;

    /// <summary>
    /// UpdateLocation compares the configuration map attribute value to the fully qualified path of
    /// the click once application manifest.
    /// </summary>
    internal class UpdateLocation : IConfigMap
    {
        /// <summary>
        /// Determines if the given configuration map attribute matches the path to the click once
        /// application manifest.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool Execute(string configMapAttribute)
        {
            bool rtnValue = false;
#if !DEBUG
            if( ApplicationDeployment.IsNetworkDeployed )
#endif
            {
                // allow an optional substring to remove from the compared strings--to make matches
                // domain name agnostic i.e.
                // UpdateLocation="http://myapp.mydomain.com/clickonce/myapp.application;.mydomain.com"
                // thus, the entry will match when deployed from
                // http://myapp.mydomain.com/clickonce/myapp.application or
                // http://myapp/clickonce/myapp.application (depending on how the click once
                // manifest was coded)
                string repl = null;
                var semi = configMapAttribute.IndexOf(";", StringComparison.Ordinal);

                if (semi >= 0 && (semi + 1) <= configMapAttribute.Length)
                {
                    repl = configMapAttribute.Substring(semi + 1).ToLower();
                    configMapAttribute = configMapAttribute.Substring(0, semi);
                }

#if DEBUG
                var actualUri = new Uri("http://myapp/clickonce/myapp.application");
#else
                var actualUri = ApplicationDeployment.CurrentDeployment.UpdateLocation;
#endif
                string actualhost = actualUri.Host.ToLower();

                if (repl != null)
                {
                    actualhost = actualhost.Replace(repl, string.Empty);
                }

                var actual = actualhost + actualUri.Port + actualUri.AbsolutePath;
                var compareUri = new Uri(configMapAttribute);
                string comparehost = compareUri.Host.ToLower();

                if (repl != null)
                {
                    comparehost = comparehost.Replace(repl, string.Empty);
                }

                var compare = comparehost + compareUri.Port + compareUri.AbsolutePath;

                rtnValue = actual.Equals(compare, StringComparison.InvariantCultureIgnoreCase);
            }

            return rtnValue;
        }
    }
}