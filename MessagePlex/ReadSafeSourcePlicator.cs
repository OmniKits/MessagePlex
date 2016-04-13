using System;

public abstract class ReadSafeSourcePlicator<T> : MessagePlicatorBase<T, SourcePlexBeaconPin<T>>
{
    sealed public override bool Enlink(T msg)
    {
        throw new NotSupportedException();
    }

    sealed protected override void LinkThem(SourcePlexBeaconPin<T> held, SourcePlexBeaconPin<T> next)
        => held?.LinkWith(next);

    sealed protected override SourcePlexBeaconPin<T> PickAPin(T msg)
        => new SourcePlexBeaconPin<T>(msg, FeedMe);

    protected abstract void FeedMe();
}
