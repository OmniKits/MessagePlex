namespace MessagePlex
{
    public abstract class BlockingSourcePlicator<T> : SimpleMessagePlicatorBase<T, BlockingSourcePlexBeaconPin<T>>
    {
        sealed public override bool Break() => false;
        sealed protected override void Dispose(bool disposing)
        {
            if (disposing)
                base.Break();
        }
        sealed public override bool Enlink(T msg) => false;

        sealed protected override bool LinkThem(BlockingSourcePlexBeaconPin<T> held, BlockingSourcePlexBeaconPin<T> next)
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

        sealed protected override BlockingSourcePlexBeaconPin<T> PickAPin(T msg)
            => new BlockingSourcePlexBeaconPin<T>(msg, Read);
    }
}
