public interface IPlexBeaconPin<T>
{
    bool HasNext { get; }
    IPlexBeaconPin<T> Next { get; }
    T Message { get; }
}
