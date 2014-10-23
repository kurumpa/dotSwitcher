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

        private void ProcessKeyPress(HookEventData evtData)
        {
            try
            {
                OnKeyPress(evtData);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
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

        private void OnKeyPress(HookEventData evtData)
        {
            var vkCode = evtData.KeyData.vkCode;
            var ctrl = evtData.CtrlIsPressed;
            var alt = evtData.AltIsPressed;
            var notModified = !ctrl && !alt;
            var shift = evtData.ShiftIsPressed;

            if (vkCode == VirtualKeyStates.VK_CONTROL ||
                vkCode == VirtualKeyStates.VK_LCONTROL ||
                vkCode == VirtualKeyStates.VK_RCONTROL ||
                vkCode == VirtualKeyStates.VK_SNAPSHOT ||
                vkCode == VirtualKeyStates.VK_SHIFT ||
                vkCode == VirtualKeyStates.VK_RSHIFT ||
                vkCode == VirtualKeyStates.VK_LSHIFT) 
            {
                return; 
            }
            if (vkCode == VirtualKeyStates.VK_SPACE && notModified) { AddToCurrentWord(evtData); return; }
            if (vkCode == VirtualKeyStates.VK_BACK && notModified) { RemoveLast(); return; }
            if (VirtualKeyStates.IsPrintable(evtData))
            {
                if (GetPreviousVkCode() == VirtualKeyStates.VK_SPACE) { BeginNewWord(); }
                AddToCurrentWord(evtData);
                return;
            }
            // todo make it global hotkey someday
            if (vkCode == VirtualKeyStates.VK_PAUSE)
            {
                if (shift) { ConvertSelection(); }
                else { ConvertLast(); }
                return;
            }
            // default: 
            BeginNewWord();

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
        private List<uint> GetCurrentWord()
        {
            return currentWord.ConvertAll<uint>(item => item.KeyData.vkCode);
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
            var word = GetCurrentWord();
            var backspaces = Enumerable.Repeat<uint>(VirtualKeyStates.VK_BACK, word.Count);

            LowLevelAdapter.SetNextKeyboardLayout();
            foreach (var vkCode in backspaces.Concat(word))
            {
                LowLevelAdapter.SendKeyPress(vkCode);
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
