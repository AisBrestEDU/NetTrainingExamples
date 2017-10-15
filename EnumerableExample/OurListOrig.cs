using System.Collections;
using System.Collections.Generic;

namespace EnumerableExample
{
   public class OurListOrig<T> : IEnumerable<T>
   {
       private readonly IEnumerable<T> _list;

       public OurListOrig(IEnumerable<T> list)
       {
           _list = list;
       }

       public IEnumerator<T> GetEnumerator()
       {
           foreach (var item in _list)
           {
               yield return item;
           }

       }

       IEnumerator IEnumerable.GetEnumerator()
       {
           return GetEnumerator();
       }
   }
}
