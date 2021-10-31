using System;
using System.Collections.Generic;
using NLog;
using NLog.Targets;

namespace PS.Shell.Models.PageService
{
    class CollectionTarget : TargetWithLayout
    {
        private readonly IList<string> _collection;

        #region Constructors

        public CollectionTarget(IList<string> collection)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        #endregion

        #region Override members

        protected override void Write(LogEventInfo logEvent)
        {
            var item = Layout.Render(logEvent);
            _collection.Add(item);
        }

        #endregion
    }
}