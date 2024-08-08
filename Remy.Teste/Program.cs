using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Remy.Engine;
using Remy.Engine.Core;

namespace Remy.Teste;

public class Program
{
    private static readonly NativeWindowSettings NWS = new()
    {
        Title = "Remy",
        ClientSize = (800, 600), /// DEFINE O TAMANHO DA JANELA
        MinimumClientSize = (800, 600), /// DEFINE UM LIMITE MINIMO PARA O TAMANHO DA JANELA
        APIVersion = new Version(4, 5), /// DEFINE QUAL VERSÃO DO OPENGL SERÁ USADO
        Vsync = VSyncMode.On,
    };

    public static void Main(string[] args)
    {
        using (Game game = new(NWS))
        {
            game.AddCena(new Cena());

            game.Run(); /// A JANELA NASCE AQUI
        }
    }
}