using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace dotSwitcher
{
    public class Switcher : IDisposable
    {
        public event EventHandler<SwitcherErrorArgs> Error;
        private HookId keyboardHook = HookId.Empty;
        private HookId mouseHook = HookId.Empty;
        private List<HookEventData> currentWord = new List<HookEventData>();

        public HookEventData ConvertLastShortcut { get; set; }
        public HookEventData ConvertSelectionShortcut { get; set; }

        public Switcher()
        {
            ConvertLastShortcut = HookEventData.FromKeyCode(VirtualKeyStates.VK_PAUSE);
            ConvertSelectionShortcut = HookEventData.FromKeyCode(VirtualKeyStates.VK_PAUSE, false, false, true);
        }

        public bool IsStarted()
        {
            return !keyboardHook.IsEmpty();
        }
        public void Start()
        {
            if (IsStarted()) { return; }
            keyboardHook = LowLevelAdapter.SetKeyboardHook(ProcessKeyPress);
            mouseHook = LowLevelAdapter.SetMouseHook(ProcessMousePress);
        }
        public void Stop()
        {
            if (!IsStarted()) { return; }
            LowLevelAdapter.ReleaseKeyboardHook(keyboardHook);
            LowLevelAdapter.ReleaseKeyboardHook(mouseHook);
            mouseHook = HookId.Empty;
            keyboardHook = HookId.Empty;
        }

        // should return true if the key is recognized as hotkey and doesn't need further processing
        private bool ProcessKeyPress(HookEventData evtData)
        {
            try
            {
                return OnKeyPress(evtData);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
            return false;
        }

        private void ProcessMousePress(HookEventData evtData)
        {
            try
            {
                BeginNewWord();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private bool OnKeyPress(HookEventData evtData)
        {
            var vkCode = evtData.KeyData.vkCode;
            var shift = evtData.ShiftIsPressed;
            
            var ctrl = evtData.CtrlIsPressed;
            var alt = evtData.AltIsPressed;
            var win = evtData.WinIsPressed;

            var notModified = !ctrl && !alt && !win;

            if (vkCode == VirtualKeyStates.VK_CONTROL ||
                vkCode == VirtualKeyStates.VK_LCONTROL ||
                vkCode == VirtualKeyStates.VK_RCONTROL ||
                // yes, don't interrupt the tracking on PrtSc!
                vkCode == VirtualKeyStates.VK_SNAPSHOT ||
                vkCode == VirtualKeyStates.VK_SHIFT ||
                vkCode == VirtualKeyStates.VK_RSHIFT ||
                vkCode == VirtualKeyStates.VK_LSHIFT) 
            {
                return false; 
            }
            if (vkCode == VirtualKeyStates.VK_SPACE && notModified) { AddToCurrentWord(evtData); return false; }
            if (vkCode == VirtualKeyStates.VK_BACK && notModified) { RemoveLast(); return false; }
            if (VirtualKeyStates.IsPrintable(evtData))
            {
                if (GetPreviousVkCode() == VirtualKeyStates.VK_SPACE) { BeginNewWord(); }
                AddToCurrentWord(evtData);
                return false;
            }
            // todo make it global hotkey someday
            // warning: ctrl+pause = VK_CANCEL
            if (ConvertLastShortcut.Equals(evtData))
            {
                ConvertLast();
                return true;
            }

            if (ConvertSelectionShortcut.Equals(evtData))
            {
                ConvertSelection();
                return true;
            }

            // default: 
            BeginNewWord();
            return false;
        }

        private void OnError(Exception ex)
        {
            if (Error != null)
            {
                Error(this, new SwitcherErrorArgs(ex));
            }
        }

        #region word manipulation
        // returns 0 if currentWord is empty
        private uint GetPreviousVkCode()
        {
            if (currentWord.Count == 0) { return 0; }
            return currentWord.Last().KeyData.vkCode;
        }
        private void BeginNewWord() { currentWord.Clear(); }
        private void AddToCurrentWord(HookEventData data) { currentWord.Add(data); }
        private void RemoveLast()
        {
            if (currentWord.Count == 0) { return; }
            currentWord.RemoveAt(currentWord.Count - 1);
        }
        #endregion

        private void ConvertSelection()
        {
            throw new NotImplementedException();
        }
        private void ConvertLast()
        {
            var word = currentWord.ToList();
            var backspaces = Enumerable.Repeat<uint>(VirtualKeyStates.VK_BACK, word.Count);

            LowLevelAdapter.SetNextKeyboardLayout();
            foreach (var vkCode in backspaces) { LowLevelAdapter.SendKeyPress(vkCode, false); }
            foreach (var data in word)
            {
                LowLevelAdapter.SendKeyPress(data.KeyData.vkCode, data.ShiftIsPressed);
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }

    public class SwitcherErrorArgs : EventArgs
    {
        public Exception Error { get; private set; }
        public SwitcherErrorArgs(Exception ex)
        {
            Error = ex;
        }
    }
}
