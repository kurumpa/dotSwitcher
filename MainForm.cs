using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotSwitcher
{
    public class SysTrayApp : Form
    {
        private Switcher engine;
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private MenuItem power;

        public SysTrayApp(Switcher engine)
        {
            this.engine = engine;
            trayMenu = new ContextMenu();
            power = new MenuItem("", OnPower);
            trayMenu.MenuItems.Add(power);
            trayMenu.MenuItems.Add("Exit", OnExit);


            trayIcon = new NotifyIcon();
            trayIcon.Text = "dotSwitcher";
            trayIcon.Icon = Properties.Resources.icon;

            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;

            
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;

            engine.Error += OnEngineError;
            engine.Start();
            UpdateMenu();
            base.OnLoad(e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            engine.Error -= OnEngineError;
            base.OnClosing(e);
        }

        private void OnEngineError(object sender, SwitcherErrorArgs args)
        {
            var ex = args.Error;
            trayIcon.ShowBalloonTip(2000, "dotSwitcher error", ex.ToString(), ToolTipIcon.None);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void UpdateMenu()
        {
            if (engine.IsStarted())
            {
                power.Text = "Turn off";
            }
            else
            {
                power.Text = "Turn on";
            }
        }
        private void OnPower(object sender, EventArgs e)
        {
            if (engine.IsStarted()) { engine.Stop(); }
            else { engine.Start(); }
            UpdateMenu();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                trayIcon.Dispose();
            }
            base.Dispose(isDisposing);
        }
    }
}
