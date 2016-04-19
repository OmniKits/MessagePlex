using System.Threading;

namespace MessagePlex
{
    public abstract class ConcurrentMessagePlicatorBase<TMsg, TPin> : MessagePlicatorBase<TMsg, TPin>
        where TPin : class, IPlexBeaconPin<TMsg>
    {
        sealed protected override bool OnNext(TMsg msg, bool nonBreaking)
        {
            var next = OnSpawnPin(msg);
            TPin held;

            if (nonBreaking)
                held = Interlocked.Exchange(ref HotPin, next);
            else
            {
                held = HotPin;
                for (;;)
                {
                    if (held == null)
                        return false;

                    var tmp = Interlocked.CompareExchange(ref HotPin, next, held);

                    if (tmp == held)
                        break;

                    held = tmp;
                }
            }

            OnLink(held, next);

            return true;
        }
    }
}
