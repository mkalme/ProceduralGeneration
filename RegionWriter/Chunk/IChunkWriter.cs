using NbtEditor;

namespace RegionWriter
{
    public interface IChunkWriter
    {
        Tag Write(Chunk chunk);
    }
}
