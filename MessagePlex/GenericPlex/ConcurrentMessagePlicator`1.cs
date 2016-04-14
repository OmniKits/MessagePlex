using System;

namespace MessagePlex
{
    public abstract class ConcurrentMessagePlicator<TMsg> : ConcurrentMessagePlicator<TMsg, TaskPlexBeaconPin<TMsg>>
    {
        sealed public override bool Break()
            => base.Break();
        sealed protected override TaskPlexBeaconPin<TMsg> PickAPin(TMsg msg)
            => new TaskPlexBeaconPin<TMsg>(msg);

        sealed protected override bool LinkThem(TaskPlexBeaconPin<TMsg> held, TaskPlexBeaconPin<TMsg> next)
            => (held?.LinkWith(next)).GetValueOrDefault(true);
    }
}
