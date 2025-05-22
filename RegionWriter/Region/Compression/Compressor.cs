namespace RegionWriter
{
    public class Compressor : ICompressor
    {
        public ISpecificCompressor ZLibCompressor { get; set; }
        public ISpecificCompressor GZipCompressor { get; set; }
        public ISpecificCompressor NoCompressionCompressor { get; set; }

        public Compressor()
        {
            ZLibCompressor = new ZLibCompressor();
            GZipCompressor = new GZipCompressor();
            NoCompressionCompressor = new NoCompressionCompressor();
        }

        public void Compress(CompressionMethod method, ref ArraySlice input, ref ArraySlice output)
        {
            switch (method)
            {
                case CompressionMethod.GZip:
                    GZipCompressor.Compress(ref input, ref output);
                    break;
                case CompressionMethod.ZLib:
                    ZLibCompressor.Compress(ref input, ref output);
                    break;
                case CompressionMethod.None:
                    NoCompressionCompressor.Compress(ref input, ref output);
                    break;
            }
        }
    }
}
