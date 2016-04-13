using System;
using System.Threading;
using System.Threading.Tasks;

using Setup = System.Action;

public sealed class SourcePlexBeaconPin<T> : IPlexBeaconPin<T>
{
    private TaskCompletionSource<IPlexBeaconPin<T>> _TCS = new TaskCompletionSource<IPlexBeaconPin<T>>();
    private Setup _Setup;

    public T Message { get; }

    internal SourcePlexBeaconPin(T msg, Setup setup)
    {
        Message = msg;
        _Setup = setup;
    }

    public IPlexBeaconPin<T> Next
    {
        get
        {
            if (_Setup == null)
                return _TCS.Task.Result;

            try
            {
                Interlocked.Exchange(ref _Setup, null)?.Invoke();
            }
            catch (Exception ex)
            {
                _TCS.SetException(ex);
            }

            return _TCS.Task.Result;
        }
    }

    public bool HasNext => _TCS.Task.IsCompleted;

    internal bool LinkWith(IPlexBeaconPin<T> next)
        => _TCS.TrySetResult(next);
}
