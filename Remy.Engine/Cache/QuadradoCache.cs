using Remy.Engine.Enum;

namespace Remy.Engine.Cache
{
    public class QuadradoCache : ObjetoCache
    {
        public Dictionary<int, float[]> Vertices = new();

        public void SetVertices(float[] _vertices)
        {
            Vertices.Add(Vertices.Count + 1, _vertices);
        }

        public QuadradoCache() : base(TipoCache.Quadrado) { }
    }
}