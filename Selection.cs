using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace dotSwitcher
{
    class Selection
    {
        //http://www.jayway.com/2013/02/06/how-to-get-selected-text-from-another-windows-program/

        public static String GetSelectionByAutomation()
        {
            var element = AutomationElement.FocusedElement;

            if (element == null) return null;

            object pattern;
            if (element.TryGetCurrentPattern(TextPattern.Pattern, out pattern))
            {
                var tp = (TextPattern) pattern;
                var sb = new StringBuilder();
 
                foreach (var r in tp.GetSelection())
                {
                    sb.AppendLine(r.GetText(-1));
                }
 
                return sb.ToString();
            }
            return null;
        }

    }
}
