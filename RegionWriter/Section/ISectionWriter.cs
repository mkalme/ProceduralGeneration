using NbtEditor;

namespace RegionWriter
{
    public interface ISectionWriter
    {
        CompoundTag Write(Chunk chunk, sbyte y);
    }
}
