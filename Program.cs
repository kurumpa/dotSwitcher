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
using System.Threading;
using System.Windows.Forms;

namespace dotSwitcher
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{D09B5AC3-1219-4987-88A3-7C80075A571B}");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var settings = Settings.Init();
                var engine = new Switcher(settings);
                Application.ApplicationExit += (s, a) => { engine.Dispose(); };
                var app = new SettingsForm(settings, engine);
                app.Exit += (s, e) => Application.Exit();
                var context = new ApplicationContext(app);
                Application.Run(context);
                mutex.ReleaseMutex();
            }
            else
            {
                LowLevelAdapter.SendShowSettingsMessage();
            }
        }
    }
}
