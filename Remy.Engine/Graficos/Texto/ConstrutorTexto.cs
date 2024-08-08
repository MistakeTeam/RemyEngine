using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Remy.Engine.Graficos.OpenGL;
using Remy.Engine.Graficos.Texto;
using Remy.Engine.IO;

namespace Remy.Engine.Graficos.Texto
{
    internal class ConstrutorTexto
    {
        private readonly BufferObject<float> VBO;
        private readonly ArrayObject VAO;
        private Fonte FonteEmUso;
        private Shader _shader;

        private string Texto;
        private Vector2 Posição;
        private float Escala;
        private Color4 Cor;

        public ConstrutorTexto(string text, float x, float y, float escala, Color4 cor)
        {
            Texto = text;
            Posição = new Vector2(x, y);
            Escala = escala;
            Cor = cor;

            FonteEmUso = GerenciarFontes.GetFonte();

            // Criar [Vertex Buffer Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Buffer_Object)
            VBO = new BufferObject<float>(4 * 6, BufferTarget.ArrayBuffer, BufferUsageHint.DynamicDraw);

            // [Vertex Array Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Array_Object)
            VAO = new ArrayObject(4 * 4);
            VAO.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0);

            _shader = new Shader("Recursos/Shaders/texto.vert", "Recursos/Shaders/texto.frag");
            _shader.Use();
        }

        public void Desenhar()
        {
            float X = Posição.X;
            float Y = Posição.Y;

            Matrix4 projectionM = Matrix4.CreateScale(new Vector3(1f / Game.Janela.X, 1f / Game.Janela.Y, 1.0f));
            projectionM = Matrix4.CreateOrthographicOffCenter(0.0f, Game.Janela.X, Game.Janela.Y, 0.0f, -1.0f, 1.0f);

            _shader.Use();

            VAO.SetUniformMatrix4(1, projectionM);
            VAO.SetUniform3(2, Cor.R, Cor.G, Cor.B);
            GL.ActiveTexture(TextureUnit.Texture0);

            VAO.Bind();

            if (Y < FonteEmUso.Characters['H'].Size.Y)
            {
                Y -= Y - FonteEmUso.Characters['H'].Size.Y;
            }

            if ((Y + FonteEmUso.Characters['H'].Size.Y) > Game.Janela.Y)
            {
                Y -= Y + FonteEmUso.Characters['H'].Size.Y - Game.Janela.Y;
            }

            // Iterar por todos as letras
            foreach (char c in Texto)
            {
                if (FonteEmUso.Characters.ContainsKey(c) == false)
                    continue;

                Character ch = FonteEmUso.Characters[c];

                float xrel = X + ch.Bearing.X * Escala;
                float yrel = Y + (FonteEmUso.Characters['H'].Size.Y - ch.Bearing.Y) * Escala;

                float w = ch.Size.X * Escala;
                float h = ch.Size.Y * Escala;

                float[,] vertices = new float[6, 4] {
                    { xrel,         yrel + h,     0.0f, 1.0f },
                    { xrel + w,     yrel,         1.0f, 0.0f },
                    { xrel,         yrel,         0.0f, 0.0f },

                    { xrel,         yrel + h,     0.0f, 1.0f },
                    { xrel + w,     yrel + h,     1.0f, 1.0f },
                    { xrel + w,     yrel,         1.0f, 0.0f }
                };

                GL.BindTexture(TextureTarget.Texture2D, ch.TextureID); // Renderizar textura de glifo sobre quad
                VBO.SetData(vertices); // Atualizar o conteúdo da memória VBO
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6); // Renderizar quad

                // Agora avance os cursores para o próximo glifo (observe que o avanço é um número de 1/64 pixels)
                X += (ch.Advance >> 6) * Escala; // Bitshift por 6 para obter o valor em pixels (2 ^ 6 = 64 (divida a quantidade de 1/64 pixels por 64 para obter a quantidade de pixels))
            }

            VBO.Bind();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}