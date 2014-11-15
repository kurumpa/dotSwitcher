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
        //public override bool Equals(Object obj)
        //{
        //    if (obj == null)
        //    {
        //        return false;
        //    }

        //    var p = obj as KeyboardEventData;
        //    if (p == null)
        //    {
        //        return false;
        //    }

        //    return KeyData.vkCode == p.KeyData.vkCode && //mb compare all KeyData ?
        //           CtrlIsPressed == p.CtrlIsPressed && AltIsPressed == p.AltIsPressed &&
        //           ShiftIsPressed == p.ShiftIsPressed && WinIsPressed == p.WinIsPressed;
        //}

        //public override int GetHashCode()//TODO
        //{
        //    return (int)(base.GetHashCode() ^ KeyData.vkCode);
        //}
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
