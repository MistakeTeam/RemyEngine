namespace Remy.Render;

public class Vector2
{
    public readonly int x;
    public readonly int y;

    public Vector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int PegarArea()
    {
        return x * y;
    }
}