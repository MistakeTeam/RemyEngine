using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Remy.Engine.Extensions;
using Remy.Engine.Logs;

namespace Remy.Engine.Plataforma
{
    public partial class Window : GameWindow
    {
        internal static Vector2i Tamanho_Janela;

        public Window(int largura = 1280, int altura = 720) : base(GameWindowSettings.Default, new()
        {
            API = ContextAPI.OpenGL,
            // APIVersion = new Version(3, 3), // DEFINE QUAL VERSÃO DO OPENGL SERÁ USADO
            Vsync = VSyncMode.On,
        })
        {
            Create(largura, altura);
        }

        // Evento é garregado quando o OpenTK inicia o aplicativo
        protected override void OnLoad()
        {
            base.OnLoad();

            // BackgroundLoaderAttribute.Invoke();

            Tamanho_Janela = new(Size.X, Size.Y - (Bounds.Size.Y - ClientRectangle.Size.Y));
        }

        // Em seguida vem o update
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (Debug)
                Title = $"{Titulo} - Resolução: {Tamanho_Janela.X}x{Tamanho_Janela.Y} {API}: {APIVersion}/{Profile} (Vsync: {VSync}) {UpdateFrequency} FPS: {1f / e.Time:0}";
            else
                Title = $"{Titulo}";
        }

        // logo após, vem o render
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // SwapBuffers(); // Aqui o quadro será pintado na tela


        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            Tamanho_Janela = new(ClientSize.X, ClientSize.Y);

            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        }

        // Evento é carregado antes do aplicativo ser encerrado
        protected override void OnUnload()
        {
            base.OnUnload(); // Executa as funções delegate atribuidas ao Unload

            Logger.WriteLine("Remy está desligando");

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}