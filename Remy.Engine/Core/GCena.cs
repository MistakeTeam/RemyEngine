namespace Remy.Engine.Core
{
    internal class GCena
    {
        internal static List<Cena> Cenas = new();

        internal static void AddCena(Cena _Cena)
        {
            Cenas.Add(_Cena);
        }
    }
}