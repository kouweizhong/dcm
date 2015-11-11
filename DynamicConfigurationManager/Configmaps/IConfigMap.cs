namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    /// </summary>
    public interface IConfigMap
    {
        /// <summary>
        /// </summary>
        /// <param name="configMapAttribute"></param>
        /// <returns></returns>
        bool Execute(string configMapAttribute);
    }
}