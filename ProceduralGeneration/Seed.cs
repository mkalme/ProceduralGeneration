namespace ProceduralGeneration
{
    public class Seed
    {
        public long OriginalSeed { get; }

        private readonly JavaRandom _random;

        public static implicit operator Seed(long seed) => new(seed);

        public Seed(long seed) 
        {
            OriginalSeed = seed;

            _random = new JavaRandom(seed);
        }

        public long GetNextSeed() 
        {
            return _random.NextLong();
        }
    }
}
