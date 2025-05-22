using NbtEditor;

namespace RegionWriter
{
    public class SectionWriter : ISectionWriter
    {
        public IPaletteWriter<Block> BlockPaletteWriter { get; set; }
        public IPaletteWriter<string> BiomePaletteWriter { get; set; }

        public SectionWriter(IKeyProvider<Block> blockProvider, IKeyProvider<string> biomeProvider)
        {
            BlockPaletteWriter = new BlockPaletteWriter(blockProvider);
            BiomePaletteWriter = new BiomePaletteWriter(biomeProvider);
        }

        public CompoundTag Write(Chunk chunk, sbyte y)
        {
            CompoundTag output = new() 
            {
                { "Y", y }
            };

            int biomeStart = 64 * (y + 4);
            int biomeLength = Math.Min(64, chunk.Biomes.Length - biomeStart);
            if (biomeLength > 0) 
            {
                ReadOnlySpan<ushort> biomeIds = chunk.Biomes.AsSpan(biomeStart, biomeLength);
                output.Add("biomes", BiomePaletteWriter.Write(biomeIds));
            }

            int blockStart = 4096 * (y + 4);
            int blockLength = Math.Min(4096, chunk.BlockStates.Length - blockStart);
            if (blockLength > 0) 
            {
                ReadOnlySpan<ushort> blockIds = chunk.BlockStates.AsSpan(blockStart, Math.Min(4096, chunk.BlockStates.Length - blockStart));
                output.Add("block_states", BlockPaletteWriter.Write(blockIds));
            }

            return output;
        }
    }
}
