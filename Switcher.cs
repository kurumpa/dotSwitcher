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
        private List<KeyboardEventArgs> currentWord = new List<KeyboardEventArgs>();
        private KeyboardHook kbdHook;
        private MouseHook mouseHook;

        public KeyboardEventArgs SwitchHotkey { get; set; }

        public KeyboardEventArgs SwitchKeyboardLayout { get; set; }
        public KeyboardEventArgs ConvertSelectionHotkey { get; set; }
        public KeyboardEventArgs AdditionalSwitch { get; set; }
        public Dictionary<KeyboardEventArgs,uint> SwitchToLocale { get; set; }

        public Switcher()
        {
            kbdHook = new KeyboardHook();
            kbdHook.KeyboardEvent += ProcessKeyPress;
            mouseHook = new MouseHook();
            mouseHook.MouseEvent += ProcessMousePress;
            SwitchToLocale = new Dictionary<KeyboardEventArgs, uint>();
        }


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
            return kbdHook.IsStarted() || mouseHook.IsStarted();
        }
        public void Start()
        {
            kbdHook.Start();
            mouseHook.Start();
        }
        public void Stop()
        {
            kbdHook.Stop();
            mouseHook.Stop();
        }

        // evtData.Handled must be set to true if the key is recognized as hotkey and doesn't need further processing
        private void ProcessKeyPress(object sender, KeyboardEventArgs evtData)
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

        private void ProcessMousePress(object sender, EventArgs evtData)
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

        private bool HaveModifiers(KeyboardEventArgs evtData)
        {
            var ctrl = evtData.Control;
            var alt = evtData.Alt;
            var win = evtData.Win;

            return ctrl || alt || win;
        }

      private bool HaveTrackingKeys(KeyboardEventArgs evtData)
      {
          var vkCode = evtData.KeyCode;

          return evtData.KeyCode == Keys.ControlKey ||
            vkCode == Keys.LControlKey ||
            vkCode == Keys.RControlKey ||
            // yes, don't interrupt the tracking on PrtSc!
            vkCode == Keys.PrintScreen ||
            vkCode == Keys.ShiftKey ||
            vkCode == Keys.RShiftKey ||
            vkCode == Keys.LShiftKey ||
            vkCode == Keys.NumLock || 
            vkCode == Keys.Scroll;
      }

        private void OnKeyPress(KeyboardEventArgs evtData)
        {
            var vkCode = evtData.KeyCode;

            if (evtData.Equals(SwitchHotkey))
            {
                ConvertLast();
                evtData.Handled = true;
                return;
            }

            if (evtData.Equals(AdditionalSwitch))
            {
                LowLevelAdapter.SetNextKeyboardLayout();
                evtData.Handled = true;
                return;
            }

            if (SwitchToLocale.Keys.Any(t=>t.Equals(evtData)))
            {
                uint locale = SwitchToLocale.First(t=>Equals(t.Key, evtData)).Value;
                LowLevelAdapter.SetLayout(locale);
            }

            if (evtData.Equals(ConvertSelectionHotkey))
            {
                ConvertSelection();
            }

            if (this.HaveTrackingKeys(evtData))
              return;

            var notModified = !this.HaveModifiers(evtData);

            if (vkCode == Keys.Space && notModified) { AddToCurrentWord(evtData); return; }
            if (vkCode == Keys.Back && notModified) { RemoveLast(); return; }
            if (IsPrintable(evtData))
            {
                if (GetPreviousVkCode() == Keys.Space) { BeginNewWord(); }
                AddToCurrentWord(evtData);
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
            LowLevelAdapter.ReleasePressedFnKeys();
            var word = currentWord.ToList();
            var backspaces = Enumerable.Repeat<Keys>(Keys.Back, word.Count);

            foreach (var vkCode in backspaces) { LowLevelAdapter.SendKeyPress(vkCode, false); }
            // funny fix for my skype
            Thread.Sleep(20);
            LowLevelAdapter.SetNextKeyboardLayout();
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
