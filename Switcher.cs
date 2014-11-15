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
        private IntPtr keyboardHook = IntPtr.Zero;
        private IntPtr mouseHook = IntPtr.Zero;
        private List<KeyboardEventArgs> currentWord = new List<KeyboardEventArgs>();

        private KeyboardEventArgs toggleLayoutShortcut = new KeyboardEventArgs(Keys.Pause, false);

        public static bool IsPrintable(KeyboardEventArgs evtData)
        {
            if (evtData.Alt || evtData.Control || evtData.Win) { return false; }
            var keyCode = evtData.KeyCode;
            if (keyCode >= Keys.D0 && keyCode <= Keys.Z) { return true; }
            if (keyCode >= Keys.Oem1 && keyCode <= Keys.OemBackslash) { return true; }
            return false;
        }

        public bool IsStarted()
        {
            return keyboardHook == IntPtr.Zero;
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
            mouseHook = IntPtr.Zero;
            keyboardHook = IntPtr.Zero;
        }

        // should return true if the key is recognized as hotkey and doesn't need further processing
        private bool ProcessKeyPress(KeyboardEventArgs evtData)
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

        private void ProcessMousePress(EventArgs evtData)
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

        private bool OnKeyPress(KeyboardEventArgs evtData)
        {
            var vkCode = evtData.KeyCode;
            var shift = evtData.Shift;
            
            var ctrl = evtData.Control;
            var alt = evtData.Alt;
            var win = evtData.Win;

            var notModified = !ctrl && !alt && !win;
            Debug.WriteLine("is modified = {0}", !notModified);

            if (vkCode == Keys.ControlKey ||
                vkCode == Keys.LControlKey ||
                vkCode == Keys.RControlKey ||
                // yes, don't interrupt the tracking on PrtSc!
                vkCode == Keys.PrintScreen ||
                vkCode == Keys.ShiftKey ||
                vkCode == Keys.RShiftKey ||
                vkCode == Keys.LShiftKey) 
            {
                return false; 
            }
            if (vkCode == Keys.Space && notModified) { AddToCurrentWord(evtData); return false; }
            if (vkCode == Keys.Back && notModified) { RemoveLast(); return false; }
            if (IsPrintable(evtData))
            {
                if (GetPreviousVkCode() == Keys.Space) { BeginNewWord(); }
                AddToCurrentWord(evtData);
                return false;
            }
            // todo make it global hotkey someday
            // warning: ctrl+pause = VK_CANCEL
            if (toggleLayoutShortcut.Equals(evtData))
            {
                if (shift) ConvertSelection();
                else ConvertLast();
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
        private Keys GetPreviousVkCode()
        {
            if (currentWord.Count == 0) { return Keys.None; }
            return currentWord.Last().KeyCode;
        }
        private void BeginNewWord() { currentWord.Clear(); }
        private void AddToCurrentWord(KeyboardEventArgs data) { currentWord.Add(data); }
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
            var backspaces = Enumerable.Repeat<Keys>(Keys.Back, word.Count);

            LowLevelAdapter.SetNextKeyboardLayout();
            foreach (var vkCode in backspaces) { LowLevelAdapter.SendKeyPress(vkCode, false); }
            foreach (var data in word)
            {
                LowLevelAdapter.SendKeyPress(data.KeyCode, data.Shift);
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
