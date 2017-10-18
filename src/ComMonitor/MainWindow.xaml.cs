using ComKit.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // https://msdn.microsoft.com/en-us/library/windows/desktop/aa363480%28v=vs.85%29.aspx
        public const int DBT_DEVNODES_CHANGED = 0x0007; // A device has been added to or removed from the system.
        public const int WmDevicechange = 0x0219; // device change event 
        
        public MainWindow()
        {
            InitializeComponent();            
        }


        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            
        }


        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            ports = SerialPortList.GetNames().ToList();

            var source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);

            this.Visibility = Visibility.Hidden;
        }

        public List<string> ports;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WmDevicechange)
            {
                //Debug.Print($"WmDevicechange {wParam} {lParam}");
                //Debug.Print($"  " + string.Join(", ", SerialPortList.GetBasicList().Select(x => x.Name)));

                var newPorts = SerialPortList.GetNames();

                if (newPorts.Count() != ports.Count)
                {
                    var added = newPorts.Except(ports);
                    var removed = ports.Except(newPorts);

                    ports = newPorts.ToList();

                    foreach(var i in added)
                    {
                        Debug.Print("New port " + i);
                        Task.Run(() => Dispatcher.Invoke(() => ShowNewPortNotification(i)));
                    }

                    foreach (var i in removed)
                    {
                        Debug.Print("Removed port " + i);
                        Task.Run(() => Dispatcher.Invoke(() => ShowRemovedPortNotification(i)));
                    }
                }
            }


            return IntPtr.Zero;
        }

        private void ShowRemovedPortNotification(string i)
        {
            tb.ShowBalloonTip(i + " removed", "", Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
        }

        private void ShowNewPortNotification(string i)
        {
            var port = SerialPortList.GetDetailList().FirstOrDefault(x => x.Name == i);

            if (port == null)
            {
                throw new Exception("Missing details!");
            }

            var note = $"{port.Manufacturer}";

            if (port.IsUsbDevice)
            {
                note += $"\n({port.GetUsbVendorName()}, {port.GetUsbDeviceName()})";
            }

            tb.ShowBalloonTip($"{i} detected ({port.Description})", note, Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
