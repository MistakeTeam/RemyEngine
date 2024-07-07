using System.Numerics;
using OpenTK.Graphics.OpenGL;
using Remy.Cliente.Utility;

namespace Remy.Cliente.Graficos.Render
{
    public class Shader
    {
        public readonly int Handle;

        public int VertexShader { get; private set; }
        public int FragmentShader { get; private set; }

        public Shader(string vertexPath, string fragmentPath)
        {
            VertexShader = LoadShader(ShaderType.VertexShader, vertexPath);
            FragmentShader = LoadShader(ShaderType.FragmentShader, fragmentPath);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            LinkProgram(Handle);

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        private int LoadShader(ShaderType type, string path)
        {
            string src = File.ReadAllText(path);
            int shader = GL.CreateShader(type);

            GL.ShaderSource(shader, src);
            GL.CompileShader(shader);

            string infoLog = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
            }

            return shader;
        }

        private static void LinkProgram(int Handle)
        {
            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
            if (success != (int)All.True)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }
        }

        public static void GetShader(int _shader)
        {
            GL.GetShader(_shader, ShaderParameter.CompileStatus, out int success);
            if (success != (int)All.True)
            {
                string infoLog = GL.GetShaderInfoLog(_shader);
                Console.WriteLine(infoLog);
            }
        }

        public int GetAttribLocation(string attribName)
        {
            var result = GL.GetAttribLocation(Handle, attribName);
            return result;
        }

        public void SetUniform(string name, int value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            GL.Uniform1(location, value);
        }

        public void SetUniform(string name, float value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            GL.Uniform1(location, value);
        }

        public unsafe void SetUniform(string name, Matrix4x4 value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            GL.UniformMatrix4(location, 1, false, (float*)&value);
        }


        /// DISPOSED

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}