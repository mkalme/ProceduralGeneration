namespace RegionWriter
{
    public class Chunk
    {
        public int X { get; set; }
        public int Z { get; set; }

        public int BlockCeiliingY { get; set; }
        public ushort[] BlockStates { get; set; }

        public int BiomeCeiliingY { get; set; }
        public ushort[] Biomes { get; set; }

        public string Status { get; set; } = "full";

        public Chunk()
        {
            BlockStates = Array.Empty<ushort>();
            Biomes = Array.Empty<ushort>();
        }
    }
}
