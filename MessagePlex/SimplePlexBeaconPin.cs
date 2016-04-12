using System;

public class SimplePlexBeaconPin<T> : IPlexBeaconPin<T>
{
    public T Message { get; }

    internal SimplePlexBeaconPin(T payload)
    {
        Message = payload;
    }

    public bool HasNext { get; private set; }
    private IPlexBeaconPin<T> _Next;
    public IPlexBeaconPin<T> Next
    {
        get
        {
            if (!HasNext)
                throw new InvalidOperationException();
            return _Next;
        }
    }

    internal void LinkWith(IPlexBeaconPin<T> next)
    {
        if(HasNext)
            throw new InvalidOperationException();

        _Next = next;
        HasNext = true;
    }
}
