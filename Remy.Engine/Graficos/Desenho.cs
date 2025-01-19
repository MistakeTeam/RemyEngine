using OpenTK.Mathematics;

namespace Remy.Engine.Graficos
{
    public abstract class Desenho : IDisposable
    {
        public abstract void Dispose(bool isDis);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private float x;
        private float y;

        public float X
        {
            get => x;
            set
            {
                if (x == value) return;
                x = value;
            }
        }

        public float Y
        {
            get => y;
            set
            {
                if (y == value) return;
                y = value;
            }
        }

        public Vector2 Posição
        {
            get => new Vector2(x, y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        private float largura;
        private float altura;

        public Vector2 Tamanho
        {
            get => new Vector2(largura, altura);
            set
            {
                largura = value.X;
                altura = value.Y;
            }
        }

        public Color4 Cor;

        public float[] Vertices;

        public abstract void Desenhar();
    }
}