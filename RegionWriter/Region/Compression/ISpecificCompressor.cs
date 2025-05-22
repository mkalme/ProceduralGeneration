namespace RegionWriter
{
    public interface ISpecificCompressor
    {
        void Compress(ref ArraySlice input, ref ArraySlice output);
    }
}
