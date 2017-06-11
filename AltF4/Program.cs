using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using System.Collections.Generic;
using System.IO;

namespace AltF4
{
    class Program
    {
        private static Process procFocus;

        private static void Main(string[] args)
        {
            Process[] _procRunning = Process.GetProcessesByName("AltF4");

            if(_procRunning.Length > 0)
            {
                Environment.Exit(0);
            }

            AutomationFocusChangedEventHandler focusHandler = OnFocusChanged;
            Automation.AddAutomationFocusChangedEventHandler(focusHandler);

            AltF4Handler.Get().OnAltF4 += OnAltF4;

            Application.Run();
        }

        private static void OnAltF4(object sender, EventArgs e)
        {
            if (procFocus != null && !procFocus.HasExited)
            {
                try
                {
                    procFocus.Kill();
                }
                catch { }
            }
        }

        private static void OnFocusChanged(object sender, AutomationFocusChangedEventArgs e)
        {
            AutomationElement focusedElement = sender as AutomationElement;
            if (focusedElement != null)
            {
                int processId = focusedElement.Current.ProcessId;
                procFocus = Process.GetProcessById(processId);
            }
        }  
    }
}
