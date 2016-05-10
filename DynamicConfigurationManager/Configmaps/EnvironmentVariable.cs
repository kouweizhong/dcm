using System;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     EnvironmentVariable matches to any environment variables and value; e.g. environmentVariable="environment=TEST".
    /// </summary>
    internal class EnvironmentVariable : IConfigMap
    {
        /// <summary>
        ///     Determines if the given configuration map attribute matches the environment variables of
        ///     the application. Create an array of items split by '=' character, don't forget to trim
        ///     the spaces.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool Execute(string configMapAttribute)
        {
            var envandvalue = configMapAttribute.Split('=');

            if (envandvalue.Length < 2)
            {
                return false;
            }

            var env = Environment.GetEnvironmentVariable(envandvalue[0].Trim());

            return !string.IsNullOrWhiteSpace(env) && env.Equals(envandvalue[1].Trim(), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}