using OpenTK.Graphics.OpenGL4;
using Remy.Engine.Utility;

namespace Remy.Engine.Graficos.OpenGL
{
    public class VertexArrayObject : IDisposable
    {
        private readonly int _handle;
        private readonly int _stride;

        public VertexArrayObject(int stride)
        {
            if (stride <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(stride));
            }

            _stride = stride;

            _handle = GL.GenVertexArray();

            Bind();
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(_handle);
        }

        public void Bind()
        {
            GL.BindVertexArray(_handle);
        }

        public void VertexAttribPointer(int location, int size, VertexAttribPointerType type, bool normalized, int offset)
        {
            GL.EnableVertexAttribArray(location);
            GL.VertexAttribPointer(location, size, type, normalized, _stride, new IntPtr(offset));

            Bind();
        }
    }
}