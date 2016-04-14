using System;
using System.Threading;
using System.Threading.Tasks;

public abstract class TaskSourcePlicator<T> : MessagePlicatorBase<T, TaskSourcePlexBeaconPin<T>>
{
    sealed public override bool Break() => false;
    sealed protected override void Dispose(bool disposing)
    {
        if (disposing)
            base.Break();
    }
    sealed public override bool Enlink(T msg) => false;

    sealed protected override bool LinkThem(TaskSourcePlexBeaconPin<T> held, TaskSourcePlexBeaconPin<T> next)
        => (held?.LinkWith(next)).GetValueOrDefault(true);

    protected abstract bool TryReadMessage(out T result);
    Task Read()
    => Task.Factory.StartNew(() =>
    {
        T result;
        if (TryReadMessage(out result))
            base.Enlink(result, true);
        else
            base.Break();
    }, TaskCreationOptions.LongRunning);

    sealed protected override TaskSourcePlexBeaconPin<T> PickAPin(T msg)
        => new TaskSourcePlexBeaconPin<T>(msg, Read);
}
