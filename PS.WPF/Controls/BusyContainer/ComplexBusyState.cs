﻿using System;
using System.Collections.Generic;
using System.Linq;
using PS.Extensions;
using PS.Patterns.Aware;

namespace PS.WPF.Controls.BusyContainer
{
    public class ComplexBusyState : BaseNotifyPropertyChanged,
                                    ITitleAware,
                                    IDescriptionAware
    {
        private readonly object _locker;
        private readonly Queue<IBusyState> _queue;
        private readonly HashSet<IBusyState> _states;

        #region Constructors

        public ComplexBusyState()
        {
            _locker = new object();
            _queue = new Queue<IBusyState>();
            _states = new HashSet<IBusyState>();
        }

        #endregion

        #region Properties

        public bool IsBusy { get; private set; }

        #endregion

        #region IDescriptionAware Members

        public string Description { get; private set; }

        #endregion

        #region ITitleAware Members

        public string Title { get; private set; }

        #endregion

        #region Members

        public IBusyState Enqueue(string title = null, string description = null)
        {
            lock (_locker)
            {
                var state = new InternalBusyState(StateTitleChanged,
                                                  StateDescriptionChanged,
                                                  StateDisposed);
                _states.Add(state);
                _queue.Enqueue(state);

                state.Title = title;
                state.Description = description;

                UpdateTopState();

                return state;
            }
        }

        private void StateDescriptionChanged(InternalBusyState state, string value)
        {
            lock (_locker)
            {
                var isTopState = _queue.Any() && _queue.Peek().AreEqual(state);
                if (!isTopState) return;

                Description = value;

                //TODO: Controllable dispatch
                OnPropertyChanged(nameof(Description));
            }
        }

        private void StateDisposed(InternalBusyState state)
        {
            lock (_locker)
            {
                _states.Remove(state);

                var isTopState = _queue.Any() && _queue.Peek().AreEqual(state);
                if (!isTopState) return;

                _queue.Dequeue();
                UpdateTopState();
            }
        }

        private void StateTitleChanged(InternalBusyState state, string value)
        {
            lock (_locker)
            {
                var isTopState = _queue.Any() && _queue.Peek().AreEqual(state);
                if (!isTopState) return;

                Title = value;

                //TODO: Controllable dispatch
                OnPropertyChanged(nameof(Title));
            }
        }

        private void UpdateTopState()
        {
            lock (_locker)
            {
                string newTitle = null;
                string newDescription = null;
                var newIsBusy = false;

                while (_queue.Any())
                {
                    var topState = _queue.Peek();
                    if (_states.Contains(topState))
                    {
                        newTitle = topState.Title;
                        newDescription = topState.Description;
                        newIsBusy = true;
                        break;
                    }

                    _queue.Dequeue();
                }

                Title = newTitle;
                Description = newDescription;
                IsBusy = newIsBusy;

                //TODO: Controllable dispatch
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        #endregion

        #region Nested type: InternalBusyState

        class InternalBusyState : BaseNotifyPropertyChanged,
                                  IBusyState
        {
            private readonly Action<InternalBusyState, string> _descriptionChanged;
            private readonly Action<InternalBusyState> _disposed;
            private readonly Action<InternalBusyState, string> _titleChanged;

            private string _description;
            private string _title;

            #region Constructors

            public InternalBusyState(Action<InternalBusyState, string> titleChanged,
                                     Action<InternalBusyState, string> descriptionChanged,
                                     Action<InternalBusyState> disposed)
            {
                _titleChanged = titleChanged ?? throw new ArgumentNullException(nameof(titleChanged));
                _descriptionChanged = descriptionChanged ?? throw new ArgumentNullException(nameof(descriptionChanged));
                _disposed = disposed ?? throw new ArgumentNullException(nameof(disposed));
            }

            #endregion

            #region IBusyState Members

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

            #endregion
        }

        #endregion
    }
}