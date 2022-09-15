using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;

namespace PS.WPF.Automation
{
    public class RelayElementAutomationPeer : FrameworkElementAutomationPeer
    {
        public RelayElementAutomationPeer(FrameworkElement owner)
            : base(owner)
        {
        }

        public string ClassName { get; set; }

        public AutomationControlType ControlType { get; set; }

        public Func<PatternInterface, UIElement, object> GetPatternFunc { get; set; }

        public override object GetPattern(PatternInterface patternInterface)
        {
            return GetPatternFunc?.Invoke(patternInterface, Owner) ?? base.GetPattern(patternInterface);
        }

        protected override List<AutomationPeer> GetChildrenCore()
        {
            return null;
        }

        protected override string GetClassNameCore()
        {
            return ClassName ?? "Undefined";
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return ControlType;
        }
    }
}
