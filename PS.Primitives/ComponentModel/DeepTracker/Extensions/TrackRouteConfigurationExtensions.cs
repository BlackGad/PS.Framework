using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using PS.ComponentModel.DeepTracker.Filters;
using PS.ComponentModel.Navigation;

namespace PS.ComponentModel.DeepTracker.Extensions
{
    public static class TrackRouteConfigurationExtensions
    {
        #region Static members

        public static ITrackRouteConfiguration Exclude(this ITrackRouteConfiguration configuration, params Route[] routes)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configuration.Exclude(new ExcludeRoute(routes));
            return configuration;
        }

        public static ITrackRouteConfiguration Exclude<TSource>(this ITrackRouteConfiguration configuration,
                                                                params Expression<Func<TSource, object>>[] propertyExpression)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var objectProperties = propertyExpression.ToLookup(p => typeof(TSource),
                                                               p =>
                                                               {
                                                                   var body = p.Body;
                                                                   if (body is UnaryExpression unaryExpression) body = unaryExpression.Operand;

                                                                   var member = body as MemberExpression;
                                                                   if (member == null)
                                                                   {
                                                                       var message = $"Expression '{body.ToString()}' refers to a method, not a property.";
                                                                       throw new ArgumentException(message);
                                                                   }

                                                                   var propInfo = member.Member as PropertyInfo;
                                                                   if (propInfo == null)
                                                                   {
                                                                       var message = $"Expression '{body.ToString()}' refers to a field, not a property.";
                                                                       throw new ArgumentException(message);
                                                                   }

                                                                   return propInfo.Name;
                                                               });

            configuration.Exclude(new ExcludeObjectProperty(objectProperties));
            return configuration;
        }

        public static ITrackRouteConfiguration Exclude(this ITrackRouteConfiguration configuration, Func<PropertyReference, object, Route, bool> excludeFunc)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configuration.Exclude(new RelayExclude(excludeFunc));
            return configuration;
        }

        public static ITrackRouteConfiguration ExcludePropertyOfType(this ITrackRouteConfiguration configuration, params Type[] types)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configuration.Exclude(new ExcludePropertyType(types));
            return configuration;
        }

        public static ITrackRouteConfiguration ExcludeSourceWithType(this ITrackRouteConfiguration configuration, params Type[] types)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configuration.Exclude(new ExcludeSourceType(types));
            return configuration;
        }

        public static ITrackRouteConfiguration Include(this ITrackRouteConfiguration configuration, params Route[] routes)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configuration.Include(new IncludeRoute(routes));
            return configuration;
        }

        public static ITrackRouteConfiguration Include(this ITrackRouteConfiguration configuration, Func<PropertyReference, object, Route, bool> includeFunc)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configuration.Include(new RelayInclude(includeFunc));
            return configuration;
        }

        public static ITrackRouteConfiguration Subscribe<T>(this ITrackRouteConfiguration configuration, EventHandler<T> handler)
            where T : EventArgs
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configuration.Subscribe(handler);
            return configuration;
        }

        #endregion
    }
}