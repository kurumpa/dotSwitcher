/**
 *   DotSwitcher: a simple keyboard layout switcher
 *   Copyright (C) 2014-2019 Kirill Mokhovtsev / kurumpa
 *   Contact: kiev.programmer@gmail.com
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace dotSwitcher
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

                switch (wParam.ToInt32())
                {
                    case LowLevelAdapter.WM_KEYDOWN:
                    case LowLevelAdapter.WM_SYSKEYDOWN:

                        var keybdinput = (KEYBDINPUT)Marshal.PtrToStructure(lParam, typeof(KEYBDINPUT));
                        var keyData = (Keys)keybdinput.Vk;

                        keyData |= LowLevelAdapter.KeyPressed(Keys.ControlKey) ? Keys.Control : 0;
                        keyData |= LowLevelAdapter.KeyPressed(Keys.Menu) ? Keys.Alt : 0;
                        keyData |= LowLevelAdapter.KeyPressed(Keys.ShiftKey) ? Keys.Shift : 0;

                        var winPressed = LowLevelAdapter.KeyPressed(Keys.LWin) || LowLevelAdapter.KeyPressed(Keys.RWin);

                        var args = new KeyboardEventArgs(keyData, winPressed);
                        OnKeyboardEvent(args);

                        return args.Handled;
                }

            }
            catch { }
            return false;
        }

    }
}
