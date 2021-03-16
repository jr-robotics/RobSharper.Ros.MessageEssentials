using System;
using System.Collections.Generic;

namespace RobSharper.Ros.MessageEssentials
{
    public static class CollectionPopulationExtensions
    {
        public static void PopulateWithInitializedRosValues<TElement>(this ICollection<TElement> collection, int count)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            if (collection.Count > 0)
                throw new InvalidOperationException("Only empty collections can be populated");
            
            if (count == 0)
                return;

            var elementType = typeof(TElement);

            object value = null;

            if (elementType.IsPrimitive)
            {
                value = Activator.CreateInstance(elementType);
            }
            else if (elementType == typeof(string))
            {
                value = string.Empty;
            }
            else if (elementType == typeof(DateTime))
            {
                value = RosTime.Zero.DateTime;
            }

            if (value != null)
            {
                AddRepeated(collection, (TElement) value, count);
            }
            else
            {
                AddFresh(collection, count);
            }
        }

        private static void AddRepeated<T>(ICollection<T> collection, T value, int count)
        {
            for (int i = 0; i < count; i++)
            {
                collection.Add(value);
            }
        }

        private static void AddFresh<T>(ICollection<T> collection, int count)
        {
            var elementType = typeof(T);
            
            for (int i = 0; i < count; i++)
            {
                var value = (T) Activator.CreateInstance(elementType);
                collection.Add(value);
            }
        }
    }
}