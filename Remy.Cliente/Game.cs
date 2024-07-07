using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Remy.Cliente.Graficos;
using Remy.Cliente.Graficos.OpenGL;
using Remy.Cliente.Graficos.Render;
using Remy.Cliente.UI;

namespace Remy.Cliente
{
    public class Game(NativeWindowSettings nws) : GameWindow(GameWindowSettings.Default, nws)
    {
        // float[] vertices =
        // {
        //     0.5f,  0.5f, 0.0f,  // top right
        //     0.5f, -0.5f, 0.0f,  // bottom right
        //     -0.5f, -0.5f, 0.0f,  // bottom left
        //     -0.5f,  0.5f, 0.0f   // top left
        // };

        // uint[] indices =
        // {  // note that we start from 0!
        //     0, 1, 3,   // first triangleks
        //     1, 2, 3    // second triangle
        // };


        // private VertexBufferObject<float> VertexBufferObject;
        // private VertexArrayObject VertexArrayObject;
        // private ElementBufferObject<uint> ElementBufferObject;
        // private Shader shader;
        // private Shader text_prog;

        private Fonte Fonte;
        private Retangulo Retangulo;

        public static Vector2i Janela { get; private set; }


        // Evento é garregado quando o OpenTK inicia o aplicativo
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.3f, 0.4f, 0.5f, 1.0f);

            //Code goes here

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);


            /// DESENHAR UMA FONTE
            // text_prog = new Shader("Recursos/Shaders/texto.vert", "Recursos/Shaders/texto.frag");
            // text_prog.Use();

            Fonte = new Fonte();

            // Matrix4 projectionM = Matrix4.CreateOrthographic(this.ClientSize.X, this.ClientSize.Y, -1.0f, 1.0f);
            Matrix4 projectionM = Matrix4.CreateOrthographicOffCenter(0.0f, this.ClientSize.X, 0.0f, this.ClientSize.Y, -1.0f, 1.0f);

            GL.UniformMatrix4(1, false, ref projectionM);
            /// DESENHAR UMA FONTE


            Retangulo = new(10f, 10f, 5f, 20f);



            // /// DESEMHA UM TRIANGULO
            // VertexBufferObject = new VertexBufferObject<float>(vertices.Length, BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
            // VertexBufferObject.SetData(vertices, 0, vertices.Length);

            // VertexArrayObject = new VertexArrayObject(3 * sizeof(float));
            // VertexArrayObject.Bind();
            // VertexArrayObject.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0);

            // ElementBufferObject = new ElementBufferObject<uint>(indices.Length, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw);
            // ElementBufferObject.SetData(indices, 0, indices.Length);

            // shader = new Shader("Recursos/Shaders/TRIANGLE_VERTEX.vert", "Recursos/Shaders/TRIANGLE_COLOR.frag");
            // shader.Use();
            /// DESEMHA UM TRIANGULO
        }

        // Em seguida vem o update
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        // logo após, vem o render
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Code goes here.
            Fonte.RenderText("This is sample text", 25.0f, 25.0f, 1.0f, Color4.Crimson);

            Fonte.RenderText("(C) LearnOpenGL.com", 540.0f, 530.0f, 0.5f, Color4.HotPink);

            /// MUDAR O TITULO DA JANELA
            Title = $"Remy: Resolução: {Size.X}x{Size.Y} OpenGL: {GL.GetString(StringName.Version)} (Vsync: {VSync}) FPS: {1f / e.Time:0}";


            Retangulo.desenhar();

            /// DESEMHA UM TRIANGULO
            // shader.Use();

            // VertexArrayObject.Bind();

            // GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            /// DESEMHA UM TRIANGULO

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);

            Janela = Size;
        }

        // Evento é carregado quando o OpenTK encerra o aplicativo
        protected override void OnUnload()
        {
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // GL.DeleteProgram(shader.Handle);

            base.OnUnload();
        }
    }
}