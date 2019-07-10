using System;
using System.Collections.Generic;
using System.Linq;

namespace PS.MVVM.Services
{
    internal class ViewRegistrationBuilder : IViewRegistrationBuilder
    {
        private readonly IViewResolverService _registryService;

        #region Constructors

        public ViewRegistrationBuilder(IViewResolverService registryService, Type viewType)
        {
            _registryService = registryService;
            ViewType = viewType;
            Associations = new List<ViewAssociationBuilder>();
        }

        #endregion

        #region Properties

        public IList<ViewAssociationBuilder> Associations { get; }

        public Type ViewType { get; }

        #endregion

        #region IViewRegistrationBuilder Members

        public IViewAssociationBuilder Associate(Type viewModelType, object region = null)
        {
            var existingAssociation = _registryService.FindAssociation(viewModelType, region);
            if (existingAssociation?.Depth == 0)
            {
                var message = $"'{viewModelType.Name}' ";
                if (region != null) message += $"with '{region}' ";
                message += $" is already associated with '{existingAssociation.ViewType.Namespace}' view";
                throw new InvalidOperationException(message);
            }

            var association = new ViewAssociationBuilder(viewModelType, region);
            if (Associations.Any(a => a.ViewModelType == viewModelType && Equals(a.Region, region)))
            {
                throw new InvalidOperationException("Association already exist");
            }

            Associations.Add(association);
            return association;
        }

        #endregion
    }
}