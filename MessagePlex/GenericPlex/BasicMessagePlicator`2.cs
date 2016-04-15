﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace MessagePlex
{
    public abstract class BasicMessagePlicator<TMsg, TLink> : MessagePlicatorBase<TMsg, TLink>
        where TLink : class, IPlexBeaconPin<TMsg>
    {
        sealed protected override bool Enlink(TMsg msg, bool nonBreaking)
        {
            if (IsDisposeTriggered)
                return false;

            var held = HeldLink;
            if (!nonBreaking && held == null)
                return false;

            var next = PickAPin(msg);
            HeldLink = next;
            LinkThem(held, next);

            return true;
        }
    }
}