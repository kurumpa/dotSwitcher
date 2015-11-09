using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace dotSwitcher
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Mutex mutex = new Mutex(true, "{D09B5AC3-1219-4987-88A3-7C80075A571B}");
        private SettingsForm app;
        private Switcher engine;
        private Settings settings;


        //Application Level exception handler
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var comException = e.Exception as COMException;

            if (comException != null && comException.ErrorCode == -2147221040)
                e.Handled = true;
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            engine.Dispose();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                settings = Settings.Init();
                engine = new Switcher(settings);
                app = new SettingsForm(settings, engine);
                app.Exit += (s, ee) => Environment.Exit(0);
                app.Show();

                mutex.ReleaseMutex();
            }
            else
            {
                LowLevelAdapter.SendShowSettingsMessage();
            }
            
        }
    }
}