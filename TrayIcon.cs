using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotSwitcher
{
    public class TrayIcon
    {
        public event EventHandler<EventArgs> DoubleClick;
        public event EventHandler<EventArgs> ExitClick;
        public event EventHandler<EventArgs> SettingsClick;
        public event EventHandler<EventArgs> TogglePowerClick;

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private MenuItem togglePowerItem;
        private bool wasShownBeforeTooltip;

        public TrayIcon (bool? visible = true)
	    {
            trayMenu = new ContextMenu();
            togglePowerItem = new MenuItem("", OnPowerClick);
            trayMenu.MenuItems.Add(togglePowerItem);
            trayMenu.MenuItems.Add("Settings", OnSettingsClick);
            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Exit", OnExitClick);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "dotSwitcher";
            trayIcon.Icon = Properties.Resources.icon;
            trayIcon.BalloonTipClosed += trayIcon_BalloonTipClosed;

            trayIcon.MouseDoubleClick += trayIcon_Click;
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = visible == true;
	    }


        public void SetRunning(bool isRunning)
        {
            togglePowerItem.Text = isRunning ? "Turn off" : "Turn on";
        }
        public void Show()
        {
            trayIcon.Visible = true;
        }
        public void Hide()
        {
            trayIcon.Visible = false;
        }
        public void ShowTooltip(string p, ToolTipIcon icon)
        {
            wasShownBeforeTooltip = trayIcon.Visible;
            Show();
            trayIcon.ShowBalloonTip(2000, "dotSwitcher error", p, icon);
        }


        private void OnExitClick(object sender, EventArgs e)
        {
            if (ExitClick != null)
            {
                ExitClick(this, null);
            }
        }

        private void OnSettingsClick(object sender, EventArgs e)
        {
            if (SettingsClick != null)
            {
                SettingsClick(this, null);
            }
        }

        private void OnPowerClick(object sender, EventArgs e)
        {
            if (TogglePowerClick != null)
            {
                TogglePowerClick(this, null);
            }
        }

        void trayIcon_Click(object sender, MouseEventArgs e)
        {
            if (DoubleClick != null)
            {
                DoubleClick(this, null);
            }
        }

        void trayIcon_BalloonTipClosed(object sender, EventArgs e)
        {
            trayIcon.Visible = wasShownBeforeTooltip;
        }
    }
}
