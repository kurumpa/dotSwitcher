using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

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
                this["SwitchHotkey"] = value; 
            }
        }
        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public KeyboardEventArgs AdditionalSwitchHotkey
        {
            get
            {
                return (KeyboardEventArgs)this["AdditionalSwitchHotkey"];
            }
            set
            {
                this["AdditionalSwitchHotkey"] = value; 
            }
        }
        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public Dictionary<uint, Keys> SwitchToParticularLayout
        {
            get
            {
                return (Dictionary<uint, Keys>)this["SwitchToParticularLayout"];
            }
            set
            {
                this["SwitchToParticularLayout"] = value; 
            }
        }


    }
    
}
