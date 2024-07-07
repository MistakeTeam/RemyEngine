using Remy.Logs;

namespace Remy.ScriptParse
{
    /// <summary>
    /// Exemplo de sintaxe no arquivo de texto: ./Recursos/Script/Text.TEXTO_BRUTO
    /// </summary>
    public class ScriptsParse
    {
        private int CHAVE_INDEX;
        private string[] TEXTO_BRUTO;
        private string[] LISTA_CHAVE;
        private int LINHA_INDEX;
        private object RESULTADO_RETORNO = "";
        private bool _start = false;

        public ScriptsParse(string path)
        {
            TEXTO_BRUTO = File.ReadAllLines(path);
        }

        public object GetChave(string CHAVE_BRUTA = "")
        {
            // Esse pedaço só pode ser executado uma vez no inicio da busca
            if (!_start)
            {
                LogFile.WriteLine("============================");
                LISTA_CHAVE = CHAVE_BRUTA.Split(".");
                CHAVE_INDEX = 0;
                LINHA_INDEX = Extra.Array.FindIndex(TEXTO_BRUTO, 0, x => x.Contains(LISTA_CHAVE[CHAVE_INDEX]));
                _start = true;
            }

            // LogFile.WriteLine("Plataforma: {0:G}", Environment.OSVersion.VersionString);
            // Mensagem.Enviar($">>>> {FindIndex(TEXTO_BRUTO, 0, x => x.Contains(LISTA_CHAVE[CHAVE_INDEX]))}");

            int ULTIMO_COLCHETE = ProcurarColchetes(LINHA_INDEX);
            string LINHA_TEXTO = TEXTO_BRUTO[LINHA_INDEX];

            if (LINHA_TEXTO.Contains(LISTA_CHAVE[CHAVE_INDEX]))
            {
                string CHAVE_LINHA = LINHA_TEXTO.Split("=")[0].Trim(), CHAVE_VALOR = LINHA_TEXTO.Split("=")[1].Trim();
                (string CHAVE_VALOR_TIPO, object CHAVE_VALOR_RES) = GetT(CHAVE_VALOR);

                LogFile.WriteLine($"Etapa {CHAVE_INDEX + 1}");
                LogFile.WriteLine($"Chave: {CHAVE_LINHA} ({CHAVE_INDEX})");
                LogFile.WriteLine($"Type: {CHAVE_VALOR_TIPO}");
                LogFile.WriteLine($"Valor: {CHAVE_VALOR_RES}");
                LogFile.WriteLine($"Range: {LINHA_INDEX}:{ULTIMO_COLCHETE}");
                if (LISTA_CHAVE[CHAVE_INDEX] == LISTA_CHAVE[^1]) LogFile.WriteLine("============================");

                switch (CHAVE_VALOR_TIPO)
                {
                    case "object":
                        if (LINHA_TEXTO.Contains(LISTA_CHAVE[CHAVE_INDEX])) ProximoIndex();
                        LogFile.WriteLine($"Proxima CHAVE_BRUTA: {LISTA_CHAVE[CHAVE_INDEX]}");

                        LINHA_INDEX++;
                        GetChave();
                        break;
                    default:
                        RESULTADO_RETORNO = CHAVE_VALOR_RES;
                        break;
                }
            }
            else
            {
                LINHA_INDEX++;
                GetChave();
            }

            return RESULTADO_RETORNO;
        }

        private void ProximoIndex()
        {
            int MaxIndex = LISTA_CHAVE.Length - 1;
            CHAVE_INDEX = CHAVE_INDEX >= MaxIndex ? MaxIndex : CHAVE_INDEX + 1;
        }

        private int ProcurarColchetes(int Start)
        {
            int leng = 0;
            int index = 0;
            for (int i = Start; i < TEXTO_BRUTO.Length; i++)
            {
                if (TEXTO_BRUTO[i].Contains("{{"))
                    leng++;
                if (TEXTO_BRUTO[i].Contains("}}"))
                    leng--;

                if (leng == 0)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        private (string type, object value) GetT(string str)
        {
            object VALOR = "";

            if (str.Contains("{{"))
            {
                return (type: "object", value: VALOR);
            }
            else
            {
                _start = false;
                LogFile.WriteLine("Analisando o resultado");
                if (int.TryParse(str, out int n))
                {
                    VALOR = n;
                    return (type: "number", value: VALOR);
                }
                else if (float.TryParse(str, out float f))
                {
                    VALOR = f;
                    return (type: "float", value: VALOR);
                }
                else if (bool.TryParse(str, out bool b))
                {
                    VALOR = b;
                    return (type: "boolean", value: VALOR);
                }
                else
                {
                    VALOR = str;
                    return (type: "string", value: VALOR);
                }
            }
        }
    }
}