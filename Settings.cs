using System;
using System.Configuration;

namespace dotSwitcher
{
    [Serializable]
    public sealed class Settings : ApplicationSettingsBase
    {
        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public KeyboardEventArgs SwitchHotkey
        {
            get
            {
                return (KeyboardEventArgs)this["SwitchHotkey"];
            }
            set
            {
                this["SwitchHotkey"] = (KeyboardEventArgs)value; 
            }
        }
    }
    
}
