using System;
using System.Threading;
using System.Threading.Tasks;

public abstract class ReadSafeSourcePlicator<T> : MessagePlicatorBase<T, SourcePlexBeaconPin<T>>
{
    sealed public override void Break()
    {
        throw new NotSupportedException();
    }
    sealed protected override void Dispose(bool disposing)
    {
        if (disposing)
            base.Break();
    }
    sealed public override bool Enlink(T msg)
    {
        throw new NotSupportedException();
    }

    sealed protected override void LinkThem(SourcePlexBeaconPin<T> held, SourcePlexBeaconPin<T> next)
        => held?.LinkWith(next);

    protected abstract bool TryReadMessage(out T result);
    Task Read()
    => Task.Factory.StartNew(() =>
    {
        T result;
        if (TryReadMessage(out result))
            base.Enlink(result, true);
        else
            base.Break();
    });

    sealed protected override SourcePlexBeaconPin<T> PickAPin(T msg)
        => new SourcePlexBeaconPin<T>(msg, Read);
}
