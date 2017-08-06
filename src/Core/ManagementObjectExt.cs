using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ComKit.Core
{
    static class ManagementObjectExt
    {
        public static string GetValue(this ManagementObject obj, string key)
        {
            return obj[key]?.ToString().Trim();
        }
    }
}
