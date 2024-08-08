namespace Remy.Engine.Graficos.Texto
{
    public class Fonte
    {
        public string Nome;
        public Dictionary<uint, Character> Characters = [];
        public Fonte(string _nome, Dictionary<uint, Character> _characters)
        {
            Nome = _nome;
            Characters = _characters;
        }
    }
}