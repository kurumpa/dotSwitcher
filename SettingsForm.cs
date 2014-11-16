using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace dotSwitcher
{
    public partial class SettingsForm : Form
    {
        bool keyIsSet;
        KeyboardHook kbdHook;
        TextBox currentTextBox;
        Settings settings;
        KeyboardEventArgs currentHotkey;

        public SettingsForm(Settings settings)
        {
            this.settings = settings;
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
                currentHotkey = e;
            }
        }

        private void InitializeValues()
        {
            shortcutTextBox.LostFocus += unsetCurrentInput;
            shortcutTextBox.Leave += unsetCurrentInput;
            shortcutTextBox.GotFocus += setCurrentInput;
            shortcutTextBox.Enter += setCurrentInput;
            shortcutTextBox.Text = settings.SwitchHotkey.ToString();
        }


        private void SettingsForm_Load(object sender, EventArgs e)
        {
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

        private void shortcutTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            settings.SwitchHotkey = currentHotkey;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
