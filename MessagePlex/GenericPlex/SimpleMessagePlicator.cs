public abstract class SimpleMessagePlicator<TMsg> : MessagePlicatorBase<TMsg, SimplePlexBeaconPin<TMsg>>
{
    sealed protected override bool Enlink(TMsg msg, bool nonBreaking)
    {
        if (IsDisposeTriggered)
            return false;

        return base.Enlink(msg, nonBreaking);
    }

    sealed protected override SimplePlexBeaconPin<TMsg> PickAPin(TMsg msg)
        => new SimplePlexBeaconPin<TMsg>(msg);

    sealed protected override bool LinkThem(SimplePlexBeaconPin<TMsg> held, SimplePlexBeaconPin<TMsg> next)
        => (held?.LinkWith(next)).GetValueOrDefault(true);
}
