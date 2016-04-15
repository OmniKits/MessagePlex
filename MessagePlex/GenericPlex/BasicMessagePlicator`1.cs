using System;
using System.Collections.Generic;

namespace MessagePlex
{
    public abstract class BasicMessagePlicator<TMsg> : BasicMessagePlicator<TMsg, BasicPlexBeaconPin<TMsg>>
    {
        sealed public override bool Break() => base.Break();
        sealed protected override void Dispose(bool disposing)
            => base.Dispose(disposing);
        sealed protected override BasicPlexBeaconPin<TMsg> PickAPin(TMsg msg)
            => new BasicPlexBeaconPin<TMsg>(msg);

        sealed protected override bool LinkThem(BasicPlexBeaconPin<TMsg> held, BasicPlexBeaconPin<TMsg> next)
            => (held?.LinkWith(next)).GetValueOrDefault(true);
    }
}
