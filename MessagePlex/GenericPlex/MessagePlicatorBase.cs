using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace MessagePlex
{
    public abstract class MessagePlicatorBase<TMsg, TLink> : DisposableBase, IMessagePlicator<TMsg>
        where TLink : class, IPlexBeaconPin<TMsg>
    {
        internal volatile TLink HeldLink;
        protected MessagePlicatorBase()
        {
            HeldLink = PickAPin(default(TMsg));
        }

        protected abstract TLink PickAPin(TMsg msg);
        protected abstract bool LinkThem(TLink held, TLink next);

        protected abstract bool Enlink(TMsg msg, bool nonBreaking);
        public abstract bool Enlink(TMsg msg);

        public virtual bool Break()
            => LinkThem(Interlocked.Exchange(ref HeldLink, null), null);
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Break();
        }
        public IEnumerator<TMsg> GetEnumerator()
            => new DoppleGanger<TMsg>(HeldLink);
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
