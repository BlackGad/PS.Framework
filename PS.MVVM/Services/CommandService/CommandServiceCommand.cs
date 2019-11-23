using System.Collections.ObjectModel;
using PS.Extensions;
using PS.Patterns.Aware;

namespace PS.MVVM.Services.CommandService
{
    public abstract class CommandServiceCommand : CommandServiceComponent,
                                                  IExtendedToolTipAware,
                                                  ILargeIconAware,
                                                  ISmallIconAware

    {
        private object _largeIcon;
        private object _smallIcon;
        private string _toolTipDescription;
        private string _toolTipFooterDescription;
        private object _toolTipFooterImage;
        private string _toolTipFooterTitle;
        private object _toolTipImage;
        private string _toolTipTitle;

        #region Constructors

        protected CommandServiceCommand()
        {
            Commands = new ObservableCollection<CommandServiceCommand>();
        }

        #endregion

        #region Properties

        public ObservableCollection<CommandServiceCommand> Commands { get; }

        #endregion

        #region IExtendedToolTipAware Members

        public string ToolTipDescription
        {
            get { return _toolTipDescription; }
            set
            {
                if (_toolTipDescription.AreEqual(value)) return;
                _toolTipDescription = value;
                OnPropertyChanged();
            }
        }

        public string ToolTipTitle
        {
            get { return _toolTipTitle; }
            set
            {
                if (_toolTipTitle.AreEqual(value)) return;
                _toolTipTitle = value;
                OnPropertyChanged();
            }
        }

        public string ToolTipFooterDescription
        {
            get { return _toolTipFooterDescription; }
            set
            {
                if (_toolTipFooterDescription.AreEqual(value)) return;
                _toolTipFooterDescription = value;
                OnPropertyChanged();
            }
        }

        public string ToolTipFooterTitle
        {
            get { return _toolTipFooterTitle; }
            set
            {
                if (_toolTipFooterTitle.AreEqual(value)) return;
                _toolTipFooterTitle = value;
                OnPropertyChanged();
            }
        }

        public object ToolTipFooterImage
        {
            get { return _toolTipFooterImage; }
            set
            {
                if (_toolTipFooterImage.AreEqual(value)) return;
                _toolTipFooterImage = value;
                OnPropertyChanged();
            }
        }

        public object ToolTipImage
        {
            get { return _toolTipImage; }
            set
            {
                if (_toolTipImage.AreEqual(value)) return;
                _toolTipImage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region ILargeIconAware Members

        public object LargeIcon
        {
            get { return _largeIcon; }
            set
            {
                if (_largeIcon.AreEqual(value)) return;
                _largeIcon = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region ISmallIconAware Members

        public object SmallIcon
        {
            get { return _smallIcon; }
            set
            {
                if (_smallIcon.AreEqual(value)) return;
                _smallIcon = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}