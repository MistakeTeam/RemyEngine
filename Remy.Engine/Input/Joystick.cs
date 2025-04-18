using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Remy.Engine.Logs;

namespace Remy.Engine.Input
{
    public class Joystick
    {
        private static IReadOnlyList<JoystickState> Joysticks;
        public static bool JoystickIsConnected;
        public static event Action<JoystickEventArgs> JoystickConnected;

        public Joystick(IReadOnlyList<JoystickState> _JoystickStates)
        {
            if (_JoystickStates[0] != null)
            {
                Joysticks = _JoystickStates;
                JoystickIsConnected = true;
                foreach (JoystickState joy in Joysticks)
                {
                    if (joy != null) Console.WriteLine(joy);
                }
            }
        }

        public static JoystickController GetJoystick(int index)
        {
            return new JoystickController(Joysticks[index]);
        }

        public void JoystickConnectedEvent(JoystickEventArgs joystick)
        {
            JoystickIsConnected = joystick.IsConnected;
            Joysticks = Game.Joysticks;

            if (joystick.IsConnected)
            {
                Logger.WriteLine("joystick foi conectado!");
                foreach (JoystickState joy in Joysticks)
                {
                    if (joy != null) Console.WriteLine(joy);
                }
            }
            else
            {
                Logger.WriteLine("joystick foi desconectado!");
            }

            JoystickConnected?.Invoke(joystick);
        }
    }
}