using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Remy.Engine.Graficos.OpenGL;
using Remy.Engine.Graficos.Texto;
using Remy.Engine.Logs;
using SharpFont;

namespace Remy.Engine.IO
{
    public class GerenciarFontes
    {
        private readonly Library _library;
        private static Dictionary<string, Fonte> Fontes = [];
        private static string FontePadrão;

        public GerenciarFontes()
        {
            _library = new Library();

            CarregarFonte("Recursos/Fonts/DroidSans.ttf");
            // CarregarFonte("Recursos/Fonts/DroidSansJapanese.ttf"); // COMO PODE ISSO TER 12585 GLYPHS????? *_*

            FontePadrão = "Droid Sans";
        }

        public void CarregarFonte(string path, uint tamanho = 20)
        {
            LogFile.WriteLine($"Carregando fonte: {path}");
            Dictionary<uint, CaractereGlyph> caracteres = [];
            Face face = new(_library, path);
            face.SetPixelSizes(0, tamanho); // Muda o tamanho da fonte
            Console.WriteLine(face.AvailableSizes);
            // definir alinhamento de pixel em 1 byte
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            LogFile.WriteLine($"Criando {face.GlyphCount} glyphs");
            for (uint c = 0; c < face.GlyphCount; c++)
            {
                try
                {
                    // Carregar o glyph
                    face.LoadChar(c, LoadFlags.Render, LoadTarget.Normal);
                    GlyphSlot glyph = face.Glyph;
                    FTBitmap bitmap = glyph.Bitmap;

                    Textura FontTexture = new Textura2D(bitmap.Width, bitmap.Rows, bitmap.Buffer);

                    // Adicione a letra
                    CaractereGlyph ch = new CaractereGlyph
                    (
                        FontTexture,
                        new Vector2(bitmap.Width, bitmap.Rows),
                        new Vector2(glyph.BitmapLeft, glyph.BitmapTop),
                        glyph.Advance.X.Value,
                        (char)c
                    );

                    caracteres.Add(c, ch);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            GL.BindTexture(TextureTarget.Texture2D, 0);    // vincular textura padrão
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 4);  // definir o alinhamento de pixels padrão (4 bytes) 

            LogFile.WriteLine($"Salvando {face.FamilyName}");
            Fontes.Add(face.FamilyName, new(face.FamilyName, caracteres));
        }

        public static Fonte GetFonte(string? nome = null)
        {
            nome ??= FontePadrão;
            return Fontes[nome];
        }
    }
}