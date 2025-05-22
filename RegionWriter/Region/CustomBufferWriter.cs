using NbtEditor;

namespace RegionWriter
{
    public class CustomBufferWriter : BufferWriter
    {
        public ArraySlice ArraySlice
        {
            get => _output;
            set => _output = value;
        }
        private ArraySlice _output;

        public override void WriteBuffer(ReadOnlySpan<byte> buffer)
        {
            buffer.CopyTo(_output.Array.AsSpan(_output.Position, buffer.Length));
            _output.AddPosition(buffer.Length);
        }

        public override void Dispose(){}
    }
}
