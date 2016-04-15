﻿namespace MessagePlex
{
    public abstract class SimpleMessagePlicator<TMsg> : SimpleMessagePlicatorBase<TMsg, SimplePlexBeaconPin<TMsg>>
    {
        sealed public override bool Break() => base.Break();
        sealed protected override void Dispose(bool disposing)
            => base.Dispose(disposing);
        sealed protected override SimplePlexBeaconPin<TMsg> PickAPin(TMsg msg)
            => new SimplePlexBeaconPin<TMsg>(msg);

        sealed protected override bool LinkThem(SimplePlexBeaconPin<TMsg> held, SimplePlexBeaconPin<TMsg> next)
            => (held?.LinkWith(next)).GetValueOrDefault(true);
    }
}
