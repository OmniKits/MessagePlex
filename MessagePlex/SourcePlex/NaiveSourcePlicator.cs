namespace MessagePlex
{
    public abstract class NaiveSourcePlicator<T> : NaiveMessagePlicatorBase<T, NaiveSourcePlexBeaconPin<T>>
    {
        sealed public override bool Break() => false;
        sealed protected override void Dispose(bool disposing)
        {
            if (disposing)
                base.Break();
        }
        sealed public override bool OnNext(T msg) => false;

        sealed protected override bool OnLink(NaiveSourcePlexBeaconPin<T> held, NaiveSourcePlexBeaconPin<T> next)
            => (held?.LinkWith(next)).GetValueOrDefault(true);

        protected abstract bool TryReadMessage(out T result);
        void Read()
        {
            T result;
            if (TryReadMessage(out result))
                base.OnNext(result, true);
            else
                base.Break();
        }

        sealed protected override NaiveSourcePlexBeaconPin<T> OnSpawnPin(T msg)
            => new NaiveSourcePlexBeaconPin<T>(msg, Read);
    }
}
