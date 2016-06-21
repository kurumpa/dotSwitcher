using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;

namespace dotSwitcher
{
    class Locales
    {
        public static uint GetCurrentLocale() //Returns current keyboard layout
        {
            int lpdwProcessId;
            var hWnd = GetForegroundWindow();
            var procID = GetWindowThreadProcessId(hWnd, out lpdwProcessId);
            var nowLocale = (uint)GetKeyboardLayout(procID) / 0xFFFF;
            return nowLocale;
        }
        public static Locale[] AllList() //Returns list of all locales that persist in system
        {
            int count = 0;
            List<Locale> locs = new List<Locale>();
            foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
            {
                count++;
                locs.Add(new Locale { 
                    Lang = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lang.Culture.NativeName.Split('(')[0].ToLower()),
                    uId = (uint)lang.Culture.KeyboardLayoutId });
            }
            return locs.ToArray();
        }
        private static bool wasShown = false; //Prevents multiple warning messages of below
        public static void IfLessThan2()//Checks if user have enough layouts
        {
            if (AllList().Length < 2 && !wasShown)
            {
                wasShown = true;
                if (MessageBox.Show("This program switches texts by system's layouts(locales/languages), please add at least 2!\nProgram will exit.", "You have too less layouts(locales/languages)!!") == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
        }
        /* Debug
        public static void GetLanguages()
        {
            // Gets the list of installed languages.
            var result = "";
            foreach (System.Windows.Forms.InputLanguage lang in System.Windows.Forms.InputLanguage.InstalledInputLanguages)
            {
                result += (lang.Culture.NativeName).ToString() + "\n";
            }
            System.Windows.Forms.MessageBox.Show(result);
        }
         */
        public struct Locale // Struct with name(variable Lang) of layout and it uID
        {
            public string Lang { get; set; }
            public uint uId { get; set; }
        }
        #region DLL Imports
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetKeyboardLayoutList(int size, [Out, MarshalAs(UnmanagedType.LPArray)] IntPtr[] hkls);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetKeyboardLayout(uint WindowsThreadProcessID);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetLocaleInfo(uint Locale, int LCType, StringBuilder lpLCData, int cchData);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetKeyboardLayout(int WindowsThreadProcessID);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int GetWindowThreadProcessId(IntPtr handleWindow, out int lpdwProcessID);
        #endregion
    }

}
