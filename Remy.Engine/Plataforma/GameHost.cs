using OpenTK.Windowing.Common;
using Remy.Engine.Graficos;
using Remy.Engine.Graficos.Interface.Containers;
using Remy.Engine.Graficos.OpenGL;
using Remy.Engine.Logs;

namespace Remy.Engine.Plataforma
{
    public abstract class GameHost : IDisposable
    {
        public Window Window { get; private set; }
        public IRenderer Renderer { get; private set; }

        #region Propriedades de eventos Action
        public event Action Load;
        #endregion


        private string Titulo;
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

            Window = new Window();
            Window.Titulo = Titulo;

            game.Load();

            // BackgroundLoaderAttribute.Invoke();

            Window.Game = Root;

            try
            {
                if (Window != null)
                {
                    // Load += game.Start;

                    Window.Load += OnLoad;
                    Window.UpdateFrame += OnUpdateFrame;
                    Window.RenderFrame += OnRenderFrame;
                    Window.Unload += OnUnLoad;


                    ////////////////////////////////////////////
                    // LISTA DE EVENTOS (ainda não utilizados) DISOPONIVEIS NO OPENTK
                    // Window.Closing
                    // Window.FocusedChanged
                    // Window.FramebufferResize
                    // Window.JoystickConnected
                    // Window.KeyDown
                    // Window.KeyUp
                    // Window.Maximized
                    // Window.Minimized
                    // Window.MouseDown
                    // Window.MouseEnter
                    // Window.MouseLeave
                    // Window.MouseMove
                    // Window.MouseUp
                    // Window.MouseWheel
                    // Window.Move
                    // Window.Refresh
                    // Window.Resize
                    // Window.TextInput
                    ///////////////////////////////////////////

                    Window.Run();
                }
            }
            catch (Exception e)
            {
                Logger.WriteLine(e.ToString());
            }
        }


        #region Inplementações dos eventos
        private void OnLoad()
        {
            Load?.Invoke();

            Renderer.Initialise("gl");

            Logger.WriteLine("Load base completa");
        }

        private void OnUpdateFrame(FrameEventArgs e)
        {
            Root.UpdateSubTree();
        }
        private void OnRenderFrame(FrameEventArgs e)
        {
            Root.UpdateDrawSubTree();

            Window.SwapBuffers(); // Aqui o quadro será pintado na tela
        }
        private void OnUnLoad() { }
        #endregion

        #region IDisposable Support

        private bool isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            isDisposed = true;

            Root?.Dispose();
            Root = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}