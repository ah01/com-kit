using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Text.RegularExpressions;

namespace ComKit.Core
{
    public class SerialPortList
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr securityAttrs, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);


        public static string[] GetNames()
        {
            return SerialPort.GetPortNames();
        }

        public static IEnumerable<SerialPortBasicInfo> GetBasicList()
        {
            return GetNames().Select(x => new SerialPortBasicInfo { Name = x }).OrderBy(x => x.Number);
        }

        public static IEnumerable<SerialPortInfo> GetDetailList()
        {
            var list = new List<SerialPortInfo>();

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");
                
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["Caption"] == null) continue;

                    var caption = queryObj["Caption"].ToString();

                    if (caption.Contains("(COM") || caption.StartsWith("serial port"))
                    {

                        var info = GetInfoFromPnPEntity(queryObj);

                        list.Add(info);


                        //foreach (PropertyData p in queryObj.Properties)
                        //{
                        //    Console.WriteLine("  {0} ({2}) : {1}", p.Name, p.Value, p.Origin);
                        //}


                        /*
                        Console.WriteLine(queryObj["Caption"] + ":");
                        Console.WriteLine("   Name:          {0}", queryObj["Name"]);
                        Console.WriteLine("   Description:   {0}", queryObj["Description"]);
                        Console.WriteLine("   Manufacturer:  {0}", queryObj["Manufacturer"]);
                        Console.WriteLine("   Service:       {0}", queryObj["Service"]);
                        Console.WriteLine("   Status:        {0}", queryObj["Status"]);
                        Console.WriteLine("   ClassGuid:     {0}", queryObj["ClassGuid"]);
                        */
                        
                    }
                }
                
            }
            catch (ManagementException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }


            return list.OrderBy(x => x.Number);
        }

        private static SerialPortInfo GetInfoFromPnPEntity(ManagementObject queryObj)
        {
            var info = new SerialPortInfo();

            info.FullName = queryObj.GetValue("Name");
            info.Name = GetPortNameFromFullName(info.FullName);
            info.Description = queryObj.GetValue("Description");
            info.Manufacturer = queryObj.GetValue("Manufacturer");
            info.Service = queryObj.GetValue("Service");
            info.Status = queryObj.GetValue("Status");
            info.ClassGuid = queryObj.GetValue("ClassGuid");
            info.DeviceID = queryObj.GetValue("DeviceID");

            return info;
        }

        static readonly Regex COM_MATCH = new Regex(@"COM\d+", RegexOptions.Compiled);

        private static string GetPortNameFromFullName(string fullName)
        {
            var m = COM_MATCH.Match(fullName);

            if (m.Success)
            {
                return m.Value;
            }

            return null;
        }
        
    }
}
