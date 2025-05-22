namespace ProceduralGeneration
{
    public static class MathUtilities
    {
        public static float NegModF(float x, float m)
        {
            float r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}
