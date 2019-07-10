using System;
using System.Linq;
using PS.Extensions;

namespace PS.Reflection
{
    public class TypeInheritanceDepth
    {
        #region Static members

        public static TypeInheritanceDepth Measure(Type sourceType, Type inheritedType)
        {
            int? weight = null;

            if (sourceType.IsAssignableFrom(inheritedType))
            {
                var sourceTypeHierarchy = inheritedType.Traverse(type => type.BaseType).ToList();
                for (var index = sourceTypeHierarchy.Count - 1; index >= 0; index--)
                {
                    var type = sourceTypeHierarchy[index];
                    if (sourceType.IsAssignableFrom(type))
                    {
                        weight = index;
                        break;
                    }
                }
            }

            return new TypeInheritanceDepth(sourceType, inheritedType, weight);
        }

        #endregion

        #region Constructors

        private TypeInheritanceDepth(Type sourceType, Type inheritedType, int? depth)
        {
            SourceType = sourceType;
            InheritedType = inheritedType;
            Depth = depth;
        }

        #endregion

        #region Properties

        public int? Depth { get; }

        public Type InheritedType { get; }

        public Type SourceType { get; }

        #endregion
    }
}