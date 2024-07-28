using OpenTK.Graphics.OpenGL4;
using System;

namespace Remy.Engine.Utility
{
    internal static class GLUtility
    {
        public static void CheckError()
        {
            var error = GL.GetError();
            if (error != ErrorCode.NoError)
                throw new Exception("[Remy] GL.GetError() returned " + error.ToString());
        }
    }
}