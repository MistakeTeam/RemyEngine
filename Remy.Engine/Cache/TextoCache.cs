using Remy.Engine.Enum;

namespace Remy.Engine.Cache
{
    public class TextoCache : ObjetoCache
    {
        public Dictionary<int, (char, float[])> Vertices = new();

        public void SetVertices(char key, float[] _vertices)
        {
            Vertices.Add(Vertices.Count + 1, (key, _vertices));
        }

        public TextoCache() : base(TipoCache.Texto) { }
    }
}