using System.Collections.Generic;

public abstract class ConcurrentMessagePlicator<TMsg> : ConcurrentMessagePlicator<TMsg, TaskPlexBeaconPin<TMsg>>
{
    sealed protected override TaskPlexBeaconPin<TMsg> PickAPin(TMsg msg)
        => new TaskPlexBeaconPin<TMsg>(msg);

    sealed protected override void LinkThem(TaskPlexBeaconPin<TMsg> held, TaskPlexBeaconPin<TMsg> next)
        => held?.LinkWith(next);
}
