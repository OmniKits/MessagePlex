using System;
using System.Threading;

namespace MessagePlex
{
    public class BlockingPlexBeaconPin<T> : IPlexBeaconPin<T>
    {
        // usually volatile is necessary for double-checked locking
        // but it's no longer the case since the only change -
        // - "other" threads can see is setting it to null
        // further tuning can be helpful
        private ManualResetEvent _MRE = new ManualResetEvent(false);

        public T Message { get; }

        internal BlockingPlexBeaconPin(T msg)
        {
            Message = msg;
        }

        public bool HasNext => _MRE == null;

        private IPlexBeaconPin<T> _Next;
        public virtual IPlexBeaconPin<T> Next
        {
            get
            {
                _MRE?.WaitOne();

                return _Next;
            }
        }

        internal bool LinkWith(IPlexBeaconPin<T> next)
        {
            if (HasNext)
                return false;

            var mre = Interlocked.Exchange(ref _MRE, null);
            if (mre == null)
                return false;

            _Next = next;
            mre.Set();

            return true;
        }
    }
}
