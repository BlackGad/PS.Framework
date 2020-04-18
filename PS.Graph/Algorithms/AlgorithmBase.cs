using System;
using System.Collections.Generic;
using PS.Graph.Algorithms.Services;

namespace PS.Graph.Algorithms
{
    public abstract class AlgorithmBase<TGraph> : IAlgorithm<TGraph>,
                                                  IAlgorithmComponent
    {
        private readonly AlgorithmServices _algorithmServices;

        private Dictionary<Type, object> _services;
        private volatile ComputationState _state = ComputationState.NotRunning;
        private volatile object _syncRoot = new object();

        #region Constructors

        /// <summary>
        ///     Creates a new algorithm with an (optional) host.
        /// </summary>
        /// <param name="host">if null, host is set to the this reference</param>
        /// <param name="visitedGraph"></param>
        protected AlgorithmBase(IAlgorithmComponent host, TGraph visitedGraph)
        {
            if (host == null)
            {
                host = this;
            }

            VisitedGraph = visitedGraph;
            _algorithmServices = new AlgorithmServices(host);
        }

        protected AlgorithmBase(TGraph visitedGraph)
        {
            VisitedGraph = visitedGraph;
            _algorithmServices = new AlgorithmServices(this);
        }

        #endregion

        #region IAlgorithm<TGraph> Members

        public TGraph VisitedGraph { get; }

        public Object SyncRoot
        {
            get { return _syncRoot; }
        }

        public ComputationState State
        {
            get
            {
                lock (_syncRoot)
                {
                    return _state;
                }
            }
        }

        public void Compute()
        {
            BeginComputation();
            Initialize();
            try
            {
                InternalCompute();
            }
            finally
            {
                Clean();
            }

            EndComputation();
        }

        public void Abort()
        {
            var raise = false;
            lock (_syncRoot)
            {
                if (_state == ComputationState.Running)
                {
                    _state = ComputationState.PendingAbortion;
                    Services.CancelManager.Cancel();
                    raise = true;
                }
            }

            if (raise)
            {
                OnStateChanged(EventArgs.Empty);
            }
        }

        public event EventHandler StateChanged;

        public event EventHandler Started;

        public event EventHandler Finished;

        public event EventHandler Aborted;

        #endregion

        #region IAlgorithmComponent Members

        public IAlgorithmServices Services
        {
            get { return _algorithmServices; }
        }

        public T GetService<T>()
            where T : IService
        {
            if (!TryGetService(out T service))
            {
                throw new InvalidOperationException("service not found");
            }

            return service;
        }

        public bool TryGetService<T>(out T service)
            where T : IService
        {
            if (TryGetService(typeof(T), out var serviceObject))
            {
                service = (T)serviceObject;
                return true;
            }

            service = default;
            return false;
        }

        #endregion

        #region Members

        protected void BeginComputation()
        {
            lock (_syncRoot)
            {
                _state = ComputationState.Running;
                Services.CancelManager.ResetCancel();
                OnStarted(EventArgs.Empty);
                OnStateChanged(EventArgs.Empty);
            }
        }

        protected virtual void Clean()
        {
        }

        protected void EndComputation()
        {
            lock (_syncRoot)
            {
                switch (_state)
                {
                    case ComputationState.Running:
                        _state = ComputationState.Finished;
                        OnFinished(EventArgs.Empty);
                        break;
                    case ComputationState.PendingAbortion:
                        _state = ComputationState.Aborted;
                        OnAborted(EventArgs.Empty);
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                Services.CancelManager.ResetCancel();
                OnStateChanged(EventArgs.Empty);
            }
        }

        protected virtual void Initialize()
        {
        }

        protected abstract void InternalCompute();

        protected virtual void OnAborted(EventArgs e)
        {
            var eh = Aborted;
            eh?.Invoke(this, e);
        }

        protected virtual void OnFinished(EventArgs e)
        {
            var eh = Finished;
            eh?.Invoke(this, e);
        }

        protected virtual void OnStarted(EventArgs e)
        {
            var eh = Started;
            eh?.Invoke(this, e);
        }

        protected virtual void OnStateChanged(EventArgs e)
        {
            var eh = StateChanged;
            eh?.Invoke(this, e);
        }

        protected virtual bool TryGetService(Type serviceType, out object service)
        {
            lock (SyncRoot)
            {
                if (_services == null)
                {
                    _services = new Dictionary<Type, object>();
                }

                if (!_services.TryGetValue(serviceType, out service))
                {
                    if (serviceType == typeof(ICancelManager))
                    {
                        _services[serviceType] = service = new CancelManager();
                    }
                    else
                    {
                        _services[serviceType] = null;
                    }
                }

                return service != null;
            }
        }

        #endregion
    }
}