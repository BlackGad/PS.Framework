using System;

namespace PS.IoC
{
    public abstract class Bootstrapper<TContainer> : IDisposable
        where TContainer : class
    {
        private readonly IBootstrapperLogger _logger;
        private TContainer _container;
        private bool _disposed;
        private bool _disposing;

        #region Constructors

        protected Bootstrapper(IBootstrapperLogger logger)
        {
            _logger = logger ?? new RelayBootstrapperLogger();
        }

        #endregion

        #region Properties

        public TContainer Container
        {
            get
            {
                CheckDisposed();
                return _container;
            }
        }

        public bool IsInitialized { get; private set; }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            CheckDisposed();
            if (_disposing) return;

            try
            {
                _disposing = true;

                _logger.Trace("Application cleanup try");
                if (Container == null) return;

                try
                {
                    _logger.Trace("Application IOC cleaning");
                    DisposeContainer(_logger, Container);
                    _logger.Debug("Application IOC cleaned successfully");
                }
                catch (Exception e)
                {
                    var message = "Error on IOC cleaning. Details: " + e.GetBaseException().Message;
                    _logger.Warn(message);
                }

                try
                {
                    _logger.Trace("Application disposing");
                    Dispose(_logger);
                    _logger.Debug("Application disposed");
                }
                catch (Exception e)
                {
                    _logger.Warn($"Error on IOC cleaning. Details: {e.GetBaseException().Message}");
                }

                _container = null;
            }
            finally
            {
                GC.SuppressFinalize(this);
                _disposing = false;
                _disposed = true;
            }
        }

        #endregion

        #region Members

        public void Initialize(TContainer parentContainer = null)
        {
            if (_disposed) throw new ObjectDisposedException("Bootstrapper is already disposed");

            try
            {
                _logger.Trace("Initializing critical components...");

                InitializeCriticalComponents(_logger, parentContainer);

                _logger.Debug("Critical components initialized successfully");
            }
            catch (Exception e)
            {
                var message = $"Unrecoverable error on IOC creation. Details: {e.GetBaseException().Message}";
                _logger.Fatal(message);
                throw new ApplicationException(message, e);
            }

            try
            {
                _logger.Trace("Creating IOC container...");
                _container = CreateContainer(_logger, parentContainer);
                if (Container != null)
                {
                    _logger.Trace("IOC container successfully created");
                }
                else
                {
                    _logger.Trace("Application does not support IOC container functionality");
                }
            }
            catch (Exception e)
            {
                var message = "Unrecoverable error on IOC creation. Details: " + e.GetBaseException().Message;
                _logger.Fatal(message);
                throw new ApplicationException(message, e);
            }

            if (Container != null)
            {
                try
                {
                    _logger.Trace("Composing IOC container...");
                    ComposeContainer(_logger, Container);
                    _logger.Trace("IOC container successfully composed");
                }
                catch (Exception e)
                {
                    var message = "Unrecoverable error on IOC compose. Details: " + e.GetBaseException().Message;
                    _logger.Fatal(message);
                    throw new ApplicationException(message, e);
                }
            }

            try
            {
                _logger.Trace("Visual theme setup...");
                SetupVisualTheme(_logger, Container);
                _logger.Trace("Visual theme setup successfully finished");
            }
            catch (Exception e)
            {
                var message = "Unrecoverable error on visual theme setup. Details: " + e.GetBaseException().Message;
                _logger.Fatal(message);
                throw new ApplicationException(message, e);
            }

            IsInitialized = true;
        }

        protected void CheckDisposed()
        {
            if (_disposed) throw new ObjectDisposedException("Bootstrapper is already disposed");
        }

        protected virtual void ComposeContainer(IBootstrapperLogger logger, TContainer container)
        {
        }

        protected virtual TContainer CreateContainer(IBootstrapperLogger logger, TContainer parentContainer)
        {
            return default;
        }

        protected virtual void Dispose(IBootstrapperLogger logger)
        {
        }

        protected virtual void DisposeContainer(IBootstrapperLogger logger, TContainer container)
        {
        }

        protected virtual void InitializeCriticalComponents(IBootstrapperLogger logger, TContainer container)
        {
        }

        protected virtual void SetupVisualTheme(IBootstrapperLogger logger, TContainer container)
        {
        }

        #endregion
    }
}