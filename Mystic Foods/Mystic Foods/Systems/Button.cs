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
        private Texture2D texture;
        private Texture2D _normalTexture;
        private Texture2D _hoverTexture;
        private SpriteFont _font;
        private string _text;
        private Rectangle _rectangle;
        private MouseState _currentMouse;
        private MouseState _previousMouse;
        private bool _isHovering;

        public event EventHandler Click;  // event เวลากดปุ่ม
        public bool Clicked { get; private set; }
        public Rectangle Rectangle => _rectangle;
        public Button(Texture2D texture, Texture2D hovertex, SpriteFont font, string text, Rectangle rectangle)
        {
            _normalTexture = texture;
            _hoverTexture = hovertex;
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
        public void UpdateStaticBtn(Vector2 cameraPos)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseWorld = new Point(
                _currentMouse.X + (int)cameraPos.X,
                _currentMouse.Y + (int)cameraPos.Y
            );

            var mouseRectangle = new Rectangle(mouseWorld.X, mouseWorld.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(_rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released &&
                    _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            var color = _isHovering ? Color.Gray : Color.White;
            spriteBatch.Draw(_normalTexture, _rectangle, color);

            if (!string.IsNullOrEmpty(_text))
            {
                var textSize = _font.MeasureString(_text);
                var textPosition = new Vector2(
                    _rectangle.X + (_rectangle.Width / 2) - (textSize.X / 2),
                    _rectangle.Y + (_rectangle.Height / 2) - (textSize.Y / 2));

                spriteBatch.DrawString(_font, _text, textPosition, Color.Black);
            }
        }
        public void DrawHover(SpriteBatch spriteBatch)
        {
            texture = _isHovering ? _hoverTexture : _normalTexture;
            spriteBatch.Draw(texture, _rectangle, Color.White);

            if (!string.IsNullOrEmpty(_text))
            {
                var textSize = _font.MeasureString(_text);
                var textPosition = new Vector2(
                    _rectangle.X + (_rectangle.Width / 2) - (textSize.X / 2),
                    _rectangle.Y + (_rectangle.Height / 2) - (textSize.Y / 2));

                spriteBatch.DrawString(_font, _text, textPosition, Color.Black);
            }
        }
        public void DrawHomeBtn(SpriteBatch spriteBatch)
        {
            var color = _isHovering ? Color.Yellow : Color.White;
            spriteBatch.Draw(_normalTexture, _rectangle, color);
            if (!string.IsNullOrEmpty(_text))
            {
                var textSize = _font.MeasureString(_text);
                var textPosition = new Vector2(
                    _rectangle.X + (_rectangle.Width / 2) - (textSize.X / 2),
                    _rectangle.Y + (_rectangle.Height / 2) - (textSize.Y / 2));

                spriteBatch.DrawString(_font, _text, textPosition, Color.Black);
            }
        }
        public void DrawCooking(SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            var color = _isHovering ? Color.Gray : Color.White;
            var drawRect = new Rectangle(
                _rectangle.X - (int)cameraPos.X,
                _rectangle.Y - (int)cameraPos.Y,
                _rectangle.Width,
                _rectangle.Height
            );
            spriteBatch.Draw(_normalTexture, drawRect, color);
            if (!string.IsNullOrEmpty(_text))
            {
                var textSize = _font.MeasureString(_text);
                var textPosition = new Vector2(
                    drawRect.X + (drawRect.Width / 2) - (textSize.X / 2),
                    drawRect.Y + (drawRect.Height / 2) - (textSize.Y / 2));

                spriteBatch.DrawString(_font, _text, textPosition, Color.Black);
            }
        }
        public void DrawTako(SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            texture = _isHovering ? _hoverTexture : _normalTexture;
            var drawRect = new Rectangle(
                _rectangle.X - (int)cameraPos.X,
                _rectangle.Y - (int)cameraPos.Y,
                _rectangle.Width,
                _rectangle.Height
            );
            spriteBatch.Draw(texture, drawRect, Color.White);
            if (!string.IsNullOrEmpty(_text))
            {
                var textSize = _font.MeasureString(_text);
                var textPosition = new Vector2(
                    drawRect.X + (drawRect.Width / 2) - (textSize.X / 2),
                    drawRect.Y + (drawRect.Height / 2) - (textSize.Y / 2));

                spriteBatch.DrawString(_font, _text, textPosition, Color.Black);
            }
        }
    }
}
