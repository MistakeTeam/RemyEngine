using Remy.Engine.Logs;

namespace Remy.Engine.Core
{
    internal class GCena
    {
        internal static List<Cena> Cenas = new();

        internal static void AddCena(Cena _Cena)
        {
            LogFile.WriteLine($"Adicionando Cena {_Cena.ID}: {_Cena.Nome}");
            Cenas.Add(_Cena);
        }
    }
}