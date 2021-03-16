using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RobSharper.Ros.MessageEssentials
{
    public static class FixedSizeInitializedList
    {
        public static IList<T> Create<T>(int count)
        {
            var list = Create(typeof(T), count);
            return (IList<T>) list;
        }

        public static IList Create(Type elementType, int count)
        {
            if (elementType == null) throw new ArgumentNullException(nameof(elementType));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            if (elementType.IsAbstract || elementType.IsInterface)
                throw new InvalidOperationException("Element type must be a concrete type which can be initialized");
            
            var listType = typeof(List<>);
            listType = listType.MakeGenericType(elementType);

            var list = (IList) Activator.CreateInstance(listType, count);

            if (count == 0)
                return list;
            
            
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
                AddRepeated(list, value, count);
            }
            else
            {
                AddFresh(list, elementType, count);
            }

            return list;
        }

        private static void AddRepeated(IList list, object value, int count)
        {
            for (int i = 0; i < count; i++)
            {
                list.Add(value);
            }
        }

        private static void AddFresh(IList list, Type elementType, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var value = Activator.CreateInstance(elementType);
                list.Add(value);
            }
        }
        
        public static Type UnwrapListType<TEnumerable>()
        {
            return UnwrapListType(typeof(TEnumerable));
        }
        
        public static Type UnwrapListType(Type listType)
        {
            if (listType == null) throw new ArgumentNullException(nameof(listType));

            var elementType = listType
                .GetInterfaces()
                .Union(new []{ listType })
                .First(i => i.IsGenericType && 
                            i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .GetGenericArguments()
                .First();
            
            return elementType;
        }
    }
}