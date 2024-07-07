using Remy.Logs;

namespace Remy
{
    public class Eventos
    {
        public static void Iniciar()
        {
            LogFile.WriteLine("Iniciando eventos...");
        }

        // public static void OnProcessExit(Action m) => OnProcessExit(new object(), EventArgs => m());
        public static void OnProcessExit(Action<object, EventArgs> m)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(m);
        }
    }
}