using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComKit.Core
{
    public class UsbRepository
    {
        struct VendorInfo
        {
            internal string name;
            internal Dictionary<int, string> devices;
        }

        static Dictionary<int, VendorInfo> vendors;

        private static void LoadFile()
        {
            vendors = new Dictionary<int, VendorInfo>();

            // http://www.linux-usb.org/usb-ids.html
            var path = @"c:\Users\AdamHorcica\Dropbox\Pracovni\Dev\com-kit\src\Core\Data\usb.ids";
            VendorInfo? currentVendor = null;

            foreach(var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                {
                    continue;
                }

                if (line.StartsWith("C ")) // begin of known classes (not interested)
                {
                    break;
                }

                if (line.StartsWith("\t\t")) // interface name (not interested)
                {
                    continue;
                }
                
                var l = ParseLine(line);

                if (line.StartsWith("\t"))
                {
                    if (currentVendor == null)
                    {
                        throw new InvalidOperationException("Fail to parse USB file");
                    }

                    currentVendor.Value.devices[l.Item1] = l.Item2;
                    continue;
                }

                currentVendor = new VendorInfo
                {
                    name = l.Item2,
                    devices = new Dictionary<int, string>()
                };
                
                vendors[l.Item1] = currentVendor.Value;
            }



        }

        private static Tuple<int, string> ParseLine(string line)
        {
            line = line.Trim();
            var del = line.Trim().IndexOf("  ");

            var id = line.Substring(0, del);
            var name = line.Substring(del).Trim();

            return Tuple.Create(Convert.ToInt32(id, 16), name);
        }


        public static UsbDeviceInfo FindDevice(int vid, int pid)
        {
            if (vendors == null)
            {
                LoadFile();
            }

            if (vendors.ContainsKey(vid))
            {
                var info = new UsbDeviceInfo();
                var vendor = vendors[vid];

                info.VendorName = vendor.name;

                if (vendor.devices.ContainsKey(pid))
                {
                    info.DeviceName = vendor.devices[pid];
                }

                return info;
            }

            return null;
        }

    }
}
