using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Remy.Engine.Logs
{
    public class Logger
    {
        private static bool Iniciado = false;
        private static readonly string Filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "//logs";
        private static string Filename;

        public Logger()
        {
            Filename = string.Format("{0}_log_{1}-{2}-{3}T{4}-{5}-{6}.log",
               AppDomain.CurrentDomain.FriendlyName,
               DateTimeOffset.Now.Day,
               DateTimeOffset.Now.Month,
               DateTimeOffset.Now.Year,
               DateTimeOffset.Now.Hour,
               DateTimeOffset.Now.Minute,
               DateTimeOffset.Now.Second
            );

            if (!Iniciado)
            {
                if (!Directory.Exists(Filepath))
                {
                    Directory.CreateDirectory(Filepath);
                }
            }

            Iniciado = true;
        }

        public static void WriteLine(string mensagem, params object?[] args) => WriteLine(string.Format(mensagem, args));
        public static void WriteLine(object mensagem) => WriteLine((string)mensagem);
        public static void WriteLine(string mensagem)
        {
            if (!Iniciado) return;

            IEnumerable<string> lines = mensagem.Replace(@"\r\n", @"\n").Split('\n');

            StackTrace st = new StackTrace(0, true);
            StackFrame sf = st.GetFrame(1)!;

            // Volte uma casa
            if (sf.GetMethod()!.Name == "WriteLine")
            {
                sf = st.GetFrame(2)!;
            }

            using (var writer = new StreamWriter(Filepath + "//" + Filename, true))
            {
                foreach (string line in lines)
                {
                    string output = $"[{DateTime.Now.ToLongTimeString()}] [{sf.GetMethod()!.DeclaringType!.Namespace}.{sf.GetMethod()!.DeclaringType!.Name}:{sf.GetMethod()!.Name}] {line.Trim()}";

                    writer.WriteLine(output);
                    Console.WriteLine(output);
                }

                writer.Close();
            }
        }
    }
}