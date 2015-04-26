using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dotSwitcher
{
    static class Utils
    {

        public static Keys[] ParseKeys(KeyboardEventArgs keyArgs)
        {
            List<Keys> keys = new List<Keys>();
            if (keyArgs.Alt)
            {
                keys.Add(Keys.Alt);
            }
            if (keyArgs.Control)
            {
                keys.Add(Keys.Control);
            }
            if (keyArgs.Shift)
            {
                keys.Add(Keys.Shift);
            }
            if (keyArgs.Win)
            {
                keys.Add(Keys.LWin);
            }
            keys.Add(keyArgs.KeyCode);
            return keys.ToArray();
        }


        public static string GetKeyCombinationString(Keys[] keys)
        {
            var strings = keys.Select(KeyName);

            return string.Join(" + ", strings);
        }

        public static string KeyName(Keys key)
        {
            var keyName = Enum.GetName(typeof(Keys), key);
            if (keyName == "ControlKey")
            {
                return "Ctrl";
            }
            return keyName;
        }

    }
}
