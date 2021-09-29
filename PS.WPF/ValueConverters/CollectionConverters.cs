using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using PS.Extensions;
using PS.Patterns.Aware;

namespace PS.WPF.ValueConverters
{
    public static class CollectionConverters
    {
        #region Constants

        public static readonly RelayValueConverter Any;
        public static readonly RelayMultiValueConverter Contain;
        public static readonly RelayValueConverter Count;
        public static readonly RelayMultiValueConverter DoesNotContain;
        public static readonly RelayMultiValueConverter FirstOrDefault;
        public static readonly RelayValueConverter Group;
        public static readonly RelayValueConverter GroupAndSort;
        public static readonly RelayValueConverter Sort;

        #endregion

        #region Static members

        private static object CreateView(object value, IEnumerable parameter)
        {
            var source = new CollectionViewSource
            {
                Source = value
            };
            var parametersList = new List<object>();
            if (parameter is IEnumerable enumerableParameter)
            {
                parametersList.AddRange(enumerableParameter);
            }
            else if (parameter != null)
            {
                parametersList.Add(parameter);
            }

            var sortDescriptions = parametersList.Enumerate<SortDescription>().ToList();
            if (sortDescriptions.Any()) source.IsLiveSortingRequested = true;

            foreach (var description in sortDescriptions)
            {
                source.SortDescriptions.Add(description);
            }

            var groupDescriptions = parametersList.Enumerate<GroupDescription>().ToList();
            if (groupDescriptions.Any()) source.IsLiveGroupingRequested = true;

            foreach (var description in groupDescriptions)
            {
                source.GroupDescriptions.Add(description);
            }

            return source.View;
        }

        #endregion

        #region Constructors

        static CollectionConverters()
        {
            Sort = new RelayValueConverter((value, targetType, parameter, culture) => CreateView(value, parameter.Enumerate<SortDescription>()));
            Group = new RelayValueConverter((value, targetType, parameter, culture) => CreateView(value, parameter.Enumerate<GroupDescription>()));
            GroupAndSort = new RelayValueConverter((value, targetType, parameter, culture) => CreateView(value, parameter.Enumerate()));

            Any = new RelayValueConverter((value, targetType, parameter, culture) => value.Enumerate().Any());
            Count = new RelayValueConverter((value, targetType, parameter, culture) => value.Enumerate().Count());
            Contain = new RelayMultiValueConverter((values, targetType, parameter, culture) =>
            {
                if (values.Length < 2) return false;

                var components = values[0].Enumerate().ToList();
                var element = values[1];
                var treatNullElementNameAsPresentInCollection = parameter is bool boolParameter && boolParameter;
                if (treatNullElementNameAsPresentInCollection && element == null) return true;

                return components.Contains(element);
            });
            DoesNotContain = new RelayMultiValueConverter((values, targetType, parameter, culture) =>
                                                              Contain.Convert(values, targetType, parameter, culture).AreEqual(false));
            FirstOrDefault = new RelayMultiValueConverter((values, targetType, parameter, culture) =>
            {
                if (values.Length < 2) return false;

                var components = values[0].Enumerate().ToList();
                var element = values[1];

                var result = components.FirstOrDefault(e => e.AreEqual(element)) ??
                             components.Enumerate<IIDAware>().FirstOrDefault(c => c.Id.AreEqual(element));

                if (result == null && element is IIDAware idAwareElement)
                {
                    result = components.Enumerate<IIDAware>().FirstOrDefault(c => c.Id.AreEqual(idAwareElement.Id));
                }

                return result;
            });
        }

        #endregion
    }
}