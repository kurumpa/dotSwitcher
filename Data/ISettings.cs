namespace dotSwitcher.Data
{
    public interface ISettings
    {
        KeyboardEventArgs SwitchHotkey { get; set; }
        KeyboardEventArgs SwitchLayoutHotkey { get; set; }
        KeyboardEventArgs ConvertSelectionHotkey { get; set; }
        int SwitchDelay { get; set; }
        bool? SmartSelection { get; set; }
    }
}