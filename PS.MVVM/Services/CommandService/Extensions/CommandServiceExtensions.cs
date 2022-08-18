using System;
using System.Collections.Generic;
using System.Linq;
using PS.Extensions;

namespace PS.MVVM.Services.CommandService.Extensions
{
    public static class CommandServiceExtensions
    {
        public static T Find<T>(this ICommandService service, object identifier)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            return service.Find(identifier).Enumerate<T>().FirstOrDefault();
        }

        public static IEnumerable<T> FindAll<T>(this ICommandService service, object identifier)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            return service.Find(identifier).Enumerate<T>();
        }

        public static IEnumerable<T> FindAll<T>(this ICommandService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            return service.Find(null).Enumerate<T>();
        }
    }
}
