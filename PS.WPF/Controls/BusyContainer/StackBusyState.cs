using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PS.Extensions;
using PS.Patterns.Aware;

namespace PS.WPF.Controls.BusyContainer
{
    public class StackBusyState : BaseNotifyPropertyChanged,
                                  ITitleAware,
                                  IDescriptionAware
    {
        private readonly object _locker;
        private readonly Stack<IBusyState> _stack;
        private readonly HashSet<IBusyState> _states;

        public StackBusyState()
        {
            _locker = new object();
            _stack = new Stack<IBusyState>();
            _states = new HashSet<IBusyState>();
        }

        public bool IsBusy { get; private set; }

        public SynchronizationContext SynchronizationContext { get; set; }

        public string Description { get; private set; }

        public string Title { get; private set; }

        public IBusyState Push(string title = null, string description = null)
        {
            lock (_locker)
            {
                var state = new InternalBusyState(StateTitleChanged,
                                                  StateDescriptionChanged,
                                                  StateDisposed);
                _states.Add(state);
                _stack.Push(state);

                state.Title = title;
                state.Description = description;

                UpdateTopState();

                return state;
            }
        }

        private void SafeCall(Action action)
        {
            if (SynchronizationContext != null)
            {
                SynchronizationContext.Send(o => action(), null);
            }
            else
            {
                action();
            }
        }

        private void StateDescriptionChanged(InternalBusyState state, string value)
        {
            lock (_locker)
            {
                var isTopState = _stack.Any() && _stack.Peek().AreEqual(state);
                if (!isTopState) return;

                Description = value;

                SafeCall(() => OnPropertyChanged(nameof(Description)));
            }
        }

        private void StateDisposed(InternalBusyState state)
        {
            lock (_locker)
            {
                _states.Remove(state);

                var isTopState = _stack.Any() && _stack.Peek().AreEqual(state);
                if (!isTopState) return;

                _stack.Pop();
                UpdateTopState();
            }
        }

        private void StateTitleChanged(InternalBusyState state, string value)
        {
            lock (_locker)
            {
                var isTopState = _stack.Any() && _stack.Peek().AreEqual(state);
                if (!isTopState) return;

                Title = value;

                SafeCall(() => OnPropertyChanged(nameof(Title)));
            }
        }

        private void UpdateTopState()
        {
            lock (_locker)
            {
                string newTitle = null;
                string newDescription = null;
                var newIsBusy = false;

                while (_stack.Any())
                {
                    var topState = _stack.Peek();
                    if (_states.Contains(topState))
                    {
                        newTitle = topState.Title;
                        newDescription = topState.Description;
                        newIsBusy = true;
                        break;
                    }

                    _stack.Pop();
                }

                Title = newTitle;
                Description = newDescription;
                IsBusy = newIsBusy;

                SafeCall(() =>
                {
                    OnPropertyChanged(nameof(Title));
                    OnPropertyChanged(nameof(Description));
                    OnPropertyChanged(nameof(IsBusy));
                });
            }
        }

        #region Nested type: InternalBusyState

        class InternalBusyState : BaseNotifyPropertyChanged,
                                  IBusyState
        {
            private readonly Action<InternalBusyState, string> _descriptionChanged;
            private readonly Action<InternalBusyState> _disposed;
            private readonly Action<InternalBusyState, string> _titleChanged;

            private string _description;
            private string _title;

            public InternalBusyState(Action<InternalBusyState, string> titleChanged,
                                     Action<InternalBusyState, string> descriptionChanged,
                                     Action<InternalBusyState> disposed)
            {
                _titleChanged = titleChanged ?? throw new ArgumentNullException(nameof(titleChanged));
                _descriptionChanged = descriptionChanged ?? throw new ArgumentNullException(nameof(descriptionChanged));
                _disposed = disposed ?? throw new ArgumentNullException(nameof(disposed));
            }

            public void Dispose()
            {
                _disposed(this);
            }

            public string Title
            {
                get { return _title; }
                set
                {
                    if (SetField(ref _title, value))
                    {
                        _titleChanged(this, value);
                    }
                }
            }

            public string Description
            {
                get { return _description; }
                set
                {
                    if (SetField(ref _description, value))
                    {
                        _descriptionChanged(this, value);
                    }
                }
            }
        }

        #endregion
    }
}
