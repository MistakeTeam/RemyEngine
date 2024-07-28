using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Remy.Cliente.Graficos.OpenGL;
using Remy.Cliente.Graficos.Render;
using Remy.Cliente.Input;
using Remy.Cliente.Utility;

namespace Remy.Cliente.UI
{
    public class Retangulo
    {
        private VertexBufferObject<float> VertexBufferObject;
        private VertexArrayObject VertexArrayObject;
        private ElementBufferObject<uint> ElementBufferObject;
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
            [
            //   X       Y       Z
                LnX,    nY,    0.0f, // top right
                LnX,    LnY,   0.0f, // bottom right
                nX,     LnY,   0.0f, // bottom left
                nX,     nY,    0.0f, // top left
            ];

            VertexBufferObject = new(vertices.Length, BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
            VertexBufferObject.SetData(vertices, 0, vertices.Length);

            VertexArrayObject = new VertexArrayObject(3 * sizeof(float));
            VertexArrayObject.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0);

            ElementBufferObject = new ElementBufferObject<uint>(indices.Length, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw);
            ElementBufferObject.SetData(indices, 0, indices.Length);

            shader = new Shader("Recursos/Shaders/TRIANGLE_VERTEX.vert", "Recursos/Shaders/TRIANGLE_COLOR.frag");
            shader.Use();

            void mouseE()
            {
                if ((InputControl.MousePosition.X > nX) && (InputControl.MousePosition.X < LnX) &&
                    (InputControl.MousePosition.Y > nY) && (InputControl.MousePosition.Y < LnY))
                {
                    Console.WriteLine($"Mouse {InputControl.MousePosition.X}/{InputControl.MousePosition.Y} está sobre o botão!!!");
                    if (InputControl.Mouse.IsButtonDown(MouseButton.Left))
                    {
                        Console.WriteLine(tex);
                    }
                }
            }

            InputControl.MouseEvent += mouseE;
        }

        public void desenhar()
        {
            shader.Use();
            GL.Uniform4(1, new Vector4(1.0f, 0.5f, 0.2f, 1.0f));

            VertexArrayObject.Bind();

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}