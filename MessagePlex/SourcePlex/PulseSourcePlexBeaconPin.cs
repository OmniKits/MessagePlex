using System.Threading;

using Setup = System.Action;

namespace MessagePlex
{
    public sealed class PulseSourcePlexBeaconPin<T> : PulsePlexBeaconPin<T>
    {
        private Setup _Setup;

        internal PulseSourcePlexBeaconPin(T msg, Setup setup)
            : base(msg)
        {
            _Setup = setup;
        }

        public override IPlexBeaconPin<T> Next
        {
            get
            {
                if (_Setup != null)
                    Interlocked.Exchange(ref _Setup, null)?.Invoke();

                return base.Next;
            }
        }
    }
}
