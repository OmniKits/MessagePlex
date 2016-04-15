namespace MessagePlex
{
    public abstract class ConcurrentMessagePlicator<TMsg> : ConcurrentMessagePlicatorBase<TMsg, BlockingPlexBeaconPin<TMsg>>
    {
        sealed public override bool Break() => base.Break();
        sealed protected override void Dispose(bool disposing)
            => base.Dispose(disposing);
        sealed protected override BlockingPlexBeaconPin<TMsg> PickAPin(TMsg msg)
            => new BlockingPlexBeaconPin<TMsg>(msg);

        sealed protected override bool LinkThem(BlockingPlexBeaconPin<TMsg> held, BlockingPlexBeaconPin<TMsg> next)
            => (held?.LinkWith(next)).GetValueOrDefault(true);
    }
}
