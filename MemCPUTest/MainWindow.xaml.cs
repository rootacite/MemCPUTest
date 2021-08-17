using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MemCPUTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("Patch.dll")]
        extern static ulong getAvailablePhysMemBytes();
        [DllImport("Patch.dll")]
        extern static ulong Test();
        [DllImport("Patch.dll")]
        extern static void FreeMems();
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer t = new DispatcherTimer();
            t.Interval = TimeSpan.FromSeconds(1);
            t.Tick += (f, v) =>
            {
                ulong left = getAvailablePhysMemBytes();

                LeftMem.Text = left.ToString() + "(" + Math.Round(left / (1024d * 1024d * 1024d), 2).ToString() + "GB" + ")";
            };

            t.Start();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Start.IsEnabled = false;
            Start.Content = "进行中......";

             new Thread(() =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                ulong rest = Test();

                sw.Stop();
                long time = sw.ElapsedMilliseconds;

                this.Dispatcher.Invoke(() =>
                {
                    Start.Content = "完成";
                    Refmem.Text = rest.ToString() + "(" + Math.Round(rest / (1024d), 2).ToString() + "GB" + ")";
                    TS.Text = (time / (double)rest).ToString() + "ms";
                    MenRet.Text = ((((double)rest / (double)time))*1000d).ToString() + "MB/s";
                });

                FreeMems();
            })
            { IsBackground=true}.Start();
        }
    }
}
