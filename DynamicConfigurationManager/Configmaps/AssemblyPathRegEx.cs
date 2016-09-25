using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     AssemblyPath compares the configMapAttribute value to the fully qualified path of the
    ///     application hosts current directory using RegEx.
    /// </summary>
    internal class AssemblyPathRegEx : IConfigMap
    {
        /// <summary>
        ///     Determine if there is a match of the given regular expression and path to the assembly.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool IsMatch(string configMapAttribute)
        {
            if (configMapAttribute.Length == 0) return false;
            var assemblyPath = Assembly.GetExecutingAssembly().CodeBase;
            var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);

            Trace.WriteLine("AssemblyPathRegEx: matching to {0}", assemblyPath);

            return re.IsMatch(assemblyPath);
        }
    }
}