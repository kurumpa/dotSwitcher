/**
 *   DotSwitcher: a simple keyboard layout switcher
 *   Copyright (C) 2014-2019 Kirill Mokhovtsev / kurumpa
 *   Contact: kiev.programmer@gmail.com
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

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
        private ISettings settings;
        public Switcher(ISettings settings)
        {
            this.settings = settings;
            kbdHook = new KeyboardHook();
            kbdHook.KeyboardEvent += ProcessKeyPress;
            mouseHook = new MouseHook();
            mouseHook.MouseEvent += ProcessMousePress;
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

            if (evtData.Equals(settings.SwitchLayoutHotkey))
            {
                SwitchLayout();
                evtData.Handled = true;
                return;
            }

            if (evtData.Equals(settings.SwitchHotkey))
            {
                ConvertLast();
                evtData.Handled = true;
                return;
            }

            if (evtData.Equals(settings.ConvertSelectionHotkey))
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
            return currentWord[currentWord.Count - 1].KeyCode;
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
            LowLevelAdapter.BackupClipboard();
            LowLevelAdapter.SendCopy();
            var selection = Clipboard.GetText();
            LowLevelAdapter.RestoreClipboard();
            if (String.IsNullOrEmpty(selection))
            {
                return;
            }

            
            LowLevelAdapter.ReleasePressedFnKeys();

            var keys = new List<Keys>(selection.Length);
            for(var i = 0; i < selection.Length; i++)
            {
                keys.Add(LowLevelAdapter.ToKey(selection[i]));
            }
            LowLevelAdapter.SetNextKeyboardLayout();

            foreach (var key in keys)
            {
                Debug.Write(key);
                if (key != Keys.None)
                {
                    LowLevelAdapter.SendKeyPress(key, (key & Keys.Shift) != Keys.None);
                }
            }

        }
        private void SwitchLayout()
        {
            BeginNewWord();
            LowLevelAdapter.SetNextKeyboardLayout();
        }
        private void ConvertLast()
        {
            LowLevelAdapter.ReleasePressedFnKeys();
            var word = currentWord.ToList();
            var backspaces = Enumerable.Repeat<Keys>(Keys.Back, word.Count);

            foreach (var vkCode in backspaces) { LowLevelAdapter.SendKeyPress(vkCode, false); }
            // Fix for skype
            Thread.Sleep(settings.SwitchDelay);
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
