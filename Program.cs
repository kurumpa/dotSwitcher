using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace dotSwitcher
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{D09B5AC3-1219-4987-88A3-7C80075A571B}");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var settings = Settings.Init();
                var engine = new Switcher(settings);
                Application.ApplicationExit += (s, a) => { engine.Dispose(); };
                var app = new SettingsForm(settings, engine);
                app.Exit += (s, e) => Application.Exit();
                var context = new ApplicationContext(app);
                Application.Run(context);
                mutex.ReleaseMutex();
            }
            else
            {
                LowLevelAdapter.SendShowSettingsMessage();
            }
        }
    }
}
