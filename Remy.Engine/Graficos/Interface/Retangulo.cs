using OpenTK.Graphics.OpenGL4;
using Remy.Engine.Graficos.OpenGL;
using Remy.Engine.Logs;
using Remy.Engine.Utility;

namespace Remy.Engine.Graficos.Interface
{
    public class Retangulo : IDisposable
    {
        private BufferObject<float> VBO;
        private ArrayObject VAO;
        private BufferObject<uint> EBO;
        private Shader shader;

        uint[] indices =
        {  // note that we start from 0!
            0, 1, 3,   // first triangleks
            1, 2, 3    // second triangle
        };

        public Retangulo(float largura, float altura, float x, float y, string tex)
        {
            if (y + altura > Game.Janela.Y)
                y -= y + altura - Game.Janela.Y;

            if (y < altura)
                y -= y - altura;

            if (x + largura > Game.Janela.X)
                x -= x + largura - Game.Janela.X;

            float nX = Converter.Intervalo(x, 0, Game.Janela.X, -1, 1);
            float LnX = Converter.Intervalo(x + largura, 0, Game.Janela.X, -1, 1);
            float nY = Converter.Intervalo(y, 0, Game.Janela.Y, -1, 1);
            float LnY = Converter.Intervalo(y + altura, 0, Game.Janela.Y, -1, 1);

            Console.WriteLine($"{nX}/{LnX}/{nY}/{LnY}");

            float[] vertices =
            {
            //  X       Y       R       G       B
                LnX,    nY,     1.0f,   0.0f,   0.0f,  // top right
                LnX,    LnY,    0.0f,   1.0f,   0.0f,  // bottom right
                nX,     LnY,    0.5f,   0.0f,   0.5f,  // bottom left
                nX,     nY,     0.0f,   0.5f,   1.0f,  // top left
            };

            VAO = new(5 * sizeof(float));

            VBO = new(vertices.Length, BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
            VBO.SetData(vertices);

            EBO = new(indices.Length, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw);
            EBO.SetData(indices);

            shader = new Shader("Recursos/Shaders/UIBase.vert", "Recursos/Shaders/UIBase.frag");
            shader.Use();

            VAO.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0);
            VAO.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 2 * sizeof(float));
        }

        public void desenhar()
        {
            VAO.Bind();
            shader.Use();
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }


        /// DISPOSED

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                VBO.Dispose();
                VAO.Dispose();
                EBO.Dispose();
                disposedValue = true;
            }
        }

        ~Retangulo()
        {
            Dispose();

            if (disposedValue == false)
            {
                LogFile.WriteLine("Vazamento de recurso de GPU! Você esqueceu de chamar Dispose()?");
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}