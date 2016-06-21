using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace dotSwitcher
{
    class MouseHook
    {
        public static IntPtr _hookID = IntPtr.Zero;

        public static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (!KeyHook.shift)
                {
                    if (MouseMessages.WM_MBUTTONDOWN == (MouseMessages)wParam)
                    {
                        Debug.WriteLine(string.Join("", dSMain.c_word));
                    }
                    if ((MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam) || MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam)
                    {
                        Debug.WriteLine("Click!");
                        dSMain.c_word.Clear();
                    }
                }
                else
                {
                    if (MouseMessages.WM_MBUTTONDOWN == (MouseMessages)wParam)
                    {
                        Debug.WriteLine(string.Join("", dSMain.c_word));
                    }
                    if ((MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam) || MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam)
                    {
                        Debug.WriteLine("Click with shift!!");
                        dSMain.c_word.Clear();
                    }
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        public const int WH_MOUSE_LL = 14;
        public enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208
        }
        #region DLLs
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion
    }

}
