using System.Numerics;

using OpenTK.Graphics.OpenGL4;
using Remy.Engine.Logs;

namespace Remy.Engine.Graficos.OpenGL
{
    public class Shader : IDisposable
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
                throw new Exception($"Erro ao tentar compilar shader do tipo: {type}, falhou com o erro: {infoLog}");
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
            return GL.GetAttribLocation(Handle, attribName);
        }

        public void SetUniform(string name, int value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform não foi encontrado no shader.");
            }
            GL.Uniform1(location, value);
        }

        public void SetUniform(string name, float value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform não foi encontrado no shader.");
            }
            GL.Uniform1(location, value);
        }

        public unsafe void SetUniform(string name, Matrix4x4 value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform não foi encontrado no shader.");
            }

            GL.UniformMatrix4(location, 1, false, (float*)&value);
        }

        /// DISPOSED

        public void Dispose()
        {
            GL.DeleteProgram(Handle);
            GC.SuppressFinalize(this);
        }
    }
}