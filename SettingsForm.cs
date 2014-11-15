using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
        bool keyIsSet;
        KeyboardHook kbdHook;
        TextBox currentTextBox;

        public SettingsForm()
        {
            currentTextBox = null;
            kbdHook = new KeyboardHook();
            kbdHook.KeyboardEvent += kbdHook_KeyboardEvent;
            InitializeComponent();
            InitializeValues();
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
                
            }
        }

        private void InitializeValues()
        {
            var config = ConfigurationManager.GetSection("hotkeys") as HotkeyConfigurationSection;

            shortcutTextBox.Text = config.SwitchLayoutHotkey.ToString();
        }


        private void txtButton_KeyDown(object sender, KeyEventArgs e)
        {
            //keyIsSet = e.KeyCode != Keys.ShiftKey  etc
        }

        private void txtButton_KeyUp(object sender, KeyEventArgs e)
        {
            //if (keyIsSet == false)
            //{
            //    shortcutTextBox.Text = Keys.None.ToString();
            //}
        }


        private void SettingsForm_Load(object sender, EventArgs e)
        {
            shortcutTextBox.LostFocus += unsetCurrentInput;
            shortcutTextBox.Leave += unsetCurrentInput;
            shortcutTextBox.GotFocus += setCurrentInput;
            shortcutTextBox.Enter += setCurrentInput;
            kbdHook.Start();
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            kbdHook.Stop();
        }

        private void setCurrentInput(object sender, EventArgs e)
        {
            currentTextBox = (TextBox)sender;
        }
        private void unsetCurrentInput(object sender, EventArgs e)
        {
            currentTextBox = null;
        }
    }
}
