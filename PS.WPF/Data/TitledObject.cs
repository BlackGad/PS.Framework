using PS.Patterns.Aware;

namespace PS.WPF.Data
{
    public class TitledObject : BaseNotifyPropertyChanged,
                                ITitleAware,
                                IPayloadAware<object>
    {
        private object _payload;
        private string _title;

        #region Constructors

        public TitledObject()
        {
        }

        public TitledObject(string title, object payload)
        {
            Title = title;
            Payload = payload;
        }

        #endregion

        #region IPayloadAware<object> Members

        public object Payload
        {
            get { return _payload; }
            set { SetField(ref _payload, value); }
        }

        #endregion

        #region ITitleAware Members

        public string Title
        {
            get { return _title; }
            set { SetField(ref _title, value); }
        }

        #endregion
    }
}