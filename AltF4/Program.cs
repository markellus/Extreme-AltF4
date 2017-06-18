using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Automation;

namespace AltF4
{
    /// <summary>
    /// Main class
    /// </summary>
    class Program
    {
        /// <summary>
        /// The program which is currently in foreground.
        /// </summary>
        private static Process procFocus;

        /// <summary>
        /// Program entry method
        /// </summary>
        private static void Main(string[] args)
        {
            //check if the program is already running
            Process[] _procRunning = Process.GetProcessesByName("AltF4");

            if(_procRunning.Length > 0)
            {
                Environment.Exit(0);
            }

            //Add event handlers
            AutomationFocusChangedEventHandler focusHandler = OnFocusChanged;
            Automation.AddAutomationFocusChangedEventHandler(focusHandler);

            AltF4Handler.Get().OnAltF4 += OnAltF4;

            //Start program loop
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
