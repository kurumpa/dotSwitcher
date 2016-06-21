using System;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
namespace dotSwitcher
{
    public class HotkeyHandler
    {
        #region DLL Imports
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion
        private int modifier;
        private int key;
        private IntPtr hWnd;
        private int id;
        public HotkeyHandler(int modifier, Keys key, Form form)
        {
            this.modifier = modifier;
            this.key = (int)key;
            this.hWnd = form.Handle;
            id = this.GetHashCode();
        }
        public bool Register() //Registers HotKey
        {
            return RegisterHotKey(hWnd, id, modifier, key);
        }
        public bool Unregister()//Unregisters HotKey
        {
            return UnregisterHotKey(hWnd, id);
        }
        public override int GetHashCode() //Creates hash-code for HotKey
        {
            return modifier ^ key ^ hWnd.ToInt32();
        }

    }
}
