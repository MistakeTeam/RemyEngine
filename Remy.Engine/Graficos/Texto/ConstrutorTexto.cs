using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Remy.Engine.Graficos.OpenGL.Buffers;
using Remy.Engine.Graficos.OpenGL.Shaders;
using Remy.Engine.IO;
using Remy.Engine.Logs;
using Remy.Engine.Plataforma;

namespace Remy.Engine.Graficos.Texto
{
    public class ConstrutorTexto : Desenhavel
    {
        private Fonte FonteEmUso;
        private List<char> Caracteres = new();
        protected internal List<float[]> Vertices = new();

        public string Texto;
        public float Escala = 1;

        public ConstrutorTexto()
        {
            FonteEmUso = GerenciarFontes.GetFonte();
        }

        protected override void Update()
        {
            float X = Posição.X;
            float Y = Posição.Y;

            if ((Y + FonteEmUso.Caracteres['H'].Size.Y) > Window.Tamanho_Janela.Y)
            {
                Y -= Y + FonteEmUso.Caracteres['H'].Size.Y - Window.Tamanho_Janela.Y;
            }

            // Iterar por todos as letras
            foreach (char c in Texto)
            {
                if (FonteEmUso.Caracteres.ContainsKey(c) == false)
                    continue;

                Caracteres.Add(c);

                CaractereGlyph ch = FonteEmUso.Caracteres[c];

                float xrel = X + ch.Bearing.X * Escala;
                float yrel = Y + (FonteEmUso.Caracteres['H'].Size.Y - ch.Bearing.Y) * Escala;

                float w = ch.Size.X * Escala;
                float h = ch.Size.Y * Escala;

                Vertices.Add([
                     xrel,         yrel + h,     0.0f, 1.0f,
                     xrel + w,     yrel,         1.0f, 0.0f,
                     xrel,         yrel,         0.0f, 0.0f,

                     xrel,         yrel + h,     0.0f, 1.0f,
                     xrel + w,     yrel + h,     1.0f, 1.0f,
                     xrel + w,     yrel,         1.0f, 0.0f
                ]);

                // Agora avance os cursores para o próximo glifo (observe que o avanço é um número de 1/64 pixels)
                X += (ch.XAdvance >> 6) * Escala; // Bitshift por 6 para obter o valor em pixels (2 ^ 6 = 64 (divida a quantidade de 1/64 pixels por 64 para obter a quantidade de pixels))
            }

            if (Caracteres.Count != Vertices.Count)
                Logger.WriteLine("Numero de caracteres e Vertices são diferentes");

            // [Vertex Buffer Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Buffer_Object)
            VBO = new BufferObject<float>(4 * 6, BufferTarget.ArrayBuffer, BufferUsageHint.DynamicDraw);

            // [Vertex Array Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Array_Object)
            VAO = new ArrayObject(4 * 4);
            VAO.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0);

            shader = new Shader("Recursos/Shaders/texto.vert", "Recursos/Shaders/texto.frag");
            shader.Use();
        }

        protected override void UpdateDraw()
        {
            shader.Use();

            Matrix4 projectionM = new();
            projectionM = Matrix4.CreateScale(new Vector3(1f / Window.Tamanho_Janela.X, 1f / Window.Tamanho_Janela.Y, 1.0f));
            projectionM = Matrix4.CreateOrthographicOffCenter(0.0f, Window.Tamanho_Janela.X, Window.Tamanho_Janela.Y, 0.0f, -1.0f, 1.0f);

            shader.SetUniform("projection", projectionM);

            VAO.SetUniform3(2, Cor.R, Cor.G, Cor.B);
            GL.ActiveTexture(TextureUnit.Texture0);

            VAO.Bind();

            for (int i = 0; i < Caracteres.Count; i++)
            {
                CaractereGlyph ch = FonteEmUso.Caracteres[Caracteres[i]];

                GL.BindTexture(TextureTarget.Texture2D, ch.Textura.Handle); // Renderizar textura de glifo sobre quad
                VBO.SetData(Vertices[i]); // Atualizar o conteúdo da memória VBO
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6); // Renderizar quad
            }

            VBO.Bind();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        /// DISPOSED

        protected override void Dispose(bool isDisposing)
        {
            VBO.Dispose();
            VAO.Dispose();
            shader.Dispose();

            VBO = null;
            VAO = null;
            shader = null;
            Caracteres.Clear();
            Vertices.Clear();

            base.Dispose(isDisposing);
        }
    }
}