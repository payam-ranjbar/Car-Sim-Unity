using System.Linq;

namespace TrafficSystem
{
    public static class Extensions
    {
        public static T[] Concatenate<T>(this T[] first, T[] second)
        {
            if (first == null) {
                return second;
            }
            if (second == null) {
                return first;
            }

            return first.Concat(second).ToArray();
        }
    }
}