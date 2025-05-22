using NbtEditor;

namespace RegionWriter
{
    public class BlockPaletteWriter : PaletteWriter<Block>
    {
        protected override int SectionSize => 16 * 16 * 16;
        protected override TagId ElementId => TagId.Compound;

        public BlockPaletteWriter(IKeyProvider<Block> blockProvider) : base(blockProvider){}

        protected override Tag SerializeState(Block state)
        {
            CompoundTag output = new()
            {
                { "Name", state.Name }
            };

            if (state.Properties.Length > 0) 
            {
                CompoundTag propertiesCompound = new(state.Properties.Length);
                foreach (Property property in state.Properties) 
                {
                    propertiesCompound.Add(property.Name, property.Value);
                }

                output.Add("Properties", propertiesCompound);
            }

            return output;
        }
    }
}
