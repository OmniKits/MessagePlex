using System;
using System.Threading.Tasks;

public sealed class TaskPlexBeaconPin<T> : ITaskPlexBeaconPin<T>
{
    private TaskCompletionSource<ITaskPlexBeaconPin<T>> _TCS;

    public Task<ITaskPlexBeaconPin<T>> ForNext => _TCS.Task;

    public T Message { get; }
    internal TaskPlexBeaconPin(T msg)
    {
        _TCS = new TaskCompletionSource<ITaskPlexBeaconPin<T>>();

        Message = msg;
    }

    public bool HasNext => ForNext.IsCompleted;
    public ITaskPlexBeaconPin<T> Next => ForNext.Result;
    IPlexBeaconPin<T> IPlexBeaconPin<T>.Next => Next;

    internal bool LinkWith(ITaskPlexBeaconPin<T> next)
        => _TCS.TrySetResult(next);
}
