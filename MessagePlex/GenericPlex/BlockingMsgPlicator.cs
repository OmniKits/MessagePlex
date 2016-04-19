namespace MessagePlex
{
    public abstract class BlockingMsgPlicator<TMsg> : NaiveMessagePlicatorBase<TMsg, BlockingPlexBeaconPin<TMsg>>
    {
        sealed public override bool Break() => base.Break();
        sealed protected override void Dispose(bool disposing)
            => base.Dispose(disposing);
        sealed protected override BlockingPlexBeaconPin<TMsg> OnSpawnPin(TMsg msg)
            => new BlockingPlexBeaconPin<TMsg>(msg);

        sealed protected override bool OnLink(BlockingPlexBeaconPin<TMsg> held, BlockingPlexBeaconPin<TMsg> next)
            => (held?.LinkWith(next)).GetValueOrDefault(true);
    }
}
