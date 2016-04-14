using System;
using System.Threading;
using System.Threading.Tasks;

using Setup = System.Func<System.Threading.Tasks.Task>;

public sealed class TaskSourcePlexBeaconPin<T> : TaskPlexBeaconPin<T>
{
    private Setup _Setup;

    internal TaskSourcePlexBeaconPin(T msg, Setup setup)
        : base(msg)
    {
        _Setup = setup;
    }

    public override Task<ITaskPlexBeaconPin<T>> ForNext
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
}
