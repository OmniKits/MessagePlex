using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Setup = System.Func<System.Threading.Tasks.Task>;

public sealed class SourcePlexBeaconPin<T> : ITaskPlexBeaconPin<T>
{
    private TaskCompletionSource<ITaskPlexBeaconPin<T>> _TCS = new TaskCompletionSource<ITaskPlexBeaconPin<T>>();
    private Setup _Setup;

    public T Message { get; }

    internal SourcePlexBeaconPin(T msg, Setup setup)
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
                if (!task.IsCompleted)
                    Debugger.Break();

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

    // TODO: find out why fail here
    internal void LinkWith(ITaskPlexBeaconPin<T> next)
        => _TCS.TrySetResult(next);
}
