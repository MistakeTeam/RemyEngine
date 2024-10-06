using OpenTK.Graphics.OpenGL4;

namespace Remy.Engine.Graficos.OpenGL
{
    public class Textura2D : Textura
    {
        public override TextureTarget TextureTarget { get { return TextureTarget.Texture2D; } }
        public int GetByteSize() => Width * Height * 4;

        public Textura2D(int width, int height, nint pixels) : base(PixelInternalFormat.R8, PixelFormat.Red, PixelType.UnsignedByte)
        {
            Width = width;
            Height = height;

            Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalformat, Width, Height, 0, format, type, pixels);
            SetWrapMode(TextureWrapMode.Repeat);
            SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
        }
    }
}