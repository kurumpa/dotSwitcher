﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotSwitcher
{
    public partial class SettingsForm : Form
    {
        public event EventHandler<EventArgs> Exit;
        Settings settings;
        Switcher engine;

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

        

        /**
         * SETTINGS FORM
         */
        void ShowForm()
        {
            engine.Stop();
            kbdHook.Start();
            icon.Hide();
            TopMost = true;
            UpdateUi();
            Show();
        }
        void HideForm()
        {
            kbdHook.Stop();
            engine.Start();
            if (settings.ShowTrayIcon == true)
            {
                icon.Show();
            }
            TopMost = false;
            UpdateUi();
            Hide();
        }
        // Update input values and icon state
        void UpdateUi()
        {
            textBoxSwitchHotkey.Text = settings.SwitchHotkey.ToString();
            textBoxConvertHotkey.Text = settings.ConvertSelectionHotkey.ToString();
            checkBoxAutorun.Checked = settings.AutoStart == true;
            checkBoxTrayIcon.Checked = settings.ShowTrayIcon == true;
            DisplaySwitchDelay(settings.SwitchDelay);
            icon.SetRunning(engine.IsStarted());
        }
        // also ESC
        void buttonCancelSettings_Click(object sender, EventArgs e)
        {
            ResetSettings();
            HideForm();
        }
        void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            if (settings.SwitchHotkey.Alt || settings.SwitchHotkey.Win)
            {
                MessageBox.Show("Sorry, win+ and alt+ hotkeys are not supported yet");
                return;
            }
            SaveSettings();
            HideForm();
        }
        void buttonExit_Click(object sender, EventArgs e)
        {
            ResetSettings();
            OnExit();
        }
        // initial hidden state
        void SettingsForm_Shown(object sender, EventArgs e)
        {
            HideForm();
        }
        // prevent the form from user-initiated closing ([x] click, Alt+F4, closing from taskbar)
        // (acts as Cancel)
        void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
            {
                return;
            }

            e.Cancel = true;
            ResetSettings();
            HideForm();
        }     
        // receive window message from another instance
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == LowLevelAdapter.WM_SHOW_SETTINGS)
            {
                ShowForm();
            }
            base.WndProc(ref m);
        }

        /**
         * TRAY ICON
         */
        TrayIcon icon;
        void InitializeTrayIcon()
        {
            icon = new TrayIcon(settings.ShowTrayIcon);
            icon.DoubleClick += icon_SettingsClick;
            icon.ExitClick += icon_ExitClick;
            icon.SettingsClick += icon_SettingsClick;
            icon.TogglePowerClick += icon_TogglePowerClick;
        }
        void icon_TogglePowerClick(object sender, EventArgs e)
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
        void icon_SettingsClick(object sender, EventArgs e)
        {
            ShowForm();
        }
        void icon_ExitClick(object sender, EventArgs e)
        {
            OnExit();
        }
        void OnEngineError(object sender, SwitcherErrorArgs e)
        {
            icon.ShowTooltip(e.Error.Message, ToolTipIcon.Error);
        }
        
        /**
         * HOTKEY INPUTS
         */
        KeyboardHook kbdHook;
        KeyboardEventArgs currentHotkey;
        HotKeyType currentHotkeyType;
        enum HotKeyType { None, Switch, Convert }
        void InitializeHotkeyBoxes()
        {
            textBoxSwitchHotkey.GotFocus += (s, e) => currentHotkeyType = HotKeyType.Switch;
            textBoxSwitchHotkey.Enter += (s, e) => currentHotkeyType = HotKeyType.Switch;
            textBoxSwitchHotkey.LostFocus += (s, e) => ApplyCurrentHotkey();
            textBoxSwitchHotkey.Leave += (s, e) => ApplyCurrentHotkey();
            textBoxConvertHotkey.GotFocus += (s, e) => currentHotkeyType = HotKeyType.Convert;
            textBoxConvertHotkey.Enter += (s, e) => currentHotkeyType = HotKeyType.Convert;
            textBoxConvertHotkey.LostFocus += (s, e) => ApplyCurrentHotkey();
            textBoxConvertHotkey.Leave += (s, e) => ApplyCurrentHotkey();
            currentHotkeyType = HotKeyType.None;
            kbdHook = new KeyboardHook();
            kbdHook.KeyboardEvent += kbdHook_KeyboardEvent;
        }
        void kbdHook_KeyboardEvent(object sender, KeyboardEventArgs e)
        {
            if (currentHotkeyType != HotKeyType.None)
            {
                var vk = e.KeyCode;
                if (vk == Keys.Escape)
                {
                    e.Handled = true;
                    ResetCurrentHotkey();
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
        void SetCurrentHotkeyInputText(string text)
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
                default:
                    currentTextBox = null;
                    break;
            }
            if (currentTextBox == null) { return; }
            Invoke((MethodInvoker)delegate { currentTextBox.Text = text; });
        }

        void ResetCurrentHotkey()
        {
            switch (currentHotkeyType)
            {
                case HotKeyType.Switch:
                    currentHotkey = settings.SwitchHotkey;
                    break;
                case HotKeyType.Convert:
                    currentHotkey = settings.ConvertSelectionHotkey;
                    break;
                default:
                    currentHotkey = null;
                    break;
            }
            SetCurrentHotkeyInputText(currentHotkey == null ? "" : currentHotkey.ToString());
        }

        void ApplyCurrentHotkey()
        {
            if (!Visible || currentHotkey == null)
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
                default:
                    break;
            }
            currentHotkeyType = HotKeyType.None;
        }

        /**
         * SETTINGS
         */
        void SaveSettings()
        {
            settings.Save();

            if (settings.AutoStart == true) { LowLevelAdapter.CreateAutorunShortcut(); }
            else { LowLevelAdapter.DeleteAutorunShortcut(); }
        }
        void ResetSettings()
        {
            settings.Reload();
        }

        /**
         * OTHER INPUTS
         */
        void checkBoxAutorun_CheckedChanged(object sender, EventArgs e)
        {
            settings.AutoStart = checkBoxAutorun.Checked;
        }
        private void checkBoxTrayIcon_CheckedChanged(object sender, EventArgs e)
        {
            settings.ShowTrayIcon = checkBoxTrayIcon.Checked;
        }
        void DisplaySwitchDelay(int delay)
        {
            textBoxDelay.Text = delay.ToString() + " ms";
        }
        private void textBoxDelay_TextChanged(object sender, EventArgs e)
        {
            short delay = 0;
            if (!Int16.TryParse(Regex.Replace(textBoxDelay.Text, "[^0-9]", ""), out delay) || delay < 1)
            {
                delay = 1;
            }
            settings.SwitchDelay = delay;
            DisplaySwitchDelay(delay);
        }

        /**
         * MISC
         */
        void OnExit()
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

    }
}
