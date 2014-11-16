using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotSwitcher
{
    public class HotkeysConfig : ConfigurationSection
    {
        public HotkeysConfig() { }

        [ConfigurationProperty("switchLayout")]
        public KeyboardEventArgsConfig SwitchLayout
        {
            get
            { return (KeyboardEventArgsConfig)this["switchLayout"]; }
            set
            { this["switchLayout"] = value; }
        }

        [ConfigurationProperty("convertSelection")]
        public KeyboardEventArgsConfig ConvertSelection
        {
            get
            { return (KeyboardEventArgsConfig)this["convertSelection"]; }
            set
            { this["convertSelection"] = value; }
        }
    }

    public class KeyboardEventArgsConfig : ConfigurationElement
    {
        public KeyboardEventArgsConfig() { }

        public KeyboardEventArgs Hotkey
        {
            get
            {
                return new KeyboardEventArgs((Keys)KeyData, WinMod);
            }
            set 
            {
                KeyData = (int)value.KeyData;
                WinMod = value.Win;
            }
        }

        [ConfigurationProperty("keyData", DefaultValue = "0", IsRequired = true)]
        public int KeyData
        {
            get
            { return (int)this["keyData"]; }
            set
            { this["keyData"] = value.ToString(); }
        }

        [ConfigurationProperty("winMod", DefaultValue = "false", IsRequired = true)]
        public bool WinMod
        {
            get
            { return (bool)this["winMod"]; }
            set
            { this["winMod"] = value.ToString(); }
        }
    }
}
