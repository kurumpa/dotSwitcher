
using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace dotSwitcher
{
    delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
    public static class LowLevelAdapter
    {
        #region dllimport
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc handler, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetKeyboardLayout(uint WindowsThreadProcessID);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool PostMessage(IntPtr hhwnd, uint msg, uint wparam, uint lparam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern short GetKeyState(int keyCode);
        [DllImport("user32.dll")]
        private static extern IntPtr GetFocus();
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, string lParam);
        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, uint Msg, out int wParam, out int lParam);
        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();
        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 SendInput(UInt32 numberOfInputs, INPUT[] inputs, Int32 sizeOfInputStructure);
        #endregion

        #region consts
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        private const int INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        private const uint WM_INPUTLANGCHANGEREQUEST = 0x50;
        private const uint INPUTLANGCHANGE_FORWARD = 0x02;
        private const uint HKL_NEXT = 1;
        private const uint WM_GETTEXT = 0x0D;
        private const uint WM_GETTEXTLENGTH = 0x0E;
        private const uint EM_GETSEL = 0xB0;
        private const uint EM_SETSEL = 0xB1;
        private const uint EM_REPLACESEL = 0xC2;
        #endregion

        private static HookProc callbackDelegate;

        public static HookId SetKeyboardHook(Action<HookEventData> cb)
        {
            var process = Process.GetCurrentProcess();
            var module = process.MainModule;
            var hModule = GetModuleHandle(module.ModuleName);
            var hookResult = IntPtr.Zero;

            callbackDelegate = (int code, IntPtr wParam, IntPtr lParam) =>
                    ProcessKeyPress(hookResult, code, wParam, lParam, cb);

            hookResult = SetWindowsHookEx(WH_KEYBOARD_LL, callbackDelegate, hModule, 0);
            return new HookId { HookResult = hookResult };
        }

        public static void ReleaseKeyboardHook(HookId id)
        {
            UnhookWindowsHookEx(id.HookResult);
            callbackDelegate = null;
        }


        public static void SetNextKeyboardLayout()
        {
            PostMessage(GetForegroundWindow(), WM_INPUTLANGCHANGEREQUEST, INPUTLANGCHANGE_FORWARD, HKL_NEXT);
        }

        private static IntPtr ProcessKeyPress(IntPtr hookResult, int nCode, IntPtr wParam, IntPtr lParam, Action<HookEventData> cb)
        {
            if (nCode >= 0)
            {
                switch (wParam.ToInt32())
                {
                    case WM_KEYDOWN:
                    case WM_SYSKEYDOWN:
                        var ctrl = (GetKeyState(VirtualKeyStates.VK_CONTROL) & 0x8000) != 0;
                        var alt = (GetKeyState(VirtualKeyStates.VK_MENU) & 0x8000) != 0;
                        var shift = (GetKeyState(VirtualKeyStates.VK_SHIFT) & 0x8000) != 0;

                        var keyData = (KeyData)Marshal.PtrToStructure(lParam, typeof(KeyData));
                        cb(new HookEventData(keyData, ctrl, alt, shift));
                        break;

                }
            }
            return CallNextHookEx(hookResult, nCode, wParam, lParam);
        }

        public static void SendKeyPress(int vkCode)
        {
            var down = new INPUT
            {
                Type = INPUT_KEYBOARD,
                Data = new MOUSEKEYBDHARDWAREINPUT
                {
                    Keyboard = new KEYBDINPUT
                        {
                            Vk = (UInt16)vkCode,
                            Scan = 0,
                            Flags = 0,
                            Time = 0,
                            ExtraInfo = IntPtr.Zero
                        }
                }
            };

            var up = new INPUT
            {
                Type = INPUT_KEYBOARD,
                Data = new MOUSEKEYBDHARDWAREINPUT
                {
                    Keyboard = new KEYBDINPUT
                    {
                        Vk = (UInt16)vkCode,
                        Scan = 0,
                        Flags = KEYEVENTF_KEYUP,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            SendInput(2, new INPUT[2] { down, up }, Marshal.SizeOf(typeof(INPUT)));

        }

    }
}