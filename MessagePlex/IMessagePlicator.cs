using System.Collections.Generic;

public interface IMessagePlicator<T> : IEnumerable<T>
{
    bool Enlink(T value);
    void Break();
}
