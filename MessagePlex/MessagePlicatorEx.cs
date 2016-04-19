using System;
using System.Collections.Generic;
using System.Text;

using MessagePlex;

public static class MessagePlicatorEx
{
    public static PlexStream ToPlexStream<TPin>(this MessagePlicatorBase<byte[], TPin> msgChain)
        where TPin : class, IPlexBeaconPin<byte[]>
        => new PlexStream(msgChain.HotPin);
}
