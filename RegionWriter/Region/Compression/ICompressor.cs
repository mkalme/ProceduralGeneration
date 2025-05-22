namespace RegionWriter {
    public interface ICompressor
    {
        void Compress(CompressionMethod method, ref ArraySlice input, ref ArraySlice output);
    }
}
