using System.Reflection;

namespace Remy.Engine.Extensions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BackgroundLoaderAttribute : Attribute
    {
        private const BindingFlags activator_flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        public BackgroundLoaderAttribute() { }

        internal static void Invoke()
        {
            var methods = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => x.IsClass)
                    .SelectMany(x => x.GetMethods(activator_flags))
                    .Where(x => x.GetCustomAttributes(typeof(BackgroundLoaderAttribute), false).FirstOrDefault() != null);

            foreach (var method in methods)
            {
                var obj = Activator.CreateInstance(method.DeclaringType!);

                if (!method.IsPrivate)
                    throw new CustomAttributeFormatException($"Um metodo com o atributo {nameof(BackgroundLoaderAttribute)} não pode ter um modificador de acesso diferente de privado");

                switch (method.GetParameters().Length)
                {
                    case 0:
                        method.Invoke(obj, null);
                        break;
                    default:
                        throw new CustomAttributeFormatException($"Um metodo com o atributo {nameof(BackgroundLoaderAttribute)} não pode ter parametros ou qualquer tipo de argumento");
                }
            }
        }
    }
}