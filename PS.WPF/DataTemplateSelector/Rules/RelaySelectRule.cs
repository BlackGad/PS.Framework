using System;

namespace PS.WPF.DataTemplateSelector.Rules
{
    public class RelaySelectRule : SelectRule
    {
        private readonly Func<object, bool> _predicate;

        public RelaySelectRule(Func<object, bool> predicate)
        {
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        public override bool IsValid(object item)
        {
            return _predicate(item);
        }
    }
}
