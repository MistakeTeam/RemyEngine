namespace Remy.Engine.Core
{
    public class Objeto
    {
        public List<Componente> Componentes = new();
        public readonly string Nome;

        public Objeto(string nome = "Novo Objeto")
        {
            Nome = nome;
        }

        public void AddComponente(Componente _Componenete)
        {
            Componentes.Add(_Componenete);
        }

        public T? GetComponent<T>() where T : Componente
        {
            foreach (var componente in Componentes)
            {
                if (typeof(T) == componente.GetType()) return (componente as T)!;
            }

            return null;
        }

        public Componente GetComponent(Type T)
        {
            foreach (var componente in Componentes)
            {
                if (T == componente.GetType()) return componente;
            }

            return null;
        }
    }
}