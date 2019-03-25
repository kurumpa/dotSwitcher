/**
 *   DotSwitcher: a simple keyboard layout switcher
 *   Copyright (C) 2014-2019 Kirill Mokhovtsev / kurumpa
 *   Contact: kiev.programmer@gmail.com
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */
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