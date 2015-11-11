namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    /// </summary>
    public interface IConfigMapAttribute
    {
        /// <summary>
        /// </summary>
        /// <param name="configMapAttribute"></param>
        /// <returns></returns>
        bool Execute(string configMapAttribute);
    }
}