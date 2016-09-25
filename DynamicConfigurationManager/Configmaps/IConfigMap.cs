namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     Interface for a configuration map handler.
    /// </summary>
    public interface IConfigMap
    {
        /// <summary>
        ///     Determine if there is a match for the given configuration map handler.
        /// </summary>
        /// <param name="attributeValue">The current configMap attribute value.</param>
        /// <returns>Return true if we found a match.</returns>
        bool IsMatch(string attributeValue);
    }
}