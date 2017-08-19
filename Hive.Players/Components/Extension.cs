using System;
using System.Collections.Generic;

namespace Hive.Players.Components
{
    public static class Extension
    {
        public static T PopAt<T>(this List<T> list, int index)
        {
            T r = list[index];
            list.RemoveAt(index);
            return r;
        }
    }
}
