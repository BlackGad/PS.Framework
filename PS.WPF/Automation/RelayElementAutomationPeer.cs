using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;

namespace PS.WPF.Automation
{
    public class RelayElementAutomationPeer : FrameworkElementAutomationPeer
    {
        #region Constructors

        public RelayElementAutomationPeer(FrameworkElement owner)
            : base(owner)
        {
        }

        #endregion

        #region Properties

        public string ClassName { get; set; }
        public AutomationControlType ControlType { get; set; }
        public Func<PatternInterface, UIElement, object> GetPatternFunc { get; set; }

        #endregion

        #region Override members

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

        #endregion
    }
}