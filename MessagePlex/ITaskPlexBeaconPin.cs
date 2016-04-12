using System.Threading.Tasks;

public interface ITaskPlexBeaconPin<TPayload> : IPlexBeaconPin<TPayload>
{
    new ITaskPlexBeaconPin<TPayload> Next { get; }
    Task<ITaskPlexBeaconPin<TPayload>> ForNext { get; }
}
