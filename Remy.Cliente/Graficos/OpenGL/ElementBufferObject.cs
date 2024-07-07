using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using Remy.Cliente.Utility;

namespace Remy.Cliente.Graficos.OpenGL
{
    public class ElementBufferObject<T> : IDisposable where T : unmanaged
    {
        private readonly int _handle;
        private readonly BufferTarget _bufferType;

        public ElementBufferObject(int size, BufferTarget bufferType, BufferUsageHint HintType)
        {
            _bufferType = bufferType;

            _handle = GL.GenBuffer();

            Bind();

            var elementSizeInBytes = Marshal.SizeOf<T>();
            GL.BufferData(bufferType, size * elementSizeInBytes, IntPtr.Zero, HintType);
        }

        public void Bind()
        {
            GL.BindBuffer(_bufferType, _handle);
        }

        public unsafe void SetData(T[] data, int startIndex, int elementCount)
        {
            Bind();

            fixed (T* dataPtr = &data[startIndex])
            {
                var elementSizeInBytes = sizeof(T);

                GL.BufferSubData(_bufferType, IntPtr.Zero, elementCount * elementSizeInBytes, new IntPtr(dataPtr));
            }
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_handle);
        }
    }
}