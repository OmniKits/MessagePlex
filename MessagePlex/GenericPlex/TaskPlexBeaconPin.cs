using System;
using System.Threading.Tasks;

public class TaskPlexBeaconPin<T> : ITaskPlexBeaconPin<T>
{
    internal TaskCompletionSource<ITaskPlexBeaconPin<T>> _TCS = new TaskCompletionSource<ITaskPlexBeaconPin<T>>();

    public T Message { get; }

    internal TaskPlexBeaconPin(T msg)
    {
        Message = msg;
    }

    public virtual Task<ITaskPlexBeaconPin<T>> ForNext => _TCS.Task;

    public bool HasNext => _TCS.Task.IsCompleted;

    public ITaskPlexBeaconPin<T> Next => ForNext.Result;
    IPlexBeaconPin<T> IPlexBeaconPin<T>.Next => Next;

    internal bool LinkWith(ITaskPlexBeaconPin<T> next)
        => _TCS.TrySetResult(next);
}
