using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpFont.PostScript;

namespace Remy.Engine.Input
{
    public class Keyboard
    {
        private KeyboardState KeyboardState;

        public Keyboard(KeyboardState Keyboard)
        {
            KeyboardState = Keyboard;
        }

        public bool IsKeyDown(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return KeyboardState.IsKeyPressed(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return KeyboardState.IsKeyReleased(key);
        }

        public bool IsAnyKeyDown => KeyboardState.IsAnyKeyDown;
    }
}