using System.Drawing;

using OpenTK.Graphics.OpenGL4;

namespace Remy.Engine.Graficos.Render
{
    public unsafe class Texture : IDisposable
    {
        public readonly int _handle;

        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public virtual int GetByteSize() => Width * Height * 4;

        public Texture(int width, int height) : this(width, height, PixelInternalFormat.Rgba8, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero)
        {

        }

        public Texture(int width, int height, PixelInternalFormat internalformat, PixelFormat format, PixelType type, nint pixels)
        {
            Width = width;
            Height = height;

            _handle = GL.GenTexture();

            Bind();

            //Reserve enough memory from the gpu for the whole image
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalformat, width, height, 0, format, type, pixels);

            SetParameters();
        }

        private void SetParameters()
        {
            //Setting some texture perameters so the texture behaves as expected.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

            //Generating mipmaps.
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
        {
            //When we bind a texture we can choose which textureslot we can bind it to.
            GL.ActiveTexture(textureSlot);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }

        private bool isDisposed;

        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Texture()
        {
            Dispose(false);
        }

        public void Dispose(bool isDisposing)
        {
            //In order to dispose we need to delete the opengl handle for the texure.
            GL.DeleteTexture(_handle);
        }

        public void SetData(Rectangle bounds, byte[] data)
        {
            Bind();
            fixed (byte* ptr = data)
            {
                GL.TexSubImage2D(
                    target: TextureTarget.Texture2D,
                    level: 0,
                    xoffset: bounds.Left,
                    yoffset: bounds.Top,
                    width: bounds.Width,
                    height: bounds.Height,
                    format: PixelFormat.Rgba,
                    type: PixelType.UnsignedByte,
                    pixels: new IntPtr(ptr)
                );
            }
        }
    }
}