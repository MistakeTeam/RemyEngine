using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Remy.Engine.Input
{
    public class InputControl
    {
        private GameWindow _gameWindow;
        private Keyboard _keyboard;
        private Mouse _mouse;
        private Joystick _joystick;

        public InputControl(GameWindow game)
        {
            _gameWindow = game;

            _keyboard = new Keyboard(_gameWindow.KeyboardState);
            _mouse = new Mouse(_gameWindow.MouseState);
            _joystick = new Joystick(_gameWindow.JoystickStates);


            // Lista de eventos
            _gameWindow.JoystickConnected += _joystick.JoystickConnectedEvent;
        }

        public void Update()
        {
            if (Keyboard.IsKeyDown(Keys.Escape))
            {
                _gameWindow.Close();
            }

            _mouse.Update();
        }
    }
}