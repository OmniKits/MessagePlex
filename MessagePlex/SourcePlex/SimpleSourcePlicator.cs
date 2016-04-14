using System;
using System.Threading;
using System.Threading.Tasks;

public abstract class SimpleSourcePlicator<T> : MessagePlicatorBase<T, EventSourcePlexBeaconPin<T>>
{
    sealed public override bool Break() => false;
    sealed protected override void Dispose(bool disposing)
    {
        if (disposing)
            base.Break();
    }
    sealed public override bool Enlink(T msg) => false;

    sealed protected override bool LinkThem(EventSourcePlexBeaconPin<T> held, EventSourcePlexBeaconPin<T> next)
        => (held?.LinkWith(next)).GetValueOrDefault(true);

    protected abstract bool TryReadMessage(out T result);
    void Read()
    {
        T result;
        if (TryReadMessage(out result))
            base.Enlink(result, true);
        else
            base.Break();
    }

    sealed protected override EventSourcePlexBeaconPin<T> PickAPin(T msg)
        => new EventSourcePlexBeaconPin<T>(msg, Read);
}
