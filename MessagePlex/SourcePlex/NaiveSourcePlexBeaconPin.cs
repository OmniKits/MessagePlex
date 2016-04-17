using Setup = System.Action;

namespace MessagePlex
{
    public sealed class NaiveSourcePlexBeaconPin<T> : SimplePlexBeaconPin<T>
    {
        private Setup _Setup;

        internal NaiveSourcePlexBeaconPin(T msg, Setup setup)
            : base(msg)
        {
            _Setup = setup;
        }

        public override IPlexBeaconPin<T> Next
        {
            get
            {
                var setup = _Setup;
                if (setup != null)
                {
                    _Setup = null;
                    setup();
                }

                return base.Next;
            }
        }
    }
}
