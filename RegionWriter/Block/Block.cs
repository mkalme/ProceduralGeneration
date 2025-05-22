namespace RegionWriter
{
    public readonly struct Block
    {
        public readonly string Name { get; init; }
        public readonly Property[] Properties { get; init; }

        public Block(string name)
        {
            Name = name;
            Properties = Array.Empty<Property>();
        }
        public Block(string name, Property[] properties)
        {
            Name = name;
            Properties = properties;
        }
    }
}
