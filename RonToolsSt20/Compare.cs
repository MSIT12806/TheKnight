using System;

namespace RonToolsSt20
{
    public static class Compare
    {
        public static T Best<T>(Func<T,T, T> CompareFunc, params T[] objs)
        {
            T result = objs[0];
            for (int i = 1; i < objs.Length; i++)
            {
                result = CompareFunc(objs[i], result);
            }
            return result;
        }
    }
}
