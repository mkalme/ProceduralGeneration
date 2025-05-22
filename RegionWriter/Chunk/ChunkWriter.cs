using NbtEditor;

namespace RegionWriter
{
    public class ChunkWriter : IChunkWriter
    {
        public ISectionWriter SectionWriter { get; set; }

        public ChunkWriter(ISectionWriter sectionWriter) 
        {
            SectionWriter = sectionWriter;
        }

        public Tag Write(Chunk chunk)
        {
            int sectionCount = (int)Math.Ceiling((Math.Max(chunk.BlockCeiliingY, chunk.BiomeCeiliingY) + 64F) / 16);
            ListTag sections = new(TagId.Compound, sectionCount);
            for (int i = 0; i < sectionCount; i++) 
            {
                sections.Add(SectionWriter.Write(chunk, (sbyte)(i - 4)));
            }

            return new CompoundTag() 
            {
                { "sections", sections },
                { "Status", chunk.Status },
                { "xPos", chunk.X },
                { "yPos", -4 },
                { "zPos", chunk.Z },
                { "DataVersion", 3463 }
            };
        }
    }
}
