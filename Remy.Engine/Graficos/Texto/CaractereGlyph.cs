using OpenTK.Mathematics;
using Remy.Engine.Graficos.OpenGL;

namespace Remy.Engine.Graficos.Texto
{
    public readonly struct CaractereGlyph(Textura textura, Vector2 Size, Vector2 Bearing, int Advance, char caractere)
    {
        public readonly char Caractere { get; } = caractere;
        public Textura Textura { get; } = textura;
        public Vector2 Size { get; } = Size;
        public Vector2 Bearing { get; } = Bearing;
        public int XAdvance { get; } = Advance;
    }
}