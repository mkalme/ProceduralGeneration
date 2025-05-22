namespace RegionWriter
{
    public readonly struct Property
    {
        public string Name { get; init; }
        public string Value { get; init; }

        public Property(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
