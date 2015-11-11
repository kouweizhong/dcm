using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    /// this class is very unreliable release builds will sometimes optimize away entire methods, so
    /// the call stack would be different
    /// see: http://stackoverflow.com/a/15368508/57883
    /// </summary>
    internal class CallingTypeFullnameRegEx : IConfigMap
    {
        public static IEnumerable<string> GetCallingTypesFullNames()
        {
            var stackTrace = new StackTrace();
            StackFrame[] frames = stackTrace.GetFrames();
            if (frames == null)
                return null;
            return frames.Select(f => f.GetMethod().DeclaringType).Distinct().Select(t => t.FullName);
        }

        public bool Execute(string configMapAttribute)
        {
            if (!string.IsNullOrWhiteSpace(configMapAttribute))
            {
                var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);
                var stackTrace = new StackTrace();
                var typeNames = GetCallingTypesFullNames().ToArray();
                if (typeNames != null)
                    foreach (var fullName in typeNames)
                    {
                        Trace.TraceInformation("CallingTypeRegEx: matching to {0}", fullName);
                        if (re.IsMatch(fullName))
                            return true;
                    }
            }
            return false;
        }
    }
}