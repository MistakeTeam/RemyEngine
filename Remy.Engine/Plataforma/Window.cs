using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Remy.Engine.Graficos.Interface.Containers;

namespace Remy.Engine.Plataforma
{
    public class Window : GameWindow
    {
        #region Configurações
        private VSyncMode Config_vsync = VSyncMode.On;
        public bool Debug = false;
        public string Titulo = string.Empty;
        #endregion

        internal static Vector2i Tamanho_Janela;

        protected internal Container Game = default;

        public Window() : base(GameWindowSettings.Default, new()
        {
            API = ContextAPI.OpenGL,
            // APIVersion = new Version(3, 3), // DEFINE QUAL VERSÃO DO OPENGL SERÁ USADO
            Vsync = VSyncMode.On,
        })
        {
            TamanhoMinimo(800, 600);
            // AlterarTamanho(1280, 720);
            VSync = Config_vsync;
        }

        public void AlterarTamanho(int largura, int altura)
        {
            ClientSize = (largura, altura);
        }

        public void TamanhoMinimo(int largura, int altura)
        {
            MinimumSize = (largura, altura);
        }

        public void InitWindow()
        {
            Run();
        }
    }
}