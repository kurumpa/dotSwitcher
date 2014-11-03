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
        //http://www.codeproject.com/Articles/442285/Global-Shortcuts-in-WinForms-and-WPF
        
        bool keyIsSet; //Would help us to know if the user has set a shortcut.

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
            e.SuppressKeyPress = true;  //Suppress the key from being processed by the underlying control.
            shortcutTextBox.Text = string.Empty;  //Empty the content of the textbox
            keyIsSet = false; //At this point the user has not specified a shortcut.

            //Set the backspace button to specify that the user does not want to use a shortcut.
            if (e.KeyData == Keys.Back)
            {
                shortcutTextBox.Text = Keys.None.ToString();
                return;
            }

            //Make the user specify a modifier. Control, Alt or Shift.
            //If a modifier is not present then clear the textbox.
            /*if (e.Modifiers == Keys.None)
            {
                MessageBox.Show("You have to specify a modifier like 'Control', 'Alt' or 'Shift'");
                shortcutTextBox.Text = Keys.None.ToString();
                return;
            }*/

            if (e.Modifiers.ToString() != "None")
            {
                //A modifier is present. Process each modifier.
                //Modifiers are separated by a ",". So we'll split them and write each one to the textbox.
                foreach (string modifier in e.Modifiers.ToString().Split(new Char[] {','}))
                {
                    shortcutTextBox.Text += modifier + " + ";
                }
            }

            //KEYCODE contains the last key pressed by the user.
            //If KEYCODE contains a modifier, then the user has not entered a shortcut. hence, KeyisSet is false
            //But if not, KeyisSet is true.
            if (e.KeyCode == Keys.ShiftKey | e.KeyCode == Keys.ControlKey | e.KeyCode == Keys.Menu)
            {
                keyIsSet = false;
            }
            else
            {
                shortcutTextBox.Text += e.KeyCode.ToString();
                keyIsSet = true;
            }

            //TODO save key code and Modifiers

        }

        private void txtButton_KeyUp(object sender, KeyEventArgs e)
        {
            //On KeyUp if KeyisSet is False then clear the textbox.
            if (keyIsSet == false)
            {
                shortcutTextBox.Text = Keys.None.ToString();
            }
        }
    }
}
