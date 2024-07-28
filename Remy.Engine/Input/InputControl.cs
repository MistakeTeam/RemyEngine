using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using Remy.Engine.Utility;

namespace Remy.Engine.Input
{
    public class InputControl
    {
        private GameWindow GW;
        public static KeyboardState Keyboard;
        public static MouseState Mouse;
        public static IReadOnlyList<JoystickState> Joysticks;
        public static bool JoystickIsConnected;
        public static event Action<JoystickEventArgs> JoystickConnected;
        public static event Action MouseEvent;
        public static Vector2 MousePosition;

        public InputControl(GameWindow game)
        {
            GW = game;

            Keyboard = GW.KeyboardState;
            Mouse = GW.MouseState;

            if (GW.JoystickStates[0] != null)
            {
                Joysticks = GW.JoystickStates;
                JoystickIsConnected = true;
                foreach (JoystickState joy in Joysticks)
                {
                    if (joy != null) Console.WriteLine(joy);
                }
            }

            GW.JoystickConnected += JoystickEvent;
        }

        public void Update()
        {
            MousePosition = new(Mouse.X / (Game.Janela.X / 2) - 1.0f, -1 * (Mouse.Y / (Game.Janela.Y / 2) - 1.0f));
            //MousePosition = new(Converter.Intervalo(Mouse.X, 0, Game.Janela.X, -1, 1), Converter.Intervalo(Mouse.Y, 0, Game.Janela.Y, -1, 1));

            if (Keyboard.IsKeyDown(Keys.Escape))
            {
                GW.Close();
            }

            if (Mouse.IsButtonDown(MouseButton.Left))
                Console.WriteLine("The left mouse button is down!");

            // if (Mouse.X > 0 && Mouse.X < Game.Janela.X && Mouse.Y > 0 && Mouse.Y < Game.Janela.Y)
            MouseEvent?.Invoke();
        }

        private void JoystickEvent(JoystickEventArgs joystick)
        {

            JoystickIsConnected = joystick.IsConnected;
            Joysticks = GW.JoystickStates;

            if (joystick.IsConnected)
            {
                Console.WriteLine("joystick foi conectado!");
                foreach (JoystickState joy in Joysticks)
                {
                    if (joy != null) Console.WriteLine(joy);
                }
            }
            else
            {
                Console.WriteLine("joystick foi desconectado!");
            }
        }
    }
}