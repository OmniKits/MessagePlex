namespace MessagePlex
{
    public abstract class ReadSafeMsgPlicator<TMsg> : BasicMessagePlicator<TMsg, TaskPlexBeaconPin<TMsg>>
    {
        sealed protected override TaskPlexBeaconPin<TMsg> PickAPin(TMsg msg)
            => new TaskPlexBeaconPin<TMsg>(msg);

        sealed protected override bool LinkThem(TaskPlexBeaconPin<TMsg> held, TaskPlexBeaconPin<TMsg> next)
            => (held?.LinkWith(next)).GetValueOrDefault(true);
    }
}
