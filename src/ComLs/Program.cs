using ComKit.Core;
using ComKit.Core.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComLs
{
    class Program
    {

        /*
        
            Flags:

                -n  ... names only
                -a  ... all information detail
                -v  ... verbose detail
                -o  ... check if port is open

        */

        static void Main(string[] args)
        {
            var parser = new ArgumentsParser();
            parser.Parse(args);

            if (parser.CheckFlag("a"))
            {
                PrintDetailList(false);
            }
            else
            {
                PrintBasicList();
            }
        }

        private static void PrintOnlyNames()
        {
            var p = SerialPortList.GetBasicList();

            foreach (var x in p)
            {
                Console.WriteLine(x.Name);
            }
        }

        private static void PrintBasicList()
        {
            var p = SerialPortList.GetDetailList();

            foreach (var x in p)
            {
                var text = $"{x.Name,-6} {x.FullName}";

                if (x.IsUsbDevice)
                {
                    text += $" [USB {x.GetUsbVendorName()} {x.GetUsbDeviceName()}]";
                }

                Console.WriteLine(text);
            }
        }

        private static void PrintDetailList(bool detectOpen)
        {
            var p = SerialPortList.GetDetailList();

            Console.WriteLine();

            foreach (var x in p)
            {
                Console.WriteLine(x.Name + ":");
                Console.WriteLine();

                if (detectOpen)
                {
                    Console.Write($"  Is Open       : ");
                    if (x.IsAvailable())
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("NO");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("YES");
                        Console.ResetColor();
                    }
                }

                Console.WriteLine($"  Name          : {x.FullName}");
                Console.WriteLine($"  Description   : {x.Description}");
                Console.WriteLine($"  Manufacturer  : {x.Manufacturer}");
                Console.WriteLine($"  Service       : {x.Service}");
                Console.WriteLine($"  Status        : {x.Status}");
                Console.WriteLine($"  DeviceID      : {x.DeviceID}");
                Console.WriteLine($"  ClassGuid     : {x.ClassGuid}");
                Console.WriteLine($"  Is USB device : {(x.IsUsbDevice ? "YES" : "NO")}");

                if (x.IsUsbDevice)
                {
                    Console.WriteLine();
                    Console.WriteLine($"  USB VID       : {x.GetUsbVID():X}");
                    Console.WriteLine($"  USB PID       : {x.GetUsbPID():X}");
                    Console.WriteLine($"  USB Vendor    : {x.GetUsbVendorName()}");
                    Console.WriteLine($"  USB Device    : {x.GetUsbDeviceName()}");
                }

                Console.WriteLine("");
            }

            if (p.Count() == 0)
            {
                Console.WriteLine("There is not serial port.");
            }
            else if (p.Count() == 1)
            {
                Console.WriteLine("There is 1 serial port.");
            }
            else
            {
                Console.WriteLine($"There are {p.Count()} serial ports.");
            }
        }
    }

}
