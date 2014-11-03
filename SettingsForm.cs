using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            shortcutTextBox.Text = Properties.Settings.Default.toggleLayoutShortcut;
        }

        private void txtButton_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true; 
            shortcutTextBox.Text = string.Empty; 
            keyIsSet = false; 

            if (e.KeyData == Keys.Back)
            {
                shortcutTextBox.Text = Keys.None.ToString();
                return;
            }

            if (e.Alt || e.Control || e.Shift)
            {
                foreach (string modifier in e.Modifiers.ToString().Split(new Char[] {','}))
                {
                    shortcutTextBox.Text += modifier + " + ";
                }
            }

            if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu)
            {
                keyIsSet = false;
            }
            else
            {
                shortcutTextBox.Text += e.KeyCode.ToString();
                keyIsSet = true;
            }

        }

        private void txtButton_KeyUp(object sender, KeyEventArgs e)
        {
            if (keyIsSet == false)
            {
                shortcutTextBox.Text = Keys.None.ToString();
            }
        }
    }
}
