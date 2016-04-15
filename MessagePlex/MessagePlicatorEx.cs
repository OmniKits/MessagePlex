using System;
using System.Collections.Generic;
using System.Text;

using MessagePlex;

public static class MessagePlicatorEx
{
    public static PlexStream ToPlexStream<TLink>(this MessagePlicatorBase<byte[], TLink> msgChain)
        where TLink : class, IPlexBeaconPin<byte[]>
        => new PlexStream(msgChain.HeldLink);
}
