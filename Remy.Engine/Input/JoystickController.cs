using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Remy.Engine.Input
{
    public class JoystickController
    {
        JoystickState _JoystaickState;

        public JoystickController(JoystickState _joy)
        {
            _JoystaickState = _joy;
        }

        public float GetAxis(JoystickAxis axies)
        {
            return _JoystaickState.GetAxis((int)axies);
        }

        public float GetAxisPrevious(JoystickAxis axies)
        {
            return _JoystaickState.GetAxisPrevious((int)axies);
        }

        public bool IsButtonDown(JoystickButtons button)
        {
            return _JoystaickState.IsButtonDown((int)button);
        }

        public bool IsButtonPressed(JoystickButtons button)
        {
            return _JoystaickState.IsButtonPressed((int)button);
        }

        public bool IsButtonReleased(JoystickButtons button)
        {
            return _JoystaickState.IsButtonReleased((int)button);
        }
    }
}