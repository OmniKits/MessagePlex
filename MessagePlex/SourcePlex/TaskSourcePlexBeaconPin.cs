using System;
using System.Threading;
using System.Threading.Tasks;

using Setup = System.Func<System.Threading.Tasks.Task>;

public sealed class TaskSourcePlexBeaconPin<T> : ITaskPlexBeaconPin<T>
{
    private TaskCompletionSource<ITaskPlexBeaconPin<T>> _TCS = new TaskCompletionSource<ITaskPlexBeaconPin<T>>();
    private Setup _Setup;

    public T Message { get; }

    internal TaskSourcePlexBeaconPin(T msg, Setup setup)
    {
        Message = msg;
        _Setup = setup;
    }

    public Task<ITaskPlexBeaconPin<T>> ForNext
    {
        get
        {
            if (_Setup == null)
                return _TCS.Task;

            Interlocked.Exchange(ref _Setup, null)?.Invoke()
            .ContinueWith(task =>
            {
                switch (task.Status)
                {
                    case TaskStatus.RanToCompletion:
                        return;
                    case TaskStatus.Canceled:
                        _TCS.SetCanceled();
                        return;
                }

                var ex = task.Exception;
                _TCS.SetException(ex == null ? new Exception() : ex.InnerExceptions.Count == 1 ? ex.InnerException : ex);
            });

            return _TCS.Task;
        }
    }

    public ITaskPlexBeaconPin<T> Next => ForNext.Result;
    IPlexBeaconPin<T> IPlexBeaconPin<T>.Next => Next;

    public bool HasNext => _TCS.Task.IsCompleted;

    internal bool LinkWith(ITaskPlexBeaconPin<T> next)
    {
        if (next == null) // breaking should not raise error at all
            return _TCS.TrySetResult(next);
        _TCS.SetResult(next);
        return true;
    }
}
