using OpenTK.Graphics.OpenGL;

namespace Remy.Engine.Graficos.OpenGL
{
    public class GLUtils
    {
        public static void GetGLError(string mensagem)
        {
            ErrorCode errorCode = GL.GetError();
            if (errorCode != ErrorCode.NoError)
            {
                throw new GLException(mensagem);
            }
        }
    }

    public class GLException : Exception
    {
        public GLException() { }
        public GLException(string mensagem) : base(mensagem) { }
    }
}