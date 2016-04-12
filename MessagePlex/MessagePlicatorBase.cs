using System;
using System.Collections;
using System.Collections.Generic;

public abstract class MessagePlicatorBase<TMsg, TLink> : MessagePlex.DisposableBase, IMessagePlicator<TMsg>
    where TLink : class, IPlexBeaconPin<TMsg>
{
    protected internal volatile TLink HeldLink;
    protected MessagePlicatorBase()
    {
        HeldLink = PickAPin(default(TMsg));
    }

    protected abstract TLink PickAPin(TMsg msg);
    protected abstract void LinkThem(TLink held, TLink next);

    protected virtual bool Enlink(TMsg msg, bool nonBreaking)
    {
        var held = HeldLink;
        if (!nonBreaking && held == null)
            return false;

        var next = PickAPin(msg);
        HeldLink = next;
        LinkThem(held, next);

        return true;
    }
    public abstract bool Enlink(TMsg value);
    public virtual void Break()
    {
        var held = HeldLink;
        HeldLink = null;
        LinkThem(held, null);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            Break();
    }

    public virtual IEnumerator<TMsg> GetEnumerator()
        => new DoppleGanger<TMsg>(HeldLink);
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
