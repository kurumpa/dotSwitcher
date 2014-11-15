
using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace dotSwitcher
{
    delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
    public static partial class LowLevelAdapter
    {
        private static HookProc kbdCallbackDelegate;
        private static HookProc mouseCallbackDelegate;

        public static IntPtr SetMouseHook(Action<EventArgs> cb)
        {
            var process = Process.GetCurrentProcess();
            var module = process.MainModule;
            var hModule = GetModuleHandle(module.ModuleName);
            var hookResult = IntPtr.Zero;
            mouseCallbackDelegate = (int code, IntPtr wParam, IntPtr lParam) =>
                    ProcessMouse(hookResult, code, wParam, lParam, cb);

            hookResult = SetWindowsHookEx(WH_MOUSE_LL, mouseCallbackDelegate, hModule, 0);
            return hookResult;
        }
        public static IntPtr SetKeyboardHook(Func<KeyboardEventArgs, bool> cb)
        {
            var process = Process.GetCurrentProcess();
            var module = process.MainModule;
            var hModule = GetModuleHandle(module.ModuleName);
            var hookResult = IntPtr.Zero;

            kbdCallbackDelegate = (int code, IntPtr wParam, IntPtr lParam) =>
                    ProcessKeyPress(hookResult, code, wParam, lParam, cb);

            hookResult = SetWindowsHookEx(WH_KEYBOARD_LL, kbdCallbackDelegate, hModule, 0);
            return hookResult;
        }

        public static void ReleaseKeyboardHook(IntPtr id)
        {
            UnhookWindowsHookEx(id);
            kbdCallbackDelegate = null;
        }

        public static void ReleaseMouseHook(IntPtr id)
        {
            UnhookWindowsHookEx(id);
            mouseCallbackDelegate = null;
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

        private static IntPtr ProcessKeyPress(IntPtr hookResult, int nCode, IntPtr wParam, IntPtr lParam, Func<KeyboardEventArgs, bool> cb)
        {
            return ProcessKeyPressInt(nCode, wParam, lParam, cb) ?
                new IntPtr(1) :
                CallNextHookEx(hookResult, nCode, wParam, lParam);
        }

        private static bool KeyPressed(Keys keyCode)
        {
            return GetKeyState((int)keyCode & 0x8000) == 0;
        }

        private static bool ProcessKeyPressInt(int nCode, IntPtr wParam, IntPtr lParam, Func<KeyboardEventArgs, bool> cb)
        {
            try
            {

                if (nCode < 0)
                    return false;

                if ("dotSwitcher Settings".Equals(GetActiveWindowTitle())) //Ignore settings windows TODO use const
                    return false;

                switch (wParam.ToInt32())
                {
                    case WM_KEYDOWN:
                    case WM_SYSKEYDOWN:

                        var keybdinput = (KEYBDINPUT)Marshal.PtrToStructure(lParam, typeof(KEYBDINPUT));
                        var keyData = (Keys)keybdinput.Vk;
 
                        keyData |= KeyPressed(Keys.ControlKey) ? Keys.Control : 0;
                        keyData |= KeyPressed(Keys.Menu) ? Keys.Alt : 0;
                        keyData |= KeyPressed(Keys.ShiftKey) ? Keys.Shift : 0;

                        var winPressed = KeyPressed(Keys.LWin) || KeyPressed(Keys.RWin);

                        var withdrawMessage = cb(new KeyboardEventArgs(keyData, winPressed));

                        return withdrawMessage;
                }

            }
            catch { }
            return false;
        }

        private static IntPtr ProcessMouse(IntPtr hookResult, int nCode, IntPtr wParam, IntPtr lParam, Action<EventArgs> cb)
        {
            try
            {
                if (nCode >= 0)
                {
                    switch (wParam.ToInt32())
                    {
                        case WM_LBUTTONDOWN:
                        case WM_RBUTTONDOWN:
                            cb(new EventArgs());
                            break;

                    }
                }
            }
            catch { }
            return CallNextHookEx(hookResult, nCode, wParam, lParam);
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


        private static INPUT MakeKeyInput(Keys vkCode, bool down)
        {
            return new INPUT
            {
                Type = INPUT_KEYBOARD,
                Data = new MOUSEKEYBDHARDWAREINPUT
                {
                    Keyboard = new KEYBDINPUT
                        {
                            Vk = (UInt16)vkCode,
                            Scan = 0,
                            Flags = down ? 0 : KEYEVENTF_KEYUP,
                            Time = 0,
                            ExtraInfo = IntPtr.Zero
                        }
                }
            };
        }

        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            var buff = new StringBuilder(nChars);
            var handle = GetForegroundWindow();

            if (GetWindowText(handle, buff, nChars) > 0)
            {
                return buff.ToString();
            }
            return null;
        }

    }
}