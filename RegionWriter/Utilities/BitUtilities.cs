using System.Buffers.Binary;

namespace RegionWriter
{
    public static class BitUtilities
    {
        public static void WriteInt24(int value, byte[] output, int offset) 
        {
            Span<byte> buffer = stackalloc byte[sizeof(int)];
            BinaryPrimitives.TryWriteInt32BigEndian(buffer, value);

            output[offset] = buffer[1];
            output[offset + 1] = buffer[2];
            output[offset + 2] = buffer[3];
        }
        public static void WriteInt32(int value, byte[] output, int offset)
        {
            BinaryPrimitives.TryWriteInt32BigEndian(output.AsSpan(offset), value);
        }
    }
}
