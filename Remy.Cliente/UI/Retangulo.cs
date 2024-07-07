using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using Remy.Cliente.Graficos.OpenGL;
using Remy.Cliente.Graficos.Render;

namespace Remy.Cliente.UI
{
    public class Retangulo
    {
        private VertexBufferObject<float> VertexBufferObject;
        private VertexArrayObject VertexArrayObject;
        private ElementBufferObject<uint> ElementBufferObject;
        private Shader shader;

        uint[] indices =
        {  // note that we start from 0!
            0, 1, 3,   // first triangleks
            1, 2, 3    // second triangle
        };

        float[] vertices =
        {
        //   X      Y     Z
            0.5f,  0.5f, 0.0f,  // top right
            0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left
        };

        public Retangulo(float largura, float altura, float x, float y)
        {
            float xpos = x / largura * 1.0f;
            float ypos = y / altura * 1.0f;

            vertices = new float[]
            {
            //   X     Y     Z
                xpos, -ypos, 0.0f, // top right
                -xpos, -ypos, 0.0f, // bottom right
                -xpos, ypos, 0.0f, // bottom left
                xpos, ypos, 0.0f, // top left
            };

            VertexBufferObject = new(vertices.Length, BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);
            VertexBufferObject.SetData(vertices, 0, vertices.Length);

            VertexArrayObject = new VertexArrayObject(3 * sizeof(float));
            VertexArrayObject.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0);

            ElementBufferObject = new ElementBufferObject<uint>(indices.Length, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw);
            ElementBufferObject.SetData(indices, 0, indices.Length);

            shader = new Shader("Recursos/Shaders/TRIANGLE_VERTEX.vert", "Recursos/Shaders/TRIANGLE_COLOR.frag");
            shader.Use();
        }

        public void desenhar()
        {
            shader.Use();

            VertexArrayObject.Bind();

            // GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}