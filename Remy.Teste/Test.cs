using Remy.Engine.Core;
using Remy.Engine.Logs;

namespace Remy.Teste
{
    public class Test : Comportamento
    {
        public override void Start()
        {
            LogFile.WriteLine("Inicio");
        }

        public override void Update()
        {
            //LogFile.WriteLine("Atualizando");
        }
    }
}