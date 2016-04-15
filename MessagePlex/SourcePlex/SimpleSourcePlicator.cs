namespace MessagePlex
{
    public abstract class SimpleSourcePlicator<T> : SimpleMessagePlicatorBase<T, SimpleSourcePlexBeaconPin<T>>
    {
        sealed public override bool Break() => false;
        sealed protected override void Dispose(bool disposing)
        {
            if (disposing)
                base.Break();
        }
        sealed public override bool Enlink(T msg) => false;

        sealed protected override bool LinkThem(SimpleSourcePlexBeaconPin<T> held, SimpleSourcePlexBeaconPin<T> next)
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

        sealed protected override SimpleSourcePlexBeaconPin<T> PickAPin(T msg)
            => new SimpleSourcePlexBeaconPin<T>(msg, Read);
    }
}
