using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Remy.Engine.Graficos.OpenGL;
using Remy.Engine.IO;

namespace Remy.Engine.Graficos.Texto
{
    public class TextoRender : IDisposable
    {
        private readonly BufferObject<float> VBO;
        private readonly ArrayObject VAO;
        private Fonte FonteEmUso;
        private Shader _shader;

        public TextoRender(Dictionary<int, (char, float[])> ListaVertices)
        {
            Color4 Cor = Color4.Crimson;
            FonteEmUso = GerenciarFontes.GetFonte();

            // [Vertex Buffer Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Buffer_Object)
            VBO = new BufferObject<float>(4 * 6, BufferTarget.ArrayBuffer, BufferUsageHint.DynamicDraw);

            // [Vertex Array Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Array_Object)
            VAO = new ArrayObject(4 * 4);
            VAO.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0);

            _shader = new Shader("Recursos/Shaders/texto.vert", "Recursos/Shaders/texto.frag");
            _shader.Use();

            Matrix4 projectionM = Matrix4.CreateScale(new Vector3(1f / Game.Janela.X, 1f / Game.Janela.Y, 1.0f));
            projectionM = Matrix4.CreateOrthographicOffCenter(0.0f, Game.Janela.X, Game.Janela.Y, 0.0f, -1.0f, 1.0f);

            // _shader.Use();

            VAO.SetUniformMatrix4(1, projectionM);
            VAO.SetUniform3(2, Cor.R, Cor.G, Cor.B);
            GL.ActiveTexture(TextureUnit.Texture0);

            VAO.Bind();

            foreach (var cache in ListaVertices)
            {
                CaractereGlyph ch = FonteEmUso.Caracteres[cache.Value.Item1];

                GL.BindTexture(TextureTarget.Texture2D, ch.Textura.Handle); // Renderizar textura de glifo sobre quad
                VBO.SetData(cache.Value.Item2); // Atualizar o conteúdo da memória VBO
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6); // Renderizar quad
            }

            VBO.Bind();
            GL.BindTexture(TextureTarget.Texture2D, 0);

            Dispose();
        }

        /// DISPOSED

        public void Dispose()
        {
            VAO.Dispose();
            VBO.Dispose();
            _shader.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}