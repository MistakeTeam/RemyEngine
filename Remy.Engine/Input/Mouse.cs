using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Remy.Engine.Input
{
    public class Mouse
    {
        private static MouseState _mouseState;

        public bool PosiçãoValida { get; set; } = true;

        public static Vector2 Posição { get; private set; }
        public static Vector2 Delta { get; private set; }
        public static Vector2 UltimaPosição { get; private set; }

        public static Vector2 Scroll { get; private set; }
        public static Vector2 ScrollDelta { get; private set; }
        public static Vector2 UltimoScroll { get; private set; }

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
            if (_mouseState.Position.X >= Plataforma.Window.Tamanho_Janela.X || _mouseState.Position.X <= 0)
                return;
            if (_mouseState.Position.Y >= Plataforma.Window.Tamanho_Janela.Y || _mouseState.Position.Y <= 0)
                return;

            Posição = _mouseState.Position;
            Delta = _mouseState.Delta;
            UltimaPosição = _mouseState.PreviousPosition;

            Scroll = _mouseState.Scroll;
            ScrollDelta = _mouseState.ScrollDelta;
            UltimoScroll = _mouseState.PreviousScroll;

            MouseEvent?.Invoke();
        }
    }
}