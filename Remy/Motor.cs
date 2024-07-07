using Remy.Comando;
using Remy.Logs;
using Remy.Render;
using Remy.ScriptParse;
using Remy.Tempo;

namespace Remy
{
    public class Motor
    {
        private string _nome;

        public GameLoop _gameloop = default!;

        public void Ligar(RemyOptions op)
        {
            _nome = op.Nome;

            Mensagem.BemVindo(op.BemVindo);

            // Iniciando modulos
            LogFile.Iniciar(op.Nome);
            Eventos.Iniciar();
            Comandos.Iniciar();

            if (_gameloop == null)
            {
                _gameloop = new GameLoop();

                _gameloop.Iniciar();
            }

            // ScriptsParse SP = new ScriptsParse($"{Directory.GetCurrentDirectory()}/Recursos/Script/Text.txt");
            // Mensagem.Enviar(SP.GetChave("tommy.id"));
            // Mensagem.Enviar(SP.GetChave("isso_pode.id"));
            // Mensagem.Enviar(SP.GetChave("isso_pode.comer"));
            // Mensagem.Enviar(SP.GetChave("isso_pode.correr"));
            // Mensagem.Enviar(SP.GetChave("cade.comida"));
            // Mensagem.Enviar(SP.GetChave("cade.mercado.arroz"));
            // Mensagem.Enviar(SP.GetChave("cade.shopping.americanas.kitkat"));
            // Mensagem.Enviar(SP.GetChave("cade.shopping.americanas.livros.fantasia.mistborn"));

            Eventos.OnProcessExit((a, b) =>
            {
                LogFile.WriteLine("[Remy] Desligando motor");
            });
        }
    }

    public class RemyOptions
    {
        public RemyOptions()
        {
            BemVindo = "Obrigrado pro jogar :)";
        }

        public string Nome { get; init; }
        public string BemVindo { get; init; }
    }
}