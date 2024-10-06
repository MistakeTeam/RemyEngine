namespace Remy.Engine.Graficos.Texto
{
    public readonly struct Fonte
    {
        private const float tamanho_padrão_texto = 20;

        public string Familia { get; }
        public string Nome { get; }
        public float Tamanho { get; }
        public bool Italico { get; }
        public bool LarguraFixa { get; }
        public string FonteNome { get; }
        public Dictionary<uint, CaractereGlyph> Caracteres { get; }

        public Fonte(string nome, Dictionary<uint, CaractereGlyph> caracteres)
        {
            Nome = nome;
            Caracteres = caracteres;
        }
    }
}