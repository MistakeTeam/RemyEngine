using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Remy.Engine.Input
{
    public class Mouse
    {
        private readonly MouseState MouseState;

        public bool PosiçãoValida { get; set; } = true;

        public Vector2i Posição { get; private set; }
        public Vector2 Delta { get; private set; }
        public Vector2i UltimaPosição { get; private set; }

        public Vector2 Scroll { get; private set; }
        public Vector2 ScrollDelta { get; private set; }
        public Vector2 UltimoScroll { get; private set; }

        public event Action MouseEvent;

        public Mouse(MouseState _mouse)
        {
            MouseState = _mouse;
        }

        public bool IsButtonDown(MouseButton button)
        {
            return MouseState.IsButtonDown(button);
        }

        public void Update()
        {
            if (MouseState.Position.X >= Plataforma.Window.Tamanho_Janela.X || MouseState.Position.X <= 0)
                return;
            if (MouseState.Position.Y >= Plataforma.Window.Tamanho_Janela.Y || MouseState.Position.Y <= 0)
                return;

            Posição = (Vector2i)MouseState.Position;
            Delta = MouseState.Delta;
            UltimaPosição = (Vector2i)MouseState.PreviousPosition;

            Scroll = MouseState.Scroll;
            ScrollDelta = MouseState.ScrollDelta;
            UltimoScroll = MouseState.PreviousScroll;

            MouseEvent?.Invoke();
        }
    }
}