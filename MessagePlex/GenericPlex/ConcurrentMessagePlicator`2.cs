﻿using System.Collections.Generic;
using System.Threading;

namespace MessagePlex
{
    public abstract class ConcurrentMessagePlicator<TMsg, TLink> : MessagePlicatorBase<TMsg, TLink>
        where TLink : class, IPlexBeaconPin<TMsg>
    {
        sealed protected override bool Enlink(TMsg msg, bool nonBreaking)
        {
            var next = PickAPin(msg);
            TLink held;

            if (nonBreaking)
                held = Interlocked.Exchange(ref HeldLink, next);
            else
            {
                held = HeldLink;
                for (;;)
                {
                    if (held == null)
                        return false;

                    var tmp = Interlocked.CompareExchange(ref HeldLink, next, held);

                    if (tmp == held)
                        break;

                    held = tmp;
                }
            }

            LinkThem(held, next);

            return true;
        }
    }
}
