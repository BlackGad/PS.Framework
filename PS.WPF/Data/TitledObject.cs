using PS.Patterns.Aware;

namespace PS.WPF.Data
{
    public class TitledObject : BaseNotifyPropertyChanged,
                                ITitleAware,
                                IPayloadAware<object>
    {
        private object _payload;
        private string _title;

        public TitledObject()
        {
        }

        public TitledObject(string title, object payload)
        {
            Title = title;
            Payload = payload;
        }

        public object Payload
        {
            get { return _payload; }
            set { SetField(ref _payload, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetField(ref _title, value); }
        }
    }
}
