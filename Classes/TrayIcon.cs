using System;
using System.Windows.Forms;

namespace dotSwitcher
{
    class TrayIcon
    {
        public event EventHandler<EventArgs> Exit;
        public event EventHandler<EventArgs> ShowHide;
        private NotifyIcon trIcon;
        private ContextMenu cMenu;
        public TrayIcon(bool? visible = true)
        {
            trIcon = new NotifyIcon();
            cMenu = new ContextMenu();
            trIcon.Icon = Properties.Resources.icon;
            trIcon.Visible = visible == true;
            cMenu.MenuItems.Add(new MenuItem("Show/Hide", ShowHideHandler));
            cMenu.MenuItems.Add(new MenuItem("Exit", ExitHandler));
            trIcon.Text = "dotSwitcher \nA simple layout switcher";
            trIcon.ContextMenu = cMenu;
            trIcon.MouseDoubleClick += ShowHideHandler;
            trIcon.BalloonTipClicked += ExitHandler;
        }
        private void ExitHandler(object sender, EventArgs e)
        {
            if (Exit != null)
            {
                Exit(this, null);
            }
        }
        private void ShowHideHandler(object sender, EventArgs e)
        {
            if (ShowHide != null)
            {
                ShowHide(this, null);
            }
        }
        public void Hide()
        {
            trIcon.Visible = false;
        }
        public void Show()
        {
            trIcon.Visible = true;
        }
    }
}
