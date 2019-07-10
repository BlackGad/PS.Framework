using System;
using PS.Data;

namespace PS.MVVM.Services
{
    public class ModelResolverService : IModelResolverService,
                                        IDisposable
    {
        #region Constants

        private static readonly object Null;

        #endregion

        private readonly ObjectsStorage<object, ObservableModelCollection> _collectionStorage;
        private readonly ObjectsStorage<object, ObservableModelObject> _objectStorage;

        #region Constructors

        static ModelResolverService()
        {
            Null = new object();
        }

        public ModelResolverService()
        {
            _collectionStorage = new ObjectsStorage<object, ObservableModelCollection>();
            _objectStorage = new ObjectsStorage<object, ObservableModelObject>();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _collectionStorage.Clear();
            _objectStorage.Clear();
        }

        #endregion

        #region IModelResolverService Members

        public IObservableModelCollection Collection(object region)
        {
            return _collectionStorage[region ?? Null];
        }

        public IObservableModelObject Object(object region)
        {
            return _objectStorage[region ?? Null];
        }

        #endregion
    }
}