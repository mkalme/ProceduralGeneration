using NbtEditor;

namespace RegionWriter
{
    public class BiomePaletteWriter : PaletteWriter<string>
    {
        protected override int SectionSize => 4 * 4 * 4;
        protected override TagId ElementId => TagId.String;

        public BiomePaletteWriter(IKeyProvider<string> biomeProvider) : base(biomeProvider){}

        protected override Tag SerializeState(string state)
        {
            return state;
        }
    }
}
