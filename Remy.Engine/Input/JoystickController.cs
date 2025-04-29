using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Remy.Engine.Input
{
    public class JoystickController
    {
        private readonly JoystickState JoystaickState;

        public JoystickController(JoystickState _joy)
        {
            JoystaickState = _joy;
        }

        public float GetAxis(JoystickAxis axies)
        {
            return JoystaickState.GetAxis((int)axies);
        }

        public float GetAxisPrevious(JoystickAxis axies)
        {
            return JoystaickState.GetAxisPrevious((int)axies);
        }

        public bool IsButtonDown(JoystickButtons button)
        {
            return JoystaickState.IsButtonDown((int)button);
        }

        public bool IsButtonPressed(JoystickButtons button)
        {
            return JoystaickState.IsButtonPressed((int)button);
        }

        public bool IsButtonReleased(JoystickButtons button)
        {
            return JoystaickState.IsButtonReleased((int)button);
        }
    }
}