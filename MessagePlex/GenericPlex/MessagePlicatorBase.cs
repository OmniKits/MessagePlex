using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace MessagePlex
{
    public abstract class MessagePlicatorBase<TMsg, TPin> : DisposableBase, IMessagePlicator<TMsg>
        where TPin : class, IPlexBeaconPin<TMsg>
    {
        internal volatile TPin HotPin;
        protected MessagePlicatorBase()
        {
            HotPin = OnSpawnPin(default(TMsg));
        }

        protected abstract TPin OnSpawnPin(TMsg msg);
        protected abstract bool OnLink(TPin held, TPin next);

        protected abstract bool OnNext(TMsg msg, bool nonBreaking);
        public abstract bool OnNext(TMsg msg);

        public virtual bool Break()
            => OnLink(Interlocked.Exchange(ref HotPin, null), null);
        bool IMessagePlicator<TMsg>.OnCompleted() => Break();
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Break();
        }
        public IDoppelGanger<TMsg> GetEnumerator()
            => new DoppelGanger<TMsg>(HotPin);
        IEnumerator<TMsg> IEnumerable<TMsg>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
