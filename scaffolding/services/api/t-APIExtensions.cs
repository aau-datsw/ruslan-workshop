using System.Collections.Generic;
using System.Linq;

namespace {api_name}
{
    public static class {api_name}Extensions
    {
        // Zips two arrays together to a list of pairs.
        public static List<(T, U)> Zip<T, U>(this IEnumerable<T> u, IEnumerable<U> v)
        {
            var zipped = new List<(T, U)>();
            var arrU = u.ToArray();
            var arrV = v.ToArray();
            
            for (int i = 0; i < u.Count(); i++)
                zipped.Add((arrU[i], arrV[i]));
            return zipped;
        }

        public static List<int> Range(this int start, int length)
        {
            return Enumerable.Range(start, length).ToList();
        }
    }
}