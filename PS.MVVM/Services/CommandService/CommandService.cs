using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using PS.Collections;
using PS.ComponentModel;
using PS.ComponentModel.DeepTracker;
using PS.ComponentModel.DeepTracker.Extensions;
using PS.Data;
using PS.Extensions;
using PS.Patterns.Aware;
using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;

namespace PS.MVVM.Services.CommandService
{
    public class CommandService : ICommandService,
                                  IDisposable,
                                  INotifyPropertyChanged
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly ObjectsStorage<object, EventHandlerSubscriptionStorage> _activations;
        private readonly DeepTracker _componentsTracker;
        private readonly MutableLookup<object, ComponentMetadata> _metadata;
        private readonly ObservableCollection<CommandServiceComponent> _trackerRegistry;
        private readonly ObservableCollection<CommandServiceComponent> _viewRegistry;

        public CommandService()
        {
            _activations = new ObjectsStorage<object, EventHandlerSubscriptionStorage>(ComponentSubscriptionActivationFactory);
            _trackerRegistry = new ObservableCollection<CommandServiceComponent>();
            _viewRegistry = new ObservableCollection<CommandServiceComponent>();
            _metadata = new MutableLookup<object, ComponentMetadata>();

            _componentsTracker = DeepTracker.Setup(_trackerRegistry)
                                            .Subscribe<ObjectAttachmentEventArgs>(OnObjectAttachmentEvent)
                                            .Subscribe<ChangedPropertyEventArgs>(OnChangedPropertyEvent)
                                            .Include<CommandServiceComponent>(instance => instance.Id)
                                            .Include<CommandBundle>(instance => instance.Groups)
                                            .Include<CommandBundle>(instance => instance.Context)
                                            .Include<CommandGroup>(instance => instance.Commands)
                                            .Include<CommandServiceCommand>(instance => instance.Commands)
                                            .Create()
                                            .Activate();
        }

        public void Add(CommandServiceComponent component)
        {
            if (_trackerRegistry.Contains(component)) return;
            _trackerRegistry.Add(component);
        }

        public IEnumerable CreateView(Func<CommandServiceComponent, bool> filter)
        {
            return new CollectionView<CommandServiceComponent>(filter)
            {
                ItemsSource = _viewRegistry
            };
        }

        public IReadOnlyCollection<CommandServiceComponent> Find(object identifier)
        {
            if (identifier is IIDAware<object> idAware) identifier = idAware.Id;
            if (identifier == null)
            {
                return _metadata.SelectMany(g => g.Select(metadata => metadata.ComponentReference.Target)
                                                  .Enumerate<CommandServiceComponent>())
                                .ToList();
            }

            if (!_metadata.Contains(identifier)) return Enumerable.Empty<CommandServiceComponent>().ToList();

            return _metadata[identifier].Select(metadata => metadata.ComponentReference.Target)
                                        .Enumerate<CommandServiceComponent>()
                                        .ToList();
        }

        public ISubscriptionAware HandleActivation(object identifier)
        {
            if (identifier == null) throw new ArgumentNullException(nameof(identifier));

            if (identifier is IIDAware<object> idAware) identifier = idAware.Id;
            return _activations[identifier];
        }

        public void Dispose()
        {
            _activations?.Dispose();
            _componentsTracker?.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnChangedPropertyEvent(object sender, ChangedPropertyEventArgs e)
        {
            //Component id was changed
            if (typeof(CommandServiceComponent).IsAssignableFrom(e.PropertyReference.SourceType) &&
                e.PropertyReference.Name.AreEqual(nameof(CommandServiceComponent.Id)))
            {
                var component = (CommandServiceComponent)e.PropertyReference.GetSource();

                var registeredMetadata = _metadata[e.OldValue].FirstOrDefault(metadata => metadata.ComponentReference.Target.AreEqual(component));
                _metadata.Remove(e.OldValue, registeredMetadata);
                _metadata.Add(e.NewValue, registeredMetadata);
                _activations[e.NewValue].Raise(this, component);
            }
        }

        private void OnObjectAttachmentEvent(object sender, ObjectAttachmentEventArgs e)
        {
            if (e is ObjectAttachedEventArgs && e.Object is CommandServiceComponent attachedComponent)
            {
                var existingMetadata = _metadata[attachedComponent.Id].FirstOrDefault(metadata => metadata.ComponentReference.Target.AreEqual(attachedComponent));
                if (existingMetadata == null)
                {
                    _metadata.Add(attachedComponent.Id, new ComponentMetadata(attachedComponent));
                    _viewRegistry.Add(attachedComponent);
                    _activations[attachedComponent.Id].Raise(this, attachedComponent);
                }
                else
                {
                    existingMetadata.AddReference();
                }
            }

            if (e is ObjectDetachedEventArgs && e.Object is CommandServiceComponent detachedComponent)
            {
                var existingMetadata = _metadata[detachedComponent.Id].FirstOrDefault(metadata => metadata.ComponentReference.Target.AreEqual(detachedComponent));
                if (existingMetadata == null) return;

                if (existingMetadata.ReleaseReference() == 0)
                {
                    _metadata.Remove(detachedComponent.Id, existingMetadata);
                    _viewRegistry.Remove(detachedComponent);
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private EventHandlerSubscriptionStorage ComponentSubscriptionActivationFactory(object id)
        {
            return new EventHandlerSubscriptionStorage((storage, @delegate) => SubscriptionAddedAction(id, storage, @delegate));
        }

        private void SubscriptionAddedAction(object id, IInvokeEventHandlerAware storage, Delegate @delegate)
        {
            var components = Find(id);
            foreach (var component in components)
            {
                storage.InvokeEventHandler(@delegate, this, component);
            }
        }

        #region Nested type: ComponentMetadata

        private class ComponentMetadata
        {
            private int _referenceCount;

            public ComponentMetadata(CommandServiceComponent component)
            {
                ComponentReference = new WeakReference(component);
                AddReference();
            }

            public WeakReference ComponentReference { get; }

            public void AddReference()
            {
                Interlocked.Increment(ref _referenceCount);
            }

            public int ReleaseReference()
            {
                return Interlocked.Decrement(ref _referenceCount);
            }
        }

        #endregion
    }
}
