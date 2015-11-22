using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace dotSwitcher.Data
{
    public enum KeyboardEventType { Unknown, KeyDown, KeyUp }
    [Serializable]
    public class KeyboardEventArgs : KeyEventArgs, ISerializable
    {
        public KeyboardEventArgs() : base(Keys.None) { }
        public bool Win { get; set; }

        public KeyboardEventType Type;

        public KeyboardEventArgs(SerializationInfo info, StreamingContext context)
            : base((Keys)info.GetValue("keyData", typeof(Keys)))
        {
            Win = (bool)info.GetValue("winMod", typeof(bool));
            Type = KeyboardEventType.KeyDown;
        }
        public KeyboardEventArgs(Keys keyData, bool isWinDown, KeyboardEventType type = KeyboardEventType.KeyDown)
            : base(keyData)
        {
            Win = isWinDown;
            Type = type;
        }
        public new static readonly KeyboardEventArgs Empty = new KeyboardEventArgs(Keys.None, false, KeyboardEventType.Unknown);

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

        public bool HasModifiers()
        {
            return Modifiers != Keys.None || Win;
        }
        public override string ToString()
        {
            var result =
                (Shift ? Keys.Shift.ToString() + " + " : "") +
                (Control ? Keys.Control.ToString() + " + " : "") +
                (Alt ? Keys.Alt.ToString() + " + " : "") +
                (Win ? "Win + " : "") +
                ((Keys)KeyCode).ToString();
            // TODO: add more readable key names (Oem-stuff and so on)
            return result.Replace("Cancel", "Pause");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("keyData", KeyData, typeof(Keys));
            info.AddValue("winMod", Win, typeof(bool));
        }
    }
}
