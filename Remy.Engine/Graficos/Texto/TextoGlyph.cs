namespace Remy.Engine.Graficos.Texto
{
    public struct TextoGlyph
    {
        public readonly CaractereGlyph Glyph;

        public readonly char Caractere => Glyph.Caractere;
        public readonly float Largura => Glyph.Textura.Width * TamanhoTexto;
        public readonly float Altura => Glyph.Textura.Height * TamanhoTexto;
        private readonly float TamanhoTexto;
        private readonly float? LarguraFixa;

        internal TextoGlyph(CaractereGlyph glyph, float tamanhoTexto, float? larguraFixa = null)
        {
            LarguraFixa = larguraFixa;
            TamanhoTexto = tamanhoTexto;

            Glyph = glyph;
        }
    }
}