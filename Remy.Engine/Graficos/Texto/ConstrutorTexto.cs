using OpenTK.Mathematics;
using Remy.Engine.Cache;
using Remy.Engine.IO;

namespace Remy.Engine.Graficos.Texto
{
    internal class ConstrutorTexto : Desenho
    {
        private Fonte FonteEmUso;

        private string Texto;
        private float Escala;

        public ConstrutorTexto(string text, float x, float y, float escala, Color4 cor, Vector2 startOffset = default)
        {
            Texto = text;
            Posição = new Vector2(x, y);
            Escala = escala;
            Cor = cor;

            FonteEmUso = GerenciarFontes.GetFonte();

            Desenhar();
        }

        public override void Desenhar()
        {
            float X = Posição.X;
            float Y = Posição.Y;

            TextoCache _txtCache = new();

            if ((Y + FonteEmUso.Caracteres['H'].Size.Y) > Game.Janela.Y)
            {
                Y -= Y + FonteEmUso.Caracteres['H'].Size.Y - Game.Janela.Y;
            }

            // Iterar por todos as letras
            foreach (char c in Texto)
            {
                if (FonteEmUso.Caracteres.ContainsKey(c) == false)
                    continue;

                CaractereGlyph ch = FonteEmUso.Caracteres[c];

                float xrel = X + ch.Bearing.X * Escala;
                float yrel = Y + (FonteEmUso.Caracteres['H'].Size.Y - ch.Bearing.Y) * Escala;

                float w = ch.Size.X * Escala;
                float h = ch.Size.Y * Escala;

                float[] vertices = [
                     xrel,         yrel + h,     0.0f, 1.0f,
                     xrel + w,     yrel,         1.0f, 0.0f,
                     xrel,         yrel,         0.0f, 0.0f,

                     xrel,         yrel + h,     0.0f, 1.0f,
                     xrel + w,     yrel + h,     1.0f, 1.0f,
                     xrel + w,     yrel,         1.0f, 0.0f
                ];

                _txtCache.SetVertices(c, vertices);

                // Agora avance os cursores para o próximo glifo (observe que o avanço é um número de 1/64 pixels)
                X += (ch.XAdvance >> 6) * Escala; // Bitshift por 6 para obter o valor em pixels (2 ^ 6 = 64 (divida a quantidade de 1/64 pixels por 64 para obter a quantidade de pixels))
            }

            Render.AddObjeto(_txtCache);
        }

        public override void Dispose(bool isDis)
        {
            throw new NotImplementedException();
        }
    }
}