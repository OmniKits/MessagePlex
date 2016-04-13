using System.Threading.Tasks;

public interface ITaskPlexBeaconPin<T> : IPlexBeaconPin<T>
{
    new ITaskPlexBeaconPin<T> Next { get; }
    Task<ITaskPlexBeaconPin<T>> ForNext { get; }
}
