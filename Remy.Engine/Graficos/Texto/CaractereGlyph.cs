using OpenTK.Mathematics;
using Remy.Engine.Graficos.OpenGL.Texturas;

namespace Remy.Engine.Graficos.Texto
{
    public sealed class CaractereGlyph(Textura textura, Vector2 Size, Vector2 Bearing, int Advance, char caractere)
    {
        public char Caractere { get; } = caractere;
        public Textura Textura { get; } = textura;
        public Vector2 Size { get; } = Size;
        public Vector2 Bearing { get; } = Bearing;
        public int XAdvance { get; } = Advance;
    }
}