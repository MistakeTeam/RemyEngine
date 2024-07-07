using OpenTK.Graphics.OpenGL4;
using MathTK = OpenTK.Mathematics;
using SharpFont;
using System.Numerics;
using Remy.Cliente.Graficos.OpenGL;
using Remy.Cliente.Graficos.Render;

namespace Remy.Cliente.Graficos
{
    public struct Character
    {
        public int TextureID { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Bearing { get; set; }
        public int Advance { get; set; }
    }

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
                    // Carreguar o glyph
                    face.LoadChar(c, LoadFlags.Render, LoadTarget.Normal);
                    GlyphSlot glyph = face.Glyph;
                    FTBitmap bitmap = glyph.Bitmap;

                    // Criar a textura do glyph
                    int texObj = GL.GenTexture();
                    GL.BindTexture(TextureTarget.Texture2D, texObj);
                    GL.TexImage2D(TextureTarget.Texture2D, 0,
                                  PixelInternalFormat.R8, bitmap.Width, bitmap.Rows, 0,
                                  PixelFormat.Red, PixelType.UnsignedByte, bitmap.Buffer);

                    // Define os parametros da textura
                    GL.TextureParameter(texObj, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TextureParameter(texObj, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    GL.TextureParameter(texObj, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                    GL.TextureParameter(texObj, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

                    // Adicione a letra
                    Character ch = new Character();
                    ch.TextureID = texObj;
                    ch.Size = new Vector2(bitmap.Width, bitmap.Rows);
                    ch.Bearing = new Vector2(glyph.BitmapLeft, glyph.BitmapTop);
                    ch.Advance = (int)glyph.Advance.X.Value;
                    _characters.Add(c, ch);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }


            // vincular textura padrão
            GL.BindTexture(TextureTarget.Texture2D, 0);

            // definir o alinhamento de pixels padrão (4 bytes) 
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 4);

            // float[] vquad =
            // {
            // // x      y      u     v    
            //     0.0f, -1.0f,   0.0f, 0.0f,
            //     0.0f,  0.0f,   0.0f, 1.0f,
            //     1.0f,  0.0f,   1.0f, 1.0f,
            //     0.0f, -1.0f,   0.0f, 0.0f,
            //     1.0f,  0.0f,   1.0f, 1.0f,
            //     1.0f, -1.0f,   1.0f, 0.0f
            // };

            // Criar [Vertex Buffer Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Buffer_Object)
            _vbo = new VertexBufferObject<float>(4 * 6, BufferTarget.ArrayBuffer, BufferUsageHint.DynamicDraw);
            // _vbo.SetData(vquad, 0, vquad.Length);

            // [Vertex Array Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Array_Object)
            _vao = new VertexArrayObject(4 * 4);
            _vao.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0);
            // _vao.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 2 * 4);

            _shader = new Shader("Recursos/Shaders/texto.vert", "Recursos/Shaders/texto.frag");
            _shader.Use();
        }


        public void RenderText(string text, float x, float y, float scale, MathTK.Color4 cor)
        {
            _shader.Use();
            GL.Uniform3(2, cor.R, cor.G, cor.B);
            GL.ActiveTexture(TextureUnit.Texture0);
            _vao.Bind();

            // Iterar por todos as letras
            foreach (var c in text)
            {
                if (_characters.ContainsKey(c) == false)
                    continue;

                Character ch = _characters[c];

                float xrel = x + ch.Bearing.X * scale;
                float yrel = y - (ch.Size.Y - ch.Bearing.Y) * scale;

                float w = ch.Size.X * scale;
                float h = ch.Size.Y * scale;

                float[,] vertices = new float[6, 4] {
                    { xrel,     yrel + h,   0.0f, 0.0f },
                    { xrel,     yrel,       0.0f, 1.0f },
                    { xrel + w, yrel,       1.0f, 1.0f },

                    { xrel,     yrel + h,   0.0f, 0.0f },
                    { xrel + w, yrel,       1.0f, 1.0f },
                    { xrel + w, yrel + h,   1.0f, 0.0f }
                };

                // Renderizar textura de glifo sobre quad
                GL.BindTexture(TextureTarget.Texture2D, ch.TextureID);

                // Atualizar o conteúdo da memória VBO
                // _vbo.Bind();
                _vbo.SetData(vertices, 0, vertices.Length);
                // GL.BufferSubData(BufferTarget.ArrayBuffer, 0, vertices.Length * sizeof(float), vertices);
                // _vbo.Bind();


                // Renderizar quad
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

                // Agora avance os cursores para o próximo glifo (observe que o avanço é um número de 1/64 pixels)
                x += (ch.Advance >> 6) * scale; // Bitshift por 6 para obter o valor em pixels (2 ^ 6 = 64 (divida a quantidade de 1/64 pixels por 64 para obter a quantidade de pixels))
            }

            _vbo.Bind();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}