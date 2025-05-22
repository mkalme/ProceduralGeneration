namespace RegionWriter
{
    public interface IBlockStateWriter
    {
        long[] Write(ReadOnlySpan<short> unlockedArray, int bitCount);
    }
}
