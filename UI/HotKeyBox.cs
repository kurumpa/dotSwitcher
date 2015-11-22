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
using dotSwitcher.Switcher;
using System.Diagnostics;

namespace dotSwitcher.UI
{
    public partial class HotKeyBox : TextBox, IDisposable
    {
        public event EventHandler Reset;
        public event EventHandler HotKeyChanged;
        KeyboardHook kbdHook = new KeyboardHook();
        public KeyboardEventArgs HotKey {
            get { return hotKey; } 
            set 
            {
                hotKey = value == null ? KeyboardEventArgs.Empty : value;
                base.Text = hotKey.ToString();
            } 
        }

        private KeyboardEventArgs hotKey;
        public HotKeyBox()
        {
            InitializeComponent();
            
            contextMenu.Items.Add("Reset", null, HandleReset);
            kbdHook.KeyboardEvent += AcceptKeyboardEvent;
        }

        private bool winIsPressed;
        public void AcceptKeyboardEvent(object sender, KeyboardEventArgs e)
        {
            var vk = e.KeyCode;

            // we're mostly interested in KeyDown
            if (e.Type == KeyboardEventType.KeyUp)
            {
                // prevent some of the buttons' default actions
                if (vk == Keys.Apps) { e.Handled = true; }
                // "release" the handled Win button
                if (vk == Keys.LWin || vk == Keys.RWin) { winIsPressed = false; }
                return;
            }


            // e.Handled should be true for Win KeyDown to prevent the button's default action
            // winIsPressed is used for save Win button state for Win + SomeButton combinations
            if (vk == Keys.LWin || vk == Keys.RWin) { winIsPressed = true; }

            // disable Ctrl + LControlKey and so on
            if (((vk == Keys.LControlKey || vk == Keys.RControlKey) && e.Control) ||
                 ((vk == Keys.LMenu || vk == Keys.RMenu) && e.Alt) ||
                 ((vk == Keys.LShiftKey || vk == Keys.RShiftKey) && e.Shift) ||
                 ((vk == Keys.LWin || vk == Keys.RWin) && e.Win))
            {
                e.Handled = true;
                return;
            }

            // clear the hotkey
            if (vk == Keys.Back && !e.HasModifiers())
            {
                e = KeyboardEventArgs.Empty;
            }

            // reset the hotkey
            if (vk == Keys.Escape && !e.HasModifiers())
            {
                e.Handled = true;
                OnReset();
                return;
            }

            // prevent any default action possible
            if (vk != Keys.LMenu && vk != Keys.RMenu
                && vk != Keys.LShiftKey && vk != Keys.RShiftKey
                && vk != Keys.LControlKey && vk != Keys.RControlKey)
            {
                e.Handled = true;
            }

            // add Win modifier if the button is not Win itself
            if (vk != Keys.LWin && vk != Keys.RWin) { e.Win = winIsPressed; }
            hotKey = e;
            OnHotKeyChanged();

            Invoke((MethodInvoker)delegate { base.Text = e.ToString(); });
        }
        
        private void HandleReset(object sender, EventArgs e)
        {
            OnReset();
        }
        protected override void OnGotFocus(EventArgs e)
        {
            kbdHook.Start();
            base.OnGotFocus(e);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            kbdHook.Stop();
            base.OnLostFocus(e);
        }
        [Obsolete("Use HotKey property instead", true)]
        public new string Text { get; set; }
        protected override void OnKeyDown(KeyEventArgs e) { }
        protected override void OnKeyUp(KeyEventArgs e) { }
        protected override void OnKeyPress(KeyPressEventArgs e) { }
        private void OnReset()
        {
            if (Reset != null)
            {
                Reset(this, EventArgs.Empty);
            }
        }
        private void OnHotKeyChanged()
        {
            if (HotKeyChanged != null)
            {
                HotKeyChanged(this, EventArgs.Empty);
            }
        }

    }
}
