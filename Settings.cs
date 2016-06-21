using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotSwitcher
{
    class Settings : ApplicationSettingsBase
    {
        [SettingsProvider(typeof(PortableSettingsProvider))]
        [UserScopedSetting()]
        [DefaultSettingValue("true")]
        public bool IconVisibility
        {
            get { return (bool)this["IconVisibility"]; }
            set { this["IconVisibility"] = (bool)value; }
        }
        [SettingsProvider(typeof(PortableSettingsProvider))]
        [UserScopedSetting()]
        [DefaultSettingValue("true")]
        public bool SpaceBreak
        {
            get { return (bool)this["SpaceBreak"]; }
            set { this["SpaceBreak"] = (bool)value; }
        }
        [SettingsProvider(typeof(PortableSettingsProvider))]
        [UserScopedSetting()]
        public uint locale1uId
        {
            get { return (uint)this["locale1uId"]; }
            set { this["locale1uId"] = (uint)value; }
        }
        [SettingsProvider(typeof(PortableSettingsProvider))]
        [UserScopedSetting()]
        public uint locale2uId 
        {
            get { return (uint)this["locale2uId"]; }
            set { this["locale2uId"] = (uint)value; }
        }
        [SettingsProvider(typeof(PortableSettingsProvider))]
        [UserScopedSetting()]
        public string locale1Lang //Locale 1 name
        {
            get { return (string)this["locale1Lang"]; }
            set { this["locale1Lang"] = (string)value; }
        }
        [SettingsProvider(typeof(PortableSettingsProvider))]
        [UserScopedSetting()]
        public string locale2Lang //Locale 2 name
        {
            get { return (string)this["locale2Lang"]; }
            set { this["locale2Lang"] = (string)value; }
        }
        [SettingsProvider(typeof(PortableSettingsProvider))]
        [UserScopedSetting()]
        [DefaultSettingValue("None")]
        public string HKCLMods // Hot Key Convert Last Modifiers
        {
            get { return (string)this["HKCLMods"]; }
            set { this["HKCLMods"] = (string)value; }
        }
        [SettingsProvider(typeof(PortableSettingsProvider))]
        [UserScopedSetting()]
        [DefaultSettingValue("19")] //Pause Key
        public int HKCLKey // Hot Key Convert Last Key
        {
            get { return (int)this["HKCLKey"]; }
            set { this["HKCLKey"] = (int)value; }
        }
        [SettingsProvider(typeof(PortableSettingsProvider))]
        [UserScopedSetting()]
        [DefaultSettingValue("None")]
        public string HKCSMods // Hot Key Convert Selection Modifiers
        {
            get { return (string)this["HKCSMods"]; }
            set { this["HKCSMods"] = (string)value; }
        }
        [SettingsProvider(typeof(PortableSettingsProvider))]
        [UserScopedSetting()]
        [DefaultSettingValue("145")] // Scroll Key
        public int HKCSKey // Hot Key Convert Selection Key
        {
            get { return (int)this["HKCSKey"]; }
            set { this["HKCSKey"] = (int)value; }
        }
    }
}
