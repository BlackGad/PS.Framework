using PS.Extensions;

namespace PS.MVVM.Services.CommandService
{
    public class CommandContext : CommandServiceComponent
    {
        private object _tag;

        #region Properties

        public object Tag
        {
            get { return _tag; }
            set
            {
                if (_tag.AreEqual(value)) return;
                _tag = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}