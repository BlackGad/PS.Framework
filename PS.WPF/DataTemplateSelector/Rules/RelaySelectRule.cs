using System;

namespace PS.WPF.DataTemplateSelector.Rules
{
    public class RelaySelectRule : SelectRule
    {
        private readonly Func<object, bool> _predicate;

        #region Constructors

        public RelaySelectRule(Func<object, bool> predicate)
        {
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        #endregion

        #region Override members

        public override bool IsValid(object item)
        {
            return _predicate(item);
        }

        #endregion
    }
}