using System;
using System.Collections;
using System.Collections.Generic;

sealed class DoppleGanger<T> : IEnumerator<T>
{
    IPlexBeaconPin<T> _Held;

    public DoppleGanger(IPlexBeaconPin<T> held)
    {
        _Held = held;
    }

    public T Current
    {
        get
        {
            try
            {
                // if MoveNext() is never called, then Current is undefined, 
                // it's acceptable that it has an unintentional value, include default(T)
                return _Held.Message;
            }
            catch (NullReferenceException)
            {
                // if this happens, the last call of MoveNext() must have returned false
                // then we need to replace NullReferenceException with InvalidOperationException
                throw new InvalidOperationException();
            }
        }
    }
    object IEnumerator.Current => Current;

    public bool MoveNext()
        => (_Held = _Held?.Next) != null;

    public void Reset()
    {
        throw new NotImplementedException();
    }
    public void Dispose() { }
}
