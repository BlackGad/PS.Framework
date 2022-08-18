using System;
using PS.Data;

namespace PS.MVVM.Services
{
    public class ModelResolverService : IModelResolverService,
                                        IDisposable
    {
        private static readonly object Null;

        private readonly ObjectsStorage<object, ObservableModelCollection> _collectionStorage;
        private readonly ObjectsStorage<object, ObservableModelObject> _objectStorage;

        static ModelResolverService()
        {
            Null = new object();
        }

        public ModelResolverService()
        {
            _collectionStorage = new ObjectsStorage<object, ObservableModelCollection>();
            _objectStorage = new ObjectsStorage<object, ObservableModelObject>();
        }

        public void Dispose()
        {
            _collectionStorage.Clear();
            _objectStorage.Clear();
        }

        public IObservableModelCollection Collection(object region)
        {
            return _collectionStorage[region ?? Null];
        }

        public IObservableModelObject Object(object region)
        {
            return _objectStorage[region ?? Null];
        }
    }
}
