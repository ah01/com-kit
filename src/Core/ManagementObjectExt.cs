using System.Management;

namespace ComKit.Core
{
    /// <summary>
    /// Extension methods for <see cref="ManagementObject"/>
    /// </summary>
    static class ManagementObjectExt
    {
        /// <summary>
        /// Get string value or null
        /// </summary>
        /// <param name="obj">Management object</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static string GetValue(this ManagementObject obj, string key)
        {
            return obj[key]?.ToString().Trim();
        }
    }
}
