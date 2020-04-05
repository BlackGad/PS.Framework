using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PS.Extensions;

namespace PS.Shell.Module.Diagram.Controls
{
    public class ItemsDragOperation : DragOperation
    {
        private readonly Dictionary<UIElement, Point> _items;

        #region Constructors

        public ItemsDragOperation(IInputElement inputElement, Point start, IEnumerable<UIElement> items)
            : base(inputElement, start)
        {
            _items = items.Enumerate().Distinct().ToDictionary(i => i, i => new Point(Canvas.GetLeft(i), Canvas.GetTop(i)));
        }

        #endregion

        #region Override members

        protected override void OnCancel()
        {
            foreach (var pair in _items)
            {
                var item = pair.Key;
                var initialPosition = pair.Value;
                var currentPosition = new Point(Canvas.GetLeft(item), Canvas.GetTop(item));
                if (Math.Abs(currentPosition.X - initialPosition.X) > double.Epsilon) Canvas.SetLeft(item, initialPosition.X);
                if (Math.Abs(currentPosition.Y - initialPosition.Y) > double.Epsilon) Canvas.SetTop(item, initialPosition.Y);
            }
        }

        protected override void OnUpdate(Vector offset)
        {
            foreach (var pair in _items)
            {
                var item = pair.Key;
                var initialPosition = pair.Value;
                var expectedPosition = initialPosition - offset;
                var currentPosition = new Point(Canvas.GetLeft(item), Canvas.GetTop(item));
                if (Math.Abs(currentPosition.X - expectedPosition.X) > double.Epsilon) Canvas.SetLeft(item, expectedPosition.X);
                if (Math.Abs(currentPosition.Y - expectedPosition.Y) > double.Epsilon) Canvas.SetTop(item, expectedPosition.Y);
            }
        }

        #endregion
    }
}