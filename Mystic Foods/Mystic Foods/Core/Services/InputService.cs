using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Mystic_Foods.Core.Services
{
    /// <summary>
    /// Default MonoGame input implementation
    /// </summary>
    public class InputService : IInputService
    {
        private KeyboardState _previousKeyboardState;
        private MouseState _previousMouseState;

        public KeyboardState CurrentKeyboardState { get; private set; }
        public KeyboardState PreviousKeyboardState => _previousKeyboardState;
        public MouseState CurrentMouseState { get; private set; }
        public MouseState PreviousMouseState => _previousMouseState;

        public Point MousePosition => CurrentMouseState.Position;

        public void Update()
        {
            _previousKeyboardState = CurrentKeyboardState;
            _previousMouseState = CurrentMouseState;

            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();
        }

        public bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        public bool IsMouseButtonPressed(MouseButton button)
        {
            return GetButtonState(CurrentMouseState, button) == ButtonState.Pressed &&
                   GetButtonState(_previousMouseState, button) == ButtonState.Released;
        }

        public bool IsMouseButtonReleased(MouseButton button)
        {
            return GetButtonState(CurrentMouseState, button) == ButtonState.Released &&
                   GetButtonState(_previousMouseState, button) == ButtonState.Pressed;
        }

        public bool IsMouseButtonDown(MouseButton button)
        {
            return GetButtonState(CurrentMouseState, button) == ButtonState.Pressed;
        }

        private static ButtonState GetButtonState(MouseState state, MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => state.LeftButton,
                MouseButton.Right => state.RightButton,
                MouseButton.Middle => state.MiddleButton,
                MouseButton.XButton1 => state.XButton1,
                MouseButton.XButton2 => state.XButton2,
                _ => ButtonState.Released
            };
        }
    }
}