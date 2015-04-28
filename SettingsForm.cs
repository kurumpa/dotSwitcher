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

            List<string> keys = new List<string>
            {
                Utils.GetKeyCombinationString(new[] {Keys.CapsLock}),
                Utils.GetKeyCombinationString(new[] {Keys.Shift, Keys.Alt}),
                Utils.GetKeyCombinationString(new[] {Keys.ControlKey, Keys.Space}),
                Utils.GetKeyCombinationString(new[] {Keys.Alt, Keys.Space}),
                Utils.GetKeyCombinationString(new[] {Keys.ControlKey})
            };

            keys.ForEach(s => comboBoxAdditionalSwitch.Items.Add(s));
            //settings.AdditionalSwitchHotkey = 
            //comboBoxAdditionalSwitch.SelectedIndex = keys.IndexOf(GetKeyCombinationString())

            //comboBoxAdditionalSwitch.Items.Add(Keys.);

            if (settings.AdditionalSwitchHotkey != null)
            {
                checkboxAdditionalSwitch.Checked = true;
                var additionalKeys = Utils.ParseKeys(settings.AdditionalSwitchHotkey);
                string keyCombinationString = Utils.GetKeyCombinationString(additionalKeys);
                comboBoxAdditionalSwitch.SelectedIndex = comboBoxAdditionalSwitch.Items.IndexOf(keyCombinationString);
            }

            var locales = LowLevelAdapter.GetkeyboardLayouts();
            for (int i = 0; i < locales.Length; i++)
            {
                Keys userSettings = Keys.None;
                if (settings.SwitchToParticularLayout != null)
                {
                    var result = settings.SwitchToParticularLayout.TryGetValue(locales[i].LocaleId, out userSettings);
                }

                Label comboBoxKeyboarLayouts = new Label();
                ComboBox comboBoxSwitchKey = new ComboBox();
                GenerateCombox(locales[i].Lang, i, comboBoxKeyboarLayouts, comboBoxSwitchKey);
                InicializeSwitchSettingsLine(comboBoxKeyboarLayouts, comboBoxSwitchKey,locales[i].Lang, userSettings);
            }
        }

        private void InicializeSwitchSettingsLine(Label textBoxLayout, ComboBox boxHotkeys, string currentLocale, Keys userSettings)
        {
            var keys = new List<Keys> { Keys.LShiftKey, Keys.RShiftKey, Keys.LControlKey, Keys.RControlKey };

            textBoxLayout.Text = currentLocale;

            var usedItems = ComboBoxes.Select(t => t.SelectedItem).ToList();
            foreach (var key in keys)
            {
                if (!usedItems.Contains(key))
                    boxHotkeys.Items.Add(key);
            }
            boxHotkeys.SelectedIndex = boxHotkeys.Items.IndexOf(userSettings);
        }

        private bool GenerateCombox(string locale, int counter, Label comboBoxKeyboarLayouts, ComboBox comboBoxSwitchKey)
        {

            const int SHIFT = 25;

            try
            {
                //
                // comboBoxKeyboarLayouts
                //
                comboBoxKeyboarLayouts.Location = new Point(8, 115 + counter * SHIFT);
                comboBoxKeyboarLayouts.Name = "comboBoxKeyboarLayouts" + locale;
                comboBoxKeyboarLayouts.Size = new Size(100, 21);
                comboBoxKeyboarLayouts.TabIndex = 11;
                comboBoxKeyboarLayouts.TextAlign = ContentAlignment.MiddleRight;


                //
                // comboBoxSwitchKey
                //
                comboBoxSwitchKey.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBoxSwitchKey.FormattingEnabled = true;
                comboBoxSwitchKey.Location = new Point(121, 115 + counter * SHIFT);
                comboBoxSwitchKey.Name = "comboBoxSwitchKey" + locale;
                comboBoxSwitchKey.Size = new Size(140, 21);
                comboBoxSwitchKey.TabIndex = 14;

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

        private void comboBoxAdditionalSwitch_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedValue = (sender as ComboBox).Text;
            var items = selectedValue.Replace(" ", "").Replace("Ctrl", "Control").Split('+');
            Keys key = items.Aggregate(Keys.None, (current, item) => current | (Keys)Enum.Parse(typeof(Keys), item));

            settings.AdditionalSwitchHotkey = new KeyboardEventArgs(key, false);
        }
        
    }
}
