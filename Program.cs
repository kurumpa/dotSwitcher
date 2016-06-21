using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace dotSwitcher
{
    class dSMain
    {
        #region DLLs
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool PostMessage(IntPtr hhwnd, uint msg, uint wparam, uint lparam);
        [DllImport("user32.dll")]
        public static extern uint RegisterWindowMessage(string message);
        #endregion
        #region Prevent another instance variables
        private const string appGUid = "91dd1b9c-3678-482a-b467-46276be3323e";
        public static uint ao = RegisterWindowMessage("AlderyOpeneddS!");
        #endregion
        #region All Main variables, arrays etc.
        public static List<KeyHook.YuKey> c_word = new List<KeyHook.YuKey>();
        public static IntPtr _hookID = IntPtr.Zero;
        public static IntPtr _mouse_hookID = IntPtr.Zero;
        public static KeyHook.LowLevelProc _proc = KeyHook.HookCallback;
        public static MouseHook.LowLevelMouseProc _mouse_proc = MouseHook.HookCallback;
        public static Locales.Locale[] locales = Locales.AllList();
        public static Settings Conf = new Settings();
        public static MainForm mahou = new MainForm(); // do not rename it, i like it ;)
        #endregion
        [STAThread]
        public static void Main()
        {
            using (var mutex = new Mutex(false, "Global\\" + appGUid))
            {
                //This is for debugging, if you want use it first uncoment it in Locales class
                //  Locales.GetLanguages();
                if (!mutex.WaitOne(0, false))
                {
                    PostMessage((IntPtr)0xffff, ao, 0, 0);
                    return;
                }
            if (locales.Length < 2)
            {
            Locales.IfLessThan2();
            }
            else
            {
                _mouse_hookID = MouseHook.SetHook(_mouse_proc);
                _hookID = KeyHook.SetHook(_proc);
                //for first run, add your locale 1 & locale 2 to settings
                if (Conf.locale1Lang == "" && Conf.locale2Lang == "")
                {
                    Conf.locale1uId = locales[0].uId;
                    Conf.locale2uId = locales[1].uId;
                    Conf.locale1Lang = locales[0].Lang;
                    Conf.locale2Lang = locales[1].Lang;
                }
                Conf.Save();
                Application.Run();
                KeyHook.UnhookWindowsHookEx(_hookID);
                MouseHook.UnhookWindowsHookEx(_mouse_hookID);
            }
        }
    }

    }
}