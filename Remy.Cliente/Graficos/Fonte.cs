using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using SharpFont;

using Remy.Cliente.Graficos.OpenGL;
using Remy.Cliente.Graficos.Render;
using Remy.Cliente.Graficos.Texto;

namespace Remy.Cliente.Graficos
{
    internal class Fonte
    {
        private readonly VertexBufferObject<float> _vbo;
        private readonly VertexArrayObject _vao;

        private Dictionary<uint, Character> _characters = [];

        private Shader _shader;

        public unsafe Fonte()
        {
            // iniciando a biblioteca
            Library lib = new Library();
            Face face = new Face(lib, "Recursos/Fonts/DroidSans.ttf");
            face.SetPixelSizes(0, 48); // Muda o tamanho da fonte

            // definir alinhamento de pixel em 1 byte 
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            // Carregue os primeiros 128 caracteres do conjunto ASCII
            for (uint c = 0; c < 128; c++)
            {
                try
                {
                    // Carregar o glyph
                    face.LoadChar(c, LoadFlags.Render, LoadTarget.Normal);
                    GlyphSlot glyph = face.Glyph;
                    FTBitmap bitmap = glyph.Bitmap;

                    Texture tre = new Texture(bitmap.Width, bitmap.Rows, PixelInternalFormat.R8, PixelFormat.Red, PixelType.UnsignedByte, bitmap.Buffer);

                    // Adicione a letra
                    Character ch = new Character
                    (
                        tre._handle,
                        new Vector2(bitmap.Width, bitmap.Rows),
                        new Vector2(glyph.BitmapLeft, glyph.BitmapTop),
                        glyph.Advance.X.Value
                    );

                    _characters.Add(c, ch);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            GL.BindTexture(TextureTarget.Texture2D, 0);    // vincular textura padrão
            // GL.PixelStore(PixelStoreParameter.UnpackAlignment, 4);  // definir o alinhamento de pixels padrão (4 bytes) 

            // Criar [Vertex Buffer Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Buffer_Object)
            _vbo = new VertexBufferObject<float>(4 * 6, BufferTarget.ArrayBuffer, BufferUsageHint.DynamicDraw);

            // [Vertex Array Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Array_Object)
            _vao = new VertexArrayObject(4 * 4);
            _vao.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0);

            _shader = new Shader("Recursos/Shaders/texto.vert", "Recursos/Shaders/texto.frag");
            _shader.Use();
        }


        public void RenderText(string text, float x, float y, float scale, Color4 cor)
        {
            Matrix4 projectionM = Matrix4.CreateScale(new Vector3(1f / Game.Janela.X, 1f / Game.Janela.Y, 1.0f));
            projectionM = Matrix4.CreateOrthographicOffCenter(0.0f, Game.Janela.X, Game.Janela.Y, 0.0f, -1.0f, 1.0f);

            _shader.Use();

            GL.UniformMatrix4(1, false, ref projectionM);
            GL.Uniform3(2, cor.R, cor.G, cor.B);
            GL.ActiveTexture(TextureUnit.Texture0);

            _vao.Bind();

            if (y < _characters['H'].Size.Y)
            {
                y -= y - _characters['H'].Size.Y;
            }

            if ((y + _characters['H'].Size.Y) > Game.Janela.Y)
            {
                y -= y + _characters['H'].Size.Y - Game.Janela.Y;
            }

            // Iterar por todos as letras
            foreach (char c in text)
            {
                if (_characters.ContainsKey(c) == false)
                    continue;

                Character ch = _characters[c];

                float xrel = x + ch.Bearing.X * scale;
                float yrel = y + (_characters['H'].Size.Y - ch.Bearing.Y) * scale;

                float w = ch.Size.X * scale;
                float h = ch.Size.Y * scale;

                float[,] vertices = new float[6, 4] {
                    { xrel,         yrel + h,     0.0f, 1.0f },
                    { xrel + w,     yrel,         1.0f, 0.0f },
                    { xrel,         yrel,         0.0f, 0.0f },

                    { xrel,         yrel + h,     0.0f, 1.0f },
                    { xrel + w,     yrel + h,     1.0f, 1.0f },
                    { xrel + w,     yrel,         1.0f, 0.0f }
                };

                GL.BindTexture(TextureTarget.Texture2D, ch.TextureID); // Renderizar textura de glifo sobre quad
                _vbo.SetData(vertices, 0, vertices.Length); // Atualizar o conteúdo da memória VBO
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6); // Renderizar quad

                // Agora avance os cursores para o próximo glifo (observe que o avanço é um número de 1/64 pixels)
                x += (ch.Advance >> 6) * scale; // Bitshift por 6 para obter o valor em pixels (2 ^ 6 = 64 (divida a quantidade de 1/64 pixels por 64 para obter a quantidade de pixels))
            }

            _vbo.Bind();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}