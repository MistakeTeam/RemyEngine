using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Remy.Engine.Graficos.OpenGL;
using Remy.Engine.Input;
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
            Color4 Cor = Color4.DarkSalmon;

            if ((y + altura) > Game.Janela.Y)
                y -= y + altura - Game.Janela.Y;

            if (y < 0)
                y = 0;

            if (x + largura > Game.Janela.X)
                x -= x + largura - Game.Janela.X;

            if (x < 0)
                x = 0;

            float nX = Converter.Intervalo(x, 0, Game.Janela.X, -1, 1);
            float LnX = Converter.Intervalo(x + largura, 0, Game.Janela.X, -1, 1);
            float nY = Converter.Intervalo(y, 0, Game.Janela.Y, -1, 1);
            float LnY = Converter.Intervalo(y + altura, 0, Game.Janela.Y, -1, 1);

            Console.WriteLine($"{x}/{x + largura}/{y}/{y + altura}");

            float[] vertices =
            {
            //  X       Y       R       G       B
                LnX,    -nY,    Cor.R,  Cor.G,  Cor.B,  // top right
                LnX,    -LnY,   Cor.R,  Cor.G,  Cor.B,  // bottom right
                nX,     -LnY,   Cor.R,  Cor.G,  Cor.B,  // bottom left
                nX,     -nY,    Cor.R,  Cor.G,  Cor.B,  // top left
            };

            VAO = new(5 * sizeof(float));

            VBO = new(vertices.Length, BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
            VBO.SetData(vertices);

            EBO = new(indices.Length, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw);
            EBO.SetData(indices);

            VAO.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0);
            VAO.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 2 * sizeof(float));

            // Matrix4 projectionM = Matrix4.CreateScale(new Vector3(1f / Game.Janela.X, 1f / Game.Janela.Y, 1.0f));
            // projectionM = Matrix4.CreateOrthographicOffCenter(0.0f, Game.Janela.X, Game.Janela.Y, 0.0f, -1.0f, 1.0f);

            // VAO.SetUniformMatrix4(2, projectionM);

            shader = new Shader("Recursos/Shaders/UIBase.vert", "Recursos/Shaders/UIBase.frag");
            shader.Use();

            Mouse.MouseEvent += delegate
            {
                if (Mouse.Posição.X > x && Mouse.Posição.Y > y)
                {
                    if (Mouse.Posição.X < (x + largura) && Mouse.Posição.Y < (y + altura))
                    {
                        Console.WriteLine("TESTE011001");
                    }
                }
            };
        }

        public void desenhar()
        {
            VAO.Bind();
            shader.Use();
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }


        /// DISPOSED

        public void Dispose()
        {
            VBO.Dispose();
            VAO.Dispose();
            EBO.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}