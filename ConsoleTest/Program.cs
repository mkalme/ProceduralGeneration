using ProceduralGeneration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Generator generator = new Generator();
            generator.Generate("ProceduralGeneration", $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\.minecraft\\saves");

            //TestPerlinNoise();

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static void TestPerlinNoise()
        {
            byte[,] perlin = new byte[1024, 1024];

            Seed seed = new Random().NextInt64();

            //INoise noise = new LayeredNoise()
            //    .AddNoise(new OctaveNoise(new PerlinNoise2D(256, seed.GetNextSeed()), 1))
            //    .AddNoise(new OctaveNoise(new PerlinNoise2D(128, seed.GetNextSeed()), 0.5F))
            //    .AddNoise(new OctaveNoise(new PerlinNoise2D(64, seed.GetNextSeed()), 0.25F))
            //    .AddNoise(new OctaveNoise(new PerlinNoise2D(32, seed.GetNextSeed()), 0.125F));

            //noise = new OctaveNoise(noise, 1, 2);

            INoise noise = new OctaveNoise(new CustomNoise(), 1, 16);

            for (int y = 0; y < perlin.GetLength(1); y++)
            {
                Parallel.For(0, perlin.GetLength(0), x => 
                {
                    float xyNoise = noise.GetNoise(x, y);
                    perlin[x, y] = (byte)((xyNoise + 1F) / 2F * byte.MaxValue);
                });
            }

            SavePerlinAsImage(perlin, $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\perlin.png");
        }
        private static void SavePerlinAsImage(byte[,] perlin, string path) 
        {
            int width = perlin.GetLength(0);
            int height = perlin.GetLength(1);

            Bitmap grayscaleBitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            ColorPalette grayscalePalette = grayscaleBitmap.Palette;
            for (int i = 0; i < 256; i++)
            {
                grayscalePalette.Entries[i] = Color.FromArgb(i, i, i);
            }
            grayscaleBitmap.Palette = grayscalePalette;

            BitmapData bitmapData = grayscaleBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, grayscaleBitmap.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(grayscaleBitmap.PixelFormat) / 8;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte pixelValue = perlin[x, y];
                    IntPtr pixelPtr = bitmapData.Scan0 + y * bitmapData.Stride + x * bytesPerPixel;
                    System.Runtime.InteropServices.Marshal.WriteByte(pixelPtr, pixelValue);
                }
            }

            grayscaleBitmap.UnlockBits(bitmapData);
            grayscaleBitmap.Save(path);

            using Process fileopener = new Process();
            fileopener.StartInfo.FileName = "explorer";
            fileopener.StartInfo.Arguments = "\"" + path + "\"";
            fileopener.Start();
        }
    }

    class CustomNoise : INoise
    {
        public INoise ContinentNoise { get; set; }
        public INoise InlandHillNoise { get; set; }
        public INoise RockNoise { get; set; }

        public CustomNoise() 
        {
            Seed seed = new Random().NextInt64();

            ContinentNoise = new LayeredNoise()
                .AddNoise(new PerlinNoise2D(4096, seed.GetNextSeed()))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(2048, seed.GetNextSeed()), 0.5F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(1024, seed.GetNextSeed()), 0.25F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(512, seed.GetNextSeed()), 0.125F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(256, seed.GetNextSeed()), 0.125F / 2))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(128, seed.GetNextSeed()), 0.125F / 4))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(64, seed.GetNextSeed()), 0.125F / 8))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(32, seed.GetNextSeed()), 0.125F / 16))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(16, seed.GetNextSeed()), 0.125F / 32))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(8, seed.GetNextSeed()), 0.125F / 64));

            //InlandHillNoise = new LayeredNoise()
            //    .AddNoise(new OctaveNoise(new PerlinNoise2D(128, seed.GetNextSeed()), 1));

            //RockNoise = new LayeredNoise()
            //    .AddNoise(new OctaveNoise(new PerlinNoise2D(32, seed.GetNextSeed()), 0.15F))
            //    .AddNoise(new OctaveNoise(new PerlinNoise2D(16, seed.GetNextSeed()), 0.1F))
            //    .AddNoise(new OctaveNoise(new PerlinNoise2D(8, seed.GetNextSeed()), 0.05F));
        }

        public float GetNoise(float xP, float yP)
        {
            float output = ContinentNoise.GetNoise(xP, yP);

            if (output < 0.1F) return -1;

            return output;
        }
    }
}