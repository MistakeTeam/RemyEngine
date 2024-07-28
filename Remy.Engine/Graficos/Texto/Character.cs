using OpenTK.Mathematics;

namespace Remy.Engine.Graficos.Texto
{
    public sealed class Character
    {
        public int TextureID { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Bearing { get; set; }
        public int Advance { get; set; }

        public Character(int _TextureID, Vector2 _Size, Vector2 _Bearing, int _Advance)
        {
            TextureID = _TextureID;
            Size = _Size;
            Bearing = _Bearing;
            Advance = _Advance;
        }
    }
}