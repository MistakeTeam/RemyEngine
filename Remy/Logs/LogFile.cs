using System.Diagnostics;
using System.Reflection;
using System.Text;
using Remy.Extra;

namespace Remy.Logs
{
    public static class LogFile
    {
        private static bool _iniciado = false;
        private static string _filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "//logs";
        private static string _filename;
        private static readonly string projectFolder = Path.GetFullPath(Assembly.GetExecutingAssembly().Location);

        // Source code: https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Scripting/StackTrace.cs
        [System.Security.SecuritySafeCritical] // System.Diagnostics.StackTrace cannot be accessed from transparent code (PSM, 2.12)
        static internal string ExtractFormattedStackTrace(StackTrace stackTrace)
        {
            StringBuilder sb = new StringBuilder(255);
            int iIndex;

            for (iIndex = 0; iIndex < stackTrace.FrameCount; iIndex++)
            {
                StackFrame frame = stackTrace.GetFrame(iIndex);
                if (frame == null)
                    continue;

                MethodBase mb = frame.GetMethod();
                if (mb == null)
                    continue;

                Type classType = mb.DeclaringType;
                if (classType == null)
                    continue;

                // Add namespace.classname:MethodName
                String ns = classType.Namespace;
                if (!string.IsNullOrEmpty(ns))
                {
                    sb.Append(ns);
                    sb.Append(".");
                }

                sb.Append(classType.Name);
                sb.Append(":");
                sb.Append(mb.Name);
                sb.Append("(");

                // Add parameters
                int j = 0;
                ParameterInfo[] pi = mb.GetParameters();
                bool fFirstParam = true;
                while (j < pi.Length)
                {
                    if (fFirstParam == false)
                        sb.Append(", ");
                    else
                        fFirstParam = false;

                    sb.Append(pi[j].ParameterType.Name);
                    j++;
                }
                sb.Append(")");

                // Add path name and line number - unless it is a Debug.Log call, then we are only interested
                // in the calling frame.
                string path = frame.GetFileName();
                if (path != null)
                {
                    sb.Append(" (at ");

                    if (!string.IsNullOrEmpty(projectFolder))
                    {
                        if (path.Replace("\\", "/").StartsWith(projectFolder))
                        {
                            path = path.Substring(
                                projectFolder.Length,
                                path.Length - projectFolder.Length
                            );
                        }
                    }

                    sb.Append(path);
                    sb.Append(":");
                    sb.Append(frame.GetFileLineNumber().ToString());
                    sb.Append(")");
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }

        public static void Iniciar(string nome)
        {
            _filename = string.Format("//{0}_log_{1}-{2}-{3}T{4}-{5}-{6}.txt",
                nome,
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

                using StreamWriter _w = File.AppendText(_filepath + _filename);
                _w.Close();
            }

            _iniciado = true;
        }

        private static void Log(string texto, string Level, params object[] args)
        {
            StackTrace trace = new StackTrace(0, true);
            string traceString = ExtractFormattedStackTrace(trace);

            WriteLine(Letra.FormatarLinha(Level));
            WriteLine(texto, args);
            WriteLine(traceString);
            WriteLine(Letra.FormatarLinha());
        }

        public static void WriteLine(string mensagem)
        {
            try
            {
                using StreamWriter _filelog = File.AppendText(_filepath + _filename);

                StackTrace st = new StackTrace(0, true);
                StackFrame sf = st.GetFrame(1);
                // string traceString = ExtractFormattedStackTrace(st);

                // Volte uma casa
                if (sf.GetMethod().Name == "WriteLine")
                {
                    sf = st.GetFrame(2);
                }

                _filelog.Write("\r\n[{0} {1}] ", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());
                // _filelog.Write(traceString);

                _filelog.Write("[{0}.{1}:{2}] {3}", sf.GetMethod().DeclaringType.Namespace, sf.GetMethod().DeclaringType.Name, sf.GetMethod().Name, mensagem);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void WriteLine(string mensagem, params object?[] args)
        {
            WriteLine(string.Format(mensagem, args));
        }

        public static void WriteLine(object? mensagem)
        {
            WriteLine((string)mensagem);
        }

        public static void Debug(string texto, params object[] args)
        {
            Log("[DEBUG] " + texto, "Debug", args);
        }

        public static void Aviso(string texto, params object[] args)
        {
            Log("[AVISO] " + texto, "Aviso", args);
        }

        public static void Erro(string texto, params object[] args)
        {
            Log("[ERROR] " + texto, "Erro", args);
        }
    }
}