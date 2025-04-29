using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Remy.Engine.Input;
using Remy.Engine.IO;
using Remy.Engine.Graficos.Interface.Containers;
using Remy.Engine.Plataforma;
using Remy.Engine.Core;

namespace Remy.Engine
{
    public abstract partial class Game : Container
    {
        // Propriedades da engine
        internal static Vector2i Janela;
        internal static string Titulo;
        internal static IReadOnlyList<JoystickState> Joysticks;
        private GerenciarFontes Fontes;
        public InputControl Input;

        protected GameHost Host { get; private set; }

        protected Cena Cena0;


        public Game(Cena _cena) : base("Game")
        {
            Cena0 = _cena;
        }


        protected internal virtual void Load()
        {
            Fontes = new GerenciarFontes();

            Input = new InputControl(Host);
        }

        public void SetHost(GameHost host)
        {
            Host = host;
        }

        protected override void Update()
        {
            Input.Update();
        }
    }
}