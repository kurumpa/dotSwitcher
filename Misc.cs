using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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

    public class HookEventData
    {
        public KeyData KeyData { get; private set; }
        public bool CtrlIsPressed { get; private set; }
        public bool AltIsPressed { get; private set; }
        public bool ShiftIsPressed { get; private set; }

        public HookEventData(KeyData data, bool ctrl, bool alt, bool shift)
        {
            KeyData = data;
            CtrlIsPressed = ctrl;
            AltIsPressed = alt;
            ShiftIsPressed = shift;
        }
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

}
