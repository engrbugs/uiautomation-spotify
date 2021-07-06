using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace uiautomation_spotify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PropertyCondition documentCond = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document);
        PropertyCondition textCond = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document);
        PropertyCondition customCond = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Custom);
        System.Windows.Automation.Condition allCond = System.Windows.Automation.Condition.TrueCondition;
        System.Windows.Automation.Condition rawCond = Automation.RawViewCondition;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void ElementControlWalk(AutomationElement sibling, int depthLimit, int contDepth = 0)
        {
            contDepth++;
            if (sibling.Current.ControlType == ControlType.Text)
            {
                Debug.WriteLine(sibling.Current.Name);
            }
            AutomationElementCollection elementNode = sibling.FindAll(TreeScope.Children, allCond);
            foreach (AutomationElement element in elementNode)
            {
                if (depthLimit <= contDepth)
                {
                    return;
                }
                ElementControlWalk(element, depthLimit, contDepth);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int[] hwnds = { 460266 };

            Debug.WriteLine("hello", "hello");
            // 9240    <-- Notepad
            // 11632     <-- Spotify
            foreach (int hwnd in hwnds)
            {
                AutomationElement root = AutomationElement.FromHandle((IntPtr)hwnd);
                AutomationElementCollection rootNode = root.FindAll(TreeScope.Children, documentCond);
                foreach (AutomationElement sibling in rootNode)
                {
                    ElementControlWalk(sibling, 10);
                }
            }
            Debug.WriteLine("End.");
        }
        private static WINDOWPLACEMENT GetPlacement(IntPtr hwnd)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(hwnd, ref placement);
            return placement;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public ShowWindowCommands showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        internal enum ShowWindowCommands : int
        {
            Hide = 0,
            Normal = 1,
            Minimized = 2,
            Maximized = 3,
        }
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_SHOWMAXIMIZED = 3;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int hwnds = 460266;
            var placement = GetPlacement((IntPtr)hwnds);
            ShowWindow((IntPtr)hwnds, SW_SHOWMAXIMIZED);
        }
    }
}
