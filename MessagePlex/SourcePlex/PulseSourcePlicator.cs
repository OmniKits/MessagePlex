namespace MessagePlex
{
    public abstract class PulseSourcePlicator<T> : MessagePlicatorBase<T, PulseSourcePlexBeaconPin<T>>
    {
        sealed public override bool Break() => false;
        sealed protected override void Dispose(bool disposing)
        {
            if (disposing)
                base.Break();
        }
        sealed public override bool Enlink(T msg) => false;

        sealed protected override bool LinkThem(PulseSourcePlexBeaconPin<T> held, PulseSourcePlexBeaconPin<T> next)
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

        sealed protected override PulseSourcePlexBeaconPin<T> PickAPin(T msg)
            => new PulseSourcePlexBeaconPin<T>(msg, Read);
    }
}
