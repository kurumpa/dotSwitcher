using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;

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
        public KeyData KeyData { get; set; }
        public bool CtrlIsPressed { get; set; }
        public bool AltIsPressed { get; set; }
        public bool ShiftIsPressed { get; set; }
        public bool WinIsPressed { get; set; }

        public HookEventData()
        {
        }

        public HookEventData(KeyData data, bool ctrl = false, bool alt = false, bool shift = false, bool win = false)
        {
            KeyData = data;
            CtrlIsPressed = ctrl;
            AltIsPressed = alt;
            ShiftIsPressed = shift;
            WinIsPressed = win;
        }

        public static HookEventData FromKeyCode(UInt32 vkCode, bool ctrl = false, bool alt = false, bool shift = false, bool win = false)
        {
            var data = new KeyData {vkCode = vkCode};
            return new HookEventData(data, ctrl, alt, shift, win);
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as HookEventData;
            if (p == null)
            {
                return false;
            }

            return KeyData.vkCode == p.KeyData.vkCode && //mb compare all KeyData ?
                   CtrlIsPressed == p.CtrlIsPressed && AltIsPressed == p.AltIsPressed &&
                   ShiftIsPressed == p.ShiftIsPressed && WinIsPressed == p.WinIsPressed;
        }

        public override int GetHashCode()//TODO
        {
            return (int) (base.GetHashCode() ^ KeyData.vkCode);
        }

        public static String Serialize(HookEventData data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static HookEventData Deserialize(String data)
        {
            return JsonConvert.DeserializeObject<HookEventData>(data);
        }
    }

    public class DummyHookEventData : HookEventData
    {
        public new KeyData KeyData { get { WrongUsage(); return new KeyData(); } private set { } }
        public new bool CtrlIsPressed { get { WrongUsage(); return false; } private set { } }
        public new bool AltIsPressed { get { WrongUsage(); return false; } private set { } }
        public new bool ShiftIsPressed { get { WrongUsage(); return false; } private set { } }
        public new bool WinIsPressed { get { WrongUsage(); return false; } private set { } }
        public DummyHookEventData() : base(new KeyData()) { }
        public void WrongUsage()
        {
            throw new NotImplementedException("This is a DummyHookEventData");
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
