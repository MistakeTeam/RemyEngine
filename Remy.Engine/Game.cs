using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Remy.Engine.Input;
using Remy.Engine.IO;
using Remy.Engine.Graficos.Interface.Containers;
using Remy.Engine.Plataforma;

namespace Remy.Engine
{
    public abstract partial class Game : Container
    {
        // Propriedades da engine
        internal static Vector2i Janela;
        internal static string Titulo;
        internal static IReadOnlyList<JoystickState> Joysticks;
        private GerenciarFontes Fontes;
        private InputControl InputControl;

        protected GameHost Host { get; private set; }


        public Game() : base("Game") { }


        protected internal virtual void Load()
        {
            Fontes = new GerenciarFontes();

            InputControl = new InputControl(Host.Window);
        }

        public void SetHost(GameHost host)
        {
            Host = host;
        }

        protected override void Update()
        {
            InputControl.Update();
        }
    }
}