using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    /// ExecutablePath compares the configMapAttribute value to the fully qualified path of the
    /// application hosts current directory.
    /// </summary>
    internal class AssemblyPath : IConfigMapAttribute
    {
        public bool Execute(string configMapAttribute)
        {
            var rtnValue = false;

            if (configMapAttribute.Length != 0)
            {
                Trace.TraceInformation("ConfigMapAttribute:{0}", configMapAttribute);

                var assemblyPath = Assembly.GetExecutingAssembly().CodeBase;
                Trace.TraceInformation("AssemblyPath: matching to {0}", assemblyPath);

                var fullPath = String.Empty;

                if (configMapAttribute.StartsWith("file:///"))
                {
                    fullPath = configMapAttribute;
                }
                else
                {
                    fullPath = Path.GetFullPath(configMapAttribute.TrimEnd('\\'));
                }

                Trace.TraceInformation("AssemblyPath: '{0}'; FullPath: '{1}'", assemblyPath, fullPath);

                rtnValue = fullPath.Equals(assemblyPath, StringComparison.InvariantCultureIgnoreCase);
            }

            return rtnValue;
        }
    }
}