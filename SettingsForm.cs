using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
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

        public IEnumerable<ComboBox> ComboBoxes
        {
            get
            {
                foreach (var control in this.Controls)
                {
                    if (control.GetType() == typeof(ComboBox))
                        yield return control as ComboBox;
                }
            }
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


            comboBoxAdditionalSwitch.Items.Add(GetKeyCombinationString(new[] { Keys.CapsLock}));
            comboBoxAdditionalSwitch.Items.Add(GetKeyCombinationString(new[] { Keys.Shift,Keys.Alt}));



            //comboBoxAdditionalSwitch.Items.Add(Keys.);

            if (settings.AdditionalSwitchHotkey != null)
            {
                checkboxAdditionalSwitch.Checked = true;

                //comboBoxAdditionalSwitch.
            }
        }

        private void InicializeSwitchSettingsLine(ComboBox boxKeyboarLayouts, ComboBox boxHotkeys)
        {
            var list = LowLevelAdapter.GetkeyboardLayouts();
            var alreadySetLocale = ComboBoxes.Select(t => t.SelectedItem).ToList();
            foreach (var locale in list)
            {
                if(!alreadySetLocale.Contains(locale))
                    boxKeyboarLayouts.Items.Add(locale.Lang);
            }

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
            if (settings.SwitchHotkey.Alt || settings.SwitchHotkey.Win)
            {
                MessageBox.Show("Sorry, win+ and alt+ hotkeys are not supported yet");
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void checkboxAdditionalSwitch_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxAdditionalSwitch.Enabled = (sender as CheckBox).Checked;
        }
    }
}
