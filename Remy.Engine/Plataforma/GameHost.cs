using System.Reflection;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using Remy.Engine.Core;
using Remy.Engine.Graficos;
using Remy.Engine.Graficos.Interface.Containers;
using Remy.Engine.Graficos.OpenGL;
using Remy.Engine.Logs;

namespace Remy.Engine.Plataforma
{
    public abstract class GameHost : Window, IDisposable
    {
        public IRenderer Renderer { get; private set; }

        private Logger Logger;

        protected GameHost(string gamename)
        {
            Titulo = gamename;
        }

        internal Container Root { get; private set; }

        public void Run(Game game)
        {
            Logger = new Logger();

            Logger.WriteLine("Iniciando Remy");
            Logger.WriteLine(Environment.CurrentDirectory);

            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                Logger.WriteLine("[ProcessExit] Remy foi forçado a fechar.");
            };

            Root = new Container("SafeArea");
            Root.Child = game;

            game.SetHost(this);

            Renderer = new GLRenderer();

            game.Load();

            // BackgroundLoaderAttribute.Invoke();

            Game = Root;

            try
            {
                InitWindow();
            }
            catch (Exception e)
            {
                Logger.WriteLine(e.ToString());
            }
        }


        #region Inplementações dos eventos
        protected override void OnLoad()
        {
            base.OnLoad();

            // BackgroundLoaderAttribute.Invoke();

            Renderer.Initialise("gl");

            int i = 0;
            foreach (
                Type component in Assembly.GetEntryAssembly()!.GetTypes().Where(t =>
                {
                    return t != null && t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Comportamento));
                })
            )
            {
                i++;
                Comportamento cc = (Comportamento)Activator.CreateInstance(component)!;
            }

            Tamanho_Janela = new(Size.X, Size.Y - (Bounds.Size.Y - ClientRectangle.Size.Y));

            BaseComportamento.Start();
            Logger.WriteLine($"{i} Classes de Comportamento foram carregadas!");
            Logger.WriteLine("OnLoad completo ✅");
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (Debug)
                Title = $"{Titulo} - Resolução: {Tamanho_Janela.X}x{Tamanho_Janela.Y} {API}: {APIVersion}/{Profile} (Vsync: {VSync}) {UpdateFrequency} FPS: {1f / e.Time:0}";
            else
                Title = $"{Titulo}";

            Root.UpdateSubTree();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            Root.UpdateDrawSubTree();

            SwapBuffers(); // Aqui o quadro será pintado na tela
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            Tamanho_Janela = new(ClientSize.X, ClientSize.Y);

            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        }
        protected override void OnUnload()
        {
            base.OnUnload(); // Executa as funções delegate atribuidas ao Unload

            Logger.WriteLine("Remy está desligando");

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
        #endregion

        #region IDisposable Support

        private bool isDisposed;

        protected new virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            isDisposed = true;

            Root?.Dispose();
            Root = null;
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}