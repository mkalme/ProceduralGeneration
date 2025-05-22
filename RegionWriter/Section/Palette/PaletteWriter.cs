using NbtEditor;

namespace RegionWriter
{
    public abstract class PaletteWriter<TState> : IPaletteWriter<TState>
    {
        public IKeyProvider<TState> IdStateProvider { get; set; }
        public IBlockStateWriter BlockStateWriter { get; set; }

        protected abstract int SectionSize { get; }
        protected abstract TagId ElementId { get; }

        public PaletteWriter(IKeyProvider<TState> idStateProvider)
        {
            IdStateProvider = idStateProvider;
            BlockStateWriter = new SimpleBlockStateWriter();
        }

        public virtual CompoundTag Write(ReadOnlySpan<ushort> ids)
        {
            Dictionary<ushort, short> indexes = new(8);

            Span<short> unlocked = stackalloc short[SectionSize];
            for (int i = 0; i < ids.Length; i++)
            {
                if (!indexes.TryGetValue(ids[i], out short index))
                {
                    index = (short)indexes.Count;
                    indexes.Add(ids[i], index);
                }

                unlocked[i] = index;
            }

            ListTag palette = new(ElementId, indexes.Count);
            foreach (ushort id in indexes.Keys)
            {
                palette.Add(SerializeState(IdStateProvider.Provide(id)));
            }

            CompoundTag output = new()
            {
                { "palette", palette }
            };

            if (indexes.Count > 1)
            {
                long[] locked = BlockStateWriter.Write(unlocked, GetBitCount(indexes.Count));
                output.Add("data", locked);
            }

            return output;
        }

        protected virtual int GetBitCount(int indexesCount) 
        {
            return Math.Max(4, (int)Math.Ceiling(Math.Log(indexesCount, 2)));
        }
        protected abstract Tag SerializeState(TState state);
    }
}
