using ZLibNet;

namespace RegionWriter
{
    public class ZLibCompressor : ISpecificCompressor
    {
        public void Compress(ref ArraySlice input, ref ArraySlice output)
        {
            using MemoryStream inputStream = new(input.Array, input.Position, input.MaxPosition - input.Position);
            using MemoryStream outputStream = new(output.Array, output.Position, output.Array.Length - output.Position, true);
            using ZLibStream compressor = new(outputStream, CompressionMode.Compress);

            long before = outputStream.Position;
            inputStream.CopyTo(compressor);
            compressor.Flush();
            int red = (int)(outputStream.Position - before);

            output.AddPosition(red);
        }
    }
}
