using Remy.Engine.Cache;
using Remy.Engine.Enum;
using Remy.Engine.Graficos.Texto;

namespace Remy.Engine.Graficos
{
    public class Render
    {
        private static List<ObjetoCache> RenderCache = [];
        public static int ContadorQuadros = 0;

        public Render()
        {

        }

        public static void AddObjeto(ObjetoCache obj)
        {
            // if (RenderCache.Find(r => r.Vertices == obj.Vertices) != null)
            //     return;

            obj.SetFrame(ContadorQuadros);
            RenderCache.Add(obj);
        }

        public void Clear()
        {
            RenderCache.Clear();
        }

        public void NovoQuadro()
        {
            if (ContadorQuadros == 60)
            {
                ContadorQuadros = 1;
            }
            else
            {
                ContadorQuadros++;
            }
        }

        public void Update()
        {
            if (RenderCache.Count > 0)
            {
                foreach (var cache in RenderCache)
                {
                    if (cache.FameIndex != ContadorQuadros)
                        return;

                    switch (cache.Tipo)
                    {
                        case TipoCache.Texto:
                            new TextoRender(((TextoCache)cache).Vertices);
                            break;
                        case TipoCache.Textura:
                        case TipoCache.Quadrado:
                        default:
                            break;
                    }
                }
            }
        }
    }
}