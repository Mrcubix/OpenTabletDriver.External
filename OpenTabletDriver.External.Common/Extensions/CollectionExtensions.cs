using System.Collections.Generic;

namespace OpenTabletDriver.External.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static void Replace<T>(this ICollection<T> list, T oldItem, T newItem)
        {
            list.Remove(oldItem);
            list.Add(newItem);
        }
    }
}