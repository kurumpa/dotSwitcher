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
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private MenuItem power;
        private bool running = false;

        public SysTrayApp()
        {
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);
            power = new MenuItem("Turn on", OnPower);
            trayMenu.MenuItems.Add(power);


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
            trayIcon.ShowBalloonTip(2000, "dotSwitcher", "dotSwitcher started", ToolTipIcon.None);

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void OnPower(object sender, EventArgs e)
        {
            if (running)
            {
                running = false;
                power.Text = "Turn on";
            }
            else
            {
                running = true;
                power.Text = "Turn off";
            }
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
