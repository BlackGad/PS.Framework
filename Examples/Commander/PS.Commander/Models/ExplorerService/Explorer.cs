using Newtonsoft.Json;

namespace PS.Commander.Models.ExplorerService
{
    public class Explorer : BaseNotifyPropertyChanged
    {
        private Area _area;
        private string _origin;

        #region Properties

        [JsonProperty]
        public Area Area
        {
            get { return _area; }
            set { SetField(ref _area, value); }
        }

        [JsonProperty]
        public string Origin
        {
            get { return _origin; }
            set { SetField(ref _origin, value); }
        }

        #endregion
    }
}