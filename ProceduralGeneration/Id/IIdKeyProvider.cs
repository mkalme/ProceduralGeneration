using RegionWriter;

namespace ProceduralGeneration
{
    public interface IIdKeyProvider<TKey> : IIdProvider<TKey>, IKeyProvider<TKey> where TKey : notnull {}
}
