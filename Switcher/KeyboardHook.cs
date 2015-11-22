using dotSwitcher.Data;
using dotSwitcher.WinApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace dotSwitcher.Switcher
{
    public class KeyboardHook
    {
        public event EventHandler<KeyboardEventArgs> KeyboardEvent;
        private HookProc callback;
        private IntPtr hookId = IntPtr.Zero;
        public KeyboardHook()
        {
            callback = ProcessKeyPress;
        }
        public bool IsStarted()
        {
            return hookId != IntPtr.Zero;
        }
        public void Start()
        {
            if (IsStarted()) { return; }
            hookId = LowLevelAdapter.SetHook(LowLevelAdapter.WH_KEYBOARD_LL, callback);
        }
        public void Stop()
        {
            if (!IsStarted()) { return; }
            LowLevelAdapter.ReleaseHook(hookId);
            hookId = IntPtr.Zero;
        }
        private void OnKeyboardEvent(KeyboardEventArgs e)
        {
            if (KeyboardEvent != null)
            {
                KeyboardEvent(this, e);
            }
        }


        private IntPtr ProcessKeyPress(int nCode, IntPtr wParam, IntPtr lParam)
        {
            return ProcessKeyPressInt(nCode, wParam, lParam) ?
                new IntPtr(1) :
                LowLevelAdapter.NextHook(nCode, wParam, lParam);
        }

        // returns true if event is handled
        private bool ProcessKeyPressInt(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {

                if (nCode < 0)
                    return false;

                bool key_down = false;
                switch (wParam.ToInt32())
                {
                    case LowLevelAdapter.WM_KEYDOWN:
                    case LowLevelAdapter.WM_SYSKEYDOWN:
                        key_down = true;
                        goto case LowLevelAdapter.WM_KEYUP;

                    case LowLevelAdapter.WM_KEYUP:
                    case LowLevelAdapter.WM_SYSKEYUP:

                        var keybdinput = (KEYBDINPUT)Marshal.PtrToStructure(lParam, typeof(KEYBDINPUT));
                        var keyData = (Keys)keybdinput.Vk;

                        keyData |= LowLevelAdapter.KeyPressed(Keys.ControlKey) ? Keys.Control : 0;
                        keyData |= LowLevelAdapter.KeyPressed(Keys.Menu) ? Keys.Alt : 0;
                        keyData |= LowLevelAdapter.KeyPressed(Keys.ShiftKey) ? Keys.Shift : 0;

                        var winPressed = LowLevelAdapter.KeyPressed(Keys.LWin) || LowLevelAdapter.KeyPressed(Keys.RWin);

                        var args = new KeyboardEventArgs(keyData, winPressed, key_down);
                        OnKeyboardEvent(args);

                        return args.Handled;
                }

            }
            catch { }
            return false;
        }

    }
}
