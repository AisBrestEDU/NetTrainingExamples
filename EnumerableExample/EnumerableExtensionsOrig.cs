using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnumerableExample
{
    public static class EnumerableExtensionsOrig
    {
        public static IEnumerable<TResult> Select<TElement, TResult>(this IEnumerable<TElement> list, Func<TElement, TResult> getValue)
        {
            foreach (var item in list)
            {
                yield return getValue(item);
            }
        }

        public static IEnumerable<int> GetRange<TElement, TResult>()
        {
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
            yield return 5;

            if (DateTime.UtcNow.Hour > 6)
            {
                throw new Exception();
            }
            if(DateTime.UtcNow.Hour > 7)
            {
                yield break;
            }

            yield return 6;
        }
    }
}
