using System;
using System.Collections.Generic;

public interface IMessagePlicator<T> : IEnumerable<T>, IDisposable
{
    bool Enlink(T value);
    void Break();
}
