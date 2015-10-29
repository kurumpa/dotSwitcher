using System;
using System.Configuration;
using System.Windows.Forms;

namespace dotSwitcher
{
    [Serializable]
    public sealed class Settings : ApplicationSettingsBase, ISettings
    {
        public static Settings Init()
        {
            var settings = new Settings();
            settings.Reload();
            if (settings.SwitchHotkey.KeyData == Keys.None)
            {
                settings.SwitchHotkey = new KeyboardEventArgs(Keys.Pause, false);
            }
            if (settings.ConvertSelectionHotkey.KeyData == Keys.None)
            {
                settings.ConvertSelectionHotkey = new KeyboardEventArgs(Keys.Pause | Keys.Shift, false);
            }
            if (settings.ShowTrayIcon == null)
            {
                settings.ShowTrayIcon = true;
            }
            if (settings.SwitchDelay < 1)
            {
                settings.SwitchDelay = 20;
            }
            settings.Save();
            return settings;
        }

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

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public KeyboardEventArgs ConvertSelectionHotkey
        {
            get
            {
                return (KeyboardEventArgs)this["ConvertSelectionHotkey"];
            }
            set
            {
                this["ConvertSelectionHotkey"] = (KeyboardEventArgs)value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public KeyboardEventArgs SwitchLayoutHotkey
        {
            get
            {
                return (KeyboardEventArgs)this["SwitchLayoutHotkey"];
            }
            set
            {
                this["SwitchLayoutHotkey"] = (KeyboardEventArgs)value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public bool? AutoStart
        {
            get
            {
                return (bool?)this["AutoStart"];
            }
            set
            {
                this["AutoStart"] = (bool?)value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public bool? ShowTrayIcon
        {
            get
            {
                return (bool?)this["ShowTrayIcon"];
            }
            set
            {
                this["ShowTrayIcon"] = (bool?)value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public int SwitchDelay
        {
            get
            {
                return (int)this["SwitchDelay"];
            }
            set
            {
                this["SwitchDelay"] = (int)value;
            }
        }
    }
    
}
