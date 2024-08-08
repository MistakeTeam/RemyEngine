using OpenTK.Graphics.OpenGL4;
using Remy.Engine.Logs;
using StbImageSharp;

namespace Remy.Engine.Graficos.OpenGL
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

        public Texture(int width, int height, byte[] pixels) : this(width, height, PixelInternalFormat.Rgba8, PixelFormat.Rgba, PixelType.UnsignedByte, pixels)
        {

        }

        public Texture(int width, int height, PixelInternalFormat internalformat, PixelFormat format, PixelType type, nint pixels)
        {
            Width = width;
            Height = height;

            _handle = GL.GenTexture();

            Bind();

            SetTexture(internalformat, format, type, pixels);
        }

        public Texture(int width, int height, PixelInternalFormat internalformat, PixelFormat format, PixelType type, byte[] pixels)
        {
            Width = width;
            Height = height;

            _handle = GL.GenTexture();

            Bind();

            SetTexture(internalformat, format, type, pixels);
        }

        public Texture(string path)
        {
            _handle = GL.GenTexture();

            Bind();

            StbImage.stbi_set_flip_vertically_on_load(1);
            using (Stream stream = File.OpenRead(path))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

                Width = image.Width;
                Height = image.Height;

                SetTexture(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
                SetParameters();
            }
        }

        public void SetTexture(PixelInternalFormat internalformat, PixelFormat format, PixelType type, byte[] pixels)
        {
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalformat, Width, Height, 0, format, type, pixels);
            // GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void SetTexture(PixelInternalFormat internalformat, PixelFormat format, PixelType type, nint pixels)
        {
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalformat, Width, Height, 0, format, type, pixels);

            SetParameters();
        }

        private void SetParameters()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        }

        public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
        {
            GL.ActiveTexture(textureSlot);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }

        /// DISPOSED

        private bool disposedValue = false;

        private void Dispose(bool isDisposing)
        {
            if (!disposedValue)
            {
                GL.DeleteTexture(_handle);
                disposedValue = true;
            }
        }

        ~Texture()
        {
            Dispose();

            if (disposedValue == false)
            {
                LogFile.WriteLine("Vazamento de recurso de GPU! Você esqueceu de chamar Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}