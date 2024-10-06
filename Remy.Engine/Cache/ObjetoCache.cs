using Remy.Engine.Enum;

namespace Remy.Engine.Cache
{
    public abstract class ObjetoCache
    {
        public string ID;
        public TipoCache Tipo;
        public int FameIndex;

        public ObjetoCache(TipoCache _tipo)
        {
            Tipo = _tipo;
            ID = $"{Tipo}_{new Random().Next(10000, 99999)}";
        }

        public void SetFrame(int _frame)
        {
            FameIndex = _frame;
        }
    }
}