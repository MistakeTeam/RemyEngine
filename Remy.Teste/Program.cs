using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Remy.Engine;
using Remy.Engine.Core;
using Remy.Engine.Logs;

namespace Remy.Teste;

public class Program
{
    private static readonly NativeWindowSettings NWS = new()
    {
        Title = "Remy",
        API = ContextAPI.OpenGL,
        APIVersion = new Version(4, 5), // DEFINE QUAL VERSÃO DO OPENGL SERÁ USADO
        MinimumClientSize = (800, 600), // DEFINE UM LIMITE MINIMO PARA O TAMANHO DA JANELA
        ClientSize = (1280, 720), // DEFINE O TAMANHO DA JANELA
        Vsync = VSyncMode.On,
        AutoLoadBindings = true,
        IsEventDriven = false, // DEFINE SE A JANELA É ORIENTADA A EVENTOS, O QUE NÃO SERVE PARA JOGOS. JANELAS ORIENTADAS A EVENTOS NECESSITAM DE AÇÃO DOS USUARIOS PARA SE ATUALIZAR E RENDERIZAR.
        WindowBorder = WindowBorder.Resizable,
    };

    public static void Main(string[] args)
    {
        using (Game game = new(NWS))
        {
            game.AddCena(new Cena());

            game.Unload += delegate
            {
                LogFile.WriteLine("Desligando jogo");
            };

            game.Run(); /// A JANELA NASCE AQUI
        }
    }
}