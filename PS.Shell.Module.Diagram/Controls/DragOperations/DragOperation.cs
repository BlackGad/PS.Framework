using System;
using System.Windows;
using System.Windows.Input;

namespace PS.Shell.Module.Diagram.Controls
{
    public abstract class DragOperation
    {
        private readonly IInputElement _inputElement;
        private readonly Point _start;
        private bool _finished;

        #region Constructors

        protected DragOperation(IInputElement inputElement, Point start)
        {
            _inputElement = inputElement ?? throw new ArgumentNullException(nameof(inputElement));
            _start = start;
            Mouse.Capture(inputElement);
        }

        #endregion

        #region Members

        public void Commit()
        {
            _finished = true;
            Mouse.Capture(null);
        }

        public void Rollback()
        {
            if (_finished) return;
            OnCancel();
            Mouse.Capture(null);
            _finished = true;
        }

        public void Update(Point newPoint)
        {
            if (_finished) return;
            OnUpdate(_start - newPoint);
        }

        protected abstract void OnCancel();

        protected abstract void OnUpdate(Vector offset);

        #endregion
    }
}