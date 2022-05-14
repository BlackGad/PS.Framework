using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PS.ComponentModel.Extensions;

namespace PS.Extensions
{
    public static class ObjectExtensions
    {
        #region Static members

        public static object ConvertNumericValueTo(this object value, Type targetType)
        {
            if (value == null) return null;
            if (targetType == null) return value;

            var valueConverter = TypeDescriptor.GetConverter(value.GetType());
            var targetConverter = TypeDescriptor.GetConverter(targetType);

            if (targetConverter.CanConvertFrom(value.GetType())) return targetConverter.ConvertFrom(value);
            if (valueConverter.CanConvertTo(targetType)) return valueConverter.ConvertTo(value, targetType);

            if (targetType.IsNullable())
            {
                var sourceTargetType = targetType.GetSourceType();
                var sourceTargetConverter = TypeDescriptor.GetConverter(sourceTargetType);

                if (sourceTargetConverter.CanConvertFrom(value.GetType())) return sourceTargetConverter.ConvertFrom(value);
                if (valueConverter.CanConvertTo(sourceTargetType)) return valueConverter.ConvertTo(value, sourceTargetType);

                return Convert.ChangeType(value, sourceTargetType);
            }

            return value;
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

        public static object UnwrapValue(this object value)
        {
            if (value is WeakReference weak) return weak.Target;
            return value;
        }

        public static object WrapValue(this object value)
        {
            var valueIsReference = value?.GetType().IsClass == true;
            return valueIsReference ? new WeakReference(value) : value;
        }

        private static IReadOnlyList<string> SplitPropertyPath(this string path)
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