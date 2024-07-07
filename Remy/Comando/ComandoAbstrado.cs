using System.Reflection;

namespace Remy.Comando
{
    public abstract class ComandoAbstrado
    {
        public abstract string Nome { get; }
        public abstract string Aliase { get; }
        public abstract string Descricao { get; }

        public abstract void Run(string[] args);

        public IEnumerable<Type> Filhos()
        {
            return Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(
                    y =>
                    {
                        Type t = y.BaseType;

                        return t != null && t.IsGenericType && !t.IsInterface && t.IsAbstract &&
                            t.GetGenericTypeDefinition() == typeof(ComandoAbstrado<>) &&
                            t.GetGenericArguments()[0] == GetType();
                    }
                )
                .ToArray();
        }

        public bool IsSubComando()
        {
            return GetType().GetProperty("Pai") != null;
        }
    }

    public abstract class ComandoAbstrado<T> : ComandoAbstrado where T : ComandoAbstrado, new()
    {
        public T Pai { get; }
    }
}
