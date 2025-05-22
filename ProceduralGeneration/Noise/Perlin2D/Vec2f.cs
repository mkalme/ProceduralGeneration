namespace ProceduralGeneration
{
    internal readonly struct Vec2f
    {
        public readonly float X { get; init; }
        public readonly float Y { get; init; }

        public Vec2f(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
