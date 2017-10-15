using System;
using System.Collections;
using System.Collections.Generic;

namespace EnumerableExample
{
    public class OurList<T> : IEnumerable<T>, IEnumerable
    {
        private readonly IEnumerable<T> _list;

        public OurList(IEnumerable<T> list)
        {
            _list = list;
        }


        public IEnumerator<T> GetEnumerator()
        {
            var enumerator = new OurEnumerator(OurListEnumeratorState.EnumeratorCreated)
            {
                Parent = this
            };
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        private sealed class OurEnumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            private OurListEnumeratorState _state;
            private T _current;
            public OurList<T> Parent;
            private IEnumerator<T> _enumerator;
            
            public OurEnumerator(OurListEnumeratorState state)
            {
                _state = state;
            }


            public void Dispose()
            {
                _state = OurListEnumeratorState.FurtherEnumerationIsNotPossible;
                _enumerator?.Dispose();
            }

            bool IEnumerator.MoveNext()
            {
                try
                {
                    switch (_state)
                    {
                        case 0:
                            _state = OurListEnumeratorState.FurtherEnumerationIsNotPossible;
                            _enumerator = Parent._list.GetEnumerator();
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
                        _current = _enumerator.Current;
                        _state = OurListEnumeratorState.AwaitingNextMove;
                        return true;
                    }
                    Dispose();
                    _enumerator = null;
                    return false;
                }
                finally
                {
                    Dispose();
                }
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            T IEnumerator<T>.Current => _current;
            object IEnumerator.Current => _current;
        }
    }
}
