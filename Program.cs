using System;
using System.Windows.Forms;

namespace dotSwitcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var engine = new Switcher();
            Application.ApplicationExit += (s, a) => engine.Dispose();
            Application.Run(new SysTrayApp(engine));
        }
    }
}
