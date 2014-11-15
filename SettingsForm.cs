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

        public SettingsForm()
        {
            InitializeComponent();
            InitializeValues();
        }

        private void InitializeValues()
        {
            var config = ConfigurationManager.GetSection("hotkeys") as HotkeyConfigurationSection;

            shortcutTextBox.Text = config.SwitchLayoutHotkey.ToString();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Debug.WriteLine("cmd");
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void txtButton_KeyDown(object sender, KeyEventArgs e)
        {
            ////Debug.WriteLine("code: {0}, data: {1}, modifiers: {2}", e.KeyCode, e.KeyData, e.Modifiers);
            //e.SuppressKeyPress = true;
            ////todo not working?
            //e.Handled = true;

            //shortcutTextBox.Text = string.Empty; 
            //keyIsSet = false; 

            //if (e.KeyData == Keys.Back)
            //{
            //    shortcutTextBox.Text = Keys.None.ToString();
            //    return;
            //}
            //if (e.Alt || e.Control || e.Shift)
            //{
            //    foreach (string modifier in e.Modifiers.ToString().Split(new Char[] { ',' }))
            //    {
            //        shortcutTextBox.Text += modifier + " + ";
            //    }
            //}

            //var data = new KeyboardEventData(e);
            //shortcutTextBox.Text = data.ToString();

            //keyIsSet = e.KeyCode != Keys.ShiftKey 
            //    && e.KeyCode != Keys.ControlKey 
            //    && e.KeyCode == Keys.Menu;
        }

        private void txtButton_KeyUp(object sender, KeyEventArgs e)
        {
            if (keyIsSet == false)
            {
                shortcutTextBox.Text = Keys.None.ToString();
            }
        }

        private void shortcutTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }
    }
}
