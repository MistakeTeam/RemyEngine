namespace Remy.Texto
{
    ///<Summary>
    /// Localizar
    ///</Summary>
    public class Localizar
    {
        ///<Summary>
        /// Entra no Bloco e retorna uma string com o valor da chave.
        ///</Summary>
        public static string PegarTexto(string bloco, string chave)
        {
            string[] txt = File.ReadAllLines(bloco);
            string linha = txt.FirstOrDefault(x => x.Contains(chave)) ?? "";
            string[] tt = linha.Split("=");

            if (linha.StartsWith('#') || tt.Length <= 1) return "LINHA_NAO_ENCONTRADA";
            if (linha.Contains(' ')) return "CHAVE_NAO_PODE_TER_ESPACO";

            string c = tt[0], v = tt[1].Replace("\"", "");

            if (!c.Any(char.IsUpper)) return "CHAVE_TEM_QUE_ESTAR_EM_MAIUSCULO";

            return v;
        }

        public static string TextoAleatorio(string bloco)
        {
            string[] txt = File.ReadAllLines(bloco);
            string linha = txt[new Random().Next(txt.Length)];

            return linha.Replace("\"", "");
        }
    }
}
