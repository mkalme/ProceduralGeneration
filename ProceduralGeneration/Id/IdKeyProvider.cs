namespace ProceduralGeneration
{
    public class IdKeyProvider<TKey> : IIdKeyProvider<TKey> where TKey : notnull
    {
        private readonly IDictionary<int, ushort> _ids;
        private readonly IDictionary<ushort, TKey> _keys;

        public IdKeyProvider() 
        {
            _ids = new Dictionary<int, ushort>();
            _keys = new Dictionary<ushort, TKey>();
        }

        public TKey Provide(ushort id)
        {
            return _keys[id];
        }
        public ushort ProvideId(TKey key)
        {
            int hash = key.GetHashCode();
            if (!_ids.TryGetValue(hash, out ushort id)) {
                id = GetNextId();

                _ids.Add(hash, id);
                _keys.Add(id, key);
            }

            return id;
        }

        public ushort AddKey(TKey key) 
        {
            ushort id = GetNextId();

            _ids.Add(key.GetHashCode(), id);
            _keys.Add(id, key);

            return id;
        }

        private ushort GetNextId() 
        {
            return (ushort)_ids.Count;
        }
    }
}
