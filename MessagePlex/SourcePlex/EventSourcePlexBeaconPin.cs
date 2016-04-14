using System;
using System.Threading;
using System.Threading.Tasks;

using Setup = System.Action;

public sealed class EventSourcePlexBeaconPin<T> : IPlexBeaconPin<T>
{
    private volatile ManualResetEvent _MRE = new ManualResetEvent(false);
    private Setup _Setup;

    public T Message { get; }

    internal EventSourcePlexBeaconPin(T msg, Setup setup)
    {
        Message = msg;
        _Setup = setup;
    }

    public bool HasNext => _MRE == null;

    private IPlexBeaconPin<T> _Next;
    public IPlexBeaconPin<T> Next
    {
        get
        {
            if (_Setup != null)
                Interlocked.Exchange(ref _Setup, null)?.Invoke();

            _MRE?.WaitOne();

            return _Next;
        }
    }

    internal bool LinkWith(IPlexBeaconPin<T> next)
    {
        if (HasNext)
            return false;

        lock (this)
        {
            var mre = _MRE;
            if (mre == null)
                return false;

            _Next = next;
            mre.Set();
            _MRE = null;
        }

        return true;
    }
}
