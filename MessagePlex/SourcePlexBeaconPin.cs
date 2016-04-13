using System;
using System.Threading;
using System.Threading.Tasks;

public sealed class SourcePlexBeaconPin<T> : ITaskPlexBeaconPin<T>
{
    private TaskCompletionSource<ITaskPlexBeaconPin<T>> _TCS = new TaskCompletionSource<ITaskPlexBeaconPin<T>>();
    private Action _Callback;

    public T Message { get; }

    internal SourcePlexBeaconPin(T msg, Action callback)
    {
        Message = msg;
        _Callback = callback;
    }

    public Task<ITaskPlexBeaconPin<T>> ForNext
    {
        get
        {
            if (_Callback == null)
                return _TCS.Task;

            Interlocked.Exchange(ref _Callback, null)?.Invoke();
            return _TCS.Task;
        }
    }

    public ITaskPlexBeaconPin<T> Next => ForNext.Result;
    IPlexBeaconPin<T> IPlexBeaconPin<T>.Next => Next;

    public bool HasNext => _TCS.Task.IsCompleted;

    internal void LinkWith(ITaskPlexBeaconPin<T> next)
        => _TCS.SetResult(next);
}
