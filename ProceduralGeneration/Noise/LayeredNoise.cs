namespace ProceduralGeneration
{
    public class LayeredNoise : INoise
    {
        public IList<INoise> Layers { get; set; }

        public LayeredNoise() 
        {
            Layers = new List<INoise>();
        }

        public float GetNoise(float x, float y)
        {
            float result = 0;

            for (int i = 0; i < Layers.Count; i++) 
            {
                result += Layers[i].GetNoise(x, y);
            }

            return Math.Clamp(result, -1, 1);
        }

        public LayeredNoise AddNoise(INoise noise) 
        {
            Layers.Add(noise);
            return this;
        }
    }
}
