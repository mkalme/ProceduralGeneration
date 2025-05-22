namespace RegionWriter
{
    public class NoCompressionCompressor : ISpecificCompressor
    {
        public void Compress(ref ArraySlice input, ref ArraySlice output)
        {
            Buffer.BlockCopy(input.Array, input.Position, output.Array, output.Position, input.MaxPosition - input.Position);
            output.AddPosition(input.MaxPosition - input.Position);
        }
    }
}
