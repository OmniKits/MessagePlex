public abstract class SimpleMessagePlicator<TMsg> : MessagePlicatorBase<TMsg, SimplePlexBeaconPin<TMsg>>
{
    protected override bool Enlink(TMsg msg, bool nonBreaking)
    {
        if (IsDisposeTriggered)
            return false;

        return base.Enlink(msg, nonBreaking);
    }

    protected override SimplePlexBeaconPin<TMsg> PickAPin(TMsg msg)
        => new SimplePlexBeaconPin<TMsg>(msg);

    protected override void LinkThem(SimplePlexBeaconPin<TMsg> held, SimplePlexBeaconPin<TMsg> next)
        => held?.LinkWith(next);
}
