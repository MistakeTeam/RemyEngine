using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Remy.Engine.Logs
{
    public class LogFile
    {
        private static StreamWriter FileLog;
        private static bool _iniciado = false;
        private static string _filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "//logs";
        private static string _filename;

        public LogFile()
        {
            _filename = string.Format("//{0}_log_{1}-{2}-{3}T{4}-{5}-{6}.txt",
               AppDomain.CurrentDomain.FriendlyName,
               DateTime.Now.Day,
               DateTime.Now.Month,
               DateTime.Now.Year,
               DateTime.Now.Hour,
               DateTime.Now.Minute,
               DateTime.Now.Second
            );

            if (!_iniciado)
            {
                if (!Directory.Exists(_filepath))
                {
                    Directory.CreateDirectory(_filepath);
                }

                FileLog = File.AppendText(_filepath + _filename);
            }

            _iniciado = true;
        }

        public static void WriteLine(string mensagem, params object?[] args) => WriteLine(string.Format(mensagem, args));
        public static void WriteLine(object mensagem) => WriteLine((string)mensagem);
        public static void WriteLine(string mensagem)
        {
            if (!_iniciado) return;

            StackTrace st = new StackTrace(0, true);
            StackFrame sf = st.GetFrame(1)!;

            // Volte uma casa
            if (sf.GetMethod()!.Name == "WriteLine")
            {
                sf = st.GetFrame(2)!;
            }

            string linha = $"[{DateTime.Now.ToLongTimeString()}] [{sf.GetMethod()!.DeclaringType!.Namespace}.{sf.GetMethod()!.DeclaringType!.Name}:{sf.GetMethod()!.Name}] {mensagem}\r\n";

            FileLog.Write(linha);
            Console.Write(linha);
        }

        public void Close() => FileLog.Close();
    }
}