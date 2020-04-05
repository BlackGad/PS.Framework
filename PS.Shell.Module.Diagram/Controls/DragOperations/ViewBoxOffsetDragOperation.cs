using System;
using System.Windows;

namespace PS.Shell.Module.Diagram.Controls
{
    public class ViewBoxOffsetDragOperation : DragOperation
    {
        private readonly Point _initialViewBoxOffset;
        private readonly DiagramWorkspace _workspace;

        #region Constructors

        public ViewBoxOffsetDragOperation(IInputElement inputElement, Point start, DiagramWorkspace workspace)
            : base(inputElement, start)
        {
            _workspace = workspace ?? throw new ArgumentNullException(nameof(workspace));
            _initialViewBoxOffset = _workspace.ViewBoxOffset;
        }

        #endregion

        #region Override members

        protected override void OnCancel()
        {
            if (_workspace.ViewBoxOffset != _initialViewBoxOffset) _workspace.ViewBoxOffset = _initialViewBoxOffset;
        }

        protected override void OnUpdate(Vector offset)
        {
            if (_workspace.ViewBoxOffset != _initialViewBoxOffset) _workspace.ViewBoxOffset = _initialViewBoxOffset;

            var expectedOffset = _initialViewBoxOffset - offset;
            if (_workspace.ViewBoxOffset != expectedOffset) _workspace.ViewBoxOffset = expectedOffset;
        }

        #endregion
    }
}