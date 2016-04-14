using System;
using System.Threading;

using Setup = System.Action;

namespace MessagePlex
{
    public sealed class PulseSourcePlexBeaconPin<T> : IPlexBeaconPin<T>
    {
        // usually volatile is necessary for double-checked locking
        // but it's no longer the case since the only change -
        // - "other" threads can see is setting it to null
        // further tuning can be helpful
        private ManualResetEvent _MRE = new ManualResetEvent(false);
        private Setup _Setup;

        public T Message { get; }

        internal PulseSourcePlexBeaconPin(T msg, Setup setup)
        {
            Message = msg;
            _Setup = setup;
        }

        public bool HasNext => _MRE == null;

        private IPlexBeaconPin<T> _Next;
        public IPlexBeaconPin<T> Next
        {
            get
            {
                if (_Setup != null)
                    Interlocked.Exchange(ref _Setup, null)?.Invoke();

                _MRE?.WaitOne();

                return _Next;
            }
        }

        internal bool LinkWith(IPlexBeaconPin<T> next)
        {
            if (HasNext)
                return false;

            lock (this)
            {
                var mre = _MRE;
                if (mre == null)
                    return false;

                _Next = next;
                mre.Set();
                _MRE = null;
            }

            return true;
        }
    }
}
