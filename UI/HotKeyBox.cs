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
                SetText(hotKey);
            } 
        }

        private KeyboardEventArgs hotKey;
        public HotKeyBox()
        {
            InitializeComponent();
            
            contextMenu.Items.Add("Reset", null, HandleReset);
            kbdHook.KeyboardEvent += AcceptKeyboardEvent;
        }

        public void AcceptKeyboardEvent(object sender, KeyboardEventArgs e)
        {
            if (e.Type == KeyboardEventType.KeyUp)
            {
                return;
            }

            var vk = e.KeyCode;
            if (vk == Keys.Escape || vk == Keys.Back)
            {
                e.Handled = true;
                //ResetCurrentHotkey(vk == Keys.Back);
                return;
            }
            if (vk != Keys.LMenu && vk != Keys.RMenu
                && vk != Keys.LWin && vk != Keys.RWin
                && vk != Keys.LShiftKey && vk != Keys.RShiftKey
                && vk != Keys.LControlKey && vk != Keys.RControlKey)
            {
                e.Handled = true;
            }

            hotKey = e;
            OnHotKeyChanged();

            Invoke((MethodInvoker)delegate { SetText(e); });
        }

        

        private void SetText(KeyboardEventArgs hotkey)
        {
            base.Text = hotkey.ToString();
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

        [Obsolete("Don't use this", true)]
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
