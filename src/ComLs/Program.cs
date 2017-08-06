using ComKit.Core;
using ComKit.Core.Cli;
using System;
using System.Linq;
using System.Reflection;

namespace ComKit.ComLs
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new ArgumentsParser();
            parser.Parse(args);
            
            if (parser.CheckFlag("h") || parser.CheckFlag("?"))
            {
                PrinteHelp();
                return;
            }

            var detectOpenPorts = parser.CheckFlag("o");
            var verbose = parser.CheckFlag("v");

            if (parser.CheckFlag("a"))
            {
                PrintDetailList(detectOpenPorts, verbose);
            }
            else if (parser.CheckFlag("n"))
            {
                PrintOnlyNames();
            }
            else
            {
                PrintBasicList();
            }
        }

        private static void PrinteHelp()
        {
            var ass = Assembly.GetExecutingAssembly();
            var ns = ass.GetTypes().Select(t => t.Namespace).FirstOrDefault();
            
            using (var stream = ass.GetManifestResourceStream(ns + ".help.txt"))
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    var help = reader.ReadToEnd();
                    Console.WriteLine(help.Trim());
                }
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

        private static void PrintDetailList(bool detectOpen, bool verbose)
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

                if (verbose)
                {
                    Console.WriteLine($"  Name          : {x.FullName}");
                }

                Console.WriteLine($"  Description   : {x.Description}");
                Console.WriteLine($"  Manufacturer  : {x.Manufacturer}");

                if (verbose)
                {
                    Console.WriteLine($"  Service       : {x.Service}");
                    Console.WriteLine($"  Status        : {x.Status}");
                    Console.WriteLine($"  DeviceID      : {x.DeviceID}");
                    Console.WriteLine($"  ClassGuid     : {x.ClassGuid}");
                }

                Console.WriteLine($"  Is USB device : {(x.IsUsbDevice ? "YES" : "NO")}");

                if (x.IsUsbDevice)
                {
                    Console.WriteLine();

                    if (verbose)
                    {
                        Console.WriteLine($"  USB VID       : {x.GetUsbVID():X}");
                        Console.WriteLine($"  USB PID       : {x.GetUsbPID():X}");
                    }
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
