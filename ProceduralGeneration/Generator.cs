using RegionWriter;
using System.Numerics;

namespace ProceduralGeneration
{
    public class Generator
    {
        public int RegionsWide { get; set; } = 25;
        public int RegionsLong { get; set; } = 25;

        public IIdKeyProvider<Block> IdBlockProvider { get; set; }
        public IIdKeyProvider<string> IdBiomeProvider { get; set; }

        public INoise ContinentNoise { get; set; }
        public INoise InlandHillNoise { get; set; }
        public INoise LowRockNoise { get; set; }
        public INoise RockNoise { get; set; }
        public INoise TallRockNoise { get; set; }
        public INoise MidAltitudeNoise { get; set; }

        public INoise OceanNoise { get; set; }

        private readonly ushort _airBlock, _groundBlock, _topBlock, _waterBlock;
        private readonly ushort _oceanBiome, _taigaBiome;

        public Generator() 
        {
            IdKeyProvider<Block> idBlockProvider = new();
            _airBlock = idBlockProvider.ProvideId(new Block("minecraft:air"));
            _groundBlock = idBlockProvider.ProvideId(new Block("minecraft:stone"));
            _topBlock = idBlockProvider.ProvideId(new Block("minecraft:grass_block"));
            _waterBlock = idBlockProvider.ProvideId(new Block("minecraft:water"));
            IdBlockProvider = idBlockProvider;

            IdKeyProvider<string> idBiomeProvider = new();
            _taigaBiome = idBiomeProvider.AddKey("terralith:shield");
            _oceanBiome = idBiomeProvider.AddKey("minecraft:cold_ocean");
            IdBiomeProvider = idBiomeProvider;

            CreateNoise();
        }

        private void CreateNoise() 
        {
            Seed seed = 436543423564L;

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

            ContinentNoise = new OctaveNoise(ContinentNoise, 1, 1);

            InlandHillNoise = new LayeredNoise()
                .AddNoise(new OctaveNoise(new PerlinNoise2D(128, seed.GetNextSeed()), 1));

            LowRockNoise = new LayeredNoise()
                .AddNoise(new OctaveNoise(new PerlinNoise2D(128, seed.GetNextSeed()), 1))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(64, seed.GetNextSeed()), 0.5F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(32, seed.GetNextSeed()), 0.25F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(8, seed.GetNextSeed()), 0.2F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(4, seed.GetNextSeed()), 0.2F));

            RockNoise = new LayeredNoise()
                .AddNoise(new OctaveNoise(new PerlinNoise2D(64, seed.GetNextSeed()), 0.5F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(32, seed.GetNextSeed()), 0.2F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(16, seed.GetNextSeed()), 0.15F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(8, seed.GetNextSeed()), 0.1F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(4, seed.GetNextSeed()), 0.03F));

            TallRockNoise = new LayeredNoise()
                .AddNoise(new OctaveNoise(new PerlinNoise2D(64, seed.GetNextSeed()), 1))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(32, seed.GetNextSeed()), 0.5F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(16, seed.GetNextSeed()), 0.25F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(8, seed.GetNextSeed()), 0.2F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(4, seed.GetNextSeed()), 0.05F));

            MidAltitudeNoise = new LayeredNoise()
                .AddNoise(new OctaveNoise(new PerlinNoise2D(128, seed.GetNextSeed()), 1))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(64, seed.GetNextSeed()), 0.5F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(32, seed.GetNextSeed()), 0.25F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(16, seed.GetNextSeed()), 0.125F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(8, seed.GetNextSeed()), 0.125F / 2));

            OceanNoise = new LayeredNoise()
                .AddNoise(new OctaveNoise(new PerlinNoise2D(512, seed.GetNextSeed()), 1))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(256, seed.GetNextSeed()), 0.5F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(128, seed.GetNextSeed()), 0.25F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(64, seed.GetNextSeed()), 0.125F))
                .AddNoise(new OctaveNoise(new PerlinNoise2D(32, seed.GetNextSeed()), 0.125F / 2));
        }

        public void Generate(string worldName, string outputDir, bool reset = true) 
        {
            string worldPath = $"{outputDir}\\{worldName}";
            
            if (Directory.Exists(worldPath) && reset) 
            {
                Directory.Delete($"{worldPath}\\region", true);
            }
            Directory.CreateDirectory($"{worldPath}\\region");

            int result = 0;

            ParallelUtilities.BufferedFor(0, RegionsWide * RegionsLong, 16, (i, r) => 
            {
                CreateRegion(i % RegionsWide, i / RegionsLong, $"{worldPath}\\region");

                Interlocked.Increment(ref result);
            }, () => 
            {
                GC.Collect();
                Console.WriteLine($"{(result / (float)(RegionsWide * RegionsLong) * 100).ToString("N02")}%");
            });

            //File.Copy("level.dat", $"{worldPath}\\level.dat");
        }

        private void CreateRegion(int x, int z, string outputDir)
        {
            Chunk[] chunks = new Chunk[1024];
            Parallel.For(0, chunks.Length, i => 
            {
                chunks[i] = new();
                chunks[i].X = i % 32 + x * 32;
                chunks[i].Z = i / 32 + z * 32;

                ModifyChunk(chunks[i]);
            });

            RegionWriter.RegionWriter writer = new RegionWriter.RegionWriter(new ChunkWriter(new SectionWriter(IdBlockProvider, IdBiomeProvider)));
            writer.Write(chunks, outputDir);
        }
        private void ModifyChunk(Chunk chunk)
        {
            chunk.BiomeCeiliingY = 319;
            chunk.Biomes = new ushort[64 * 48];
            Array.Fill(chunk.Biomes, _taigaBiome);

            Span<short> yLevels = stackalloc short[256];
            GenerateYLevel(chunk.X, chunk.Z, yLevels);

            short maxY = 63;
            for (int i = 0; i < yLevels.Length; i++) 
            {
                if (yLevels[i] > maxY) maxY = yLevels[i];
            }

            maxY = (short)(RegionWriter.MathUtilities.SectionMod(maxY, 16) * 16 + 15);
            chunk.BlockCeiliingY = maxY;

            chunk.BlockStates = new ushort[(maxY + 64 + 1) * 256];
            Array.Fill(chunk.BlockStates, _groundBlock);

            bool isTaigaChunk = false;

            for (int z = 0; z < 16; z++) 
            {
                for (int x = 0; x < 16; x++)
                {
                    int index = x + z * 16;
                    short y = yLevels[index];

                    for (int thisY = maxY; thisY > y; thisY--) 
                    {
                        int i = index + 256 * (thisY + 64);
                        chunk.BlockStates[i] = thisY < 63 ? _waterBlock : _airBlock;
                    }

                    if (y > 61) {
                        chunk.BlockStates[index + 256 * (y + 64)] = _topBlock;
                        isTaigaChunk = true;
                    }
                }

                chunk.Status = "noise";

                if (!isTaigaChunk) 
                {
                    Array.Fill(chunk.Biomes, _oceanBiome);
                }
            }
        }

        private void GenerateYLevel(int chunkX, int chunkZ, Span<short> output) 
        {
            for (int z = 0; z < 16; z++) 
            {
                for (int x = 0; x < 16; x++)
                {
                    int xP = chunkX * 16 + x, yP = chunkZ * 16 + z;

                    float y = 60;
                    
                    float continentNoise = ContinentNoise.GetNoise(xP, yP);
                    if(continentNoise >= 0)
                    {
                        y += continentNoise * 50;

                        float inlandNoise = InlandHillNoise.GetNoise(xP, yP) * continentNoise;
                        y += inlandNoise * 20;

                        continentNoise += inlandNoise;
                    }

                    if (y > 62) 
                    {
                        y += Math.Min(continentNoise * 200 - 8, 150);
                    }

                    if (y > 90) 
                    {
                        y += Math.Min(continentNoise * (y - 90) * 2, 100);
                    }

                    if (y > 100)
                    {
                        y += Math.Min(continentNoise * (y - 100) * 1.5F, 50);
                    }

                    if (y > 70 && y < 200) 
                    {
                        float midAltitudeNoise = MidAltitudeNoise.GetNoise(xP, yP);
                        float delta;

                        if (y > 80)
                        {
                            delta = 1 - (y - 80) / 120;
                        }
                        else 
                        {
                            delta = 1 - (80 - y) / 10; 
                        }

                        y += delta * 70 * midAltitudeNoise;
                    }

                    if (y > 200) 
                    {
                        float rockNoise = RockNoise.GetNoise(xP, yP);
                        y += rockNoise * (Math.Min(y, 350) - 200);
                    }

                    if (y > 250)
                    {
                        float tallRockNoise = TallRockNoise.GetNoise(xP, yP);
                        y += tallRockNoise * (Math.Min(y, 300) - 250);
                    }

                    if (y >= 150 && y <= 300) 
                    {
                        float delta = (75 - Math.Abs(y - 225)) / 75;

                        float lowRockNoise = LowRockNoise.GetNoise(xP, yP);
                        y += lowRockNoise * delta * 50;
                    }

                    if (y < 62)
                    {
                        float oceanNoise = (continentNoise < 0 ? -continentNoise : continentNoise / 20) * (OceanNoise.GetNoise(xP, yP) - 1) * 100;
                        y += oceanNoise;
                    }

                    output[z * 16 + x] = (short)y;
                }
            }
        }
    }
}