using System.Collections.Generic;

namespace Extensions
{
    public static class CollectionExtensions
    {
        // https://stackoverflow.com/questions/273313/randomize-a-listt
        public static void Shuffle<T>(this IList<T> list, System.Random random)
        {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}