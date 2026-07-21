using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Mystic_Foods.Core.UI
{
    /// <summary>
    /// Button styles for different visual states
    /// </summary>
    public enum ButtonStyle
    {
        Default,      // Simple color tint on hover
        HoverTexture, // Separate hover texture
        Custom        // Custom draw delegate
    }

    /// <summary>
    /// Button configuration using Builder pattern
    /// </summary>
    public class ButtonConfig
    {
        public Texture2D Texture { get; set; }
        public Texture2D HoverTexture { get; set; }
        public Texture2D PressedTexture { get; set; }
        public SpriteFont Font { get; set; }
        public string Text { get; set; } = "";
        public Rectangle Bounds { get; set; }
        public Color NormalColor { get; set; } = Color.White;
        public Color HoverColor { get; set; } = Color.LightGray;
        public Color PressedColor { get; set; } = Color.Gray;
        public Color TextColor { get; set; } = Color.Black;
        public ButtonStyle Style { get; set; } = ButtonStyle.Default;
        public Vector2 TextOffset { get; set; } = Vector2.Zero;
        public float _origin;
        public Action<Button> OnClick { get; set; }
        public Action<Button> OnHoverEnter { get; set; }
        public Action<Button> OnHoverExit { get; set; }
    }

    /// <summary>
    /// Button builder for fluent configuration
    /// </summary>
    public class ButtonBuilder
    {
        private readonly ButtonConfig _config = new();

        public ButtonBuilder WithTexture(Texture2D texture)
        {
            _config.Texture = texture;
            return this;
        }

        public ButtonBuilder WithHoverTexture(Texture2D texture)
        {
            _config.HoverTexture = texture;
            _config.Style = ButtonStyle.HoverTexture;
            return this;
        }

        public ButtonBuilder WithPressedTexture(Texture2D texture)
        {
            _config.PressedTexture = texture;
            return this;
        }

        public ButtonBuilder WithFont(SpriteFont font)
        {
            _config.Font = font;
            return this;
        }

        public ButtonBuilder WithText(string text)
        {
            _config.Text = text;
            return this;
        }

        public ButtonBuilder WithBounds(Rectangle bounds)
        {
            _config.Bounds = bounds;
            return this;
        }

        public ButtonBuilder WithBounds(int x, int y, int width, int height)
        {
            _config.Bounds = new Rectangle(x, y, width, height);
            return this;
        }

        public ButtonBuilder WithColors(Color normal, Color hover, Color pressed)
        {
            _config.NormalColor = normal;
            _config.HoverColor = hover;
            _config.PressedColor = pressed;
            return this;
        }

        public ButtonBuilder WithTextColor(Color color)
        {
            _config.TextColor = color;
            return this;
        }

        public ButtonBuilder WithStyle(ButtonStyle style)
        {
            _config.Style = style;
            return this;
        }

        public ButtonBuilder WithTextOffset(Vector2 offset)
        {
            _config.TextOffset = offset;
            return this;
        }

        public ButtonBuilder OnClick(Action<Button> action)
        {
            _config.OnClick = action;
            return this;
        }

        public ButtonBuilder OnHoverEnter(Action<Button> action)
        {
            _config.OnHoverEnter = action;
            return this;
        }

        public ButtonBuilder OnHoverExit(Action<Button> action)
        {
            _config.OnHoverExit = action;
            return this;
        }

        public Button Build()
        {
            return new Button(_config);
        }
    }

    /// <summary>
    /// Refactored Button component following SOLID principles
    /// - Single Responsibility: Only handles button interaction and rendering
    /// - Open/Closed: Extensible via ButtonStyle enum and custom delegates
    /// - Liskov Substitution: Can be used anywhere IUIElement is expected
    /// - Interface Segregation: Implements only IUIElement
    /// - Dependency Inversion: Depends on abstractions (IInputService)
    /// </summary>
    public class Button : IUIElement
    {
        private readonly ButtonConfig _config;
        private readonly IInputService _input;
        
        private ButtonState _currentState = ButtonState.Normal;
        private bool _wasHovered = false;
        private bool _isPressed = false;

        public Rectangle Bounds => _config.Bounds;
        public bool IsHovered => _currentState == ButtonState.Hover || _currentState == ButtonState.Pressed;
        public bool IsPressed => _currentState == ButtonState.Pressed;
        public bool IsEnabled { get; set; } = true;
        public bool IsVisible { get; set; } = true;

        public event Action<Button> Clicked;
        public event Action<Button> HoverEnter;
        public event Action<Button> HoverExit;

        internal Button(ButtonConfig config)
        {
            _config = config;
            _input = ServiceLocator.Get<IInputService>();
        }

        public void Update(GameTime gameTime)
        {
            if (!IsEnabled || !IsVisible) return;

            var mousePos = _input.MousePosition;
            var mouseRect = new Rectangle(mousePos.X, mousePos.Y, 1, 1);
            bool isHovering = mouseRect.Intersects(_config.Bounds);

            // State transitions
            var previousState = _currentState;

            if (!isHovering)
            {
                _currentState = ButtonState.Normal;
            }
            else if (_input.IsMouseButtonPressed(MouseButton.Left))
            {
                _currentState = ButtonState.Pressed;
                _isPressed = true;
            }
            else if (_isPressed && _input.IsMouseButtonReleased(MouseButton.Left))
            {
                _currentState = ButtonState.Hover;
                _isPressed = false;
                OnClick();
            }
            else
            {
                _currentState = ButtonState.Hover;
            }

            // Hover events
            if (isHovering && !_wasHovered)
            {
                OnHoverEnter();
            }
            else if (!isHovering && _wasHovered)
            {
                OnHoverExit();
            }

            _wasHovered = isHovering;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            Texture2D texture = GetCurrentTexture();
            Color color = GetCurrentColor();

            spriteBatch.Draw(texture, _config.Bounds, color);

            // Draw text if present
            if (!string.IsNullOrEmpty(_config.Text) && _config.Font != null)
            {
                var textSize = _config.Font.MeasureString(_config.Text);
                var textPos = new Vector2(
                    _config.Bounds.Center.X - textSize.X / 2 + _config.TextOffset.X,
                    _config.Bounds.Center.Y - textSize.Y / 2 + _config.TextOffset.Y
                );

                spriteBatch.DrawString(_config.Font, _config.Text, textPos, _config.TextColor);
            }
        }

        private Texture2D GetCurrentTexture()
        {
            return _currentState switch
            {
                ButtonState.Hover when _config.HoverTexture != null => _config.HoverTexture,
                ButtonState.Pressed when _config.PressedTexture != null => _config.PressedTexture,
                _ => _config.Texture ?? ServiceLocator.Get<IGraphicsService>().WhiteTexture
            };
        }

        private Color GetCurrentColor()
        {
            if (_config.Style == ButtonStyle.HoverTexture && _config.HoverTexture != null)
                return Color.White;

            return _currentState switch
            {
                ButtonState.Hover => _config.HoverColor,
                ButtonState.Pressed => _config.PressedColor,
                _ => _config.NormalColor
            };
        }

        private void OnClick()
        {
            _config.OnClick?.Invoke(this);
            Clicked?.Invoke(this);
        }

        private void OnHoverEnter()
        {
            _config.OnHoverEnter?.Invoke(this);
            HoverEnter?.Invoke(this);
        }

        private void OnHoverExit()
        {
            _config.OnHoverExit?.Invoke(this);
            HoverExit?.Invoke(this);
        }

        public void SetBounds(Rectangle bounds) => _config.Bounds = bounds;
        public void SetText(string text) => _config.Text = text;
        public void SetEnabled(bool enabled) => IsEnabled = enabled;
        public void SetVisible(bool visible) => IsVisible = visible;
    }

    internal enum ButtonState
    {
        Normal,
        Hover,
        Pressed,
        Disabled
    }

    /// <summary>
    /// Interface for UI elements - Interface Segregation Principle
    /// </summary>
    public interface IUIElement
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        Rectangle Bounds { get; }
        bool IsVisible { get; set; }
        bool IsEnabled { get; set; }
    }
}