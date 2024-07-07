using System.Reflection;
using Remy.Logs;

namespace Remy.Comando
{
    public class Comandos
    {
        // Dicionarios para listar os comandos:
        private static readonly Dictionary<List<string>, ComandoAbstrado> CClasses = new(); // Lista a classe de cada comando
        private static readonly List<string> CLista = new(); // Lista o nome de cada comando

        public static void Iniciar()
        {
            LogFile.WriteLine("Iniciando comandos...");

            // É feita uma busca no assembly principal pelo namespace: "MistakeTeam.Azana.Comandos"
            foreach (
                Type comando in Assembly
                    .GetEntryAssembly()
                    .GetTypes()
                    .Where(
                        mytype => mytype.IsClass && !mytype.IsAbstract && mytype.IsSubclassOf(typeof(ComandoAbstrado))
                    )
                )
            {
                // Se existir alguma classe no namespace, cada uma delas será listado aqui
                if (comando != null)
                {
                    ComandoAbstrado cc = (ComandoAbstrado)Activator.CreateInstance(comando);

                    if (cc.IsSubComando()) continue;

                    List<string> _cls = new()
                    {
                        cc.Nome
                    };

                    CLista.Add(cc.Nome);
                    if (cc.Aliase != "")
                    {
                        _cls.Add(cc.Aliase);
                        CLista.Add(cc.Aliase);
                    }

                    CClasses.Add(_cls, cc);
                }
            }

            LogFile.WriteLine("{0} comandos foram registrados", CClasses.Count);
        }

        public static ComandoAbstrado? Procurar(string Nome)
        {
            return CClasses.FirstOrDefault(n => n.Key.Contains(Nome)).Value ?? null;
        }

        // Vamos executar o comando?
        public static void Executar(string[] args)
        {
            ComandoAbstrado t = Procurar(args[0]);

            // Existe?
            if (t == null)
            {
                LogFile.WriteLine("Comando não existe.");
                return;
            }

            t.Run(args.Where(x => x != args[0]).ToArray()); // A magia acontece aqui
        }
    }
}
