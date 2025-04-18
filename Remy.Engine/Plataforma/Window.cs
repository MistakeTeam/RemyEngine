using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Remy.Engine.Graficos.Interface.Containers;

namespace Remy.Engine.Plataforma
{
    public partial class Window
    {
        #region Configurações
        private VSyncMode Config_vsync = VSyncMode.On;
        public bool Debug = false;
        public string Titulo = string.Empty;
        #endregion


        private int LarguraMinJanela = 800;
        private int AlturaMinJanela = 600;
        private int LarguraJanela;
        private int AlturaJanela;


        protected internal Container Game = default;

        internal void Create(int largura, int altura)
        {
            LarguraJanela = largura;
            AlturaJanela = altura;

            ClientSize = (LarguraJanela, AlturaJanela); // DEFINE O TAMANHO DA JANELA
            MinimumSize = (LarguraMinJanela, AlturaMinJanela); // DEFINE UM LIMITE MINIMO PARA O TAMANHO DA JANELA
            VSync = Config_vsync;
        }
    }
}