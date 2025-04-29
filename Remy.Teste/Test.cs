using OpenTK.Mathematics;
using Remy.Engine;
using Remy.Engine.Core;
using Remy.Engine.Extensions;
using Remy.Engine.Graficos.Interface.Formas;
using Remy.Engine.Graficos.Texto;
using Remy.Engine.Input;
using Remy.Engine.Logs;

namespace Remy.Teste
{
    public class Test : Game
    {
        private Quadrado Quad;
        private ConstrutorTexto Texto_Teste;

        public Test() : base(new Cena("Main"))
        {
        }

        protected override void Load()
        {
            base.Load();

            Logger.WriteLine("Iniciando jogo");
            Add(Quad = new Quadrado
            {
                Posição = new(200, 0),
                Tamanho = new(100, 100)
            });

            Add(Texto_Teste = new ConstrutorTexto
            {
                Posição = new(20, 100),
                Cor = Color4.Crimson,
            });
        }

        protected override void Update()
        {
            base.Update();

            Texto_Teste.Texto = $"P: {Input.Mouse.Posição} // ({Texto_Teste.Altura}, {Texto_Teste.Largura})";
        }
    }
}