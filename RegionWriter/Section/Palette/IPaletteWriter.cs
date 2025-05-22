using NbtEditor;

namespace RegionWriter
{
    public interface IPaletteWriter<TState>
    {
        CompoundTag Write(ReadOnlySpan<ushort> ids);
    }
}
