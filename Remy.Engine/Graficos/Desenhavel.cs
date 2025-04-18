using OpenTK.Mathematics;
using Remy.Engine.Graficos.Interface.Containers;
using Remy.Engine.Graficos.OpenGL.Buffers;
using Remy.Engine.Graficos.OpenGL.Shaders;

namespace Remy.Engine.Graficos
{
    public abstract class Desenhavel : IDisposable
    {
        protected virtual void Dispose(bool isDisposing)
        {
            if (IsDisposed)
                return;

            parent = null;

            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected internal bool IsDisposed { get; private set; }

        #region Posição e Tamanho

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

        private Vector2 posição
        {
            get => new(x, y);
            set
            {
                x = value.X;
                y = value.Y;
            }
        }

        public Vector2 Posição
        {
            get => posição;
            set
            {
                if (posição == value) return;
                posição = value;
            }
        }

        private float largura;
        private float altura;

        public float Largura
        {
            get => largura;
            set
            {
                if (largura == value) return;
                largura = value;
            }
        }

        public float Altura
        {
            get => altura;
            set
            {
                if (altura == value) return;
                altura = value;
            }
        }

        private Vector2 tamanho
        {
            get => new(largura, altura);
            set
            {
                largura = value.X;
                altura = value.Y;
            }
        }

        public Vector2 Tamanho
        {
            get => tamanho;
            set
            {
                if (tamanho == value) return;
                tamanho = value;
            }
        }

        #endregion

        private CompositorDesenhavel parent;

        /// <summary>
        /// The parent of this drawable in the scene graph.
        /// </summary>
        public CompositorDesenhavel Parent
        {
            get => parent;
            internal set
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);

                if (value == null)
                    return;

                if (parent == value) return;

                if (parent != null)
                    throw new InvalidOperationException("May not add a drawable to multiple containers.");

                parent = value;
            }
        }

        public Color4 Cor = Color4.White;


        #region OpenGL

        protected internal uint[] Indices;
        protected internal BufferObject<float> VBO;
        protected internal ArrayObject VAO;
        protected internal BufferObject<uint> EBO;
        protected internal Shader shader;
        #endregion

        protected virtual void Update() { }

        protected virtual void UpdateDraw() { }

        public virtual bool UpdateSubTree()
        {
            Update();
            return true;
        }

        public virtual bool UpdateDrawSubTree()
        {
            UpdateDraw();
            return true;
        }
    }
}