using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mystic_Foods.Core.Graphics
{
    /// <summary>
    /// Manages virtual resolution and screen scaling for responsive design
    /// Implements Singleton pattern for global access
    /// </summary>
    public class ScreenManager
    {
        private static ScreenManager _instance;
        public static ScreenManager Instance => _instance ??= new ScreenManager();

        // Virtual resolution (game's internal resolution)
        private int _virtualWidth = 1920;
        private int _virtualHeight = 1080;
        
        // Actual window/screen resolution
        private int _actualWidth;
        private int _actualHeight;
        
        // Scale factors
        private float _scaleX;
        private float _scaleY;
        private float _scale;
        
        // Viewport for letterboxing/pillarboxing
        private Viewport _viewport;
        private Matrix _transformationMatrix;
        
        // Screen mode
        private bool _isFullScreen;
        
        private ScreenManager()
        {
            _actualWidth = _virtualWidth;
            _actualHeight = _virtualHeight;
            UpdateScale();
        }

        /// <summary>
        /// Initialize with desired virtual resolution
        /// </summary>
        public void Initialize(GraphicsDeviceManager graphics, int virtualWidth = 1920, int virtualHeight = 1080, bool fullScreen = false)
        {
            _virtualWidth = virtualWidth;
            _virtualHeight = virtualHeight;
            _isFullScreen = fullScreen;
            
            // Get actual screen resolution
            _actualWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _actualHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            
            // Configure graphics device
            graphics.PreferredBackBufferWidth = _actualWidth;
            graphics.PreferredBackBufferHeight = _actualHeight;
            graphics.IsFullScreen = _isFullScreen;
            graphics.HardwareModeSwitch = false;
            graphics.ApplyChanges();
            
            UpdateScale();
        }

        /// <summary>
        /// Update viewport when window resizes
        /// </summary>
        public void UpdateViewport(int width, int height)
        {
            _actualWidth = width;
            _actualHeight = height;
            UpdateScale();
        }

        private void UpdateScale()
        {
            _scaleX = (float)_actualWidth / _virtualWidth;
            _scaleY = (float)_actualHeight / _virtualHeight;
            _scale = Math.Min(_scaleX, _scaleY);
            
            // Calculate viewport for letterboxing
            var viewportWidth = (int)(_virtualWidth * _scale);
            var viewportHeight = (int)(_virtualHeight * _scale);
            var viewportX = (_actualWidth - viewportWidth) / 2;
            var viewportY = (_actualHeight - viewportHeight) / 2;
            
            _viewport = new Viewport(viewportX, viewportY, viewportWidth, viewportHeight);
            
            // Transformation matrix for drawing
            _transformationMatrix = Matrix.CreateScale(_scale) * 
                                   Matrix.CreateTranslation(viewportX, viewportY, 0);
        }

        public Matrix GetTransformationMatrix() => _transformationMatrix;
        public Viewport GetViewport() => _viewport;
        
        public int VirtualWidth => _virtualWidth;
        public int VirtualHeight => _virtualHeight;
        public int ActualWidth => _actualWidth;
        public int ActualHeight => _actualHeight;
        public float Scale => _scale;
        
        /// <summary>
        /// Convert screen coordinates to virtual coordinates
        /// </summary>
        public Vector2 ScreenToVirtual(Vector2 screenPos)
        {
            return Vector2.Transform(screenPos, Matrix.Invert(_transformationMatrix));
        }
        
        /// <summary>
        /// Convert virtual coordinates to screen coordinates
        /// </summary>
        public Vector2 VirtualToScreen(Vector2 virtualPos)
        {
            return Vector2.Transform(virtualPos, _transformationMatrix);
        }
        
        /// <summary>
        /// Get a rectangle scaled to virtual resolution
        /// </summary>
        public Rectangle GetVirtualRectangle(Rectangle screenRect)
        {
            var topLeft = ScreenToVirtual(new Vector2(screenRect.X, screenRect.Y));
            var bottomRight = ScreenToVirtual(new Vector2(screenRect.Right, screenRect.Bottom));
            return new Rectangle(
                (int)topLeft.X, (int)topLeft.Y,
                (int)(bottomRight.X - topLeft.X),
                (int)(bottomRight.Y - topLeft.Y)
            );
        }
    }
}