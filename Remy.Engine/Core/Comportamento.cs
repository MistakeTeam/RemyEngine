namespace Remy.Engine.Core
{
    public abstract class Comportamento : Componente
    {
        protected Comportamento()
        {
            BaseComportamento.Register(this);
        }

        public bool Habilitado = true;

        public abstract void Start();

        public abstract void Update();
    }

    internal class BaseComportamento : BaseComponente<Comportamento>
    {
        public static void Start()
        {
            foreach (var component in Components)
            {
                if (!component.Habilitado) continue;
                component.Start();
            }
        }

        public static void Update()
        {
            foreach (var component in Components)
            {
                if (!component.Habilitado) continue;
                component.Update();
            }
        }
    }
}