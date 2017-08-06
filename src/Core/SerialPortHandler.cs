using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ComKit.Core
{
    public class SerialPortHandler
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr securityAttrs, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);


        // http://msdn.microsoft.com/en-us/library/windows/desktop/aa365247(v=vs.85).aspx#win32_device_namespaces

        public static bool IsAvailable(string name)
        {
            int dwFlagsAndAttributes = 0x40000000;

            //Borrowed from Microsoft's Serial Port Open Method :)
            var hFile = CreateFile(@"\\.\" + name, -1073741824, 0, IntPtr.Zero, 3, dwFlagsAndAttributes, IntPtr.Zero);

            if (hFile.IsInvalid)
            {
                return false;
            }

            hFile.Close();
            return true;
        }

        public static void GetApp(string device)
        {
            Process process = new Process();
            process.StartInfo.FileName = @"c:\Tools\sysinternals\handle.exe";
            process.StartInfo.Arguments = "-a"; // Note the /c command (*)

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            
            //* Read the output (or the error)
            //string output = process.StandardOutput.Rad .ReadToEnd();

            //var p = output.IndexOf(device);

            //output.LastIndexOf()


            
            process.WaitForExit();
        }


    }
}
