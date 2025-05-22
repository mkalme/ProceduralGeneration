namespace ProceduralGeneration
{
    public sealed class JavaRandom
    {
        private static long _multiplier = 0x5deece66dL;
        private long _seed;

        public JavaRandom(long seed)
        {
            SetInternalSeed(seed);
        }

        public long NextLong()
        {
            return ((long)NextBits(32) << 32) + NextBits(32);
        }
        private int NextBits(int bits)
        {
            _seed = (_seed * _multiplier + 0xbL) & ((1L << 48) - 1);
            return (int)(_seed >>> (48 - bits));
        }
        private void SetInternalSeed(long seed)
        {
            this._seed = (seed ^ _multiplier) & ((1L << 48) - 1);
        }
    }
}
