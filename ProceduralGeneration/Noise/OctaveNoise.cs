namespace ProceduralGeneration
{
    public class OctaveNoise : INoise
    {
        public INoise Noise { get; set; }

        public float Ampltitude { get; set; }
        public float Scale { get; set; }
        public bool Clamp { get; set; }

        public OctaveNoise(INoise noise, float amplitude)
        {
            Noise = noise;
            Ampltitude = amplitude;
            Scale = 1;
            Clamp = false;
        }
        public OctaveNoise(INoise noise, float amplitude, bool clamp = false)
        {
            Noise = noise;
            Ampltitude = amplitude;
            Scale = 1;
            Clamp = clamp;
        }
        public OctaveNoise(INoise noise, float amplitude, float scale = 1, bool clamp = false) 
        {
            Noise = noise;
            Ampltitude = amplitude;
            Scale = scale;
            Clamp = clamp;
        }

        public float GetNoise(float x, float y)
        {
            float output =  Noise.GetNoise(x * Scale, y * Scale) * Ampltitude;
            if (Clamp) output = Math.Clamp(output, -1, 1);
            
            return output;
        }
    }
}
