using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Remy.Engine.Input
{
    public class Keyboard
    {
        private static KeyboardState _keyboardState;

        public Keyboard(KeyboardState _keyboard)
        {
            _keyboardState = _keyboard;
        }

        public static bool IsKeyDown(Keys key)
        {
            return _keyboardState.IsKeyDown(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return _keyboardState.IsKeyPressed(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return _keyboardState.IsKeyReleased(key);
        }

        public static bool IsAnyKeyDown => _keyboardState.IsAnyKeyDown;
    }
}