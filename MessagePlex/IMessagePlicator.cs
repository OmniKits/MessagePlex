using System;
using System.Collections.Generic;

public interface IMessagePlicator<T> : IEnumerable<T>, IDisposable
{
    new IDoppelGanger<T> GetEnumerator();
    bool OnNext(T value);
    bool OnCompleted();
}
