using Setup = System.Action;

namespace MessagePlex
{
    public sealed class SimpleSourcePlexBeaconPin<T> : SimplePlexBeaconPin<T>
    {
        private Setup _Setup;

        internal SimpleSourcePlexBeaconPin(T msg, Setup setup)
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
