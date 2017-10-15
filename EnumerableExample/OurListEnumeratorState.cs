using System;
using System.Collections.Generic;
using System.Text;

namespace EnumerableExample
{
    public enum OurListEnumeratorState
    {
        EnumeratorCreated = 0,
        ParentEnumeratorCreated = -3,
        AwaitingNextMove = 1,
        FurtherEnumerationIsNotPossible = -1,
        EnumeratorNotCreated = -2

    }
}
