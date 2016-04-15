using System.Threading;

using Setup = System.Action;

namespace MessagePlex
{
    public sealed class BlockingSourcePlexBeaconPin<T> : BlockingPlexBeaconPin<T>
    {
        private Setup _Setup;

        internal BlockingSourcePlexBeaconPin(T msg, Setup setup)
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
