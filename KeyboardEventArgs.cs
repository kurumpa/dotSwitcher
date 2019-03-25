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
 
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace dotSwitcher
{
    [Serializable]
    public class KeyboardEventArgs : KeyEventArgs, ISerializable
    {
        public KeyboardEventArgs() : base(Keys.None) { }
        public bool Win { get; private set; }
        public KeyboardEventArgs(SerializationInfo info, StreamingContext context)
            : base((Keys)info.GetValue("keyData", typeof(Keys)))
        {
            Win = (bool)info.GetValue("winMod", typeof(bool));
        }
        public KeyboardEventArgs(Keys keyData, bool isWinDown)
            : base(keyData)
        {
            Win = isWinDown;
        }
        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as KeyboardEventArgs;
            if (p == null)
            {
                return false;
            }
            var equals = p.KeyData == KeyData &&
                p.KeyCode == KeyCode &&
                p.Win == Win;

            return equals;
        }

        public override int GetHashCode()
        {
            return (int)(base.GetHashCode() ^  Win.GetHashCode());
        }
        public override string ToString()
        {
            var result =
                (Shift ? Keys.Shift.ToString() + " + " : "") +
                (Control ? Keys.Control.ToString() + " + " : "") +
                (Alt ? Keys.Alt.ToString() + " + " : "") +
                (Win ? "Win + " : "") +
                ((Keys)KeyCode).ToString();
            return result;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("keyData", KeyData, typeof(Keys));
            info.AddValue("winMod", Win, typeof(bool));
        }
    }
}
