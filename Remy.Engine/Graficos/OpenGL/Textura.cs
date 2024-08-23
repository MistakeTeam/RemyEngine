using OpenTK.Graphics.OpenGL4;

namespace Remy.Engine.Graficos.OpenGL
{
    public abstract unsafe class Textura : IDisposable
    {
        public readonly int Handle;
        public abstract TextureTarget TextureTarget { get; }
        public PixelInternalFormat internalformat;
        public PixelFormat format;
        public PixelType type;

        public Textura(PixelInternalFormat internalformat, PixelFormat format, PixelType type)
        {
            Handle = GL.GenTexture();

            this.internalformat = internalformat;
            this.format = format;
            this.type = type;
        }

        public void SetParameter(TextureParameterName parameterName, int value)
        {
            GL.TexParameter(TextureTarget, parameterName, value);
        }

        public void SetWrapMode(TextureWrapMode wrapMode)
        {
            var mode = (int)wrapMode;
            SetParameter(TextureParameterName.TextureWrapR, mode);
            SetParameter(TextureParameterName.TextureWrapS, mode);
            SetParameter(TextureParameterName.TextureWrapT, mode);
        }

        public void SetFilter(TextureMinFilter minFilter, TextureMagFilter magFilter)
        {
            SetParameter(TextureParameterName.TextureMinFilter, (int)minFilter);
            SetParameter(TextureParameterName.TextureMagFilter, (int)magFilter);
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget, Handle);
        }

        public void Bind(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            Bind();
        }

        /// DISPOSED

        public void Dispose()
        {
            GL.DeleteTexture(Handle);
            GC.SuppressFinalize(this);
        }
    }
}