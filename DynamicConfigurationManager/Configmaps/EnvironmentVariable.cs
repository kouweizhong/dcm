namespace DynamicConfigurationManager.ConfigMaps
{
    using System;

    /// <summary>
    /// EnvironmentVariable matches to any environment variables and value; e.g. environmentVariable="environment=INIT".
    /// </summary>
    internal class EnvironmentVariable : IConfigMap
    {
        /// <summary>
        /// Determines if the given configuration map attribute matches the environment variables of
        /// the application. Create an array of items split by '=' character, don't forget to trim
        /// the spaces.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool Execute(string configMapAttribute)
        {
            string[] envandvalue = configMapAttribute.Split('=');

            if (envandvalue.Length < 2)
            {
                return false;
            }

            string env = Environment.GetEnvironmentVariable(envandvalue[0].Trim());

            if (string.IsNullOrWhiteSpace(env))
            {
                return false;
            }

            return env.Equals(envandvalue[1].Trim(), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}