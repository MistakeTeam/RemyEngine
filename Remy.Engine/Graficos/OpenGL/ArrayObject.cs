using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Remy.Engine.Graficos.OpenGL
{
    public class ArrayObject : IDisposable
    {
        private readonly int _handle;
        private readonly int _stride;

        public ArrayObject(int stride)
        {
            if (stride <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(stride));
            }

            _stride = stride;

            _handle = GL.GenVertexArray();

            Bind();
        }

        public void Bind()
        {
            GL.BindVertexArray(_handle);
        }

        public void VertexAttribPointer(int location, int size, VertexAttribPointerType type, bool normalized, int offset)
        {
            GL.EnableVertexAttribArray(location);
            GL.VertexAttribPointer(location, size, type, normalized, _stride, offset);

            Bind();
        }

        public void SetUniformMatrix4(int location, Matrix4 projectionM)
        {
            GL.UniformMatrix4(location, false, ref projectionM);
        }

        public void SetUniform3(int location, float x, float y, float z) => SetUniform3(location, new Vector3(x, y, z));

        public void SetUniform3(int location, Vector3 vec)
        {
            GL.Uniform3(location, vec);
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(_handle);
        }
    }
}