using System;
using System.Collections.Generic;

namespace PS.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
                                                       TKey key,
                                                       Func<TKey, TValue> addFactory,
                                                       Func<TKey, TValue, TValue> updateFactory)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (addFactory == null) throw new ArgumentNullException(nameof(addFactory));
            if (updateFactory == null) throw new ArgumentNullException(nameof(updateFactory));
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, addFactory(key));
            }
            else
            {
                dictionary[key] = updateFactory(key, dictionary[key]);
            }

            return dictionary[key];
        }

        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
                                                       TKey key,
                                                       Func<TKey, TValue> factory)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, factory(key));
            }
            else
            {
                dictionary[key] = factory(key);
            }

            return dictionary[key];
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> addFactory = null)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            addFactory = addFactory ?? (k => Activator.CreateInstance<TValue>());
            if (!dictionary.ContainsKey(key)) dictionary.Add(key, addFactory(key));
            return dictionary[key];
        }
    }
}
