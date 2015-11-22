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
    public sealed class KeyboardHook : IDisposable
    {
        public event EventHandler<KeyboardEventArgs> KeyboardEvent;
        private IntPtr hookId = IntPtr.Zero;

        public bool IsStarted()
        {
            return hookId != IntPtr.Zero;
        }
        public void Start()
        {
            if (IsStarted()) { return; }
            hookId = LowLevelAdapter.SetHook(LowLevelAdapter.WH_KEYBOARD_LL, KeyboardEventHook);
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


        private IntPtr KeyboardEventHook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool isHandled;
            ProcessKeyboardEvent(nCode, wParam, lParam, out isHandled);
            return isHandled?
                new IntPtr(1) :
                LowLevelAdapter.NextHook(nCode, wParam, lParam);
        }

        // returns true if event is handled
        private void ProcessKeyboardEvent(int nCode, IntPtr wParam, IntPtr lParam, out bool isHandled)
        {
            isHandled = false;
            try
            {

                if (nCode < 0)
                    return;

                bool isKeyDownEvent = false;
                switch (wParam.ToInt32())
                {
                    case LowLevelAdapter.WM_KEYDOWN:
                    case LowLevelAdapter.WM_SYSKEYDOWN:
                        isKeyDownEvent = true;
                        goto case LowLevelAdapter.WM_KEYUP;

                    case LowLevelAdapter.WM_KEYUP:
                    case LowLevelAdapter.WM_SYSKEYUP:

                        var keybdinput = (KEYBDINPUT)Marshal.PtrToStructure(lParam, typeof(KEYBDINPUT));
                        var keyData = (Keys)keybdinput.Vk;

                        keyData |= LowLevelAdapter.KeyPressed(Keys.ControlKey) ? Keys.Control : 0;
                        keyData |= LowLevelAdapter.KeyPressed(Keys.Menu) ? Keys.Alt : 0;
                        keyData |= LowLevelAdapter.KeyPressed(Keys.ShiftKey) ? Keys.Shift : 0;

                        var winPressed = LowLevelAdapter.KeyPressed(Keys.LWin) || LowLevelAdapter.KeyPressed(Keys.RWin);

                        var args = new KeyboardEventArgs(keyData, winPressed, isKeyDownEvent ? KeyboardEventType.KeyDown : KeyboardEventType.KeyUp);
                        OnKeyboardEvent(args);

                        isHandled = args.Handled;
                        break;
                }

            }
            catch { }
        }


        public void Dispose()
        {
            Stop();
        }
    }
}
