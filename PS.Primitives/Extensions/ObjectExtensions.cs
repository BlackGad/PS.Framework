﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using PS.ComponentModel.Extensions;

namespace PS.Extensions
{
    public static class ObjectExtensions
    {
        #region Static members

        public static bool AreEqual(this object source, object target)
        {
            if (ReferenceEquals(source, target)) return true;
            if (source == null && target == null) return true;
            if (source == null) return false;
            if (target == null) return false;

            if (source is XNode sourceNode && target is XNode targetNode)
            {
                return XNode.DeepEquals(sourceNode, targetNode);
            }

            var sourceType = source.GetType();
            var targetType = target.GetType();

            if (sourceType != targetType)
            {
                if (sourceType.IsNumeric() && targetType.IsNumeric()) return (dynamic)source == (dynamic)target;

                return false;
            }

            var equatableInterface = sourceType.GetTypeInfo()
                                               .ImplementedInterfaces
                                               .Where(i => i.IsGenericType)
                                               .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEquatable<>));
            if (equatableInterface != null && equatableInterface.GetGenericArguments().Single().IsAssignableFrom(targetType))
            {
                var equalMethod = equatableInterface.GetMethods().Single();
                return (bool)equalMethod.Invoke(source, new[] { target });
            }

            var equalityComparerInterface = sourceType.GetTypeInfo()
                                                      .ImplementedInterfaces
                                                      .Where(i => i.IsGenericType)
                                                      .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEqualityComparer<>));
            if (equalityComparerInterface != null && equalityComparerInterface.GetGenericArguments().Single().IsAssignableFrom(targetType))
            {
                var equalsMethod = equalityComparerInterface.GetMethods().Single(m => Equals(m.Name, nameof(Equals)));
                return (bool)equalsMethod.Invoke(source, new[] { source, target });
            }

            return Equals(source, target);
        }

        public static string GetEffectiveString(this object item)
        {
            var result = string.Empty;

            if (item != null)
            {
                var valueType = item.GetType();
                result = TypeDescriptor.GetConverter(valueType).ConvertToString(item);
            }

            return result;
        }

        public static object GetEffectiveValue(this object item, string propertyPath)
        {
            if (item == null) return null;

            var parts = SplitPropertyPath(propertyPath);
            if (!parts.Any()) return item;

            var currentItem = item;
            var currentItemType = currentItem.GetType();

            foreach (var part in parts)
            {
                if (currentItem == null) return null;

                var descriptor = currentItemType.GetPropertyDescriptors().FirstOrDefault(d => d.Name.AreEqual(part));
                if (descriptor == null) return null;

                currentItem = descriptor.GetValue(currentItem);
                currentItemType = currentItem?.GetType();
            }

            return currentItem;
        }

        public static int GetHash(this object instance)
        {
            return instance?.GetHashCode() ?? 0;
        }

        public static int MergeHash(this int hash, int addHash)
        {
            return (hash * 397) ^ addHash;
        }

        public static IReadOnlyList<string> SplitPropertyPath(this string path)
        {
            path = path ?? string.Empty;
            path = "(" + path + ")";

            var parenthesesLevel = 0;
            var currentPosition = 0;
            var openBracketPositionStack = new Stack<int>();
            var braceRanges = new List<Tuple<int, int, int>>();

            while (currentPosition < path.Length)
            {
                var openBraceIndex = path.IndexOf('(', currentPosition);
                var closeBraceIndex = path.IndexOf(')', currentPosition);

                if (openBraceIndex != -1 && closeBraceIndex != -1)
                {
                    if (openBraceIndex < closeBraceIndex)
                    {
                        parenthesesLevel++;
                        openBracketPositionStack.Push(openBraceIndex);
                    }
                    else
                    {
                        if (openBracketPositionStack.TryPop(out var lastOpenBrace))
                        {
                            braceRanges.Add(new Tuple<int, int, int>(parenthesesLevel, lastOpenBrace, closeBraceIndex + 1));
                        }

                        parenthesesLevel--;
                    }

                    currentPosition = Math.Min(openBraceIndex, closeBraceIndex) + 1;
                }
                else if (openBraceIndex != -1)
                {
                    parenthesesLevel++;
                    currentPosition = openBraceIndex + 1;
                }
                else if (closeBraceIndex != -1)
                {
                    if (openBracketPositionStack.TryPop(out var lastOpenBrace))
                    {
                        braceRanges.Add(new Tuple<int, int, int>(parenthesesLevel, lastOpenBrace, closeBraceIndex + 1));
                    }

                    parenthesesLevel--;
                    currentPosition = closeBraceIndex + 1;
                }
                else
                {
                    break;
                }
            }

            var replaceMap = new Dictionary<string, string>();
            foreach (var range in braceRanges.Where(t => t.Item1 == 2))
            {
                var replacement = Guid.NewGuid().ToString("N");
                replaceMap.Add(replacement, path.Substring(range.Item2, range.Item3 - range.Item2));
                path = path.Substring(0, range.Item2) + replacement + path.Substring(range.Item3, path.Length - range.Item3);
            }

            var splitParts = path.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            for (var index = 0; index < splitParts.Length; index++)
            {
                splitParts[index] = replaceMap.Aggregate(splitParts[index], (agg, replacement) => agg.Replace(replacement.Key, replacement.Value));
                if (splitParts[index].StartsWith("(")) splitParts[index] = splitParts[index].Substring(1);
                if (splitParts[index].EndsWith(")")) splitParts[index] = splitParts[index].Substring(0, splitParts[index].Length - 1);
            }

            return splitParts.Where(p => !string.IsNullOrEmpty(p)).ToArray();
        }

        #endregion
    }
}