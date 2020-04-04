using System.Collections;
using System.Windows;

namespace PS.WPF.Data
{
    public sealed class CompositeContainer : InheritanceContextPropagator
    {
        #region Property definitions

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source),
                                        typeof(IEnumerable),
                                        typeof(CompositeContainer),
                                        new FrameworkPropertyMetadata(null));

        #endregion

        #region Properties

        public IEnumerable Source
        {
            get { return (IEnumerable)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        #endregion
    }
}