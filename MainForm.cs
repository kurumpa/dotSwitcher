using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using IWshRuntimeLibrary;

namespace dotSwitcher
{
    public partial class MainForm : Form
    {
        #region DLL imports
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int extraInfo);
        [DllImport("user32.dll")]
        static extern short MapVirtualKey(int wCode, int wMapType);
        #endregion
        HotkeyHandler Mainhk, ExitHk, HKConvertLast, HKConvertSelection; // Hotkeys
        static bool HKCLReg = false, HKCSReg = false; // These to prevent re-registering of same HotKey
        bool shift = false, alt = false, ctrl = false;
        static string tempCLMods, tempCSMods; // Temporary modifiers
        static int tempCLKey, tempCSKey; // Temporary keys
        TrayIcon icon;
        List<string> lcnmid = new List<string>(); // List of locales that avaible
        public MainForm()
        {
            InitializeComponent();
            foreach (Locales.Locale lc in dSMain.locales)
            {
                lcnmid.Add(lc.Lang + "(" + lc.uId + ")");
            }
            icon = new TrayIcon(dSMain.Conf.IconVisibility);
            icon.Exit += exitToolStripMenuItem_Click;
            icon.ShowHide += showHideToolStripMenuItem_Click;
            Mainhk = new HotkeyHandler(Modifiers.ALT + Modifiers.CTRL + Modifiers.SHIFT, Keys.Insert, this); // This is special HotKey
            Mainhk.Register();
            ExitHk = new HotkeyHandler(Modifiers.ALT + Modifiers.CTRL + Modifiers.SHIFT, Keys.F12, this);// This is too special
            ExitHk.Register();CheckModifiers(dSMain.Conf.HKCLMods);
            HKConvertLast = new HotkeyHandler((alt ? Modifiers.ALT : 0x0000) + (ctrl ? Modifiers.CTRL : 0x0000) + (shift ? Modifiers.SHIFT : 0x0000), (Keys)dSMain.Conf.HKCLKey, this);
            HKConvertLast.Register();
            HKCLReg = true;
            CheckModifiers(dSMain.Conf.HKCSMods);
            HKConvertSelection = new HotkeyHandler((alt ? Modifiers.ALT : 0x0000) + (ctrl ? Modifiers.CTRL : 0x0000) + (shift ? Modifiers.SHIFT : 0x0000), (Keys)dSMain.Conf.HKCSKey, this);
            HKConvertSelection.Register();
            HKCSReg = true;
        }
        public void CheckModifiers(string inpt)
        {
            if (String.IsNullOrEmpty(inpt)) { inpt = "None"; } // inpt can be empty because of replaces so we switch it to "None" to avoid null reference exception.
            shift = inpt.Contains("Shift") ? true : false;
            alt = inpt.Contains("Alt") ? true : false;
            ctrl = inpt.Contains("Control") ? true : false;
        }
        public static string Remake(Keys k) //Make readable some special keys
        {
            if (k >= Keys.D0 && k <= Keys.D9)
            {
                return k.ToString().Replace("D", "");
            }
            if (k == Keys.ShiftKey ||
                k == Keys.Menu ||
                k == Keys.ControlKey ||
                k == Keys.LWin ||
                k == Keys.RWin)
            {
                return "";
            }
            if (k == Keys.Scroll)
            {
                return k.ToString().Replace("Cancel", "Scroll");
            }
            if (k == Keys.Cancel)
            {
                return k.ToString().Replace("Cancel", "Pause");
            }
            return k.ToString();
        }
        public static string OemReadable(string inpt)//Make readable Oem Keys
        {
            return inpt.Replace("Oemtilde", "`")
                  .Replace("OemMinus", "-")
                  .Replace("Oemplus", "+")
                  .Replace("OemBackslash", "\\")
                  .Replace("Oem5", "\\")
                  .Replace("OemOpenBrackets", "{")
                  .Replace("OemCloseBrackets", "}")
                  .Replace("Oem6", "}")
                  .Replace("OemSemicolon", ";")
                  .Replace("Oem1", ";")
                  .Replace("OemQuotes", "\"")
                  .Replace("Oem7", "\"")
                  .Replace("OemPeriod", ".")
                  .Replace("Oemcomma", ",")
                  .Replace("OemQuestion", "/");
        } 
        #region Form/Controls Events
        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshLocales();
            tbCLHK.Text = OemReadable((dSMain.Conf.HKCLMods.Replace(",", " +") + " + " + (Keys)dSMain.Conf.HKCLKey).Replace("None + ", ""));
            tbCSHK.Text = OemReadable((dSMain.Conf.HKCSMods.Replace(",", " +") + " + " + (Keys)dSMain.Conf.HKCSKey).Replace("None + ", ""));
            cbLangOne.SelectedIndex = lcnmid.IndexOf(dSMain.Conf.locale1Lang + "(" + dSMain.Conf.locale1uId + ")");
            cbLangTwo.SelectedIndex = lcnmid.IndexOf(dSMain.Conf.locale2Lang + "(" + dSMain.Conf.locale2uId + ")");
            Debug.WriteLine(lcnmid.IndexOf(dSMain.Conf.locale2Lang + "(" + dSMain.Conf.locale2uId + ")") + dSMain.Conf.locale2Lang + "(" + dSMain.Conf.locale2uId + ")");
            TrayIconCheckBox.Checked = dSMain.Conf.IconVisibility;
            cbAutorun.Checked =
            System.IO.File.Exists(System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Startup),
            "Mahou.lnk")) ? true : false;
            cbSpaceBreak.Checked = dSMain.Conf.SpaceBreak;
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Visible = false;
            }
        }
        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {
            RefreshLocales();
        }
        private void MainForm_Activated(object sender, EventArgs e)
        {
            RefreshLocales();
        }
        private void cbAutorun_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutorun.Checked)
            {
                CreateShortcut();
            }
            else
            {
                DeleteShortcut();
            }
        }
        private void cbLangOne_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Locales.Locale lc in dSMain.locales)
            {
                if (cbLangOne.Text == lc.Lang + "(" + lc.uId + ")")
                {
                    dSMain.Conf.locale1Lang = lc.Lang;
                    dSMain.Conf.locale1uId = lc.uId;
                }
            }
        }
        private void tbCLHK_KeyDown(object sender, KeyEventArgs e)// Catch hotkey for Convert Last action
        {
            tbCLHK.Text = OemReadable((e.Modifiers.ToString().Replace(",", " +") + " + " + Remake(e.KeyCode)).Replace("None + ", ""));
            tempCLMods = e.Modifiers.ToString().Replace(",", " +").Replace("None", "");
            tempCLKey = (int)e.KeyCode;
        }
        private void tbCSHK_KeyDown(object sender, KeyEventArgs e)// Catch hotkey for Convert Selection action
        {
            tbCSHK.Text = OemReadable((e.Modifiers.ToString().Replace(",", " +") + " + " + Remake(e.KeyCode)).Replace("None + ", ""));
            tempCSMods = e.Modifiers.ToString().Replace(",", " +").Replace("None", "");
            tempCSKey = (int)e.KeyCode;
        }
        private void cbLangTwo_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Locales.Locale lc in dSMain.locales)
            {
                if (cbLangTwo.Text == lc.Lang + "(" + lc.uId + ")")
                {
                    dSMain.Conf.locale2Lang = lc.Lang;
                    dSMain.Conf.locale2uId = lc.uId;
                }
            }
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tempCLMods))
            {
                dSMain.Conf.HKCLMods = tempCLMods;
            }
            if (tempCLKey != 0)
            {
                dSMain.Conf.HKCLKey = tempCLKey;
            }
            if (!string.IsNullOrEmpty(tempCSMods))
            {
                dSMain.Conf.HKCSMods = tempCSMods;
            }
            if (tempCSKey != 0)
            {
                dSMain.Conf.HKCSKey = tempCSKey;
            }
            dSMain.Conf.Save();
            if (HKCLReg)
            {
                HKConvertLast.Unregister();
            }
            CheckModifiers(dSMain.Conf.HKCLMods);
            HKConvertLast = new HotkeyHandler((alt ? Modifiers.ALT : 0x0000) + (ctrl ? Modifiers.CTRL : 0x0000) + (shift ? Modifiers.SHIFT : 0x0000), (Keys)dSMain.Conf.HKCLKey, this);
            HKConvertLast.Register();
            HKCLReg = true;
            if (HKCSReg)
            {
                HKConvertSelection.Unregister();
            }
            CheckModifiers(dSMain.Conf.HKCSMods);
            HKConvertLast = new HotkeyHandler((alt ? Modifiers.ALT : 0x0000) + (ctrl ? Modifiers.CTRL : 0x0000) + (shift ? Modifiers.SHIFT : 0x0000), (Keys)dSMain.Conf.HKCSKey, this);
            HKConvertLast.Register();
            HKCLReg = true;
            RefreshIconVisibility();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tempCLMods))
            {
                dSMain.Conf.HKCLMods = tempCLMods;
            }
            if (tempCLKey != 0)
            {
                dSMain.Conf.HKCLKey = tempCLKey;
            }
            if (!string.IsNullOrEmpty(tempCSMods))
            {
                dSMain.Conf.HKCSMods = tempCSMods;
            }
            if (tempCSKey != 0)
            {
                dSMain.Conf.HKCSKey = tempCSKey;
            }
            dSMain.Conf.Save();
            if (HKCLReg)
            {
                HKConvertLast.Unregister();
            }
            CheckModifiers(dSMain.Conf.HKCLMods);
            HKConvertLast = new HotkeyHandler((alt ? Modifiers.ALT : 0x0000) + (ctrl ? Modifiers.CTRL : 0x0000) + (shift ? Modifiers.SHIFT : 0x0000), (Keys)dSMain.Conf.HKCLKey, this);
            HKConvertLast.Register();
            HKCLReg = true;
            if (HKCSReg)
            {
                HKConvertSelection.Unregister();
            }
            CheckModifiers(dSMain.Conf.HKCSMods);
            HKConvertLast = new HotkeyHandler((alt ? Modifiers.ALT : 0x0000) + (ctrl ? Modifiers.CTRL : 0x0000) + (shift ? Modifiers.SHIFT : 0x0000), (Keys)dSMain.Conf.HKCSKey, this);
            HKConvertLast.Register();
            HKCLReg = true;
            RefreshIconVisibility();
            this.Visible = false;
        }
        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Press Ctrl+Alt+Shift+Insert to toggle dotSwitcher main window visibility.\nPress Ctrl+Alt+Shift+F12 to shutdown dotSwitcher.\n\n*Note that if you typing in not of selected in settings layouts(locales/languages), pressing \"Pause\"(by default) will switch typed text to Language 1.\n\n**If you have problems with symbols conversion(selection) try switching languages (1=>2 & 2=>1)\nRegards.", "****Attention****", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mhouIcon_DoubleClick(object sender, EventArgs e)
        {
            dotSwitcher.dSMain.mahou.Show();
        }

        private void showHideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleVisibility();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitProgram();
        }

        private void TrayIconCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            dSMain.Conf.IconVisibility = TrayIconCheckBox.Checked;
        }

        private void cbSpaceBreak_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSpaceBreak.Checked)
            {
                dSMain.Conf.SpaceBreak = true;
            }
            else
            {
                dSMain.Conf.SpaceBreak = false;
            }
        }
        #endregion
        #region Functions & WndProc
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Modifiers.WM_HOTKEY_MSG_ID)
            {
                CheckModifiers(dSMain.Conf.HKCLMods);
                if ((Keys)(((int)m.LParam >> 16) & 0xFFFF) == (Keys)dSMain.Conf.HKCLKey && ((int)m.LParam & 0xFFFF) == (alt ? Modifiers.ALT : 0x0000) + (ctrl ? Modifiers.CTRL : 0x0000) + (shift ? Modifiers.SHIFT : 0x0000))
                {
                    //These three below are needed to release all modifiers, so even if you will still hold any of it
                   //it will skip them and do as it must.
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); // Alt Up
                    keybd_event((int)Keys.ShiftKey, (byte)MapVirtualKey((int)Keys.ShiftKey, 0), 2, 0); // Shift Up
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); // Control Up
                    KeyHook.ConvertLast();
                }
                CheckModifiers(dSMain.Conf.HKCSMods);
                if ((Keys)(((int)m.LParam >> 16) & 0xFFFF) == (Keys)dSMain.Conf.HKCSKey && ((int)m.LParam & 0xFFFF) == (alt ? Modifiers.ALT : 0x0000) + (ctrl ? Modifiers.CTRL : 0x0000) + (shift ? Modifiers.SHIFT : 0x0000))
                {
                    //same as above comment
                    keybd_event((int)Keys.Menu, (byte)MapVirtualKey((int)Keys.Menu, 0), 2, 0); // Alt Up
                    keybd_event((int)Keys.ShiftKey, (byte)MapVirtualKey((int)Keys.ShiftKey, 0), 2, 0); // Shift Up
                    keybd_event((int)Keys.ControlKey, (byte)MapVirtualKey((int)Keys.ControlKey, 0), 2, 0); // Control Up
                    KeyHook.ConvertSelection();
                }
                if ((Keys)(((int)m.LParam >> 16) & 0xFFFF) == Keys.Insert && ((int)m.LParam & 0xFFFF) == Modifiers.ALT + Modifiers.CTRL + Modifiers.SHIFT)
                {
                    ToggleVisibility();
                }
                if ((Keys)(((int)m.LParam >> 16) & 0xFFFF) == Keys.F12 && ((int)m.LParam & 0xFFFF) == Modifiers.ALT + Modifiers.CTRL + Modifiers.SHIFT)
                {
                    ExitProgram();
                }
            }
            if (m.Msg == dotSwitcher.dSMain.ao)
            {
                ToggleVisibility();
            }
            base.WndProc(ref m);
        }

        public void ToggleVisibility()
        {
            if (this.Visible != false)
            {
                this.Visible = false;
            }
            else
            {
                this.TopMost = true;
                this.Visible = true;
                System.Threading.Thread.Sleep(5);
                this.TopMost = false;
            }
            Refresh();
        }

        public void ExitProgram()
        {
            dSMain.Conf.Save();
            icon.Hide();
            Refresh();
            Application.Exit();
        }

        private void RefreshLocales() // This is used often to check if user will have at least 2 locales,else program will exit...
        {                                                                                                                     // |
            dSMain.locales = Locales.AllList();                                                                               // |
            cbLangOne.Items.Clear();                                                                                          // | 
            cbLangTwo.Items.Clear();                                                                                          // |
            Locales.IfLessThan2();                                                                                            //<-
            foreach (Locales.Locale lc in dSMain.locales)
            {
                cbLangOne.Items.Add(lc.Lang + "(" + lc.uId + ")");
                cbLangTwo.Items.Add(lc.Lang + "(" + lc.uId + ")");
            }
        }
        private void RefreshIconVisibility()
        {
            if (dSMain.Conf.IconVisibility)
            {
                icon.Show();
                Refresh();
            }
            else
            {
                icon.Hide();
                Refresh();
            }
        }
        public static void CreateShortcut() // Creates startup shortcut for program
        {
            var currentPath = Assembly.GetExecutingAssembly().Location;
            var shortcutLocation = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                "dotSwitcher.lnk");
            var description = "dotSwitcher - Simple layout switcher";
            if (System.IO.File.Exists(shortcutLocation))
            {
                return;
            }
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);
            shortcut.Description = description;
            shortcut.TargetPath = currentPath;
            shortcut.Save();
        }
        public static void DeleteShortcut() // Deletes startup shortcut for program
        {
            if (System.IO.File.Exists(System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Startup),
            "dotSwitcher.lnk")))
            {
                System.IO.File.Delete(System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                    "dotSwitcher.lnk"));
            }
        }
        #endregion

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/kurumpa/");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/BladeMight/");
        }
    }
}
