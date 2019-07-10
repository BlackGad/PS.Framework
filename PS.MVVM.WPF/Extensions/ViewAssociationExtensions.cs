using System.Windows;
using System.Windows.Data;
using PS.MVVM.Services;

namespace PS.MVVM.Extensions
{
    public static class ViewAssociationExtensions
    {
        #region Static members

        public static Style GetContainerStyle(this IViewAssociation association)
        {
            association.Metadata.TryGetValue("ContainerStyle", out var result);
            return result as Style;
        }

        public static Binding GetHierarchyBinding(this IViewAssociation association)
        {
            association.Metadata.TryGetValue("HierarchyBinding", out var result);
            return result as Binding;
        }

        public static IViewAssociationBuilder SetContainerStyle(this IViewAssociationBuilder builder, Style style)
        {
            builder?.SetMetadata("ContainerStyle", style);
            return builder;
        }

        public static IViewAssociationBuilder SetHierarchyBinding(this IViewAssociationBuilder builder, Binding binding)
        {
            builder?.SetMetadata("HierarchyBinding", binding);
            return builder;
        }

        #endregion
    }
}