using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace De.Markellus.Altf4.Gui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose();
            base.OnExit(e);
        }
    }
}
