//using System.Collections.Generic;
//using System.Linq;
//using System.Windows;
//using PS.Shell.Module.Diagram.Controls.MVVM;

//namespace PS.Shell.Module.Diagram.Controls
//{
//    public class SelectorService
//    {
//        private readonly List<UIElement> _selectedItems;

//        #region Constructors

//        public SelectorService()
//        {
//            _selectedItems = new List<UIElement>();
//        }

//        #endregion

//        #region Properties

//        public IReadOnlyList<UIElement> SelectedItems
//        {
//            get { return _selectedItems; }
//        }

//        #endregion

//        #region Members

//        public void Add(UIElement element)
//        {
//            if (_selectedItems.Contains(element)) return;
//            _selectedItems.Add(element);

//            if (element is Node node && node.Data is INode data)
//            {
//                data.Visual.IsSelected = true;
//            }
//        }

//        public void Clear()
//        {
//            var itemsToRemove = _selectedItems.ToList();
//            foreach (var item in itemsToRemove)
//            {
//                Remove(item);
//            }
//        }

//        public void Remove(UIElement element)
//        {
//            if (_selectedItems.Remove(element) && element is Node node && node.Data is INode data)
//            {
//                data.Visual.IsSelected = false;
//            }
//        }

//        public void Set(UIElement element)
//        {
//            if (_selectedItems.Contains(element))
//            {
//                var itemsToRemove = _selectedItems.Except(new[] { element }).ToList();
//                foreach (var item in itemsToRemove)
//                {
//                    Remove(item);
//                }
//            }
//            else
//            {
//                Clear();
//                Add(element);
//            }
//        }

//        #endregion
//    }
//}