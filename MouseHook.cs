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
using System.Text;

namespace dotSwitcher
{
    public class MouseHook
    {
        public event EventHandler<EventArgs> MouseEvent;
        private IntPtr hookId = IntPtr.Zero;
        private HookProc callback;
        public MouseHook()
        {
            callback = ProcessMouse;
        }


        public bool IsStarted()
        {
            return hookId != IntPtr.Zero;
        }
        public void Start()
        {
            if (IsStarted()) { return; }
            hookId = LowLevelAdapter.SetHook(LowLevelAdapter.WH_MOUSE_LL, callback);
        }
        public void Stop()
        {
            if (!IsStarted()) { return; }
            LowLevelAdapter.ReleaseHook(hookId);
            hookId = IntPtr.Zero;
        }
        private void OnMouseEvent(EventArgs e)
        {
            if (MouseEvent != null)
            {
                MouseEvent(this, e);
            }
        }


        private IntPtr ProcessMouse(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0)
                {
                    switch (wParam.ToInt32())
                    {
                        case LowLevelAdapter.WM_LBUTTONDOWN:
                        case LowLevelAdapter.WM_RBUTTONDOWN:
                            OnMouseEvent(new EventArgs());
                            break;

                    }
                }
            }
            catch { }
            return LowLevelAdapter.NextHook(nCode, wParam, lParam);
        }

    }
}
