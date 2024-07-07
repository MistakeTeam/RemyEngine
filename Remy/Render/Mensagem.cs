using Remy.Extra;

namespace Remy.Render
{
    public class Mensagem
    {
        public static void Enviar(object value, params object?[]? args)
        {
            // Array values = Enum.GetValues(typeof(ConsoleColor));
            // Console.ForegroundColor = (ConsoleColor)values.GetValue(new Random().Next(values.Length));

            if (args == null)
                Console.WriteLine(string.Format(Convert.ToString(value)));
            else
                Console.WriteLine(string.Format(Convert.ToString(value), args));
        }

        internal static void BemVindo(string msg)
        {
            Enviar(Letra.FormatarLinha("Bem-vindo! ようこそ"));
            Enviar(msg);
            Enviar(Letra.FormatarLinha());
        }
    }
}
