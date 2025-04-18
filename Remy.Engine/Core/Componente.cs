using Remy.Engine.Logs;

namespace Remy.Engine.Core
{
    public partial class Componente
    {
        public Objeto Pai;
    }

    internal class BaseComponente<T> where T : Componente
    {
        public static readonly List<T> Components = new();

        public static void Register(T component)
        {
            if (!Components.Contains(component))
            {
                Logger.WriteLine("Regstrando componente " + component.GetType().Name);
                Components.Add(component);
            }
            else
            {
                Logger.WriteLine("Componente já foi adicionado.");
            }
        }
    }
}