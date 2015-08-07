using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotSwitcher
{
    public partial class SettingsForm : Form
    {
        public event EventHandler<EventArgs> Exit;
        private Settings settings;
        private Switcher engine;
        private TrayIcon icon;
        private bool exiting = false;

        KeyboardHook kbdHook;
        KeyboardEventArgs currentHotkey;
        TextBox currentTextBox;

        public SettingsForm(Settings settings, Switcher engine)
        {
            this.settings = settings;
            this.engine = engine;
            engine.Error += OnEngineError;

            icon = new TrayIcon(settings.ShowTrayIcon);
            icon.DoubleClick += icon_SettingsClick;
            icon.ExitClick += icon_ExitClick;
            icon.SettingsClick += icon_SettingsClick;
            icon.TogglePowerClick += icon_TogglePowerClick;
            icon.SetRunning(engine.IsStarted());

            currentTextBox = null;
            kbdHook = new KeyboardHook();
            kbdHook.KeyboardEvent += kbdHook_KeyboardEvent;

            InitializeComponent();
            InitializeValues();
        }

        private void ShowForm()
        {
            kbdHook.Start();
            icon.Hide();
            engine.Stop();
            TopMost = true;
            Show();
        }
        private void HideForm()
        {
            kbdHook.Stop();
            if (!exiting)
            {
                engine.Start();
                if (settings.ShowTrayIcon == true)
                {
                    icon.SetRunning(true);
                    icon.Show();
                }
            }
            TopMost = false;
            Hide();
        }

        private void buttonCancelSettings_Click(object sender, EventArgs e)
        {
            settings.Reload();
            shortcutTextBox.Text = settings.SwitchHotkey.ToString();
            HideForm();
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            if (settings.SwitchHotkey.Alt || settings.SwitchHotkey.Win)
            {
                MessageBox.Show("Sorry, win+ and alt+ hotkeys are not supported yet");
                return;
            }
            settings.Save();
            HideForm();
        }
        private void OnEngineError(object sender, SwitcherErrorArgs e)
        {
            icon.ShowTooltip(e.Error.Message, ToolTipIcon.Error);
        }
        private void InitializeValues()
        {
            shortcutTextBox.LostFocus += unsetCurrentInput;
            shortcutTextBox.Leave += unsetCurrentInput;
            shortcutTextBox.GotFocus += setCurrentInput;
            shortcutTextBox.Enter += setCurrentInput;
            shortcutTextBox.Text = settings.SwitchHotkey.ToString();
        }
        private void setCurrentInput(object sender, EventArgs e)
        {
            currentTextBox = (TextBox)sender;
        }
        private void unsetCurrentInput(object sender, EventArgs e)
        {
            currentTextBox = null;
        }

        private void shortcutTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            settings.SwitchHotkey = currentHotkey;
        }

        void icon_TogglePowerClick(object sender, EventArgs e)
        {
            if (engine.IsStarted())
            {
                engine.Stop();
                icon.SetRunning(false);
            }
            else
            {
                engine.Start();
                icon.SetRunning(true);
            }
        }

        void icon_SettingsClick(object sender, EventArgs e)
        {
            ShowForm();
        }

        void icon_ExitClick(object sender, EventArgs e)
        {
            exiting = true;
            engine.Stop();
            if (Exit != null)
            {
                Exit(this, null);
            }
        }

        protected override void WndProc(ref Message m)
        {

            if (m.Msg == LowLevelAdapter.WM_SHOW_SETTINGS)
            {
                ShowForm();
            }
            base.WndProc(ref m);
        }
        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            HideForm();
        }
        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !exiting;
            if (e.CloseReason == CloseReason.UserClosing)
            {
                buttonCancelSettings_Click(this, e);
                return;
            }
            HideForm();
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            engine.Error -= OnEngineError;
        }
        void kbdHook_KeyboardEvent(object sender, KeyboardEventArgs e)
        {
            if (currentTextBox != null)
            {
                var vk = e.KeyCode;
                if (vk != Keys.LMenu && vk != Keys.RMenu
                    && vk != Keys.LWin && vk != Keys.RWin
                    && vk != Keys.LShiftKey && vk != Keys.RShiftKey
                    && vk != Keys.LControlKey && vk != Keys.RControlKey)
                {
                    e.Handled = true;
                }
                Invoke((MethodInvoker)delegate { currentTextBox.Text = e.ToString(); });
                currentHotkey = e;
            }
        }
        
        
    }
}
