namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    /// Interface for a configuration map handler.
    /// </summary>
    public interface IConfigMap
    {
        /// <summary>
        /// Exceute method of a configuration map handler.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        bool Execute(string configMapAttribute);
    }
}