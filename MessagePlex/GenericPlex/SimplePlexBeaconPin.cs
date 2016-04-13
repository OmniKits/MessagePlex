using System;

public sealed class SimplePlexBeaconPin<T> : IPlexBeaconPin<T>
{
    public T Message { get; }

    internal SimplePlexBeaconPin(T msg)
    {
        Message = msg;
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

    internal bool LinkWith(IPlexBeaconPin<T> next)
    {
        if(HasNext)
            return false;

        _Next = next;
        HasNext = true;
        return true;
    }
}
