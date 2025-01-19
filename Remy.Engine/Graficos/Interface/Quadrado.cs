using OpenTK.Mathematics;
using Remy.Engine.Cache;
using Remy.Engine.Utility;

namespace Remy.Engine.Graficos.Interface
{
    public class Quadrado : Desenho
    {
        public Quadrado(Vector2 posição, Vector2 tamanho, Color4 cor)
        {
            Posição = posição;
            Tamanho = tamanho;
            Cor = cor;
        }

        public override void Desenhar()
        {
            QuadradoCache Cache = new();

            if ((Y + Tamanho.Y) > Game.Janela.Y)
                Y -= Y + Tamanho.Y - Game.Janela.Y;

            if (Y < 0)
                Y = 0;

            if (X + Tamanho.X > Game.Janela.X)
                X -= X + Tamanho.X - Game.Janela.X;

            if (X < 0)
                X = 0;

            float nX = Converter.Intervalo(X, 0, Game.Janela.X, -1, 1);
            float LnX = Converter.Intervalo(X + Tamanho.X, 0, Game.Janela.X, -1, 1);
            float nY = Converter.Intervalo(Y, 0, Game.Janela.Y, -1, 1);
            float LnY = Converter.Intervalo(Y + Tamanho.Y, 0, Game.Janela.Y, -1, 1);

            Vertices =
            [
            //  X       Y       R       G       B
                LnX,    -nY,    Cor.R,  Cor.G,  Cor.B,  // top right
                LnX,    -LnY,   Cor.R,  Cor.G,  Cor.B,  // bottom right
                nX,     -LnY,   Cor.R,  Cor.G,  Cor.B,  // bottom left
                nX,     -nY,    Cor.R,  Cor.G,  Cor.B,  // top left
            ];

            Cache.SetVertices(Vertices);
        }

        public override void Dispose(bool isDis)
        {
            throw new NotImplementedException();
        }
    }
}