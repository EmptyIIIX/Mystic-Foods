using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_Foods.Systems
{
    public class Button
    {
        private Texture2D _texture;
        private SpriteFont _font;
        private string _text;
        private Rectangle _rectangle;
        private MouseState _currentMouse;
        private MouseState _previousMouse;
        private bool _isHovering;

        public event EventHandler Click;  // event เวลากดปุ่ม
        public bool Clicked { get; private set; }

        public Button(Texture2D texture, SpriteFont font, string text, Rectangle rectangle)
        {
            _texture = texture;
            _font = font;
            _text = text;
            _rectangle = rectangle;
        }

        public void Update()
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(_rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released &&
                    _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, EventArgs.Empty); // trigger event
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            var color = _isHovering ? Color.Gray : Color.White;
            spriteBatch.Draw(_texture, _rectangle, color);

            if (!string.IsNullOrEmpty(_text))
            {
                var textSize = _font.MeasureString(_text);
                var textPosition = new Vector2(
                    _rectangle.X + (_rectangle.Width / 2) - (textSize.X / 2),
                    _rectangle.Y + (_rectangle.Height / 2) - (textSize.Y / 2));

                spriteBatch.DrawString(_font, _text, textPosition, Color.Black);
            }
        }
    }
}
