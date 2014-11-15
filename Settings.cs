using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotSwitcher
{
    public class HotkeyConfigurationSection : System.Configuration.ConfigurationSection
    {
        [ConfigurationProperty("switchLayout")]
        public Keys SwitchLayoutHotkey
        {
            get { return (Keys)this["switchLayout"]; }
            set { this["switchLayout"] = value.ToString(); }
        }
    }
}
