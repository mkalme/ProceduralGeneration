using RegionWriter;

namespace ProceduralGeneration
{
    //Credit https://pastebin.com/gXEYsmw8
    public class PerlinNoise2D : INoise
    {
        public int CellSize { get; }
        public long Seed { get; }

        private int _seedHash;

        public PerlinNoise2D(int cellSize, long seed) 
        {
            CellSize = cellSize;
            Seed = seed;
            _seedHash = CreateHash((int)(seed & uint.MaxValue), (int)(seed >> 32));
        }

        public float GetNoise(float x, float y)
        {
            int x0 = (int)(x - MathUtilities.NegModF(x, CellSize));
            int y0 = (int)(y - MathUtilities.NegModF(y, CellSize));
            int x1 = x0 + CellSize;
            int y1 = y0 + CellSize;

            float sx = (x - x0) / CellSize;
            float sy = (y - y0) / CellSize;

            float n0 = CalculateDotGridGradient(x0, y0, x, y);
            float n1 = CalculateDotGridGradient(x1, y0, x, y);
            float ix0 = Interpolate(n0, n1, sx);

            n0 = CalculateDotGridGradient(x0, y1, x, y);
            n1 = CalculateDotGridGradient(x1, y1, x, y);
            float ix1 = Interpolate(n0, n1, sx);

            return Interpolate(ix0, ix1, sy);
        }

        private float CalculateDotGridGradient(int ix, int iy, float x, float y)
        {
            Vec2f gradient = CreateRandomGradient(ix, iy);

            float dx = (x - ix) / CellSize;
            float dy = (y - iy) / CellSize;

            return dx * gradient.X + dy * gradient.Y;
        }
        private static float Interpolate(float a0, float a1, float w)
        {
            return (a1 - a0) * (3.0F - w * 2.0F) * w * w + a0;
        }

        private Vec2f CreateRandomGradient(int ix, int iy)
        {
            int a = CreateHash(CreateHash(ix, iy), _seedHash);
            float random = a * ((float)Math.PI / ~(~0u >> 1));

            return new Vec2f((float)Math.Sin(random), (float)Math.Cos(random));
        }
        private static int CreateHash(int left, int right)
        {
            int w = 8 * 4;
            int s = w / 2;
            int a = left, b = right;
            a *= 180545081;

            b ^= a << s | a >> w - s;
            b *= 1911520717;

            a ^= b << s | b >> w - s;
            a *= 2048419325;

            return a;
        }
    }
}
