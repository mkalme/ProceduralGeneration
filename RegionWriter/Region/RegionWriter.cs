using NbtEditor;
using System.Buffers;

namespace RegionWriter
{
    public class RegionWriter : IRegionWriter
    {
        public IChunkWriter ChunkWriter { get; set; }
        public ICompressor Compressor { get; set; }
        public ITagSerializer<Tag> TagSerializer { get; set; }
        public CompressionMethod CompressionMethod { get; set; } = CompressionMethod.ZLib;

        public int ConcurrentCount { get; }

        private readonly ITagSerializer<Tag>[] _tagSerializers;
        private readonly INbtWriter[] _nbtWriters;
        private readonly CustomBufferWriter[] _bufferWriters;

        public RegionWriter(IChunkWriter chunkWriter, int concurrentCount = 8)
        {
            ChunkWriter = chunkWriter;
            Compressor = new Compressor();
            TagSerializer = new TagSerializer();

            ConcurrentCount = concurrentCount;
            _tagSerializers = new ITagSerializer<Tag>[ConcurrentCount];
            _nbtWriters = new INbtWriter[ConcurrentCount];
            _bufferWriters = new CustomBufferWriter[ConcurrentCount];
            for (int i = 0; i < ConcurrentCount; i++) 
            {
                _tagSerializers[i] = new TagSerializer();
                _bufferWriters[i] = new CustomBufferWriter();
                _nbtWriters[i] = new NbtWriter(_bufferWriters[i]);
            }
        }

        public void Write(Chunk[] chunks, string outputDir)
        {
            FetchLocation(chunks[0], out int x, out int z);
            string name = CreateName(x, z);

            byte[] fileBuffer = ArrayPool<byte>.Shared.Rent(1024 * 1024 * 32);
            const int SECTOR_SIZE = 4096;
            int offset = 2;

            using (FileStream fileStream = File.Create($"{outputDir}\\{name}")) 
            {
                ArraySlice[] compressedChunkSlices = new ArraySlice[ConcurrentCount];
                Chunk[] compressedChunks = new Chunk[ConcurrentCount];

                ParallelUtilities.BufferedFor(0, chunks.Length, ConcurrentCount, (index, r) =>
                {
                    Chunk chunk = chunks[index];
                    Tag chunkTag = ChunkWriter.Write(chunk);

                    compressedChunks[r] = chunk;

                    //Write chunk NBT tag data into a byte array
                    byte[] uncompressedChunkBuffer = ArrayPool<byte>.Shared.Rent(1024 * 256);
                    _bufferWriters[r].ArraySlice = new ArraySlice(uncompressedChunkBuffer);
                    _tagSerializers[r].Serialize(chunkTag, _nbtWriters[r]);
                    ArraySlice uncompressedChunkSlice = _bufferWriters[r].ArraySlice.AsOffsetZero();

                    //Compress payload
                    byte[] compressedChunkBuffer = ArrayPool<byte>.Shared.Rent(1024 * 32);
                    compressedChunkSlices[r] = new ArraySlice(compressedChunkBuffer);
                    compressedChunkSlices[r].AddPosition(5);
                    Compressor.Compress(CompressionMethod, ref uncompressedChunkSlice, ref compressedChunkSlices[r]);

                    BitUtilities.WriteInt32(compressedChunkSlices[r].MaxPosition - 1, compressedChunkBuffer, 0);
                    compressedChunkBuffer[4] = (byte)CompressionMethod;
                    compressedChunkSlices[r].MaxPosition += SECTOR_SIZE - compressedChunkSlices[r].MaxPosition % SECTOR_SIZE;

                    ArrayPool<byte>.Shared.Return(uncompressedChunkBuffer, true);
                }, () =>
                {
                    for (int i = 0; i < compressedChunkSlices.Length; i++)
                    {
                        ArraySlice compressedChunkSlice = compressedChunkSlices[i];
                        if (compressedChunkSlice.IsEmpty()) continue;

                        int index = 4 * (MathUtilities.NegMod(compressedChunks[i].X, 32) + MathUtilities.NegMod(compressedChunks[i].Z, 32) * 32);
                        BitUtilities.WriteInt24(offset, fileBuffer, index);

                        fileBuffer[index + 3] = (byte)(compressedChunkSlice.MaxPosition / SECTOR_SIZE);

                        Buffer.BlockCopy(compressedChunkSlice.Array, 0, fileBuffer, offset * SECTOR_SIZE, compressedChunkSlice.MaxPosition);
                        offset += compressedChunkSlice.MaxPosition / SECTOR_SIZE;

                        ArrayPool<byte>.Shared.Return(compressedChunkSlices[i].Array, true);
                        compressedChunkSlices[i] = ArraySlice.Empty;
                    }
                });

                fileStream.Write(fileBuffer, 0, offset * SECTOR_SIZE);
            }

            ArrayPool<byte>.Shared.Return(fileBuffer, true);
        }

        private static void FetchLocation(Chunk chunk, out int x, out int z)
        {
            x = MathUtilities.SectionMod(chunk.X, 32);
            z = MathUtilities.SectionMod(chunk.Z, 32);
        }
        private static string CreateName(int x, int z)
        {
            return $"r.{x}.{z}.mca";
        }
    }
}
