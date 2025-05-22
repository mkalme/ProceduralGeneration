namespace RegionWriter
{
    public interface IKeyProvider<TKey>
    {
        public TKey Provide(ushort id);
    }
}
