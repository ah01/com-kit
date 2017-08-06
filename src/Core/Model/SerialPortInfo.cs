using System;
using System.Text.RegularExpressions;

namespace ComKit.Core
{
    public class SerialPortInfo : SerialPortBasicInfo
    {
        public string Description { get; internal set; }
        public string Manufacturer { get; internal set; }
        public string Service { get; internal set; }
        public string Status { get; internal set; }
        public string ClassGuid { get; internal set; }
        public string DeviceID { get; internal set; }

        public bool IsUsbDevice
        {
            get
            {
                return DeviceID.StartsWith(@"USB\", System.StringComparison.OrdinalIgnoreCase);
            }
        }
        
        public string FullName { get; internal set; }

        Regex VID_MATCH = new Regex(@"VID_([0-9A-F]{4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        Regex PID_MATCH = new Regex(@"PID_([0-9A-F]{4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public int GetUsbVID()
        {
            if (!IsUsbDevice)
            {
                return -1;
            }

            var m = VID_MATCH.Match(DeviceID);

            if (m.Success)
            {
                var num = m.Groups[1].Value;
                return Convert.ToInt32(num, 16);
            }

            return -1;
        }

        public int GetUsbPID()
        {
            if (!IsUsbDevice)
            {
                return -1;
            }

            var m = PID_MATCH.Match(DeviceID);

            if (m.Success)
            {
                var num = m.Groups[1].Value;
                return Convert.ToInt32(num, 16);
            }

            return -1;
        }

        public object GetUsbDeviceName()
        {
            var d = UsbRepository.FindDevice(GetUsbVID(), GetUsbPID());

            if (d != null)
            {
                return d.DeviceName;
            }

            return null;
        }

        public object GetUsbVendorName()
        {
            var d = UsbRepository.FindDevice(GetUsbVID(), GetUsbPID());

            if (d != null)
            {
                return d.VendorName;
            }

            return null;
        }
    }
}