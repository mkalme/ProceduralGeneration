namespace RegionWriter
{
    public static class MathUtilities
    {
        public static int NegMod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
        public static int SectionMod(int x, int m)
        {
            int mod = NegMod(x, m);
            return (x - mod) / m;
        }
    }
}
