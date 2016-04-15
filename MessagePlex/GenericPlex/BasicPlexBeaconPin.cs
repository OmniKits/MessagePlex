using System;

namespace MessagePlex
{
    public class BasicPlexBeaconPin<T> : IPlexBeaconPin<T>
    {
        public T Message { get; }

        internal BasicPlexBeaconPin(T msg)
        {
            Message = msg;
        }

        public bool HasNext { get; private set; }
        private IPlexBeaconPin<T> _Next;
        public virtual IPlexBeaconPin<T> Next
        {
            get
            {
                if (!HasNext)
                    throw new InvalidOperationException();
                return _Next;
            }
        }

        internal bool LinkWith(IPlexBeaconPin<T> next)
        {
            if (HasNext)
                return false;

            _Next = next;
            HasNext = true;
            return true;
        }
    }
}
