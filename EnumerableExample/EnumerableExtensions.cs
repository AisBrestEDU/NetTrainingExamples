using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EnumerableExample
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TResult> Select<TElement, TResult>(this IEnumerable<TElement> list, Func<TElement, TResult> getValue)
        {
            return new SelectEnumerable<TElement, TResult>(OurListEnumeratorState.EnumeratorNotCreated)
            {
                List = list,
                GeValue = getValue
            };
        }


        private sealed class SelectEnumerable<TElement, TResult> : IEnumerable<TResult>, IEnumerable, IEnumerator<TResult>, IEnumerator, IDisposable
        {
            public IEnumerable<TElement> List;
            public Func<TElement, TResult> GeValue;

            private OurListEnumeratorState _state;
            private TResult _current;
            private readonly int _initialThreadId;
            private IEnumerable<TElement> _list;
            private Func<TElement, TResult> _getValue;
            private IEnumerator<TElement> _enumerator;

            public SelectEnumerable(OurListEnumeratorState initState)
            {
                _state = initState;
                _initialThreadId = Environment.CurrentManagedThreadId;
            }

            public void Dispose()
            {
                _state = OurListEnumeratorState.FurtherEnumerationIsNotPossible;
                _enumerator?.Dispose();
            }

            bool IEnumerator.MoveNext()
            {
                // ISSUE: fault handler
                try
                {
                    switch (_state)
                    {
                        case OurListEnumeratorState.EnumeratorCreated:
                            _state = OurListEnumeratorState.FurtherEnumerationIsNotPossible;
                            _enumerator = _list.GetEnumerator();
                            _state = OurListEnumeratorState.ParentEnumeratorCreated;
                            break;
                        case OurListEnumeratorState.AwaitingNextMove:
                            _state = OurListEnumeratorState.ParentEnumeratorCreated;
                            break;
                        default:
                            return false;
                    }
                    if (_enumerator.MoveNext())
                    {
                        _current = _getValue(_enumerator.Current);
                        _state = OurListEnumeratorState.AwaitingNextMove;
                        return true;
                    }
                    Dispose();
                    _enumerator = (IEnumerator<TElement>)null;
                    return false;
                }
                finally
                {
                    Dispose();
                }
            }

            TResult IEnumerator<TResult>.Current => _current;

            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator.Current => _current;

            public IEnumerator<TResult> GetEnumerator()
            {
                SelectEnumerable<TElement, TResult> enumerator;
                if (_state == OurListEnumeratorState.EnumeratorNotCreated && _initialThreadId == Environment.CurrentManagedThreadId)
                {
                    _state = OurListEnumeratorState.EnumeratorCreated;
                    enumerator = this;
                }
                else
                {
                    enumerator = new SelectEnumerable<TElement, TResult>(OurListEnumeratorState.EnumeratorCreated);
                }

                enumerator._list = List;
                enumerator._getValue = GeValue;
                return enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
