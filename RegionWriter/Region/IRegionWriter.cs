namespace RegionWriter
{
    public interface IRegionWriter
    {
        void Write(Chunk[] chunk, string outputDir);
    }
}