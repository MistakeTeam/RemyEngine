namespace Remy.Engine.Graficos
{
    public abstract class Renderer : IRenderer
    {
        protected abstract void Initialise(string graphicsSurface);

        void IRenderer.Initialise(string graphicsSurface)
        {
            Initialise(graphicsSurface);
        }
    }
}