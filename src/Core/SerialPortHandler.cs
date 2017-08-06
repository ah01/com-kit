using System;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace ComKit.Core
{
    public static class SerialPortHandler
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr securityAttrs, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);
        
        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa365247(v=vs.85).aspx#win32_device_namespaces

        public static bool IsAvailable(string name)
        {
            int dwFlagsAndAttributes = 0x40000000;

            // Borrowed from Microsoft's Serial Port Open Method :)
            var hFile = CreateFile(@"\\.\" + name, -1073741824, 0, IntPtr.Zero, 3, dwFlagsAndAttributes, IntPtr.Zero);

            if (hFile.IsInvalid)
            {
                return false;
            }

            hFile.Close();
            return true;
        }
    }
}
