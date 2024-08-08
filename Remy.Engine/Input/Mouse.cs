using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Remy.Engine.Input
{
    public class Mouse
    {
        private static MouseState _mouseState;
        public static float X;
        public static float Y;
        public static Vector2 MousePosition;
        public static event Action MouseEvent;

        public Mouse(MouseState _mouse)
        {
            _mouseState = _mouse;
        }

        public static bool IsButtonDown(MouseButton button)
        {
            return _mouseState.IsButtonDown(button);
        }

        public void Update()
        {
            X = _mouseState.X;
            Y = _mouseState.Y;
            MousePosition = new(Mouse.X / (Game.Janela.X / 2) - 1.0f, -1 * (Mouse.Y / (Game.Janela.Y / 2) - 1.0f));
            //MousePosition = new(Converter.Intervalo(Mouse.X, 0, Game.Janela.X, -1, 1), Converter.Intervalo(Mouse.Y, 0, Game.Janela.Y, -1, 1));

            MouseEvent?.Invoke();
        }
    }
}