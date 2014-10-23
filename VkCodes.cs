using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotSwitcher
{
    public static class VirtualKeyStates
    {
        public const int VK_LBUTTON = 0x01;
        public const int VK_RBUTTON = 0x02;
        public const int VK_CANCEL = 0x03;
        public const int VK_MBUTTON = 0x04;
        //
        public const int VK_XBUTTON1 = 0x05;
        public const int VK_XBUTTON2 = 0x06;
        //
        public const int VK_BACK = 0x08;
        public const int VK_TAB = 0x09;
        //
        public const int VK_CLEAR = 0x0C;
        public const int VK_RETURN = 0x0D;
        //
        public const int VK_SHIFT = 0x10;
        public const int VK_CONTROL = 0x11;
        public const int VK_MENU = 0x12;
        public const int VK_PAUSE = 0x13;
        public const int VK_CAPITAL = 0x14;
        //
        public const int VK_KANA = 0x15;
        public const int VK_HANGEUL = 0x15; 
        public const int VK_HANGUL = 0x15;
        public const int VK_JUNJA = 0x17;
        public const int VK_FINAL = 0x18;
        public const int VK_HANJA = 0x19;
        public const int VK_KANJI = 0x19;
        //
        public const int VK_ESCAPE = 0x1B;
        //
        public const int VK_CONVERT = 0x1C;
        public const int VK_NONCONVERT = 0x1D;
        public const int VK_ACCEPT = 0x1E;
        public const int VK_MODECHANGE = 0x1F;
        //
        public const int VK_SPACE = 0x20;
        public const int VK_PRIOR = 0x21;
        public const int VK_NEXT = 0x22;
        public const int VK_END = 0x23;
        public const int VK_HOME = 0x24;
        public const int VK_LEFT = 0x25;
        public const int VK_UP = 0x26;
        public const int VK_RIGHT = 0x27;
        public const int VK_DOWN = 0x28;
        public const int VK_SELECT = 0x29;
        public const int VK_PRINT = 0x2A;
        public const int VK_EXECUTE = 0x2B;
        public const int VK_SNAPSHOT = 0x2C;
        public const int VK_INSERT = 0x2D;
        public const int VK_DELETE = 0x2E;
        public const int VK_HELP = 0x2F;
        //
        public const int VK_LWIN = 0x5B;
        public const int VK_RWIN = 0x5C;
        public const int VK_APPS = 0x5D;
        //
        public const int VK_SLEEP = 0x5F;
        //
        public const int VK_NUMPAD0 = 0x60;
        public const int VK_NUMPAD1 = 0x61;
        public const int VK_NUMPAD2 = 0x62;
        public const int VK_NUMPAD3 = 0x63;
        public const int VK_NUMPAD4 = 0x64;
        public const int VK_NUMPAD5 = 0x65;
        public const int VK_NUMPAD6 = 0x66;
        public const int VK_NUMPAD7 = 0x67;
        public const int VK_NUMPAD8 = 0x68;
        public const int VK_NUMPAD9 = 0x69;
        public const int VK_MULTIPLY = 0x6A;
        public const int VK_ADD = 0x6B;
        public const int VK_SEPARATOR = 0x6C;
        public const int VK_SUBTRACT = 0x6D;
        public const int VK_DECIMAL = 0x6E;
        public const int VK_DIVIDE = 0x6F;
        public const int VK_F1 = 0x70;
        public const int VK_F2 = 0x71;
        public const int VK_F3 = 0x72;
        public const int VK_F4 = 0x73;
        public const int VK_F5 = 0x74;
        public const int VK_F6 = 0x75;
        public const int VK_F7 = 0x76;
        public const int VK_F8 = 0x77;
        public const int VK_F9 = 0x78;
        public const int VK_F10 = 0x79;
        public const int VK_F11 = 0x7A;
        public const int VK_F12 = 0x7B;
        public const int VK_F13 = 0x7C;
        public const int VK_F14 = 0x7D;
        public const int VK_F15 = 0x7E;
        public const int VK_F16 = 0x7F;
        public const int VK_F17 = 0x80;
        public const int VK_F18 = 0x81;
        public const int VK_F19 = 0x82;
        public const int VK_F20 = 0x83;
        public const int VK_F21 = 0x84;
        public const int VK_F22 = 0x85;
        public const int VK_F23 = 0x86;
        public const int VK_F24 = 0x87;
        //
        public const int VK_NUMLOCK = 0x90;
        public const int VK_SCROLL = 0x91;
        //
        public const int VK_OEM_NEC_EQUAL = 0x92;   // '=' key on numpad
        //
        public const int VK_OEM_FJ_JISHO = 0x92;   // 'Dictionary' key
        public const int VK_OEM_FJ_MASSHOU = 0x93;   // 'Unregister word' key
        public const int VK_OEM_FJ_TOUROKU = 0x94;   // 'Register word' key
        public const int VK_OEM_FJ_LOYA = 0x95;   // 'Left OYAYUBI' key
        public const int VK_OEM_FJ_ROYA = 0x96;   // 'Right OYAYUBI' key
        //
        public const int VK_LSHIFT = 0xA0;
        public const int VK_RSHIFT = 0xA1;
        public const int VK_LCONTROL = 0xA2;
        public const int VK_RCONTROL = 0xA3;
        public const int VK_LMENU = 0xA4;
        public const int VK_RMENU = 0xA5;
        //
        public const int VK_BROWSER_BACK = 0xA6;
        public const int VK_BROWSER_FORWARD = 0xA7;
        public const int VK_BROWSER_REFRESH = 0xA8;
        public const int VK_BROWSER_STOP = 0xA9;
        public const int VK_BROWSER_SEARCH = 0xAA;
        public const int VK_BROWSER_FAVORITES = 0xAB;
        public const int VK_BROWSER_HOME = 0xAC;
        //
        public const int VK_VOLUME_MUTE = 0xAD;
        public const int VK_VOLUME_DOWN = 0xAE;
        public const int VK_VOLUME_UP = 0xAF;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;
        public const int VK_MEDIA_PREV_TRACK = 0xB1;
        public const int VK_MEDIA_STOP = 0xB2;
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const int VK_LAUNCH_MAIL = 0xB4;
        public const int VK_LAUNCH_MEDIA_SELECT = 0xB5;
        public const int VK_LAUNCH_APP1 = 0xB6;
        public const int VK_LAUNCH_APP2 = 0xB7;
        //
        public const int VK_OEM_1 = 0xBA;   // ';:' for US
        public const int VK_OEM_PLUS = 0xBB;   // '+' any country
        public const int VK_OEM_COMMA = 0xBC;   // ',' any country
        public const int VK_OEM_MINUS = 0xBD;   // '-' any country
        public const int VK_OEM_PERIOD = 0xBE;   // '.' any country
        public const int VK_OEM_2 = 0xBF;   // '/?' for US
        public const int VK_OEM_3 = 0xC0;   // '`~' for US
        //
        public const int VK_OEM_4 = 0xDB;  //  '[{' for US
        public const int VK_OEM_5 = 0xDC;  //  '\|' for US
        public const int VK_OEM_6 = 0xDD;  //  ']}' for US
        public const int VK_OEM_7 = 0xDE;  //  ''"' for US
        public const int VK_OEM_8 = 0xDF;
        //
        public const int VK_OEM_AX = 0xE1;  //  'AX' key on Japanese AX kbd
        public const int VK_OEM_102 = 0xE2;  //  "<>" or "\|" on RT 102-key kbd.
        public const int VK_ICO_HELP = 0xE3;  //  Help key on ICO
        public const int VK_ICO_00 = 0xE4;  //  00 key on ICO
        //
        public const int VK_PROCESSKEY = 0xE5;
        //
        public const int VK_ICO_CLEAR = 0xE6;
        //
        public const int VK_PACKET = 0xE7;
        //
        public const int VK_OEM_RESET = 0xE9;
        public const int VK_OEM_JUMP = 0xEA;
        public const int VK_OEM_PA1 = 0xEB;
        public const int VK_OEM_PA2 = 0xEC;
        public const int VK_OEM_PA3 = 0xED;
        public const int VK_OEM_WSCTRL = 0xEE;
        public const int VK_OEM_CUSEL = 0xEF;
        public const int VK_OEM_ATTN = 0xF0;
        public const int VK_OEM_FINISH = 0xF1;
        public const int VK_OEM_COPY = 0xF2;
        public const int VK_OEM_AUTO = 0xF3;
        public const int VK_OEM_ENLW = 0xF4;
        public const int VK_OEM_BACKTAB = 0xF5;
        //
        public const int VK_ATTN = 0xF6;
        public const int VK_CRSEL = 0xF7;
        public const int VK_EXSEL = 0xF8;
        public const int VK_EREOF = 0xF9;
        public const int VK_PLAY = 0xFA;
        public const int VK_ZOOM = 0xFB;
        public const int VK_NONAME = 0xFC;
        public const int VK_PA1 = 0xFD;
        public const int VK_OEM_CLEAR = 0xFE;

        public static bool IsPrintable(HookEventData evtData)
        {
            if (evtData.AltIsPressed || evtData.CtrlIsPressed) { return false; }
            var vkCode = evtData.KeyData.vkCode;
            if (vkCode >= 0x30 && vkCode <= 0x5A) { return true; }
            if (vkCode >= VK_OEM_1 && vkCode <= VK_OEM_3) { return true; }
            if (vkCode == VK_OEM_102 || vkCode == VK_ICO_00) { return true; }
            if (vkCode >= VK_OEM_4 && vkCode <= VK_OEM_8) { return true; }
            return false;
        }
    }
}
