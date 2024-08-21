namespace Remy.Engine.Graficos
{
    public class ObjetoCache
    {
        public string ID;
        public TipoObjeto Tipo;
        public int FameIndex;
        public Dictionary<int, (char, float[,])> Vertices = new();

        public ObjetoCache(TipoObjeto _tipo)
        {
            Tipo = _tipo;
            ID = $"{Tipo}_{new Random().Next(10000, 99999)}";
        }

        public void SetFrame(int _frame)
        {
            FameIndex = _frame;
        }

        public void SetVertices(char key, float[,] _vertices)
        {
            Vertices.Add(Vertices.Count + 1, (key, _vertices));
        }
    }

    public enum TipoObjeto
    {
        Quadrado,
        Texto,
        Textura
    }
}