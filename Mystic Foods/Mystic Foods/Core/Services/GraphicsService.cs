using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mystic_Foods.Core.Services
{
    /// <summary>
    /// Implementation of virtual resolution system for responsive design
    /// Follows Single Responsibility Principle - handles only graphics/scaling concerns
    /// </summary>
    public class GraphicsService : IGraphicsService
    {
        private int _virtualWidth = 1920;
        private int _virtualHeight = 1080;
        private int _actualWidth;
        private int _actualHeight;
        private float _scaleX;
        private float _scaleY;
        private float _uniformScale;
        private Matrix _transformMatrix;
        private Rectangle _viewport;
        private Texture2D _whiteTexture;
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphicsDevice;

        public int VirtualWidth => _virtualWidth;
        public int VirtualHeight => _virtualHeight;
        public int ActualWidth => _actualWidth;
        public int ActualHeight => _actualHeight;
        public float ScaleX => _scaleX;
        public float ScaleY => _scaleY;
        public float UniformScale => _uniformScale;
        public Matrix TransformMatrix => _transformMatrix;
        public Rectangle Viewport => _viewport;
        public Texture2D WhiteTexture => _whiteTexture;
        public SpriteBatch SpriteBatch => _spriteBatch;

        public void Initialize(GraphicsDevice graphicsDevice, int virtualWidth = 1920, int virtualHeight = 1080)
        {
            _graphicsDevice = graphicsDevice;
            _virtualWidth = virtualWidth;
            _virtualHeight = virtualHeight;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _whiteTexture = new Texture2D(graphicsDevice, 1, 1);
            _whiteTexture.SetData(new[] { Color.White });
            
            // Get actual screen size
            _actualWidth = graphicsDevice.PresentationParameters.BackBufferWidth;
            _actualHeight = graphicsDevice.PresentationParameters.BackBufferHeight;
            
            UpdateScale();
        }

        public void OnResize(int width, int height)
        {
            _actualWidth = width;
            _actualHeight = height;
            UpdateScale();
        }

        private void UpdateScale()
        {
            _scaleX = (float)_actualWidth / _virtualWidth;
            _scaleY = (float)_actualHeight / _virtualHeight;
            _uniformScale = Math.Min(_scaleX, _scaleY);

            // Calculate viewport for letterboxing/pillarboxing
            int viewportWidth = (int)(_virtualWidth * _uniformScale);
            int viewportHeight = (int)(_virtualHeight * _uniformScale);
            int viewportX = (_actualWidth - viewportWidth) / 2;
            int viewportY = (_actualHeight - viewportHeight) / 2;

            _viewport = new Rectangle(viewportX, viewportY, viewportWidth, viewportHeight);
            
            // Transformation matrix for virtual resolution
            _transformMatrix = Matrix.CreateTranslation(-_virtualWidth / 2f, -_virtualHeight / 2f, 0) *
                              Matrix.CreateScale(_uniformScale, _uniformScale, 1) *
                              Matrix.CreateTranslation(_actualWidth / 2f, _actualHeight / 2f, 0);
        }

        public Rectangle ScaleRectangle(Rectangle rect)
        {
            return new Rectangle(
                (int)(rect.X * _uniformScale),
                (int)(rect.Y * _uniformScale),
                (int)(rect.Width * _uniformScale),
                (int)(rect.Height * _uniformScale)
            );
        }

        public Vector2 ScaleVector(Vector2 vec)
        {
            return vec * _uniformScale;
        }

        public Point ScalePoint(Point pt)
        {
            return new Point((int)(pt.X * _uniformScale), (int)(pt.Y * _uniformScale));
        }

        public Vector2 ScreenToVirtual(Vector2 screenPos)
        {
            // Convert from screen coordinates to virtual coordinates
            var adjusted = screenPos - new Vector2(_viewport.X, _viewport.Y);
            return adjusted / _uniformScale;
        }

        public Vector2 VirtualToScreen(Vector2 virtualPos)
        {
            return virtualPos * _uniformScale + new Vector2(_viewport.X, _viewport.Y);
        }

        public void Begin(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null,
                         SamplerState samplerState = null, DepthStencilState depthStencilState = null,
                         RasterizerState rasterizerState = null, Effect effect = null,
                         Matrix? transformMatrix = null)
        {
            _spriteBatch.Begin(
                sortMode,
                blendState ?? BlendState.AlphaBlend,
                samplerState ?? SamplerState.PointClamp,
                depthStencilState ?? DepthStencilState.None,
                rasterizerState ?? RasterizerState.CullCounterClockwise,
                effect,
                transformMatrix ?? _transformMatrix
            );
        }

        public void End()
        {
            _spriteBatch.End();
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color,
                        float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            _spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            _spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, Color color,
                              float rotation = 0, Vector2 origin = default, float scale = 1,
                              SpriteEffects effects = SpriteEffects.None, float layerDepth = 0)
        {
            _spriteBatch.DrawString(font, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public Vector2 MeasureString(SpriteFont font, string text)
        {
            return font.MeasureString(text);
        }
    }
}