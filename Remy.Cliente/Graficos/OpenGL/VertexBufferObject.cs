using OpenTK.Graphics.OpenGL4;
using Remy.Cliente.Utility;
using System.Runtime.InteropServices;

namespace Remy.Cliente.Graficos.OpenGL
{
    public class VertexBufferObject<T> : IDisposable where T : unmanaged
    {
        public readonly int _handle;
        private readonly BufferTarget _bufferType;

        public VertexBufferObject(int size, BufferTarget bufferType, BufferUsageHint Dynamic)
        {
            _bufferType = bufferType;
            _handle = GL.GenBuffer();

            Bind();

            var elementSizeInBytes = Marshal.SizeOf<T>();
            GL.BufferData(bufferType, size * elementSizeInBytes, IntPtr.Zero, Dynamic);

            Bind();
        }

        public void Bind()
        {
            GL.BindBuffer(_bufferType, _handle);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_handle);
        }

        public unsafe void SetData(T[] data, int startIndex, int elementCount)
        {
            Bind();

            GL.BufferSubData(_bufferType, 0, elementCount * sizeof(T), data);

            Bind();
        }

        public unsafe void SetData(T[,] data, int startIndex, int elementCount)
        {
            Bind();

            GL.BufferSubData(_bufferType, 0, elementCount * sizeof(T), data);

            Bind();
        }
    }
}