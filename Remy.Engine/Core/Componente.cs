using Remy.Engine.Logs;

namespace Remy.Engine.Core
{
    public partial class Componente
    {

    }

    internal class BaseComponente<T> where T : Componente
    {
        public static readonly List<T> Components = new();

        public static void Register(T component)
        {
            if (!Components.Contains(component))
            {
                LogFile.WriteLine("Regstrando componente " + component.GetType().Name);
                Components.Add(component);
            }
            else
            {
                LogFile.WriteLine("Componente já foi adicionado.");
            }
        }
    }
}