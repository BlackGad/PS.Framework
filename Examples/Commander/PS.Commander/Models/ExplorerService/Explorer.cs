using System;
using Newtonsoft.Json;

namespace PS.Commander.Models.ExplorerService
{
    public class Explorer : BaseNotifyPropertyChanged
    {
        private string _id;
        private string _origin;

        #region Constructors

        public Explorer()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        #endregion

        #region Properties

        [JsonProperty]
        public string Id
        {
            get { return _id; }
            set { SetField(ref _id, value); }
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