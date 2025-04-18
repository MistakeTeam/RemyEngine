using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Remy.Engine.Graficos.OpenGL.Shaders;
using Remy.Engine.Plataforma;

namespace Remy.Engine.Graficos.Interface.Formas
{
    public class Quadrado : Desenhavel
    {
        protected internal float[] Vertices;

        public Quadrado()
        {
            Indices =
            [  // note that we start from 0!
                0, 1, 3,   // first triangleks
                1, 2, 3    // second triangle
            ];
        }

        protected override void Update()
        {
            base.Update();

            if ((Y + Altura) > Window.Tamanho_Janela.Y)
                Y -= Y + Altura - Window.Tamanho_Janela.Y;

            if (Y < 0)
                Y = 0;

            if (X + Largura > Window.Tamanho_Janela.X)
                X -= X + Largura - Window.Tamanho_Janela.X;

            if (X < 0)
                X = 0;

            Vertices =
            [// X            Y           R        G        B
                X+Largura,   Y,          Cor.R,   Cor.G,   Cor.B,  // top right
                X+Largura,   Y+Altura,   Cor.R,   Cor.G,   Cor.B,  // bottom right
                X,           Y+Altura,   Cor.R,   Cor.G,   Cor.B,  // bottom left
                X,           Y,          Cor.R,   Cor.G,   Cor.B,  // top left
            ];

            VAO = new(5 * sizeof(float));

            VBO = new(Vertices.Length, BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
            VBO.SetData(Vertices);

            EBO = new(Indices.Length, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw);
            EBO.SetData(Indices);

            VAO.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0);
            VAO.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 2 * sizeof(float));

            shader = new Shader("Recursos/Shaders/UIBase.vert", "Recursos/Shaders/UIBase.frag");
            shader.Use();

            Matrix4 projectionM = new Matrix4();
            projectionM = Matrix4.CreateScale(new Vector3(1f / Window.Tamanho_Janela.X, 1f / Window.Tamanho_Janela.Y, 1.0f));
            projectionM = Matrix4.CreateOrthographicOffCenter(0.0f, Window.Tamanho_Janela.X, Window.Tamanho_Janela.Y, 0.0f, -1.0f, 1.0f);

            shader.SetUniform("projection", projectionM);
        }

        protected override void UpdateDraw()
        {
            base.UpdateDraw();

            VAO.Bind();
            shader.Use();
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        /// DISPOSED

        protected override void Dispose(bool isDisposing)
        {
            VBO.Dispose();
            VAO.Dispose();
            EBO.Dispose();
            shader.Dispose();

            VBO = null;
            VAO = null;
            EBO = null;
            shader = null;

            base.Dispose(isDisposing);
        }
    }

    public class Sprite : Desenhavel
    {
        public Sprite() { }
    }
}