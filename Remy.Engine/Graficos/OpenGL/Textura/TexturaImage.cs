using OpenTK.Graphics.OpenGL;
using StbImageSharp;

namespace Remy.Engine.Graficos.OpenGL.Texturas
{
    public class TexturaImage : Textura
    {
        public override TextureTarget TextureTarget { get { return TextureTarget.Texture2D; } }

        public TexturaImage(string path) : base(PixelInternalFormat.Rgba8, PixelFormat.Rgba, PixelType.UnsignedByte)
        {
            Bind();

            StbImage.stbi_set_flip_vertically_on_load(1);
            using (Stream stream = File.OpenRead(path))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

                Width = image.Width;
                Height = image.Height;

                Bind();
                GL.TexImage2D(TextureTarget.Texture2D, 0, internalformat, Width, Height, 0, format, type, image.Data);
                SetWrapMode(TextureWrapMode.Repeat);
                SetFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
            }
        }
    }
}