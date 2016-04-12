using System.Collections.Generic;

public abstract class ReadSafeMsgPlicator<TMsg> : MessagePlicatorBase<TMsg, TaskPlexBeaconPin<TMsg>>
{
    sealed protected override TaskPlexBeaconPin<TMsg> PickAPin(TMsg msg)
        => new TaskPlexBeaconPin<TMsg>(msg);

    sealed protected override void LinkThem(TaskPlexBeaconPin<TMsg> held, TaskPlexBeaconPin<TMsg> next)
        => held?.LinkWith(next);
}
