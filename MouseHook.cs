using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
