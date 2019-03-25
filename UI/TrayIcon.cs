/**
 *   DotSwitcher: a simple keyboard layout switcher
 *   Copyright (C) 2014-2019 Kirill Mokhovtsev / kurumpa
 *   Contact: kiev.programmer@gmail.com
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotSwitcher.UI
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
