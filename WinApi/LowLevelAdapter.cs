using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.ComponentModel;
using IWshRuntimeLibrary;
using System.Reflection;
using System.Windows.Automation;
using System.Collections.Generic;

namespace dotSwitcher.WinApi
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

        private static IntPtr GetFocusedHandle()
        {
            var threadId = GetCurrentThreadId();
            var wndThreadId = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);

            if (threadId == wndThreadId)
            {
                return IntPtr.Zero;
            }

            AttachThreadInput(wndThreadId, threadId, true);
            IntPtr focusedHandle = GetFocus();
            AttachThreadInput(wndThreadId, threadId, false);
            return focusedHandle;
        }

        public static IntPtr GetCurrentLayout()
        {
            var wndThreadId = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
            return GetKeyboardLayout(wndThreadId);
        }


        public static void SetNextKeyboardLayout()
        {
            IntPtr hWnd = IntPtr.Zero;
            var threadId = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
            var info = new GUITHREADINFO();
            info.cbSize = Marshal.SizeOf(info);
            var success = GetGUIThreadInfo(threadId, ref info);

            // target = hwndCaret || hwndFocus || (AttachThreadInput + GetFocus) || hwndActive || GetForegroundWindow
            var focusedHandle = GetFocusedHandle();
            if (success)
            {
                if (info.hwndCaret != IntPtr.Zero)
                {
                    hWnd = info.hwndCaret;
                }
                else if (info.hwndFocus != IntPtr.Zero)
                {
                    hWnd = info.hwndFocus;
                }
                else if (focusedHandle != IntPtr.Zero)
                {
                    hWnd = focusedHandle;
                }
                else if (info.hwndActive != IntPtr.Zero)
                {
                    hWnd = info.hwndActive;
                }
            }
            else
            {
                hWnd = focusedHandle;
            }
            if(hWnd == IntPtr.Zero) { hWnd = GetForegroundWindow();  }

            //PostMessage(hWnd, WM_INPUTLANGCHANGEREQUEST, INPUTLANGCHANGE_FORWARD, HKL_NEXT);

            var shiftDown = MakeKeyInput(Keys.LShiftKey, true);
            var shiftUp = MakeKeyInput(Keys.LShiftKey, false);
            var altDown = MakeKeyInput(Keys.LMenu, true);
            var altUp = MakeKeyInput(Keys.LMenu, false);

            SendInput(2, new INPUT[2] { altDown, shiftDown }, Marshal.SizeOf(typeof(INPUT)));
            SendInput(2, new INPUT[2] { altUp, shiftUp }, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendCopy()
        {
            var ctrlDown = MakeKeyInput(Keys.LControlKey, true);
            var ctrlUp = MakeKeyInput(Keys.LControlKey, false);
            var cDown = MakeKeyInput(Keys.C, true);
            var cUp = MakeKeyInput(Keys.C, false);
            SendInput(2, new INPUT[2] { ctrlDown, cDown }, Marshal.SizeOf(typeof(INPUT)));
            Thread.Sleep(20);
            SendInput(2, new INPUT[2] { ctrlUp, cUp }, Marshal.SizeOf(typeof(INPUT)));
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

        public static void ReleasePressedFnKeys()
        {
            // temp solution
            //ReleasePressedKey(Keys.LMenu, true),
            //ReleasePressedKey(Keys.RMenu, true),
            //ReleasePressedKey(Keys.LWin, true),
            //ReleasePressedKey(Keys.RWin, true),
            ReleasePressedKey(Keys.RControlKey, false);
            ReleasePressedKey(Keys.LControlKey, false);
            ReleasePressedKey(Keys.LShiftKey, false);
            ReleasePressedKey(Keys.RShiftKey, false);
        }

        public static bool ReleasePressedKey(Keys keyCode, bool releaseTwice)
        {
            if (!KeyPressed(keyCode)) { return false; }
            //Debug.WriteLine("{0} was down", keyCode);
            var keyUp = MakeKeyInput(keyCode, false);
            if (releaseTwice)
            {
                var secondDown = MakeKeyInput(keyCode, true);
                var secondUp = MakeKeyInput(keyCode, false);
                SendInput(3, new INPUT[3] { keyUp, secondDown, secondUp }, Marshal.SizeOf(typeof(INPUT)));
            }
            else
            {
                SendInput(1, new INPUT[1] { keyUp }, Marshal.SizeOf(typeof(INPUT)));
            }
            return true;
        }

        public static void SendShowSettingsMessage()
        {
            PostMessage((IntPtr)HWND_BROADCAST, WM_SHOW_SETTINGS, 0, 0);
        }


        private static string GetAutorunPath()
        {
            return System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                "dotSwitcher.lnk");
        }
        public static void CreateAutorunShortcut()
        {
            var currentPath = Assembly.GetExecutingAssembly().Location;
            var shortcutLocation = GetAutorunPath();
            var description = "Simple keyboard layout switcher";
            if (System.IO.File.Exists(shortcutLocation))
            {
                return;
            }
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);
            shortcut.Description = description;
            shortcut.TargetPath = currentPath;
            shortcut.Save();
        }
        public static void DeleteAutorunShortcut()
        {
            System.IO.File.Delete(GetAutorunPath());
        }

        static Dictionary<string, object> lBackup = new Dictionary<string, object>();
        static IDataObject lDataObject = null;
        static string[] lFormats = new string[] {};

        public static Keys ToKey(char ch)
        {
            var layout = GetCurrentLayout();

            short keyNumber = VkKeyScanEx(ch, layout);
            if (keyNumber == -1)
            {
                return System.Windows.Forms.Keys.None;
            }
            return (System.Windows.Forms.Keys)(((keyNumber & 0xFF00) << 8) | (keyNumber & 0xFF));
        }
        public static void BackupClipboard()
        {
            //lDataObject = Clipboard.GetDataObject();
            //if (lDataObject == null) 
            //{
            //    return;
            //}
            //lFormats = lDataObject.GetFormats(false);
            //lBackup = new Dictionary<string, object>();
            //foreach(var lFormat in lFormats)
            //{
            //  lBackup.Add(lFormat, lDataObject.GetData(lFormat, false));
            //}
            //Debug.WriteLine(lDataObject);
            //Debug.WriteLine(lFormats);
        }

        public static void RestoreClipboard()
        {
            //Debug.WriteLine(lDataObject);
            //Debug.WriteLine(lFormats);
            //if (lDataObject == null)
            //{
            //    return;
            //}
            //foreach (var lFormat in lFormats)
            //{
            //    lDataObject.SetData(lBackup[lFormat]);
            //}
            //Clipboard.SetDataObject(lDataObject);
        }

    }
}