using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace dotSwitcher
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyData
    {
        public UInt32 vkCode;
        public UInt32 scanCode;
        public KeyDataFlags flags;
        public UInt32 time;
        public IntPtr dwExtraInfo;
    }

    [Flags()]
    public enum KeyDataFlags : int
    {
        LLKHF_EXTENDED = 0x01,
        LLKHF_INJECTED = 0x10,
        LLKHF_ALTDOWN = 0x20,
        LLKHF_UP = 0x80,
        KEYEVENTF_KEYUP = 0x02
    }

    public class KeyboardEventArgs : KeyEventArgs
    {
        public bool Win { get; private set; }
        public KeyboardEventArgs(Keys keyData, bool isWinDown) : base(keyData)
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
        //public override string ToString()
        //{
        //    var result =
        //        (ShiftIsPressed ? Keys.Shift.ToString() + " + " : "") +
        //        (CtrlIsPressed ? Keys.Control.ToString() + " + " : "") +
        //        (AltIsPressed ? Keys.Alt.ToString() + " + " : "") +
        //        ((Keys)KeyData.vkCode).ToString();
        //    return result;
        //}
        
    }

   

    public class HookId
    {
        public IntPtr HookResult { get; set; }
        public static HookId Empty = new HookId { HookResult = IntPtr.Zero };
        public bool IsEmpty() { return HookResult == IntPtr.Zero; }
    }

    struct INPUT
    {
        public UInt32 Type;
        public MOUSEKEYBDHARDWAREINPUT Data;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct MOUSEKEYBDHARDWAREINPUT
    {
        [FieldOffset(0)]
        public MOUSEINPUT Mouse;
        [FieldOffset(0)]
        public KEYBDINPUT Keyboard;
        [FieldOffset(0)]
        public HARDWAREINPUT Hardware;
    }

    #pragma warning disable 649
    struct MOUSEINPUT
    {
        public Int32 X;
        public Int32 Y;
        public UInt32 MouseData;
        public UInt32 Flags;
        public UInt32 Time;
        public IntPtr ExtraInfo;
    }

    struct KEYBDINPUT
    {
        public UInt16 Vk;
        public UInt16 Scan;
        public UInt32 Flags;
        public UInt32 Time;
        public IntPtr ExtraInfo;
    }

    struct HARDWAREINPUT
    {
        public UInt32 Msg;
        public UInt16 ParamL;
        public UInt16 ParamH;
    }
    #pragma warning restore 649

}
