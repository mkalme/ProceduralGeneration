namespace RegionWriter
{
    public struct ArraySlice
    {
        public byte[] Array { get; set; }
        public int MaxPosition { get; set; }
        public int Position { get; set; }

        public static ArraySlice Empty => new();

        public ArraySlice(byte[] array, int maxPosition = 0, int position = 0)
        {
            Array = array;
            MaxPosition = maxPosition;
            Position = position;
        }

        public bool IsEmpty() => Array is null || Array.Length == 0;

        public ArraySlice AsOffsetZero() 
        {
            Position = 0;
            return this;
        }

        public void AddPosition(int value) 
        {
            Position += value;
            MaxPosition = Math.Max(Position, MaxPosition);
        }
    }
}
