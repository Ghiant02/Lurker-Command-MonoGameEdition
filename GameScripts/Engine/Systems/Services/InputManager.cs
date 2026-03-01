using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Services
{
    public static class InputManager
    {
        private static KeyboardState _currentKeyEntry;
        private static KeyboardState _prevKeyEntry;
        private static MouseState _currentMouseEntry;
        private static MouseState _prevMouseEntry; 
        public static int ScrollDelta => _currentMouseEntry.ScrollWheelValue - _prevMouseEntry.ScrollWheelValue;

        public static void Update()
        {
            _prevKeyEntry = _currentKeyEntry;
            _prevMouseEntry = _currentMouseEntry;

            _currentKeyEntry = Keyboard.GetState();
            _currentMouseEntry = Mouse.GetState();
        }

        public static bool IsKeyDown(Keys key) => _currentKeyEntry.IsKeyDown(key);

        public static bool IsKeyPressed(Keys key) => _currentKeyEntry.IsKeyDown(key) && _prevKeyEntry.IsKeyUp(key);

        public static bool IsMouseButtonPressed(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => _currentMouseEntry.LeftButton == ButtonState.Pressed && _prevMouseEntry.LeftButton == ButtonState.Released,
                MouseButton.Right => _currentMouseEntry.RightButton == ButtonState.Pressed && _prevMouseEntry.RightButton == ButtonState.Released,
                _ => false
            };
        }

        public static Vector2 MousePosition => _currentMouseEntry.Position.ToVector2();
    }

    public enum MouseButton { Left, Right }
}