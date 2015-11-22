using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dotSwitcher.Data;

namespace dotSwitcher.UI
{
    public partial class HotKeyBox : TextBox
    {
        public KeyboardEventArgs HotKey { get; set; }
        public HotKeyBox()
        {
            InitializeComponent();
        }
        public void AcceptKeyboardEvent(object sender, KeyboardEventArgs e)
        {

        }
    }
}
