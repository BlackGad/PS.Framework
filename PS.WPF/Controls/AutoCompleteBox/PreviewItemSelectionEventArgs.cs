using System.Windows;

namespace PS.WPF.Controls
{
    public class PreviewItemSelectionEventArgs : RoutedEventArgs
    {
        public PreviewItemSelectionEventArgs(string input, RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
            Input = input;
        }

        public string Input { get; }

        public object Item { get; set; }
    }
}
