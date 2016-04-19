namespace MessagePlex
{
    public abstract class NaiveMessagePlicatorBase<TMsg, TPin> : MessagePlicatorBase<TMsg, TPin>
        where TPin : class, IPlexBeaconPin<TMsg>
    {
        sealed protected override bool OnNext(TMsg msg, bool nonBreaking)
        {
            if (IsDisposeTriggered)
                return false;

            var held = HotPin;
            if (!nonBreaking && held == null)
                return false;

            var next = OnSpawnPin(msg);
            HotPin = next;
            OnLink(held, next);

            return true;
        }
    }
}
