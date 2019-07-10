using System;
using System.Collections.Generic;
using System.Linq;
using PS.Extensions;
using PS.Reflection;

namespace PS.MVVM.Services
{
    public class ViewResolverService : IViewResolverService
    {
        private readonly Dictionary<Type, ViewRegistrationBuilder> _registry;

        #region Constructors

        public ViewResolverService()
        {
            _registry = new Dictionary<Type, ViewRegistrationBuilder>();
        }

        #endregion

        #region IViewResolverService Members

        public IViewRegistrationBuilder Register(Type viewType)
        {
            if (viewType == null) throw new ArgumentNullException(nameof(viewType));
            return _registry.GetOrAdd(viewType, type => new ViewRegistrationBuilder(this, type));
        }

        IViewAssociation IViewResolverService.FindAssociation(Type viewModelType, object region)
        {
            if (viewModelType == null) return null;

            Tuple<ViewRegistrationBuilder, ViewAssociationBuilder, int> nearestAssociation = null;
            foreach (var registration in _registry.Values)
            {
                var associationsWithSameRegion = registration.Associations
                                                             .Where(a => Equals(a.Region, region))
                                                             .Select(a => new
                                                             {
                                                                 Association = a,
                                                                 InheritanceDepth = TypeInheritanceDepth.Measure(a.ViewModelType, viewModelType)
                                                             })
                                                             .Where(a => a.InheritanceDepth.Depth.HasValue)
                                                             .ToList();
                if (!associationsWithSameRegion.Any()) continue;

                var directAssociation = associationsWithSameRegion.FirstOrDefault(a => a.InheritanceDepth.Depth == 0);
                if (directAssociation != null)
                {
                    return new ViewAssociation
                    {
                        Region = region,
                        ViewType = registration.ViewType,
                        ViewModelType = directAssociation.Association.ViewModelType,
                        Metadata = directAssociation.Association.Metadata,
                        Depth = 0
                    };
                }

                var localNearestAssociation = associationsWithSameRegion.MinBy(s => s.InheritanceDepth.Depth);
                var localNearestAssociationDepth = localNearestAssociation.InheritanceDepth.Depth ?? int.MaxValue;
                if (nearestAssociation == null || nearestAssociation.Item3 > localNearestAssociationDepth)
                {
                    nearestAssociation = new Tuple<ViewRegistrationBuilder, ViewAssociationBuilder, int>(
                        registration,
                        localNearestAssociation.Association,
                        localNearestAssociationDepth);
                }
            }

            if (nearestAssociation != null)
            {
                return new ViewAssociation
                {
                    Region = region,
                    ViewType = nearestAssociation.Item1.ViewType,
                    ViewModelType = nearestAssociation.Item2.ViewModelType,
                    Metadata = nearestAssociation.Item2.Metadata,
                    Depth = nearestAssociation.Item3
                };
            }

            return null;
        }

        #endregion
    }
}