namespace ProceduralGeneration
{
    public interface IIdProvider<TKey>
    {
        ushort ProvideId(TKey key);
    }
}
