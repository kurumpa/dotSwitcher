using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace dotSwitcher
{
    public static partial class LowLevelAdapter
    {

        public static IntPtr SetHook(int type, HookProc callback)
        {
            var process = Process.GetCurrentProcess();
            var module = process.MainModule;
            var handle = GetModuleHandle(module.ModuleName);
            return SetWindowsHookEx(type, callback, handle, 0);
        }
        public static void ReleaseHook(IntPtr id)
        {
            UnhookWindowsHookEx(id);
        }
        public static IntPtr NextHook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        public static bool KeyPressed(Keys keyCode)
        {
            return (GetKeyState((int)keyCode) & 0x8000) == 0x8000;
        }

       
        public static void SetNextKeyboardLayout()
        {
            IntPtr hWnd = GetForegroundWindow();
            uint processId;
            uint activeThreadId = GetWindowThreadProcessId(hWnd, out processId);
            uint currentThreadId = GetCurrentThreadId();

            // todo: refactor this
            AttachThreadInput(activeThreadId, currentThreadId, true);
            IntPtr focusedHandle = GetFocus();
            AttachThreadInput(activeThreadId, currentThreadId, false);

            PostMessage(focusedHandle == IntPtr.Zero ? hWnd : focusedHandle, WM_INPUTLANGCHANGEREQUEST, INPUTLANGCHANGE_FORWARD, HKL_NEXT);
        }

        public static void SendKeyPress(Keys vkCode, bool shift = false)
        {
            var down = MakeKeyInput(vkCode, true);
            var up = MakeKeyInput(vkCode, false);

            if (shift)
            {
                var shiftDown = MakeKeyInput(Keys.ShiftKey, true);
                var shiftUp = MakeKeyInput(Keys.ShiftKey, false);
                SendInput(4, new INPUT[4] { shiftDown, down, up, shiftUp }, Marshal.SizeOf(typeof(INPUT)));
            }
            else
            {
                SendInput(2, new INPUT[2] { down, up }, Marshal.SizeOf(typeof(INPUT)));
            }

        }

        //private static string GetActiveWindowTitle()
        //{
        //    const int nChars = 256;
        //    var buff = new StringBuilder(nChars);
        //    var handle = GetForegroundWindow();

        //    if (GetWindowText(handle, buff, nChars) > 0)
        //    {
        //        return buff.ToString();
        //    }
        //    return null;
        //}

    }
}