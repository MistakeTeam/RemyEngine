using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Remy.Engine.Input
{
    public class InputControl
    {
        public readonly GameWindow GameWindow;
        public readonly Keyboard Keyboard;
        public readonly Mouse Mouse;
        public readonly Joystick Joystick;

        public InputControl(GameWindow game)
        {
            GameWindow = game;

            Keyboard = new Keyboard(GameWindow.KeyboardState);
            Mouse = new Mouse(GameWindow.MouseState);
            Joystick = new Joystick(GameWindow.JoystickStates);

            // Lista de eventos
            GameWindow.JoystickConnected += Joystick.JoystickConnectedEvent;
        }

        public void Update()
        {
            if (Keyboard.IsKeyDown(Keys.Escape))
            {
                GameWindow.Close();
            }

            Mouse.Update();
        }
    }
}