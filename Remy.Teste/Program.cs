using Remy.Engine;
using Remy.Engine.Plataforma;

namespace Remy.Teste;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        using (GameHost host = Host.ChooseOSHost(@"sample-game"))
        using (Game game = new Test())
            host.Run(game);
    }
}