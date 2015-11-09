using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace dotSwitcher
{
    public partial class SettingsForm : Window
    {
        private readonly Switcher engine;
        private readonly Settings settings;
        private KeyboardEventArgs currentHotkey;
        private HotKeyType currentHotkeyType;
        /**
         * TRAY ICON
         */
        private TrayIcon icon;
        /**
         * HOTKEY INPUTS
         */
        private KeyboardHook kbdHook;

        public SettingsForm(Settings settings, Switcher engine)
        {
            this.settings = settings;
            this.engine = engine;
            engine.Error += OnEngineError;

            InitializeComponent();
            InitializeTrayIcon();
            InitializeHotkeyBoxes();

            UpdateUi();
        }

        public event EventHandler<EventArgs> Exit;

        private void SettingsWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        /**
         * SETTINGS FORM
         */

        private void ShowForm()
        {
            engine.Stop();
            kbdHook.Start();
            icon.Hide();
            Topmost = true;
            UpdateUi();
            Show();
        }

        private void HideForm()
        {
            kbdHook.Stop();
            engine.Start();
            if (settings.ShowTrayIcon == true)
            {
                icon.Show();
            }
            Topmost = false;
            UpdateUi();
            Hide();
        }

        // Update input values and icon state
        private void UpdateUi()
        {
            textBoxSwitchHotkey.Text = ReplaceCtrls(settings.SwitchHotkey.ToString());
            textBoxConvertHotkey.Text = settings.ConvertSelectionHotkey.ToString();
            textBoxSwitchLayoutHotkey.Text = ReplaceCtrls(settings.SwitchLayoutHotkey.ToString());
            checkBoxAutorun.IsChecked = settings.AutoStart == true;
            checkBoxTrayIcon.IsChecked = settings.ShowTrayIcon == true;
            DisplaySwitchDelay(settings.SwitchDelay);
            icon.SetRunning(engine.IsStarted());
        }

        // also ESC
        private void buttonCancelSettings_Click(object sender, EventArgs e)
        {
            ResetSettings();
            HideForm();
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            if (settings.SwitchHotkey.Alt || settings.SwitchHotkey.Win)
            {
                MessageBox.Show("Sorry, win+ and alt+ hotkeys are not supported yet");
                return;
            }
            SaveSettings();
            HideForm();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            ResetSettings();
            OnExit();
        }

        // initial hidden state
        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            HideForm();
        }

        private void InitializeTrayIcon()
        {
            icon = new TrayIcon(settings.ShowTrayIcon);
            icon.DoubleClick += icon_SettingsClick;
            icon.ExitClick += icon_ExitClick;
            icon.SettingsClick += icon_SettingsClick;
            icon.TogglePowerClick += icon_TogglePowerClick;
        }

        private void icon_TogglePowerClick(object sender, EventArgs e)
        {
            if (engine.IsStarted())
            {
                engine.Stop();
                UpdateUi();
            }
            else
            {
                engine.Start();
                UpdateUi();
            }
        }

        private void icon_SettingsClick(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void icon_ExitClick(object sender, EventArgs e)
        {
            OnExit();
        }

        private void OnEngineError(object sender, SwitcherErrorArgs e)
        {
            icon.ShowTooltip(e.Error.Message, ToolTipIcon.Error);
        }

        private void InitializeHotkeyBoxes()
        {
            textBoxSwitchHotkey.GotFocus += (s, e) => currentHotkeyType = HotKeyType.Switch;
            textBoxSwitchHotkey.LostFocus += (s, e) => ApplyCurrentHotkey();

            textBoxConvertHotkey.GotFocus += (s, e) => currentHotkeyType = HotKeyType.Convert;
            textBoxConvertHotkey.LostFocus += (s, e) => ApplyCurrentHotkey();

            textBoxSwitchLayoutHotkey.GotFocus += (s, e) => currentHotkeyType = HotKeyType.SwitchLayout;
            textBoxSwitchLayoutHotkey.LostFocus += (s, e) => ApplyCurrentHotkey();

            currentHotkeyType = HotKeyType.None;
            kbdHook = new KeyboardHook();
            kbdHook.KeyboardEvent += kbdHook_KeyboardEvent;
        }

        private void kbdHook_KeyboardEvent(object sender, KeyboardEventArgs e)
        {
            if (currentHotkeyType != HotKeyType.None)
            {
                var vk = e.KeyCode;
                if (vk == Keys.Escape || vk == Keys.Back)
                {
                    e.Handled = true;
                    ResetCurrentHotkey(vk == Keys.Back);
                    return;
                }
                if (vk != Keys.LMenu && vk != Keys.RMenu
                    && vk != Keys.LWin && vk != Keys.RWin
                    && vk != Keys.LShiftKey && vk != Keys.RShiftKey
                    && vk != Keys.LControlKey && vk != Keys.RControlKey)
                {
                    e.Handled = true;
                }
                SetCurrentHotkeyInputText(e.ToString());
                currentHotkey = e;
            }
        }

        // TODO: refactor this (make HotkeyInput : TextBox)
        private void SetCurrentHotkeyInputText(string text)
        {
            TextBox currentTextBox;
            switch (currentHotkeyType)
            {
                case HotKeyType.Switch:
                    currentTextBox = textBoxSwitchHotkey;
                    break;
                case HotKeyType.Convert:
                    currentTextBox = textBoxConvertHotkey;
                    break;
                case HotKeyType.SwitchLayout:
                    currentTextBox = textBoxSwitchLayoutHotkey;
                    break;
                default:
                    currentTextBox = null;
                    break;
            }
            if (currentTextBox == null)
            {
                return;
            }
            Dispatcher.Invoke((MethodInvoker) delegate { currentTextBox.Text = ReplaceCtrls(text); });
        }

        private string ReplaceCtrls(string text)
        {
            return text
                .Replace("Control + LControlKey", "Left Ctrl")
                .Replace("Control + RControlKey", "Right Ctrl")
                .Replace("Shift + LShiftKey", "Left Shift")
                .Replace("Shift + RShiftKey", "Right Shift")
                .Replace("Alt + LMenu", "Left Alt")
                .Replace("Alt + RMenu", "Right Alt");
        }

        private void ResetCurrentHotkey(bool clear)
        {
            switch (currentHotkeyType)
            {
                case HotKeyType.Switch:
                    currentHotkey = clear ? null : settings.SwitchHotkey;
                    break;
                case HotKeyType.Convert:
                    currentHotkey = clear ? null : settings.ConvertSelectionHotkey;
                    break;
                case HotKeyType.SwitchLayout:
                    currentHotkey = clear ? null : settings.SwitchLayoutHotkey;
                    break;
                default:
                    currentHotkey = null;
                    break;
            }
            SetCurrentHotkeyInputText(currentHotkey == null ? "None" : ReplaceCtrls(currentHotkey.ToString()));
        }

        private void ApplyCurrentHotkey()
        {
            if (!IsVisible || currentHotkey == null)
            {
                return;
            }
            switch (currentHotkeyType)
            {
                case HotKeyType.Switch:
                    settings.SwitchHotkey = currentHotkey;
                    break;
                case HotKeyType.Convert:
                    settings.ConvertSelectionHotkey = currentHotkey;
                    break;
                case HotKeyType.SwitchLayout:
                    settings.SwitchLayoutHotkey = currentHotkey;
                    break;
                default:
                    break;
            }
            currentHotkeyType = HotKeyType.None;
        }

        /**
         * SETTINGS
         */

        private void SaveSettings()
        {
            settings.Save();

            if (settings.AutoStart == true)
            {
                LowLevelAdapter.CreateAutorunShortcut();
            }
            else
            {
                LowLevelAdapter.DeleteAutorunShortcut();
            }
        }

        private void ResetSettings()
        {
            settings.Reload();
        }

        /**
         * OTHER INPUTS
         */

        private void checkBoxAutorun_CheckedChanged(object sender, EventArgs e)
        {
            settings.AutoStart = checkBoxAutorun.IsChecked;
        }

        private void checkBoxTrayIcon_CheckedChanged(object sender, EventArgs e)
        {
            settings.ShowTrayIcon = checkBoxTrayIcon.IsChecked;
        }

        private void DisplaySwitchDelay(int delay)
        {
            textBoxDelay.Text = delay + " ms";
        }

        private void textBoxDelay_TextChanged(object sender, EventArgs e)
        {
            short delay = 0;
            if (!short.TryParse(Regex.Replace(textBoxDelay.Text, "[^0-9]", ""), out delay) || delay < 1)
            {
                delay = 1;
            }
            settings.SwitchDelay = delay;
            DisplaySwitchDelay(delay);
        }

        /**
         * MISC
         */

        private void OnExit()
        {
            engine.Stop();
            if (Exit != null)
            {
                Exit(this, null);
            }
        }

        private void buttonGithub_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/kurumpa/dotSwitcher/issues");
        }

        // prevent the form from user-initiated closing ([x] click, Alt+F4, closing from taskbar)
        // (acts as Cancel)
        private void SettingsForm_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            HideForm();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private enum HotKeyType
        {
            None,
            Switch,
            Convert,
            SwitchLayout
        }
    }
}