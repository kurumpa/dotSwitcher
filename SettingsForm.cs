using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
namespace dotSwitcher
{
    public partial class SettingsForm : Form
    {
        private bool keyIsSet;
        private KeyboardHook kbdHook;
        private TextBox currentTextBox;
        private Settings settings;
        private KeyboardEventArgs currentHotkey;

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

        private void kbdHook_KeyboardEvent(object sender, KeyboardEventArgs e)
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

            var locales = LowLevelAdapter.GetkeyboardLayouts();
            for (int i = 0; i < locales.Length; i++)
            {
                ComboBox comboBoxKeyboarLayouts = new ComboBox();
                ComboBox comboBoxSwitchKey = new ComboBox();
                GenerateCombox(locales[i].Lang, i, comboBoxKeyboarLayouts, comboBoxSwitchKey);
                InicializeSwitchSettingsLine(comboBoxKeyboarLayouts, comboBoxSwitchKey);
            }
        }

        private string GetKeyCombinationString(Keys[] keys)
        {
            var strings = keys.Select(KeyName);
            return string.Join(" + ", strings);
        }

        private string KeyName(Keys key)
        {
            return Enum.GetName(typeof(Keys), key);
        }


        private void InicializeSwitchSettingsLine(ComboBox boxKeyboarLayouts, ComboBox boxHotkeys)
        {
            var locales = LowLevelAdapter.GetkeyboardLayouts();
            var keys = new List<Keys> { Keys.LShiftKey, Keys.RShiftKey, Keys.LControlKey, Keys.RControlKey };

            var usedItems = ComboBoxes.Select(t => t.SelectedItem).ToList();
            foreach (var locale in locales)
            {
                if (!usedItems.Contains(locale))
                    boxKeyboarLayouts.Items.Add(locale.Lang);
            }
            foreach (var key in keys)
            {
                if (!usedItems.Contains(key))
                    boxHotkeys.Items.Add(key);
            }
        }


        private bool GenerateCombox(string locale, int counter, ComboBox comboBoxKeyboarLayouts, ComboBox comboBoxSwitchKey)
        {

            const int SHIFT = 25;

            try
            {
                //
                // comboBoxSwitchKey
                //
                comboBoxSwitchKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                comboBoxSwitchKey.FormattingEnabled = true;
                comboBoxSwitchKey.Location = new System.Drawing.Point(135, 115 + counter * SHIFT);
                comboBoxSwitchKey.Name = "comboBoxSwitchKey" + locale;
                comboBoxSwitchKey.Size = new System.Drawing.Size(121, 21);
                comboBoxSwitchKey.TabIndex = 14;

                //
                // comboBoxKeyboarLayouts
                //
                comboBoxKeyboarLayouts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                comboBoxKeyboarLayouts.FormattingEnabled = true;
                comboBoxKeyboarLayouts.Location = new System.Drawing.Point(8, 115 + counter * SHIFT);
                comboBoxKeyboarLayouts.Name = "comboBoxKeyboarLayouts" + locale;
                comboBoxKeyboarLayouts.Size = new System.Drawing.Size(121, 21);
                comboBoxKeyboarLayouts.TabIndex = 11;

                this.switchKeyboardGroupbox.Controls.Add(comboBoxSwitchKey);
                this.switchKeyboardGroupbox.Controls.Add(comboBoxKeyboarLayouts);
                Size initialSize = this.switchKeyboardGroupbox.Size;
                this.switchKeyboardGroupbox.Size = new System.Drawing.Size(initialSize.Width, initialSize.Height + SHIFT);

                return true;
            }
            catch (Exception e)
            {
                return false;
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
