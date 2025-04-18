using System.Text;
using OpenTK.Graphics.OpenGL;
using Remy.Engine.Logs;

namespace Remy.Engine.Graficos.OpenGL
{
    public class GLRenderer : Renderer
    {
        protected override void Initialise(string graphicsSurface)
        {
            GL.ClearColor(0.3f, 0.4f, 0.5f, 1.0f);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            string extensions = GetExtensions();

            Logger.WriteLine($@"GL Initialized
                                GL Version:                 {GL.GetString(StringName.Version)}
                                GL Renderer:                {GL.GetString(StringName.Renderer)}
                                GL Shader Language version: {GL.GetString(StringName.ShadingLanguageVersion)}
                                GL Vendor:                  {GL.GetString(StringName.Vendor)}
                                GL Extensions:              {extensions}");
        }

        public static string GetExtensions()
        {
            GL.GetInteger((GetPName)All.NumExtensions, out int numExtensions);

            var extensionsBuilder = new StringBuilder();

            for (int i = 0; i < numExtensions; i++)
                extensionsBuilder.Append($"{GL.GetString(StringNameIndexed.Extensions, i)} ");

            return extensionsBuilder.ToString().TrimEnd();
        }

        protected internal void NovoQuado()
        {

        }
    }
}