using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotSwitcher
{
    public class KeyboardEventArgs : KeyEventArgs
    {
        public bool Win { get; private set; }
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

            return p.KeyData == KeyData &&
                p.KeyCode == KeyCode &&
                p.Win == Win;
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

    }
}
