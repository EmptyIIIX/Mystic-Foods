using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Mystic_Foods.Core.Services
{
    /// <summary>
    /// Abstraction for input handling - Dependency Inversion Principle
    /// </summary>
    public interface IInputService
    {
        Point MousePosition { get; }
        bool IsMouseButtonPressed(MouseButton button);
        bool IsMouseButtonReleased(MouseButton button);
        bool IsMouseButtonDown(MouseButton button);
        bool IsKeyPressed(Keys key);
        bool IsKeyReleased(Keys key);
        bool IsKeyDown(Keys key);
        KeyboardState CurrentKeyboardState { get; }
        KeyboardState PreviousKeyboardState { get; }
        MouseState CurrentMouseState { get; }
        MouseState PreviousMouseState { get; }
        void Update();
    }

    public enum MouseButton
    {
        Left,
        Right,
        Middle,
        XButton1,
        XButton2
    }
}