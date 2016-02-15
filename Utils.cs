using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
            var strings = keys.Where(t => t != Keys.None).Select(KeyName).OrderBy(t =>
                                {
                                    switch (t)
                                    {
                                        case "Alt":
                                            return 1;
                                        case "Ctrl":
                                            return 2;
                                        case "Shift":
                                            return 3;
                                        default:
                                            return 4;
                                    }
                                }
                            ).ThenBy(t => t);

            return string.Join(" + ", strings);
        }

        public static string KeyName(Keys key)
        {
            var keyName = Enum.GetName(typeof(Keys), key);
            if (keyName == "Control")
            {
                return "Ctrl";
            }
            return keyName;
        }

        internal static string GetKeyCombinationString(Keys key)
        {
            return GetKeyCombinationString(new[] { key });
        }

        internal static Keys GetKeyByName(string keyName)
        {
            Keys keyValue;
            if (!Enum.TryParse(keyName, true, out keyValue)) {
                return Keys.None;
            }

            return keyValue;
        }
    }
}
