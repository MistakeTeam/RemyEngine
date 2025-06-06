using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Remy.Engine.Graficos.OpenGL.Buffers
{
    public class BufferObject<T> : IDisposable where T : unmanaged
    {
        public readonly int Handle;
        public readonly int Size;
        private readonly BufferTarget _bufferType;

        // [Vertex Buffer Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Buffer_Object)
        public BufferObject(int size, BufferTarget bufferType, BufferUsageHint Dynamic)
        {
            _bufferType = bufferType;
            Handle = GL.GenBuffer();
            Size = size;

            Bind();

            var elementSizeInBytes = Marshal.SizeOf<T>();
            GL.BufferData(bufferType, Size * elementSizeInBytes, IntPtr.Zero, Dynamic);
            GLUtils.GetGLError($"Erro ao tentar compilar");
        }

        public void Bind()
        {
            GL.BindBuffer(_bufferType, Handle);
            GLUtils.GetGLError($"Erro ao tentar compilar");
        }

        public void Dispose()
        {
            GL.DeleteBuffer(Handle);
        }

        public unsafe void SetData(T[] data, int startIndex = 0)
        {
            GL.BufferSubData(_bufferType, startIndex, Size * sizeof(T), data);

            Bind();
        }

        public unsafe void SetData(T[,] data, int startIndex = 0)
        {
            GL.BufferSubData(_bufferType, startIndex, Size * sizeof(T), data);

            Bind();
        }
    }
}