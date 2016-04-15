namespace MessagePlex
{
    public abstract class BasicMessagePlicator<TMsg> : BasicMessagePlicator<TMsg, BasicPlexBeaconPin<TMsg>>
    {
        sealed protected override bool Enlink(TMsg msg, bool nonBreaking)
        {
            if (IsDisposeTriggered)
                return false;

            return base.Enlink(msg, nonBreaking);
        }

        sealed protected override BasicPlexBeaconPin<TMsg> PickAPin(TMsg msg)
            => new BasicPlexBeaconPin<TMsg>(msg);

        sealed protected override bool LinkThem(BasicPlexBeaconPin<TMsg> held, BasicPlexBeaconPin<TMsg> next)
            => (held?.LinkWith(next)).GetValueOrDefault(true);
    }
}
